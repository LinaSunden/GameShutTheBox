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
                

                return messageStatus switch
                {
                    MessageStatus.Player1Winner => MainViewModel.Instance.CurrentLanguage.PlayerWinner,
                    MessageStatus.Player2Winner => MainViewModel.Instance.CurrentLanguage.PlayerWinner,
                    MessageStatus.BonusGame => MainViewModel.Instance.CurrentLanguage.BonusGame,
                    MessageStatus.BonusGameWon1 => MainViewModel.Instance.CurrentLanguage.BonusGameWon,
                    MessageStatus.BonusGameWon2 => MainViewModel.Instance.CurrentLanguage.BonusGameWon,
                    MessageStatus.Over45Player1 => MainViewModel.Instance.CurrentLanguage.Over45Player1,
                    MessageStatus.Player1Turn => MainViewModel.Instance.CurrentLanguage.PlayerTurn,
                    MessageStatus.Player2Turn => MainViewModel.Instance.CurrentLanguage.PlayerTurn,
                    MessageStatus.BonusGameTurn => MainViewModel.Instance.CurrentLanguage.BonusGameTurn,
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
