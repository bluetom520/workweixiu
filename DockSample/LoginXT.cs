using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;  
using System.Diagnostics;
using DevComponents.DotNetBar;

namespace DockSample
{
    public partial class LoginXT : Office2007Form
    {
        SqlConnection con_1;
        public string struser;
        public string strpassword;
        public bool bok=false;
        public bool bcancle=false;
        public static string username;
        public static string user_danwei="";
        public static string DW_NUM = "";
        public static string quanxian;
        public static bool WZ = false;
        public static string strAddr;
        string strMac = "";
        bool FLAG = false;
        public string strconndb;
        [DllImport("kernel32")]//写INI文件
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32")]//读INI文件
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSzie, string lpFileName);
       
        public LoginXT()
        {
            InitializeComponent();
            con_1= new SqlConnection(MainForm.connetstring);
         }


        public void SetConndb()
        {
//            strconndb = "102.12.65.200";
//
//            strConnection = "server=102.12.65.200;database=dn2005;uid=bluetom;pwd=jinbo112358";
            //strConnection += "initial catalog=DN2005;Server='" + strconndb + "';";
            //strConnection += "Connect Timeout=30";
        }
        public void connDB()
        {
//            SetConndb();
//
//            strConn = new SqlConnection(strConnection);
//            strConn.Open();
        }
        


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Update();
        //    struser = comboBox1.Text.Trim();
        //    //user_danwei = comboBox1.Text.Trim();
        //    strpassword = textBox2.Text;

        //    try
        //    {
        //        if (comboBox1.Text == "" )
        //        {
        //            MessageBox.Show("用户名不能为空");
        //        }
        //        else
        //        {
        //            if (textBox2.Text == "")
        //            {
        //                MessageBox.Show("密码不能为空");
        //            }
        //            else
        //            {
        //                //SqlConnection conn = new SqlConnection("Data Source=102.12.65.200;Initial Catalog=DN2005;User ID=dn2005;Password=dn2005");
        //                connDB();
        //                string str_data = "SELECT 权限代号 FROM dbo.[人员表] where 姓名='" + struser + "'";//取数据库时间
        //                SqlCommand cmd2 = new SqlCommand(str_data, strConn);
        //                SqlDataReader dr = cmd2.ExecuteReader();
        //                while (dr.Read())
        //                {
        //                    quanxian = dr[0].ToString();
        //                }
        //                dr.Close();

        //                SqlCommand cmd = new SqlCommand("select count(*) from 人员表 where 姓名='" + struser + "' and 密码='" + strpassword + "'",strConn);
        //                int i = Convert.ToInt32(cmd.ExecuteScalar());
                        

        //                if (i > 0)
        //                {
        //                    //DataTable ds = new DataTable();                            
        //                    //SqlCommand cmd1 = new SqlCommand("select * from TS_TSXX_User_Online_Information where 姓名='" + struser + "'", strConn);
        //                    //SqlDataAdapter da = new SqlDataAdapter(cmd1);
        //                    //SqlCommandBuilder build = new SqlCommandBuilder(da);                            
        //                    //da.Fill(ds);
        //                    //if (ds.Rows.Count == 0)
        //                    //{
        //                    //    username = struser;
        //                    //    update_useronline();

        //                    //    MainForm mfm = new MainForm();
        //                    //    mfm.Show();
        //                    //    this.Hide();
        //                    //}
        //                    //else
        //                    //{
        //                    //    if (ds.Rows[0]["登陆状态"].ToString() == "1")
        //                    //    {
        //                    //        MessageBox.Show("用户已在其它机器登陆，请勿反复登陆！");
        //                    //    }
        //                    //    else
        //                    //    {
        //                            username = struser;
        //                            user_danwei = comboBox1.Text;
        //                            update_useronline();
        //                            write_last_user();

        //                            //MainForm mfm = new MainForm();
        //                            ////mfm.Show();
        //                            this.Hide();
        //                            bok = true;
        //                            //bcancle = false;
        //                    this.DialogResult = DialogResult.OK;
        //                      //  }
        //                    //}
        //                }
        //                else
        //                {
        //                    MessageBox.Show("用户名或者密码错误");
        //                    textBox2.Text = "";
        //                }
        //                strConn.Close();
        //            }
        //        }

               
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        

        //}

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            bcancle = true;
            bok = false; 
            Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtLoginPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                buttonX1_Click(sender, e);
            }
            if (e.KeyChar == 27)
            {
                buttonX2_Click(sender, e);
            }
        }

        private void LoginXT_Load(object sender, EventArgs e)
        {
            ImeHelper.SetIme(this);

            get_wz();

            if (!FLAG)
            {
                MessageBox.Show("您的主机未注册，请联系管理员！");
                this.DialogResult = DialogResult.Cancel;
                bcancle = true;
                bok = false;
                Close();
            }
            try
            {
                if (con_1.State == ConnectionState.Closed)
                con_1.Open();
                string str = "select 姓名 from J_员工信息表";
                SqlDataAdapter da = new SqlDataAdapter(str, con_1);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxEx1.Items.Add(dt.Rows[i]["姓名"].ToString().Trim());
                    
                }

            }
            catch
            {
            }
            con_1.Close();
            read_last_user();
            textBoxX1.Focus();

            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
//            comboBox2.Items.Clear();
            //SqlConnection conn = new SqlConnection("server=102.12.65.200;database=dn2005;uid=bluetom;pwd=jinbo112358");
            //conn.Open();
            //string danwei_selected = comboBox1.SelectedItem.ToString();
            //string sql = "select  * from 人员表 where 单位='" + danwei_selected + "'";
            //SqlCommand cmd = new SqlCommand(sql, conn);

            //SqlDataReader objSqlReader;
            //objSqlReader = cmd.ExecuteReader();

            //while (objSqlReader.Read())
            //{
            //    comboBox1.Items.Add(objSqlReader.GetValue(6).ToString());
            //}
            //objSqlReader.Close();
            //conn.Close();
        }


        private void get_wz()
        {
            try
            {
                NetworkInterface[] aclLocalNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in aclLocalNetworkInterfaces)
                {

                    string temp = adapter.Id;
                    IPInterfaceProperties IPInterfaceProperties = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    {
                        if (UnicastIPAddressInformation.Address.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                        {
                            if (adapter.OperationalStatus == OperationalStatus.Up)
                            {
                                if (UnicastIPAddressInformation.Address.ToString().IndexOf("192.168.0.")==0)                                   
                                    strMac = GetMacAddr(adapter);

                            }
                            //string temp2 = UnicastIPAddressInformation.Address.ToString();
                        }
                    }
                    int t = temp.Length;
                    //if (adapter.OperationalStatus == OperationalStatus.Up)
                    //{

                    //    if (t == 38)
                    //        strMac = GetMacAddr(adapter);

                    //}
                }
                if (con_1.State == ConnectionState.Closed)
                    con_1.Open();
                if (strMac != "")
                {
                    string str1 = "select *  from J_注册用户表   where MAC='" + strMac + "'";
                    SqlDataAdapter da = new SqlDataAdapter(str1, con_1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string zc = dt.Rows[0]["注册"].ToString();
                        if (zc == "1")
                        {
                            FLAG = true;
                        }


                    }
                    else
                    {
                        string machineName = Environment.MachineName;
                        string str = "insert into J_注册用户表 (主机名,MAC)values('" + machineName + "','" + strMac + "')";
                        SqlCommand sqlcom = new SqlCommand(str, con_1);
                        sqlcom.ExecuteNonQuery();
                        sqlcom.Dispose();
                    }
                }



            }
            catch
            {

            }
            con_1.Close();

        }

        // 获取mac地址
        private string GetMacAddr(NetworkInterface adapter)
        {
            String strMacAddr = "";
            PhysicalAddress clMacAddr = adapter.GetPhysicalAddress();
            byte[] abMacAddr = clMacAddr.GetAddressBytes();

            for (int i = 0; i < abMacAddr.Length; i++)
            {
                strMacAddr = strMacAddr + abMacAddr[i].ToString("X2");

                // 在每个字节间插入冒号
                if (abMacAddr.Length - 1 != i)
                {
                    strMacAddr = strMacAddr + ":";
                }
            }

            return strMacAddr;
        }


        private void write_last_user()
        {
            String Afilename = "user_rem.ini";
            IniFiles inifiles = new IniFiles(Afilename);
            //string path = DummyDoc.getpath + "user_rem.ini";
            //WritePrivateProfileString("单位", "科别", comboBox1.Text,path);
            //WritePrivateProfileString("姓名", "用户名", comboBox2.Text, path);
//            inifiles.WriteString("单位", "科别", comboBox1.Text);
            inifiles.WriteString("姓名", "用户名", comboBoxEx1.Text);     
        }
        private void read_last_user()
        {
            String Afilename = "user_rem.ini";
            IniFiles inifiles = new IniFiles(Afilename);
            string strdef = "";
            string strvalue;
//            strvalue = inifiles.ReadString("单位", "科别", strdef);
//            comboBox1.Text = strvalue;
            strvalue = inifiles.ReadString("姓名", "用户名", strdef);
            comboBoxEx1.Text = strvalue; 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Update();
            struser = comboBoxEx1.Text.Trim();
            //user_danwei = comboBox1.Text.Trim();
            strpassword = textBoxX1.Text;

            try
            {
                if (comboBoxEx1.Text == "")
                {
                    MessageBox.Show("用户名不能为空");
                }
                else
                {
                    if (textBoxX1.Text == "")
                    {
                        MessageBox.Show("密码不能为空");
                    }
                    else
                    {
                        //SqlConnection conn = new SqlConnection("Data Source=102.12.65.200;Initial Catalog=DN2005;User ID=dn2005;Password=dn2005");
                        if(con_1.State==ConnectionState.Closed)
                        con_1.Open();
                        string str_data = "SELECT 角色 FROM dbo.[J_员工信息表] where 姓名='" + struser + "'";//取数据库时间
                        SqlCommand cmd2 = new SqlCommand(str_data, con_1);
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            quanxian = dr[0].ToString().Trim();
                        }
                        dr.Close();

                        SqlCommand cmd = new SqlCommand("select count(*) from J_员工信息表 where 姓名='" + struser + "' and 密码='" + strpassword + "'", con_1);
                        int i = Convert.ToInt32(cmd.ExecuteScalar());


                        if (i > 0)
                        {

                            username = struser;
                            //user_danwei = comboBox1.Text;
                            //update_useronline();
                            write_last_user();

                            //MainForm mfm = new MainForm();
                            ////mfm.Show();
                            this.Hide();
                            bok = true;
                            //bcancle = false;
                            this.DialogResult = DialogResult.OK;
                            //  }
                            //}
                        }
                        else
                        {
                            MessageBox.Show("用户名或者密码错误");
                            textBoxX1.Text = "";
                        }
                        con_1.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            bcancle = true;
            bok = false;
            Close();
        }

        private void LoginXT_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                buttonX1_Click(sender, e);
            }
            if (e.KeyChar == 27)
            {
                buttonX2_Click(sender, e);
            }
        }

        private void labelX3_Click(object sender, EventArgs e)
        {

        }
        //public string ReadString(string section, string key, string def)//////////读zd_KHDSZ.ini
        //{
        //    StringBuilder temp = new StringBuilder(1024);
        //    //string path = DummyDoc.getpath + "user_rem.ini";
        //    GetPrivateProfileString(section, key, def, temp, 1024, path);
        //    return temp.ToString();
        //}

    }
}
