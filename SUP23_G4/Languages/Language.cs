using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUP23_G4.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SUP23_G4.Languages
{
    public class Language: BaseViewModel
    {

        public Language()
        {
           
        }

        public string PlayerName1 { get; set; }

        public string PlayerName2 { get; set; }

        public string Points { get; set; }

        public string Round {  get; set; }

        public string MyTurn { get; set; }

        public string GameRuleBtn { get; set; }

        public string ThrowDiceBtn { get;set; }

        public string MakeMove { get; set; }

        public string LanguageName { get; set; }

        public string Flag {  get; set; }

        public string SelectLanguage { get; set; }



    }
}
