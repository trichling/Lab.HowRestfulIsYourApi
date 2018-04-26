using System.Collections.Generic;
using dotnetCologne.RichardsonMaturityModel.Api.Models;

namespace dotnetCologne.RichardsonMaturityModel.Api.Repositories 
{

    public interface ITimesheetRepository
    {
        
        IEnumerable<Timesheet> GetAll();
        bool Exists(string name);
        Timesheet GetByName(string name);
        void Save(Timesheet timesheet);
        void Delete(string name);

    }

}