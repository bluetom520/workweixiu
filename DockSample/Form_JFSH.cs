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
    public partial class Form_JFSH : Office2007Form
    {
        SqlConnection con;
        string wxbh;
        string gzbz;
        private string dj;

        public string DJ
        {
            get { return dj; }

            set { dj = value; }
        }
        public Form_JFSH(string str_t)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str_t;

        }

        private void Form_JFSH_Load(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str = "select a.*,b.检测维修费用 from J_维修积分表 a left outer join J_报价详细表 b on a.维修编号=b.维修编号 where  a.维修编号='" + wxbh + "' and 审核标志=0 order by a.序号 desc";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string str_temp = dt.Rows[0]["检测维修费用"].ToString();
                    textBoxX7.Text = dt.Rows[0]["工时"].ToString();
                    textBoxX16.Text = dt.Rows[0]["难度"].ToString();

                    double bj = Convert.ToDouble(str_temp);
                    double result = Math.Sqrt(bj) / 10;
                    result = Math.Round(result, 1);
                    textBoxX8.Text = result.ToString();
                    textBoxX9.Text = dt.Rows[0]["新品"].ToString();
                    textBoxX12.Text = dt.Rows[0]["返修次数"].ToString();
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
            }
            catch
            {

            }
            con.Close();
        }
        private void cal_xj()
        {
            try
            {
                decimal temp = 0;
                decimal[] str_data = new decimal[4] { 0, 0, 0, 0 };
                decimal str_jf = 0;//小计

                decimal str_hj = 0;//合计

                decimal str_kf = 0;

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

                int num = Convert.ToInt32(textBoxX12.Text);

                for (int i = 0; i < str_data.Length; i++)
                {
                    temp += str_data[i];
                }

                if (textBoxX11.Text != "")
                {
                    str_kf = Convert.ToDecimal(textBoxX11.Text);
                }
                if (textBoxX10.Text != "")
                {
                    str_jf = Convert.ToDecimal(textBoxX10.Text);
                }

                int jf = 0;
                int kf = 0;
                if (num == 0)
                {
                    jf = 1;
                    kf = 0;
                }
                else
                {
                    jf = 1;
                    kf = num;

                }
                str_hj = temp + str_jf * jf - str_kf * num; ;
                
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
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
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
                string str1 = "update J_维修积分表 set 审核标志=1,日期='" + DateTime.Now.ToString() + "',工时=" + str_temp[0] + ",难度=" + str_temp[1] + " ,价值=" + str_temp[2] + " ,新品=" + str_temp[3];
                str1 += " ,加分=" + str_temp[4] + " ,扣分=" + str_temp[5] + " ,合计=" + str_temp[6] + " where 维修编号='" + wxbh + "' and 审核标志=0";
                SqlCommand SQL = new SqlCommand(str1, con);
                SQL.ExecuteNonQuery();
                SQL.Dispose();
                con.Close();
                dj = str_temp[6].ToString();
                this.DialogResult = DialogResult.OK;
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

        private void textBoxX7_TextChanged(object sender, EventArgs e)
        {
            cal_xj();
        }

        private void textBoxX8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
