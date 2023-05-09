namespace System.Instant.Tests
{
    using MongoDB.Driver;
    using System.Instant.Stock;
    using System.Reflection;

    using Xunit;

    public class StockTest
    {
        private IFigure ifigure;
        private IFigures ifigures;
        private Figures figures;
        private Figure figure;

        [Fact]
        public void Figures_Compile_Test()
        {
            figure = new Figure(typeof(FieldsAndPropertiesModel));
            ifigure = Figure_Compilation_Helper_Test(figure, new FieldsAndPropertiesModel());

            figures = new Figures(figure, "InstantSequence_Compilation_Test");

            var rttab = figures.Combine();

            for (int i = 0; i < 10000; i++)
            {
                rttab.Add((long)int.MaxValue + i, rttab.NewFigure());
            }

            for (int i = 9999; i > -1; i--)
            {
                rttab[i] = rttab.Get(i + (long)int.MaxValue);
            }
        }

        [Fact]
        public void Figures_MutatorAndAccessorById_Test()
        {
            figure = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            ifigure = Figure_Compilation_Helper_Test(figure, fom);

            figures = new Figures(figure, "InstantSequence_Compilation_Test");

            ifigures = figures.Combine();

            Stocker stocker = new Stocker<FieldsAndPropertiesModel>();

            stocker.Open();

            var newfigure = ifigures.NewFigure();
            ifigures.Put(newfigure);
            newfigure.UniqueCode.ValueToXYZ(10, 100 * 100, 34989);

            ifigures[0].ValueArray = ifigure.ValueArray;

            stocker[newfigure.UniqueCode] = newfigure;

            object loadfigure = stocker[newfigure.UniqueCode];

            Assert.Equal(ifigure[4], ifigures[0, 4]);
        }

        [Fact]
        public void Figures_MutatorAndAccessorByName_Test()
        {
            figure = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            ifigure = Figure_Compilation_Helper_Test(figure, fom);

            figures = new Figures(figure, "InstantSequence_Compilation_Test");

            ifigures = figures.Combine();

            ifigures.Add(ifigures.NewFigure());
            ifigures[0, nameof(fom.Name)] = ifigure[nameof(fom.Name)];

            Assert.Equal(ifigure[nameof(fom.Name)], ifigures[0, nameof(fom.Name)]);
        }

        [Fact]
        public void Figures_NewFigure_Test()
        {
            figure = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            ifigure = Figure_Compilation_Helper_Test(figure, fom);

            figures = new Figures(figure, "InstantSequence_Compilation_Test");

            ifigures = figures.Combine();

            IFigure rcst = ifigures.NewFigure();

            Assert.NotNull(rcst);
        }

        [Fact]
        public void Figures_SetRubrics_Test()
        {
            figure = new Figure(typeof(FieldsAndPropertiesModel));
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            ifigure = Figure_Compilation_Helper_Test(figure, fom);

            figures = new Figures(figure, "InstantSequence_Compilation_Test");

            var rttab = figures.Combine();

            Assert.Equal(rttab.Rubrics, figures.Rubrics);
        }

        private IFigure Figure_Compilation_Helper_Test(Figure figure, FieldsAndPropertiesModel fom)
        {
            IFigure rts = figure.Combine();

            for (int i = 1; i < figure.Rubrics.Count; i++)
            {
                var r = figure.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        rts[r.Name] = fi.GetValue(fom);
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        rts[r.Name] = pi.GetValue(fom);
                }
            }
            return rts;
        }
    }
}
