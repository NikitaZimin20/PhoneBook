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
    internal class AccountViewModel : ViewModelBase
    {
        public UserModel User { get; }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private NavigationStore _navigationstore;
       
        private readonly CaseManager _caseManager;
       
      
        public bool CanOpen => _caseManager == CaseManager.Change;
      
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }
        
        
        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            
            _navigationstore = new NavigationStore();        
            if (_caseManager == CaseManager.Save)
                XmlWorker.AddToXML(User);
            else
                XmlWorker.ChangeXML(User);
            GoHome(_navigationstore);
        }

        private bool CanExecuteDeleteCommand(object obj) => true;
       
        private void OnExecuteDeleteCommand(object obj)
        {
            if (MessageBox.Show("Do you want to delete this field?",
                   "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                XmlWorker.DeleteFromXml(User.ID);
                _navigationstore = new NavigationStore();
                GoHome(_navigationstore);
            }
               
        }
        private void GoHome(NavigationStore navigationStore)
        {
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
            NavigateHomeCommand.Execute(navigationStore.CurrentViewModel);
        }
        
       
        public AccountViewModel(NavigationStore navigationstore,UserModel user,CaseManager caseManager)
        {
           
           
            User = user ?? new UserModel();
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
