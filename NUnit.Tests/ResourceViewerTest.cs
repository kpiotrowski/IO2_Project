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

        [SetUp]
        public void setUpTest()
        {
            this.res = new resourceViewer();
        } 

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void findResourceLocationEmptyFileId(string fileId)
        {
            Assert.AreEqual(null, this.res.findResourceLocation(fileId));
        }
    }
}
