using SUP23_G4.Views.Dice;
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

namespace SUP23_G4.Views.GameComponents
{
    /// <summary>
    /// Interaction logic for Die.xaml
    /// </summary>
    public partial class Die : UserControl
    {
        public Die()
        {
            InitializeComponent();
        }



        public int DieValue
        {
            get { return (int)GetValue(DieValueProperty); }
            set { SetValue(DieValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DieValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DieValueProperty =
            DependencyProperty.Register("DieValue", typeof(int), typeof(Die), new PropertyMetadata(0));






    }
}
