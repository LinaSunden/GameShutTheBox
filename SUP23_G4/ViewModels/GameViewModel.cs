using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Die>? Dice { get; private set; }

        public void DiceToss()
        {
            Random r = new Random();
            Die die = new Die
            {
                DieValue = r.Next(1, 7)
            };
        }
    }
}
