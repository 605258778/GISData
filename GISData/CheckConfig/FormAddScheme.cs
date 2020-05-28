using GISData.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckConfig
{
    public partial class FormAddScheme : Form
    {
        public FormAddScheme()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectDB db = new ConnectDB();
            string isdefault = this.checkBox1.Checked ? "1" : "0";
            Boolean result = db.Insert("insert into GISDATA_SCHEME (SCHEME_NAME,IS_DEFAULT) values ('" + this.textBox1.Text + "','"+isdefault+"')");
            if (result) 
            {
                if (isdefault == "1") 
                {
                    db.Update("update GISDATA_SCHEME set IS_DEFAULT= '0' where SCHEME_NAME <>'" + this.textBox1.Text + "'");
                }
                MessageBox.Show("保存成功！");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FormAddScheme_Load(object sender, EventArgs e)
        {

        }
    }
}
