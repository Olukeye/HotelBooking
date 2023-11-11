using HotelBooking.Model;

namespace HotelBooking.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        IGenericRepository<Room> Rooms { get; }

        Task Save();
    }
    /*
       * 
       * IDisposabale is a method of dumping unused collection in form of gabbages and
       * managed object when that object is no longer used.It enhances system performance and free up space.
       * "The Dispose method serves a crucial purpose to free unmanaged resources the Garbage Collector can't handle itself".
    */
}
