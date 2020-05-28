using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;

namespace GISData.Common
{
    class ConnectDB
    {
        MySqlConnection conn = null;
        //连接数据库
        public ConnectDB() 
        {
            CommonClass common = new CommonClass();
            String connetStr = common.GetConfigValue("Host");
            conn = new MySqlConnection(connetStr);
        }

        public DataTable GetDataBySql(string sql) 
        {
            try 
            { 
                Console.WriteLine(sql);
                // This will hold the records. 
                this.conn.Open();
                MySqlCommand mycom = conn.CreateCommand();
                mycom.CommandText = sql;
                MySqlDataAdapter adap = new MySqlDataAdapter(mycom);

                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, this.conn);
                this.conn.Close();
                DataSet myDataSet = new DataSet();        // 创建DataSet
                adap.Fill(myDataSet);
                return myDataSet.Tables[0];
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public DataSet GetDataSetBySql(string sql) 
        {
            Console.WriteLine(sql);
            this.conn.Open();
            MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, this.conn);
            DataSet myDataSet = new DataSet();        // 创建DataSet
            myDataAdapter.Fill(myDataSet);
            this.conn.Close();
            return myDataSet;
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT TOP 1 * FROM [" + tableName + "]";
                cmd.Connection = conn;
                MySqlDataReader dr = cmd.ExecuteReader();
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
            Console.WriteLine(sql);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
            conn.Close();
            return iRet > 0;
        }
        //插入
        public int InsertReturnId(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int iRet = cmd.ExecuteNonQuery();
            int ii = 0;
            if (iRet > 0) 
            {
                cmd = new MySqlCommand("select @@identity as id", conn);
                ii = Convert.ToInt32(cmd.ExecuteScalar());
            }
            conn.Close();
            return ii;
        }
        
        //更新
        public bool Update(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
            conn.Close();
            return iRet > 0;
        }
        //删除
        public bool Delete(string sql)
        {
            Console.WriteLine("执行sql:" + sql);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
            conn.Close();
            return iRet > 0;
        }
    }
}
