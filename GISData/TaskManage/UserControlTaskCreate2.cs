namespace TaskManage
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraNavBar;
    using DevExpress.XtraNavBar.ViewInfo;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geodatabase;
    using FormBase;
    using FunFactory;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using Utilities;
    using DevExpress.XtraGrid.Views.Base;
    using td.logic.sys;
    using td.db.orm;
    using td.db.mid.sys;
    using System.Collections.Generic;
using td.forest.task.linker;

    public class UserControlTaskCreate2 : UserControlBase1
    {
        private ButtonEdit buttonEditDataPath;
        private CheckedListBoxControl cListBoxKind;
        private CheckedListBoxControl cListBoxKind2;
        private CheckedListBoxControl cListBoxKind3;
        private CheckedListBoxControl cListBoxLayer;
        private IContainer components;
        private GridColumn gridColumn1;
        private GridColumn gridColumn2;
        private GridColumn gridColumn3;
        private GridControl gridControl1;
        private GridControl gridControl2;
        private GridView gridView1;
        private GridView gridView2;
        internal ImageList ImageList1;
        internal ImageList ImageList2;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelprogress;
        private ListBoxControl listBoxDataList;
        private ListBoxControl listBoxDist;
        private IFeatureLayer m_CountyLayer;
        private ITable m_CountyTable;
        private IFeatureLayer m_EditLayer;
        private IFeatureLayer m_TownLayer;
        private ITable m_TownTable;
        private IFeatureLayer m_VillageLayer;
        private ITable m_VillageTable;
        private const string mClassName = "TaskManage.UserControlTaskCreate2";
     
        private string mEditKind = "小班";
        private string mEditKindCode = "";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private DataTable mFieldTable;
        private HookHelper mHookHelper;
        private bool mIsBatch = true;
        private string mKindCode = "";
      //  private DataTable mKindTable;
        private ArrayList mRangeList;
        private bool mSelected;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
      //  private DataTable mTaskTable;
        private IList<T_EDITTASK_ZT_Mid> m_taskList;
             
        private const string myClassName = "编辑任务创建";
        private NavBarControl navBarControl1;
        private NavBarGroup navBarGroup1;
        private NavBarGroup navBarGroup2;
        private NavBarGroup navBarGroup3;
        private NavBarGroup navBarGroup4;
        private NavBarGroup navBarGroup5;
        private NavBarGroupControlContainer navBarGroupControlContainer1;
        private NavBarGroupControlContainer navBarGroupControlContainer2;
        private NavBarGroupControlContainer navBarGroupControlContainer3;
        private NavBarGroupControlContainer navBarGroupControlContainer4;
        private NavBarGroupControlContainer navBarGroupControlContainer5;
        private Panel panel10;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private Panel panel14;
        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private Panel panelKind;
        private ProgressBarControl progressBar;
        private RadioGroup radioGroup1;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private SimpleButton simpleButtonAdd;
        private SimpleButton simpleButtonClear;
        private SimpleButton simpleButtonCreate;
        private SimpleButton simpleButtonDrawRange;
        private SimpleButton simpleButtonFind;
        private SimpleButton simpleButtonLocation;
        private SimpleButton simpleButtonRemove;
        internal TreeListColumn tcolBase1;
        internal TreeListColumn tcolBase2;
        internal TreeList tList;
        private TextEdit txtDistName;

        public UserControlTaskCreate2()
        {
            this.InitializeComponent();
        }
        public ProjectManager PM
        {
            get { return DBServiceFactory<ProjectManager>.Service; }
        }
        private void cListBoxKind_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Unchecked)
            {
                this.cListBoxKind2.Visible = false;
                this.cListBoxKind3.Visible = false;
            }
            else
            {
                try
                {
                    if ((!this.mSelected && (this.cListBoxKind.SelectedIndex != -1)) && (this.cListBoxKind.Tag != null))
                    {
                        for (int i = 0; i < this.cListBoxKind.Items.Count; i++)
                        {
                            if (i != this.cListBoxKind.SelectedIndex)
                            {
                                this.cListBoxKind.Items[i].CheckState = CheckState.Unchecked;
                            }
                        }
                        try
                        {
                            this.cListBoxKind2.Items.Clear();
                            this.cListBoxKind3.Items.Clear();
                        }
                        catch (Exception)
                        {
                            this.cListBoxKind2.Items.Clear();
                            this.cListBoxKind3.Items.Clear();
                        }
                       // string str = "";
                        DataTable tag = this.cListBoxKind.Tag as DataTable;
                        //str = tag.Rows[this.cListBoxKind.SelectedIndex]["code"].ToString().Substring(0, 2);
                       // this.mEditKindCode = tag.Rows[this.cListBoxKind.SelectedIndex]["code"].ToString();
                        IList<T_DESIGNKIND_Mid> lst = PM.FindSecondTreeByKindCode(this.mKindCode);
                        if (lst != null)
                        {
                            for (int j = 0; j < lst.Count; j++)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.Items.Add(lst[j].name);
                                this.mSelected = false;
                            }
                            if (this.cListBoxKind2.Items.Count > 0)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.SelectedIndex = 0;
                                this.cListBoxKind2.Items[0].CheckState = CheckState.Checked;
                                this.mSelected = false;
                                this.cListBoxKind2.Visible = true;
                            }
                            if (this.cListBoxKind2.Items.Count == 0)
                            {
                                this.cListBoxKind.Width = this.panelKind.Width - 4;
                                this.cListBoxKind2.Visible = false;
                                this.cListBoxKind3.Visible = false;
                            }
                            else
                            {
                                this.cListBoxKind2.Tag = lst;

                               // str = lst[this.cListBoxKind.SelectedIndex].code.Substring(0, 4);
                                this.mEditKindCode = lst[this.cListBoxKind.SelectedIndex].code;
                               // DataTable table3 = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where (code like '" + str + "%' and right(code ,2 )<>'00' and right(code,4)<>'0000' and kind='" + this.mKindCode + "')");
                             //   IList<T_DESIGNKIND_Mid> lst3 = PM.("code like '" + str + "%' and right(code ,2 )<>'00' and right(code,4)<>'0000' and kind='" + this.mKindCode + "'");
                                IList<T_DESIGNKIND_Mid> lst3 = PM.FindThirdTreeByKindCode(mEditKindCode);
                                if (lst3 == null)
                                {
                                    return;
                                }
                                for (int k = 0; k < lst3.Count; k++)
                                {
                                    this.mSelected = true;
                                    this.cListBoxKind3.Items.Add(lst3[k].name);
                                    this.mSelected = false;
                                }
                                if (this.cListBoxKind3.Items.Count > 0)
                                {
                                    this.mSelected = true;
                                    this.cListBoxKind3.SelectedIndex = 0;
                                    this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                                    this.mSelected = false;
                                    this.cListBoxKind3.Visible = true;
                                }
                                if (this.cListBoxKind3.Items.Count == 0)
                                {
                                    this.cListBoxKind.Width = (this.panelKind.Width / 2) - 4;
                                    this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                    this.cListBoxKind2.Visible = true;
                                    this.cListBoxKind3.Visible = false;
                                }
                                else
                                {
                                    this.cListBoxKind.Width = (this.panelKind.Width / 3) - 6;
                                    this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                    this.cListBoxKind3.Width = this.cListBoxKind.Width;
                                    this.cListBoxKind2.Visible = true;
                                    this.cListBoxKind3.Visible = true;
                                    this.mEditKindCode = lst3[0].code;
                                    this.cListBoxKind3.Tag = lst3;
                                }
                            }
                            T_EDITTASK_ZT_Mid mid = m_taskList[0];
                            string str2 = "";
                            if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                            {
                                str2 = this.cListBoxKind.SelectedItem.ToString();
                            }
                            if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                            {
                                str2 = str2 + this.cListBoxKind2.SelectedItem.ToString();
                            }
                            if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                            {
                                str2 = str2 + this.cListBoxKind3.SelectedItem.ToString();
                            }
                            mid .taskname= string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str2, "作业设计" });
                            string str3 = this.mKindCode + this.mEditKindCode;
                            if (str3.Length == 7)
                            {
                                str3 = "0" + str3;
                            }
                            mid.taskkind = str3;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void cListBoxKind_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cListBoxKind2_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Unchecked)
            {
                this.cListBoxKind3.Visible = false;
            }
            else
            {
                try
                {
                    if ((!this.mSelected && (this.cListBoxKind2.SelectedIndex != -1)) && (this.cListBoxKind2.Tag != null))
                    {
                        for (int i = 0; i < this.cListBoxKind2.Items.Count; i++)
                        {
                            if (i != this.cListBoxKind2.SelectedIndex)
                            {
                                this.cListBoxKind2.Items[i].CheckState = CheckState.Unchecked;
                            }
                        }
                        try
                        {
                            this.cListBoxKind3.Items.Clear();
                        }
                        catch (Exception)
                        {
                            this.cListBoxKind3.Items.Clear();
                        }
                        string str = "";
                        DataTable tag = this.cListBoxKind2.Tag as DataTable;
                        str = tag.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString().Substring(0, 4);
                        this.mEditKindCode = tag.Rows[this.cListBoxKind2.SelectedIndex]["code"].ToString();
                       // DataTable dataTable = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where (code like '" + str + "%' and right(code ,2 )<>'00' and kind='" + this.mKindCode + "')");
                      //  IList<T_DESIGNKIND_Mid> lst = PM.FindDKBySql("code like '" + str + "%' and right(code ,2 )<>'00' and kind='" + this.mKindCode + "'");
                     
                        IList<T_DESIGNKIND_Mid> lst = PM.FindThirdTreeByKindCode(mKindCode);
                        for (int j = 0; j < lst.Count; j++)
                        {
                            this.mSelected = true;
                            this.cListBoxKind3.Items.Add(lst[j].name);
                            this.mSelected = false;
                        }
                        if (this.cListBoxKind3.Items.Count > 0)
                        {
                            this.mSelected = true;
                            this.cListBoxKind3.SelectedIndex = 0;
                            this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                            this.mSelected = false;
                            this.cListBoxKind3.Visible = true;
                        }
                        if (this.cListBoxKind3.Items.Count == 0)
                        {
                            this.cListBoxKind3.Visible = false;
                        }
                        else
                        {
                            this.mEditKindCode = lst[this.cListBoxKind3.SelectedIndex].code;
                                
                            if (this.cListBoxKind3.Items.Count == 0)
                            {
                                this.cListBoxKind.Width = (this.panelKind.Width / 2) - 4;
                                this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                this.cListBoxKind2.Visible = true;
                                this.cListBoxKind3.Visible = false;
                            }
                            else
                            {
                                this.cListBoxKind.Width = (this.panelKind.Width / 3) - 6;
                                this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                this.cListBoxKind3.Width = this.cListBoxKind.Width;
                                this.cListBoxKind2.Visible = true;
                                this.cListBoxKind3.Visible = true;
                            }
                            this.cListBoxKind3.Tag = lst;
                        }
                        T_EDITTASK_ZT_Mid mid = m_taskList[0];
                        string str2 = "";
                        if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                        {
                            str2 = this.cListBoxKind.SelectedItem.ToString();
                        }
                        if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                        {
                            str2 = str2 + this.cListBoxKind2.SelectedItem.ToString();
                        }
                        if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                        {
                            str2 = str2 + this.cListBoxKind3.SelectedItem.ToString();
                        }
                        mid .taskname= string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str2, "作业设计" });
                        string str3 = this.mKindCode + this.mEditKindCode;
                        if (str3.Length == 7)
                        {
                            str3 = "0" + str3;
                        }
                        mid.taskkind = str3;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void cListBoxKind2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cListBoxKind3_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State != CheckState.Unchecked)
            {
                try
                {
                    if (!this.mSelected)
                    {
                        for (int i = 0; i < this.cListBoxKind3.Items.Count; i++)
                        {
                            if (i != this.cListBoxKind3.SelectedIndex)
                            {
                                this.cListBoxKind3.Items[i].CheckState = CheckState.Unchecked;
                            }
                        }
                        DataTable tag = this.cListBoxKind3.Tag as DataTable;
                        this.mEditKindCode = tag.Rows[this.cListBoxKind3.SelectedIndex]["code"].ToString();
                        T_EDITTASK_ZT_Mid mid = this.m_taskList[0];
                        string str = "";
                        if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                        {
                            str = this.cListBoxKind.SelectedItem.ToString();
                        }
                        if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                        {
                            str = str + this.cListBoxKind2.SelectedItem.ToString();
                        }
                        if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                        {
                            str = str + this.cListBoxKind3.SelectedItem.ToString();
                        }
                        mid.taskname = string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str, "作业设计" });
                        string str2 = this.mKindCode + this.mEditKindCode;
                        if (str2.Length == 7)
                        {
                            str2 = "0" + str2;
                        }
                        mid.taskkind = str2;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void cListBoxKind3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Hook(object hook, string sEditKind)
        {
            try
            {
                this.mEditKind = sEditKind;
                if (hook != null)
                {
                    this.mHookHelper = new HookHelperClass();
                    this.mHookHelper.Hook = hook;
                    if (this.InitialValue())
                    {
                        if (this.mKindCode == "2")
                        {
                            this.tList.Visible = true;
                            this.listBoxDist.Visible = false;
                            this.InitialTreeList();
                        }
                        else
                        {
                            this.tList.Visible = false;
                            this.listBoxDist.Visible = true;
                            this.InitialDistList();
                        }
                    }
                    this.InitialControl();
                }
            }
            catch (Exception)
            {
            }
        }
        private DB2GridViewManager DBVM
        {
            get
            {
                return DBServiceFactory<DB2GridViewManager>.Service;
            }
        }
        private void InitialControl()
        {
            try
            {
                this.cListBoxLayer.Items.Clear();
                string configValue = "";
                if (this.mEditKind == "小班")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLayer");
                }
                else if (this.mEditKind == "造林")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditZLLayer");
                }
                else if (this.mEditKind == "采伐")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditCFLayer");
                }
                else if (this.mEditKind == "林改")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLGLayer");
                }
                this.cListBoxLayer.Items.Add(configValue, true);
                string[] strArray = UtilFactory.GetConfigOpt().GetConfigValue("ConnectLayer").Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    this.cListBoxLayer.Items.Add(strArray[i], true);
                }
                this.gridControl2.Visible = true;
                this.gridControl2.DataSource = null;
                this.gridView2.Columns.Clear();
                this.gridControl2.DataSource = this.m_taskList;
                this.gridView2.RefreshData();
                this.gridView2.OptionsView.ShowColumnHeaders = false;
                IList<T_SYS_META_FIELDS_Mid> lst = new List<T_SYS_META_FIELDS_Mid>();
                T_SYS_META_FIELDS_Mid mid = new T_SYS_META_FIELDS_Mid() { FIEL_AL = "名称", FIEL_NA = "taskname", TAB_ID ="abcd1234"};
                DBVM.InitViewColumn(lst, gridView2);
                //for (int j = 0; j < this.mTaskTable.Columns.Count; j++)
                //{
                //    if (this.mTaskTable.Columns[j].ColumnName == "taskname")
                //    {
                //        this.mTaskTable.Columns[j].Caption = "名称";
                //        this.gridView2.Columns[j].Visible = true;
                //    }
                //    else
                //    {
                //        this.gridView2.Columns[j].Visible = false;
                //    }
                //}
                //DataRow row = this.mTaskTable.NewRow();
                T_EDITTASK_ZT_Mid tskMid = new T_EDITTASK_ZT_Mid();

                string str2 = "";
                if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                {
                    str2 = this.cListBoxKind.SelectedItem.ToString();
                }
                if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                {
                    str2 = str2 + this.cListBoxKind2.SelectedItem.ToString();
                }
                if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                {
                    str2 = str2 + this.cListBoxKind3.SelectedItem.ToString();
                }
                tskMid.taskname= string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str2, "作业设计" });
                string str3 = this.mKindCode + this.mEditKindCode;
                if (str3.Length == 7)
                {
                    str3 = "0" + str3;
                }
                tskMid.taskkind = str3;
                if (this.listBoxDist.SelectedIndex != -1)
                {
                    tskMid.distcode = this.mRangeList[this.listBoxDist.SelectedIndex].ToString();
                }
                else if (this.tList.Selection.Count != 0)
                {
                    tskMid.distcode = this.mRangeList[0].ToString();
                }
                tskMid.taskstate = "1";
                tskMid .taskyear= DateTime.Now.Year.ToString();
                tskMid.createtime = DateTime.Now.ToLocalTime().ToString();
                tskMid .edittime= DateTime.Now.ToLocalTime().ToString();
               // this.mTaskTable.Rows.Add(row);
                m_taskList.Add(tskMid);
                this.gridView2.RefreshData();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate2", "InitialControl", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitialDistList()
        {
            try
            {
                this.listBoxDist.Items.Clear();
                this.mRangeList = new ArrayList();
                this.m_CountyLayer.FeatureClass.FeatureCount(null);
                IFeatureCursor cursor = this.m_CountyLayer.FeatureClass.Search(null, false);
                IFeature feature = cursor.NextFeature();
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldCode");
                int index = feature.Fields.FindField(configValue);
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldName");
                feature.Fields.FindField(configValue);
                string str2 = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode");
                while (feature != null)
                {
                    IQueryFilter queryFilter = new QueryFilterClass {
                        WhereClause = str2 + "='" + feature.get_Value(index).ToString() + "'"
                    };
                    ICursor cursor2 = this.m_CountyTable.Search(queryFilter, false);
                    IRow row = cursor2.NextRow();
                    int num2 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode"));
                    int num3 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName"));
                    while (row != null)
                    {
                        if (row.get_Value(num2).ToString() == feature.get_Value(index).ToString())
                        {
                            this.listBoxDist.Items.Add(row.get_Value(num3).ToString());
                            this.mRangeList.Add(row.get_Value(num2).ToString());
                        }
                        row = cursor2.NextRow();
                    }
                    feature = cursor.NextFeature();
                }
                if (this.listBoxDist.Items.Count > 0)
                {
                    this.listBoxDist.SelectedIndex = 0;
                    this.txtDistName.Text = this.listBoxDist.SelectedItem.ToString();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate2", "InitialDistList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTaskCreate2));
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.panelKind = new System.Windows.Forms.Panel();
            this.cListBoxKind3 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cListBoxKind2 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.cListBoxKind = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.tList = new DevExpress.XtraTreeList.TreeList();
            this.tcolBase1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tcolBase2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listBoxDist = new DevExpress.XtraEditors.ListBoxControl();
            this.panel13 = new System.Windows.Forms.Panel();
            this.txtDistName = new DevExpress.XtraEditors.TextEdit();
            this.panel14 = new System.Windows.Forms.Panel();
            this.simpleButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonDrawRange = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonLocation = new DevExpress.XtraEditors.SimpleButton();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.navBarGroupControlContainer4 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.navBarGroupControlContainer5 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.simpleButtonClear = new DevExpress.XtraEditors.SimpleButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.listBoxDataList = new DevExpress.XtraEditors.ListBoxControl();
            this.panel12 = new System.Windows.Forms.Panel();
            this.simpleButtonAdd = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonRemove = new DevExpress.XtraEditors.SimpleButton();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonEditDataPath = new DevExpress.XtraEditors.ButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer3 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.cListBoxLayer = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup4 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup5 = new DevExpress.XtraNavBar.NavBarGroup();
            this.ImageList2 = new System.Windows.Forms.ImageList(this.components);
            this.simpleButtonCreate = new DevExpress.XtraEditors.SimpleButton();
            this.labelprogress = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.navBarGroupControlContainer1.SuspendLayout();
            this.panelKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind)).BeginInit();
            this.navBarGroupControlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDist)).BeginInit();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDistName.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.panel10.SuspendLayout();
            this.navBarGroupControlContainer4.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            this.navBarGroupControlContainer5.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDataList)).BeginInit();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDataPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            this.navBarGroupControlContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Controls.Add(this.panelKind);
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(312, 150);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // panelKind
            // 
            this.panelKind.BackColor = System.Drawing.Color.Transparent;
            this.panelKind.Controls.Add(this.cListBoxKind3);
            this.panelKind.Controls.Add(this.panel9);
            this.panelKind.Controls.Add(this.cListBoxKind2);
            this.panelKind.Controls.Add(this.panel8);
            this.panelKind.Controls.Add(this.cListBoxKind);
            this.panelKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKind.Location = new System.Drawing.Point(0, 0);
            this.panelKind.Name = "panelKind";
            this.panelKind.Padding = new System.Windows.Forms.Padding(7, 2, 5, 2);
            this.panelKind.Size = new System.Drawing.Size(312, 150);
            this.panelKind.TabIndex = 18;
            // 
            // cListBoxKind3
            // 
            this.cListBoxKind3.CheckOnClick = true;
            this.cListBoxKind3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cListBoxKind3.Location = new System.Drawing.Point(223, 2);
            this.cListBoxKind3.Name = "cListBoxKind3";
            this.cListBoxKind3.Size = new System.Drawing.Size(84, 146);
            this.cListBoxKind3.TabIndex = 11;
            this.cListBoxKind3.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cListBoxKind3_ItemCheck);
            this.cListBoxKind3.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind3_SelectedIndexChanged);
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(218, 2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(5, 146);
            this.panel9.TabIndex = 13;
            // 
            // cListBoxKind2
            // 
            this.cListBoxKind2.CheckOnClick = true;
            this.cListBoxKind2.Dock = System.Windows.Forms.DockStyle.Left;
            this.cListBoxKind2.Location = new System.Drawing.Point(106, 2);
            this.cListBoxKind2.Name = "cListBoxKind2";
            this.cListBoxKind2.Size = new System.Drawing.Size(112, 146);
            this.cListBoxKind2.TabIndex = 10;
            this.cListBoxKind2.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cListBoxKind2_ItemCheck);
            this.cListBoxKind2.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind2_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(101, 2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(5, 146);
            this.panel8.TabIndex = 12;
            // 
            // cListBoxKind
            // 
            this.cListBoxKind.CheckOnClick = true;
            this.cListBoxKind.Dock = System.Windows.Forms.DockStyle.Left;
            this.cListBoxKind.Location = new System.Drawing.Point(7, 2);
            this.cListBoxKind.Name = "cListBoxKind";
            this.cListBoxKind.Size = new System.Drawing.Size(94, 146);
            this.cListBoxKind.TabIndex = 9;
            this.cListBoxKind.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cListBoxKind_ItemCheck);
            this.cListBoxKind.SelectedIndexChanged += new System.EventHandler(this.cListBoxKind_SelectedIndexChanged);
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "设计范围";
            this.navBarGroup2.ControlContainer = this.navBarGroupControlContainer2;
            this.navBarGroup2.Expanded = true;
            this.navBarGroup2.GroupClientHeight = 163;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup2.Name = "navBarGroup2";
            this.navBarGroup2.SmallImageIndex = 17;
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Controls.Add(this.tList);
            this.navBarGroupControlContainer2.Controls.Add(this.listBoxDist);
            this.navBarGroupControlContainer2.Controls.Add(this.panel13);
            this.navBarGroupControlContainer2.Controls.Add(this.label8);
            this.navBarGroupControlContainer2.Controls.Add(this.panel2);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Padding = new System.Windows.Forms.Padding(5);
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(312, 159);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // tList
            // 
            this.tList.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Empty.Options.UseBackColor = true;
            this.tList.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tList.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tList.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.EvenRow.Options.UseBackColor = true;
            this.tList.Appearance.EvenRow.Options.UseForeColor = true;
            this.tList.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tList.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tList.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.tList.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tList.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tList.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tList.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tList.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tList.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tList.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tList.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.GroupButton.Options.UseBackColor = true;
            this.tList.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tList.Appearance.GroupButton.Options.UseForeColor = true;
            this.tList.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tList.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tList.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tList.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tList.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tList.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tList.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tList.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tList.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tList.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tList.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tList.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tList.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tList.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tList.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.HorzLine.Options.UseBackColor = true;
            this.tList.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.OddRow.Options.UseBackColor = true;
            this.tList.Appearance.OddRow.Options.UseForeColor = true;
            this.tList.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tList.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tList.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Preview.Options.UseBackColor = true;
            this.tList.Appearance.Preview.Options.UseForeColor = true;
            this.tList.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tList.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tList.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.Row.Options.UseBackColor = true;
            this.tList.Appearance.Row.Options.UseForeColor = true;
            this.tList.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tList.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tList.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tList.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tList.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tList.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.TreeLine.Options.UseBackColor = true;
            this.tList.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tList.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tList.Appearance.VertLine.Options.UseBackColor = true;
            this.tList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tcolBase1,
            this.tcolBase2});
            this.tList.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tList.Location = new System.Drawing.Point(5, 96);
            this.tList.LookAndFeel.SkinName = "Blue";
            this.tList.Name = "tList";
            this.tList.BeginUnboundLoad();
            this.tList.AppendNode(new object[] {
            "乡1",
            null}, -1, 2, 2, 1);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 0, -1, -1, 1);
            this.tList.AppendNode(new object[] {
            "村2",
            null}, 0, 0, 0, 1);
            this.tList.AppendNode(new object[] {
            "乡2",
            null}, -1, 0, 0, 0);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 3, 0, 0, 0);
            this.tList.AppendNode(new object[] {
            "乡3",
            null}, -1);
            this.tList.AppendNode(new object[] {
            "村1",
            null}, 5);
            this.tList.EndUnboundLoad();
            this.tList.OptionsBehavior.Editable = false;
            this.tList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tList.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.tList.OptionsSelection.InvertSelection = true;
            this.tList.OptionsView.ShowColumns = false;
            this.tList.OptionsView.ShowHorzLines = false;
            this.tList.OptionsView.ShowIndicator = false;
            this.tList.OptionsView.ShowVertLines = false;
            this.tList.Size = new System.Drawing.Size(302, 58);
            this.tList.StateImageList = this.ImageList1;
            this.tList.TabIndex = 87;
            this.tList.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tList.Visible = false;
            this.tList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tList_MouseClick);
            this.tList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tList_MouseUp);
            // 
            // tcolBase1
            // 
            this.tcolBase1.Caption = "名称";
            this.tcolBase1.FieldName = "设备号";
            this.tcolBase1.MinWidth = 100;
            this.tcolBase1.Name = "tcolBase1";
            this.tcolBase1.Visible = true;
            this.tcolBase1.VisibleIndex = 0;
            this.tcolBase1.Width = 100;
            // 
            // tcolBase2
            // 
            this.tcolBase2.Caption = "定位";
            this.tcolBase2.FieldName = "定位";
            this.tcolBase2.Name = "tcolBase2";
            this.tcolBase2.Visible = true;
            this.tcolBase2.VisibleIndex = 1;
            this.tcolBase2.Width = 20;
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "blank16.ico");
            this.ImageList1.Images.SetKeyName(1, "tick16.ico");
            this.ImageList1.Images.SetKeyName(2, "PART16.ICO");
            this.ImageList1.Images.SetKeyName(3, "");
            this.ImageList1.Images.SetKeyName(4, "");
            this.ImageList1.Images.SetKeyName(5, "");
            this.ImageList1.Images.SetKeyName(6, "");
            this.ImageList1.Images.SetKeyName(7, "");
            this.ImageList1.Images.SetKeyName(8, "");
            this.ImageList1.Images.SetKeyName(9, "");
            this.ImageList1.Images.SetKeyName(10, "");
            this.ImageList1.Images.SetKeyName(11, "");
            this.ImageList1.Images.SetKeyName(12, "");
            this.ImageList1.Images.SetKeyName(13, "");
            this.ImageList1.Images.SetKeyName(14, "");
            this.ImageList1.Images.SetKeyName(15, "");
            this.ImageList1.Images.SetKeyName(16, "(30,24).png");
            this.ImageList1.Images.SetKeyName(17, "(00,02).png");
            this.ImageList1.Images.SetKeyName(18, "(00,17).png");
            this.ImageList1.Images.SetKeyName(19, "(00,46).png");
            this.ImageList1.Images.SetKeyName(20, "(01,10).png");
            this.ImageList1.Images.SetKeyName(21, "(01,25).png");
            this.ImageList1.Images.SetKeyName(22, "(05,32).png");
            this.ImageList1.Images.SetKeyName(23, "(06,32).png");
            this.ImageList1.Images.SetKeyName(24, "(07,32).png");
            this.ImageList1.Images.SetKeyName(25, "(08,32).png");
            this.ImageList1.Images.SetKeyName(26, "(08,36).png");
            this.ImageList1.Images.SetKeyName(27, "(09,36).png");
            this.ImageList1.Images.SetKeyName(28, "(10,26).png");
            this.ImageList1.Images.SetKeyName(29, "(11,26).png");
            this.ImageList1.Images.SetKeyName(30, "(11,29).png");
            this.ImageList1.Images.SetKeyName(31, "(12,29).png");
            this.ImageList1.Images.SetKeyName(32, "(11,32).png");
            this.ImageList1.Images.SetKeyName(33, "(11,36).png");
            this.ImageList1.Images.SetKeyName(34, "(13,32).png");
            this.ImageList1.Images.SetKeyName(35, "(19,31).png");
            this.ImageList1.Images.SetKeyName(36, "(22,18).png");
            this.ImageList1.Images.SetKeyName(37, "(25,27).png");
            this.ImageList1.Images.SetKeyName(38, "(29,43).png");
            this.ImageList1.Images.SetKeyName(39, "(30,14).png");
            this.ImageList1.Images.SetKeyName(40, "5.png");
            this.ImageList1.Images.SetKeyName(41, "10.png");
            this.ImageList1.Images.SetKeyName(42, "11.png");
            this.ImageList1.Images.SetKeyName(43, "16.png");
            this.ImageList1.Images.SetKeyName(44, "17.png");
            this.ImageList1.Images.SetKeyName(45, "18.png");
            this.ImageList1.Images.SetKeyName(46, "19.png");
            this.ImageList1.Images.SetKeyName(47, "20.png");
            this.ImageList1.Images.SetKeyName(48, "21.png");
            this.ImageList1.Images.SetKeyName(49, "22.png");
            this.ImageList1.Images.SetKeyName(50, "25.png");
            this.ImageList1.Images.SetKeyName(51, "31.png");
            this.ImageList1.Images.SetKeyName(52, "41.png");
            this.ImageList1.Images.SetKeyName(53, "add.png");
            this.ImageList1.Images.SetKeyName(54, "bullet_minus.png");
            this.ImageList1.Images.SetKeyName(55, "control_add_blue.png");
            this.ImageList1.Images.SetKeyName(56, "control_power_blue.png");
            this.ImageList1.Images.SetKeyName(57, "control_remove_blue.png");
            this.ImageList1.Images.SetKeyName(58, "cross.png");
            this.ImageList1.Images.SetKeyName(59, "down.png");
            this.ImageList1.Images.SetKeyName(60, "draw_tools.png");
            this.ImageList1.Images.SetKeyName(61, "Feedicons_v2_010.png");
            this.ImageList1.Images.SetKeyName(62, "Feedicons_v2_011.png");
            this.ImageList1.Images.SetKeyName(63, "Feedicons_v2_031.png");
            this.ImageList1.Images.SetKeyName(64, "Feedicons_v2_032.png");
            this.ImageList1.Images.SetKeyName(65, "Feedicons_v2_033.png");
            this.ImageList1.Images.SetKeyName(66, "flag blue.png");
            this.ImageList1.Images.SetKeyName(67, "flag red.png");
            this.ImageList1.Images.SetKeyName(68, "flag yellow.png");
            this.ImageList1.Images.SetKeyName(69, "(44,23).png");
            this.ImageList1.Images.SetKeyName(70, "(12,29).png");
            this.ImageList1.Images.SetKeyName(71, "(34,00).png");
            this.ImageList1.Images.SetKeyName(72, "(03,02).png");
            this.ImageList1.Images.SetKeyName(73, "(49,06).png");
            this.ImageList1.Images.SetKeyName(74, "(09,13).png");
            this.ImageList1.Images.SetKeyName(75, "(16,47).png");
            this.ImageList1.Images.SetKeyName(76, "(13,47).png");
            this.ImageList1.Images.SetKeyName(77, "(18,01).png");
            this.ImageList1.Images.SetKeyName(78, "(18,13).png");
            this.ImageList1.Images.SetKeyName(79, "(19,01).png");
            this.ImageList1.Images.SetKeyName(80, "(28,40).png");
            this.ImageList1.Images.SetKeyName(81, "(39,47).png");
            this.ImageList1.Images.SetKeyName(82, "(45,12).png");
            this.ImageList1.Images.SetKeyName(83, "(45,17).png");
            this.ImageList1.Images.SetKeyName(84, "(45,41).png");
            // 
            // listBoxDist
            // 
            this.listBoxDist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDist.Location = new System.Drawing.Point(5, 96);
            this.listBoxDist.MultiColumn = true;
            this.listBoxDist.Name = "listBoxDist";
            this.listBoxDist.Size = new System.Drawing.Size(302, 58);
            this.listBoxDist.TabIndex = 83;
            this.listBoxDist.SelectedIndexChanged += new System.EventHandler(this.listBoxDist_SelectedIndexChanged);
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Transparent;
            this.panel13.Controls.Add(this.txtDistName);
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Controls.Add(this.simpleButtonFind);
            this.panel13.Controls.Add(this.simpleButtonDrawRange);
            this.panel13.Controls.Add(this.simpleButtonLocation);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(5, 61);
            this.panel13.Name = "panel13";
            this.panel13.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel13.Size = new System.Drawing.Size(302, 35);
            this.panel13.TabIndex = 82;
            // 
            // txtDistName
            // 
            this.txtDistName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDistName.Location = new System.Drawing.Point(0, 5);
            this.txtDistName.Name = "txtDistName";
            this.txtDistName.Size = new System.Drawing.Size(217, 20);
            this.txtDistName.TabIndex = 78;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel14.Location = new System.Drawing.Point(217, 5);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(7, 25);
            this.panel14.TabIndex = 83;
            // 
            // simpleButtonFind
            // 
            this.simpleButtonFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonFind.Enabled = false;
            this.simpleButtonFind.ImageIndex = 9;
            this.simpleButtonFind.ImageList = this.ImageList1;
            this.simpleButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonFind.Location = new System.Drawing.Point(224, 5);
            this.simpleButtonFind.Name = "simpleButtonFind";
            this.simpleButtonFind.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonFind.TabIndex = 79;
            this.simpleButtonFind.ToolTip = "查找";
            this.simpleButtonFind.Click += new System.EventHandler(this.simpleButtonFind_Click);
            // 
            // simpleButtonDrawRange
            // 
            this.simpleButtonDrawRange.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDrawRange.Enabled = false;
            this.simpleButtonDrawRange.ImageIndex = 72;
            this.simpleButtonDrawRange.ImageList = this.ImageList1;
            this.simpleButtonDrawRange.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonDrawRange.Location = new System.Drawing.Point(250, 5);
            this.simpleButtonDrawRange.Name = "simpleButtonDrawRange";
            this.simpleButtonDrawRange.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonDrawRange.TabIndex = 82;
            this.simpleButtonDrawRange.ToolTip = "图上选择";
            this.simpleButtonDrawRange.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButtonLocation
            // 
            this.simpleButtonLocation.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonLocation.ImageIndex = 74;
            this.simpleButtonLocation.ImageList = this.ImageList1;
            this.simpleButtonLocation.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonLocation.Location = new System.Drawing.Point(276, 5);
            this.simpleButtonLocation.Name = "simpleButtonLocation";
            this.simpleButtonLocation.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonLocation.TabIndex = 81;
            this.simpleButtonLocation.ToolTip = "图上定位";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.ImageList = this.ImageList1;
            this.label8.Location = new System.Drawing.Point(5, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(302, 26);
            this.label8.TabIndex = 84;
            this.label8.Text = "区划名称:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.radioGroup1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(302, 30);
            this.panel2.TabIndex = 86;
            this.panel2.Visible = false;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioGroup1.Location = new System.Drawing.Point(0, 0);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "县级区划"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "指定范围")});
            this.radioGroup1.Size = new System.Drawing.Size(224, 30);
            this.radioGroup1.TabIndex = 86;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(7, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(303, 168);
            this.gridControl1.TabIndex = 9;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "目标属性字段";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "匹配源属性字段";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Transparent;
            this.panel10.Controls.Add(this.gridControl1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 170);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(7, 2, 7, 9);
            this.panel10.Size = new System.Drawing.Size(317, 179);
            this.panel10.TabIndex = 14;
            // 
            // navBarGroupControlContainer4
            // 
            this.navBarGroupControlContainer4.Controls.Add(this.panel4);
            this.navBarGroupControlContainer4.Name = "navBarGroupControlContainer4";
            this.navBarGroupControlContainer4.Size = new System.Drawing.Size(312, 112);
            this.navBarGroupControlContainer4.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.gridControl2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(5);
            this.panel4.Size = new System.Drawing.Size(312, 112);
            this.panel4.TabIndex = 16;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(5, 5);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox2});
            this.gridControl2.Size = new System.Drawing.Size(302, 102);
            this.gridControl2.TabIndex = 16;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsCustomization.AllowColumnMoving = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView2.OptionsFilter.AllowFilterEditor = false;
            this.gridView2.OptionsFilter.AllowMRUFilterList = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowIndicator = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "名称";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // navBarGroupControlContainer5
            // 
            this.navBarGroupControlContainer5.Controls.Add(this.panel10);
            this.navBarGroupControlContainer5.Controls.Add(this.panel11);
            this.navBarGroupControlContainer5.Controls.Add(this.panel7);
            this.navBarGroupControlContainer5.Name = "navBarGroupControlContainer5";
            this.navBarGroupControlContainer5.Size = new System.Drawing.Size(317, 349);
            this.navBarGroupControlContainer5.TabIndex = 4;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Transparent;
            this.panel11.Controls.Add(this.label7);
            this.panel11.Controls.Add(this.simpleButtonClear);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 140);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(7, 0, 7, 5);
            this.panel11.Size = new System.Drawing.Size(317, 30);
            this.panel11.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(7, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "属性字段匹配:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // simpleButtonClear
            // 
            this.simpleButtonClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonClear.ImageIndex = 56;
            this.simpleButtonClear.ImageList = this.ImageList1;
            this.simpleButtonClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonClear.Location = new System.Drawing.Point(284, 0);
            this.simpleButtonClear.Name = "simpleButtonClear";
            this.simpleButtonClear.Size = new System.Drawing.Size(26, 25);
            this.simpleButtonClear.TabIndex = 11;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.Controls.Add(this.listBoxDataList);
            this.panel7.Controls.Add(this.panel12);
            this.panel7.Controls.Add(this.buttonEditDataPath);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(7, 2, 7, 7);
            this.panel7.Size = new System.Drawing.Size(317, 140);
            this.panel7.TabIndex = 13;
            // 
            // listBoxDataList
            // 
            this.listBoxDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDataList.Location = new System.Drawing.Point(7, 75);
            this.listBoxDataList.Name = "listBoxDataList";
            this.listBoxDataList.Size = new System.Drawing.Size(303, 58);
            this.listBoxDataList.TabIndex = 12;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Transparent;
            this.panel12.Controls.Add(this.simpleButtonAdd);
            this.panel12.Controls.Add(this.simpleButtonRemove);
            this.panel12.Controls.Add(this.label9);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(7, 45);
            this.panel12.Name = "panel12";
            this.panel12.Padding = new System.Windows.Forms.Padding(0, 2, 5, 2);
            this.panel12.Size = new System.Drawing.Size(303, 30);
            this.panel12.TabIndex = 13;
            // 
            // simpleButtonAdd
            // 
            this.simpleButtonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonAdd.ImageIndex = 55;
            this.simpleButtonAdd.ImageList = this.ImageList1;
            this.simpleButtonAdd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonAdd.Location = new System.Drawing.Point(246, 2);
            this.simpleButtonAdd.Name = "simpleButtonAdd";
            this.simpleButtonAdd.Size = new System.Drawing.Size(26, 26);
            this.simpleButtonAdd.TabIndex = 10;
            this.simpleButtonAdd.ToolTip = "添加";
            // 
            // simpleButtonRemove
            // 
            this.simpleButtonRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonRemove.ImageIndex = 57;
            this.simpleButtonRemove.ImageList = this.ImageList1;
            this.simpleButtonRemove.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.simpleButtonRemove.Location = new System.Drawing.Point(272, 2);
            this.simpleButtonRemove.Name = "simpleButtonRemove";
            this.simpleButtonRemove.Size = new System.Drawing.Size(26, 26);
            this.simpleButtonRemove.TabIndex = 9;
            this.simpleButtonRemove.ToolTip = "移除";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.ImageList = this.ImageList1;
            this.label9.Location = new System.Drawing.Point(0, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 26);
            this.label9.TabIndex = 8;
            this.label9.Text = "导入数据列表:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonEditDataPath
            // 
            this.buttonEditDataPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEditDataPath.Location = new System.Drawing.Point(7, 25);
            this.buttonEditDataPath.Name = "buttonEditDataPath";
            this.buttonEditDataPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditDataPath.Size = new System.Drawing.Size(303, 20);
            this.buttonEditDataPath.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(7, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(303, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "导入数据路径:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.ContentButtonHint = null;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer3);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer4);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer5);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1,
            this.navBarGroup2,
            this.navBarGroup3,
            this.navBarGroup4,
            this.navBarGroup5});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 279;
            this.navBarControl1.Size = new System.Drawing.Size(320, 636);
            this.navBarControl1.SmallImages = this.ImageList1;
            this.navBarControl1.StoreDefaultPaintStyleName = true;
            this.navBarControl1.TabIndex = 28;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "设计类型";
            this.navBarGroup1.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupClientHeight = 154;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.SmallImageIndex = 28;
            // 
            // navBarGroupControlContainer3
            // 
            this.navBarGroupControlContainer3.Controls.Add(this.cListBoxLayer);
            this.navBarGroupControlContainer3.Name = "navBarGroupControlContainer3";
            this.navBarGroupControlContainer3.Padding = new System.Windows.Forms.Padding(5);
            this.navBarGroupControlContainer3.Size = new System.Drawing.Size(317, 92);
            this.navBarGroupControlContainer3.TabIndex = 2;
            // 
            // cListBoxLayer
            // 
            this.cListBoxLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cListBoxLayer.Location = new System.Drawing.Point(5, 5);
            this.cListBoxLayer.MultiColumn = true;
            this.cListBoxLayer.Name = "cListBoxLayer";
            this.cListBoxLayer.Size = new System.Drawing.Size(307, 82);
            this.cListBoxLayer.TabIndex = 0;
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "设定图层";
            this.navBarGroup3.ControlContainer = this.navBarGroupControlContainer3;
            this.navBarGroup3.Expanded = true;
            this.navBarGroup3.GroupClientHeight = 93;
            this.navBarGroup3.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup3.Name = "navBarGroup3";
            this.navBarGroup3.SmallImageIndex = 3;
            this.navBarGroup3.Visible = false;
            // 
            // navBarGroup4
            // 
            this.navBarGroup4.Caption = "任务名称";
            this.navBarGroup4.ControlContainer = this.navBarGroupControlContainer4;
            this.navBarGroup4.Expanded = true;
            this.navBarGroup4.GroupClientHeight = 116;
            this.navBarGroup4.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup4.Name = "navBarGroup4";
            this.navBarGroup4.SmallImageIndex = 6;
            // 
            // navBarGroup5
            // 
            this.navBarGroup5.Caption = "导入数据";
            this.navBarGroup5.ControlContainer = this.navBarGroupControlContainer5;
            this.navBarGroup5.Expanded = true;
            this.navBarGroup5.GroupClientHeight = 350;
            this.navBarGroup5.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup5.Name = "navBarGroup5";
            this.navBarGroup5.SmallImageIndex = 4;
            this.navBarGroup5.Visible = false;
            // 
            // ImageList2
            // 
            this.ImageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList2.ImageStream")));
            this.ImageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList2.Images.SetKeyName(0, "");
            // 
            // simpleButtonCreate
            // 
            this.simpleButtonCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonCreate.ImageIndex = 26;
            this.simpleButtonCreate.ImageList = this.ImageList1;
            this.simpleButtonCreate.Location = new System.Drawing.Point(235, 0);
            this.simpleButtonCreate.Name = "simpleButtonCreate";
            this.simpleButtonCreate.Size = new System.Drawing.Size(71, 28);
            this.simpleButtonCreate.TabIndex = 9;
            this.simpleButtonCreate.Text = "生成";
            this.simpleButtonCreate.Click += new System.EventHandler(this.simpleButtonCreate_Click);
            // 
            // labelprogress
            // 
            this.labelprogress.BackColor = System.Drawing.Color.Transparent;
            this.labelprogress.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelprogress.Location = new System.Drawing.Point(0, 0);
            this.labelprogress.Name = "labelprogress";
            this.labelprogress.Size = new System.Drawing.Size(167, 28);
            this.labelprogress.TabIndex = 8;
            this.labelprogress.Text = "生成进度:";
            this.labelprogress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelprogress.Visible = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.labelprogress);
            this.panel6.Controls.Add(this.simpleButtonCreate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(7, 7);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(306, 28);
            this.panel6.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.progressBar);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 636);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(7, 7, 7, 9);
            this.panel5.Size = new System.Drawing.Size(320, 37);
            this.panel5.TabIndex = 26;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(7, 7);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(306, 21);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            // 
            // UserControlTaskCreate2
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.navBarControl1);
            this.Controls.Add(this.panel5);
            this.Name = "UserControlTaskCreate2";
            this.Size = new System.Drawing.Size(320, 673);
            this.navBarGroupControlContainer1.ResumeLayout(false);
            this.panelKind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxKind)).EndInit();
            this.navBarGroupControlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDist)).EndInit();
            this.panel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDistName.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.panel10.ResumeLayout(false);
            this.navBarGroupControlContainer4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            this.navBarGroupControlContainer5.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDataList)).EndInit();
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditDataPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            this.navBarGroupControlContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cListBoxLayer)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private void InitialKindList()
        {
            try
            {
               // if (this.mKindTable != null)
                {
                    this.cListBoxKind.Items.Clear();
                    this.cListBoxKind2.Items.Clear();
                    this.cListBoxKind2.Visible = false;
                    this.cListBoxKind3.Items.Clear();
                    this.cListBoxKind3.Visible = false;
                 //   DataTable dataTable = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where ( code like '%0000' and kind='" + this.mKindCode + "') ");
                    IList<T_DESIGNKIND_Mid> lst = PM.FindTreeByKindCode(mKindCode);

                    
                    string str = "";
                    for (int i = 0; i < lst.Count; i++)
                    {
                        this.mSelected = true;
                        this.cListBoxKind.Items.Add(lst[i].name);
                        this.mSelected = false;
                    }
                    if (this.cListBoxKind.Items.Count > 0)
                    {
                        this.mSelected = true;
                        this.cListBoxKind.SelectedIndex = 0;
                        this.cListBoxKind.Items[0].CheckState = CheckState.Checked;
                        this.mSelected = false;
                        this.cListBoxKind2.Visible = true;
                    }
                    if (this.cListBoxKind.Items.Count == 0)
                    {
                        this.cListBoxKind2.Visible = false;
                        this.cListBoxKind3.Visible = false;
                    }
                    else
                    {
                        this.cListBoxKind.Tag = lst;
                        str = lst[this.cListBoxKind.SelectedIndex].code.Substring(0, 2);
                        this.mEditKindCode = lst[this.cListBoxKind.SelectedIndex].code;
                      //  DataTable table2 = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where ( code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )='00' and kind='" + this.mKindCode + "')");
                        IList<T_DESIGNKIND_Mid> lst2 = PM.FindSecondTreeByKindCode(mEditKindCode);//PM.FindDKBySql("code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )='00' and kind='" + this.mKindCode + "'");

                        if (lst2 != null)
                        {
                            for (int j = 0; j < lst2.Count; j++)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.Items.Add(lst2[j].name);
                                this.mSelected = false;
                            }
                            if (this.cListBoxKind2.Items.Count > 0)
                            {
                                this.mSelected = true;
                                this.cListBoxKind2.SelectedIndex = 0;
                                this.cListBoxKind2.Items[0].CheckState = CheckState.Checked;
                                this.mSelected = false;
                                this.cListBoxKind3.Visible = true;
                            }
                            if (this.cListBoxKind2.Items.Count == 0)
                            {
                                this.cListBoxKind.Width = this.panelKind.Width - 4;
                                this.cListBoxKind2.Visible = false;
                                this.cListBoxKind3.Visible = false;
                            }
                            else
                            {
                                this.cListBoxKind2.Tag = lst2;
                                str = lst2[this.cListBoxKind2.SelectedIndex].code.Substring(0, 4);
                                this.mEditKindCode = lst2[this.cListBoxKind2.SelectedIndex].code;
                              //  DataTable table3 = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where (code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )<>'00' and kind='" + this.mKindCode + "')");
                                IList<T_DESIGNKIND_Mid> lst3 = PM.FindThirdTreeByKindCode(mEditKindCode);// PM.FindDKBySql("code like '" + str + "%' and right(code ,4 )<>'0000' and right(code ,2 )<>'00' and kind='" + this.mKindCode + "'");


                                if (lst3 != null)
                                {
                                    for (int k = 0; k < lst3.Count; k++)
                                    {
                                        this.mSelected = true;
                                        this.cListBoxKind3.Items.Add(lst3[k].name);
                                        this.mSelected = false;
                                    }
                                    if (this.cListBoxKind3.Items.Count > 0)
                                    {
                                        this.mSelected = true;
                                        this.cListBoxKind3.SelectedIndex = 0;
                                        this.cListBoxKind3.Items[0].CheckState = CheckState.Checked;
                                        this.mSelected = false;
                                        this.mEditKindCode = lst3[this.cListBoxKind3.SelectedIndex].code;
                                    }
                                    if (this.cListBoxKind3.Items.Count == 0)
                                    {
                                        this.cListBoxKind.Width = (this.panelKind.Width / 2) - 4;
                                        this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                        this.cListBoxKind2.Visible = true;
                                        this.cListBoxKind3.Visible = false;
                                    }
                                    else
                                    {
                                        this.cListBoxKind.Width = (this.panelKind.Width / 3) - 6;
                                        this.cListBoxKind2.Width = this.cListBoxKind.Width;
                                        this.cListBoxKind3.Width = this.cListBoxKind.Width;
                                        this.cListBoxKind2.Visible = true;
                                        this.cListBoxKind3.Visible = true;
                                    }
                                    this.cListBoxKind3.Tag = lst3;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate2", "InitialKindList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitialTreeList()
        {
            try
            {
                this.mRangeList = new ArrayList();
                TreeListNode node = null;
                TreeListNode parentNode = null;
                TreeListNode node3 = null;
                TreeListNode node4 = null;
                this.tList.ClearNodes();
                this.tList.OptionsView.ShowRoot = true;
                this.tList.SelectImageList = null;
                this.tList.StateImageList = null;
                this.tList.OptionsView.ShowButtons = true;
                this.tList.TreeLineStyle = LineStyle.None;
                this.tList.RowHeight = 20;
                this.tList.OptionsBehavior.AutoPopulateColumns = true;
                this.m_CountyLayer.FeatureClass.FeatureCount(null);
                IFeatureCursor cursor = this.m_CountyLayer.FeatureClass.Search(null, false);
                IFeature feature = cursor.NextFeature();
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldCode");
                int index = feature.Fields.FindField(configValue);
                string str2 = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode");
                while (feature != null)
                {
                    IQueryFilter queryFilter = new QueryFilterClass {
                        WhereClause = str2 + "='" + feature.get_Value(index).ToString() + "'"
                    };
                    ICursor cursor2 = this.m_CountyTable.Search(queryFilter, false);
                    IRow row = cursor2.NextRow();
                    int num2 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode"));
                    int num3 = row.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName"));
                    while (row != null)
                    {
                        if (row.get_Value(num2).ToString() == feature.get_Value(index).ToString())
                        {
                            node3 = this.tList.AppendNode(row.get_Value(num3).ToString(), node4);
                            node3.ImageIndex = -1;
                            node3.StateImageIndex = -1;
                            node3.SelectImageIndex = -1;
                            node3.SetValue(0, row.get_Value(num3).ToString());
                            node3.Tag = row.get_Value(num2).ToString();
                            this.mRangeList.Add(row.get_Value(num2).ToString());
                            IQueryFilter filter2 = new QueryFilterClass {
                                WhereClause = str2 + " like '" + row.get_Value(num2).ToString() + "%'"
                            };
                            ICursor cursor3 = this.m_TownTable.Search(filter2, false);
                            for (IRow row2 = cursor3.NextRow(); row2 != null; row2 = cursor3.NextRow())
                            {
                                parentNode = this.tList.AppendNode(row2.get_Value(num3).ToString(), node3);
                                parentNode.ImageIndex = -1;
                                parentNode.StateImageIndex = -1;
                                parentNode.SelectImageIndex = -1;
                                parentNode.SetValue(0, row2.get_Value(num3).ToString());
                                parentNode.Tag = row2.get_Value(num2).ToString();
                                parentNode.Expanded = false;
                                if (!this.mIsBatch)
                                {
                                    IQueryFilter filter3 = new QueryFilterClass {
                                        WhereClause = str2 + " like '" + row2.get_Value(num2).ToString() + "%'"
                                    };
                                    ICursor cursor4 = this.m_VillageTable.Search(filter3, false);
                                    for (IRow row3 = cursor3.NextRow(); row3 != null; row3 = cursor4.NextRow())
                                    {
                                        node = this.tList.AppendNode(row3.get_Value(num3).ToString(), parentNode);
                                        node.ImageIndex = -1;
                                        node.StateImageIndex = -1;
                                        node.SelectImageIndex = -1;
                                        node.SetValue(0, row3.get_Value(num3).ToString());
                                        node.Tag = row3.get_Value(num2).ToString();
                                        node.Expanded = false;
                                    }
                                }
                            }
                        }
                        row = cursor2.NextRow();
                    }
                    feature = cursor.NextFeature();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate2", "InitialList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private bool InitialValue()
        {
            try
            {
                
                if (this.mEditKind == "造林")
                {
                    this.mKindCode = "1";
                }
                else if (this.mEditKind == "采伐")
                {
                    this.mKindCode = "2";
                }
                else
                {
                    this.mKindCode = "";
                }
               // this.mKindTable = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_DesignKind where kind='" + this.mKindCode + "'");
                this.InitialKindList();
                IMap focusMap = this.mHookHelper.FocusMap;
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyLayerName");
                this.m_CountyLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_CountyLayer == null)
                {
                    return false;
                }
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("TownLayerName");
                this.m_TownLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_TownLayer == null)
                {
                    return false;
                }
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("VillageLayerName");
                this.m_VillageLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                if (this.m_VillageLayer == null)
                {
                    return false;
                }
                string sSourceFile = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditDataPath");
                IFeatureWorkspace featureWorkspace = null;
                if (sSourceFile.Contains(".gdb") || sSourceFile.Contains(".GDB"))
                {
                    featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSFileGDBWorkspaceFactory);
                }
                else if (sSourceFile.Contains(".mdb") || sSourceFile.Contains(".MDB"))
                {
                    featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSAccessWorkspaceFactory);
                }
                if (featureWorkspace == null)
                {
                    return false;
                }
                string name = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableName");
                this.m_CountyTable = featureWorkspace.OpenTable(name);
                if (this.m_CountyTable == null)
                {
                    return false;
                }
                name = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableName");
                this.m_TownTable = featureWorkspace.OpenTable(name);
                if (this.m_TownTable == null)
                {
                    return false;
                }
                name = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableName");
                this.m_VillageTable = featureWorkspace.OpenTable(name);
                if (this.m_VillageTable == null)
                {
                    return false;
                }
                if (this.mEditKind == "小班")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLayer");
                }
                else if (this.mEditKind == "造林")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditZLLayer");
                }
                else if (this.mEditKind == "采伐")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditCFLayer");
                }
                else if (this.mEditKind == "林改")
                {
                    configValue = UtilFactory.GetConfigOpt().GetConfigValue("EditLGLayer");
                }
                this.m_EditLayer = GISFunFactory.LayerFun.FindFeatureLayer(focusMap as IBasicMap, configValue, true);
                this.mRangeList = new ArrayList();
                 this.m_taskList=  PM.TaskService.FindBySql("createtime='" + DateTime.Now.ToLocalTime().ToString() + "'");
                //this.mTaskTable = TaskManageClass.GetDataTable(this.mDBAccess, "select * from T_EditTask where createtime='" + DateTime.Now.ToLocalTime().ToString() + "'");
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlTaskCreate2", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private void listBoxDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelprogress.Visible = false;
                this.txtDistName.Text = this.listBoxDist.SelectedItem.ToString();
                m_taskList[0].distcode = this.mRangeList[this.listBoxDist.SelectedIndex].ToString();
                string str = "";
                if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                {
                    str = this.cListBoxKind.SelectedItem.ToString();
                }
                if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                {
                    str = str + this.cListBoxKind2.SelectedItem.ToString();
                }
                if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                {
                    str = str + this.cListBoxKind3.SelectedItem.ToString();
                }
                m_taskList[0].taskname = string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str, "作业设计" });
                this.gridView2.RefreshData();
            }
            catch (Exception)
            {
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelprogress.Visible = false;
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.simpleButtonFind.Enabled = false;
                this.simpleButtonDrawRange.Enabled = false;
            }
            else
            {
                this.simpleButtonFind.Enabled = true;
                this.simpleButtonDrawRange.Enabled = true;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
        }

        private void simpleButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                this.labelprogress.Text = "开始生成操作";
                this.labelprogress.Visible = true;
                bool flag = false;
                string sSourceFile = UtilFactory.GetConfigOpt().RootPath + @"\" + UtilFactory.GetConfigOpt().GetConfigValue("EditDataPath");
                IFeatureWorkspace featureWorkspace = null;
                if (sSourceFile.Contains(".gdb") || sSourceFile.Contains(".GDB"))
                {
                    featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSFileGDBWorkspaceFactory);
                }
                else if (sSourceFile.Contains(".mdb") || sSourceFile.Contains(".MDB"))
                {
                    featureWorkspace = GISFunFactory.WorkspaceFun.GetFeatureWorkspace(sSourceFile, WorkspaceSource.esriWSAccessWorkspaceFactory);
                }
                IWorkspace workspace2 = featureWorkspace as IWorkspace;
                IDataset dataset = workspace2 as IDataset;
                ITable table = null;
                IDataset dataset2 = workspace2 as IDataset;
                string str2 = "";
                string str3 = "";
                string str4 = "";
                string str5 = "";
                if (this.mEditKind == "小班")
                {
                    str2 = "XiaoBan";
                }
                else if (this.mEditKind == "造林")
                {
                    str2 = "AFF_V";
                    str3 = "AFF_V_P";
                    str4 = "T_AFF";
                    str5 = "AFF_Relation";
                }
                else if (this.mEditKind == "采伐")
                {
                    str2 = "HAR_V";
                    str3 = "F_HAR_V_P";
                    str4 = "T_HAR";
                    str5 = "F_HAR_Relation";
                }
                else if (this.mEditKind == "林改")
                {
                    str2 = "LinGai";
                    str3 = "LG";
                }
                dataset = featureWorkspace.OpenFeatureDataset(str2 + "_Templ");
                table = featureWorkspace.OpenTable(str4 + "_Templ");
                IGeoDataset dataset3 = dataset as IGeoDataset;
                try
                {
                    dataset2 = featureWorkspace.OpenFeatureDataset(str2 + "_" + DateTime.Today.Year);
                }
                catch (Exception)
                {
                    dataset2 = featureWorkspace.CreateFeatureDataset(str2 + "_" + DateTime.Today.Year, dataset3.SpatialReference);
                }
                IFeatureDataset dataset4 = dataset2 as IFeatureDataset;
                IEnumDataset subsets = dataset4.Subsets;
                IDataset dataset6 = subsets.Next();
                IFeatureClass class2 = null;
                IFeatureClass class3 = null;
                IFeatureClass class4 = null;
                while (dataset6 != null)
                {
                    if (dataset6.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        class2 = dataset6 as IFeatureClass;
                        if (dataset6.Name.Contains(str3 + "_" + DateTime.Today.Year))
                        {
                            class3 = class2;
                            break;
                        }
                    }
                    dataset6 = subsets.Next();
                }
                if (class3 == null)
                {
                    subsets = dataset.Subsets;
                    for (dataset6 = subsets.Next(); dataset6 != null; dataset6 = subsets.Next())
                    {
                        if (dataset6.Type == esriDatasetType.esriDTFeatureClass)
                        {
                            class2 = dataset6 as IFeatureClass;
                            if (dataset6.Name.Contains("Templ"))
                            {
                                class4 = class2;
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (class4 != null)
                    {
                        IFeatureClassDescription description = new FeatureClassDescriptionClass();
                        IObjectClassDescription description2 = (IObjectClassDescription) description;
                        class3 = dataset4.CreateFeatureClass(str3 + "_" + DateTime.Today.Year, class4.Fields, description2.InstanceCLSID, description2.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "Shape", "");
                        UID cLSID = new UIDClass {
                            Value = "esriGeoDatabase.Object"
                        };
                        featureWorkspace.CreateTable(str4 + "_" + DateTime.Today.Year, table.Fields, cLSID, null, "");
                        featureWorkspace.OpenTable(str5 + "_" + DateTime.Today.Year);
                        IRelationshipClassContainer container = (IRelationshipClassContainer) dataset4;
                        IObjectClass originClass = featureWorkspace.OpenTable(str3 + "_" + DateTime.Today.Year) as IObjectClass;
                        IObjectClass destinationClass = featureWorkspace.OpenTable(str4 + "_" + DateTime.Today.Year) as IObjectClass;
                        container.CreateRelationshipClass(str3 + "_" + DateTime.Today.Year, originClass, destinationClass, str4 + "_" + DateTime.Today.Year, str3 + "_" + DateTime.Today.Year, esriRelCardinality.esriRelCardinalityOneToMany, esriRelNotification.esriRelNotificationNone, false, false, null, "UUID", "", "UUID", "");
                        flag = true;
                    }
                }
                if (class3 != null)
                {
                    for (int i = 0; i < m_taskList.Count; i++)
                    {
                        string str6 = str3 + "_" + DateTime.Today.Year;
                        //this.mTaskTable.Rows[i]["ID"] = UtilFactory.GetDBOpt().GetMaxID(this.mDBAccess, "T_EditTask");
                        m_taskList[i].layername = str6;
                        m_taskList[i].tablename = str4 + "_" + DateTime.Today.Year;
                        m_taskList[i].edittime = DateTime.Now.ToLocalTime().ToString();
                        m_taskList[i] .datasetname= str2 + "_" + DateTime.Today.Year;
                        bool flag1 = this.mEditKind == "采伐";
                        string sCmdText = null;
                        string str8 = "T_EditTask";
                        //object obj2 = "INSERT INTO " + str8 + "(taskname,taskkind,distcode,taskstate,taskyear,createtime,edittime,datasetname,layername,tablename)";
                        //sCmdText = string.Concat(new object[] { 
                        //    obj2, " VALUES('",m_taskList[i]["taskname"], "','", this.mTaskTable.Rows[i]["taskkind"], "','", this.mTaskTable.Rows[i]["distcode"], "','", this.mTaskTable.Rows[i]["taskstate"], "','", this.mTaskTable.Rows[i]["taskyear"], "','", this.mTaskTable.Rows[i]["createtime"], "','", this.mTaskTable.Rows[i]["edittime"], "','",
                        //    this.mTaskTable.Rows[i]["datasetname"], "','", this.mTaskTable.Rows[i]["layername"], "','", this.mTaskTable.Rows[i]["tablename"], "')"
                        //});
                        //this.mDBAccess.ExecuteScalar(sCmdText);
                        PM.TaskService.Add(m_taskList[i]);
                        if (true)
                        {
                            this.labelprogress.Text = "生成" + this.mEditKind + "作业图层成功";
                            this.labelprogress.Visible = true;
                        }
                        else
                        {
                            this.labelprogress.Text = "生成" + this.mEditKind + "作业图层失败";
                            this.labelprogress.Visible = true;
                        }
                    }
                }
                dataset4 = null;
                featureWorkspace = null;
            }
            catch (Exception)
            {
                this.labelprogress.Text = "生成" + this.mEditKind + "作业图层失败";
                this.labelprogress.Visible = true;
            }
        }

        private void simpleButtonFind_Click(object sender, EventArgs e)
        {
            this.labelprogress.Visible = false;
        }

        private void tList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.labelprogress.Visible = false;
                if (this.tList.Selection.Count >= 1)
                {
                    TreeListNode node = this.tList.Selection[0];
                    if (node.Nodes.Count > 0)
                    {
                        string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldCode");
                        IQueryFilter queryFilter = new QueryFilterClass {
                            WhereClause = configValue + "='" + node.Tag.ToString() + "'"
                        };
                        this.m_CountyLayer.Search(queryFilter, false).NextFeature();
                        this.txtDistName.Text = node.GetValue(0).ToString();
                        this.txtDistName.Tag = node.Tag.ToString();
                    }
                    else
                    {
                        string str2 = UtilFactory.GetConfigOpt().GetConfigValue("TownFieldCode");
                        IQueryFilter filter2 = new QueryFilterClass {
                            WhereClause = str2 + "='" + node.Tag.ToString() + "'"
                        };
                        this.m_TownLayer.Search(filter2, false).NextFeature();
                        this.txtDistName.Text = node.GetValue(0).ToString();
                        this.txtDistName.Tag = node.Tag.ToString();
                    }
                    this.txtDistName.Text = node.GetValue(0).ToString();
                    m_taskList[0].distcode = node.Tag.ToString();
                   // this.mTaskTable.Rows[0]["distcode"] = node.Tag.ToString();
                    string str3 = "";
                    if ((this.cListBoxKind.SelectedIndex != -1) && (this.cListBoxKind.Items.Count > 0))
                    {
                        str3 = this.cListBoxKind.SelectedItem.ToString();
                    }
                    if ((this.cListBoxKind2.SelectedIndex != -1) && (this.cListBoxKind2.Items.Count > 0))
                    {
                        str3 = str3 + this.cListBoxKind2.SelectedItem.ToString();
                    }
                    if ((this.cListBoxKind3.SelectedIndex != -1) && (this.cListBoxKind3.Items.Count > 0))
                    {
                        str3 = str3 + this.cListBoxKind3.SelectedItem.ToString();
                    }
                    m_taskList[0].taskname = string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str3, "作业设计" });
                   // this.mTaskTable.Rows[0]["taskname"] = string.Concat(new object[] { this.txtDistName.Text, DateTime.Now.Year, "年", str3, "作业设计" });
                    this.gridView2.RefreshData();
                }
            }
            catch (Exception)
            {
            }
        }

        private void tList_MouseUp(object sender, MouseEventArgs e)
        {
        }

        public bool IsBatch
        {
            get
            {
                return this.mIsBatch;
            }
            set
            {
                this.mIsBatch = value;
                if (this.mHookHelper != null)
                {
                    this.InitialControl();
                }
            }
        }
    }
}

