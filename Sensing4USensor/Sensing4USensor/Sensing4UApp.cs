using Sensing4USensor.Models;
using Sensing4USensor.Utils;
using System.IO;
using System.Windows.Forms;

namespace Sensing4USensor
{
    /// <summary>
    /// Main application form for the Sensing4U Sensor Management System.
    /// Allows users to load, view, filter, and save sensor datasets.
    /// </summary>
    public partial class Sensing4UApp : Form
    {
        /// <summary>
        /// Stores all loaded datasets.
        /// </summary>
        private List<SensorData[,]> datasets = new();

        /// <summary>
        /// Index of the currently selected dataset.
        /// </summary>
        private double currentIndex = 0;

        /// <summary>
        /// Lower threshold for filtering sensor data.
        /// </summary>
        private double lowerBound = 0.0;

        /// <summary>
        /// Upper threshold for filtering sensor data.
        /// </summary>
        private double upperBound = 0.0;

        /// <summary>
        /// Panel to display dynamically created sensor buttons.
        /// </summary>
        private FlowLayoutPanel? pnlSensorButtons;

        /// <summary>
        /// Initializes the application and UI components.
        /// </summary>
        public Sensing4UApp()
        {
            InitializeComponent();
            pnlSensorButtons = new FlowLayoutPanel();
            pnlSensorButtons.Name = "pnlSensorButtons";
            pnlSensorButtons.Dock = DockStyle.Fill;
            pnlSensorButtons.FlowDirection = FlowDirection.TopDown;
            pnlSensorButtons.WrapContents = false;
            pnlSensorButtons.AutoScroll = true;

            groupBox3.Controls.Clear();
            groupBox3.Controls.Add(pnlSensorButtons);
        }

        /// <summary>
        /// Event handler to set the filtering range from input fields.
        /// </summary>
        private void btnSetRange_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtLower.Text, out double low) && double.TryParse(txtUpper.Text, out double high))
            {
                this.lowerBound = low;
                this.upperBound = high;
                MessageBox.Show("Range updated.");
                UpdateGrid();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values.");
            }
        }

        /// <summary>
        /// Resets filtering range to default values (0.0 - 0.0).
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.lowerBound = 0.0;
            this.upperBound = 0.0;
            txtLower.Text = "0.0";
            txtUpper.Text = "0.0";
            UpdateGrid();
            MessageBox.Show("Range reset to 0.0 - 0.0");
        }

        /// <summary>
        /// Updates the name (label) for the currently selected dataset.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNodeLabel.Text))
            {
                MessageBox.Show("Please enter a label.");
                return;
            }

            string newLabel = txtNodeLabel.Text.Trim();

            if (datasets.Count > 0)
            {
                var currentData = datasets[(int)currentIndex];
                for (int d = 0; d < currentData.GetLength(0); d++)
                    for (int h = 0; h < currentData.GetLength(1); h++)
                        currentData[d, h].SensorType = newLabel;
            }

            if (pnlSensorButtons != null)
            {
                foreach (Control ctrl in pnlSensorButtons.Controls)
                {
                    if (ctrl is Button btn && (int)btn.Tag == (int)currentIndex)
                    {
                        btn.Text = newLabel;
                        break;
                    }
                }
            }

            groupBox4.Text = newLabel;
            MessageBox.Show("Sensor label updated successfully.");
        }

        /// <summary>
        /// Loads a sensor data file (.bin or .csv), processes it, and creates a dynamic button.
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Supported Files (*.bin;*.csv)|*.bin;*.csv"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(openFile.FileName).ToLower();
                List<SensorData>? rawList = null;

                if (ext == ".bin")
                    rawList = SensorFileManager.Instance.ReadPythonDetailedBinary(openFile.FileName);
                else if (ext == ".csv")
                    rawList = SensorFileManager.Instance.ReadGridCsv(openFile.FileName);

                if (rawList != null && rawList.Count > 0)
                {
                    var matrix = SensorFileManager.Instance.ToDailyHourlyArray(rawList);
                    datasets.Add(matrix);

                    string sensorName = Path.GetFileNameWithoutExtension(openFile.FileName);
                    int sensorIndex = datasets.Count - 1;

                    Button btn = new Button
                    {
                        Text = sensorName,
                        Width = 120,
                        Height = 30,
                        Tag = sensorIndex,
                        Margin = new Padding(5, 10, 5, 5)
                    };

                    btn.Click += (s, ev) =>
                    {
                        currentIndex = (int)((Button)s).Tag;
                        groupBox4.Text = sensorName;
                        UpdateGrid();
                        UpdateAverage();
                    };

                    pnlSensorButtons.Controls.Add(btn);

                    currentIndex = sensorIndex;
                    groupBox4.Text = sensorName;
                    UpdateGrid();
                    UpdateAverage();
                }
                else
                {
                    MessageBox.Show("Failed to load data.");
                }

                MessageBox.Show("Dataset loaded successfully.");
            }
        }

        /// <summary>
        /// Saves the currently selected dataset to a binary file.
        /// </summary>
        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            if (datasets.Count == 0)
            {
                MessageBox.Show("No data to save.");
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Binary Files (*.bin)|*.bin"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                SensorFileManager.Instance.WriteBinary(saveFile.FileName, datasets[(int)currentIndex]);
                MessageBox.Show("File saved successfully.");
            }
        }

        /// <summary>
        /// Searches for datasets by date and highlights matching rows.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(search) || datasets.Count == 0)
                return;

            var current = datasets[(int)currentIndex];
            int days = current.GetLength(0);
            int hours = current.GetLength(1);

            dgvSensorData.Rows.Clear();
            List<(DateTime, DataGridViewRow)> allRows = new();

            DateTime startDate = new DateTime(2025, 3, 12);

            for (int d = 0; d < days; d++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvSensorData);

                DateTime date = startDate.AddDays(d);
                string dateString = date.ToString("dd/MM/yyyy");
                row.Cells[0].Value = dateString;

                bool isMatch = dateString.ToLower().Contains(search) ||
                               date.ToString("yyyy-MM-dd").Contains(search) ||
                               date.ToString("d").ToLower().Contains(search);

                for (int h = 0; h < hours; h++)
                {
                    var item = current[d, h];
                    item.ColorCategory = SensorColorClassifier.GetColor(item.Value, lowerBound, upperBound);
                    row.Cells[h + 1].Value = item.Value.ToString("F2");
                }

                if (isMatch)
                    row.DefaultCellStyle.BackColor = Color.Yellow;

                allRows.Add((date, row));
            }

            var sortedRows = allRows.OrderBy(x => x.Item1).ToList();

            foreach (var (date, row) in sortedRows)
                dgvSensorData.Rows.Add(row);

            MessageBox.Show("Search completed. Highlighted matching rows.");
        }

        /// <summary>
        /// Clears the interface including the grid and sensor buttons.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvSensorData.Rows.Clear();
            dgvSensorData.Columns.Clear();

            pnlSensorButtons?.Controls.Clear();

            MessageBox.Show("Interface cleared.");
        }

        /// <summary>
        /// Updates the grid with values from the selected dataset.
        /// </summary>
        private void UpdateGrid()
        {
            dgvSensorData.Columns.Clear();
            dgvSensorData.Rows.Clear();

            if (datasets.Count == 0) return;

            var data = datasets[(int)currentIndex];
            int days = data.GetLength(0);
            int hours = data.GetLength(1);

            dgvSensorData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSensorData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSensorData.Columns.Add("Date", "Date");

            for (int h = 0; h < hours; h++)
                dgvSensorData.Columns.Add($"Hour{h}", $"Hour {h}");

            DateTime startDate = new DateTime(2025, 3, 12);
            for (int d = 0; d < days; d++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvSensorData);

                DateTime date = startDate.AddDays(d);
                row.Cells[0].Value = date.ToString("dd/MM/yyyy");

                for (int h = 0; h < hours; h++)
                {
                    var sensor = data[d, h];
                    sensor.ColorCategory = SensorColorClassifier.GetColor(sensor.Value, lowerBound, upperBound);
                    row.Cells[h + 1].Value = sensor.Value.ToString("F2");

                    switch (sensor.ColorCategory)
                    {
                        case ColorCategory.Low:
                            row.Cells[h + 1].Style.BackColor = Color.LightBlue;
                            break;
                        case ColorCategory.Acceptable:
                            row.Cells[h + 1].Style.BackColor = Color.LightGreen;
                            break;
                        case ColorCategory.High:
                            row.Cells[h + 1].Style.BackColor = Color.IndianRed;
                            break;
                    }
                }

                dgvSensorData.Rows.Add(row);
            }
        }

        /// <summary>
        /// Calculates and displays the average sensor value of the current dataset.
        /// </summary>
        private void UpdateAverage()
        {
            if (datasets.Count == 0) return;

            var data = datasets[(int)currentIndex];
            double total = 0;
            int count = 0;

            foreach (var item in data)
            {
                total += item.Value;
                count++;
            }

            txtAverage.Text = (count > 0 ? total / count : 0).ToString("F2");
        }

        /// <summary>
        /// Tests whether the singleton instance of SensorFileManager works correctly.
        /// </summary>
        private void TestSingletonInstance()
        {
            var instance1 = SensorFileManager.Instance;
            var instance2 = SensorFileManager.Instance;

            if (ReferenceEquals(instance1, instance2))
                MessageBox.Show("✅ Singleton test passed: Same instance returned.");
            else
                MessageBox.Show("❌ Singleton test failed: Different instances.");
        }

        /// <summary>
        /// Handles form load event to perform singleton instance test.
        /// </summary>
        private void Sensing4UApp_Load(object sender, EventArgs e)
        {
            TestSingletonInstance();
        }
    }
}
