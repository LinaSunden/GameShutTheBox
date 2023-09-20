using SUP23_G4.Commands;
using SUP23_G4.Converters;
using SUP23_G4.Dto;
using SUP23_G4.Enums;
using SUP23_G4.Languages;
using SUP23_G4.Models;
using SUP23_G4.Properties;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using SUP23_G4.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {


        #region Konstruktor

     
        public GameViewModel(PlayerSettingsDto SettingsDto)
        {

           
            Player1 = SettingsDto.Player1;
            Player2 = SettingsDto.Player2;
            GameTiles = new ObservableCollection<Tile>();
            Languages = new ObservableCollection<Language>();
            Languages = GetLanguages();
            FillCollectionOfGameTiles();
            RollDiceCommand = new RelayCommand(x => DiceToss());
            ExecuteMoveCommand = new RelayCommand(x => CompareSelectedTilesWithDiceValue());
            NewSelectedTileCommand = new RelayCommand(x => UpdateStatusOfChosenGameTileInObservableCollection(x));
            PointCounterCommand = new RelayCommand(x => PointCounter());
            ShowGameRulesCommand = new RelayCommand(x => ViewGameRules());
            TestBonusGame = new RelayCommand(x => StartBonusGame()); //TODO: Ta bort commando när vi har testat klart bonusomgång
            MuteSoundEffects = new RelayCommand(x => SoundEffectsOnAndOff());
            TurnPlayer1 = Visibility.Visible;
            TurnPlayer2 = Visibility.Hidden;
           
            ImageSource = "/Resources/SpeakerButton.png";
        }

        public GameViewModel()
        {

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

        public ICommand ShowGameRulesCommand { get; }
        public ICommand TestBonusGame { get; set; } //TODO: Ta bort commando när vi har testat klart bonusomgång

        public PlayerSettingsDto SettingsDto;
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public ObservableCollection<Language> Languages { get; set; }

        public ObservableCollection<Tile> GameTiles { set; get; }

        public Visibility ExecuteMove { get; set; } = Visibility.Hidden;

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

        //public bool IsTileEnabled { get; set; } = false;

        public Tile tile = new Tile();

        public Brush ForegroundBrushPlayer1 { get; set; } = Brushes.White;

        public Brush ForegroundBrushPlayer2 { get; set; } = Brushes.White;

        private List<List<int>> Collection { get; set; }

        public Visibility GameRuleVisibility { get; set; } = Visibility.Hidden;

        public string? DisplayDiceSum { get; set; }

        public Visibility DisplayDiceSumVisibility { get; set; } = Visibility.Visible;
       
        public ICommand MuteSoundEffects { get; set; }

        public bool SoundEffectsAllowed { get; set; } = true;

        public SoundPlayer closingTileSound = new SoundPlayer(Properties.Resources.ClosingTile);

        public SoundPlayer diceTossSound = new SoundPlayer(Properties.Resources.dice_rolls_30cm);

        public int CboSelectedIndex { get; set; } = 0;

        public string ImageSource { get; set; }

        #endregion





        #region Instansvariabler

        public StartViewModel _startViewModel;

        #endregion




        #region Metoder
        /// <summary>
        /// Skapar 10 tiles med värde 1-10 och ger dem status AvailableGameTile och lägger dem i en ObservableCollection som heter GameTiles.
        /// OBS! Ändrar status från avaiableGameTiles till Notavailabe för att testa TilesBeforeDiceToss
        /// </summary>
        public void FillCollectionOfGameTiles()
        {

            Tile tile;
            for (int i = 1; i <= 10; i++)
            {
                tile = new Tile();
                {
                    tile.TileValue = i;
                    tile.CurrentStatus = Status.NotAvailableGameTile;
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
            DiceTossSound();
            //IsTileEnabled = true;
            DisplayDiceSum = $"= {DiceValue}";
            DisplayDiceSumVisibility = Visibility.Visible;
            GetAvailableTiles();
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
            if(TilesBeforeDiceToss() == true)
            {
                UpdateStatusOfAvailableTiles();
            }
            
        }

        public bool TilesBeforeDiceToss()
        {
            foreach(Tile t in GameTiles)
            {
                if(t.CurrentStatus != Status.NotAvailableGameTile)
                {
                    return true; 
                }
            }
            return false;
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
        /// OBS!! Ändrar status från AvailableGameTile till NotAvaible för att testa metoden TilesBeforeDiceToss
        /// </summary>
        public void SetNewGameTurn()
        {

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus != Status.NotAvailableGameTile)
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                }
            }
            //IsTileEnabled = false; 
            VisibilityDiceButton();

        }
        /// <summary>
        /// Ger vyn förutsättningar och utseende för att spela en bonusomgång
        /// </summary>
        public void StartBonusGame()
        {
            Player1.Score = 0;
            Player2.Score = 0;
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
            Player1.Score = 0;
            Player2.Score = 0;
            ForegroundBrushPlayer1 = Brushes.White;
            ForegroundBrushPlayer2 = Brushes.White;
            GameRoundCounter = 0;
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
            //IsTileEnabled = false;
            DisplayDiceSumVisibility = Visibility.Hidden;
            ClosingTileSound();
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
                        Player1.Score = Player1.Score += tile.TileValue;
                        
                    }
                    else
                    {
                         Player2.Score = Player2.Score += tile.TileValue;
                        
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
                MessageBox.Show($"Nu är din bonustur slut. Du har {Player1.Score} poäng. Det är nu {Player2.Name}s tur");

            }
            else
            {
                if(Player1.Score < Player2.Score) { MessageBox.Show($"Grattis {Player1.Name}, du har vunnit bonusomgången! Du fick {Player1.Score} och {Player2.Name} fick {Player2.Score}."); }
                else { MessageBox.Show($"Grattis {Player1.Name}, du har vunnit bonusomgången! Du fick {Player2.Score} och {Player1.Name} fick {Player1.Score}."); }

                StartNewGame();
            }
      

        }
        /// <summary>
        /// Metod som utser vinnaren vanlig spelomgång alternativ ger övergång till en bonusomgång
        /// </summary>
        public void WinnerOfGame()
        {

            if (PlayerTurnCounter == 1)
            {
                if (Player1.Score < 45)
                {

                    MessageBox.Show($"Nu är din tur slut. Du har {Player1.Score} poäng. Det är nu {Player2.Name}s tur");

                }
                else if (Player1.Score >= 45)
                {
                    ForegroundBrushPlayer1 = Brushes.Red;
                    MessageBox.Show($"Du har fått {Player1.Score} poäng. Om {Player2.Name} inte får mer poäng än dig så förlorar du");
                }

            }

            else if(PlayerTurnCounter == 2)
            {
                if (Player1.Score < 45 && Player2.Score < 45)
                {
                    MessageBox.Show($"Nu är din tur slut. Du har {Player2.Score} poäng. Det är nu {Player1.Name}s tur");
                }

                else if(Player1.Score < Player2.Score && Player2.Score >= 45)
                {
                    MessageBox.Show($"Grattis {Player1.Name}, du har vunnit!");
                    StartNewGame();
                }
                else if (Player1.Score >= 45 && Player1.Score > Player2.Score)
                {
                    MessageBox.Show($"Grattis {Player2.Name}, du har vunnit!");
                    StartNewGame();
                }
                else if(Player1.Score == Player2.Score && Player1.Score >=45 && Player2.Score >= 45)
                {
                    ForegroundBrushPlayer2 = Brushes.Red;
                    MessageBoxResult result = MessageBox.Show("Spelet slutade lika då båda spelarna fick samma poäng, vill ni köra en bonusomgång?", "Oavgjort", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartBonusGame();
                    }
                    else if (result == MessageBoxResult.No)
                    {

                    }
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
        private List<List<int>> SortOutDownWardTiles(List<List<int>> collection)
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

        /// <summary>
        /// Metod som gör att spelreglerna kan visas i GameView under tiden som spelet spelas
        /// Ändrar texten till Dölj spelregler när knappen har klickats en gång och på motsvarande sätt för varje språk
        /// </summary>
        public void ViewGameRules()
        {

            if (CboSelectedIndex == 0)
            {
                foreach (Language language in Languages)
                {
                    if (language.GameRuleBtn == "Visa spelregler")
                    {
                        GameRuleVisibility = Visibility.Visible;
                        language.GameRuleBtn = "Dölj spelregler";
                    }

                    else if (language.GameRuleBtn == "Dölj spelregler")
                    {
                        GameRuleVisibility = Visibility.Hidden;
                        language.GameRuleBtn = "Visa spelregler";

                    }
                }
            }

            else if (CboSelectedIndex == 1)
            {
                foreach (Language language in Languages)
                {
                    if (language.GameRuleBtn == "Show game rules")
                    {
                        GameRuleVisibility = Visibility.Visible;
                        language.GameRuleBtn = "Hide game rules";
                    }
                    else if (language.GameRuleBtn == "Hide game rules")
                    {
                        GameRuleVisibility = Visibility.Hidden;
                        language.GameRuleBtn = "Show game rules";
                    }
                }
            }
          

        
        }


            public static ObservableCollection<Language> GetLanguages()
            {
                var languages = new ObservableCollection<Language>()
            {new Language(){PlayerName1="Spelare 1: ",
                            PlayerName2="Spelare 2: ",
                            Points="Poäng: ",
                            Round="Omgång: ",
                            MyTurn="Din tur",
                            GameRuleBtn="Visa spelregler",
                            ThrowDiceBtn="Kasta tärningar",
                            MakeMove="Genomför drag",
                            LanguageName ="Svenska",
                            Flag= "/Resources/SwedenFlag.png",
                            SelectLanguage="Välj språk"},

            new Language(){ PlayerName1="Player 1: ",
                            PlayerName2="Player 2: ",
                            Points="Points: ",
                            Round="Round: ",
                            MyTurn="My turn",
                            GameRuleBtn="Show game rules",
                            ThrowDiceBtn="Throw dice",
                            MakeMove="Make move",
                            LanguageName="English",
                            Flag= "/Resources/GreatBritainFlag.png",
                            SelectLanguage="Select Language"},
           };
                return languages;
            }

        private void SoundEffectsOnAndOff()
        {
            if (SoundEffectsAllowed)
            {
                SoundEffectsAllowed = false;
                ImageSource = "/Resources/MutedSpeakerButton.png";
            }
            else if (!SoundEffectsAllowed)
            {
                SoundEffectsAllowed = true;
                ImageSource = "/Resources/SpeakerButton.png";
            }
        }
        private void DiceTossSound()
        {
            if (SoundEffectsAllowed)
            {
                diceTossSound.Play();
            }
        }
        private void ClosingTileSound()
        {
            if (SoundEffectsAllowed)
            {
                closingTileSound.Play();
            }
        }
    }


        #endregion


    } 

