﻿using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace IO2P
{
    /// <summary>
    /// Klasa zapisuje plik na zdalnym dysku i dodaje wpis z lokalizacją do bazy danych.
    /// </summary>
    class resourceAdder
    {

        public String FTP_HOST { get; set; } = Environment.ExpandEnvironmentVariables("%FTP_HOST%");
        public String FTP_USER { get; set; } = Environment.ExpandEnvironmentVariables("%FTP_USER%");
        public String FTP_PASS { get; set; } = Environment.ExpandEnvironmentVariables("%FTP_PASS%");
        List<String> imageExtensionsList = new List<String> { "jpg", "png", "bmp", "gif", "svg", "jpe", "jpeg", "tiff" };
        List<String> videoExtensionsList = new List<String> { "aec", "bik", "m4e", "m75", "m4v", "mp4", "mp4v", "ogv" };
        List<String> soundExtensionsList = new List<String> { "mp3", "ogg", "3ga", "aac", "flac", "midi", "wav", "wma" };

        /// <summary>
        /// Zapisuje na dysku zdalnym obraz/wideo nadesłany przez użytkownika i dodaje go do bazy danych.
        /// </summary>
        /// <param name="filename">Nazwa pod jaką obraz/wideo ma zostać zapisany</param>
        /// <param name="category">Kategoria pliku</param>
        /// <param name="fileType">Typ pliku</param>
        /// <returns>Informacja czy zapis przebiegł pomyślnie</returns>
        public bool addResource(String filename, String fileType, String category)
        {
            if (!saveResource(filename, FTP_HOST, FTP_USER, FTP_PASS)) return false; 
            if (!addDatabaseEntry(filename, FTP_HOST, fileType, category))
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
        /// <param name="fileType">Typ pliku</param>
        /// <returns>Informacja o sukceie/porażce dodawania do bazy danych</returns>
        public bool addDatabaseEntry(String filename, String diskname, String fileType, String category)
        {
            try
            {
                IMongoCollection<fileEntry> collection = DbaseMongo.Instance.db.GetCollection<fileEntry>(DbaseMongo.DefaultCollection);
                collection.InsertOne(new fileEntry(filename, diskname, category, fileType));
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
            else new resourceDeleter().removeResource(diskname + "/" + filename);
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
            String fileType = form.fileType;
            var data = files.GetEnumerator();
            data.MoveNext();
            Stream fileStream = data.Current.Value;
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            String[] nameparts = data.Current.Name.Split('.');
            if (nameparts.Length < 2) throw new UnknownFileExtensionException();
            String extension = nameparts[nameparts.GetLength(0) - 1].ToLowerInvariant();
            if (fileType.Equals("image"))
            {
                if (!imageExtensionsList.Contains(extension)) throw new NotAnImageFileException();
            }
            else if (fileType.Equals("video"))
            {
                if (!videoExtensionsList.Contains(extension)) throw new NotAVideoFileException();
            }
            else if (fileType.Equals("audio"))
            {
                if (!soundExtensionsList.Contains(extension)) throw new NotASoundFileException();
            }
            if (filename.Equals(null) || filename.Equals("")) filename = data.Current.Name;
            else filename = filename + "." + extension;
            byte[] datas = Encoding.ASCII.GetBytes(data.Current.Value.ToString());
            downloadResource(filename, buffer);
            addResource(filename, fileType, category);
            removeResource(filename, "local", "", "");
            return true;
        }
    }
}
