namespace Netty.ConsoleApp
{

    using System;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;
    using Netty;

    internal class Options
    {
        [Option('p', "physical-path", Required = true, HelpText = "The physical path to the website.")]
        public string PhysicalPath { get; set; }

        [Option('v', "virtual-path", DefaultValue = "/", HelpText = "The virtual path the website runs on.")]
        public string VirtualPath { get; set; }

        [Option('i', "port", DefaultValue = -1, HelpText = "The port the website runs on.")]
        public int Port { get; set; }

        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    /// <summary>
    ///     A class that provides an entry point for the application.
    /// </summary>
    public class Program
    {

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        public static int Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return -1;
            }

            Console.Write("Starting Web Server ... ");

            int port = options.Port;
            if (port == -1)
            {
                port = NetworkUtility.FindRandomOpenPort();
            }

            var webServer = new NettyServer(options.PhysicalPath, options.VirtualPath, port);

            // Update an application setting, and then start the server
            webServer.Start();

            Console.WriteLine("Done.");
            Console.WriteLine("Listening at: {0}", webServer.Port);
            Console.WriteLine("Press [ENTER] to exit.");

            Console.ReadLine();

            // Stop the web server - this will restore the configuration to the original values
            Console.WriteLine("Stopping Web Server ... ");

            webServer.Stop();

            return 0;
        }

    }

}