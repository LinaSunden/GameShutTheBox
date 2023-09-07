using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SUP23_G4.Converters
{
    class DiceNumberToDotConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DiceNumber && value != null)
            {
                var diceNumber = (DiceNumber)value;
                List<string> visibleDots = new List<string>();

                switch (diceNumber)
                {
                    case DiceNumber.One:
                        visibleDots.Add("Dot7");
                        break;
                    case DiceNumber.Two:
                        visibleDots.Add("Dot1");
                        visibleDots.Add("Dot4");
                        break;
                    case DiceNumber.Three:
                        visibleDots.Add("Dot1");
                        visibleDots.Add("Dot4");
                        visibleDots.Add("Dot7");
                        break;
                    case DiceNumber.Four:
                        visibleDots.Add("Dot1");
                        visibleDots.Add("Dot2");
                        visibleDots.Add("Dot4");
                        visibleDots.Add("Dot5");
                        break;
                    case DiceNumber.Five:
                        visibleDots.Add("Dot1");
                        visibleDots.Add("Dot2");
                        visibleDots.Add("Dot4");
                        visibleDots.Add("Dot5");
                        visibleDots.Add("Dot7");
                        break;
                    case DiceNumber.Six:
                        visibleDots.Add("Dot1");
                        visibleDots.Add("Dot2");
                        visibleDots.Add("Dot3");
                        visibleDots.Add("Dot4");
                        visibleDots.Add("Dot5");
                        visibleDots.Add("Dot6");
                        break;
                }

                return visibleDots;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
