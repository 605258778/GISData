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

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormTopoDia : Form
    {
        private string stepNo;

        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
        }

        private void FormTopoDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView()
        {
            this.treeView1.Nodes.Clear();
            ConnectDB cdb = new ConnectDB();
            CommonClass common = new CommonClass();
            try
            {
                DataTable dt = cdb.GetDataBySql("select * from GISDATA_TBTOPO");
                DataRow[] dr = dt.Select("PARENTID = 0");
                TreeNode rootNode = new TreeNode();
                rootNode.Text = "属性检查";
                rootNode.Tag = "0";
                treeView1.Nodes.Add(rootNode);
                for (int i = 0; i < dr.Length; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr[i]["NAME"].ToString();
                    tn.Tag = dr[i]["ID"].ToString();
                    common.FillTree(tn, dt);
                    rootNode.Nodes.Add(tn);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("数据库出错:" + e.Message));
            }
        }
    }
}
