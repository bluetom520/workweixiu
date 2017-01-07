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
    public partial class Form_GZJB : Office2007Form
    {
        SqlConnection con;
        public Form_GZJB()
        {
            InitializeComponent();
        }

        private void Form_GZJB_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            con = new SqlConnection(MainForm.connetstring);
            Queue_data();
        }

        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_工资级别表 order by 序号";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX1.Columns["级别"].Width = 70;
                this.dataGridViewX1.Columns["等级工资"].Width = 70;
                this.dataGridViewX1.Columns["津贴"].Width = 50;
                this.dataGridViewX1.Columns["全勤奖"].Width = 50;
                this.dataGridViewX1.Columns["积分奖"].Width = 70;
                this.dataGridViewX1.Columns["管理奖"].Width = 70;
                this.dataGridViewX1.Columns["其他奖励"].Width = 60;
                this.dataGridViewX1.Columns["标准工资"].Width = 70;
                this.dataGridViewX1.Columns["标准积分"].Width = 70;
            }
            catch
            {
            }
            con.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            add_data();
            Queue_data();
        }

        private void add_data()
        {
            try
            {
                if (textBoxX1.Text == "")
                    return;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string[] str_data = new string[2];
                decimal[] str_temp = new decimal[8];
                str_data[0] = textBoxX1.Text;
                if (textBoxX2.Text != "")
                    str_temp[0] = Convert.ToDecimal(textBoxX2.Text);
                else
                    str_temp[0] = 0;
                if (textBoxX3.Text != "")
                    str_temp[1] = Convert.ToDecimal(textBoxX3.Text);
                else
                    str_temp[1] = 0;
                if (textBoxX4.Text != "")
                    str_temp[2] = Convert.ToDecimal(textBoxX4.Text);
                else
                    str_temp[2] = 0;
                if (textBoxX5.Text != "")
                    str_temp[3] = Convert.ToDecimal(textBoxX5.Text);
                else
                    str_temp[3] = 0;
                if (textBoxX6.Text != "")
                    str_temp[4] = Convert.ToDecimal(textBoxX6.Text);
                else
                    str_temp[4] = 0;
                if (textBoxX7.Text != "")
                    str_temp[5] = Convert.ToDecimal(textBoxX7.Text);
                else
                    str_temp[5] = 0;
                if (textBoxX8.Text != "")
                    str_temp[7] = Convert.ToDecimal(textBoxX8.Text);
                else
                    str_temp[7] = 0;
                decimal num = 0;
                for(int i=0;i<6;i++)
                {
                    num += str_temp[i];
                }
                str_temp[6] = num;



                string str = "insert into J_工资级别表 values('" + str_data[0] + "'," + str_temp[0] + "";
                str += "," + str_temp[1] + "," + str_temp[2] + "," + str_temp[3] + "," + str_temp[4] + "," + str_temp[5] + "," + str_temp[6] + ",'" + str_temp[7] + "')";

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

        private void buttonX4_Click(object sender, EventArgs e)
        {
            default_data();


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
            textBoxX7.Text = "";
            textBoxX8.Text = "";
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["级别"].Value.ToString().Trim();
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["等级工资"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["津贴"].Value.ToString().Trim();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["全勤奖"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["积分奖"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["管理奖"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["其他奖励"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["标准积分"].Value.ToString().Trim();
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
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

                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_工资级别表 where  序号='"+str2+"'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    string[] str_data = new string[2];
                    decimal[] str_temp = new decimal[8];
                    str_data[0] = textBoxX1.Text;
                    if (textBoxX2.Text != "")
                        str_temp[0] = Convert.ToDecimal(textBoxX2.Text);
                    else
                        str_temp[0] = 0;
                    if (textBoxX3.Text != "")
                        str_temp[1] = Convert.ToDecimal(textBoxX3.Text);
                    else
                        str_temp[1] = 0;
                    if (textBoxX4.Text != "")
                        str_temp[2] = Convert.ToDecimal(textBoxX4.Text);
                    else
                        str_temp[2] = 0;
                    if (textBoxX5.Text != "")
                        str_temp[3] = Convert.ToDecimal(textBoxX5.Text);
                    else
                        str_temp[3] = 0;
                    if (textBoxX6.Text != "")
                        str_temp[4] = Convert.ToDecimal(textBoxX6.Text);
                    else
                        str_temp[4] = 0;
                    if (textBoxX7.Text != "")
                        str_temp[5] = Convert.ToDecimal(textBoxX7.Text);
                    else
                        str_temp[5] = 0;
                    if (textBoxX8.Text != "")
                        str_temp[7] = Convert.ToDecimal(textBoxX8.Text);
                    else
                        str_temp[7] = 0;
                    decimal num = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        num += str_temp[i];
                    }
                    str_temp[6] = num;
                    dt.Rows[0]["级别"] = str_data[0];
                    dt.Rows[0]["等级工资"] = str_temp[0];
                    dt.Rows[0]["津贴"] = str_temp[1];
                    dt.Rows[0]["全勤奖"] = str_temp[2];
                    dt.Rows[0]["积分奖"] = str_temp[3];
                    dt.Rows[0]["管理奖"] = str_temp[4];
                    dt.Rows[0]["其他奖励"] = str_temp[5];
                    dt.Rows[0]["标准积分"] = str_temp[7]; ;
                    dt.Rows[0]["标准工资"] = str_temp[6];

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

        private void buttonX2_Click(object sender, EventArgs e)
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
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string str = "delete from J_工资级别表 where 序号='" + str1 + "'";
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
    }
}
