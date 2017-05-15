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
        private fileEntry file;
        private Mock<IMongoDatabase> dbMock = new Mock<IMongoDatabase>();
        private Mock<IMongoCollection<fileEntry>> dbCollection = new Mock<IMongoCollection<fileEntry>>();

        [SetUp]
        public void setUpTest()
        {
            this.res = new resourceDeleter();
        }

        [TestCase("59185507cd9412107c226b99", "plik.jpg", "folder", "cat", "audio")]
        public void findAndDeleteResourceLocation_JPG(string fieldId, string name, string loc, string cat, string type)
        {
            this.file = new fileEntry(name, loc, cat, type);
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), null, default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            string result = this.res.findAndDeleteResourceLocation(fieldId);
            StringAssert.AreEqualIgnoringCase(loc+"/"+name , result);
        }

        [TestCase("59185507cd9412107c226b99", "test.png", "localization", "newKat", "image")]
        public void findAndDeleteResourceLocation_PNG(string fieldId, string name, string loc, string cat, string type)
        {
            this.file = new fileEntry(name, loc, cat, type);
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), null, default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            string result = this.res.findAndDeleteResourceLocation(fieldId);
            StringAssert.AreEqualIgnoringCase(loc + "/" + name, result);
        }

        [TestCase("59185507cd9412107c226b99", "mongo.mp3", "testowanie/nowejLokalizacji", "Empty", "video")]
        public void findAndDeleteResourceLocation_MP3(string fieldId, string name, string loc, string cat, string type)
        {
            this.file = new fileEntry(name, loc, cat, type);
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), null, default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            string result = this.res.findAndDeleteResourceLocation(fieldId);
            StringAssert.AreEqualIgnoringCase(loc + "/" + name, result);
        }

        [TestCase("59185507cd9412107c226b99", "mock.avi", "folder", "superKategoria", "video")]
        public void findAndDeleteResourceLocation_AVI(string fieldId, string name, string loc, string cat, string type)
        {
            this.file = new fileEntry(name, loc, cat, type);
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), null, default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            string result = this.res.findAndDeleteResourceLocation(fieldId);
            StringAssert.AreEqualIgnoringCase(loc + "/" + name, result);
        }

        [TestCase("lkjlj", "typAudio.bmp", "testu", "fjdslkfjsdklfjsdlkfj", "image")]
        public void findAndDeleteResourceLocation_BMP(string fieldId, string name, string loc, string cat, string type)
        {
            this.file = new fileEntry(name, loc, cat, type);
            dbCollection.Setup(col => col.FindOneAndDelete<fileEntry>(It.IsAny<FilterDefinition<fileEntry>>(), null, default(System.Threading.CancellationToken))).Returns(file);
            dbMock.Setup(db => db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection, null)).Returns(dbCollection.Object);
            DbaseMongo.Instance.db = dbMock.Object;
            string result = this.res.findAndDeleteResourceLocation(fieldId);
            StringAssert.AreEqualIgnoringCase(null, result);
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
