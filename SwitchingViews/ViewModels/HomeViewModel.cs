using DevExpress.Mvvm;
using PhoneBook;
using PhoneBook.Commands;
using PhoneBook.FileWorkes;
using PhoneBook.Services;
using SwitchingViews.Commands;
using SwitchingViews.Models;
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
        public event Action UserChanged;
        private UserModel _selecteduser;
        string _pattern;
        
        public  ICommand NavigateSaveCommand { get; set; }
        public ICommand NavigateAccountChangeCommand { get; set; }
       

        public ObservableCollection<UserModel> User { get; set; }
        
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                SelectedUser = User.FirstOrDefault(s => s.StartWith(Pattern));
            }
        }
        public UserModel SelectedUser
        {
            get { return _selecteduser; }
            set
            {
                _selecteduser = value;
                UserChanged?.Invoke();
                OnPropertyChanged(nameof(SelectedUser));


            }
        }
        public HomeViewModel(NavigationStore navigationstore)
        {
       
            NavigateSaveCommand = new NavigateCommand<AccountViewModel>(navigationstore,()=>new AccountViewModel(navigationstore));
            NavigateAccountChangeCommand= new NavigateCommand<AccountViewModel>(navigationstore, () => new AccountViewModel(navigationstore));
            User = new ObservableCollection<UserModel>();           
            OnPropertyChanged(nameof(User));
            User = XmlWorker.LoadFromXml(User);
            



        }

       
    }
}
