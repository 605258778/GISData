using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.DataRegister
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void buttonAddConnect_Click(object sender, EventArgs e)
        {
            FormDBConnectInfo fdci = new FormDBConnectInfo();
            fdci.Show();
        }
    }
}
