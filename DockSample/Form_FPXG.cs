using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
namespace DockSample
{
    public partial class Form_FPXG : Office2007Form
    {
        string wxbh;
        SqlConnection con;
        string gzbz;
        public Form_FPXG(string str_temp, string str_2)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str_temp;
            gzbz = str_2;
        }

        private void Form_FPXG_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str1;
                if(gzbz=="1")
                    str1 = "select * from J_应收明细 a left outer join  J_客户信息 b on a.客户编号=b.客户编号 where  维修编号='" + wxbh + "'";
                else
                    str1 = "select * from J_收支流水 where  维修编号 ='" + wxbh + "' and 项目名称='维修收费'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                //{
                //    comboBoxEx4.Items.Add(ds.Tables[3].Rows[i]["名称"].ToString());
                //}
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{
                //    comboBoxEx9.Items.Add(ds.Tables[2].Rows[i]["名称"].ToString());
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    textBoxX1.Text = wxbh;
                    if (gzbz == "1")
                    {

                        textBoxX2.Text = ds.Tables[0].Rows[0]["客户名称"].ToString();
                        textBoxX3.Text = ds.Tables[0].Rows[0]["开票金额"].ToString();
                        textBoxX4.Text = ds.Tables[0].Rows[0]["发票号码"].ToString();
                        textBoxX5.Text = ds.Tables[0].Rows[0]["摘要明细"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["开票日期"].ToString());
                    }
                    else
                    {
                        textBoxX2.Text = ds.Tables[0].Rows[0]["往来单位"].ToString();
                        textBoxX3.Text = ds.Tables[0].Rows[0]["开票金额"].ToString();
                        textBoxX4.Text = ds.Tables[0].Rows[0]["发票号码"].ToString();
                        textBoxX5.Text = ds.Tables[0].Rows[0]["摘要明细"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["开票日期"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void textBoxX50_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str1;
                if (gzbz == "1")
                {
                    str1 = "update J_应收明细 set  开票金额=" + Convert.ToDecimal(textBoxX3.Text) + ",发票号码='" + textBoxX4.Text + "',摘要明细='" + textBoxX5.Text + "'";
                    str1 += ",开票日期='" + dateTimePicker1.Value.ToString()+ "'  where 维修编号='" + wxbh + "'";
                }
                else
                {
                    str1 = "update J_收支流水 set  开票金额=" + Convert.ToDecimal(textBoxX3.Text) + ",发票号码='" + textBoxX4.Text + "',摘要明细='" + textBoxX5.Text + "'";
                    str1 += ",开票日期='" + dateTimePicker1.Value.ToString() + "'  where  维修编号 ='" + wxbh + "' and 项目名称='维修收费'";
                }

                SqlCommand SQL = new SqlCommand(str1, con);
                SQL.ExecuteNonQuery();
                SQL.Dispose();
                con.Close();

                this.DialogResult = DialogResult.OK;
            }

            catch
            {

            }
        }

    }
}
