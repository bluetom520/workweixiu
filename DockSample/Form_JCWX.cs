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
    public partial class Form_JCWX : DockContent
    {
        SqlConnection con;

        public static int hwnd = 0;
        public static DataGridViewRow row;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_JCWX()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
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
                string str = "";
                if (MainForm.str_wxgl != "1")
                {
                    str = "select 维修编号,接修日期,修品大类,修品小类,修品型号,修品品牌,修品SN1,规格参数,故障描述,外观,优先级,技术员,预计积分,维修状态,维修情况,业务员,预约日期,返修次数 from J_维修处理表 where  接修日期 between '" + d1 + "' and '" + d2 + "' and 当前状态=2 and 技术员='" + LoginXT.username + "'";
                }
                else
                {
                    str = "select 维修编号,接修日期,修品大类,修品小类,修品型号,修品品牌,修品SN1,规格参数,故障描述,外观,优先级,技术员,预计积分,维修状态,维修情况,业务员,预约日期,返修次数 from J_维修处理表 where  接修日期 between '" + d1 + "' and '" + d2 + "' and 当前状态=2 ";

                }

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
            //row = this.dataGridViewX1.SelectedRows[0];
            //Form_PG frm = new Form_PG();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    Queue_data();
            //}
        }

        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_商品信息表 where 类别='状态'";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string temp = this.dataGridViewX1.SelectedRows[0].Cells["维修状态"].Value.ToString().Trim();
                if (temp == "已完工")
                {
                    buttonX3.Enabled = true;
                    buttonX5.Enabled = false;
                }
                else
                {
                    buttonX3.Enabled = false;
                    buttonX5.Enabled = true;
                }
                comboBoxEx1.Text = temp;
                //comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["故障描述"].Value.ToString().Trim();
                richTextBox1.Text = this.dataGridViewX1.SelectedRows[0].Cells["维修情况"].Value.ToString().Trim();
                //textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["保修情况"].Value.ToString().Trim();
                QUEUE2();
            }
        }

        private void QUEUE2()
        {

            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                string str = "select * from J_配件领用 where  维修编号 ='" + str2 + "'";
                str += ";select * from J_维修资料库 where  维修编号 ='" + str2 + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridViewX2.DataSource = ds.Tables[0];
                dataGridViewX3.DataSource = ds.Tables[1];
                //this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX2.Columns["配件名称"].Visible = false;
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
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                Form_LY frm = new Form_LY(str2);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    QUEUE2();
                }
            }
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                QUEUE2();
            }
        }

        private void delete_data()
        {
            if (this.dataGridViewX2.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX2.SelectedRows[0].Cells["序号"].Value.ToString();
                string str2 = this.dataGridViewX2.SelectedRows[0].Cells["领用数量"].Value.ToString();
                string str3 = this.dataGridViewX2.SelectedRows[0].Cells["配件编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_配件领用 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    str = "update J_配件信息 set 累计领用量=累计领用量-" + Convert.ToInt32(str2) + " where 配件编号='" + str3 + "'";
                    SqlCommand SQL3 = new SqlCommand(str, con);
                    SQL3.ExecuteNonQuery();
                    SQL3.Dispose();

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

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                DATA_upload frm = new DATA_upload(str2);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    QUEUE2();
                }
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data2();
                QUEUE2();
            }
        }
        private void delete_data2()
        {
            if (this.dataGridViewX3.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX3.SelectedRows[0].Cells["序号"].Value.ToString();

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_维修资料库 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();


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

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认状态吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    change_data();
                }
                //Queue_data();
               // 
            }
        }
        private void change_data()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                if (comboBoxEx1.Text == "已完工")
                {
                    if (richTextBox1.Text == "" )
                    {
                        MessageBox.Show("您还有维修过程未填写或者资料未上传！");
                        return;
                    }
                }

                string str = "update J_维修处理表 set 维修状态='" + comboBoxEx1.Text + "',维修情况='" + richTextBox1.Text + "'   where 维修编号='" + str2 + "'";

                SqlCommand SQL3 = new SqlCommand(str, con);
                SQL3.ExecuteNonQuery();
                SQL3.Dispose();
                this.dataGridViewX1.SelectedRows[0].Cells["维修状态"].Value = comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells["维修情况"].Value = richTextBox1.Text;
                if (comboBoxEx1.Text == "已完工")
                {
                    buttonX3.Enabled = true;
                    buttonX5.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确认完工吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string str1 = "update J_维修处理表 set 当前状态=3,完工日期='"+DateTime.Now.ToString()+"'  where 维修编号='" + str2 + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();
                        Queue_data();
                        DataTable dt = (DataTable)dataGridViewX2.DataSource;
                        dt.Clear();
                        dataGridViewX2.DataSource = dt;
                        dt = (DataTable)dataGridViewX3.DataSource;
                        dt.Clear();
                        dataGridViewX3.DataSource = dt;
                        ////dataGridViewX2.Rows.Clear();
                        //dataGridViewX3.Rows.Clear();
                        comboBoxEx1.Text = "";
                        richTextBox1.Text = "";
                        textBoxX6.Text = "";
                        buttonX3.Enabled = false;

                    }
                    catch
                    {

                    }
                    con.Close();
                }

            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认取消吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (this.dataGridViewX1.SelectedRows.Count > 0)
                {
                    try
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str2 = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();
                        string str1 = "update J_维修处理表 set 当前状态=1  where 维修编号='" + str2 + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();
                        Queue_data();
                        //dataGridViewX2.c
                        //dataGridViewX3.Rows.Clear();
                        comboBoxEx1.Text = "";
                        richTextBox1.Text = "";

                    }
                    catch
                    {

                    }
                    con.Close();
                }

            }
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

        private void buttonX10_Click(object sender, EventArgs e)
        {
            Form_PJ frm = new Form_PJ();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("申请成功！");
            }
        }
    }
}
