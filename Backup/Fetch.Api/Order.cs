using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Fetch.Api
{
    /// <summary>
    /// Order is a model object that mirrors the type used by the Fetch REST API, and contains
    /// all the properties and methods offered by the API.
    /// </summary>
    [XmlType( "order" )]
    public class Order
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Order()
        {
            this.IsNew = true;
            this.orderItems = new SerializableList<OrderItem>();
        }

        /// <summary>
        /// Gets or Sets whether or not to send an email when this order is created
        /// </summary>
        [XmlElement( "send_email" )]
        public bool SendEmailFlag
        {
            get { return sendEmail; }
            set { sendEmail = value; }
        }
        private bool sendEmail;

        /// <summary>
        /// unique ID of this order
        /// </summary>
        [XmlElement( "id" )]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string id;

        /// <summary>
        /// textual description of the order
        /// </summary>
        [XmlElement( "title" )]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string title;

        /// <summary>
        /// first name of the customer
        /// </summary>
        [XmlElement( "first_name" )]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        private string firstName;

        /// <summary>
        /// last name of the customer
        /// </summary>
        [XmlElement( "last_name" )]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string lastName;

        /// <summary>
        /// email address of the customer
        /// </summary>
        [XmlElement( "email" )]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string email;

        /// <summary>
        /// date that this order will expire, and no longer be available for download
        /// </summary>
        [XmlElement( "expiration_date" )]
        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
        private DateTime expirationDate;

        /// <summary>
        /// number of times items in this order have been downloaded
        /// </summary>
        [XmlElement( "download_count" )]
        public int DownloadCount
        {
            get { return downloadCount; }
            set { downloadCount = value; }
        }
        private int downloadCount;

        /// <summary>
        /// date that this order was created
        /// </summary>
        [XmlElement( "created_at" )]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        private DateTime createdAt;

        /// <summary>
        /// items that are included in this order
        /// </summary>
        [XmlElement( "order_items" )]
        public SerializableList<OrderItem> OrderItems
        {
            get
            {
                // make sure this attribute is always on it,
                // b/c it does not come back when deserialized
                orderItems.Attributes["type"] = "array";
                return orderItems;
            }
            set {  orderItems = value; }
        }
        private SerializableList<OrderItem> orderItems;

        /// <summary>
        /// Gets true if this order has not been committed to Fetch.  Gets false if this
        /// order already exists in Fetch.
        /// </summary>
        [XmlIgnore]
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }
        private bool isNew;

        #region Instance Methods

        /// <summary>
        /// Commits this order data to Fetch.  Order is created if new,
        /// order is updated if existing.
        /// </summary>
        public void Save()
        {
            if ( IsNew )
            {
                Create( this );
            }
            else
            {
                Update( this );
            }
        }

        /// <summary>
        /// Removes this order from Fetch.
        /// </summary>
        public void Delete()
        {
            Delete( this.Id );
        }

        /// <summary>
        /// Sets this order to expired, so it may no longer be downloaded.
        /// </summary>
        public void Expire()
        {
            Expire( this.Id );
        }

        /// <summary>
        /// Forces a send of an email to the customer of this order
        /// </summary>
        public void SendEmail()
        {
            SendEmail( this.Id );
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Retrieve all orders available from Fetch
        /// </summary>
        /// <returns></returns>
        public static OrderCollection RetrieveAll()
        {
            RestConnection<OrderCollection> conn = new RestConnection<OrderCollection>();
            OrderCollection orders = conn.InvokeGet( "orders" );
            orders.ForEach( delegate( Order o ) { o.isNew = false; } );
            return orders;
        }

        /// <summary>
        /// Retrieve a specific order from Fetch
        /// </summary>
        /// <param name="id">unique ID of the order to retrieve</param>
        public static Order Retrieve( string id )
        {
            RestConnection<Order> conn = new RestConnection<Order>();
            Order order = conn.InvokeGet( "orders", id );
            order.IsNew = false;
            return order;
        }

        /// <summary>
        /// Deletes the given order from Fetch
        /// </summary>
        /// <param name="id">unique ID of the order to delete</param>
        public static void Delete( string id )
        {
            RestConnection<Message> conn = new RestConnection<Message>();
            Message response = conn.InvokeGet( "orders", id, "delete" );
            if ( !response.Text.Equals( Message.OkResponse ) )
            {
                throw new FetchException( string.Format( "Delete operation not successful: {0}", response.Text ) );
            }
        }

        /// <summary>
        /// Expires the given order in Fetch, so it may no longer be downloaded.
        /// </summary>
        /// <param name="id">unique ID of the order</param>
        public static void Expire( string id )
        {
            RestConnection<Message> conn = new RestConnection<Message>();
            Message response = conn.InvokeGet( "orders", id, "expire" );
            if ( !response.Text.Equals( Message.OkResponse ) )
            {
                throw new FetchException( string.Format( "Expire operation not successful: {0}", response.Text ) );
            }
        }

        /// <summary>
        /// Forces an email to be sent to the customer
        /// </summary>
        /// <param name="id">unique ID of the order</param>
        public static void SendEmail( string id )
        {
            RestConnection<Message> conn = new RestConnection<Message>();
            Message response = conn.InvokeGet( "orders", id, "send_email" );
            if ( !response.Text.Equals( Message.OkResponse ) )
            {
                throw new FetchException( string.Format( "SendEmail operation not successful: {0}", response.Text ) );
            }
        }

        /// <summary>
        /// Creates an order in Fetch
        /// </summary>
        /// <param name="order">the order data to create</param>
        /// <returns>the actual Order object created by Fetch</returns>
        internal static Order Create( Order order )
        {
            RestConnection<Order> conn = new RestConnection<Order>();
            Order response = conn.InvokePut( "orders", order, "create" );
            return response;
        }

        /// <summary>
        /// Updates an order in Fetch
        /// </summary>
        /// <param name="order">the order data to update</param>
        /// <returns>the actual Order object that was updated in Fetch</returns>
        internal static Order Update( Order order )
        {
            RestConnection<Order> conn = new RestConnection<Order>();
            Order response = conn.InvokePut( "orders", order, order.Id, "update" );
            return response;
        }

        #endregion

    }

    /// <summary>
    /// Represents a collection of orders that serializes to match the XML 
    /// returned by Fetch
    /// </summary>
    [Serializable]
    [XmlType( "orders" )]
    public class OrderCollection : List<Order>
    {
    }

    /// <summary>
    /// object that is used to describe items included in an order
    /// </summary>
    [Serializable]
    [XmlType( "order_item" )]
    public class OrderItem
    {
        /// <summary>
        /// unique ID of an item in an order
        /// </summary>
        [XmlElement( "sku" )]
        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }
        private string sku;

    }

}
