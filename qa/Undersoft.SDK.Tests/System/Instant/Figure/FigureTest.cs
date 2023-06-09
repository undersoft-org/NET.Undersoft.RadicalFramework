namespace System.Instant.Tests
{
    using System.Reflection;
    using System.Uniques;

    using Xunit;

    public class FigureTest
    {
        public FigureTest() { }

        [Fact]
        public void Figure_Memberinfo_FieldsAndPropertiesModel_Compilation_Test()
        {
            Figure derivedType = new Figure<Agreement>(FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(derivedType, new Agreement());

            Figure referenceType = new Figure<Agreement>();
            IFigure figureB = Figure_Compilation_Helper_Test(referenceType, new Agreement());

            Figure valueType = new Figure<Agreement>(FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new Agreement());
        }

        [Fact]
        public void Figure_Memberinfo_FieldsOnlyModel_Compilation_Test()
        {
            Figure derivedType = new Figure(typeof(FieldsOnlyModel), FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(derivedType, new FieldsOnlyModel());

            Figure referenceType = new Figure(typeof(FieldsOnlyModel), FigureMode.Reference);
            IFigure figureB = Figure_Compilation_Helper_Test(referenceType, new FieldsOnlyModel());

            Figure valueType = new Figure(typeof(FieldsOnlyModel), FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new FieldsOnlyModel());
        }

        [Fact]
        public void Figure_Memberinfo_PropertiesOnlyModel_Compilation_Test()
        {
            Figure derivedType = new Figure(typeof(PropertiesOnlyModel), FigureMode.Derived);
            IFigure figureA = Figure_Compilation_Helper_Test(
                derivedType,
                new PropertiesOnlyModel()
            );

            Figure referenceType = new Figure(typeof(PropertiesOnlyModel), FigureMode.Reference);
            IFigure figureB = Figure_Compilation_Helper_Test(
                referenceType,
                new PropertiesOnlyModel()
            );

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);
            IFigure figureC = Figure_Compilation_Helper_Test(valueType, new PropertiesOnlyModel());
        }

        [Fact]
        public void Figure_MemberRubric_FieldsOnlyModel_Compilation_Test()
        {
            Figure referenceType = new Figure(
                FigureMocks.Figure_MemberRubric_FieldsOnlyModel(),
                "Figure_MemberRubric_FieldsOnlyModel_Reference"
            );
            FieldsOnlyModel fom = new FieldsOnlyModel();
            IFigure figureA = Figure_Compilation_Helper_Test(referenceType, fom);

            Figure valueType = new Figure(
                FigureMocks.Figure_Memberinfo_FieldsOnlyModel(),
                "Figure_MemberRubric_FieldsOnlyModel_ValueType",
                FigureMode.ValueType
            );
            fom = new FieldsOnlyModel();
            IFigure figureB = Figure_Compilation_Helper_Test(valueType, fom);
        }

        [Fact]
        public void Figure_ValueArray_GetSet_Test()
        {
            Figure referenceType = new Figure(typeof(FieldsAndPropertiesModel));

            Figure_Compilation_Helper_Test(
                referenceType,
                Figure_Compilation_Helper_Test(referenceType, new FieldsAndPropertiesModel())
            );

            Figure valueType = new Figure(typeof(PropertiesOnlyModel), FigureMode.ValueType);

            Figure_Compilation_Helper_Test(
                valueType,
                Figure_Compilation_Helper_Test(valueType, new FieldsAndPropertiesModel())
            );
        }

        private IFigure Figure_Compilation_Helper_Test(Figure str, Agreement fom)
        {
            IFigure rts = str.Combine();
            fom.Kind = AgreementKind.Agree;
            rts[0] = 1;
            Assert.Equal(fom.Kind, rts[0]);
            rts["Id"] = 555UL;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Comments)] = fom.Comments;
            Assert.Equal(fom.Comments, rts[nameof(fom.Comments)]);
            rts.UniqueCode = new Uscn(DateTime.Now.ToBinary());
            string hexTetra = rts.UniqueCode.ToString();
            Uscn ssn = new Uscn(hexTetra);
            Assert.Equal(ssn, rts.UniqueCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var ri = str.Rubrics[i].RubricInfo;
                if (ri.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)ri).Name);
                    if (fi != null)
                        rts[ri.Name] = fi.GetValue(fom);
                }
                if (ri.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)ri).Name);
                    if (pi != null)
                    {
                        var value = pi.GetValue(fom);
                        if (value != null)
                            rts[ri.Name] = value;
                    }
                }
            }

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal((rts)[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                    {
                        var value = pi.GetValue(fom);
                        if (value != null)
                            Assert.Equal((rts)[r.Name], pi.GetValue(fom));
                    }
                }
            }
            return rts;
        }

        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsAndPropertiesModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts[0] = 202;
            Assert.Equal(fom.Id, rts[0]);
            rts["Id"] = 404;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, rts[nameof(fom.Name)]);
            rts.UniqueCode = new Uscn(DateTime.Now.ToBinary());
            string hexTetra = rts.UniqueCode.ToString();
            Uscn ssn = new Uscn(hexTetra);
            Assert.Equal(ssn, rts.UniqueCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var ri = str.Rubrics[i].RubricInfo;
                if (ri.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)ri).Name);
                    if (fi != null)
                        rts[ri.Name] = fi.GetValue(fom);
                }
                if (ri.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)ri).Name);
                    if (pi != null)
                        (rts)[ri.Name] = pi.GetValue(fom);
                }
            }

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal((rts)[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal((rts)[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }

        private IFigure Figure_Compilation_Helper_Test(Figure str, FieldsOnlyModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts["Id"] = 404;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, rts[nameof(fom.Name)]);

            rts.UniqueCode = new Uscn(DateTime.Now.ToBinary());
            string hexTetra = rts.UniqueCode.ToString();
            Uscn ssn = new Uscn(hexTetra);
            Assert.Equal(ssn, rts.UniqueCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
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

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal(rts[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal(rts[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }

        private void Figure_Compilation_Helper_Test(Figure str, IFigure figure)
        {
            IFigure rts = str.Combine();
            object[] values = rts.ValueArray;
            rts.ValueArray = figure.ValueArray;
            for (int i = 0; i < values.Length; i++)
                Assert.Equal(figure[i], rts.ValueArray[i]);
        }

        private IFigure Figure_Compilation_Helper_Test(Figure str, PropertiesOnlyModel fom)
        {
            IFigure rts = str.Combine();
            fom.Id = 202;
            rts["Id"] = 404;
            Assert.NotEqual(fom.Id, rts[nameof(fom.Id)]);
            rts[nameof(fom.Name)] = fom.Name;
            Assert.Equal(fom.Name, rts[nameof(fom.Name)]);
            rts.UniqueCode = new Uscn(DateTime.Now.ToBinary());
            string hexTetra = rts.UniqueCode.ToString();
            Uscn ssn = new Uscn(hexTetra);
            Assert.Equal(ssn, rts.UniqueCode);

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
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

            for (int i = 1; i < str.Rubrics.Count; i++)
            {
                var r = str.Rubrics[i].RubricInfo;
                if (r.MemberType == MemberTypes.Field)
                {
                    var fi = fom.GetType().GetField(((FieldInfo)r).Name);
                    if (fi != null)
                        Assert.Equal(rts[r.Name], fi.GetValue(fom));
                }
                if (r.MemberType == MemberTypes.Property)
                {
                    var pi = fom.GetType().GetProperty(((PropertyInfo)r).Name);
                    if (pi != null)
                        Assert.Equal(rts[r.Name], pi.GetValue(fom));
                }
            }
            return rts;
        }
    }
}
