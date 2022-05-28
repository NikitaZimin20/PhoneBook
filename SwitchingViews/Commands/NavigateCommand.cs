using SwitchingViews.Stores;
using SwitchingViews.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.Commands
{
    internal class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationstore;
        private readonly Func<TViewModel> _createnewmodel;

        public NavigateCommand(NavigationStore navigationstore,Func<TViewModel> createnewmodel)
        {
            _navigationstore = navigationstore;
            _createnewmodel = createnewmodel;
        }

        public override void Execute(object parameter)
        {
            _navigationstore.CurrentViewModel = _createnewmodel();
        }
    }
}
