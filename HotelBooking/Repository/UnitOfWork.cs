using HotelBooking.Model;
using HotelBooking.IRepository;
using HotelBooking.Data;

namespace HotelBooking.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;
        private IGenericRepository<Room> _rooms;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);

        public IGenericRepository<Room> Rooms => _rooms ??= new GenericRepository<Room>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
    /* 
       * Unit of work is a way of grouping all your business logic in one or single transaction!!
       * "The unit of work design pattern guarantees data integrity and consistency in applications. 
       * It ensures that all changes made to multiple objects in the application are committed to the
       * database or rolled back.It provides an organized and consistent way to manage database changes
       * and transactions".
   */
}
