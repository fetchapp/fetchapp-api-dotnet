using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace Fetch.Api
{
    /// <summary>
    /// Static utility functions
    /// </summary>
    internal static class Utility
    {

        /// <summary>
        /// Serializes the given object into an XML string.  Object must have the
        /// SerializableAttribute applied.
        /// </summary>
        /// <param name="target">object to serialize</param>
        /// <returns>XML string</returns>
        public static string SerializeToXml( object target )
        {
            using ( MemoryStream memoryStream = new MemoryStream() )
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add( "", "" );

                XmlTextWriter xmlWriter = new XmlTextWriter( memoryStream, Encoding.UTF8 );

                XmlSerializer serializer = new XmlSerializer( target.GetType() );
                serializer.Serialize( xmlWriter, target, xmlns );

                return Encoding.UTF8.GetString( memoryStream.ToArray() );
            }
        }

        /// <summary>
        /// Deserializes an object of the generic parameter type from the given
        /// stream of XML
        /// </summary>
        /// <typeparam name="T">type of the object to deserialize</typeparam>
        /// <param name="stream">XML stream containing the object data</param>
        /// <returns>instance of deserialized type</returns>
        public static T DeserializeFromXmlStream<T>( Stream stream )
        {
            XmlSerializer serializer = new XmlSerializer( typeof( T ) );
            return (T)serializer.Deserialize( stream );
        }

        /// <summary>
        /// Deserializes an object of the generic parameter type from the given 
        /// XML string
        /// </summary>
        /// <typeparam name="T">type of the object to deserialize</typeparam>
        /// <param name="xml">string of XML</param>
        /// <returns>instance of deserialized type</returns>
        public static T DeserializeFromXml<T>( string xml )
        {
            using ( StringReader reader = new StringReader( xml ) )
            {
                XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                return (T)serializer.Deserialize( reader );
            }
        }

    }
}
