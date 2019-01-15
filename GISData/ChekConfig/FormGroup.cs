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
        private TreeView treeView;
        //private formNewPro.DelegateRefreshTree RefreshTree;
        public FormGroup()
        {
            InitializeComponent();
        }
        public FormGroup(TreeView treeView)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.treeView = treeView;
            //this.RefreshTree = RefreshTree;
        }

        private void buttonGroupOK_Click(object sender, EventArgs e)
        {
            string level = this.treeView.SelectedNode != null ? this.treeView.SelectedNode.Tag.ToString() : "0";
            string groupText = this.textBoxGroup.Text;
            string sql = "insert into GISDATA_TBATTR (PARENTID,NAME) VALUES(" + level + ",'" + groupText + "')";
            ConnectDB db = new ConnectDB();
            Boolean result = db.Insert(sql);
            if (result) 
            {
                MessageBox.Show("添加成功！", "提示");
            }
        }
    }
}
