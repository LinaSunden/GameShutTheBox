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
                    MessageStatus.Player1Winner => mainViewModel.CurrentLanguage.Player1Winner,
                    MessageStatus.Player2Winner => "Grattis Player2 du har vunnit!\n\rVill du köra en rematch?",
                    MessageStatus.BonusGame => "Spelet slutade lika då \n\rbåda spelarna fick samma poäng,\n\rVill ni köra en bonusomgång?",
                    //MessageStatus.GameFinished1,
                    //MessageStatus.GameFinished2,
                    MessageStatus.BonusGameWon1 => "Grattis Player1, du har vunnit \n\rbonusomgångenomgången\n\roch spelet! Vill du köra en rematch?",
                    MessageStatus.BonusGameWon2 => "Grattis Player2, du har vunnit \n\rbonusomgångenomgången\n\roch spelet! Vill du köra en rematch?",
                    MessageStatus.Player1Turn => "Player1 tur är över,\n\rdet är nu Player2s tur",
                    MessageStatus.Player2Turn => "Player2 tur är över,\n\rdet är nu Player1s tur",
                    MessageStatus.BonusGameTurn => "Nu är din bonustur slut.\n\rDet är nu Player2s tur",

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
