using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {

        private readonly RoomBookingRequestProcessor roomBookingRequestProcessor;
        private readonly RoomBookingRequest roomBookingRequest;
        private readonly Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _avaliableRooms;

        public RoomBookingRequestProcessorTest()
        {
            roomBookingRequest = new RoomBookingRequest
            {
                FullName = "John Doe",
                Email = "jhondoe@request.com",
                Date = new DateTime(2024, 7, 1)
            };
            _avaliableRooms = new List<Room> { new Room { Id = 1 } };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(x => x.GetAvaliableRooms(roomBookingRequest.Date))
                .Returns(_avaliableRooms);

            roomBookingRequestProcessor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnRoomBookingResponseWithRequestValues()
        {
            //Arrange  -- Prepare data , create objects, etc.

            //var roomBookingRequestProcessor = new RoomBookingRequestProcessor();

            //Act -- Call the method to be tested
            RoomBookResult result = roomBookingRequestProcessor.BookRoom(roomBookingRequest);

            //Assert -- Verify the result
            Assert.NotNull(result);
            Assert.Equal(roomBookingRequest.FullName, result.FullName);
            Assert.Equal(roomBookingRequest.Email, result.Email);
            Assert.Equal(roomBookingRequest.Date, result.Date);

            result.ShouldNotBeNull(); // using Shouldly library
            result.FullName.ShouldBe(roomBookingRequest.FullName);
            result.Email.ShouldBe(roomBookingRequest.Email);
            result.Date.ShouldBe(roomBookingRequest.Date);

        }

        [Fact]
        public void ShouldThrowExceptionForNullRequest()
        {
            //Arrange

            //Act & Assert
            var exception = Should.Throw<ArgumentNullException>(() => roomBookingRequestProcessor.BookRoom(null));
            exception.ParamName.ShouldBe("roomBookingRequest");

        }

        [Fact]
        public void ShouldSaveRoomBookingRequest()
        {
            //Arrange
            RoomBooking savedRoomBooking = null;

            //Act
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(rb => savedRoomBooking = rb);
            roomBookingRequestProcessor.BookRoom(roomBookingRequest);

            //Assert
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);
            savedRoomBooking.ShouldNotBeNull();
            savedRoomBooking.FullName.ShouldBe(roomBookingRequest.FullName);
            savedRoomBooking.Email.ShouldBe(roomBookingRequest.Email);
            savedRoomBooking.Date.ShouldBe(roomBookingRequest.Date);
            savedRoomBooking.RoomId.ShouldBe(_avaliableRooms.First().Id);
        }

        [Fact]
        public void ShouldNotSaveRoomBookingRequestIfNoneAvaliable()
        {
            _avaliableRooms.Clear();

            roomBookingRequestProcessor.BookRoom(roomBookingRequest);
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingSuccessFlag.Failure, false)]
        [InlineData(BookingSuccessFlag.Success, true)]
        public void ShouldReturnSuccessOrFailureFlagInResult_Theory(BookingSuccessFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
                _avaliableRooms.Clear();

            RoomBookResult result = roomBookingRequestProcessor.BookRoom(roomBookingRequest);
            bookingSuccessFlag.ShouldBe(result.BookingSuccessFlag);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(1, true)]
        public void ShouldReturnSuccessOrFailureFlagInRoomBookingIdResult_Theory(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
                _avaliableRooms.Clear();
            else
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(rb => rb.Id = roomBookingId.Value);

            RoomBookResult result = roomBookingRequestProcessor.BookRoom(roomBookingRequest);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
