using System;

namespace IO2P
{
    /// <summary>
    /// Klasa, której obiekty reprezentują pojedyncze wpisy w bazie danych informujące o obrazach/wideo.
    /// </summary>
    class fileEntry
    {
        public String filename { get; set;  }
        public String filetype { get; set; }
        public String localization { get; set; }
        public DateTime addDate { get; set; }
        public String category { get; set;  }

        public fileEntry(String filename, String diskname, String cName)
        {
            filename = filename.Split('.')[0];
            filetype = filename.Split('.')[1];
            localization = diskname + "/" + filename;
            addDate = DateTime.UtcNow;
            category = cName;
        }

    }
}
