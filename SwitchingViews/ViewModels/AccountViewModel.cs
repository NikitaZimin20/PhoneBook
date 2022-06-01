using PhoneBook.Commands;
using PhoneBook.FileWorkes;
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
        UserModel _user;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private NavigationStore _navigationstore;
        private readonly ErrorViewModel _errorsViewModel;
        private readonly CaseManager _caseManager;
        private readonly Dictionary<Fields, ErrorsModel> _errorlist;
        string _name;
        string _surname;
        string _phone;
        public bool CanOpen => _caseManager == CaseManager.Change;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }
        public string Name
        {
            get
            {
                _name= (_name ?? _user?.Name)??string.Empty  ;
                return _name;
            }
            set
            {
                _name = value;
                ShowErrors(_name, nameof(Name),Fields.Name);
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get
            {
               _surname= (_surname??_user?.Surname  )??string.Empty;
                return _surname;
            } 
            set
            {
               _surname = value;
                ShowErrors(_surname,nameof(Surname),Fields.Surname);
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string Phone
        {
            get
            {
               _phone= (_phone ?? _user?.Phone)??string.Empty  ;
                return _phone;
            } 
            set
            {
                _phone = value;
                ShowErrors(_phone,nameof(Phone), Fields.Phone);
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string ID =>  _user?.ID ?? string.Empty;
        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            _user = new UserModel() {ID=ID,Name=_name,Surname=_surname,Phone=_phone };
            _navigationstore = new NavigationStore();        
            if (_caseManager == CaseManager.Save)
                XmlWorker.AddToXML(_user);
            else
                XmlWorker.ChangeXML(_user);
            GoHome(_navigationstore);
        }

        private bool CanExecuteDeleteCommand(object obj) => true;
        private void ShowErrors(string property, string propetyName, Fields type)
        {
            _errorsViewModel.ClearErrors(nameof(property));
            if (property.Length < 2 && type != Fields.Phone)

                _errorsViewModel.AddError(propetyName, _errorlist[type].MoreSymbols);

            else if (property.Length > 50 && type != Fields.Phone)

                _errorsViewModel.AddError(propetyName, _errorlist[type].LessSymbols);

            else if (property.Any(ch => !Char.IsLetterOrDigit(ch)))

                _errorsViewModel.AddError(propetyName, _errorlist[type].LessSymbols);

            else if (property.Where(Char.IsDigit).ToArray().Length == 11)
                _errorsViewModel.AddError(propetyName, _errorlist[type].Phone);
        }
        private void OnExecuteDeleteCommand(object obj)
        {
            if (MessageBox.Show("Do you want to delete this field?",
                   "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                XmlWorker.DeleteFromXml(ID);
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
            return _errorsViewModel.GetErrors(propertyName);
        }
        public AccountViewModel(NavigationStore navigationstore,UserModel user,CaseManager caseManager)
        {
            _errorlist = new Dictionary<Fields, ErrorsModel>() { {Fields.Name, new ErrorsModel { MoreSymbols = JsonWorker.GetDescription("MoreSymbols", "Name"), LessSymbols = JsonWorker.GetDescription("LessSymbols", "Name"), Incorrectsymbols = JsonWorker.GetDescription("Incorrect Symbols") } },
                { Fields.Surname,new ErrorsModel { MoreSymbols = JsonWorker.GetDescription("MoreSymbols", "Surname"), LessSymbols = JsonWorker.GetDescription("LessSymbols", "Surname"), Incorrectsymbols = JsonWorker.GetDescription("Incorrect Symbols") } },
                {Fields.Phone,new ErrorsModel{Phone=JsonWorker.GetDescription("Phone") } } };
            _errorsViewModel = new ErrorViewModel();
            _errorsViewModel.ErrorsChanged += Changed;
            _user = user;
            _caseManager = caseManager;
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationstore,()=>new HomeViewModel(navigationstore));
            SaveCommand = new RelayCommand(OnExecuteTakeNoteCommand,CanExecuteTakeNoteCommand);
            DeleteCommand = new RelayCommand(OnExecuteDeleteCommand, CanExecuteDeleteCommand);

        }
        private void Changed(object? sender, DataErrorsChangedEventArgs e)
        {
           ErrorsChanged?.Invoke(this, e);
        }
    }
}
