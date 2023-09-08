using SUP23_G4.Commands;
using SUP23_G4.Enums;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using SUP23_G4.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private StartViewModel _startViewModel;
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        //public GameViewModel()
        //{
        //    ShowDiceNumber();
        //    RollDiceCommand = new RelayCommand(x => ShowDiceNumber());
        //}
        public GameViewModel(StartViewModel startViewModel)
        {
            _startViewModel = startViewModel;
            Player1 = startViewModel.Player1;
            Player2 = startViewModel.Player2;

            FillCollectionOfGameTiles();
            //ShowDiceNumber();
            RollDiceCommand = new RelayCommand(x => ShowDiceNumber());

            
        }




        #region Egenskaper
        public int DieOne { get; private set; }
        public int DieTwo { get; private set; }
        public int DiceValue { get; private set; }
        //public List<int> AvailableTiles { get; set; } = new List<int>();

        public System.Windows.Visibility VisibilityOne1 { get; private set; } = Visibility.Visible;
        public System.Windows.Visibility VisibilityOne2 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityOne3 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityOne4 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityOne5 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityOne6 { get; private set; } = Visibility.Hidden;

        public System.Windows.Visibility VisibilityTwo1 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityTwo2 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityTwo3 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityTwo4 { get; private set; } = Visibility.Hidden;
        public System.Windows.Visibility VisibilityTwo5 { get; private set; } = Visibility.Visible;
        public System.Windows.Visibility VisibilityTwo6 { get; private set; } = Visibility.Hidden;

        public ICommand RollDiceCommand { get; }

        public ObservableCollection<Tile> GameTiles { set; get; } = new ObservableCollection<Tile>();
           
        public void FillCollectionOfGameTiles() 
        {
            Tile tile; 
            for (int i= 1; i<=10; i++) 
            {
                tile = new Tile();
                {
                    tile.DisplayValue = i.ToString();
                    tile.Value = i; 
                };
                GameTiles.Add(tile);
            }

        }


        public GameViewModel()
        {
            
        }



        #endregion

        #region Metoder

        /// <summary>
        /// Kastar två tärningar och får uppdaterade värden på DieOne och DieTwo
        /// </summary>
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
            DiceValue = DieOne + DieTwo;
            FillListOfAvailableTiles();
        }

        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
        public void FillListOfAvailableTiles()
        {
            int diceValue = DiceValue;

            //List<int> tiles = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ObservableCollection<Tile> gameTiles = GameTiles;
            ObservableCollection<Tile> availableTiles = new ObservableCollection<Tile>();
            //List<int> availableTiles = new List<int>();
            Tile availableTile; 
            foreach (Tile tile in GameTiles)
            {
                if (tile.Value == diceValue)
                {
                    for (int i = 1; i <= diceValue; i++)
                    {
                        availableTile = new Tile()
                        {
                            Value = i,
                            DisplayValue = i.ToString(),
                        };
                        availableTiles.Add(availableTile);
                    }
                    break;
                }
                else if (diceValue > GameTiles.Count())
                {
                    availableTiles = GameTiles;
                }
            }
            GameTiles = availableTiles;

            //GetAvailableTiles(availableTiles, diceValue);

        }

        /// <summary>
        /// Metod som testar om spelarens val av brickor är möjliga att välja
        /// för att nå tärningarnas summa (Eventuellt överflödig)
        /// </summary>
        //public void AvailableTilesAfterSelectedTile(int selectedTile, int targetSum)
        //{
        //    List<int> availableTiles = new List<int>();

        //    if (selectedTile == targetSum)
        //    {

        //    }
        //    else if (selectedTile < targetSum) 
        //    { 
        //        foreach (int i in AvailableTiles)
        //        {
        //            if (selectedTile + i == targetSum)
        //            {
        //                availableTiles.Add(selectedTile);
        //                availableTiles.Add(i);
        //            }

        //        }
            
        //    }
        //}

        /// <summary>
        /// Metod som undersöker vilka kombinationer av brickor som är
        /// möjliga för att nå tärningarnas summa
        /// </summary>
        public void GetAvailableTiles(List<int>tiles, int targetSum)
        {

            List<List<int>> collection = new List<List<int>>();
            List<int> availableTiles;

            foreach (int i in tiles)
            {
                if (i == targetSum)
                {
                    availableTiles = new List<int>()
                    { 
                        i,
                    };
                    collection.Add(availableTiles);
                    break;
                }
                else if (i < targetSum)
                {
                    for(int j = i + 1; j <= tiles.Count(); j++)
                    {
                        if (i + j ==  targetSum)
                        {
                            availableTiles = new List<int>()
                            {
                                i,
                                j,
                            };
                            collection.Add(availableTiles);
                            break;
                        }
                        else if (i + j < targetSum)
                        {
                            for (int k = i + 2;  k <= tiles.Count(); k++)
                            {
                                if (i + j + k == targetSum)
                                {
                                    availableTiles = new List<int>()
                                    {
                                        i,
                                        j,
                                        k,
                                    };
                                    collection.Add(availableTiles);
                                    break;
                                }
                                else if (i + j + k < targetSum)
                                {
                                    for ( int l = i + 3; l <= tiles.Count(); l++)
                                    {
                                        if (j + l + k +l == targetSum)
                                        {
                                            availableTiles = new List<int>()
                                            {
                                                i,
                                                j,
                                                k,
                                                l,
                                            };
                                            collection.Add(availableTiles);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metod som testar om spelarens valda brickor blir tärningarnas
        /// summa (Eventuellt överflödig)
        /// </summary>
        public bool CalculateSelectedTiles(List<int>selectedTiles, int targetSum)
        {
            int calculatedSum = 0;

            foreach(int i in selectedTiles)
            {
                calculatedSum += i;
            }
            if (calculatedSum == targetSum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Väljer vilken specifk tärningssida för båda tärningarna som ska synas 
        /// i gränssnittet när tärningen har kastats.
        /// </summary>
        public void ShowDiceNumber()
        {
            DiceToss();

            VisibilityOne1 = System.Windows.Visibility.Hidden;
            VisibilityOne2 = System.Windows.Visibility.Hidden;
            VisibilityOne3 = System.Windows.Visibility.Hidden;
            VisibilityOne4 = System.Windows.Visibility.Hidden;
            VisibilityOne5 = System.Windows.Visibility.Hidden;
            VisibilityOne6 = System.Windows.Visibility.Hidden;

            VisibilityTwo1 = System.Windows.Visibility.Hidden;
            VisibilityTwo2 = System.Windows.Visibility.Hidden;
            VisibilityTwo3 = System.Windows.Visibility.Hidden;
            VisibilityTwo4 = System.Windows.Visibility.Hidden;
            VisibilityTwo5 = System.Windows.Visibility.Hidden;
            VisibilityTwo6 = System.Windows.Visibility.Hidden;



            switch (DieOne)
            {
                case 1:
                    VisibilityOne1 = System.Windows.Visibility.Visible;
                    

                    break;
                case 2:
                    VisibilityOne2 = System.Windows.Visibility.Visible;
                  

                    break;
                case 3:
                    VisibilityOne3 = System.Windows.Visibility.Visible;
                    
                    break;
                case 4:
                    VisibilityOne4 = System.Windows.Visibility.Visible;
                   
                    break;
                case 5:
                    VisibilityOne5 = System.Windows.Visibility.Visible;
                    
                    break;
                case 6:
                    VisibilityOne6 = System.Windows.Visibility.Visible;
                    
                    break;
                default:
                    break;
            }


            switch (DieTwo)
            {


                case 1:
                    VisibilityTwo1 = System.Windows.Visibility.Visible;
                    
                    
                    break;
                case 2:
                    VisibilityTwo2 = System.Windows.Visibility.Visible;
                   

                    break;
                case 3:
                    VisibilityTwo3 = System.Windows.Visibility.Visible;
                    
                    break;
                case 4:
                    VisibilityTwo4 = System.Windows.Visibility.Visible;
                    
                    break;
                case 5:
                    VisibilityTwo5 = System.Windows.Visibility.Visible;
                    
                    break;
                case 6:
                    VisibilityTwo6 = System.Windows.Visibility.Visible;
                    
                    break;

                default:
                  
                    break;

            }

        }
        #endregion
        

    }
}
