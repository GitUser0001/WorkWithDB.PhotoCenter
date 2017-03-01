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

        private bool _isPersonal = true;

        public bool IsPersonal
        {
            get
            {
                return _isPersonal;
            }

            set
            {
                _isPersonal = value;
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
            if (StateHolder.RegistratingClient == null)
            {
                throw new InvalidOperationException("Client = null");
            }

            Model.DiscountCard discountCard = new Model.DiscountCard()
            {
                TypeName = CardType,
                IsPersonal = IsPersonal,
                Discount = Disount,
                Code = BarCode
            };

            StateHolder.RegistratingClient.DiscountCard = discountCard;


            try
            {
                using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
                {
                    unitOfWork.DiscountCardRepository.Save(discountCard);
                    unitOfWork.ClientRepository.Save(StateHolder.RegistratingClient);
                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            StateHolder.RegistratingClient = null;
            WindowManager.ChangeMainView(parameter as string);
        }

        public bool CanExecuteCreateCustomerCommand(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CardType) && Disount > 0 && Disount <= 50 && BarCode.ToString().Length == 8;
        }
    }
}
