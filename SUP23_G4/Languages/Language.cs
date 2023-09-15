using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUP23_G4.ViewModels;

namespace SUP23_G4.Languages
{
    public class Language: BaseViewModel
    {

        public Language()
        {
            PlayerName1 = "Spelare 1:";
        }

        public string PlayerName1 { get; set; }



    }
}
