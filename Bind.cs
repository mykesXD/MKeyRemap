using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRemap
{
    public class Bind : INotifyPropertyChanged
    {
        public bool _bindEdit;
        public bool _bindDelete;
        public bool _keyBind;
        public bool BinderEdit {
            get
            {
                return _bindEdit;
            }
            set
            {
                _bindEdit = value;
                OnPropertyChanged(nameof(BinderEdit));
            }
        }
        public bool BinderDelete
        {
            get
            {
                return _bindDelete;
            }
            set
            {
                _bindDelete = value;
                OnPropertyChanged(nameof(BinderDelete));
            }
        }
        public bool BinderKey
        {
            get
            {
                return _keyBind;
            }
            set
            {
                _keyBind = value;
                OnPropertyChanged(nameof(BinderKey));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
