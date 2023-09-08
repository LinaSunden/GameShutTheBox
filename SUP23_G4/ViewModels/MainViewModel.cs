using SUP23_G4.Commands;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    public class MainViewModel : BaseViewModel
    {


        #region Konstruktor
        public MainViewModel()
        {
            CurrentViewModel = new StartViewModel(this);
            StartViewClickCommand = new RelayCommand(x => SwitchToStartView());
            StartGameCommand = new RelayCommand(x => StartGame());
        }


        #endregion

        #region Egenskaper
        public BaseViewModel CurrentViewModel { get; set; }
        public ICommand StartViewClickCommand { get; set; }
        public ICommand StartGameCommand { get; set; }

        #endregion

        #region Instansvariabler
        private BaseViewModel _mainViewModel;

        #endregion
        #region Metoder
        // Byter CurrentViewModel till StartViewModel//
        private void SwitchToStartView()
        {
            CurrentViewModel = new StartViewModel(this);
        }
        // Skickar med vår StartViewModel till GameViewModel direkt när programmet startas.
        private void StartGame()
        {
            GameViewModel gameViewModel = new GameViewModel((StartViewModel)CurrentViewModel);
            CurrentViewModel = gameViewModel;
        }

        #endregion

    }
}
