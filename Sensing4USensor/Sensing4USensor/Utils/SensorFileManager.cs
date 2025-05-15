using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Sensing4USensor.Models;

namespace Sensing4USensor.Utils
{
    /// <summary>
    /// Singleton class responsible for reading and writing sensor data,
    /// and managing a shared list of sensor records across the application.
    /// </summary>
    public class SensorFileManager
    {
        // Singleton instance holder
        private static SensorFileManager? _instance;

        /// <summary>
        /// Provides access to the singleton instance.
        /// </summary>
        public static SensorFileManager Instance => _instance ??= new SensorFileManager();

        // Shared sensor data used throughout the application
        private static List<SensorData> sensorData = new();

        /// <summary>
        /// Returns the shared list of sensor data.
        /// </summary>
        public List<SensorData> SensorData => sensorData;

        /// <summary>
        /// Clears all sensor data stored in the singleton.
        /// </summary>
        public void ClearData()
        {
            sensorData.Clear();
            Console.WriteLine("✅ Sensor data has been cleared.");
        }

        /// <summary>
        /// Converts a list of SensorData into a 2D array [day, hour].
        /// </summary>
        public SensorData[,] ToDailyHourlyArray(ref List<SensorData> data)
        {
            var groupedByDate = new SortedDictionary<DateTime, SensorData[]>();

            foreach (var sensor in data)
            {
                var date = sensor.Timestamp.Date;

                if (!groupedByDate.ContainsKey(date))
                    groupedByDate[date] = new SensorData[24];

                int hour = sensor.Timestamp.Hour;
                if (groupedByDate[date][hour] == null)
                    groupedByDate[date][hour] = sensor;
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
        /// <summary>
        /// Loads sensor data from a binary file into the shared list.
        /// </summary>
        public void LoadFromBinary(string path)
        {
            Trace.WriteLine("Entering btnLoad_Click()");
            if (!File.Exists(path)) return;

            try
            {
                var loadedData = new List<SensorData>();

                using FileStream fs = new FileStream(path, FileMode.Open);
                using BinaryReader reader = new BinaryReader(fs);

                while (fs.Position < fs.Length)
                {
                    int id = reader.ReadInt32();

                    var bytes = new List<byte>();
                    byte b;
                    while ((b = reader.ReadByte()) != 0)
                        bytes.Add(b);

                    string sensorType = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                    long ticks = reader.ReadInt64();
                    DateTime timestamp = new DateTime(ticks);
                    double value = reader.ReadDouble();

                    loadedData.Add(new SensorData(id, sensorType, timestamp, value));
                }

                sensorData = loadedData;
                Console.WriteLine($"✅ Loaded {loadedData.Count} sensor records from: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to read binary file: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads a Python-style detailed binary file and returns the list of SensorData.
        /// </summary>
        public List<SensorData> ReadPythonDetailedBinary(string path)
        {
            Trace.WriteLine("Entering btnLoad_Click()");
            var loadedData = new List<SensorData>();
            if (!File.Exists(path)) return loadedData;

            try
            {
                using FileStream fs = new FileStream(path, FileMode.Open);
                using BinaryReader reader = new BinaryReader(fs);

                while (fs.Position < fs.Length)
                {
                    int id = reader.ReadInt32();

                    var bytes = new List<byte>();
                    byte b;
                    while ((b = reader.ReadByte()) != 0)
                        bytes.Add(b);

                    string sensorType = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                    long ticks = reader.ReadInt64();
                    DateTime timestamp = new DateTime(ticks);
                    double value = reader.ReadDouble();

                    loadedData.Add(new SensorData(id, sensorType, timestamp, value));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to read binary file: {ex.Message}");
            }

            // Optionally update the shared list as well
            sensorData = new List<SensorData>(loadedData);
            return loadedData;
        }

        /// <summary>
        /// Loads sensor data from a CSV file with hourly columns into the shared list.
        /// </summary>
        public void LoadFromCsv(string path)
        {
            Trace.WriteLine("Entering btnLoad_Click()");
            if (!File.Exists(path)) return;

            try
            {
                var loadedData = new List<SensorData>();
                var lines = File.ReadAllLines(path);

                string sensorType = "GridCSV";
                int sensorId = 1;

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
                            loadedData.Add(new SensorData(sensorId++, sensorType, timestamp, value));
                        }
                    }
                }

                sensorData = loadedData;
                Console.WriteLine($"✅ Loaded {loadedData.Count} records from CSV: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to read CSV file: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads a grid CSV file and returns the flat list of SensorData.
        /// </summary>
        public List<SensorData> ReadGridCsv(string path)
        {
            Trace.WriteLine("Entering btnLoad_Click()");
            var loadedData = new List<SensorData>();
            if (!File.Exists(path)) return loadedData;

            try
            {
                var lines = File.ReadAllLines(path);
                string sensorType = "GridCSV";
                int sensorId = 1;

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
                            loadedData.Add(new SensorData(sensorId++, sensorType, timestamp, value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to read CSV file: {ex.Message}");
            }

            // Optionally update the shared list as well
            sensorData = new List<SensorData>(loadedData);
            return loadedData;
        }

        /// <summary>
        /// Saves the provided 2D array of SensorData to a binary file.
        /// </summary>
        public void WriteBinary(string path, SensorData[,] sensorArray)
        {
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Create);
                using BinaryWriter writer = new BinaryWriter(fs);

                foreach (var sensor in sensorArray)
                {
                    writer.Write(sensor.Id);
                    writer.Write(sensor.SensorType + '\0');
                    writer.Write(sensor.Timestamp.Ticks);
                    writer.Write(sensor.Value);
                }

                Console.WriteLine($"✅ Sensor data saved to: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to write binary file: {ex.Message}");
            }
        }

    }
}
