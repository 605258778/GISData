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

namespace GISData.ChekConfig.CheckDialog
{
    public partial class FormDomain : Form
    {
        public ListBox listBoxFieldValue
        {
            get { return listBoxField; }
            set { this.listBoxField = value; }
        }
        private ComboBox comboBoxDataSour;

        public FormDomain()
        {
            InitializeComponent();
        }

        public FormDomain(ComboBox comboBox)
        {
            InitializeComponent();
            comboBoxDataSour = comboBox;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textBoxDomain = this.textBoxDomain.Text;
            if (listBox1.FindString(textBoxDomain) != ListBox.NoMatches)
            {
                MessageBox.Show("此项已经存在");
                return;
            }
            //将文本框中文本加入到ListBox的列表项中
            listBox1.Items.Add(textBoxDomain);
            this.textBoxDomain.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
                this.listBox1.Items.Remove(this.listBox1.SelectedItem);
            if (this.listBox1.Items.Count == 0)
                return;
        }

        private void FormDomain_Load(object sender, EventArgs e)
        {
            string table = comboBoxDataSour.SelectedValue.ToString();
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select FIELD_NAME,FIELD_ALSNAME from GISDATA_MATEDATA where REG_NAME = '" + table + "'");
            listBoxField.DataSource = dt;
            listBoxField.DisplayMember = "FIELD_ALSNAME";
            listBoxField.ValueMember = "FIELD_NAME";
        }
    }
}
