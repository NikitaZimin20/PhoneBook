using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.ViewModels
{
    internal class ErrorViewModel: INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private readonly Dictionary<string, List<string>> _propertyyErrors = new Dictionary<string, List<string>>() { };
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyyErrors.GetValueOrDefault(propertyName, null);
        }
        public void ClearErrors(string propertyname)
        {
            if (_propertyyErrors.Remove(propertyname))

                OnErrorsChanged(propertyname);
        }
        public bool HasErrors => _propertyyErrors.Any();
        public void AddError(string propertyName, string errorMessage)
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
          
        }

    }
}
