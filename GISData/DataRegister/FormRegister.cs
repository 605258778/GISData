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
        private string regName;
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
        public void refreshDataGridField(string regName)
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select ID,FIELD_NAME AS 字段名,FIELD_ALSNAME AS 别名,DATA_TYPE AS 数据类型,IS_AUTO AS 是否自称,IS_KEY AS 是否主键,MAXLEN AS 字段长度,IS_NULL AS 是否为空,IS_READONLY AS 是否只读,CAN_SHOW AS 是否显示,CODE_PK AS 字典域,CODE_WHERE AS 字典域条件 from GISDATA_MATEDATA where REG_NAME = '" + regName+"'");
            this.dataGridViewFieldView.DataSource = dt;
            if (this.dataGridViewFieldView.Columns.Count > 0)
            {
                this.dataGridViewFieldView.Columns[0].Visible = false;
            }
        }

        public void refreshDataGridReg()
        {
            ConnectDB cd = new ConnectDB();
            DataTable dt = cd.GetDataBySql("select REG_NAME,REG_ALIASNAME AS 注册名称 from GISDATA_REGINFO");
            this.dataGridViewDataReg.DataSource = dt;
            if (this.dataGridViewDataReg.Columns.Count > 0)
            {
                this.dataGridViewDataReg.Columns[0].Visible = false;
            }
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
            if (this.treeViewReg.SelectedNode.Level == 1) 
            {
                DialogResult dr = MessageBox.Show("确定注册？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    ConnectDB cd = new ConnectDB();
                    Boolean result = cd.Insert("insert into GISDATA_REGINFO (REG_NAME,REG_ALIASNAME) values ('" + treeViewReg.SelectedNode.Tag + "','" + treeViewReg.SelectedNode.Text + "')");
                    if (result) 
                    {
                        TreeNodeMul node = this.treeViewReg.SelectedNode as TreeNodeMul;
                        IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                        IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(node.Item1, 0) as IFeatureWorkspace;
                        //打开数据文件中航路点这个表
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(node.Tag.ToString());
                        Boolean boolFlag = true;
                        for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++) 
                        {
                            IField field = pFeatureClass.Fields.get_Field(i);
                            string REG_NAME = node.Tag.ToString();
                            string FIELDID = i.ToString();
                            string FIELD_NAME = field.Name;
                            string FIELD_ALSNAME = field.AliasName;
                            string DATA_TYPE = field.Type.ToString();
                            string MAXLEN = field.Length.ToString();
                            string MINLEN = field.Length.ToString();
                            string CODE_PK = "";
                            string CODE_WHERE = "";
                            string IS_NULL = field.IsNullable.ToString();
                            string DEFAULT_VALUE = field.DefaultValue.ToString();
                            Boolean insertResult = cd.Insert("insert into GISDATA_MATEDATA (REG_NAME,FIELDID,FIELD_NAME,FIELD_ALSNAME,DATA_TYPE,MAXLEN,MINLEN,CODE_PK,CODE_WHERE,IS_NULL,DEFAULT_VALUE) values ('" + REG_NAME + "','" + FIELDID + "','" + FIELD_NAME + "','" + FIELD_ALSNAME + "','" + DATA_TYPE + "','" + MAXLEN + "','" + MINLEN + "','" + CODE_PK + "','" + CODE_WHERE + "','" + IS_NULL + "','" + DEFAULT_VALUE + "')");
                            if (!insertResult) 
                            {
                                boolFlag = false;
                            }
                        }
                        if (boolFlag)
                        {
                            MessageBox.Show("注册成功！", "提示");
                            refreshDataGridReg();
                        }
                        else 
                        {
                            MessageBox.Show("注册失败！", "提示");
                        }
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
                        MessageBox.Show("删除成功！", "提示");
                    }
                }
            }
        }

        private void dataGridViewDataReg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvr = dataGridViewDataReg.CurrentRow;
            string val = dgvr.Cells["REG_NAME"].Value.ToString();
            this.regName = val;
            refreshDataGridField(val);
        }
        //删除注册信息
        private void buttonDelReg_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgvr = dataGridViewDataReg.CurrentRow;
            string val = dgvr.Cells["REG_NAME"].Value.ToString();
            if (val != "") 
            {
                DialogResult dr = MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    ConnectDB cd = new ConnectDB();
                    Boolean result = cd.Delete("DELETE from GISDATA_REGINFO WHERE REG_NAME = '" + val+"'");
                    if (result)
                    {
                        Boolean result1 = cd.Delete("DELETE from GISDATA_MATEDATA WHERE REG_NAME = '" + val + "'");
                        if (result1)
                        {
                            refreshDataGridReg();
                            MessageBox.Show("删除成功！", "提示");
                        }
                    }
                }
            }
        }

        private void dataGridViewFieldView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvr = this.dataGridViewFieldView.CurrentRow;
            FormDomain fd = new FormDomain(dgvr, regName);
            fd.Show();
        }
    }
}
