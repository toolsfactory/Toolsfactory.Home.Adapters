﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Toolsfactory.Home.Adapters.Gasprices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;
using Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.DTO;
using System.Linq;
using Tiveria.Common;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig
{

    public class TankerkoenigManager : IGasStations
    {
        #region private fields
        private readonly IDictionary<string, GasStationDetails> _items = new Dictionary<string, GasStationDetails>();
        private readonly TankerkoenigManagerOptions _options;
        private readonly HttpClient _client;
        private readonly ILogger<TankerkoenigManager> _logger;
        #endregion

        #region public properties
        public GasStationDetails this[string id] => _items.ContainsKey(id) ? _items[id] : null;

        public int Count => _items.Count;

        public Guid ApiKey { get; }
        #endregion

        #region Constructors & Destructors
        public TankerkoenigManager(IOptions<TankerkoenigManagerOptions> options, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            Ensure.That(httpClientFactory).IsNotNull();
            Ensure.That(loggerFactory).IsNotNull();
            Ensure.That(options.Value).IsNotNull();
            Ensure.That(options.Value.ApiKey).IsNotEmpty();

            _options = options!.Value;
            _client = httpClientFactory.CreateClient("tanker");
            _logger = loggerFactory.CreateLogger<TankerkoenigManager>();

            ApiKey = options.Value.ApiKey;
        }
        #endregion

        #region Enumerators
        public IEnumerator<GasStationDetails> GetEnumerator()
        {
            return _items.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }
        #endregion

        #region Public Implementations
        public async Task<bool> InitializeAsync(double latitude, double longitude, double maxDistanceKM = 5, GasType gasTypes = GasType.All)
        {
            ValidateLngAndLat(longitude, latitude);
            var requestUrl = GenerateRequestUrl();

            var response = await _client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var body = await response.Content.ReadAsStringAsync();

            if (!CheckRequestStatus(body))
            { return false; }

            return ExtractAndConvertStationsData(body);

            string TranslateGasType(GasType gt)
                => gt switch
                {
                    GasType.Diesel => "diesel",
                    GasType.E5 => "e5",
                    GasType.E10 => "e10",
                    _ => "all"
                };

            string GenerateRequestUrl()
            {
                var c = CultureInfo.InvariantCulture;
                var latInv = latitude.ToString(c);
                var lngInv = longitude.ToString(c);
                var distInv = maxDistanceKM.ToString(c);

                var requestUrl = $"{_options.BaseUrl}/json/list.php?lat={latInv}&lng={lngInv}&sort=dist&rad={distInv}&type={TranslateGasType(gasTypes)}&apikey={ApiKey}";
                return requestUrl;
            }

            bool ExtractAndConvertStationsData(string body)
            {
                try
                {
                    var data = JsonSerializer.Deserialize<StationListDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull});
                    _items.Clear();
                    foreach (var item in data.Stations)
                    {
                        _items.Add(item.Id, item.MapToBusiness());
                    }
                    return true;
                }
                catch
                { return false; }
            }
        }

        public async Task<bool> InitializeAsync(string[] ids)
        {
            bool cleared = false;
            string body = "";
            foreach (var id in ids)
            {
                var requestUrl = GenerateRequestUrl(id);

                var response = await _client.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                body = await response.Content.ReadAsStringAsync();

                if (!CheckRequestStatus(body))
                { continue; }

                if (!cleared)
                {
                    _items.Clear();
                    cleared = true;
                }
                ExtractAndConvertStationDetails();
            }
            return true;

            string GenerateRequestUrl(string id)
            {
                var requestUrl = $"{_options.BaseUrl}/json/detail.php?id={id}&apikey={ApiKey}";
                return requestUrl;
            }

            bool ExtractAndConvertStationDetails()
            {
                try
                {
                    var data = JsonSerializer.Deserialize<StationDetailsDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                    _items.Add(data.Station.Id, data.Station.MapToBusiness());
                    return true;
                }
                catch
                { return false; }
            }
        }

        public async Task<bool> UpdatePricesAsync()
        {
            if (_items.Count == 0)
                return false;

            var requestUrl = GenerateRequestUrl();

            var response = await _client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var body = await response.Content.ReadAsStringAsync();

            if (!CheckRequestStatus(body))
            { return false; }

            ExtractAndConvertPriceUpdates();

            return true;

            string GenerateRequestUrl()
            {
                var ids = String.Join(',', _items.Keys.ToList());
                var requestUrl = $"{_options.BaseUrl}/json/prices.php?ids={ids}&apikey={ApiKey}";
                return requestUrl;
            }

            bool ExtractAndConvertPriceUpdates()
            {
                try
                {
                    var data = JsonSerializer.Deserialize<PriceListDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                    foreach (var item in data.Prices)
                    {
                        if (_items.ContainsKey(item.Key))
                        {
                            _items[item.Key].Gasprices = item.Value.MapToBusiness();
                        }
                    }
                    return true;
                }
                catch
                { return false; }
            }
        }
        #endregion

        #region Private Helpers
        private bool CheckRequestStatus(string body)
        {
            try
            {
                return JsonSerializer.Deserialize<RequestStatusDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }).Ok;
            }
            catch
            {
                throw new JsonException();
            }
        }

        private void ValidateLngAndLat(double longitude, double latitude)
        {
            if (Math.Abs(latitude) > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be in range of -90 to 90");
            if (Math.Abs(longitude) > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be in range of -180 to 180");

        }
        #endregion
    }
}
