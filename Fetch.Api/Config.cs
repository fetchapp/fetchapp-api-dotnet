using System;
using System.Text;

namespace Fetch.Api
{
    /// <summary>
    /// Configuration parameters for the connection to Fetch
    /// </summary>
    public static class Config
    {

        /// <summary>
        /// The domain used to connect to your account
        /// </summary>
        /// <example>http://&lt;subdomain.fetchapp.com&gt;/</example>
        public static string Domain
        {
            get { return domain; }
            set { domain = value; }
        }
        private static string domain;

        /// <summary>
        /// The subdomain used to connect to your account. DEPRECIATED.
        /// </summary>
        /// <example>http://&lt;subdomain&gt;.fetchapp.com/</example>
        [Obsolete("Please use Domain instead")]
        public static string SubDomain
        {
            get { return subDomain; }
            set { subDomain = value; }
        }
        private static string subDomain;

        /// <summary>
        /// Authentication key used to log into the Fetch API. 
        /// Log in to your Fetch website and click "API" to retrieve your Key and Token.
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }
        private static string key;

        /// <summary>
        /// Authentication token used to log into the Fetch API.
        /// Log in to your Fetch website and click "API" to retrieve your Key and Token.
        /// </summary>
        public static string Token
        {
            get { return token; }
            set { token = value; }
        }
        private static string token;

        /// <summary>
        /// Retrieves the key/token pair encoded to be sent in a request to authenticate 
        /// the request
        /// </summary>
        /// <returns>base64 encoded key/token pair</returns>
        internal static string GetAuthorization()
        {
            if ( string.IsNullOrEmpty( Config.Key ) || string.IsNullOrEmpty( Config.Token ) )
                throw new FetchException( "Fetch API not configured.  Please set values in Fetch.Api.Config" );

            return Convert.ToBase64String( Encoding.UTF8.GetBytes( string.Format( "{0}:{1}", key, token ) ) );
        }

    }
}
