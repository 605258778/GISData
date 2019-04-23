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
    public partial class FormNoGaps : Form
    {
        public FormNoGaps()
        {
            InitializeComponent();
        }

        public string textBoxareaValue
        {
            get { return textBoxarea.Text; }
            set { textBoxarea.Text = value; }
        }
    }
}
