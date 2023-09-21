using SUP23_G4.Commands;
using SUP23_G4.Dto;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        #region Konstruktor

        public StartViewModel(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            StartGameCommand = new RelayCommand(x => StartGame());
            GameRulesCommand = new RelayCommand(x => GoToGameRules());
            MuteMusicCommand = new RelayCommand(x => MuteStartMusic());
            StartScreenMusic.Play();
            IsMusicPlaying = true;
            SpeakerImage = "/Resources/SpeakerButton.png";
        }
        public StartViewModel()
        {

        }
        #endregion
        #region Egenskaper
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
    
        public PlayerSettingsDto SettingsDto;

        public SoundPlayer StartScreenMusic = new SoundPlayer(Properties.Resources.StartViewMusic);
        public ICommand StartGameCommand { get; set; }
        public ICommand GameRulesCommand { get; set; }
        public ICommand MuteMusicCommand { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string SpeakerImage { get; set; }
        public int CboSelectedIndex { get; set; } = 0;
        public bool IsMusicPlaying { get; set; }
        #endregion

        #region Instansvariabler

        private MainViewModel _mainViewModel;

        #endregion
        #region Metoder
        /// <summary>
        /// Byter CurrentViewModel från att visa Startview till att visa Gameview
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void StartGame()
        {
            StartScreenMusic.Stop();
            CreatePlayer();
            _mainViewModel.CurrentViewModel = new GameViewModel(SettingsDto);
        }
        /// <summary>
        /// Skapar 2 spelare samt en Dto som lagrar dessa.
        /// </summary>
        private void CreatePlayer()
        {
            Player1 = new Player(Player1Name);
            Player2 = new Player(Player2Name);
            SettingsDto = new PlayerSettingsDto(Player1, Player2);
        }
        /// <summary>
        /// Byter CurrentViewModel från att visa Startview till att visa GameRules
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void GoToGameRules()
        {
            _mainViewModel.CurrentViewModel = new GameRulesModel();
        }
        /// <summary>
        /// En knapp för att muta och starta Startview musik samt byta bild.
        /// </summary>
        private void MuteStartMusic()
        {
            if (IsMusicPlaying)
            {
                StartScreenMusic.Stop();
                IsMusicPlaying = false;
                SpeakerImage = "/Resources/MutedSpeakerButton.png";
            }
            else if (!IsMusicPlaying)
            {
                StartScreenMusic.Play();
                IsMusicPlaying = true;
                SpeakerImage = "/Resources/SpeakerButton.png";
            }
        }
        #endregion













    }
}
