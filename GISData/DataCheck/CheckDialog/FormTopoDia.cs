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

namespace GISData.DataCheck.CheckDialog
{
    public partial class FormTopoDia : Form
    {
        private string stepNo;

        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
        }

        public void SelectAll() 
        {
            this.gridView1.SelectAll();
        }

        public void UnSelectAll()
        {
            int[] rows = this.gridView1.GetSelectedRows();
            for (int i = 0; i < rows.Count(); i++)
            {
                this.gridView1.UnselectRow(rows[i]);
            }
        }

        private void FormTopoDia_Load(object sender, EventArgs e)
        {
            bindTreeView();
        }

        private void bindTreeView()
        {
            ConnectDB db = new ConnectDB();
            DataTable dt = db.GetDataBySql("select NAME,STATE,ERROR,TYPE,TABLENAME,WHERESTRING,SUPTABLE,INPUTTEXT from GISDATA_TBTOPO");
            this.gridControl1.DataSource = dt;
            
            //this.gridView1.Columns[4].Visible = false;
            //this.gridView1.Columns[5].Visible = false;
            //this.gridView1.Columns[6].Visible = false;
            //this.gridView1.Columns[7].Visible = false;
            //this.gridView1.Columns[8].Visible = false;
            //this.gridView1.Columns.Add();
            this.gridView1.OptionsBehavior.Editable = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
        }
    }
}
