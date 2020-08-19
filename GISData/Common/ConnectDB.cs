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
            try 
            {
                CommonClass common = new CommonClass();
                String connetStr = common.GetConfigValue("Host");
                conn = new MySqlConnection(connetStr);
             }
            catch(Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
            }
            
        }

        public DataTable GetDataBySql(string sql) 
        {
            try 
            {
                LogHelper.WriteLog(typeof(ConnectDB), sql);
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
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return null;
            }
        }


        public DataSet GetDataSetBySql(string sql) 
        {
            try 
            {
                Console.WriteLine(sql);
                LogHelper.WriteLog(typeof(ConnectDB), sql);
                this.conn.Open();
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, this.conn);
                DataSet myDataSet = new DataSet();        // 创建DataSet
                myDataAdapter.Fill(myDataSet);
                this.conn.Close();
                return myDataSet;
            }
            catch(Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                
                return null;
            }
            
        }


        /// <summary>
        /// 获取指定表名的列名与描述注释
        /// </summary>
        /// <param name="mdbFilePath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<string> GetTableFieldsDisFromMdb(string tableName)
        {
            try
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
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return null;
            }
        }
        //插入
        public bool Insert(string sql)
        {
            try
            {
                LogHelper.WriteLog(typeof(ConnectDB), sql);
                Console.WriteLine(sql);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                conn.Close();
                return iRet > 0;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return false;
            }
        }
        //插入
        public int InsertReturnId(string sql)
        {
            try
            {
                LogHelper.WriteLog(typeof(ConnectDB), sql);
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
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return -1;
            }
        }
        
        //更新
        public bool Update(string sql)
        {
            try
            {
                LogHelper.WriteLog(typeof(ConnectDB), sql);
                Console.WriteLine("执行sql:" + sql);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                conn.Close();
                return iRet > 0;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return false;
            }
        }
        //删除
        public bool Delete(string sql)
        {
            try
            {
                LogHelper.WriteLog(typeof(ConnectDB), sql);
                Console.WriteLine("执行sql:" + sql);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int iRet = cmd.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                conn.Close();
                return iRet > 0;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(ConnectDB), e);
                return false;
            }
        }

        /// <summary>
        /// 存储进数据库
        /// </summary>
        /// <param name="dt">需要存储的表</param>
        /// <param name="DBTableName">数据库表的名字</param>
        /// <returns></returns>
        public int Save2MySqlDB(DataTable dt, string DBTableName)
        {
            if (dt.Rows.Count < 1)
            {
                return dt.Rows.Count;
            }
            string sb = this.GetCommdString(dt, DBTableName);
            int res = -1;
            string result = "";
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sb, conn))
            {
                try
                {
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    res = -1;
                    // Unknown column 'names' in 'field list' 
                    result = "操作失败！" + ex.Message.Replace("Unknown column", "未知列").Replace("in 'field list'", "存在字段集合中！");
                }
            }
            conn.Close();
            return res;
        }



        /// <summary>
        /// 得到存储语句（比较高效）
        /// </summary>
        /// <param name="dt">需要存储的表</param>
        /// <param name="DBTableName">数据库表的名字</param>
        /// <returns></returns>
        public string GetCommdString(DataTable dt, string DBTableName)
        {
            List<string> mySqlList = new List<string>();
            string sb = "INSERT INTO " + DBTableName + "(";
            mySqlList.Add(sb);
            sb = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb = sb + dt.Columns[i].ColumnName + ",";
            }
            sb = sb.Remove(sb.LastIndexOf(','), 1);
            sb = sb + ") VALUES ";
            mySqlList.Add(sb);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb = "(";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb = sb + "'" + dt.Rows[i][j] + "',";
                }
                sb = sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb = sb + "),";
                if (i < dt.Rows.Count - 1)
                {
                    mySqlList.Add(sb);
                }
                else
                {
                    sb = sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb = sb + ";";
                    mySqlList.Add(sb);
                }
            }
            string str = string.Join("", mySqlList.ToArray());
            return str;
        }
    }
}
