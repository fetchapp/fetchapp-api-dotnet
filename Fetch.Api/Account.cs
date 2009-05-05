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
    [XmlType("account")]
    public class Account
    {

        /// <summary>
        /// unique identifier for this account (currently the subdomain)
        /// </summary>
        [XmlElement("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string id;

        /// <summary>
        /// Account name
        /// </summary>
        [XmlElement("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string name;

        /// <summary>
        /// Account email address
        /// </summary>
        [XmlElement("email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string email;

        /// <summary>
        /// Billing email address
        /// </summary>
        [XmlElement("billing_email")]
        public string BillingEmail
        {
            get { return billingEmail; }
            set { billingEmail = value; }
        }
        private string billingEmail;

        /// <summary>
        /// Shopify account address (optional)
        /// </summary>
        [XmlElement("shopify_address")]
        public string ShopifyAddress
        {
            get { return shopifyAddress; }
            set { shopifyAddress = value; }
        }
        private string shopifyAddress;

        /// <summary>
        /// Account URL
        /// </summary>
        [XmlElement("url")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string url;

        /// <summary>
        /// Paypal Address
        /// </summary>
        [XmlElement("paypal_address")]
        public string PaypalAddress
        {
            get { return paypalAddress; }
            set { paypalAddress = value; }
        }
        private string paypalAddress;

        /// <summary>
        /// Number of hours an Item is available for download after an order is placed
        /// </summary>
        [XmlElement("order_expiration_in_hours")]
        public int OrderExpirationInHours
        {
            get { return orderExpiration; }
            set { orderExpiration = value; }
        }
        private int orderExpiration;

        /// <summary>
        /// Number of times an Item can be downloaded for an order
        /// </summary>
        [XmlElement("download_limit_per_item")]
        public int DownloadLimitPerItem
        {
            get { return download_limit; }
            set { download_limit = value; }
        }
        private int download_limit;

        /// <summary>
        /// API key
        /// </summary>
        [XmlElement("api_key")]
        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }
        private string apiKey;

        /// <summary>
        /// API Token
        /// </summary>
        [XmlElement("api_token")]
        public string ApiToken
        {
            get { return apiToken; }
            set { apiToken = value; }
        }
        private string apiToken;

        #region Static Methods

        /// <summary>
        /// Retrieves the account details
        /// </summary>
        public static Account Details()
        {
            RestConnection<Account> conn = new RestConnection<Account>();
            Account account = conn.InvokeGet("account"); 
            return account;
        }

        /// <summary>
        /// Generates a new API Token
        /// </summary>
        public static string NewToken()
        {
            RestConnection<string> conn = new RestConnection<string>();
            string token = conn.InvokeGet("new_token");
            return token;
        }

        #endregion

    }

}
