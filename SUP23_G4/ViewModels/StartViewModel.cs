using SUP23_G4.Commands;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this._mainViewModel = mainViewModel;

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

        #endregion













    }
}
