namespace Netty
{

    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Web.Hosting;
    using System.Xml;

    /// <summary>
    ///     A class that serves as the base for all ASP hosted websites.
    /// </summary>
    public class NettyServer
    {

        /// <summary>
        /// The pattern used to locate an AppSetting node with a specified key.
        /// </summary>
        private const string AppSettingXpathPattern = "/configuration/appSettings/add[@key='{0}']";

        private readonly IAspServerConfiguration _config;
        private bool _isRunning;
        private AspServerListener _listener;
        private Thread _processThread;
        private readonly string _originalWebConfig;
        private readonly string _webConfigFile;
        private bool _webConfigAltered;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Netty.NettyServer" /> class.
        /// </summary>
        /// <param name="physicalPath">The physical path to the website.</param>
        /// <param name="virtualPath">The virtual path the website runs on.</param>
        public NettyServer(string physicalPath, string virtualPath) : 
            this(physicalPath, virtualPath, NetworkUtility.FindRandomOpenPort(), true)
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
        public NettyServer(string physicalPath, string virtualPath, int port) :
            this(physicalPath, virtualPath, port, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Netty.NettyServer" /> class.
        /// </summary>
        /// <param name="physicalPath">The physical path to the website.</param>
        /// <param name="virtualPath">The virtual path the website runs on.</param>
        /// <param name="port">The port the website runs on.</param>
        /// <param name="enforcePortCheck"><c>true</c> to ensure the port is unused; otherwise, <c>false</c>.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="virtualPath"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="port"/> is already in use.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="port"/> exceeds the allows TCP port range.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"><paramref name="physicalPath"/> does not exist.</exception>
        public NettyServer(string physicalPath, string virtualPath, int port, bool enforcePortCheck)
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

            _webConfigFile = Path.Combine(physicalPath, "web.config");

            _originalWebConfig = File.Exists(_webConfigFile) ? File.ReadAllText(_webConfigFile) : null;
            _webConfigAltered = false;

            virtualPath = virtualPath[0] == '/' ? virtualPath : "/" + virtualPath;
            virtualPath = virtualPath[virtualPath.Length - 1] == '/' ? virtualPath : virtualPath + "/";

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
        ///     Alters the application setting for the web site.
        /// </summary>
        /// <param name="key">The key for the application setting.</param>
        /// <param name="value">The value of the application setting.</param>
        /// <returns>The <see cref="T:Netty.NettyServer"/> the configuration was altered for.</returns>
        public NettyServer AlterApplicationSetting(string key, string value)
        {
            var xpath = String.Format(CultureInfo.InvariantCulture, NettyServer.AppSettingXpathPattern, key);
            var server = this.AlterConfigurationNodeAttributeValue(xpath, "value", value);
            return server;

        }

        /// <summary>
        ///     Alters the configuration for the website.
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The <see cref="T:Netty.NettyServer"/> the configuration was altered for.</returns>
        public NettyServer AlterConfigurationNodeAttributeValue(string xpath, string attribute, string value)
        {

            if (_originalWebConfig == null)
            {
                return this;
            }

            var doc = new XmlDocument();
            doc.Load(_webConfigFile);

            var node = doc.SelectSingleNode(xpath);

            if (node != null && node.Attributes != null && node.Attributes[attribute] != null)
            {
                node.Attributes[attribute].Value = value;
                doc.Save(_webConfigFile);
                _webConfigAltered = true;
            }

            return this;

        }

        /// <summary>
        ///     Alters the configuration for the website.
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The <see cref="T:Netty.NettyServer"/> the configuration was altered for.</returns>
        public NettyServer AlterConfigurationNodeValue(string xpath, string value)
        {

            if (_originalWebConfig == null)
            {
                return this;
            }

            var doc = new XmlDocument();
            doc.Load(_webConfigFile);

            var node = doc.SelectSingleNode(xpath);

            if (node != null)
            {
                node.Value = value;
                doc.Save(_webConfigFile);
                _webConfigAltered = true;
            }

            return this;

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

            if (_webConfigAltered && _originalWebConfig != null)
            {
                var configFile = Path.Combine(this.PhysicalPath, "web.config");
                File.WriteAllText(configFile, _originalWebConfig);
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
                CultureInfo.InvariantCulture);

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