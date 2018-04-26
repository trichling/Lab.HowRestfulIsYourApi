using System;

namespace dotnetCologne.RichardsonMaturityModel.Api.Models 
{

    public class Timesheet
    {

        public Timesheet(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; set; }
    }

}