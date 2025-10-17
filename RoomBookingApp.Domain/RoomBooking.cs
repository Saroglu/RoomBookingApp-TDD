using RoomBookingApp.Domain.BaseModels;
using System.ComponentModel.DataAnnotations;
namespace RoomBookingApp.Domain
{
    public class RoomBooking : RoomBookingBase
    {
        public int Id { get; set; }

        public Room Room { get; set; }
        public int RoomId { get; set; }

    }
}