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
using System.Windows.Forms.VisualStyles;
using demo.Utils;
using demo.BLL;

namespace demo
{
    public partial class Purchase_Orders : Form
    {
        List<object> listall = new List<object>();
        string[] strings = new string[3];
        LJsql LJsql = new LJsql();
        Part part = new Part();
        attribute attribute = new attribute();
        purchase_Orders _Orders = new purchase_Orders();    

        public Purchase_Orders()
        {
            InitializeComponent();
            Suppliers_test();
            Warehouse_test();
            Part_test();
            Batch_test();
        }

        private void Suppliers_test() 
        {
            DataSet dataSet = LJsql.DataGet("select Name from Suppliers");
            comboBox1.DataSource = dataSet.Tables[0];
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
        }

        private void Warehouse_test() 
        {
            DataSet dataSet = LJsql.DataGet("select Name from warehouses");
            comboBox2.DataSource = dataSet.Tables[0];
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Name";
        }

        private void Part_test() 
        {
            DataSet dataSet = LJsql.DataGet("select Name from parts");
            comboBox3.DataSource = dataSet.Tables[0];
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Name";
        }

        private void Batch_test() 
        {
            DataSet dataSet = LJsql.DataGet("select BatchNumber FROM orderitems WHERE BatchNumber != \"\" GROUP BY BatchNumber");
            comboBox4.DataSource = dataSet.Tables[0];
            comboBox4.DisplayMember = "BatchNumber";
            comboBox4.ValueMember = "BatchNumber";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("是否退出","",MessageBoxButtons.YesNo);
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

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled =false;
            comboBox2.Enabled =false;
            dateTimePicker1.Enabled =false;
            attribute.Suppliers = comboBox1.Text;
            attribute.Warehouse = comboBox2.Text;
            attribute.Date = dateTimePicker1.Text;
            attribute.Part_Name = comboBox3.Text;
            attribute.Batch_Number = comboBox4.Text;
            attribute.Amount = textBox1.Text;
            string[] listarr;
            if (_Orders.IsBatchNum(attribute.Part_Name) == true)
            {
                listarr = new string[4] { attribute.Part_Name, attribute.Batch_Number, attribute.Amount, "Remove" };
            }
            else 
            {
                listarr = new string[4] { attribute.Part_Name, "", attribute.Amount, "Remove" };
            }
            strings = new string[] { attribute.Suppliers, attribute.Warehouse, attribute.Date};
            listall.Add(listarr);
            string value = _Orders.is_null(attribute.Suppliers, attribute.Warehouse, attribute.Date, attribute.Part_Name, attribute.Amount);
            if (value != "")
            {
                MessageBox.Show(value);
                return;
            }
            else 
            {
                dataGridView1.Rows.Add(listarr);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string value = _Orders.Insert_sql(listall,strings);
            MessageBox.Show(value);
            dataGridView1.Rows.Clear();
        }
    }
}
