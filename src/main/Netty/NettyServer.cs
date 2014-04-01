namespace Netty
{

    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Web.Hosting;

    /// <summary>
    ///     A class that serves as the base for all ASP hosted websites.
    /// </summary>
    public class NettyServer
    {

        private readonly IAspServerConfiguration _config;
        private bool _isRunning;
        private AspServerListener _listener;
        private Thread _processThread;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Netty.NettyServer" /> class.
        /// </summary>
        /// <param name="physicalPath">The physical path to the website.</param>
        /// <param name="virtualPath">The virtual path the website runs on.</param>
        public NettyServer(string physicalPath, string virtualPath) : 
            this(physicalPath, virtualPath, NetworkUtility.FindRandomOpenPort())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Netty.NettyServer" /> class.
        /// </summary>
        /// <param name="physicalPath">The physical path to the website.</param>
        /// <param name="virtualPath">The virtual path the website runs on.</param>
        /// <param name="port">The port the website runs on.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="virtualPath"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="port"/> is already in use.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="port"/> exceeds the allows TCP port range.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"><paramref name="physicalPath"/> does not exist.</exception>
        public NettyServer(string physicalPath, string virtualPath, int port)
        {

            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }

            if (NetworkUtility.IsPortBeingUsed(port))
            {
                throw new ArgumentException(ErrorMessages.PortInUse, "port");
            }

            if (port > NetworkUtility.MaximumPort || port < NetworkUtility.MinimumPort)
            {
                throw new ArgumentOutOfRangeException("port");
            }

            var di = new DirectoryInfo(physicalPath);
            
            if (!di.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            physicalPath = di.FullName;

            virtualPath = virtualPath[0] == '/' ? virtualPath : "/" + virtualPath;
            virtualPath = virtualPath[virtualPath.Length - 1] == '/' ?  virtualPath : virtualPath + "/";

            _config = new AspServerConfiguration()
            {
                Initializer = typeof(WebsiteInitializer),
                PhysicalPath = physicalPath,
                Port = port,
                VirtualPath = virtualPath
            };

        }

        /// <summary>
        /// Gets a value indicating whether the server is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the server is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        ///     Gets the physical path to the website.
        /// </summary>
        /// <value>
        /// The physical path to the website.
        /// </value>
        public string PhysicalPath
        {
            get { return _config.PhysicalPath; }
        }

        /// <summary>
        ///     Gets the TCP port the web runner is listening on.
        /// </summary>
        /// <value>
        ///     The TCP port the web runner is listening on.
        /// </value>
        public int Port
        {
            get { return _config.Port; }
        }

        /// <summary>
        ///     Gets the virtual path the website runs on.
        /// </summary>
        /// <value>
        ///     The virtual path the website runs on.
        /// </value>
        public string VirtualPath
        {
            get { return _config.VirtualPath; }
        }

        /// <summary>
        ///     Starts the instance of the ASP.Net server.
        /// </summary>
        public void Start()
        {

            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            _processThread = new Thread(CreateListenerAndStart);
            _processThread.Start();

        }

        /// <summary>
        ///     Stops the ASP.Net server.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The service is being shut down.")]
        public void Stop()
        {

            if (_listener != null)
            {
                try
                {
                    _listener.StopListening();
                }
                catch
                {
                    // Known empty catch - we are trying to shut down anyway
                }
            }

            if (_processThread != null && _processThread.IsAlive)
            {
                _processThread.Abort();
                _processThread.Join();
            }

        }

        /// <summary>
        ///     Creates a new <see cref="T:Netty.AspServerListener" /> in its own application domain, and begins listening for HTTP traffic.
        /// </summary>
        private void CreateListenerAndStart()
        {

            var parts = CreateListnerWithinAppDomain(_config.Initializer);

            _listener = (AspServerListener)parts.Item1;

            var initializer = (IAspInitializer)parts.Item2;
            _listener.ApplyConfiguration(_config, initializer);

            _listener.StartListening();

            while (_isRunning)
            {
                _listener.ProcessRequest();
            }

        }

        /// <summary>
        /// Creates the worker application domain with host.
        /// </summary>
        /// <param name="initializerType">Type of the initializer.</param>
        /// <returns>
        /// The object registration created for the instance.
        /// </returns>
        private Tuple<IRegisteredObject, IRegisteredObject> CreateListnerWithinAppDomain(Type initializerType)
        {

            // Generate a new application identifier
            string applicationIdentifier = Guid.NewGuid().ToString();

            // Get the ASP.NET hosting manager
            var manager = ApplicationManager.GetApplicationManager();

            // Get the BuildManagerHost type from the ASP.NET runtime assembly (it is marked as internal)
            var buildManagerHostType =
                typeof(System.Web.HttpRuntime).Assembly.GetType("System.Web.Compilation.BuildManagerHost");

            // Create a new instance of the BuildManagerHost based on the type within the asp.net hosting manager
            var buildManagerHost = manager.CreateObject(
                applicationIdentifier, buildManagerHostType, _config.VirtualPath, _config.PhysicalPath, false);

            // Register the Netty assembly with the host
            var listenerType = typeof(AspServerListener);

            buildManagerHostType.InvokeMember(
                "RegisterAssembly", 
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, 
                null,
                buildManagerHost, 
                new object[] { listenerType.Assembly.FullName, listenerType.Assembly.Location }, 
                System.Globalization.CultureInfo.InvariantCulture);

            // Register the MW.Web assembly with the host
            var configurationType = typeof(AspServerConfiguration);
            buildManagerHostType.InvokeMember(
                "RegisterAssembly", 
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, 
                null,
                buildManagerHost, 
                new object[] { configurationType.Assembly.FullName, configurationType.Assembly.Location }, 
                System.Globalization.CultureInfo.InvariantCulture);

            // Generate a new AspServerListener within the application domain
            var listener = manager.CreateObject(
                applicationIdentifier, listenerType, _config.VirtualPath, _config.PhysicalPath, false);

            // Generate a new initializer within the application domain
            var initializer = manager.CreateObject(
                applicationIdentifier, initializerType, _config.VirtualPath, _config.PhysicalPath, false);

            // return the listener
            return new Tuple<IRegisteredObject, IRegisteredObject>(listener, initializer);

        }

    }

}