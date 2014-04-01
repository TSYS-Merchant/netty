namespace Netty
{

    using System.Collections.Generic;

    /// <summary>
    ///     A class that contains standard HTTP status codes.
    /// </summary>
    public static class HttpStatusCodes
    {

        /// <summary>
        ///     HTTP Status 200 - Ok.
        /// </summary>
        public const int HttpOk = 200;

        /// <summary>
        ///     HTTP 202 - Accepted.
        /// </summary>
        public const int HttpAccepted = 202;

        /// <summary>
        ///     HTTP Error 400 - Bad Request.
        /// </summary>
        public const int HttpBadRequest = 400;

        /// <summary>
        ///     HTP Error 401 - Unauthorized.
        /// </summary>
        public const int HttpUnauthorized = 401;

        /// <summary>
        ///     HTTP Error 403 - Forbidden.
        /// </summary>
        public const int HttpForbidden = 403;

        /// <summary>
        ///     HTTP Error 404 - Not Found.
        /// </summary>
        public const int HttpNotFound = 404;

        /// <summary>
        ///     HTTP Error 411 - Length required.
        /// </summary>
        public const int HttpLengthRequired = 411;

        /// <summary>
        ///     HTTP Error 500 - Internal Server Error.
        /// </summary>
        public const int HttpInternalServerError = 500;

        /// <summary>
        ///     HTTP Error 503 - Service Unavailable.
        /// </summary>
        public const int HttpServiceUnavailable = 503;

        private static readonly Dictionary<int, string> Descriptions = new Dictionary<int, string>()
        {
            {
                HttpStatusCodes.HttpOk, "OK"
            },
            {
                HttpStatusCodes.HttpAccepted, "Accepted"
            },
            {
                HttpStatusCodes.HttpBadRequest, "Bad Request"
            },
            {
                HttpStatusCodes.HttpUnauthorized, "Unauthorized"
            },
            {
                HttpStatusCodes.HttpForbidden, "Forbidden"
            },
            {
                HttpStatusCodes.HttpNotFound, "Not Found"
            },
            {
                HttpStatusCodes.HttpLengthRequired, "Length Required"
            },
            {
                HttpStatusCodes.HttpInternalServerError, "Internal Server Error"
            },
            {
                HttpStatusCodes.HttpServiceUnavailable, "Service Unavailable"
            }
        };

        /// <summary>
        ///     Gets the description for a specified HTTP status code.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the description for the <paramref name="httpStatusCode" />.
        /// </returns>
        public static string GetStatusDescription(int httpStatusCode)
        {

            string description = null;

            if (Descriptions.ContainsKey(httpStatusCode))
            {
                description = Descriptions[httpStatusCode];
            }

            return description;

        }

    }

}