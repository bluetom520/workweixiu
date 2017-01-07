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
    public partial class Form_BJ : Office2007Form
    {
        SqlConnection con;
        decimal temp = 0;
        string ZT;
        string wxbh;
        string khbh;
        public Form_BJ(string t)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            ZT = t;

        }

        private void Form_BJ_Load(object sender, EventArgs e)
        {
            textBoxX1.Text = Form_JCBJ.row.Cells["维修编号"].Value.ToString();

            textBoxX2.Text = Form_JCBJ.row.Cells["修品大类"].Value.ToString();
            textBoxX3.Text = Form_JCBJ.row.Cells["修品小类"].Value.ToString();
            //textBoxX4.Text = Form_JCBJ.row.Cells["维修编号"].Value.ToString();
            textBoxX5.Text ="1";
            textBoxX6.Text = Form_JCBJ.row.Cells["故障描述"].Value.ToString();
            khbh = Form_JCBJ.row.Cells["客户编号"].Value.ToString();
            wxbh = textBoxX1.Text;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_维修报价 where  修品大类='" + textBoxX2.Text + "' and 修品小类='" + textBoxX3.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxX4.Text = dt.Rows[0]["维修报价"].ToString();
                }
                if (ZT == "XG")
                {
                    str = "select * from J_报价详细表 where  维修编号='" + wxbh + "'";
                    da = new SqlDataAdapter(str, con);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        textBoxX7.Text = dt.Rows[0]["检测维修费用"].ToString();
                        textBoxX8.Text = dt.Rows[0]["材料费用"].ToString();
                        textBoxX9.Text = dt.Rows[0]["现场费用"].ToString();
                        textBoxX10.Text = dt.Rows[0]["运输费用"].ToString();
                        textBoxX11.Text = dt.Rows[0]["管理费用"].ToString();
                    }
                }
                str = "select * from J_客户信息 where  客户编号='" + khbh + "'";
                da = new SqlDataAdapter(str, con);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxX16.Text = dt.Rows[0]["通信地址"].ToString();
                    textBoxX17.Text = dt.Rows[0]["联系人"].ToString();
                    textBoxX18.Text = dt.Rows[0]["邮政编码"].ToString();
                    textBoxX19.Text = dt.Rows[0]["联系电话"].ToString();
                    //textBoxX20.Text = dt.Rows[0]["移动电话"].ToString();
                    textBoxX21.Text = dt.Rows[0]["客户类别"].ToString();
                    textBoxX22.Text = dt.Rows[0]["客户名称"].ToString();
                }


            }
            catch
            {

            }
            con.Close();
            string temp = Form_JCBJ.row.Cells["预付款"].Value.ToString();
            if (temp != "")
            {
                if (Convert.ToDecimal(temp) > 0)
                {
                    textBoxX15.Text = temp;
                    checkBoxX1.Enabled = false;
                }
            }
        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                Form_XJSZ frm = new Form_XJSZ(textBoxX1.Text);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    textBoxX15.Text = frm.DJ;
                }
            }
        }

        private void textBoxX7_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void cal_xj()
        {
            try
            {
                temp = 0;
                decimal[] str_data = new decimal[4];
                decimal str_xj = 0;//小计
                decimal str_sv = 0;//税金
                decimal str_hj = 0;//合计
                decimal str_gl=0;//管理

                if (textBoxX7.Text != "")
                {
                    str_data[0] = Convert.ToDecimal(textBoxX7.Text);
                }
                if (textBoxX8.Text != "")
                {
                    str_data[1] = Convert.ToDecimal(textBoxX8.Text);
                }
                if (textBoxX9.Text != "")
                {
                    str_data[2] = Convert.ToDecimal(textBoxX9.Text);
                }
                if (textBoxX10.Text != "")
                {
                    str_data[3] = Convert.ToDecimal(textBoxX10.Text);
                }
                //if (textBoxX7.Text != "")
                //{
                //    str_data[4] = Convert.ToDecimal(textBoxX7.Text);
                //}
                for (int i =0; i < str_data.Length; i++)
                {
                    temp += str_data[i];
                }
                //str_gl=(temp * 9 / 100);
                //textBoxX11.Text = str_gl.ToString();
                if (textBoxX11.Text != "")
                {
                   str_gl= Convert.ToDecimal(textBoxX11.Text);
                }
                str_xj = temp + str_gl;
                textBoxX12.Text = str_xj.ToString();
                //str_sv = str_xj * 17 / 100;
                //textBoxX13.Text = str_sv.ToString();
                if (textBoxX13.Text != "")
                {
                    str_sv = Convert.ToDecimal(textBoxX13.Text);
                }
                str_hj = str_xj + str_sv;
                textBoxX14.Text = str_hj.ToString();
            }

            catch
            {

            }
        }

        private void textBoxX16_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX8_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX9_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX10_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX11_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX13_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX11_KeyUp(object sender, KeyEventArgs e)
        {
            //try
            //{

            //    decimal str_xj = 0;//小计
            //    decimal str_sv = 0;//税金
            //    decimal str_hj = 0;//合计
            //    decimal str_gl = 0;//管理

            //    if (textBoxX11.Text != "")
            //        str_gl = Convert.ToDecimal(textBoxX11.Text);
            //    //str_gl = (temp * 9 / 100);
            //    //textBoxX11.Text = str_gl.ToString();
            //    str_xj = temp + str_gl;
            //    textBoxX12.Text = str_xj.ToString();
            //    str_sv = str_xj * 17 / 100;
            //    textBoxX13.Text = str_sv.ToString();
            //    str_hj = str_xj + str_sv;
            //    textBoxX14.Text = str_hj.ToString();
            //}

            //catch
            //{

            //}
        }

        private void textBoxX13_KeyUp(object sender, KeyEventArgs e)
        {
            decimal str_xj = 0;//小计
            decimal str_sv = 0;//税金
            decimal str_hj = 0;//合计



            str_xj = Convert.ToDecimal(textBoxX12.Text);

            if (textBoxX13.Text != "")
                str_sv = Convert.ToDecimal(textBoxX13.Text);

            str_hj = str_xj + str_sv;
            textBoxX14.Text = str_hj.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认报价吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //////////////////////////////////////////////

                    decimal[] str_temp = new decimal[9];
                    if (textBoxX7.Text != "")
                        str_temp[0] = Convert.ToDecimal(textBoxX7.Text);
                    else
                        str_temp[0] = 0;
                    //if (textBoxX7.Text != "")
                    //    str_temp[1] = Convert.ToDecimal(textBoxX7.Text);
                    //else
                        str_temp[1] = 0;
                    if (textBoxX8.Text != "")
                        str_temp[2] = Convert.ToDecimal(textBoxX8.Text);
                    else
                        str_temp[2] = 0;
                    if (textBoxX9.Text != "")
                        str_temp[3] = Convert.ToDecimal(textBoxX9.Text);
                    else
                        str_temp[3] = 0;
                    if (textBoxX10.Text != "")
                        str_temp[4] = Convert.ToDecimal(textBoxX10.Text);
                    else
                        str_temp[4] = 0;

                    if (textBoxX11.Text != "")
                        str_temp[5] = Convert.ToDecimal(textBoxX11.Text);
                    else
                        str_temp[5] = 0;
                    if (textBoxX12.Text != "")
                        str_temp[6] = Convert.ToDecimal(textBoxX12.Text);
                    else
                        str_temp[6] = 0;
                    if (textBoxX13.Text != "")
                        str_temp[7] = Convert.ToDecimal(textBoxX13.Text);
                    else
                        str_temp[7] = 0;
                    if (textBoxX14.Text != "")
                        str_temp[8] = Convert.ToDecimal(textBoxX14.Text);
                    else
                        str_temp[8] = 0;
                    string str = "";
                    if (ZT == "BJ")
                    {
                        str = "insert into J_报价详细表 values('" + textBoxX1.Text + "'," + str_temp[0] + "";
                        str += ", " + str_temp[1] + "," + str_temp[2] + "," + str_temp[3] + "," + str_temp[4] + "," + str_temp[5] + "," + str_temp[6] + "," + str_temp[7] + "," + str_temp[8] + ",'" + LoginXT.username + "','" + DateTime.Now.ToString() + "')";
                    }
                    else
                    {
                        str = "update J_报价详细表 set 检测维修费用=" + str_temp[0] + ",材料费用=" + str_temp[2] + ",现场费用=" + str_temp[3] + ",运输费用=" + str_temp[4] + ",管理费用=" + str_temp[5] + ",小计费用=" + str_temp[6] + ",税金=" + str_temp[7] + ",总计费用=" + str_temp[8] + ",报价时间='" + DateTime.Now.ToString() + "' where 维修编号='"+wxbh+"'";
                    }

                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();


                    //////////////////////////////////////////////



                    decimal[] str_temp2 = new decimal[2];
                    if (textBoxX14.Text != "")
                        str_temp2[0] = Convert.ToDecimal(textBoxX14.Text);
                    else
                        str_temp2[0] = 0;
                    if (textBoxX15.Text != "")
                        str_temp2[1] = Convert.ToDecimal(textBoxX15.Text);
                    else
                        str_temp2[1] = 0;
                    string str1 = "update J_维修处理表 set 维修报价=" + str_temp2[0] + ",预付款=" + str_temp2[1] + "  where 维修编号='" + textBoxX1.Text + "'";
                    SqlCommand SQL3 = new SqlCommand(str1, con);
                    SQL3.ExecuteNonQuery();
                    SQL3.Dispose();
                    Form_JCBJ.row.Cells["维修报价"].Value = str_temp2[0];
                    Form_JCBJ.row.Cells["预付款"].Value = str_temp2[1];

                }
                catch
                {

                }
                con.Close();
                this.DialogResult = DialogResult.OK;
                //this.Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cal_sj();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            cal_sj();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            cal_sj();
        }

        private void cal_sj()
        {
            decimal str_sv = 0;//税金
            //decimal str_temp = 0;//合计
            decimal str_xj = 0;//小计
            if (textBoxX12.Text != "")
            {
                str_xj = Convert.ToDecimal(textBoxX12.Text);

            }
            if (radioButton1.Checked)
            {
                str_sv = 0;
            }
            else if (radioButton2.Checked)
            {
                str_sv = str_xj * 6 / 100;

            }
            else
            {
                str_sv = str_xj * 17 / 100;
                //textBoxX13.Text = str_sv.ToString();
            }
            textBoxX13.Text = str_sv.ToString();


        }

    
    }
}
