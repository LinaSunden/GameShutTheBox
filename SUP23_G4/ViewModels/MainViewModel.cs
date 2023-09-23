using SUP23_G4.Commands;
using SUP23_G4.FileHandler;
using SUP23_G4.Languages;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.IO;
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
            GoToStartCommand = new RelayCommand(x => GoToStartView());
            Language = new();
            UpdateLanguage();
        }
        #endregion

        #region Egenskaper
        public BaseViewModel CurrentViewModel { get; set; }
        public ICommand GoToStartCommand { get; }
        public Language Language { get; set; }
        private int _cboSelectedIndex = 1;
        public int CboSelectedIndex
        {
            get { return _cboSelectedIndex; }
            set
            {
                if (_cboSelectedIndex != value)
                {
                    _cboSelectedIndex = value;
                    UpdateLanguage();
                }
            }
        }

        #endregion

        #region Metoder
        /// <summary>
        /// En metod som gör att du för en förfrågan om du vill gå till startsidan från Gamview, men den går direkt till startsidan från spelregler.
        /// </summary>
        private void GoToStartView()
        {
            if (CurrentViewModel is GameViewModel)
            {
                MessageBoxResult result = MessageBox.Show("Vill du avsluta spelet och gå tillbaka till startsidan?", "Avsluta spelet", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    CurrentViewModel = new StartViewModel(this);
                }
            }
            else
            {
                CurrentViewModel = new StartViewModel(this);
            }
        }


        /// <summary>
        /// Ändrar visningsspråket i appen baserat på val som spelaren gör i combobox på StartView
        /// </summary>
        private void UpdateLanguage()
        {
            if (File.Exists("English.json") && CboSelectedIndex == 1)
            {
                Language = JsonFileHandler.Open<Language>("English.json");

            }
            else if (File.Exists("Swedish.json") && CboSelectedIndex == 0)
            {
                Language = JsonFileHandler.Open<Language>("Swedish.json");
            }
            else
            {
                Language = new Language();
            }
        }
        #endregion
    }
}
