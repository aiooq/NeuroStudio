namespace NNet
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("model#1");
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer_learn_to_chart = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_model_remove = new System.Windows.Forms.Button();
            this.treeView_models = new System.Windows.Forms.TreeView();
            this.button_add_new_model = new System.Windows.Forms.Button();
            this.tabControl_model = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.dataGridView_learn = new System.Windows.Forms.DataGridView();
            this.tabChart = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_error_test = new System.Windows.Forms.Label();
            this.label_error_train = new System.Windows.Forms.Label();
            this.button_load = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_run_train = new System.Windows.Forms.Button();
            this.label_runs = new System.Windows.Forms.Label();
            this.numericUpDown_moment = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_speed = new System.Windows.Forms.NumericUpDown();
            this.label_traning = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_epochs = new System.Windows.Forms.TextBox();
            this.button_run_test = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_starts = new System.Windows.Forms.Button();
            this.label_data = new System.Windows.Forms.Label();
            this.button_data = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_hidden_layers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_neuro_types = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl_model.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_learn)).BeginInit();
            this.tabChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_moment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_speed)).BeginInit();
            this.SuspendLayout();
            // 
            // timer_learn_to_chart
            // 
            this.timer_learn_to_chart.Tick += new System.EventHandler(this.Timer_Update_Chart);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 585);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1111, 10);
            this.progressBar1.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_model_remove);
            this.splitContainer1.Panel1.Controls.Add(this.treeView_models);
            this.splitContainer1.Panel1.Controls.Add(this.button_add_new_model);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_model);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1111, 583);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 4;
            // 
            // button_model_remove
            // 
            this.button_model_remove.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_model_remove.Location = new System.Drawing.Point(0, 560);
            this.button_model_remove.Name = "button_model_remove";
            this.button_model_remove.Size = new System.Drawing.Size(130, 23);
            this.button_model_remove.TabIndex = 4;
            this.button_model_remove.Text = "Remove model";
            this.button_model_remove.UseVisualStyleBackColor = true;
            this.button_model_remove.Click += new System.EventHandler(this.button_model_remove_Click);
            // 
            // treeView_models
            // 
            this.treeView_models.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_models.HotTracking = true;
            this.treeView_models.LabelEdit = true;
            this.treeView_models.Location = new System.Drawing.Point(0, 23);
            this.treeView_models.Name = "treeView_models";
            treeNode2.Name = "Узел0";
            treeNode2.Text = "model#1";
            this.treeView_models.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView_models.Size = new System.Drawing.Size(130, 560);
            this.treeView_models.TabIndex = 3;
            this.treeView_models.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_models_BeforeLabelEdit);
            this.treeView_models.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_models_AfterSelect);
            // 
            // button_add_new_model
            // 
            this.button_add_new_model.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_add_new_model.Location = new System.Drawing.Point(0, 0);
            this.button_add_new_model.Name = "button_add_new_model";
            this.button_add_new_model.Size = new System.Drawing.Size(130, 23);
            this.button_add_new_model.TabIndex = 1;
            this.button_add_new_model.Text = "Add new model";
            this.button_add_new_model.UseVisualStyleBackColor = true;
            this.button_add_new_model.Click += new System.EventHandler(this.button_model_add_new_Click);
            // 
            // tabControl_model
            // 
            this.tabControl_model.Controls.Add(this.tabData);
            this.tabControl_model.Controls.Add(this.tabChart);
            this.tabControl_model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_model.HotTrack = true;
            this.tabControl_model.Location = new System.Drawing.Point(190, 0);
            this.tabControl_model.Name = "tabControl_model";
            this.tabControl_model.SelectedIndex = 0;
            this.tabControl_model.Size = new System.Drawing.Size(787, 583);
            this.tabControl_model.TabIndex = 5;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dataGridView_learn);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Size = new System.Drawing.Size(779, 557);
            this.tabData.TabIndex = 2;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // dataGridView_learn
            // 
            this.dataGridView_learn.AllowUserToOrderColumns = true;
            this.dataGridView_learn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_learn.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView_learn.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_learn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_learn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_learn.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_learn.Name = "dataGridView_learn";
            this.dataGridView_learn.Size = new System.Drawing.Size(779, 557);
            this.dataGridView_learn.TabIndex = 0;
            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.chart1);
            this.tabChart.Location = new System.Drawing.Point(4, 22);
            this.tabChart.Name = "tabChart";
            this.tabChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabChart.Size = new System.Drawing.Size(779, 557);
            this.tabChart.TabIndex = 1;
            this.tabChart.Text = "Chart";
            this.tabChart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(773, 551);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_error_test);
            this.panel1.Controls.Add(this.label_error_train);
            this.panel1.Controls.Add(this.button_load);
            this.panel1.Controls.Add(this.button_clear);
            this.panel1.Controls.Add(this.button_run_train);
            this.panel1.Controls.Add(this.label_runs);
            this.panel1.Controls.Add(this.numericUpDown_moment);
            this.panel1.Controls.Add(this.numericUpDown_speed);
            this.panel1.Controls.Add(this.label_traning);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBox_epochs);
            this.panel1.Controls.Add(this.button_run_test);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Controls.Add(this.button_starts);
            this.panel1.Controls.Add(this.label_data);
            this.panel1.Controls.Add(this.button_data);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBox_name);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox_hidden_layers);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox_neuro_types);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 583);
            this.panel1.TabIndex = 4;
            // 
            // label_error_test
            // 
            this.label_error_test.AutoSize = true;
            this.label_error_test.Location = new System.Drawing.Point(97, 468);
            this.label_error_test.Name = "label_error_test";
            this.label_error_test.Size = new System.Drawing.Size(33, 13);
            this.label_error_test.TabIndex = 76;
            this.label_error_test.Text = "value";
            // 
            // label_error_train
            // 
            this.label_error_train.AutoSize = true;
            this.label_error_train.Location = new System.Drawing.Point(97, 434);
            this.label_error_train.Name = "label_error_train";
            this.label_error_train.Size = new System.Drawing.Size(33, 13);
            this.label_error_train.TabIndex = 75;
            this.label_error_train.Text = "value";
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(6, 514);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(100, 29);
            this.button_load.TabIndex = 74;
            this.button_load.Text = "Load";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_model_load_Click);
            // 
            // button_clear
            // 
            this.button_clear.Enabled = false;
            this.button_clear.Location = new System.Drawing.Point(5, 546);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(177, 29);
            this.button_clear.TabIndex = 73;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_model_clear_Click);
            // 
            // button_run_train
            // 
            this.button_run_train.Enabled = false;
            this.button_run_train.Location = new System.Drawing.Point(6, 426);
            this.button_run_train.Name = "button_run_train";
            this.button_run_train.Size = new System.Drawing.Size(85, 28);
            this.button_run_train.TabIndex = 72;
            this.button_run_train.Text = "Train ->";
            this.button_run_train.UseVisualStyleBackColor = true;
            this.button_run_train.Click += new System.EventHandler(this.button_run_train_Click);
            // 
            // label_runs
            // 
            this.label_runs.AutoSize = true;
            this.label_runs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_runs.Location = new System.Drawing.Point(13, 405);
            this.label_runs.Name = "label_runs";
            this.label_runs.Size = new System.Drawing.Size(167, 16);
            this.label_runs.TabIndex = 71;
            this.label_runs.Text = "Run model to datasets:";
            // 
            // numericUpDown_moment
            // 
            this.numericUpDown_moment.DecimalPlaces = 2;
            this.numericUpDown_moment.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDown_moment.Location = new System.Drawing.Point(113, 325);
            this.numericUpDown_moment.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_moment.Name = "numericUpDown_moment";
            this.numericUpDown_moment.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown_moment.TabIndex = 69;
            this.numericUpDown_moment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_moment.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_moment.ValueChanged += new System.EventHandler(this.NumericUpDown_moment_ValueChanged);
            // 
            // numericUpDown_speed
            // 
            this.numericUpDown_speed.DecimalPlaces = 2;
            this.numericUpDown_speed.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDown_speed.Location = new System.Drawing.Point(113, 299);
            this.numericUpDown_speed.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_speed.Name = "numericUpDown_speed";
            this.numericUpDown_speed.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown_speed.TabIndex = 68;
            this.numericUpDown_speed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_speed.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_speed.ValueChanged += new System.EventHandler(this.NumericUpDown_speed_ValueChanged);
            // 
            // label_traning
            // 
            this.label_traning.AutoSize = true;
            this.label_traning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_traning.Location = new System.Drawing.Point(61, 235);
            this.label_traning.Name = "label_traning";
            this.label_traning.Size = new System.Drawing.Size(65, 16);
            this.label_traning.TabIndex = 67;
            this.label_traning.Text = "Training";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 66;
            this.label6.Text = "Moment";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 65;
            this.label7.Text = "Speed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 64;
            this.label4.Text = "Epochs";
            // 
            // textBox_epochs
            // 
            this.textBox_epochs.Location = new System.Drawing.Point(6, 274);
            this.textBox_epochs.Name = "textBox_epochs";
            this.textBox_epochs.Size = new System.Drawing.Size(176, 20);
            this.textBox_epochs.TabIndex = 63;
            this.textBox_epochs.Text = "100000";
            this.textBox_epochs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_epochs.TextChanged += new System.EventHandler(this.TextBox_epochs_TextChanged);
            // 
            // button_run_test
            // 
            this.button_run_test.Enabled = false;
            this.button_run_test.Location = new System.Drawing.Point(6, 460);
            this.button_run_test.Name = "button_run_test";
            this.button_run_test.Size = new System.Drawing.Size(85, 28);
            this.button_run_test.TabIndex = 62;
            this.button_run_test.Text = "Test ->";
            this.button_run_test.UseVisualStyleBackColor = true;
            this.button_run_test.Click += new System.EventHandler(this.button_run_test_Click);
            // 
            // button_save
            // 
            this.button_save.Enabled = false;
            this.button_save.Location = new System.Drawing.Point(113, 514);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(69, 29);
            this.button_save.TabIndex = 61;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_model_save_Click);
            // 
            // button_starts
            // 
            this.button_starts.Location = new System.Drawing.Point(5, 350);
            this.button_starts.Name = "button_starts";
            this.button_starts.Size = new System.Drawing.Size(177, 29);
            this.button_starts.TabIndex = 60;
            this.button_starts.Text = "Start training";
            this.button_starts.UseVisualStyleBackColor = true;
            this.button_starts.Click += new System.EventHandler(this.button_start_Click);
            // 
            // label_data
            // 
            this.label_data.AutoSize = true;
            this.label_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_data.Location = new System.Drawing.Point(72, 167);
            this.label_data.Name = "label_data";
            this.label_data.Size = new System.Drawing.Size(41, 16);
            this.label_data.TabIndex = 59;
            this.label_data.Text = "Data";
            // 
            // button_data
            // 
            this.button_data.Location = new System.Drawing.Point(6, 185);
            this.button_data.Name = "button_data";
            this.button_data.Size = new System.Drawing.Size(178, 29);
            this.button_data.TabIndex = 58;
            this.button_data.Text = "Select data...";
            this.button_data.UseVisualStyleBackColor = true;
            this.button_data.Click += new System.EventHandler(this.File_Load_Data_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 57;
            this.label5.Text = "Name";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(6, 51);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(178, 20);
            this.textBox_name.TabIndex = 56;
            this.textBox_name.Text = "model#1";
            this.textBox_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_name.Click += new System.EventHandler(this.TextBox_name_Click);
            this.textBox_name.TextChanged += new System.EventHandler(this.TextBox_name_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Hidden layers";
            // 
            // textBox_hidden_layers
            // 
            this.textBox_hidden_layers.Location = new System.Drawing.Point(6, 96);
            this.textBox_hidden_layers.Name = "textBox_hidden_layers";
            this.textBox_hidden_layers.Size = new System.Drawing.Size(178, 20);
            this.textBox_hidden_layers.TabIndex = 53;
            this.textBox_hidden_layers.Text = "20,20";
            this.textBox_hidden_layers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_hidden_layers.TextChanged += new System.EventHandler(this.TextBox_hidden_layers_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(46, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "Configuration";
            // 
            // comboBox_neuro_types
            // 
            this.comboBox_neuro_types.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox_neuro_types.FormattingEnabled = true;
            this.comboBox_neuro_types.Items.AddRange(new object[] {
            "NN",
            "RNN"});
            this.comboBox_neuro_types.Location = new System.Drawing.Point(6, 136);
            this.comboBox_neuro_types.Name = "comboBox_neuro_types";
            this.comboBox_neuro_types.Size = new System.Drawing.Size(178, 21);
            this.comboBox_neuro_types.TabIndex = 51;
            this.comboBox_neuro_types.Text = "NN";
            this.comboBox_neuro_types.TextChanged += new System.EventHandler(this.ComboBox_recurrence_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1111, 595);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Classifire";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl_model.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_learn)).EndInit();
            this.tabChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_moment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_speed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_learn_to_chart;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView_models;
        private System.Windows.Forms.Button button_add_new_model;
        private System.Windows.Forms.TabControl tabControl_model;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.DataGridView dataGridView_learn;
        private System.Windows.Forms.TabPage tabChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_run_train;
        private System.Windows.Forms.Label label_runs;
        private System.Windows.Forms.NumericUpDown numericUpDown_moment;
        private System.Windows.Forms.NumericUpDown numericUpDown_speed;
        private System.Windows.Forms.Label label_traning;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_epochs;
        private System.Windows.Forms.Button button_run_test;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_starts;
        private System.Windows.Forms.Label label_data;
        private System.Windows.Forms.Button button_data;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_hidden_layers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_neuro_types;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Label label_error_test;
        private System.Windows.Forms.Label label_error_train;
        private System.Windows.Forms.Button button_model_remove;
    }
}

