using demo.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.BLL
{
    class Management
    {
        private LJsql LJsql = new LJsql();
        private List<string[]> part = new List<string[]>();
        private List<string> Part_ID = new List<string>();
        private string N_Warehouse;
        private string N_De_Warehouse;
        private string N_PartName;
        private string N_BatchNumber;
        private string N_Amount;
        private string N_Date;

        private void Data(string Source_Warehouse,string Destination_warehouse, string Date, string Part_Name, string Batch_Number, string Amount) 
        {
            N_Warehouse = Source_Warehouse;
            N_De_Warehouse = Destination_warehouse;
            N_PartName = Part_Name;
            N_BatchNumber = Batch_Number;
            N_Amount = Amount;
            N_Date = Date;
        }

        public int Move(string Source_Warehouse, string Destination_warehouse, string Date)
        {
            Data(Source_Warehouse, Destination_warehouse, Date,null,null,null);
            return Add_PartAmount();
        }

        private int Add_PartAmount() 
        {
            string insert_orders = $"INSERT INTO orders" +
                $"(TransactionTypeID,SupplierID,SourceWarehouseID,DestinationWarehouseID,Date) " +
                $"VALUES(2," +
                $"null," +
                $"(select ID from warehouses where Name = \"{N_Warehouse}\")," +
                $"(select ID from warehouses where Name = \"{N_De_Warehouse}\")," +
                $"\"{N_Date}\")";
            int value = LJsql.ChangeData(insert_orders);
            foreach (var item in part) 
            {
                Get_ID(item);
                Dele_PartAmount(item);
                return Change(item,value);
            }
            return 0;
        }

        private int Change(string[] strings,int value)
        {
            int _value = 0;
            string insert_orderitems = $"INSERT INTO orderitems(OrderID,PartID,BatchNumber,Amount) " +
                $"VALUES(" +
                $"(select ID from orders order BY ID desc limit 0,1)," +
                $"(select ID from parts where Name = \"{strings[1]}\")," +
                $"\"{strings[2]}\"," +
                $"{strings[3]})";
            _value = LJsql.ChangeData(insert_orderitems);
            if (value == 1 || _value == 1)
            {
                part.Clear();
                Part_ID.Clear();
                return 1;
            }
            return 0;
        }

        private void Dele_PartAmount(string[] strings)
        {
            foreach (var value in Part_ID)
            {
                string Amount = LJsql.SelectData($"select Amount from orderitems where ID = {value}");
                if (Convert.ToDouble(Amount) >= Convert.ToDouble(strings[3])) 
                {
                    int value1 = LJsql.ChangeData($"UPDATE orderitems " +
                        $"SET Amount = Amount - {Convert.ToDouble(strings[3])} " +
                        $"WHERE ID = \"{value}\";");
                    break;
                }
                else
                {
                    LJsql.ChangeData($"UPDATE orderitems " +
                        $"SET Amount = Amount - Amount " +
                        $"WHERE ID = \"{value}\";");
                    strings[3] = (Convert.ToDouble(strings[3]) - Convert.ToDouble(Amount)).ToString();
                }
                if (Convert.ToDouble(strings[3]) - Convert.ToDouble(Amount) <= 0) 
                {
                    break;
                }
            }
        }

        public void Get_ID(string[] strings) 
        {
            string ID = LJsql.SelectData($"select ID from warehouses where Name = \"{N_Warehouse}\"");
            DataSet DS = LJsql.DataGet($"SELECT orderitems.ID " +
                $"FROM orders,orderitems,parts " +
                $"WHERE orderitems.PartID = parts.ID " +
                $"AND orders.ID = orderitems.OrderID " +
                $"AND DestinationWarehouseID = {ID} " +
                $"AND parts.`Name` = \"{strings[1]}\" " +
                $"AND BatchNumber = \"{strings[2]}\"");
            DataTable dataTable = DS.Tables[0];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Part_ID.Add(Data_ID(dataTable.Rows[i]));
            }
        }

        private string Data_ID(DataRow DR) 
        {
            if (DR != null) 
            {
                return DR["ID"].ToString();
            }
            return null;
        }

        public void Dele(string Source_Warehouse, string Part_Name, string Batch_Number,string Amount) 
        {
            Data(Source_Warehouse,null,null,Part_Name,Batch_Number,Amount);
            string[] strings = Get_List();
            int index = part.IndexOf(strings);
            strings[4] = Convert.ToString(Convert.ToInt32(strings[4]) + Convert.ToInt32(Amount));
            part.Insert(index,strings);
            part.RemoveAt(index + 1);
        }

        private string[] Get_List() 
        {
            foreach (string[] s in part)
            {
                if (s[0] == N_Warehouse && s[1] == N_PartName && s[2] == N_BatchNumber) 
                {
                    return s;
                }
            }
            return null;
        }

        public bool Add(string Source_Warehouse,string Part_Name,string Batch_Number,string Amount) 
        {
            Data(Source_Warehouse,null,null, Part_Name, Batch_Number, Amount);
            bool value = Action();
            string[] strings = Get_List();
            if (value == true) 
            {
                string ID = LJsql.SelectData($"select ID from warehouses where Name = \"{N_Warehouse}\"");
                strings[4] = LJsql.SelectData($"SELECT SUM(Amount) " +
                    $"FROM orders,orderitems,parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND DestinationWarehouseID = {ID} " +
                    $"AND parts.`Name` = \"{N_PartName}\" " +
                    $"AND BatchNumber = \"{N_BatchNumber}\"");
            }
            double amount = Convert.ToDouble(strings[4]);
            int m_Amount = Convert.ToInt32(N_Amount);
            if (amount >= m_Amount) 
            {
                amount -= m_Amount;
                strings[3] = (Convert.ToInt32(strings[3]) + m_Amount).ToString();
                strings[4] = amount.ToString();
                return true;
            }
            return false;
        }

        private bool Action() 
        {
            bool value = Null_List();
            if (value == true)
            {
                return true;
            }
            var trues = Get_List();
            if (trues == null) 
            {
                Add_List();
                return true;
            }
            return false;
        }

        private void Add_List()
        {
            string[] strings = { N_Warehouse, N_PartName, N_BatchNumber, "0", "" };
            part.Add(strings);
        }

        private bool Null_List()
        {
            if (part.Count == 0) 
            {
                string[] strings = {N_Warehouse,N_PartName,N_BatchNumber,"0",""};
                part.Add(strings);
                return true;
            }
            return false;
        }
    }
}
