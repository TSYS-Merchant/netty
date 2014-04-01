namespace Netty
{

    /// <summary>
    ///     An interface that provides a generic implementation around the context of all HTTP requests and responses.
    /// </summary>
    internal interface IHttpContext
    {

        /// <summary>
        ///     Gets the <see cref="T:Netty.IHttpRequest" /> that represents a client's request for a resource.
        /// </summary>
        /// <value>
        ///     A <see cref="T:Netty.IHttpRequest" /> that represents a client's request for a resource.
        /// </value>
        IHttpRequest Request { get; }

        /// <summary>
        ///     Gets the <see cref="T:Netty.IHttpResponse" /> object containing the response that will be sent to the client in response to the client's request.
        /// </summary>
        /// <value>
        ///     A <see cref="T:Netty.IHttpResponse" /> object containing the response that will be sent to the client in response to the client's request.
        /// </value>
        IHttpResponse Response { get; }

    }

}