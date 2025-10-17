using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _context;
        public RoomBookingService(RoomBookingAppDbContext context)
        {
            _context = context; 
        }

        public IEnumerable<Room> GetAvaliableRooms(DateTime dateTime)
        {
            //var unAvailableRooms = _context.RoomBookings.Where(rb => rb.Date == dateTime)
            //    .Select(rb => rb.RoomId)
            //    .ToList();
            //var availableRooms = _context.Rooms
            //    .Where(r => !unAvailableRooms.Contains(r.Id))
            //    .ToList();
            //return availableRooms;
            
            return _context.Rooms
                .Where(r => !r.RoomBookings.Any(x=>x.Date == dateTime))
                .ToList();

        }

        public void Save(RoomBooking roomBooking)
        {
            _context.RoomBookings.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
