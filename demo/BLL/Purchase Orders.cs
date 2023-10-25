using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo.DAL;


namespace demo.BLL
{
    class purchase_Orders
    {
        LJsql LJsql = new LJsql();

        //public string Add(string Suppliers, string Warehouse, string Date, string Part_Name,string Batch_Number,int Amount) 
        //{
        //    string sele_sql = $"select ID from parts where Name = {Part_Name};";
        //    string PartID = LJsql.SelectData(sele_sql);
        //    string Change_sql = $"INSERT INTO orderitems (OrderID,PartID,BatchNumber,Amount) VALUES(1,{PartID},{Batch_Number},{Amount});";
        //    string change = LJsql.DataGet();

        //}

        public bool IsBatchNum(string PartName) 
        {
            string sele_sql = $"select BatchNumberHasRequired from parts where Name = {PartName}";
            int bool_num = Convert.ToInt32(LJsql.SelectData(sele_sql));
            if (bool_num == 0)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        public string is_null(string Suppliers, string Warehouse, string Date, string Part_Name, string Amount) 
        {
            string value = "";
            if (Suppliers == "")
                value = "供应商不能为空";
            if (Warehouse == "")
                value = "仓库不能为空";
            if (Date == "")
                value = "日期不能为空";
            if (Part_Name == "")
                value = "零部件不能为空";
            if (Amount == "" || Convert.ToInt32(Amount) <= 0)
                value = "数量过少";
            return value;
        }

        public string Insert_sql(List<object> list, string[] strings) 
        {
            string insert_orders = $"INSERT INTO orders" +
                $"(TransactionTypeID,SupplierID,SourceWarehouseID,DestinationWarehouseID,Date) " +
                $"VALUES(1," +
                $"(SELECT ID FROM suppliers WHERE `Name` = \"{strings[0]}\")," +
                $"null," +
                $"(select ID from warehouses where Name = \"{strings[1]}\")," +
                $"\"{strings[2]}\")";
            int value = LJsql.ChangeData(insert_orders);
            int _value = 0;
            foreach (string[] item in list) 
            {
                string insert_orderitems = $"INSERT INTO orderitems" +
                    $"(OrderID,PartID,BatchNumber,Amount) " +
                    $"VALUES((select ID from orders order BY ID desc limit 0,1)," +
                    $"(select ID from parts where Name = \"{item[0]}\")," +
                    $"\"{item[1]}\"," +
                    $"\"{item[2]}\")";
                _value = LJsql.ChangeData(insert_orderitems);
            }
            if (value == 1 || _value == 1) 
            {
                return "添加成功";
            }
            return "添加失败";
        }
    }
}
