using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    public class nancyTest : NancyModule
    {
        public nancyTest()
        {
            Get["/"] = _ => "Received GET request";
            Get["/test2"] = _ =>
            {
                return "Test2";
            };
            Get["/test"] = _ => "Test";
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request);
        }
    }
}
