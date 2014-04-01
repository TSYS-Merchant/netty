namespace Netty
{

    using System;
    using System.Security.Permissions;
    using System.Web;

    /// <summary>
    ///     Provides an HTTP handler for processing ASP.NET requests.
    /// </summary>
    internal class AspNetHandler : MarshalByRefObject, IHttpHandler
    {

        /// <summary>
        ///     Gets or sets the physical path the files for the ASP.NET website are located.
        /// </summary>
        /// <value>
        ///     The physical path the files for the ASP.NET website are located.
        /// </value>
        public string PhysicalPath { get; set; }

        /// <summary>
        ///     Gets or sets the virtual path used by the ASP.NET runtime.
        /// </summary>
        /// <value>
        ///     The virtual path used by the ASP.NET runtime.
        /// </value>
        public string VirtualPath { get; set; }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the
        /// <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" />
        /// property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        [System.Security.SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        ///     Processes the request from the <see cref="T:Netty.IHttpContext" />.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     <c>true</c> to continue processing; otherwise, <c>false</c>.
        /// </returns>
        [AspNetHostingPermission(SecurityAction.Assert, Level = AspNetHostingPermissionLevel.Medium)]
        public bool ProcessRequest(IHttpContext context)
        {

            var workerRequest = new AspServerRequest(context, this.VirtualPath, this.PhysicalPath);

            HttpRuntime.ProcessRequest(workerRequest);

            return true;

        }

    }

}