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

namespace GISData.ChekConfig
{
    public partial class FormGroup : Form
    {
        private string selectedId;
        private string scheme;
        private int checkNo;
        //private formNewPro.DelegateRefreshTree RefreshTree;
        public FormGroup()
        {
            InitializeComponent();
        }
        public FormGroup(string nodeId, string scheme,int checkNo)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.selectedId = nodeId;
            this.scheme = scheme;
            this.checkNo = checkNo;
        }

        private void buttonGroupOK_Click(object sender, EventArgs e)
        {
            string groupText = this.textBoxGroup.Text;
            string sql = "insert into GISDATA_TBATTR (PARENTID,NAME,SCHEME,STEP_NO) VALUES(" + selectedId + ",'" + groupText + "','" + this.scheme + "'," + this.checkNo + ")";
            ConnectDB db = new ConnectDB();
            Boolean result = db.Insert(sql);
            if (result) 
            {
                MessageBox.Show("添加成功！", "提示");
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormGroup_Load(object sender, EventArgs e)
        {

        }
    }
}
