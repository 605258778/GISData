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

namespace GISData.Login
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbtype = this.comboBox1.Text;
            string user = this.textBox1.Text;
            string password = this.textBox2.Text;
            if (dbtype == "本机")
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select * from GISDATA_USER WHERE USER ='" + user + "'");
                DataRow[] drs = dt.Select(null);
                if (drs.Length == 0)
                {
                    MessageBox.Show("该用户不存在");
                }
                else
                {
                    string pw = drs[0]["PASSWORD"].ToString();
                    if (password == pw)
                    {
                        this.Close();
                        this.DialogResult = DialogResult.OK;
                    }
                    else 
                    {
                        MessageBox.Show("密码错误");
                    }
                }
            }
           
        }
    }
}