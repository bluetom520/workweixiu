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
    public partial class Form_PG : Office2007Form
    {
        SqlConnection con;
        decimal temp = 0;
        public Form_PG()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_BJ_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            string str_temp = Form_WXPG.row.Cells["检测维修费用"].Value.ToString();
            textBoxX1.Text = Form_WXPG.row.Cells["维修编号"].Value.ToString();
            textBoxX2.Text = Form_WXPG.row.Cells["修品大类"].Value.ToString();
            textBoxX3.Text = Form_WXPG.row.Cells["修品小类"].Value.ToString();
            textBoxX4.Text = Form_WXPG.row.Cells["维修报价"].Value.ToString();
            TimeSpan t1= Convert.ToDateTime(Form_WXPG.row.Cells["预约日期"].Value.ToString()) - Convert.ToDateTime(Form_WXPG.row.Cells["接修日期"].Value.ToString());
            textBoxX5.Text = t1.Days.ToString();
            textBoxX6.Text = Form_WXPG.row.Cells["故障描述"].Value.ToString();//返修次数
            textBoxX12.Text = Form_WXPG.row.Cells["返修次数"].Value.ToString();
            double bj = Convert.ToDouble(str_temp);
            double result = Math.Sqrt(bj) / 10;
            result = Math.Round(result, 1);
            textBoxX8.Text = result.ToString();
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
                    
                    textBoxX7.Text = dt.Rows[0]["工时"].ToString();
                    //textBoxX8.Text = dt.Rows[0]["价值"].ToString();

                    textBoxX9.Text = dt.Rows[0]["新品"].ToString();
                    textBoxX16.Text = dt.Rows[0]["难度"].ToString();

                }
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

                string str1 = "select 姓名 from J_员工信息表 where 部门='维修部'";

                str1 += ";select 名称 from J_基础信息表 where 类别='送修'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["姓名"].ToString());
                }

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    comboBoxEx2.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }
        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {

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
                decimal[] str_data = new decimal[4]{0,0,0,0};
                decimal str_jf = 0;//小计

                decimal str_hj = 0;//合计


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
                if (textBoxX16.Text != "")
                {
                    str_data[3] = Convert.ToDecimal(textBoxX16.Text);
                }
                //switch ((int)str_data[3])
                //{
                //    case 0: textBoxX10.Text ="0";
                //        textBoxX11.Text = "2";
                //        break;
                //    case 1: textBoxX10.Text = "1";
                //        textBoxX11.Text = "2";
                //        break;
                //    case 2: textBoxX10.Text = "2";
                //        textBoxX11.Text = "1";
                //        break;
                //    case 3: textBoxX10.Text = "2";
                //        textBoxX11.Text = "1";
                //        break;
                //}
                for (int i = 0; i < str_data.Length; i++)
                {
                    temp += str_data[i];
                }
                str_jf = Convert.ToInt32(textBoxX10.Text);
                str_hj = temp + str_jf;

                //str_gl=(temp * 9 / 100);
                //textBoxX11.Text = str_gl.ToString();
                //str_xj = temp + str_gl;
                //textBoxX12.Text = str_xj.ToString();
                //str_sv = str_xj * 17 / 100;
                //textBoxX13.Text = str_sv.ToString();
                //str_hj = str_xj + str_sv;
                textBoxX14.Text = str_hj.ToString();
            }
            catch
            {

            }
        }

        private void textBoxX16_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(textBoxX16.Text) < 3)
            {
                textBoxX11.Text = (3 - Convert.ToDouble(textBoxX16.Text)).ToString();
            }
            else
            {
                textBoxX11.Text = "0";
            }
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

        }

        private void textBoxX11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxX13_TextChanged(object sender, EventArgs e)
        {

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
            //decimal str_xj = 0;//小计
            //decimal str_sv = 0;//税金
            //decimal str_hj = 0;//合计



            //str_xj = Convert.ToDecimal(textBoxX12.Text);

            //if (textBoxX13.Text != "")
            //    str_sv = Convert.ToDecimal(textBoxX13.Text);

            //str_hj = str_xj + str_sv;
            //textBoxX14.Text = str_hj.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认派工吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //////////////////////////////////////////////
                    if (!checkBoxX1.Checked)
                    {
                        if (comboBoxEx1.Text == "")
                        {
                            MessageBox.Show("请选择技术员名称!");
                            return;
                        }
                        decimal[] str_temp = new decimal[7];
                        if (textBoxX7.Text != "")
                            str_temp[0] = Convert.ToDecimal(textBoxX7.Text);
                        else
                            str_temp[0] = 0;
                        if (textBoxX16.Text != "")
                            str_temp[1] = Convert.ToDecimal(textBoxX16.Text);
                        else
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
                        if (textBoxX14.Text != "")
                            str_temp[6] = Convert.ToDecimal(textBoxX14.Text);
                        else
                            str_temp[6] = 0;


                        string str = "delete from J_维修积分表 where 维修编号='" + textBoxX1.Text + "'";//删除原来的积分表
                        SqlCommand sqlcom = new SqlCommand(str, con);
                        sqlcom.ExecuteNonQuery();


                        str = "insert into J_维修积分表 values('" + textBoxX1.Text + "'," + str_temp[0] + "";
                        str += ", " + str_temp[1] + "," + str_temp[2] + "," + str_temp[3] + "," + str_temp[4] + "," + str_temp[5] + "," + str_temp[6] + ",'" + comboBoxEx1.Text + "','" + DateTime.Now.ToString() + "',0,0,0)";

                        sqlcom = new SqlCommand(str, con);
                        sqlcom.ExecuteNonQuery();
                        sqlcom.Dispose();

                        //////////////////////////////////////////////



                        //decimal[] str_temp2 = new decimal[2];
                        //if (textBoxX14.Text != "")
                        //    str_temp2[0] = Convert.ToDecimal(textBoxX14.Text);
                        //else
                        //    str_temp2[0] = 0;
                        //if (textBoxX15.Text != "")
                        //    str_temp2[1] = Convert.ToDecimal(textBoxX15.Text);
                        //else
                        //    str_temp2[1] = 0;
                        string str1 = "update J_维修处理表 set 技术员='" + comboBoxEx1.Text + "',预计积分=" + str_temp[6] + ",当前状态=2,返修次数=0  where 维修编号='" + textBoxX1.Text + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();
                    }
                    else
                    {
                        if (comboBoxEx2.Text == "")
                        {
                            MessageBox.Show("请选择送修公司名称!");
                            return;
                        }
                        string str1 = "update J_维修处理表 set 技术员='" + comboBoxEx2.Text + "',预计积分=" + 0 + ",当前状态=10,返修次数=0,送修标志=1  where 维修编号='" + textBoxX1.Text + "'";
                        SqlCommand SQL3 = new SqlCommand(str1, con);
                        SQL3.ExecuteNonQuery();
                        SQL3.Dispose();
                    }
                    //Form_WXPG.row.Cells[" 技术员"].Value = LoginXT.username;
                    //Form_WXPG.row.Cells["预计积分"].Value = str_temp2[6];

                }
                catch
                {

                }
                con.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxX10_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                comboBoxEx1.Enabled = false;
                comboBoxEx2.Enabled = true;
                buttonX1.Text = "送修";
            }
            else
            {
                comboBoxEx1.Enabled = true;
                comboBoxEx2.Enabled = false;
                buttonX1.Text = "派工";
            }
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    
    }
}
