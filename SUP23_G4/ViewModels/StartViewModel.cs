using SUP23_G4.Commands;
using SUP23_G4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    internal class StartViewModel : BaseViewModel
    {
        #region Konstruktor

        public StartViewModel(MainViewModel mainViewModel)
        {
            StartGameCommand = new RelayCommand(x => SwitchToGameView());
            GameRulesCommand = new RelayCommand(x => SwitchToGameRules());
            this._mainViewModel = mainViewModel;


        }

        #endregion

        #region Egenskaper

        public ICommand StartGameCommand { get; set; }
        public ICommand GameRulesCommand { get; set; }

        #endregion


        #region Instansvariabler

        private MainViewModel _mainViewModel;

        #endregion



        #region Metoder
        /// <summary>
        /// Byter CurrentViewModel från att visa startVy till att visa GameVy
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected void SwitchToGameView()
        {
            _mainViewModel.CurrentViewModel = new GameViewModel();

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
