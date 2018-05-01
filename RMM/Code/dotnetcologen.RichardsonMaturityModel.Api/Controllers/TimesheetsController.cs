using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using FluentSiren.Builders;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers {

    [Route("timesheets")]
    public class TimesheetsController:Controller {
        private readonly IMapper mapper;
        private readonly ITimesheetRepository repostitory;

        public TimesheetsController(ITimesheetRepository repostitory) 
        {
            mapper = new MapperConfiguration(c => {
                c.CreateMap<Timesheet, TimesheetHeader>();
            }).CreateMapper();
            this.repostitory = repostitory;
        }

        [HttpGet]
        [Route("", Name = "GetAll")]
        [Produces("application/vnd.siren+json")]
        [ProducesResponseType(typeof(IEnumerable<TimesheetHeader>), 200)]
        public IActionResult GetAll() 
        {
            var result = repostitory.GetAll();



            return Ok(result);
        }

        [HttpGet]
        [Route("{name}", Name="GetByName")]
        [Produces("application/vnd.siren+json")]
        [ProducesResponseType(typeof(Timesheet), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetByName([FromRoute] string name) 
        {
            if (!repostitory.Exists(name))
                return NotFound();

            var timesheet = repostitory.GetByName(name);

            return Ok(timesheet);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Timesheet), 201)]
        [ProducesResponseType(typeof(void), 409)]
        public IActionResult Create([FromBody] NewTimesheet newTimesheet)
        {
            if (repostitory.Exists(newTimesheet.Name))
                return this.ConflictWithRoute("GetByName", new { name = newTimesheet.Name });

            var timesheet = new Timesheet(newTimesheet.Name);
            repostitory.Save(timesheet);

            return CreatedAtRoute("GetByName", new { name = timesheet.Name }, timesheet);
        }

        [HttpPatch]
        [Route("{name}")]
        [Consumes("application/json-patch+json")] // https://tools.ietf.org/html/rfc6902
        [ProducesResponseType(typeof(Timesheet), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public IActionResult Update([FromRoute] string name, [FromBody] JsonPatchDocument<Timesheet> patchDocument)
        {
            if (!repostitory.Exists(name))
                return NotFound();

            var timesheet = repostitory.GetByName(name);
            patchDocument.ApplyTo(timesheet);

            if (name != timesheet.Name) // Name has changed, check for conflicts
            {
                if (repostitory.Exists(timesheet.Name)) // new name conflicts with exisitng timesheet
                    return this.ConflictWithRoute("GetByName", new { name = timesheet.Name });

                repostitory.Delete(name);
            }

            repostitory.Save(timesheet);
            return Ok(timesheet);
        }
    }

}
