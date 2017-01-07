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
    public partial class Form_PJML : Office2007Form
    {
        SqlConnection con;
        string lb="";
        public Form_PJML()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_PJML_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            Queue_data();
            //Queue_data2("");
        }
        //private void Queue_data()
        //{
        //    advTree1.Nodes.Clear();
        //    try
        //    {
        //        if (con.State == ConnectionState.Closed)
        //            con.Open();
        //        string str = "select * from J_基础信息表 where 类别='配件'";
        //        SqlDataAdapter da = new SqlDataAdapter(str, con);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            DevComponents.AdvTree.Node tn = new DevComponents.AdvTree.Node();
        //            tn.Text = dt.Rows[i]["名称"].ToString();
        //            tn.ImageIndex = 0;
        //            advTree1.Nodes.Add(tn);
        //            comboBoxEx1.Items.Add(dt.Rows[i]["名称"].ToString());
        //        }

        //    }
        //    catch
        //    {
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

        private void Queue_data()
        {
            advTree1.Nodes.Clear();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_配件分类";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        DevComponents.AdvTree.Node tn = new DevComponents.AdvTree.Node();
                        tn.Text = dt.Rows[i]["name"].ToString();
                        tn.Tag = dt.Rows[i]["ID"].ToString();
                        tn.ImageIndex = 0;
                        advTree1.Nodes.Add(tn);
                    }
                    else
                    {
                        DevComponents.AdvTree.Node tn = new DevComponents.AdvTree.Node();
                        tn.Text = dt.Rows[i]["name"].ToString();
                        tn.Tag = dt.Rows[i]["ID"].ToString();
                        tn.ImageIndex = 0;
                        string temp = dt.Rows[i]["PARENTID"].ToString();
                        int t = advTree1.Nodes.Count;
                        //if (temp == "1")
                        //{
                        //    advTree1.Nodes[0].Nodes.Add(tn);

                        //}
                        //else
                        //{
                        //    foreach (DevComponents.AdvTree.Node T_PARENT in advTree1.Nodes[0].Nodes)
                        //    {


                        //            if (T_PARENT.Tag.ToString() == temp)
                        //                T_PARENT.Nodes.Add(tn);


                        //    }
                        //}
                        foreach (DevComponents.AdvTree.Node T_PARENT in advTree1.Nodes)
                        {
                            if (T_PARENT.Tag.ToString() == temp)
                                T_PARENT.Nodes.Add(tn);
                            else
                            {
                                if (T_PARENT.Nodes.Count > 0)
                                {
                                    foreach (DevComponents.AdvTree.Node T_2 in advTree1.Nodes[0].Nodes)
                                    {
                                        if (T_2.Tag.ToString() == temp)
                                            T_2.Nodes.Add(tn);
                                    }
                                }
                            }
                        }
                    }

                }
                advTree1.ExpandAll();


            }
            catch
            {
            }
            con.Close();
        }
        private void Queue_data2(string temp)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_配件目录 ";
                //if (temp != "")
                //{
                //    str += " where 配件子类='" + temp + "' ";
                //}
                if (advTree1.SelectedNode.Level == 1)
                {
                    str += "where 配件类别='" + temp + "'";
                }
                if (advTree1.SelectedNode.Level == 2)
                {
                    str += "where  配件子类='" + temp + "'";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;

                this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["配件类别"].Width = 70;
                this.dataGridViewX1.Columns["配件子类"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 120;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件单位"].Width = 30;
                this.dataGridViewX1.Columns["建议进货价"].Width = 70;
                this.dataGridViewX1.Columns["建议销售价"].Width = 70;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                this.dataGridViewX1.Columns["补充说明"].Width = 100;
                this.dataGridViewX1.Columns["配件名称"].Visible = false;
            }
            catch
            {
            }
            con.Close();
        }

        private void advTree1_Click(object sender, EventArgs e)
        {
            if(advTree1.SelectedNode.Level>=0)
            Queue_data2(advTree1.SelectedNode.Text);
        }

        private void labelX5_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //splitContainer2.Panel1Collapsed = false;
            if (MessageBox.Show("确认修改吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                change_data();
                Queue_data2(lb);
            }
        }
        private void change_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                lb = comboBoxEx1.Text;
                
                string str2 = this.dataGridViewX1.SelectedRows[0].Cells["流水号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "select * from J_配件目录 where  流水号='" + str2 + "'";

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
                    dt.Rows[0]["存储位置"] = textBoxX3.Text;
                    dt.Rows[0]["配件规格"] = textBoxX4.Text;
                    dt.Rows[0]["配件单位"] = comboBoxEx2.Text;
                    dt.Rows[0]["建议进货价"] = str_temp[0];
                    dt.Rows[0]["建议销售价"] = str_temp[1];
                    dt.Rows[0]["配件型号"] = textBoxX5.Text;
                    dt.Rows[0]["补充说明"] = textBoxX8.Text;

                    da.Update(dt);


                    string str_kc = "select * from J_配件信息 where  配件编号='" + textBoxX2.Text + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(str_kc, con);
                    SqlCommandBuilder t_b2 = new SqlCommandBuilder(da2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    //dt2.Rows[0]["配件编号"] = textBoxX2.Text;
                    dt2.Rows[0]["配件类别"] = comboBoxEx1.Text;
                    dt2.Rows[0]["配件子类"] = comboBoxEx3.Text;
                    dt2.Rows[0]["存储位置"] = textBoxX3.Text;
                    dt2.Rows[0]["配件规格"] = textBoxX4.Text;
                    dt2.Rows[0]["计量单位"] = comboBoxEx2.Text;
                    //dt2.Rows[0]["建议进货价"] = str_temp[0];
                    //dt2.Rows[0]["建议销售价"] = str_temp[1];
                    dt2.Rows[0]["配件型号"] = textBoxX5.Text;
                    dt2.Rows[0]["补充说明"] = textBoxX8.Text;

                    da2.Update(dt2);
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
            //textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["报价编号"].Value.ToString().Trim();
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString().Trim();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString().Trim();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件规格"].Value.ToString().Trim();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString().Trim();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["建议进货价"].Value.ToString().Trim();
                textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells["建议销售价"].Value.ToString().Trim();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["补充说明"].Value.ToString().Trim();
                comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件类别"].Value.ToString().Trim();
                comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件单位"].Value.ToString().Trim();
                comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件子类"].Value.ToString().Trim();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (comboBoxEx1.Text == "" || textBoxX3.Text == "")
            {
                return;

            }
            else
            {
                add_data();
                Queue_data2(lb);
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
                if (textBoxX6.Text != "")
                    str_temp[0] = Convert.ToDecimal(textBoxX6.Text);
                else
                    str_temp[0] = 0;
                if (textBoxX7.Text != "")
                    str_temp[1] = Convert.ToDecimal(textBoxX7.Text);
                else
                    str_temp[1] = 0;


                lb=comboBoxEx1.Text;


                string str = "insert into J_配件目录 values('" + textBoxX2.Text + "',''";
                str += ",'" + textBoxX4.Text + "','" + comboBoxEx1.Text + "','" + comboBoxEx3.Text + "','" + comboBoxEx2.Text + "'," + str_temp[0] + "," + str_temp[1] + ",'" + textBoxX5.Text + "','" + textBoxX8.Text + "','" + textBoxX3.Text + "')";

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
            //textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX5.Text = "";
            textBoxX6.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
            comboBoxEx3.Text = "";
        }
        private void comboBoxEx1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select top 1 * from J_配件目录 where 配件类别='" + comboBoxEx1.Items[comboBoxEx1.SelectedIndex].ToString() + "' ORDER BY 流水号 desc";
                
                SqlDataAdapter da = new SqlDataAdapter(str1, con);
                DataTable dt = new DataTable();

                da.Fill(dt);
                int num = 0;
                if(dt.Rows.Count>0)
                {
                    string temp = dt.Rows[0]["配件编号"].ToString();
                    int ll = temp.Length;
                    if (ll == 7)
                    {
                        temp = temp.Substring(temp.Length - 3, 3);
                    }
                    else
                    {
                        temp = temp.Substring(temp.Length - 4, 4);
                    }
                    num = Convert.ToInt32(temp)+1;

                }
                else
                {
                    num = dt.Rows.Count + 1;
                }

          
                textBoxX2.Text = "PJ" + (comboBoxEx1.SelectedIndex + 1).ToString().PadLeft(3, '0') + num.ToString().PadLeft(4, '0');
                //textBoxX1.Text = "";
                //textBoxX2.Text = "";
                textBoxX3.Text = "";
                textBoxX4.Text = "";
                textBoxX5.Text = "";
                textBoxX6.Text = "";
                textBoxX7.Text = "";
                textBoxX8.Text = "";
                comboBoxEx2.Text = "";
                comboBoxEx3.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                Queue_data2(lb);
            }
        }
        private void delete_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    lb = comboBoxEx1.Text;
                    string str = "delete from J_配件目录 where 配件编号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();

                    string str2 = "delete from J_配件信息 where 配件编号='" + str1 + "'";
                    SqlCommand sqlcom2 = new SqlCommand(str2, con);
                    sqlcom2.ExecuteNonQuery();
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

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Queue_data3();
        }

        private void Queue_data3()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "Select name from syscolumns Where ID=OBJECT_ID('J_配件目录')";
                SqlDataAdapter da2 = new SqlDataAdapter(str, con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                str = "select * from J_配件目录 ";

                if (textBoxX5.Text != "")
                {
                    str += " where ";
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        string temp = dt2.Rows[i][0].ToString();
                        if (temp.IndexOf("价") >= 0||temp == "流水号")
                        {
                            str += "isnull(convert(varchar,"+temp+"),'')+";
                        }
                        else
                        {
                          
                            str += "isnull(" +temp+ ",'')+";
                        }
                    }
                    str = str.Substring(0, str.Length - 1);
                    str += " like '%" + textBoxX5.Text + "%'";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;

                this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["配件类别"].Width = 70;
                this.dataGridViewX1.Columns["配件子类"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 120;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件单位"].Width = 30;
                this.dataGridViewX1.Columns["建议进货价"].Width = 70;
                this.dataGridViewX1.Columns["建议销售价"].Width = 70;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                this.dataGridViewX1.Columns["补充说明"].Width = 100;
            }
            catch
            {
            }
            con.Close();
    }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxEx3.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select * from J_配件分类 where PARENTID=(select id from J_配件分类 where name='" + comboBoxEx1.Text + "')";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt2 = new DataTable();
                da.Fill(dt2);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    comboBoxEx3.Items.Add(dt2.Rows[i]["name"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            default_data();
        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
