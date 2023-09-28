using SUP23_G4.Commands;
using SUP23_G4.Dto;
using SUP23_G4.FileHandler;
using SUP23_G4.Models.Languages;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        private static MainViewModel? _instance;
        public BaseViewModel CurrentViewModel { get; set; } = new StartViewModel();
        public static MainViewModel? Instance { get => _instance ?? (_instance = new MainViewModel()); }


        #region Konstruktor
        public MainViewModel()
        {
            GoToStartCommand = new RelayCommand(page => GoToStartView());
            GameRulesCommand = new RelayCommand(page => StartGameRules());
            StartGameCommand = new RelayCommand(page => StartGame(page));
            Language = new();
            UpdateLanguage();
        }



        #endregion

        #region Egenskaper
        public Language Language { get; set; } // ska det stå CurrentLanguage?
        public ICommand GoToStartCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand GameRulesCommand { get; }
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
        /// Byter vy till StartView
        /// </summary>
 
        public void GoToStartView()
        {
            if (CurrentViewModel is GameViewModel)
            {
                MessageBoxResult result = MessageBox.Show("Vill du avsluta spelet och gå tillbaka till startsidan?", "Avsluta spelet", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    CurrentViewModel = new StartViewModel();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                CurrentViewModel = new StartViewModel();
            }
        }


        /// <summary>
        /// Byter vy till GameRules
        /// </summary>
        private void StartGameRules()
        {
            CurrentViewModel = new GameRulesViewModel();
        }

        /// <summary>
        /// Byter vy till GameView och skickar med PlayerSettingsDton
        /// </summary>
        /// <param name="dto"></param>
        private void StartGame(object dto)
        {
            var settingsDto = dto as PlayerSettingsDto;
            CurrentViewModel = new GameViewModel(settingsDto);
        }
        /// <summary>
        /// En metod som gör att du för en förfrågan om du vill gå till startsidan från Gamview, men den går direkt till startsidan från spelregler.
        /// </summary>
  
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
