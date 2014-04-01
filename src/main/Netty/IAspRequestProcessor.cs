namespace Netty
{

    /// <summary>
    ///     An interface that represents an ASP.Net request processor.
    /// </summary>
    internal interface IAspRequestProcessor
    {

        /// <summary>
        ///     Gets or sets the default HTTP handler used by the ASP.Net server.
        /// </summary>
        /// <value>
        ///     The default HTTP handler used by the ASP.Net server.
        /// </value>
        IHttpHandler DefaultHandler { get; set; }

        /// <summary>
        ///     Processes the request from the <see cref="T:Netty.IHttpContext" />.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the request.</param>
        void ProcessRequest(IHttpContext httpContext);

    }

}