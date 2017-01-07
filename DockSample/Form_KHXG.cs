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
    public partial class Form_KHXG : Office2007Form
    {
        SqlConnection con;
        private string num;
        //string wxbh;

        public string khbh
        {
            get { return num; }

            set { num = value; }
        }
        public Form_KHXG()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_KHXG_Load(object sender, EventArgs e)
        {
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 客户编号,客户名称 from J_客户信息 ";
                //if (comboBoxEx1.Text != "" && comboBoxEx1.Text != "")
                //{

                //    str += " where 客户类别='" + comboBoxEx1.Text + "'";

                //    str += " and 客户名称 like '%" + textBoxX6.Text + "%'";

                //}
                //else
                //{
                //    if (comboBoxEx1.Text != "")
                //    {
                //        str += " where 客户类别='" + comboBoxEx1.Text + "'";
                //    }
                //    if (textBoxX6.Text != "")
                //    {
                //        str += " where 客户名称 like '^%" + textBoxX6.Text + "%'";
                //    }
                //}
                //str += " order by 客户类别 ";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["报价编号"].Width = 70;
                this.dataGridViewX1.Columns["客户名称"].Width =200;
                //this.dataGridViewX1.Columns["修品小类"].Width =1000;
                //this.dataGridViewX1.Columns["维修报价"].Width = 50;
                //this.dataGridViewX1.Columns["补充说明"].Width = 150;
                //this.dataGridViewX1.Columns["工时"].Width = 40;
                //this.dataGridViewX1.Columns["难度"].Width = 40;
                //this.dataGridViewX1.Columns["价值"].Width = 40;
                //this.dataGridViewX1.Columns["新品"].Width = 40;
            }
            catch
            {
            }
            con.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {

                num = this.dataGridViewX1.SelectedRows[0].Cells["客户编号"].Value.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("请选择要修改的列！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
