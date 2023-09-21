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

namespace SUP23_G4.Languages
{
    public class Language: BaseViewModel
    {

        public Language()
        {
            
        }

        public string PlayerName1 { get; set; } = "Spelare 1: ";

        public string PlayerName2 { get; set; } = "Spelare 2: ";

        public string Points { get; set; } = "Poäng: ";

        public string Round {  get; set; } = "Omgång: ";

        public string MyTurn { get; set; } = "Din tur";

        public string GameRuleBtn { get; set; } = "Visa spelregler";

        public string ThrowDiceBtn { get; set; } = "Kasta tärningar";

        public string MakeMove { get; set; } = "Genomför drag";

        public string LanguageName { get; set; } = "Svenska";

        public string Flag { get; set; } = "/Resources/SwedenFlag.png";

        public string SelectLanguage { get; set; } = "Välj språk";





    }
}
