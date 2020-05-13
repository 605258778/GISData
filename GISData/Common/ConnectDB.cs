using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISData.Common
{
    class ConnectDB
    {
        private static string host = null;
        private static string password = null;
        OleDbConnection conn = null;
        //获取数据
        public ConnectDB() 
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + host + "\\GISData.mdb;Jet OLEDB:Database Password=" + password); //Jet OLEDB:Database Password=
        }

        public ConnectDB(string connecttype)
        {
            CommonClass common = new CommonClass();
            if (connecttype == "远程")
            {
                host = common.GetConfigValue("Host");
                password = common.GetConfigValue("Password");
            }
            else
            {
                host = Application.StartupPath;
                password = common.GetConfigValue("Password");
            }
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + host + "\\GISData.mdb;Jet OLEDB:Database Password="+password); //Jet OLEDB:Database Password=
        }

        public DataTable GetDataBySql(string sql) 
        {
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (dr.HasRows)
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dt.Columns.Add(dr.GetName(i));
                }
                dt.Rows.Clear();
            }
            while (dr.Read())
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    row[i] = dr[i];
                }
                dt.Rows.Add(row);
            }
            cmd.Dispose();
            conn.Close();
            return dt;
        }
        public DataSet GetDataSetBySql(string sql) 
        {
            Console.WriteLine("执行sql:" + sql);
            OleDbCommand cmd = new OleDbCommand(sql, conn);//执行数据连接  
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 获取指定表名的列名与描述注释
        /// </summary>
        /// <param name="mdbFilePath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetTableFieldsDisFromMdb(string tableName)
        {
            conn.Open();
            List<string> list = new List<string>();
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.CommandText = "SELECT TOP 1 * FROM [" + tableName + "]";
                cmd.Connection = conn;
                OleDbDataReader dr = cmd.ExecuteReader();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    list.Add(dr.GetName(i));
                }
            }
            conn.Close();
            return list;
        }
        //插入
        public bool Insert(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
        //插入
        public int InsertReturnId(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            int ii = 0;
            if (i > 0) 
            {
                oleDbCommand = new OleDbCommand("select @@identity as id", conn);
                ii = Convert.ToInt32(oleDbCommand.ExecuteScalar()); //返回被修改的数目
            }
            conn.Close();
            return ii;
        }
        
        //更新
        public bool Update(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
        //删除
        public bool Delete(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
    }
}
