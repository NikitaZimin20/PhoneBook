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
    {   private readonly HomeViewModel _homeviewmodel;
        public UserModel User => _homeviewmodel.SelectedUser;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private NavigationStore _navigationstore;
        public bool CanOpen => User!=null;
      
        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        private bool CanExecuteTakeNoteCommand(object p) => true;
        private void OnExecuteTakeNoteCommand(object p)
        {
            
            _navigationstore = new NavigationStore();        
            if (User==null)
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
        public AccountViewModel(NavigationStore navigationstore)
        {
           _homeviewmodel= (HomeViewModel)navigationstore.CurrentViewModel;
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationstore,()=>new HomeViewModel(navigationstore));
            SaveCommand = new RelayCommand(OnExecuteTakeNoteCommand,CanExecuteTakeNoteCommand);
            DeleteCommand = new RelayCommand(OnExecuteDeleteCommand, CanExecuteDeleteCommand);

        }

       
    }
}
