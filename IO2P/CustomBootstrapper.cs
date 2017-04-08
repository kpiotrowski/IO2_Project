using Nancy.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2P
{
    /// <summary>
    /// Klasa ustawiająca statyczną zawartość (szablony css, pliki javascript)
    /// i wybierająca RootPathProvider dla Nancy.
    /// </summary>
    class CustomBootstrapper : Nancy.DefaultNancyBootstrapper
    {

        /// <summary>
        /// Metoda ustawiająca RootPathProvider dla Nancy
        /// </summary>
        protected override Nancy.IRootPathProvider RootPathProvider
        {
            get
            {
                return new Nancy.Hosting.Self.FileSystemRootPathProvider();
            }
        }

        /// <summary>
        /// Metoda ustawiająca statyczną zawartość (szablony css, pliki javascript) dla Nancy
        /// </summary>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.AddDirectory("js", "front/js");
            nancyConventions.StaticContentsConventions.AddDirectory("css", "front/css");
        }
    }
}
