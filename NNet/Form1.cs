using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NNet
{
    public partial class Form1 : Form
    {
        public int selected = 0;
        public List<Classifire> classifirs = new List<Classifire>();
        public WebServer server = new WebServer();

        public bool DataGridToDataSet()
        {
            //double num;
            //DateTime dt;
            classifirs[selected].data.Clear();
            for (int i = 0; i < dataGridView_learn.ColumnCount; i++)
            {
                if (dataGridView_learn.Columns[i].Name.IndexOf("in") >= 0)
                    classifirs[selected].data.inputs.Add(new Classifire.DataSet.Parameter(dataGridView_learn.Columns[i].Name));
                else
                {
                    if (dataGridView_learn.Columns[i].Name.IndexOf("out") >= 0 && 
                        dataGridView_learn.Columns[i].Name.IndexOf("neuro") < 0)
                        classifirs[selected].data.outputs.Add(new Classifire.DataSet.Parameter(dataGridView_learn.Columns[i].Name));
                }
            }

            int index_input, index_output;
            for (int j = 0; j < dataGridView_learn.RowCount; j++)
            {
                index_input = 0;
                index_output = 0;

                for (int i = 0; i < dataGridView_learn.ColumnCount; i++)
                {
                    if (dataGridView_learn[i, j].Value == null) continue;
                    //if (dataGridView_learn[i, j].Value == null ||
                    //    (!double.TryParse(dataGridView_learn[i, j].Value.ToString(), out num) &&
                    //     !DateTime.TryParse(dataGridView_learn[i, j].Value.ToString(), out dt))) break; // Подумать чтобы не произошло смещения данных, если созданный пример частично будет собран, и вдруг null

                    if (dataGridView_learn.Columns[i].Name.IndexOf("in") >= 0)
                    {
                        classifirs[selected].data.inputs[index_input].orig.Add((double)dataGridView_learn[i, j].Value);
                        index_input++;
                    }
                    else
                    {
                        if (dataGridView_learn.Columns[i].Name.IndexOf("out") >= 0 && dataGridView_learn.Columns[i].Name.IndexOf("neuro out") < 0)
                        {
                            classifirs[selected].data.outputs[index_output].orig.Add((double)dataGridView_learn[i, j].Value);
                            index_output++;
                        }
                    }
                }
            }

            if (classifirs[selected].data.inputs.Count <= 0 && classifirs[selected].data.outputs.Count <= 0)
            {
                MessageBox.Show("Нет входных и выходных данных (in/out), возможно не указаны наименования столбцов 'in'/'out'");
                return(false);
            }
            else
            {
                if (classifirs[selected].data.inputs.Count <= 0)
                {
                    MessageBox.Show("Нет входных данных (in), возможно не указаны наименования столбцов 'in'");
                    return (false);
                }

                if (classifirs[selected].data.outputs.Count <= 0)
                {
                    MessageBox.Show("Нет выходных данных (out), возможно не указаны наименования столбцов 'out'");
                    return (false);
                }
            }
            return (true);
        }
        public void DataSetToDataGrid()
        {
            dataGridView_learn.DataSource = null;
            dataGridView_learn.Columns.Clear();

            int count_max = classifirs[selected].data.GetMaxCount();
            if (count_max == 0) return;

            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.Items.Add("Train");
            comboBoxColumn.Items.Add("Test");
            dataGridView_learn.Columns.Add(comboBoxColumn);
            dataGridView_learn.Columns[dataGridView_learn.Columns.Count - 1].HeaderText = "Type";
            dataGridView_learn.Columns.Add("Loss", "Loss (%)");

            dataGridView_learn.Rows.Add(count_max);

            for (int i = 0; i < classifirs[selected].data.inputs.Count; i++)
            {
                dataGridView_learn.Columns.Add("in_" + i.ToString(), classifirs[selected].data.inputs[i].name);
                
                int index_column = dataGridView_learn.Columns.Count - 1;
                for (int ii = 0; ii < classifirs[selected].data.inputs[i].orig.Count; ii++)
                {
                    dataGridView_learn[index_column, ii].Value = classifirs[selected].data.inputs[i].orig[ii];
                }
            }

            for (int i = 0; i < classifirs[selected].data.outputs.Count; i++)
            {
                dataGridView_learn.Columns.Add("out_" + i.ToString(), classifirs[selected].data.outputs[i].name);
                
                int index_column = dataGridView_learn.Columns.Count - 1;
                for (int ii = 0; ii < classifirs[selected].data.outputs[i].orig.Count; ii++)
			    {
                    dataGridView_learn[index_column, ii].Value = classifirs[selected].data.outputs[i].orig[ii];
                }
            }

            for (int i = 0; i < classifirs[selected].data.outputs.Count; i++)
                dataGridView_learn.Columns.Add("neuro out_" + i.ToString(), "neuro " + classifirs[selected].data.outputs[i].name);

            ColumnTypesSet();
        }
        
        public Form1()
        {
            InitializeComponent();

            classifirs.Add(new NeuroNet());

            server.Start();
            server.owner = this;

            timer_learn_to_chart.Start();

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine; // Тип графика - линия
            chart1.Series[0].BorderWidth = 2;       // устанавливаем толщину линии
            chart1.Series[0].Name = "Learn =";
            chart1.ChartAreas[0].CursorX.Interval = 0.001;
            chart1.ChartAreas[0].CursorY.Interval = 0.001;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            Add_Chart_Series("Test =");
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            if (classifirs[selected].thread_fit.IsAlive)
            {
                classifirs[selected].thread_fit.Abort();
                Timer_Update_Chart(sender, e);
                return;
            }

            ColumnTypesSet();

            if (classifirs[selected].layers.Count <= 0)
            {
                Timer_Update_Chart(sender, e);
                classifirs[selected].CreateThread();
            }

            classifirs[selected].FitThread(Convert.ToInt64(textBox_epochs.Text));
        }
        
        private void button_run_train_Click(object sender, EventArgs e)
        {
            if(classifirs[selected].layers.Count<=0) return;

            classifirs[selected].PredictThread();

            while(true)
            {
                if (classifirs[selected].thread_predict.IsAlive) continue;

                for (int i = 0; i < classifirs[selected].data.train.Count; i++)
                {
                    // Заполняем таблицу данными выходов
                    int outs = classifirs[selected].data.train[i].outs_neuro_fact.Count;
                    int start_column = dataGridView_learn.ColumnCount - outs;
                    for (int j = 0; j < outs; j++)
                    {
                        dataGridView_learn[start_column + j, classifirs[selected].data.train[i].index].Value = classifirs[selected].data.train[i].outs_neuro_fact[j].ToString();
                    }
                }

                // Вычисляем результирующий процент
                label_error_train.Text = ((1 - classifirs[selected].data.error_train) * 100).ToString("0.00000") + " %";
                tabControl_model.SelectedIndex = 0;
                break;
            }
        }
        private void button_run_test_Click(object sender, EventArgs e)
        {
            if (classifirs[selected].layers.Count <= 0) return;

            classifirs[selected].EvaluateThread();

            while (true)
            {
                if (classifirs[selected].thread_evaluate.IsAlive) continue;

                // Заполняем таблицу данными выходов
                for (int i = 0; i < classifirs[selected].data.test.Count; i++)
                {
                    int outs = classifirs[selected].data.test[i].outs_neuro_fact.Count;
                    int start_column = dataGridView_learn.ColumnCount - outs;
                    for (int j = 0; j < outs; j++)
                    {
                        dataGridView_learn[start_column + j, classifirs[selected].data.test[i].index].Value = classifirs[selected].data.test[i].outs_neuro_fact[j].ToString();
                    }
                }

                // Вычисляем результирующий процент
                label_error_test.Text = ((1 - classifirs[selected].data.error_test) * 100).ToString("0.00000") + " %";
                tabControl_model.SelectedIndex = 0;
                break;
            }
        }
        
        private void button_model_save_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            classifirs[selected].Save(folder.SelectedPath);
        }
        private void button_model_load_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "*.nn";
                ofd.Filter = "*.nn|*.nn";
                ofd.Title = "Выберите файл модели";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                classifirs[selected].Load(ofd.FileName, ofd.OpenFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_model_clear_Click(object sender, EventArgs e)
        {
            button_save.Enabled = false;
            button_clear.Enabled = false;

            button_starts.Text = "Start training";

            classifirs[selected].thread_fit.Abort();
            classifirs[selected].Clear();

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            progressBar1.Value = 0;
            textBox_epochs.Enabled = true;
            comboBox_neuro_types.Enabled = true;
            textBox_hidden_layers.Enabled = true;
        }
        
        public void button_model_add_new_Click(object sender, EventArgs e)
        {
            treeView_models.Nodes.Add("model#" + (treeView_models.Nodes.Count + 1).ToString());

            classifirs.Add(new NeuroNet(treeView_models.Nodes[treeView_models.Nodes.Count - 1].Text));
        }
        public void button_model_remove_Click(object sender, EventArgs e)
        {
            Type type = sender.GetType();
            int index = selected;
            if (type.Name == "Int32") index = Convert.ToInt32(sender);
            
            treeView_models.Nodes.RemoveAt(index);
            classifirs.RemoveAt(index);
        }
        
        private void Timer_Update_Chart(object sender, EventArgs e)
        {
            if (classifirs[selected].layers.Count <= 0)
            {
                button_starts.Text = "Start training";
                textBox_epochs.Enabled = true;
                button_run_test.Enabled = false;
                button_run_train.Enabled = false;
                comboBox_neuro_types.Enabled = true;
                textBox_hidden_layers.Enabled = true;
                return;
            }

            if (classifirs[selected].thread_fit.IsAlive)
            {
                if (textBox_epochs.Enabled)
                {
                    button_starts.Text = "Stop training";
                    textBox_epochs.Enabled = false;
                    button_run_train.Enabled = false;
                    button_run_test.Enabled = false;
                    button_clear.Enabled = true;
                    button_save.Enabled = true;
                    comboBox_neuro_types.Enabled = false;
                    textBox_hidden_layers.Enabled = false;
                }
            }
            else
            {
                if (button_starts.Text != "Continue training")
                {
                    progressBar1.Value = 0;
                    textBox_epochs.Enabled = true;
                    button_run_test.Enabled = true;
                    button_run_train.Enabled = true;
                    comboBox_neuro_types.Enabled = false;
                    textBox_hidden_layers.Enabled = false;
                    button_starts.Text = "Continue training";
                }
                return;
            }

            progressBar1.Value = (int)((double)classifirs[selected].epoch / (double)Convert.ToInt64(textBox_epochs.Text) * 100);

            for (int i = 0; i < classifirs[selected].data.train.Count; i++)
                dataGridView_learn[1, classifirs[selected].data.train[i].index].Value = classifirs[selected].data.train[i].loss.ToString("0.00000");

            for (int i = 0; i < classifirs[selected].data.test.Count; i++)
                dataGridView_learn[1, classifirs[selected].data.test[i].index].Value = classifirs[selected].data.test[i].loss.ToString("0.00000");

            //// Заполняем таблицу данными выходов
            //for (int i = 0; i < classifire[selected].data.test.Count; i++)
            //{
            //    int outs = classifire[selected].data.test[i].outs_neuro_fact.Count;
            //    int start_column = dataGridView_learn.ColumnCount - outs;
            //    for (int j = 0; j < outs; j++)
            //    {
            //        dataGridView_learn[start_column + j, classifire[selected].data.train[i].index].Value = classifire[selected].data.train[i].outs_neuro_fact[j].ToString();
            //        dataGridView_learn[start_column + j, classifire[selected].data.test[i].index].Value = classifire[selected].data.test[i].outs_neuro_fact[j].ToString();
            //    }
            //}

            // Вычисляем результирующий процент
            label_error_train.Text = ((1 - classifirs[selected].data.error_train) * 100).ToString("0.00000") + " %";
            label_error_test.Text = ((1 - classifirs[selected].data.error_test) * 100).ToString("0.00000") + " %";

            chart1.Series[0].Points.AddY(classifirs[selected].data.error_train);
            chart1.Series[1].Points.AddY(classifirs[selected].data.error_test);

            if (chart1.Series[0].Points.Count > 2000) chart1.Series[0].Points.RemoveAt(0);
            if (chart1.Series[1].Points.Count > 2000) chart1.Series[1].Points.RemoveAt(0);

            if (classifirs[selected].errors_learn.Count > 2000) 
                classifirs[selected].errors_learn.RemoveRange(0, 10);
            if (classifirs[selected].errors_test.Count > 2000)
                classifirs[selected].errors_test.RemoveRange(0, 10);

            try
            {
                double max_learn = classifirs[selected].errors_learn.Max();
                double max_test = classifirs[selected].errors_test.Max();
                chart1.ChartAreas[0].AxisY.Maximum = (max_learn + max_test);
            }
            catch { }

            chart1.Series[0].Name = "Learn = " + classifirs[selected].data.error_train.ToString("0.00000");
            chart1.Series[1].Name = "Test = " + classifirs[selected].data.error_test.ToString("0.00000");
        }
        
        private void Add_Chart_Series(string name)
        {
            chart1.Series.Add(name);
            // Тип графика - линия
            chart1.Series[chart1.Series.Count - 1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            // устанавливаем толщину линии
            chart1.Series[chart1.Series.Count - 1].BorderWidth = 2;
        }
        
        // Загрузка данных из файлов
        private void File_Load_Data_Click(object sender, EventArgs e)
        {
            if (classifirs[selected].thread_fit.IsAlive) return;
            if (File_Load())
            {
                ColumnsCorrecting();

                if (DataGridToDataSet()) ColumnTypesSet();
            }
        }
        private bool File_Load()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "*.xls;*.xlsx";
                ofd.Filter = "Excel 2007(*.xlsx)|*.xlsx|Excel 2003(*.xls)|*.xls";
                ofd.Title = "Выберите документ для загрузки данных";

                if (ofd.ShowDialog() != DialogResult.OK) return (false);

                String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                ofd.FileName +
                ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

                System.Data.OleDb.OleDbConnection con =
                    new System.Data.OleDb.OleDbConnection(constr);
                con.Open();

                DataSet ds = new DataSet();
                DataTable schemaTable = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables,
                    new object[] { null, null, null, "TABLE" });

                string sheet1 = (string)schemaTable.Rows[0].ItemArray[2];
                string select = String.Format("SELECT * FROM [{0}]", sheet1);

                System.Data.OleDb.OleDbDataAdapter ad =
                    new System.Data.OleDb.OleDbDataAdapter(select, con);

                ad.Fill(ds);

                DataTable tb = ds.Tables[0];
                con.Close();

                dataGridView_learn.Columns.Clear();
                DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
                comboBoxColumn.Items.Add("Train");
                comboBoxColumn.Items.Add("Test");
                dataGridView_learn.Columns.Add(comboBoxColumn);
                dataGridView_learn.Columns[dataGridView_learn.Columns.Count - 1].HeaderText = "Type";
                dataGridView_learn.Columns.Add("Loss", "Loss (%)");
                dataGridView_learn.DataSource = tb;
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (false);
            }
        }
        private void ColumnsCorrecting() // Удаление столбцов без in/out, добавление neuro out (добавить расчет средней корреляции)
        {
            int count = dataGridView_learn.Columns.Count;
            for (int i = 2; i < count; i++)
            {
                if (dataGridView_learn.Columns[i].Name.IndexOf("in") < 0 &&
                    dataGridView_learn.Columns[i].Name.IndexOf("out") < 0)
                {
                    dataGridView_learn.Columns.RemoveAt(i);
                    continue;
                }

                if (dataGridView_learn.Columns[i].Name.IndexOf("out") >= 0)
                    dataGridView_learn.Columns.Add("neuro " + dataGridView_learn.Columns[i].Name, "neuro " + dataGridView_learn.Columns[i].Name);
            }
        }
        public void ColumnTypesSet() // РЕФАКТОРИНГ!!!
        {
            if (classifirs[selected].data.data_types.Count > dataGridView_learn.RowCount)
            {
                // Типы данных из таблицы
                //classifirs[selected].data.data_types.Clear();
                //for (int i = 0; i < dataGridView_learn.RowCount; i++)
                for (int i = classifirs[selected].data.data_types.Count; i < dataGridView_learn.RowCount; i++)
                {
                    classifirs[selected].data.AddDataTypeByName((string)dataGridView_learn[0, i].Value);
                }
            }
            else
            {
                if (classifirs[selected].data.data_types.Count == 0)
                    classifirs[selected].data.Separation();

                // Типы данных в таблицу
                for (int i = 0; i < classifirs[selected].data.data_types.Count; i++)
                {
                    dataGridView_learn[0, i].Value = classifirs[selected].data.GetDataTypeNameByIndex(i);
                }
            }
        }

        // Обновление интерфейса из-за обновления выбранной нейросети по API
        public void UpdateInterface()
        {
            numericUpDown_speed.Value = (decimal)classifirs[selected].speed;
            numericUpDown_moment.Value = (decimal)classifirs[selected].moment;
            textBox_name.Text = classifirs[selected].name;
            textBox_epochs.Text = classifirs[selected].epoch.ToString();
            textBox_hidden_layers.Text = classifirs[selected].layers_hidden;
            comboBox_neuro_types.Text = Classifire.type_names[(int)classifirs[selected].type];
        }

        // Изменения свойств через интерфейс
        private void NumericUpDown_speed_ValueChanged(object sender, EventArgs e)
        {
            classifirs[selected].Set(ENUM_PARAMETERS.SPEED, numericUpDown_speed.Value);
        }
        private void NumericUpDown_moment_ValueChanged(object sender, EventArgs e)
        {
            classifirs[selected].Set(ENUM_PARAMETERS.MOMENT, numericUpDown_moment.Value);
        }
        private void TextBox_name_TextChanged(object sender, EventArgs e)
        {
            classifirs[selected].Set(ENUM_PARAMETERS.NAME, textBox_name.Text);
            treeView_models.SelectedNode.Text = textBox_name.Text;
        }
        private void TextBox_name_Click(object sender, EventArgs e)
        {
            if (treeView_models.SelectedNode == null)
            {
                if (treeView_models.Nodes.Count == 0)
                    treeView_models.Nodes.Add(textBox_name.Text);

                treeView_models.SelectedNode = treeView_models.Nodes[treeView_models.Nodes.Count - 1];
            }
        }
        private void TextBox_epochs_TextChanged(object sender, EventArgs e)
        {
            classifirs[selected].Set(ENUM_PARAMETERS.EPOCH, textBox_epochs.Text);
        }
        private void TextBox_hidden_layers_TextChanged(object sender, EventArgs e)
        {
            classifirs[selected].Set(ENUM_PARAMETERS.LAYERS, textBox_hidden_layers.Text);
        }
        private void TreeView_models_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selected = treeView_models.SelectedNode.Index;
            DataSetToDataGrid();
            UpdateInterface();
        }
        private void ComboBox_recurrence_TextChanged(object sender, EventArgs e)
        {
            switch (comboBox_neuro_types.Text)
            {
                case "NN":
                    classifirs[selected].Set(ENUM_PARAMETERS.TYPE, ENUM_CLASSIFIRIES.NN);
                    break;
                case "RNN":
                    classifirs[selected].Set(ENUM_PARAMETERS.TYPE, ENUM_CLASSIFIRIES.RNN);
                    break;
                case "CNN":
                    classifirs[selected].Set(ENUM_PARAMETERS.TYPE, ENUM_CLASSIFIRIES.CNN);
                    break;
                case "GRU":
                    classifirs[selected].Set(ENUM_PARAMETERS.TYPE, ENUM_CLASSIFIRIES.GRU);
                    break;
                case "LSTM":
                    classifirs[selected].Set(ENUM_PARAMETERS.TYPE, ENUM_CLASSIFIRIES.LSTM);
                    break;
            }
        }
        private void treeView_models_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            treeView_models.Nodes[selected].EndEdit(true);
        }
    }
}
