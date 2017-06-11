﻿using MongoDB.Bson;
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
            UpdateDefinition<fileEntry> update;
            if (String.IsNullOrWhiteSpace(category)) update = new JsonUpdateDefinition<fileEntry>(new BsonDocument("filename", filename).ToString());
            else if (String.IsNullOrWhiteSpace(filename)) update = new JsonUpdateDefinition<fileEntry>(new BsonDocument("category", category).ToString());
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("filename", filename);
                dict.Add("category", category);
                update = new JsonUpdateDefinition<fileEntry>(new BsonDocument(dict).ToString());
            }
            collection.FindOneAndUpdate<fileEntry>(filter, update);
        }
    }
}