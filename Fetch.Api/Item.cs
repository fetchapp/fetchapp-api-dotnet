using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Fetch.Api
{
    /// <summary>
    /// Item is a model object that mirrors the type used by the Fetch REST API, and contains
    /// all the properties and methods offered by the API.
    /// </summary>
    [Serializable]
    [XmlType( "item" )]
    public class Item
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Item()
        {
            this.isNew = true;
        }

        /// <summary>
        /// unique identifier for this item
        /// </summary>
        [XmlElement( "sku" )]
        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }
        private string sku;

        /// <summary>
        /// friendly name of the item
        /// </summary>
        [XmlElement( "name" )]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string name;

        /// <summary>
        /// number of times this Item has been downloaded from Fetch
        /// </summary>
        [XmlElement( "download-count" )]
        public int DownloadCount
        {
            get { return downloadCount; }
            set { downloadCount = value; }
        }
        private int downloadCount;

        /// <summary>
        /// date this item was created
        /// </summary>
        [XmlElement( "created-at" )]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        private DateTime createdAt;

        /// <summary>
        /// The name of the file attached to this item
        /// </summary>
        [XmlElement( "filename" )]
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        private string filename;

        /// <summary>
        /// size in bytes of the file attached to this item
        /// </summary>
        [XmlElement( "size-bytes" )]
        public long SizeBytes
        {
            get { return size; }
            set { size = value; }
        }
        private long size;

        /// <summary>
        /// type of file attached to this item
        /// </summary>
        [XmlElement( "content-type" )]
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        private string contentType;

        /// <summary>
        /// Gets true if this item has not been committed to Fetch.  Gets false if this
        /// item already exists in Fetch.
        /// </summary>
        [XmlIgnore]
        public bool IsNew
        {
            get { return isNew; }
            private set { isNew = value; }
        }
        private bool isNew;

        #region Instance Methods

        /// <summary>
        /// Commits the current item to Fetch.  Item is created if new, 
        /// or updated if existing.
        /// </summary>
        public void Save()
        {
            if ( IsNew )
            {
                Item response = Create( this );
                this.IsNew = false;
            }
            else
            {
                Update( this );
            }
        }

        /// <summary>
        /// Deletes the current item from Fetch.
        /// </summary>
        public void Delete()
        {
            Delete( this.Sku );
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Retrieve all the existing items from Fetch
        /// </summary>
        public static ItemCollection RetrieveAll()
        {
            RestConnection<ItemCollection> conn = new RestConnection<ItemCollection>();
            ItemCollection items = conn.InvokeGet( "items" );
            items.ForEach( delegate( Item i ) { i.IsNew = false; } );
            return items;
        }

        /// <summary>
        /// Retrieve a specific item from Fetch
        /// </summary>
        /// <param name="sku">The SKU of the Item to retrieve</param>
        public static Item Retrieve( string sku )
        {
            RestConnection<Item> conn = new RestConnection<Item>();
            Item item = conn.InvokeGet( "items", sku );
            item.IsNew = false;
            return item;
        }

        /// <summary>
        /// Deletes an item from Fetch
        /// </summary>
        /// <param name="sku">the SKU of the Item to delete</param>
        public static void Delete( string sku )
        {
            RestConnection<Message> conn = new RestConnection<Message>();
            Message response = conn.InvokeGet( "items", sku, "delete" );
            if ( !response.Text.Equals( Message.OkResponse ) )
            {
                throw new FetchException( string.Format( "Delete operation not successful: {0}", response.Text ) );
            }
        }

        /// <summary>
        /// Creates a new item in Fetch
        /// </summary>
        /// <returns>the actual item object that was created by Fetch</returns>
        internal static Item Create( Item item )
        {
            RestConnection<Item> conn = new RestConnection<Item>();
            Item response = conn.InvokePut( "items", item, "create" );
            return response;
        }

        /// <summary>
        /// Updates the given item in Fetch
        /// </summary>
        /// <returns>the actual item object that was updated in Fetch</returns>
        internal static Item Update( Item item )
        {
            RestConnection<Item> conn = new RestConnection<Item>();
            Item response = conn.InvokePut( "items", item, item.Sku, "update" );
            return response;
        }

        #endregion

    }

    /// <summary>
    /// Represents a collection of items that serializes to match the XML 
    /// returned by Fetch
    /// </summary>
    [Serializable]
    [XmlRoot( "items" )]
    public class ItemCollection : List<Item>
    {
    }
}
