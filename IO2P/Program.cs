using System;
using Nancy.Conventions;

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

            protected override void ConfigureConventions(NancyConventions nancyConventions)
            {
                base.ConfigureConventions(nancyConventions);
                nancyConventions.StaticContentsConventions.AddDirectory("js", "front/js");
                nancyConventions.StaticContentsConventions.AddDirectory("css", "front/css");
            }
        }
        
    }
}
