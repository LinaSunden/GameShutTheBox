using SUP23_G4.Commands;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            GoToStartCommand = new RelayCommand(x => GoToStart());
        }


        #endregion

        #region Egenskaper
        public BaseViewModel CurrentViewModel { get; set; }
        public ICommand StartViewClickCommand { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand GoToStartCommand { get; }

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

    /// <summary>
    /// En metod som gör att du för en förfrågan om du vill gå till startsidan från Gamview, men den går direkt till startsidan från spelregler.
    /// </summary>
    public void GoToStart()
        {
            if (CurrentViewModel is GameViewModel)
            {
                MessageBoxResult result = MessageBox.Show("Vill du avsluta spelet och gå tillbaka till startsidan?", "Avsluta spelet", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    CurrentViewModel = new StartViewModel(this);
                }
                else if (result == MessageBoxResult.No)
                {

                }
            }
            else
            {
                CurrentViewModel = new StartViewModel(this);
            }

        }

        #endregion

    }
}
