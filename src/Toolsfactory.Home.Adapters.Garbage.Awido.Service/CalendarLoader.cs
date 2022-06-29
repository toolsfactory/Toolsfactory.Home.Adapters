using Ical.Net;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Toolsfactory.Home.Adapters.Garbage.Awido.Service
{
    public class CalendarLoader
    {
        private readonly CalendarCollection _calendarList;
        private readonly CalendarLoaderOptions _options;
        private readonly List<int> _loadedYears = new List<int>();
        private readonly List<string> _loadedCalendarFiles = new List<string>();

        public IReadOnlyList<int> LoadedYears => _loadedYears; 
        public IReadOnlyList<string> LoadedCalendarFiles => _loadedCalendarFiles;

        public CalendarLoader(CalendarCollection calendarList, CalendarLoaderOptions options)
        {
            _calendarList = calendarList;
            _options = VerifyOptions(options);
        }

        private CalendarLoaderOptions VerifyOptions(CalendarLoaderOptions options)
        {
            if (options.YearDigits != 2 && options.YearDigits != 4)
                options = options with { YearDigits = 4 };
            if (options.LoadNextYearErliestMonth < 10 && options.LoadNextYearErliestMonth > 12)
                options = options with { LoadNextYearErliestMonth = 12 };
            return options;
        }

        public async Task<bool> TryRefreshCalendarsAsync(DateTime date)
        {
            return (await RefreshCalendarAsync(date, true) && await RefreshCalendarAsync(date.AddYears(1), false));
        }

        private async Task<bool> RefreshCalendarAsync(DateTime date, bool clear)
        {
            if (date.Year < 2020 || date.Year > 2080)
                return false;
            var yearstr = (_options.YearDigits == 2) ? date.ToString("yy") : date.ToString("yyyy");

            var result = await TryLoadCalendarTextForYearAsync(yearstr);
            if (result.Success)
            {
                if (clear)
                {
                    _calendarList.Clear();
                    _loadedCalendarFiles.Clear();
                    _loadedYears.Clear();
                }
                _calendarList.Add(result.Calendar);
                _loadedCalendarFiles.Add(result.Url);
                _loadedYears.Add(date.Year);
                return true;
            }
            else
                return false;
        }

        private async Task<(bool Success, Calendar Calendar, string Url)> TryLoadCalendarTextForYearAsync(string year)
        {
            try
            {
                var url = _options.DownloadUrlTemplate.Replace("{year}", year);
                var text = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                var calendar = Calendar.Load(text);
                return (true, calendar, url);
            }
            catch
            {
                return (false, null, null);
            }
        }
    }
}
