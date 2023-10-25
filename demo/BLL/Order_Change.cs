using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.DAL
{
    class Order_Change
    {
        LJsql LJsql = new LJsql();

        public int Change(string[] strings,string Name,string Amount) 
        {
            string Part_ID = LJsql.SelectData($"select ID from parts where Name = \"{Name}\"");
            string ID = LJsql.SelectData($"select ID from warehouses where Name = \"{strings[5]}\"");
            string T_ID = LJsql.SelectData($"select ID from transactionTypes where Name = \"{strings[1]}\"");
            string orderitems_ID = LJsql.SelectData($"SELECT orderitems.ID " +
                $"FROM orders,orderitems,parts " +
                $"WHERE orderitems.PartID = parts.ID " +
                $"AND orders.ID = orderitems.OrderID " +
                $"AND parts.`Name` = \"{strings[0]}\" " +
                $"AND Amount = {strings[3]} " +
                $"AND TransactionTypeID = {T_ID} " +
                $"AND Date = \"{strings[2]}\" " +
                $"AND DestinationWarehouseID = {ID} " +
                $"limit 0,1;");
            return LJsql.ChangeData($"UPDATE orderitems SET Amount = {Amount} , PartID = {Part_ID} WHERE ID = {orderitems_ID};");
        }
    }
}
