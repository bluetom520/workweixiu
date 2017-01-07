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
    public partial class Form_GSXX : Office2007Form
    {
        SqlConnection con;
        public Form_GSXX()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_GSXX_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_公司信息 ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                textBoxX1.Text = dt.Rows[0]["传真电话"].ToString();
                textBoxX2.Text = dt.Rows[0]["移动电话"].ToString();
                textBoxX3.Text = dt.Rows[0]["电子信箱"].ToString();
                textBoxX4.Text = dt.Rows[0]["公司地址"].ToString();
                textBoxX5.Text = dt.Rows[0]["邮政编码"].ToString();
                textBoxX6.Text = dt.Rows[0]["公司名称"].ToString();
                textBoxX7.Text = dt.Rows[0]["总负责人"].ToString();
                textBoxX8.Text = dt.Rows[0]["服务电话"].ToString();
                textBoxX9.Text = dt.Rows[0]["公司网站"].ToString();
                richTextBox1.Text = dt.Rows[0]["报价合同"].ToString();

            }
            catch
            {
            }
            con.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                change_data();
                this.Close();
            }
        }
        private void change_data()
        {

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_公司信息";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    dt.Rows[0]["传真电话"] = textBoxX1.Text;
                    dt.Rows[0]["移动电话"] = textBoxX2.Text;
                    dt.Rows[0]["电子信箱"] = textBoxX3.Text;
                    dt.Rows[0]["公司地址"] = textBoxX4.Text;
                    dt.Rows[0]["邮政编码"] = textBoxX5.Text;
                    dt.Rows[0]["公司名称"] = textBoxX6.Text;
                    dt.Rows[0]["总负责人"] = textBoxX7.Text;
                    dt.Rows[0]["服务电话"] = textBoxX8.Text;
                    dt.Rows[0]["公司网站"] = textBoxX9.Text;
                    dt.Rows[0]["报价合同"] = richTextBox1.Text;


                    da.Update(dt);
                    //default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();



        }
    }
}
