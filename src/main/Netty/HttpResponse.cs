namespace Netty
{

    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    ///     Provides a concrete implementation of the IHttpResponse interface based on a HttpListenerResponse.
    /// </summary>
    internal class HttpResponse : MarshalByRefObject, IHttpResponse, IDisposable
    {

        private readonly HttpListenerResponse _response;
        private bool _disposed;
        private MemoryStream _responseStream;
        private int _statusCode;
        private string _statusDescription;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpResponse" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        public HttpResponse(HttpListenerResponse response)
        {
            _response = response;
            _responseStream = new MemoryStream();

            _statusCode = HttpStatusCodes.HttpOk;
            _statusDescription = HttpStatusCodes.GetStatusDescription(_statusCode);
        }

        /// <summary>
        ///     Gets or sets the MIME type of the content returned.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> instance that contains the text of the response's Content-Type header.
        /// </value>
        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        /// <summary>
        ///     Gets the collection of header name/value pairs returned by the server.
        /// </summary>
        /// <value>
        ///     A System.Net.WebHeaderCollection instance that contains all the explicitly set HTTP headers to be included in the response.
        /// </value>
        public WebHeaderCollection Headers
        {
            get { return _response.Headers; }
        }

        /// <summary>
        ///     Gets the output stream for the HTTP response.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.IO.Stream"/> object to which a response can be written.
        /// </value>
        public Stream OutputStream
        {
            get { return _responseStream; }
        }

        /// <summary>
        ///     Gets or sets the HTTP status code to be returned to the client.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Int32" /> value that specifies the HTTP status code for the requested.
        /// </value>
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        ///     Gets or sets a text description of the HTTP status code returned to the client.
        /// </summary>
        /// <value>
        ///     The text description of the HTTP status code returned to the client.
        /// </value>
        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }

        /// <summary>
        ///     Adds the specified header and value to the HTTP headers for this response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to set.</param>
        /// <param name="value">The value for the name header.</param>
        public void AddHeader(string name, string value)
        {
            _response.AddHeader(name, value);
        }

        /// <summary>
        ///     Appends a value to the specified HTTP header to be sent with this response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to append value to.</param>
        /// <param name="value">The value to append to the name header.</param>
        public void AppendHeader(string name, string value)
        {
            _response.AppendHeader(name, value);
        }

        /// <summary>
        ///     Clears the output stream.
        /// </summary>
        public void ClearOutputStream()
        {
            _responseStream.Close();
            _responseStream = new MemoryStream();
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
        ///     Flushes all data in the instance to the underlying HttpListenerResponse.
        /// </summary>
        public void FlushResponse()
        {

            // Set the status code
            _response.StatusCode = _statusCode;

            // Fix the description - if no description is found, use the one provided.
            _response.StatusDescription = HttpStatusCodes.GetStatusDescription(_statusCode) ?? _statusDescription;

            // Update to not use HTTP chunked transfer encoding
            _response.ContentLength64 = _responseStream.Length;
            _response.SendChunked = false;

            // Dump out the response output stream
            var buffer = new byte[4096];
            int bytesRead;

            _responseStream.Position = 0;

            while ((bytesRead = _responseStream.Read(buffer, 0, 4096)) > 0)
            {
                _response.OutputStream.Write(buffer, 0, bytesRead);
            }

            _responseStream.Close();
            _response.OutputStream.Close();
            _response.Close();

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
        ///     Configures the response to redirect the client to the specified URL.
        /// </summary>
        /// <param name="url">The URL that the client should use to locate the requested resource.</param>
        public void Redirect(string url)
        {
            _response.Redirect(url);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_responseStream != null && !_disposed)
            {
                _responseStream.Dispose();
                _responseStream = null;
            }

            _disposed = true;
        }

    }

}