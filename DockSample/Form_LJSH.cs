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
    public partial class Form_LJSH : DockContent
    {
        SqlConnection con;

        public static int hwnd = 0;
        public static DataGridViewRow row;
        bool FLAG_SK = false;//未收款
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_LJSH()
        {
            InitializeComponent();
            dateTimeInput1.Value =Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        //private void Form_WXPG_Load(object sender, EventArgs e)
        //{

        //    Queue_data();
        //}
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 维修编号,接修日期,修品大类,修品小类,修品型号,修品品牌,修品SN1,规格参数,故障描述,外观,优先级,技术员,预计积分,预付款,维修报价,维修状态,维修情况,b.客户名称,业务员,预约日期,返修次数,快递单号,返还方式,返回日期,保修时间,a.客户编号,挂账标志,送修标志,送货单号 from J_维修处理表 a left outer join J_客户信息 b  on a.客户编号=b.客户编号 where  接修日期 between '" + d1 + "' and '" + d2 + "' and 当前状态=4 ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX1.Columns["接修日期"].DefaultCellStyle.Format = "yyyy-MM-dd";
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

        private void Form_WXPG_Load_1(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            //if (MainForm.str_jsbc == "1")
            //{
            //    buttonX4.Enabled = false;
            //    //buttonX5.Enabled = false;
            //    //buttonX4.Enabled = false;
            //}
            Data_initial();
            Queue_data();
            
        }

        private void Form_WXPG_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void 维修派工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            row = this.dataGridViewX1.SelectedRows[0];
            Form_PG frm = new Form_PG();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Queue_data();
            }
        }

        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 账户名称 from J_收支账户";
                str1 += ";select * from J_商品信息表 where 类别='项目'";
                str1 += ";select 客户名称 from J_客户信息";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["账户名称"].ToString());
                }

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {

                    comboBoxEx2.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                }
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{

                //    comboBoxEx3.Items.Add(ds.Tables[2].Rows[i]["客户名称"].ToString());
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //comboBoEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["维修状态"].Value.ToString().Trim();
            //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string fhfs = this.dataGridViewX1.SelectedRows[0].Cells["返还方式"].Value.ToString();
                string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();

                if (fhfs != "")
                {
                    buttonX5.Enabled = false;
                    buttonX6.Enabled = true;

                }
                else
                {
                    buttonX5.Enabled = true;
                    buttonX6.Enabled = false;
                    //if (gzbz == "0")
                    //{
                    //    //if (MainForm.str_jsbc == "1")
                    //    buttonX3.Enabled = true;

                    //}
                    //else
                    //{
                    //    buttonX3.Enabled = false;
                    //}
                }
                if (gzbz == "0")
                {
                    //if (MainForm.str_jsbc == "1")
                    buttonX3.Enabled = true;

                }
                else
                {
                    buttonX3.Enabled = false;
                }
                if (fhfs != "" && gzbz != "0")
                {
                    buttonX4.Enabled = true;
                }
                else
                {
                    buttonX4.Enabled = false;
                }

                //if()
                textBoxX1.Text = "0";
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["维修报价"].Value.ToString().Trim();
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["预付款"].Value.ToString().Trim();
                //////textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["保修范围"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["快递单号"].Value.ToString().Trim();
                textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells["送货单号"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["客户名称"].Value.ToString().Trim();
                comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells["返还方式"].Value.ToString().Trim();

                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返回日期"].Value.ToString().Trim();
                //if (temp == "")
                dateTimeInput3.Value = Convert.ToDateTime(DateTime.Now.ToString());
                //else
                //    dateTimeInput3.Value = Convert.ToDateTime(temp);
                string temp = this.dataGridViewX1.SelectedRows[0].Cells["保修时间"].Value.ToString().Trim();
                if (temp == "")
                    dateTimeInput4.Value = Convert.ToDateTime(DateTime.Now.AddMonths(6).ToString());
                else
                    dateTimeInput4.Value = Convert.ToDateTime(temp);
                //string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                if (gzbz == "1")//返修
                {
                    checkBoxX1.Enabled = false;
                    checkBoxX1.Checked = true;
                    comboBoxEx2.Enabled = false;
                    textBoxX6.Enabled = false;
                    textBoxX3.Enabled = false;
                    textBoxX4.Enabled = false;
                    textBoxX10.Enabled = false;
                    comboBoxEx1.Enabled = false;
                    //dataGridViewX1.Enabled = false;
                    dateTimePicker1.Enabled = false;

                }
                else if (gzbz == "0")
                {
                    checkBoxX1.Enabled = true;
                    comboBoxEx1.Enabled = true;
                    comboBoxEx2.Enabled = true;
                    //comboBoxEx3.Enabled = true;
                    textBoxX3.Enabled = true;
                    textBoxX4.Enabled = true;
                    textBoxX10.Enabled = true;
                    //dataGridViewX1.Enabled = false;
                    dateTimePicker1.Enabled = true;
                    textBoxX6.Enabled = true;
                }
                else
                {
                    //checkBoxX1.Checked = false;
                    //checkBoxX1.Enabled = false;
                    checkBoxX1.Checked = false;
                    comboBoxEx1.Enabled = false;
                    comboBoxEx2.Enabled = false;
                    //comboBoxEx3.Enabled = false;
                    textBoxX3.Enabled = false;
                    textBoxX4.Enabled = false;
                    textBoxX10.Enabled = false;
                    textBoxX6.Enabled = false;
                    //dataGridViewX1.Enabled = false;
                    dateTimePicker1.Enabled = false;
                }
                QUEUE2();

                if (MainForm.str_jsbc == "1")
                {
                    buttonX4.Enabled = false;
                    buttonX5.Enabled = false;
                    buttonX6.Enabled = false;
                }
                else
                {
                    buttonX3.Enabled = false;
                }
            }
        }

        private void QUEUE2()
        {

            try
            {
                //string d1 = dateTimeInput1.Value.ToString();
                //string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str;
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                if(gzbz=="2")
                    str = "select * from J_收支流水   where 维修编号='" + wxbh + "' and 项目名称='维修收费'";
                else
                    str = "select * from J_应收明细   where 维修编号='" + wxbh + "' and 业务类别='维修收费'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxX4.Text = dt.Rows[0]["开票金额"].ToString().Trim();
                    textBoxX3.Text = dt.Rows[0]["发票号码"].ToString().Trim();


                    if (gzbz == "2")
                    {
                        comboBoxEx2.Text = dt.Rows[0]["项目名称"].ToString().Trim();
                        textBoxX6.Text = dt.Rows[0]["收入"].ToString().Trim();
                        comboBoxEx1.Text = dt.Rows[0]["账户"].ToString().Trim();
                    }
                    else
                    {
                        comboBoxEx2.Text = dt.Rows[0]["业务类别"].ToString().Trim();
                        textBoxX6.Text = dt.Rows[0]["挂账金额"].ToString().Trim();
                    }
                    textBoxX10.Text = dt.Rows[0]["摘要明细"].ToString().Trim();
                    //textBoxX7.Text = dt.Rows[0]["往来单位"].ToString().Trim();//
                    FLAG_SK = true;
                }
                else
                {
                    textBoxX4.Text = "";
                    textBoxX3.Text = "";
                    textBoxX6.Text = "";
                    comboBoxEx1.Text = "";
                    comboBoxEx2.Text = "";
                   // textBoxX7.Text = "";
                }
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
            }
            catch
            {
            }
            con.Close();
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
            Form_LY frm=new Form_LY(str2);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                QUEUE2();
            }
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    delete_data();
            //    QUEUE2();
            //}
        }



        private void buttonX6_Click(object sender, EventArgs e)
        {
            //string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
            //DATA_upload frm = new DATA_upload(str2);
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    QUEUE2();
            //}
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    delete_data2();
            //    QUEUE2();
            //}
        }
        private void delete_data2()
        {
            //if (this.dataGridViewX3.SelectedRows.Count > 0)
            //{
            //    string str1 = this.dataGridViewX3.SelectedRows[0].Cells["序号"].Value.ToString();

            //    try
            //    {
            //        if (con.State == ConnectionState.Closed)
            //            con.Open();

            //        string str = "delete from J_维修资料库 where 序号='" + str1 + "'";
            //        SqlCommand sqlcom = new SqlCommand(str, con);
            //        sqlcom.ExecuteNonQuery();


            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }

            //    con.Close();

            //}
            //else
            //{
            //    MessageBox.Show("请选择要删除的列！");
            //}
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {

        }
        private void change_data()
        {
            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
            //    string str = "update J_维修处理表 set 维修状态='" + comboBoxEx1.Text + "',维修情况='" + richTextBox1.Text + "'   where 维修编号='" + str2 + "'";
            //    SqlCommand SQL3 = new SqlCommand(str, con);
            //    SQL3.ExecuteNonQuery();
            //    SQL3.Dispose();

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //con.Close();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                this.comboBoxEx1.Enabled = false;
                textBoxX6.Enabled = false;
                textBoxX6.Text = "0";
                textBoxX9.Enabled = true;
                //textBoxX10.Enabled = false;
            }
            else
            {
                this.comboBoxEx1.Enabled =  true;
                textBoxX6.Enabled = true;
                textBoxX6.Text = "";
                textBoxX9.Enabled = false;
                //textBoxX10.Enabled = true;
            }
        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonX5_Click_1(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确认取机保存吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        //string khbh = this.dataGridViewX1.SelectedRows[0].Cells["客户编号"].Value.ToString();
                        //string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                        if (comboBoxEx4.Text == "")
                        {
                            MessageBox.Show("请选择返回方式！");
                            return;
                        }

                        string str = "update J_维修处理表 set 返还方式='" + comboBoxEx4.Text + "',返回日期='" + dateTimeInput3.Value.ToShortDateString() + "',保修时间='" + dateTimeInput4.Value.ToShortDateString() + "',快递单号='" + textBoxX8.Text + "',送货单号='" + textBoxX11.Text + "'  where 维修编号='" + wxbh + "'";
                        SqlCommand SQL = new SqlCommand(str, con);
                        SQL.ExecuteNonQuery();
                        default_data();
                    }
                    catch
                    {

                    }
                    con.Close();
                    Queue_data();
                }
            }

            else
            {
                MessageBox.Show("请选择要保存的项目！");
            }
        }
        private void default_data()
        {
            // comboBox1.Text = "";
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX5.Text = "";
            textBoxX6.Text = "";
            //textBoxX7.Text = "";
            textBoxX8.Text = "";
            textBoxX9.Text = "30";
            textBoxX10.Text = "";
            textBoxX11.Text = "";
            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
            textBoxX7.Text = "";
            comboBoxEx4.Text = "";
            checkBoxX1.Checked = false;
            //comboBoxEx5.Text = "";
            //comboBoxEx18.Text = "";
            buttonX3.Enabled = false;
            buttonX4.Enabled = false;
            buttonX5.Enabled = false;
            buttonX6.Enabled = false;

        }
        private void buttonX3_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("转入审核吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string str1 = "update J_维修处理表 set 当前状态=5,完工日期='" + DateTime.Now.ToString() + "'  where 维修编号='" + str2 + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();

                    }
                    catch
                    {

                    }
                    con.Close();
                }
                Queue_data();
            }
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            decimal[] str_data = new decimal[3] { 0, 0, 0 };
            if (textBoxX2.Text != "")
            {
                str_data[0] = Convert.ToDecimal(textBoxX2.Text);
            }
            if (textBoxX1.Text != "")
            {
                str_data[1] = Convert.ToDecimal(textBoxX1.Text);
            }
            str_data[2] = str_data[0] - str_data[1];
            textBoxX5.Text = str_data[2].ToString();
        }

        private void 详细情况ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_XXXX frm = new Form_XXXX(wxbh);
                frm.ShowDialog();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBoxX7.Text != "")
            {
                Form_KPZL frm = new Form_KPZL(textBoxX7.Text);
                frm.ShowDialog();
            }
        }

        private void buttonX4_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认客户验收，进行积分结算吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        string sxbz = this.dataGridViewX1.SelectedRows[0].Cells["送修标志"].Value.ToString();
                        string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                        if (sxbz != "1")//送修没有积分
                        {
                            Form_JFSH frm = new Form_JFSH(wxbh);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {


                                int num = Convert.ToInt32(this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString());
                                //string gzbz = Convert.ToInt32(this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString());
                                //int jf = 1;
                                //int kf = 0;
                                if (Convert.ToDecimal(frm.DJ) > 0)
                                {
                                    string str1 = "update J_维修处理表 set 预计积分=" + Convert.ToDecimal(frm.DJ) + "  where 维修编号='" + wxbh + "'";
                                    SqlCommand SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                    SQL3.Dispose();
                                }

                                if (gzbz == "1")//挂账标志
                                {
                                    string str1 = "update J_维修处理表 set 当前状态=9,审核人='" + LoginXT.username + "'  where 维修编号='" + wxbh + "'";
                                    SqlCommand SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                    str1 = "update J_应收明细 set 客户认可=1  where 维修编号='" + wxbh + "'";
                                    SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                    SQL3.Dispose();
                                }
                                else
                                {
                                    string str1 = "update J_维修处理表 set 当前状态=5,审核人='" + LoginXT.username + "'  where 维修编号='" + wxbh + "'";
                                    SqlCommand SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                }

                            }
                        }
                        else
                        {
                            if (gzbz == "1")//挂账标志
                            {
                                string str1 = "update J_维修处理表 set 当前状态=9  where 维修编号='" + wxbh + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                                str1 = "update J_应收明细 set 客户认可=1  where 维修编号='" + wxbh + "'";
                                SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                                SQL3.Dispose();
                            }
                            else
                            {
                                string str1 = "update J_维修处理表 set 当前状态=5  where 维修编号='" + wxbh + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                            }
                        }



                        default_data();

                    }
                    catch
                    {

                    }
                    con.Close();
                }
                Queue_data();
            }
        }

        private void buttonX6_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认返修吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    string temp = this.dataGridViewX1.SelectedRows[0].Cells["返修次数"].Value.ToString();
                    string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                    string sxbz = this.dataGridViewX1.SelectedRows[0].Cells["送修标志"].Value.ToString();
                    string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (sxbz == "1")
                        {
                            int num = Convert.ToInt32(temp) + 1;
                            string str1 = "update J_维修处理表 set 当前状态=10,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() + "',返还方式='' where 维修编号='" + wxbh + "'";
                            SqlCommand SQL3 = new SqlCommand(str1, con);
                            SQL3.ExecuteNonQuery();

                        }
                        else
                        {
                            if (gzbz == "2")//保期返修未修复
                            {
                                int num = Convert.ToInt32(temp) + 1;
                                string str1 = "update J_维修处理表 set 当前状态=2,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() + "',返还方式='' where 维修编号='" + wxbh + "'";
                                SqlCommand SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();

                                str1 = "update J_维修积分表 set 返修次数='" + num + "' where 维修编号='" + wxbh + "' and 审核标志=0";
                                SQL3 = new SqlCommand(str1, con);
                                SQL3.ExecuteNonQuery();
                                SQL3.Dispose();
                            }
                            else//未结单返修
                            {
                                if (temp == "2")
                                {

                                    string str1 = "update J_维修处理表 set 当前状态=1 where 维修编号='" + wxbh + "'";
                                    SqlCommand SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                    MessageBox.Show("返修两次以上，重新进入派工！");
                                }
                                else
                                {
                                    int num = Convert.ToInt32(temp) + 1;
                                    string str1 = "update J_维修处理表 set 当前状态=2,返修次数='" + num + "',接修日期='" + DateTime.Now.ToString() + "',返还方式='' where 维修编号='" + wxbh + "'";
                                    SqlCommand SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();

                                    str1 = "update J_维修积分表 set 返修次数='" + num + "' where 维修编号='" + wxbh + "' and 审核标志=0";
                                    SQL3 = new SqlCommand(str1, con);
                                    SQL3.ExecuteNonQuery();
                                    SQL3.Dispose();

                                }
                            }
                        }

                        default_data();


                    }
                    catch
                    {

                    }
                    con.Close();
                }
                Queue_data();
            }
        }

        private void buttonX3_Click_2(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确认结算保存吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {

                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        int state = 5;
                        int flag = 0;
                        int fxbz = 0;//保期返修标志 
                        string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string khbh = this.dataGridViewX1.SelectedRows[0].Cells["客户编号"].Value.ToString();
                        string gzbz = this.dataGridViewX1.SelectedRows[0].Cells["挂账标志"].Value.ToString();
                        if (comboBoxEx2.Text == "")
                        {
                            MessageBox.Show("请选择收支项目！");
                            return;
                        }

                        if (gzbz == "0")//未结
                        {
                            decimal[] str_temp = new decimal[2] { 0, 0 };


                            if (textBoxX6.Text != "")
                                str_temp[0] = Convert.ToDecimal(textBoxX6.Text);
                            else
                                str_temp[0] = 0;

                            decimal temp2 = Convert.ToDecimal(textBoxX5.Text);


                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (checkBoxX1.Checked)//挂账
                            {

                                string str1 = "insert into  J_应收明细 values('" + wxbh + "','" + khbh + "'";
                                str1 += ",'" + DateTime.Now.ToString() + "'," + temp2 + ",'" + LoginXT.username + "',1,'" + DateTime.Now.AddDays(Convert.ToInt32(textBoxX9.Text)) + "','" + comboBoxEx2.Text + "','" + textBoxX4.Text + "','" + textBoxX3.Text + "','" + dateTimePicker1.Value.ToString() + "',0,0,0,'" + textBoxX10.Text + "')";

                                SqlCommand SQL2 = new SqlCommand(str1, con);
                                SQL2.ExecuteNonQuery();
                                SQL2.Dispose();
                                //string str1 = "update J_维修处理表 set 当前状态=8,完工日期='" + DateTime.Now.ToString() + "',挂账标志=1 where 维修编号='" + wxbh + "'";
                                //SqlCommand SQL3 = new SqlCommand(str1, con);
                                //SQL3.ExecuteNonQuery();
                                //SQL3.Dispose();
                                state = 8;
                                flag = 1;
                            }
                            else//直接结算
                            {
                                if (!FLAG_SK)
                                {

                                    string str1 = "insert into J_收支流水  (日期,收入,支出,账户,往来单位,项目名称,维修编号,开票金额,发票号码,经办人,开票日期,摘要明细)values('" + DateTime.Now.ToString() + "'," + str_temp[0] + "";
                                    str1 += "," + str_temp[1] + ",'" + comboBoxEx1.Text + "','" + textBoxX7.Text + "','" + comboBoxEx2.Text + "','" + wxbh + "','" + textBoxX4.Text + "','" + textBoxX3.Text + "','" + LoginXT.username + "','" + dateTimePicker1.Value.ToString() + "','" + textBoxX10.Text + "')";

                                    SqlCommand SQL2 = new SqlCommand(str1, con);
                                    SQL2.ExecuteNonQuery();
                                    SQL2.Dispose();
                                    state = 5;
                                    flag = 2;// 结单
                                    //decimal temp = 0;

                                    //temp = str_temp[0];
                                    //string str1 = "update J_维修处理表 set 当前状态=5,完工日期='" + DateTime.Now.ToString() + "',挂账标志=0  where 维修编号='" + wxbh + "'";
                                    //SqlCommand SQL3 = new SqlCommand(str1, con);
                                    //SQL3.ExecuteNonQuery();
                                    //SQL3.Dispose();

                                    //str1 = "update J_收支账户 set 账户结余=账户结余+" + temp + " where 账户名称='" + comboBoxEx1.Text + "'";
                                    //SqlCommand SQL3 = new SqlCommand(str1, con);
                                    //SQL3.ExecuteNonQuery();
                                    //SQL3.Dispose();
                                }
                            }
                        }
                        else //保期返修
                        {
                            //if (gzbz == "1")//返修
                            //{
                            //    state = 8;
                            //    flag = 1;
                            //}
                            //else
                            //{
                            //    fxbz = 1;
                            //    flag = 2;// 结单
                            //}
                        }


                        string str = "update J_维修处理表 set 挂账标志='" + flag + "'  where 维修编号='" + wxbh + "'";
                        SqlCommand SQL = new SqlCommand(str, con);
                        SQL.ExecuteNonQuery();


                        default_data();

                    }
                    catch
                    {

                    }
                    con.Close();
                    Queue_data();
                }
            }
            else
            {
                MessageBox.Show("请选择要保存的项目！");
            }
        }

        private void dataGridViewX1_Click(object sender, EventArgs e)
        {

        }
    }
}
