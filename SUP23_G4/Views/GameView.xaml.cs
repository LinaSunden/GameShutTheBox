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


namespace SUP23_G4.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            
        }

        public GameViewModel GameViewModel = new();

        //Det fungerar inte att klicka direkt på tärningarna för att kasta dem. Funderar på om det
        //beror på att GameViewModel nyas upp här eller att det inte går att använda en metod från GameViewModel
        // i GameView. Den går in i metoden och kör den men värdet ändras inte..
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                GameViewModel.ShowDiceNumber();
            }

        }
    }
}
