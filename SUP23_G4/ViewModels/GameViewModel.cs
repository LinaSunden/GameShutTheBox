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
using System.Drawing;
using System.Linq;
using System.Media;
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
            GameTiles = new ObservableCollection<Tile>();
            FillCollectionOfGameTiles();
            RollDiceCommand = new RelayCommand(x => DiceToss());
            ExecuteMoveCommand = new RelayCommand(x => CompareSelectedTilesWithDiceValue());
            NewSelectedTileCommand = new RelayCommand(x => UpdateStatusOfChosenGameTileInObservableCollection(x));
            PointCounterCommand = new RelayCommand(x => PointCounter());

            TurnPlayer1 = Visibility.Visible;
            TurnPlayer2 = Visibility.Hidden;
            Player2Name = "Frida";
            Player1Name = "Gibson"; 

        }

        #endregion



        #region Egenskaper
        public int DieOne { get; set; } = 5;

        public int DieTwo { get; set; } = 3;
        public int DiceValue { get; private set; }

        public ICommand RollDiceCommand { get; }
        public ICommand NewSelectedTileCommand { get; set; }
        public ICommand ExecuteMoveCommand { get; }
        public ICommand PointCounterCommand { get; }
        public ICommand GoToStartCommand { get; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; } 

        public ObservableCollection<Tile> GameTiles { set; get; }

        public Visibility ExecuteMove {  get; set; } = Visibility.Hidden;

        public Visibility TurnPlayer1 { get; set; }

        public Visibility TurnPlayer2 { get; set; } 
        public Visibility GameRoundVisibility { get; set; }
        public Visibility BonusRoundVisibility { get; set; } = Visibility.Hidden; 
        public bool IsThrowEnable { get; set; } = true;
        public int GameRoundCounter { get; set; } = 1;
        public int PlayerTurnCounter { get; set; } = 1;     
        public int Player1Point { get; set; } = 40;
        public int Player2Point { get; set; } = 40;
        public string Player1Name { get; set; } 
        public string Player2Name { get; set; }

        public bool IsTileEnabled { get; set; } = false;

        public Tile tile = new Tile();

        public Brush ForegroundBrushPlayer1 { get; set; } = Brushes.White;

        public Brush ForegroundBrushPlayer2 { get; set; } = Brushes.White;

        private List<List<int>> Collection { get; set; }


        #endregion


        #region Instansvariabler

        public StartViewModel _startViewModel;

        #endregion




        #region Metoder
        /// <summary>
        /// Skapar 10 tiles med värde 1-10 och ger dem status AvailableGameTile och lägger dem i en ObservableCollection som heter GameTiles.
        /// </summary>
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
            VisibilityGameButton();
            GetAvailableTiles();
            var soundPlayer = new SoundPlayer(Properties.Resources.dice_rolls_30cm);
            soundPlayer.Play();
            IsTileEnabled = true;
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
                    
                }
            }
            UpdateStatusOfAvailableTiles();
        }

        public void UpdateStatusOfAvailableTiles()
        {
            List<List<int>> updatedCollection = new List<List<int>>(Collection);

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.SelectedGameTile)
                {
                    foreach (List<int> list in Collection)
                    {
                        if (!list.Contains(tile.TileValue))
                        {
                            updatedCollection.Remove(list);
                        }
                    }
                }
            }

            List<int> sortedTiles = SortOutDuplicates(updatedCollection);
            SetStatusOfGameTiles(sortedTiles);
        }
     
        /// <summary>
        /// Metod som sätter alla tiles till available, används vid start av ny spelares tur
        /// </summary>
        public void SetNewGameTurn()
        {
            
            foreach(Tile tile in GameTiles)
            {
                if(tile.CurrentStatus != Status.AvailableGameTile) 
                {
                    tile.CurrentStatus = Status.AvailableGameTile;
                }
            }
            VisibilityDiceButton();

        }
        /// <summary>
        /// Ger vyn förutsättningar och utseende för att spela en bonusomgång
        /// </summary>
        public void StartBonusGame()
        {
            Player1Point = 0;
            Player2Point = 0;
            ForegroundBrushPlayer1 = Brushes.White;
            ForegroundBrushPlayer2 = Brushes.White;
            SwitchGameRoundVisibility();

        }
        /// <summary>
        /// Metod som ger vyn försättningar och utseende för att spela en vanlig spelomgång
        /// </summary>
        public void StartNewGame()
        {
            SetNewGameTurn();
            Player1Point = 0;
            Player2Point = 0;
            ForegroundBrushPlayer1 = Brushes.White;
            ForegroundBrushPlayer2 = Brushes.White;
            GameRoundCounter = 1;
            BonusRoundVisibility = Visibility.Hidden;
            GameRoundVisibility = Visibility.Visible; 

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

        public void NotAvailableToAvailable()
        {
            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.NotAvailableGameTile)
                {
                    tile.CurrentStatus = Status.AvailableGameTile;
                }
            }
        }

        /// <summary>
        /// Metod som gör
        /// </summary>
        public void VisibilityGameButton()
        {
            ExecuteMove = Visibility.Visible;
            IsThrowEnable = false;
        }

        /// <summary>
        /// Metod som gör DiceButton synlig igen
        /// </summary>
        public void VisibilityDiceButton()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
        }

        /// <summary>
        /// GameRound Startar vid 1, och plusar för tillfället vid varje genomförd drag, en omgång består av varje spelares drag.
        /// </summary>
        public void MoveIsExecuted()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
            NotAvailableToAvailable();
            IsTileEnabled = false;
        }
 


        /// <summary>
        /// Metod för att räkna ut varje spelares poäng. Metoden plussar på spelarens poäng med poängen från föregåenden omgång. 
        /// </summary>
        public void PointCounter()
        {

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus == Status.AvailableGameTile || tile.CurrentStatus == Status.NotAvailableGameTile)
                {
                    if (PlayerTurnCounter == 1)
                    {
                        Player1Point = Player1Point += tile.TileValue;
                        
                    }
                    else
                    {
                         Player2Point =  Player2Point += tile.TileValue;
                        
                    }
                }
            }
        }
        /// <summary>
        /// Metod som gör så att aktuell spelares namn och poäng markeras med en grön ruta i vyn
        /// </summary>
        public void SwitchPlayerTurn()
        {
            if (PlayerTurnCounter == 1)
            {
                TurnPlayer2 = Visibility.Visible;
                TurnPlayer1 = Visibility.Hidden;
                PlayerTurnCounter++;            
            }
            else
            {
                TurnPlayer1 = Visibility.Visible;
                TurnPlayer2 = Visibility.Hidden;
                PlayerTurnCounter = 1;
                GameRoundCounter++;

            }
            
        }
        /// <summary>
        /// Metod som gör bonusomgången synlig i vyn
        /// </summary>
        public void SwitchGameRoundVisibility()
        {
            GameRoundVisibility = Visibility.Hidden;
            BonusRoundVisibility = Visibility.Visible; 
            
        }

        /// <summary>
        /// Metod som avgör vilken typ av spelomgång det är, vanlig eller bonusomgång
        /// </summary>
        public void GameWinner()
        {
            if(BonusRoundVisibility == Visibility.Visible)
            {
                BonusGame();
            }
            else
            {
                WinnerOfGame();
            }
        }
        /// <summary>
        /// Metod som utser vinnaren i bonusomgång
        /// </summary>
        private void BonusGame()
        {   
            
            if (PlayerTurnCounter == 1)
            {
                if (Player1Point < 45)
                {
                    MessageBox.Show($"Nu är din bonustur slut. Du har {Player1Point} poäng. Det är nu {Player2Name}s tur");
                }
                else if (Player1Point >= 45)
                {
                    ForegroundBrushPlayer1 = Brushes.Red;
                    MessageBox.Show($"Du har fått {Player1Point} poäng. " +
                        $"Om {Player2Name} inte får fler poäng än du så förlorar du bonusomgången");
                }
            }
            
            else if (PlayerTurnCounter ==2 && Player1Point >= 45 && Player2Point >= 45)
            {
                if (Player1Point < Player2Point) { MessageBox.Show($"Grattis {Player1Name}, du har vunnit bonusomgången! Du fick {Player1Point} och {Player2Name} fick {Player2Point}."); }
                else { MessageBox.Show($"Grattis {Player2Name}, du har vunnit bonusomgången! Du fick {Player2Point} och {Player1Name} fick {Player1Point}."); }

                StartNewGame(); 
            }

            else 
            {
                if (Player2Point < 45 && Player1Point < 45)
                {
                    MessageBox.Show($"Nu är din bonustur slut. Du har {Player2Point} poäng.Det är nu {Player1Name}s tur");
                }
                else if (Player2Point < 45 && Player1Point >= 45)
                {
                    MessageBox.Show($"Grattis {Player2Name}, du har vunnit! Du fick {Player2Point} poäng och {Player1Name} fick {Player1Point} poäng.");
                    StartNewGame();
                }
                else if (Player1Point < 45 && Player2Point >= 45)
                {
                    MessageBox.Show($"Grattis {Player1Name}, du har vunnit bonusomgången! Du fick {Player1Point} poäng och {Player2Name} fick {Player2Point} poäng.");
                    StartNewGame();
                }
                
            }


        }
        /// <summary>
        /// Metod som utser vinnaren vanlig spelomgång alternativ ger övergång till en bonusomgång
        /// </summary>
        public void WinnerOfGame()
        {          
            
            if (PlayerTurnCounter == 1 && Player1Point < 45)
            {
                MessageBox.Show($"Nu är din tur slut. Du har {Player1Point} poäng. Det är nu {Player2Name}s tur");
            }

            else if (PlayerTurnCounter == 1 && Player1Point >= 45) 
            {
                ForegroundBrushPlayer1 = Brushes.Red;
                MessageBox.Show($"Du har fått {Player1Point} poäng. " +
                    $"Om {Player2Name} inte får 45 poäng eller mer så förlorar du");
            }

            else if (PlayerTurnCounter == 2 && Player2Point < 45 && Player1Point >= 45)
            {
                MessageBox.Show($"Grattis {Player2Name}, du har vunnit!");
                StartNewGame(); 
            }
            else if (PlayerTurnCounter == 2 && Player2Point < 45 && Player1Point < 45)
            {
                MessageBox.Show($"Nu är din tur slut. Du har {Player2Point} poäng.Det är nu {Player1Name}s tur");
            }
            else if (PlayerTurnCounter == 2 && Player1Point < 45 && Player2Point >= 45)
            {
                MessageBox.Show($"Grattis {Player1Name}, du har vunnit!");
                StartNewGame();

            }

            else if (PlayerTurnCounter == 2 && Player1Point >= 45 && Player2Point >= 45)
            {
                ForegroundBrushPlayer2 = Brushes.Red;
                MessageBoxResult result = MessageBox.Show("Spelet slutade lika då båda spelarna fick över 45 poäng i samma omgång, vill ni köra en bonusomgång?", "Oavgjort", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    StartBonusGame(); 
                    
                }
                else if (result == MessageBoxResult.No)
                {

                }
            }

        }

        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
        public void SetStatusOfGameTiles(List<int> sortedList)
        {
            int count = 0;
            foreach (Tile tile in GameTiles)
            {
                if (sortedList.Contains(tile.TileValue) && tile.CurrentStatus != Status.DownwardGameTile && tile.CurrentStatus != Status.SelectedGameTile)
                {
                    tile.CurrentStatus = Status.AvailableGameTile;
                }
                else if (!sortedList.Contains(tile.TileValue) && tile.CurrentStatus != Status.DownwardGameTile)
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                }
                if (tile.CurrentStatus == Status.AvailableGameTile || tile.CurrentStatus == Status.SelectedGameTile)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                PointCounter();
                GameWinner(); 
                SwitchPlayerTurn();
                SetNewGameTurn();
                
            }
     
        }



        /// <summary>
        /// Metod som undersöker vilka kombinationer av brickor som är
        /// möjliga för att nå tärningarnas summa
        /// </summary>
        /// 

        //TITTA HÄR!!!
        public void GetAvailableTiles()
        {
            List<int> tiles = new List<int>();

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus != Status.DownwardGameTile)
                {
                    tiles.Add(tile.TileValue);
                }
            }

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
                    foreach (int j in tiles)
                    {
                        if (i != j)
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
                                foreach (int k in tiles)
                                {
                                    if (i != k && j != k)
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
                                            foreach (int l in tiles)
                                            {
                                                if (i != j && i != k && i != l)
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
                    }
                }
            }
            Collection = collection;
            List<int> sortedList = SortOutDuplicates(Collection);
            SetStatusOfGameTiles(sortedList);
        }
        

            /// <summary>
            /// Tar bort alla listor från listan med listor som innehåller nedvända/otillgängliga värden
            /// </summary>
            private List<List<int>> SortOutDownWardTiles(List<List<int>>collection)
        {
            List<List<int>> updatedCollection = new List<List<int>>(collection);
            foreach (List<int> list in updatedCollection)
            {
                foreach (int i in list)
                {
                    if (GameTiles[i - 1].CurrentStatus == Status.DownwardGameTile)
                    {
                        updatedCollection.Remove(list);
                    }
                }
            }
            return updatedCollection;
        }

        /// <summary>
        /// Skickar ut en lista med tillåtna värden från listan med listor.
        /// Dubbletter av värden sorteras bort och listan sorteras i ordning, minst till störst
        /// </summary>
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
                //MessageBox.Show("Rätt");

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
                //MessageBox.Show("För lågt");
            }
            else if (calculatedSum > DiceValue)
            {
                //MessageBox.Show("För högt");
            }
        }
    }
        #endregion
        
       
    }

