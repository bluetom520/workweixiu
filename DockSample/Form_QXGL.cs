using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DockSample
{
    public partial class Form_QXGL : Office2007Form
    {
        SqlConnection con;
        DataTable dt;
        public Form_QXGL()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_QXGL_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Queue_data();
        }

        private void Queue_data()
        {
            advTree1.Nodes.Clear();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_权限管理表";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DevComponents.AdvTree.Node tn = new DevComponents.AdvTree.Node();
                    tn.Text = dt.Rows[i]["角色"].ToString();
                    tn.ImageIndex = 0;
                    advTree1.Nodes.Add(tn);
                }

            }
            catch
            {
            }
            con.Close();
        }

        private void advTree1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    if (i < groupPanel1.Controls.Count + 2)
                    {
                        foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel1.Controls)
                        {

                            if (ck.Text == dt.Columns[i].ColumnName)
                            {
                                if (dt.Rows[advTree1.SelectedIndex][i].ToString() == "1")
                                {
                                    ck.Checked = true;
                                }
                                else
                                {
                                    ck.Checked = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel2.Controls)
                        {
                            if (ck.Text == dt.Columns[i].ColumnName.Substring(2))
                            {
                                if (dt.Rows[advTree1.SelectedIndex][i].ToString() == "1")
                                {
                                    ck.Checked = true;
                                }
                                else
                                {
                                    ck.Checked = false;
                                }
                            }
                        }
                    }
                    //DevComponents.DotNetBar.Controls.CheckBoxX ck = (DevComponents.DotNetBar.Controls.CheckBoxX)groupPanel1.Controls[dt.Columns[i].ColumnName];

                }
            }
            catch
            {

            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Form_Text frm = new Form_Text();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "insert into J_权限管理表 (角色)VALUES('" + frm.str_temp + "')";
                    SqlCommand SQL = new SqlCommand(str, con);
                    SQL.ExecuteNonQuery();
                    SQL.Dispose();
                }
                catch
                {

                }
                con.Close();
            }
            Queue_data();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (advTree1.SelectedNodes.Count > 0)
            {
                try
                {
                    Form_Text frm = new Form_Text();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = "update J_权限管理表 set 角色='" + frm.str_temp + "' where 角色='" + advTree1.SelectedNode.Text + "'";
                        SqlCommand SQL = new SqlCommand(str, con);
                        SQL.ExecuteNonQuery();
                        SQL.Dispose();

                    }
                }
                catch (Exception ex)
                {

                }
                con.Close();
                Queue_data();
            }

            else
            {
                MessageBoxEx.Show("请选择要修改的项！");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (advTree1.SelectedNodes.Count > 0)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string str = "delete from  J_权限管理表  where 角色='" + advTree1.SelectedNode.Text + "'";
                    SqlCommand SQL = new SqlCommand(str, con);
                    SQL.ExecuteNonQuery();
                    SQL.Dispose();
                }
                catch
                {

                }
                con.Close();
                Queue_data();
            }
            else
            {
                MessageBoxEx.Show("请选择要删除的项！");
            }


        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            Update_data();
            Queue_data();
        }


        private void Update_data()
        {
            if (advTree1.SelectedNodes.Count > 0)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();


                    string str = "select * from J_权限管理表 where 角色='" + advTree1.SelectedNode.Text + "'";
                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel1.Controls)
                    {
                        if (ck.Checked)
                        {
                            dt.Rows[0][ck.Text] = 1;
                        }
                        else
                        {
                            dt.Rows[0][ck.Text] =0;
                        }
                    }
                    foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel2.Controls)
                    {
                        if (ck.Checked)
                        {
                            dt.Rows[0]["S_"+ck.Text] = 1;
                        }
                        else
                        {
                            dt.Rows[0]["S_"+ck.Text] = 0;
                        }
                    }
                    //if (listBox1.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < listBox1.Items.Count; i++)
                    //    {
                    //        dt.Rows[0][listBox1.Items[i].ToString()] = 0;
                    //    }
                    //}
                    //if (listBox2.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < listBox2.Items.Count; i++)
                    //    {
                    //        dt.Rows[0][listBox2.Items[i].ToString()] = 1;
                    //    }
                    //}


                    da.Update(dt);
                    MessageBoxEx.Show("保存成功！");
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
}