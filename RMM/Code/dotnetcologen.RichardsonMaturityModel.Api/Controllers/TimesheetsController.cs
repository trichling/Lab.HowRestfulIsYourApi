using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Gach.CollectionJson.Model.Newtonsoft;
using System.Linq;
using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers {

    [Route("timesheets")]
    public class TimesheetsController:Controller {
        private readonly ITimesheetRepository repostitory;

        public TimesheetsController(ITimesheetRepository repostitory) 
        {
            this.repostitory = repostitory;
        }

        [HttpGet]
        [Produces("application/vnd.collection+json")]
        [ProducesResponseType(typeof(IEnumerable<Timesheet>), 200)]
        public IActionResult GetAll() 
        {
            var timesheets = repostitory.GetAll();

            var template = new Template();
            template.Data.Add(new DataElement("name") { Prompt = "Name" });

            //var timesheetItems = timesheets.Select(t => new Item<Timesheet, Link>() {  })
            var response = new Collection(new Uri("/timesheets", UriKind.Relative))
            {
                Template = template,
                Items = timesheets.Select(t => new Item(new Uri($"/timesheets/{t.Name}", UriKind.Relative)) {
                    Data = new List<DataElement>() { new DataElement("Id") { Value = t.Id.ToString() }, new DataElement("Name") { Value = t.Name } }
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{name}", Name="GetByName")]
        [ProducesResponseType(typeof(Timesheet), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetByName([FromRoute] string name) 
        {
            if (!repostitory.Exists(name))
                return NotFound();

            return Ok(repostitory.GetByName(name));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Timesheet), 201)]
        [ProducesResponseType(typeof(void), 409)]
        public IActionResult Create([FromBody] TimesheetTemplate newTimesheetTemplate)
        {
            var tempalteName = newTimesheetTemplate.Template.Data.Single(d => d.Name == "name").Value;
            if (repostitory.Exists(tempalteName))
                return this.ConflictWithRoute("GetByName", new { name = tempalteName });

            var timesheet = new Timesheet(tempalteName);
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
