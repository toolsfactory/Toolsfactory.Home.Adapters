namespace Toolsfactory.Home.Adapters.Heating.Wolf.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        static object[] TestData_SetDatapointValueReq = new object[]
        {
            // input, mainservice, subservice, StartDP, NumDP
            new object[] { "0F80", "9.001", "38.4"},
            new object[] { "1726", "9.001", "73.2"},
            new object[] { "00C8", "9.002", "2"},
            new object[] { "87CE", "9.002", "-0.5"},
            new object[] { "6DC5", "9.006", "120995.84"},
            new object[] { "01", "1.001", "true"},
            new object[] { "00", "1.001", "false"},
            new object[] { "01", "1.003", "true"},
            new object[] { "00", "1.003", "false"},
            new object[] { "01", "1.009", "true"},
            new object[] { "00", "1.009", "false"},

            new object[] { "00008000", "13.010", "32768"},
            new object[] { "4500", "9.024", "3276.8"},
            new object[] { "00", "1.009", "false"},
            new object[] { "00", "1.009", "false"},
        };

        [TestCaseSource(nameof(TestData_SetDatapointValueReq))]
        [Test]
        public void Test1(string input, string dptid, string expected)
        {
            var data = input.RemoveAll(' ').ToByteArray();
            var ok = DPT2HomieDataConverter.TryTranslateDptToHomie(data, dptid, out var value, out var homietype);

            Assert.IsTrue(ok);
            Assert.AreEqual(expected, value);
        }
    }
}