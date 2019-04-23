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

namespace GISData.CheckConfig.CheckTopo.CheckDialog
{
    public partial class FormNoInterLine : Form
    {
        public FormNoInterLine()
        {
            InitializeComponent();
        }

        public string comboBoxLineValue
        {
            get { return comboBoxLine.SelectedValue.ToString(); }
            set { comboBoxLine.Text = value; }
        }

        private void FormNoInterLine_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO where FEATURE_TYPE = 'esriGeometryPolyline'");
            comboBoxLine.DataSource = dt;
            comboBoxLine.DisplayMember = "REG_ALIASNAME";
            comboBoxLine.ValueMember = "REG_NAME";
        }
    }
}
