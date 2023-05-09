namespace System.Series.Tests
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class Deck64_Test : DeckTestHelper
    {
        public Deck64_Test() : base()
        {
            registry = new Deck64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Deck64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        [Benchmark]
        public void Deck64_IndentifierKeys_Test()
        {
            Deck_Integrated_Test(identifierKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Deck64_IntKeys_Test()
        {
            Deck_Integrated_Test(intKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Deck64_LongKeys_Test()
        {
            Deck_Integrated_Test(longKeyTestCollection.Take(100000).ToArray());
        }

        [Benchmark]
        public void Deck64_StringKeys_Test()
        {
            Deck_Integrated_Test(stringKeyTestCollection.Take(100000).ToArray());
        }
    }
}
