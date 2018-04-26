using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;
using dotnetCologne.RichardsonMaturityModel.Api.Repositories;
using dotnetCologne.RichardsonMaturityModel.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;
using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Controllers {

    [Route("timesheets")]
    public class TimesheetsController:Controller {
        private readonly ITimesheetRepository repostitory;

        public TimesheetsController(ITimesheetRepository repostitory) 
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
                case "ShowAllTimesheets":
                    return Ok(repostitory.GetAll());
                
                case "ShowTimeSheet":
                    var getTimesheetRequest = parameters.ToObject<GetTimesheetRequest>();
                    return Ok(repostitory.GetByName(getTimesheetRequest.Name));

                case "CreateTimeSheet":
                    var createTimesheetRequest = parameters.ToObject<CreateTimesheetRequest>();
                    var timesheet = new Timesheet(createTimesheetRequest.Name);
                    repostitory.Save(timesheet);
                    return Ok(timesheet);

                case "RenameTimeSheet":
                    var renameTimeSheetRequest = parameters.ToObject<RenameTimesheetReqeust>();
                    var timesheet1 = repostitory.GetByName(renameTimeSheetRequest.TimesheetToRename);
                    timesheet1.Name = renameTimeSheetRequest.NewName;
                    repostitory.Delete(renameTimeSheetRequest.TimesheetToRename);
                    repostitory.Save(timesheet1);
                    return Ok();
            }

            return Ok();
        }


        #region "Ignored for RMM Level 1"
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Timesheet>), 200)]
        public IActionResult GetAll() 
        {
            return Ok(repostitory.GetAll());
        }

        [ApiExplorerSettings(IgnoreApi = true)]        
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

        [ApiExplorerSettings(IgnoreApi = true)]
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

        [ApiExplorerSettings(IgnoreApi = true)]
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
        #endregion
    }

}
