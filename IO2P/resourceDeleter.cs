using System;
using Nancy;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;

namespace IO2P
{
    class resourceDeleter
    {
        private String FTP_HOST = Environment.ExpandEnvironmentVariables("%FTP_HOST%");
        private String FTP_USER = Environment.ExpandEnvironmentVariables("%FTP_USER%");
        private String FTP_PASS = Environment.ExpandEnvironmentVariables("%FTP_PASS%");
        public bool handleRequest(Request request)
        {
            String fileId = request.Form.fileId;
            //fileId = request.Query["fileId"];
            String fileLocation = findAndDeleteResourceLocation(fileId);
            removeResource(fileLocation);
            return true;
        }

        public string findAndDeleteResourceLocation(string fileId)
        {
            try
            {
                IMongoCollection<fileEntry> collection = DbaseMongo.Instance.db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection);
                if(String.IsNullOrEmpty(fileId) || String.IsNullOrWhiteSpace(fileId))
                {
                    throw new Exception("fileId is empty");
                }
                FilterDefinition<fileEntry> filter = new BsonDocument("_id", ObjectId.Parse(fileId));
                fileEntry Entry = collection.FindOneAndDelete<fileEntry>(filter);
                return Entry.localization;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void removeResource(string fileLocation)
        {
            FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(fileLocation));
            ftpReq.Method = WebRequestMethods.Ftp.DeleteFile;
            ftpReq.Credentials = new NetworkCredential(FTP_USER, FTP_PASS);
            WebResponse response = ftpReq.GetResponse();
        }
    }
}