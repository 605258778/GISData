using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GISData.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckConfig.CheckAttr.CheckDialog
{
    public partial class FormXmmc : Form
    {
        private System.Windows.Forms.ComboBox comboBoxDataSour;
        private string type;
        private string selectedId;
        private DataTable FieldDT;
        public FormXmmc()
        {
            InitializeComponent();
        }
        public FormXmmc(System.Windows.Forms.ComboBox comboBoxDataSour, string type1, string selectedId1)
        {
            InitializeComponent();
            this.comboBoxDataSour = comboBoxDataSour;
            this.type = type1;
            this.selectedId = selectedId1;
            FieldDT = new DataTable();
            FieldDT.Columns.Add("FIELD");
            FieldDT.Columns.Add("RELATION");
            FieldDT.Columns.Add("TASKFIELD");
            
        }

        public DataTable FieldDTValue
        {
            get { return this.FieldDT; }
            set { this.FieldDT = value; }
        }

        private void FormXmmc_Load(object sender, EventArgs e)
        {
            loadField();
            ConnectDB db = new ConnectDB();
            List<string> dt = db.GetTableFieldsDisFromMdb("GISDATA_TASK");
            GetComboBox(dt, "TASKFIELD");
            DataTable dtrow = new DataTable();
            this.gridControl1.DataSource = dtrow;
            gridView1.Columns["RELATION"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
            if (this.type == "edit")
            {
                DataTable editDB = db.GetDataBySql("select FIELD from GISDATA_TBATTR where id = " + selectedId);
                DataRow[] drs = editDB.Select(null);
                string field = drs[0]["FIELD"].ToString();
                string[] arrStr = field.Split('#');
                string fieldStr = arrStr[0];
                string taskfieldStr = arrStr[1];
                string[] arrFieldStr = fieldStr.Split('&');
                string[] arrTaskFieldStr = taskfieldStr.Split('&');
                for (int i = 0; i < arrFieldStr.Length;i++ )
                {
                    DataRow newRow = FieldDT.NewRow();
                    FieldDT.Rows.Add(newRow);
                    newRow["RELATION"] = "<---->";
                    newRow["FIELD"] = arrFieldStr[i];
                    newRow["TASKFIELD"] = arrTaskFieldStr[i];
                }
                this.gridControl1.DataSource = FieldDT;
            }
        }

        private void loadField() 
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            GetComboBox(dt,"FIELD");
        }

        //设置combobox下拉框
        private void GetComboBox(DataTable dt,string field)
        {
            IList<CboItemEntity> ic = ToList(dt); 
            RepositoryItemComboBox combobox = new RepositoryItemComboBox();
            combobox.Items.AddRange(ic as ICollection);
            combobox.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.RepositoryItems.Add(combobox);
            this.gridView1.Columns[field].ColumnEdit = combobox;
            combobox.SelectedIndexChanged += new EventHandler(ComboBoxEdit_SelectedIndexChanged1);
            combobox.ParseEditValue += new ConvertEditValueEventHandler(repositoryItemComboBox1_ParseEditValue);
            combobox.ValidateOnEnterKey = false;
        }
        //设置combobox下拉框
        private void GetComboBox(List<string> list, string field)
        {
            RepositoryItemComboBox combobox = new RepositoryItemComboBox();
            combobox.Items.AddRange(list as ICollection);
            combobox.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            gridControl1.RepositoryItems.Add(combobox);
            this.gridView1.Columns[field].ColumnEdit = combobox;
            combobox.SelectedIndexChanged += new EventHandler(ComboBoxEdit_SelectedIndexChanged);
            combobox.ParseEditValue += new ConvertEditValueEventHandler(repositoryItemComboBox1_ParseEditValue);
            combobox.ValidateOnEnterKey = false;
        }

        private void repositoryItemComboBox1_ParseEditValue(object sender, ConvertEditValueEventArgs e)
        {
            if (e.Value != null) 
            {
                e.Value = e.Value.ToString();
                e.Handled = true;
            }
        } 
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow newRow = FieldDT.NewRow();
            FieldDT.Rows.Add(newRow);
            newRow["RELATION"] = "<---->";
            this.gridControl1.DataSource = this.FieldDT;
        }

        private IList<CboItemEntity> ToList(DataTable dt)
        {
            IList<CboItemEntity> list = new List<CboItemEntity>();
            DataRow[] drs = dt.Select(null);
            foreach (DataRow dr in drs)
            {
                CboItemEntity item = new CboItemEntity();
                item.Text = dr[0].ToString();
                item.Value = dr[0].ToString();
                list.Add(item);
            }
            return list;
        }

        private void ComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //1.获取下拉框选中值
                string value = (sender as ComboBoxEdit).SelectedItem.ToString();
                
                //2.获取gridview选中的行
                GridView myView = (gridControl1.MainView as GridView);
                int index = myView.GetDataSourceRowIndex(myView.FocusedRowHandle);
                //3.保存选中值到datatable
                FieldDT.Rows[index]["TASKFIELD"] = value;
                this.gridControl1.DataSource = FieldDT;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示");
            }
        }
        private void ComboBoxEdit_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CboItemEntity item = new CboItemEntity();
            try
            {
                //1.获取下拉框选中值
                item = (CboItemEntity)(sender as ComboBoxEdit).SelectedItem;
                string text = item.Text.ToString();
                string value = item.Value.ToString();

                //2.获取gridview选中的行
                GridView myView = (gridControl1.MainView as GridView);
                int index = myView.GetDataSourceRowIndex(myView.FocusedRowHandle);
                //3.保存选中值到datatable
                FieldDT.Rows[index]["FIELD"] = value;
                this.gridControl1.DataSource = FieldDT;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            foreach (int itemRow in selectRows)
            {
                this.gridView1.DeleteRow(itemRow);
            }
        }
    }
    public class CboItemEntity
    {
        private object _text = 0;
        private object _Value = "";
        /// <summary>
        /// 显示值
        /// </summary>
        public object Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        /// <summary>
        /// 对象值
        /// </summary>
        public object Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }

        public override string ToString()
        {
            return this.Text.ToString();
        }
    }
}
