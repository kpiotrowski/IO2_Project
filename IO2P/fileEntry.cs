using MongoDB.Bson.Serialization.Attributes;
using System;

namespace IO2P
{
    /// <summary>
    /// Klasa, której obiekty reprezentują pojedyncze wpisy w bazie danych informujące o obrazach/wideo.
    /// </summary>
    class fileEntry
    {
        [BsonElement("fn")]
        public String filename { get; set;  }
        [BsonElement("ft")]
        public String filetype { get; set; }
        [BsonElement("loc")]
        public String localization { get; set; }
        [BsonElement("date")]
        public DateTime addDate { get; set; }
        [BsonElement("cat")]
        public String category { get; set;  }

        public fileEntry(String filenameG, String diskname, String cName)
        {
            filename = filenameG.Split('.')[0];
            filetype = filenameG.Split('.')[1];
            localization = diskname + "/" + filenameG;
            addDate = DateTime.UtcNow;
            category = cName;
        }

    }
}
