using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace IO2P
{
    class resourceAdder
    {
        private String strDatabase = "";
        private String strDBUser = "";
        private String strDBPass = "";
        /// <summary>
        /// Zapisuje na dysku zdalnym obraz/wideo nadesłany przez użytkownika i dodaje go do bazy danych.
        /// </summary>
        /// <param name="resource">Obraz/wideo nadesłany przez użytkownika</param>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(Object resource, String filename)
        {
            String defaultDisk = "";
            if (!saveResource(resource, filename, defaultDisk)) return false; //Zastąp blokiem try/catch i zestawem wyjątków (brak połączenia, plik o zadanej nazwie już istnieje)
            if (!addDatabaseEntry(filename, defaultDisk))  //Zastąp blokiem try/catch i zestawem wyjątków (brak połączenia, błąd bazy(?))
            {
                while (!removeResource(filename, defaultDisk)) ;  //Zastąp blokiem try/catch i zestawem wyjątków z odpowiedimi reakcjami (brak połączenia, plik nie istnieje)
                return false;
            }
            return true;
        }

        /// <summary>
        /// Zapisuje na dysku zadany obraz/wideo.
        /// </summary>
        /// <param name="resource">Obraz/wideo nadesłany do zapisu</param>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma być zapisany</param>
        /// <param name="diskname">Dysk na którym obraz/wideo ma być zapisany</param>
        /// <returns>Informacja o sukcesie/porażce zapisu</returns>
        public bool saveResource(Object resource, String filename, String diskname)
        {
            return false;
        }

        /// <summary>
        /// Dodaje wpis informujący o lokalizacji obrazu/wideo do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo został zapisany</param>
        /// <param name="diskname">Dysk na którym obraz/wideo został zapisany</param>
        /// <returns>Informacja o sukceie/porażce dodawania do bazy danych</returns>
        public bool addDatabaseEntry(String filename, String diskname)
        {
            var credential = MongoCredential.CreateCredential(strDatabase, strDBUser, strDBPass);
            var settings = new MongoClientSettings {ReplicaSetName = "rs0" };
            var client = new MongoClient(settings);
            var db = client.GetDatabase(strDatabase);

            return false;
        }

        /// <summary>
        /// Usuwa plik z dysku
        /// </summary>
        /// <param name="filename">Nazwa pliku do usunięcia</param>
        /// <param name="diskname">Dysk na którym plik został zapisany</param>
        /// <returns>Informacja o sukcesie porażce usuwania</returns>
        public bool removeResource(String filename, String diskname)
        {
            return true;
        }
    }
}
