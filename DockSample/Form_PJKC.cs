using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;
using WeifenLuo.WinFormsUI.Docking;
namespace DockSample
{
    public partial class Form_PJKC : DockContent
    {
        SqlConnection con;
        public static DataGridViewRow row;
        public static int hwnd = 0;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_PJKC()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_PJKC_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            if (MainForm.str_zlsc == "1")
            {
                toolStripButton3.Enabled = true;
            }
            Data_initial();
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 配件编号,配件类别,配件子类,存储位置,配件规格,配件型号,累计入库量,累计领用量,累计入库量-累计领用量 as 库存量,采购价,销售价,计量单位,补充说明 from J_配件信息 ";
                //if (comboBoxEx3.Text != "" && comboBoxEx2.Text != "")
                //{
                //    str += "where 配件子类'" + comboBoxEx3.Text + "' and 存储位置='" + comboBoxEx2.Text + "'";
                //}
                //else if (comboBoxEx3.Text != "" || comboBoxEx2.Text != "")
                //{
                //    str += "where ";
                //    if (comboBoxEx1.Text != "")
                //        str += " where 配件子类='" + comboBoxEx3.Text + "'";
                //    else
                //    {
                //        str += " where 存储位置='" + comboBoxEx2.Text + "'";
                //    }
                //}
                if (comboBoxEx1.Text != "")
                {
                    str += "where 配件类别='" + comboBoxEx1.Text + "'";
                }
                if (comboBoxEx3.Text != "")
                {
                    str += "and 配件子类='" + comboBoxEx3.Text + "'";
                }
                //if (comboBoxEx2.Text != "")
                //{
                //    str += "and  存储位置 like '%" + comboBoxEx2.Text + "%'";
                //}
                if (textBoxX10.Text != "")
                {
                    str += "where 配件型号 like '%" + textBoxX10.Text + "%'";
                }
                str += " order by 配件编号";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["配件编号"].Width = 60;
                //this.dataGridViewX1.Columns["配件类别"].Width = 70;
                //this.dataGridViewX1.Columns["配件子类"].Width = 70;
                //this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 100;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                //this.dataGridViewX1.Columns["累计入库量"].Width = 60;
                //this.dataGridViewX1.Columns["累计领用量"].Width = 60;
                //this.dataGridViewX1.Columns["库存量"].Width = 60;
                //this.dataGridViewX1.Columns["采购价"].Width = 60;
                //this.dataGridViewX1.Columns["销售价"].Width = 60;
                //this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;
                this.dataGridViewX1.Columns["配件名称"].Visible = false;

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
        private void Queue_data2()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "Select name from syscolumns Where ID=OBJECT_ID('J_配件信息')";
                SqlDataAdapter da2 = new SqlDataAdapter(str, con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);


                str = "select 配件编号,配件类别,配件子类,存储位置,配件规格,配件型号,累计入库量,累计领用量,累计入库量-累计领用量 as 库存量,采购价,销售价,计量单位,补充说明 from J_配件信息 ";

                if (textBoxX12.Text != "")
                {
                    str += " where ";
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        string temp = dt2.Rows[i][0].ToString();
                        if (temp.IndexOf("价") >= 0 || temp.IndexOf("累计") >= 0)
                        {
                            str += "isnull(convert(varchar," + temp + "),'')+";
                        }
                        else
                        {

                            str += "isnull(" + temp + ",'')+";
                        }
                    }
                    str = str.Substring(0, str.Length - 1);
                    str += " like '%" + textBoxX12.Text + "%'";
                }
                str += " order by 配件编号";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["配件编号"].Width = 60;
                //this.dataGridViewX1.Columns["配件类别"].Width = 70;
                //this.dataGridViewX1.Columns["配件子类"].Width = 70;
                //this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 100;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                //this.dataGridViewX1.Columns["累计入库量"].Width = 60;
                //this.dataGridViewX1.Columns["累计领用量"].Width = 60;
                //this.dataGridViewX1.Columns["库存量"].Width = 60;
                //this.dataGridViewX1.Columns["采购价"].Width = 60;
                //this.dataGridViewX1.Columns["销售价"].Width = 60;
                //this.dataGridViewX1.Columns["计量单位"].Width = 40;
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
        private void Data_initial()
        {

            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();

            //    string str1 = "select 名称 from J_基础信息表 where 类别='配件'";

            //    SqlDataAdapter da = new SqlDataAdapter(str1, con);

            //    DataSet ds = new DataSet();
            //    da.Fill(ds);

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //con.Close();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件分类 where PARENTID=1";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["name"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();


        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //textBoxX1.Text = "";
                //textBoxX2.Text = "";
                //textBoxX3.Text = "";
                //textBoxX4.Text = "";
                //textBoxX5.Text = "";
                //textBoxX6.Text = "";
                //textBoxX7.Text = "";
                //textBoxX8.Text = "";
                ////comboBoxEx1.Text = "";
                //comboBoxEx2.Text = "";
                //comboBoxEx3.Text = "";
                comboBoxEx3.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件分类 where PARENTID=(select id from J_配件分类 where name='" + comboBoxEx1.Text + "')";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt2 = new DataTable();
                da.Fill(dt2);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    comboBoxEx3.Items.Add(dt2.Rows[i]["name"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
            Queue_data();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Queue_data2(); 
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件类别"].Value.ToString().Trim();
                comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件子类"].Value.ToString().Trim();
                //comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString().Trim();
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["计量单位"].Value.ToString().Trim();
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["累计入库量"].Value.ToString().Trim();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件规格"].Value.ToString().Trim();
                //textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["补充说明"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["采购价"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["销售价"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["累计领用量"].Value.ToString().Trim();
                textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells["库存量"].Value.ToString().Trim();
                textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString().Trim();
                textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString().Trim();
                //textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells["库存量"].Value.ToString().Trim();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                change_data();
                Queue_data();
            }
        }
        private void change_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                //lb = comboBoxEx1.Text;
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_配件信息 where  配件编号='" + str2 + "'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    decimal[] str_temp = new decimal[2];
                    if (textBoxX6.Text != "")
                        str_temp[0] = Convert.ToDecimal(textBoxX6.Text);
                    else
                        str_temp[0] = 0;
                    if (textBoxX7.Text != "")
                        str_temp[1] = Convert.ToDecimal(textBoxX7.Text);
                    else
                        str_temp[1] = 0;
                    dt.Rows[0]["累计领用量"] = Convert.ToInt32(textBoxX8.Text);
                    //dt.Rows[0]["配件类别"] = comboBoxEx1.Text;
                    //dt.Rows[0]["存储位置"] = textBoxX3.Text;
                    //dt.Rows[0]["配件规格"] = textBoxX4.Text;
                    //dt.Rows[0]["配件单位"] = comboBoxEx2.Text;
                    //dt.Rows[0]["建议进货价"] = str_temp[0];
                    dt.Rows[0]["销售价"] = str_temp[1];
                    //dt.Rows[0]["适用机型"] = textBoxX5.Text;
                    dt.Rows[0]["补充说明"] = textBoxX5.Text;

                    da.Update(dt);
                    default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();


            }
            else
            {
                MessageBox.Show("请选择要修改的列！");
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
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            textBoxX9.Text = "";
            textBoxX10.Text = "";
            textBoxX11.Text = "";
            textBoxX12.Text = "";
            comboBoxEx1.Text = "";
            //comboBoxEx2.Text = "";
            comboBoxEx3.Text = "";
        }

        private void dataGridViewX1_DoubleClick(object sender, EventArgs e)
        {
            //row = this.dataGridViewX1.SelectedRows[0];
            //this.DialogResult = DialogResult.OK;
        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            default_data();
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Queue_data();
        }

        private void Form_PJKC_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void comboBoxEx1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void comboBoxEx3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    comboBoxEx2.Items.Clear();
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();

            //    string str1 = "select * from J_配件目录 where 配件子类='" + comboBoxEx3.Text + "'";

            //    SqlDataAdapter da = new SqlDataAdapter(str1, con);

            //    DataTable dt2 = new DataTable();
            //    da.Fill(dt2);

            //    for (int i = 0; i < dt2.Rows.Count; i++)
            //    {
            //        comboBoxEx2.Items.Add(dt2.Rows[i]["存储位置"].ToString());
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //con.Close();
            //Queue_data();
        }

        private void comboBoxEx2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
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
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_配件信息 where 配件编号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    default_data();
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
    }
}
