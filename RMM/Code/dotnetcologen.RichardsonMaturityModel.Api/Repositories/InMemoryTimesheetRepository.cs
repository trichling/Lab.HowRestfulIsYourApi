using System;
using System.Collections.Generic;
using System.Linq;
using dotnetCologne.RichardsonMaturityModel.Api.Models;

namespace dotnetCologne.RichardsonMaturityModel.Api.Repositories
{

    public class InMemoryTimesheetRepository : ITimesheetRepository
    {

        private Dictionary<string, Timesheet> timesheets;

        public InMemoryTimesheetRepository()
        {
            timesheets = new Dictionary<string, Timesheet>() {
                { "Test", new Timesheet("Test", new List<TimeBooking>() {
                    new TimeBooking(DateTime.Today, DateTime.Now.AddHours(-2), TimeSpan.FromMinutes(15), DateTime.Now.AddHours(2)),
                    new TimeBooking(DateTime.Today.AddDays(-1), DateTime.Now.AddHours(-1), TimeSpan.FromMinutes(10), DateTime.Now.AddHours(2)),
                    new TimeBooking(DateTime.Today.AddDays(-2), DateTime.Now.AddHours(-2), TimeSpan.FromMinutes(20), DateTime.Now.AddHours(3))
                }) }
            };
        }

        public void Delete(string name)
        {
            timesheets.Remove(name);
        }

        public bool Exists(string name)
        {
            return timesheets.ContainsKey(name);
        }

        public IEnumerable<Timesheet> GetAll()
        {
            return timesheets.Values.ToList();
        }

        public Timesheet GetByName(string name)
        {
            return timesheets[name];
        }

        public void Save(Timesheet timesheet)
        {
            if (!Exists(timesheet.Name))
                timesheets.Add(timesheet.Name, timesheet);

            timesheets[timesheet.Name] = timesheet;
        }
    }

}