namespace System.Instant.Tests
{
    using System.Instant.Relationing;
    using System.Uniques;
    using Xunit;

    public class FiguresLinkmapTest
    {
        [Fact]
        public void FiguresLinkmap_Test()
        {
            ISleeve figureA = new Sleeve(typeof(FieldsAndPropertiesModel)).Combine();

            ISleeve figureB = new Sleeve(typeof(FieldsAndPropertiesModel)).Combine();

            IFigures figuresA = new Figures(figureA).Combine();

            IFigures figuresB = new Figures(figureB).Combine();

            figuresA = FiguresLinkmap_AddFigures_A_Helper_Test(figuresA);

            figuresB = FiguresLinkmap_AddFigures_B_Helper_Test(figuresB);

            Relation fl = new Relation(figureA, figureB);
        }

        private IFigures FiguresLinkmap_AddFigures_A_Helper_Test(IFigures figures)
        {
            IFigures _figures = figures;
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            IFigure figureMock = _figures.NewFigure();
            int idSeed = (int)figureMock["Id"];
            DateTime seedKeyTick = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                IFigure figure = _figures.NewFigure();
                figure.ValueArray = figureMock.ValueArray;
                figure[7] = new Uscn(30000 + i * 300);
                figure.UniqueKey = (ulong)(int.MaxValue - (i * 30));
                figure.UniqueType = (ulong)(30000 + i * 300);
                _figures.Put(figure);
            }
            return _figures;
        }

        private IFigures FiguresLinkmap_AddFigures_B_Helper_Test(IFigures figures)
        {
            IFigures _figures = figures;
            FieldsAndPropertiesModel fom = new FieldsAndPropertiesModel();
            IFigure figureMock = _figures.NewFigure();
            int idSeed = (int)figureMock["Id"];
            DateTime seedKeyTick = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                {
                    IFigure figure = _figures.NewFigure();
                    figure.ValueArray = figureMock.ValueArray;
                    figure[7] = new Uscn(30000 + i * 300);
                    figure.UniqueKey = (ulong)(int.MaxValue + (i * 30));
                    figure.UniqueType = (ulong)(30000 + i * 300);
                    _figures.Put(figure);
                }
            }
            return _figures;
        }
    }
}
