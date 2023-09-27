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
        public string HomePage { get; set; } = "Startsida";
        #endregion


        #region GameView
        public string Player1 { get; set; } = "Spelare 1: "; //finns även i StartView

        public string Player2 { get; set; } = "Spelare 2: "; //finns även i StartView

        public string Score { get; set; } = "Poäng: ";

        public string Round { get; set; } = "Omgång: ";

        public string MyTurn { get; set; } = "Din tur";

        public string GameRuleBtn { get; set; } = "Visa spelregler";

        public string ThrowDiceBtn { get; set; } = "Kasta tärningar";

        public string ConfirmMove { get; set; } = "Genomför drag";

        #endregion

        #region StartView
        public string LanguageName { get; set; } = "Svenska";

        public string Flag { get; set; } = "/Resources/SwedenFlag.png";

        public string SelectLanguage { get; set; } = "Välj språk";

        public string StartGame { get; set; } = "Starta spelet";

        public string GameRules { get; set; } = "Spelregler";

        #endregion



        #region Metoder
        private void UpdateLanguage()
        {
            switch (GameLanguage.Swedish)
            {

            }

            switch (GameLanguage.English) 
            {

            }

        }

        #endregion



    }
}
