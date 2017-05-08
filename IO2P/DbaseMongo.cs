using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IO2P
{
    /// <summary>
    /// Singleton zarządzający połączeniem z bazą danych
    /// </summary>
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

        /// <summary>
        /// Metoda tworząca instancje singletona
        /// </summary>
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

        /// <summary>
        /// Metoda zmieniająca bazę do połączenia
        /// </summary>
        /// <param name="userName">Nowa nazwa użytkownika</param>
        /// <param name="pass">Nowe hasło</param>
        /// <param name="hostName">Nowy adres hosta</param>
        /// <param name="portNumber">Nowy numer portu</param>
        /// <param name="db">Nowa nazwa bazy danych</param>
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
        /// <summary>
        /// Metoda dodaje kolekcje do juz instniejacej listy, mozliwe jest filtrowanie kolekcji
        /// </summary>
        /// <typeparam name="T">Typ w jakim ma zostac pobrana kolekcja</typeparam>
        /// <param name="list">Lista do ktorej zostanie dodana kolekcja</param>
        /// <param name="name">Nazwa kolekcji</param>
        /// <param name="filter">filtr, domyslnie null</param>
        public void getCollection<T>(List<T> list, string name, FilterDefinition<T> filter = null)
        {
            if (filter == null) filter = FilterDefinition<T>.Empty;
            var collection = this.db.GetCollection<T>(name);
            list.AddRange(collection.Find(filter).ToList());
        }
        /// <summary>
        /// Metoda zwracająca zadaną kolekcje z bazy
        /// </summary>
        /// <param name="name">Nazwa kolekcji</param>
        public void showCollection(string name)
        {
            List<BsonDocument> list = new List<BsonDocument>();
            this.getCollection(list,name);
            foreach (BsonDocument item in list)
            {
                Console.WriteLine(item.ToJson());
            }
        }
    }
}
