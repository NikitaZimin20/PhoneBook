using PhoneBook;
using PhoneBook.Commands;
using PhoneBook.Services;
using SwitchingViews.Commands;
using SwitchingViews.Models;
using SwitchingViews.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SwitchingViews.ViewModels
{
    internal class AccountViewModel : ViewModelBase,INotifyDataErrorInfo
    {
        private readonly Dictionary<string,List<string>>_propertyyErrors=new Dictionary<string, List<string>>() { };
        private UserModel _selecteduser;
        private XmlWorker _worker = new XmlWorker();
        private NavigationStore _navigationstore;
        private  string _name;
        private string _phone;
        private string _surname;
        private string _id;
        private readonly CaseManager _caseManager;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public string Name
        {
            get
            {
               
                return _name;
            }
            set
            {
                _name = value;

                ClearErrors(nameof(Name));
                if (_name.Length < 2)
                {
                    AddError(nameof(Name), "Invalid Name,It should cosist of at least 2 charachters");
                }
                else if (_name.Length > 50)
                {
                    AddError(nameof(Name), "Invalid Name,It should cosist of less then 50 characters");
                }
                else if (_name.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    AddError(nameof(Name), "It shouldn't contain spesial symbols");
                }
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                ClearErrors(nameof(Surname));
                if (_surname.Length < 2)
                {
                    AddError(nameof(Surname), "Invalid Surname,It should cosist of at least 2 charachters");
                }
                else if (_surname.Length > 50)
                {
                    AddError(nameof(Surname), "Invalid Surname,It should cosist of less then 50 characters");
                }
                else if (_surname.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    AddError(nameof(Surname), "It shouldn't contain spesial symbols");
                }

                OnPropertyChanged(nameof(Surname));
            }
        }
        public bool IsDeleteeButtonOpen { get; }
        public string Phone
        {
            get => _phone;
            set
            {

                _phone = value;
                var numbers = _phone.Where(Char.IsDigit).ToArray();
                ClearErrors(nameof(Phone));
                if (numbers.Length!=11)
                {
                    AddError(nameof(Phone), "It should consist 11 letters");
                }
                if (numbers.Length==0)
                {
                    AddError(nameof(Phone), "Empty field");
                }
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
        

        public bool CanCreate => !HasErrors;
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }


        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            _navigationstore = new NavigationStore();        
            _selecteduser = new UserModel() { ID=this.ID,Name = this.Name, Surname = this.Surname, Phone = this.Phone };
            if (_caseManager == CaseManager.Save)
                _worker.AddToXML(_selecteduser);
            else
            {
                MessageBox.Show("Test");
                _worker.ChangeXML(_selecteduser);
            }
               

            GoHome(_navigationstore);
        }

        private bool CanExecuteDeleteCommand(object obj) => true;

        private void OnExecuteDeleteCommand(object obj)
        {
            if (MessageBox.Show("Do you want to delete this field?",
                   "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _worker.DeleteFromXml(ID);
                _navigationstore = new NavigationStore();
                GoHome(_navigationstore);
            }
               
        }

        private void GoHome(NavigationStore navigationStore)
        {
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
            NavigateHomeCommand.Execute(navigationStore.CurrentViewModel);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyyErrors.GetValueOrDefault(propertyName,null);
        }
        private void ClearErrors(string propertyname)
        {
            if (_propertyyErrors.Remove(propertyname))

                OnErrorsChanged(propertyname);
        }

        public ObservableCollection<UserModel> User { get; set; }

        public bool HasErrors => _propertyyErrors.Any();
        public bool CanOpen=>_caseManager == CaseManager.Change;
        public void AddError(string propertyName,string errorMessage)
        {
            if (!_propertyyErrors.ContainsKey(propertyName))
            {
                _propertyyErrors.Add(propertyName, new List<string>());
            }
            _propertyyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }
        private void OnErrorsChanged(string? propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(CanCreate));
        }

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
            
            
            SaveCommand = new RelayCommand(OnExecuteTakeNoteCommand,CanExecuteTakeNoteCommand);
            DeleteCommand = new RelayCommand(OnExecuteDeleteCommand, CanExecuteDeleteCommand);

        }
     


    } 
    
    
}
