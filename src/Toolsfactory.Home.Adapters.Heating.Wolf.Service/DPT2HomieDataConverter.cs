using System.Globalization;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public static class DPT2HomieDataConverter
    {
        private static Dictionary<string, string> _mappings = new Dictionary<string, string>
            () { { "", "" },
            {"1.001", "boolean" }, // (on/off)
            {"1.002", "boolean" }, // (true/false)
            {"1.003", "boolean" }, // (enable/disable)
            {"1.009", "boolean" }, // (open/close)
            {"5.001", "integer" },
            {"9.001", "float" },
            {"9.002", "float" },
            {"9.006", "float" },
            {"9.024", "float" },
            {"9.025", "float" },
            {"11.001", "DateTime" },
            {"13.002", "integer" },
            {"13.010", "integer" },
            {"13.013", "integer" },
            {"20.102", "integer" },
            {"20.103", "integer" },
            {"20.105", "integer" },
            //{"10.001", "TimeOfDay" }
        };

        public static bool TryTranslateDptToHomie(byte[] data, string dptid, out string value, out string homietype)
        {
            value = "";
            if (!_mappings.TryGetValue(dptid, out homietype))
                    return false;

            var decoder =  Tiveria.Home.Knx.Datapoint.DatapointTypesList.GetTypeById(dptid);
            if (decoder == null)
                return false; // "ERROR FINDING KNX DATAPOINT DECODER";

            switch (decoder.MainCategory)
            {
                case 1:  value = (decoder as Tiveria.Home.Knx.Datapoint.DPType1)?.Decode(data).ToString().ToLowerInvariant() ?? "ERROR - cannot decode DPT1"; break; // value = obj is bool && (bool) obj ? "true" : "false"; break;
                case 5:
                case 13:
                case 20: value = decoder.DecodeString(data, 0, false, true); break;
                case 9:  value = decoder.DecodeString(data, 0, false, true); break;
                case 11: value = decoder is Tiveria.Home.Knx.Datapoint.DPType11 ? ((DateTime) decoder.DecodeObject(data)).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture) : "ERROR - cannot decode DPT11"; break;
                default: value = "ERROR - unknown"; break;
            }
            return true;
        }
    }
}
