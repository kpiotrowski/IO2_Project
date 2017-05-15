using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Moq;

namespace IO2P.test
{
    [TestFixture]
    public class ResourceViewerTest
    {
        private resourceViewer res;
        private Mock<IMongoCollection<fileEntry>> dbCollection;
        private Mock<IMongoDatabase> dbMock;
        private Mock<IAsyncCursor<fileEntry>> dbCursor;
        private Mock<FilterDefinition<fileEntry>> dbFilter;
        

        [SetUp]
        public void setUpTest()
        {
            this.res = new resourceViewer();
            this.dbMock = new Mock<IMongoDatabase>();
            this.dbCollection = new Mock<IMongoCollection<fileEntry>>();
            this.dbCursor = new Mock<IAsyncCursor<fileEntry>>();
            this.dbFilter = new Mock<FilterDefinition<fileEntry>>();
        } 

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void findResourceLocationEmptyFileId(string fileId)
        {
            Assert.AreEqual(null, this.res.findResourceLocation(fileId));
        }

        [Test]
        public void findReosurceTestOne()
        {
            //It.IsAny<IAggregateFluent<fileEntry>>()
            dbCursor.Setup(cr => cr.First<fileEntry>(It.IsAny<CancellationToken>())).Returns(new fileEntry("test.jpg", "testy", "testy", "image"));
            dbCollection.Setup(col => col.FindSync<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), It.IsAny<FindOptions<fileEntry, fileEntry>>(), It.IsAny<CancellationToken>())).Returns(dbCursor.Object));
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            //Assert.That();
        }
    }
}
