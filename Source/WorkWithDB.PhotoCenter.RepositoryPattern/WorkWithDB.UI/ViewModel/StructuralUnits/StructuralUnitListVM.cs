using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
                    new Thread(new ThreadStart(GetStructuralUnitList)).Start();
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
            StateHolder.StructureUnitEditing = null;
            WindowManager.ChangeMainView(parameter as string);
        }

        private void GetStructuralUnitList()
        {
            try
            {
                using (var scope = UnitOfWorkFactory.CreateInstance())
                {
                    StructuralUnitList = new ObservableCollection<Model.StructuralUnit>(scope.StructuralUnitRepository.GetAll());
                }
            }
            catch (Exception)
            {
            }
        }

        private RelayCommand _editStructuralUnitCommand;
        public ICommand EditStructuralUnit
        {
            get
            {
                if (_editStructuralUnitCommand == null)
                    _editStructuralUnitCommand = new RelayCommand(ExecuteEditStructuralUnitCommand);
                return _editStructuralUnitCommand;
            }
        }

        private void ExecuteEditStructuralUnitCommand(object parameter)
        {
            StateHolder.StructureUnitEditing = _selectedStUnit;
            WindowManager.ChangeMainView(parameter as string);
        }

        private RelayCommand _deleteStructuralUnit;
        public ICommand DeleteStructuralUnit
        {
            get
            {
                if (_deleteStructuralUnit == null)
                    _deleteStructuralUnit = new RelayCommand(ExecuteDeleteStructuralUnitCommand);
                return _deleteStructuralUnit;
            }
        }
        
        private void ExecuteDeleteStructuralUnitCommand(object parameter)
        {
            try
            {
                using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
                {
                    unitOfWork.StructuralUnitRepository.Delete(SelectedStUnit.Id);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            WindowManager.ChangeMainView(parameter as string);
        }
    }
}
