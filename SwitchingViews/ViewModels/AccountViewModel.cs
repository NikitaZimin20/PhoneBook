using PhoneBook.Models;
using SwitchingViews.Commands;
using SwitchingViews.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwitchingViews.ViewModels
{
    internal class AccountViewModel : ViewModelBase
    {
        private UserModel _selecteduser;
       public UserModel SelectedUser
        {
            get { return _selecteduser; }
            set { _selecteduser = value; }
        }
        public ICommand NavigateHomeCommand { get; }
        public ObservableCollection<UserModel> User { get; set; }
        
        public AccountViewModel(NavigationStore navigationstore)
        {
            
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationstore,()=>new HomeViewModel(navigationstore));
           
        }
             
    
    } 
    
    
}
