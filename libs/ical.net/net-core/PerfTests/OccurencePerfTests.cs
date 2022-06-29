﻿using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace PerfTests
{
    public class OccurencePerfTests
    {
        [Benchmark]
        public void MultipleEventsWithUntilOccurrencesSearchingByWholeCalendar()
        {
            var calendar = GetFourCalendarEventsWithUntilRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1);
            var occurences = calendar.GetOccurrences(searchStart, searchEnd);
        }

        [Benchmark]
        public void MultipleEventsWithUntilOccurrences()
        {
            var calendar = GetFourCalendarEventsWithUntilRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1);
            var eventOccurrences = calendar.Events
                .SelectMany(e => e.GetOccurrences(searchStart, searchEnd))
                .ToList();
        }

        [Benchmark]
        public void MultipleEventsWithUntilOccurrencesEventsAsParallel()
        {
            var calendar = GetFourCalendarEventsWithUntilRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1).AddDays(10);
            var eventOccurrences = calendar.Events
                .AsParallel()
                .SelectMany(e => e.GetOccurrences(searchStart, searchEnd))
                .ToList();
        }

        private Calendar GetFourCalendarEventsWithUntilRule()
        {
            const string tzid = "America/New_York";
            const int limit = 4;

            var startTime = DateTime.Now.AddDays(-1);
            var interval = TimeSpan.FromDays(1);

            var events = Enumerable
                .Range(0, limit)
                .Select(n =>
                {
                    var rrule = new RecurrencePattern(FrequencyType.Daily, 1)
                    {
                        Until = startTime.AddDays(10),
                    };

                    var e = new CalendarEvent
                    {
                        Start = new CalDateTime(startTime.AddMinutes(5), tzid),
                        End = new CalDateTime(startTime.AddMinutes(10), tzid),
                        RecurrenceRules = new List<RecurrencePattern> { rrule },
                    };
                    startTime += interval;
                    return e;
                });

            var c = new Calendar();
            c.Events.AddRange(events);
            return c;
        }

        [Benchmark]
        public void MultipleEventsWithCountOccurrencesSearchingByWholeCalendar()
        {
            var calendar = GetFourCalendarEventsWithCountRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1);
            var occurences = calendar.GetOccurrences(searchStart, searchEnd);
        }

        [Benchmark]
        public void MultipleEventsWithCountOccurrences()
        {
            var calendar = GetFourCalendarEventsWithCountRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1);
            var eventOccurrences = calendar.Events
                .SelectMany(e => e.GetOccurrences(searchStart, searchEnd))
                .ToList();
        }

        [Benchmark]
        public void MultipleEventsWithCountOccurrencesEventsAsParallel()
        {
            var calendar = GetFourCalendarEventsWithCountRule();
            var searchStart = calendar.Events.First().DtStart.AddYears(-1);
            var searchEnd = calendar.Events.Last().DtStart.AddYears(1).AddDays(10);
            var eventOccurrences = calendar.Events
                .AsParallel()
                .SelectMany(e => e.GetOccurrences(searchStart, searchEnd))
                .ToList();
        }

        private Calendar GetFourCalendarEventsWithCountRule()
        {
            const string tzid = "America/New_York";
            const int limit = 4;

            var startTime = DateTime.Now.AddDays(-1);
            var interval = TimeSpan.FromDays(1);

            var events = Enumerable
                .Range(0, limit)
                .Select(n =>
                {
                    var rrule = new RecurrencePattern(FrequencyType.Daily, 1)
                    {
                        Count = 100,
                    };

                    var e = new CalendarEvent
                    {
                        Start = new CalDateTime(startTime.AddMinutes(5), tzid),
                        End = new CalDateTime(startTime.AddMinutes(10), tzid),
                        RecurrenceRules = new List<RecurrencePattern> { rrule },
                    };
                    startTime += interval;
                    return e;
                });

            var c = new Calendar();
            c.Events.AddRange(events);
            return c;
        }
    }
}
