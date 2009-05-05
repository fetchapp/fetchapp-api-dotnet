using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Fetch.Api;
using NUnit.Framework;

namespace Fetch.Tests
{
    [TestFixture]
    public class ItemTests
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Config.Key = "pixallent";
            Config.Token = "pixallent";
            Config.Domain = "pixallent.myhost.dev:3000";
        }

        [Test]
        public void CreateRetrieveUpdateDelete()
        {
            string sku = DateTime.Now.ToString( "MMddHHmm" );
            string name = string.Format( "test item - {0}", DateTime.Now.ToString( "MMdd HHmm" ) );

            // create test
            Item item = new Item();
            item.Sku = sku;
            item.Name = name;
            item.Save();

            // retrieve test
            item = Item.Retrieve( sku );
            Assert.AreEqual( name, item.Name );

            // update test
            item.Name += " update test";
            item.Save();

            // delete test
            item.Delete();

        }

        [Test]
        public void RetrieveAllItems()
        {
            ItemCollection items = Item.RetrieveAll();
            Assert.IsNotNull( items );
        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void RetrieveInvalidItem()
        {
            Item item = Item.Retrieve( "SomeCrazySkuThatWillNeverExistHopefully" );
        }

        [Test]
        [ExpectedException( typeof( FetchException ) )]
        public void CreateInvalidItem()
        {
            Item item = new Item();
            item.Name = "Bad Item";
            item.Save();
        }


    }
}
