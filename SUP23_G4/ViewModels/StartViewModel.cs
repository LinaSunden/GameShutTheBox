﻿using SUP23_G4.Commands;
using SUP23_G4.Dto;
using SUP23_G4.FileHandler;
using SUP23_G4.Languages;
using SUP23_G4.Models;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.GameComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace SUP23_G4.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        #region Konstruktor

        public StartViewModel()
        {

            Language = new();
            StartScreenMusic.Play();
            IsMusicPlaying = true;
            SpeakerImage = "/Resources/SpeakerButton.png";
            //SetupGame();
            //MainViewModel.Instance.CurrentViewModel = new GameViewModel(SettingsDto);
            //_mainViewModel.CurrentViewModel = new GameViewModel(SettingsDto);
            SetUpGameCommand = new RelayCommand(S => SetupGame()); 
        }

        #endregion

        #region Egenskaper
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        //public Language Language { get; set; }
        public PlayerSettingsDto SettingsDto;
        public Language Language {get; set;}  
        public SoundPlayer StartScreenMusic = new SoundPlayer(Properties.Resources.StartViewMusic);
        public ICommand MuteMusicCommand { get; set; }
        public ICommand SetUpGameCommand { get; }
        public int TargetPoints { get; set; } = 45;
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string SpeakerImage { get; set; }
        public bool IsMusicPlaying { get; set; }
        #endregion

        #region Instansvariabler

        //private MainViewModel _mainViewModel;

        #endregion
        
        #region Metoder
        /// <summary>
        /// Stoppar startmusiken och lägger till spelarnamn i Dton
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SetupGame()
        {
            StartScreenMusic.Stop();
            CreatePlayers();
            MainViewModel.Instance.StartGameCommand(SettingsDto);
            
            //MainViewModel.Instance.CurrentViewModel = new MainViewModel(SettingsDto); //hur får vi med oss DTOn?
            
            //_mainViewModel.CurrentViewModel = new GameViewModel(SettingsDto);
        }
        /// <summary>
        /// Skapar 2 spelare samt en Dto som lagrar dessa.
        /// </summary>
        private void CreatePlayers()
        {
            Player1 = new Player(Player1Name);
            Player2 = new Player(Player2Name);
            SettingsDto = new PlayerSettingsDto(Player1, Player2, TargetPoints);
        }

        ///// <summary>
        ///// En knapp för att muta och starta Startview musik samt byta bild.
        ///// </summary>
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
