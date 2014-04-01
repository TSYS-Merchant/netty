namespace Netty
{

    using System;
    using System.Net;

    /// <summary>
    ///     A class that provides a concrete implementation of the IHttpContext interface.
    /// </summary>
    internal class HttpContext : MarshalByRefObject, IHttpContext, IDisposable
    {

        private bool _disposed;
        private HttpRequest _request;
        private HttpResponse _response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpContext" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public HttpContext(HttpListenerContext context)
        {
            ApplyNewContext(context);
        }

        /// <summary>
        ///     Gets the <see cref="T:Netty.IHttpRequest" /> that represents a client's request for a resource.
        /// </summary>
        /// <value>
        ///     A <see cref="T:Netty.IHttpRequest" /> that represents a client's request for a resource.
        /// </value>
        public IHttpRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        ///     Gets the <see cref="T:Netty.IHttpResponse" /> object containing the response that will be sent to the client in response to the client's request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:Netty.IHttpResponse" /> object containing the response that will be sent to the client in response to the client's request.
        /// </value>
        public IHttpResponse Response
        {
            get { return _response; }
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
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_request != null)
                {
                    _request.Dispose();
                    _request = null;
                }

                if (_response != null)
                {
                    _response.Dispose();
                    _response = null;
                }
            }

            _disposed = true;
        }

        /// <summary>
        ///     Updates the context based on an HttpListenerContext.
        /// </summary>
        /// <param name="context">The HttpListenerContext used to update this instance.</param>
        private void ApplyNewContext(HttpListenerContext context)
        {

            _request = new HttpRequest(context.Request);
            _response = new HttpResponse(context.Response);

        }

    }

}