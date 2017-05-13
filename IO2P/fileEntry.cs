using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace IO2P
{
    /// <summary>
    /// Klasa, której obiekty reprezentują pojedyncze wpisy w bazie danych informujące o obrazach/wideo.
    /// </summary>
    [BsonIgnoreExtraElements]
    class fileEntry
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("filename")]
        public string filename { get; set; }
        [BsonElement("fileExtension")]
        public string fileExtension { get; set; }
        [BsonElement("localization")]
        public string localization { get; set; }
        [BsonElement("date")]
        public string addDate { get; set; }
        //public DateTime addDate { get; set; }
        [BsonElement("category")]
        public string category { get; set;  }
        [BsonElement("fileType")]
        public string fileType { get; set; }
        [BsonElement("contentType")]
        public string contentType { get; set; }


        public fileEntry(string filenameG, string diskname, string cName, String fileType)
        {
            filename = filenameG.Split('.')[0];
            fileExtension = filenameG.Split('.')[1];
            localization = diskname + "/" + filenameG;
            addDate = DateTime.UtcNow.ToString();
            category = cName;
            this.fileType = fileType;
            contentType = MimeTypes.MimeTypeMap.GetMimeType(fileExtension);
            //contentType = fileType + "/" + fileExtension
        }

    }
}
