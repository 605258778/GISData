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
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedItem = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string role = this.comboBox1.Text;
            string user = this.textBox1.Text;
            string password = this.textBox2.Text;
            string password1 = this.textBox3.Text;
            if (role == "" || user == "" || password == "") 
            {
                MessageBox.Show("信息填记不完整");
            }
            else if (password != password1)
            {
                MessageBox.Show("两次密码输入不一致");
            }
            else 
            {
                ConnectDB db = new ConnectDB();
                DataTable dt = db.GetDataBySql("select count(*) as numUser from GISDATA_USER WHERE USER ='" + user + "'");
                if (dt == null) 
                {
                    MessageBox.Show("请检查数据库连接");
                    return;
                }
                if (dt.Select(null)[0]["numUser"].ToString() != "0")
                {
                    MessageBox.Show("该用户已存在");
                }
                else 
                {
                    Boolean result = db.Insert("insert into GISDATA_USER (USER,PASSWORD,ROLE) values ('" + user + "','" + password + "','" + role + "')");
                    if (result) 
                    {
                        MessageBox.Show("注册成功");
                        this.Close();
                    }
                }
            }
        }
    }
}
