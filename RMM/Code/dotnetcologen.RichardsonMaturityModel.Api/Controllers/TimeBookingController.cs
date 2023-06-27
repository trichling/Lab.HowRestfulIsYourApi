using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Hal;
using Hal.AspNetCore;
using Hal.Builders;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers 
{

    [ServiceFilter(typeof(SupportsHalAttribute))]
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
            var bookingsModel = timesheet.Bookings.Select(b => new { b.Date, b.Duration });

            var response = new ResourceBuilder()
                .WithState(new { Count = bookingsModel.Count() })
                .AddSelfLink().WithLinkItem($"/timesheets/{name}/bookings")
                .AddEmbedded("bookings").Resource(new ResourceBuilder()
                    .WithState(bookingsModel)
                    .AddSelfLink().WithLinkItem("/timesheets/" + name + "/bookings/{date}", templated: true)
                );

            return Ok(response);
        }

        [HttpGet]
        [Route("{date}", Name = "GetByDate")]
        [ProducesResponseType(typeof(TimeBooking), 200)]
        public IActionResult GetByDate([FromRoute] string name, [FromRoute] DateTime date)
        {
            var timesheet = repostitory.GetByName(name);
            var booking = timesheet.GetBookingByDate(date);

            var response = new ResourceBuilder()
                .WithState(booking)
                .AddSelfLink().WithLinkItem($"/timesheets/{name}/bookings/{date}");

            return Ok(response);
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