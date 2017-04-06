using System;
using MongoDB.Driver;
using System.Net;
using System.IO;

namespace IO2P
{
    class resourceAdder
    {
        /// <summary>
        /// Zapisuje na dysku zdalnym obraz/wideo nadesłany przez użytkownika i dodaje go do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <param name="category">Kategoria pliku</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(String filename, String category)
        {
            if (!saveResource(filename, Environment.ExpandEnvironmentVariables("disk"), Environment.ExpandEnvironmentVariables("diskUser"), Environment.ExpandEnvironmentVariables("diskPass"))) return false; 
            if (!addDatabaseEntry(filename, Environment.ExpandEnvironmentVariables("disk"),category))
            {
                if (!removeResource(filename, Environment.ExpandEnvironmentVariables("disk"), Environment.ExpandEnvironmentVariables("diskUser"), Environment.ExpandEnvironmentVariables("diskPass")))
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
            var credential = MongoCredential.CreateMongoCRCredential(Environment.ExpandEnvironmentVariables("DB_NAME"), Environment.ExpandEnvironmentVariables("DB_USER"), Environment.ExpandEnvironmentVariables("DB_PASS"));
            var settings = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(Environment.ExpandEnvironmentVariables("DB_HOST"), Int32.Parse(Environment.ExpandEnvironmentVariables("DB_PORT")))
            };
            var client = new MongoClient(settings);
            var db = client.GetDatabase(Environment.ExpandEnvironmentVariables("DB_NAME"));
            db.GetCollection<fileEntry>("fileEntries").InsertOne(new fileEntry(filename, diskname, category));
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
            if (diskname.Equals("local"))
            {
                File.Delete(filename);
            }
            else return true; //call to resourceRemover (not in this sprint)
            return true;
        }

        /// <summary>
        /// Zarządza obsługą POST'a do /newfile
        /// </summary>
        /// <param name="request">Zawartość post</param>
        public bool handlePost(Nancy.Request request)
        {
            var form = request.Form;
            //byte[] buffer = new byte[request.Body.Length];
            //request.Body.Read(buffer, 0, buffer.Length);
            //String body = Encoding.Default.GetString(buffer);
            //String[] reqParams = body.Split('&');
            String filename = form.filename;//reqParams[0].Split('=')[1];
            String category = form.category;//reqParams[1].Split('=')[1];
            String data = form.datas;// reqParams[2].Split('=')[1];
            byte[] datas = Convert.FromBase64String(data);
            downloadResource(filename, datas);
            addResource(filename, category);
            removeResource(filename, "local", "", "");
            return true;
        }
    }
}
