﻿using DevExpress.XtraReports.UI;
using DevExpress.XtraTreeList;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using GISData.Common;
using GISData.DataCheck.CheckDialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopologyCheck.Error;

namespace GISData.DataCheck
{
    public partial class FormCheckMain : Form
    {
        private IHookHelper m_hookHelper = null;
        public FormCheckMain()
        {
            InitializeComponent();
        }
        public FormCheckMain(IHookHelper m_hookHelper)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            BindScheme();
            this.m_hookHelper = m_hookHelper;
        }
        private FormAttrDia AttrDia;
        private FormTopoDia TopoDia;
        private FormReportDia ReportDia;
        private List<CheckBox> CheckBoxArr = new List<CheckBox>();
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        /// <summary>
        /// 加载质检方案数据源
        /// </summary>
        private void BindScheme()
        {
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select * from GISDATA_SCHEME order by IS_DEFAULT desc");
            comboBoxScheme.DataSource = result;
            comboBoxScheme.DisplayMember = "SCHEME_NAME";
            comboBoxScheme.ValueMember = "SCHEME_NAME";
        }

        private void FormCheckMain_Load(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.ColumnStyles[1].Width = 0;
            this.tabControl1.Width = 1200;
            this.Width = 1200;
            loadStep();
        }
        /// <summary>
        /// 加载检查项
        /// </summary>
        public void loadStep()
        {
            //this.splitContainer1.Panel2.Controls.RemoveByKey("");
            this.tabControl1.Controls.Clear();
            ConnectDB db = new ConnectDB();
            DataTable result = db.GetDataBySql("select * from GISDATA_CONFIGSTEP where IS_CONFIG = '1' and SCHEME = '"+this.comboBoxScheme.Text.ToString()+"' order by STEP_NO");
            DataRow[] dr = result.Select("1=1");
            for (int i = 0; i < dr.Length; i++)
            {
                string stepNo = dr[i]["STEP_NO"].ToString();
                string stepName = dr[i]["STEP_NAME"].ToString();
                string stepType = dr[i]["STEP_TYPE"].ToString();
                string isConfig = dr[i]["IS_CONFIG"].ToString();
                CheckBox cb = new CheckBox();
                cb.Text = "";
                TabPage tp = new TabPage();

                tp.Name = stepType;
                cb.Name = stepType;
                cb.Location = new Point(7, (i+1)*60-39);
                cb.Size = new Size(18, 18);
                tp.AccessibleDescription = stepType;//AccessibleDescription属性暂赋值为质检类型
                tp.SendToBack();
                tp.Text = "第" + stepNo + "步(" + stepName + ")";
                tp.Tag = stepNo;
                cb.Tag = stepNo;
                cb.Click += (se, a) => checkState(se,a,stepType);
                this.splitContainer1.Panel2.Controls.Add(cb);
                this.tabControl1.Controls.Add(tp);
                this.tabControl1.Dock = DockStyle.Fill; ;
                cb.BringToFront();
                this.CheckBoxArr.Add(cb);
                string scheme = this.comboBoxScheme.Text;
                if (stepType == "结构检查")
                {
                    //FormStructureDia fsa = new FormStructureDia(stepNo, cb);
                    //StructureDia = fsa;
                    //fsa.Dock = DockStyle.Fill;
                    //ShowForm(tp, fsa);
                }
                else if (stepType == "属性检查")
                {
                    FormAttrDia attr = new FormAttrDia(stepNo, cb,scheme, this.checkBox1, this.gridControlError);
                    AttrDia = attr;
                    attr.Dock = DockStyle.Fill;
                    ShowForm(tp, attr);
                }
                else if (stepType == "图形检查")
                {
                    FormTopoDia topo = new FormTopoDia(stepNo, cb, scheme, this.checkBox1, this.gridControlError);
                    TopoDia = topo;
                    topo.Dock = DockStyle.Fill;
                    ShowForm(tp, topo);
                }
                else if (stepType == "统计报表")
                {
                    FormReportDia report = new FormReportDia(stepNo, cb,scheme);
                    ReportDia = report;
                    report.Dock = DockStyle.Fill;
                    ShowForm(tp, report);
                }
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void checkState(object sender, EventArgs e,string type) 
        {
            CheckBox CheckBoxSender = (CheckBox)sender;
            if (type == "结构检查")
            {
                //if (CheckBoxSender.Checked)
                //{
                //    StructureDia.SelectAll();
                //}
                //else {
                //    StructureDia.UnSelectAll();
                //}
               
            }
            else if (type == "属性检查")
            {
                if (CheckBoxSender.Checked)
                {
                    AttrDia.SelectAll();
                }
                else
                {
                    AttrDia.UnSelectAll();
                }
            }
            else if (type == "图形检查")
            {
                if (CheckBoxSender.Checked)
                {
                    TopoDia.SelectAll();
                }
                else
                {
                    TopoDia.UnSelectAll();
                }
            }
            else if (type == "统计报表")
            {
                if (CheckBoxSender.Checked)
                {
                    ReportDia.SelectAll();
                }
                else
                {
                    ReportDia.UnSelectAll();
                }
            }
        }

        /// <summary>
        /// 重绘tabControl，使其竖向展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle tabArea = tabControl1.GetTabRect(e.Index);//主要是做个转换来获得TAB项的RECTANGELF
            RectangleF tabTextArea = (RectangleF)(tabControl1.GetTabRect(e.Index));
            tabTextArea.Location = new Point(30, int.Parse(tabTextArea.Top.ToString()));
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();//封装文本布局信息
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            Font font = this.tabControl1.Font;
            SolidBrush brush = new SolidBrush(Color.Black);//绘制边框的画笔
            g.DrawString(((TabControl)(sender)).TabPages[e.Index].Text, font, brush, tabTextArea, sf);
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
        public void ShowForm(TabPage page, Form frm)
        {
            lock (this)
            {
                try
                {
                    page.Controls.Clear();
                    frm.Dock = DockStyle.Fill;
                    frm.TopLevel = true;
                    frm.MdiParent = this;
                    frm.Parent = page;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Show();
                }
                catch (System.Exception ex)
                {
                    //
                }
            }
        }
        /// <summary>
        /// 开始检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCheckStar_Click(object sender, EventArgs e)
        {
            if (AttrDia.attrErrorTable!= null)
                AttrDia.attrErrorTable.Clear();
            if (TopoDia.topoErrorTable != null)
                TopoDia.topoErrorTable.Clear();
            foreach (CheckBox item in CheckBoxArr)
            {
                if(item.CheckState == CheckState.Checked)
                {
                    if (item.Name == "结构检查")
                    {

                        //StructureDia.doCheckStructure();
                    }
                    else if (item.Name == "属性检查")
                    {
                        AttrDia.doCheckAttr();
                    }
                    else if (item.Name == "图形检查")
                    {
                        TopoDia.doCheckTopo(this.m_hookHelper);
                    }
                    else if (item.Name == "统计报表")
                    {
                        ReportDia.DoReport();
                    }
                }
            }
            MessageBox.Show("检查完成", "提示");
        }
        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.Width = this.Width+500;
                this.tableLayoutPanel1.ColumnStyles[0].Width = this.Width - 500;
                this.tableLayoutPanel1.ColumnStyles[1].Width = 500;
            }
            else 
            {
                this.Width = this.Width - 500;
                this.tableLayoutPanel1.ColumnStyles[0].Width = this.Width - 500;
                this.tableLayoutPanel1.ColumnStyles[1].Width = 0;
            }
        }
        /// <summary>
        /// 双击查看单个要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlError_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewError.ViewCaption == "拓扑检查") 
            {
                IActiveView activeView = this.m_hookHelper.ActiveView;
                ErrManager.ClearElement(activeView);
                var index = this.gridViewError.GetFocusedDataSourceRowIndex();//获取数据行的索引值，从0开始
                DataRowView row = (DataRowView)this.gridViewError.GetRow(index);//获取选中行的那个单元格的值
                ErrType type = (ErrType)int.Parse(row["ErrType"].ToString());
                int oid = int.Parse(row["FeatureID"].ToString());
                string ErrPos = row["ErrPos"].ToString();
                object ee = row["Geometry"];

                CommonClass common = new CommonClass();
                IFeatureLayer pLayer = common.GetLayerByName(this.gridViewError.NewItemRowText);
                IEnumLayer enumlayer = activeView.FocusMap.Layers;
                ILayer itemLayer = enumlayer.Next();
                Boolean existsLayer = false;
                while (itemLayer != null)
                {
                    if (itemLayer.Name == this.gridViewError.NewItemRowText)
                    {
                        existsLayer = true;
                        break;
                    }
                    itemLayer = enumlayer.Next();
                }
                if (!existsLayer)
                {
                    activeView.FocusMap.AddLayer(pLayer);
                }

                ESRI.ArcGIS.Geometry.ISpatialReference geo = row["Geometry"] as ESRI.ArcGIS.Geometry.ISpatialReference;
                switch (type)
                {
                    case ErrType.OverLap:
                        ErrManager.ZoomToErr(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"]);
                        ErrManager.AddErrTopoElement(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"], oid);
                        //ErrManager.ZoomToErr(activeView, pFeature);
                        //ErrManager.AddErrTopoElement(activeView, (IGeometry)row["Geometry"], ref list2);
                        break;

                    case ErrType.Gap:
                        ErrManager.ZoomToErr(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"]);
                        ErrManager.AddErrTopoElement(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"], oid);
                        break;
                    case ErrType.MultiPart:
                        ErrManager.ZoomToErr(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"]);
                        ErrManager.AddErrTopoElement(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"], oid);
                        break;
                    case ErrType.SelfIntersect:
                        string[] strArray = ErrPos.Split(new char[] { ';' });
                        ESRI.ArcGIS.Geometry.IPointCollection pPointCollection1 = new ESRI.ArcGIS.Geometry.MultipointClass();
                        ESRI.ArcGIS.Geometry.IGeometry pGeo = null;
                        foreach (string str in strArray)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string[] strArray2 = str.Split(new char[] { ',' });
                                double pX = double.Parse(strArray2[0]);
                                double pY = double.Parse(strArray2[1]);
                                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.PointClass();
                                point.X = pX;
                                point.Y = pY;
                                pPointCollection1.AddPoint(point);
                            }
                        }
                        ESRI.ArcGIS.Geometry.IMultipoint pMuPoint = pPointCollection1 as ESRI.ArcGIS.Geometry.IMultipoint;
                        pGeo = pMuPoint as ESRI.ArcGIS.Geometry.IGeometry;
                        List<IElement> list2 = new List<IElement>();
                        ErrManager.ZoomToErr(activeView, pGeo);
                        ErrManager.AddErrPointElement(activeView, ErrPos, geo, oid);
                        break;
                    default:
                        ErrManager.ZoomToErr(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"]);
                        ErrManager.AddErrTopoElement(activeView, (ESRI.ArcGIS.Geometry.IGeometry)row["Geometry"], oid);
                        break;
                }
                activeView.Refresh();
            }
            else if (this.gridViewError.ViewCaption == "属性检查") 
            {
                string tablename = this.gridViewError.NewItemRowText;
                IActiveView activeView = this.m_hookHelper.ActiveView;
                IQueryFilter pQuery = new QueryFilterClass();
                int val = this.gridViewError.GetFocusedDataSourceRowIndex();
                DataRowView row = (DataRowView)this.gridViewError.GetRow(val);
                //设置高亮要素的查询条件  
                CommonClass common = new CommonClass();
                IFeatureLayer pLayer = common.GetLayerByName(tablename);
                IEnumLayer enumlayer = activeView.FocusMap.Layers;
                ILayer itemLayer = enumlayer.Next();
                Boolean existsLayer = false;
                while (itemLayer != null) 
                {
                    if (itemLayer.Name == tablename) 
                    {
                        existsLayer = true;
                        break;
                    }
                    itemLayer = enumlayer.Next();
                }
                if (!existsLayer) 
                {
                    activeView.FocusMap.AddLayer(pLayer);
                }
                int oid = int.Parse(row[pLayer.FeatureClass.OIDFieldName].ToString());
                pQuery.WhereClause = pLayer.FeatureClass.OIDFieldName + "=" + oid;
                IFeature feature = pLayer.FeatureClass.GetFeature(oid);
                IFeatureSelection pFeatSelection;
                pFeatSelection = pLayer as IFeatureSelection;
                pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
                activeView.Extent = feature.ShapeCopy.Envelope;
                activeView.Refresh();
            }
            
        }
        /// <summary>
        /// 选择方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadStep();
        }
        /// <summary>
        /// 检查完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            FormAcceptance acceptance = new FormAcceptance(ReportDia,AttrDia,TopoDia);
            acceptance.ShowDialog();
            if (acceptance.DialogResult == DialogResult.OK) 
            {
                doAcceptance(acceptance.ModelName);
            }
        }
        /// <summary>
        /// 生成验收单
        /// </summary>
        /// <param name="modelName"></param>
        private void doAcceptance(string modelName)
        {
            XtraReport mReport = new XtraReport();
            mReport.LoadLayout(Application.StartupPath + "\\Report\\" + modelName); //报表模板文件

            DataTable taskError = new DataTable();
            if (ReportDia.wwcxmTable != null)
            {
                taskError = ReportDia.wwcxmTable;
            }
            taskError.TableName = "taskError";
            DataTable checkError = new DataTable();
            DataTable attrError = AttrDia.attrErrorTable;
            if (attrError != null)
            {
                checkError = attrError;
            }
            DataTable topotable = TopoDia.topoErrorTable;
            if (topotable != null)
            {
                checkError.Merge(topotable);
                checkError.TableName = "checkError";
            }

            //查找组件
            DetailReportBand DetailReport = mReport.FindControl("DetailReport", true) as DetailReportBand;
            DetailReportBand DetailReport1 = mReport.FindControl("DetailReport1", true) as DetailReportBand;
            XRLabel lxr = mReport.FindControl("lxr", true) as XRLabel;//联系人
            XRLabel lxdh = mReport.FindControl("lxdh", true) as XRLabel;//联系方式
            XRLabel jcr = mReport.FindControl("jcr", true) as XRLabel;//检查人
            XRLabel gldwstring = mReport.FindControl("gldw", true) as XRLabel;//管理单位
            XRLabel startime = mReport.FindControl("startime", true) as XRLabel;//开始检查时间
            XRLabel printtime = mReport.FindControl("printtime", true) as XRLabel;//打印时间
            XRLabel checklog = mReport.FindControl("checklog", true) as XRLabel;//检查日志

            CommonClass common = new CommonClass();
            string gldw = common.GetConfigValue("GLDW");

            ConnectDB db = new ConnectDB();
            DataTable DT = db.GetDataBySql("select * from GISDATA_GLDW where GLDW = '" + gldw + "'");
            DataRow dr = DT.Select(null)[0];

            lxr.Text = dr["CONTACTS"].ToString();
            lxdh.Text = dr["TEL"].ToString();
            jcr.Text = common.GetConfigValue("USER") == "" ? "曾伟" : common.GetConfigValue("USER");
            gldwstring.Text = dr["GLDWNAME"].ToString();
            startime.Text = dr["STARTTIME"].ToString();
            //checklog.Text = dr["CHECKLOG"].ToString().Replace("br",Convert.ToChar(10).ToString());
            checklog.Text = dr["CHECKLOG"].ToString();
            printtime.Text = DateTime.Now.ToLocalTime().ToString();
            DetailReport.DataSource = taskError;
            DetailReport1.DataSource = checkError;
            mReport.ShowPreviewDialog();
        }
        /// <summary>
        /// 显示未通过项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked) 
            {
                FilterCondition _curFc = new FilterCondition(FilterConditionEnum.LessOrEqual, this.AttrDia.treeList1.Columns["ERROR"], 0);
                if (!this.AttrDia.treeList1.OptionsBehavior.EnableFiltering)
                    this.AttrDia.treeList1.OptionsBehavior.EnableFiltering = true;
                this.AttrDia.treeList1.FilterConditions.Clear();
                this.AttrDia.treeList1.FilterConditions.Add(_curFc);
                //this.TopoDia.gridView1.Columns["ERROR"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.LessOrEqual;
                this.TopoDia.gridView1.ActiveFilterString = "ERROR > 0";
            }
            else
            {
                this.AttrDia.treeList1.FilterConditions.Clear();
                this.TopoDia.gridView1.ActiveFilterString = null;
            }
        }
    }
}
