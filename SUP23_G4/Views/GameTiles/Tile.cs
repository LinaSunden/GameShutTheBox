using SUP23_G4.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            DependencyProperty.Register("CurrentStatus", typeof(Status), typeof(Tile), new PropertyMetadata(Status.AvailableGameTile));

        //public ObservableCollection<Tile> TileswithValue { get; set; }

        public string DisplayValue { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
