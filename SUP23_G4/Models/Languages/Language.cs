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
        public string HomePage { get; set; } /*= "Startsida";*/
        #endregion


        #region GameView
        public string Player1 { get; set; } /*= "Spelare 1: ";*/ //finns även i StartView

        public string Player2 { get; set; } /*= "Spelare 2: ";*/ //finns även i StartView

        public string Score { get; set; } /*="Poäng: ";*/

        public string Round { get; set; } /*= "Omgång: ";*/

        public string MyTurn { get; set; } /*= "Din tur";*/

        public string GameRuleBtn { get; set; }/* = "Visa spelregler";*/

        public string ThrowDiceBtn { get; set; } /*= "Kasta tärningar";*/

        public string ConfirmMove { get; set; }/* = "Genomför drag";*/

        #endregion

        #region StartView
        public string LanguageName { get; set; } /*= "Svenska";*/

        public string Flag { get; set; } /*= "/Resources/SwedenFlag.png";*/

        public string SelectLanguage { get; set; } /*= "Välj språk";*/

        public string StartGame { get; set; } /*= "Starta spelet";*/

        public string GameRules { get; set; }/* = "Spelregler";*/

        public string DecidePoints { get; set; } /*= "Antal poäng att spela till";*/

        #endregion


        #region Meddelandeboxar

        public string Player1Winner { get; set; }/* = "Grattis Player1 du har vunnit!\n\rVill du köra en rematch?";*/

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
