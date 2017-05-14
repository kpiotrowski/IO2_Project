using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;

namespace IO2P
{
    class FileFilter
    {
        public string filterFileCollection(String fileType,Request request)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq(fileEntry.DBfileType,fileType);

            string name = request.Query[fileEntry.DBfileName];
            if (!String.IsNullOrEmpty(name))
            {
                filter &= builder.Regex(fileEntry.DBfileName, new BsonRegularExpression("/.*" + name + ".*/i"));
            }
            string category = request.Query[fileEntry.DBcategory];
            if (!String.IsNullOrEmpty(category))
            {
                filter &= builder.Regex(fileEntry.DBcategory, new BsonRegularExpression("/.*" + category + ".*/i"));
            }
            string extension = request.Query[fileEntry.DBfileExtenstion];
            if (!String.IsNullOrEmpty(extension))
            {
                filter &= builder.Regex(fileEntry.DBfileExtenstion, new BsonRegularExpression("/^" + extension + "$/i"));
            }

            List<BsonDocument> list = new List<BsonDocument>();
            DbaseMongo.Instance.getCollection(list, DbaseMongo.DefaultCollection, filter);

            //Console.WriteLine("fileType: " + fileType + " name: " + name + " category: " + category + " extension " + extension);
            //Console.WriteLine(list.ToJson());
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            return list.ToJson(jsonWriterSettings);
        }
    }
}
