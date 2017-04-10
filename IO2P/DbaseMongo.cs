using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IO2P
{
    class DbaseMongo
    {
        private static DbaseMongo instance;
        private IMongoDatabase database;
        private IMongoClient client;

        private static string user = Environment.ExpandEnvironmentVariables("%DB_USER%");
        private static string password = Environment.ExpandEnvironmentVariables("%DB_PASS%");
        private static string host = Environment.ExpandEnvironmentVariables("%DB_HOST%");
        private static string port = Environment.ExpandEnvironmentVariables("%DB_PORT%");
        private static string databaseName = Environment.ExpandEnvironmentVariables("%DB_NAME%");

        private DbaseMongo()
        {
            string databaseURL = "mongodb://" + user + ":" + password + "@" + host + ":" + port + "/" + databaseName;
            this.client = new MongoClient(databaseURL);
            this.database = this.client.GetDatabase(databaseName);
        }

        public static DbaseMongo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbaseMongo();
                }
                return instance;
            }
        }

        public static void initDataBase(string userName, string pass, string hostName, string portNumber, string db)
        {
            user = userName;
            password = pass;
            host = hostName;
            port = portNumber;
            databaseName = db;
        }

        public IMongoDatabase db
        {
            get
            {
                return this.database;
            }
        }
        public IMongoClient Client
        {
            get
            {
                return this.client;
            }
        }

        public void showCollection<T>(IMongoCollection<T> collection)
        {
            var list = collection.Find(FilterDefinition<T>.Empty).ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item.ToJson());
            }
        }

    }
}
