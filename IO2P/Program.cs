using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Nancy;
//using static Nancy.Hosting.Self.FileSystemRootPathProvider;

namespace IO2P
{
    class Program //: Nancy.NancyModule
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

        /*public Program()
        {
            Get["/"] = x =>
            {
                return "Test";
            };
            Get["/test"] = _ => "Test";
            Get["/ajax"] = x => View["C:/Users/Tomek/Desktop/IA4/ajax.html"];
            Post["/newfile"] = _ => new resourceAdder().handlePost(this.Request);
            //Get["/listfiles"] = _ => twoja funkcja.
        }*/


        private class CustomBootstrapper : Nancy.DefaultNancyBootstrapper
        {
            /*protected override Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
            {
                get
                {
                    return Nancy.Bootstrapper.NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
                }
            }

            void OnConfigurationBuilder(Nancy.Bootstrapper.NancyInternalConfiguration x)
            {
                x.ViewLocationProvider = typeof(Nancy.ViewEngines.ResourceViewLocationProvider);
            }

            protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
            {
                base.ConfigureApplicationContainer(container);
                Nancy.ViewEngines.ResourceViewLocationProvider.RootNamespaces.Add(
                  System.Reflection.Assembly.GetAssembly(typeof(Nancy.Diagnostics.Modules.MainModule)), "VSMDemo.Web.Views");
            }*/

            protected override Nancy.IRootPathProvider RootPathProvider
            {
                get
                {
                    return new Nancy.Hosting.Self.FileSystemRootPathProvider();
                    //return new Nancy.Hosting.Aspnet.AspNetRootPathProvider();
                    //return base.RootPathProvider;
                }
            }
        }
    }
}
