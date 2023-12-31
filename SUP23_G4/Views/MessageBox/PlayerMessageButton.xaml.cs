﻿using SUP23_G4.Commands;
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

namespace SUP23_G4.Views.MessageBox
{
    /// <summary>
    /// Interaction logic for PlayerMessageButton.xaml
    /// </summary>
    public partial class PlayerMessageButton : UserControl
    {
        public PlayerMessageButton()
        {
            InitializeComponent();
            
        }
        

        public MessageStatus CurrentMessage
        {
            get { return (MessageStatus)GetValue(CurrentMessageProperty); }
            set { SetValue(CurrentMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMessageProperty =
            DependencyProperty.Register("CurrentMessage", typeof(MessageStatus), typeof(PlayerMessageButton), new PropertyMetadata(null));


    }
}
