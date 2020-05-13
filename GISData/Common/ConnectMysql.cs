using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GISData.Common
{
    class ConnectMysql
    {
        private void connect() 
        {
            String connetStr = "server=127.0.0.1;port=3306;user=root;password=root; database=minecraftdb;";
            // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {    
                  conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                  Console.WriteLine("已经建立连接");
                  //在这里使用代码对数据库进行增删查改
            }
            catch (MySqlException ex)
            {
                  Console.WriteLine(ex.Message);
            }
            finally
            {
                  conn.Close();
            }
        }
    }
}
