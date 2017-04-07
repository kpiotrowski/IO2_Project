using Nancy;

namespace IO2P
{
    /// <summary>
    /// Routing nancy programu - klasa
    /// </summary>
    public class nancyTest : NancyModule
    {
        /// <summary>
        /// Routing nancy programu
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
