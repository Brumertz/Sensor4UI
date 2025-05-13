using Sensing4USensor.Models;
using Sensing4USensor.Utils;
using System.IO;
using System.Windows.Forms;

namespace Sensing4USensor
{
    public partial class Sensing4UApp : Form
    {
        private List<SensorData[,]> datasets = new();
        private double currentIndex = 0;
        private double lowerBound = 0.0;
        private double upperBound = 0.0;
        private FlowLayoutPanel? pnlSensorButtons;
        public Sensing4UApp()
        {
            InitializeComponent();
            pnlSensorButtons = new FlowLayoutPanel();
            pnlSensorButtons.Name = "pnlSensorButtons";
            pnlSensorButtons.Dock = DockStyle.Fill;
            pnlSensorButtons.FlowDirection = FlowDirection.TopDown;
            pnlSensorButtons.WrapContents = false;
            pnlSensorButtons.AutoScroll = true;

            groupBox3.Controls.Clear(); // optional: remove old buttons like SensorA/B
            groupBox3.Controls.Add(pnlSensorButtons);

        }

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

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.lowerBound = 0.0;
            this.upperBound = 0.0;
            txtLower.Text = "0.0";
            txtUpper.Text = "0.0";
            UpdateGrid();

            MessageBox.Show("Range reset to 0.0 - 0.0");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNodeLabel.Text))
            {
                MessageBox.Show("Please enter a label.");
                return;
            }

            string newLabel = txtNodeLabel.Text.Trim();

            // 1. Update the current dataset's SensorType
            if (datasets.Count > 0)
            {
                var currentData = datasets[(int)currentIndex];

                for (int d = 0; d < currentData.GetLength(0); d++)
                {
                    for (int h = 0; h < currentData.GetLength(1); h++)
                    {
                        currentData[d, h].SensorType = newLabel;
                    }
                }
            }

            // 2. Update the button text in pnlSensorButtons
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

            // 3. Update groupBox4 label (the grid title)
            groupBox4.Text = newLabel;

            MessageBox.Show("Sensor label updated successfully.");
        }

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
                    // Convert list to 2D array
                    var matrix = SensorFileManager.Instance.ToDailyHourlyArray(rawList);
                    datasets.Add(matrix);

                    // Get sensor name from file name
                    string sensorName = Path.GetFileNameWithoutExtension(openFile.FileName);
                    int sensorIndex = datasets.Count - 1;

                    // Create dynamic button
                    Button btn = new Button
                    {
                        Text = sensorName,
                        Width = 120,
                        Height = 30,
                        Tag = sensorIndex,
                        Margin = new Padding(5, 10, 5, 5) // Top spacing
                    };

                    // Set click event
                    btn.Click += (s, ev) =>
                    {
                        currentIndex = (int)((Button)s).Tag;
                        groupBox4.Text = sensorName;
                        UpdateGrid();
                        UpdateAverage();
                    };

                    // Add button to persistent panel
                    pnlSensorButtons.Controls.Add(btn);

                    // Automatically select the new dataset
                    currentIndex = sensorIndex;
                    groupBox4.Text = sensorName;
                    UpdateGrid();
                    UpdateAverage();
                }
                else
                {
                    MessageBox.Show("Failed to load data.");
                }
            }
        }



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
                SensorFileManager.Instance.WriteBinary(saveFile.FileName, datasets[(int)currentIndex]); // Fix: Cast currentIndex to int
                MessageBox.Show("File saved successfully.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string search = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(search) || datasets.Count == 0)
                return;

            var current = datasets[(int)currentIndex];
            int days = current.GetLength(0);
            int hours = current.GetLength(1);

            dgvSensorData.Rows.Clear();

            List<(DateTime, DataGridViewRow)> allRows = new(); // 📅 Store with date for sorting

            DateTime startDate = new DateTime(2025, 3, 12); // Adjust if dynamic

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
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow; // ✅ Highlight matched rows
                }

                allRows.Add((date, row)); // 📅 Save with date for sorting
            }

            // 📅 Sort all rows by date ascending
            var sortedRows = allRows.OrderBy(x => x.Item1).ToList();

            foreach (var (date, row) in sortedRows)
                dgvSensorData.Rows.Add(row);
            MessageBox.Show("Search completed. Highlighted matching rows.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvSensorData.Rows.Clear();
            dgvSensorData.Columns.Clear();

            if (pnlSensorButtons != null)
                pnlSensorButtons.Controls.Clear();

            MessageBox.Show("Interface cleared.");
        }


        // Refresh the DataGridView with colors based on range

        private void UpdateGrid()
        {
            dgvSensorData.Columns.Clear();
            dgvSensorData.Rows.Clear();

            if (datasets.Count == 0) return;

            var data = datasets[(int)currentIndex]; // Fix: Cast currentIndex to int
            int days = data.GetLength(0); // ✅ ahora días es el primer índice
            int hours = data.GetLength(1); // ✅ y horas el segundo

            dgvSensorData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSensorData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ➕ Primera columna: Fecha
            dgvSensorData.Columns.Add("Date", "Date");

            // ➕ Crear columnas para cada hora
            for (int h = 0; h < hours; h++)
            {
                dgvSensorData.Columns.Add($"Hour{h}", $"Hour {h}");
            }

            // 🔁 Crear filas por día
            DateTime startDate = new DateTime(2025, 3, 12); // o la fecha inicial de tu archivo
            for (int d = 0; d < days; d++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvSensorData);

                DateTime date = startDate.AddDays(d);
                row.Cells[0].Value = date.ToString("dd/MM/yyyy");

                for (int h = 0; h < hours; h++)
                {
                    var sensor = data[d, h]; // ✅ nota: [día, hora]

                    sensor.ColorCategory = SensorColorClassifier.GetColor((double)sensor.Value, lowerBound, upperBound);
                    row.Cells[h + 1].Value = sensor.Value.ToString("F2");

                    // 🎨 Aplicar color según categoría
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


        // Calculate average value from current dataset
        private void UpdateAverage()
        {
            if (datasets.Count == 0) return;

            var data = datasets[(int)currentIndex]; // Fix: Cast currentIndex to int
            double total = 0;
            int count = 0;

            foreach (var item in data)
            {
                total += item.Value;
                count++;
            }

            txtAverage.Text = (count > 0 ? total / count : 0).ToString("F2");
        }
        private void TestSingletonInstance()
        {
            var instance1 = SensorFileManager.Instance;
            var instance2 = SensorFileManager.Instance;

            if (ReferenceEquals(instance1, instance2))
                MessageBox.Show("✅ Singleton test passed: Same instance returned.");
            else
                MessageBox.Show("❌ Singleton test failed: Different instances.");
        }

        private void Sensing4UApp_Load(object sender, EventArgs e)
        {
            TestSingletonInstance();
        }
    }
}

