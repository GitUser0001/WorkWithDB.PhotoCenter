using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract.Repository;

namespace WorkWithDB.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGoodsRepository GoodsRepository { get; }
        IAvailabilityRepository AvailabilityRepository { get; }
        ICameraRentRepository CameraRentRepository { get; }
        ICameraRepository CameraRepository { get; }
        IClientRepository ClientRepository { get; }
        IDeliveryGoodsRepository DeliveryGoodsRepository { get; }
        IDeliveryRepository DeliveryRepository { get; }
        IDiscountCardRepository DiscountCardRepository { get; }
        IDisplayOfPhotoRepository DisplayOfPhotoRepository { get; }
        IFiliyaRepository FiliyaRepository { get; }
        IFilmTypeRepository FilmTypeRepository { get; }
        IGoodsSoldRepository GoodsSoldRepository { get; }
        IKioskRepository KioskRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPaperFormatRepository PaperFormatRepository { get; }
        IPhotoFormatRepository PhotoFormatRepository { get; }
        IPhotographerRepository PhotographerRepository { get; }
        IPhotoServiceRepository PhotoServiceRepository { get; }
        IPrintingRepository PrintingRepository { get; }
        IProviderRepository ProviderRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IServiceTypeRepository ServiceTypeRepository { get; }
        IStructuralUnitRepository StructuralUnitRepository { get; }

        void Commit();
        void RollBack();
    }
}
