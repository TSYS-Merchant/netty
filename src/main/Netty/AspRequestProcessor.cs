namespace Netty
{

    using System;

    /// <summary>
    ///     A class that executes requests.
    /// </summary>
    internal class AspRequestProcessor : MarshalByRefObject, IAspRequestProcessor
    {

        /// <summary>
        ///     Gets or sets the default HTTP handler used by the ASP.Net server.
        /// </summary>
        /// <value>
        ///     The default HTTP handler used by the ASP.Net server.
        /// </value>
        public IHttpHandler DefaultHandler { get; set; }

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

        /// <summary>
        ///     Processes the request from the <see cref="T:Netty.IHttpContext" />.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the request.</param>
        public void ProcessRequest(IHttpContext httpContext)
        {

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            this.DefaultHandler.ProcessRequest(httpContext);

        }

    }

}