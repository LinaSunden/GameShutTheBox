using SUP23_G4.Enums;
using SUP23_G4.Views.Dice;
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
    class DiceValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && value != null)
            {
                int diceValue = (int)value;

               bool isDiceSideVisible = diceValue == int.Parse(parameter.ToString());

                if (isDiceSideVisible)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
               
            }
            return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
