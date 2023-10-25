using demo.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using demo.BLL;
using demo.Utils;

namespace demo.UI
{
    public partial class Warehouse_Management : Form
    {
        LJsql LJsql = new LJsql();
        attribute attribute = new attribute();
        Management management = new Management();

        private void Data()
        {
            attribute.Source_Warehouse = comboBox1.Text;
            attribute.Destination_warehouse = comboBox2.Text;
            attribute.Date = dateTimePicker1.Text;
            attribute.Part_Name = comboBox3.Text;
            attribute.Batch_Number = comboBox4.Text;
        }

        public Warehouse_Management()
        {
            InitializeComponent();
            Source_Warehouse_test();
        }

        private void Source_Warehouse_test()
        {
            DataSet dataSet = LJsql.DataGet($"select Name from warehouses");
            comboBox1.TextChanged -= comboBox1_TextChanged;
            comboBox1.DataSource = dataSet.Tables[0];
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
            comboBox1.Text = null;
            comboBox1.TextChanged += comboBox1_TextChanged;
        }

        private void Destination_Warehouse_test()
        {
            DataSet dataSet = LJsql.DataGet($"select Name from warehouses where Name != \"{comboBox1.Text}\"");
            comboBox2.DataSource = dataSet.Tables[0];
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Name";
            comboBox2.Text = null;
        }

        private void Part_test()
        {
            int index = Convert.ToInt32(LJsql.SelectData($"select ID from warehouses where Name = \"{comboBox1.Text}\""));
            DataSet dataSet = LJsql.DataGet($"SELECT parts.`Name` FROM orders,orderitems,parts WHERE orderitems.PartID = parts.ID AND orders.ID = orderitems.OrderID AND DestinationWarehouseID = {index} GROUP BY parts.`Name`;");
            comboBox3.DataSource = dataSet.Tables[0];
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Name";
        }

        private void Batch_test()
        {
            int index = Convert.ToInt32(LJsql.SelectData($"select ID from warehouses where Name = \"{comboBox1.Text}\""));
            DataSet dataSet = LJsql.DataGet($"SELECT BatchNumber FROM orders,orderitems,parts WHERE orderitems.PartID = parts.ID AND orders.ID = orderitems.OrderID AND DestinationWarehouseID = {index} and parts.`Name` = \"{comboBox3.Text}\" GROUP BY BatchNumber;");
            comboBox4.DataSource = dataSet.Tables[0];
            comboBox4.DisplayMember = "BatchNumber";
            comboBox4.ValueMember = "BatchNumber";
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Part_test();
            Destination_Warehouse_test();
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            Batch_test();
        }

        private void Null()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data();
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            attribute.Amount = textBox1.Text;
            bool t_amount = management.Add(attribute.Source_Warehouse,attribute.Part_Name,attribute.Batch_Number,attribute.Amount);
            if (t_amount == false)
            {
                MessageBox.Show("数量不足");
                return;
            }
            else 
            {
                string[] arr = new string[] {attribute.Part_Name, attribute.Batch_Number, attribute.Amount, "Remove" };
                dataGridView1.Rows.Add(arr);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("是否退出", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Manag manag = new Manag();
                this.Hide();
                manag.ShowDialog();
            }
            else
            {
                return;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                Data();
                attribute.Part_Name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                attribute.Batch_Number = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                attribute.Amount = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                management.Dele(attribute.Source_Warehouse, attribute.Part_Name, attribute.Batch_Number, attribute.Amount);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Data();
            int value = management.Move(attribute.Source_Warehouse,attribute.Destination_warehouse,attribute.Date);
            if (value != 1) 
            {
                MessageBox.Show("添加失败");
                return;
            }
            MessageBox.Show("添加成功");
            dataGridView1.Rows.Clear();
            comboBox3.Text = null;
            comboBox4.Text = null;
            textBox1.Text = null;
        }
    }
}
