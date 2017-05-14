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
         fileEntry file = new fileEntry("plik.jpg", "folder", "image", "typ");
         Assert.Equals("plik",file.getFilename());
         Assert.Equals("jpg",file.getFileExtension());
         Assert.Equals("image", file.getCategory());
         Assert.Equals("typ", file.getFileType());
      }

      [Test]
      public void addFilenameWithoutExtension(){
         Assert.Throws<Exception>(() => new fileEntry("plik", "folder", "image", "image"));
      }

      [Test]
      public void emptyFilename(){
         Assert.Throws<Exception>(() => new fileEntry("", "folder", "image", "image"));
      }

      [Test]
      public void testSetLocalizationNoFolder(){
         fileEntry file = new fileEntry("plik.png", "", "image", "image");
         Assert.Equals("plik.png",file.getLocalization());
      }

      public void testSetLocalizationWithPath(){
         fileEntry file = new fileEntry("plik.png", "folder/folder", "image", "image");
         Assert.Equals("folder/folder/plik.png",file.getLocalization());
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
         Assert.Equals("image/png",file.getContentType());
      }

      [Test]
      public void testGetMimeTypeNotKnown(){
         fileEntry file = new fileEntry("plik.jakiesdziwneinieznanerozszerzenie", "folder", "image", "image");
         Assert.Equals("application/octet-stream",file.getContentType());
      }
    }
}
