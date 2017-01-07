using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;
using System.Data.OleDb;
namespace DockSample
{
    public partial class Form_KQGL : Office2007Form
    {
        SqlConnection con;
        public Form_KQGL()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime( DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-01");

            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JFGL_Load(object sender, EventArgs e)
        {
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString("yyyy-MM") + "-01";

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "SELECT 日期,姓名,部门,迟到 as '迟到/分钟' ,早退 as '早退/分钟',请假 as '请假/天',合计 as '合计/小时' from J_员工考勤表 where 日期 ='" + d1 + "' ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                //this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX1.Columns["日期"].DefaultCellStyle.Format = "yyyy年MM月";
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
            }
            catch
            {
            }
            con.Close();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01");
            //dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string path = "";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                    string strcon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0'";
                    DataTable dt = new DataTable();
                    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [考勤汇总$]", strcon);
                    oada.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        string date_kq = dt.Rows[0][1].ToString().Trim().Split('~')[0];// 记录日期
                        string str1 = "delete from J_员工考勤表 where 日期='" + Convert.ToDateTime(date_kq).ToString() + "'";

                        SqlCommand sqlcom2 = new SqlCommand(str1, con);
                        sqlcom2.ExecuteNonQuery();
                        sqlcom2.Dispose();
                        dateTimeInput1.Value = Convert.ToDateTime(date_kq);
                        for (int i = 3; i < dt.Rows.Count; i++)
                        {
                            string[] temp_data = new string[7];
                            if (dt.Rows[i][1].ToString() != "")
                            {
                                temp_data[0] = Convert.ToDateTime(date_kq).ToString();
                                temp_data[1] = dt.Rows[i][1].ToString();// 姓名
                                temp_data[2] = dt.Rows[i][2].ToString();//部门
                                temp_data[3] = dt.Rows[i][6].ToString();//迟到
                                temp_data[4] = dt.Rows[i][8].ToString();//早退
                                temp_data[5] = dt.Rows[i][13].ToString();//请假
                                //int temp = Math.Ceiling((Convert.ToDouble(temp_data[3]) + Convert.ToDouble(temp_data[4])) / 60);
                                int temp = (int)Math.Ceiling((Convert.ToDouble(temp_data[3]) + Convert.ToDouble(temp_data[4])) / 60) + Convert.ToInt32(temp_data[5]) * 8;
                                temp_data[6] = temp.ToString();//合计
                                string str = "insert into J_员工考勤表 values(";
                                string str_temp = "";
                                for (int t = 0; t < temp_data.Length; t++)
                                {
                                    str_temp += ",'" + temp_data[t] + "'";
                                }
                                str_temp = str_temp.Substring(1) + ")";
                                str += str_temp;
                                SqlCommand sqlcom = new SqlCommand(str, con);
                                sqlcom.ExecuteNonQuery();
                                sqlcom.Dispose();
                            }

                        }
                        MessageBox.Show("导入成功！");

                        Queue_data();

                    }

                    //textBox3.Text = Path.GetFileName(path);
                }

            }
            catch
            {

            }
            con.Close();

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            Queue_data();
        }
    }
}
