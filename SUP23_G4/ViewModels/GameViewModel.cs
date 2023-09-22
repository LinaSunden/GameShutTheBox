using SUP23_G4.Commands;
using SUP23_G4.Converters;
using SUP23_G4.Dto;
using SUP23_G4.Enums;
using SUP23_G4.Languages;
using SUP23_G4.Models;
using SUP23_G4.Properties;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.GameComponents;
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
using System.Security.RightsManagement;
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
            TargetPoints = SettingsDto.TargetPoints;
            RollDiceCommand = new RelayCommand(x => DiceToss());
            ExecuteMoveCommand = new RelayCommand(x => CompareSelectedTilesWithDiceValue());
            TileClickedCommand = new RelayCommand(x => UpdateStatusOfChosenGameTile(x));
            ViewGameRulesCommand = new RelayCommand(x => ViewGameRules());
            SoundEffectsCommand = new RelayCommand(x => SoundEffectsOnAndOff());
            GameTiles = new ObservableCollection<Tile>();
            Languages = new ObservableCollection<Language>();
            Dice = new ObservableCollection<Die>();
            TestBonusGame = new RelayCommand(x => StartBonusRound()); //TODO: Ta bort commando när vi har testat klart bonusomgång
            Languages = GetLanguages();
            FillCollectionOfGameTiles();
            CreateDice();
            Player1Turn = Visibility.Visible;
            Player2Turn = Visibility.Hidden;
            SpeakerImage = "/Resources/SpeakerButton.png";

        }
        public GameViewModel()
        {

        }
        #endregion
        #region Egenskaper
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public ObservableCollection<Language> Languages { get; set; }
        public ObservableCollection<Tile> GameTiles { set; get; }
        public ObservableCollection<Die> Dice { set; get; }
        
        /// <summary>
        /// Collection används för att hålla olika sifferkombinationer.
        /// </summary>
        private List<List<int>> Collection { get; set; }
        public Brush Player1ForegroundBrush { get; set; } = Brushes.White;
        public Brush Player2ForegroundBrush { get; set; } = Brushes.White;
        public ICommand RollDiceCommand { get; }
        public ICommand TileClickedCommand { get; set; }
        public ICommand ExecuteMoveCommand { get; }
        public ICommand GoToStartCommand { get; }
        public ICommand SoundEffectsCommand { get; set; }
        public ICommand ViewGameRulesCommand { get; }
        public ICommand TestBonusGame { get; set; } //TODO: Ta bort commando när vi har testat klart bonusomgång}
        public Visibility ExecuteMove { get; set; } = Visibility.Hidden;
        public Visibility Player1Turn { get; set; }
        public Visibility Player2Turn { get; set; }
        public Visibility GameRoundVisibility { get; set; }
        public Visibility GameRuleVisibility { get; set; } = Visibility.Hidden;
        public Visibility DisplayDiceSumVisibility { get; set; } = Visibility.Visible;
        public Visibility BonusRoundVisibility { get; set; } = Visibility.Hidden;
        public Visibility TileValueVisibility { get; set; } = Visibility.Hidden;
        public int DiceSum { get; private set; }
        public int GameRoundCounter { get; set; } = 1;
        public int PlayerTurnCounter { get; set; } = 1;
        public int CboSelectedIndex { get; set; } = 0;
        private int TargetPoints {  get; set; }
        public string SpeakerImage { get; set; }
        public string? DisplayDiceSum { get; set; }
        public bool IsSoundEffectsAllowed { get; set; } = true;
        public bool IsThrowEnable { get; set; } = true;
        #endregion

        #region Instansvariabler

        private SoundPlayer _closingTileSound = new SoundPlayer(Properties.Resources.ClosingTile);
        private SoundPlayer _diceTossSound = new SoundPlayer(Properties.Resources.dice_rolls_30cm);
        #endregion

        #region Metoder

        #region Tiles   

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
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                };
                GameTiles.Add(tile);
            }
        }
        /// <summary>
        /// Ändrar status på vald tile från view
        /// </summary>
        public void UpdateStatusOfChosenGameTile(Object x)
        {
            if (DiceSum == 0)
            {
                return;
            }

            var tile = (Tile)x;
            ChangeStatusOfChosenTile(tile);


            foreach (Tile t in GameTiles)
            {
                if (tile.TileValue == t.TileValue)
                {
                    t.CurrentStatus = tile.CurrentStatus;

                }
            }
            if (IsTileNotAvailable())
            {
                UpdateStatusOfAvailableTiles();
            }

        }
        /// <summary>
        /// Ändrar status på vald tile i kollektionen av tiles
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
        /// Metod som används för att sätta rätt status på en tile inför ett tärningskast.
        /// </summary>
        public bool IsTileNotAvailable()
        {
            foreach (Tile t in GameTiles)
            {
                if (t.CurrentStatus != Status.NotAvailableGameTile)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Metod som tar bort alla sifferkombinationer som inte överenstämmer med vald siffra.
        /// </summary>
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
        /// Metod som ändrar status på ospelade tiles till Available.
        /// </summary>
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
        /// Metod som räknar ut vilka tiles som är tillgängliga utifrån 
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
                ScoreCounter();
                NewGameTurn();
                SwitchPlayerTurn();
                if (BonusRoundVisibility == Visibility.Visible) { BonusGame(); } else WinnerOfGame();
            }


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
        /// Metod som undersöker vilka kombinationer av tiles som är
        /// möjliga för att nå tärningarnas summa
        /// </summary>
        /// 
        public void FindAvailableTiles()
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
                if (i == DiceSum)
                {
                    availableTiles = new List<int>()
                    {
                        i,
                    };
                    collection.Add(availableTiles);
                    break;
                }
                else if (i < DiceSum)
                {
                    foreach (int j in tiles)
                    {
                        if (i < j)
                        {
                            if (i + j == DiceSum)
                            {
                                availableTiles = new List<int>()
                            {
                                i,
                                j,
                            };
                                collection.Add(availableTiles);
                                break;
                            }
                            else if (i + j < DiceSum)
                            {
                                foreach (int k in tiles)
                                {
                                    if (j < k)
                                    {
                                        if (i + j + k == DiceSum)
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
                                        else if (i + j + k < DiceSum)
                                        {
                                            foreach (int l in tiles)
                                            {
                                                if (k < l)
                                                {
                                                    if (i + j + k + l == DiceSum)
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
        /// Metod som testar om spelarens valda tiles blir tärningarnas
        /// summa och sätter relevant status 
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

            if (calculatedSum == DiceSum)
            {
                foreach (Tile tile in GameTiles)
                {
                    if (tile.CurrentStatus == Status.SelectedGameTile)
                    {
                        tile.CurrentStatus = Status.DownwardGameTile;
                    }
                }
                MoveIsExecuted();
            }
            else if (calculatedSum < DiceSum)
            {
               TileValueVisibility = Visibility.Visible;    
            }
        }
        /// <summary>
        /// Metod som kollar om alla tiles är nedvända innan tärningen kastas
        /// </summary>
        private bool IsAllTilesDownward()
        {
            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus != Status.DownwardGameTile)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Visibility

        public void VisibilityGameEnding()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = false;
        }
        /// <summary>
        /// Metod som gör tärningarna inte går att klicka samt visar "genomför drag knapp".
        /// </summary>
        public void VisibilityMakeMoveButton()
        {
            ExecuteMove = Visibility.Visible;
            IsThrowEnable = false;
        }
        /// <summary>
        /// Metod som gör att tärningarna går att klicka samt att "genomför drag knapp" ej är synlig.
        /// </summary>
        public void MakeDiceClickable()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
        }
        /// <summary>
        /// Metod som gör bonusomgången synlig i vyn
        /// </summary>
        public void VisibilityBonusRound()
        {
            GameRoundVisibility = Visibility.Hidden;
            BonusRoundVisibility = Visibility.Visible;
        }
        /// <summary>
        /// Metod som gör så att aktuell spelares namn och poäng markeras med en grön ruta i vyn
        /// </summary>
        public void SwitchPlayerTurn()
        {
            if (PlayerTurnCounter == 1)
            {
                Player2Turn = Visibility.Visible;
                Player1Turn = Visibility.Hidden;
                PlayerTurnCounter++;
            }
            else
            {
                Player1Turn = Visibility.Visible;
                Player2Turn = Visibility.Hidden;
                PlayerTurnCounter = 1;
                GameRoundCounter++;

            }

        }

        #endregion

        #region Sound
        /// <summary>
        /// En knapp för att muta och starta ljudeffekter i Gameview samt byta bild.
        /// </summary>
        private void SoundEffectsOnAndOff()
        {
            if (IsSoundEffectsAllowed)
            {
                IsSoundEffectsAllowed = false;
                SpeakerImage = "/Resources/MutedSpeakerButton.png";
            }
            else if (!IsSoundEffectsAllowed)
            {
                IsSoundEffectsAllowed = true;
                SpeakerImage = "/Resources/SpeakerButton.png";
            }
        }
        /// <summary>
        /// Ljud för tärningskast
        /// </summary>
        private void DiceTossSound()
        {
            if (IsSoundEffectsAllowed)
            {
                _diceTossSound.Play();
            }
        }
        /// <summary>
        /// Ljud för nedfällning av tile.
        /// </summary>
        private void ClosingTileSound()
        {
            if (IsSoundEffectsAllowed)
            {
                _closingTileSound.Play();
            }
        }
        #endregion





        /// <summary>
        /// Skapar två tärningar och sätter fasta värden när spelet startas 
        /// </summary>
        public void CreateDice()
        {
            
            for (int i = 0; i < 2; i++)
            {
                Die die = new Die();
                if (i == 0)
                {
                    die.DieValue = 3;
                    Dice.Add(die);
                }

                else if (i == 1)
                {
                    die.DieValue = 5;
                    Dice.Add(die);
                }

                
            }
           

        }


        /// <summary>
        /// Kastar två tärningar och får uppdaterade värden på DieOne och DieTwo
        /// </summary>
        public void DiceToss()
        {
           
            DiceSum = 0;

            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                
                Die die = new Die();
                if (i == 0)
                {
                    die.DieValue = r.Next(1, 7);
                    DiceSum += die.DieValue;
                    Dice[0]=die;         
                }
                else if (i == 1)
                {
                    die.DieValue = r.Next(1, 7);
                    DiceSum += die.DieValue;
                    Dice[1]=die;
                }
            }

            VisibilityMakeMoveButton();
            DiceTossSound();
            DisplayDiceSum = $"= {DiceSum}";
            DisplayDiceSumVisibility = Visibility.Visible;
            FindAvailableTiles();
        }
        /// <summary>
        /// Metod som sätter alla tiles som inte är notavailable till notavailable, används vid start av ny spelares tur
        /// </summary>
        public void NewGameTurn()
        {

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus != Status.NotAvailableGameTile)
                {
                    tile.CurrentStatus = Status.NotAvailableGameTile;
                }
            }

            MakeDiceClickable();

        }
        /// <summary>
        /// Ger vyn förutsättningar och utseende för att spela en bonusomgång
        /// </summary>
        public void StartBonusRound()
        {
            Player1.Score = 0;
            Player2.Score = 0;
            Player1ForegroundBrush = Brushes.White;
            Player2ForegroundBrush = Brushes.White;
            VisibilityBonusRound();
            NewGameTurn();

        }
        /// <summary>
        /// Metod som ger vyn försättningar och utseende för att spela en vanlig spelomgång
        /// </summary>
        public void StartRematch()  // Kopplas till messagebox som ska avgöra om du vill göra en rematch med samma spelare eller gå ur spelet till startsida.
        {
            NewGameTurn();
            Player1.Score = 0;
            Player2.Score = 0;
            Player1ForegroundBrush = Brushes.White;
            Player2ForegroundBrush = Brushes.White;
            GameRoundCounter = 0;
            BonusRoundVisibility = Visibility.Hidden;
            GameRoundVisibility = Visibility.Visible;
            Player1Turn = Visibility.Visible;
            Player2Turn = Visibility.Hidden;

        }
        /// <summary>
        /// Metod som avslutar ett drag och gör spelet redo för ett nytt tärningskast.
        /// </summary>
        public void MoveIsExecuted()
        {
            if (IsAllTilesDownward())
            {
                ClosingTileSound();
                ScoreCounter();
                WinnerOfGame();
                NewGameTurn();
                SwitchPlayerTurn();
            }
            else
            {
                ExecuteMove = Visibility.Hidden;
                DisplayDiceSumVisibility = Visibility.Hidden;
                TileValueVisibility = Visibility.Hidden;
                IsThrowEnable = true;
                DiceSum = 0;
                NotAvailableToAvailable();
                ClosingTileSound();
            }            
        }

        /// <summary>
        /// Metod för att räkna ut varje spelares poäng. Metoden plussar på spelarens poäng med poängen från föregåenden tur. 
        /// </summary>
        public void ScoreCounter()
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
        /// Metod som utser vinnaren i bonusomgång
        /// </summary>
        private void BonusGame()
        {

            if (PlayerTurnCounter == 2)
            {
                if (Player1.Score < 56)
                {
                    MessageBox.Show($"Nu är din bonustur slut. Du har {Player1.Score} poäng. Det är nu {Player2.Name}s tur");
                }
            }
            else if (PlayerTurnCounter == 1)
            {
                if (Player2.Score < Player1.Score)
                {
                    Player1ForegroundBrush = Brushes.Red;
                    Player2ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxResult result = MessageBox.Show($"Grattis {Player2.Name}, du har vunnit bonusomgången och spelet! Vill du köra en rematch?", "Grattis!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartRematch();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VisibilityGameEnding();
                    }

                }
                else if (Player1.Score < Player2.Score)
                {
                    Player2ForegroundBrush = Brushes.Red;
                    Player1ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxResult result = MessageBox.Show($"Grattis {Player1.Name}, du har vunnit bonusomgångeno ch spelet! Vill du köra en rematch?", "Grattis!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartRematch();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VisibilityGameEnding();
                    }
                }
            }
        }
        /// <summary>
        /// Metod som utser vinnaren vanlig spelomgång alternativ ger övergång till en bonusomgång
        /// </summary>
        public void WinnerOfGame()
        {
            if (PlayerTurnCounter == 2)
            {
                if (Player1.Score < TargetPoints)
                {
                    MessageBox.Show($"Nu är din tur slut. Du har {Player1.Score} poäng. Det är nu {Player2.Name}s tur");
                }
                else if (Player1.Score >= TargetPoints)
                {
                    Player1ForegroundBrush = Brushes.Red;
                    MessageBox.Show($"Du har fått {Player1.Score} poäng. Om {Player2.Name} inte får mer poäng än dig så förlorar du");
                }
            }
            else if (PlayerTurnCounter == 1)
            {
                if (Player1.Score > Player2.Score && Player1.Score >= TargetPoints)
                { 
                    Player1ForegroundBrush = Brushes.Red;
                    Player2ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxResult result = MessageBox.Show($"Grattis {Player2.Name}, du har vunnit! Vill du köra en rematch?", "Grattis!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartRematch();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VisibilityGameEnding();
                    }
                }

                if (Player2.Score > Player1.Score && Player2.Score >= TargetPoints)
                {
                    Player2ForegroundBrush = Brushes.Red;
                    Player1ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxResult result = MessageBox.Show($"Grattis {Player1.Name}, du har vunnit! Vill du köra en rematch?", "Grattis!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartRematch();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VisibilityGameEnding();
                    }
                }

                else if (Player2.Score < TargetPoints && Player1.Score < TargetPoints)
                {
                    MessageBox.Show($"Nu är din tur slut. Du har {Player2.Score} poäng. Det är nu {Player1.Name}s tur");
                }

                else if (Player1.Score == Player2.Score && Player1.Score >= TargetPoints && Player2.Score >= TargetPoints)
                {
                    Player2ForegroundBrush = Brushes.Red;
                    MessageBoxResult result = MessageBox.Show("Spelet slutade lika då båda spelarna fick samma poäng, vill ni köra en bonusomgång?", "Oavgjort", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StartBonusRound();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        VisibilityGameEnding();
                    }
                }
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
        /// <summary>
        /// Metod som en kollektion med språkalternativ.
        /// </summary>
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

        #endregion
    }

}

