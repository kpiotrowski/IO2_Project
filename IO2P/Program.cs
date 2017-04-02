using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    class Program : Nancy.NancyModule
    {
        static void Main(string[] args)
        {

        }

        public Program()
        {
            Get["/"] = _ => "Test";
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request)
        }
    }
}
