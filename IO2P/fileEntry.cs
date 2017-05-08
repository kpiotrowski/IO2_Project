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
        [BsonElement("fn")]
        public string filename { get; set; }
        [BsonElement("ft")]
        public string filetype { get; set; }
        [BsonElement("loc")]
        public string localization { get; set; }
        [BsonElement("date")]
        public string addDate { get; set; }
        //public DateTime addDate { get; set; }
        [BsonElement("cat")]
        public string category { get; set;  }

        public fileEntry(string filenameG, string diskname, string cName)
        {
            filename = filenameG.Split('.')[0];
            filetype = filenameG.Split('.')[1];
            localization = diskname + "/" + filenameG;
            addDate = DateTime.UtcNow.ToString();
            category = cName;
        }

    }
}
