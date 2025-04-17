using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sensing4USensor.Models;

namespace Sensing4USensor.Utils
{
    public static class SensorFileManager
    {
        /// <summary>
        /// Writes SensorData[,] to a binary file.
        /// </summary>
        public static void WriteBinary(string path, SensorData[,] data)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
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
                            writer.Write(sensor.SensorType + '\0'); // null-terminated string
                            writer.Write(sensor.Timestamp.Ticks);   // .NET ticks
                            writer.Write(sensor.Value);             // double
                        }
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
        /// Reads SensorData list from a binary file (Python-compatible format).
        /// </summary>
        public static List<SensorData>? ReadPythonDetailedBinary(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                var data = new List<SensorData>();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int id = reader.ReadInt32();

                        // Read null-terminated string
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
        /// Reads SensorData from a CSV file formatted as: Date,hour 0,hour 1,...,hour 23
        /// </summary>
        public static List<SensorData>? ReadGridCsv(string path)
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
        /// Converts a flat list to a 2D array [count, 1].
        /// </summary>
        public static SensorData[,] To2DArray(List<SensorData> list)
        {
            var result = new SensorData[list.Count, 1];
            for (int i = 0; i < list.Count; i++)
            {
                result[i, 0] = list[i];
            }
            return result;
        }

        /// <summary>
        /// Groups SensorData into a [day, hour] 2D array.
        /// </summary>
        public static SensorData[,] ToDailyHourlyArray(List<SensorData> data)
        {
            var groupedByDate = new SortedDictionary<DateTime, SensorData[]>();

            foreach (var sensor in data)
            {
                var date = sensor.Timestamp.Date;

                if (!groupedByDate.ContainsKey(date))
                    groupedByDate[date] = new SensorData[24];

                int hour = sensor.Timestamp.Hour;

                // Only assign if it's not already set
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
    }}