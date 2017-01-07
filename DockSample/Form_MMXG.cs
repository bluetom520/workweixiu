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
    public partial class Form_MMXG : Office2007Form
    {

        SqlConnection con_1;
        public Form_MMXG()
        {
            InitializeComponent();
            con_1 = new SqlConnection(MainForm.connetstring);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            label2.Text = LoginXT.username;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con_1.Open();
                string str = "select * from J_员工信息表 where 姓名='" + label2.Text + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con_1);
                SqlCommandBuilder t_build = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                string str1 = dt.Rows[0]["密码"].ToString().Trim();
                if (str1 == textBox1.Text)
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        dt.Rows[0]["密码"] = textBox2.Text;
                        da.Update(dt);
                    }
                    else
                    {
                        MessageBox.Show("您输入的两次密码不一致,请重新输入！");
                    }
                }
                else
                {
                    MessageBox.Show("您输入的旧密码错误，请重新输入！");
                }
                MessageBox.Show("修改成功！");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con_1.Close();

            }
        }
    }
}
