namespace ConsoleExample
{

    using System;
    using Netty;

    public class Program
    {

        static void Main(string[] args)
        {

            Console.Write("Starting Web Server ... ");

            var webServer = new NettyServer(@"..\..\..\SampleWebsite", "/Sample/");
            webServer.Start();

            Console.WriteLine("Done.");
            Console.WriteLine("Listening at: {0}", webServer.Port);
            Console.WriteLine("Press [ENTER] to exit.");

            Console.ReadLine();

            Console.WriteLine("Stopping Web Server ... ");

            webServer.Stop();

        }

    }

}