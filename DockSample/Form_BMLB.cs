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
    public partial class Form_BMLB : Office2007Form
    {
        string lb;
        DataTable dt;
        SqlConnection con;
        public Form_BMLB(string temp)
        {
            InitializeComponent();
            lb = temp;
        }

        private void Form_BMLB_Load(object sender, EventArgs e)
        {
            this.Text = lb + "类别设置";
            con = new SqlConnection(MainForm.connetstring);
            Queue_data();
        }
        private void Queue_data()
        {
            advTree1.Nodes.Clear();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_基础信息表 where 类别='"+lb+"'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DevComponents.AdvTree.Node tn = new DevComponents.AdvTree.Node();
                    tn.Text = dt.Rows[i]["名称"].ToString();
                    tn.ImageIndex = 0;
                    advTree1.Nodes.Add(tn);
                }

            }
            catch
            {
            }
            con.Close();
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
                    string str = "insert into J_基础信息表  (名称,类别)VALUES('" + frm.str_temp + "','" + lb + "')";
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
                        string str = "update J_基础信息表 set 名称='" + frm.str_temp + "' where 名称='" + advTree1.SelectedNode.Text + "' and 类别='" + lb + "'";
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
                    string str = "delete from  J_基础信息表 where 名称='" + advTree1.SelectedNode.Text + "' and 类别='" + lb + "'";
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

    }
}
