using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataRegister
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void buttonAddConnect_Click(object sender, EventArgs e)
        {
            FormDBConnectInfo fdci = new FormDBConnectInfo(this.treeViewReg);
            fdci.Show();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            refreshTreeViewReg();
            refreshDataGridReg();
        }
        public void refreshDataGridReg()
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select REG_NAME AS 注册名称 from GISDATA_REGINFO");
            this.dataGridViewDataReg.DataSource = dt;
            //this.dataGridViewDataReg.AutoGenerateColumns = false;//不自动  
        }

        //刷新注册连接树
        public void refreshTreeViewReg() 
        {
            this.treeViewReg.Nodes.Clear();
            //加载注册数据连接
            ConnectDB cd = new ConnectDB();
            GetAllFeatures gaf = new GetAllFeatures();
            DataTable dt = cd.GetDataBySql("select * from GISDATA_REGCONNECT");
            DataRow[] dr = dt.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                string regPath = dr[i]["REG_PATH"].ToString();
                string regType = dr[i]["REG_TYPE"].ToString();
                IFeatureWorkspace space;
                if (regType == "Access数据库")
                {
                    AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(regPath, 0);
                }
                else
                {
                    FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
                    space = (IFeatureWorkspace)fac.OpenFromFile(regPath, 0);
                }
                IWorkspace iwk = (IWorkspace)space;
                List<IDataset> ifcList = gaf.GetAllFeatureClass(iwk);
                TreeNodeMul tn = new TreeNodeMul();
                tn.Text = dr[i]["REG_NAME"].ToString();
                tn.Tag = dr[i]["ID"].ToString();
                for(int j = 0; j < ifcList.Count; j++)
                {
                    TreeNodeMul tnChildre = new TreeNodeMul();
                    IFeatureClass ifc = ifcList[j] as IFeatureClass;
                    tnChildre.Tag = ifcList[j].Name;
                    tnChildre.Text = ifc.AliasName;
                    tnChildre.Item1 = regPath;
                    tnChildre.Item2 = regType;
                    tn.Nodes.Add(tnChildre);
                }
                this.treeViewReg.Nodes.Add(tn);
            }
        }

        private void treeViewReg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            { 
            
            }
        }
        //添加注册连接
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (this.treeViewReg.SelectedNode.Level == 1) {
                DialogResult dr = MessageBox.Show("确定注册？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    ConnectDB cd = new ConnectDB();
                    Boolean result = cd.Insert("insert into GISDATA_REGINFO (REG_NAME) values ('" + treeViewReg.SelectedNode.Text + "')");
                    if (result) 
                    {
                        TreeNodeMul node = this.treeViewReg.SelectedNode as TreeNodeMul;
                        IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                        IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(node.Item1, 0) as IFeatureWorkspace;
                        //打开数据文件中航路点这个表
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(node.Name);
                        for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++) 
                        {
                            IField field = pFeatureClass.Fields.get_Field(i);
                            Boolean insertResult = cd.Insert("insert into GISDATA_REGINFO (REG_NAME) values ('" + treeViewReg.SelectedNode.Text + "')");
                        }
                            MessageBox.Show("注册成功！", "提示");
                    }
                }
            }
        }
        //删除注册连接
        private void buttonDelConnecte_Click(object sender, EventArgs e)
        {
            if (this.treeViewReg.SelectedNode.Level == 0){
                DialogResult dr = MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    ConnectDB cd = new ConnectDB();
                    Boolean result = cd.Delete("DELETE from GISDATA_REGCONNECT WHERE ID = " + this.treeViewReg.SelectedNode.Tag);
                    if (result)
                    {
                        refreshTreeViewReg();
                        MessageBox.Show("删除成功！", "提示");
                    }
                }
            }
        }

        private void dataGridViewDataReg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
