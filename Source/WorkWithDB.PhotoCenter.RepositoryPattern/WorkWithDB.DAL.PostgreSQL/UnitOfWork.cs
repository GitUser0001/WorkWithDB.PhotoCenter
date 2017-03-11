using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using Npgsql;
using WorkWithDB.DAL.PostgreSQL.Repository;
using System.Configuration;

namespace WorkWithDB.DAL.PostgreSQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        private IGoodsRepository _goodsRepository;
        private IAvailabilityRepository _availabilityRepository;
        private ICameraRentRepository _cameraRentRepository;
        private ICameraRepository _cameraRepository;
        private IClientRepository _clientRepository;
        private IDeliveryGoodsRepository _deliveryGoodsRepository;
        private IDeliveryRepository _deliveryRepository;
        private IDiscountCardRepository _discountCardRepository;
        private IDisplayOfPhotoRepository _displayOfPhotoRepository;
        private IFiliyaRepository _filiyaRepository;
        private IFilmTypeRepository _filmTypeRepository;
        private IGoodsSoldRepository _goodsSoldRepository;
        private IKioskRepository _kioskRepository;
        private IOrderRepository _orderRepository;
        private IPaperFormatRepository _paperFormatRepository;
        private IPhotoFormatRepository _photoFormatRepository;
        private IPhotographerRepository _photographerRepository;
        private IPhotoServiceRepository _photoServiceRepository;
        private IPrintingRepository _printingRepository;
        private IProviderRepository _providerRepository;
        private IServiceRepository _serviceRepository;
        private IServiceTypeRepository _serviceTypeRepository;
        private IStructuralUnitRepository _structuralUnitRepository;

        public UnitOfWork(string address = null)
        {
            var connectionAddress = address ?? ConfigurationManager.ConnectionStrings["DefaultAddress"].ConnectionString;
            var connectionString = connectionAddress + ConfigurationManager.ConnectionStrings["DbInfo"].ConnectionString;

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }
     

        public IAvailabilityRepository AvailabilityRepository
        {
            get
            {
                if (_availabilityRepository == null)
                {
                    _availabilityRepository = new AvailabilityRepository(_connection, _transaction);
                }

                return _availabilityRepository;
            }
        }

        public ICameraRentRepository CameraRentRepository
        {
            get
            {
                if (_cameraRentRepository == null)
                {
                    _cameraRentRepository = new CameraRentRepository(_connection, _transaction);
                }

                return _cameraRentRepository;
            }
        }

        public ICameraRepository CameraRepository
        {
            get
            {
                if (_cameraRepository == null)
                {
                    _cameraRepository = new CameraRepository(_connection, _transaction);
                }

                return _cameraRepository;
            }
        }

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_connection, _transaction);
                }

                return _clientRepository;
            }
        }

        public IDeliveryGoodsRepository DeliveryGoodsRepository
        {
            get
            {
                if (_deliveryGoodsRepository == null)
                {
                    _deliveryGoodsRepository = new DeliveryGoodsRepository(_connection, _transaction);
                }

                return _deliveryGoodsRepository;
            }
        }

        public IDeliveryRepository DeliveryRepository
        {
            get
            {
                if (_deliveryRepository == null)
                {
                    _deliveryRepository = new DeliveryRepository(_connection, _transaction);
                }

                return _deliveryRepository;
            }
        }

        public IDiscountCardRepository DiscountCardRepository
        {
            get
            {
                if (_discountCardRepository == null)
                {
                    _discountCardRepository = new DiscountCardRepository(_connection, _transaction);
                }

                return _discountCardRepository;
            }
        }

        public IDisplayOfPhotoRepository DisplayOfPhotoRepository
        {
            get
            {
                if (_displayOfPhotoRepository == null)
                {
                    _displayOfPhotoRepository = new DisplayOfPhotoRepository(_connection, _transaction);
                }

                return _displayOfPhotoRepository;
            }
        }

        public IFiliyaRepository FiliyaRepository
        {
            get
            {
                if (_filiyaRepository == null)
                {
                    _filiyaRepository = new FiliyaRepository(_connection, _transaction);
                }

                return _filiyaRepository;
            }
        }

        public IFilmTypeRepository FilmTypeRepository
        {
            get
            {
                if (_filmTypeRepository == null)
                {
                    _filmTypeRepository = new FilmTypeRepository(_connection, _transaction);
                }

                return _filmTypeRepository;
            }
        }

        public IGoodsSoldRepository GoodsSoldRepository
        {
            get
            {
                if (_goodsSoldRepository == null)
                {
                    _goodsSoldRepository = new GoodsSoldRepository(_connection, _transaction);
                }

                return _goodsSoldRepository;
            }
        }

        public IKioskRepository KioskRepository
        {
            get
            {
                if (_kioskRepository == null)
                {
                    _kioskRepository = new KioskRepository(_connection, _transaction);
                }

                return _kioskRepository;
            }        
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_connection, _transaction);
                }

                return _orderRepository;
            }        
        }

        public IPaperFormatRepository PaperFormatRepository
        {
            get
            {
                if (_paperFormatRepository == null)
                {
                    _paperFormatRepository = new PaperFormatRepository(_connection, _transaction);
                }

                return _paperFormatRepository;
            }        
        }

        public IPhotoFormatRepository PhotoFormatRepository
        {
            get
            {
                if (_photoFormatRepository == null)
                {
                    _photoFormatRepository = new PhotoFormatRepository(_connection, _transaction);
                }

                return _photoFormatRepository;
            }        
        }

        public IPhotographerRepository PhotographerRepository
        {
            get
            {
                if (_photographerRepository == null)
                {
                    _photographerRepository = new PhotographerRepository(_connection, _transaction);
                }

                return _photographerRepository;
            }        
        }

        public IPhotoServiceRepository PhotoServiceRepository
        {
            get
            {
                if (_photoServiceRepository == null)
                {
                    _photoServiceRepository = new PhotoServiceRepository(_connection, _transaction);
                }

                return _photoServiceRepository;
            }        
        }

        public IPrintingRepository PrintingRepository
        {
            get
            {
                if (_printingRepository == null)
                {
                    _printingRepository = new PrintingRepository(_connection, _transaction);
                }

                return _printingRepository;
            }        
        }

        public IProviderRepository ProviderRepository
        {
            get
            {
                if (_providerRepository == null)
                {
                    _providerRepository = new ProviderRepository(_connection, _transaction);
                }

                return _providerRepository;
            }        
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository == null)
                {
                    _serviceRepository = new ServiceRepository(_connection, _transaction);
                }

                return _serviceRepository;
            }        
        }

        public IServiceTypeRepository ServiceTypeRepository
        {
            get
            {
                if (_serviceTypeRepository == null)
                {
                    _serviceTypeRepository = new ServiceTypeRepository(_connection, _transaction);
                }

                return _serviceTypeRepository;
            }        
        }

        public IStructuralUnitRepository StructuralUnitRepository
        {
            get
            {
                if (_structuralUnitRepository == null)
                {
                    _structuralUnitRepository = new StructuralUnitRepository(_connection, _transaction);
                }

                return _structuralUnitRepository;
            }        
        }

        public IGoodsRepository GoodsRepository
        {
            get
            {
                if (_goodsRepository == null)
                {
                    _goodsRepository = new GoodsRepository(_connection, _transaction);
                }

                return _goodsRepository;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
            }
            finally
            {
                _connection.Dispose();
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }
    }
}
