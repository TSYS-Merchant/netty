namespace Netty
{

    using System.IO;
    using System.Net;

    /// <summary>
    ///     An interface that provides a generic implementation around the context of an HTTP request.
    /// </summary>
    internal interface IHttpResponse
    {

        /// <summary>
        ///     Gets or sets the MIME type of the content returned.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.String" /> instance that contains the text of the response's Content-Type header.
        /// </value>
        string ContentType { get; set; }

        /// <summary>
        ///     Gets the collection of header name/value pairs returned by the server.
        /// </summary>
        /// <value>
        ///     A System.Net.WebHeaderCollection instance that contains all the explicitly set HTTP headers to be included in the response.
        /// </value>
        WebHeaderCollection Headers { get; }

        /// <summary>
        ///     Gets the output stream for the HTTP response.
        /// </summary>
        /// <value>
        ///     A <see cref="T:System.IO.Stream"/> object to which a response can be written.
        /// </value>
        Stream OutputStream { get; }

        /// <summary>
        ///     Gets or sets the HTTP status code to be returned to the client.
        /// </summary>
        /// <value>
        ///     An <see cref="T:System.Int32" /> value that specifies the HTTP status code for the requested.
        /// </value>
        int StatusCode { get; set; }

        /// <summary>
        ///     Gets or sets a text description of the HTTP status code returned to the client.
        /// </summary>
        /// <value>
        ///     The text description of the HTTP status code returned to the client.
        /// </value>
        string StatusDescription { get; set; }

        /// <summary>
        ///     Adds the specified header and value to the HTTP headers for this response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to set.</param>
        /// <param name="value">The value for the name header.</param>
        void AddHeader(string name, string value);

        /// <summary>
        ///     Appends a value to the specified HTTP header to be sent with this response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to append value to.</param>
        /// <param name="value">The value to append to the name header.</param>
        void AppendHeader(string name, string value);

        /// <summary>
        ///     Clears the output stream.
        /// </summary>
        void ClearOutputStream();

        /// <summary>
        ///     Flushes all data in the instance to the underlying HttpListenerResponse.
        /// </summary>
        void FlushResponse();

        /// <summary>
        ///     Configures the response to redirect the client to the specified URL.
        /// </summary>
        /// <param name="url">The URL that the client should use to locate the requested resource.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Based on the underlying framework.")]
        void Redirect(string url);

    }

}