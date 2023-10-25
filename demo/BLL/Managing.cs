using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.DAL
{
    class Managing
    {
        LJsql LJsql = new LJsql();
        private List<string[]> strlist = new List<string[]>();
        private string[] record;

        public int Dele(string Part_Name, string Transaction_Types, string Date,string Amount, string Destination) 
        {
            string ID = LJsql.SelectData($"select ID from warehouses where Name = \"{Destination}\"");
            string T_ID = LJsql.SelectData($"select ID from transactionTypes where Name = \"{Transaction_Types}\"");
            string orderitems_ID = LJsql.SelectData($"SELECT orderitems.ID " +
                $"FROM orders,orderitems,parts " +
                $"WHERE orderitems.PartID = parts.ID " +
                $"AND orders.ID = orderitems.OrderID " +
                $"AND parts.`Name` = \"{Part_Name}\" " +
                $"AND Amount = {Amount} " +
                $"AND TransactionTypeID = {T_ID} " +
                $"AND Date = \"{Date}\" " +
                $"AND DestinationWarehouseID = {ID} " +
                $"limit 0,1;");
            return LJsql.ChangeData($"DELETE FROM orderitems WHERE ID = {orderitems_ID}");
        }

        public List<string[]> All_Activity() 
        {
            string Getlist = $"select parts.`Name` AS Part_Name,transactiontypes.`Name` as Transactiontype_Name,Date,Amount,orders.SourceWarehouseID,orders.DestinationWarehouseID " +
                $"FROM  orders,orderitems,parts,transactiontypes " +
                $"WHERE orderitems.PartID = parts.ID " +
                $"AND orders.ID = orderitems.OrderID " +
                $"AND orders.TransactionTypeID = transactiontypes.ID;";
            DataSet dataSet = LJsql.DataGet(Getlist);
            DataTable dataTable = dataSet.Tables[0];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Data_List(dataTable.Rows[i]);
                strlist.Add(record);
            }
            return strlist;
        }

        public int Length() 
        {
            string Get_Length = $"SELECT COUNT(*) FROM orderitems";
            return Convert.ToInt32(LJsql.SelectData(Get_Length));
        }

        private void Data_List(DataRow row) 
        {
            if (row != null)
            {

                record = new string[7];
                record[0] = row["Part_Name"].ToString();
                record[1] = row["Transactiontype_Name"].ToString();
                record[2] = row["Date"].ToString();
                record[3] = row["Amount"].ToString();
                record[4] = LJsql.SelectData($"select Name from warehouses where ID = {row["SourceWarehouseID"]}");
                record[5] = LJsql.SelectData($"select Name from warehouses where ID = {row["DestinationWarehouseID"]}");
                record[6] = null;
            }
        }
    }
}
