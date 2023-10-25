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
using demo.Utils;
using demo.BLL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace demo.UI
{
    public partial class Inventory_Report : Form
    {
        LJsql LJsql = new LJsql();
        attribute attribute = new attribute();
        Inventory inventory = new Inventory();
        private List<string> strings = new List<string>() {"Part Name",null,"Aciton"};
        private List<string> Part_Name = new List<string>();
        private List<string> Stock = new List<string>();
        private List<string> HasRequired = new List<string>();

        public Inventory_Report()
        {
            InitializeComponent();
            Source_Warehouse_test();
            Change(radioButton1);
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

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            inventory.zero();
            test();
        }

        private void test() 
        {
            Part_Name = inventory.Part_test(comboBox1.Text, strings[1]);
            if (strings[1] == "Out of Stock")
            {
                Part_Name = inventory.Stock_Amount( strings[1], Part_Name);
            }
            else
            {
                Stock = inventory.Stock_Amount(strings[1], Part_Name);
            }
            HasRequired = inventory.N_Action(Part_Name);
            Add_List();
        }

        private void Add_List() 
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Part_Name.Count; i++)
            {
                string[] arr;
                if (Convert.ToBoolean(HasRequired[i]) == true) 
                {
                    arr = new string[] { Part_Name[i], Stock[i],"View Batch Numbers" };
                }
                else
                {
                    arr = new string[] { Part_Name[i], Stock[i], null };
                }
                dataGridView1.Rows.Add(arr);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Change(radioButton1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                Change(radioButton2);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                Change(radioButton3);
        }

        private void Change(RadioButton radioButton) 
        {
            strings[1] = radioButton.Text;
            dataGridView1.Columns[1].HeaderText = strings[1];
            inventory.zero();
            test();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            List<string> a = new List<string>();
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                attribute.Part_Name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                a = inventory.Get_BatchNumber(attribute.Part_Name);
            }
            string str = "";
            foreach (string row in a) 
            {
                str += row;
            }
            MessageBox.Show(str);
        }
    }
}
