using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingControllerTests
    {
        private Mock<IRoomBookingRequestProcessor> _processor;
        private RoomBookingController _controller;
        private RoomBookingRequest _roomBookingRequest;
        private RoomBookResult _roomBookResult;

        public RoomBookingControllerTests()
        {
            _processor = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_processor.Object);
            _roomBookingRequest = new RoomBookingRequest();
            _roomBookResult = new RoomBookResult();

            _processor.Setup(x => x.BookRoom(_roomBookingRequest))
                .Returns(_roomBookResult);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult))]
        [InlineData(0, false, typeof(BadRequestObjectResult))]
        public async Task ShouldReturnOkOrBadRequestWhenValidResult(int expectedMethodCalls, bool isModelValid, Type expectedResultType)
        {
            // Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");
            }

            // Act
            var result = await _controller.BookRoom(_roomBookingRequest);

            // Assert
            result.ShouldBeOfType(expectedResultType);
            _processor.Verify(x => x.BookRoom(_roomBookingRequest), Times.Exactly(expectedMethodCalls));
        }
    }
}
