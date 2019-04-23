using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.CheckConfig.CheckTopo.CheckDialog
{
    public partial class Formxbm : Form
    {
        public Formxbm()
        {
            InitializeComponent();
        }

        public string textBoxwhereValue
        {
            get { return textBoxwhere.Text; }
            set { textBoxwhere.Text = value; }
        }

        public string textBoxinputValue
        {
            get { return textBoxinput.Text; }
            set { textBoxinput.Text = value; }
        }
    }
}
