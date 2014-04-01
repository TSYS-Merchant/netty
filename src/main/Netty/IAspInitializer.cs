namespace Netty
{

    using System.Web.Hosting;

    /// <summary>
    ///     Provides an interface for classes that can initialize the configuration of an ASP.Net server.
    /// </summary>
    internal interface IAspInitializer : IRegisteredObject
    {

        /// <summary>
        ///     Initializes the <see cref="T:Netty.AspRequestProcessor" /> based on the configuration within the context of an application.
        /// </summary>
        /// <param name="virtualPath">The virtual path used by the ASP.Net server.</param>
        /// <param name="physicalPath">The physical path used by the ASP.Net server.</param>
        /// <returns>
        ///     An <see cref="T:Netty.AspRequestProcessor" /> used for the ASP.Net server.
        /// </returns>
        AspRequestProcessor ConfigurationInitializer(string virtualPath, string physicalPath);

    }

}