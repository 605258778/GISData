using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.ChekConfig.CheckDialog
{
    public partial class FormLogic : Form
    {
        public FormLogic()
        {
            InitializeComponent();
        }

        public string textBoxWhereValue
        {
            get { return textBoxWhere.Text; }
            set { textBoxWhere.Text = value; }
        }
        public string textBoxResultValue
        {
            get { return textBoxResult.Text; }
            set { textBoxResult.Text = value; }
        }

        private void FormLogic_Load(object sender, EventArgs e)
        {

        }
    }
}
