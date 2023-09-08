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

        /// <summary>
        /// Metod som räknar ut vilka brickor som är tillgängliga utifrån 
        /// det sammanlagda värdet av båda tärningar
        /// </summary>
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

        }

        /// <summary>
        /// Metod som testar om spelarens val av brickor är möjliga att välja
        /// för att nå tärningarnas summa (Eventuellt överflödig)
        /// </summary>
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

        /// <summary>
        /// Metod som undersöker vilka kombinationer av brickor som är
        /// möjliga för att nå tärningarnas summa
        /// </summary>
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

        /// <summary>
        /// Metod som testar om spelarens valda brickor blir tärningarnas
        /// summa (Eventuellt överflödig)
        /// </summary>
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
   
    }
}
