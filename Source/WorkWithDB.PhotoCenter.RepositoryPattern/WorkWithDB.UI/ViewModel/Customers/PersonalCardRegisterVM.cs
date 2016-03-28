using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.Customers
{
    public class PersonalCardRegisterVM : ViewModelBase
    {
        private string _cardType;
        public string CardType
        {
            get
            {
                return _cardType;
            }

            set
            {
                _cardType = value;
                OnPropertyChanged();
            }
        }

        private int _discount;
        public int Disount
        {
            get
            {
                return _discount;
            }

            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }

        private int _barCode;
        public int BarCode
        {
            get
            {
                return _barCode;
            }

            set
            {
                _barCode = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _createCustomerCommand;
        public ICommand CreateCustomer
        {
            get
            {
                if (_createCustomerCommand == null)
                    _createCustomerCommand = new RelayCommand(ExecuteCreateCustomerCommand, CanExecuteCreateCustomerCommand);
                return _createCustomerCommand;
            }
        }

        public void ExecuteCreateCustomerCommand(object parameter)
        {
            Model.DiscountCard discountCard = new Model.DiscountCard()
            {
                TypeName = CardType,
                IsPersonal = true,
                Discount = Disount,
                Code = BarCode
            };

            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                unitOfWork.DiscountCardRepository.Save(discountCard);
                unitOfWork.ClientRepository.Save(StateHolder.RegistratingClient);
                unitOfWork.Commit();
                WindowManager.ChangeView(parameter as string);
            }
        }

        public bool CanExecuteCreateCustomerCommand(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CardType) && Disount > 0 && Disount <= 50 && BarCode.ToString().Length == 8;
        }
    }
}
