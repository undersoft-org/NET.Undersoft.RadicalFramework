namespace System.Instant.Mathset.Tests
{
    using System.Linq;

    using Xunit;

    public class MathsetTest
    {
        private Figure instFig;
        private Figures instMtic;
        private Computation rck;
        private IFigures spcMtic;

        public MathsetTest()
        {
            instMtic = new Figures(typeof(MathsetMockModel), "Figures_Mathset_Test");

            spcMtic = instMtic.Combine();

            MathsetMockModel fom = new MathsetMockModel();

            for (int i = 0; i < 2000 * 1000; i++)
            {
                ISleeve f = spcMtic.NewSleeve();
                f.Devisor = new MathsetMockModel();

                f["NetPrice"] = (double)f["NetPrice"] + i;
                f["SellFeeRate"] = (double)f["SellFeeRate"] / 2;
                spcMtic.Add(i, f);
            }
        }

        [Fact]
        public void Mathset_Computation_Formula_Test()
        {
            rck = new Computation(spcMtic);

            Mathset ml = rck.GetMathset("SellNetPrice");

            ml.Formula =
                (ml[nameof(MathsetMockModel.NetPrice)] * (ml["SellFeeRate"] / 100D))
                + ml["NetPrice"];

            Mathset ml2 = rck.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute();

            var a = spcMtic
                .Query(c => (double)c["NetPrice"] > 10 && (double)c["NetPrice"] < 50)
                .ToArray();

            ml.Formula = (ml["NetPrice"] * (ml["SellFeeRate"] / 95D)) + ml["NetPrice"];

            rck.Compute();

            var b = spcMtic
                .Query(c => (double)c["NetPrice"] < 10 || (double)c["NetPrice"] > 50)
                .ToArray();

            spcMtic
                .AsValues()
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

        [Fact]
        public void Mathset_Computation_LogicOnStack_Formula_Test()
        {
            spcMtic
                .AsValues()
                .ForEach(
                    (c) =>
                    {
                        if (((double)c["NetPrice"] < 10 || (double)c["NetPrice"] > 50))
                        {
                            c["SellNetPrice"] =
                                ((double)c["NetPrice"] * ((double)c["SellFeeRate"] / 100D))
                                + (double)c["NetPrice"];
                        }

                        c["SellGrossPrice"] = (double)c["SellNetPrice"] * (double)c["TaxRate"];
                    }
                );

            rck = new Computation(spcMtic);

            Mathset ml = rck.GetMathset("SellNetPrice");

            ml.Formula = (
                ((ml["NetPrice"] < 10) | (ml["NetPrice"] > 50))
                * ((ml["NetPrice"] * (ml["SellFeeRate"] / 100)) + ml["NetPrice"])
            );

            Mathset ml2 = rck.GetMathset("SellGrossPrice");

            ml2.Formula = ml * ml2["TaxRate"];

            rck.Compute();

            rck.Compute();
        }
    }
}
