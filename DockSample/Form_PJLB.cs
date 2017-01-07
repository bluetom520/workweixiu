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
    public partial class Form_PJLB : Office2007Form
    {
        string lb;
        DataTable dt;
        SqlConnection con;
        public Form_PJLB()
        {
            InitializeComponent();

        }

        private void Form_BMLB_Load(object sender, EventArgs e)
        {

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
                string str = "select * from J_配件分类";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                dt = new DataTable();
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

        private DevComponents.AdvTree.Node GetNode(DevComponents.AdvTree.Node node)
        {
            if (node.Nodes.Count == 0)
                return new DevComponents.AdvTree.Node(node.Text);
            DevComponents.AdvTree.Node ns = new DevComponents.AdvTree.Node(node.Text);
            foreach (DevComponents.AdvTree.Node item in node.Nodes)
            {
                DevComponents.AdvTree.Node n = GetNode(item);

                ns.Nodes.Add(n);
            }
            return ns;
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                if (advTree1.SelectedNode.Level < 2)
                {
                    Form_Text frm = new Form_Text();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            string str = "insert into J_配件分类  (PARENTID,NAME)VALUES('" + advTree1.SelectedNode.Tag + "','" + frm.str_temp + "')";
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
            }
            catch
            {
            }
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
                        string str = "update J_配件分类  set NAME='" + frm.str_temp + "' where ID='" + advTree1.SelectedNode.Tag + "'";
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
                if (advTree1.SelectedNode.Nodes.Count > 0)
                {
                    MessageBox.Show("还有子项未删除！");
                }
                else
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string str = "delete from  J_配件分类 where ID='" + advTree1.SelectedNode.Tag + "'";
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
            }
            else
            {
                MessageBoxEx.Show("请选择要删除的项！");
            }
        }

    }
}
