using PhoneBook.FileWorkes;
using PhoneBook.Services;
using SwitchingViews.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.Models
{
    public class UserModel: INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _id;
        private string _name;
        private string _surname;
        private string _phone;
        private readonly ErrorViewModel _errorsViewModel;
        private readonly Dictionary<Fields, ErrorsModel> _errorlist;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public UserModel()
        {
            _errorlist = new Dictionary<Fields, ErrorsModel>() { {Fields.Name, new ErrorsModel { MoreSymbols = JsonWorker.GetDescription("MoreSymbols", "Name"), LessSymbols = JsonWorker.GetDescription("LessSymbols", "Name"), Incorrectsymbols = JsonWorker.GetDescription("Incorrect Symbols") } },
                { Fields.Surname,new ErrorsModel { MoreSymbols = JsonWorker.GetDescription("MoreSymbols", "Surname"), LessSymbols = JsonWorker.GetDescription("LessSymbols", "Surname"), Incorrectsymbols = JsonWorker.GetDescription("Incorrect Symbols") } },
                {Fields.Phone,new ErrorsModel{Phone=JsonWorker.GetDescription("Phone") } } };
            _errorsViewModel = new ErrorViewModel();
            _errorsViewModel.ErrorsChanged += Changed;
        }

        private void Changed(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
              
                OnPropertyChanged("ID");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ShowErrors(value, nameof(Name),Fields.Name);
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                ShowErrors(value,nameof(Surname),Fields.Surname);
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                ShowErrors(value,nameof(Phone),Fields.Phone);
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public bool StartWith(string pattern)
        {
            if (ID.StartsWith(pattern)) return true;
            if (Name.StartsWith(pattern)) return true;
            if (Surname.StartsWith(pattern)) return true;
            if (Phone.StartsWith(pattern)) return true;


            return false;


        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
        private void ShowErrors(string property, string propetyName, Fields type)
        {
            _errorsViewModel.ClearErrors(nameof(property));
            if (property.Length < 2 && type != Fields.Phone)

                _errorsViewModel.AddError(propetyName, _errorlist[type].MoreSymbols);

            else if (property.Length > 50 && type != Fields.Phone)

                _errorsViewModel.AddError(propetyName, _errorlist[type].LessSymbols);

            else if (property.Any(ch => !Char.IsLetterOrDigit(ch))&&type!=Fields.Phone)

                _errorsViewModel.AddError(propetyName, _errorlist[type].LessSymbols);

            else if (property.Where(Char.IsDigit).ToArray().Length != 11 &&type==Fields.Phone)
                _errorsViewModel.AddError(propetyName, _errorlist[type].Phone);

        }
    }
}
