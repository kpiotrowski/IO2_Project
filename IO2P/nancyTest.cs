using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    class nancyTest : NancyModule
    {
        public nancyTest()
        {
            Get["/"] = _ => "Received GET request";
            Get["/test2"] = _ =>
            {
                return "Test2";
            };
            Get["/test"] = _ => "Test";
            Get["/ajax"] = _ => View["C:/Users/Tomek/Desktop/IA4/ajax.html"];
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request);
        }
    }
}
