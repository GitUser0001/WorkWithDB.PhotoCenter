using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.Customers
{
    public class CustomerRegisterVM : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private bool _isPro;

        public bool IsPro
        {
            get
            {
                return _isPro;
            }

            set
            {
                _isPro = value;
                OnPropertyChanged();
            }
        }
        
        private RelayCommand _addPersonalCardCommand;
        public ICommand AddPersonalCard
        {
            get
            {
                if (_addPersonalCardCommand == null)
                    _addPersonalCardCommand = new RelayCommand(ExecuteAddPersonalCardCommand, CanExecuteAddPersonalCardCommand);
                return _addPersonalCardCommand;
            }
        }

        public void ExecuteAddPersonalCardCommand(object parameter)
        {
            Model.Client client = new Model.Client()
            {
                FullName = LastName + " " + Name,
                IsProfesional = IsPro,
                RegistrationDate = DateTime.Now
            };

            StateHolder.RegistratingClient = client;

            WindowManager.ChangeView(parameter as string);
        }

        public bool CanExecuteAddPersonalCardCommand(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(LastName);
        }

    }
}
