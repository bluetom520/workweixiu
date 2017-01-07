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
    public partial class Form_XXJF : Office2007Form
    {
        SqlConnection con;
        string name;
        private DataGridViewCellStyle m_RowStyleNormal;
        private DataGridViewCellStyle m_RowStyleAlternate;
        private DataGridViewCellStyle m_normal;
        private DataGridViewCellStyle m_skyblue;
        public Form_XXJF(string str)
        {
            InitializeComponent();
            con = new SqlConnection(MainForm.connetstring);
            name = str;
        }
        private void SetRowStyle()
        {
            //可根据需要设置更多样式属性，如字体、对齐、前景色、背景色等
            m_RowStyleNormal = new DataGridViewCellStyle();
            m_RowStyleNormal.BackColor = Color.LightGreen;
            m_RowStyleNormal.SelectionBackColor = Color.LightSteelBlue;

            m_RowStyleAlternate = new DataGridViewCellStyle();
            m_RowStyleAlternate.BackColor = Color.LightGray;
            m_RowStyleAlternate.SelectionBackColor = Color.LightSlateGray;

            m_normal = new DataGridViewCellStyle();
            m_normal.BackColor = Color.White;
            m_normal.SelectionBackColor = Color.LightGray;

            m_skyblue = new DataGridViewCellStyle();
            m_skyblue.BackColor = Color.Orange;
        }
        private void Form_XXJF_Load(object sender, EventArgs e)
        {
            this.Text += "--" + name;
            Data_initial();
            SetRowStyle();
        }
        private void Data_initial()
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string d1 = Form_JFGL.year1;
                string d2 = Form_JFGL.year2;
                string str1 = "select * from J_维修积分表 where 技术员='" + name + "' and 日期 between '" + d1 + "' and '" + d2 + "'  order by 日期";


                SqlDataAdapter da = new SqlDataAdapter(str1, con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewX1.DataSource = dt;
                this.dataGridViewX1.Columns["序号"].Visible = false;
                this.dataGridViewX1.Columns["技术员"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
                        DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "审核标志")
            {
                string getdata = dgv.Rows[e.RowIndex].Cells["审核标志"].Value.ToString();
                switch (getdata)
                {
                    case "0": e.Value = "未结";
                        break;
                    case "1": e.Value = "已结";
                        break;

                }

        }
    }

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (this.dataGridViewX1.Rows[e.RowIndex].Cells["审核标志"].Value == DBNull.Value)
                return;
            string str1 = this.dataGridViewX1.Rows[e.RowIndex].Cells["审核标志"].Value.ToString();
            if (str1 == "0")/////////////无提示
            {
                //RowIcon = JWZB.Properties.Resources.ball_green;
                //strToolTip = "已解决";
                //e.Graphics.DrawImage(RowIcon, e.RowBounds.Left + this.dataGridView2.RowHeadersWidth - 20, e.RowBounds.Top + 4, 16, 16);//绘制图标
                //this.dataGridView2.Rows[e.RowIndex].HeaderCell.ToolTipText = strToolTip;//设置提示信息
                this.dataGridViewX1.Rows[e.RowIndex].DefaultCellStyle = m_RowStyleNormal;


            }
            else if (str1 == "1")/////////////无提示
            {
                //RowIcon = JWZB.Properties.Resources.ball_green;
                //strToolTip = "已解决";
                //e.Graphics.DrawImage(RowIcon, e.RowBounds.Left + this.dataGridView2.RowHeadersWidth - 20, e.RowBounds.Top + 4, 16, 16);//绘制图标
                //this.dataGridView2.Rows[e.RowIndex].HeaderCell.ToolTipText = strToolTip;//设置提示信息
                this.dataGridViewX1.Rows[e.RowIndex].DefaultCellStyle = m_RowStyleAlternate;


            }
            else
            {

            }
        }
            
        }
}
