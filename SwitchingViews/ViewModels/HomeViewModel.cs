using SwitchingViews.Commands;
using SwitchingViews.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwitchingViews.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
       public  ICommand NavigateAccountCommand { get; }
       
        public HomeViewModel(NavigationStore navigationstore)
        {
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(navigationstore,()=>new AccountViewModel(navigationstore));
        }
      
    }
}
