using Nancy;
using System.IO;

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
            Get["/"] = _ => View["front/index.html"];
            Get["/index.html"] = _ => View["front/index.html"];
            Get["/myfiles.html"] = _ => View["front/myfiles.html"];
            Get["/listfiles"] = _ => View["front/myfiles.html"];
            Get["/upload.html"] = _ => View["front/upload.html"];
            Get["/newfile"] = _ => View["front/upload.html"];
            // Get["/getfile"] = _ =>
            // {
            //     string contentType = this.Request.Query["contentType"];
            //     byte[] file = new resourceViewer().handleRequest(this.Request);
            //     return Response.FromByteArray(file, contentType);
            // };
            Post["/getfile"] = _ =>
            {
                string contentType = this.Request.Form.contentType;
                byte[] file = new resourceViewer().handleRequest(this.Request);
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
            Post["/upload.html"] = _ => new resourceAdder().handlePost(this.Request);
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
