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

                    Status.AvailableGameTile => 100,
                    Status.NotAvailableGameTile => 100,
                    Status.SelectedGameTile => 100,
                    Status.DownwardGameTile => 310,

                    _ => 50, 
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
