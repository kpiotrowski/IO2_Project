using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace IO2P.test
{
    [TestFixture]
    public class ResourceAdderTest
    {
        private resourceAdder resourceAdder;
        private Mock<IMongoCollection<fileEntry>> dbCollection;
        private Mock<IMongoDatabase> dbMock;
        [SetUp]
        public void setUpTest()
        {
            this.dbMock = new Mock<IMongoDatabase>();
            this.dbCollection = new Mock<IMongoCollection<fileEntry>>();
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            this.resourceAdder = new resourceAdder();
            this.resourceAdder.FTP_HOST = "";
            this.resourceAdder.FTP_PASS = "";
            this.resourceAdder.FTP_USER = "";
        }

        [Test]
        public void addDatabaseEntry_Correct()
        {
            dbCollection.Setup(col => col.InsertOne(It.IsAny<fileEntry>(), null, default(System.Threading.CancellationToken)));
            DbaseMongo.Instance.db = dbMock.Object;
            this.resourceAdder.addDatabaseEntry("test", "folder", "image", "cat");
        }

        [Test]
        public void addDatabaseEntry_Error()
        {
            dbCollection.Setup(col => col.InsertOne(It.IsAny<fileEntry>(), null, default(System.Threading.CancellationToken))).Throws( new Exception());
            DbaseMongo.Instance.db = dbMock.Object;
            Assert.Throws<Exception>(() => this.resourceAdder.addDatabaseEntry("test", "folder", "image", "cat"));
        }
    }
}
