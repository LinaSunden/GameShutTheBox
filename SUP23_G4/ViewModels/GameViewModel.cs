﻿using SUP23_G4.Enums;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using SUP23_G4.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        #region Egenskaper
        public int DieOne { get; private set; }
        public int DieTwo { get; private set; }

        public DiceNumber DiceNumber { get; set; }

        public ObservableCollection<Tile> GameTiles { set; get; } = new ObservableCollection<Tile>();
           
        public void FillCollectionOfGameTiles() 
        {
            Tile tile; 
            for (int i= 1; i<=10; i++) 
            {
                tile = new Tile();
                {
                    tile.DisplayValue = i.ToString();
                    tile.Value = i; 
                };
                GameTiles.Add(tile);
            }

        }


        public GameViewModel()
        {
            FillCollectionOfGameTiles();
        }



        #endregion

        #region Metoder
        public void DiceToss()
        {
            Random r = new Random();
            
            for (int i = 0; i < 2; i++)
            {
                Dice die = new Dice();
                if (i == 0)
                {
                    die.DieValue = r.Next(1, 7);
                    DieOne = die.DieValue;
                }
                else if (i == 1)
                {
                    die.DieValue = r.Next(1, 7);
                    DieTwo = die.DieValue;
                }
            }          
        }
        #endregion
        

    }
}
