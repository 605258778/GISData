namespace TopologyCheck.Error
{
    using ESRI.ArcGIS.Geodatabase;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using TopologyCheck.Checker;
    using Utilities;

    public class ErrorTable
    {
        private static DataTable _table;
        private const string mClassName = "TopologyCheck.Error.ErrorTable";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        public ErrorTable()
        {
            if (_table == null)
            {
                _table = this.CreateErrorTable();
            }
        }

        public bool AddErr(IFeatureCursor pCursor)
        {
            bool flag = false;
            if (_table == null)
            {
                return false;
            }
            if (this.ClearTable(ErrType.Area))
            {
                IFeature feature = null;
                string str = 5.ToString();
                while ((feature = pCursor.NextFeature()) != null)
                {
                    flag = true;
                    DataRow row = _table.NewRow();
                    row["FeatureID"] = feature.OID;
                    row["ErrDes"] = "面积过小";
                    row["ErrType"] = str;
                    _table.Rows.Add(row);
                }
            }
            return flag;
        }

        public void AddErr(List<ErrorEntity> pErrEntity, ErrType pTy)
        {
            if (_table != null)
            {
                if ((pErrEntity == null) || (pErrEntity.Count < 1))
                {
                    this.ClearTable(pTy);
                }
                else if (this.ClearTable(pTy))
                {
                    string str = ((int) pErrEntity[0].ErrType).ToString();
                    foreach (ErrorEntity entity in pErrEntity)
                    {
                        DataRow row = _table.NewRow();
                        row["FeatureID"] = entity.FeatureID;
                        row["ErrDes"] = entity.ErrMsg;
                        row["ErrPos"] = entity.ErrPos;
                        row["ErrType"] = str;
                        _table.Rows.Add(row);
                    }
                }
            }
        }

        public void AddGapErr(IList<GapErrorEntity> pErrEntity)
        {
            if (_table != null)
            {
                if ((pErrEntity == null) || (pErrEntity.Count < 1))
                {
                    this.ClearTable(ErrType.Gap);
                }
                else if (this.ClearTable(ErrType.Gap))
                {
                    foreach (GapErrorEntity entity in pErrEntity)
                    {
                        DataRow row = _table.NewRow();
                        row["FeatureID"] = entity.FeatureID;
                        row["ErrDes"] = "缝隙";
                        row["ErrType"] = ErrType.Gap;
                        row["Geometry"] = entity.ErrGeo;
                        _table.Rows.Add(row);
                    }
                }
            }
        }

        public void AddMultiOverlapErr(IFeatureClass pFClass)
        {
            if (_table != null)
            {
                ErrType multiOverlap = ErrType.MultiOverlap;
                if (pFClass == null)
                {
                    this.ClearTable(multiOverlap);
                }
                else if (this.ClearTable(multiOverlap))
                {
                    string str = ((int) multiOverlap).ToString();
                    IFeature feature = null;
                    int index = -1;
                    for (int i = 0; i < pFClass.Fields.FieldCount; i++)
                    {
                        if (pFClass.Fields.get_Field(i).Name.Contains("ID_"))
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index >= 0)
                    {
                        int num3 = 0;
                        IFeatureCursor o = pFClass.Search(null, false);
                        while ((feature = o.NextFeature()) != null)
                        {
                            num3++;
                            DataRow row = _table.NewRow();
                            row["FeatureID"] = feature.get_Value(index).ToString();
                            row["ErrDes"] = "重叠";
                            row["ErrType"] = str;
                            row["Geometry"] = feature.ShapeCopy;
                            _table.Rows.Add(row);
                        }
                        Marshal.ReleaseComObject(o);
                    }
                }
            }
        }

        public void AddTopoErr(IFeatureClass pFClass, ErrType pTy)
        {
            if (_table != null)
            {
                if (pFClass == null)
                {
                    this.ClearTable(pTy);
                }
                else if (this.ClearTable(pTy))
                {
                    string str = ((int) pTy).ToString();
                    IFeature feature = null;
                    int index = -1;
                    int num2 = pFClass.Fields.FindField("OBJECTID");
                    if (num2 < 0)
                    {
                        for (int i = 0; i < pFClass.Fields.FieldCount; i++)
                        {
                            if (pFClass.Fields.get_Field(i).Name.Contains("ID_"))
                            {
                                index = i;
                            }
                        }
                    }
                    else
                    {
                        index = num2;
                    }
                    if ((pTy != ErrType.OverLap) || (index >= 0))
                    {
                        int num4 = 0;
                        string str2 = "";
                        IFeatureCursor o = pFClass.Search(null, false);
                        while ((feature = o.NextFeature()) != null)
                        {
                            num4++;
                            DataRow row = _table.NewRow();
                            switch (pTy)
                            {
                                case ErrType.OverLap:
                                {
                                    if ((num4 % 2) != 1)
                                    {
                                        break;
                                    }
                                    str2 = feature.get_Value(index).ToString();
                                    continue;
                                }
                                case ErrType.Gap:
                                {
                                    row["FeatureID"] = "错误区域" + num4.ToString();
                                    row["ErrDes"] = "缝隙";
                                    row["ErrType"] = str;
                                    row["Geometry"] = feature.ShapeCopy;
                                    _table.Rows.Add(row);
                                    continue;
                                }
                                default:
                                {
                                    continue;
                                }
                            }
                            row["FeatureID"] = str2 + "," + feature.get_Value(index).ToString();
                            row["ErrDes"] = "重叠";
                            row["ErrType"] = str;
                            row["Geometry"] = feature.ShapeCopy;
                            _table.Rows.Add(row);
                        }
                        Marshal.ReleaseComObject(o);
                    }
                }
            }
        }

        public bool ClearTable()
        {
            _table.Clear();
            return true;
        }

        public bool ClearTable(ErrType pTy)
        {
            if (_table == null)
            {
                return false;
            }
            int num = (int) pTy;
            for (int i = _table.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = _table.Rows[i];
                if (row["ErrType"].ToString() == num.ToString())
                {
                    _table.Rows.RemoveAt(i);
                }
            }
            return true;
        }

        private DataTable CreateErrorTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("FeatureID", typeof(string));
            table.Columns.Add("ErrDes", typeof(string));
            table.Columns.Add("ErrPos", typeof(string));
            table.Columns.Add("ErrType", typeof(int));
            table.Columns.Add("Geometry", typeof(object));
            return table;
        }

        public bool DeleteErrEntity(string pFeatureId)
        {
            if (_table == null)
            {
                return false;
            }
            for (int i = _table.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = _table.Rows[i];
                if (row["FeatureID"].ToString() == pFeatureId)
                {
                    _table.Rows.RemoveAt(i);
                }
            }
            return true;
        }

        public bool DeleteErrEntity(string pFeatureId, ErrType pTy)
        {
            if (_table == null)
            {
                return false;
            }
            for (int i = _table.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = _table.Rows[i];
                if (row["FeatureID"].ToString() == pFeatureId)
                {
                    int num2 = (int) pTy;
                    if (row["ErrType"].ToString() == num2.ToString())
                    {
                        _table.Rows.RemoveAt(i);
                    }
                }
            }
            return true;
        }

        public DataTable GetTable(List<ErrType> pTy)
        {
            if (_table == null)
            {
                return null;
            }
            string filterExpression = "ErrType=";
            foreach (ErrType type in pTy)
            {
                filterExpression = filterExpression + ((int) type).ToString() + " or ErrType=";
            }
            filterExpression = filterExpression.Remove(filterExpression.Length - 12);
            DataRow[] rowArray = _table.Select(filterExpression);
            if (rowArray.Length < 1)
            {
                return null;
            }
            DataTable table = _table.Clone();
            for (int i = 0; i < rowArray.Length; i++)
            {
                table.Rows.Add(rowArray[i].ItemArray);
            }
            return table;
        }

        public DataTable GetTable(ErrType pTy)
        {
            if (_table == null)
            {
                return null;
            }
            string filterExpression = "ErrType=" + ((int) pTy).ToString();
            DataRow[] rowArray = _table.Select(filterExpression);
            if (rowArray.Length < 1)
            {
                return null;
            }
            DataTable table = _table.Clone();
            for (int i = 0; i < rowArray.Length; i++)
            {
                table.Rows.Add(rowArray[i].ItemArray);
            }
            return table;
        }
    }
}

