using Sensing4U_Sensor_App.Model;
using Sensing4U_Sensor_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class DataProcessor
{
    private static DataProcessor? _instance;
    private LinkedList<SensorData> _sensorData;

    private DataProcessor()
    {
        _sensorData = new LinkedList<SensorData>();
    }

    public static DataProcessor Instance => _instance ?? (_instance = new DataProcessor());

    public LinkedList<SensorData> SensorData => _sensorData;

    //Load bin data management 
    public void LoadData(string filePath, bool isBinary)
    {
        _sensorData.Clear();

        if (isBinary)
            _sensorData = FileHandler.LoadFromBinary(filePath);
        else
            _sensorData = FileHandler.LoadFromCsv(filePath);
    }

    // Sort data
    public void SortData(string sortBy)
    {
        switch (sortBy.ToLower())
        {
            case "label":
                _sensorData = new LinkedList<SensorData>(_sensorData.OrderBy(x => x.Label));
                break;
            case "value":
                _sensorData = new LinkedList<SensorData>(_sensorData.OrderBy(x => x.Value));
                break;
            case "color":
                _sensorData = new LinkedList<SensorData>(_sensorData.OrderBy(x => x.Color));
                break;
            default:
                Console.WriteLine("Invalid sort option.");
                break;
        }
    }

    // Update Colours 
    public void UpdateColorForData(double lowerRange, double upperRange)
    {
        foreach (var sensor in _sensorData)
        {
            if (sensor.Value < lowerRange)
                sensor.Color = "Blue";  // Below range
            else if (sensor.Value > upperRange)
                sensor.Color = "Red";   // Above range
            else
                sensor.Color = "Green"; // Within range
        }
    }
    // Calculate Average
    public double CalculateAverage()
    {
        return _sensorData.Average(x => x.Value);
    }

    // Binary Search
    public SensorData? BinarySearch(string label)
    {
        var dataArray = _sensorData.ToArray();
        int left = 0;
        int right = dataArray.Length - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int comparison = string.Compare(dataArray[mid].Label, label, StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
                return dataArray[mid];
            else if (comparison < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return null; // Not found
    }
}
