using PhoneBook;
using PhoneBook.Commands;
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
    internal class HomeViewModel : ViewModelBase
    {
        
        private UserModel selectedUser;
        private readonly XmlWorker worker = new();
        public  ICommand NavigateAccountCommand { get; set; }
      
       
        public ObservableCollection<UserModel> User { get; set; }
       public UserModel SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        

        public HomeViewModel(NavigationStore navigationstore)
        {
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(navigationstore,()=>new AccountViewModel(navigationstore));
            User = new ObservableCollection<UserModel>();
            AccountViewModel model = new AccountViewModel(navigationstore);
            OnPropertyChanged(nameof(User));
            User = worker.LoadFromXml(User);
            



        }
      
    }
}
