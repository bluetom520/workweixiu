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
    public partial class Form_ZJXM : Office2007Form
    {
        SqlConnection con;
        string ID = "";
        string wxbh;
        public Form_ZJXM(string str)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str;
        }

        private void Form_ZJXM_Load(object sender, EventArgs e)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_修品质检表 where 维修编号='"+wxbh+"'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ID = dt.Rows[0][0].ToString();
                    for (int i = 2; i < dt.Columns.Count; i++)
                    {
                        if (i < groupPanel1.Controls.Count + 2)
                        {
                            foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel1.Controls)
                            {

                                if (ck.Text == dt.Columns[i].ColumnName)
                                {
                                    if (dt.Rows[0][i].ToString() == "1")
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
                    }

                }
            }
            catch
            {
            }
            con.Close();
        
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str = "select * from J_修品质检表  where 维修编号='" + wxbh + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                SqlCommandBuilder t_b = new SqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel1.Controls)
                    {
                        if (ck.Checked)
                        {
                            dt.Rows[0][ck.Text] = 1;
                        }
                        else
                        {
                            dt.Rows[0][ck.Text] = 0;
                        }
                    }

                }
                else
                {
                    DataRow newrow = dt.NewRow();
                    newrow["维修编号"] = wxbh;
                    foreach (DevComponents.DotNetBar.Controls.CheckBoxX ck in groupPanel1.Controls)
                    {

                        if (ck.Checked)
                        {
                            newrow[ck.Text] = 1;
                        }
                        else
                        {
                            newrow[ck.Text] = 0;
                        }
                    }
                    dt.Rows.Add(newrow);
                    
                }
                da.Update(dt);
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


 
                //MessageBoxEx.Show("保存成功！");
                //default_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
            if (checkBoxX1.Checked && checkBoxX2.Checked && checkBoxX3.Checked && checkBoxX4.Checked)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
