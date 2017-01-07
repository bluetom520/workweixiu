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
    public partial class Form_WXZL : DockContent
    {
        SqlConnection con;
        public static int hwnd = 0;
        //接收信息事件委托
        public delegate void DataArrivalEventHandler(string msg);
        //事件对象
        public event DataArrivalEventHandler DataArrivalEvent;
        public Form_WXZL()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JCBJ_Load(object sender, EventArgs e)
        {
            if (MainForm.str_zlsc == "1")
            {
                buttonX3.Enabled = true;
            }
            if (MainForm.str_zlxz == "1")
            {
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                contextMenuStrip1.Enabled = false;
            }
            hwnd = (int)this.Handle;
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString();
                string d2 = dateTimeInput2.Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "Select name from syscolumns Where ID=OBJECT_ID('J_维修资料库')";
                SqlDataAdapter da2 = new SqlDataAdapter(str, con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);

                str = "select * from J_维修资料库 where  录入时间 between '" + d1 + "' and '" + d2 + "' ";
                if (comboBoxEx1.Text != "")
                {
                    str += " and 类别='"+comboBoxEx1.Text+"'";
                }
                if (textBox1.Text != "")
                {
                    str += " and 文件名 like '%" + textBox1.Text + "%'";
                }
                if (textBox2.Text != "")
                {
                    str += " and ";
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        string temp = dt2.Rows[i][0].ToString();
                        if (temp.IndexOf("时间") >= 0 || temp.IndexOf("序号") >= 0)
                        {
                            //str += "isnull(convert(varchar," + temp + "),'')+";
                        }
                        else
                        {

                            str += "isnull(" + temp + ",'')+";
                        }
                    }
                    str = str.Substring(0, str.Length - 1);
                    str += " like '%" + textBox2.Text + "%'";
                }
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;

                this.dataGridViewX1.Columns["附件名"].Width = 150;
                this.dataGridViewX1.Columns["文件名"].Width =150;
                this.dataGridViewX1.Columns["附件路径"].Width = 200;
                this.dataGridViewX1.Columns["简介"].Width = 200;
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
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            comboBoxEx1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Queue_data();
        }

        private void Form_XPZJ_FormClosed(object sender, FormClosedEventArgs e)
        {
            hwnd = 0;
        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
        }

        private void 质检通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认下载吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                FJ_DOWN frm = new FJ_DOWN(this.dataGridViewX1.SelectedRows[0].Cells["附件路径"].Value.ToString(),true);
                frm.Show();
            }

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FJ_DOWN frm = new FJ_DOWN(this.dataGridViewX1.SelectedRows[0].Cells["附件路径"].Value.ToString(), false);
                frm.Show();
                string fila_name = "temp\\" + this.dataGridViewX1.SelectedRows[0].Cells["附件名"].Value.ToString();
                System.Diagnostics.Process.Start(fila_name);
            }
            catch
            {
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                delete_data();
                Queue_data();
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

                    string str = "delete from J_维修资料库 where 序号='" + str1 + "'";
                    SqlCommand sqlcom = new SqlCommand(str, con);
                    sqlcom.ExecuteNonQuery();
                    //default_data();
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
    }
}
