namespace Netty
{

    /// <summary>
    ///     An interface for a module that performs actions against an incoming HTTP request.
    /// </summary>
    internal interface IHttpHandler
    {

        /// <summary>
        ///     Processes the request from the <see cref="T:Netty.IHttpContext" />.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     <c>true</c> to continue processing; otherwise, <c>false</c>.
        /// </returns>
        bool ProcessRequest(IHttpContext context);

    }

}