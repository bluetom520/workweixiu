using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using grproLib;
using System.Data.OleDb;
namespace DockSample
{
    public partial class Form_GZGL : DockContent
    {
        SqlConnection con;
        public static int hwnd = 0;
        private GridppReport Report = new GridppReport();
        public Form_GZGL()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime( DateTime.Now.Year.ToString()+"-"+DateTime.Now.AddMonths(-1).Month.ToString()+"-01");

            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JFGL_Load(object sender, EventArgs e)
        {
            hwnd = (int)this.Handle;
            Queue_data();
        }
        private void Queue_data()
        {

            try
            {
                string d1 = dateTimeInput1.Value.ToString("yyyy-MM") + "-01";

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "SELECT * from J_员工薪资表 where 日期 ='" + d1 + "' ";

                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["流水号"].Visible = false;
                this.dataGridViewX1.Columns["日期"].DefaultCellStyle.Format = "yyyy年MM月";
                this.dataGridViewX1.Columns["工号"].Width = 70;
                this.dataGridViewX1.Columns["姓名"].Width = 70;
                this.dataGridViewX1.Columns["部门"].Width = 70;
                this.dataGridViewX1.Columns["职务"].Width = 70;
                this.dataGridViewX1.Columns["级别"].Width = 120;
                this.dataGridViewX1.Columns["标准积分"].Width = 70;
                this.dataGridViewX1.Columns["本月积分"].Width = 70;
                this.dataGridViewX1.Columns["等级工资"].Width = 70;
                this.dataGridViewX1.Columns["津贴"].Width = 70;
                this.dataGridViewX1.Columns["全勤奖"].Width = 70;
                this.dataGridViewX1.Columns["积分奖"].Width = 70;
                this.dataGridViewX1.Columns["管理奖"].Width = 70;
                this.dataGridViewX1.Columns["其他奖励"].Width = 70;
                this.dataGridViewX1.Columns["积分奖惩"].Width = 70;
                this.dataGridViewX1.Columns["考勤扣款"].Width = 70;
                this.dataGridViewX1.Columns["加班奖"].Width = 70;
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

            try
            {
                string d1 = dateTimeInput1.Value.ToString("yyyy-MM") + "-01";
                string d2 = dateTimeInput1.Value.AddMonths(-1).ToString("yyyy-MM") + "-16";
                string d3 = dateTimeInput1.Value.ToString("yyyy-MM") + "-16";
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str2 = "SELECT * from J_员工薪资表 where 日期 ='" + d1 + "' ";

                SqlDataAdapter da2 = new SqlDataAdapter(str2, con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    if (MessageBox.Show(dateTimeInput1.Value.Month.ToString() + "月份的工资已存在，确定覆盖吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string str3 = "delete from J_员工薪资表 where 日期 ='" + d1 + "' ";

                        SqlCommand sqlcom2 = new SqlCommand(str3, con);
                        sqlcom2.ExecuteNonQuery();
                        sqlcom2.Dispose();

                        string str1 = "select * from VIEW_GZ a left outer join (select * from J_员工考勤表 where 日期 ='" + d1 + "')  b on a.姓名=b.姓名 left outer join ";
                        str1 += " (SELECT 技术员,sum(合计) as 合计积分 from [J_维修积分表]  where  日期 between '" + d2 + "' and '" + d3 + "' and  审核标志=1 GROUP BY 技术员) c on a.姓名=c.技术员";
                        SqlDataAdapter da = new SqlDataAdapter(str1, con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string[] str_temp = new string[6];
                            str_temp[0] = d1;
                            str_temp[1] = dt.Rows[i]["编号"].ToString();//工号
                            str_temp[2] = dt.Rows[i]["姓名"].ToString();//姓名
                            str_temp[3] = dt.Rows[i]["部门"].ToString();//部门
                            str_temp[4] = dt.Rows[i]["职务"].ToString();//职务
                            str_temp[5] = dt.Rows[i]["级别"].ToString();//级别
                            decimal[] str_jf = new decimal[2]{0,0};

                            if(dt.Rows[i]["标准积分"].ToString()!="")
                                str_jf[0] = Convert.ToDecimal(dt.Rows[i]["标准积分"].ToString());

                            if (dt.Rows[i]["合计积分"].ToString() != "")
                                str_jf[1] = Convert.ToDecimal(dt.Rows[i]["合计积分"].ToString());

                            decimal[] str_money = new decimal[12];
                            str_money[0] = Convert.ToDecimal(dt.Rows[i]["等级工资"].ToString());//等级工资in
                            str_money[1] = Convert.ToDecimal(dt.Rows[i]["津贴"].ToString());//津贴
                            string temp = dt.Rows[i]["合计"].ToString();//合计
                            if (temp != "" && temp != "0")
                            {
                                str_money[2] = 0;
                                str_money[8] = 0 - str_money[0] * Convert.ToDecimal(temp) / (22 * 8);//考勤扣款
                            }
                            else
                            {
                                str_money[2] = Convert.ToDecimal(dt.Rows[i]["全勤奖"].ToString());//全勤奖
                                str_money[8] = 0;
                            }
                            str_money[3] = Convert.ToDecimal(dt.Rows[i]["积分奖"].ToString());//积分奖
                            str_money[4] = Convert.ToDecimal(dt.Rows[i]["管理奖"].ToString());//管理奖
                            str_money[5] = Convert.ToDecimal(dt.Rows[i]["其他奖励"].ToString());//其他奖励
                            temp = dt.Rows[i]["标准积分"].ToString();//标准积分
                            if (Convert.ToDecimal(temp) >= 0)
                            {
                                string temp2 = dt.Rows[i]["合计积分"].ToString();//合计积分
                                IniFiles inifile = new IniFiles("config.ini");
                                string ds = inifile.ReadString("积分", "价值", "");

                                if (temp2 != "")
                                {
                                    if (Convert.ToDecimal(temp) <= Convert.ToDecimal(temp2))
                                    {
                                        str_money[6] =(Convert.ToDecimal(temp2) - Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                        str_money[7] = 0;
                                    }
                                    else
                                    {
                                        str_money[6] = 0;
                                        str_money[7] = (Convert.ToDecimal(temp2) - Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                    }
                                }
                                else
                                {
                                    str_money[7] = (0- Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                }
                            }
                            else
                            {
                                str_money[6] = 0;
                            }
                            str_money[9] = 0;
                            str_money[10] = str_money[0] + str_money[1] + str_money[2] + str_money[3] + str_money[4] + str_money[5] + str_money[6];
                            for (int j = 0; j < 10; j++)
                            {
                                str_money[11] += str_money[j];
                            }

                            string str = "insert into J_员工薪资表 values(";

                            for (int t = 0; t < str_temp.Length; t++)
                            {
                                str += "'" + str_temp[t] + "',";
                            }
                            for (int t = 0; t < str_jf.Length; t++)
                            {
                                str += "" + str_jf[t] + ",";
                            }
                            for (int t = 0; t < str_money.Length; t++)
                            {
                                str += "" + Math.Abs(str_money[t]) + ",";
                            }
                            str = str.Substring(0, str.Length - 1);
                            str += ")";
                            SqlCommand sqlcom = new SqlCommand(str, con);
                            sqlcom.ExecuteNonQuery();
                            sqlcom.Dispose();

                        }

                    }
                }
                else
                {


                    if (MessageBox.Show(dateTimeInput1.Value.Month.ToString() + "月份的工资不存在，确定生成吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {

                        string str1 = "select * from VIEW_GZ a left outer join (select * from J_员工考勤表 where 日期 ='" + d1 + "')  b on a.姓名=b.姓名 left outer join ";
                        str1 += " (SELECT 技术员,sum(合计) as 合计积分 from [J_维修积分表]  where  日期 between '" + d2 + "' and '" + d3 + "' and  审核标志=1 GROUP BY 技术员) c on a.姓名=c.技术员";
                        SqlDataAdapter da = new SqlDataAdapter(str1, con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string[] str_temp = new string[6];
                            str_temp[0] = d1;
                            str_temp[1] = dt.Rows[i]["编号"].ToString();//工号
                            str_temp[2] = dt.Rows[i]["姓名"].ToString();//姓名
                            str_temp[3] = dt.Rows[i]["部门"].ToString();//部门
                            str_temp[4] = dt.Rows[i]["职务"].ToString();//部门
                            str_temp[5] = dt.Rows[i]["级别"].ToString();//级别
                            decimal[] str_jf = new decimal[2] { 0, 0 };

                            if (dt.Rows[i]["标准积分"].ToString() != "")
                                str_jf[0] = Convert.ToDecimal(dt.Rows[i]["标准积分"].ToString());

                            if (dt.Rows[i]["合计积分"].ToString() != "")
                                str_jf[1] = Convert.ToDecimal(dt.Rows[i]["合计积分"].ToString());

                            decimal[] str_money = new decimal[12];
                            str_money[0] = Convert.ToDecimal(dt.Rows[i]["等级工资"].ToString());//等级工资in
                            str_money[1] = Convert.ToDecimal(dt.Rows[i]["津贴"].ToString());//津贴
                            string temp = dt.Rows[i]["合计"].ToString();//合计
                            if (temp != "" && temp != "0")
                            {
                                str_money[2] = 0;
                                str_money[8] = 0 - str_money[0] * Convert.ToDecimal(temp) / (22 * 8);//考勤扣款
                            }
                            else
                            {
                                str_money[2] = Convert.ToDecimal(dt.Rows[i]["全勤奖"].ToString());//全勤奖
                                str_money[8] = 0;
                            }
                            str_money[3] = Convert.ToDecimal(dt.Rows[i]["积分奖"].ToString());//积分奖
                            str_money[4] = Convert.ToDecimal(dt.Rows[i]["管理奖"].ToString());//管理奖
                            str_money[5] = Convert.ToDecimal(dt.Rows[i]["其他奖励"].ToString());//其他奖励
                            temp = dt.Rows[i]["标准积分"].ToString();//标准积分
                            if (Convert.ToDecimal(temp) > 0)
                            {
                                string temp2 = dt.Rows[i]["合计积分"].ToString();//合计积分
                                IniFiles inifile = new IniFiles("config.ini");
                                string ds = inifile.ReadString("积分", "价值", "");

                                if (temp2 != "")
                                {
                                    if (Convert.ToDecimal(temp) <= Convert.ToDecimal(temp2))
                                    {
                                        str_money[6] = (Convert.ToDecimal(temp2) - Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                        str_money[7] = 0;
                                    }
                                    else
                                    {
                                        str_money[6] = 0;
                                        str_money[7] = (Convert.ToDecimal(temp2) - Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                    }
                                }
                                else
                                {
                                    str_money[7] = (0 - Convert.ToDecimal(temp)) * Convert.ToDecimal(ds);
                                }
                            }
                            else
                            {
                                str_money[6] = 0;
                            }
                            str_money[9] = 0;
                            str_money[10] = str_money[0] + str_money[1] + str_money[2] + str_money[3] + str_money[4] + str_money[5]+str_money[6];
                            for (int j = 0; j < 10; j++)
                            {
                                str_money[11] += str_money[j];
                            }

                            string str = "insert into J_员工薪资表 values(";

                            for (int t = 0; t < str_temp.Length; t++)
                            {
                                str += "'" + str_temp[t] + "',";
                            }
                            for (int t = 0; t < str_jf.Length; t++)
                            {
                                str += "" + str_jf[t] + ",";
                            }
                            for (int t = 0; t < str_money.Length; t++)
                            {
                                str += "" + Math.Abs(str_money[t]) + ",";
                            }
                            str = str.Substring(0, str.Length - 1);
                            str += ")";
                            SqlCommand sqlcom = new SqlCommand(str, con);
                            sqlcom.ExecuteNonQuery();
                            sqlcom.Dispose();

                        }
                    }
                }
                Queue_data();
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

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form_GZGL_FormClosing(object sender, FormClosingEventArgs e)
        {
            hwnd = 0;
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定生成报表吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //载入报表模板文件，必须保证 Grid++Report 的安装目录在‘C:\Grid++Report 5.0’下，
                //关于动态设置报表路径与数据绑定参数请参考其它例子程序
                Report.LoadFromFile(@"工资报表.grf");
                //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                //    @"User ID=Admin;Data Source=C:\Grid++Report 5.0\\Samples\Data\Northwind.mdb";
                ReportInitialize();
                Report.PrintPreview(true);
            }
        }
        private void ReportInitialize()
        {
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {

                //string wxbh = this.dataGridViewX1.SelectedRows[0].Cells["维修编号"].Value.ToString();

                try
                {

                    //if (con.State == ConnectionState.Closed)
                    //    con.Open();
                    string d1 = dateTimeInput1.Value.ToString("yyyy-MM") + "-01";
                    string str = "SELECT * from J_员工薪资表 where 日期 ='" + d1 + "' ";
                    //SqlDataAdapter da = new SqlDataAdapter(str, con);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    //GridppReportDemo.Utility.FillRecordToReport(Report, dt);
                    //Report.ConnectionString = @"Provider=SQLNCLI.1;" + MainForm.connetstring;//+ ";Provider=SQLOLEDB.1;Persist Security Info=True;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=IIXYBDKBL3N4HVU;Use Encryption for Data=False;Tag with column collation when possible=False";
                    //Report.DetailGrid.Recordset.ConnectionString = "Provider=SQLOLEDB.1;Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=wxxt;Data Source=192.168.1.134;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=IIXYBDKBL3N4HVU;Use Encryption for Data=False;Tag with column collation when possible=False";
                    //Report.ConnectionString = "Provider=SQLOLEDB.1;Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=wxxt;Data Source=192.168.0.150,1433\\SERVER-PC\\SQLEXPRESS;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=SERVER-PC;Use Encryption for Data=False;Tag with column collation when possible=False";
                    Report.DetailGrid.Recordset.ConnectionString = "Provider=SQLOLEDB.1;Password=sa;Persist Security Info=True;User ID=sa;Initial Catalog=wxxt;Data Source=192.168.0.150,1433\\SERVER-PC\\SQLEXPRESS;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=SERVER-PC;Use Encryption for Data=False;Tag with column collation when possible=False";
                    Report.DetailGrid.Recordset.QuerySQL =str;
 

                    //Report.QuerySQL = str;
                    Report.Title = d1;
                    //Report.DetailGrid.Recordset.ConnectionString = MainForm.connetstring;SQLOLEDB.3;
                    //Report.DetailGrid.Recordset.QuerySQL = str;Provider=SQLNCLI10.1
                }
                catch
                {
                }
                //con.Close();

            }
        }

    }
}
