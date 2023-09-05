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
        #region Egenskaper
        public int DieOne { get; private set; }
        public int DieTwo { get; private set; }

        #endregion

        #region Metoder
        public void DiceToss()
        {
            Random r = new Random();
            
            for (int i = 0; i < 2; i++)
            {
                Die die = new Die();
                if (i == 0)
                {
                    die.DieValue = r.Next(1, 7);
                    DieOne = die.DieValue;
                }
                else if (i == 1)
                {
                    die.DieValue = r.Next(1, 7);
                    DieTwo = die.DieValue;
                }
            }          
        }
        #endregion

    }
}
