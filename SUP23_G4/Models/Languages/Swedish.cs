using SUP23_G4.FileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.Models.Languages
{
    public class Swedish : Language
    {
        public Swedish()
        {
            if (File.Exists("Swedish.json"))
            {
                SelectedLanguage = JsonFileHandler.Open<Language>("Swedish.json");
            }
        }

    }
}
