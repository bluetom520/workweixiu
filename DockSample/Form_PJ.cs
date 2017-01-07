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
    public partial class Form_PJ : Office2007Form
    {
        SqlConnection con;
        public Form_PJ()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认申请配件吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (textBoxX1.Text == "" || textBoxX3.Text == "")
                {
                    MessageBox.Show("您有带*号的参数未填写！");
                }
                else
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = "insert into J_配件申请 (配件型号,申请数量,配件品牌,申请人,申请日期)values('" + textBoxX3.Text + "','" + textBoxX1.Text + "','" + textBoxX2.Text + "'";
                        str += ",'" + LoginXT.username + "','" + DateTime.Now.ToString() + "')";

                        SqlCommand sqlcom = new SqlCommand(str, con);
                        sqlcom.ExecuteNonQuery();
                        sqlcom.Dispose();
                    }
                    catch
                    {

                    }
                    con.Close();

                    this.DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
