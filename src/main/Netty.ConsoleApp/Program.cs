namespace Netty.ConsoleApp
{

    using System;
    using Netty;

    /// <summary>
    ///     A class that provides an entry point for the application.
    /// </summary>
    public class Program
    {

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {

            Console.Write("Starting Web Server ... ");

            // Setup a new web server using a random port.  The ports can be shared as long as
            // the virtual path is unique.
            var webServer = new NettyServer(@"..\..\..\SampleWebsite", "/Sample/");

            // Update an application setting, and then start the server
            webServer
                .AlterApplicationSetting("Key1", "I am updated.")
                .Start();

            Console.WriteLine("Done.");
            Console.WriteLine("Listening at: {0}", webServer.Port);
            Console.WriteLine("Press [ENTER] to exit.");

            Console.ReadLine();

            // Stop the web server - this will restore the configuration to the original values
            Console.WriteLine("Stopping Web Server ... ");

            webServer.Stop();

        }

    }

}