using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IO2P.test
{
    [TestFixture]
    public class resourceDeleterTest
    {
        private resourceDeleter res;
        [SetUp]
        public void setUpTest()
        {
            Mock<IMongoDatabase> dbMock = new Mock<IMongoDatabase>();
            Mock<IMongoCollection<fileEntry>> dbCollection = new Mock<IMongoCollection<fileEntry>>();
            fileEntry file = new fileEntry("plik.jpg", "folder", "cat", "audio");
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(),null,default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            this.res = new resourceDeleter();
        }

        [Test]
        public void findAndDeleteResourceLocation_Correct()
        {
            string loc = this.res.findAndDeleteResourceLocation("59185507cd9412107c226b99");
            StringAssert.AreEqualIgnoringCase("folder/plik.jpg" , loc);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void findAndDeleteResourceLocation_WhiteSpace_fileId(string fieldId)
        {
           Assert.AreEqual(null,this.res.findAndDeleteResourceLocation(fieldId));
           
        }
      
    }
}
