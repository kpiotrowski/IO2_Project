using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Net;]

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
            String fileLocation = findResourceLocation(fileId);
            byte[] file = downloadResource(fileLocation);
            return file;
        }

        private byte[] downloadResource(string fileLocation)
        {
            FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(fileLocation));
            ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpReq.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
            ftpReq.UseBinary = true;

            Stream reqStream = ftpReq.GetRequestStream();
            byte[] buffer = new byte[reqStream.Length];
            reqStream.Read(buffer, 0, buffer.Length);
            reqStream.Close();
                        
            return buffer;
        }

        private string findResourceLocation(string fileId)
        {
            try
            {
                IMongoCollection<fileEntry> collection = DbaseMongo.Instance.db.GetCollection<fileEntry>("fileEntries");
                FilterDefinition<fileEntry> filter = new BsonDocument("Id", fileId);
                IAsyncCursor<fileEntry> find = collection.FindSync<fileEntry>(filter);
                fileEntry Entry = null;
                if((Entry = find.First<fileEntry>()) != null)
                {
                    return Entry.localization;
                }
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
