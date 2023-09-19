using SUP23_G4.Commands;
using SUP23_G4.Enums;
using SUP23_G4.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SUP23_G4.Views.GameTiles
{
    public class Tile : UserControl
    {


        public Status CurrentStatus
        {
            get { return (Status)GetValue(CurrentStatusProperty); }
            set { SetValue(CurrentStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentStatusProperty =
            DependencyProperty.Register("CurrentStatus", typeof(Status), typeof(Tile), new PropertyMetadata(null));



        public int TileValue
        {
            get { return (int)GetValue(TileValueProperty); }
            set { SetValue(TileValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileValueProperty =
            DependencyProperty.Register("TileValue", typeof(int), typeof(Tile), new PropertyMetadata(0));




        public ICommand SelectedTileCommand
        {
            get { return (ICommand)GetValue(SelectedTileCommandProperty); }
            set { SetValue(SelectedTileCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTileCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTileCommandProperty =
            DependencyProperty.Register("SelectedTileCommand", typeof(ICommand), typeof(Tile), new PropertyMetadata(null));

    }
}
