namespace TaskManage
{
    using System;
    using System.Management;

    internal class WMITool
    {
        public string GetCPUID()
        {
            try
            {
                ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
                string str = null;
                foreach (ManagementObject obj2 in instances)
                {
                    str = obj2.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public ManagementObject GetWMIObject(WMIObject objName)
        {
            if (objName.GetType() != Type.GetType("System.DBNull"))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From " + objName.ToString());
                if (searcher == null)
                {
                    return null;
                }
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        return (ManagementObject) enumerator.Current;
                    }
                }
            }
            return null;
        }

        public enum WMIObject
        {
            Win32_OperatingSystem,
            Win32_ComputerSystem,
            Win32_Process,
            Win32_BIOS,
            Win32_LogicalDisk,
            Win32_PhysicalMedia,
            Win32_PnPEntity,
            Win32_NetworkAdapterConfiguration
        }
    }
}

