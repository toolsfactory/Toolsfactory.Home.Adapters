using System.Globalization;

namespace Toolsfactory.Home.Adapters.UnitTests
{
    public class Tests
    {

        string TranslateValue(string item, string data)
        {
            var parsed = double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out var pdata);
            if (!parsed)
                return data;

            double value = pdata;
            if (item == "windchillf" || item == "dewptf" || item == "tempf" || item == "indoortempf")
            {
                value = pdata.Fahrenheit2Celsius();
            }
            else if (item == "baromin")
            {
                value = pdata.InHg2mBar();
            }
            else if (item.IndexOf("rainin") > -1)
            {
                value = pdata.Inch2MM();
            }
            else if (item.IndexOf("mph") > -1)
            {
                value = pdata.Mile2KM();
            }
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var data = TranslateValue("humidity", "99");
            Assert.AreEqual("99", data);
            Assert.Pass();
        }
    }
}