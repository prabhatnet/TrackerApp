using System.Collections.Generic;
using System.IO;

namespace TrackerApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            string currentPath = Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"../../../"));
            string filePath = currentPath + "TrackerDataFoo1.json";
            string filePath2 = currentPath + "TrackerDataFoo2.json";
            List<DataSummary> SummaryData = new List<DataSummary>();

            // Load Files in Objects
            dynamic objTracker1 = TrackerProcessor.LoadFile(filePath);
            dynamic objTracker2 =  TrackerProcessor.LoadFile(filePath2);

            // Map objects 
            TrackerDataFoo1 data1 = TrackerProcessor.GetSensorDataFile1(objTracker1);
            TrackerDataFoo2 data2 = TrackerProcessor.GetSensorDataFile2(objTracker2);

            // Merge Results into common Object
            TrackerProcessor.MergeResults(data1, data2, SummaryData);

            // Display Results
            TrackerProcessor.DisplayResults(SummaryData);
        }
    }
}
