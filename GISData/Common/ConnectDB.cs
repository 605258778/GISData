using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISData.Common
{
    class ConnectDB
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + Application.StartupPath + "\\GISData.mdb"); //Jet OLEDB:Database Password=
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
        public bool Insert(string sql)
        {
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
        public bool Update(string sql)
        {
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
        public bool Delete(string sql)
        {
            conn.Open();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的数目
            conn.Close();
            return i > 0;
        }
    }
}
