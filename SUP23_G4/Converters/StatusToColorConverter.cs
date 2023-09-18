using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                    Status.AvailableGameTile => new SolidColorBrush(Colors.Transparent),
                    Status.NotAvailableGameTile => new SolidColorBrush(Color.FromArgb (180, 105, 105, 105)), //(int alpha, int red, int green, int blue) alpha = opacitet från 0-255, färg= grå
                    Status.SelectedGameTile => new SolidColorBrush(Color.FromArgb (180, 255, 255, 0)), //opacitet = 180, färg = gul
                    Status.DownwardGameTile => new SolidColorBrush(Colors.Transparent),
                    _ => new SolidColorBrush(Colors.White),
                } ; ;
            }
            return new SolidColorBrush(Colors.Transparent);
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }
}
