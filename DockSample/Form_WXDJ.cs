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
    public partial class Form_WXDJ :  Office2007Form
    {
        SqlConnection con;
        DataTable dt2;
        string khbh="";
        string wxbh="";
        public Form_WXDJ()
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_WXDJ_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();
            create_wxbh();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void create_wxbh()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string temp="WX-" + DateTime.Now.ToString("yyMMdd-");
                string str = "select count(*) from J_维修处理表 where 接修日期>'" + DateTime.Now.ToString("yyyy-MM-dd") +"' and 维修编号 like '"+temp+"%'";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();

                da.Fill(dt);
                wxbh = "WX-" + DateTime.Now.ToString("yyMMdd-") + (Convert.ToInt32(dt.Rows[0][0].ToString())+ 1).ToString().PadLeft(3,'0');
                textBoxX1.Text = wxbh;

            }
            catch
            {

            }
        }
        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认登记吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //change_data();
                //Queue_data();
                if (comboBoxEx8.Text == "" || comboBoxEx9.Text == "" || comboBoxEx9.Text == "全部" || comboBoxEx1.Text == "" || comboBoxEx2.Text == "" || comboBoxEx3.Text == "" || comboBoxEx7.Text == ""||textBoxX10.Text == "" || textBoxX9.Text == "" || textBoxX11.Text == "" || textBoxX2.Text == "" || textBoxX3.Text == "" ||  textBoxX5.Text == "" || textBoxX18.Text == "")
                {

                        MessageBox.Show("您有带*的参数未填写！");
                        return;
                    
                }
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    if (comboBoxEx3.Text != "")
                    {
                        if (!comboBoxEx3.Items.Contains(comboBoxEx3.Text.Trim()))
                        {
                            string str = "insert into J_商品信息表  (名称,类别)VALUES('" + comboBoxEx3.Text + "','品牌')";
                            SqlCommand SQL = new SqlCommand(str, con);
                            SQL.ExecuteNonQuery();
                            SQL.Dispose();
                        }
                    }
                    //if (comboBoxEx4.Text != "")
                    //{
                    //    if (!comboBoxEx4.Items.Contains(comboBoxEx4.Text))
                    //    {
                    //        string str = "insert into J_商品信息表  (名称,类别)VALUES('" + comboBoxEx4.Text + "','商家')";

                    //        SqlCommand SQL = new SqlCommand(str, con);
                    //        SQL.ExecuteNonQuery();
                    //        SQL.Dispose();
                    //    }
                    //}
                    //if (comboBoxEx6.Text != "")
                    //{
                    //    if (!comboBoxEx6.Items.Contains(comboBoxEx6.Text))
                    //    {
                    //        string str = "insert into J_商品信息表  (名称,类别)VALUES('" + comboBoxEx6.Text + "','外观')";
                    //        SqlCommand SQL = new SqlCommand(str, con);
                    //        SQL.ExecuteNonQuery();
                    //        SQL.Dispose();
                    //    }
                    //}
                    //if (comboBoxEx8.Text != "")
                    //{
                        //if (!comboBoxEx8.Items.Contains(comboBoxEx8.Text))
                        //{

                        //    khbh = "KH" + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("HHmmss");
                        //    string str = "insert into J_客户信息 VALUES('" + khbh + "','" + comboBoxEx9.Text + "','" + comboBoxEx8.Text + "','" + textBoxX10.Text + "','" + textBoxX11.Text + "','" + textBoxX12.Text + "','" + textBoxX13.Text + "','" + textBoxX9.Text + "','','','')";
                        //    SqlCommand SQL = new SqlCommand(str, con);
                        //    SQL.ExecuteNonQuery();
                        //    SQL.Dispose();
                        //}
                    //}
                    string[] str_data = new string[20];
                    str_data[0] = textBoxX1.Text;
                    str_data[1] = dateTimePicker1.Value.ToString(); ;
                    str_data[2] = comboBoxEx1.Text;
                    str_data[3] = comboBoxEx2.Text;
                    str_data[4] = textBoxX2.Text;
                    str_data[5] = comboBoxEx3.Text;
                    str_data[6] = textBoxX3.Text;
                    str_data[7] = textBoxX4.Text;
                    str_data[8] = "";
                    str_data[9] = textBoxX5.Text;
                    str_data[10] = "";
                    str_data[11] = textBoxX18.Text;
                    str_data[12] = comboBoxEx7.Text;
                    str_data[13] = khbh;
                    str_data[14] =LoginXT.username;
                    int num = 30;
                    if (textBoxX7.Text != "")
                    {
                        num = Convert.ToInt32(textBoxX7.Text);
                    }

                      
                    str_data[15] = DateTime.Now.AddDays(+num).ToString();
                    str_data[16] = "1";
                    str_data[17] = textBoxX14.Text;
                    str_data[18] = textBoxX15.Text;
                    str_data[19] = textBoxX17.Text;
                    string str1 = "insert into J_维修处理表 (维修编号,接修日期,修品大类,修品小类,修品型号,修品品牌,修品SN1,规格参数,随机附件,故障描述,保修情况,外观,优先级,客户编号,业务员,预约日期,修品数量,修品SN2,修品PN号,备注)VALUES(";
                    for (int i = 0; i < str_data.Length; i++)
                    {
                        str1 += "'"+str_data[i]+"',";
                    }
                    str1 = str1.Substring(0,str1.Length-1);
                    str1 += ")";
                    SqlCommand SQL2 = new SqlCommand(str1, con);
                    SQL2.ExecuteNonQuery();
                    SQL2.Dispose();


                    default_data();
                    Data_initial2();
                    create_wxbh();
                }
                catch
                {

                }
                con.Close();
            }
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 名称 from J_基础信息表 where 类别='维修'";

                str1 += ";select * from J_商品信息表 where 类别='品牌'";

                //str1 += ";select * from J_商品信息表 where 类别='外观'";
                str1 += ";select * from J_基础信息表 where 类别='客户'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);
                comboBoxEx1.Items.Clear();
                comboBoxEx3.Items.Clear();
                comboBoxEx9.Items.Clear();
                comboBoxEx9.Items.Add("全部");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
                }
                dt2 = ds.Tables[1];

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    
                    comboBoxEx3.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                }
                //for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                //{
                //    comboBoxEx4.Items.Add(ds.Tables[3].Rows[i]["名称"].ToString());
                //}
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    comboBoxEx9.Items.Add(ds.Tables[2].Rows[i]["名称"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }
        private void Data_initial2()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 名称 from J_基础信息表 where 类别='维修'";

                str1 += ";select * from J_商品信息表 where 类别='品牌'";

                //str1 += ";select * from J_商品信息表 where 类别='外观'";
                //str1 += ";select * from J_基础信息表 where 类别='客户'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);
                comboBoxEx1.Items.Clear();
                comboBoxEx3.Items.Clear();
                //comboBoxEx9.Items.Clear();
               // comboBoxEx9.Items.Add("全部");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
                }
                //dt2 = ds.Tables[1];

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {

                    comboBoxEx3.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                }
                //for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                //{
                //    comboBoxEx4.Items.Add(ds.Tables[3].Rows[i]["名称"].ToString());
                //}
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{
                //    comboBoxEx9.Items.Add(ds.Tables[2].Rows[i]["名称"].ToString());
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }
        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxEx2.Items.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 修品小类 from J_维修报价 where 修品大类='"+comboBoxEx1.Text+"'";

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx2.Items.Add(ds.Tables[0].Rows[i]["修品小类"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void comboBoxEx8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt2.Rows.Count > 0)
            {
                textBoxX9.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["通信地址"].ToString();
                textBoxX10.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["联系人"].ToString();
                textBoxX11.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["联系电话"].ToString();
                textBoxX6.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["联系人2"].ToString();
                textBoxX12.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["联系电话2"].ToString();
                textBoxX13.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["邮政编码"].ToString();
                khbh = dt2.Rows[comboBoxEx8.SelectedIndex]["客户编号"].ToString();
                comboBoxEx9.Text = dt2.Rows[comboBoxEx8.SelectedIndex]["客户类别"].ToString();
                //khbh=
            }
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
            //textBoxX8.Text = "";
            textBoxX9.Text = "";
            textBoxX10.Text = "";
            textBoxX11.Text = "";
            textBoxX12.Text = "";
            textBoxX13.Text = "";
            textBoxX14.Text = "";
            textBoxX15.Text = "";
            textBoxX16.Text = "1";
            textBoxX17.Text = "";
            textBoxX18.Text = "";
            comboBoxEx1.Text = "";
            comboBoxEx2.Text = "";
            comboBoxEx3.Text = "";
            //comboBoxEx4.Text = "";
            //comboBoxEx5.Text = "";
            //comboBoxEx18.Text = "";
            comboBoxEx7.Text = "";
            comboBoxEx8.Text = "";
        }

        private void comboBoxEx9_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    string str1;
            //    comboBoxEx8.Items.Clear();
            //    if (comboBoxEx9.Text != "全部")
            //    {
            //        str1 = "select * from J_客户信息 where 客户类别='" + comboBoxEx9.Text + "'";
            //    }
            //    else
            //    {
            //        str1 = "select * from J_客户信息 order by 客户类别";
            //    }

            //    SqlDataAdapter da = new SqlDataAdapter(str1, con);

            //    dt2 = new DataTable();
            //    da.Fill(dt2);
            //    for (int i = 0; i < dt2.Rows.Count; i++)
            //    {

            //        comboBoxEx8.Items.Add(dt2.Rows[i]["客户名称"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //con.Close();

 
        }

        private void comboBoxEx9_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str1;
                comboBoxEx8.Items.Clear();
                if (comboBoxEx9.SelectedIndex!=0)
                {
                    str1 = "select * from J_客户信息 where 客户类别='" + comboBoxEx9.Items[comboBoxEx9.SelectedIndex].ToString()+ "'";
                }
                else
                {
                    str1 = "select * from J_客户信息 order by 客户类别";
                }

                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                dt2 = new DataTable();
                da.Fill(dt2);
                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    comboBoxEx8.Items.Add(dt2.Rows[i]["客户名称"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();
        }

        private void comboBoxEx9_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Form_XPWG frm = new Form_XPWG(textBoxX1.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if(frm.ZS!="0")
                {
                    textBoxX18.Text=frm.ZS+"张图片";
                }
            }
        }

    }
}
