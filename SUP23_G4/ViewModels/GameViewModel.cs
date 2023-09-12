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
            ExecuteMoveCommand = new RelayCommand(x => CompareSelectedTilesWithDiceValue());
            NewSelectedTileCommand = new RelayCommand(x => UpdateStatusOfChosenGameTileInObservableCollection(x));

        }

        #endregion



        #region Egenskaper
        public int DieOne { get; set; } = 5;

        public int DieTwo { get; set; } = 3;
        public int DiceValue { get; private set; }

        public ICommand RollDiceCommand { get; }

        public ICommand ExecuteMoveCommand { get; }

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public ObservableCollection<Tile> GameTiles { set; get; } = new ObservableCollection<Tile>();

        public Visibility ExecuteMove {  get; set; } = Visibility.Hidden;

        public bool IsThrowEnable { get; set; } = true;

        public ICommand NewSelectedTileCommand { get; set; }

        public Tile tile = new Tile();




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
            SetStatusOfGameTiles();
            VisibilityGameButton();
        }


        public void ChangeStatusOfChosenTile(Tile tile)
        {

            if (tile.CurrentStatus == Status.AvailableGameTile)
            {
                tile.CurrentStatus = Status.SelectedGameTile;
            }

            else if (tile.CurrentStatus == Status.SelectedGameTile)
            {
                tile.CurrentStatus = Status.AvailableGameTile;
            }
        }


        public void UpdateStatusOfChosenGameTileInObservableCollection(Object x)
        {
            var tile = (Tile)x;
            ChangeStatusOfChosenTile(tile);
            foreach (Tile t in GameTiles)
            {
                if (tile.TileValue == t.TileValue)
                {
                    t.CurrentStatus = tile.CurrentStatus;
                }

            }
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
        }

        public void FillCollectionOfGameTiles()
        {
            Tile tile;
            for (int i = 1; i <= 10; i++)
            {
                tile = new Tile();
                {
                    tile.TileValue = i;
                    tile.CurrentStatus = Status.AvailableGameTile;
                };
                GameTiles.Add(tile);
            }
        }


        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
        public void SetStatusOfGameTiles()
        {

            foreach (Tile tile in GameTiles)
            {
                if (tile.TileValue <= DiceValue && tile.CurrentStatus != Status.DownwardGameTile)
                {
                    tile.CurrentStatus = Status.AvailableGameTile;

                }
                else if (tile.TileValue > DiceValue && tile.CurrentStatus != Status.DownwardGameTile) 
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;               
                }
            }
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
        /// summa och sätter relevant status (Eventuellt överflödig)
        /// </summary>
        public void CompareSelectedTilesWithDiceValue()
        {
            int calculatedSum = 0;

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.SelectedGameTile)
                {
                    calculatedSum += tile.TileValue;
                }
            }

            if (calculatedSum == DiceValue)
            {
                MessageBox.Show("Rätt");
                
                foreach (Tile tile in GameTiles)
                {
                    if (tile.CurrentStatus == Status.SelectedGameTile)
                    {
                        tile.CurrentStatus = Status.DownwardGameTile;
                    }
                }
                MoveIsExecuted();
            }
            else if (calculatedSum < DiceValue)
            {
                MessageBox.Show("För lågt");
            }
            else if (calculatedSum > DiceValue)
            {
                MessageBox.Show("För högt");
            }
        }





    }
        #endregion
        
       
    }

