using System;
using System.Collections.Generic;

namespace DependencyResolution.ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloWorld();

            CircularReference();

            MultipleDependencies();

            MultipleItemsResolution();

            PreResolvedResolution();
        }

        static void HelloWorld()
        {
            Console.WriteLine("Hello World Example:");

            var e = new ItemNode<string>("e");
            var l = new ItemNode<string>("l");
            var l2 = new ItemNode<string>("l");
            var l3 = new ItemNode<string>("l");
            var h = new ItemNode<string>("h");
            var w = new ItemNode<string>("w");
            var space = new ItemNode<string>(" ");
            var d = new ItemNode<string>("d");
            var r = new ItemNode<string>("r");
            var o = new ItemNode<string>("o");
            var o2 = new ItemNode<string>("o");

            d.IsDependentOn(l3);
            l3.IsDependentOn(r);
            r.IsDependentOn(o2);
            o2.IsDependentOn(w);
            w.IsDependentOn(space);

            space.IsDependentOn(o);
            o.IsDependentOn(l2);
            l2.IsDependentOn(l);
            l.IsDependentOn(e);
            e.IsDependentOn(h);

            var resolver = new DependencyResolver<string>();
            var resolvedOrder = resolver.GetResolved(d);

            PrintStringNodes(resolvedOrder);
        }

        static void CircularReference()
        {
            Console.WriteLine("Circular Reference Example:");

            var a = new ItemNode<string>("a");
            var b = new ItemNode<string>("b");
            var c = new ItemNode<string>("c");
            var d = new ItemNode<string>("d");

            a.IsDependentOn(b);
            b.IsDependentOn(c);
            c.IsDependentOn(d); // here it comes
            d.IsDependentOn(b); // here it comes

            var resolver = new DependencyResolver<string>();

            try
            {
                var resolvedOrder = resolver.GetResolved(d);
            }
            catch (CircularReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void MultipleDependencies()
        {
            Console.WriteLine("Multiple Dependencies Example:");

            var aspnet = new ItemNode<string>("ASP.NET");
            var networking = new ItemNode<string>("Networking");
            var dotnet = new ItemNode<string>(".NET");
            var clr = new ItemNode<string>("CLR");
            var os = new ItemNode<string>("OS");
            var hardware = new ItemNode<string>("Hardware");

            aspnet.IsDependentOn(networking);
            aspnet.IsDependentOn(dotnet);

            networking.IsDependentOn(os);
            networking.IsDependentOn(hardware);

            dotnet.IsDependentOn(clr);
            dotnet.IsDependentOn(os);

            clr.IsDependentOn(os);

            os.IsDependentOn(hardware);

            var resolver = new DependencyResolver<string>();
            var resolvedOrder = resolver.GetResolved(aspnet);
            PrintStringNodes(resolvedOrder);
        }

        static void MultipleItemsResolution()
        {
            Console.WriteLine("Multiple Items Resolution Example:");

            var aspnet = new ItemNode<string>("ASP.NET");
            var networking = new ItemNode<string>("Networking");
            var dotnet = new ItemNode<string>(".NET");
            var clr = new ItemNode<string>("CLR");
            var os = new ItemNode<string>("OS");
            var hardware = new ItemNode<string>("Hardware");

            var imageLoader = new ItemNode<string>("ImageLoader");
            var loader = new ItemNode<string>("Loader");
            var textLoader = new ItemNode<string>("TextLoader");

            aspnet.IsDependentOn(networking);
            aspnet.IsDependentOn(dotnet);

            networking.IsDependentOn(os);
            networking.IsDependentOn(hardware);

            dotnet.IsDependentOn(clr);
            dotnet.IsDependentOn(os);

            clr.IsDependentOn(os);

            os.IsDependentOn(hardware);

            imageLoader.IsDependentOn(loader);
            loader.IsDependentOn(networking);
            loader.IsDependentOn(dotnet);
            textLoader.IsDependentOn(loader);

            var resolver = new DependencyResolver<string>();
            var resolvedOrder = resolver.GetResolved(new[] {
                imageLoader, textLoader, aspnet
            });
            PrintStringNodes(resolvedOrder);
        }

        static void PreResolvedResolution()
        {
            Console.WriteLine("Pre-resolved Resolution Example:");

            var networking = new ItemNode<string>("Networking");
            var dotnet = new ItemNode<string>(".NET");
            var clr = new ItemNode<string>("CLR");
            var os = new ItemNode<string>("OS");
            var hardware = new ItemNode<string>("Hardware");

            var imageLoader = new ItemNode<string>("ImageLoader");
            var loader = new ItemNode<string>("Loader");
            var textLoader = new ItemNode<string>("TextLoader");

            networking.IsDependentOn(os);
            networking.IsDependentOn(hardware);

            dotnet.IsDependentOn(clr);
            dotnet.IsDependentOn(os);

            clr.IsDependentOn(os);
            os.IsDependentOn(hardware);

            imageLoader.IsDependentOn(loader);
            loader.IsDependentOn(dotnet);
            textLoader.IsDependentOn(loader);

            var resolver = new MemorableDependencyResolver<string>();
            resolver.MarkAsResolved(dotnet);

            var resolvedOrder = resolver.GetResolved(new[] {
                imageLoader, textLoader
            });
            PrintStringNodes(resolvedOrder);
        }

        private static void PrintStringNodes(IEnumerable<ItemNode<string>> resolvedOrder)
        {
            foreach (var node in resolvedOrder)
            {
                Console.WriteLine(node.Item);
            }

            Console.WriteLine();
        }
    }
}
