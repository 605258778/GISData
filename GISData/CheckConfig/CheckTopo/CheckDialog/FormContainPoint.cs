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
    public partial class FormContainPoint : Form
    {
        public FormContainPoint()
        {
            InitializeComponent();
        }
        public string textBoxWhereValue 
        { 
            get { return textBoxWhere.Text; } 
            set { textBoxWhere.Text = value; } 
        }
        public string comboBoxPointValue
        {
            get { return comboBoxPoint.SelectedValue.ToString(); }
            set { comboBoxPoint.Text = value; }
        }
        public string textBoxNumPointValue
        {
            get { return textBoxNumPoint.Text; }
            set { textBoxNumPoint.Text = value; }
        }

        private void FormContainPoint_Load(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select REG_NAME,REG_ALIASNAME from GISDATA_REGINFO where FEATURE_TYPE = 'esriGeometryPoint'");
            comboBoxPoint.DataSource = dt;
            try 
            {
                comboBoxPoint.DisplayMember = "REG_ALIASNAME";
                comboBoxPoint.ValueMember = "REG_NAME";
            }
            catch 
            { 
            
            }
            
        }

        private void textBoxNumPoint_Validated(object sender, EventArgs e)
        {
            int tmp;
            if (!int.TryParse(textBoxNumPoint.Text, out tmp))
            {
                MessageBox.Show("请输入数字");
            }
        }
    }
}
