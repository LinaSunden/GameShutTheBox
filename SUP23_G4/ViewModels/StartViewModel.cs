using SUP23_G4.Commands;
using SUP23_G4.Dto;
using SUP23_G4.FileHandler;
using SUP23_G4.Models;
using SUP23_G4.Models.Languages;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.GameComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace SUP23_G4.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        #region Konstruktor

        public StartViewModel()
        {

            //Language = new();
            StartScreenMusic = new SoundPlayer(Properties.Resources.StartViewMusic);
            StartScreenMusic.Play();
            IsMusicPlaying = true;
            SetUpGameCommand = new RelayCommand(S => SetupGame());
            SpeakerImage = "/Resources/Image/SpeakerButton.png";
            MuteMusicCommand = new RelayCommand(x => MuteStartMusic());


        }

        #endregion

        #region Egenskaper
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public PlayerSettingsDto SettingsDto;
        //public Language Language {get; set;}  
        public SoundPlayer StartScreenMusic;
        public ICommand MuteMusicCommand { get; set; }
        public ICommand SetUpGameCommand { get; }
        public int TargetPoints { get; set; } = 45;
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string SpeakerImage { get; set; }
        public bool IsMusicPlaying { get; set; }
        #endregion

        
        #region Metoder
        /// <summary>
        /// Stoppar startmusiken, lägger till spelarnamn i Dton och skickar med den till GameViewModel
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SetupGame()
        {
            StartScreenMusic.Stop();
            CreatePlayers();
            MainViewModel.Instance.StartGameCommand.Execute(SettingsDto);
            MainViewModel.Instance.CurrentViewModel = new GameViewModel(SettingsDto); 
          
        }
        /// <summary>
        /// Skapar 2 spelare samt en Dto som lagrar dessa.
        /// </summary>
        private void CreatePlayers()
        {
            Player1 = new Player(Player1Name);
            Player2 = new Player(Player2Name);
            SettingsDto = new PlayerSettingsDto(Player1, Player2, TargetPoints);
        }

        ///// <summary>
        ///// En knapp för att muta och starta Startview musik samt byta bild.
        ///// </summary>
        private void MuteStartMusic()
        {
            if (IsMusicPlaying)
            {
                StartScreenMusic.Stop();
                IsMusicPlaying = false;
                SpeakerImage = "/Resources/Image/MutedSpeakerButton.png";
            }
            else if (!IsMusicPlaying)
            {
                StartScreenMusic.Play();
                IsMusicPlaying = true;
                SpeakerImage = "/Resources/Image/SpeakerButton.png";
            }
        }


        #endregion













    }
}
