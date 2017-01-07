using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using System.IO;
namespace DockSample
{
    public partial class Form_XXXG : Office2007Form
    {
        string wxbh;

        SqlConnection con;
        public Form_XXXG(string str_temp)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            wxbh = str_temp;

        }

        private void Form_XXXG_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);
            Data_initial();

        }

        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 名称 from J_基础信息表 where 类别='维修'";

                str1 += ";select * from J_商品信息表 where 类别='品牌'";

                str1 += "select * from J_维修处理表  where  维修编号='" + wxbh + "'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();         
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["名称"].ToString());
                }


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
                if (ds.Tables[2].Rows.Count > 0)
                {
                    textBoxX1.Text = wxbh;
                    textBoxX2.Text = ds.Tables[2].Rows[0]["修品型号"].ToString();
                    textBoxX3.Text = ds.Tables[2].Rows[0]["备注"].ToString();
                    textBoxX4.Text = ds.Tables[2].Rows[0]["规格参数"].ToString();
                    textBoxX7.Text = ds.Tables[2].Rows[0]["修品SN1"].ToString();
                    textBoxX11.Text = ds.Tables[2].Rows[0]["故障描述"].ToString();
                    textBoxX12.Text = ds.Tables[2].Rows[0]["修品PN号"].ToString();
                    //textBoxX13.Text = ds.Tables[2].Rows[0]["随机附件"].ToString();
                    textBoxX14.Text = ds.Tables[2].Rows[0]["修品SN2"].ToString();
                    comboBoxEx1.Text = ds.Tables[2].Rows[0]["修品大类"].ToString();
                    comboBoxEx2.Text = ds.Tables[2].Rows[0]["修品小类"].ToString();
                    comboBoxEx3.Text = ds.Tables[2].Rows[0]["修品品牌"].ToString();
                    comboBoxEx7.Text = ds.Tables[2].Rows[0]["优先级"].ToString();
                }
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

                string str1 = "select 修品小类 from J_维修报价 where 修品大类='" + comboBoxEx1.Text + "'";

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

        private void buttonX4_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "update J_维修处理表 set  修品型号='" + textBoxX2.Text + "',备注='" + textBoxX3.Text + "',规格参数='" + textBoxX4.Text + "',修品SN1='" + textBoxX7.Text + "',故障描述='" + textBoxX11.Text + "',修品PN号='" + textBoxX12.Text + "'";
                str1 += ",修品SN2='" + textBoxX14.Text + "',修品大类='" + comboBoxEx1.Text + "',修品小类='" + comboBoxEx2.Text + "',修品品牌='" + comboBoxEx3.Text + "',优先级='" + comboBoxEx7.Text + "'  where 维修编号='" + wxbh + "'";
                SqlCommand SQL = new SqlCommand(str1, con);
                SQL.ExecuteNonQuery();
                SQL.Dispose();
                con.Close();

                this.DialogResult = DialogResult.OK;
            }

            catch
            {

            }
        }
    }
}
