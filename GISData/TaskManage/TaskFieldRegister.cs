using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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

namespace GISData.TaskManage
{
    public partial class TaskFieldRegister : Form
    {
        private GridColumn FidldName;
        private string TableName;
        private string updateType = "insert";
        public TaskFieldRegister()
        {
            InitializeComponent();
        }
        public TaskFieldRegister(object fieldName,string tablename)
        {
            InitializeComponent();
            this.TableName = tablename;
            this.FidldName = fieldName as GridColumn;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TaskFieldRegister_Load(object sender, EventArgs e)
        {
            this.comboBoxDic.Items.Add(new DictionaryEntry("资源数据字典", "GISDATA_ZYSJZD"));
            this.comboBoxDic.Items.Add(new DictionaryEntry("政区数据字典", "GISDATA_ZQSJZD"));
            this.comboBoxDic.DisplayMember = "Key";
            this.comboBoxDic.ValueMember = "Value";
            this.textEditField.EditValue = this.FidldName.FieldName;
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select CODE_PK,CODE_WHERE from GISDATA_MATEDATA WHERE REG_NAME='" + this.TableName + "' and FIELD_NAME ='"+this.FidldName.FieldName+"'");
            DataRow[] dr = dt.Select(null);
            updateType = "insert";
            if (dr.Length > 0) 
            {
                updateType = "edit";
                this.comboBoxDic.SelectedIndex = FindByValue(this.comboBoxDic, dr[0]["CODE_PK"].ToString());
                this.comboBoxDic.SelectedValue = dr[0]["CODE_PK"].ToString();
                this.textEditWhere.Text = dr[0]["CODE_WHERE"].ToString();
            }
        }

        private int FindByValue(System.Windows.Forms.ComboBox box, string strValue) 
        {
            int index = 0;
            foreach (DictionaryEntry item in box.Items)
            {
                if (item.Value.ToString() == strValue)
                {
                    index = box.FindString(item.Key.ToString());
                    break;
                }
            }
            return index;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DictionaryEntry selectValue = (DictionaryEntry)comboBoxDic.SelectedItem;
            string dic = selectValue.Value.ToString();
            string where = this.textEditWhere.Text;
            Boolean flag = false;
            if (updateType == "insert")
            {
                flag = db.Insert("insert into GISDATA_MATEDATA (REG_NAME,FIELD_NAME,FIELD_ALSNAME,DATA_TYPE,CODE_PK,CODE_WHERE) values ('" + this.TableName + "','" + this.FidldName.FieldName + "','" + this.FidldName.FieldName + "','" + this.FidldName.ColumnType.ToString() + "','" + dic + "','" + where + "')");
            }
            else 
            {
                flag = db.Update("update GISDATA_MATEDATA set CODE_PK ='" + dic + "',CODE_WHERE='" + where + "' where REG_NAME ='" + this.TableName + "' AND FIELD_NAME ='" + this.FidldName.FieldName + "'");
            }
            if (flag)
            {
                XtraMessageBox.Show("注册成功");
            }
            else 
            {
                XtraMessageBox.Show("注册失败");
            }
            this.Close();
        }
    }
}
