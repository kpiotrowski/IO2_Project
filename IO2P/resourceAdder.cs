using System;
using MongoDB.Driver;
using System.Net;
using System.IO;
using System.Text;

namespace IO2P
{
    /// <summary>
    /// Klasa zapisuje plik na zdalnym dysku i dodaje wpis z lokalizacją do bazy danych.
    /// </summary>
    class resourceAdder
    {
        private String DB_PORT = Environment.ExpandEnvironmentVariables("%DB_PORT%");
        private String DB_HOST = Environment.ExpandEnvironmentVariables("%DB_HOST%");
        private String DB_NAME = Environment.ExpandEnvironmentVariables("%DB_NAME%");
        private String DB_USER = Environment.ExpandEnvironmentVariables("%DB_USER%");
        private String DB_PASS = Environment.ExpandEnvironmentVariables("%DB_PORT%");
        private String FTP_HOST = Environment.ExpandEnvironmentVariables("%FTP_HOST%");
        private String FTP_USER = Environment.ExpandEnvironmentVariables("%FTP_USER%");
        private String FTP_PASS = Environment.ExpandEnvironmentVariables("%FTP_PASS%");
        /// <summary>
        /// Zapisuje na dysku zdalnym obraz/wideo nadesłany przez użytkownika i dodaje go do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <param name="category">Kategoria pliku</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(String filename, String category)
        {
            if (!saveResource(filename, FTP_HOST, FTP_USER, FTP_PASS)) return false; 
            if (!addDatabaseEntry(filename, FTP_HOST, category))
            {
                if (!removeResource(filename, FTP_HOST, FTP_USER, FTP_PASS))
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
        /// Zapisuje na dysku (zdalnym) zadany obraz/wideo.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma być zapisany</param>
        /// <param name="diskname">Dysk (host) na którym obraz/wideo ma być zapisany</param>
        /// <param name="username">Nazwa użytkownika do zalogowania</param>
        /// <param name="password">Hasło do zalogowania</param>
        /// <returns>Informacja o sukcesie/porażce zapisu</returns>
        public bool saveResource(String filename, String diskname, String username, String password)
        {
            return true;
            try
            {
                FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(diskname+ "/" + filename));
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
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Dodaje wpis informujący o lokalizacji obrazu/wideo do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo został zapisany</param>
        /// <param name="diskname">Dysk na którym obraz/wideo został zapisany</param>
        /// <param name="category">Kategoria pliku</param>
        /// <returns>Informacja o sukceie/porażce dodawania do bazy danych</returns>
        public bool addDatabaseEntry(String filename, String diskname, String category)
        {
            try
            {
                var url = "mongodb://" + DB_USER + ":" + DB_PASS + "@" + DB_HOST + ":" + DB_PORT + "/" + DB_NAME;
                var client = new MongoClient(url);
                var db = client.GetDatabase(DB_NAME);
                //var collection = db.GetCollection<MongoDB.Bson.BsonDocument>("fileEntries");
                var collection = db.GetCollection<fileEntry>("fileEntries");
                //String json = "{ fn : \"" +  filename.Split('.')[0] + "\", ft : \"" + filename.Split('.')[1] + "\", loc : \"" + diskname + "/" + filename + "\", cat : \"" + category + "\"}";
                collection.InsertOne(new fileEntry(filename, diskname, category));
                //MongoDB.Bson.BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<MongoDB.Bson.BsonDocument>(json);
                //collection.InsertOne(document);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Usuwa plik z dysku (zdalnego lub serwera)
        /// </summary>
        /// <param name="filename">Nazwa pliku do usunięcia</param>
        /// <param name="diskname">Dysk na którym plik został zapisany</param>
        /// <param name="username">Nazwa użytkownika do zalogowania</param>
        /// <param name="password">Hasło do zalogowania</param>
        /// <returns>Informacja o sukcesie porażce usuwania</returns>
        public bool removeResource(String filename, String diskname, String username, String password)
        {
            if (diskname.Equals("local"))
            {
                File.Delete(filename);
            }
            else return true; //call to resourceRemover (not in this sprint)
            return true;
        }

        /// <summary>
        /// Zarządza obsługą żądania POST dodającego nowy plik do bazy.
        /// </summary>
        /// <param name="request">Żądanie do obsłużenia</param>
        public bool handlePost(Nancy.Request request)
        {
            var form = request.Form;
            var files = request.Files;
            String filename = form.filename;
            String category = form.category;
            var data = files.GetEnumerator();
            data.MoveNext();
            var fileStream = data.Current.Value;
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            String[] nameparts = data.Current.Name.Split('.');
            if (filename.Equals(null) || filename.Equals("")) filename = data.Current.Name;
            else filename = filename + "." + nameparts[nameparts.GetLength(0) - 1];
            byte[] datas = Encoding.ASCII.GetBytes(data.Current.Value.ToString());
            downloadResource(filename, buffer);
            addResource(filename, category);
            removeResource(filename, "local", "", "");
            return true;
        }
    }
}
