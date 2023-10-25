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

namespace demo.UI
{
    public partial class Change : Form
    {
        private LJsql LJsql = new LJsql();
        private Order_Change order_change = new Order_Change();
        public string[] strings = new string[6];

        public Change()
        {
            InitializeComponent();
            Part_test();
        }

        public void test(string[] strings) 
        {
            this.strings = strings;
            comboBox1.Text = strings[0];
            textBox1.Text = strings[3];
        }

        private void Part_test()
        {
            DataSet dataSet = LJsql.DataGet($"select Name from parts");
            comboBox1.DataSource = dataSet.Tables[0];
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("是否更改", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Show_Change();
            }
            else 
            {
                return;
            }
        }

        private void Show_Change() 
        {
            int value = order_change.Change(strings, comboBox1.Text, textBox1.Text);
            if (value == 1)
            {
                MessageBox.Show("已修改");
                Manag manag = new Manag();
                this.Hide();
                manag.ShowDialog();
            }
            else
            {
                MessageBox.Show("数据有误，修改失败");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (strings[0] != comboBox1.Text || strings[3] != textBox1.Text) 
            {
                DialogResult dialog = MessageBox.Show("是否放弃修改", "", MessageBoxButtons.YesNo);
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
        }
    }
}
