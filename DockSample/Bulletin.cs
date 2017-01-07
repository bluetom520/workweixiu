using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DevComponents.DotNetBar;

namespace DockSample
{
    public partial class Bulletin : Office2007Form
    {
            
        int port = 11118;
        SqlConnection con;
        DataTable dt_list;
        private UdpClient uc;
        private IPEndPoint iep;
        //private Thread th;
        bool flag=false;
        public static string str_data="";
        public Bulletin()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            uc = new UdpClient();
        }

        private void Bulletin_Load(object sender, EventArgs e)
        {
            listbox_load();
            comboBox1.Text = "全部";
        }
        private void listbox_load()
        {
            listBox1.Items.Clear();
            dt_list = new DataTable();
            try
            {
                con.Open();
                string str = "select * from J_公告表 where  无效标志!=1";

                SqlDataAdapter da = new SqlDataAdapter(str, con);

                da.Fill(dt_list);
                if (dt_list.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_list.Rows.Count; i++)
                    {
                        listBox1.Items.Add(dt_list.Rows[i]["提醒名称"].ToString());
                    }

                    //textBox2.Text = dt_list.Rows[0]["提醒名称"].ToString();
                    //textBox1.Text = dt_list.Rows[0]["提醒内容"].ToString();
                    //comboBox1.Text = dt_list.Rows[0]["目的单位"].ToString();
                    //dateTimePicker1.Value = Convert.ToDateTime(dt_list.Rows[0]["开始日期"].ToString());
                    //dateTimePicker2.Value = Convert.ToDateTime(dt_list.Rows[0]["结束日期"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void listbox_add()
        {
            try
            {
                con.Open();
                if (textBox2.Text == ""||comboBox1.Text=="")
                {
                    MessageBox.Show("公告参数不能为空！");
                    return;
                }
                else
                {
                    string str = "select * from J_公告表 where 目的单位='" + comboBox1.Text + "' and 提醒名称='" + textBox2.Text + "'";
                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_build = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("公告名称已存在，请点修改！");
                    }
                    else
                    {
                        DataRow newrow = dt.NewRow();
                        newrow["提醒名称"] = textBox2.Text;
                        newrow["提醒内容"] = textBox1.Text;
                        newrow["目的单位"] = comboBox1.Text;
                        newrow["发布单位"] = LoginXT.user_danwei;
                        newrow["开始日期"] = dateTimePicker1.Value;
                        newrow["结束日期"] = dateTimePicker2.Value;
                        newrow["无效标志"] = 0;
                        dt.Rows.Add(newrow);
                        da.Update(dt);
                        str_data = textBox2.Text + "|" + textBox1.Text;
                        flag = true;
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void listbox_change()
        {
            try
            {
                con.Open();
                if (textBox2.Text == "")
                {
                    MessageBox.Show("公告名称不能为空！");
                    return;
                }
                else
                {
                    string str = "select * from J_公告表 where  提醒名称='" + textBox2.Text + "'";
                    SqlDataAdapter da = new SqlDataAdapter(str, con);
                    SqlCommandBuilder t_build = new SqlCommandBuilder(da);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("公告名称不存在，请点增加！");
                    }
                    else
                    {
                        dt.Rows[0]["提醒名称"] = textBox2.Text;
                        dt.Rows[0]["提醒内容"] = textBox1.Text;
                        dt.Rows[0]["目的单位"] = comboBox1.Text;

                        dt.Rows[0]["开始日期"] = dateTimePicker1.Value;
                        dt.Rows[0]["结束日期"] = dateTimePicker2.Value;
                        da.Update(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void listbox_delete()
        {
            try
            {

                string str = "select * from J_公告表 where 目的单位='" + comboBox1.Text + "' and 提醒名称='" + listBox1.SelectedItem.ToString() + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                SqlCommandBuilder t_build = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //dt.Rows.RemoveAt(0);
                    dt.Rows[0].Delete();
                    da.Update(dt);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listbox_add();
            listbox_load();
            if (flag)
            {
                iep = new IPEndPoint(IPAddress.Parse("26.2.255.255"), port);
                //string str = "tz";
                byte[] b = (Encoding.Default.GetBytes(str_data));
                uc.Send(b, b.Length, iep);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定修改？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                listbox_change();
                listbox_load();
            }
            else
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定失效？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (listBox1.Items.Count > 0)
                {
                    try
                    {
                        con.Open();
                        string str = "update J_公告表 set 无效日期='" + DateTime.Now.ToString() + "',无效标志=1 where 提醒名称='" + listBox1.SelectedItem.ToString() + "'";


                        SqlCommand sqlcom = new SqlCommand(str, con);
                        sqlcom.ExecuteNonQuery();                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                    listbox_load();
                }
            }
            else
            {
                return;
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0 && listBox1.SelectedIndex >= 0)
            {

                textBox2.Text = dt_list.Rows[listBox1.SelectedIndex]["提醒名称"].ToString();
                textBox1.Text = dt_list.Rows[listBox1.SelectedIndex]["提醒内容"].ToString();
                comboBox1.Text = dt_list.Rows[listBox1.SelectedIndex]["目的单位"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dt_list.Rows[listBox1.SelectedIndex]["开始日期"].ToString());
                dateTimePicker2.Value = Convert.ToDateTime(dt_list.Rows[listBox1.SelectedIndex]["结束日期"].ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }
    }
}
