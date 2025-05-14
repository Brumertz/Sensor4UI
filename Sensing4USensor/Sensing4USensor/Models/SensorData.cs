using System;

namespace Sensing4USensor.Models
{
    /// <summary>
    /// Represents the color category of a sensor reading, based on its value range.
    /// </summary>
    public enum ColorCategory
    {
        /// <summary>Sensor value is below the acceptable lower bound.</summary>
        Low,
        /// <summary>Sensor value is within the acceptable range.</summary>
        Acceptable,
        /// <summary>Sensor value is above the acceptable upper bound.</summary>
        High
    }

    /// <summary>
    /// Represents a sensor reading including metadata like timestamp, value, and category.
    /// </summary>
    public class SensorData
    {
        /// <summary>Unique identifier for the sensor reading.</summary>
        public int Id { get; set; }

        /// <summary>Type or label of the sensor that generated the reading.</summary>
        public string SensorType { get; set; }

        /// <summary>Date and time of the sensor reading.</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Measured value from the sensor.</summary>
        public double Value { get; set; }

        /// <summary>Color category assigned based on the value range.</summary>
        public ColorCategory ColorCategory { get; set; }

        /// <summary>
        /// Extracts the date portion of the timestamp (ignores time).
        /// </summary>
        public DateTime Date => Timestamp.Date;

        /// <summary>
        /// Extracts the hour portion of the timestamp.
        /// </summary>
        public int Hour => Timestamp.Hour;

        /// <summary>
        /// Default constructor for fallback or placeholder data.
        /// </summary>
        public SensorData()
        {
            Id = 0;
            SensorType = "N/A";
            Timestamp = DateTime.MinValue;
            Value = 0.0;
            ColorCategory = ColorCategory.Acceptable;
        }

        /// <summary>
        /// Creates a SensorData object with full properties.
        /// </summary>
        /// <param name="id">Sensor ID.</param>
        /// <param name="sensorType">Sensor label or type.</param>
        /// <param name="timestamp">Timestamp of the reading.</param>
        /// <param name="value">Measured value.</param>
        public SensorData(int id, string sensorType, DateTime timestamp, double value)
        {
            Id = id;
            SensorType = sensorType;
            Timestamp = timestamp;
            Value = value;
            ColorCategory = ColorCategory.Acceptable;
        }

        /// <summary>
        /// Overload constructor with default ID.
        /// </summary>
        /// <param name="sensorType">Sensor label or type.</param>
        /// <param name="timestamp">Timestamp of the reading.</param>
        /// <param name="value">Measured value.</param>
        public SensorData(string sensorType, DateTime timestamp, double value)
            : this(0, sensorType, timestamp, value)
        {
        }

        /// <summary>
        /// Converts the SensorData instance into a formatted string.
        /// </summary>
        /// <returns>Formatted sensor reading with category and timestamp.</returns>
        public override string ToString()
        {
            return $"{Id} | {SensorType} | {Value:F2} | {Timestamp:yyyy-MM-dd HH:mm} ({ColorCategory})";
        }
    }
}
