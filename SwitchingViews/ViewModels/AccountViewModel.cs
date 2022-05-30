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
        private XmlWorker _worker = new XmlWorker();
        private NavigationStore _navigationstore;
        private  string _name;
        private string _phone;
        private string _surname;
        private string _id;
        private readonly CaseManager _caseManager;
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

        public string ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }




        public ICommand NavigateHomeCommand { get; set; }
       public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            _navigationstore = new NavigationStore();        
            _selecteduser = new UserModel() { Name = this.Name, Surname = this.Surname, Phone = this.Phone };
            if (_caseManager == CaseManager.Save)
                _worker.AddToXML(_selecteduser);
            else _worker.ChangeXML(_selecteduser);

            GoHome(_navigationstore);
        }

        private bool CanExecuteDeleteCommand(object obj) => true;

        private void OnExecuteDeleteCommand(object obj)
        {
            _worker.DeleteFromXml(ID);
            _navigationstore = new NavigationStore();
            GoHome(_navigationstore);
        }

        private void GoHome(NavigationStore navigationStore)
        {
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
            NavigateHomeCommand.Execute(navigationStore.CurrentViewModel);
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
                ID = model.ID;
            }
            _caseManager = CaseManager.Save;
            
            SaveCommand = new RelayCommand(OnExecuteTakeNoteCommand,CanExecuteTakeNoteCommand);
            DeleteCommand = new RelayCommand(OnExecuteDeleteCommand, CanExecuteDeleteCommand);

        }
     


    } 
    
    
}
