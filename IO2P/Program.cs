﻿using System;
using Nancy.Conventions;

namespace IO2P
{
    /// <summary>
    /// Główna klasa programu serwera.
    /// </summary>
    class Program 
    {
        /// <summary>
        /// Główna metoda programu serwera.
        /// </summary>
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


    }
}
