using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.SqlClient;
using System.IO;
namespace DockSample
{
    public partial class Form_WXCX : DockContent
    {
        SqlConnection con;
        public static int hwnd = 0;
        string D_path;
        string data_bb;//版本
        DirectoryInfo Dinfo;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        private DataGridViewCellStyle m_RowStyleNormal;
        private DataGridViewCellStyle m_RowStyleAlternate;
        private DataGridViewCellStyle m_normal;
        private DataGridViewCellStyle m_skyblue;
        public Form_WXCX()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JCBJ_Load(object sender, EventArgs e)
        {
            SetRowStyle();
            hwnd = (int)this.Handle;
            if (MainForm.str_sjdc == "1")
            {
                buttonX4.Enabled = true;
            }
            if (MainForm.str_zlsc== "1")
            {
                buttonX3.Enabled = true;
            }
            comboBoxEx2.SelectedIndex = 0;
            Queue_data();
        }
        private void SetRowStyle()
        {
            //可根据需要设置更多样式属性，如字体、对齐、前景色、背景色等
            m_RowStyleNormal = new DataGridViewCellStyle();
            m_RowStyleNormal.BackColor = Color.LightGreen;
            m_RowStyleNormal.SelectionBackColor = Color.LightSteelBlue;

            m_RowStyleAlternate = new DataGridViewCellStyle();
            m_RowStyleAlternate.BackColor = Color.LightGray;
            m_RowStyleAlternate.SelectionBackColor = Color.LightSlateGray;

            m_normal = new DataGridViewCellStyle();
            m_normal.BackColor = Color.White;
            m_normal.SelectionBackColor = Color.LightGray;

            m_skyblue = new DataGridViewCellStyle();
            m_skyblue.BackColor = Color.Orange;
        }


        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select a.*,b.客户名称 from J_维修处理表 a left outer join  J_客户信息 b on a.客户编号=b.客户编号 where  接修日期 between '" + d1 + "' and '" + d2 + "'";
                if (comboBoxEx1.Text != "")
                {
                    str += " and 当前状态='"+comboBoxEx1.SelectedIndex+"'";
                }
                if (comboBoxEx2.Text != "模糊查询")
                {
                    if (textBoxX2.Text != "")
                    {
                        str += " and " + comboBoxEx2.Text + " like '%" + textBoxX2.Text + "%'";
                    }
                    //else
                    //{
                    //    MessageBox.Show("请输入查询内容！");
                    //}

                }
                else
                {
                    if (textBoxX2.Text != "")
                    {
                        str += " and ";
                        for (int i = 1; i < comboBoxEx2.Items.Count; i++)
                        {
                            str += "isnull("+comboBoxEx2.Items[i].ToString()+",'')+";
                        }
                        str = str.Substring(0, str.Length - 1);
                        str += " like '%" + textBoxX2.Text + "%'";
                    }
                }
                if (checkBoxX1.Checked)
                {
                    str += " and 返修次数>0";
                }
                if (checkBoxX2.Checked)
                {
                    str += " and 挂账标志=1";
                }
                if (checkBoxX3.Checked)
                {
                    str += " and 送修标志=1";
                }
                str += " order by 维修编号 desc";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;
                if (MainForm.str_sfjs != "1")
                {
                    this.dataGridViewX1.Columns["维修报价"].Visible = false;
                    this.dataGridViewX1.Columns["预付款"].Visible = false;

                }
                if (MainForm.str_khzl!= "1")
                {

                    this.dataGridViewX1.Columns["客户名称"].Visible = false;
                    this.dataGridViewX1.Columns["客户编号"].Visible = false;
                }
                this.dataGridViewX1.Columns["保修情况"].Visible = false;
                this.dataGridViewX1.Columns["随机附件"].Visible = false;
                this.dataGridViewX1.Columns["修品数量"].Visible = false;
                this.dataGridViewX1.Columns["接修日期"].DefaultCellStyle.Format = "yyyy-MM-dd";
                this.dataGridViewX1.Columns["客户名称"].DisplayIndex = 3;
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
                //this.Parent.
                //如果父窗体已注册了自定义事件
                if (DataArrivalEvent != null)
                {
                    DataArrivalEvent(dt.Rows.Count.ToString());
                }
                this.dataGridViewX1.Columns[0].Frozen = true;
                this.dataGridViewX1.Columns[1].Frozen = true;

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
            comboBoxEx1.Text = "";
            comboBoxEx2.SelectedIndex = 0;
            textBoxX2.Text = "";
            checkBoxX1.Checked = false;
            checkBoxX2.Checked = false;
            checkBoxX3.Checked = false;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void Form_WXCX_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
            this.dataGridViewX1.EndEdit();
            string temp = this.dataGridViewX1.Rows[e.RowIndex].Cells["当前状态"].Value.ToString();

            //if (temp == "8")
            //{
                //this.客户返修ToolStripMenuItem.Visible= true;
                //this.客户认可ToolStripMenuItem.Visible = true;
                e.ContextMenuStrip = this.contextMenuStrip1;
                //this.contextMenuStrip1.Items[0].Enabled = true;
                ////this.contextMenuStrip1.Items[1].Enabled = true;
                //            this.contextMenuStrip1.Enabled=true;
            //}
            //else 
                if (temp == "6" || temp == "9")
            {
                //this.contextMenuStrip1.Items[0].Visible = false;
                //this.contextMenuStrip1.Items[1].Visible = false;
                e.ContextMenuStrip = this.contextMenuStrip3;
            }
            else if (temp == "10")
            {
                //this.contextMenuStrip1.Items[0].Visible = false;
                //this.contextMenuStrip1.Items[1].Visible = false;
                e.ContextMenuStrip = this.contextMenuStrip4;
            }
            else
            {
                e.ContextMenuStrip = this.contextMenuStrip2;
            }

        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "当前状态")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["当前状态"].Value.ToString();
                switch (getdata)
                {
                    case "0": e.Value = "待报价";
                        break;
                    case "1": e.Value = "待派工";
                        break;
                    case "2": e.Value = "维修中";
                        break;
                    case "3": e.Value = "待质检";
                        break;
                    case "4": e.Value = "待结算";
                        break;
                    case "5": e.Value = "待审核";
                        break;
                    case "6": e.Value = "已结束";
                        break;
                    case "7": e.Value = "已取消";
                        break;
                    case "8": e.Value = "待验收";
                        break;
                    case "9": e.Value = "待收款";
                        break;
                    case "10": e.Value = "送修中";
                        break;
                }

                // 		        e.FormattingApplied=true;

            }
            //if (dgv.Columns[e.ColumnIndex].Name == "优先级")
            //{
                string getdata2 = dgv.Rows[e.RowIndex].Cells["优先级"].Value.ToString();

                if (getdata2 == "紧急")
                {
                    dgv.Rows[e.RowIndex].Cells["维修编号"].Style.ForeColor = Color.Red;
                }
                else
                {
                    dgv.Rows[e.RowIndex].Cells["维修编号"].Style.ForeColor = Color.Black;
                }
                // 		        e.FormattingApplied=true;

            //}
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 客户认可ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

                //if (MessageBox.Show("确认通过吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                //{
                //    if (this.dataGridViewX1.SelectedRows.Count > 0)
                //    {
                //        try
                //        {

                //            if (con.State == ConnectionState.Closed)
                //                con.Open();
                //            string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                //            string str1 = "update J_维修处理表 set 当前状态=9  where 维修编号='" + wxbh+ "'";
                //            SqlCommand SQL3 = new SqlCommand(str1, con);
                //            SQL3.ExecuteNonQuery();
                 

                //            str1 = "update J_应收明细 set 客户认可=1  where 维修编号='" + wxbh + "'";
                //            SQL3 = new SqlCommand(str1, con);
                //            SQL3.ExecuteNonQuery();
                //            SQL3.Dispose();

                //        }
                //        catch
                //        {

                //        }
                //        con.Close();
                //    }
                //    Queue_data();
                //}

        }

        private void 客户返修ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确认返修吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    if (this.dataGridViewX1.SelectedRows.Count > 0)
            //    {
            //        string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
            //        string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
            //        string sxbz = this.dataGridViewX1.SelectedRows[0].Cells["送修标志"].Value.ToString();
            //        try
            //        {

            //            if (con.State == ConnectionState.Closed)
            //                con.Open();
            //            if (temp == "2")
            //            {

            //                string str1 = "update J_维修处理表 set 当前状态=1 where 维修编号='" + wxbh + "'";
            //                SqlCommand SQL3 = new SqlCommand(str1, con);
            //                SQL3.ExecuteNonQuery();
            //                MessageBox.Show("返修两次以上，重新进入派工！");
            //            }
            //            else
            //            {
            //                int num = Convert.ToInt32(temp) + 1;
            //                string str1 = "update J_维修处理表 set 当前状态=2,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() + "' where 维修编号='" + wxbh + "'";
            //                SqlCommand SQL3 = new SqlCommand(str1, con);
            //                SQL3.ExecuteNonQuery();
            //                if (sxbz != "1")
            //                {
            //                    str1 = "update J_维修积分表 set 返修次数='" + num + "' where 维修编号='" + wxbh + "'";
            //                    SQL3 = new SqlCommand(str1, con);
            //                    SQL3.ExecuteNonQuery();
            //                    SQL3.Dispose();
            //                }
            //            }




            //        }
            //        catch
            //        {

            //        }
            //        con.Close();
            //    }
            //    Queue_data();
            //}

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

        private void 结单返修ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_XXXX frm = new Form_XXXX(wxbh);
                frm.ShowDialog();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认返修吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                    string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                    string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                    string jsy = this.dataGridViewX1.SelectedRows[0].Cells["技术员"].Value.ToString();
                    string sxbz = this.dataGridViewX1.SelectedRows[0].Cells["送修标志"].Value.ToString();
                    DateTime dt = Convert.ToDateTime(this.dataGridViewX1.SelectedRows[0].Cells["保修时间"].Value.ToString());
                    DateTime dt2 = DateTime.Now;
                    if (dt2 < dt)
                    {
                        try
                        {

                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (sxbz == "1")
                            {
                                int num = Convert.ToInt32(temp) + 1;
                                string str1 = "update J_维修处理表 set 当前状态=10,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() +"' where 维修编号='" + wxbh + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                            }
                            else
                            {
                                if (gzbz == "2")
                                {
                                    decimal[] str_temp = new decimal[7] { 0, 0, 0, 0, 0, 1, 0 };
                                    string str = "insert into J_维修积分表 values('" + wxbh + "'," + str_temp[0] + "";
                                    str += ", " + str_temp[1] + "," + str_temp[2] + "," + str_temp[3] + "," + str_temp[4] + "," + str_temp[5] + "," + str_temp[6] + ",'" + jsy + "','" + DateTime.Now.ToString() + "',0,1,1)";

                                    SqlCommand sqlcom = new SqlCommand(str, con);
                                    sqlcom.ExecuteNonQuery();
                                    sqlcom.Dispose();
                                }
                                int num = Convert.ToInt32(temp) + 1;
                                string str1 = "update J_维修处理表 set 当前状态=2,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() + "' where 维修编号='" + wxbh + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                            }
                            //if (gzbz == "1")
                            //{
                            //    str1 = "update J_维修积分表 set 返修次数='" + num + "' where 维修编号='" + wxbh + "'";
                            //    SQL3 = new SqlCommand(str1, con);
                            //    SQL3.ExecuteNonQuery();
                            //    SQL3.Dispose();
                            //}






                        }
                        catch
                        {

                        }
                        con.Close();
                        Queue_data();
                    }
                    else
                    {
                        MessageBox.Show("已过保修期，请重新登记！");
                    }
                }

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认转入取机结算吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string str1 = "update J_维修处理表 set 当前状态=4,完工日期='" + DateTime.Now.ToString() + "',维修状态='维修结束'  where 维修编号='" + wxbh + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();


                        //str1 = "update J_应收明细 set 客户认可=1  where 维修编号='" + wxbh + "'";
                        //SQL3 = new SqlCommand(str1, con);
                        //SQL3.ExecuteNonQuery();
                        //SQL3.Dispose();

                    }
                    catch
                    {

                    }
                    con.Close();
                }
                Queue_data();
            }
        }

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (this.dataGridViewX1.Rows[e.RowIndex].Cells["当前状态"].Value == DBNull.Value)
                return;
            string str1 = this.dataGridViewX1.Rows[e.RowIndex].Cells["当前状态"].Value.ToString();
            if (str1 == "6")/////////////无提示
            {
                //RowIcon = JWZB.Properties.Resources.ball_green;
                //strToolTip = "已解决";
                //e.Graphics.DrawImage(RowIcon, e.RowBounds.Left + this.dataGridView2.RowHeadersWidth - 20, e.RowBounds.Top + 4, 16, 16);//绘制图标
                //this.dataGridView2.Rows[e.RowIndex].HeaderCell.ToolTipText = strToolTip;//设置提示信息
                this.dataGridViewX1.Rows[e.RowIndex].DefaultCellStyle = m_RowStyleNormal;


            }
            else if (str1 == "7")/////////////无提示
            {
                //RowIcon = JWZB.Properties.Resources.ball_green;
                //strToolTip = "已解决";
                //e.Graphics.DrawImage(RowIcon, e.RowBounds.Left + this.dataGridView2.RowHeadersWidth - 20, e.RowBounds.Top + 4, 16, 16);//绘制图标
                //this.dataGridView2.Rows[e.RowIndex].HeaderCell.ToolTipText = strToolTip;//设置提示信息
                this.dataGridViewX1.Rows[e.RowIndex].DefaultCellStyle =m_RowStyleAlternate;


            }
            else if (str1 == "9")/////////////无提示
            {
                //RowIcon = JWZB.Properties.Resources.ball_green;
                //strToolTip = "已解决";
                //e.Graphics.DrawImage(RowIcon, e.RowBounds.Left + this.dataGridView2.RowHeadersWidth - 20, e.RowBounds.Top + 4, 16, 16);//绘制图标
                //this.dataGridView2.Rows[e.RowIndex].HeaderCell.ToolTipText = strToolTip;//设置提示信息
                this.dataGridViewX1.Rows[e.RowIndex].DefaultCellStyle = m_skyblue;


            }
            else
            {

            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                Queue_data();
            }
        }
        private void delete_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_维修处理表  where 流水号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    //default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();

            }
            else
            {
                MessageBox.Show("请选择要删除的列！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            D_path = DateTime.Now.ToString("yyyyMMdd");
            Dinfo = new DirectoryInfo(D_path);
            Dinfo.Create();
            string time1 = dateTimeInput1.Value.ToShortDateString();
            string time2 = dateTimeInput2.Value.ToShortDateString();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DataTable dt = new DataTable();

                dt = (DataTable)this.dataGridViewX1.DataSource;
                //loadingCircle1.Active = false;
                //loadingCircle1.Visible = false;
                //if (radioButton3.Checked)
                //{
                    data_bb = "2003";
                //}
                //else
                //{
                //    data_bb = "2007";
                //}
                //waitCircle.stop();
                Table_ToExcel t_Excel = new Table_ToExcel();
                string tempPath = Directory.GetCurrentDirectory() + "\\" + D_path + "\\" + "业务记录" + DateTime.Now.ToString("HHmmss") + "（" + time1 + "--" + time2 + "）";
                t_Excel.u_DataTableToExcel1(dt, tempPath, 65000, data_bb);
                //frm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }
    }
}
