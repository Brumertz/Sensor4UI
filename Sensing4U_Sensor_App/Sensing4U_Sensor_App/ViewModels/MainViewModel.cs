using Sensing4U_Sensor_App.Model;
using Sensing4U_Sensor_App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace Sensing4U_Sensor_App.ViweModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _lowerRange = 10.0;
        private double _upperRange = 50.0;
        private ObservableCollection<SensorData> _sensorData = new ObservableCollection<SensorData>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<SensorData> SensorData
        {
            get => _sensorData;
            set
            {
                _sensorData = value;
                OnPropertyChanged();
            }
        }

        public double LowerRange
        {
            get => _lowerRange;
            set
            {
                _lowerRange = value;
                OnPropertyChanged();
                UpdateColors();
            }
        }

        public double UpperRange
        {
            get => _upperRange;
            set
            {
                _upperRange = value;
                OnPropertyChanged();
                UpdateColors();
            }
        }

        public MainViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var data = DataProcessor.Instance.SensorData;
            SensorData = new ObservableCollection<SensorData>(data);
        }

        private void UpdateColors()
        {
            DataProcessor.Instance.UpdateColorForData(LowerRange, UpperRange);
            LoadData();
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double CalculateAverage()
        {
            return DataProcessor.Instance.CalculateAverage();
        }

        public void LoadDataCommand(string filePath, bool isBinary)
        {
            DataProcessor.Instance.LoadData(filePath, isBinary);
            LoadData();
        }

        public void SaveDataCommand(string filePath, bool isBinary)
        {
            if (isBinary)
                FileHandler.SaveToBinary(DataProcessor.Instance.SensorData, filePath);
            else
                FileHandler.SaveToCsv(DataProcessor.Instance.SensorData, filePath);
        }
    }

}
