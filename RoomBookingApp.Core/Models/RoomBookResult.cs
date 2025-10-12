using RoomBookingApp.Core.Enums;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookResult : RoomBookingBase
    {
        public BookingSuccessFlag BookingSuccessFlag { get; set; }

        public int? RoomBookingId { get; set; }
    }
}