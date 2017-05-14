using Nancy;
using System.IO;
using System.Text;
using Nancy.ViewEngines.Razor;

namespace IO2P
{
    /// <summary>
    /// Klasa z routingiem nancy programu
    /// </summary>
    public class nancyTest : NancyModule
    {
        /// <summary>
        /// Konstruktor klasy z routingiem Nancy programu.
        /// </summary>
        public nancyTest()
        {
            Get["/"] = _ => View["front/index.cshtml"];
            Get["/index.html"] = _ => View["front/index.cshtml"];
            Get["/myfiles.html"] = _ => View["front/myfiles.cshtml"];
            Get["/listfiles/{fileType}"] = param =>
            {
                FileFilter filter = new FileFilter();
                return filter.filterFileCollection(param.fileType, this.Request);
                //var jsonBytes = Encoding.UTF8.GetBytes(json);
                //return new Response
                //{
                //    ContentType = "application/json",
                //    Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                //};
            };
            Get["/upload.html"] = _ => View["front/upload.cshtml"];
            Get["/newfile"] = _ => View["front/upload.cshtml"];
            Post["/getfile"] = _ =>
            {
                resourceViewer resView = new resourceViewer();
                byte[] file = resView.handleRequest(this.Request);
                string contentType = resView.getContentType();
                return Response.FromByteArray(file, contentType);
            };
            Get["/getfile"] = _ =>
            {
                resourceViewer resView = new resourceViewer();
                byte[] file = resView.handleRequest(this.Request);
                string contentType = resView.getContentType();
                return Response.FromByteArray(file, contentType);
            };
            Post["/removefile"] = _ =>
            {
               new resourceDeleter().handleRequest(this.Request);
               return true;
            };
            // Get["/removefile"] = _ =>
            // {
            //     new resourceDeleter().handleRequest(this.Request);
            //    return true;
            // };
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request);
            Post["/newfile"] = _ =>
            {
                try
                {
                    new resourceAdder().handlePost(this.Request);
                }
                catch(UnknownFileExtensionException)
                {
                    return "Ustalenie typu pliku okazało się niemożliwe";
                }
                catch(NotAnImageFileException)
                {
                    return "Załączaony plik nie jest obrazkiem";
                }
                catch(NotAVideoFileException)
                {
                    return "Załączony plik nie jest wideo";
                }
                catch (NotASoundFileException)
                {
                    return "Załączony plik nie jest dźwiękiem";
                }
                return View["front/myfiles.html"];
            };
        }

        public void showFiles()
        {

        }
    }
}
