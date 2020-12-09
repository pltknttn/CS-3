using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderScheduler.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        #region INotifyDataErrorInfo  
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
             ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        public void AddError(string propertyName, string error)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors.Add(propertyName, new List<string> { error });
            else
                _validationErrors[propertyName].Add(error);

            OnErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            if (!_validationErrors.ContainsKey(propertyName)) return;
            
            _validationErrors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }
        #endregion
    }
}
