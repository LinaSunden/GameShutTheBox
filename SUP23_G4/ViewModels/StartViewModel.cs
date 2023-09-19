using SUP23_G4.Commands;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.MuteButton;
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
            StartGameCommand = new RelayCommand(x => StartGame());
            GameRulesCommand = new RelayCommand(x => SwitchToGameRules());
            MuteMusic = new RelayCommand(x => MuteStartMusic());
            this._mainViewModel = mainViewModel;
            StartScreenMusic.Play();
            IsMusicPlaying = true;
            ImageSource = "/Resources/SpeakerButton.png";
        }

        public StartViewModel()
        {
            
        }

        #endregion

        #region Egenskaper
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand GameRulesCommand { get; set; }
        //public ICommand CreatePlayerCommand { get; set; }
        public ICommand MuteMusic { get; set; }
        public bool IsMusicPlaying { get; set; }

        public SoundPlayer StartScreenMusic = new SoundPlayer(Properties.Resources.StartViewMusic);

        public string ImageSource { get; set; }
        

        #endregion


        #region Instansvariabler

        private MainViewModel _mainViewModel;

        #endregion



        #region Metoder
        /// <summary>
        /// Byter CurrentViewModel från att visa startVy till att visa GameVy
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void StartGame()
        {
            CreatePlayer();

            _mainViewModel.CurrentViewModel = new GameViewModel(this);
        }

        private void CreatePlayer()
        {
            Player1 = new Player(PlayerOneName);
            Player2 = new Player(PlayerTwoName);
        }

        /// <summary>
        /// Byter CurrentViewModel från att visa startVy till att visa GameRules
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void SwitchToGameRules()
        {
            _mainViewModel.CurrentViewModel = new GameRulesModel();
        }

        public void MuteStartMusic()
        {
            if (IsMusicPlaying)
            {
                StartScreenMusic.Stop();
                IsMusicPlaying = false;
                ImageSource = "/Resources/MutedSpeakerButton.png";
            }
            else if (!IsMusicPlaying)
            {
                StartScreenMusic.Play();
                IsMusicPlaying = true;
                ImageSource = "/Resources/SpeakerButton.png";
            }
        }
        #endregion













    }
}
