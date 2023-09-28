using SUP23_G4.Commands;
using SUP23_G4.Dto;
using SUP23_G4.Enums;
using SUP23_G4.FileHandler;
using SUP23_G4.Models.Languages;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ChangeLanguageToSwedish = new RelayCommand (x => ChangeLanguage(GameLanguage.Swedish));
            ChangeLanguageToEnglish = new RelayCommand(x => ChangeLanguage(GameLanguage.English));
            ChangeLanguage(GameLanguage.English);

        }

        /// <summary>
        /// Byter vy till StartView
        /// </summary>
        private void GoToStartView()
        {  
            CurrentViewModel = new StartViewModel();        
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

        #endregion

        #region Egenskaper
        public ICommand GoToStartCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand GameRulesCommand { get; }
        public ICommand ChangeLanguageToSwedish { get; }
        public ICommand ChangeLanguageToEnglish { get; }
        public Language CurrentLanguage { get; set; }
        public GameLanguage GameLanguage { get; set; }

  
        #endregion

        #region Metoder
        /// <summary>
        /// Ändrar visningsspråket i appen baserat på val som spelaren gör i combobox på MainView
        /// </summary>
        /// <param name="gameLanguage"></param>
        public void ChangeLanguage (GameLanguage gameLanguage)
        {
            CurrentLanguage = Language.UpdateLanguage(gameLanguage);
        }

        /// <summary>
        /// En metod som gör att du för en förfrågan om du vill gå till startsidan från Gamview, men den går direkt till startsidan från spelregler.
        /// </summary>
        //private void GoToStartView()
        //{
        //    if (CurrentViewModel is GameViewModel)
        //    {
        //        MessageBoxResult result = MessageBox.Show("Vill du avsluta spelet och gå tillbaka till startsidan?", "Avsluta spelet", MessageBoxButton.YesNo);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            CurrentViewModel = new StartViewModel();
        //        }
        //    }
        //    else
        //    {
        //        CurrentViewModel = new StartViewModel();
        //    }
        //}

     
        #endregion
    }
}
