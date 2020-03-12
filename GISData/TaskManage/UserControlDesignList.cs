namespace TaskManage
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Controls;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using FormBase;
    using FunFactory;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Linq;
    using td.db.mid.sys;
    using td.db.orm;
    using td.db.service.factory;
    using td.db.service.sys;
    using td.forest.task.linker;
    using td.logic.sys;
    using Utilities;

    /// <summary>
    /// 设计列表窗体
    /// </summary>
    public class UserControlDesignList : UserControlBase1
    {
        private SimpleButton ButtonFind;
        private IContainer components;
        private PopupContainerEdit curPopupEdit;
        private DateEdit dateEdit1;
        private DateEdit dateEdit2;
        private GridColumn gridColumn3;
        public GridControl gridControl1;
        public GridView gridView1;
        private ImageList imageList1;
        private Label label1;
        private Label label10;
        private Label label13;
        private Label label14;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelEdit;
        private Label labelInfo;
        private Label labelInfo2;
        private ITable m_CityTable;
        private ITable m_CountyTable;
        private ITable m_TownTable;
        private ITable m_VillageTable;
        private const string mClassName = "TaskManage.UserControlDesignList";
       
        private IFeatureLayer mEditLayer;
        private IFeatureLayer mEditLayer2;
        public T_EDITTASK_ZT_Mid m_curProject;
        private ITable mEditTable;
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private ArrayList mFeatureList;
        private IFeatureWorkspace mFeatureWorkspace;
        private DataTable mFieldTable;
        private IGroupLayer mGroupLayer;
        private IHookHelper mHookHelper;
        private string mKindCode = "";
       // private DataTable mKindTable;
        private IList<T_DESIGNKIND_Mid>  m_kindLst;
        private IMap mMap;
        private ArrayList mNameList;        
        private IList<T_EDITTASK_ZT_Mid> m_projectList;
        private ArrayList mQueryList;
        private bool mSelected;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel32;
        private Panel panel33;
        private Panel panel35;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panelButtons;
        private PanelControl panelControl2;
        private Panel panelEdit;
        private Panel panelKind;
        private Panel panelName;
        internal PopupContainerControl PopupContainerDesi;
        internal PopupContainerControl popupContainerDesi2;
        internal PopupContainerControl popupContainerDist;
        internal PopupContainerControl popupContainerDist2;
        internal PopupContainerEdit popupEditDist;
        internal PopupContainerEdit popupEditKind;
        /// <summary>
        /// 区划范围
        /// </summary>
        internal PopupContainerEdit popupEditQDist;
        /// <summary>
        /// 设计类型
        /// </summary>
        internal PopupContainerEdit popupEditQKind;
        private RepositoryItemComboBox repositoryItemComboBox2;
        private SimpleButton simpleButtonAddNew;
        private SimpleButton simpleButtonDelete;
        private SimpleButton simpleButtonDeleteAll;
        private SimpleButton simpleButtonEdit;
        private SimpleButton simpleButtonEditCancle;
        private SimpleButton simpleButtonEditOK;
        private SimpleButton simpleButtonMore;
        public SimpleButton simpleButtonOpen;
        private SimpleButton simpleButtonReset;
        internal TreeListColumn tcolBase1;
        /// <summary>
        /// 设计名称
        /// </summary>
        private TextEdit textName;
        private TextBox textName2;
        internal TreeList tListDesignKind;
        internal TreeList tListDesignKind2;
        internal TreeList tListDist;
        internal TreeList tListDist2;
        internal TreeListColumn treeListColumn1;
        internal TreeListColumn treeListColumn2;
        internal TreeListColumn treeListColumn3;

        public UserControlDesignList()
        {
            this.InitializeComponent();
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            this.FindProject();
        }

        public void ButtonOpenClick(bool seldist)
        {
            try
            {
                int focusedDataSourceRowIndex = this.gridView1.GetFocusedDataSourceRowIndex();
                if (focusedDataSourceRowIndex != -1)
                {
                    T_EDITTASK_ZT_Mid prjMid= m_projectList[focusedDataSourceRowIndex];
                    if (this.simpleButtonOpen.Text == "打开")
                    {
                        EditTask.TaskID = prjMid.ID;
                        EditTask.TaskName = prjMid.taskname;
                        EditTask.KindCode = prjMid.taskkind;
                        EditTask.TaskState = TaskState2.Edit;
                        this.simpleButtonAddNew.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                        this.simpleButtonDeleteAll.Enabled = false;
                        this.simpleButtonEdit.Enabled = false;
                        if (prjMid .taskstate== "1")
                        {
                            prjMid.taskstate = "2";
                            DBServiceFactory<T_EDITTASK_ZT_Service>.Service.Edit(prjMid);
                        }
                        this.OpenTask(prjMid, seldist);
                        this.simpleButtonOpen.Text = "关闭";
                        this.simpleButtonOpen.ImageIndex = 0x10;
                        this.gridControl1.Enabled = false;
                        this.panel1.Enabled = false;
                        this.panel2.Enabled = false;
                        this.panel3.Enabled = false;
                        this.panel35.Enabled = false;
                        this.panel5.Enabled = false;
                    }
                    else
                    {
                        EditTask.TaskID = 0L;
                        EditTask.TaskName = "采伐";
                        EditTask.KindCode = "02";
                        EditTask.DistCode = EditTask.DistCode.Substring(0, 6);
                        this.simpleButtonOpen.Text = "打开";
                        this.simpleButtonOpen.ImageIndex = 15;
                        this.simpleButtonAddNew.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                        this.simpleButtonDeleteAll.Enabled = true;
                        this.simpleButtonEdit.Enabled = true;
                        this.SetLayerFilter(false);
                        if (this.mEditLayer2 != null)
                        {
                            this.mGroupLayer.Delete(this.mEditLayer2);
                        }
                        this.gridControl1.Enabled = true;
                        this.panel1.Enabled = true;
                        this.panel2.Enabled = true;
                        this.panel3.Enabled = true;
                        this.panel35.Enabled = true;
                        this.panel5.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "ButtonOpenClick", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private bool CreateProject()
        {
            try
            {
              
             
                string taskYear = EditTask.TaskYear;
                T_EDITTASK_ZT_Mid mid = new  T_EDITTASK_ZT_Mid();
                mid.taskname = this.textName2.Text;
                mid.createtime = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");

                mid.edittime = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                mid.datasetname = UtilFactory.GetConfigOpt().GetConfigValue("CaiFaDataset");
                mid .layername= UtilFactory.GetConfigOpt().GetConfigValue("CaiFaLayer") + "_" + taskYear;
                mid.tablename = UtilFactory.GetConfigOpt().GetConfigValue("CaiFaTableName") + "_" + taskYear;
                mid.taskpath = "";
                mid.logiccheckstate = "0";
                mid.bh = "";
                mid.toplogiccheckstate = "0";
                mid.taskstate = "1";
                mid .taskyear= taskYear;
                mid.distcode = this.tListDist.Selection[0].Tag.ToString();
                //this.popupEditKind.Tag.ToString();
                //T_DESIGNKIND_Mid kind = this.popupEditKind.Tag as T_DESIGNKIND_Mid;
                //string str4 = "0" + this.mKindCode + kind.code ;
                mid.taskkind = "0" + this.mKindCode + this.popupEditKind.Tag.ToString();
                return DBServiceFactory<T_EDITTASK_ZT_Service>.Service.Add(mid);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "CreateProject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        private bool DeleteEditLayerFeature(string taskid)
        {
            try
            {
                IWorkspaceEdit editWorkspace = EditTask.EditWorkspace as IWorkspaceEdit;
                if (editWorkspace == null)
                {
                    return false;
                }
                if (taskid == "")
                {
                    editWorkspace.StartEditing(false);
                    editWorkspace.StartEditOperation();
                    IDataset featureClass = this.mEditLayer.FeatureClass as IDataset;
                    featureClass.Workspace.ExecuteSQL("delete from " + featureClass.Name);
                    featureClass = this.mEditTable as IDataset;
                    featureClass.Workspace.ExecuteSQL("delete from " + featureClass.Name);
                    editWorkspace.StopEditOperation();
                    editWorkspace.StopEditing(true);
                    editWorkspace.StartEditing(false);
                    editWorkspace.StartEditOperation();
                    return true;
                }
                GC.Collect();
                IQueryFilter filter = new QueryFilterClass {
                    WhereClause = "TASK_ID='" + taskid + "'"
                };
                IFeatureCursor cursor = this.mEditLayer.FeatureClass.Search(filter, false);
                IFeature feature = cursor.NextFeature();
                editWorkspace.StartEditing(false);
                editWorkspace.StartEditOperation();
                Application.DoEvents();
                while (feature != null)
                {
                    IDataset mEditTable = this.mEditTable as IDataset;
                    string str = "";
                    string[] strArray = UtilFactory.GetConfigOpt().GetConfigValue("CaiFaTableNameD").Split(new char[] { ',' });
                    string[] strArray2 = UtilFactory.GetConfigOpt().GetConfigValue("CaiFaFieldNameD").Split(new char[] { ',' });
                    if ((strArray.Length > 0) && (strArray2.Length > 2))
                    {
                        int index = this.mEditLayer.FeatureClass.Fields.FindField(strArray2[2]);
                        if (index > -1)
                        {
                            if (feature.get_Value(index).ToString() == "")
                            {
                                goto Label_02C4;
                            }
                            str = strArray[0] + "='" + feature.get_Value(index).ToString() + "'";
                        }
                    }
                    for (int i = 1; i < strArray.Length; i++)
                    {
                        int num3 = this.mEditLayer.FeatureClass.Fields.FindField(strArray2[i + 2]);
                        if (num3 > -1)
                        {
                            if (feature.get_Value(num3).ToString() == "")
                            {
                                goto Label_02C4;
                            }
                            str = str + " and " + strArray[i] + "='" + feature.get_Value(num3).ToString() + "'";
                        }
                    }
                    if (str != "")
                    {
                        mEditTable.Workspace.ExecuteSQL("delete from " + mEditTable.Name + " where " + str);
                    }
                    else
                    {
                        mEditTable.Workspace.ExecuteSQL("delete from " + mEditTable.Name);
                    }
                Label_02C4:
                    feature.Delete();
                    feature.Store();
                    feature = cursor.NextFeature();
                }
                editWorkspace.StopEditOperation();
                editWorkspace.StopEditing(true);
                editWorkspace.StartEditing(false);
                editWorkspace.StartEditOperation();
                return true;
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "DeleteEditLayerFeature", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FindProject()
        {
            try
            {
                string str = "(taskkind like '0" + this.mKindCode + "%') ";
                if (this.popupEditQKind.Text != "")
                {
                    string str2 = this.popupEditQKind.Tag.ToString();
                    if (str2.Contains("0000"))
                    {
                        str = "taskkind like '0" + this.mKindCode + str2.Replace("0000", "") + "%'";
                    }
                    else
                    {
                        str = "taskkind like '0" + this.mKindCode + str2 + "'";
                    }
                }
                if (this.popupEditQDist.Text != "")
                {
                    string str3 = this.popupEditQDist.Tag.ToString();
                    if (str != "")
                    {
                        str = str + " and distcode like '" + str3 + "%'";
                    }
                    else
                    {
                        str = "distcode like '" + str3 + "%'";
                    }
                }
                if (this.textName.Text.Trim() != "")
                {
                    string str4 = "taskname like '%" + this.textName.Text.Trim() + "%'";
                    if (str != "")
                    {
                        str = str + " and " + str4;
                    }
                    else
                    {
                        str = str4;
                    }
                }
                //检索该时间段内的项目
                if ((this.dateEdit1.Text != "") || (this.dateEdit2.Text != ""))
                {
                    string str5 = "";
                 
                    string str6 = "";
                    string str7 = "";
                    if (this.dateEdit1.Text.Trim() != "")
                    {
                        str6 = DateTime.Parse(this.dateEdit1.Text + " 00:00:00").ToString("yyyyMMddHHmmss");
                        str5 = " replace(replace(replace(CONVERT(varchar, cast(createtime as datetime), 120 ),'-',''),' ',''),':','')>'" + str6 + "'";
                    }
                    if (this.dateEdit2.Text.Trim() != "")
                    {
                        str7 = DateTime.Parse(this.dateEdit2.Text + " 23:59:59").ToString("yyyyMMddHHmmss");
                        if (str5 != "")
                        {
                            str5 = str5 + " and replace(replace(replace(CONVERT(varchar, cast(edittime as datetime), 120 ),'-',''),' ',''),':','')<'" + str7 + "'";
                        }
                        else
                        {
                            str5 = "replace(replace(replace(CONVERT(varchar, cast(edittime as datetime), 120 ),'-',''),' ',''),':','')<'" + str7 + "'";
                        }
                    }
                    
                    if (str != "")
                    {
                        str = str + " and " + str5;
                    }
                    else
                    {
                        str = str5;
                    }
                }
                m_projectList= DBServiceFactory<T_EDITTASK_ZT_Service>.Service.FindBySql(str);             
                if (m_projectList.Count == 0)
                {
                    this.simpleButtonOpen.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonDeleteAll.Enabled = false;
                    this.simpleButtonAddNew.Enabled = true;
                }
                else
                {
                    this.simpleButtonOpen.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonDeleteAll.Enabled = true;
                    this.simpleButtonAddNew.Enabled = true;
                }
                this.InitialList();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "FindProject", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
        private T_EDITTASK_ZT_Service TaskService
        {
            get { return DBServiceFactory<T_EDITTASK_ZT_Service>.Service; }
        }
        private T_DESIGNKIND_Service DesignService
        {
            get { return DBServiceFactory<T_DESIGNKIND_Service>.Service; }
        }
        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int focusedRowHandle = this.gridView1.FocusedRowHandle;
            if (((focusedRowHandle != -1) && this.panelEdit.Visible) && !this.labelEdit.Text.Contains("新增"))
            {
                T_EDITTASK_ZT_Mid prjMid = m_projectList[focusedRowHandle];
                string str = prjMid.taskkind;
                str = str.Substring(2, str.Length - 2);
                T_DESIGNKIND_Mid dMid=DesignService.FindOneBySql("code = '" + str + "' and kind='" + this.mKindCode + "'");
                if (null != dMid)
                {
                    this.popupEditKind.Text = dMid.name;
                    this.popupEditKind.Tag = dMid.code;
                    IQueryFilter queryFilter = new QueryFilterClass {
                        WhereClause = " ccode = '" + prjMid.distcode + "'"
                    };
                    if (this.m_CityTable.RowCount(queryFilter) > 0)
                    {
                        IRow row = this.m_CityTable.Search(queryFilter, false).NextRow();
                        int index = row.Fields.FindField("cname");
                        this.popupEditDist.Text = row.get_Value(index).ToString();
                    }
                    this.popupEditDist.Tag = prjMid.distcode;
                    this.textName2.Text = prjMid.taskname;
                }
            }
        }

        private void InitialDistList(TreeList tList)
        {
            try
            {
                TreeListNode node = null;
                TreeListNode parentNode = null;
                TreeListNode node3 = null;
                TreeListNode node4 = null;
                TreeListNode node5 = null;
                tList.ClearNodes();
                tList.OptionsView.ShowRoot = true;
                tList.SelectImageList = null;
                tList.OptionsView.ShowButtons = true;
                tList.TreeLineStyle = LineStyle.None;
                tList.RowHeight = 20;
                tList.OptionsBehavior.AutoPopulateColumns = true;
                tList.OptionsView.AutoWidth = true;
                if (tList.Columns.Count == 2)
                {
                    tList.Columns[1].Visible = false;
                }
                IQueryFilter queryFilter = new QueryFilterClass {
                    WhereClause = UtilFactory.GetConfigOpt().GetConfigValue("CityCodeTableWhereStr")
                };
                if (this.m_CityTable.RowCount(queryFilter) == 0)
                {
                    UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName");
                    string configValue = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode");
                    IQueryFilter filter2 = new QueryFilterClass {
                        WhereClause = "CINDEX='103'"
                    };
                    ICursor cursor = this.m_CountyTable.Search(filter2, false);
                    IRow row = cursor.NextRow();
                    int index = row.Fields.FindField(configValue);
                    while (row != null)
                    {
                        IQueryFilter filter3 = new QueryFilterClass {
                            WhereClause = configValue + "='" + row.get_Value(index).ToString() + "' and CINDEX='103'"
                        };
                        ICursor cursor2 = this.m_CountyTable.Search(filter3, false);
                        IRow row2 = cursor2.NextRow();
                        int num3 = row2.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode"));
                        int num4 = row2.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName"));
                        while (row2 != null)
                        {
                            if (row2.get_Value(num3).ToString() == row.get_Value(index).ToString())
                            {
                                node3 = tList.AppendNode(row2.get_Value(num4).ToString(), node5);
                                node3.SetValue(0, row2.get_Value(num4).ToString());
                                node3.Tag = row2.get_Value(num3).ToString();
                                IQueryFilter filter4 = new QueryFilterClass {
                                    WhereClause = configValue + " like '" + row2.get_Value(num3).ToString() + "%' and CINDEX='104'"
                                };
                                ICursor cursor3 = this.m_TownTable.Search(filter4, true);
                                for (IRow row3 = cursor3.NextRow(); row3 != null; row3 = cursor3.NextRow())
                                {
                                    parentNode = tList.AppendNode(row3.get_Value(num4).ToString(), node3);
                                    parentNode.SetValue(0, row3.get_Value(num4).ToString());
                                    parentNode.Tag = row3.get_Value(num3).ToString();
                                    parentNode.Expanded = false;
                                    IQueryFilter filter5 = new QueryFilterClass {
                                        WhereClause = configValue + " like '" + row3.get_Value(num3).ToString() + "%' and CINDEX='105'"
                                    };
                                    ICursor cursor4 = this.m_VillageTable.Search(filter5, false);
                                    for (IRow row4 = cursor4.NextRow(); row4 != null; row4 = cursor4.NextRow())
                                    {
                                        node = tList.AppendNode(row4.get_Value(num4).ToString(), parentNode);
                                        node.SetValue(0, row4.get_Value(num4).ToString());
                                        node.Tag = row4.get_Value(num3).ToString();
                                        node.Expanded = false;
                                    }
                                }
                                node3.ExpandAll();
                            }
                            row2 = cursor2.NextRow();
                        }
                        row = cursor.NextRow();
                    }
                }
                else
                {
                    string name = UtilFactory.GetConfigOpt().GetConfigValue("CityCodeTableFieldName");
                    string str3 = UtilFactory.GetConfigOpt().GetConfigValue("CityCodeTableFieldCode");
                    ICursor cursor5 = this.m_CityTable.Search(queryFilter, false);
                    IRow row5 = cursor5.NextRow();
                    int num5 = row5.Fields.FindField(str3);
                    int num6 = row5.Fields.FindField(name);
                    while (row5 != null)
                    {
                        node3 = tList.AppendNode(row5.get_Value(num6).ToString(), node5);
                        node3.SetValue(0, row5.get_Value(num6).ToString());
                        node3.Tag = row5.get_Value(num5).ToString();
                        IQueryFilter filter6 = new QueryFilterClass();
                        string str4 = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableWhereStr");
                        if (str4 != "")
                        {
                            filter6.WhereClause = str3 + " like '" + row5.get_Value(num5).ToString() + "%' and " + str4;
                        }
                        else
                        {
                            filter6.WhereClause = str3 + " like '" + row5.get_Value(num5).ToString() + "%' ";
                        }
                        ICursor cursor6 = this.m_CountyTable.Search(filter6, false);
                        IRow row6 = cursor6.NextRow();
                        int num7 = row6.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldCode"));
                        int num8 = row6.Fields.FindField(UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableFieldName"));
                        while (row6 != null)
                        {
                            node4 = tList.AppendNode(row6.get_Value(num8).ToString(), node3);
                            node4.SetValue(0, row6.get_Value(num8).ToString());
                            node4.Tag = row6.get_Value(num7).ToString();
                            IQueryFilter filter7 = new QueryFilterClass {
                                WhereClause = str3 + " like '" + row6.get_Value(num7).ToString() + "%' and CINDEX='104'"
                            };
                            ICursor cursor7 = this.m_TownTable.Search(filter7, true);
                            for (IRow row7 = cursor7.NextRow(); row7 != null; row7 = cursor7.NextRow())
                            {
                                parentNode = tList.AppendNode(row7.get_Value(num8).ToString(), node4);
                                parentNode.SetValue(0, row7.get_Value(num8).ToString());
                                parentNode.Tag = row7.get_Value(num7).ToString();
                                parentNode.Expanded = true;
                                IQueryFilter filter8 = new QueryFilterClass {
                                    WhereClause = str3 + " like '" + row7.get_Value(num7).ToString() + "%' and CINDEX='105'"
                                };
                                ICursor cursor8 = this.m_VillageTable.Search(filter8, false);
                                for (IRow row8 = cursor8.NextRow(); row8 != null; row8 = cursor8.NextRow())
                                {
                                    node = tList.AppendNode(row8.get_Value(num8).ToString(), parentNode);
                                    node.SetValue(0, row8.get_Value(num8).ToString());
                                    node.Tag = row8.get_Value(num7).ToString();
                                    node.Expanded = false;
                                }
                            }
                            node4.Expanded = true;
                            row6 = cursor6.NextRow();
                        }
                        node3.Expanded = true;
                        row5 = cursor5.NextRow();
                    }
                }
                tList.Selection.Clear();
                tList.FocusedNode = null;
                tList.Refresh();
                tList.OptionsSelection.Reset();
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "InitialDistList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDesignList));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.labelInfo = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.simpleButtonOpen = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleButtonAddNew = new DevExpress.XtraEditors.SimpleButton();
            this.label14 = new System.Windows.Forms.Label();
            this.simpleButtonEdit = new DevExpress.XtraEditors.SimpleButton();
            this.label13 = new System.Windows.Forms.Label();
            this.simpleButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButtonDeleteAll = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel35 = new System.Windows.Forms.Panel();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.label31 = new System.Windows.Forms.Label();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.label30 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButtonReset = new DevExpress.XtraEditors.SimpleButton();
            this.panel33 = new System.Windows.Forms.Panel();
            this.simpleButtonMore = new DevExpress.XtraEditors.SimpleButton();
            this.panel32 = new System.Windows.Forms.Panel();
            this.ButtonFind = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.popupEditQKind = new DevExpress.XtraEditors.PopupContainerEdit();
            this.popupContainerDesi2 = new DevExpress.XtraEditors.PopupContainerControl();
            this.tListDesignKind2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.PopupContainerDesi = new DevExpress.XtraEditors.PopupContainerControl();
            this.tListDesignKind = new DevExpress.XtraTreeList.TreeList();
            this.tcolBase1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textName = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelEdit = new System.Windows.Forms.Panel();
            this.panelName = new System.Windows.Forms.Panel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.textName2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.popupEditDist = new DevExpress.XtraEditors.PopupContainerEdit();
            this.popupContainerDist = new DevExpress.XtraEditors.PopupContainerControl();
            this.tListDist = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.simpleButtonEditOK = new DevExpress.XtraEditors.SimpleButton();
            this.label18 = new System.Windows.Forms.Label();
            this.simpleButtonEditCancle = new DevExpress.XtraEditors.SimpleButton();
            this.panelKind = new System.Windows.Forms.Panel();
            this.popupEditKind = new DevExpress.XtraEditors.PopupContainerEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.labelEdit = new System.Windows.Forms.Label();
            this.popupContainerDist2 = new DevExpress.XtraEditors.PopupContainerControl();
            this.tListDist2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.popupEditQDist = new DevExpress.XtraEditors.PopupContainerEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            this.panel35.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupEditQKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDesi2)).BeginInit();
            this.popupContainerDesi2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerDesi)).BeginInit();
            this.PopupContainerDesi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).BeginInit();
            this.panelEdit.SuspendLayout();
            this.panelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupEditDist.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDist)).BeginInit();
            this.popupContainerDist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tListDist)).BeginInit();
            this.panel6.SuspendLayout();
            this.panelKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupEditKind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDist2)).BeginInit();
            this.popupContainerDist2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tListDist2)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupEditQDist.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo.ImageIndex = 8;
            this.labelInfo.ImageList = this.imageList1;
            this.labelInfo.Location = new System.Drawing.Point(4, 351);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(292, 24);
            this.labelInfo.TabIndex = 82;
            this.labelInfo.Text = "     共计";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "drawing_pen-16.png");
            this.imageList1.Images.SetKeyName(1, "monitor_16.png");
            this.imageList1.Images.SetKeyName(2, "clipboard_16.png");
            this.imageList1.Images.SetKeyName(3, "folder_16.png");
            this.imageList1.Images.SetKeyName(4, "plus_16.png");
            this.imageList1.Images.SetKeyName(5, "document_16.png");
            this.imageList1.Images.SetKeyName(6, "pencil_16.png");
            this.imageList1.Images.SetKeyName(7, "label_16.png");
            this.imageList1.Images.SetKeyName(8, "flag_16.png");
            this.imageList1.Images.SetKeyName(9, "info_16.png");
            this.imageList1.Images.SetKeyName(10, "clock_16.png");
            this.imageList1.Images.SetKeyName(11, "search_16.png");
            this.imageList1.Images.SetKeyName(12, "globe_16.png");
            this.imageList1.Images.SetKeyName(13, "folder_blue.png");
            this.imageList1.Images.SetKeyName(14, "27.png");
            this.imageList1.Images.SetKeyName(15, "(01,34).png");
            this.imageList1.Images.SetKeyName(16, "(02,29).png");
            this.imageList1.Images.SetKeyName(17, "delete.png");
            this.imageList1.Images.SetKeyName(18, "Feedicons_v2_031.png");
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panelButtons.Controls.Add(this.simpleButtonOpen);
            this.panelButtons.Controls.Add(this.label3);
            this.panelButtons.Controls.Add(this.simpleButtonAddNew);
            this.panelButtons.Controls.Add(this.label14);
            this.panelButtons.Controls.Add(this.simpleButtonEdit);
            this.panelButtons.Controls.Add(this.label13);
            this.panelButtons.Controls.Add(this.simpleButtonDelete);
            this.panelButtons.Controls.Add(this.label2);
            this.panelButtons.Controls.Add(this.simpleButtonDeleteAll);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(4, 375);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.panelButtons.Size = new System.Drawing.Size(292, 34);
            this.panelButtons.TabIndex = 81;
            // 
            // simpleButtonOpen
            // 
            this.simpleButtonOpen.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonOpen.ImageIndex = 15;
            this.simpleButtonOpen.ImageList = this.imageList1;
            this.simpleButtonOpen.Location = new System.Drawing.Point(4, 4);
            this.simpleButtonOpen.Name = "simpleButtonOpen";
            this.simpleButtonOpen.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonOpen.TabIndex = 12;
            this.simpleButtonOpen.Text = "打开";
            this.simpleButtonOpen.ToolTip = "打开作业设计";
            this.simpleButtonOpen.Click += new System.EventHandler(this.simpleButtonOpen_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(60, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(2, 24);
            this.label3.TabIndex = 17;
            // 
            // simpleButtonAddNew
            // 
            this.simpleButtonAddNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonAddNew.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAddNew.Image")));
            this.simpleButtonAddNew.Location = new System.Drawing.Point(62, 4);
            this.simpleButtonAddNew.Name = "simpleButtonAddNew";
            this.simpleButtonAddNew.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonAddNew.TabIndex = 14;
            this.simpleButtonAddNew.Text = "增加";
            this.simpleButtonAddNew.ToolTip = "增加作业设计";
            this.simpleButtonAddNew.Click += new System.EventHandler(this.simpleButtonAddNew_Click);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Right;
            this.label14.Location = new System.Drawing.Point(118, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(2, 24);
            this.label14.TabIndex = 13;
            // 
            // simpleButtonEdit
            // 
            this.simpleButtonEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEdit.Image")));
            this.simpleButtonEdit.Location = new System.Drawing.Point(120, 4);
            this.simpleButtonEdit.Name = "simpleButtonEdit";
            this.simpleButtonEdit.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonEdit.TabIndex = 1;
            this.simpleButtonEdit.Text = "修改";
            this.simpleButtonEdit.ToolTip = "修改作业设计名称";
            this.simpleButtonEdit.Click += new System.EventHandler(this.simpleButtonEdit_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Right;
            this.label13.Location = new System.Drawing.Point(176, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(2, 24);
            this.label13.TabIndex = 11;
            // 
            // simpleButtonDelete
            // 
            this.simpleButtonDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonDelete.Image")));
            this.simpleButtonDelete.Location = new System.Drawing.Point(178, 4);
            this.simpleButtonDelete.Name = "simpleButtonDelete";
            this.simpleButtonDelete.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonDelete.TabIndex = 0;
            this.simpleButtonDelete.Text = "删除";
            this.simpleButtonDelete.ToolTip = "删除作业设计";
            this.simpleButtonDelete.Click += new System.EventHandler(this.simpleButtonDelete_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(234, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 24);
            this.label2.TabIndex = 19;
            // 
            // simpleButtonDeleteAll
            // 
            this.simpleButtonDeleteAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonDeleteAll.ImageIndex = 18;
            this.simpleButtonDeleteAll.ImageList = this.imageList1;
            this.simpleButtonDeleteAll.Location = new System.Drawing.Point(236, 4);
            this.simpleButtonDeleteAll.Name = "simpleButtonDeleteAll";
            this.simpleButtonDeleteAll.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonDeleteAll.TabIndex = 18;
            this.simpleButtonDeleteAll.Text = "清空";
            this.simpleButtonDeleteAll.ToolTip = "删除所有作业设计及图斑";
            this.simpleButtonDeleteAll.Click += new System.EventHandler(this.simpleButtonDeleteAll_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(4, 188);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox2});
            this.gridControl1.Size = new System.Drawing.Size(292, 163);
            this.gridControl1.TabIndex = 98;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.Blue;
            this.gridView1.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.Blue;
            this.gridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Yellow;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "名称";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // panel35
            // 
            this.panel35.Controls.Add(this.dateEdit2);
            this.panel35.Controls.Add(this.label31);
            this.panel35.Controls.Add(this.dateEdit1);
            this.panel35.Controls.Add(this.label30);
            this.panel35.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel35.Location = new System.Drawing.Point(4, 78);
            this.panel35.Name = "panel35";
            this.panel35.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel35.Size = new System.Drawing.Size(292, 26);
            this.panel35.TabIndex = 100;
            // 
            // dateEdit2
            // 
            this.dateEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(159, 0);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit2.Size = new System.Drawing.Size(133, 20);
            this.dateEdit2.TabIndex = 24;
            // 
            // label31
            // 
            this.label31.Dock = System.Windows.Forms.DockStyle.Left;
            this.label31.Location = new System.Drawing.Point(147, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(12, 20);
            this.label31.TabIndex = 23;
            this.label31.Text = "-";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dateEdit1
            // 
            this.dateEdit1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(70, 0);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Size = new System.Drawing.Size(77, 20);
            this.dateEdit1.TabIndex = 22;
            // 
            // label30
            // 
            this.label30.Dock = System.Windows.Forms.DockStyle.Left;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(70, 20);
            this.label30.TabIndex = 100;
            this.label30.Text = "创建时间:";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.simpleButtonReset);
            this.panel2.Controls.Add(this.panel33);
            this.panel2.Controls.Add(this.simpleButtonMore);
            this.panel2.Controls.Add(this.panel32);
            this.panel2.Controls.Add(this.ButtonFind);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(4, 130);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 6);
            this.panel2.Size = new System.Drawing.Size(292, 32);
            this.panel2.TabIndex = 101;
            // 
            // simpleButtonReset
            // 
            this.simpleButtonReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonReset.Image")));
            this.simpleButtonReset.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonReset.Location = new System.Drawing.Point(112, 2);
            this.simpleButtonReset.Name = "simpleButtonReset";
            this.simpleButtonReset.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonReset.TabIndex = 14;
            this.simpleButtonReset.Text = "重置";
            this.simpleButtonReset.ToolTip = "重新设置查询条件";
            this.simpleButtonReset.Click += new System.EventHandler(this.simpleButtonReset_Click);
            // 
            // panel33
            // 
            this.panel33.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel33.Location = new System.Drawing.Point(168, 2);
            this.panel33.Name = "panel33";
            this.panel33.Size = new System.Drawing.Size(6, 24);
            this.panel33.TabIndex = 18;
            this.panel33.Visible = false;
            // 
            // simpleButtonMore
            // 
            this.simpleButtonMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpleButtonMore.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonMore.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonMore.Image")));
            this.simpleButtonMore.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonMore.Location = new System.Drawing.Point(174, 2);
            this.simpleButtonMore.Name = "simpleButtonMore";
            this.simpleButtonMore.Size = new System.Drawing.Size(56, 24);
            this.simpleButtonMore.TabIndex = 13;
            this.simpleButtonMore.Tag = "基本";
            this.simpleButtonMore.Text = "更多";
            this.simpleButtonMore.ToolTip = "更多查询条件";
            this.simpleButtonMore.Visible = false;
            // 
            // panel32
            // 
            this.panel32.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel32.Location = new System.Drawing.Point(230, 2);
            this.panel32.Name = "panel32";
            this.panel32.Size = new System.Drawing.Size(6, 24);
            this.panel32.TabIndex = 17;
            // 
            // ButtonFind
            // 
            this.ButtonFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFind.Image")));
            this.ButtonFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.ButtonFind.Location = new System.Drawing.Point(236, 2);
            this.ButtonFind.Name = "ButtonFind";
            this.ButtonFind.Size = new System.Drawing.Size(56, 24);
            this.ButtonFind.TabIndex = 12;
            this.ButtonFind.Text = "查找";
            this.ButtonFind.ToolTip = "小班查找";
            this.ButtonFind.Click += new System.EventHandler(this.ButtonFind_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.popupEditQKind);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 26);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel1.Size = new System.Drawing.Size(292, 26);
            this.panel1.TabIndex = 102;
            // 
            // popupEditQKind
            // 
            this.popupEditQKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.popupEditQKind.EditValue = "";
            this.popupEditQKind.Location = new System.Drawing.Point(70, 0);
            this.popupEditQKind.Name = "popupEditQKind";
            this.popupEditQKind.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.popupEditQKind.Properties.Appearance.Options.UseBackColor = true;
            this.popupEditQKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("popupEditQKind.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.popupEditQKind.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.popupEditQKind.Properties.PopupControl = this.popupContainerDesi2;
            this.popupEditQKind.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.popupEditQKind.Properties.ShowPopupShadow = false;
            this.popupEditQKind.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.popupEditQKind.Size = new System.Drawing.Size(222, 22);
            this.popupEditQKind.TabIndex = 107;
            this.popupEditQKind.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.PopupContainerEdit1_ButtonClick);
            this.popupEditQKind.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.PopupContainerEdit1_ButtonPressed);
            this.popupEditQKind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.popupEditQKind_KeyPress);
            // 
            // popupContainerDesi2
            // 
            this.popupContainerDesi2.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.popupContainerDesi2.Appearance.Options.UseBackColor = true;
            this.popupContainerDesi2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.popupContainerDesi2.Controls.Add(this.tListDesignKind2);
            this.popupContainerDesi2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.popupContainerDesi2.Location = new System.Drawing.Point(89, 167);
            this.popupContainerDesi2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.popupContainerDesi2.Name = "popupContainerDesi2";
            this.popupContainerDesi2.Padding = new System.Windows.Forms.Padding(1);
            this.popupContainerDesi2.Size = new System.Drawing.Size(170, 166);
            this.popupContainerDesi2.TabIndex = 109;
            // 
            // tListDesignKind2
            // 
            this.tListDesignKind2.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.Empty.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tListDesignKind2.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.EvenRow.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.EvenRow.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.FocusedCell.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tListDesignKind2.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind2.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.FocusedRow.BackColor = System.Drawing.Color.DodgerBlue;
            this.tListDesignKind2.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind2.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind2.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind2.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind2.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tListDesignKind2.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind2.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind2.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.GroupButton.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tListDesignKind2.Appearance.GroupButton.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind2.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind2.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tListDesignKind2.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind2.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind2.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind2.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tListDesignKind2.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tListDesignKind2.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tListDesignKind2.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind2.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.HorzLine.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.OddRow.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.OddRow.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tListDesignKind2.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tListDesignKind2.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.Preview.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.Preview.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind2.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.Row.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.Row.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tListDesignKind2.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind2.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tListDesignKind2.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tListDesignKind2.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.TreeLine.Options.UseBackColor = true;
            this.tListDesignKind2.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind2.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind2.Appearance.VertLine.Options.UseBackColor = true;
            this.tListDesignKind2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tListDesignKind2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.tListDesignKind2.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tListDesignKind2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tListDesignKind2.Location = new System.Drawing.Point(3, 3);
            this.tListDesignKind2.LookAndFeel.SkinName = "Blue";
            this.tListDesignKind2.Name = "tListDesignKind2";
            this.tListDesignKind2.OptionsBehavior.Editable = false;
            this.tListDesignKind2.OptionsView.ShowColumns = false;
            this.tListDesignKind2.OptionsView.ShowHorzLines = false;
            this.tListDesignKind2.OptionsView.ShowIndicator = false;
            this.tListDesignKind2.OptionsView.ShowVertLines = false;
            this.tListDesignKind2.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowForFocusedRow;
            this.tListDesignKind2.Size = new System.Drawing.Size(164, 160);
            this.tListDesignKind2.TabIndex = 78;
            this.tListDesignKind2.TreeLevelWidth = 12;
            this.tListDesignKind2.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tListDesignKind2.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tListDesignKind2_FocusedNodeChanged);
            this.tListDesignKind2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tListDesignKind2_MouseUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "设备号";
            this.treeListColumn1.MinWidth = 100;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 100;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 100;
            this.label4.Text = "设计类型:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PopupContainerDesi
            // 
            this.PopupContainerDesi.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PopupContainerDesi.Appearance.Options.UseBackColor = true;
            this.PopupContainerDesi.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.PopupContainerDesi.Controls.Add(this.tListDesignKind);
            this.PopupContainerDesi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PopupContainerDesi.Location = new System.Drawing.Point(100, 170);
            this.PopupContainerDesi.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.PopupContainerDesi.Name = "PopupContainerDesi";
            this.PopupContainerDesi.Padding = new System.Windows.Forms.Padding(1);
            this.PopupContainerDesi.Size = new System.Drawing.Size(170, 166);
            this.PopupContainerDesi.TabIndex = 108;
            // 
            // tListDesignKind
            // 
            this.tListDesignKind.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Empty.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.EvenRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.EvenRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FocusedCell.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tListDesignKind.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FocusedRow.BackColor = System.Drawing.Color.DodgerBlue;
            this.tListDesignKind.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDesignKind.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.GroupButton.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.GroupButton.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDesignKind.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDesignKind.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tListDesignKind.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tListDesignKind.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tListDesignKind.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.HorzLine.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.OddRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.OddRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tListDesignKind.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tListDesignKind.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Preview.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.Preview.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tListDesignKind.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.Row.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.Row.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tListDesignKind.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tListDesignKind.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tListDesignKind.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tListDesignKind.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.TreeLine.Options.UseBackColor = true;
            this.tListDesignKind.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDesignKind.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDesignKind.Appearance.VertLine.Options.UseBackColor = true;
            this.tListDesignKind.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tListDesignKind.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tcolBase1});
            this.tListDesignKind.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tListDesignKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tListDesignKind.Location = new System.Drawing.Point(3, 3);
            this.tListDesignKind.LookAndFeel.SkinName = "Blue";
            this.tListDesignKind.Name = "tListDesignKind";
            this.tListDesignKind.OptionsBehavior.Editable = false;
            this.tListDesignKind.OptionsView.ShowColumns = false;
            this.tListDesignKind.OptionsView.ShowHorzLines = false;
            this.tListDesignKind.OptionsView.ShowIndicator = false;
            this.tListDesignKind.OptionsView.ShowVertLines = false;
            this.tListDesignKind.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowForFocusedRow;
            this.tListDesignKind.Size = new System.Drawing.Size(164, 160);
            this.tListDesignKind.TabIndex = 78;
            this.tListDesignKind.TreeLevelWidth = 12;
            this.tListDesignKind.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tListDesignKind.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tListDesignKind_FocusedNodeChanged);
            this.tListDesignKind.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tListDesignKind_MouseUp);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.textName);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(4, 104);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel3.Size = new System.Drawing.Size(292, 26);
            this.panel3.TabIndex = 103;
            // 
            // textName
            // 
            this.textName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName.Location = new System.Drawing.Point(70, 0);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(222, 20);
            this.textName.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 100;
            this.label1.Text = "设计名称:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.ImageIndex = 2;
            this.label5.ImageList = this.imageList1;
            this.label5.Location = new System.Drawing.Point(4, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(292, 26);
            this.label5.TabIndex = 104;
            this.label5.Text = "     设计列表";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(292, 26);
            this.label6.TabIndex = 105;
            this.label6.Text = "     查询条件";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelEdit
            // 
            this.panelEdit.Controls.Add(this.panelName);
            this.panelEdit.Controls.Add(this.panel4);
            this.panelEdit.Controls.Add(this.panel6);
            this.panelEdit.Controls.Add(this.panelKind);
            this.panelEdit.Controls.Add(this.labelEdit);
            this.panelEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEdit.Location = new System.Drawing.Point(4, 409);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(292, 151);
            this.panelEdit.TabIndex = 106;
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.panelControl2);
            this.panelName.Controls.Add(this.label7);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelName.Location = new System.Drawing.Point(0, 78);
            this.panelName.Name = "panelName";
            this.panelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panelName.Size = new System.Drawing.Size(292, 43);
            this.panelName.TabIndex = 105;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.textName2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(70, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(222, 37);
            this.panelControl2.TabIndex = 102;
            // 
            // textName2
            // 
            this.textName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName2.Location = new System.Drawing.Point(2, 2);
            this.textName2.Multiline = true;
            this.textName2.Name = "textName2";
            this.textName2.Size = new System.Drawing.Size(218, 33);
            this.textName2.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 37);
            this.label7.TabIndex = 100;
            this.label7.Text = "设计名称:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.popupEditDist);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 52);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel4.Size = new System.Drawing.Size(292, 26);
            this.panel4.TabIndex = 108;
            // 
            // popupEditDist
            // 
            this.popupEditDist.Dock = System.Windows.Forms.DockStyle.Top;
            this.popupEditDist.EditValue = "";
            this.popupEditDist.Location = new System.Drawing.Point(70, 0);
            this.popupEditDist.Name = "popupEditDist";
            this.popupEditDist.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.popupEditDist.Properties.Appearance.Options.UseBackColor = true;
            this.popupEditDist.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("popupEditDist.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.popupEditDist.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.popupEditDist.Properties.PopupControl = this.popupContainerDist;
            this.popupEditDist.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.popupEditDist.Properties.ShowPopupShadow = false;
            this.popupEditDist.Size = new System.Drawing.Size(222, 22);
            this.popupEditDist.TabIndex = 108;
            this.popupEditDist.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupEditDist_ButtonClick);
            this.popupEditDist.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupEditDist_ButtonPressed);
            this.popupEditDist.TextChanged += new System.EventHandler(this.popupEditDist_TextChanged);
            // 
            // popupContainerDist
            // 
            this.popupContainerDist.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.popupContainerDist.Appearance.Options.UseBackColor = true;
            this.popupContainerDist.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.popupContainerDist.Controls.Add(this.tListDist);
            this.popupContainerDist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.popupContainerDist.Location = new System.Drawing.Point(76, 165);
            this.popupContainerDist.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.popupContainerDist.Name = "popupContainerDist";
            this.popupContainerDist.Padding = new System.Windows.Forms.Padding(1);
            this.popupContainerDist.Size = new System.Drawing.Size(170, 166);
            this.popupContainerDist.TabIndex = 110;
            // 
            // tListDist
            // 
            this.tListDist.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tListDist.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.Empty.Options.UseBackColor = true;
            this.tListDist.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tListDist.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tListDist.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.EvenRow.Options.UseBackColor = true;
            this.tListDist.Appearance.EvenRow.Options.UseForeColor = true;
            this.tListDist.Appearance.FocusedCell.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tListDist.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDist.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tListDist.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tListDist.Appearance.FocusedRow.BackColor = System.Drawing.Color.DodgerBlue;
            this.tListDist.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDist.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tListDist.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tListDist.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tListDist.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDist.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tListDist.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tListDist.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tListDist.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.GroupButton.Options.UseBackColor = true;
            this.tListDist.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tListDist.Appearance.GroupButton.Options.UseForeColor = true;
            this.tListDist.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tListDist.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tListDist.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tListDist.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDist.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tListDist.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tListDist.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tListDist.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tListDist.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tListDist.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tListDist.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tListDist.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDist.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.HorzLine.Options.UseBackColor = true;
            this.tListDist.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tListDist.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.OddRow.Options.UseBackColor = true;
            this.tListDist.Appearance.OddRow.Options.UseForeColor = true;
            this.tListDist.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tListDist.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tListDist.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.Preview.Options.UseBackColor = true;
            this.tListDist.Appearance.Preview.Options.UseForeColor = true;
            this.tListDist.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tListDist.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tListDist.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.Row.Options.UseBackColor = true;
            this.tListDist.Appearance.Row.Options.UseForeColor = true;
            this.tListDist.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tListDist.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.White;
            this.tListDist.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tListDist.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tListDist.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tListDist.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tListDist.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.TreeLine.Options.UseBackColor = true;
            this.tListDist.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDist.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist.Appearance.VertLine.Options.UseBackColor = true;
            this.tListDist.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tListDist.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.tListDist.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tListDist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tListDist.Location = new System.Drawing.Point(3, 3);
            this.tListDist.LookAndFeel.SkinName = "Blue";
            this.tListDist.Name = "tListDist";
            this.tListDist.OptionsBehavior.Editable = false;
            this.tListDist.OptionsView.ShowColumns = false;
            this.tListDist.OptionsView.ShowHorzLines = false;
            this.tListDist.OptionsView.ShowIndicator = false;
            this.tListDist.OptionsView.ShowVertLines = false;
            this.tListDist.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowForFocusedRow;
            this.tListDist.Size = new System.Drawing.Size(164, 160);
            this.tListDist.TabIndex = 78;
            this.tListDist.TreeLevelWidth = 12;
            this.tListDist.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tListDist.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tListDist_FocusedNodeChanged);
            this.tListDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tListDist_MouseUp);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "名称";
            this.treeListColumn2.FieldName = "设备号";
            this.treeListColumn2.MinWidth = 100;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            this.treeListColumn2.Width = 100;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 20);
            this.label9.TabIndex = 100;
            this.label9.Text = "区划范围:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.panel6.Controls.Add(this.labelInfo2);
            this.panel6.Controls.Add(this.simpleButtonEditOK);
            this.panel6.Controls.Add(this.label18);
            this.panel6.Controls.Add(this.simpleButtonEditCancle);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 121);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel6.Size = new System.Drawing.Size(292, 30);
            this.panel6.TabIndex = 107;
            // 
            // labelInfo2
            // 
            this.labelInfo2.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo2.ForeColor = System.Drawing.Color.Blue;
            this.labelInfo2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo2.ImageIndex = 9;
            this.labelInfo2.ImageList = this.imageList1;
            this.labelInfo2.Location = new System.Drawing.Point(0, 0);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(172, 24);
            this.labelInfo2.TabIndex = 107;
            this.labelInfo2.Text = "     ";
            this.labelInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInfo2.Visible = false;
            // 
            // simpleButtonEditOK
            // 
            this.simpleButtonEditOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEditOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEditOK.Image")));
            this.simpleButtonEditOK.Location = new System.Drawing.Point(172, 0);
            this.simpleButtonEditOK.Name = "simpleButtonEditOK";
            this.simpleButtonEditOK.Size = new System.Drawing.Size(58, 24);
            this.simpleButtonEditOK.TabIndex = 1;
            this.simpleButtonEditOK.Text = "确定";
            this.simpleButtonEditOK.Click += new System.EventHandler(this.simpleButtonEditOK_Click);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Dock = System.Windows.Forms.DockStyle.Right;
            this.label18.Location = new System.Drawing.Point(230, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(4, 24);
            this.label18.TabIndex = 11;
            // 
            // simpleButtonEditCancle
            // 
            this.simpleButtonEditCancle.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButtonEditCancle.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEditCancle.Image")));
            this.simpleButtonEditCancle.Location = new System.Drawing.Point(234, 0);
            this.simpleButtonEditCancle.Name = "simpleButtonEditCancle";
            this.simpleButtonEditCancle.Size = new System.Drawing.Size(58, 24);
            this.simpleButtonEditCancle.TabIndex = 0;
            this.simpleButtonEditCancle.Text = "取消";
            this.simpleButtonEditCancle.Click += new System.EventHandler(this.simpleButtonEditCancle_Click);
            // 
            // panelKind
            // 
            this.panelKind.Controls.Add(this.popupEditKind);
            this.panelKind.Controls.Add(this.label8);
            this.panelKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKind.Location = new System.Drawing.Point(0, 26);
            this.panelKind.Name = "panelKind";
            this.panelKind.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panelKind.Size = new System.Drawing.Size(292, 26);
            this.panelKind.TabIndex = 104;
            // 
            // popupEditKind
            // 
            this.popupEditKind.Dock = System.Windows.Forms.DockStyle.Top;
            this.popupEditKind.EditValue = "";
            this.popupEditKind.Location = new System.Drawing.Point(70, 0);
            this.popupEditKind.Name = "popupEditKind";
            this.popupEditKind.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.popupEditKind.Properties.Appearance.Options.UseBackColor = true;
            this.popupEditKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("popupEditKind.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.popupEditKind.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.popupEditKind.Properties.PopupControl = this.PopupContainerDesi;
            this.popupEditKind.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.popupEditKind.Properties.ShowPopupShadow = false;
            this.popupEditKind.Size = new System.Drawing.Size(222, 22);
            this.popupEditKind.TabIndex = 108;
            this.popupEditKind.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupEditKind_ButtonClick);
            this.popupEditKind.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupEditKind_ButtonPressed);
            this.popupEditKind.EditValueChanged += new System.EventHandler(this.popupContainerEdit2_EditValueChanged);
            this.popupEditKind.TextChanged += new System.EventHandler(this.popupEditKind_TextChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 20);
            this.label8.TabIndex = 100;
            this.label8.Text = "设计类型:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEdit
            // 
            this.labelEdit.BackColor = System.Drawing.Color.Transparent;
            this.labelEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEdit.ForeColor = System.Drawing.Color.Blue;
            this.labelEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelEdit.ImageIndex = 0;
            this.labelEdit.ImageList = this.imageList1;
            this.labelEdit.Location = new System.Drawing.Point(0, 0);
            this.labelEdit.Name = "labelEdit";
            this.labelEdit.Size = new System.Drawing.Size(292, 26);
            this.labelEdit.TabIndex = 106;
            this.labelEdit.Text = "     新增";
            this.labelEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // popupContainerDist2
            // 
            this.popupContainerDist2.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.popupContainerDist2.Appearance.Options.UseBackColor = true;
            this.popupContainerDist2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.popupContainerDist2.Controls.Add(this.tListDist2);
            this.popupContainerDist2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.popupContainerDist2.Location = new System.Drawing.Point(62, 162);
            this.popupContainerDist2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.popupContainerDist2.Name = "popupContainerDist2";
            this.popupContainerDist2.Padding = new System.Windows.Forms.Padding(1);
            this.popupContainerDist2.Size = new System.Drawing.Size(170, 166);
            this.popupContainerDist2.TabIndex = 111;
            // 
            // tListDist2
            // 
            this.tListDist2.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.tListDist2.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.Empty.Options.UseBackColor = true;
            this.tListDist2.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.tListDist2.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.tListDist2.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.EvenRow.Options.UseBackColor = true;
            this.tListDist2.Appearance.EvenRow.Options.UseForeColor = true;
            this.tListDist2.Appearance.FocusedCell.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tListDist2.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDist2.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.tListDist2.Appearance.FocusedCell.Options.UseForeColor = true;
            this.tListDist2.Appearance.FocusedRow.BackColor = System.Drawing.Color.DodgerBlue;
            this.tListDist2.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.LightCyan;
            this.tListDist2.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.tListDist2.Appearance.FocusedRow.Options.UseBackColor = true;
            this.tListDist2.Appearance.FocusedRow.Options.UseForeColor = true;
            this.tListDist2.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist2.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDist2.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist2.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.FooterPanel.Options.UseBackColor = true;
            this.tListDist2.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.tListDist2.Appearance.FooterPanel.Options.UseForeColor = true;
            this.tListDist2.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist2.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist2.Appearance.GroupButton.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.GroupButton.Options.UseBackColor = true;
            this.tListDist2.Appearance.GroupButton.Options.UseBorderColor = true;
            this.tListDist2.Appearance.GroupButton.Options.UseForeColor = true;
            this.tListDist2.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist2.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.tListDist2.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.GroupFooter.Options.UseBackColor = true;
            this.tListDist2.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.tListDist2.Appearance.GroupFooter.Options.UseForeColor = true;
            this.tListDist2.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist2.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.tListDist2.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.tListDist2.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.tListDist2.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.tListDist2.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.tListDist2.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.tListDist2.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.tListDist2.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.tListDist2.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.tListDist2.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDist2.Appearance.HorzLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.HorzLine.Options.UseBackColor = true;
            this.tListDist2.Appearance.OddRow.BackColor = System.Drawing.Color.White;
            this.tListDist2.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.OddRow.Options.UseBackColor = true;
            this.tListDist2.Appearance.OddRow.Options.UseForeColor = true;
            this.tListDist2.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.tListDist2.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(129)))), ((int)(((byte)(185)))));
            this.tListDist2.Appearance.Preview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.Preview.Options.UseBackColor = true;
            this.tListDist2.Appearance.Preview.Options.UseForeColor = true;
            this.tListDist2.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.tListDist2.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.tListDist2.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.Row.Options.UseBackColor = true;
            this.tListDist2.Appearance.Row.Options.UseForeColor = true;
            this.tListDist2.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(217)))));
            this.tListDist2.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.White;
            this.tListDist2.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.tListDist2.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.SelectedRow.Options.UseBackColor = true;
            this.tListDist2.Appearance.SelectedRow.Options.UseForeColor = true;
            this.tListDist2.Appearance.TreeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tListDist2.Appearance.TreeLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.TreeLine.Options.UseBackColor = true;
            this.tListDist2.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.tListDist2.Appearance.VertLine.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tListDist2.Appearance.VertLine.Options.UseBackColor = true;
            this.tListDist2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tListDist2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn3});
            this.tListDist2.CustomizationFormBounds = new System.Drawing.Rectangle(269, 370, 208, 163);
            this.tListDist2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tListDist2.Location = new System.Drawing.Point(3, 3);
            this.tListDist2.LookAndFeel.SkinName = "Blue";
            this.tListDist2.Name = "tListDist2";
            this.tListDist2.OptionsBehavior.Editable = false;
            this.tListDist2.OptionsView.ShowColumns = false;
            this.tListDist2.OptionsView.ShowHorzLines = false;
            this.tListDist2.OptionsView.ShowIndicator = false;
            this.tListDist2.OptionsView.ShowVertLines = false;
            this.tListDist2.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowForFocusedRow;
            this.tListDist2.Size = new System.Drawing.Size(164, 160);
            this.tListDist2.TabIndex = 78;
            this.tListDist2.TreeLevelWidth = 12;
            this.tListDist2.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tListDist2.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tListDist2_FocusedNodeChanged);
            this.tListDist2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tListDist2_MouseUp);
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "名称";
            this.treeListColumn3.FieldName = "设备号";
            this.treeListColumn3.MinWidth = 100;
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 0;
            this.treeListColumn3.Width = 100;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.popupEditQDist);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(4, 52);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel5.Size = new System.Drawing.Size(292, 26);
            this.panel5.TabIndex = 112;
            // 
            // popupEditQDist
            // 
            this.popupEditQDist.Dock = System.Windows.Forms.DockStyle.Top;
            this.popupEditQDist.EditValue = "";
            this.popupEditQDist.Location = new System.Drawing.Point(70, 0);
            this.popupEditQDist.Name = "popupEditQDist";
            this.popupEditQDist.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.popupEditQDist.Properties.Appearance.Options.UseBackColor = true;
            this.popupEditQDist.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("popupEditQDist.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.popupEditQDist.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.NoBorder;
            this.popupEditQDist.Properties.PopupControl = this.popupContainerDist2;
            this.popupEditQDist.Properties.PopupFormMinSize = new System.Drawing.Size(100, 0);
            this.popupEditQDist.Properties.ShowPopupShadow = false;
            this.popupEditQDist.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.popupEditQDist.Size = new System.Drawing.Size(222, 22);
            this.popupEditQDist.TabIndex = 108;
            this.popupEditQDist.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupContainerEdit2_ButtonClick);
            this.popupEditQDist.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupContainerEdit2_ButtonPressed);
            this.popupEditQDist.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.popupEditQDist_KeyPress);
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 20);
            this.label10.TabIndex = 100;
            this.label10.Text = "区划范围:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControlDesignList
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.Controls.Add(this.popupContainerDist2);
            this.Controls.Add(this.popupContainerDist);
            this.Controls.Add(this.popupContainerDesi2);
            this.Controls.Add(this.PopupContainerDesi);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel35);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Name = "UserControlDesignList";
            this.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Size = new System.Drawing.Size(300, 560);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            this.panel35.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupEditQKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDesi2)).EndInit();
            this.popupContainerDesi2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerDesi)).EndInit();
            this.PopupContainerDesi.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tListDesignKind)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textName.Properties)).EndInit();
            this.panelEdit.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupEditDist.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDist)).EndInit();
            this.popupContainerDist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tListDist)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panelKind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupEditKind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerDist2)).EndInit();
            this.popupContainerDist2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tListDist2)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupEditQDist.Properties)).EndInit();
            this.ResumeLayout(false);

        }
  
        private void InitialKindList(TreeList tList)
        {
            try
            {
                if (this.m_kindLst != null)
                {
                    TreeListNode node = null;
                    TreeListNode parentNode = null;
                    TreeListNode node3 = null;
                    TreeListNode node4 = null;
                    try
                    {
                        if (tList.Nodes.Count > 0)
                        {
                            tList.Nodes.Clear();
                        }                     
                    }
                    catch (Exception)
                    {
                    }
                    tList.OptionsView.ShowRoot = true;
                    tList.SelectImageList = null;
                    tList.OptionsView.ShowButtons = true;
                    tList.TreeLineStyle = LineStyle.None;
                    tList.RowHeight = 20;
                    tList.OptionsBehavior.AutoPopulateColumns = true;
                    for (int i = 0; i < m_kindLst.Count; i++)
                    {
                        this.mSelected = true;
                        try
                        {
                            node3 = tList.AppendNode(m_kindLst[i].name, node4);
                        }
                        catch (Exception)
                        {
                            node3 = tList.AppendNode(m_kindLst[i].name, node4);
                        }
                        node3.SetValue(0, m_kindLst[i].name);
                        ///源代码：node3.Tag = m_kindLst[i];
                        node3.Tag = m_kindLst[i].code;
                        node3.ImageIndex = -1;
                        node3.StateImageIndex = 0;
                        node3.SelectImageIndex = -1;
                        node3.ExpandAll();
                        IList<T_DESIGNKIND_Mid> dList2 = m_kindLst[i].SubList;
                       
                        for (int k = 0; k < dList2.Count; k++)
                        {
                            parentNode = tList.AppendNode(dList2[k].name, node3);
                            parentNode.ImageIndex = -1;
                            parentNode.StateImageIndex = 0;
                            parentNode.SelectImageIndex = -1;
                            parentNode.SetValue(0, dList2[k].name);
                            ///源代码parentNode.Tag = dList2[k];
                            parentNode.Tag = dList2[k].code;
                            parentNode.Expanded = false;

                            IList<T_DESIGNKIND_Mid> dList3 = dList2[k].SubList; //DesignService.FindBySql("code like '" + str2 + "%' and right(code ,4 )<>'0000' and right(code ,2 )<>'00' and kind='" + this.mKindCode + "'");

                            for (int m = 0; m < dList3.Count; m++)
                            {
                                node = tList.AppendNode(dList3[m].name, parentNode);
                                node.ImageIndex = -1;
                                node.StateImageIndex = 0;
                                node.SelectImageIndex = -1;
                                node.SetValue(0, dList3[m].name);
                                node.Tag = dList3[i];
                                node.Expanded = false;
                            }
                        }
                        node3.ExpandAll();
                        tList.Selection.Clear();
                        tList.Refresh();
                        tList.OptionsSelection.Reset();
                        this.mSelected = false;
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "InitialKindList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
        private DB2GridViewManager DBVM { get { return DBServiceFactory<DB2GridViewManager>.Service; } }
        private void InitialList()
        {
            try
            {
                this.gridControl1.DataSource = null;
                this.gridView1.Columns.Clear();
                DBVM.InitViewColumn("设计名称", "taskname", gridView1);
                this.gridControl1.DataSource = this.m_projectList;
                this.gridView1.RefreshData();
                this.gridView1.OptionsBehavior.Editable = false;
                this.gridControl1.Enabled = true;

                this.labelInfo.Text = "     共计" + m_projectList.Count + "个作业设计";
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "InitialList", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }
        private ProjectManager PM
        {
            get { return DBServiceFactory<ProjectManager>.Service; }
        }
        public void InitialValue(object hook, IFeatureLayer pFeatureLayer)
        {
            try
            {
                this.mHookHelper = new HookHelperClass();
                this.mHookHelper.Hook = hook;
                this.mEditLayer = pFeatureLayer;
                this.mMap = this.mHookHelper.FocusMap;
                string str = "CaiFa";
                string configValue = UtilFactory.GetConfigOpt().GetConfigValue(str + "GroupName");
                this.mGroupLayer = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, configValue, true) as IGroupLayer;
                string sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str + "LayerName2");
                string name = UtilFactory.GetConfigOpt().GetConfigValue(str + "TableName");
                if (this.mGroupLayer != null)
                {
                    this.mEditLayer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(this.mGroupLayer, sLayerName, true) as IFeatureLayer;
                    this.mEditTable = EditTask.EditWorkspace.OpenTable(name);
                }
         
                this.mKindCode = "2";
                m_kindLst = PM.FindTreeByKindCode(mKindCode);
                m_projectList = PM.FindByKindCode(mKindCode);
                this.mFeatureWorkspace = EditTask.EditWorkspace;
                if (this.mFeatureWorkspace != null)
                {
                    string str5 = UtilFactory.GetConfigOpt().GetConfigValue("CityCodeTableName");
                    this.m_CityTable = this.mFeatureWorkspace.OpenTable(str5);
                    if (this.m_CityTable != null)
                    {
                        str5 = UtilFactory.GetConfigOpt().GetConfigValue("CountyCodeTableName");
                        this.m_CountyTable = this.mFeatureWorkspace.OpenTable(str5);
                        if (this.m_CountyTable != null)
                        {
                            str5 = UtilFactory.GetConfigOpt().GetConfigValue("TownCodeTableName");
                            this.m_TownTable = this.mFeatureWorkspace.OpenTable(str5);
                            if (this.m_TownTable != null)
                            {
                                str5 = UtilFactory.GetConfigOpt().GetConfigValue("VillageCodeTableName");
                                this.m_VillageTable = this.mFeatureWorkspace.OpenTable(str5);
                                //为设计类型下拉列表初始化数据
                                this.InitialKindList(this.tListDesignKind);
                                //为区划范围下拉列表初始化数据 
                                this.InitialKindList(this.tListDesignKind2);
                                //为项目名称列表初始化数据 
                                this.InitialList();
                                //
                                this.InitialDistList(this.tListDist);
                                this.InitialDistList(this.tListDist2);
                                this.panelEdit.Visible = false;
                                this.textName.Text = "";
                                this.textName2.Text = "";
                                this.labelInfo2.Visible = false;
                                if (EditTask.TaskID != 0L)
                                {
                                   var pjLst= m_projectList.Where(p => p.ID == EditTask.TaskID);
                                   foreach(var pj in pjLst)
                                    {
                                        this.ButtonOpenClick(false);
                                        break;
                                   }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "InitialValue", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        public void OpenTask(T_EDITTASK_ZT_Mid prjMid, bool seldist)
        {
            try
            {
                this.m_curProject = prjMid;
               
                string sLayerName = "";
                string configValue = "";
                string str3 = "";
                sLayerName = UtilFactory.GetConfigOpt().GetConfigValue("CaiFalyr");
                configValue = UtilFactory.GetConfigOpt().GetConfigValue("CaiFalyr2");
                IFeatureWorkspace editWorkspace = EditTask.EditWorkspace;
                IWorkspace workspace2 = editWorkspace as IWorkspace;
                IDataset dataset = workspace2 as IDataset;
                IFeatureDataset dataset2 = editWorkspace.OpenFeatureDataset(prjMid.datasetname) as IFeatureDataset;
                IEnumDataset subsets = dataset2.Subsets;
                IDataset dataset4 = subsets.Next();
                IFeatureClass class2 = null;
                IFeatureClass class3 = null;
                while (dataset4 != null)
                {
                    if (dataset4.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        class2 = dataset4 as IFeatureClass;
                        string[] strArray = dataset4.Name.Split(new char[] { '.' });
                        string str4 = strArray[strArray.Length - 1];
                        if (str4.Equals(prjMid.layername))
                        {
                            class3 = class2;
                            break;
                        }
                    }
                    dataset4 = subsets.Next();
                }
                if (class3 != null)
                {
                    IMapDocument document = null;
                    document = new MapDocumentClass();
                    string sDocument = UtilFactory.GetConfigOpt().RootPath + @"\Template\" + configValue;
                    try
                    {
                        document.Open(sDocument, "");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(sDocument + "  " + exception.Message);
                    }
                    ILayer layer = null;
                    if (document.DocumentType == esriMapDocumentType.esriMapDocumentTypeLyr)
                    {
                        layer = document.get_Layer(0, 0);
                        try
                        {
                            ((IFeatureLayer) layer).FeatureClass = class3;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    document.Close();
                    document = new MapDocumentClass();
                    sDocument = UtilFactory.GetConfigOpt().RootPath + @"\Template\" + sLayerName;
                    document.Open(sDocument, "");
                    ILayer layer2 = null;
                    if (document.DocumentType == esriMapDocumentType.esriMapDocumentTypeLyr)
                    {
                        layer2 = document.get_Layer(0, 0);
                        try
                        {
                            ((IFeatureLayer) layer2).FeatureClass = class3;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    document.Close();
                    if (layer2 != null)
                    {
                        str3 = "CaiFa";
                        sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str3 + "LayerName");
                        string str6 = UtilFactory.GetConfigOpt().GetConfigValue(str3 + "GroupName");
                        IGroupLayer pGroupLayer = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, str6, true) as IGroupLayer;
                        str6 = UtilFactory.GetConfigOpt().GetConfigValue(str3 + "GroupName2");
                        GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, str6, true);
                        if (pGroupLayer != null)
                        {
                            this.mEditLayer = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, sLayerName, true) as IFeatureLayer;
                            sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str3 + "LayerName2");
                            this.mEditLayer2 = GISFunFactory.LayerFun.FindLayerInGroupLayer(pGroupLayer, sLayerName, true) as IFeatureLayer;
                        }
                        else
                        {
                            this.mEditLayer = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, sLayerName, true) as IFeatureLayer;
                            sLayerName = UtilFactory.GetConfigOpt().GetConfigValue(str3 + "LayerName2");
                            this.mEditLayer2 = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, sLayerName, true) as IFeatureLayer;
                        }
                        if (this.mEditLayer != null)
                        {
                            this.mEditLayer.FeatureClass = class3;
                            this.mEditLayer.Visible = true;
                            if (this.mEditLayer2 != null)
                            {
                                this.mEditLayer2.FeatureClass = ((IFeatureLayer) layer).FeatureClass;
                                this.mEditLayer2.Visible = true;
                            }
                            else
                            {
                                this.mEditLayer2 = layer as IFeatureLayer;
                                this.mEditLayer2.Visible = true;
                                if (pGroupLayer != null)
                                {
                                    pGroupLayer.Add(this.mEditLayer2);
                                }
                            }
                            TaskManageClass.TaskState = TaskState.Open;
                            TaskManageClass.LogicCheckState = LogicCheckState.Failure;
                            TaskManageClass.ToplogicCheckState = ToplogicCheckState.Failure;
                            EditTask.KindCode = prjMid.taskkind;
                            EditTask.TaskName = prjMid.taskname;
                            EditTask.DistCode = prjMid.distcode;
                            EditTask.TaskState = (TaskState2)int.Parse(prjMid.taskstate);
                            EditTask.TaskYear = prjMid.taskyear;
                            EditTask.CreateTime = prjMid.createtime;
                            EditTask.EditTime = prjMid.edittime;
                            EditTask.DatasetName = prjMid.datasetname;
                            EditTask.LayerName = prjMid.layername;
                            EditTask.TableName = prjMid.tablename;
                            EditTask.TaskID = prjMid.ID;
                            if (prjMid.logiccheckstate == "1")
                            {
                                EditTask.LogicChkState = LogicCheckState.Success;
                            }
                            else if (prjMid.logiccheckstate == "0")
                            {
                                EditTask.LogicChkState = LogicCheckState.Failure;
                            }
                            if (prjMid.toplogiccheckstate == "1")
                            {
                                EditTask.ToplogicChkState = ToplogicCheckState.Success;
                            }
                            else if (prjMid.toplogiccheckstate == "0")
                            {
                                EditTask.ToplogicChkState = ToplogicCheckState.Failure;
                            }
                            EditTask.EditLayer = this.mEditLayer;
                            string str7 = "Task_ID= '" + EditTask.TaskID + "'";
                            IFeatureLayerDefinition mEditLayer = (IFeatureLayerDefinition) this.mEditLayer;
                            mEditLayer.DefinitionExpression = str7;
                            str7 = "Task_ID<> '" + EditTask.TaskID + "' or TASK_ID IS NULL";
                            mEditLayer = (IFeatureLayerDefinition) this.mEditLayer2;
                            mEditLayer.DefinitionExpression = str7;
                        }
                        else
                        {
                            this.mEditLayer2 = layer as IFeatureLayer;
                            this.mEditLayer = layer2 as IFeatureLayer;
                            TaskManageClass.TaskState = TaskState.Open;
                            TaskManageClass.LogicCheckState = LogicCheckState.Failure;
                            TaskManageClass.ToplogicCheckState = ToplogicCheckState.Failure;
                            EditTask.KindCode = prjMid.taskkind;
                            EditTask.TaskName = prjMid.taskname;
                            EditTask.DistCode =prjMid.distcode;
                            EditTask.TaskState = (TaskState2) int.Parse(prjMid.taskstate);
                            EditTask.TaskYear =prjMid.taskyear;
                            EditTask.CreateTime = prjMid.createtime;
                            EditTask.EditTime = prjMid.edittime;
                            EditTask.DatasetName = prjMid.datasetname;
                            EditTask.LayerName = prjMid.layername;
                            EditTask.TableName = prjMid.tablename ;
                            EditTask.TaskID = prjMid.ID;
                            if (prjMid.logiccheckstate == "1")
                            {
                                EditTask.LogicChkState = LogicCheckState.Success;
                            }
                            else if (prjMid.logiccheckstate == "0")
                            {
                                EditTask.LogicChkState = LogicCheckState.Failure;
                            }
                            if (prjMid.toplogiccheckstate == "1")
                            {
                                EditTask.ToplogicChkState = ToplogicCheckState.Success;
                            }
                            else if (prjMid.toplogiccheckstate == "0")
                            {
                                EditTask.ToplogicChkState = ToplogicCheckState.Failure;
                            }
                            EditTask.EditLayer = this.mEditLayer;
                            string str8 = "Task_ID= '" + EditTask.TaskID + "'";
                            IFeatureLayerDefinition definition2 = (IFeatureLayerDefinition) this.mEditLayer;
                            definition2.DefinitionExpression = str8;
                            str8 = "Task_ID<> '" + EditTask.TaskID + "' or TASK_ID IS NULL";
                            definition2 = (IFeatureLayerDefinition) this.mEditLayer2;
                            definition2.DefinitionExpression = str8;
                            if (pGroupLayer != null)
                            {
                                pGroupLayer.Add(this.mEditLayer);
                                pGroupLayer.Add(this.mEditLayer2);
                            }
                            else
                            {
                                this.mMap.AddLayer(this.mEditLayer2);
                                this.mMap.AddLayer(this.mEditLayer);
                            }
                        }
                        
                        string str9 = "";
                        string str10 = "";
                        layer2 = null;
                        IFeatureLayer layer4 = null;
                        IQueryFilter queryFilter = null;
                        IFeature pFeature = null;
                        if (EditTask.DistCode.Length == 6)
                        {
                            str9 = UtilFactory.GetConfigOpt().GetConfigValue("CountyLayerName");
                            str10 = UtilFactory.GetConfigOpt().GetConfigValue("CountyFieldCode");
                            layer2 = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, str9, true);
                        }
                        else if (EditTask.DistCode.Length == 9)
                        {
                            str9 = UtilFactory.GetConfigOpt().GetConfigValue("TownLayerName");
                            str10 = UtilFactory.GetConfigOpt().GetConfigValue("TownFieldCode");
                            layer2 = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, str9, true);
                        }
                        else if (EditTask.DistCode.Length == 12)
                        {
                            str9 = UtilFactory.GetConfigOpt().GetConfigValue("VillageLayerName");
                            str10 = UtilFactory.GetConfigOpt().GetConfigValue("VillageFieldCode");
                            layer2 = GISFunFactory.LayerFun.FindLayer(this.mMap as IBasicMap, str9, true);
                        }
                        if (layer2 != null)
                        {
                            layer4 = layer2 as IFeatureLayer;
                            queryFilter = new QueryFilterClass {
                                WhereClause = str10 + "='" + EditTask.DistCode + "'"
                            };
                            pFeature = layer4.Search(queryFilter, false).NextFeature();
                            GISFunFactory.FeatureFun.ZoomToFeature(this.mHookHelper.FocusMap, pFeature);
                            if (seldist)
                            {
                                this.mMap.SelectFeature(layer4, pFeature);
                            }
                            (this.mHookHelper.Hook as IMapControl2).FlashShape(pFeature.Shape, 3, 300, null);
                        }
                        else
                        {
                            IQueryFilter filter2 = new QueryFilterClass {
                                WhereClause = "Task_ID= '" + EditTask.TaskID + "'"
                            };
                            if (this.mEditLayer.FeatureClass.FeatureCount(filter2) > 0)
                            {
                                IFeatureCursor cursor = this.mEditLayer.FeatureClass.Search(filter2, false);
                                IFeature feature2 = cursor.NextFeature();
                                IGeometry shapeCopy = feature2.ShapeCopy;
                                if (shapeCopy.SpatialReference != this.mMap.SpatialReference)
                                {
                                    shapeCopy.Project(this.mMap.SpatialReference);
                                    shapeCopy.SpatialReference = this.mMap.SpatialReference;
                                }
                                IEnvelope envelope = shapeCopy.Envelope;
                                while (feature2 != null)
                                {
                                    shapeCopy = feature2.ShapeCopy;
                                    if (shapeCopy.SpatialReference != this.mMap.SpatialReference)
                                    {
                                        shapeCopy.Project(this.mMap.SpatialReference);
                                        shapeCopy.SpatialReference = this.mMap.SpatialReference;
                                    }
                                    if (envelope.XMin > shapeCopy.Envelope.XMin)
                                    {
                                        envelope.XMin = shapeCopy.Envelope.XMin;
                                    }
                                    if (envelope.YMin > shapeCopy.Envelope.YMin)
                                    {
                                        envelope.YMin = shapeCopy.Envelope.YMin;
                                    }
                                    if (envelope.XMax < shapeCopy.Envelope.XMax)
                                    {
                                        envelope.XMax = shapeCopy.Envelope.XMax;
                                    }
                                    if (envelope.YMax < shapeCopy.Envelope.YMax)
                                    {
                                        envelope.YMax = shapeCopy.Envelope.YMax;
                                    }
                                    feature2 = cursor.NextFeature();
                                }
                                envelope.Expand(1.1, 1.1, false);
                                this.mHookHelper.ActiveView.FullExtent = envelope;
                            }
                            else
                            {
                                IDataset featureClass = this.mEditLayer.FeatureClass as IDataset;
                                IGeoDataset dataset6 = featureClass as IGeoDataset;
                                this.mHookHelper.ActiveView.FullExtent = dataset6.Extent;
                            }
                        }
                        this.mHookHelper.ActiveView.Refresh();
                    }
                }
            }
            catch (Exception exception2)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "OpenTask", exception2.GetHashCode().ToString(), exception2.Source, exception2.Message, "", "", "");
            }
        }

        private void PopupContainerEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void PopupContainerEdit1_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            this.popupContainerDesi2.Width = this.popupEditQKind.Width;
            this.popupContainerDesi2.Refresh();
        }

        private void popupContainerEdit2_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupContainerEdit2_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            this.popupContainerDist2.Width = this.popupEditQDist.Width;
            this.popupContainerDist2.Refresh();
        }

        private void popupContainerEdit2_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void popupEditDist_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupEditDist_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupEditDist_TextChanged(object sender, EventArgs e)
        {
            if (this.popupEditDist.Text.Trim() == "")
            {
                this.simpleButtonEditOK.Enabled = false;
            }
            else
            {
                this.simpleButtonEditOK.Enabled = true;
            }
            this.textName2.Text = this.popupEditDist.Text + EditTask.TaskYear + "年" + this.popupEditKind.Text + "作业设计";
        }

        private void popupEditKind_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupEditKind_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
        }

        private void popupEditKind_TextChanged(object sender, EventArgs e)
        {
            if (this.popupEditKind.Text.Trim() == "")
            {
                this.simpleButtonEditOK.Enabled = false;
            }
            else
            {
                this.simpleButtonEditOK.Enabled = true;
            }
            this.textName2.Text = this.popupEditDist.Text + EditTask.TaskYear + "年" + this.popupEditKind.Text + "作业设计";
        }

        private void popupEditQDist_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void popupEditQKind_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void SetLayerFilter(bool flag)
        {
            try
            {
                if (flag)
                {
                    string str = "Task_ID= '" + EditTask.TaskID + "'";
                    IFeatureLayerDefinition mEditLayer = (IFeatureLayerDefinition) this.mEditLayer;
                    mEditLayer.DefinitionExpression = str;
                    str = "Task_ID<> '" + EditTask.TaskID + "' or TASK_ID IS NULL";
                    mEditLayer = (IFeatureLayerDefinition) this.mEditLayer2;
                    mEditLayer.DefinitionExpression = str;
                    this.mHookHelper.ActiveView.Refresh();
                }
                else
                {
                    string str2 = "";
                    IFeatureLayerDefinition definition2 = (IFeatureLayerDefinition) this.mEditLayer;
                    definition2.DefinitionExpression = str2;
                    this.mHookHelper.ActiveView.Refresh();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "SetLayerFilter", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonAddNew_Click(object sender, EventArgs e)
        {
            this.panelKind.Enabled = true;
            this.panelEdit.Visible = true;
            this.labelInfo2.Visible = false;
            this.labelEdit.Text = "    新增";
            this.textName2.Text = "";
            this.popupEditKind.Text = "";
            this.simpleButtonEditOK.Enabled = false;
            this.simpleButtonOpen.Enabled = false;
        }

        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.GetFocusedDataSourceRowIndex() != -1)
                {
                  T_EDITTASK_ZT_Mid prjMid=  m_projectList[this.gridView1.GetFocusedDataSourceRowIndex()];
                    if (MessageBox.Show("确定删除伐区作业设计【" +prjMid.taskname+"】,相关伐区班块都将被删除。", "删除伐区作业设计", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                    {
                       
                        if(TaskService.Delete(prjMid.ID)!=-1)
                        {
                            this.FindProject();
                            this.labelInfo.Text = "    删除成功";
                            this.labelInfo.Visible = true;
                        }
                        else
                        {
                            this.labelInfo.Text = "    删除失败";
                            this.labelInfo.Visible = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "simpleButtonDelete_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.gridView1.RowCount == 0) && (this.m_projectList.Count == 0))
                {
                    bool flag = false;
                    IFeatureLayer layer = EditTask.UnderLayers[2] as IFeatureLayer;
                    if (layer.FeatureClass.FeatureCount(null) > 0)
                    {
                        if (MessageBox.Show("确定清空所有伐区设计图斑?", "删除伐区作业设计", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                        flag = true;
                    }
                    else
                    {
                        layer = EditTask.UnderLayers[0] as IFeatureLayer;
                        if (layer.FeatureClass.FeatureCount(null) > 0)
                        {
                            if (MessageBox.Show("确定清空所有伐区设计图斑?", "删除伐区作业设计", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        this.DeleteEditLayerFeature("");
                        this.FindProject();
                        this.mHookHelper.ActiveView.Refresh();
                    }
                }
                if (MessageBox.Show("确定删除所有伐区作业设计,相关伐区设计班块都将被删除。", "删除伐区作业设计", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
                {
                    for (int i = 0; i < this.m_projectList.Count; i++)
                    {
                       
                        if (PM.Delete(m_projectList[i].ID))
                        {
                            this.FindProject();
                            this.labelInfo.Text = "    删除成功";
                            this.labelInfo.Visible = true;
                        }
                        else
                        {
                            this.labelInfo.Text = "    删除失败";
                            this.labelInfo.Visible = true;
                        }
                    }
                    this.DeleteEditLayerFeature("");
                    this.FindProject();
                    this.mHookHelper.ActiveView.Refresh();
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "simpleButtonDeleteAll_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.labelEdit.Text = "    修改";
                int focusedRowHandle = this.gridView1.FocusedRowHandle;
                if (focusedRowHandle != -1)
                {
                    T_EDITTASK_ZT_Mid mid=m_projectList[focusedRowHandle];
                    this.popupEditKind.Text = "";
                    this.textName2.Text = "";
                    this.panelKind.Enabled = true;
                    this.panelEdit.Visible = true;
                    this.labelInfo2.Visible = false;
                    string str = mid.taskkind;
                    str = str.Substring(2, str.Length - 2);
                    IList<T_DESIGNKIND_Mid> lst=PM.FindByKindAndCode(str, mKindCode);
                    if (lst.Count > 0)
                    {
                        this.popupEditKind.Text = lst[0].name;
                        this.textName2.Text = mid.taskname;
                    }
                    this.simpleButtonEditOK.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "TaskManage.UserControlDesignList", "simpleButtonEdit_Click", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
            }
        }

        private void simpleButtonEditCancle_Click(object sender, EventArgs e)
        {
            this.panelEdit.Visible = false;
            this.labelInfo2.Visible = false;
            if ((this.m_projectList.Count > 0) && (this.gridView1.SelectedRowsCount > -1))
            {
                this.simpleButtonOpen.Enabled = true;
            }
        }

        private void simpleButtonEditOK_Click(object sender, EventArgs e)
        {
            if (this.labelEdit.Text.Contains("新增"))
            {
                if (this.CreateProject())
                {
                    this.FindProject();
                    this.gridView1.SelectCell(m_projectList.Count - 1, this.gridView1.Columns[0]);
                    this.labelInfo2.Text = "    创建成功";
                    this.labelInfo2.Visible = true;
                    int num = 0;
                    for (int i = 0; i < 0x186a0; i++)
                    {
                        num++;
                    }
                    this.panelEdit.Visible = false;
                    this.labelInfo2.Visible = false;
                }
                else
                {
                    this.labelInfo2.Text = "    创建失败";
                    this.labelInfo2.Visible = true;
                }
            }
            else if (this.labelEdit.Text.Contains("修改"))
            {
                int focusedRowHandle = this.gridView1.FocusedRowHandle;
                if (focusedRowHandle != -1)
                {
                    T_EDITTASK_ZT_Mid mid=m_projectList[focusedRowHandle];
                    mid.taskname = this.textName2.Text.Trim();
                    bool flag = PM.TaskService.Edit(mid);
                    if (flag)
                    {
                        this.FindProject();
                        this.gridView1.SelectCell(focusedRowHandle, this.gridView1.Columns[0]);
                        this.labelInfo2.Text = "    修改成功";
                        this.labelInfo2.Visible = true;
                        int num4 = 0;
                        for (int j = 0; j < 0x186a0; j++)
                        {
                            num4++;
                        }
                        this.panelEdit.Visible = false;
                        this.labelInfo2.Visible = false;
                    }
                    else
                    {
                        this.labelInfo2.Text = "    修改失败";
                        this.labelInfo2.Visible = true;
                    }
                }
            }
        }

 

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.textName.Text = "";
            this.textName.Tag = null;
            this.popupEditQKind.Text = "";
            this.popupEditQKind.Tag = "";
            this.popupEditQDist.Text = "";
            this.popupEditQDist.Tag = "";
            this.dateEdit1.Text = "";
            this.dateEdit2.Text = "";
        }

        private void tListDesignKind_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node != null)
            {
                this.popupEditKind.Text = e.Node.GetDisplayText(0);
                this.popupEditKind.Tag = e.Node.Tag;
            }
        }

        private void tListDesignKind_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void tListDesignKind2_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node != null)
            {
                this.popupEditQKind.Text = e.Node.GetDisplayText(0);
                this.popupEditQKind.Tag = e.Node.Tag;
            }
        }

        private void tListDesignKind2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void tListDist_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node != null)
            {
                this.popupEditDist.Text = e.Node.GetDisplayText(0);
                this.popupEditDist.Tag = e.Node.Tag;
            }
        }

        private void tListDist_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void tListDist2_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node != null)
            {
                this.popupEditQDist.Text = e.Node.GetDisplayText(0);
                this.popupEditQDist.Tag = e.Node.Tag;
            }
        }

        private void tListDist2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void simpleButtonOpen_Click(object sender, EventArgs e)
        {

        }
    }
}

