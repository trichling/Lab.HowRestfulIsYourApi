using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace dotnetCologne.RichardsonMaturityModel.Api.Models 
{

    public class Timesheet
    {
        private readonly List<TimeBooking> _bookings;

        public Timesheet(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            _bookings = new List<TimeBooking>();
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public ReadOnlyCollection<TimeBooking> Bookings => new ReadOnlyCollection<TimeBooking>(_bookings);

        public TimeBooking GetBookingByDate(DateTime date) 
        {
            return Bookings.SingleOrDefault(b => b.Date.Date == date.Date);
        }

        public TimeBooking Update(DateTime bookingDate, TimeBooking updatedBooking)
        {
            var currentBooking = GetBookingByDate(bookingDate);

            currentBooking.Start = updatedBooking.Start;
            currentBooking.Pause = updatedBooking.Pause;
            currentBooking.End = updatedBooking.End;

            return currentBooking;
        }

        public void BookTime(DateTime date, DateTime start, TimeSpan pause, DateTime end)
        {
            var booking = new TimeBooking(date, start, pause, end);
            _bookings.Add(booking);
        }
    }

    public class TimeBooking
    {
        
        public TimeBooking(DateTime date, DateTime start, TimeSpan pause, DateTime end)
        {
            Date = date;
            Start = start;
            Pause = pause;
            End = end;
        }

        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Pause { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration => End - Start - Pause;
    }
}