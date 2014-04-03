netty
=====

Netty – a small, fast, embeddable web server and [ASP.NET](http://en.wikipedia.org/wiki/ASP.NET) [application server](http://en.wikipedia.org/wiki/Application_server).

Netty was inspired by [Jetty](http://www.eclipse.org/jetty/about.php) – an embedded Java web server and [Servlet](http://en.wikipedia.org/wiki/Java_Servlet) container. Like Jetty, Netty is designed to run in-process, which is extremely useful when you want to quickly start serving static or dynamic content, but don’t want or need the overhead of using IIS. Anything that you can host in [IIS](http://en.wikipedia.org/wiki/Internet_Information_Services), you should be able to host in Netty. This includes:
* ASP.NET web pages
*	SOAP web services written using WCF or ASMX
*	RESTful web services written using WebAPI
*	Static content, such as HTML, CSS, Javascript, and images
*	And more!

Why use Netty?
====

*	Netty is a great software testing companion. Spin up a web server in your test framework’s [SetupFixture](http://www.nunit.org/index.php?p=setupFixture&r=2.4) and test all of your web pages and web services right in-process. No messy deployment steps, no extra scripts, no leftover files, just code. And as a neat side-effect, it’s easy to measure the effect of your functional tests and regression tests on your code coverage. See our [BDD Test Example](src/examples/SampleBddTest/AddSomeNumbersSteps.cs) for how this could work.
*	Netty is great when you aren’t writing a traditional browser-based web application. With Netty, it’s possible to ship your whole website or client-server app, without requiring your customers to set up and configure IIS. Our [Console Application Example](src/examples/ConsoleExample/Program.cs) demonstrates how this can work.
*	As a quick, ad-hoc webserver. You can use the standalone Netty console application to quickly start serving up any local directory from your hard drive.

At Merchant Warehouse, we’re using Netty as part of our [Behavior Driven Testing Framework](http://en.wikipedia.org/wiki/Behavior-driven_development) as well as the backbone of our [Store and Forward](http://en.wikipedia.org/wiki/Store_and_forward) product, which allows merchants to accept credit and debit card transactions, even while they’re offline. Netty is known to work great on Windows, Linux, and Mac.

#Examples

Here's "Hello World" in Netty:

```c#
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
```

#Supported platforms

Netty builds on versions 4.0 and later of Microsoft's CLR. It has also been built and tested using Mono 3.2 on CentOS 6.4.

#Downloading / Installation

Currently, Netty is only available as a source download. If you’d like to provide a Nuget package, that'd be very welcome.

# Contributing

We love contributions! Please send [pull requests](https://help.github.com/articles/using-pull-requests) our way. All that we ask is that you please include unit tests with all of your pull requests.

# Getting help

We also love bug reports & feature requests. You can file bugs and feature requests in our [Github Issue Tracker](https://github.com/merchantwarehouse/netty/issues). Please consider including the following information when you file a ticket:
* What version you're using
* What command or code you ran
* What output you saw
* How the problem can be reproduced. A small Visual Studio project zipped up or code snippet that demonstrates or reproduces the issue is always appreciated.

You can also always find help on the [Netty Google Group](https://groups.google.com/forum/#!forum/netty-web-server).
