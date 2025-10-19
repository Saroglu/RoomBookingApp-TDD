using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private IRoomBookingRequestProcessor _processor;

        public RoomBookingController(IRoomBookingRequestProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public IActionResult BookRoom(RoomBookingRequest roomBookingRequest)
        {
            if (ModelState.IsValid)
            {
                var result = _processor.BookRoom(roomBookingRequest);
                if (result.BookingSuccessFlag == Core.Enums.BookingSuccessFlag.Success)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms available");
            }
            return BadRequest(ModelState);
        }
    }
}
