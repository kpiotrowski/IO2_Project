using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Net;
using System.IO;

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
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(String filename)
        {
            String defaultDisk = "";
            if (!downloadResource(filename))
            if (!saveResource(filename, defaultDisk, "", "")) return false; 
            if (!addDatabaseEntry(filename, defaultDisk))
            {
                if (!removeResource(filename, defaultDisk, "", "")) ;
                {
                    //Zapisz do logu - nieusunięty plik na zdalnym dysku
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Pobiera obraz/wideo od użytkownika i zapisuje lokalnie
        /// </summary>
        /// <param name="filename">Nazwa, pod którą plik jest zapisywany</param>
        public bool downloadResource(string filename)
        {

            return false;
        }

        /// <summary>
        /// Zapisuje na dysku zadany obraz/wideo.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma być zapisany</param>
        /// <param name="diskname">Dysk (host) na którym obraz/wideo ma być zapisany</param>
        /// <param name="username">Nazwa użytkownika do zalogowania</param>
        /// <param name="password">Hasło do zalogowania</param>
        /// <returns>Informacja o sukcesie/porażce zapisu</returns>
        public bool saveResource(String filename, String diskname, String username, String password)
        {
            try
            {
                FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(diskname));
                ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
                ftpReq.Credentials = new NetworkCredential(username, password);
                ftpReq.UseBinary = true;

                FileStream stream = File.OpenRead(filename);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                Stream reqStream = ftpReq.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            //return false;
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
        /// <param name="username">Nazwa użytkownika do zalogowania</param>
        /// <param name="password">Hasło do zalogowania</param>
        /// <returns>Informacja o sukcesie porażce usuwania</returns>
        public bool removeResource(String filename, String diskname, String username, String password)
        {
            return true;
        }
    }
}
