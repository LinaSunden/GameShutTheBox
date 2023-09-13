﻿using SUP23_G4.Commands;
using SUP23_G4.Enums;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using SUP23_G4.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            ExecuteMoveCommand = new RelayCommand(x => MoveIsExecuted());
            PointCounterCommand = new RelayCommand(x => PointCounter());

            TurnPlayer1 = Visibility.Visible;
            TurnPlayer2 = Visibility.Hidden;

        }

        #endregion



        #region Egenskaper
        public int DieOne { get; set; } = 5;

        public int DieTwo { get; set; } = 3;
        public int DiceValue { get; private set; }

        public ICommand RollDiceCommand { get; }

        public ICommand ExecuteMoveCommand { get; }
        public ICommand PointCounterCommand { get; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; } 

        public ObservableCollection<Tile> GameTiles { set; get; } = new ObservableCollection<Tile>();

        public Visibility ExecuteMove {  get; set; } = Visibility.Hidden;

        public Visibility TurnPlayer1 { get; set; }

        public Visibility TurnPlayer2 { get; set; } 

        public bool IsThrowEnable { get; set; } = true;
        public int GameRound { get; set; } = 1;
        public int Player1Point { get; set; } = 0;
        public int Player2Point { get; set; } = 0;

        public ICommand NewSelectedTileCommand { get; set; }

        public Tile tile = new Tile();

        public Brush ForegroundBrushPlayer1 { get; set; } = Brushes.White;

        public Brush ForegroundBrushPlayer2 { get; set; } = Brushes.White;




        #endregion


        #region Instansvariabler

        public StartViewModel _startViewModel;

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
            GetAvailableTiles();
            //SetStatusOfGameTiles();
            VisibilityGameButton();
        }

        /// <summary>
        /// Ändrar status på vald tile från view
        /// </summary>
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

        /// <summary>
        /// Ändrar status på vald tile i kollektionen av tiles
        /// </summary>
        public void UpdateStatusOfChosenGameTileInObservableCollection(Object x)
        {
            var tile = (Tile)x;
            ChangeStatusOfChosenTile(tile);
            foreach (Tile t in GameTiles)
            {
                if (tile.TileValue == t.TileValue)
                {
                    t.CurrentStatus = tile.CurrentStatus;                    
                    UpdateStatusOfAvailableTiles(GetTargetSum());
                }
            }
        }

        public void UpdateStatusOfAvailableTiles(int targetSum)
        {
            
            if (targetSum == 0)
            {
                foreach (Tile t in GameTiles)
                {
                    if (t.CurrentStatus == Status.AvailableGameTile)
                    {
                        t.CurrentStatus = Status.NotAvailableGameTile;
                    }               
                }
            }
            else if (targetSum > 0)
            {
                int combinedSum = 0;

                foreach (Tile t in GameTiles)
                {
                    if ((t.TileValue == targetSum) && t.CurrentStatus == Status.AvailableGameTile)
                    {
                        t.CurrentStatus = Status.AvailableGameTile;
                    }
                    else if ((t.TileValue > targetSum) && t.CurrentStatus == Status.AvailableGameTile)
                    {
                        t.CurrentStatus = Status.NotAvailableGameTile;
                    }
                    else if ((t.TileValue < targetSum) && t.CurrentStatus == Status.AvailableGameTile)
                    {
                        combinedSum += t.TileValue;
                        if (combinedSum > targetSum)
                        {
                            t.CurrentStatus = Status.NotAvailableGameTile;
                            combinedSum -= t.TileValue;
                        }
                        //foreach (Tile u in GameTiles)
                        //{
                        //    if (t.TileValue + u.TileValue == targetSum)
                        //    {
                        //        t.CurrentStatus = Status.AvailableGameTile;
                        //        u.CurrentStatus = Status.AvailableGameTile;
                        //    }
                        //    else if (t.TileValue + u.TileValue > targetSum)
                        //    {
                        //        u.CurrentStatus = Status.NotAvailableGameTile;
                        //    }
                        //}
                        //for (int i = 1; i < targetSum; i++)
                        //{
                        //    if (t.TileValue != i)
                        //    {
                        //        int combinedSum = t.TileValue + i;
                        //        if (combinedSum == targetSum)
                        //        {

                        //        }
                        //    }
                        //}

                        //if ((targetSum -= t.TileValue) < targetSum)
                        //{
                        //    t.CurrentStatus = Status.NotAvailableGameTile;
                        //}

                    }

                }
            }
        }
        /// <summary>
        /// Metod som uppdaterar riktvärdet för metoden "UpdateStatusOfAvailableTiles"
        /// </summary>
        private int GetTargetSum()
        {
            int targetSum = DiceValue;

            foreach (Tile t in GameTiles)
            {
                if (t.CurrentStatus == Status.SelectedGameTile)
                {
                    targetSum -= t.TileValue;
                }
            }
            return targetSum;
        }

        public void VisibilityGameButton()
        {
            ExecuteMove = Visibility.Visible;
            IsThrowEnable = false;
        }

        /// <summary>
        /// GameRound Startar vid 1, och plusar för tillfället vid varje genomförd drag, en omgång består av varje spelares drag.
        /// </summary>
        public void MoveIsExecuted()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
            GameRound = GameRound + 1;
            SwitchPlayerTurn();
        }

        /// <summary>
        /// Metod för att räkna ut varje spelares poäng. För tillfället är vald brickas status satt till NotAvailableGameTile, inte DownWardTile som är målet
        /// En property har gjorts som int Player1Point som tar Player1's värde då Vi inte fick till Binding Player1.Score att uppdateras.
        /// En Modulus används för att avgöra om Spelare 1 / Spelare 2 får poäng. Spelare 1 = Ojämna omgångar, Spelare 2 = Jämna omgångar.
        /// </summary>
        public void PointCounter()
        {

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.NotAvailableGameTile)
                {
                    if (GameRound % 2 != 0)
                    {
                         Player1Point = Player1.Score += tile.TileValue;
                    }
                    else
                    {
                         Player2Point =  Player2.Score += tile.TileValue;
                    }
                }

            }
            Point45();
        }

        public void SwitchPlayerTurn()
        {
            if (GameRound % 2 != 0)
            {
                TurnPlayer1 = Visibility.Visible;
                TurnPlayer2 = Visibility.Hidden;
            }
            else
            {
                TurnPlayer2 = Visibility.Visible;
                TurnPlayer1 = Visibility.Hidden;
            }
        }

        public void Point45()
        {
            if (Player1Point >= 45)
            {
                ForegroundBrushPlayer1 = Brushes.Red;
                MessageBox.Show($"Du har fått {Player1Point} poäng. " +
                    $"Om spelare2 inte får fler poäng förlorar du");
            }
            if (Player2Point >= 45)
            {
               ForegroundBrushPlayer2 = Brushes.Red;
                MessageBox.Show($"Du har fått {Player2Point} poäng. " +
                     $"Om spelare1 inte får fler poäng förlorar du");
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
                    tile.CurrentStatus = Status.AvailableGameTile;
                };
                GameTiles.Add(tile);
            }
        }


        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
        public void SetStatusOfGameTiles(List<int> sortedList)
        {
            foreach (Tile tile in GameTiles)
            {
                if (sortedList.Contains(tile.TileValue) && tile.CurrentStatus != Status.DownwardGameTile)
                {
                    tile.CurrentStatus = Status.AvailableGameTile;
                }
                else
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                }

            }
        }

        /// <summary>
        /// Metod som testar om spelarens val av brickor är möjliga att välja
        /// för att nå tärningarnas summa (Eventuellt överflödig)
        /// </summary>


        /// <summary>
        /// Metod som undersöker vilka kombinationer av brickor som är
        /// möjliga för att nå tärningarnas summa
        /// </summary>
        public void GetAvailableTiles()
        {
            List<int> tiles = new List<int>() {1,2,3,4,5,6,7,8,9,10 };
            List<List<int>> collection = new List<List<int>>();
            List<int> availableTiles;

            foreach (int i in tiles)
            {
                if (i == DiceValue)
                {
                    availableTiles = new List<int>()
                    {
                        i,
                    };
                    collection.Add(availableTiles);
                    break;
                }
                else if (i < DiceValue)
                {
                    for (int j = i + 1; j <= tiles.Count(); j++)
                    {
                        if (i + j == DiceValue)
                        {
                            availableTiles = new List<int>()
                            {
                                i,
                                j,
                            };
                            collection.Add(availableTiles);
                            break;
                        }
                        else if (i + j < DiceValue)
                        {
                            for (int k = i + 2; k <= tiles.Count(); k++)
                            {
                                if (i + j + k == DiceValue)
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
                                else if (i + j + k < DiceValue)
                                {
                                    for (int l = i + 3; l <= tiles.Count(); l++)
                                    {
                                        if (j + l + k + l == DiceValue)
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
            collection = SortOutDownWardTiles(collection);
            List<int> sortedList = SortOutDuplicates(collection);
            SetStatusOfGameTiles(sortedList);
        }

        private List<List<int>> SortOutDownWardTiles(List<List<int>>collection)
        {
            foreach (List<int> list in collection)
            {
                foreach (int i in list)
                {
                    if (GameTiles[i + 1].CurrentStatus == Status.DownwardGameTile)
                    {
                        collection.Remove(list);
                    }
                }
            }
            return collection;
        }
        private List<int> SortOutDuplicates(List<List<int>> collection)
        {
            List<int> sorted = new List<int>();

            foreach (List<int> list in collection)
            {
                foreach (int i in list)
                {
                    if (!sorted.Contains(i))
                    {
                        sorted.Add(i);
                    }
                }
            }
            sorted.Sort();
            return sorted;
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

