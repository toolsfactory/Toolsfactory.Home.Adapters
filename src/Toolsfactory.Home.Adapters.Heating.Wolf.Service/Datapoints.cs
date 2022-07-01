﻿namespace Toolsfactory.Home.Adapters.Heating.Wolf
{
    public static class Datapoints
    {
        private static Dictionary<ushort, HeatingDataPoint> DatapointsMap = new Dictionary<ushort, HeatingDataPoint>();


        static Datapoints()
        {
            DatapointsMap.Add(1, new HeatingDataPoint(1, "hg1", "Störung", "error", "1.001", false));
            DatapointsMap.Add(2, new HeatingDataPoint(2, "hg1", "Betriebsart", "mode", "20.105", false));
            DatapointsMap.Add(3, new HeatingDataPoint(3, "hg1", "Modulationsgrad  Brennerleistung", "modburnercap", "5.001", false));
            DatapointsMap.Add(4, new HeatingDataPoint(4, "hg1", "Kesseltemperatur", "boilertemp", "9.001", false));
            DatapointsMap.Add(5, new HeatingDataPoint(5, "hg1", "Sammlertemperatur", "collectortemp", "9.001", false));
            DatapointsMap.Add(6, new HeatingDataPoint(6, "hg1", "Rücklauftemperatur", "refluxtemp", "9.001", false));
            DatapointsMap.Add(7, new HeatingDataPoint(7, "hg1", "Warmwassertemperatur", "Wwatertemp", "9.001", false));
            DatapointsMap.Add(8, new HeatingDataPoint(8, "hg1", "Außentemperatur", "outsidetemp", "9.001", false));
            DatapointsMap.Add(9, new HeatingDataPoint(9, "hg1", "Status Brenner / Flamme", "burnerstate", "1.001", false));
            DatapointsMap.Add(10, new HeatingDataPoint(10, "hg1", "Status Heizkreispumpe", "heatingcircutpumpstate", "1.001", false));
            DatapointsMap.Add(11, new HeatingDataPoint(11, "hg1", "Status Speicherladepumpe", "tankcharchingpumpstate", "1.001", false));
            DatapointsMap.Add(12, new HeatingDataPoint(12, "hg1", "Status 3-Wege-Umschaltventil", "valvestate", "1.009", false));
            DatapointsMap.Add(13, new HeatingDataPoint(13, "hg1", "Anlagendruck", "pressure", "9.006", false));
            DatapointsMap.Add(14, new HeatingDataPoint(14, "hg2", "Störung", "error", "1.001", false));
            DatapointsMap.Add(15, new HeatingDataPoint(15, "hg2", "Betriebsart", "mode", "20.105", false));
            DatapointsMap.Add(16, new HeatingDataPoint(16, "hg2", "Modulationsgrad / Brennerleistung", "modburnercap", "5.001", false));
            DatapointsMap.Add(17, new HeatingDataPoint(17, "hg2", "Kesseltemperatur", "boilertemp", "9.001", false));
            DatapointsMap.Add(18, new HeatingDataPoint(18, "hg2", "Sammlertemperatur", "collectortemp", "9.001", false));
            DatapointsMap.Add(19, new HeatingDataPoint(19, "hg2", "Rücklauftemperatur", "refluxtemp", "9.001", false));
            DatapointsMap.Add(20, new HeatingDataPoint(20, "hg2", "Warmwassertemperatur", "watertemp", "9.001", false));
            DatapointsMap.Add(21, new HeatingDataPoint(21, "hg2", "Außentemperatur", "outsidetemp", "9.001", false));
            DatapointsMap.Add(22, new HeatingDataPoint(22, "hg2", "Status Brenner / Flamme", "burnerstate", "1.001", false));
            DatapointsMap.Add(23, new HeatingDataPoint(23, "hg2", "Status Heizkreispumpe", "heatingcircutpumpstate", "1.001", false));
            DatapointsMap.Add(24, new HeatingDataPoint(24, "hg2", "Status Speicherladepumpe", "tankcharchingpumpstate", "1.001", false));
            DatapointsMap.Add(25, new HeatingDataPoint(25, "hg2", "Status 3-Wege-Umschaltventil", "valvestate", "1.009", false));
            DatapointsMap.Add(26, new HeatingDataPoint(26, "hg2", "Anlagendruck", "pressure", "9.006", false));
            DatapointsMap.Add(27, new HeatingDataPoint(27, "hg3", "Störung", "error", "1.001", false));
            DatapointsMap.Add(28, new HeatingDataPoint(28, "hg3", "Betriebsart", "mode", "20.105", false));
            DatapointsMap.Add(29, new HeatingDataPoint(29, "hg3", "Modulationsgrad / Brennerleistung", "modburnercap", "5.001", false));
            DatapointsMap.Add(30, new HeatingDataPoint(30, "hg3", "Kesseltemperatur", "boilertemp", "9.001", false));
            DatapointsMap.Add(31, new HeatingDataPoint(31, "hg3", "Sammlertemperatur", "collectortemp", "9.001", false));
            DatapointsMap.Add(32, new HeatingDataPoint(32, "hg3", "Rücklauftemperatur", "refluxtemp", "9.001", false));
            DatapointsMap.Add(33, new HeatingDataPoint(33, "hg3", "Warmwassertemperatur", "watertemp", "9.001", false));
            DatapointsMap.Add(34, new HeatingDataPoint(34, "hg3", "Außentemperatur", "outsidetemp", "9.001", false));
            DatapointsMap.Add(35, new HeatingDataPoint(35, "hg3", "Status Brenner / Flamme", "burnerstate", "1.001", false));
            DatapointsMap.Add(36, new HeatingDataPoint(36, "hg3", "Status Heizkreispumpe", "heatingcircutpumpstate", "1.001", false));
            DatapointsMap.Add(37, new HeatingDataPoint(37, "hg3", "Status Speicherladepumpe", "tankcharchingpumpstate", "1.001", false));
            DatapointsMap.Add(38, new HeatingDataPoint(38, "hg3", "Status 3-Wege-Umschaltventil", "valvestate", "1.009", false));
            DatapointsMap.Add(39, new HeatingDataPoint(39, "hg3", "Anlagendruck", "pressure", "9.006", false));
            DatapointsMap.Add(40, new HeatingDataPoint(40, "hg4", "Störung", "error", "1.001", false));
            DatapointsMap.Add(41, new HeatingDataPoint(41, "hg4", "Betriebsart", "mode", "20.105", false));
            DatapointsMap.Add(42, new HeatingDataPoint(42, "hg4", "Modulationsgrad / Brennerleistung", "modburnercap", "5.001", false));
            DatapointsMap.Add(43, new HeatingDataPoint(43, "hg4", "Kesseltemperatur", "boilertemp", "9.001", false));
            DatapointsMap.Add(44, new HeatingDataPoint(44, "hg4", "Sammlertemperatur", "collectortemp", "9.001", false));
            DatapointsMap.Add(45, new HeatingDataPoint(45, "hg4", "Rücklauftemperatur", "refluxtemp", "9.001", false));
            DatapointsMap.Add(46, new HeatingDataPoint(46, "hg4", "Warmwassertemperatur", "watertemp", "9.001", false));
            DatapointsMap.Add(47, new HeatingDataPoint(47, "hg4", "Außentemperatur", "outsidetemp", "9.001", false));
            DatapointsMap.Add(48, new HeatingDataPoint(48, "hg4", "Status Brenner / Flamme", "burnerstate", "1.001", false));
            DatapointsMap.Add(49, new HeatingDataPoint(49, "hg4", "Status Heizkreispumpe", "heatingcircutpumpstate", "1.001", false));
            DatapointsMap.Add(50, new HeatingDataPoint(50, "hg4", "Status Speicherladepumpe", "tankcharchingpumpstate", "1.001", false));
            DatapointsMap.Add(51, new HeatingDataPoint(51, "hg4", "Status 3-Wege-Umschaltventil", "valvestate", "1.009", false));
            DatapointsMap.Add(52, new HeatingDataPoint(52, "hg4", "Anlagendruck", "pressure", "9.006", false));
            DatapointsMap.Add(53, new HeatingDataPoint(53, "sys", "Störung", "error", "1.001", false));
            DatapointsMap.Add(54, new HeatingDataPoint(54, "sys", "Außentemperatur", "outsidetemp", "9.001", false));
            DatapointsMap.Add(55, new HeatingDataPoint(55, "bm1", "Raumtemperatur", "roomtemp", "9.001", false));
            DatapointsMap.Add(56, new HeatingDataPoint(56, "bm1", "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true));
            DatapointsMap.Add(57, new HeatingDataPoint(57, "bm1", "Programmwahl Heizkreis", "heatcircutprogram", "20.102", true));
            DatapointsMap.Add(58, new HeatingDataPoint(58, "bm1", "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true));
            DatapointsMap.Add(59, new HeatingDataPoint(59, "bm1", "Heizkreis Zeitprogramm 1", "heatingcircutprogram1", "1.001", true));
            DatapointsMap.Add(60, new HeatingDataPoint(60, "bm1", "Heizkreis Zeitprogramm 2", "heatingcircutprogram2", "1.001", true));
            DatapointsMap.Add(61, new HeatingDataPoint(61, "bm1", "Heizkreis Zeitprogramm 3", "heatingcircutprogram3", "1.001", true));
            DatapointsMap.Add(62, new HeatingDataPoint(62, "bm1", "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true));
            DatapointsMap.Add(63, new HeatingDataPoint(63, "bm1", "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true));
            DatapointsMap.Add(64, new HeatingDataPoint(64, "bm1", "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true));
            DatapointsMap.Add(65, new HeatingDataPoint(65, "bm1", "Sollwertkorrektur", "targetvaluecorrection", "9.002", true));
            DatapointsMap.Add(66, new HeatingDataPoint(66, "bm1", "Sparfaktor", "savingsfactor", "9.002", true));
            DatapointsMap.Add(67, new HeatingDataPoint(67, "bm2", "Störung", "error", "1.001", false));
            DatapointsMap.Add(68, new HeatingDataPoint(68, "bm2", "Raumtemperatur", "roomtemp", "9.001", false));
            DatapointsMap.Add(69, new HeatingDataPoint(69, "bm2", "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true));
            DatapointsMap.Add(70, new HeatingDataPoint(70, "bm2", "Programmwahl Mischer", "mixerprogram", "20.102", true));
            DatapointsMap.Add(71, new HeatingDataPoint(71, "bm2", "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true));
            DatapointsMap.Add(72, new HeatingDataPoint(72, "bm2", "Mischer Zeitprogramm 1", "mixertimeprogram1", "1.001", true));
            DatapointsMap.Add(73, new HeatingDataPoint(73, "bm2", "Mischer Zeitprogramm 2", "mixertimeprogram2", "1.001", true));
            DatapointsMap.Add(74, new HeatingDataPoint(74, "bm2", "Mischer Zeitprogramm 3", "mixertimeprogram3", "1.001", true));
            DatapointsMap.Add(75, new HeatingDataPoint(75, "bm2", "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true));
            DatapointsMap.Add(76, new HeatingDataPoint(76, "bm2", "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true));
            DatapointsMap.Add(77, new HeatingDataPoint(77, "bm2", "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true));
            DatapointsMap.Add(78, new HeatingDataPoint(78, "bm2", "Sollwertkorrektur", "targetvaluecorrection", "9.002", true));
            DatapointsMap.Add(79, new HeatingDataPoint(79, "bm2", "Sparfaktor", "savingsfactor", "9.002", true));
            DatapointsMap.Add(80, new HeatingDataPoint(80, "bm3", "Störung", "error", "1.001", false));
            DatapointsMap.Add(81, new HeatingDataPoint(81, "bm3", "Raumtemperatur", "roomtemp", "9.001", false));
            DatapointsMap.Add(82, new HeatingDataPoint(82, "bm3", "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true));
            DatapointsMap.Add(83, new HeatingDataPoint(83, "bm3", "Programmwahl Mischer", "mixerprogram", "20.102", true));
            DatapointsMap.Add(84, new HeatingDataPoint(84, "bm3", "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true));
            DatapointsMap.Add(85, new HeatingDataPoint(85, "bm3", "Mischer Zeitprogramm 1", "mixertimeprogram1", "1.001", true));
            DatapointsMap.Add(86, new HeatingDataPoint(86, "bm3", "Mischer Zeitprogramm 2", "mixertimeprogram2", "1.001", true));
            DatapointsMap.Add(87, new HeatingDataPoint(87, "bm3", "Mischer Zeitprogramm 3", "mixertimeprogram3", "1.001", true));
            DatapointsMap.Add(88, new HeatingDataPoint(88, "bm3", "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true));
            DatapointsMap.Add(89, new HeatingDataPoint(89, "bm3", "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true));
            DatapointsMap.Add(90, new HeatingDataPoint(90, "bm3", "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true));
            DatapointsMap.Add(91, new HeatingDataPoint(91, "bm3", "Sollwertkorrektur", "targetvaluecorrection", "9.002", true));
            DatapointsMap.Add(92, new HeatingDataPoint(92, "bm3", "Sparfaktor", "savingsfactor", "9.002", true));
            DatapointsMap.Add(93, new HeatingDataPoint(93, "bm4", "Störung", "error", "1.001", false));
            DatapointsMap.Add(94, new HeatingDataPoint(94, "bm4", "Raumtemperatur", "roomtemp", "9.001", false));
            DatapointsMap.Add(95, new HeatingDataPoint(95, "bm4", "Warmwassersolltemperatur", "warmwatertargettemp", "9.001", true));
            DatapointsMap.Add(96, new HeatingDataPoint(96, "bm4", "Programmwahl Mischer", "mixerprogram", "20.102", true));
            DatapointsMap.Add(97, new HeatingDataPoint(97, "bm4", "Programmwahl Warmwasser", "warmwaterprogram", "20.103", true));
            DatapointsMap.Add(98, new HeatingDataPoint(98, "bm4", "Mischer Zeitprogramm 1", "mixertimeprogram1", "1.001", true));
            DatapointsMap.Add(99, new HeatingDataPoint(99, "bm4", "Mischer Zeitprogramm 2", "mixertimeprogram2", "1.001", true));
            DatapointsMap.Add(100, new HeatingDataPoint(100, "bm4", "Mischer Zeitprogramm 3", "mixertimeprogram3", "1.001", true));
            DatapointsMap.Add(101, new HeatingDataPoint(101, "bm4", "Warmwasser Zeitprogramm 1", "warmwatertimeprogram1", "1.001", true));
            DatapointsMap.Add(102, new HeatingDataPoint(102, "bm4", "Warmwasser Zeitprogramm 2", "warmwatertimeprogram2", "1.001", true));
            DatapointsMap.Add(103, new HeatingDataPoint(103, "bm4", "Warmwasser Zeitprogramm 3", "warmwatertimeprogram3", "1.001", true));
            DatapointsMap.Add(104, new HeatingDataPoint(104, "bm4", "Sollwertkorrektur", "targetvaluecorrection", "9.002", true));
            DatapointsMap.Add(105, new HeatingDataPoint(105, "bm4", "Sparfaktor", "savingsfactor", "9.002", true));
            DatapointsMap.Add(106, new HeatingDataPoint(106, "km", "Störung", "error", "1.001", false));
            DatapointsMap.Add(107, new HeatingDataPoint(107, "km", "Sammlertemperatur", "collectortemperature", "9.001", false));
            DatapointsMap.Add(108, new HeatingDataPoint(108, "km", "Gesamtmodulationsgrad", "totalmodulation", "5.001", false));
            DatapointsMap.Add(109, new HeatingDataPoint(109, "km", "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false));
            DatapointsMap.Add(110, new HeatingDataPoint(110, "km", "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false));
            DatapointsMap.Add(111, new HeatingDataPoint(111, "km", "Status Ausgang A1", "outputA1state", "1.003", false));
            DatapointsMap.Add(112, new HeatingDataPoint(112, "km", "Eingang E1", "inpute1", "9.001", false));
            DatapointsMap.Add(113, new HeatingDataPoint(113, "km", "Eingang E2", "inpute2", "9.001", false));
            DatapointsMap.Add(114, new HeatingDataPoint(114, "mm1", "Störung", "error", "1.001", false));
            DatapointsMap.Add(115, new HeatingDataPoint(115, "mm1", "Warmwassertemperatur", "warmwatertemperature", "9.001", false));
            DatapointsMap.Add(116, new HeatingDataPoint(116, "mm1", "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false));
            DatapointsMap.Add(117, new HeatingDataPoint(117, "mm1", "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false));
            DatapointsMap.Add(118, new HeatingDataPoint(118, "mm1", "Status Ausgang A1", "outputa1state", "1.003", false));
            DatapointsMap.Add(119, new HeatingDataPoint(119, "mm1", "Eingang E1", "inpute1", "9.001", false));
            DatapointsMap.Add(120, new HeatingDataPoint(120, "mm1", "Eingang E2", "inpute2", "9.001", false));
            DatapointsMap.Add(121, new HeatingDataPoint(121, "mm2", "Störung", "error", "1.001", false));
            DatapointsMap.Add(122, new HeatingDataPoint(122, "mm2", "Warmwassertemperatur", "warmwatertemperature", "9.001", false));
            DatapointsMap.Add(123, new HeatingDataPoint(123, "mm2", "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false));
            DatapointsMap.Add(124, new HeatingDataPoint(124, "mm2", "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false));
            DatapointsMap.Add(125, new HeatingDataPoint(125, "mm2", "Status Ausgang A1", "outputa1state", "1.003", false));
            DatapointsMap.Add(126, new HeatingDataPoint(126, "mm2", "Eingang E1", "inpute1", "9.001", false));
            DatapointsMap.Add(127, new HeatingDataPoint(127, "mm2", "Eingang E2", "inpute2", "9.001", false));
            DatapointsMap.Add(128, new HeatingDataPoint(128, "mm3", "Störung", "error", "1.001", false));
            DatapointsMap.Add(129, new HeatingDataPoint(129, "mm3", "Warmwassertemperatur", "warmwatertemperature", "9.001", false));
            DatapointsMap.Add(130, new HeatingDataPoint(130, "mm3", "Vorlauftemperatur Mischerkreis", "mixercircuittemperature", "9.001", false));
            DatapointsMap.Add(131, new HeatingDataPoint(131, "mm3", "Status Mischerkreispumpe", "mixercircuitpumpstate", "1.001", false));
            DatapointsMap.Add(132, new HeatingDataPoint(132, "mm3", "Status Ausgang A1", "outputa1state", "1.003", false));
            DatapointsMap.Add(133, new HeatingDataPoint(133, "mm3", "Eingang E1", "inpute1", "9.001", false));
            DatapointsMap.Add(134, new HeatingDataPoint(134, "mm3", "Eingang E2", "inpute2", "9.001", false));
            DatapointsMap.Add(135, new HeatingDataPoint(135, "sm", "Störung", "error", "1.001", false));
            DatapointsMap.Add(136, new HeatingDataPoint(136, "sm", "Warmwassertemperatur Solar 1", "warmwatertemperaturesolar", "9.001", false));
            DatapointsMap.Add(137, new HeatingDataPoint(137, "sm", "Temperatur Kollektor 1", "collector1temperature", "9.001", false));
            DatapointsMap.Add(138, new HeatingDataPoint(138, "sm", "Eingang E1", "inpute1", "9.001", false));
            DatapointsMap.Add(139, new HeatingDataPoint(139, "sm", "Eingang E2 (Durchfluss)", "inpute2", "9.025", false));
            DatapointsMap.Add(140, new HeatingDataPoint(140, "sm", "Eingang E3", "inpute3", "9.001", false));
            DatapointsMap.Add(141, new HeatingDataPoint(141, "sm", "Status Solarkreispumpe SKP1", "solarcircuitpumpstate", "1.001", false));
            DatapointsMap.Add(142, new HeatingDataPoint(142, "sm", "Status Ausgang A1", "outputa1state", "1.003", false));
            DatapointsMap.Add(143, new HeatingDataPoint(143, "sm", "Status Ausgang A2", "outputa2state", "1.003", false));
            DatapointsMap.Add(144, new HeatingDataPoint(144, "sm", "Status Ausgang A3", "outputa3state", "1.003", false));
            DatapointsMap.Add(145, new HeatingDataPoint(145, "sm", "Status Ausgang A4", "outputa4state", "1.003", false));
            DatapointsMap.Add(146, new HeatingDataPoint(146, "sm", "Durchfluss", "flow", "9.025", false));
            DatapointsMap.Add(147, new HeatingDataPoint(147, "sm", "aktuelle Leistung", "currentpower", "9.024", false));
            DatapointsMap.Add(148, new HeatingDataPoint(148, "cwl", "Störung", "error", "1.001", false));
            DatapointsMap.Add(149, new HeatingDataPoint(149, "cwl", "Programm CWL", "program", "20.103", true));
            DatapointsMap.Add(150, new HeatingDataPoint(150, "cwl", "Zeitprogramm 1", "timeprogram1", "1.001", true));
            DatapointsMap.Add(151, new HeatingDataPoint(151, "cwl", "Zeitprogramm 2", "timeprogrm1", "1.001", true));
            DatapointsMap.Add(152, new HeatingDataPoint(152, "cwl", "Zeitprogramm 3", "timeprogram3", "1.001", true));
            DatapointsMap.Add(153, new HeatingDataPoint(153, "cwl", "Zeitweise Intensivlüftung AN/AUS", "intenseairing", "1.001", true));
            DatapointsMap.Add(154, new HeatingDataPoint(154, "cwl", "Zeitweise Intensivlüftung Startdatum", "intensairingstartdate", "11.001", true));
            DatapointsMap.Add(155, new HeatingDataPoint(155, "cwl", "Zeitweise Intensivlüftung Enddatum", "intensairingenddate", "11.001", true));
            DatapointsMap.Add(156, new HeatingDataPoint(156, "cwl", "Zeitweise Intensivlüftung Startzeit", "intensairingstarttime", "10.001", true));
            DatapointsMap.Add(157, new HeatingDataPoint(157, "cwl", "Zeitweise Intensivlüftung Endzeit", "intensairingendtime", "10.001", true));
            DatapointsMap.Add(158, new HeatingDataPoint(158, "cwl", "Zeitweiser Feuchteschutz AN/AUS", "moistureprotection", "1.001", true));
            DatapointsMap.Add(159, new HeatingDataPoint(159, "cwl", "Zeitweiser Feuchteschutz Startdatum", "moistureprotectionstartdat", "11.001", true));
            DatapointsMap.Add(160, new HeatingDataPoint(160, "cwl", "Zeitweiser Feuchteschutz Enddatum", "moistureprotectionenddate", "11.001", true));
            DatapointsMap.Add(161, new HeatingDataPoint(161, "cwl", "Zeitweiser Feuchteschutz Startzeit", "moistureprotectionstarttim", "10.001", true));
            DatapointsMap.Add(162, new HeatingDataPoint(162, "cwl", "Zeitweiser Feuchteschutz Endzeit", "moistureprotectionendtime", "10.001", true));
            DatapointsMap.Add(163, new HeatingDataPoint(163, "cwl", "Lüftungsstufe", "airinglevel", "5.001", false));
            DatapointsMap.Add(164, new HeatingDataPoint(164, "cwl", "Ablufttemperatur", "exhaustedairtemperature", "9.001", false));
            DatapointsMap.Add(165, new HeatingDataPoint(165, "cwl", "Frischlufttemperatur", "freshairtemperature", "9.001", false));
            DatapointsMap.Add(166, new HeatingDataPoint(166, "cwl", "Luftdurchsatz Zuluft", "throughputfreshair", "13.002", false));
            DatapointsMap.Add(167, new HeatingDataPoint(167, "cwl", "Luftdurchsatz Abluft", "throughtputexhaustedair", "13.002", false));
            DatapointsMap.Add(168, new HeatingDataPoint(168, "cwl", "Bypass Initialisierung", "bypassinit", "1.002", false));
            DatapointsMap.Add(169, new HeatingDataPoint(169, "cwl", "Bypass öffnet/offen", "bypassopen", "1.002", false));
            DatapointsMap.Add(170, new HeatingDataPoint(170, "cwl", "Bypass schließt/geschlossen", "bypassclosed", "1.002", false));
            DatapointsMap.Add(171, new HeatingDataPoint(171, "cwl", "Bypass Fehler", "bypasserror", "1.002", false));
            DatapointsMap.Add(172, new HeatingDataPoint(172, "cwl", "Frost Status: Initialisierung/Warte", "froststateinit", "1.002", false));
            DatapointsMap.Add(173, new HeatingDataPoint(173, "cwl", "Frost Status: Kein Frost", "froststatenone", "1.002", false));
            DatapointsMap.Add(174, new HeatingDataPoint(174, "cwl", "Frost Status: Vorwärmer", "froststatepreheater", "1.002", false));
            DatapointsMap.Add(175, new HeatingDataPoint(175, "cwl", "Frost Status: Fehler/Unausgeglichen", "froststateerror", "1.002", false));
            DatapointsMap.Add(176, new HeatingDataPoint(176, "hg1", "Störung", "error", "1.001", false));
            DatapointsMap.Add(177, new HeatingDataPoint(177, "bwl", "Betriebsart", "operatingmode", "20.105", false));
            DatapointsMap.Add(178, new HeatingDataPoint(178, "bwl", "Heizleistung", "heatingcapacity", "9.024", false));
            DatapointsMap.Add(179, new HeatingDataPoint(179, "bwl", "Kühlleistung", "coolingcapacity", "9.024", false));
            DatapointsMap.Add(180, new HeatingDataPoint(180, "bwl", "Kesseltemperatur", "boilertemperature", "9.001", false));
            DatapointsMap.Add(181, new HeatingDataPoint(181, "bwl", "Sammlertemperatur", "collectortemeperature", "9.001", false));
            DatapointsMap.Add(182, new HeatingDataPoint(182, "bwl", "Rücklauftemperatur", "returntemperature", "9.001", false));
            DatapointsMap.Add(183, new HeatingDataPoint(183, "bwl", "Warmwassertemperatur", "warmwatertemperature", "9.001", false));
            DatapointsMap.Add(184, new HeatingDataPoint(184, "bwl", "Außentemperatur", "outsidetemperature", "9.001", false));
            DatapointsMap.Add(185, new HeatingDataPoint(185, "bwl", "Status Heizkreispumpe", "heatingcircuitpumpstate", "1.001", false));
            DatapointsMap.Add(186, new HeatingDataPoint(186, "bwl", "Status Zubringer-/Heizkreispumpe", "feederpumpstate", "1.001", false));
            DatapointsMap.Add(187, new HeatingDataPoint(187, "bwl", "Status 3-Wege-Umschaltventil HZ/WW", "wwswitchvalvestate", "1.009", false));
            DatapointsMap.Add(188, new HeatingDataPoint(188, "bwl", "Status 3-Wege-Umschaltventil HZ/K", "cwswitchvalvestate", "1.009", false));
            DatapointsMap.Add(189, new HeatingDataPoint(189, "bwl", "Status E-Heizung", "eheatingstate", "1.001", false));
            DatapointsMap.Add(190, new HeatingDataPoint(190, "bwl", "Anlagendruck", "pressure", "9.006", false));
            DatapointsMap.Add(191, new HeatingDataPoint(191, "bwl", "Leistungsaufnahme", "powerconsumption", "9.024", false));
            DatapointsMap.Add(192, new HeatingDataPoint(192, "cwl", "Filterwarnung aktiv", "filterwarning", "1.001", false));
            DatapointsMap.Add(193, new HeatingDataPoint(193, "cwl", "Filterwarnung zurücksetzen", "filterwarningreset", "1.001", true));
            DatapointsMap.Add(194, new HeatingDataPoint(194, "sys", "1xWarmwasserladung", "warmwateronce", "1.001", true));
            DatapointsMap.Add(195, new HeatingDataPoint(195, "sm", "Tagesertrag", "dayyield", "13.010", false));
            DatapointsMap.Add(196, new HeatingDataPoint(196, "sm", "Gesamtertrag", "totalyield", "13.013", false));
            DatapointsMap.Add(197, new HeatingDataPoint(197, "hg1", "Abgastemperatur", "exhaustgastemperature", "9.001", false));
            DatapointsMap.Add(198, new HeatingDataPoint(198, "hg1", "Leistungsvorgabe", "performancetarget", "5.001", true));
            DatapointsMap.Add(199, new HeatingDataPoint(199, "hg1", "Kesselsolltemperaturvorgabe", "boilertemperaturetarget", "9.001", true));
            DatapointsMap.Add(200, new HeatingDataPoint(200, "hg2", "Abgastemperatur", "exhaustgastemperature", "9.001", false));
            DatapointsMap.Add(201, new HeatingDataPoint(201, "hg2", "Leistungsvorgabe", "performancetarget", "5.001", true));
            DatapointsMap.Add(202, new HeatingDataPoint(202, "hg2", "Kesselsolltemperaturvorgabe", "boilertemperaturetarget", "9.001", true));
            DatapointsMap.Add(203, new HeatingDataPoint(203, "hg3", "Abgastemperatur", "exhaustgastemperature", "9.001", false));
            DatapointsMap.Add(204, new HeatingDataPoint(204, "hg3", "Leistungsvorgabe", "performancetarget", "5.001", true));
            DatapointsMap.Add(205, new HeatingDataPoint(205, "hg3", "Kesselsolltemperaturvorgabe", "boilertemperaturetarget", "9.001", true));
            DatapointsMap.Add(206, new HeatingDataPoint(206, "hg4", "Abgastemperatur", "exhaustgastemperature", "9.001", false));
            DatapointsMap.Add(207, new HeatingDataPoint(207, "hg4", "Leistungsvorgabe", "performancetarget", "5.001", true));
            DatapointsMap.Add(208, new HeatingDataPoint(208, "hg4", "Kesselsolltemperaturvorgabe", "boilertemperaturetarget", "9.001", true));
            DatapointsMap.Add(209, new HeatingDataPoint(209, "km", "Gesamtmodulationsgradvorgabe", "totalmodulationtarget", "5.001", true));
            DatapointsMap.Add(210, new HeatingDataPoint(210, "km", "Sammlersolltemeraturvorgabe", "collectortemperaturetarget", "9.001", true));
        }

        public static bool TryGetValue(ushort id, out HeatingDataPoint dp)
        {
            return DatapointsMap.TryGetValue(id, out dp);
        }

        public record HeatingDataPoint(ushort id, string device, string name, string dptname, string dptid, bool writeable);

    }
}

