namespace Netty
{

    using System;

    /// <summary>
    ///     A class used to initialize a website within the context of an ASP.Net server.
    /// </summary>
    internal class WebsiteInitializer : MarshalByRefObject, IAspInitializer
    {

        /// <summary>
        ///     Initializes the <see cref="T:Netty.AspRequestProcessor" /> based on the configuration within the context of an application.
        /// </summary>
        /// <param name="virtualPath">The virtual path used by the ASP.Net server.</param>
        /// <param name="physicalPath">The physical path used by the ASP.Net server.</param>
        /// <returns>
        ///     An <see cref="T:Netty.AspRequestProcessor" /> used for the ASP.Net server.
        /// </returns>
        public AspRequestProcessor ConfigurationInitializer(string virtualPath, string physicalPath)
        {

            var processor = new AspRequestProcessor();

            // Setup the ASP.Net handler
            var aspHandler = new AspNetHandler()
            {
                PhysicalPath = physicalPath,
                VirtualPath = virtualPath
            };

            // Set the default handler
            processor.DefaultHandler = aspHandler;

            // Returns the processor
            return processor;

        }

        /// <summary>
        ///     Requests a registered object to unregister.
        /// </summary>
        /// <param name="immediate"><c>true</c> to indicate the registered object should unregister from the hosting environment before returning; otherwise, <c>false</c>.</param>
        public void Stop(bool immediate)
        {
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

    }

}