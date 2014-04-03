namespace Netty
{

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>
    ///     Provides an HttpWorkerRequest that is used by the ASP.Net runtime.
    /// </summary>
    internal class AspServerRequest : HttpWorkerRequest
    {

        private const string HttpVersionFormat = "HTTP/{0}.{1}";

        private readonly IHttpContext _context;
        private readonly string _physicalPath;
        private readonly string _virtualPath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AspServerRequest" /> class.
        /// </summary>
        /// <param name="context">The HTTP context used by the ASP.Net request.</param>
        /// <param name="virtualPath">The virtual path for the ASP.Net website.</param>
        /// <param name="physicalPath">The physical path to the files for the ASP.Net website.</param>
        public AspServerRequest(IHttpContext context, string virtualPath, string physicalPath)
        {

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }

            if (physicalPath == null)
            {
                throw new ArgumentNullException("physicalPath");
            }

            _context = context;
            _virtualPath = virtualPath;
            _physicalPath = physicalPath;
        }

        /// <summary>
        ///     Used by the runtime to notify the <see cref="T:System.Web.HttpWorkerRequest" /> that request processing for the current request is complete.
        /// </summary>
        public override void EndOfRequest()
        {
            // Do not close - let the finished pipeline do that
        }

        /// <summary>
        ///     Sends all pending response data to the client.
        /// </summary>
        /// <param name="finalFlush"><c>true</c> if this is the last time response data will be flushed; otherwise, <c>false</c>.</param>
        public override void FlushResponse(bool finalFlush)
        {
            _context.Response.OutputStream.Flush();
        }

        /// <summary>
        ///     Returns the specified member of the request header.
        /// </summary>
        /// <returns>
        ///     The HTTP verb returned in the request header.
        /// </returns>
        public override string GetHttpVerbName()
        {
            return _context.Request.HttpMethod;
        }

        /// <summary>
        ///     Provides access to the HTTP version of the request (for example, "HTTP/1.1").
        /// </summary>
        /// <returns>
        ///     The HTTP version returned in the request header.
        /// </returns>
        public override string GetHttpVersion()
        {
            var version = string.Format(
                CultureInfo.InvariantCulture, 
                AspServerRequest.HttpVersionFormat, 
                _context.Request.ProtocolVersion.Major,
                _context.Request.ProtocolVersion.Minor);

            return version;
        }

        /// <summary>
        ///     Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>
        ///     The server IP address returned in the request header.
        /// </returns>
        public override string GetLocalAddress()
        {
            return _context.Request.LocalEndPoint.Address.ToString();
        }

        /// <summary>
        ///     Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>
        ///     The server port number returned in the request header.
        /// </returns>
        public override int GetLocalPort()
        {
            return _context.Request.LocalEndPoint.Port;
        }

        /// <summary>
        ///     Returns the query string specified in the request URL.
        /// </summary>
        /// <returns>
        ///     The request query string.
        /// </returns>
        public override string GetQueryString()
        {
            var queryString = string.Empty;
            var delimiter = _context.Request.RawUrl.IndexOf('?');

            if (delimiter > -1)
            {
                queryString = _context.Request.RawUrl.Substring(delimiter + 1);
            }

            return queryString;
        }

        /// <summary>
        ///     Returns the URL path contained in the request header with the query string appended.
        /// </summary>
        /// <returns>
        ///     The raw URL path of the request header.
        /// </returns>
        public override string GetRawUrl()
        {
            return _context.Request.RawUrl;
        }

        /// <summary>
        ///     Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>
        ///     The client's IP address.
        /// </returns>
        public override string GetRemoteAddress()
        {
            return _context.Request.RemoteEndPoint.Address.ToString();
        }

        /// <summary>
        ///     Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>
        ///     The client's HTTP port number.
        /// </returns>
        public override int GetRemotePort()
        {
            return _context.Request.RemoteEndPoint.Port;
        }

        /// <summary>
        ///     Returns the virtual path to the requested URI.
        /// </summary>
        /// <returns>
        ///     The path to the requested URI.
        /// </returns>
        public override string GetUriPath()
        {
            return _context.Request.Url.LocalPath;
        }

        /// <summary>
        ///     Adds a standard HTTP header to the response.
        /// </summary>
        /// <param name="index">
        ///     The header index. For example, <see cref="F:System.Web.HttpWorkerRequest.HeaderContentLength" />.
        /// </param>
        /// <param name="value">The value of the header.</param>
        public override void SendKnownResponseHeader(int index, string value)
        {
            var headerName = HttpWorkerRequest.GetKnownResponseHeaderName(index);
            _context.Response.Headers[headerName] = value;
        }

        /// <summary>
        ///     Adds the specified number of bytes from a byte array to the response.
        /// </summary>
        /// <param name="data">The byte array to send.</param>
        /// <param name="length">The number of bytes to send, starting at the first byte.</param>
        public override void SendResponseFromMemory(byte[] data, int length)
        {
            _context.Response.OutputStream.Write(data, 0, length);
        }

        /// <summary>
        ///     Specifies the HTTP status code and status description of the response, such as SendStatus(200, "Ok").
        /// </summary>
        /// <param name="statusCode">The status code to send.</param>
        /// <param name="statusDescription">The status description to send.</param>
        public override void SendStatus(int statusCode, string statusDescription)
        {
            _context.Response.StatusCode = statusCode;
            _context.Response.StatusDescription = statusDescription;
        }

        /// <summary>
        ///     Adds a nonstandard HTTP header to the response.
        /// </summary>
        /// <param name="name">The name of the header to send.</param>
        /// <param name="value">The value of the header.</param>
        public override void SendUnknownResponseHeader(string name, string value)
        {
            _context.Response.Headers[name] = value;
        }

        /// <summary>
        ///     Adds the contents of the specified file to the response and specifies the starting position in the file and the number of bytes to send.
        /// </summary>
        /// <param name="handle">The handle of the file to send.</param>
        /// <param name="offset">The starting position in the file.</param>
        /// <param name="length">The number of bytes to send.</param>
        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
        }

        /// <summary>
        ///     Adds the contents of the specified file to the response and specifies the starting position in the file and the number of bytes to send.
        /// </summary>
        /// <param name="filename">The name of the file to send.</param>
        /// <param name="offset">The starting position in the file.</param>
        /// <param name="length">The number of bytes to send.</param>
        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            if (File.Exists(filename))
            {
                var buf = new byte[length];
                var bytesRead = 0;

                using (var infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    bytesRead = infile.Read(buf, (int)offset, (int)length);
                }

                _context.Response.OutputStream.Write(buf, 0, bytesRead);
            }
        }

        /// <summary>
        ///     Terminates the connection with the client.
        /// </summary>
        public override void CloseConnection()
        {
        }

        /// <summary>
        ///     Returns the virtual path to the currently executing server application.
        /// </summary>
        /// <returns>
        ///     The virtual path of the current application.
        /// </returns>
        public override string GetAppPath()
        {
            return _virtualPath;
        }

        /// <summary>
        ///     Returns the physical path to the currently executing server application.
        /// </summary>
        /// <returns>
        ///     The physical path of the current application.
        /// </returns>
        public override string GetAppPathTranslated()
        {
            return _physicalPath;
        }

        /// <summary>
        ///     Reads request data from the client (when not preloaded).
        /// </summary>
        /// <param name="buffer">The byte array to read data into.</param>
        /// <param name="size">The maximum number of bytes to read.</param>
        /// <returns>
        ///     The number of bytes read.
        /// </returns>
        public override int ReadEntityBody(byte[] buffer, int size)
        {
            var bytesRead = _context.Request.InputStream.Read(buffer, 0, size);
            return bytesRead;
        }

        /// <summary>
        ///     Returns a nonstandard HTTP request header value.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <returns>
        ///     The header value.
        /// </returns>
        public override string GetUnknownRequestHeader(string name)
        {
            return _context.Request.Headers[name];
        }

        /// <summary>
        ///     Get all nonstandard HTTP header name-value pairs.
        /// </summary>
        /// <returns>
        ///     An array of header name-value pairs.
        /// </returns>
        public override string[][] GetUnknownRequestHeaders()
        {
            System.Collections.Specialized.NameValueCollection headers = _context.Request.Headers;

            var count = headers.Count;
            var headerPairs = new List<string[]>(count);

            for (var i = 0; i < count; i++)
            {
                var headerName = headers.GetKey(i);
                if (HttpWorkerRequest.GetKnownRequestHeaderIndex(headerName) == -1)
                {
                    string headerValue = headers.Get(i);
                    headerPairs.Add(
                        new string[]
                        {
                            headerName, headerValue
                        });
                }
            }

            string[][] unknownRequestHeaders = headerPairs.ToArray();
            return unknownRequestHeaders;
        }

        /// <summary>
        ///     Returns the standard HTTP request header that corresponds to the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index of the header. For example, the <see cref="F:System.Web.HttpWorkerRequest.HeaderAllow" /> field.
        /// </param>
        /// <returns>
        ///     The HTTP request header.
        /// </returns>
        public override string GetKnownRequestHeader(int index)
        {
            switch (index)
            {
                case HeaderUserAgent:
                    return _context.Request.UserAgent;
                default:
                    var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);
                    return _context.Request.Headers[name];
            }
        }

        /// <summary>
        ///     Returns a single server variable from a dictionary of server variables associated with the request.
        /// </summary>
        /// <param name="name">The name of the requested server variable.</param>
        /// <returns>
        ///     The requested server variable.
        /// </returns>
        public override string GetServerVariable(string name)
        {
            switch (name)
            {
                case "HTTPS":
                    return _context.Request.IsSecureConnection ? "on" : "off";

                case "HTTP_USER_AGENT":
                    return _context.Request.Headers["User-Agent"];

                case "LOGON_USER":
                    return string.Empty;

                case "AUTH_TYPE":
                    return string.Empty;

                default:
                    return null;

            }

        }

        /// <summary>
        ///     When overridden in a derived class, returns the client's impersonation token.
        /// </summary>
        /// <returns>
        ///     A value representing the client's impersonation token. The default is 0.
        /// </returns>
        public override IntPtr GetUserToken()
        {
            return IntPtr.Zero;
        }

        /// <summary>
        ///     When overridden in a derived class, returns the virtual path to the requested URI.
        /// </summary>
        /// <returns>
        ///     The path to the requested URI.
        /// </returns>
        public override string GetFilePath()
        {
            var filePath = _context.Request.Url.LocalPath;

            var aspxPosition = filePath.IndexOf(".aspx", StringComparison.OrdinalIgnoreCase);
            var asmxPosition = filePath.IndexOf(".asmx", StringComparison.OrdinalIgnoreCase);
            var finalPosition = asmxPosition > -1 ? asmxPosition : aspxPosition;

            if (finalPosition > -1)
            {
                filePath = filePath.Substring(0, finalPosition + 5);
            }

            return filePath;
        }

        /// <summary>
        ///     Returns the physical file path to the requested URI and translates it from virtual path to physical path.
        /// </summary>
        /// <returns>
        ///     The translated physical file path to the requested URI.
        /// </returns>
        public override string GetFilePathTranslated()
        {

            var translatedPath = this.GetFilePath();

            if (translatedPath.Length > _virtualPath.Length)
            {
                translatedPath = translatedPath.Substring(_virtualPath.Length);
            }

            translatedPath = translatedPath.Replace('/', '\\');
            translatedPath = Path.Combine(_physicalPath, translatedPath);

            return translatedPath;

        }

        /// <summary>
        ///     Returns additional path information for a resource with a URL extension.
        /// </summary>
        /// <returns>
        ///     Additional path information for a resource.
        /// </returns>
        public override string GetPathInfo()
        {

            var s1 = GetFilePath();
            var s2 = _context.Request.Url.LocalPath;

            if (s1.Length == s2.Length)
            {
                return string.Empty;
            }

            return s2.Substring(s1.Length);

        }

    }

}