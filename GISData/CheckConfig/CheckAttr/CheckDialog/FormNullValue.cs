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
    public partial class FormNullValue : Form
    {
        private ComboBox comboBoxDataSour;
        private string selectedId;
        private string type;
        public FormNullValue()
        {
            InitializeComponent();
        }
        public string SelectedValue
        {
            get { return this.listBoxField.SelectedValue.ToString(); }
            set { listBoxField.SelectedItem = value; }
        }

        public FormNullValue(ComboBox comboBoxDataSour, string type, string selectedId)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.comboBoxDataSour = comboBoxDataSour;
            this.type = type;
            this.selectedId = selectedId;
        }

        private void FormNullValue_Load(object sender, EventArgs e)
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            listBoxField.DataSource = dt;
            listBoxField.DisplayMember = "FIELD_ALSNAME";
            listBoxField.ValueMember = "FIELD_NAME";
            if (this.type == "edit")
            {
                DataTable editDB = db.GetDataBySql("select FIELD from GISDATA_TBATTR where id = " + selectedId);
                DataRow[] drs = editDB.Select("1=1");
                string field = drs[0]["FIELD"].ToString();
                for (int i = 0; i < listBoxField.Items.Count; i++)
                {
                    DataRowView item = (DataRowView)listBoxField.Items[i];
                    if (item.Row[0].ToString().Equals(field))
                    {
                        this.listBoxField.SelectedIndex = i;
                    }
                }
            }
        }
    }
}
