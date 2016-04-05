using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.UI.Helpers;

namespace WorkWithDB.UI.ViewModel.StructuralUnits.CurrentUnitSetter
{
    public class StructuralUnitViewerVM : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _changeStUnitCommand;
        public ICommand ChangeStUnit
        {
            get
            {
                if (_changeStUnitCommand == null)
                    _changeStUnitCommand = new RelayCommand(ExecuteChangeStUnitCommand, CanExecuteChangeStUnitCommand);
                return _changeStUnitCommand;
            }
        }

        public void ExecuteChangeStUnitCommand(object parameter)
        {            
            new WorkWithDB.UI.Views.StructuralUnits.CurrentUnitSetter.StructuralUnitSetter().ShowDialog();
            
            if (StateHolder.StructuteUnit != null)
            {
                Id = StateHolder.StructuteUnit.Id;                
            }
        }

        public bool CanExecuteChangeStUnitCommand(object parameter)
        {
            return true;
        }
    }
}
