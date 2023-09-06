using SUP23_G4.Commands;
using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SUP23_G4.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        #region Konstruktor
        public MainViewModel()
        {
            CurrentViewModel = new StartViewModel(this);
        }

        #endregion

        #region Egenskaper
        public BaseViewModel CurrentViewModel { get; set; }

        #endregion

        #region Instansvariabler
        private BaseViewModel _mainViewModel;
        #endregion

    }
}
