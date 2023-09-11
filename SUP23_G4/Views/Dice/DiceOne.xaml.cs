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


        public int DieOneValue
        {
            get { return (int)GetValue(DieOneValueProperty); }
            set { SetValue(DieOneValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieOneValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieOneValueProperty =
            DependencyProperty.Register("DieOneValue", typeof(int), typeof(DiceOne), new PropertyMetadata(0));



     
    }
}


