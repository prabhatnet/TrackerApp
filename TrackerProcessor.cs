using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrackerApplication
{
    static class TrackerProcessor
    {
        public static void DisplayResults(List<DataSummary> SummaryData)
        {
            foreach (DataSummary summary in SummaryData)
            {
                Console.WriteLine("Tracker Id: " + summary.TrackerId);
                Console.WriteLine("Tracker Id: " + summary.TrackerName);
                Console.WriteLine("Average Temp: " + summary.AvgTemp);
                Console.WriteLine("Average Humidiy: " + summary.AvgHumidity);
                Console.WriteLine("Total Temperature Count: " + summary.TempCount);
                Console.WriteLine("Total Humidity Count: " + summary.HumidityCount);
                Console.WriteLine("First Reported Record: " + summary.FirstCrumbDtm);
                Console.WriteLine("Last Reported Record: " + summary.LastCrumbDtm);
                Console.WriteLine("---------------------------------------------");
            }
            Console.ReadLine();
        }

        public static void MergeResults(TrackerDataFoo1 data1, TrackerDataFoo2 data2, List<DataSummary> SummaryData)
        {
            foreach (Tracker tracker in data1.Trackers)
            {
                DataSummary summary = FillTracker1Details(data1, tracker);
                SummaryData.Add(summary);
            }

            foreach (Device device in data2.Devices)
            {
                DataSummary summary = FillTracker2Details(data2, device);
                SummaryData.Add(summary);
            }
        }

        private static DataSummary FillTracker1Details(TrackerDataFoo1 data1, Tracker tracker)
        {
            DataSummary summary = new DataSummary();
            summary.CompanyId = data1.CompanyId;
            summary.CompanyName = data1.CompanyName;
            summary.TrackerId = tracker.TrackerId;
            summary.TrackerName = tracker.TrackerName;
            summary.FirstCrumbDtm = tracker.Sensors?.FirstOrDefault()?.Crumbs.FirstOrDefault()?.CreatedDtm;
            summary.LastCrumbDtm = tracker?.Sensors?.FirstOrDefault().Crumbs.LastOrDefault().CreatedDtm;

            // Temperature Final Values
            List<Sensor> tempSensors = tracker?.Sensors.Where(x => x.Name == "Temperature").ToList();
            double SumTotalTemp = 0;
            summary.TempCount = 0;
            foreach (Sensor temp in tempSensors)
            {
                summary.TempCount += temp.Crumbs.Count;
                SumTotalTemp = Convert.ToDouble(temp.Crumbs.Sum(X => X.Value).Value);
            }
            if (summary.TempCount > 0)
                summary.AvgTemp = SumTotalTemp / summary.TempCount;

            // Humidity Final Values
            List<Sensor> tempSensors1 = tracker?.Sensors.Where(x => x.Name == "Humidty").ToList();
            double SumTotalHum = 0;
            summary.HumidityCount = 0;
            foreach (Sensor hum in tempSensors1)
            {
                summary.HumidityCount += hum.Crumbs.Count;
                SumTotalHum = Convert.ToDouble(hum.Crumbs.Sum(X => X.Value).Value);
            }
            if (summary.HumidityCount > 0)
                summary.AvgHumidity = SumTotalHum / summary.HumidityCount;
            return summary;
        }

        private static DataSummary FillTracker2Details(TrackerDataFoo2 data2, Device device)
        {
            DataSummary summary = new DataSummary();
            summary.CompanyId = data2.CompanyId;
            summary.CompanyName = data2.CompanyName;
            summary.TrackerId = device.TrackerId;
            summary.TrackerName = device.TrackerName;
            summary.FirstCrumbDtm = device?.SensorDatas?.FirstOrDefault()?.CreatedDtm;
            summary.LastCrumbDtm = device?.SensorDatas?.LastOrDefault().CreatedDtm;

            // Temperature Final Values
            List<SensorData> tempValues = device?.SensorDatas.Where(x => x.SensorType == "TEMP").ToList();
            summary.TempCount = 0;
            summary.TempCount += tempValues.Count();
            summary.AvgTemp = Convert.ToDouble(tempValues.Average(x => x.Value));

            // Humidity Final Values
            List<SensorData> humValues = device?.SensorDatas.Where(x => x.SensorType == "HUM").ToList();
            summary.HumidityCount = 0;
            summary.HumidityCount += humValues.Count();
            summary.AvgHumidity = Convert.ToDouble(humValues.Average(x => x.Value));
            return summary;
        }

        public static TrackerDataFoo1 GetSensorDataFile1(dynamic objTracker1)
        {
            TrackerDataFoo1 data = new TrackerDataFoo1();
            data.CompanyId = objTracker1.PartnerId;
            data.CompanyName = objTracker1.PartnerName;
            data.Trackers = new System.Collections.Generic.List<Tracker>();
            foreach (dynamic tracker in objTracker1.Trackers)
            {
                Tracker trac = new Tracker();
                trac.TrackerId = tracker.Id;
                trac.TrackerName = tracker.Model;
                trac.StartDate = tracker.ShipmentStartDtm;
                trac.Sensors = new System.Collections.Generic.List<Sensor>();

                Sensor sensorTemp = new Sensor(); //Temperature
                sensorTemp.Id = tracker.Sensors[0].Id;
                sensorTemp.Name = tracker.Sensors[0].Name;
                sensorTemp.Crumbs = new System.Collections.Generic.List<Crumb>();
                foreach (dynamic crumb in tracker.Sensors[0].Crumbs) //Temperature
                {
                    Crumb cru = new Crumb();
                    cru.CreatedDtm = crumb.CreatedDtm;
                    cru.Value = crumb.Value;
                    sensorTemp.Crumbs.Add(cru);
                }

                trac.Sensors.Add(sensorTemp);
                Sensor sensorHum = new Sensor(); //Humidity
                sensorHum.Id = tracker.Sensors[1].Id;
                sensorHum.Name = tracker.Sensors[1].Name;
                sensorHum.Crumbs = new System.Collections.Generic.List<Crumb>();
                foreach (dynamic crumb in tracker.Sensors[1].Crumbs)
                {
                    Crumb cru = new Crumb();
                    cru.CreatedDtm = crumb.CreatedDtm;
                    cru.Value = crumb.Value;
                    sensorHum.Crumbs.Add(cru);
                }
                trac.Sensors.Add(sensorHum);

                data.Trackers.Add(trac);

            }

            return data;
        }

        public static TrackerDataFoo2 GetSensorDataFile2(dynamic objTracker2)
        {
            TrackerDataFoo2 data = new TrackerDataFoo2();
            data.CompanyId = objTracker2.CompanyId; //
            data.CompanyName = objTracker2.Company; //
            data.Devices = new System.Collections.Generic.List<Device>();
            foreach (dynamic tracker in objTracker2.Devices)
            {
                Device trac = new Device();
                trac.TrackerId = tracker.DeviceID;  //
                trac.TrackerName = tracker.Name;
                trac.StartDate = tracker.StartDateTime;

                trac.SensorDatas = new System.Collections.Generic.List<SensorData>();
                foreach (dynamic sdata in tracker.SensorData) //All data collection
                {
                    SensorData sd = new SensorData();
                    sd.SensorType = sdata.SensorType;
                    sd.CreatedDtm = sdata.DateTime;
                    sd.Value = sdata.Value;
                    trac.SensorDatas.Add(sd);
                }
                data.Devices.Add(trac);
            }

            return data;
        }

        public static dynamic LoadFile(string filePath)
        {
            JObject o2 = new JObject();
            // read JSON directly from a file

            using (StreamReader file = File.OpenText(filePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                o2 = (JObject)JToken.ReadFrom(reader);

            }
            var jsonString = o2.ToString();
            var objTracker1 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
            return objTracker1;
        }
    }
}
