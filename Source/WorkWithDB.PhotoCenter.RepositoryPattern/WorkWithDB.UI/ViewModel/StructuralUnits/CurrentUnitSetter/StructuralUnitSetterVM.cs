using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.StructuralUnits.CurrentUnitSetter
{
    public class StructuralUnitSetterVM : ViewModelBase
    {
        private Model.StructuralUnit _structuralUnit;

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

        private RelayCommand _setStUnitCommand;
        public ICommand SetStUnit
        {
            get
            {
                if (_setStUnitCommand == null)
                    _setStUnitCommand = new RelayCommand(ExecuteSetStUnitCommand, CanExecuteSetStUnitCommand);
                return _setStUnitCommand;
            }
        }

        public void ExecuteSetStUnitCommand(object parameter)
        {
            try
            {
                using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
                {
                    _structuralUnit = unitOfWork.StructuralUnitRepository.GetByID(Id);
                }
                
                if (_structuralUnit == null)
                {
                    throw new ArgumentOutOfRangeException("There is no structural unit with id: " + Id + "\nCheck your input");
                }

                StateHolder.StructuteUnitCurrent = _structuralUnit;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }                                   
        }

        public bool CanExecuteSetStUnitCommand(object parameter)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return false;
            }
            
            return _id != 0;
        }
    }
}