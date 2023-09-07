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

        public System.Windows.Visibility VisibilityOne1 { get; private set; }
        public System.Windows.Visibility VisibilityOne2 { get; private set; }
        public System.Windows.Visibility VisibilityOne3 { get; private set; }
        public System.Windows.Visibility VisibilityOne4 { get; private set; }
        public System.Windows.Visibility VisibilityOne5 { get; private set; }
        public System.Windows.Visibility VisibilityOne6 { get; private set; }

        public System.Windows.Visibility VisibilityTwo1 { get; private set; }
        public System.Windows.Visibility VisibilityTwo2 { get; private set; }
        public System.Windows.Visibility VisibilityTwo3 { get; private set; }
        public System.Windows.Visibility VisibilityTwo4 { get; private set; }
        public System.Windows.Visibility VisibilityTwo5 { get; private set; }
        public System.Windows.Visibility VisibilityTwo6 { get; private set; }

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

            switch (DieOne)
            {
                case 1:
                    VisibilityOne1 = System.Windows.Visibility.Visible;
                    VisibilityOne2 = System.Windows.Visibility.Hidden;
                    VisibilityOne3 = System.Windows.Visibility.Hidden;
                    VisibilityOne4 = System.Windows.Visibility.Hidden;
                    VisibilityOne5 = System.Windows.Visibility.Hidden;
                    VisibilityOne6 = System.Windows.Visibility.Hidden;

                    break;
                case 2:
                    VisibilityOne2 = System.Windows.Visibility.Visible;
                    VisibilityOne1 = System.Windows.Visibility.Hidden;
                    VisibilityOne3 = System.Windows.Visibility.Hidden;
                    VisibilityOne4 = System.Windows.Visibility.Hidden;
                    VisibilityOne5 = System.Windows.Visibility.Hidden;
                    VisibilityOne6 = System.Windows.Visibility.Hidden;

                    break;
                case 3:
                    VisibilityOne3 = System.Windows.Visibility.Visible;
                    VisibilityOne1 = System.Windows.Visibility.Hidden;
                    VisibilityOne2 = System.Windows.Visibility.Hidden;
                    VisibilityOne4 = System.Windows.Visibility.Hidden;
                    VisibilityOne5 = System.Windows.Visibility.Hidden;
                    VisibilityOne6 = System.Windows.Visibility.Hidden;
                    break;
                case 4:
                    VisibilityOne4 = System.Windows.Visibility.Visible;
                    VisibilityOne1 = System.Windows.Visibility.Hidden;
                    VisibilityOne2 = System.Windows.Visibility.Hidden;
                    VisibilityOne3 = System.Windows.Visibility.Hidden;
                    VisibilityOne5 = System.Windows.Visibility.Hidden;
                    VisibilityOne6 = System.Windows.Visibility.Hidden;
                    break;
                case 5:
                    VisibilityOne5 = System.Windows.Visibility.Visible;
                    VisibilityOne1 = System.Windows.Visibility.Hidden;
                    VisibilityOne2 = System.Windows.Visibility.Hidden;
                    VisibilityOne3 = System.Windows.Visibility.Hidden;
                    VisibilityOne4 = System.Windows.Visibility.Hidden;
                    VisibilityOne6 = System.Windows.Visibility.Hidden;
                    break;
                case 6:
                    VisibilityOne6 = System.Windows.Visibility.Visible;
                    VisibilityOne1 = System.Windows.Visibility.Hidden;
                    VisibilityOne2 = System.Windows.Visibility.Hidden;
                    VisibilityOne3 = System.Windows.Visibility.Hidden;
                    VisibilityOne4 = System.Windows.Visibility.Hidden;
                    VisibilityOne5 = System.Windows.Visibility.Hidden;
                    break;
                default:
                    break;
            }


            switch (DieTwo)
            {


                case 1:
                    VisibilityTwo1 = System.Windows.Visibility.Visible;
                    VisibilityTwo2 = System.Windows.Visibility.Hidden;
                    VisibilityTwo3 = System.Windows.Visibility.Hidden;
                    VisibilityTwo4 = System.Windows.Visibility.Hidden;
                    VisibilityTwo5 = System.Windows.Visibility.Hidden;
                    VisibilityTwo6 = System.Windows.Visibility.Hidden;
                    
                    break;
                case 2:
                    VisibilityTwo2 = System.Windows.Visibility.Visible;
                    VisibilityTwo1 = System.Windows.Visibility.Hidden;
                    VisibilityTwo3 = System.Windows.Visibility.Hidden;
                    VisibilityTwo4 = System.Windows.Visibility.Hidden;
                    VisibilityTwo5 = System.Windows.Visibility.Hidden;
                    VisibilityTwo6 = System.Windows.Visibility.Hidden;

                    break;
                case 3:
                    VisibilityTwo3 = System.Windows.Visibility.Visible;
                    VisibilityTwo1 = System.Windows.Visibility.Hidden;
                    VisibilityTwo2 = System.Windows.Visibility.Hidden;
                    VisibilityTwo4 = System.Windows.Visibility.Hidden;
                    VisibilityTwo5 = System.Windows.Visibility.Hidden;
                    VisibilityTwo6 = System.Windows.Visibility.Hidden;
                    break;
                case 4:
                    VisibilityTwo4 = System.Windows.Visibility.Visible;
                    VisibilityTwo1 = System.Windows.Visibility.Hidden;
                    VisibilityTwo2 = System.Windows.Visibility.Hidden;
                    VisibilityTwo3 = System.Windows.Visibility.Hidden;
                    VisibilityTwo5 = System.Windows.Visibility.Hidden;
                    VisibilityTwo6 = System.Windows.Visibility.Hidden;
                    break;
                case 5:
                    VisibilityTwo5 = System.Windows.Visibility.Visible;
                    VisibilityTwo1 = System.Windows.Visibility.Hidden;
                    VisibilityTwo2 = System.Windows.Visibility.Hidden;
                    VisibilityTwo3 = System.Windows.Visibility.Hidden;
                    VisibilityTwo4 = System.Windows.Visibility.Hidden;
                    VisibilityTwo6 = System.Windows.Visibility.Hidden;
                    break;
                case 6:
                    VisibilityTwo6 = System.Windows.Visibility.Visible;
                    VisibilityTwo1 = System.Windows.Visibility.Hidden;
                    VisibilityTwo2 = System.Windows.Visibility.Hidden;
                    VisibilityTwo3 = System.Windows.Visibility.Hidden;
                    VisibilityTwo4 = System.Windows.Visibility.Hidden;
                    VisibilityTwo5 = System.Windows.Visibility.Hidden;
                    break;

                default:
                  
                    break;

            }

        }
        #endregion

    }
}
