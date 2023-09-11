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
    /// Interaction logic for DiceTwo.xaml
    /// </summary>
    public partial class DiceTwo : UserControl
    {
        public DiceTwo()
        {
            InitializeComponent();
        }


        public int DieTwoValue
        {
            get { return (int)GetValue(DieTwoValueProperty); }
            set { SetValue(DieTwoValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieTwoValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieTwoValueProperty =
            DependencyProperty.Register("DieTwoValue", typeof(int), typeof(DiceTwo), new PropertyMetadata(0));


    }
}
