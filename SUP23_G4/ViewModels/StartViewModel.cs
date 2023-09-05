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
            this._mainViewModel = mainViewModel;

        }

        #endregion

        #region Property
        public ICommand StartGameCommand { get; set; }

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

        #endregion













    }
}
