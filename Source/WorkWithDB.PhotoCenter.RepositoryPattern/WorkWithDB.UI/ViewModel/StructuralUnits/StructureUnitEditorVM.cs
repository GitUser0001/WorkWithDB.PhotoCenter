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
        private Model.StructuralUnit existedStUnit;

        public StructureUnitEditorVM()
        {
            existedStUnit = StateHolder.StructureUnitEditing;
            
            if (existedStUnit != null)
            {
                UpdateViewWithExistedObj(existedStUnit);
            }
        }

        private void UpdateViewWithExistedObj(Model.StructuralUnit stUnit)
        {
            existedStUnit = stUnit;
            _name = stUnit.Name;
            _ownerInfo = stUnit.OwnerInfo;
            _address = stUnit.Adress;
            _workers = stUnit.Jobs;
            _isFiliya = true;
        }

        private void UpdateObjWithViewData(ref Model.StructuralUnit stUnit)
        {
            stUnit.Adress = Address;
            stUnit.Jobs = Workers;
            stUnit.Name = Name;
            stUnit.Opening_Date = stUnit.Opening_Date != null ? stUnit.Opening_Date : DateTime.Now;
            stUnit.OwnerInfo = OwnerInfo;
        }

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
                if (existedStUnit == null)
                {
                    _isFiliya = value;
                    OnPropertyChanged();
                }                
            }
        }

        private int _filiyaId;
        public int FiliyaId
        {
            get
            {
                return _filiyaId;
            }

            set
            {
                _filiyaId = value;
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

        private void ExecuteAddStructuralUnitCommand(object parameter)
        {
            Model.StructuralUnit strucutraUnit = existedStUnit ?? new Model.StructuralUnit();
            UpdateObjWithViewData(ref strucutraUnit);            
             
            try
            {
                using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
                {
                    if (existedStUnit != null)
                    {
                        unitOfWork.StructuralUnitRepository.SaveOrUpdate(strucutraUnit);
                    }
                    else
                    {
                        unitOfWork.StructuralUnitRepository.Save(strucutraUnit);
                    }

                    if (IsFiliya)
                    {
                        Model.Filiya filiya = new Model.Filiya()
                        {
                            Id = strucutraUnit.Id,
                            StructureUnit = strucutraUnit
                        };

                        if (existedStUnit != null)
                        {
                            unitOfWork.FiliyaRepository.SaveOrUpdate(filiya);
                        }
                        else
                        {
                            unitOfWork.FiliyaRepository.Save(filiya);
                        }                        
                    }
                    else
                    {
                        var filiya = unitOfWork.FiliyaRepository.GetByID(FiliyaId);

                        if (filiya != null)
                        {
                            Model.Kiosk kiosk = new Model.Kiosk()
                            {
                                Filiya = filiya,
                                StructureUnit = strucutraUnit
                            };

                            unitOfWork.KioskRepository.Save(kiosk);
                        }
                        else
                        {
                            throw new ArgumentException("No filia with id : " + FiliyaId);
                        }
                    }

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }

            WindowManager.ChangeMainView(parameter as string);
        }

        private bool CanExecuteAddStructuralUnitCommand(object parameter)
        {
            if (IsFiliya)
            {
                return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Address) &&
                    Workers > 0 && !string.IsNullOrWhiteSpace(OwnerInfo);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Address) &&
                                Workers > 0 && !string.IsNullOrWhiteSpace(OwnerInfo);  
            }
        }
    }
}
