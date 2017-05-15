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
    public class FileEntryTest
    {
      [Test]
      public void addFilenameWithExtensionCorrect(){
         fileEntry file = new fileEntry("plik.jpg", "folder", "cat", "audio");
            StringAssert.AreEqualIgnoringCase("plik",file.filename);
            StringAssert.AreEqualIgnoringCase("jpg",file.fileExtension);
            StringAssert.AreEqualIgnoringCase("cat", file.category);
            StringAssert.AreEqualIgnoringCase("audio", file.fileType);
      }

      [Test]
      public void addFilenameWithoutExtension(){
         Assert.Throws<Exception>(() => new fileEntry("plik", "folder", "cat", "image"));
      }

      [Test]
      public void emptyFilename(){
         Assert.Throws<Exception>(() => new fileEntry("", "folder", "cat", "image"));
      }

      [Test]
      public void testSetLocalizationNoFolder(){
         fileEntry file = new fileEntry("plik.png", "", "cat", "image");
            StringAssert.AreEqualIgnoringCase("plik.png",file.localization);
      }

      [Test]
      public void testSetLocalizationWithPath(){
         fileEntry file = new fileEntry("plik.png", "folder/folder", "cat", "image");
            StringAssert.AreEqualIgnoringCase("folder/folder/plik.png",file.localization);
      }

      [Test]
      public void testEmptyCategory(){
         Assert.Throws<Exception>(() => new fileEntry("plik.png", "folder", "", "image"));
      }

      [Test]
      public void testFileTypeEmpty(){
         Assert.Throws<Exception>(() => new fileEntry("plik.png", "folder", "image", ""));
      }

      [Test]
      public void testNotSupportedFileType(){
         Assert.Throws<Exception>(() => new fileEntry("plik.png", "folder", "obrazek", "jakisdziwnyplik"));
      }

      [Test]
      public void testGetMimeTypePng(){
         fileEntry file = new fileEntry("plik.png", "folder", "image", "image");
         StringAssert.AreEqualIgnoringCase("image/png",file.contentType);
      }

      [Test]
      public void testGetMimeTypeNotKnown(){
         fileEntry file = new fileEntry("plik.jakiesdziwneinieznanerozszerzenie", "folder", "image", "image");
         StringAssert.AreEqualIgnoringCase("application/octet-stream",file.contentType);
      }
    }
}
