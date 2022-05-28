using SwitchingViews.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.Stores
{
    internal class NavigationStore
    {
        public event Action CurrentViewModelChanged;
        private ViewModelBase _currentviewmodel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentviewmodel;
            set
            {
                _currentviewmodel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
