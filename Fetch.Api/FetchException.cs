using System;

namespace Fetch.Api
{
    /// <summary>
    /// Exceptions specific to the Fetch API
    /// </summary>
    public class FetchException : ApplicationException
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public FetchException() : base() { }

        /// <summary>
        /// constructor with a message to include
        /// </summary>
        public FetchException( string message ) : base( message ) { }

        /// <summary>
        /// constructor with a message and inner exception to include
        /// </summary>
        public FetchException( string message, Exception ex ) : base( message, ex ) { }

    }

}
