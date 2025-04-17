namespace Sensing4USensor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSetRange = new Button();
            label2 = new Label();
            groupBox1 = new GroupBox();
            label4 = new Label();
            txtAverage = new TextBox();
            btnSave = new Button();
            txtUpper = new TextBox();
            txtNodeLabel = new TextBox();
            txtLower = new TextBox();
            label3 = new Label();
            btnReset = new Button();
            groupBox2 = new GroupBox();
            groupBox5 = new GroupBox();
            btnSearch = new Button();
            txtSearch = new TextBox();
            btnSaveToFile = new Button();
            btnLoad = new Button();
            btnClear = new Button();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            dgvSensorData = new DataGridView();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSensorData).BeginInit();
            SuspendLayout();
            // 
            // btnSetRange
            // 
            btnSetRange.Location = new Point(149, 79);
            btnSetRange.Name = "btnSetRange";
            btnSetRange.Size = new Size(94, 29);
            btnSetRange.TabIndex = 3;
            btnSetRange.Text = "Set";
            btnSetRange.UseVisualStyleBackColor = true;
            btnSetRange.Click += btnSetRange_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 38);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 8;
            label2.Text = "SensorRange";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtAverage);
            groupBox1.Controls.Add(btnSave);
            groupBox1.Controls.Add(txtUpper);
            groupBox1.Controls.Add(txtNodeLabel);
            groupBox1.Controls.Add(txtLower);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(btnReset);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btnSetRange);
            groupBox1.Location = new Point(3, 61);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(256, 562);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info Panel";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 278);
            label4.Name = "label4";
            label4.Size = new Size(86, 20);
            label4.TabIndex = 18;
            label4.Text = "Node Label";
            // 
            // txtAverage
            // 
            txtAverage.Location = new Point(9, 231);
            txtAverage.Name = "txtAverage";
            txtAverage.Size = new Size(66, 27);
            txtAverage.TabIndex = 17;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(9, 348);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(232, 29);
            btnSave.TabIndex = 16;
            btnSave.Text = "Save Nodo";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtUpper
            // 
            txtUpper.Location = new Point(81, 79);
            txtUpper.Name = "txtUpper";
            txtUpper.Size = new Size(66, 27);
            txtUpper.TabIndex = 15;
            // 
            // txtNodeLabel
            // 
            txtNodeLabel.Location = new Point(9, 315);
            txtNodeLabel.Name = "txtNodeLabel";
            txtNodeLabel.Size = new Size(229, 27);
            txtNodeLabel.TabIndex = 13;
            // 
            // txtLower
            // 
            txtLower.Location = new Point(9, 79);
            txtLower.Name = "txtLower";
            txtLower.Size = new Size(66, 27);
            txtLower.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 196);
            label3.Name = "label3";
            label3.Size = new Size(112, 20);
            label3.TabIndex = 11;
            label3.Text = "Sensor Avarage";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(9, 135);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(234, 29);
            btnReset.TabIndex = 10;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(btnSaveToFile);
            groupBox2.Controls.Add(btnLoad);
            groupBox2.Controls.Add(btnClear);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Location = new Point(808, 61);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(225, 562);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Actions";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(btnSearch);
            groupBox5.Controls.Add(txtSearch);
            groupBox5.Location = new Point(25, 170);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(185, 116);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Searching";
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(17, 67);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(143, 27);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 34);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(141, 27);
            txtSearch.TabIndex = 2;
            txtSearch.Text = "Search";
            // 
            // btnSaveToFile
            // 
            btnSaveToFile.Location = new Point(40, 112);
            btnSaveToFile.Name = "btnSaveToFile";
            btnSaveToFile.Size = new Size(143, 26);
            btnSaveToFile.TabIndex = 5;
            btnSaveToFile.Text = "Save DataSet";
            btnSaveToFile.UseVisualStyleBackColor = true;
            btnSaveToFile.Click += btnSaveToFile_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(40, 38);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(144, 34);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "Load file";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(40, 78);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(144, 28);
            btnClear.TabIndex = 1;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // groupBox3
            // 
            groupBox3.Location = new Point(25, 315);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(178, 220);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Sensors";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dgvSensorData);
            groupBox4.Location = new Point(277, 61);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(501, 538);
            groupBox4.TabIndex = 13;
            groupBox4.TabStop = false;
            groupBox4.Text = "Temp";
            // 
            // dgvSensorData
            // 
            dgvSensorData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSensorData.Dock = DockStyle.Fill;
            dgvSensorData.Location = new Point(3, 23);
            dgvSensorData.Name = "dgvSensorData";
            dgvSensorData.RowHeadersWidth = 51;
            dgvSensorData.Size = new Size(495, 512);
            dgvSensorData.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 634);
            Controls.Add(groupBox4);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Sensing4USensor";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSensorData).EndInit();
            ResumeLayout(false);



        }



        #endregion
        private Button btnSetRange;
        private Label label2;
        private GroupBox groupBox1;
        private TextBox txtUpper;
        private TextBox txtNodeLabel;
        private TextBox txtLower;
        private Label label3;
        private Button btnReset;
        private Label label4;
        private TextBox txtAverage;
        private Button btnSave;
        private GroupBox groupBox2;
        private Button btnClear;
        private GroupBox groupBox3;
        private Button btnSearch;
        private TextBox txtSearch;
        private GroupBox groupBox4;
        private DataGridView dgvSensorData;
        private Button btnLoad;
        private Button btnSaveToFile;
        private GroupBox groupBox5;
    }
}
