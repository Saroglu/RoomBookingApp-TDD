using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public interface IRoomBookingRequestProcessor
    {
        RoomBookResult BookRoom(RoomBookingRequest roomBookingRequest);
    }
}