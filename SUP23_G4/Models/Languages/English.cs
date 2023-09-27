using SUP23_G4.FileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.Models.Languages
{
    public class English : Language
    {
        public English()
        {
            if (File.Exists("English.json"))
            {
                LanguageEnglish = JsonFileHandler.Open<Language>("English.json");

            }
        }

        public Language LanguageEnglish { get; set; }
    }
}
