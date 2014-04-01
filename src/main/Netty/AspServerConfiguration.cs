namespace Netty
{

    using System;

    /// <summary>
    ///     Provides a concrete implementation of the IAspServerConfiguration interface.
    /// </summary>
    internal class AspServerConfiguration : MarshalByRefObject, IAspServerConfiguration
    {

        /// <summary>
        ///     Gets or sets the type of the initializer used by the configuration.
        /// </summary>
        /// <value>
        ///     The type of the initializer used by the configuration.
        /// </value>
        public Type Initializer { get; set; }

        /// <summary>
        ///     Gets or sets the port the ASP.Net server listens on.
        /// </summary>
        /// <value>
        ///     The port the ASP.Net server listens on.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        ///     Gets or sets the physical path to the website files.
        /// </summary>
        /// <value>
        ///     The physical path to the website files.
        /// </value>
        public string PhysicalPath { get; set; }

        /// <summary>
        ///     Gets or sets the virtual path used by the ASP.Net server to represent the website.
        /// </summary>
        /// <value>
        ///     The virtual path used by the ASP.Net server to represent the website.
        /// </value>
        public string VirtualPath { get; set; }

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

    }

}