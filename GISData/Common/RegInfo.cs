using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISData.Common
{
    public class RegInfo
    {
        public RegInfo()
        { }
        /// <summary>
        /// 所对应的课程类型
        /// </summary>
        private string regName;

        public string RegName
        {
            get { return regName; }
            set { regName = value; }
        }
 
        /// <summary>
        /// 书所对应的ISBN号
        /// </summary>
        private string regPath;

        public string RegPath
        {
            get { return regPath; }
            set { regPath = value; }
        }
 
        /// <summary>
        /// 书名
        /// </summary>
        private string regType;

        public string RegType
        {
            get { return regType; }
            set { regType = value; }
        }
 
        /// <summary>
        /// 作者
        /// </summary>
        private string regTable;

        public string RegTable
        {
            get { return regTable; }
            set { regTable = value; }
        }
    }
}
