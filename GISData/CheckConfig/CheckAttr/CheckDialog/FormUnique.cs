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

namespace GISData.ChekConfig.CheckDialog
{
    public partial class FormUnique : Form
    {
        private ComboBox comboBoxDataSour;
        private string type;
        private string selectedId;

        public string textCheckedValue
        {
            get {
                string uniqueField = "";
                foreach (DataRowView itemChecked in this.checkedListBox1.CheckedItems)
                {
                    uniqueField += itemChecked.Row[0] + "&";
                }
                return uniqueField.Substring(0, uniqueField.Length - 1); 
            }
            set 
            {
                string[] FieldArray = value.Split('&');
                foreach (string iitem in FieldArray)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)checkedListBox1.Items[i];
                        if (item.Row[0].ToString() == iitem) 
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                    
                }
            }
        }

        public FormUnique(ComboBox comboBoxDataSour,string type1, string selectedId1)
        {
            InitializeComponent();
            this.comboBoxDataSour = comboBoxDataSour;
            this.type = type1;
            this.selectedId = selectedId1;
        }

        private void FormUnique_Load(object sender, EventArgs e)
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            checkedListBox1.DataSource = dt;
            checkedListBox1.DisplayMember = "FIELD_ALSNAME";
            checkedListBox1.ValueMember = "FIELD_NAME";
            if (this.type == "edit")
            {
                DataTable editDB = db.GetDataBySql("select FIELD from GISDATA_TBATTR where id = " + selectedId);
                DataRow[] drs = editDB.Select("1=1");
                string field = drs[0]["FIELD"].ToString();
                this.textCheckedValue = field;
            }
        }
    }
}
