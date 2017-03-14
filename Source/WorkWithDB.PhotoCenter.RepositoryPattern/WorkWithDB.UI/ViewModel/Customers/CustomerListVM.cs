using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.UI.Helpers;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI.ViewModel.Customers
{
    public class CustomerListVM : ViewModelBase
    {
        private ObservableCollection<Model.Client> _customersList;
        public ObservableCollection<Model.Client> CustomersList
        {
            get
            {
                if (_customersList == null)
                {
                    new Thread(new ThreadStart(GetCustomersUnitList)).Start();
                }

                return _customersList;
            }
            set
            {
                _customersList = value;
                OnPropertyChanged();
            }
        }

        private void GetCustomersUnitList()
        {
            try
            {
                using (var scope = UnitOfWorkFactory.CreateInstance())
                {
                    CustomersList = new ObservableCollection<Model.Client>(scope.ClientRepository.GetAll());
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

    }
}
