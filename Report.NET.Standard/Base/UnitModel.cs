namespace Report.NET.Standard.Base
{
    public class UnitModel
    {
        /// <summary>Conversion factor: millimeter to point</summary>
        private const double MMToPoint = 1.0 / 25.4 * 72.0;

        /// <summary>Conversion factor: point to millimeter</summary>
        private const double PointToMM = 1.0 / 72.0 * 25.4;

        private double unit = 0;

        public double MM
        {
            get => unit;
            set => unit = value;
        }

        public double Point
        {
            get => unit * MMToPoint;
            set => unit = value * PointToMM;
        }

        public static UnitModel operator *(UnitModel a, double m) => new UnitModel() { MM = a.MM * m };
        public static UnitModel operator /(UnitModel a, double m) => new UnitModel() { MM = a.MM / m };
        public static UnitModel operator *(UnitModel a, UnitModel m) => new UnitModel() { MM = a.MM * m.MM };
        public static UnitModel operator /(UnitModel a, UnitModel m) => new UnitModel() { MM = a.MM / m.MM };
        public static UnitModel operator +(UnitModel a, UnitModel m) => new UnitModel() { MM = a.MM + m.MM };
        public static UnitModel operator -(UnitModel a, UnitModel m) => new UnitModel() { MM = a.MM - m.MM };
    }
}
