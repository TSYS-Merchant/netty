namespace Netty
{

    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    ///     Provides a concrete implementation of IHttpRequest based on a HttpListenerRequest.
    /// </summary>
    internal class HttpRequest : MarshalByRefObject, IHttpRequest, IDisposable
    {

        private readonly HttpListenerRequest _request;
        private bool _disposed = false;
        private MemoryStream _requestStream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpRequest" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public HttpRequest(HttpListenerRequest request)
        {
            _request = request;

            _requestStream = new MemoryStream();

            int bytesRead;
            byte[] buffer = new byte[4096];

            while ((bytesRead = _request.InputStream.Read(buffer, 0, 4096)) > 0)
            {
                _requestStream.Write(buffer, 0, bytesRead);
            }

            // Reset the position
            _requestStream.Position = 0;

        }

        /// <summary>
        ///     Gets the MIME type of the body data included in the request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the text of the request's Content-Type header.
        /// </value>
        public string ContentType
        {
            get { return _request.ContentType; }
        }

        /// <summary>
        ///     Gets the collection of header name/value pairs sent in the request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Net.WebHeaderCollection" /> that contains the HTTP headers included in the request.
        /// </value>
        public System.Collections.Specialized.NameValueCollection Headers
        {
            get { return _request.Headers; }
        }

        /// <summary>
        ///     Gets the HTTP method specified by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the method used in the request.
        /// </value>
        public string HttpMethod
        {
            get { return _request.HttpMethod; }
        }

        /// <summary>
        ///     Gets a stream that contains the body data sent by the client.
        /// </summary>
        /// <value>
        ///     A readable <see cref="System.IO.Stream" /> object that contains the bytes sent by the client
        ///     in the body of the request. This property returns <see cref="System.IO.Stream.Null" /> if
        ///     no data is sent with the request.
        /// </value>
        public System.IO.Stream InputStream
        {
            get { return _requestStream; }
        }

        /// <summary>
        ///     Gets a value indicating whether whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the connection is an SSL connection; otherwise, <c>false</c>.
        /// </value>
        public bool IsSecureConnection
        {
            get { return _request.IsSecureConnection; }
        }

        /// <summary>
        ///     Gets the server IP address and port number to which the request is directed.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address that the request is sent to.
        /// </value>
        public IPEndPoint LocalEndPoint
        {
            get { return _request.LocalEndPoint; }
        }

        /// <summary>
        ///     Gets the HTTP version used by the requesting client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Version" /> that identifies the client's version of HTTP.
        /// </value>
        public Version ProtocolVersion
        {
            get { return _request.ProtocolVersion; }
        }

        /// <summary>
        ///     Gets the query string included in the request.
        /// </summary>
        /// <value>
        ///     A System.Collections.Specialized.NameValueCollection object that contains
        ///     the query data included in the request <see cref="P:Netty.IHttpRequest.Url" />.
        /// </value>
        public System.Collections.Specialized.NameValueCollection QueryString
        {
            get { return _request.QueryString; }
        }

        /// <summary>
        ///     Gets the URL information (without the host and port) requested by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the raw URL for this request.
        /// </value>
        public string RawUrl
        {
            get { return _request.RawUrl; }
        }

        /// <summary>
        ///     Gets the value of the <c>Referer</c> HTTP header.
        /// </summary>
        /// <value>
        ///     The value of the <c>Referer</c> HTTP header. The default value is <c>null</c>.
        /// </value>
        public string Referer
        {
            get
            {
                string referer = null;

                if (_request.UrlReferrer != null)
                {
                    referer = _request.UrlReferrer.ToString();
                }

                return referer;
            }
        }

        /// <summary>
        ///     Gets the client IP address and port number from which the request originated.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address and port number from which the request originated.
        /// </value>
        public IPEndPoint RemoteEndPoint
        {
            get { return _request.RemoteEndPoint; }
        }

        /// <summary>
        ///     Gets the <see cref="T:System.Uri" /> object requested by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Uri" /> object that identifies the resource requested by the client.
        /// </value>
        public Uri Url
        {
            get { return _request.Url; }
        }

        /// <summary>
        ///     Gets the raw user agent string of the client browser.
        /// </summary>
        /// <value>
        ///     The raw user agent string of the client browser.
        /// </value>
        public string UserAgent
        {
            get { return _request.UserAgent; }
        }

        #region MarshalObjByRef Members

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

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_requestStream != null && !_disposed)
            {
                _requestStream.Dispose();
                _requestStream = null;
            }

            _disposed = true;
        }

        #endregion
    }

}