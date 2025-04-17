using System;

namespace Sensing4USensor.Models
{
    public enum ColorCategory
    {
        Low,
        Acceptable,
        High
    }

    public class SensorData
    {
        public int Id { get; set; }
        public string SensorType { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public ColorCategory ColorCategory { get; set; }

        public DateTime Date => Timestamp.Date;
        public int Hour => Timestamp.Hour;

        // ✅ Required for array initialization and DataGrid fallback
        public SensorData()
        {
            Id = 0;
            SensorType = "N/A";
            Timestamp = DateTime.MinValue;
            Value = 0.0;
            ColorCategory = ColorCategory.Acceptable;
        }

        public SensorData(int id, string sensorType, DateTime timestamp, double value)
        {
            Id = id;
            SensorType = sensorType;
            Timestamp = timestamp;
            Value = value;
            ColorCategory = ColorCategory.Acceptable;
        }

        public SensorData(string sensorType, DateTime timestamp, double value)
            : this(0, sensorType, timestamp, value)
        {
        }

        public override string ToString()
        {
            return $"{Id} | {SensorType} | {Value:F2} | {Timestamp:yyyy-MM-dd HH:mm} ({ColorCategory})";
        }
    }
}
