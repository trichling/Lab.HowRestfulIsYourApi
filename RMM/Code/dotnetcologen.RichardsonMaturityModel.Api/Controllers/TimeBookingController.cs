using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using Newtonsoft.Json.Linq;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers 
{

    [Route("timesheets/bookings")]
    public class TimeBookingController : Controller
    {

        private readonly ITimesheetRepository repostitory;

        public TimeBookingController(ITimesheetRepository repostitory) 
        {
            this.repostitory = repostitory;
        }

        [HttpPost]
        [Route("execute")]
        [Consumes("application/json")]
        public IActionResult Execute([FromQuery] string operation, [FromBody] JObject parameters)
        {
            switch (operation)
            {
                case "ShowAllTimebookings":
                    var showAllTimebookingsRequest = parameters.ToObject<GetAllTimebookingsRequest>();
                    var timesheet2 = repostitory.GetByName(showAllTimebookingsRequest.Timesheet);
                    return Ok(timesheet2.Bookings);
                
                case "ShowTimebooking":
                    var showTimebookingRequest = parameters.ToObject<GetTimebookingRequest>();
                    var timesheet3 = repostitory.GetByName(showTimebookingRequest.Timesheet);
                    return Ok(timesheet3.GetBookingByDate(showTimebookingRequest.Date));

                case "CreateTimebooking":
                    var createTimebookingRequest = parameters.ToObject<CreateTimebookingsRequest>();
                    var timesheet4 = repostitory.GetByName(createTimebookingRequest.Timesheet);
                    timesheet4.BookTime(createTimebookingRequest.Date, createTimebookingRequest.Start, createTimebookingRequest.Pause, createTimebookingRequest.End);
                    repostitory.Save(timesheet4);
                    return Ok();

                case "UpdateTimebooking":
                    var udpateTimebookingRequest = parameters.ToObject<UpdateimebookingRequest>();
                    var timesheet5 = repostitory.GetByName(udpateTimebookingRequest.Timesheet);
                    timesheet5.Update(udpateTimebookingRequest.Date, new TimeBooking(DateTime.MinValue, udpateTimebookingRequest.Start, udpateTimebookingRequest.Pause, udpateTimebookingRequest.End));
                    repostitory.Save(timesheet5);
                    return Ok();
            }

            return Ok();
        }

        #region "Ignored for RMM Level 1"
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TimeBooking>), 200)]
        public IActionResult GetAll([FromRoute] string name)
        {
            var timesheet = repostitory.GetByName(name);
            return Ok(timesheet.Bookings);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("{date}", Name = "GetByDate")]
        [ProducesResponseType(typeof(TimeBooking), 200)]
        public IActionResult GetByDate([FromRoute] string name, [FromRoute] DateTime date)
        {
            var timesheet = repostitory.GetByName(name);
            return Ok(timesheet.GetBookingByDate(date));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ProducesResponseType(typeof(TimeBooking), 201)]
        public IActionResult Create([FromRoute] string name, [FromBody] TimeBooking booking)
        {
            var timesheet = repostitory.GetByName(name);
            timesheet.BookTime(booking.Date, booking.Start, booking.Pause, booking.End);
            repostitory.Save(timesheet);

            return CreatedAtRoute("GetByDate", new { name = name, date = booking.Date }, timesheet.GetBookingByDate(booking.Date));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut]
        [Route("{date}", Name = "GetByDate")]
        [ProducesResponseType(typeof(TimeBooking), 201)]
        public IActionResult UpdateAll([FromRoute] string name, [FromRoute] DateTime date, [FromBody] TimeBooking booking)
        {
            var timesheet = repostitory.GetByName(name);
            timesheet.Update(date, booking);
            repostitory.Save(timesheet);

            return Ok(timesheet.GetBookingByDate(date));
        }
        #endregion
    }

}