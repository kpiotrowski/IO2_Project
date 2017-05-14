using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;


namespace IO2P
{
    class FileFilter
    {
        public string filterFileCollection(String fileType,Request request)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq(fileEntry.DBfileType,fileType);

            string name = request.Query["name"];
            if (!name.Equals(""))
            {
                filter &= builder.Regex(fileEntry.DBfileName, new BsonRegularExpression("/%" + name + "%/i"));
            }
            string category = request.Query["category"];
            if (!category.Equals(""))
            {
                filter &= builder.Regex(fileEntry.DBcategory, new BsonRegularExpression("/%" + category + "%/i"));
            }
            string extension = request.Query["extension"];
            if (!extension.Equals(""))
            {
                filter &= builder.Regex(fileEntry.DBfileExtenstion, new BsonRegularExpression("/^" + extension + "$/i"));
            }

            List<BsonDocument> list = new List<BsonDocument>();
            DbaseMongo.Instance.getCollection(list, DbaseMongo.DefaultCollection,filter);
            return list.ToJson();
        }
    }
}
