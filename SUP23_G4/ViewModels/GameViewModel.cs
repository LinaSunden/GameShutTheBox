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


        #region Konstruktor

        public GameViewModel()
        {

        }

        public GameViewModel(StartViewModel startViewModel)
        {

            _startViewModel = startViewModel;
            Player1 = startViewModel.Player1;
            Player2 = startViewModel.Player2;
            FillCollectionOfGameTiles();
            //ShowDiceNumber();
            RollDiceCommand = new RelayCommand(x => DiceToss());
            ExecuteMoveCommand = new RelayCommand(x => MoveIsExecuted());
            PointCounterCommand = new RelayCommand(x => PointCounter());
        }

        #endregion



        #region Egenskaper
        public int DieOne { get; set; } = 5;

        public int DieTwo { get; set; } = 3;
        public int DiceValue { get; private set; }

        public ICommand RollDiceCommand { get; }

        public ICommand ExecuteMoveCommand { get; }
        public ICommand PointCounterCommand { get; }

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; } 

        public ObservableCollection<Tile> GameTiles { set; get; } = new ObservableCollection<Tile>();

        public Visibility ExecuteMove {  get; set; } = Visibility.Hidden;

        public bool IsThrowEnable { get; set; } = true;
        public int GameRound { get; set; } = 3;

    
        #endregion


        #region Instansvariabler

        private StartViewModel _startViewModel;

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
                DiceOne diceOne = new();
                DiceTwo diceTwo = new();
                if (i == 0)
                {
                    diceOne.DieOneValue = r.Next(1, 7);
                    DieOne = diceOne.DieOneValue;

                }
                else if (i == 1)
                {
                    diceTwo.DieTwoValue = r.Next(1, 7);
                    DieTwo = diceTwo.DieTwoValue;
                }
            }
            DiceValue = DieOne + DieTwo;
            FillListOfAvailableTiles();
            VisibilityGameButton();
        }

        public void VisibilityGameButton()
        {
            ExecuteMove = Visibility.Visible;
            IsThrowEnable = false;
        }

        public void MoveIsExecuted()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
            GameRound = GameRound + 1;
            
        }

        public void PointCounter()
        {
             foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.NotAvailableGameTile)
                {
                    if (GameRound == GameRound % 2)
                    {
                        Player2.Score += tile.TileValue;
                    }
                    else
                    {
                        Player1.Score += tile.TileValue;
                    }
                }
                
            }
        }

        public void FillCollectionOfGameTiles()
        {
            Tile tile;
            for (int i = 1; i <= 10; i++)
            {
                tile = new Tile();
                {
                    tile.TileValue = i;
                };
                GameTiles.Add(tile);
            }
        }


        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
        public void FillListOfAvailableTiles()
        {
            int diceValue = DiceValue;

            ObservableCollection<Tile> availableTiles = new ObservableCollection<Tile>();
            //List<int> availableTiles = new List<int>();
            //Tile availableTile; 
            foreach (Tile tile in GameTiles)
            {
                if (tile.TileValue <= diceValue)
                {
                    tile.CurrentStatus = Status.AvailableGameTile;
                    //for (int i = 1; i <= diceValue; i++)
                    //{
                    //    availableTile = new Tile()
                    //    {
                    //        Value = i,
                    //        DisplayValue = i.ToString(),
                    //        CurrentStatus = Status.AvailableGameTile
                    //    };
                    //    availableTiles.Add(availableTile);
                    //}
                    //break;
                }
                else if (tile.TileValue > diceValue ) 
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                
                }
                //else if (diceValue > GameTiles.Count())
                //{
                //    availableTiles = GameTiles;
                //}
            }
            //GameTiles = availableTiles;

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
        public void GetAvailableTiles(List<int> tiles, int targetSum)
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
                    for (int j = i + 1; j <= tiles.Count(); j++)
                    {
                        if (i + j == targetSum)
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
                            for (int k = i + 2; k <= tiles.Count(); k++)
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
                                    for (int l = i + 3; l <= tiles.Count(); l++)
                                    {
                                        if (j + l + k + l == targetSum)
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
        public bool CalculateSelectedTiles(List<int> selectedTiles, int targetSum)
        {
            int calculatedSum = 0;

            foreach (int i in selectedTiles)
            {
                calculatedSum += i;
            }
            if (calculatedSum == targetSum)
            {
                return true;
            }

            return false;
        }


        


        }
        #endregion
        
       
    }

