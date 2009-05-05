using System;
using System.Xml.Serialization;

namespace Fetch.Api
{
    /// <summary>
    /// Represents messages returned from Fetch REST GET commands that contain results
    /// of the opertaion.
    /// </summary>
    [Serializable]
    [XmlType( "message" )]
    public class Message
    {

        /// <summary>
        /// The body of the message.
        /// </summary>
        [XmlText]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private string text;

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns>text of the message</returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// constant representing a successful response
        /// </summary>
        public static readonly string OkResponse = "Ok.";

    }
}
