using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.DAL
{
    class LJsql
    {
        private string SQLLJ = "server=127.0.0.1;port =3306;user=root;password=123456;database=session1";
        public MySqlConnection mySqlConnection;

        private void connectSQL()
        {
            try
            {
                mySqlConnection = new MySqlConnection(SQLLJ);
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
            }
            finally
            {
                Console.WriteLine("数据库连接成功");
            }
        }

        public string SelectData(string sql)
        {
            string selesql = null;
            connectSQL();
            try
            {
                mySqlConnection.Open();
                MySqlCommand cmd = new MySqlCommand(sql,mySqlConnection);
                if (cmd.ExecuteScalar() != null) 
                {
                    selesql = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception Exception)
            {   
                Console.WriteLine(Exception);
            }finally 
            { 
                mySqlConnection.Close(); 
            }
            return selesql;
        }

        public DataSet DataGet(string sql) 
        {
            connectSQL();
            //mySqlConnection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, mySqlConnection);//新建MySqlDataAdapter
            DataSet ds = new DataSet();//新建DataSet;
            sda.Fill(ds);//Fill();在DataSet中添加或刷新行
            if (mySqlConnection != null)//判断连接状态
            {
                mySqlConnection.Close();
            }
            return ds;//返回已经被填写好的DataSet
        }

        public int ChangeData(string sql)
        {
            int iftrue = 0;
            connectSQL();
            if (mySqlConnection.State != ConnectionState.Open)
            {
                mySqlConnection.Open();
            }
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            iftrue = mySqlCommand.ExecuteNonQuery();
            if (mySqlConnection.State != ConnectionState.Closed)
            {
                mySqlConnection.Close();
            }
            return iftrue;
        }
    }
}
