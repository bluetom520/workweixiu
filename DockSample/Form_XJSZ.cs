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
    public partial class Form_XJSZ : Office2007Form
    {
        SqlConnection con;
        string wxbh;
        private string  dj;

        public string DJ
        {
            get{ return dj;}

            set { dj = value; }
        }
        public Form_XJSZ(string str)
        {
            InitializeComponent();
            wxbh = str;
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_XJSZ_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //change_data();
                //Queue_data();
                if (comboBoxEx1.Text == "" || comboBoxEx2.Text == "" || textBoxX1.Text == "")
                    return;
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    if (comboBoxEx2.Text != "")
                    {
                        if (!comboBoxEx2.Items.Contains(comboBoxEx2.Text))
                        {
                            string str = "insert into J_商品信息表  (名称,类别)VALUES('" + comboBoxEx2.Text + "','项目')";
                            SqlCommand SQL = new SqlCommand(str, con);
                            SQL.ExecuteNonQuery();
                            SQL.Dispose();
                        }
                    }

                    //if (comboBoxEx8.Text != "")
                    //{
                    //    if (!comboBoxEx8.Items.Contains(comboBoxEx8.Text))
                    //    {

                    //        khbh = "KH" + DateTime.Now.Year.ToString("yyyy") + DateTime.Now.ToString("HHmmss");
                    //        string str = "insert into J_客户信息 VALUES('" + khbh + "','" + comboBoxEx8.Text + "','" + textBoxX10.Text + "','" + textBoxX11.Text + "','" + textBoxX12.Text + "','" + textBoxX13.Text + "','" + textBoxX9.Text + "')";
                    //        SqlCommand SQL = new SqlCommand(str, con);
                    //        SQL.ExecuteNonQuery();
                    //        SQL.Dispose();
                    //    }
                    //}
                    decimal[] str_temp = new decimal[2]{0,0};
                    if (radioButton1.Checked)
                    {

                        if (textBoxX1.Text != "")
                            str_temp[0] = Convert.ToDecimal(textBoxX1.Text);
                        else
                            str_temp[0] = 0;

                    }
                    else
                    {
                        if (textBoxX1.Text != "")
                            str_temp[1] = Convert.ToDecimal(textBoxX1.Text);
                        else
                            str_temp[1] = 0;
                    }
                    //string[] str_data = new string[10];
                    //str_data[0] = textBoxX1.Text;
                    //str_data[1] = DateTime.Now.ToString();
                    //str_data[2] = comboBoxEx1.Text;
                    //str_data[3] = comboBoxEx2.Text;
                    //str_data[4] = textBoxX2.Text;
                    //str_data[5] = comboBoxEx3.Text;
                    //str_data[6] = textBoxX3.Text;
                    //str_data[7] = textBoxX2.Text;
                    //str_data[8] = textBoxX2.Text;
                    //str_data[9] = textBoxX2.Text;
                    ////str_data[10] = comboBoxEx5.Text;
                    //str_data[11] = comboBoxEx6.Text;
                    //str_data[12] = comboBoxEx7.Text;
                    //str_data[13] = khbh;
                    //str_data[14] = LoginXT.username;
                    //int num = 30;
                    //if (textBoxX7.Text != "")
                    //{
                    //    num = Convert.ToInt32(textBoxX7.Text);
                    //}

                    

                    string str1 = "insert into J_收支流水 (日期,收入,支出,账户,往来单位,项目名称,维修编号,开票金额,发票号码,经办人,开票日期,摘要明细)values('" + DateTime.Now.ToString() + "'," + str_temp[0] + "";
                    str1 += "," + str_temp[1] + ",'" + comboBoxEx1.Text + "','" + comboBoxEx3.Text + "','" + comboBoxEx2.Text + "','" + wxbh + "','" + textBoxX2.Text + "','" + textBoxX3.Text + "','" + LoginXT.username + "','" +dateTimePicker1.Value.ToString() + "','" +richTextBox1.Text+ "')";

                    SqlCommand SQL2 = new SqlCommand(str1, con);
                    SQL2.ExecuteNonQuery();
                    SQL2.Dispose();
                    //decimal temp=0;
                    //if (radioButton1.Checked)
                    //{
                    //    temp= str_temp[0];
                    //}
                    //else
                    //{
                    //    temp=0- str_temp[1];
                    //}
                    //str1 = "update J_收支账户 set 账户结余=账户结余+" + temp + " where 账户名称='" + comboBoxEx1.Text + "'";
                    //SqlCommand SQL3 = new SqlCommand(str1, con);
                    //SQL3.ExecuteNonQuery();
                    //SQL3.Dispose();
                    dj = textBoxX1.Text;
                    default_data();

                }
                catch
                {

                }
                con.Close();
                
                Data_initial();
                
                this.DialogResult = DialogResult.OK;
                
            }
        }
        private void default_data()
        {
            // comboBox1.Text = "";
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";

            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
            comboBoxEx3.Text = "";

        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
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
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {

                    comboBoxEx3.Items.Add(ds.Tables[2].Rows[i]["客户名称"].ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }
    }
}
