using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.PostgreSQL;
using WorkWithDB.UI.Helpers;
using WorkWithDB.UI.Helpers.TextValidations;

namespace WorkWithDB.UI.ViewModel.Settings
{
    public class SettingsVM : ViewModelBase
    {
        private string _ip;
        public string Ip
        {
            get
            {
                return _ip;
            }

            set
            {
                _ip = value;
                OnPropertyChanged();
            }
        }

        private string _port = "5432";
        public string Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _saveSettings;
        public ICommand SaveSettings
        {
            get
            {
                if (_saveSettings == null)
                    _saveSettings = new RelayCommand(ExecuteSaveSettingsCommand, CanExecuteSaveSettingsCommand);
                return _saveSettings;
            }
        }

        public void ExecuteSaveSettingsCommand(object parameter)
        {
            try
            {
                string address = string.Format("Server={0};Port={1};", Ip, Port);
                UnitOfWorkFactory.__Initialize(() => new UnitOfWork(address));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public bool CanExecuteSaveSettingsCommand(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Ip) &&
                   !string.IsNullOrWhiteSpace(Port) &&
                   SettingsValidator.Ip(Ip) &&
                   SettingsValidator.Port(Port);
        }
    }
}
