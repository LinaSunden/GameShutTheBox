using SUP23_G4.Enums;
using SUP23_G4.Models.Languages;
using SUP23_G4.ViewModels;
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
     public class SituationToMessageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageStatus && value != null)
            {
                var messageStatus = (MessageStatus)value;
                MainViewModel mainViewModel = parameter as MainViewModel;

                return messageStatus switch
                {
                    MessageStatus.Player1Winner => "Grattis Player1 du har vunnit!\n\rVill du köra en rematch?",
                    MessageStatus.Player2Winner => "GRATTIS!\r\bDu har vunnit! Vill du köra en rematch?",
                    MessageStatus.BonusGame => "Spelet slutade lika då\r\bbåda spelarna fick samma poäng.\r\nVill ni köra en bonusomgång?",
                    MessageStatus.BonusGameWon1 => "GRATTIS!\r\bDu har vunnit bonusomgångenomgången\r\boch spelet! Vill du köra en rematch?",
                    MessageStatus.BonusGameWon2 => "GRATTIS!\r\bDu har vunnit bonusomgångenomgången\r\boch spelet! Vill du köra en rematch?",
                    MessageStatus.Over45Player1 => "Din tur är nu över!\r\bOm inte din motståndare får mer poäng\r\bän dig förlorar du.",
                    MessageStatus.Player1Turn => "Din tur är nu över!\n\r\n\r",
                    MessageStatus.Player2Turn => "Din tur är nu över!\n\r\n\r",
                    MessageStatus.BonusGameTurn => "Nu är din bonustur slut.\n\r\n\r",
                    MessageStatus.EndGame => "Vill du avsluta spelet \r\boch gå tillbaka till startsida?",
                    _ => "",
                };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
