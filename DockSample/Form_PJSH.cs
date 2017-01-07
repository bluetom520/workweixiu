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
    public partial class Form_PJSH : Office2007Form
    {
        SqlConnection con;
        public Form_PJSH()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_PJSH_Load(object sender, EventArgs e)
        {
            Data_initial();
            Queue_data();
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 姓名 from J_员工信息表 where 部门='维修部'";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["姓名"].ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }
        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
            this.dataGridViewX1.EndEdit();
            string zt= this.dataGridViewX1.Rows[e.RowIndex].Cells["状态"].Value.ToString();
            this.dataGridViewX1.EndEdit();
            if (MainForm.str_pjsh == "1")
            {
                if (zt == "0")
                {
                    //this.客户返修ToolStripMenuItem.Visible= true;
                    //this.客户认可ToolStripMenuItem.Visible = true;

                    e.ContextMenuStrip = this.contextMenuStrip1;
                    //else
                    //    e.ContextMenuStrip = null;

                }
                else if (zt == "1")
                {
                    e.ContextMenuStrip = this.contextMenuStrip2;

                }
                else
                {
                    e.ContextMenuStrip = null;
                }
            }
            else
            {
                e.ContextMenuStrip = null;
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Form_PJ frm = new Form_PJ();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("申请成功！");
                Queue_data();
            }
        }

        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                string str = "select * from J_配件申请 where 申请日期 between '" + d1 + "' and '" + d2 + "' ";
                //if (comboBoxEx1.Text != "" && comboBoxEx3.Text != "")
                //{
                //    str += "where 配件类别='" + comboBoxEx1.Text + "' and 配件子类='" + comboBoxEx3.Text + "'";
                //}
                //else  if (comboBoxEx1.Text != "" || comboBoxEx2.Text != "")
                //{
                //    str += "where ";
                //    if (comboBoxEx1.Text != "")
                //        str += " where 配件类别='" + comboBoxEx1.Text + "'";
                //    else
                //    {
                //        str += " where 配件子类='" + comboBoxEx3.Text + "'";
                //    }
                //}
                if (comboBoxEx1.Text != "")
                {
                    str += " and 申请人='"+comboBoxEx1.Text+"'";
                }
                if (comboBoxEx2.Text != "")
                {
                    str += " and 状态='" + comboBoxEx2.SelectedIndex + "'";
                }
                str += " order by 序号 desc";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;


            }
            catch
            {
            }
            con.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "状态")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["状态"].Value.ToString();
                switch (getdata)
                {
                    case "0": e.Value = "申请中";
                        break;
                    case "1": e.Value = "已通过";
                        break;
                    case "2": e.Value = "已拒绝";
                        break;
                    case "3": e.Value = "已购买";
                        break;

                }

                // 		        e.FormattingApplied=true;

            }
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认通过吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    //row = this.dataGridViewX1.SelectedRows[0];
                    //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                    //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();




                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                        string str1 = "update J_配件申请 set 状态=1,审核人='"+LoginXT.username+"',审核日期='"+DateTime.Now.ToString()+"'  where 序号='" + str + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();

                        //str1 = "update J_维修处理表 set 当前状态=5,挂账标志=2  where 维修编号='" + wxbh + "'";
                        //SqlCommand SQL2 = new SqlCommand(str1, con);
                        //SQL2.ExecuteNonQuery();

                    }
                    catch
                    {

                    }
                    con.Close();

                    Queue_data();



                }
            }
        }

        private void 审核拒绝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认拒绝吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    //row = this.dataGridViewX1.SelectedRows[0];
                    //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                    //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();




                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                        string str1 = "update J_配件申请 set 状态=2,审核人='" + LoginXT.username + "',审核日期='" + DateTime.Now.ToString() + "'  where 序号='" + str + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();

                        //str1 = "update J_维修处理表 set 当前状态=5,挂账标志=2  where 维修编号='" + wxbh + "'";
                        //SqlCommand SQL2 = new SqlCommand(str1, con);
                        //SQL2.ExecuteNonQuery();

                    }
                    catch
                    {

                    }
                    con.Close();

                    Queue_data();



                }
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认状态吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    //row = this.dataGridViewX1.SelectedRows[0];
                    //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                    //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();




                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                        string str1 = "update J_配件申请 set 状态=3  where 序号='" + str + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();

                        //str1 = "update J_维修处理表 set 当前状态=5,挂账标志=2  where 维修编号='" + wxbh + "'";
                        //SqlCommand SQL2 = new SqlCommand(str1, con);
                        //SQL2.ExecuteNonQuery();

                    }
                    catch
                    {

                    }
                    con.Close();

                    Queue_data();



                }
            }
        }

        private void 配件已购ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}
