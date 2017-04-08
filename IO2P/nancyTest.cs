using Nancy;

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
            Post["/upload.html"] = _ => new resourceAdder().handlePost(this.Request);
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request);
            Post["/newfile"] = _ =>
            {
                new resourceAdder().handlePost(this.Request);
                return View["front/myfiles.html"];
            };
        }
    }
}
