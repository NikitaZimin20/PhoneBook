using PhoneBook;
using PhoneBook.Commands;
using PhoneBook.Models;
using PhoneBook.Stores;
using SwitchingViews.Commands;
using SwitchingViews.Models;
using SwitchingViews.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SwitchingViews.ViewModels
{
    internal class AccountViewModel : ViewModelBase
    {
        
        private UserModel _selecteduser;
        private XmlWorker _worker;
        private NavigationStore _navigationstore;
        private  string _name;
        private string _phone;
        private string _surname;
        private readonly CaseManager _caseManager = CaseManager.Save;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }




        public ICommand NavigateHomeCommand { get; set; }
       public ICommand SaveCommand { get; }

        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            _navigationstore = new NavigationStore();
            _worker = new XmlWorker();
            _selecteduser = new UserModel() { Name = this.Name, Surname = this.Surname, Phone = this.Phone };
            if (_caseManager == CaseManager.Save)
                _worker.AddToXML(_selecteduser);
            else _worker.ChangeXML(_selecteduser);
           
           

        }
        private void GoHome(NavigationStore navigationStore)
        {
            navigationStore = new HomeViewModel(navigationStore);
            NavigateHomeCommand.Execute(_navigationstore.CurrentViewModel);
        }


        public ObservableCollection<UserModel> User { get; set; }
        
        public AccountViewModel(NavigationStore navigationstore,UserModel model)
        {            
            HomeViewModel homeViewModel = new HomeViewModel(navigationstore);
         
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationstore,()=>new HomeViewModel(navigationstore));
            if (model is not null)
            {
                _caseManager = CaseManager.Change;
                Name = model.Name;
                Surname = model.Surname;
                Phone = model.Phone;
            }
            
            
            SaveCommand = new RelayCommand(OnExecuteTakeNoteCommand,CanExecuteTakeNoteCommand);
           
        }
     


    } 
    
    
}
