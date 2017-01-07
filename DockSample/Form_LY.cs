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
    public partial class Form_LY : Office2007Form
    {
        SqlConnection con;
        string wxbh;
        public Form_LY(string str)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str;
        }

        private void Form_LY_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            Queue_data2();
        }

        private void textBoxX2_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void Data_initial()
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

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (textBoxX7.Text == "")
            {
                MessageBox.Show("请输入配件数量！");
                return;
            }
            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();




                //lb = comboBoxEx1.Text;


                string str = "insert into J_配件领用 values('" + wxbh + "','" + textBoxX2.Text + "'";
                str += ",'" + textBoxX1.Text + "','" + textBoxX8.Text + "','','" + textBoxX4.Text + "','" + textBoxX10.Text + "','" + textBoxX5.Text + "','" + textBoxX7.Text + "','" + textBoxX6.Text + "','" + LoginXT.username + "','" + DateTime.Now.ToString() + "','" + textBoxX3.Text + "')";

                SqlCommand sqlcom = new SqlCommand(str, con);
                sqlcom.ExecuteNonQuery();
                sqlcom.Dispose();
                str = "update J_配件信息 set 累计领用量=累计领用量+" + Convert.ToInt32(textBoxX7.Text) + " where 配件编号='" + textBoxX2.Text + "'";
                SqlCommand SQL3 = new SqlCommand(str, con);
                SQL3.ExecuteNonQuery();
                SQL3.Dispose();
            }
            catch
            {
            }
            con.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //Form_PJKC frm = new Form_PJKC();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    textBoxX1.Text = Form_PJKC.row.Cells["配件类别"].Value.ToString();
            //    textBoxX2.Text = Form_PJKC.row.Cells["配件编号"].Value.ToString();
            //    textBoxX3.Text = Form_PJKC.row.Cells["存储位置"].Value.ToString();
            //    textBoxX4.Text = Form_PJKC.row.Cells["规格型号"].Value.ToString();
            //    textBoxX5.Text = Form_PJKC.row.Cells["计量单位"].Value.ToString();
            //    textBoxX6.Text = Form_PJKC.row.Cells["销售价"].Value.ToString();
            //    textBoxX8.Text = Form_PJKC.row.Cells["配件子类"].Value.ToString();
            //}
        }

        private void advTree1_Click(object sender, EventArgs e)
        {
            if (advTree1.SelectedNode.Level>=0)
                Queue_data(advTree1.SelectedNode.Text);
        }

        private void Queue_data(string str_zl)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select 配件编号,配件类别,配件子类,存储位置,配件规格,配件型号,累计入库量,累计领用量,累计入库量-累计领用量 as 库存量,采购价,销售价,计量单位,补充说明 from J_配件信息 ";

                if (advTree1.SelectedNode.Level ==1)
                {
                    str += "where 配件类别='" + str_zl + "'";
                }
                if (advTree1.SelectedNode.Level == 2)
                {
                    str += "where  配件子类='" + str_zl + "'";
                }

                str += " order by 配件编号";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                this.dataGridViewX1.Columns["配件编号"].Width = 60;
                this.dataGridViewX1.Columns["配件类别"].Width = 70;
                this.dataGridViewX1.Columns["配件子类"].Width = 70;
                this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 100;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                this.dataGridViewX1.Columns["累计入库量"].Width = 60;
                this.dataGridViewX1.Columns["累计领用量"].Width = 60;
                this.dataGridViewX1.Columns["库存量"].Width = 60;
                this.dataGridViewX1.Columns["采购价"].Width = 60;
                this.dataGridViewX1.Columns["销售价"].Width = 60;
                this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;
                //this.dataGridViewX1.Columns["配件名称"].Visible = false;
            }
            catch
            {
            }
            con.Close();
        }

        private void Queue_data2()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str = "Select name from syscolumns Where ID=OBJECT_ID('J_配件信息')";
                SqlDataAdapter da2 = new SqlDataAdapter(str, con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                str = "select 配件编号,配件类别,配件子类,存储位置,配件规格,配件型号,累计入库量,累计领用量,累计入库量-累计领用量 as 库存量,采购价,销售价,计量单位,补充说明 from J_配件信息 ";

                if (comboBoxEx1.Text != "模糊查询")
                {
                    if (textBoxX9.Text != "")
                    {
                        str += "where " + comboBoxEx1.Text + " like '%" + textBoxX9.Text + "%'";
                    }
                    //else
                    //{
                    //    MessageBox.Show("请输入查询内容！");
                    //}
                }
                else
                {
                    if (textBoxX9.Text != "")
                    {
                        str += " where ";
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            string temp = dt2.Rows[i][0].ToString();
                            if (temp.IndexOf("价") >= 0 || temp.IndexOf("累计") >= 0)
                            {
                                str += "isnull(convert(varchar," + temp + "),'')+";
                            }
                            else
                            {

                                str += "isnull(" + temp + ",'')+";
                            }
                        }
                        str = str.Substring(0, str.Length - 1);
                        str += " like '%" + textBoxX9.Text + "%'";
                    }
                }

                str += " order by 配件编号";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                this.dataGridViewX1.Columns["配件编号"].Width = 60;
                this.dataGridViewX1.Columns["配件类别"].Width = 70;
                this.dataGridViewX1.Columns["配件子类"].Width = 70;
                this.dataGridViewX1.Columns["配件编号"].Width = 70;
                this.dataGridViewX1.Columns["存储位置"].Width = 100;
                this.dataGridViewX1.Columns["配件规格"].Width = 100;
                this.dataGridViewX1.Columns["配件型号"].Width = 100;
                this.dataGridViewX1.Columns["累计入库量"].Width = 60;
                this.dataGridViewX1.Columns["累计领用量"].Width = 60;
                this.dataGridViewX1.Columns["库存量"].Width = 60;
                this.dataGridViewX1.Columns["采购价"].Width = 60;
                this.dataGridViewX1.Columns["销售价"].Width = 60;
                this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;
                //this.dataGridViewX1.Columns["配件名称"].Visible = false;
            }
            catch
            {
            }
            con.Close();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件类别"].Value.ToString();
                textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件编号"].Value.ToString();
                textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells["存储位置"].Value.ToString();
                textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件规格"].Value.ToString();
                textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells["计量单位"].Value.ToString();
                textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells["销售价"].Value.ToString();
                textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件子类"].Value.ToString();
                textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells["配件型号"].Value.ToString();
            }
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            Queue_data2();
        }

    }
}
