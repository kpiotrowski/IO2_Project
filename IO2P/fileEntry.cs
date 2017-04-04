using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    class fileEntry
    {
        public String filename { get; set;  }
        public String filetype { get; set; }
        public String localization { get; set; }
        public fileEntry(String filename, String diskname)
        {
            filename = filename.Split('.')[0];
            filetype = filename.Split('.')[1];
            localization = diskname + "/" + filename;
        }
        
    }
}
