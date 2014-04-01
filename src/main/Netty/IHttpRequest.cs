namespace Netty
{

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;

    /// <summary>
    ///     An interface that provides a generic implementation around the context of an HTTP request.
    /// </summary>
    internal interface IHttpRequest
    {

        /// <summary>
        ///     Gets the MIME type of the body data included in the request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the text of the request's Content-Type header.
        /// </value>
        string ContentType { get; }

        /// <summary>
        ///     Gets the collection of header name/value pairs sent in the request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Net.WebHeaderCollection" /> that contains the HTTP headers included in the request.
        /// </value>
        NameValueCollection Headers { get; }

        /// <summary>
        ///     Gets the HTTP method specified by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the method used in the request.
        /// </value>
        string HttpMethod { get; }

        /// <summary>
        ///     Gets a stream that contains the body data sent by the client.
        /// </summary>
        /// <value>
        ///     A readable <see cref="System.IO.Stream" /> object that contains the bytes sent by the client
        ///     in the body of the request. This property returns <see cref="System.IO.Stream.Null" /> if
        ///     no data is sent with the request.
        /// </value>
        Stream InputStream { get; }

        /// <summary>
        ///     Gets a value indicating whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the connection is an SSL connection; otherwise, <c>false</c>.
        /// </value>
        bool IsSecureConnection { get; }

        /// <summary>
        ///     Gets the server IP address and port number to which the request is directed.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address that the request is sent to.
        /// </value>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        ///     Gets the HTTP version used by the requesting client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Version" /> that identifies the client's version of HTTP.
        /// </value>
        Version ProtocolVersion { get; }

        /// <summary>
        ///     Gets the query string included in the request.
        /// </summary>
        /// <value>
        ///     A System.Collections.Specialized.NameValueCollection object that contains
        ///     the query data included in the request <see cref="P:Netty.IHttpRequest.Url" />.
        /// </value>
        NameValueCollection QueryString { get; }

        /// <summary>
        ///     Gets the URL information (without the host and port) requested by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> that contains the raw URL for this request.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Based on the underlying framework.")]
        string RawUrl { get; }

        /// <summary>
        ///     Gets the value of the <c>Referer</c> HTTP header.
        /// </summary>
        /// <value>
        ///     The value of the <c>Referer</c> HTTP header. The default value is <c>null</c>.
        /// </value>
        string Referer { get; }

        /// <summary>
        ///     Gets the client IP address and port number from which the request originated.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address and port number from which the request originated.
        /// </value>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        ///     Gets the <see cref="T:System.Uri" /> object requested by the client.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.Uri" /> object that identifies the resource requested by the client.
        /// </value>
        Uri Url { get; }

        /// <summary>
        ///     Gets the raw user agent string of the client browser.
        /// </summary>
        /// <value>
        ///     The raw user agent string of the client browser.
        /// </value>
        string UserAgent { get; }

    }

}