namespace System.Instant.Tests
{
    using System.Runtime.InteropServices;
    using System.Uniques;

    [StructLayout(LayoutKind.Sequential)]
    public class FieldsAndPropertiesModel
    {
        public int Id { get; set; } = 404;

        [FigureKey]
        public long Key = long.MaxValue;

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias { get; set; } = "ProperSize";

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ByteArray { get; set; }

        public double Factor { get; set; } = 2 * (long)int.MaxValue;

        public Guid GlobalId { get; set; } = new Guid();

        public bool Status { get; set; }

        public Uscn SystemKey { get; set; } = Uscn.New;

        public long time = 0;
        public DateTime Time
        {
            get => DateTime.FromBinary(time);
            set => time = value.ToBinary();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FieldsOnlyModel
    {
        [FigureKey]
        public int Id = 404;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias = "ProperSize";

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray = new byte[10];
        public double Factor = 2 * (long)int.MaxValue;
        public Guid GlobalId = new Guid();
        public long Key = long.MaxValue;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name = "SizeIsTwoTimesLonger";

        [FigureDisplay("AvgPrice")]
        [FigureTreatment(SummaryOperand = SummarizeOperand.Sum)]
        public double Price;
        public bool Status;
        public Uscn SystemKey = Uscn.Empty;
        public DateTime Time = DateTime.Now;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PropertiesOnlyModel
    {
        [FigureKey(IsAutoincrement = true, Order = 0)]
        public int Id { get; set; } = 405;

        [FigureKey]
        public long Key { get; set; } = long.MaxValue;

        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Alias { get; set; } = "ProperSize";

        [FigureAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ByteArray { get; set; }

        public double Factor { get; set; } = 2 * (long)int.MaxValue;

        public Guid GlobalId { get; set; } = new Guid();

        [FigureKey(Order = 1)]
        [FigureDisplay("ProductName")]
        [FigureAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Name { get; set; } = "SizeIsTwoTimesLonger";

        [FigureDisplay("AvgPrice")]
        [FigureTreatment(SummaryOperand = SummarizeOperand.Avg)]
        public double Price { get; set; }

        public bool Status { get; set; }

        public Uscn SystemKey { get; set; } = Uscn.Empty;

        public DateTime Time { get; set; } = DateTime.Now;
    }
}
