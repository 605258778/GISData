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
        private CheckBox checkBox;

        public FormTopoDia()
        {
            InitializeComponent();
        }

        public FormTopoDia(string stepNo, CheckBox cb)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.stepNo = stepNo;
            this.checkBox = cb;
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
            this.gridView1.OptionsBehavior.Editable = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            if (selectRows.Length > 0)
            {
                this.checkBox.CheckState = CheckState.Checked;
            }
            else {
                this.checkBox.CheckState = CheckState.Unchecked;
            }
        }

        public void doCheckTopo()
        {
            int[] selectRows = this.gridView1.GetSelectedRows();
            foreach(int itemRow in selectRows)
            {
                DataRow row = this.gridView1.GetDataRow(itemRow);
                string  NAME = row["NAME"].ToString();
                string STATE = row["STATE"].ToString();
                string ERROR = row["ERROR"].ToString();
                string TYPE = row["TYPE"].ToString();
                string TABLENAME = row["TABLENAME"].ToString();
                string WHERESTRING = row["WHERESTRING"].ToString();
                string SUPTABLE = row["SUPTABLE"].ToString();
                string INPUTTEXT = row["INPUTTEXT"].ToString();
            }
        }
    }
}
