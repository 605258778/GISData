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

namespace GISData.User
{
    public partial class FormValidation : Form
    {
        public FormValidation()
        {
            InitializeComponent();
        }

        private void FormValidation_Load(object sender, EventArgs e)
        {
            CommonClass common = new CommonClass();
            string user = common.GetConfigValue("USER");
            this.textBox1.Text = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = this.textBox2.Text;
            if (password == "")
            {
                MessageBox.Show("请输入密码");
            }
            else 
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select * from GISDATA_USER WHERE USER ='" + this.textBox1.Text + "'");
                string role = dt.Select(null)[0]["ROLE"].ToString();
                string pw = dt.Select(null)[0]["PASSWORD"].ToString();
                if (pw != password) 
                {
                    MessageBox.Show("密码错误");
                }
                else if (role == "超级管理员")
                {
                    FormUser userDIa = new FormUser();
                    userDIa.Show();
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("无管理员权限");
                }
            }
        }
    }
}
