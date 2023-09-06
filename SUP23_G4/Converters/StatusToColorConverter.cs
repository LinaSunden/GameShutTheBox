using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SUP23_G4.Converters
{
    public class StatusToColorConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status && value != null)
            {
                var status = (Status)value;
                return status switch
                {
                    Status.AvailableGameTile => new SolidColorBrush(Colors.Tan),
                    Status.NotAvailableGameTile => new SolidColorBrush(Colors.Gray),
                    Status.SelectedGameTile => new SolidColorBrush(Colors.Yellow),
                    _ => new SolidColorBrush(Colors.White),
                };
            }
            return new SolidColorBrush(Colors.Tan);
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }
}
