using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views.Dice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public List<int> AvailableTiles { get; set; } = new List<int>();


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
            int diceValue = 12;
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
                else if (diceValue > tiles.Count())
                {
                    availableTiles = tiles;
                }
            }
            AvailableTiles = availableTiles;


            int[] arrayTiles = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            GetAvailableTiles(availableTiles, diceValue);
            
            //ArrayOfAvailableTiles(arrayTiles, diceValue);
            
            //SubsetExists(arrayTiles, diceValue);
            
            
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
        public void AvailableTilesAfterSelectedTile(int selectedTile, int targetSum)
        {
            List<int> availableTiles = new List<int>();

            if (selectedTile == targetSum)
            {

            }
            else if (selectedTile < targetSum) 
            { 
                foreach (int i in AvailableTiles)
                {
                    if (selectedTile + i == targetSum)
                    {
                        availableTiles.Add(selectedTile);
                        availableTiles.Add(i);
                    }

                }
            
            }
        }

        public void GetAvailableTiles(List<int>tiles, int targetSum)
        {

            List<List<int>> collection = new List<List<int>>();
            List<int> availableTiles;

            foreach (int i in tiles)
            {
                if (i == targetSum)
                {
                    availableTiles = new List<int>()
                    { 
                        i,
                    };
                    collection.Add(availableTiles);
                    break;
                }
                else if (i < targetSum)
                {
                    for(int j = i + 1; j <= tiles.Count(); j++)
                    {
                        if (i + j ==  targetSum)
                        {
                            availableTiles = new List<int>()
                            {
                                i,
                                j,
                            };
                            collection.Add(availableTiles);
                            break;
                        }
                        else if (i + j < targetSum)
                        {
                            for (int k = i + 2;  k <= tiles.Count(); k++)
                            {
                                if (i + j + k == targetSum)
                                {
                                    availableTiles = new List<int>()
                                    {
                                        i,
                                        j,
                                        k,
                                    };
                                    collection.Add(availableTiles);
                                    break;
                                }
                                else if (i + j + k < targetSum)
                                {
                                    for ( int l = i + 3; l <= tiles.Count(); l++)
                                    {
                                        if (j + l + k +l == targetSum)
                                        {
                                            availableTiles = new List<int>()
                                            {
                                                i,
                                                j,
                                                k,
                                                l,
                                            };
                                            collection.Add(availableTiles);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool CalculateSelectedTiles(List<int>selectedTiles, int targetSum)
        {
            int calculatedSum = 0;

            foreach(int i in selectedTiles)
            {
                calculatedSum += i;
            }
            if (calculatedSum == targetSum)
            {
                return true;
            }

            return false;
        }

        //public void ArrayOfAvailableTiles(int[] tiles, int targetSum)
        //{
        //    int count = 0;
        //    int[][] selectableTiles = new int[20][];

        //    for (int i = 1; i < tiles.Length + 1; i++)
        //    {
        //        if (tiles[i - 1] == targetSum)
        //        {
        //            selectableTiles[count] = new int[] { i };
        //            count++;
        //            break;
        //        }
        //        else if (tiles[i - 1] < targetSum)
        //        {
        //            for (int j = i + 1; j < tiles.Length + 1; j++)
        //            {
        //                if (tiles[i - 1] + tiles[j - 1] == targetSum)
        //                {
        //                    selectableTiles[count] = new int[] {i, j};
        //                    count++;
        //                    break;
        //                }
        //                else if (tiles[i - 1] + tiles[j - 1] < targetSum)
        //                {
        //                    for (int k = i + 2; k < tiles.Length + 1; k++)
        //                    {
        //                        if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] == targetSum)
        //                        {
        //                            selectableTiles[count] = new int[] { i, j, k };
        //                            count++;
        //                            break;
        //                        }
        //                        else if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] < targetSum)
        //                        {
        //                            for (int l = i + 3;  l < tiles.Length + 1; l++)
        //                            {
        //                                if (tiles[i - 1] + tiles[j - 1] + tiles[k - 1] + tiles[l - 1] == targetSum)
        //                                {
        //                                    selectableTiles[count] = new int[] { i, j, k, l };
        //                                    count++;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
                        
        //            }
        //        }
        //        else if (tiles[i] > targetSum)
        //        {
        //            break;
        //        }
        //    }
        //}
        //public bool SubsetExists(int[] tiles, int targetSum)
        //{
        //    int n = tiles.Length;

        //    bool[,] subset = new bool[targetSum + 1, n + 1]; 

        //    for (int i  = 0; i <= n; i++)
        //    {
        //        subset[0, i] = true;
        //    }

        //    for (int i = 1; i <= targetSum; i++)
        //    {
        //        subset[i, 0] = false;
        //    }

        //    for (int i = 1; i <= targetSum; i++ )
        //    {
        //        for (int j = 1; j <= n; j++)
        //        {
        //            subset[i, j] = subset[i, j - 1];
        //            if (i >= tiles[j - 1])
        //            {
        //                subset[i, j] = subset[i, j] || subset[i - tiles[j - 1], j - 1];
        //            }
        //        }
        //    }

        //    return subset[targetSum, n];
        //}

        //public void FillListOfSubsets(int[] tiles, int targetSum)
        //{
        //    bool[,] dp;

        //    List<int> subset = new List<int>();


        //}
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
