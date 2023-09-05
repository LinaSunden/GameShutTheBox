using SUP23_G4.ViewModels.Base;
using SUP23_G4.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP23_G4.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; } = new StartViewModel();
    }
}
