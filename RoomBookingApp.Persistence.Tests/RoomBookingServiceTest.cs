using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // Arrange
            // (Set up the necessary context and dependencies)
            var date = new DateTime(2025, 10, 17);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvaliableRoomTest").Options;

            using var context = new RoomBookingAppDbContext(dbOptions);

            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });


            context.Add(new RoomBooking { Date = date, RoomId = 1 });
            context.Add(new RoomBooking { Date = date.AddDays(-1), RoomId = 2 });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            // (Call the method to be tested)
            var availableRooms = roomBookingService.GetAvaliableRooms(date);

            // Assert
            // (Verify the results)
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);

            availableRooms.Count().ShouldBe(2);
            availableRooms.ShouldContain(r => r.Name == "Room 2");
            availableRooms.ShouldNotContain(r => r.Name == "Room 1");
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("ShouldSaveTest").Options;

            var roomBooking = new RoomBooking
            {
                Date = new DateTime(2025, 11, 20),
                RoomId = 1
            };

            using var context = new RoomBookingAppDbContext(dbOptions);
            
            var roomBookingService = new RoomBookingService(context);

            // Act
            roomBookingService.Save(roomBooking);

            // Assert
            var savedBooking = context.RoomBookings.FirstOrDefault();
            Assert.NotNull(savedBooking);
            Assert.Equal(roomBooking.Date, savedBooking.Date);
            Assert.Equal(roomBooking.RoomId, savedBooking.RoomId);
        }
    }
}
