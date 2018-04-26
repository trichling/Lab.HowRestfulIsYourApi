using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Models 
{

    public class RenameTimesheetReqeust
    {

        public string TimesheetToRename { get; set; }
        public string NewName { get; set; }

    }

}