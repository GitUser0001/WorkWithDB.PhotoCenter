using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkWithDB.UI.Helpers;

namespace WorkWithDB.UI.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private UIElement _mainView;
        private UIElement _secondaryView;

        public MainWindowVM()
        {
            WindowManager.SetMainViewModel(this);
        }

        public UIElement MainView
        {
            get
            {
                return _mainView;
            }

            set
            {
                _mainView = value;
                OnPropertyChanged();
            }
        }

        public UIElement SecondaryView
        {
            get
            {
                return _secondaryView;
            }

            set
            {
                _secondaryView = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _customerCommand;
        public ICommand Customer
        {
            get
            {
                if (_customerCommand == null)
                    _customerCommand = new RelayCommand(ExecuteAddPersonalCardCommand, CanExecuteAddPersonalCardCommand);
                return _customerCommand;
            }
        }

        public void ExecuteAddPersonalCardCommand(object parameter)
        {
            WindowManager.ChangeView(parameter as string);
        }

        public bool CanExecuteAddPersonalCardCommand(object parameter)
        {
            return true;
        }
    }
}
