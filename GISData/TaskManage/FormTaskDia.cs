using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.TaskManage
{
    public partial class FormTaskDia : Form
    {
        public FormTaskDia()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Files|*.xls;*.xlsx";              // 设定打开的文件类型
            //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;                       // 打开app对应的路径
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  // 打开桌面

            // 如果选定了文件
            string filePath = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 取得文件路径及文件名
                filePath = openFileDialog.FileName;
                DataTable excelDataTable = ReadExcelToTable(filePath);      // 读出excel并放入datatable
                DataRow[] dr = excelDataTable.Select(null);

       
                ConnectDB db = new ConnectDB();
                db.Delete("delete from GISDATA_TASK");
                for (int i = 1; i < dr.Length; i++)
                {
                    string YZLGLDW = dr[i]["YZLGLDW"].ToString();
                    string ZCSBND = dr[i]["ZCSBND"].ToString();
                    string YZLFS = dr[i]["YZLFS"].ToString();
                    string XMMC = dr[i]["XMMC"].ToString();
                    string RWMJ = dr[i]["RWMJ"].ToString();
                    db.Insert("INSERT INTO GISDATA_TASK (YZLGLDW,ZCSBND,YZLFS,RWMJ,XMMC) values ('" + YZLGLDW + "','" + ZCSBND + "','" + YZLFS + "','" + RWMJ + "','" + XMMC + "')");
                }
                gridControl1.DataSource = excelDataTable;        // 测试用, 输出到dataGridView
                MessageBox.Show("导入成功");
            }
        }
        private static DataTable ReadExcelToTable(string path)
        {
            try
            {
                // 连接字符串(Office 07及以上版本 不能出现多余的空格 而且分号注意)
                string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
                // 连接字符串(Office 07以下版本, 基本上上面的连接字符串就可以了) 
                //string connstring = Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    // 取得所有sheet的名字
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    // 取得第一个sheet的名字
                    string firstSheetName = sheetsName.Rows[0][2].ToString();

                    // 查询字符串 
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);

                    // OleDbDataAdapter是充当 DataSet 和数据源之间的桥梁，用于检索和保存数据
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);

                    // DataSet是不依赖于数据库的独立数据集合
                    DataSet set = new DataSet();

                    // 使用 Fill 将数据从数据源加载到 DataSet 中
                    ada.Fill(set);
                    return set.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void FormTaskDia_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_TASK");
            gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
        }
    }
}
