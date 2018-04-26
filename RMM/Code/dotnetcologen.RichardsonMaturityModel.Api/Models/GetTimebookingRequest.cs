using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Models 
{

    public class GetTimebookingRequest
    {

        public string Timesheet { get; set; }
        public DateTime Date { get; set; }

    }

}