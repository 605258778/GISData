﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISData.ChekConfig
{
    public partial class FormStep : Form
    {
        public FormStep()
        {
            InitializeComponent();
        }

        private void buttonAddStepOk_Click(object sender, EventArgs e)
        {
            if (comboBoxChekType.SelectedItem.ToString() == "结构检查") {
                this.Close();
                FormConfigMain fcfm = new FormConfigMain();
                fcfm.ShowForm();
            }
        }
    }
}
