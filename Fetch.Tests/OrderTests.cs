using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Fetch.Api;
using NUnit.Framework;
using System.Diagnostics;

namespace Fetch.Tests
{
    [TestFixture]
    public class OrderTests
    {

        private string item1Sku = "00000001";
        private string item2Sku = "00000010";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            Config.Key = "pixallent";
            Config.Token = "pixallent";
            Config.Domain = "pixallent.myhost.dev:3000";
        }

        [Test]
        public void CreateRetrieveUpdateDelete()
        {
            Order order = CreateOrder();
            string orderId = order.Id;
            string title = order.Title;

            // create
            order.Save();

            // retrieve
            order = Order.Retrieve( orderId );
            Assert.AreEqual( title, order.Title );

            // update
            order.Title += " update test";
            order.Save();

            // delete
            order.Delete();

        }

        [Test]
        public void RetrieveAllOrders()
        {
            OrderCollection orders = Order.RetrieveAll();
            Assert.IsNotNull( orders );
        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void RetrieveInvalidOrder()
        {
            Order order = Order.Retrieve( "SomeCrazySkuThatWillNeverExistHopefully" );
        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void CreateInvalidOrder()
        {
            Order order = new Order();
            order.Title = "Blah";
            order.Save();
        }

        [Test]
        public void ExpireOrder()
        {
            Order order = CreateOrder();
            order.Save();

            string orderId = order.Id;

            // try expire
            order.Expire();

            order = Order.Retrieve( orderId );
            Assert.IsTrue( order.ExpirationDate < DateTime.Now );

            // remove the test order
            order.Delete();

        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void ExpireInvalidOrder()
        {
            Order.Expire( "SomeCrazySkuThatWillNeverExistHopefully" );
        }
        
        [Test]
        public void SendEmail()
        {
            Order order = CreateOrder();
            order.Save();

            // try email
            order.SendEmail();

            // remove the test order
            order.Delete();
        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void EmailInvalidOrder()
        {
            Order.SendEmail( "SomeCrazySkuThatWillNeverExistHopefully" );
        }

        private Order CreateOrder()
        {
            string orderId = DateTime.Now.ToString( "MMddHHmm" );
            string title = string.Format( "test order - {0}", DateTime.Now.ToString( "MMdd HHmm" ) );

            Order order = new Order();
            order.Id = orderId;
            order.Title = title;
            order.FirstName = "Unit";
            order.LastName = "Test";
            order.Email = "you@email.com";
            order.ExpirationDate = DateTime.Now.AddDays( 1.0 );
            order.SendEmailFlag = false;

            OrderItem i = new OrderItem();
            i.Sku = item1Sku;
            order.OrderItems.Add( i );

            i = new OrderItem();
            i.Sku = item2Sku;
            order.OrderItems.Add( i );
            return order;
        }



    }
}
