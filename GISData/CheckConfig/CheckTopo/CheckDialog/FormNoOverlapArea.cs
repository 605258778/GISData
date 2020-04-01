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
    public partial class FormNoOverlapArea : Form
    {
        public FormNoOverlapArea()
        {
            InitializeComponent();
        }

        public string comboBoxOverLayerValue
        {
            get { return comboBoxOverLayer.SelectedValue.ToString(); }
            set { comboBoxOverLayer.Text = value; }
        }

        private void FormNoOverlapArea_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO where FEATURE_TYPE = 'esriGeometryPolygon'");
            comboBoxOverLayer.DataSource = dt;
            comboBoxOverLayer.DisplayMember = "REG_ALIASNAME";
            comboBoxOverLayer.ValueMember = "REG_NAME";
        }

        private void comboBoxOverLayer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
