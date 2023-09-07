using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUP23_G4.Views.Dice
{
    /// <summary>
    /// Interaction logic for Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        public Dice()
        {
            InitializeComponent();

            
        }


        public int DieValue { get; set; }


        //public int DieValue
        //{
        //    get { return (int)GetValue(DieValueProperty); }
        //    set { SetValue(DieValueProperty, value); }
        //}

        //Using a DependencyProperty as the backing store for DieValue.This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DieValueProperty =
        //    DependencyProperty.Register("DieValue", typeof(int), typeof(Dice), new PropertyMetadata(0));




        //public DiceNumber CurrentDiceNumber
        //{
        //    get { return (DiceNumber)GetValue(CurrentDiceNumberProperty); }
        //    set { SetValue(CurrentDiceNumberProperty, value); }
        //}

        //Using a DependencyProperty as the backing store for CurrentDiceNumber.This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CurrentDiceNumberProperty =
        //    DependencyProperty.Register("CurrentDiceNumber", typeof(DiceNumber), typeof(Dice), new PropertyMetadata(DiceNumber.Five));







    }
}

