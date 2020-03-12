using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Columns;
using td.db.mid.sys;
using td.db.orm;
using td.logic.sys;

namespace td.forest.task.linker
{
    /// <summary>
    /// 将数据库中的字段对应到界面上
    /// </summary>
    public class DB2GridViewManager
    {
        public DB2GridViewManager() { }

        public MetaDataManager MDM
        {
            get { return DBServiceFactory<MetaDataManager>.Service; }
        }
        public void InitViewColumn(string tableName,DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
             IList<T_SYS_META_FIELDS_Mid> lst=MDM.FindColumnsByTable(tableName);
             InitViewColumn(lst, gridView);
        }
        public void InitViewColumn(IList<T_SYS_META_FIELDS_Mid> colList,DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            foreach (T_SYS_META_FIELDS_Mid mid in colList)
            {
                GridColumn col1 = gridView.Columns.Add();
                col1.Caption = mid.FIEL_AL;
                col1.FieldName = mid.FIEL_NA;
                col1.Name = mid.FIEL_NA + "_" + mid.TAB_ID;
                col1.Visible = true;
            }
        }
        public void InitViewColumn(T_SYS_META_FIELDS_Mid mid, DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
           
                GridColumn col1 = gridView.Columns.Add();
                col1.Caption = mid.FIEL_AL;
                col1.FieldName = mid.FIEL_NA;
                col1.Name = mid.FIEL_NA + "_" + mid.TAB_ID;
                col1.Visible = true;
            
        }
        public void InitViewColumn(string caption,string fieldName, DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {

            GridColumn col1 = gridView.Columns.Add();
            col1.Caption = caption;
            col1.FieldName = fieldName;
            col1.Name = fieldName + "_" + 1000;
            col1.Visible = true;

        }
    }
}
