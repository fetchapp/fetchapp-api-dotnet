using System;
using System.IO;
using System.Net;
using System.Text;

namespace Fetch.Api
{

    /// <summary>
    /// sends and receives communications with a RESTful web service using GET and POST.
    /// </summary>
    /// <typeparam name="ResponseType">the type of object to be deserialized from the 
    /// response XML the web service returns</typeparam>
    internal class RestConnection<ResponseType>
    {
        private string hostname;

        /// <summary>
        /// default constructor
        /// </summary>
        public RestConnection()
        {
            if ( string.IsNullOrEmpty( Config.Key ) || string.IsNullOrEmpty( Config.Token ) )
                throw new FetchException( "Fetch API not configured.  Please set values in Fetch.Api.Config" );

            this.hostname = string.Format( "http://{0}.fetchapp.com/api/", Config.Subdomain );
        }

        /// <summary>
        /// initiates a GET request to the WS, and calls the given method
        /// </summary>
        /// <example>http://restful.webservice.com/&lt;methodName&gt;</example>
        /// <param name="methodName">name of the method to execute</param>
        /// <returns>an object deserialized from the XML returned</returns>
        public ResponseType InvokeGet( string methodName )
        {
            string url = string.Concat( hostname, methodName );
            ResponseType output = default( ResponseType );

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
                request.UserAgent = "";
                request.Method = "GET";
                request.Headers["Authorization"] = string.Format( "Basic {0}", Config.GetAuthorization() );

                WebResponse response = request.GetResponse();

                if ( response.ContentType.Contains( "application/xml" ) )
                {
                    output = Utility.DeserializeFromXmlStream<ResponseType>( response.GetResponseStream() );
                }
                else
                {
                    throw new NotImplementedException( string.Format( "Does not know how to deserialize this content: {0}", response.ContentType ) );
                }

            }
            catch ( WebException webex )
            {
                ThrowWebException( webex );
            }

            return output;
        }

        /// <summary>
        /// initiates a GET request to the WS, and calls the given method with the optional arguments
        /// </summary>
        /// <example>http://restful.webservice.com/&lt;methodName&gt;/&lt;arg0&gt;/&lt;arg1&gt;</example>
        /// <param name="methodName">name of the method to execute</param>
        /// <param name="arguments">list of additional arguments to add the the URL</param>
        /// <returns>an object deserialized from the XML returned</returns>
        public ResponseType InvokeGet( string methodName, params string[] arguments )
        {
            StringBuilder output = new StringBuilder( methodName );
            foreach ( string arg in arguments )
            {
                output.AppendFormat( "/{0}", arg );
            }
            return InvokeGet( output.ToString() );
        }

        /// <summary>
        /// initiates a POST request to the WS to the given method and POSTs the given object,
        /// serialized as XML
        /// </summary>
        /// <example>http://restful.webservice.com/&lt;methodName&gt;</example>
        /// <param name="methodName">name of the method to execute</param>
        /// <param name="toSend">object to serialize into XML and POST</param>
        /// <returns>an object deserialized from the XML returned</returns>
        public ResponseType InvokePut( string methodName, object toSend )
        {
            string url = string.Concat( hostname, methodName );
            ResponseType output = default( ResponseType );

            if ( toSend == null )
                throw new FetchException( "Nothing to send!" );

            try
            {
                WebRequest request = WebRequest.Create( url );
                request.Method = "POST";
                request.Headers["Authorization"] = string.Format( "Basic {0}", Config.GetAuthorization() );
                request.ContentType = "application/xml; charset=utf-8";

                string xml = Utility.SerializeToXml( toSend );
                // this takes out a certain strange character that gets inserted
                xml = xml.Replace( char.ConvertFromUtf32( 65279 ), string.Empty );

                using ( StreamWriter writer = new StreamWriter( request.GetRequestStream() ) )
                {
                    writer.Write( xml );
                    writer.Flush();
                    writer.Close();
                }

                WebResponse response = request.GetResponse();

                if ( response.ContentType.Contains( "application/xml" ) )
                {
                    output = Utility.DeserializeFromXmlStream<ResponseType>( response.GetResponseStream() );
                }
                else
                {
                    throw new NotImplementedException( string.Format( "Does not know how to deserialize this content: {0}", response.ContentType ) );
                }

            }
            catch ( WebException webex )
            {
                ThrowWebException( webex );
            }

            return output;
        }

        /// <summary>
        /// initiates a POST request to the WS to the given method and optional parameters and 
        /// POSTs the given object serialized as XML
        /// </summary>
        /// <example>http://restful.webservice.com/&lt;methodName&gt;/&lt;arg0&gt;/&lt;arg1&gt;</example>
        /// <param name="methodName">name of the method to execute</param>
        /// <param name="toSend">object to serialize into XML and POST</param>
        /// <param name="arguments">list of additional arguments to add the the URL</param>
        /// <returns>an object deserialized from the XML returned</returns>
        public ResponseType InvokePut( string methodName, object toSend, params string[] arguments )
        {
            StringBuilder output = new StringBuilder( methodName );
            foreach ( string arg in arguments )
            {
                output.AppendFormat( "/{0}", arg );
            }
            return InvokePut( output.ToString(), toSend );
        }

        /// <summary>
        /// Reads any error messages thrown from the connection, and rethrows the exception w/ 
        /// the additional information
        /// </summary>
        private void ThrowWebException( WebException webex )
        {
            Message response;
            try
            {
                response = Utility.DeserializeFromXmlStream<Message>( webex.Response.GetResponseStream() );
            }
            catch
            {
                string message = string.Empty;
                Stream stream = webex.Response.GetResponseStream();
                stream.Position = 0;
                using ( StreamReader reader = new StreamReader( stream ) )
                {
                    message = reader.ReadToEnd();
                }
                throw new FetchException( string.Format( "Error connecting to host: {0}", message ), webex );
            }
            throw new FetchException( string.Format( "Error connecting to host: {0}", response.Text ), webex );
        }

    }
}
