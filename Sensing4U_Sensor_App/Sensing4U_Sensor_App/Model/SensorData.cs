using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sensing4U_Sensor_App.Model;

namespace Sensing4U_Sensor_App.Model
{
    [Serializable]
     public class SensorData
    {
        public string? Label { get; set; } // Sensor label
        public double Value { get; set; } // Sensor value
        public string? Color { get; set; } // Color representing the sensor value state
    }
}


