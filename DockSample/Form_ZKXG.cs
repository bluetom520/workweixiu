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
    public partial class Form_ZKXG : Office2007Form
    {
        public static DataGridViewRow row;
        //string wxbh;
        SqlConnection con;
        string xh;
        bool flag = true;//true  收入 false 支出
        public Form_ZKXG()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_ZKXG_Load(object sender, EventArgs e)
        {
            xh = Form_SZSH.row.Cells["序号"].Value.ToString();
            textBoxX3.Text = Form_SZSH.row.Cells["账户"].Value.ToString();
            textBoxX2.Text = Form_SZSH.row.Cells["项目名称"].Value.ToString();
            decimal temp = Convert.ToDecimal(Form_SZSH.row.Cells["收入"].Value.ToString());
            if (temp > 0)
            {
                textBoxX1.Text = Form_SZSH.row.Cells["收入"].Value.ToString();
                flag = true;
            }
            else
            {
                textBoxX1.Text = Form_SZSH.row.Cells["支出"].Value.ToString();
                flag = false;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //change_data();
                //Queue_data();
                if ( textBoxX1.Text == "")
                    return;
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //if (comboBoxEx2.Text != "")
                    //{
                    //    if (!comboBoxEx2.Items.Contains(comboBoxEx2.Text))
                    //    {
                    //        string str = "insert into J_商品信息表  (名称,类别)VALUES('" + comboBoxEx2.Text + "','项目')";
                    //        SqlCommand SQL = new SqlCommand(str, con);
                    //        SQL.ExecuteNonQuery();
                    //        SQL.Dispose();
                    //    }
                    //}

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
                    decimal[] str_temp = new decimal[2] { 0, 0 };
                    if (flag)
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



                    string str1 = "update J_收支流水 set 收入=" + str_temp[0] + ",支出=" + str_temp[1]  ;
                    str1 += " where 序号='"+xh+"'";

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
                    //str1 = "update J_收支账户 set 账户结余=账户结余+" + temp + " where 账户名称='" + comboBoxEx1.Text + "'";//审核后入账
                    //SqlCommand SQL3 = new SqlCommand(str1, con);
                    //SQL3.ExecuteNonQuery();
                    //SQL3.Dispose();
                    //dj = textBoxX1.Text;
                    //default_data();

                }
                catch
                {

                }
                con.Close();

                //Data_initial();

                this.DialogResult = DialogResult.OK;

            }
        }
    }
}
