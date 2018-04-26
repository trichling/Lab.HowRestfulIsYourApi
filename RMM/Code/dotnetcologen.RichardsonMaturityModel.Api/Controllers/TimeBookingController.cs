using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers 
{

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("timesheets/{name}/bookings")]
    public class TimeBookingController : Controller
    {

        private readonly ITimesheetRepository repostitory;

        public TimeBookingController(ITimesheetRepository repostitory) 
        {
            this.repostitory = repostitory;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TimeBooking>), 200)]
        public IActionResult GetAll([FromRoute] string name)
        {
            var timesheet = repostitory.GetByName(name);
            return Ok(timesheet.Bookings);
        }

        [HttpGet]
        [Route("{date}", Name = "GetByDate")]
        [ProducesResponseType(typeof(TimeBooking), 200)]
        public IActionResult GetByDate([FromRoute] string name, [FromRoute] DateTime date)
        {
            var timesheet = repostitory.GetByName(name);
            return Ok(timesheet.GetBookingByDate(date));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TimeBooking), 201)]
        public IActionResult Create([FromRoute] string name, [FromBody] TimeBooking booking)
        {
            var timesheet = repostitory.GetByName(name);
            timesheet.BookTime(booking.Date, booking.Start, booking.Pause, booking.End);
            repostitory.Save(timesheet);

            return CreatedAtRoute("GetByDate", new { name = name, date = booking.Date }, timesheet.GetBookingByDate(booking.Date));
        }

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
    }

}