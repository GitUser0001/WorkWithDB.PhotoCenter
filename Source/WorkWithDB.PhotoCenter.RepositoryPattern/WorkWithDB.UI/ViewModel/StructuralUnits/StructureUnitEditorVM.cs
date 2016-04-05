using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.StructuralUnits
{
    public class StructureUnitEditorVM : ViewModelBase
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

        private string _ownerInfo;
        public string OwnerInfo
        {
            get
            {
                return _ownerInfo;
            }

            set
            {
                _ownerInfo = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private int _workers;
        public int Workers
        {
            get
            {
                return _workers;
            }

            set
            {
                _workers = value;
                OnPropertyChanged();
            }
        }

        private bool _isFiliya;

        public bool IsFiliya
        {
            get
            {
                return _isFiliya;
            }

            set
            {
                _isFiliya = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _addStructuralUnitCommand;
        public ICommand AddStructuralUnit
        {
            get
            {
                if (_addStructuralUnitCommand == null)
                    _addStructuralUnitCommand = new RelayCommand(ExecuteAddStructuralUnitCommand, CanExecuteAddStructuralUnitCommand);
                return _addStructuralUnitCommand;
            }
        }

        public void ExecuteAddStructuralUnitCommand(object parameter)
        {
            Model.StructuralUnit strucutreUnit = new Model.StructuralUnit()
            {
                Adress = Address,
                Jobs = Workers,
                Name = Name,
                Opening_Date = DateTime.Now,
                OwnerInfo = OwnerInfo
            };

            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                unitOfWork.StructuralUnitRepository.Save(strucutreUnit);

                if (IsFiliya)
                {
                    Model.Filiya filiya = new Model.Filiya()
                    {
                        StructureUnit = strucutreUnit
                    };

                    unitOfWork.FiliyaRepository.Save(filiya);
                }

                unitOfWork.Commit();
            }

            WindowManager.ChangeMainView(parameter as string);
        }

        public bool CanExecuteAddStructuralUnitCommand(object parameter)
        {
            return IsFiliya && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Address) &&
                    Workers > 0 && !string.IsNullOrWhiteSpace(OwnerInfo);
        }
    }
}
