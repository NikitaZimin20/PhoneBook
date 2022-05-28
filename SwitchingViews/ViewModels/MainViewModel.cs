using SwitchingViews.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.ViewModels
{
    internal class MainViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationstore;
        public ViewModelBase CurrentViewModel =>_navigationstore.CurrentViewModel;
        public MainViewModel(NavigationStore navigationstore)
        {
            
            _navigationstore = navigationstore;
            _navigationstore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
