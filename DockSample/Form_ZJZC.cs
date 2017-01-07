using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;
namespace DockSample
{
    public partial class Form_ZJZC : Office2007Form
    {
        SqlConnection con;
        public Form_ZJZC()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_ZJZC_Load(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认注册吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string mac = this.dataGridViewX1.SelectedRows[0].Cells["MAC"].Value.ToString();
                    string str1 = "update J_注册用户表  set 注册=1  where MAC='" + mac + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                }
                catch
                {

                }
                Queue_data();
            }
        }

        private void dataGridViewX1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void Queue_data()
        {

            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_注册用户表 ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;

                this.dataGridViewX1.Columns["MAC"].Width = 120;
                this.dataGridViewX1.Columns["主机名"].Width = 150;
                //this.dataGridViewX1.Columns["配件编号"].Width = 70;
                //this.dataGridViewX1.Columns["存储位置"].Width = 100;
                //this.dataGridViewX1.Columns["规格型号"].Width = 100;
                //this.dataGridViewX1.Columns["入库日期"].Width = 110;
                //this.dataGridViewX1.Columns["入库数量"].Width = 60;
                //this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["购买单价"].Width = 60;
                //this.dataGridViewX1.Columns["销售单价"].Width = 60;
                //this.dataGridViewX1.Columns["供货商家"].Width = 100;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;
            }
            catch
            {
            }
            con.Close();



        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "注册")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["注册"].Value.ToString();
                if (getdata == "1")
                {
                    e.Value = "已注册";
                }
                else
                {
                    e.Value = "未注册";
                }
            }
        }

        private void 取消注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认取消吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string mac = this.dataGridViewX1.SelectedRows[0].Cells["MAC"].Value.ToString();
                    string str1 = "update J_注册用户表  set 注册=0  where MAC='" + mac + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                }
                catch
                {

                }
                Queue_data();
            }
        }
    }
}
