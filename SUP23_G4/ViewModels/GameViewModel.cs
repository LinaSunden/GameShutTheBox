using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SUP23_G4.ViewModels
{
    public class GameViewModel : BaseViewModel
    {

        public int DieOne { get; private set; }
        public int DieTwo { get; private set; }
        public int DiceValue { get; private set; }

        public void DiceToss()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                Die die = new Die();
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
            DiceValue = DieOne + DieTwo;
        }
        public void FillListOfAvailableTiles(/*int diceValue*/)
        {
            int diceValue = 10;
            List<int> tiles = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> availableTiles = new List<int>();
            foreach (int tile in tiles)
            {
                if (tile == diceValue)
                {
                    for (int i = 1; i <= tile; i++)
                    {
                        availableTiles.Add(i);
                    }
                    break;
                }
            }
            int[] arrayTiles = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            AvailableTiles(arrayTiles, diceValue);
            //    if (availableTiles.Count == 0)
            //    {
            //        AvailableTiles(tiles);
            //    }
            //    else
            //    {
            //        AvailableTiles(availableTiles);
            //    }

            //}
            ////public void AvailableTiles(List<int> availableTiles)
            ////{

        }
        public void AvailableTiles(int[] tiles, int targetSum)
        {

            int[][] selectableTiles = new int[5][];

            for (int i = 1; i < tiles.Length; i++)
            {
                if (tiles[i - 1] == targetSum)
                {
                    selectableTiles[i - 1] = new int[] { i };
                    break;
                }
                else if (tiles[i - 1] < targetSum)
                {
                    for (int j = i + 1; j < tiles.Length; j++)
                    {
                        if (tiles[i - 1] + tiles[j - 1] == targetSum)
                        {
                            selectableTiles[i - 1] = new int[] {i, j};
                            break;
                        }
                        else if (tiles[i - 1] + tiles[j - 1] < targetSum)
                        {
                            for (int k = i + 2; k < tiles.Length; k++)
                            {
                                if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] == targetSum)
                                {
                                    selectableTiles[i - 1] = new int[] { i, j, k };
                                    break;
                                }
                                else if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] < targetSum)
                                {
                                    for (int l = i + 3;  l < tiles.Length; l++)
                                    {
                                        if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] + tiles[l - 1] == targetSum)
                                        {
                                            selectableTiles[i - 1] = new int[] { i, j, k, l };
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
                else if (tiles[i] > targetSum)
                {
                    break;
                }
            }
        }
        //public void SubsetExists(int[] tiles, int targetSum)
        //{


        //    if (tiles.Length == 0 || targetSum <= 0)
        //    {
        //        return;
        //    }

        //    int arrSize = tiles.Length;

        //    int[][] count = new int[10][];
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            count[i] = new int[] { targetSum + 1 };
        //        }
        //    };

        //    for (int i = 0; i < targetSum; i++)
        //    {
        //        count[i][0] = 1;
        //    }

        //    for (int j = 0; j <= targetSum; j++)
        //    {
        //        if (tiles[0] == j)
        //        {
        //            count[0][j] = 1;
        //        }
        //    }

        //    for (int i =1; i < arrSize; i++)
        //    {
        //        for (int j = 1; j <= targetSum; j++)
        //        {
        //            int includingCurrentValue = 0;
        //            int excludingCurrentValue = 0;

        //            if (tiles[i] <= j) 
        //            {
        //                includingCurrentValue = count[i - 1][j - tiles[i]];
        //            }

        //            excludingCurrentValue = count[i - 1][j];

        //            count[i][j] = includingCurrentValue + excludingCurrentValue;
        //        }
        //    }

        //}
    }
}
