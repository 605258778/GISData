﻿using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.Common
{
    public class CommonClass
    {
        /// <summary>
        /// 填充树形结构
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dt"></param>
        public void FillTree(TreeNode node, DataTable dt)
        {
            DataRow[] drr = dt.Select("PARENTID='" + node.Tag.ToString() + "'");
            if (drr.Length > 0)
            {
                for (int i = 0; i < drr.Length; i++)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = drr[i]["NAME"].ToString();
                    tnn.Tag = drr[i]["ID"].ToString();
                    if (drr[i]["PARENTID"].ToString() == node.Tag.ToString())
                    {
                        FillTree(tnn, dt);
                    }
                    node.Nodes.Add(tnn);
                }
            }
        }

        //取消节点选中状态之后，取消所有父节点的选中状态
        public void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        public void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes=currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            } 
        }
        /// <summary>
        /// DataGridView添加一列checkbox
        /// </summary>
        /// <param name="dg"></param>
        public void AddCheckBox(DataGridView dg)
        {
            //repositoryItemResourcesComboBox1 combox = new repositoryItemResourcesComboBox();
            DataGridViewCheckBoxColumn columncb = new DataGridViewCheckBoxColumn();
            columncb.HeaderText = "选择";
            columncb.Name = "cb_check";
            columncb.TrueValue = true;
            columncb.FalseValue = false;
            columncb.DataPropertyName = "IsChecked";
            dg.Columns.Insert(0, columncb);    //添加的checkbox在第一列
            //dg.Columns.Add(columncb);     //添加的checkbox在最后一列

        }
        /// <summary>
        /// 在DataGridView控件的CellMouseClick属性中：点击数据勾选上checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e, DataGridView UserGridView)
        {

            //checkbox 勾上
            if ((bool)UserGridView.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
            {
                UserGridView.Rows[e.RowIndex].Cells[0].Value = false;
            }
            else
            {
                UserGridView.Rows[e.RowIndex].Cells[0].Value = true;
            }

        }
        /// <summary>
        /// 设置CheckBox子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        public void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }
        /// <summary>
        /// 设置CheckBox父节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        public void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                if (b)
                {
                    node.ParentNode.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    node.ParentNode.CheckState = check;
                }
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        /// <summary>
        /// 为列头绘制CheckBox
        /// </summary>
        /// <param name="view">GridView</param>
        /// <param name="checkItem">RepositoryItemCheckEdit</param>
        /// <param name="fieldName">需要绘制Checkbox的列名</param>
        /// <param name="e">ColumnHeaderCustomDrawEventArgs</param>
        public static void DrawHeaderCheckBox(GridView view, RepositoryItemCheckEdit checkItem, string fieldName, ColumnHeaderCustomDrawEventArgs e)
        {
            /*说明：
             *参考：https://www.devexpress.com/Support/Center/Question/Details/Q354489
             *在CustomDrawColumnHeader中使用
             *eg：
             * private void gvCabChDetail_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
             * {
             * GridView _view = sender as GridView;
             * _view.DrawHeaderCheckBox(CheckItem, "Check", e);
             * }
             */
            if (e.Column != null && e.Column.FieldName.Equals(fieldName))
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(checkItem, e.Graphics, e.Bounds, getCheckedCount(view, fieldName) == view.DataRowCount);
                e.Handled = true;
            }
        }
        private static void DrawCheckBox(RepositoryItemCheckEdit checkItem, Graphics g, Rectangle r, bool Checked)
        {
            CheckEditViewInfo _info;
            CheckEditPainter _painter;
            ControlGraphicsInfoArgs _args;
            _info = checkItem.CreateViewInfo() as CheckEditViewInfo;
            _painter = checkItem.CreatePainter() as CheckEditPainter;
            _info.EditValue = Checked;

            _info.Bounds = r;
            _info.PaintAppearance.ForeColor = Color.Black;
            _info.CalcViewInfo(g);
            _args = new ControlGraphicsInfoArgs(_info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            _painter.Draw(_args);
            _args.Cache.Dispose();
        }
        private static int getCheckedCount(GridView view, string filedName)
        {
            int count = 0;
            for (int i = 0; i < view.DataRowCount; i++)
            {
                object _cellValue = view.GetRowCellValue(i, view.Columns[filedName]);
                if (_cellValue == null) continue;
                if (string.IsNullOrEmpty(_cellValue.ToString().Trim())) continue;
                bool _checkStatus = false;
                if (bool.TryParse(_cellValue.ToString(), out _checkStatus))
                {
                    if (_checkStatus)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
         /// 修改AppSettings中配置
         /// </summary>
         /// <param name="key">key值</param>
         /// <param name="value">相应值</param>
         public bool SetConfigValue(string key, string value)
         {
             try
             {
                 Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                 if (config.AppSettings.Settings[key] != null)
                     config.AppSettings.Settings[key].Value = value;
                 else
                     config.AppSettings.Settings.Add(key, value);
                 config.Save(ConfigurationSaveMode.Modified);
                 ConfigurationManager.RefreshSection("appSettings");
                 return true;
             }
             catch
             {
                 return false;
             }
         }
        /// <summary>
        /// 获取AppSettings中某一节点值
        /// </summary>
        /// <param name="key"></param>
        public string GetConfigValue(string key)
        {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                     return  config.AppSettings.Settings[key].Value;
                else
                  
                return string.Empty;
        }    
    }
}
