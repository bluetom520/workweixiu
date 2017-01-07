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
    public partial class Form_KHXX : Office2007Form
    {
        SqlConnection con;
        public Form_KHXX()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_WXBJ_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
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
                string str = "select * from J_客户信息 ";
                if (comboBoxEx1.Text != "" && comboBoxEx1.Text != "")
                {

                        str += " where 客户类别='" + comboBoxEx1.Text + "'";

                        str += " and 客户名称 like '%" + textBoxX6.Text + "%'";
                    
                }
                else
                {
                    if (comboBoxEx1.Text != "")
                    {
                        str += " where 客户类别='" + comboBoxEx1.Text + "'";
                    }
                    if (textBoxX6.Text != "")
                    {
                        str += " where 客户名称 like '^%" + textBoxX6.Text + "%'";
                    }
                }
                str += " order by 客户类别 ";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["报价编号"].Width = 70;
                //this.dataGridViewX1.Columns["修品大类"].Width = 70;
                //this.dataGridViewX1.Columns["修品小类"].Width =100;
                //this.dataGridViewX1.Columns["维修报价"].Width = 50;
                //this.dataGridViewX1.Columns["补充说明"].Width = 150;
                //this.dataGridViewX1.Columns["工时"].Width = 40;
                //this.dataGridViewX1.Columns["难度"].Width = 40;
                //this.dataGridViewX1.Columns["价值"].Width = 40;
                //this.dataGridViewX1.Columns["新品"].Width = 40;
            }
            catch
            {
            }
            con.Close();
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 名称 from J_基础信息表 where 类别='客户'";
                

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
                comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["客户类别"].Value.ToString().Trim();
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系人"].Value.ToString().Trim();
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系电话"].Value.ToString().Trim();
                textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系人2"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系电话2"].Value.ToString().Trim();
                textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系人3"].Value.ToString().Trim();
                textBoxX12.Text = this.dataGridViewX1.SelectedRows[0].Cells["联系电话3"].Value.ToString().Trim();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["通信地址"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["邮政编码"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["客户名称"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["开户行"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户"].Value.ToString().Trim();
                textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells["税号"].Value.ToString().Trim();
            }


        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.SelectedIndex >= 0&&textBoxX2.Text!="")
            {
                add_data();
                Queue_data();
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {



        }

        private void add_data()
        {
            try
            {
                if (textBoxX1.Text == "")
                    return;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string khbh = "KH" + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("HHmmss");
                string str = "insert into J_客户信息 VALUES('" + khbh + "','" + comboBoxEx1.Text + "','" + textBoxX6.Text + "','" + textBoxX1.Text + "','" + textBoxX2.Text + "','" + textBoxX10.Text + "','" + textBoxX3.Text + "','" + textBoxX11.Text + "','" + textBoxX12.Text + "','" + textBoxX5.Text + "','" + textBoxX4.Text + "','" + textBoxX7.Text + "','" + textBoxX8.Text + "','" + textBoxX9.Text + "')";
                SqlCommand SQL = new SqlCommand(str, con);
                SQL.ExecuteNonQuery();
                SQL.Dispose();
                default_data();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            con.Close();



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

                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["客户编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_客户信息 where  客户编号='" + str2 + "'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //decimal[] str_temp = new decimal[5];
                    //if (textBoxX3.Text != "")
                    //    str_temp[0] = Convert.ToDecimal(textBoxX3.Text);
                    //else
                    //    str_temp[0] = 0;
                    //if (textBoxX5.Text != "")
                    //    str_temp[1] = Convert.ToDecimal(textBoxX5.Text);
                    //else
                    //    str_temp[1] = 0;
                    //if (textBoxX6.Text != "")
                    //    str_temp[2] = Convert.ToDecimal(textBoxX6.Text);
                    //else
                    //    str_temp[2] = 0;
                    //if (textBoxX7.Text != "")
                    //    str_temp[3] = Convert.ToDecimal(textBoxX7.Text);
                    //else
                    //    str_temp[3] = 0;
                    //if (textBoxX8.Text != "")
                    //    str_temp[4] = Convert.ToDecimal(textBoxX8.Text);
                    //else
                    //    str_temp[4] = 0;
                    dt.Rows[0]["客户类别"] = comboBoxEx1.Text;
                    dt.Rows[0]["客户名称"] = textBoxX6.Text;
                    dt.Rows[0]["联系人"] = textBoxX1.Text;
                    dt.Rows[0]["联系电话"] = textBoxX2.Text;
                    dt.Rows[0]["联系人2"] = textBoxX10.Text;
                    dt.Rows[0]["联系电话2"] = textBoxX3.Text;
                    dt.Rows[0]["联系人3"] = textBoxX11.Text;
                    dt.Rows[0]["联系电话3"] = textBoxX12.Text;
                    dt.Rows[0]["通信地址"] = textBoxX4.Text;
                    dt.Rows[0]["邮政编码"] = textBoxX5.Text;
                    dt.Rows[0]["开户行"] = textBoxX7.Text;
                    dt.Rows[0]["账户"] = textBoxX8.Text;
                    dt.Rows[0]["税号"] = textBoxX9.Text;


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
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["客户编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_客户信息 where 客户编号='" + str1 + "'";
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

        private void comboBoxEx1_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void comboBoxEx1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxEx1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();

            //    string str1 = "select * from J_维修报价 where 修品大类='" + comboBoxEx1.Items[comboBoxEx1.SelectedIndex].ToString() + "'";

            //    SqlDataAdapter da = new SqlDataAdapter(str1, con);
            //    DataTable dt = new DataTable();

            //    da.Fill(dt);

            //    int num = dt.Rows.Count + 1;
            //    textBoxX1.Text = "ZY" + (comboBoxEx1.SelectedIndex + 1).ToString().PadLeft(2, '0') + num.ToString().PadLeft(3, '0');
            //    //textBoxX1.Text = "";
            //    textBoxX2.Text = "";
            //    textBoxX3.Text = "";
            //    textBoxX4.Text = "";
            //    textBoxX5.Text = "";
            //    textBoxX6.Text = "";
            //    //textBoxX7.Text = "";
            //    //textBoxX8.Text = "";
            //    //comboBoxEx2.Text = "";
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //con.Close();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxEx8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxX12_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxEx1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //textBoxX1.Text = "";
            //textBoxX2.Text = "";
            //textBoxX3.Text = "";
            //textBoxX4.Text = "";
            //textBoxX5.Text = "";
            //textBoxX6.Text = "";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX5.Text = "";
            textBoxX6.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            textBoxX9.Text = "";
            textBoxX11.Text = "";
            textBoxX12.Text = "";
            textBoxX10.Text = "";
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }
    }
}
