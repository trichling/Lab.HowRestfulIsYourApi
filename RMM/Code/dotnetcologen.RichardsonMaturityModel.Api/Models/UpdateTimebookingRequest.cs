using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Models 
{

    public class UpdateimebookingRequest
    {

        public string Timesheet { get; set; }
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Pause { get; set; }
        public DateTime End { get; set; }

    }

}