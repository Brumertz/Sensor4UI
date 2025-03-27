using Sensing4U_Sensor_App.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sensing4U_Sensor_App.Services
{
    public static class FileHandler
    {
        // Load Bin Data
        public static LinkedList<SensorData> LoadFromBinary(string filePath)
        {
            LinkedList<SensorData> data = new LinkedList<SensorData>();

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        string label = reader.ReadString();
                        double value = reader.ReadDouble();
                        string color = reader.ReadString();
                        data.AddLast(new SensorData { Label = label, Value = value, Color = color });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading binary file: {ex.Message}");
            }

            return data;
        }
        //Save Bin Data
        public static void SaveToBinary(LinkedList<SensorData> data, string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    foreach (var item in data)
                    {
                        writer.Write(item.Label);
                        writer.Write(item.Value);
                        writer.Write(item.Color);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to binary file: {ex.Message}");
            }
        }
        

        public static LinkedList<SensorData> LoadFromCsv(string filePath)
        {
            LinkedList<SensorData> data = new LinkedList<SensorData>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        string label = parts[0];
                        double value = Convert.ToDouble(parts[1]);
                        string color = parts[2];
                        data.AddLast(new SensorData { Label = label, Value = value, Color = color });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV file: {ex.Message}");
            }

            return data;
        }
       
        // Save for csv
        public static void SaveToCsv(LinkedList<SensorData> data, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var item in data)
                    {
                        writer.WriteLine($"{item.Label},{item.Value},{item.Color}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to CSV file: {ex.Message}");
            }
        }
    
        //Convert to 2dArray
    public static string[,] ConvertTo2DArray(LinkedList<SensorData> data)
        {
            int rowCount = data.Count;
            string[,] array = new string[rowCount, 2];

            int row = 0;
            foreach (var sensor in data)
            {
                array[row, 0] = sensor.Label ?? string.Empty; // Fix for CS8601
                array[row, 1] = sensor.Value.ToString();
                row++;
            }

            return array;
        }
       
        /// Prints the 2D array for debugging.
        public static void Print2DArray(string[,] data)
        {
            int rows = data.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"Row {i}: {data[i, 0]}, {data[i, 1]}");
            }
        }
    }
}
