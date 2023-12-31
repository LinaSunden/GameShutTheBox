﻿using SUP23_G4.Commands;
using SUP23_G4.Converters;
using SUP23_G4.Dto;
using SUP23_G4.Enums;
using SUP23_G4.Models;
using SUP23_G4.Models.Languages;
using SUP23_G4.Properties;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.GameComponents;
using SUP23_G4.Views.GameTiles;
using SUP23_G4.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

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
            EndGameCommand = new RelayCommand(x => EndGame());
            StartBonusRoundCommand = new RelayCommand(x => StartBonusRound());
            ViewGameRulesCommand = new RelayCommand(x => ViewGameRules());
            SoundEffectsCommand = new RelayCommand(x => SoundEffectsOnAndOff());
            StartRematchCommand = new RelayCommand(x => StartRematch());
            GameTiles = new ObservableCollection<Tile>();
            Dice = new ObservableCollection<Die>();
            PMButton = new();
            FillCollectionOfGameTiles();
            CreateDice();
            Player1Turn = Visibility.Visible;
            Player2Turn = Visibility.Hidden;
            SpeakerImage = "/Resources/Image/SpeakerButton.png";
            
        }

        #endregion
        #region Egenskaper
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public ObservableCollection<Tile> GameTiles { get; private set; }
        public ObservableCollection<Die> Dice { get; private set; }
        /// <summary>
        /// Collection används för att hålla olika sifferkombinationer.
        /// </summary>
        private List<List<int>> Collection { get; set; }
        public Brush Player1ForegroundBrush { get; set; } = Brushes.White;
        public Brush Player2ForegroundBrush { get; set; } = Brushes.White;
        public PlayerMessageButton PMButton { get; set; }
        public ICommand RollDiceCommand { get; }
        public ICommand TileClickedCommand { get; }
        public ICommand ExecuteMoveCommand { get; }
        public ICommand GoToStartCommand { get; }
        public ICommand SoundEffectsCommand { get; }
        public ICommand ViewGameRulesCommand { get; }
        public ICommand StartRematchCommand { get; }
        public ICommand VisibilityGameEndingCommand { get; }
        public ICommand EndGameCommand { get; }
        public ICommand StartBonusRoundCommand { get; }
        public Visibility ExecuteMove { get; set; } = Visibility.Collapsed;
        public Visibility Player1Turn { get; set; }
        public Visibility Player2Turn { get; set; }
        public Visibility SBTLogoVisibility { get; set; } = Visibility.Collapsed;
        public Visibility Player1LabelVisibility { get; set; } = Visibility.Collapsed;
        public Visibility Player2LabelVisibility { get; set; } = Visibility.Collapsed;
        public Visibility EndGameVisibility { get; set; } = Visibility.Collapsed;
        public Visibility BonusButtonVisibility { get; set; } = Visibility.Collapsed;
        public Visibility GameButtonsVisibility { get; set; } = Visibility.Collapsed;
        public Visibility GameRoundVisibility { get; set; } 
        public Visibility GameRulesSwedishVisibility { get; set; } = Visibility.Collapsed;
        public Visibility GameRulesEnglishVisibility {  get; set; } = Visibility.Collapsed;
        public Visibility DisplayDiceSumVisibility { get; set; } = Visibility.Visible;
        public Visibility BonusRoundVisibility { get; private set; } = Visibility.Collapsed;
        public Visibility TileValueVisibility { get; private set; } = Visibility.Collapsed;
        public Visibility MessageBoxVisibility { get; private set; } = Visibility.Collapsed;
        public Visibility Gif { get; set; } = Visibility.Collapsed;
        public int DiceSum { get; private set; }
        public int GameRoundCounter { get; private set; } = 1;
        public int PlayerTurnCounter { get; private set; } = 1;
        private int TargetPoints {  get; set; }
        public string SpeakerImage { get; set; }
        public string? DisplayDiceSum { get; set; }
        public bool IsSoundEffectsAllowed { get; private set; } = true;
        public bool IsThrowEnable { get; private set; } = true;

        #endregion

        #region Instansvariabler

        private SoundPlayer _closingTileSound = new SoundPlayer(Properties.Resources.ClosingTile);
        private SoundPlayer _diceTossSound = new SoundPlayer(Properties.Resources.dice_rolls_30cm);
        private List<List<int>> _collection;
        private static bool[,] _dp;
        #endregion

        #region Metoder

        #region Tiles   

        /// <summary>
        /// Skapar 10 tiles med värde 1-10 och ger dem status AvailableGameTile och lägger dem i en ObservableCollection som heter GameTiles.
        /// </summary>
        private void FillCollectionOfGameTiles()
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
        private void UpdateStatusOfChosenGameTile(Object x)
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
        private void ChangeStatusOfChosenTile(Tile tile)
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
        private bool IsTileNotAvailable()
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
        private void UpdateStatusOfAvailableTiles()
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
        private void NotAvailableToAvailable()
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
        private void SetStatusOfGameTiles(List<int> sortedList)
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
        /// Metod som fyller en tvådimensionell array med booleska värden. Det som styr true eller false
        /// är värdena på de tiles som är tillgängliga samt tärningarnas summa
        /// </summary>
        ///        
        private void Fill2DArray()
        {
            List<int> tiles = new List<int>();

            foreach (Tile tile in GameTiles)
            {
                if (tile.CurrentStatus != Status.DownwardGameTile)
                {
                    tiles.Add(tile.TileValue);
                }
            }
            int sum = DiceSum;
            int n = tiles.Count;

            _dp = new bool[n, sum + 1];

            for (int i = 0; i < n; i++)
            {
                _dp[i, 0] = true;
            }

            if (tiles[0] <= sum)
            {
                _dp[0, tiles[0]] = true;
            }

            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < sum + 1; j++)
                {
                    _dp[i, j] = (tiles[i] <= j) 
                        ? (_dp[i - 1, j] || _dp[i - 1, j - tiles[i]]) 
                        : _dp[i - 1, j];                 
                }
            }

            _collection = new List<List<int>>();
            List<int> p = new List<int>();
            TilesCombinationToList(tiles, n - 1, sum, p);
            Collection = _collection;
            List<int> sortedList = SortOutDuplicates(Collection);
            SetStatusOfGameTiles(sortedList);

        }
        /// <summary>
        /// Metod som tar emot en lista med värden som kan bli tärningarnas summa och lägger till den
        /// i en lista av listor
        /// </summary>
        private void FillListOfLists(List<int> v)
        {
            List<int> availableTiles = new List<int>();
            foreach (var i in v)
            {
                availableTiles.Add(i);
            }
            _collection.Add(availableTiles);
        }
        /// <summary>
        /// En rekursiv loop som kollar vilka värden av tiles som kan bli tärningarnas summa och lägger dessa
        /// i en lista samt skickar de vidare till en annan metod
        /// </summary>
        private void TilesCombinationToList(List<int> tiles, int i, int sum, List<int> p)
        {

            if (i == 0 && sum != 0 && _dp[0, sum])
            {
                p.Add(tiles[i]);
                FillListOfLists(p);
                p.Clear();
                return;
            }

            if (i == 0 && sum == 0)
            {
                FillListOfLists(p);
                p.Clear();
                return;
            }
            if (i == 0)
            {
                return;
            }

            if (_dp[i - 1, sum])
            {
                List<int> b = new List<int>();
                b.AddRange(p);
                TilesCombinationToList(tiles, i - 1, sum, b);
            }

            if (sum >= tiles[i] && _dp[i - 1, sum - tiles[i]])
            {
                p.Add(tiles[i]);
                TilesCombinationToList(tiles, i - 1, sum - tiles[i], p);
            }          
        }
        /// <summary>
        /// Metod som testar om spelarens valda tiles blir tärningarnas
        /// summa och sätter relevant status 
        /// </summary>
        private void CompareSelectedTilesWithDiceValue()
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
        /// <summary>
        /// Metod som kollapsar Messagebox och label så man inte ser dem
        /// </summary>
        public void VisibilityMessageBoxLabel()
        {
            MessageBoxVisibility = Visibility.Collapsed;
            Player1LabelVisibility = Visibility.Collapsed;
            Player2LabelVisibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Metod som gör tärningarna inte går att klicka samt visar "genomför drag knapp".
        /// </summary>
        private void VisibilityMakeMoveButton()
        {
            ExecuteMove = Visibility.Visible;
            IsThrowEnable = false;
        }
        /// <summary>
        /// Metod som gör att tärningarna går att klicka samt att "genomför drag knapp" ej är synlig.
        /// </summary>
        private void MakeDiceClickable()
        {
            ExecuteMove = Visibility.Hidden;
            IsThrowEnable = true;
        }
        /// <summary>
        /// Metod som gör bonusomgången synlig i vyn
        /// </summary>
        private void VisibilityBonusRound()
        {
            GameRoundVisibility = Visibility.Hidden;
            BonusRoundVisibility = Visibility.Visible;
        }
        /// <summary>
        /// Metod som gör så att aktuell spelares namn och poäng markeras med en grön ruta i vyn
        /// </summary>
        private void SwitchPlayerTurn()
        {
            bool IsPlayer2Turn = PlayerTurnCounter == 1;

            if (IsPlayer2Turn)
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
        /// <summary>
        /// Metod som gör att spelreglerna kan visas i GameView under tiden som spelet spelas
        /// Ändrar texten till Dölj spelregler när knappen har klickats en gång och på motsvarande sätt för varje språk
        /// </summary>
        public void ViewGameRules()
        {

            if (MainViewModel.Instance.GameLanguage == GameLanguage.Swedish)
            {

                if (MainViewModel.Instance.CurrentLanguage.GameRuleBtn == "Visa spelregler")
                {
                    GameRulesSwedishVisibility = Visibility.Visible;
                    MainViewModel.Instance.CurrentLanguage.GameRuleBtn = "Dölj spelregler";
                }

                else if (MainViewModel.Instance.CurrentLanguage.GameRuleBtn == "Dölj spelregler")
                {
                    GameRulesSwedishVisibility = Visibility.Hidden;
                    MainViewModel.Instance.CurrentLanguage.GameRuleBtn = "Visa spelregler";
                }



            }
            else if (MainViewModel.Instance.GameLanguage == GameLanguage.English)
            {

                if (MainViewModel.Instance.CurrentLanguage.GameRuleBtn == "Show game rules")
                {
                    GameRulesEnglishVisibility = Visibility.Visible;
                    MainViewModel.Instance.CurrentLanguage.GameRuleBtn = "Hide game rules";
                }
                else if (MainViewModel.Instance.CurrentLanguage.GameRuleBtn == "Hide game rules")
                {
                    GameRulesEnglishVisibility = Visibility.Hidden;
                    MainViewModel.Instance.CurrentLanguage.GameRuleBtn = "Show game rules";
                }

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
                SpeakerImage = "/Resources/Image/MutedSpeakerButton.png";
            }
            else if (!IsSoundEffectsAllowed)
            {
                IsSoundEffectsAllowed = true;
                SpeakerImage = "/Resources/Image/SpeakerButton.png";
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
        private void CreateDice()
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
        private void DiceToss()
        {
            VisibilityMessageBoxLabel();
            SBTLogoVisibility = Visibility.Collapsed;
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
            Fill2DArray();
        }
        /// <summary>
        /// Metod som sätter alla tiles som inte är notavailable till notavailable, används vid start av ny spelares tur
        /// </summary>
        private void NewGameTurn()
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
        private void StartBonusRound()
        {
            VisibilityMessageBoxLabel();
            VisibilityBonusRound();
            Player1.Score = 0;
            Player2.Score = 0;
            Player1ForegroundBrush = Brushes.White;
            Player2ForegroundBrush = Brushes.White;
            NewGameTurn();

        }
        /// <summary>
        /// Metod som ger vyn försättningar och utseende för att spela en vanlig spelomgång
        /// </summary>
        private void StartRematch() 
        {
            VisibilityMessageBoxLabel();
            NewGameTurn();
            Player1.Score = 0;
            Player2.Score = 0;
            Player1ForegroundBrush = Brushes.White;
            Player2ForegroundBrush = Brushes.White;
            GameRoundCounter = 1;
            BonusRoundVisibility = Visibility.Hidden;
            GameRoundVisibility = Visibility.Visible;
            MessageBoxVisibility = Visibility.Collapsed;
            GameButtonsVisibility = Visibility.Collapsed;
            BonusButtonVisibility = Visibility.Collapsed;
            Player1Turn = Visibility.Visible;
            Player2Turn = Visibility.Hidden;
            Gif = Visibility.Collapsed;

        }
        /// <summary>
        /// Metod som avslutar pågående match och går tillbaka till startsidan.
        /// </summary>
        private void EndGame()
        {
            MessageBoxVisibility = Visibility.Visible;
            MainViewModel.Instance.CurrentViewModel = new StartViewModel();
        }
        /// <summary>
        /// Metod som avslutar ett drag och gör spelet redo för ett nytt tärningskast.
        /// </summary>
        private void MoveIsExecuted()
        {
            if (IsAllTilesDownward())
            {
                SBTLogoVisibility = Visibility.Visible;
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
        private void ScoreCounter()
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
            bool IsPlayer1Turn = PlayerTurnCounter == 2;
            bool IsPlayer2Turn = PlayerTurnCounter == 1;
            bool IsPlayer1ScoreUnder56 = Player1.Score < 56;
            bool IsPlayer1Winner = Player1.Score < Player2.Score;
            bool IsPlayer2Winner = Player2.Score < Player1.Score;          

            if (IsPlayer1Turn)
            {
                if (IsPlayer1ScoreUnder56)
                {
                    MessageBoxVisibility = Visibility.Visible;
                    PMButton.CurrentMessage = MessageStatus.BonusGameTurn;
                    Player1LabelVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                }
            }
            else if (IsPlayer2Turn)
            {
                if (IsPlayer2Winner)
                {
                    Player1ForegroundBrush = Brushes.Red;
                    Player2ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxVisibility = Visibility.Visible;
                    GameButtonsVisibility = Visibility.Visible;
                    Gif = Visibility.Visible;
                    PMButton.CurrentMessage = MessageStatus.BonusGameWon2;
                    Player2LabelVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                }
                else if (IsPlayer1Winner)
                {
                    Player2ForegroundBrush = Brushes.Red;
                    Player1ForegroundBrush = Brushes.Goldenrod;
                    MessageBoxVisibility = Visibility.Visible;
                    GameButtonsVisibility = Visibility.Visible;
                    Gif = Visibility.Visible;
                    PMButton.CurrentMessage = MessageStatus.BonusGameWon1;
                    Player1LabelVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                }
            }
        }
        /// <summary>
        /// Metod som utser vinnaren vanlig spelomgång alternativ ger övergång till en bonusomgång
        /// </summary>
        private void WinnerOfGame()
        {
            bool IsPlayer1Turn = PlayerTurnCounter == 2;
            bool IsPlayer2Turn = PlayerTurnCounter == 1;
            bool IsPlayer1TurnOver = Player1.Score < TargetPoints;
            bool IsPlayer1Over45 = Player1.Score >= TargetPoints;
            bool IsPlayer2Winner = Player1.Score > Player2.Score && Player1.Score >= TargetPoints;
            bool IsPlayer1Winner = Player2.Score > Player1.Score && Player2.Score >= TargetPoints;
            bool IsPlayer2TurnDone = Player2.Score < TargetPoints && Player1.Score < TargetPoints;
            bool IsBonusGame = Player1.Score == Player2.Score && Player1.Score >= TargetPoints && Player2.Score >= TargetPoints; 

            if (IsPlayer1Turn)
            {
                if (IsPlayer1TurnOver) 
                {
                    Player1LabelVisibility = Visibility.Visible;
                    MessageBoxVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                    PMButton.CurrentMessage = MessageStatus.Player1Turn;
                }
                else if (IsPlayer1Over45) 
                {
                    Player1ForegroundBrush = Brushes.Red;
                    Player1LabelVisibility = Visibility.Visible;
                    MessageBoxVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                    PMButton.CurrentMessage = MessageStatus.Over45Player1;
                }
            }
            else if (IsPlayer2Turn)
            {
                if (IsPlayer2Winner) 
                { 
                    Player1ForegroundBrush = Brushes.Red;
                    Player2ForegroundBrush = Brushes.Goldenrod;
                    Player2LabelVisibility = Visibility.Visible;
                    GameButtonsVisibility = Visibility.Visible;
                    Gif = Visibility.Visible;
                    MessageBoxVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                    PMButton.CurrentMessage = MessageStatus.Player2Winner;
                }

                if (IsPlayer1Winner) 
                {
                    Player2ForegroundBrush = Brushes.Red;
                    Player1ForegroundBrush = Brushes.Goldenrod;
                    Player1LabelVisibility = Visibility.Visible;
                    GameButtonsVisibility = Visibility.Visible;
                    Gif = Visibility.Visible;
                    MessageBoxVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                    PMButton.CurrentMessage = MessageStatus.Player1Winner;
                }

                else if (IsPlayer2TurnDone) 
                {
                    Player2LabelVisibility = Visibility.Visible;
                    MessageBoxVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                    PMButton.CurrentMessage = MessageStatus.Player2Turn;
                    
                }

                else if (IsBonusGame)
                {
                    Player2ForegroundBrush = Brushes.Red;
                    MessageBoxVisibility = Visibility.Visible;
                    PMButton.CurrentMessage = MessageStatus.BonusGame;
                    BonusRoundVisibility = Visibility.Visible;
                    BonusButtonVisibility = Visibility.Visible;
                    GameButtonsVisibility = Visibility.Visible;
                    DisplayDiceSumVisibility = Visibility.Hidden; 
                }
        }
    
        }
        


        #endregion
    }

}

