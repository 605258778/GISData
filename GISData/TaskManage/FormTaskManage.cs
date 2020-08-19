using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using GISData.Common;
using Spire.Xls;
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
    public partial class FormTaskManage : Form
    {
        public FormTaskManage()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Files|*.xls;*.xlsx";              // 设定打开的文件类型
            //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;                       // 打开app对应的路径
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  // 打开桌面

            // 如果选定了文件
            string filePath = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filePath = openFileDialog.FileName;
                    Workbook book = new Workbook();
                    book.LoadFromFile(filePath);
                    DataTable excelDataTable = book.Worksheets[0].ExportDataTable();

                    ConnectDB db = new ConnectDB();
                    db.Delete("delete from GISDATA_TASK");
                    int result = db.Save2MySqlDB(excelDataTable, "GISDATA_TASK");

                    gridControl1.DataSource = excelDataTable;        // 测试用, 输出到dataGridView

                    if (result > 1) 
                    {
                        MessageBox.Show("导入成功");
                    }
                }catch(Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormTaskManage), ex);
                }
            }
        }

        private void button2_Click11(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Files|*.xls;*.xlsx";              // 设定打开的文件类型
            //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;                       // 打开app对应的路径
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  // 打开桌面

            // 如果选定了文件
            string filePath = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
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
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormTaskManage), ex);
                }
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
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormTaskManage), ex);
                return null;
            }
        }

        private void FormTaskManage_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select * from GISDATA_TASK");
            gridControl1.DataSource = dt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            DataTable dtgldw = db.GetDataBySql("select * from GISDATA_GLDW");
            gridControlGLDW.DataSource = dtgldw;
            this.gridViewGLDW.OptionsBehavior.Editable = false;
            this.gridViewGLDW.OptionsSelection.MultiSelect = true;
        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)//判断是否是列标题的右键菜单
            {
                GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
                //menu.Items.RemoveAt(6);//移除右键菜单中的第7个功能，从0开始
                //menu.Items.Clear();//清除所有功能
                DXMenuItem dxm = new DXMenuItem();
                dxm.Caption = "字段注册";
                dxm.Tag = menu.Column;
                dxm.Click += new EventHandler(DevIncludeM_Click);
                menu.Items.Add(dxm);
            }
        }

        private void DevIncludeM_Click(object sender, EventArgs e)
        {
            DXMenuItem meanuItem = (DXMenuItem)sender;
            TaskFieldRegister register = new TaskFieldRegister(meanuItem.Tag, "GISDATA_TASK");
            register.ShowDialog();
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
                db.Delete("delete from GISDATA_GLDW");
                for (int i = 0; i < dr.Length; i++)
                {
                    string GLDW = dr[i]["GLDW"].ToString();
                    string GLDWNAME = dr[i]["GLDWNAME"].ToString();
                    string CONTACTS = dr[i]["CONTACTS"].ToString();
                    string TEL = dr[i]["TEL"].ToString();
                    string STARTTIME = dr[i]["STARTTIME"].ToString();
                    string STATE = dr[i]["STATE"].ToString();
                    string ENDTIME = dr[i]["ENDTIME"].ToString();
                    string OTHER = dr[i]["OTHER"].ToString();
                    string SPATIALR = dr[i]["SPATIALR"].ToString();
                    db.Insert("INSERT INTO GISDATA_GLDW (GLDW,GLDWNAME,CONTACTS,TEL,STARTTIME,STATE,ENDTIME,OTHER,SPATIALR) values ('" + GLDW + "','" + GLDWNAME + "','" + CONTACTS + "','" + TEL + "','" + STARTTIME + "','" + STATE + "','" + ENDTIME + "','" + OTHER + "','" + SPATIALR + "')");
                }
                gridControlGLDW.DataSource = excelDataTable;        // 测试用, 输出到dataGridView
                MessageBox.Show("导入成功");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Title = "另存为";
                saveFileDialog1.FileName = "newExcelName.xlsx"; //设置默认另存为的名字，可选
                saveFileDialog1.Filter = "Excel 文件(*.xlsx)|*.xlsx|Excel 文件(*.xls)|*.xls|所有文件(*.*)|*.*";
                saveFileDialog1.AddExtension = false;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (this.checkBox1.Checked)
                    {
                        DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                        this.gridViewGLDW.AppearancePrint.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        this.gridViewGLDW.ExportToXlsx(saveFileDialog1.FileName);
                    }
                    else 
                    {
                        try
                        {
                            Workbook book = new Workbook();

                            Worksheet sheet = book.Worksheets[0];
                            DataView dv = this.gridViewGLDW.DataSource as DataView;
                            sheet.InsertDataTable(dv.ToTable(), true, 1, 1);
                            book.SaveToFile(saveFileDialog1.FileName, saveFileDialog1.FileName.EndsWith("xlsx") ? ExcelVersion.Version2013 : ExcelVersion.Version2007);
                            MessageBox.Show("导出成功");
                        }catch(Exception ex)
                        {
                            LogHelper.WriteLog(typeof(FormTaskManage), ex);
                            MessageBox.Show("导出失败");
                        }
                    }
                    
                }
            }
        }

        private void gridControlGLDW_Click(object sender, EventArgs e)
        {

        }
    }
}
