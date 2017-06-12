using MongoDB.Bson;
using MongoDB.Driver;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    class resourceEditer
    {
        public bool handleRequest(Request request, String id)
        {
            string category = request.Form.category;//request.Query["category"];
            string filename = request.Form.filename;//request.Query["filename"];
            if (String.IsNullOrWhiteSpace(category) && String.IsNullOrWhiteSpace(filename)) return false;
            updateResource(id, filename, category);
            return true;
        }

        private void updateResource(string fileId, string filename, string category)
        {

            IMongoCollection<fileEntry> collection = DbaseMongo.Instance.db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection);
            FilterDefinition<fileEntry> filter = new BsonDocument("_id", ObjectId.Parse(fileId));
            UpdateDefinition<fileEntry> update = null;
            if (String.IsNullOrWhiteSpace(category)) update = Builders<fileEntry>.Update.Set(fileEntry.DBfileName, filename).CurrentDate("lastModified");
            else if (String.IsNullOrWhiteSpace(filename)) update = Builders<fileEntry>.Update.Set(fileEntry.DBcategory, category).CurrentDate("lastModified");
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("filename", filename);
                dict.Add("category", category);
                update = Builders<fileEntry>.Update.Set(fileEntry.DBfileName, filename).Set(fileEntry.DBcategory, category).CurrentDate("lastModified");
            }
            collection.FindOneAndUpdate<fileEntry>(filter, update);
        }
    }
}
