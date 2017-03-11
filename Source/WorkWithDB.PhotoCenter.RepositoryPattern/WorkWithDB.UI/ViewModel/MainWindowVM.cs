using System.Windows;
using System.Windows.Input;
using WorkWithDB.UI.Helpers;

namespace WorkWithDB.UI.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private UIElement _mainView;
        private UIElement _secondaryView;
        private UIElement _infoView;

        public MainWindowVM()
        {
            _infoView = new Views.StructuralUnits.CurrentUnitSetter.StructuralUnitViewer();
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

        public UIElement InfoVeiew
        {
            get
            {
                return _infoView;
            }

            set
            {
                _infoView = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _changeMainViewCommand;
        public ICommand ChangeMainView
        {
            get
            {
                if (_changeMainViewCommand == null)
                    _changeMainViewCommand = new RelayCommand(ExecuteAddPersonalCardCommand);
                return _changeMainViewCommand;
            }
        }

        public void ExecuteAddPersonalCardCommand(object parameter)
        {
            WindowManager.ChangeMainView(parameter as string);
        }
    }
}
