using SUP23_G4.Commands;
using SUP23_G4.Enums;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public GameViewModel()
        {
            ShowDiceNumber();
            RollDiceCommand = new RelayCommand(x => ShowDiceNumber());
        }


        #region Egenskaper
        public int DieOne { get; private set; }
        public int DieTwo { get; private set; } = 2;

        public System.Windows.Visibility Visibility1 { get; private set; }
        public System.Windows.Visibility Visibility5 { get; private set; } 

       public ICommand RollDiceCommand { get; }

        #endregion

        #region Metoder
        public void DiceToss()
        {
            Random r = new Random();
            
            for (int i = 0; i < 2; i++)
            {
                Dice die = new Dice();
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


        public void ShowDiceNumber()
        {
            DiceToss();

    
            switch (DieTwo)
            {


                case 1:
                    Visibility1 = System.Windows.Visibility.Visible;
                    Visibility5 = System.Windows.Visibility.Hidden;
                    
                    break;
                case 2:
                    Visibility1 = System.Windows.Visibility.Hidden;
                    Visibility5 = System.Windows.Visibility.Visible;

                    break;
                default:
                  
                    break;



            }

        }
        #endregion

    }
}
