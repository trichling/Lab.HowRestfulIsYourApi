using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;
using Gach.CollectionJson.Model.Newtonsoft;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers 
{

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
            var template = new Template();
            template.Data.Add(new DataElement("date") { Prompt = "Date" });
            template.Data.Add(new DataElement("start") { Prompt = "Start" });
            template.Data.Add(new DataElement("pause") { Prompt = "Pause" });
            template.Data.Add(new DataElement("end") { Prompt = "End" });

            var response = new Collection(new Uri($"/timesheets/{name}/bookings", UriKind.Relative))
            {
                Template = template,
                Items = timesheet.Bookings.Select(b => new Item(new Uri($"/timesheets/{name}/bookings/{b.Date}", UriKind.Relative)) {
                    Data = new List<DataElement>() { new DataElement("Date") { Value = b.Date.ToString() }, new DataElement("Duration") { Value = b.Duration.ToString() } }
                }).ToList()
            };


            return Ok(response);
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
        public IActionResult Create([FromRoute] string name, [FromBody] TimebookingTemplate booking)
        {
            var date = DateTime.Parse(booking.Template.Data.Single(d => d.Name == "date").Value);
            var start = DateTime.Parse(booking.Template.Data.Single(d => d.Name == "start").Value);
            var pause = TimeSpan.Parse(booking.Template.Data.Single(d => d.Name == "pause").Value);
            var end = DateTime.Parse(booking.Template.Data.Single(d => d.Name == "end").Value);

            var timesheet = repostitory.GetByName(name);
            timesheet.BookTime(date, start, pause, end);
            repostitory.Save(timesheet);

            return CreatedAtRoute("GetByDate", new { name = name, date = date}, timesheet.GetBookingByDate(date));
        }

        [HttpPut]
        [Route("{date}")]
        [ProducesResponseType(typeof(TimeBooking), 201)]
        public IActionResult UpdateAll([FromRoute] string name, [FromRoute] DateTime date, [FromBody] TimebookingTemplate booking)
        {
            var start = DateTime.Parse(booking.Template.Data.Single(d => d.Name == "start").Value);
            var pause = TimeSpan.Parse(booking.Template.Data.Single(d => d.Name == "pause").Value);
            var end = DateTime.Parse(booking.Template.Data.Single(d => d.Name == "end").Value);

            var timesheet = repostitory.GetByName(name);
            timesheet.Update(date, start, pause, end);
            repostitory.Save(timesheet);

            return Ok(timesheet.GetBookingByDate(date));
        }
    }

}