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
        //private formNewPro.DelegateRefreshTree RefreshTree;
        public FormGroup()
        {
            InitializeComponent();
        }
        public FormGroup(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            InitializeComponent();
            this.selectedId = node.GetValue("ID").ToString();
        }

        private void buttonGroupOK_Click(object sender, EventArgs e)
        {
            string groupText = this.textBoxGroup.Text;
            string sql = "insert into GISDATA_TBATTR (PARENTID,NAME) VALUES(" + selectedId + ",'" + groupText + "')";
            ConnectDB db = new ConnectDB();
            Boolean result = db.Insert(sql);
            if (result) 
            {
                MessageBox.Show("添加成功！", "提示");
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
