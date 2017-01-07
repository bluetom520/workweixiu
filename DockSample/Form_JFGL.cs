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
    public partial class Form_JFGL : Office2007Form
    {
        SqlConnection con;
        public static string year1;
        public static string year2;
        public Form_JFGL()
        {
            InitializeComponent();
            dateTimeInput1.Value = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString());
            dateTimeInput2.Value = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString());
            con = new SqlConnection(MainForm.connetstring);
        }

        private void Form_JFGL_Load(object sender, EventArgs e)
        {
            Data_initial();
            Queue_data();
        }

        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                string str1 = "select 姓名 from J_员工信息表 where 部门='维修部'";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(ds.Tables[0].Rows[i]["姓名"].ToString());
                }


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
                year1 = d1;
                year2 = d2;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string str = "SELECT a.技术员,a.合计积分,b.级别,b.标准积分  from  (SELECT 技术员,sum(合计) as 合计积分 from [J_维修积分表]  where  日期 between '" + d1 + "' and '" + d2 + "' and  审核标志=1 GROUP BY 技术员) a LEFT OUTER JOIN  VIEW_YGXX b on a.技术员=b.姓名";
                string str = "SELECT b.姓名,a.结算积分,a.未结积分,b.级别,b.标准积分  from VIEW_YGXX b LEFT OUTER JOIN   (SELECT 技术员,sum(CASE WHEN 审核标志 = '1' THEN " +
                    "  合计 ELSE 0 END" +
                    ") as 结算积分,sum(CASE WHEN 审核标志 = '0' THEN " +
                    "  合计 ELSE 0 END" +
                    ") as 未结积分  from [J_维修积分表]  where  日期 between '" + d1 + "' and '" + d2 + "'  GROUP BY 技术员) a  on a.技术员=b.姓名 where a.未结积分>=0";
                if (comboBoxEx1.Text != "")
                {
                    str += " and b.姓名='" + comboBoxEx1.Text + "'";
                }
                str += " order by a.结算积分 ";
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

        private void xiangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string temp = this.dataGridViewX1.SelectedRows[0].Cells["姓名"].Value.ToString();
            Form_XXJF frm = new Form_XXJF(temp);
            frm.ShowDialog();

        }

        private void dataGridViewX1_RowContextMenuStripChanged(object sender, DataGridViewRowEventArgs e)
        {


        }

        private void dataGridViewX1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            this.dataGridViewX1.ClearSelection();
            this.dataGridViewX1.Rows[e.RowIndex].Selected = true;
            this.dataGridViewX1.EndEdit();
        }
    }
}
