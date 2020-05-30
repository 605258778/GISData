using ADODB;
using GISData.Common;
using MySql.Data.MySqlClient;
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
    public partial class FormHostConfig : Form
    {
        public FormHostConfig()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = getHost();
            if (TestConnect(host))
            {
                CommonClass common = new CommonClass();
                common.SetConfigValue("Host", host);
                this.Close();
            }
            else 
            {
                MessageBox.Show("连接无效");
            }
            
        }

        private string getHost() 
        {
            string ip = this.textBox1.Text;
            string dk = this.textBox2.Text;
            string sjk = this.textBox3.Text;
            string user = this.textBox4.Text;
            string password = this.textBox5.Text;
            string host = "server=" + ip + ";port=" + dk + ";user=" + user + ";password=" + password + "; database=" + sjk + ";Charset=utf8;";
            return host;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string host = getHost();
            if (TestConnect(host))
            {
                MessageBox.Show("连接成功");
            }
            else 
            {
                MessageBox.Show("连接失败");
            }
            
        }

        private bool TestConnect(string host) 
        {
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(host);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void FormHostConfig_Load(object sender, EventArgs e)
        {

        } 
    }
}
