using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.StructuralUnits
{
    public class StructuralUnitListVM : ViewModelBase
    {
        private ObservableCollection<Model.StructuralUnit> _structuralUnitList;
        public ObservableCollection<Model.StructuralUnit> StructuralUnitList
        {
            get
            {
                if (_structuralUnitList == null)
	            {
		            using (var scope = UnitOfWorkFactory.CreateInstance())
                    {
                        _structuralUnitList = new ObservableCollection<Model.StructuralUnit>(scope.StructuralUnitRepository.GetAll());
                    }  
                }

                return _structuralUnitList;	            
            }
            set
            {
                _structuralUnitList = value;
                OnPropertyChanged();
            }
        }

        private Model.StructuralUnit _selectedStUnit;
        public Model.StructuralUnit SelectedStUnit
        {
            get
            {
                return _selectedStUnit;
            }
            set
            {
                _selectedStUnit = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _addStructuralUnitCommand;
        public ICommand AddStructuralUnit
        {
            get
            {
                if (_addStructuralUnitCommand == null)
                    _addStructuralUnitCommand = new RelayCommand(ExecuteAddStructuralUnitCommand);
                return _addStructuralUnitCommand;
            }
        }

        public void ExecuteAddStructuralUnitCommand(object parameter)
        {
            WindowManager.ChangeMainView(parameter as string);
        }
    }
}
