using demo.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace demo.BLL
{
    class Inventory
    {
        LJsql LJsql = new LJsql();
        private int index;
        private List<string> Part_Name = new List<string>();
        private List<string> Stock = new List<string>();
        private List<string> strings1 = new List<string>();
        private List<string> bools = new List<string>();
        private List<string> test = new List<string>(); 

        public List<string> Get_BatchNumber(string Part) 
        {
            DataSet dataSet = LJsql.DataGet($"SELECT BatchNumber FROM orders, orderitems, parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND parts.`Name` = \"{Part}\"" +
                    $"GROUP BY BatchNumber;");
            DataTable dt = dataSet.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                test.Add(Data_ID(dt.Rows[i], "BatchNumber") + "\n ");
            }
            return test;
        }

        public List<string> N_Action(List<string> Name) 
        {
            foreach (var item in Name)
            {
                string Need = LJsql.SelectData($"SELECT BatchNumberHasRequired FROM parts WHERE parts.`Name` = \"{item}\";");
                bools.Add(Need);
            }
            return bools;
        }

        public List<string> Stock_Amount(string Name,List<string> strings) 
        {
            switch (Name)
            {
                case "Current Stock":
                    Current_Stock();
                    return Stock;
                case "Received Stock":
                    Received_Stock();
                    return Stock;
                case "Out of Stock":
                    return Out_of_Stock(strings);
                default:
                    return Stock;
            }
        }

        private List<string> Out_of_Stock(List<string> strings)
        {
            for (int i = 0; i < strings.Count; i++)
            {
                string Amount = LJsql.SelectData($"SELECT SUM(Amount) " +
                    $"FROM orders,orderitems,parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND DestinationWarehouseID = {index} " +
                    $"AND parts.`Name` = \"{strings[i]}\";");
                if (Amount == "0")
                {
                    strings1.Add(Part_Name[i]);
                }
            }
            return strings1;
        }

        private void Received_Stock() 
        {
            for (int i = 0; i < Part_Name.Count; i++)
            {
                string Amount = LJsql.SelectData($"SELECT SUM(Amount) " +
                    $"FROM orders,orderitems,parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND DestinationWarehouseID = {index} " +
                    $"AND parts.`Name` = \"{Part_Name[i]}\";");
                string Move_Amount = LJsql.SelectData($"SELECT SUM(Amount) " +
                    $"FROM orders,orderitems,parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND DestinationWarehouseID != {index} " +
                    $"and parts.`Name` = \"{Part_Name[i]}\" " +
                    $"and TransactionTypeID = 2;");
                if (Move_Amount == "") 
                    Stock.Add((Convert.ToDouble(Amount) + 0).ToString());
                else
                    Stock.Add((Convert.ToDouble(Amount) + Convert.ToDouble(Move_Amount)).ToString());
            }
        }

        private void Current_Stock() 
        {
            for (int i = 0; i < Part_Name.Count; i++)
            {
                string Amount = LJsql.SelectData($"SELECT SUM(Amount) " +
                    $"FROM orders,orderitems,parts " +
                    $"WHERE orderitems.PartID = parts.ID " +
                    $"AND orders.ID = orderitems.OrderID " +
                    $"AND DestinationWarehouseID = {index} " +
                    $"AND parts.`Name` = \"{Part_Name[i]}\";");
                Stock.Add(Amount);
            }
        }

        public List<string> Part_test(string Name,string Stock_Name)
        {
            index = Convert.ToInt32(LJsql.SelectData($"select ID from warehouses where Name = \"{Name}\""));
            DataSet dataSet = LJsql.DataGet($"SELECT parts.`Name` " +
                $"FROM orders,orderitems,parts " +
                $"WHERE orderitems.PartID = parts.ID " +
                $"AND orders.ID = orderitems.OrderID " +
                $"AND DestinationWarehouseID = {index} " +
                $"GROUP BY parts.`Name`;");
            DataTable dataTable = dataSet.Tables[0];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Part_Name.Add(Data_ID(dataTable.Rows[i],"Name"));
            }
            return Part_Name;
        }

        private string Data_ID(DataRow DR,string Name)
        {
            if (DR != null)
            {
                return DR[Name].ToString();
            }
            return null;
        }

        public void zero() 
        {
            Part_Name.Clear();
            Stock.Clear();
            strings1.Clear();
            bools.Clear();
            test.Clear();
        }
    }
}
