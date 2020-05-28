using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckConfig.CheckReport
{
    public partial class FormPivotConfig : Form
    {
        private Dictionary<string, string> DicPivot = new Dictionary<string, string>();
        private string[] dataSourceArr;
        private DataTable dtSource;
        public FormPivotConfig(Dictionary<string, string> DicPivot, string[] dataSourceArr)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.DicPivot = DicPivot;
            this.dataSourceArr = dataSourceArr;
        }

        private void FormPivotConfig_Load(object sender, EventArgs e)
        {
            setField();
            this.dtSource = new DataTable();
            dtSource.Columns.Add("FIELDNAME");
            dtSource.Columns.Add("TYPE");
        }

        public DataTable dtSourceValue
        {
            get
            {
                
                return this.dtSource;
            }
            set
            {
                this.dtSource = value;
            }
        }

        public Dictionary<string, string> DicPivotValue
        {
            get
            {

                return this.DicPivot;
            }
            set
            {
                this.DicPivot = value;
            }
        }

        private void setField() 
        {
            DataTable fieldTable = new DataTable();
            DataTable fieldTableDS = new DataTable();
            ConnectDB db = new ConnectDB();
            for (int i = 0; i < dataSourceArr.Length; i++) 
            {
                string itemTable = dataSourceArr[i].Trim();
                DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + itemTable + "'");
                if (i == 0)
                {
                    fieldTable = dt;
                    fieldTableDS = dt;
                }
                else 
                {
                    fieldTableDS = this.MergeDataTable(fieldTable, dt, "FIELD_NAME");
                }
            }
            DataRow[] dr = fieldTable.Select(null);
            this.checkedComboBoxEdit1.Properties.DataSource = fieldTableDS;
            this.checkedComboBoxEdit1.Properties.DisplayMember = "FIELD_ALSNAME";
            this.checkedComboBoxEdit1.Properties.ValueMember = "FIELD_NAME";
            this.checkedComboBoxEdit2.Properties.DataSource = fieldTableDS;
            this.checkedComboBoxEdit2.Properties.DisplayMember = "FIELD_ALSNAME";
            this.checkedComboBoxEdit2.Properties.ValueMember = "FIELD_NAME";
            this.comboBox2.DataSource = fieldTableDS;
            this.comboBox2.DisplayMember = "FIELD_ALSNAME";
            this.comboBox2.ValueMember = "FIELD_NAME";
        }

        /// <summary>
        /// 取两个DataTable的交集,删除重复数据
        /// </summary>
        /// <param name="sourceDataTable">源DataTable</param>
        /// <param name="targetDataTable">目标DataTable</param>
        /// <param name="primaryKey">两个表的主键</param>
        /// <returns>合并后的表</returns>
        private DataTable MergeDataTable(DataTable sourceDataTable, DataTable targetDataTable, string primaryKey)
        {
            DataTable returnTable = new DataTable();
            foreach(DataColumn itemColum in sourceDataTable.Columns)
            {
                DataColumn insertColum = new DataColumn(itemColum.ColumnName);
                returnTable.Columns.Add(insertColum);
            }
            if (sourceDataTable != null || targetDataTable != null || !sourceDataTable.Equals(targetDataTable))
            {
                sourceDataTable.PrimaryKey = new DataColumn[] { sourceDataTable.Columns[primaryKey] };
                DataTable dt = targetDataTable.Copy();
                foreach (DataRow tRow in dt.Rows)
                {
                    try 
                    {
                        DataRow drFind = sourceDataTable.Rows.Find(tRow.Field<string>(primaryKey));
                        if (drFind != null) 
                        {
                            returnTable.ImportRow(tRow);
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return returnTable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string valueField = comboBox2.SelectedValue.ToString();
            string valuetype = comboBox1.SelectedItem.ToString();
            DataRow dr = dtSource.NewRow();
            dr["FIELDNAME"] = valueField;
            dr["TYPE"] = valuetype;
            dtSource.Rows.Add(dr);
            this.gridControl1.DataSource = dtSource;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DicPivot.Add("ROW", this.checkedComboBoxEdit1.EditValue.ToString());
            DicPivot.Add("COLUMNS", this.checkedComboBoxEdit2.EditValue.ToString());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
