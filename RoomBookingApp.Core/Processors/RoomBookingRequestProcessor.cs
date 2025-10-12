using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;
        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        public RoomBookResult BookRoom(RoomBookingRequest roomBookingRequest)
        {
            if (roomBookingRequest == null)
            {
                throw new ArgumentNullException(nameof(roomBookingRequest));
            }

            var availableRooms = _roomBookingService.GetAvaliableRooms(roomBookingRequest.Date);

            if (availableRooms.Any())
            {
                _roomBookingService.Save(CreateRoomBookingObject<RoomBooking>(roomBookingRequest));
            }

            return CreateRoomBookingObject<RoomBookResult>(roomBookingRequest);
        }

        private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest roomBookingRequest)
            where TRoomBooking : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                FullName = roomBookingRequest.FullName,
                Email = roomBookingRequest.Email,
                Date = roomBookingRequest.Date
            };
        }
    }
}