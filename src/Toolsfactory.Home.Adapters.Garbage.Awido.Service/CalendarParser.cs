using Ical.Net;
using Ical.Net.CalendarComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolsfactory.Home.Adapters.Garbage.Awido.Service
{
    public class CalendarParser
    {
        private CalendarCollection _calendarList;

        public CalendarParser(CalendarCollection calendarList)
        {
            _calendarList = calendarList;
        }

        public DateTime? GetNextDateFor(string category, DateTime start)
        {
            var from = (start.Hour > 12) ? start.AddDays(1) : start;
            var next = _calendarList.GetOccurrences(from, from.AddDays(190))
                .Where(x => (x.Source is CalendarEvent) && ((CalendarEvent)x.Source).Summary.ToLower().StartsWith(category.ToLower()))
                .OrderBy(x => x.Source.Start)
                .FirstOrDefault();
            return next?.Source.Start.Date;
        }

        public string GetNextDateForFormatted(string category, DateTime start)
        {
            var nextdate = GetNextDateFor(category, start);
            return FormatDate(start, nextdate);
        }


        public DateTime? GetNextDateFor(string category)
        {
            return GetNextDateFor(category, DateTime.Now);
        }
        public string GetNextDateForFormatted(string category)
        {
            return GetNextDateForFormatted(category, DateTime.Now);
        }

        public string GetNextDateForAnyFormatted(IReadOnlyDictionary<string, string> categoriesMapping, DateTime start)
        {
            Nullable<DateTime> next = DateTime.MaxValue;
            foreach (var cat in categoriesMapping)
            {
                var current = GetNextDateFor(cat.Value, start);
                if (current.HasValue && current.Value < next)
                    next = current;
            }
            return FormatDate(start, next);
        }

        private static string FormatDate(DateTime start, DateTime? nextdate)
        {
            if (!nextdate.HasValue)
                return "Fehler";
            if (nextdate.Value.Date == start.Date)
                return "Heute";
            else if (nextdate.Value.Date == start.AddDays(1).Date)
                return "Morgen";
            else if (nextdate.Value <= start.AddDays(6))
                return nextdate!.Value.ToString("dddd");
            else
                return nextdate.Value.Date.ToString("dd.MM.yy");
        }
    }
}
