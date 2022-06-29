using Ical.Net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Protocols.Homie.Devices.Properties;

namespace Toolsfactory.Home.Adapters.Garbage.Awido.Service
{
    public class HomieEnvironmentBuilder
    {
        private const string FormatPropertyKey = "nextformat";

        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly IReadOnlyDictionary<string, string> _categoriesMapping;
        private readonly HomieMqttServerConfiguration _homieHostConfig;
        private readonly CalendarLoaderOptions _calenderLoaderOptions;
        private readonly ILoggerFactory _loggerFactory;
        private readonly CalendarCollection _calendars = new();
        private CalendarLoader _calendarLoader;
        private CalendarParser _calendarParser;
        private StringProperty _lastUpdateProperty;
        private StringProperty _yearsLoadedProperty;
        private StringProperty _nextMuellAnyProperty;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public HomieDeviceHost DeviceHost { get; private set; }
        public Device RootDevice { get; private set; }
        public bool IsStarted { get { return (DeviceHost == null) ? false : DeviceHost.IsStarted; } }

        public HomieEnvironmentBuilder(string deviceId, string deviceName, IReadOnlyDictionary<string, string> categoriesMapping, HomieMqttServerConfiguration homieHostConfig, CalendarLoaderOptions calenderLoaderOptions, ILoggerFactory loggerFactory)
        {
            _deviceId = deviceId;
            _deviceName = deviceName;
            _categoriesMapping = categoriesMapping;
            _homieHostConfig = homieHostConfig;
            _calenderLoaderOptions = calenderLoaderOptions;
            _loggerFactory = loggerFactory;
            CreateDevice();
            CreateHost();
            CreateCalendarLoader();
            CreateCalendarParser();
        }

        public async Task ReloadCalendarsAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _calendarLoader.TryRefreshCalendarsAsync(DateTime.Now);
            }
            finally
            { 
                _semaphoreSlim.Release();
            }
        }

        public async Task StartAsync()
        {
            await DeviceHost.StartAsync();
            await ReloadCalendarsAsync();
            UpdateItems();
        }

        public async Task StopAsync()
        {
            await DeviceHost.StopAsync();
        }

        public void UpdateItems()
        {
            _semaphoreSlim.Wait();
            try
            {

                if (IsStarted)
                UpdateAll();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void CreateCalendarLoader()
        {
            _calendarLoader = new CalendarLoader(_calendars, _calenderLoaderOptions);
        }

        private void CreateCalendarParser()
        {
            _calendarParser = new CalendarParser(_calendars);
        }

        private void CreateDevice()
        {
            RootDevice = new Device(_deviceId, _deviceName);
            AddMuellNodesAndProperties();
            AddAnyNodeAndProperties();
            AddStatusNodeAndProperties();
            AddUpdateAllCommandNode();
        }

        private void AddMuellNodesAndProperties()
        {
            foreach (var item in _categoriesMapping)
            {
                var node = RootDevice
                    .AddNode(item.Key, "Abfuhrtermin", item.Value)
                        //                        .WithDateTimeProperty(RawPropertyKey, "Nächster Termin")
                        .WithStringProperty(FormatPropertyKey, "Nächster Termin (formatiert)");
            }
        }

        private void AddAnyNodeAndProperties()
        {
            _nextMuellAnyProperty = RootDevice.AddNode("any", "Abfuhrtermin", "Nächster Abfuhrtermin").AddStringProperty("any", "Nächster Termin (formatiert)");
        }

        private void AddUpdateAllCommandNode()
        {
            RootDevice
                .AddNode("update", "command", "Update anfordern")
                .AddBooleanProperty("all", "UpdateAll Command", "", true)
                    .PropertyCommandReceived += UpdateAllReceived;

        }


        private void AddStatusNodeAndProperties()
        {
            var node = RootDevice.AddNode("status", "status", "device status");
            _lastUpdateProperty = node.AddStringProperty("lastupdate", "last Update");
            _yearsLoadedProperty = node.AddStringProperty("yearsloaded", "all years available");
        }

        private void CreateHost()
        {
            DeviceHost = new HomieDeviceHost(RootDevice, _homieHostConfig, _loggerFactory.CreateLogger<HomieDeviceHost>());
        }

        private void UpdateAllReceived(DateTime timestamp, string newvalue)
        {
            UpdateAll();
        }

        private void UpdateAll()
        {
            foreach (var item in _categoriesMapping)
            {
                var value = _calendarParser.GetNextDateFor(item.Value);
                var rawvalue = (value.HasValue) ? value.Value.ToString("dd.MM.yy") : "09.09.99";
//                RootDevice.Nodes[item.Key].Properties[RawPropertyKey].RawValue = rawvalue;
                RootDevice.Nodes[item.Key].Properties[FormatPropertyKey].RawValue = _calendarParser.GetNextDateForFormatted(item.Value);
            }

            _lastUpdateProperty.Value = DateTime.Now.ToLongTimeString();
            _yearsLoadedProperty.Value = String.Join(',', _calendarLoader.LoadedYears);
            _nextMuellAnyProperty.Value = _calendarParser.GetNextDateForAnyFormatted(_categoriesMapping, DateTime.Now);
        }
    }
}
