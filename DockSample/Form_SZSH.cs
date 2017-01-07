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
    public partial class Form_SZSH : DockContent
    {

        public static DataGridViewRow row;
        SqlConnection con;
        public static int hwnd = 0;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_SZSH()
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
                string str = "select * from J_收支流水 where  日期 between '" + d1 + "' and '" + d2 + "' and 审核标志=0";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

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

        private void Form_SZSH_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                    string zhmc = this.dataGridViewX1.SelectedRows[0].Cells["账户"].Value.ToString();
                    string str_sr = this.dataGridViewX1.SelectedRows[0].Cells["收入"].Value.ToString();
                    string str_zc = this.dataGridViewX1.SelectedRows[0].Cells["支出"].Value.ToString();
                    string str1 = "update J_收支流水 set 审核人='" + LoginXT.username + "',审核日期='" + DateTime.Now.ToString() + "',审核标志=1  where 序号='" + str2 + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                    SQL3.Dispose();
                    decimal temp = 0;
                    if (Convert.ToDecimal(str_sr)>0)
                    {
                        temp = Convert.ToDecimal(str_sr);
                    }
                    else
                    {
                        temp = 0 - Convert.ToDecimal(str_zc);
                    }
                    str1 = "update J_收支账户 set 账户结余=账户结余+" + temp + " where 账户名称='" + zhmc + "'";
                    SqlCommand SQL = new SqlCommand(str1, con);
                    SQL.ExecuteNonQuery();
                    SQL.Dispose();
                   
                }
                catch
                {

                }
                con.Close();
            }
            Queue_data();
        }

        private void 账款修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    row = this.dataGridViewX1.SelectedRows[0];
                    //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["结单"].Value.ToString();
                    //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();

                    Form_ZKXG frm = new Form_ZKXG();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {

                        //try
                        //{
                        //    if (con.State == ConnectionState.Closed)
                        //        con.Open();
                        //    //string str = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                        //    //string str1 = "update J_应收明细 set 实收金额=" + Convert.ToDecimal(frm.DJ) + ",结单=1,提醒=0  where 流水号='" + str + "'";
                        //    //SqlCommand SQL3 = new SqlCommand(str1, con);
                        //    //SQL3.ExecuteNonQuery();
                        //    //SQL3.Dispose();

                        //    //str1 = "update J_维修处理表 set 当前状态=5,挂账标志=2  where 维修编号='" + wxbh + "'";
                        //    //SqlCommand SQL2 = new SqlCommand(str1, con);
                        //    //SQL2.ExecuteNonQuery();

                        //}
                        //catch
                        //{

                        //}
                        //con.Close();

                        Queue_data();
                    }
                }
            }
        }
    }
}
