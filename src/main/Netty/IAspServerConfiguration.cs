namespace Netty
{

    using System;

    /// <summary>
    ///     Provides configuration information about an ASP.Net web server.
    /// </summary>
    internal interface IAspServerConfiguration
    {

        /// <summary>
        ///     Gets or sets the type of the initializer used by the configuration.
        /// </summary>
        /// <value>
        ///     The type of the initializer used by the configuration.
        /// </value>
        Type Initializer { get; set; }

        /// <summary>
        ///     Gets or sets the physical path to the website files.
        /// </summary>
        /// <value>
        ///     The physical path to the website files.
        /// </value>
        string PhysicalPath { get; set; }

        /// <summary>
        ///     Gets or sets the port the ASP.Net server listens on.
        /// </summary>
        /// <value>
        ///     The port the ASP.Net server listens on.
        /// </value>
        int Port { get; set; }

        /// <summary>
        ///     Gets or sets the virtual path used by the ASP.Net server to represent the website.
        /// </summary>
        /// <value>
        ///     The virtual path used by the ASP.Net server to represent the website.
        /// </value>
        string VirtualPath { get; set; }

    }

}