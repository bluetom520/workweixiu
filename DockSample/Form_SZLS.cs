using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.SqlClient;
namespace DockSample
{
    public partial class Form_SZLS : DockContent
    {
        SqlConnection con;
        public static int hwnd = 0;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_SZLS()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JCBJ_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            Data_initial();
            Queue_data();
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 账户名称 from J_收支账户";
                //str1 += ";select * from J_商品信息表 where 类别='项目'";
                //str1 += ";select 客户名称 from J_客户信息";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["账户名称"].ToString());
                }

                //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //{

                //    comboBoxEx2.Items.Add(ds.Tables[1].Rows[i]["名称"].ToString());
                //}
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{

                //    comboBoxEx3.Items.Add(ds.Tables[2].Rows[i]["客户名称"].ToString());
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }


        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select * from J_收支流水 where  日期 between '" + d1 + "' and '" + d2 + "' and 审核标志=1";
                if (comboBoxEx1.Text != "")
                {
                    str += " and 账户='"+comboBoxEx1.Text+"'";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                //this.dataGridViewX1.Columns["入库编号"].Width = 110;
                //this.dataGridViewX1.Columns["配件类别"].Width = 70;
                //this.dataGridViewX1.Columns["配件编号"].Width = 70;
                //this.dataGridViewX1.Columns["存储位置"].Width = 100;
                //this.dataGridViewX1.Columns["规格型号"].Width = 100;
                //this.dataGridViewX1.Columns["入库日期"].Width = 110;
                //this.dataGridViewX1.Columns["入库数量"].Width = 60;
                //this.dataGridViewX1.Columns["计量单位"].Width = 40;
                //this.dataGridViewX1.Columns["购买单价"].Width = 60;
                //this.dataGridViewX1.Columns["销售单价"].Width = 60;
                //this.dataGridViewX1.Columns["供货商家"].Width = 100;
                //this.dataGridViewX1.Columns["入库人员"].Width = 40;

                //如果父窗体已注册了自定义事件
                if (DataArrivalEvent != null)
                {
                    DataArrivalEvent(dt.Rows.Count.ToString());
                }
            }
            catch
            {
            }
            con.Close();
        }
        

        private void buttonX1_Click(object sender, EventArgs e)
        {
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            comboBoxEx1.Text = "";
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void Form_SZLS_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form_SZLS_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Form_SCMM frm = new Form_SCMM();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    delete_data();
                    Queue_data();
                }
            }
        }

        private void delete_data()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                string str1 = this.dataGridViewX1.SelectedRows[0].Cells["序号"].Value.ToString();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //lb = comboBoxEx1.Text;
                    string str = "delete from J_收支流水 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    //default_data();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }

                con.Close();

            }
            else
            {
                MessageBox.Show("请选择要删除的列！");
            }
        }
    }
}
