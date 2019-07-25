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

        public FormNullValue()
        {
            InitializeComponent();
        }
        public string SelectedValue
        {
            get { return this.listBoxField.SelectedValue.ToString(); }
            set { listBoxField.SelectedItem = value; }
        }

        public FormNullValue(ComboBox comboBoxDataSour)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.comboBoxDataSour = comboBoxDataSour;
        }

        private void FormNullValue_Load(object sender, EventArgs e)
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            listBoxField.DataSource = dt;
            listBoxField.DisplayMember = "FIELD_ALSNAME";
            listBoxField.ValueMember = "FIELD_NAME";
        }
    }
}
