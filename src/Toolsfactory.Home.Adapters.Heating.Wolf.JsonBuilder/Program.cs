namespace Toolsfactory.Home.Adapters.Heating.Wolf.JsonBuilder
{
    using System.Text.Json;
    internal class Program
    {
        public static List<Node> Nodes = new List<Node>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Initialize();
            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Nodes, options);

            Console.WriteLine(jsonString);

            string path = @"data.json";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(jsonString);
                }
            }
        }

        private static void Initialize()
        {
            for (ushort i = 0; i < 4; i++)
                Nodes.Add(new Node($"heating{i + 1}", new List<HeatingDataPoint>() {
                    new HeatingDataPoint((ushort)((i*13) + 1), "Störung", "error", "1.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 2), "Betriebsart", "mode", "20.105", false),
                    new HeatingDataPoint((ushort)((i*13) + 3), "Modulationsgrad  Brennerleistung", "modburnercap", "5.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 4), "Kesseltemperatur", "boilertemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 5), "Sammlertemperatur", "collectortemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 6), "Rücklauftemperatur", "refluxtemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 7), "Warmwassertemperatur", "Wwatertemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 8), "Außentemperatur", "outsidetemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 9), "Status Brenner / Flamme", "burnerstate", "1.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 10), "Status Heizkreispumpe", "heatingcircutpumpstate", "1.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 11), "Status Speicherladepumpe", "tankcharchingpumpstate", "1.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 12), "Status 3-Wege-Umschaltventil", "valvestate", "1.009", false),
                    new HeatingDataPoint((ushort)((i*13) + 13), "Anlagendruck", "pressure", "9.006", false),

                    new HeatingDataPoint((ushort)((i*3) + 197), "Abgastemperatur", "exhaustgastemperature", "9.001", false),
                    new HeatingDataPoint((ushort)((i*3) + 198), "Leistungsvorgabe", "performancetarget", "5.001", true),
                    new HeatingDataPoint((ushort)((i*3) + 199), "Kesselsolltemperaturvorgabe", "boilertemperaturetarget", "9.001", true),
                }));

            Nodes.Add(new Node($"system", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(53, "Störung", "error", "1.001", false),
                new HeatingDataPoint(54, "Außentemperatur", "outsidetemp", "9.001", false),
                new HeatingDataPoint(194, "1xWarmwasserladung", "warmwateronce", "1.001", true),
            }));

            Nodes.Add(new Node($"direct", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(56, "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true),
                new HeatingDataPoint(57, "Programmwahl Heizkreis", "heatcircutprogram", "20.102", true),
                new HeatingDataPoint(58, "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true),
                new HeatingDataPoint(59, "Heizkreis Zeitprogramm 1", "heatingcircutprogram1", "1.001", true),
                new HeatingDataPoint(60, "Heizkreis Zeitprogramm 2", "heatingcircutprogram2", "1.001", true),
                new HeatingDataPoint(61, "Heizkreis Zeitprogramm 3", "heatingcircutprogram3", "1.001", true),
                new HeatingDataPoint(62, "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true),
                new HeatingDataPoint(63, "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true),
                new HeatingDataPoint(64, "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true),
                new HeatingDataPoint(65, "Sollwertkorrektur", "targetvaluecorrection", "9.002", true),
                new HeatingDataPoint(66, "Sparfaktor", "savingsfactor", "9.002", true)
            }));

            for (ushort i = 0; i < 3; i++)
                Nodes.Add(new Node($"mixercircuit{i + 1}", new List<HeatingDataPoint>() {
                    new HeatingDataPoint((ushort)((i*13) + 67), "Störung", "error", "1.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 68), "Raumtemperatur", "roomtemp", "9.001", false),
                    new HeatingDataPoint((ushort)((i*13) + 69), "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 70), "Programmwahl Heizkreis", "heatcircutprogram", "20.102", true),
                    new HeatingDataPoint((ushort)((i*13) + 71), "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true),
                    new HeatingDataPoint((ushort)((i*13) + 72), "Heizkreis Zeitprogramm 1", "heatingcircutprogram1", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 73), "Heizkreis Zeitprogramm 2", "heatingcircutprogram2", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 74), "Heizkreis Zeitprogramm 3", "heatingcircutprogram3", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 75), "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 76), "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 77), "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true),
                    new HeatingDataPoint((ushort)((i*13) + 78), "Sollwertkorrektur", "targetvaluecorrection", "9.002", true),
                    new HeatingDataPoint((ushort)((i*13) + 79), "Sparfaktor", "savingsfactor", "9.002", true)
                }));

            Nodes.Add(new Node($"cascade", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(106, "Störung", "error", "1.001", false),
                new HeatingDataPoint(107, "Sammlertemperatur", "collectortemperature", "9.001", false),
                new HeatingDataPoint(108, "Gesamtmodulationsgrad", "totalmodulation", "5.001", false),
                new HeatingDataPoint(109, "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false),
                new HeatingDataPoint(110, "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false),
                new HeatingDataPoint(111, "Status Ausgang A1", "outputA1state", "1.003", false),
                new HeatingDataPoint(112, "Eingang E1", "inpute1", "9.001", false),
                new HeatingDataPoint(113, "Eingang E2", "inpute2", "9.001", false),
                new HeatingDataPoint(209, "Gesamtmodulationsgradvorgabe", "totalmodulationtarget", "5.001", true),
                new HeatingDataPoint(210, "Sammlersolltemeraturvorgabe", "collectortemperaturetarget", "9.001", true),
            }));


            for (ushort i = 0; i < 3; i++)
                Nodes.Add(new Node($"mixermodule{i + 1}", new List<HeatingDataPoint>() {
                    new HeatingDataPoint((ushort)((i*7) + 114), "Störung", "error", "1.001", false),
                    new HeatingDataPoint((ushort)((i*7) + 115), "Warmwassertemperatur", "warmwatertemperature", "9.001", false),
                    new HeatingDataPoint((ushort)((i*7) + 116), "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false),
                    new HeatingDataPoint((ushort)((i*7) + 117), "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false),
                    new HeatingDataPoint((ushort)((i*7) + 118), "Status Ausgang A1", "outputa1state", "1.003", false),
                    new HeatingDataPoint((ushort)((i*7) + 119), "Eingang E1", "inpute1", "9.001", false),
                    new HeatingDataPoint((ushort)((i*7) + 120), "Eingang E2", "inpute2", "9.001", false)
                }));


            Nodes.Add(new Node($"solar", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(135, "Störung", "error", "1.001", false),
                new HeatingDataPoint(136, "Warmwassertemperatur Solar 1", "warmwatertemperaturesolar", "9.001", false),
                new HeatingDataPoint(137, "Temperatur Kollektor 1", "collector1temperature", "9.001", false),
                new HeatingDataPoint(138, "Eingang E1", "inpute1", "9.001", false),
                new HeatingDataPoint(139, "Eingang E2 (Durchfluss)", "inpute2", "9.025", false),
                new HeatingDataPoint(140, "Eingang E3", "inpute3", "9.001", false),
                new HeatingDataPoint(141, "Status Solarkreispumpe SKP1", "solarcircuitpumpstate", "1.001", false),
                new HeatingDataPoint(142, "Status Ausgang A1", "outputa1state", "1.003", false),
                new HeatingDataPoint(143, "Status Ausgang A2", "outputa2state", "1.003", false),
                new HeatingDataPoint(144, "Status Ausgang A3", "outputa3state", "1.003", false),
                new HeatingDataPoint(145, "Status Ausgang A4", "outputa4state", "1.003", false),
                new HeatingDataPoint(146, "Durchfluss", "flow", "9.025", false),
                new HeatingDataPoint(147, "aktuelle Leistung", "currentpower", "9.024", false)
            }));


            Nodes.Add(new Node($"solar", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(148, "Störung", "error", "1.001", false),
                new HeatingDataPoint(149, "Programm CWL", "program", "20.103", true),
                new HeatingDataPoint(150, "Zeitprogramm 1", "timeprogram1", "1.001", true),
                new HeatingDataPoint(151, "Zeitprogramm 2", "timeprogrm1", "1.001", true),
                new HeatingDataPoint(152, "Zeitprogramm 3", "timeprogram3", "1.001", true),
                new HeatingDataPoint(153, "Zeitweise Intensivlüftung AN/AUS", "intenseairing", "1.001", true),
                new HeatingDataPoint(154, "Zeitweise Intensivlüftung Startdatum", "intensairingstartdate", "11.001", true),
                new HeatingDataPoint(155, "Zeitweise Intensivlüftung Enddatum", "intensairingenddate", "11.001", true),
                new HeatingDataPoint(156, "Zeitweise Intensivlüftung Startzeit", "intensairingstarttime", "10.001", true),
                new HeatingDataPoint(157, "Zeitweise Intensivlüftung Endzeit", "intensairingendtime", "10.001", true),
                new HeatingDataPoint(158, "Zeitweiser Feuchteschutz AN/AUS", "moistureprotection", "1.001", true),
                new HeatingDataPoint(159, "Zeitweiser Feuchteschutz Startdatum", "moistureprotectionstartdate", "11.001", true),
                new HeatingDataPoint(160, "Zeitweiser Feuchteschutz Enddatum", "moistureprotectionenddate", "11.001", true),
                new HeatingDataPoint(161, "Zeitweiser Feuchteschutz Startzeit", "moistureprotectionstarttime", "10.001", true),
                new HeatingDataPoint(162, "Zeitweiser Feuchteschutz Endzeit", "moistureprotectionendtime", "10.001", true),
                new HeatingDataPoint(163, "Lüftungsstufe", "airinglevel", "5.001", false),
                new HeatingDataPoint(164, "Ablufttemperatur", "exhaustedairtemperature", "9.001", false),
                new HeatingDataPoint(165, "Frischlufttemperatur", "freshairtemperature", "9.001", false),
                new HeatingDataPoint(166, "Luftdurchsatz Zuluft", "throughputfreshair", "13.002", false),
                new HeatingDataPoint(167, "Luftdurchsatz Abluft", "throughtputexhaustedair", "13.002", false),
                new HeatingDataPoint(168, "Bypass Initialisierung", "bypassinit", "1.002", false),
                new HeatingDataPoint(169, "Bypass öffnet/offen", "bypassopen", "1.002", false),
                new HeatingDataPoint(170, "Bypass schließt/geschlossen", "bypassclosed", "1.002", false),
                new HeatingDataPoint(171, "Bypass Fehler", "bypasserror", "1.002", false),
                new HeatingDataPoint(172, "Frost Status: Initialisierung/Warte", "froststateinit", "1.002", false),
                new HeatingDataPoint(173, "Frost Status: Kein Frost", "froststatenone", "1.002", false),
                new HeatingDataPoint(174, "Frost Status: Vorwärmer", "froststatepreheater", "1.002", false),
                new HeatingDataPoint(175, "Frost Status: Fehler/Unausgeglichen", "froststateerror", "1.002", false),
                new HeatingDataPoint(195, "Tagesertrag", "dayyield", "13.010", false),
                new HeatingDataPoint(196, "Gesamtertrag", "totalyield", "13.013", false),

            }));

            Nodes.Add(new Node($"bwl1s", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(176, "Störung", "error", "1.001", false),
                new HeatingDataPoint(177, "Betriebsart", "operatingmode", "20.105", false),
                new HeatingDataPoint(178, "Heizleistung", "heatingcapacity", "9.024", false),
                new HeatingDataPoint(179, "Kühlleistung", "coolingcapacity", "9.024", false),
                new HeatingDataPoint(180, "Kesseltemperatur", "boilertemperature", "9.001", false),
                new HeatingDataPoint(181, "Sammlertemperatur", "collectortemeperature", "9.001", false),
                new HeatingDataPoint(182, "Rücklauftemperatur", "returntemperature", "9.001", false),
                new HeatingDataPoint(183, "Warmwassertemperatur", "warmwatertemperature", "9.001", false),
                new HeatingDataPoint(184, "Außentemperatur", "outsidetemperature", "9.001", false),
                new HeatingDataPoint(185, "Status Heizkreispumpe", "heatingcircuitpumpstate", "1.001", false),
                new HeatingDataPoint(186, "Status Zubringer-/Heizkreispumpe", "feederpumpstate", "1.001", false),
                new HeatingDataPoint(187, "Status 3-Wege-Umschaltventil HZ/WW", "wwswitchvalvestate", "1.009", false),
                new HeatingDataPoint(188, "Status 3-Wege-Umschaltventil HZ/K", "cwswitchvalvestate", "1.009", false),
                new HeatingDataPoint(189, "Status E-Heizung", "eheatingstate", "1.001", false),
                new HeatingDataPoint(190, "Anlagendruck", "pressure", "9.006", false),
                new HeatingDataPoint(191, "Leistungsaufnahme", "powerconsumption", "9.024", false),
            }));

            Nodes.Add(new Node($"cwlexcellent", new List<HeatingDataPoint>()
            {
                new HeatingDataPoint(192, "Filterwarnung aktiv", "filterwarning", "1.001", false),
                new HeatingDataPoint(193, "Filterwarnung zurücksetzen", "filterwarningreset", "1.001", true),
            }));
        }

        public record HeatingDataPoint(ushort id, string name, string dptname, string dptid, bool writeable);
        public record Node(string Name, List<HeatingDataPoint> Properties);

    }

}