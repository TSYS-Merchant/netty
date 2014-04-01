namespace Netty
{

    using System;
    using System.Globalization;
    using System.Net;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Hosting;

    /// <summary>
    ///     A class that listens for connections to an ASP.Net server, and routes the requests appropriately.
    /// </summary>
    [System.Security.SecurityCritical]
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    internal class AspServerListener : MarshalByRefObject, IDisposable, IRegisteredObject
    {

        private IAspServerConfiguration _config;
        private bool _disposed;
        private HttpListener _httpListener;
        private IAspRequestProcessor _processor;

        /// <summary>
        ///     Applies the configuration of the server to the instance.
        /// </summary>
        /// <param name="configuration">
        ///     The <see cref="AspServerConfiguration" /> used to initialize the instance.
        /// </param>
        /// <param name="initializer">The initializer used to setup the pipeline for this instance.</param>
        public void ApplyConfiguration(IAspServerConfiguration configuration, IAspInitializer initializer)
        {

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (initializer == null)
            {
                throw new ArgumentNullException("initializer");
            }

            _config = configuration;
            _httpListener = new HttpListener();

            _httpListener.Prefixes.Add(
                string.Format(CultureInfo.InvariantCulture, "http://*:{0}{1}", _config.Port, _config.VirtualPath));
            _processor = initializer.ConfigurationInitializer(_config.VirtualPath, _config.PhysicalPath);

        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        ///     An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the
        ///     <see
        ///         cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" />
        ///     property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        [System.Security.SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        ///     Processes the next HTTP request.
        /// </summary>
        [AspNetHostingPermission(SecurityAction.Assert, Level = AspNetHostingPermissionLevel.Medium)]
        public void ProcessRequest()
        {

            try
            {
                var listenerContext = _httpListener.GetContext();
                using (var httpContext = new HttpContext(listenerContext))
                {
                    _processor.ProcessRequest(httpContext);
                    httpContext.Response.FlushResponse();
                }
            }
            catch (HttpListenerException)
            {
                // This can occur naturally during a shutdown - just ignore it
            }

        }

        /// <summary>
        ///     Starts the instance listening to requests.
        /// </summary>
        public void StartListening()
        {
            _httpListener.Start();
            _httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        }

        /// <summary>
        ///     Requests a registered object to unregister.
        /// </summary>
        /// <param name="immediate"><c>true</c> to indicate the registered object should unregister from the hosting environment before returning; otherwise, <c>false</c>.</param>
        public void Stop(bool immediate)
        {
            this.StopListening();
        }

        /// <summary>
        ///     Stops the instance from listening for requests.
        /// </summary>
        public void StopListening()
        {
            _httpListener.Stop();
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {

            if (!_disposed && _httpListener != null)
            {
                _httpListener.Close();
                _httpListener = null;
            }

            _disposed = true;

        }

    }

}