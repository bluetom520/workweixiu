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
    public partial class Form_KPZL : Office2007Form
    {
        SqlConnection con;
        string khmc;
        public Form_KPZL(string t)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            khmc = t;
        }

        private void Form_KPZL_Load(object sender, EventArgs e)
        {
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_客户信息 where 客户名称='"+khmc+"'";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    textBoxX1.Text = dt.Rows[0]["联系人"].ToString().Trim();
                    textBoxX2.Text = dt.Rows[0]["联系电话"].ToString().Trim();
                    textBoxX3.Text = dt.Rows[0]["移动电话"].ToString().Trim();
                    textBoxX4.Text = dt.Rows[0]["通信地址"].ToString().Trim();
                    //textBoxX5.Text = dt.Rows[0]["邮政编码"].ToString().Trim();
                    textBoxX6.Text = dt.Rows[0]["客户名称"].ToString().Trim();
                    textBoxX7.Text = dt.Rows[0]["开户行"].ToString().Trim();
                    textBoxX8.Text = dt.Rows[0]["账户"].ToString().Trim();
                    textBoxX9.Text = dt.Rows[0]["税号"].ToString().Trim();
                }

            }
            catch
            {
            }
            con.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
