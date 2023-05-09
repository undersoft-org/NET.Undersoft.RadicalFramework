namespace System.Series.Tests
{
    using BenchmarkDotNet.Attributes;
    using System.Diagnostics;
    using System.Linq;

    public class Album64_Test : AlbumTestHelper
    {
        public Album64_Test() : base()
        {
            registry = new Album64<string>();
            DefaultTraceListener Logfile = new DefaultTraceListener();
            Logfile.Name = "Logfile";
            Trace.Listeners.Add(Logfile);
            Logfile.LogFileName = $"Album64_{DateTime.Now.ToFileTime().ToString()}_Test.log";
        }

        [Benchmark]
        public void Album64_IndentifierKeys_Test()
        {
            Album_Integrated_Test(identifierKeyTestCollection.Take(250000).ToArray());
        }

        [Benchmark]
        public void Album64_IntKeys_Test()
        {
            Album_Integrated_Test(intKeyTestCollection.Take(250000).ToArray());
        }

        [Benchmark]
        public void Album64_LongKeys_Test()
        {
            Album_Integrated_Test(longKeyTestCollection.Take(250000).ToArray());
        }

        [Benchmark]
        public void Album64_StringKeys_Test()
        {
            Album_Integrated_Test(stringKeyTestCollection.Take(250000).ToArray());
        }
    }
}
