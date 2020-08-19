using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoCAD_Windows.Converters
{
    public class LineweightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "1")
                return (netDxf.Lineweight)value switch
                {
                    netDxf.Lineweight.Default => 0,
                    netDxf.Lineweight.ByBlock => 1,
                    netDxf.Lineweight.ByLayer => 2,
                    netDxf.Lineweight.W0 => 3,
                    netDxf.Lineweight.W5 => 4,
                    netDxf.Lineweight.W9 => 5,
                    netDxf.Lineweight.W13 => 6,
                    netDxf.Lineweight.W15 => 7,
                    netDxf.Lineweight.W18 => 8,
                    netDxf.Lineweight.W20 => 9,
                    netDxf.Lineweight.W25 => 10,
                    netDxf.Lineweight.W30 => 11,
                    netDxf.Lineweight.W35 => 12,
                    netDxf.Lineweight.W40 => 13,
                    netDxf.Lineweight.W50 => 14,
                    netDxf.Lineweight.W53 => 15,
                    netDxf.Lineweight.W60 => 16,
                    netDxf.Lineweight.W70 => 17,
                    netDxf.Lineweight.W80 => 18,
                    netDxf.Lineweight.W90 => 19,
                    netDxf.Lineweight.W100 => 20,
                    netDxf.Lineweight.W106 => 21,
                    netDxf.Lineweight.W120 => 22,
                    netDxf.Lineweight.W140 => 23,
                    netDxf.Lineweight.W158 => 24,
                    netDxf.Lineweight.W200 => 25,
                    netDxf.Lineweight.W211 => 26,
                    _ => netDxf.Lineweight.Default
                };
            else if (parameter.ToString() == "2")
                return (netDxf.Lineweight)value switch
                {
                    netDxf.Lineweight.Default => "Default",
                    netDxf.Lineweight.ByBlock => "ByBlock",
                    netDxf.Lineweight.ByLayer => "ByLayer",
                    netDxf.Lineweight.W0 => "0.00 mm",
                    netDxf.Lineweight.W5 => "0.05 mm", 
                    netDxf.Lineweight.W9 => "0.09 mm",
                    netDxf.Lineweight.W13 => "0.13 mm",
                    netDxf.Lineweight.W15 => "0.15 mm",
                    netDxf.Lineweight.W18 => "0.18 mm",
                    netDxf.Lineweight.W20 => "0.20 mm",
                    netDxf.Lineweight.W25 => "0.25 mm",
                    netDxf.Lineweight.W30 => "0.30 mm",
                    netDxf.Lineweight.W35 => "0.35 mm",
                    netDxf.Lineweight.W40 => "0.40 mm",
                    netDxf.Lineweight.W50 => "0.50 mm",
                    netDxf.Lineweight.W53 => "0.53 mm",
                    netDxf.Lineweight.W60 => "0.60 mm",
                    netDxf.Lineweight.W70 => "0.70 mm",
                    netDxf.Lineweight.W80 => "0.80 mm",
                    netDxf.Lineweight.W90 => "0.90 mm",
                    netDxf.Lineweight.W100 => "1.00 mm",
                    netDxf.Lineweight.W106 => "1.06 mm",
                    netDxf.Lineweight.W120 => "1.20 mm",
                    netDxf.Lineweight.W140 => "1.40 mm",
                    netDxf.Lineweight.W158 => "1.58 mm",
                    netDxf.Lineweight.W200 => "2.00 mm",
                    netDxf.Lineweight.W211 => "2.11 mm",
                    _ => netDxf.Lineweight.Default
                };

            return new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "1")
                return System.Convert.ToInt32(value) switch
                {
                    0 => netDxf.Lineweight.Default,
                    1 => netDxf.Lineweight.ByBlock,
                    2 => netDxf.Lineweight.ByLayer,
                    3 => netDxf.Lineweight.W0,
                    4 => netDxf.Lineweight.W5,
                    5 => netDxf.Lineweight.W9,
                    6 => netDxf.Lineweight.W13,
                    7 => netDxf.Lineweight.W15,
                    8 => netDxf.Lineweight.W18,
                    9 => netDxf.Lineweight.W20,
                    10 => netDxf.Lineweight.W25,
                    11 => netDxf.Lineweight.W30,
                    12 => netDxf.Lineweight.W35,
                    13 => netDxf.Lineweight.W40,
                    14 => netDxf.Lineweight.W50,
                    15 => netDxf.Lineweight.W53,
                    16 => netDxf.Lineweight.W60,
                    17 => netDxf.Lineweight.W70,
                    18 => netDxf.Lineweight.W80,
                    19 => netDxf.Lineweight.W90,
                    20 => netDxf.Lineweight.W100,
                    21 => netDxf.Lineweight.W106,
                    22 => netDxf.Lineweight.W120,
                    23 => netDxf.Lineweight.W140,
                    24 => netDxf.Lineweight.W158,
                    25 => netDxf.Lineweight.W200,
                    26 => netDxf.Lineweight.W211,
                    _ => netDxf.Lineweight.Default
                };

            return new NotImplementedException();
        }
    }
}
