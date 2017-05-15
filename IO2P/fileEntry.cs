using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nancy;
using System;

namespace IO2P
{
    /// <summary>
    /// Klasa, której obiekty reprezentują pojedyncze wpisy w bazie danych informujące o obrazach/wideo.
    /// </summary>
    [BsonIgnoreExtraElements]
    class fileEntry
    {
        public const string DBfileName = "filename";
        public const string DBfileExtenstion = "fileExtension";
        public const string DBdate = "date";
        public const string DBlocalization = "localization";
        public const string DBcategory = "category";
        public const string DBfileType = "fileType";
        public const string DBcontentType = "contentType";

        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement(DBfileName)]
        public string filename { get; set; }
        [BsonElement(DBfileExtenstion)]
        public string fileExtension { get; set; }
        [BsonElement(DBlocalization)]
        public string localization { get; set; }
        [BsonElement(DBdate)]
        public string addDate { get; set; }
        //public DateTime addDate { get; set; }
        [BsonElement(DBcategory)]
        public string category { get; set;  }
        [BsonElement(DBfileType)]
        public string fileType { get; set; }
        [BsonElement(DBcontentType)]
        public string contentType { get; set; }


        public fileEntry(string filenameG, string diskname, string cName, string fileType)
        {
           if(filenameG=="") {
             throw new Exception("Filename cannot be empty");
          }
           if(filenameG.Split('.').Length<2) {
             throw new Exception("Filename must have extension");
          }
            filename = filenameG.Split('.')[0];
            fileExtension = filenameG.Split('.')[1];
            if(diskname=="") {
               localization = filenameG;
            }
            else {
               localization = diskname + "/" + filenameG;
            }
            addDate = DateTime.UtcNow.ToString();
            category = cName;
            if(cName=="") {
               throw new Exception("Category cannot be empty");
            }
            if(!fileType.Equals("image") && !fileType.Equals("audio") && !fileType.Equals("video")){
               throw new Exception("Not supported File Type");
            }
            this.fileType = fileType;
            contentType = Nancy.MimeTypes.GetMimeType(filenameG);
            //contentType = fileType + "/" + fileExtension;
        }

    }
}
