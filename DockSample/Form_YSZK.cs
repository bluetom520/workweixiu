using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.SqlClient;
namespace DockSample
{
    public partial class Form_YSZK : DockContent
    {
        SqlConnection con;
        public static int hwnd = 0;
        public static DataGridViewRow row;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_YSZK()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JCBJ_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_应收明细 a left outer join  J_客户信息 b on a.客户编号=b.客户编号 where  日期 between '" + d1 + "' and '" + d2 + "'  ";
                if (checkBoxX1.Checked)
                {
                    str += " and 结单=0";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;

                //this.dataGridViewX1.Columns["入库编号"].Width = 110;
                //this.dataGridViewX1.Columns["配件类别"].Width = 70;
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

                //如果父窗体已注册了自定义事件
                if (DataArrivalEvent != null)
                {
                    DataArrivalEvent(dt.Rows.Count.ToString());
                }
            }
            catch
            {
            }
            con.Close();
        }
        

        private void buttonX1_Click(object sender, EventArgs e)
        {
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void Form_XPZJ_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
            string temp = this.dataGridViewX1.Rows[e.RowIndex].Cells["客户认可"].Value.ToString();
            string jd = this.dataGridViewX1.Rows[e.RowIndex].Cells["结单"].Value.ToString();
            this.dataGridViewX1.EndEdit();
            if (temp == "1" && jd=="0")
            {
                //this.客户返修ToolStripMenuItem.Visible= true;
                //this.客户认可ToolStripMenuItem.Visible = true;
                e.ContextMenuStrip = this.contextMenuStrip1;

            }
            else
            {

                e.ContextMenuStrip = this.contextMenuStrip2;
            }
        }

        private void 质检通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认结单吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    row = this.dataGridViewX1.SelectedRows[0];
                    string str2 = this.dataGridViewX1.SelectedRows[0].Cells["结单"].Value.ToString();
                    string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                    if (str2 == "1")
                    {
                        MessageBox.Show("您已结单！");
                        return;
                    }
                    Form_XJJD frm = new Form_XJJD(wxbh);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                            
                            try
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                string str = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                                string str1 = "update J_应收明细 set 实收金额=" +Convert.ToDecimal( frm.DJ) + ",结单=1,提醒=0  where 流水号='" + str + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                                SQL3.Dispose();

                                str1 = "update J_维修处理表 set 当前状态=5,挂账标志=2  where 维修编号='" + wxbh + "'";
                                SqlCommand SQL2= new SqlCommand(str1, con);
                                SQL2.ExecuteNonQuery();

                            }
                            catch
                            {

                            }
                            con.Close();
                        
                        Queue_data();
                    }
                }
            }

        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "结单")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["结单"].Value.ToString();
                switch (getdata)
                {
                    case "0": e.Value = "未结";
                        break;
                    case "1": e.Value = "已结";
                        break;

                }

                // 		        e.FormattingApplied=true;

            }
            if (dgv.Columns[e.ColumnIndex].Name == "客户认可")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["客户认可"].Value.ToString();
                switch (getdata)
                {
                    case "0": e.Value = "否";
                        break;
                    case "1": e.Value = "是";
                        break;

                }

                // 		        e.FormattingApplied=true;

            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_XXXX frm = new Form_XXXX(wxbh);
                frm.ShowDialog();
            }
        }

        private void 详细信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_XXXX frm = new Form_XXXX(wxbh);
                frm.ShowDialog();
            }
        }
    }
}
