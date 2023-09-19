using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SUP23_G4.Converters
{
    public class StatusToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status && value != null)
            {
                var status = (Status)value;
                return status switch
                {

                    Status.AvailableGameTile => 80,
                    Status.NotAvailableGameTile => 80,
                    Status.SelectedGameTile => 80,
                    Status.DownwardGameTile => 250,

                    _ => 50, //osäker på vilken storlek vi ska sätta här? har gjort den extra liten för att identifiera fel i test
                };
            }
            return 50;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
