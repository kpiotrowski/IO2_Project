using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    class Program 
    {
        static void Main(string[] args)
        {
            try
            {
                var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664/"), new CustomBootstrapper());
                nancyHost.Start();
                Console.WriteLine("Web server running...");
                //System.Diagnostics.Process.Start("http://localhost:9664");
                Console.ReadLine();
                nancyHost.Stop();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Console.ReadLine();
            }
        }


        private class CustomBootstrapper : Nancy.DefaultNancyBootstrapper
        {

            protected override Nancy.IRootPathProvider RootPathProvider
            {
                get
                {
                    return new Nancy.Hosting.Self.FileSystemRootPathProvider();
                }
            }
        }
    }
}
