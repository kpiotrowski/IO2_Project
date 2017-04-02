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
using System.Net.Sockets;
using System.Net.Http;
using System.Web;

namespace IO2P
{
    class resourceAdder
    {
        private String host = "146.185.128.224";
        private int port = 27017 ;
        /// <summary>
        /// Zapisuje na dysku zdalnym obraz/wideo nadesłany przez użytkownika i dodaje go do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(String filename)
        {
            String defaultDisk = "";
            if (!saveResource(filename, defaultDisk, "", "")) return false; 
            if (!addDatabaseEntry(filename, defaultDisk))
            {
                if (!removeResource(filename, defaultDisk, Environment.ExpandEnvironmentVariables("diskUser"), Environment.ExpandEnvironmentVariables("diskPass")))
                {
                    //Zapisz do logu - nieusunięty plik na zdalnym dysku
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Zapisuje lokalnie pobrany od użytkownika plik
        /// </summary>
        /// <param name="filename">Nazwa, pod którą plik jest zapisywany</param>
        /// <param name="data">Dane pliku</param>
        public bool downloadResource(string filename, byte[] data)
        {
            FileStream stream = File.Create(filename);
            stream.Close();
            stream = File.OpenWrite(filename);
            stream.Write(data, 0, data.Length);
            stream.Close();
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
            var credential = MongoCredential.CreateMongoCRCredential(Environment.ExpandEnvironmentVariables("DB_NAME"), Environment.ExpandEnvironmentVariables("DB_USER"), Environment.ExpandEnvironmentVariables("DB_PASS"));
            var settings = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(host, port)
            };
            var client = new MongoClient(settings);
            var db = client.GetDatabase(Environment.ExpandEnvironmentVariables("DB_NAME"));
            /*using (var stream = new StreamWriter(filename + ".json"))
            using (var writer = new MongoDB.Bson.IO.JsonWriter(stream))
            {
                writer.WriteStartDocument();
                writer.WriteName("Nazwa pliku");
                writer.WriteString(filename.Split('.')[0]);
                writer.WriteName("Typ pliku");
                writer.WriteString(filename.Split('.')[1]);
                writer.WriteName("Lokalizacja pliku");
                writer.WriteString(diskname + "/" + filename);
                writer.WriteEndDocument();
            }*/
           // db.CreateCollection("fileEntries");
            db.GetCollection<fileEntry>("fileEntries").InsertOne(new fileEntry(filename, diskname));
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

        /// <summary>
        /// Zarządza obsługą POST'a do /newfile
        /// </summary>
        /// <param name="request">Zawartość żądania</param>
        public bool handlePost(Nancy.Request request)
        {
            byte[] buffer = new byte[request.Body.Length];
            request.Body.Read(buffer, 0, buffer.Length);
            downloadResource("test", buffer);
            addResource("test");
            return false;
        }
    }
}
