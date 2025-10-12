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
            var result = CreateRoomBookingObject<RoomBookResult>(roomBookingRequest);
            if (availableRooms.Any())
            {
                var room = availableRooms.First();
                var roomBooking = CreateRoomBookingObject<RoomBooking>(roomBookingRequest);
                roomBooking.RoomId = room.Id;

                _roomBookingService.Save(roomBooking);

                result.RoomBookingId = roomBooking.Id;
                result.BookingSuccessFlag = Enums.BookingSuccessFlag.Success;
            }else
            {
                result.BookingSuccessFlag = Enums.BookingSuccessFlag.Failure;
            }

            return result;
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