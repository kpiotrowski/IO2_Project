using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace IO2P
{
    class resourceViewer
    {
        private String FTP_HOST = Environment.ExpandEnvironmentVariables("%FTP_HOST%");
        private String FTP_USER = Environment.ExpandEnvironmentVariables("%FTP_USER%");
        private String FTP_PASS = Environment.ExpandEnvironmentVariables("%FTP_PASS%");
        public byte[] handleRequest(Nancy.Request request)
        {
            String fileId = request.Form.fileId;
            //fileId = request.Query["fileId"];
            String fileLocation = findResourceLocation(fileId);
            byte[] file = downloadResource(fileLocation);
            return file;
        }
        private byte[] downloadResource(string fileLocation)
        {
            long size;

            FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(fileLocation));
            ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpReq.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
            ftpReq.UseBinary = true;

            FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse();
            size = response.ContentLength;
            long buffsize = 4096;
            Stream resStream = response.GetResponseStream();
            byte[] buffer = new byte[buffsize];
            MemoryStream mStream = new MemoryStream();
            int readed = 0;
            readed = resStream.Read(buffer, 0, buffer.Length);
            while(readed > 0)
            {
                mStream.Write(buffer, 0, readed);
                readed = resStream.Read(buffer, 0, buffer.Length);
            }
            resStream.Close();
                        
            return mStream.ToArray();
        }

        private string findResourceLocation(string fileId)
        {
            try
            {
                IMongoCollection<fileEntry> collection = DbaseMongo.Instance.db.GetCollection<fileEntry>("fileEntries");
                FilterDefinition<fileEntry> filter = new BsonDocument("_id", ObjectId.Parse(fileId));
                IAsyncCursor<fileEntry> find = collection.FindSync<fileEntry>(filter);
                fileEntry Entry = null;
                if((Entry = find.First<fileEntry>()) != null)
                {
                    find.Dispose();
                    return Entry.localization;
                }
                find.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
