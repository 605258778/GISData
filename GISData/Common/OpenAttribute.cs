using System;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using GISData.MainMap; 

namespace GISData.Common
{
    public sealed class OpenAttribute : BaseCommand
    {
        IMapControl3 m_mapControl;
        AxMapControl _MapControl;

        public OpenAttribute(AxMapControl pMapControl)
        {
            base.m_caption = "查看属性表";
            _MapControl = pMapControl;
        }

        public override void OnClick()
        {
            FormAttribute formtable = new FormAttribute(_MapControl, m_mapControl);
            formtable.Show();

        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }
    }  
}
