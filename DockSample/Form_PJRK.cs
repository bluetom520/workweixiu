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
    public partial class Form_PJRK : Office2007Form
    {
        SqlConnection con;
        DataTable dt2;
        public Form_PJRK()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_PJRK_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_配件入库 ";
                //if (comboBoxEx1.Text != "" && comboBoxEx3.Text != "")
                //{
                //    str += "where 配件类别='" + comboBoxEx1.Text + "' and 配件子类='" + comboBoxEx3.Text + "'";
                //}
                //else  if (comboBoxEx1.Text != "" || comboBoxEx2.Text != "")
                //{
                //    str += "where ";
                //    if (comboBoxEx1.Text != "")
                //        str += " where 配件类别='" + comboBoxEx1.Text + "'";
                //    else
                //    {
                //        str += " where 配件子类='" + comboBoxEx3.Text + "'";
                //    }
                //}
                if (comboBoxEx1.Text != "" )
                {
                    str += "where 配件类别='" + comboBoxEx1.Text + "'";
                }
                if (comboBoxEx3.Text != "")
                {
                    str += "and 配件子类='" + comboBoxEx3.Text + "'";
                }
                //if (comboBoxEx2.Text != "")
                //{
                //    str += "and 存储位置='" + comboBoxEx2.Text + "'";
                //}
                str += " order by 入库编号 desc";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;

                this.dataGridViewX1.Columns["入库编号"].Width = 110;
                this.dataGridViewX1.Columns["配件类别"].Width = 70;
                this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 100;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                this.dataGridViewX1.Columns["入库日期"].Width = 110;
                this.dataGridViewX1.Columns["入库数量"].Width = 60;
                this.dataGridViewX1.Columns["计量单位"].Width = 40;
                this.dataGridViewX1.Columns["购买单价"].Width = 60;
                this.dataGridViewX1.Columns["销售单价"].Width = 60;
                this.dataGridViewX1.Columns["供货商家"].Width = 100;
                this.dataGridViewX1.Columns["入库人员"].Width = 40;
                this.dataGridViewX1.Columns["配件名称"].Visible = false;
            }
            catch
            {
            }
            con.Close();
        }

        //private void Data_initial()
        //{

        //    try
        //    {
        //        if (con.State == ConnectionState.Closed)
        //            con.Open();

        //        string str1 = "select 名称 from J_基础信息表 where 类别='配件'";

        //        SqlDataAdapter da = new SqlDataAdapter(str1, con);

        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    con.Close();

        //}

        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件分类 where PARENTID=1";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["name"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.Text == "" || comboBoxEx3.Text == ""||textBoxX3.Text==""||textBoxX8.Text==""||textBoxX2.Text=="")
            {
                MessageBox.Show("您有未填写的参数！");
                return;


            }
            else
            {
                add_data();
                Queue_data();
                //splitContainer2.Panel1Collapsed = false;
            }
        }
        private void add_data()
        {
            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();

                decimal[] str_temp = new decimal[2];
                if (textBoxX7.Text != "")
                    str_temp[0] = Convert.ToDecimal(textBoxX7.Text);
                else
                    str_temp[0] = 0;
                if (textBoxX8.Text != "")
                    str_temp[1] = Convert.ToDecimal(textBoxX8.Text);
                else
                    str_temp[1] = 0;


                //lb = comboBoxEx1.Text;

                string rkbh = "RK" + DateTime.Now.ToString("yyyyMMddhhmmss");
                string str = "insert into J_配件入库 values('" + rkbh + "','" + comboBoxEx1.Text + "','" + comboBoxEx3.Text + "'";
                str += ",'" + textBoxX2.Text + "','','" + comboBoxEx2.Text + "','" + comboBoxEx4.Text + "','" + DateTime.Now.ToString() + "','" + textBoxX3.Text + "','" + textBoxX1.Text + "'," + str_temp[1] + "," + str_temp[0] + ",'" + textBoxX5.Text + "','" + LoginXT.username + "','" + textBoxX10.Text + "')";

                SqlCommand sqlcom = new SqlCommand(str, con);
                sqlcom.ExecuteNonQuery();

                str = "select * from J_配件信息 where 配件编号='" + textBoxX2.Text + "'";
                SqlDataAdapter da= new SqlDataAdapter(str, con);
                SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    dt.Rows[0]["累计入库量"] = Convert.ToInt64(dt.Rows[0]["累计入库量"].ToString()) + Convert.ToInt32(textBoxX3.Text);
                    dt.Rows[0]["采购价"] = str_temp[1];
                    dt.Rows[0]["销售价"] = str_temp[0];
                    dt.Rows[0]["配件子类"] = comboBoxEx3.Text;
                    dt.Rows[0]["存储位置"] = textBoxX10.Text;
                    dt.Rows[0]["计量单位"] = textBoxX1.Text;
                }
                else
                {
                    DataRow newrow = dt.NewRow();
                    newrow["配件编号"] = textBoxX2.Text;
                    newrow["配件类别"] = comboBoxEx1.Text;
                    newrow["配件子类"] = comboBoxEx3.Text;
                    newrow["存储位置"] = textBoxX10.Text;
                    newrow["配件规格"] = comboBoxEx2.Text;
                    newrow["配件型号"] = comboBoxEx4.Text;
                    newrow["计量单位"] = textBoxX1.Text;
                    newrow["累计入库量"] = Convert.ToInt32(textBoxX3.Text);
                    newrow["累计领用量"] = 0;
                    newrow["采购价"] = str_temp[0];
                    newrow["销售价"] = str_temp[1];
                    newrow["补充说明"] ="";
                    //newrow["配件编号"] = textBoxX2.Text;
                    dt.Rows.Add(newrow);
                }
                da.Update(dt);
                                default_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();



        }
        private void default_data()
        {
            // comboBox1.Text = "";
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            //textBoxX4.Text = "";
            textBoxX5.Text = "";
            textBoxX6.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            //textBoxX9.Text = "";
            textBoxX10.Text = "";
            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
            comboBoxEx3.Text = "";
            comboBoxEx4.Text = "";
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                textBoxX1.Text = "";
                textBoxX2.Text = "";
                textBoxX3.Text = "";
                //textBoxX4.Text = "";
                textBoxX5.Text = "";
                textBoxX6.Text = "";
                textBoxX7.Text = "";
                textBoxX8.Text = "";
                textBoxX10.Text = "";
                comboBoxEx4.Text = "";
                comboBoxEx2.Text = "";
                comboBoxEx3.Text = "";
                comboBoxEx3.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件分类 where PARENTID=(select id from J_配件分类 where name='" + comboBoxEx1.Text + "')";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxEx3.Items.Add(dt.Rows[i]["name"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
            Queue_data();
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Queue_data();
            //textBoxX2.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["配件编号"].ToString();
            //textBoxX4.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["配件规格"].ToString();
            //textBoxX1.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["配件单位"].ToString();
            //textBoxX6.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["建议进货价"].ToString();
            //textBoxX7.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["建议销售价"].ToString();
            //textBoxX9.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["配件型号"].ToString();

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    change_data();
            //    Queue_data();
            //}
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
                    string str = "select * from J_配件入库 where  序号'" + str2 + "'";

                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    decimal[] str_temp = new decimal[2];
                    if (textBoxX6.Text != "")
                        str_temp[0] = Convert.ToDecimal(textBoxX6.Text);
                    else
                        str_temp[0] = 0;
                    if (textBoxX7.Text != "")
                        str_temp[1] = Convert.ToDecimal(textBoxX7.Text);
                    else
                        str_temp[1] = 0;
                    dt.Rows[0]["配件编号"] = textBoxX2.Text;
                    dt.Rows[0]["配件类别"] = comboBoxEx1.Text;
                    dt.Rows[0]["配件子类"] = comboBoxEx3.Text;
                    dt.Rows[0]["存储位置"] = textBoxX7.Text;
                    dt.Rows[0]["配件规格"] = comboBoxEx2.Text ;
                    dt.Rows[0]["配件单位"] = textBoxX1.Text;
                    dt.Rows[0]["建议进货价"] = str_temp[0];
                    dt.Rows[0]["建议销售价"] = str_temp[1];
                    dt.Rows[0]["配件型号"] = comboBoxEx4.Text;
                    dt.Rows[0]["补充说明"] = textBoxX8.Text;
                    dt.Rows[0]["供货商家"] = textBoxX5.Text;
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

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件类别"].Value.ToString().Trim();

                comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件子类"].Value.ToString().Trim();
                comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件规格"].Value.ToString().Trim();
                comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString().Trim();
                //textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件单位"].Value.ToString().Trim();
                //textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["入库数量"].Value.ToString().Trim();
                //textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["规格型号"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["供货商家"].Value.ToString().Trim();
                //textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["销售单价"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["购买单价"].Value.ToString().Trim();
            }
        }

        private void dataGridViewX1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxEx2.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select distinct 配件规格 from J_配件目录 where 配件子类='" + comboBoxEx3.Text + "'";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxEx2.Items.Add(dt.Rows[i]["配件规格"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
            Queue_data();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            default_data();
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt2.Rows.Count > 0)
            {
                textBoxX2.Text = dt2.Rows[comboBoxEx4.SelectedIndex]["配件编号"].ToString();
                //textBoxX4.Text = dt2.Rows[comboBoxEx2.SelectedIndex]["配件单位"].ToString();
                textBoxX1.Text = dt2.Rows[comboBoxEx4.SelectedIndex]["配件单位"].ToString();
                textBoxX6.Text = dt2.Rows[comboBoxEx4.SelectedIndex]["建议进货价"].ToString();
                textBoxX7.Text = dt2.Rows[comboBoxEx4.SelectedIndex]["建议销售价"].ToString();
                textBoxX10.Text = dt2.Rows[comboBoxEx4.SelectedIndex]["存储位置"].ToString();
            }
        }

        private void comboBoxEx2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                comboBoxEx4.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件目录 where 配件规格='" + comboBoxEx2.Text + "'";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                dt2 = new DataTable();
                da.Fill(dt2);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    comboBoxEx4.Items.Add(dt2.Rows[i]["配件型号"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
            Queue_data();
        }
    }
}
