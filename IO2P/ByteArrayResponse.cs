using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    /// <summary>
    /// 
    /// </summary>
    public class ByteArrayResponse : Response
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="contentType"></param>
        public ByteArrayResponse(byte[] file, string contentType = null)
        {
            this.ContentType = contentType ?? "application/octet-stream";

            this.Contents = stream =>
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(file);
                }
            };
        }
    }
}
