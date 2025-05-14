using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sensing4USensor.Models;

namespace Sensing4USensor.Utils
{
    /// <summary>
    /// Provides methods to read and write sensor data from binary and CSV files.
    /// Implements the Singleton pattern for consistent file management.
    /// </summary>
    public class SensorFileManager
    {
        private static SensorFileManager? _instance;

        /// <summary>
        /// Private constructor to enforce Singleton pattern.
        /// </summary>
        private SensorFileManager() { }

        /// <summary>
        /// Singleton instance of the SensorFileManager.
        /// </summary>
        public static SensorFileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SensorFileManager();
                return _instance;
            }
        }

        /// <summary>
        /// Saves a 2D array of sensor data to a binary file.
        /// </summary>
        /// <param name="path">Path where the file will be saved.</param>
        /// <param name="data">The 2D array of SensorData to write.</param>
        public void WriteBinary(string path, SensorData[,] data)
        {
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Create);
                using BinaryWriter writer = new BinaryWriter(fs);

                int rows = data.GetLength(0);
                int cols = data.GetLength(1);

                writer.Write(rows);
                writer.Write(cols);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        var sensor = data[i, j];
                        writer.Write(sensor.Id);
                        writer.Write(sensor.SensorType + '\0');
                        writer.Write(sensor.Timestamp.Ticks);
                        writer.Write(sensor.Value);
                    }
                }

                Console.WriteLine($"✅ Saved binary file: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Writing binary file: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads sensor data from a binary file created using Python-compatible format.
        /// </summary>
        /// <param name="path">Path of the binary file.</param>
        /// <returns>List of SensorData objects or null if an error occurs.</returns>
        public List<SensorData>? ReadPythonDetailedBinary(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                var data = new List<SensorData>();
                using FileStream fs = new FileStream(path, FileMode.Open);
                using BinaryReader reader = new BinaryReader(fs);

                while (fs.Position < fs.Length)
                {
                    int id = reader.ReadInt32();

                    List<byte> bytes = new();
                    byte b;
                    while ((b = reader.ReadByte()) != 0)
                        bytes.Add(b);

                    string sensorType = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                    long ticks = reader.ReadInt64();
                    DateTime timestamp = new DateTime(ticks);
                    double value = reader.ReadDouble();

                    data.Add(new SensorData(id, sensorType, timestamp, value));
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Reading binary file: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Reads sensor data from a CSV file with the format: Date,hour 0,...,hour 23.
        /// </summary>
        /// <param name="path">Path to the CSV file.</param>
        /// <returns>List of SensorData or null if reading fails.</returns>
        public List<SensorData>? ReadGridCsv(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                var data = new List<SensorData>();
                var lines = File.ReadAllLines(path);

                var header = lines[0].Split(',');
                int sensorId = 1;
                string sensorType = "GridCSV";

                for (int i = 1; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length < 25) continue;

                    if (!DateTime.TryParse(parts[0], out DateTime baseDate))
                        continue;

                    for (int h = 0; h < 24; h++)
                    {
                        if (double.TryParse(parts[h + 1], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            DateTime timestamp = baseDate.AddHours(h);
                            data.Add(new SensorData(sensorId, sensorType, timestamp, value));
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Reading Grid CSV: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts a flat list of SensorData into a 2D array with one column.
        /// </summary>
        /// <param name="list">The list to convert.</param>
        /// <returns>A 2D array of SensorData [n,1].</returns>
        public SensorData[,] To2DArray(List<SensorData> list)
        {
            var result = new SensorData[list.Count, 1];
            for (int i = 0; i < list.Count; i++)
            {
                result[i, 0] = list[i];
            }
            return result;
        }

        /// <summary>
        /// Converts a list of SensorData into a 2D array organized by [day, hour].
        /// </summary>
        /// <param name="data">Flat list of sensor data records.</param>
        /// <returns>2D array of SensorData grouped by day and hour.</returns>
        public SensorData[,] ToDailyHourlyArray(List<SensorData> data)
        {
            var groupedByDate = new SortedDictionary<DateTime, SensorData[]>();

            foreach (var sensor in data)
            {
                var date = sensor.Timestamp.Date;

                if (!groupedByDate.ContainsKey(date))
                    groupedByDate[date] = new SensorData[24];

                int hour = sensor.Timestamp.Hour;

                if (groupedByDate[date][hour] == null)
                {
                    groupedByDate[date][hour] = sensor;
                }
            }

            int days = groupedByDate.Count;
            var result = new SensorData[days, 24];
            int dayIndex = 0;

            foreach (var day in groupedByDate.Keys)
            {
                for (int h = 0; h < 24; h++)
                {
                    result[dayIndex, h] = groupedByDate[day][h] ?? new SensorData
                    {
                        Id = -1,
                        SensorType = "N/A",
                        Timestamp = DateTime.MinValue,
                        Value = 0.0
                    };
                }
                dayIndex++;
            }

            return result;
        }
    }
}
