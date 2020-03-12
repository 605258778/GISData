namespace TaskManage
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class vGUIDGen
    {
        [DllImport("yqmap.dll", CharSet=CharSet.Ansi,CallingConvention=CallingConvention.Cdecl)]
        private static extern void GetCPUID_(StringBuilder pstr, int len);
        public static string GetCUPID()
        {
            try
            {
                StringBuilder pstr = new StringBuilder(50);
                GetCPUID_(pstr, 20);
                return pstr.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetIDEInfo()
        {
            try
            {
                StringBuilder strCode = new StringBuilder(50);
                GetIDEInfo_(strCode);
                return strCode.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        [DllImport("yqmap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetIDEInfo_(StringBuilder strCode);
        [DllImport("yqmap.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetMacByCmd_(string strCode);
    }
}

