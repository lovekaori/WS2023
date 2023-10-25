using demo.DAL;
using demo.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo
{
    public partial class Manag : Form
    {
        Managing Managing = new Managing();
        private List<string[]> strlist = new List<string[]>();

        public Manag()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Purchase_Orders purchase_Orders = new Purchase_Orders();
            this.Hide();
            purchase_Orders.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Warehouse_Management warehouse_Management = new Warehouse_Management();
            this.Hide();
            warehouse_Management.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Inventory_Report inventory_Report = new Inventory_Report();
            this.Hide();
            inventory_Report.ShowDialog();
        }

        private void Manag_Load(object sender, EventArgs e)
        {
            strlist = Managing.All_Activity();
            foreach (string[] value in strlist)
            {
                dataGridView1.Rows.Add(value);
            }
            for (int i = 0; i < 24; i++)
            {
                test(i);
            }
        }

        private void test(int rowindex)
        {
            int index = this.dataGridView1.Columns["Column7"].Index;
            Label labels1 = GetBtnByType("lable1","Edit", rowindex);
            Label labels2 = GetBtnByType("lable2", "Remove", rowindex);
            this.dataGridView1.Controls.Add(labels1);
            this.dataGridView1.Controls.Add(labels2);
            Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(index,rowindex,true);
            labels1.Size = labels2.Size = new Size(rect.Width / 2, rect.Height);
            labels1.Location = new Point(rect.Left, rect.Top + 5);
            labels2.Location = new Point(rect.Left + labels1.Width, rect.Top + 5);
            labels1.Click += new EventHandler(CustomLab1_Click);
            labels2.Click += new EventHandler(CustomLab2_Click);
        }

        private void CustomLab1_Click(object sender, EventArgs e)
        {
            Change change = new Change();
            Label btn = (Label)sender;
            int index = Convert.ToInt32(btn.Tag);
            change.test(get_String(index));
            this.Hide();
            change.ShowDialog();
        }

        private string[] get_String(int index) 
        {
            string[] strings = new string[dataGridView1.Columns.Count];
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Rows[index].Cells[i ].Value == null)
                {
                    strings[i] = "";
                }
                else
                {
                    strings[i] = dataGridView1.Rows[index].Cells[i].Value.ToString();
                }
            }
            return strings;
        }

        private void CustomLab2_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("是否删除", "", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Label btn = (Label)sender;
                int index = Convert.ToInt32(btn.Tag);
                string Part_Name = dataGridView1.Rows[index].Cells[0].Value.ToString();
                string Transaction_Types = dataGridView1.Rows[index].Cells[1].Value.ToString();
                string Date = dataGridView1.Rows[index].Cells[2].Value.ToString();
                string Amount = dataGridView1.Rows[index].Cells[3].Value.ToString();
                string Destination = dataGridView1.Rows[index].Cells[5].Value.ToString();
                int value = Managing.Dele(Part_Name, Transaction_Types, Date, Amount, Destination);
                if (value == 1) 
                {
                    dataGridView1.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show("删除失败");
                }
            }
            else
            {
                return;
            }
        }

        private Label GetBtnByType(string strBtnName, string strBtnText, int rowIndex)
        {
            Label lab = new Label();
            lab.Name = strBtnName;
            lab.Text = strBtnText;
            lab.ForeColor = strBtnName == "BtnDel" ? Color.Red : Color.FromArgb(64, 158, 255);
            lab.BackColor = Color.Transparent;
            lab.Cursor = Cursors.Hand;
            lab.Tag = rowIndex.ToString();
            lab.AutoSize = true;
            return lab;
        }

    }
}
