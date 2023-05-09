namespace System.Instant.Mathset
{
    using BenchmarkDotNet.Attributes;
    using System.Linq;

    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RPlotExporter]
    public class MathsetBenchmark
    {
        private Figure figure;
        private Figures factory;
        private Computation cmp0;
        private Computation cmp1;
        private IFigures figures;

        public MathsetBenchmark()
        {
            factory = new Figures(typeof(MathsetMockModel), "Figures_Mathset_Test");

            figures = factory.Combine();

            MathsetMockModel fom = new MathsetMockModel();

            for (int i = 0; i < 2000 * 1000; i++)
            {
                ISleeve f = figures.NewSleeve();
                f.Devisor = new MathsetMockModel();

                f["NetPrice"] = (double)f["NetPrice"] + i;
                f["SellFeeRate"] = (double)f["SellFeeRate"] / 2;
                figures.Add(i, f);
            }

            cmp1 = new Computation(figures);

            Mathset m0 = cmp1["SellNetPrice"];

            m0.Formula = (m0["NetPrice"] * (m0["SellFeeRate"] / 100D)) + m0["NetPrice"];

            Mathset m1 = cmp1["SellGrossPrice"];

            m1.Formula = m0 * m1["TaxRate"];

            cmp1.Compute();
        }

        public void Mathset_With_Compilation()
        {
            cmp0 = new Computation(figures);

            Mathset ms0 = cmp0["SellNetPrice"];

            ms0.Formula = (ms0["NetPrice"] * (ms0["SellFeeRate"] / 100D)) + ms0["NetPrice"];

            Mathset ms1 = cmp0["SellGrossPrice"];

            ms1.Formula = ms0 * ms1["TaxRate"];

            cmp0.Compute();
        }

        [Benchmark]
        public void Mathset_Without_Compilation()
        {
            cmp1.Compute();
        }

        [Benchmark]
        public void Parellel_ForEach_Loop()
        {
            figures
                .AsParallel()
                .ForEach(
                    (c) =>
                    {
                        c["SellNetPrice"] =
                            ((double)c["NetPrice"] * ((double)c["SellFeeRate"] / 100D))
                            + (double)c["NetPrice"];

                        c["SellGrossPrice"] = (double)c["SellNetPrice"] * (double)c["TaxRate"];
                    }
                );
        }
    }
}
