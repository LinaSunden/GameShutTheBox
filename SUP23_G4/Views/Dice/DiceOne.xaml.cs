using SUP23_G4.Commands;
using SUP23_G4.Enums;
using SUP23_G4.ViewModels;
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
    public partial class DiceOne : UserControl
    {
        public DiceOne()
        {
            InitializeComponent();

        }


        //public int DieOneValue { get; set; }


        public int DieTwoValue { get; set; }




        public int DieOneValue
        {
            get { return (int)GetValue(DieOneValueProperty); }
            set { SetValue(DieOneValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieOneValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieOneValueProperty =
            DependencyProperty.Register("DieOneValue", typeof(int), typeof(DiceOne), new PropertyMetadata(0));



        //public Visibility VisibilityDiceOne1
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne1Property); }
        //    set { SetValue(VisibilityDiceOne1Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne1Property =
        //    DependencyProperty.Register("VisibilityDiceOne1", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));

        //public Visibility VisibilityDiceOne2
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne2Property); }
        //    set { SetValue(VisibilityDiceOne2Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne2Property =
        //    DependencyProperty.Register("VisibilityDiceOne2", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));

        //public Visibility VisibilityDiceOne3
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne3Property); }
        //    set { SetValue(VisibilityDiceOne3Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne3Property =
        //    DependencyProperty.Register("VisibilityDiceOne3", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));

        //public Visibility VisibilityDiceOne4
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne4Property); }
        //    set { SetValue(VisibilityDiceOne4Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne4Property =
        //    DependencyProperty.Register("VisibilityDiceOne4", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));

        //public Visibility VisibilityDiceOne5
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne5Property); }
        //    set { SetValue(VisibilityDiceOne5Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne5Property =
        //    DependencyProperty.Register("VisibilityDiceOne5", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));

        //public Visibility VisibilityDiceOne6
        //{
        //    get { return (Visibility)GetValue(VisibilityDiceOne6Property); }
        //    set { SetValue(VisibilityDiceOne6Property, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisibilityDiceOne1.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisibilityDiceOne6Property =
        //    DependencyProperty.Register("VisibilityDiceOne6", typeof(Visibility), typeof(DiceOne), new PropertyMetadata(Visibility.Hidden));







        //public Visibility VisibilityDiceOne1 { get;  set; } = Visibility.Hidden;
        //public Visibility VisibilityDiceOne2 { get;  set; } = Visibility.Hidden;
        //public Visibility VisibilityDiceOne3 { get;  set; } = Visibility.Visible;
        //public Visibility VisibilityDiceOne4 { get;  set; } = Visibility.Hidden;
        //public Visibility VisibilityDiceOne5 { get;  set; } = Visibility.Hidden;
        //public Visibility VisibilityDiceOne6 { get;  set; } = Visibility.Hidden;



        //public void ShowDiceNumber()
        //{
        //    GameViewModel.DiceToss();

        //    VisibilityDiceOne1 = System.Windows.Visibility.Hidden;
        //    VisibilityDiceOne2 = System.Windows.Visibility.Hidden;
        //    VisibilityDiceOne3 = System.Windows.Visibility.Hidden;
        //    VisibilityDiceOne4 = System.Windows.Visibility.Hidden;
        //    VisibilityDiceOne5 = System.Windows.Visibility.Hidden;
        //    VisibilityDiceOne6 = System.Windows.Visibility.Hidden;


        //    switch (DieOneValue)
        //    {
        //        case 1:
        //            VisibilityDiceOne1 = System.Windows.Visibility.Visible;


        //            break;
        //        case 2:
        //            VisibilityDiceOne2 = System.Windows.Visibility.Visible;


        //            break;
        //        case 3:
        //            VisibilityDiceOne3 = System.Windows.Visibility.Visible;

        //            break;
        //        case 4:
        //            VisibilityDiceOne4 = System.Windows.Visibility.Visible;

        //            break;
        //        case 5:
        //            VisibilityDiceOne5 = System.Windows.Visibility.Visible;

        //            break;
        //        case 6:
        //            VisibilityDiceOne6 = System.Windows.Visibility.Visible;

        //            break;
        //        default:
        //            break;
        //    }





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







        //}
    }
}


