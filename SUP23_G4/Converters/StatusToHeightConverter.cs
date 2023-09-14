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

                    Status.AvailableGameTile => 250,
                    Status.NotAvailableGameTile => 50,
                    Status.SelectedGameTile => 50,
                    Status.DownwardGameTile => 150,

                    _ => 250, //osäker på vilken storlek vi ska sätta här? har gjort den extra stor för att identifiera fel i test
                };
            }
            return 150;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
