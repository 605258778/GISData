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

namespace GISData.ChekConfig
{
    public partial class FormStep : Form
    {
        private FormConfigMain formConfigMain;

        public FormStep()
        {
            InitializeComponent();
        }

        public FormStep(FormConfigMain formConfigMain)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.formConfigMain = formConfigMain;
        }

        private void buttonAddStepOk_Click(object sender, EventArgs e)
        {
            string stepName = textBoxStepName.Text;
            if (stepName != "" && comboBoxChekType.Text != "")
            {
                string stepType = comboBoxChekType.SelectedItem.ToString();
                ConnectDB db = new ConnectDB();
                Boolean result = db.Update("update GISDATA_CONFIGSTEP set STEP_NAME='" + stepName + "',STEP_TYPE='" + stepType + "',IS_CONFIG = '1' where STEP_NO = " + this.formConfigMain.click_NO);
                if (result)
                {
                    if (stepType == "结构检查")
                    {
                        this.Close();
                        //this.formConfigMain.ShowForm();
                    }
                    this.formConfigMain.loadStep();
                }
            }
        }
    }
}
