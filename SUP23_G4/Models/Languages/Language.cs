using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUP23_G4.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using SUP23_G4.FileHandler;
using SUP23_G4.Enums;

namespace SUP23_G4.Models.Languages
{
    public class Language : BaseViewModel
    {

        public Language()
        {

        }


        #region MainWindow
        public string HomePage { get; set; } 
        #endregion


        #region GameView
        public string Player1 { get; set; } 

        public string Player2 { get; set; } 

        public string Score { get; set; } 

        public string Round { get; set; } 

        public string MyTurn { get; set; } 

        public string GameRuleBtn { get; set; }

        public string ThrowDiceBtn { get; set; } 

        public string ConfirmMove { get; set; }

        public string valueTooLow { get; set; }

        #endregion

        #region StartView
        public string LanguageName { get; set; } 

        public string Flag { get; set; } 

        public string SelectLanguage { get; set; } 

        public string StartGame { get; set; } 

        public string GameRules { get; set; }

        public string DecidePoints { get; set; } 

        #endregion


        #region Meddelandeboxar

        public string PlayerWinner { get; set; }

        public string BonusGame {  get; set; }

        public string BonusGameWon { get; set; }

        public string Over45Player1 { get; set; }

        public string PlayerTurn { get; set; }

        public string BonusGameTurn { get; set; }


        #endregion

    

        #region Metoder

        /// <summary>
        /// Ändrar visningsspråket i appen genom en enum. Property SelectedLanguage fylls med det valda språket
        /// </summary>
        /// <param name="gameLanguage"></param>
        /// <returns></returns>
        public static Language UpdateLanguage(GameLanguage gameLanguage)
        {

            switch (gameLanguage)
            {
                case GameLanguage.Swedish:
                    return new Swedish();
                    break;
                case GameLanguage.English:
                    return new English();
                    break;
                default:
                    return null;
              

            }
        
        }

       

        #endregion



    }
}
