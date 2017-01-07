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
    public partial class Form_ZHGL : Office2007Form
    {
        SqlConnection con;
        public Form_ZHGL()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_ZHGL_Load(object sender, EventArgs e)
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
                string str = "select * from J_收支账户 ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;

                this.dataGridViewX1.Columns["账户备注"].Width = 120;
                //this.dataGridViewX1.Columns["修品大类"].Width = 70;
                //this.dataGridViewX1.Columns["修品小类"].Width =100;
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

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.SelectedIndex >= 0 && textBoxX2.Text != "")
            {
                add_data();
                Queue_data();
            }
        }

        private void add_data()
        {
            try
            {
                if (textBoxX1.Text == "")
                    return;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                decimal str_temp = 0; ;
                if (textBoxX1.Text != "")
                    str_temp = Convert.ToDecimal(textBoxX1.Text);






                string str = "insert into J_收支账户 values('" + comboBoxEx1.Text + "','" + textBoxX2.Text + "'," + str_temp + ",'" + textBoxX5.Text + "','" + textBoxX4.Text + "','" + textBoxX6.Text + "'";
                str += ",'" + textBoxX3.Text + "')";

                SqlCommand sqlcom = new SqlCommand(str, con);
                sqlcom.ExecuteNonQuery();
                default_data();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            con.Close();



        }
        private void default_data()
        {
            // comboBox1.Text = "";
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX5.Text = "";
            textBoxX6.Text = "";
            comboBoxEx1.Text = "";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                change_data();
                Queue_data();
            }
        }

        private void change_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {

                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_收支账户 where 流水号='" + str2 + "'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    decimal str_temp = 0; ;
                    if (textBoxX1.Text != "")
                        str_temp = Convert.ToDecimal(textBoxX1.Text);
                    dt.Rows[0]["账户结余"] = str_temp;
                    dt.Rows[0]["账户备注"] = textBoxX3.Text;
                    dt.Rows[0]["开户行"] = textBoxX5.Text;
                    dt.Rows[0]["开户名"] = textBoxX4.Text;
                    dt.Rows[0]["账户"] = textBoxX6.Text;
                    da.Update(dt);
                    default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();


            }
            else
            {
                MessageBox.Show("请选择要修改的列！");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                Queue_data();
            }
        }
        private void delete_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_收支账户 where 流水号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    default_data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                con.Close();

            }
            else
            {
                MessageBox.Show("请选择要删除的列！");
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户结余"].Value.ToString().Trim();
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户名称"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户备注"].Value.ToString().Trim();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["开户名"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["开户行"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户"].Value.ToString().Trim();

                comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["账户备注"].Value.ToString().Trim();
            }

        }
    }
}
