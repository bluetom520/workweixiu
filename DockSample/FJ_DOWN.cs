using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using DevComponents.DotNetBar;
namespace DockSample
{
    public partial class FJ_DOWN : Office2007Form
    {
        SqlConnection con_1;
        public string hostname;
        public int port;
        public string user;
        public string password;
        public string str_path;
        int num;
        bool flag;
        string[] str_p;

        public FJ_DOWN(string str,bool f1)
        {
            InitializeComponent();
            flag = f1;
            con_1 = new SqlConnection(MainForm.connetstring);
            str_path = str;
            str_p = str_path.Split('\n');
            num = str_p.Length;
            for (int i = 0; i < num; i++)
            {
                GroupBox g = contruct_group(i,10, 10 + (84 + 10) * i);
                this.Controls.Add(g);
                this.Controls["groupBox_" + i.ToString()].Controls["label_2"].Text += Path.GetFileName(str_p[i]);
            }
            this.Height += (84 + 10) * (num-1);
        }


        private void get_ftp()
        {
            try
            {
                if (con_1.State == ConnectionState.Closed)
                    con_1.Open();

                string str = "select * from J_FTP服务";
                SqlDataAdapter da = new SqlDataAdapter(str, con_1);
                DataTable dt = new DataTable();
                da.Fill(dt);
                IniFiles inifile = new IniFiles("config.ini");

                string ds = inifile.ReadString("数据库", "IP", "");
                hostname = ds;
                port = Convert.ToInt32(dt.Rows[0]["端口"].ToString());
                user = dt.Rows[0]["帐号"].ToString();
                password = dt.Rows[0]["密码"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

                con_1.Close();
            

        }

        private void FJ_DOWN_Load(object sender, EventArgs e)
        {
            get_ftp();
            file_down();
            //this.Controls["groupBox_1"].Controls["label_2"].Text = "1111";
            //this.Controls["groupBox_2"].Controls["label_2"].Text = "2222";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 30;
            for (int i = 0; i < num; i++)
            {
                Zjs.Ftp.ftp f = (Zjs.Ftp.ftp)this.Controls["groupBox_" + i.ToString()].Controls["ftp_1"];
                this.Controls["groupBox_" + i.ToString()].Controls["label_1"].Text = f.progressBar.Value.ToString() + "%";
                
                if (f.progressBar.Value == 100)
                {

                        string str = Path.GetFileName(str_p[i]);
                        FileInfo info = new FileInfo(str);
                        info.Delete();
                        this.Close();
                  
                    //timer1.Stop();
                   

                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            //string path_file;
           
            //ftp2.Hostname = hostname;
            //ftp2.Port = port;
            //ftp2.User = user;
            //ftp2.Password = password;
            //try
            //{

            //    if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        path_file = this.folderBrowserDialog1.SelectedPath;
            //        ftp2.Remote = "";

            //        ftp2.Filesavedir = path_file;
            //        //string str = this.dataGridView1.SelectedRows[0].Cells["附件路径"].Value.ToString();
            //        string str = str_path;
            //        string[] str_p = str.Split('\n');
            //        ftp2.getfile(str_p[0]);
            //        //foreach (string p in str_p)
            //        //{

            //        //    ftp1.getfile(p);
            //        //    //timer1.Start();
            //        //    Thread.Sleep(10);
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //                   path_wav = path_wav = Readstring("FilePath", "savefile", "", PeizhiPath);
            //    MessageBox.Show(ex.Message);
            //}

        }
        private void file_down()
        {
            string path_file;

            //string[] str_p = str_path.Split('\n');
                try
                {
                    if (flag)
                    {
                        if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                        {
                            timer1.Start();
                            for (int i = 0; i < num; i++)
                            {
                                //this.Controls["groupBox_" + i.ToString()].Controls["label_2"].Text +=Path.GetFileName(str_p[i]);
                                Zjs.Ftp.ftp f = (Zjs.Ftp.ftp)this.Controls["groupBox_" + i.ToString()].Controls["ftp_1"];
                                f.Hostname = hostname;
                                f.Port = port;
                                f.User = user;
                                f.Password = password;
                                path_file = this.folderBrowserDialog1.SelectedPath;
                                f.Remote = "";

                                f.Filesavedir = path_file;


                                f.getfile(str_p[i]);
                                //foreach (string p in str_p)
                                //{

                                //    ftp1.getfile(p);
                                //    //timer1.Start();
                                //    Thread.Sleep(10);
                                //}
                            }
                        }
                    }
                    else
                    {
                        timer1.Start();
                        for (int i = 0; i < num; i++)
                        {
                            //this.Controls["groupBox_" + i.ToString()].Controls["label_2"].Text +=Path.GetFileName(str_p[i]);
                            Zjs.Ftp.ftp f = (Zjs.Ftp.ftp)this.Controls["groupBox_" + i.ToString()].Controls["ftp_1"];
                            f.Hostname = hostname;
                            f.Port = port;
                            f.User = user;
                            f.Password = password;
                            string D_path = "temp";
                            DirectoryInfo Dinfo = new DirectoryInfo(D_path);
                            Dinfo.Create();
                            path_file = D_path;
                            f.Remote = "";

                            f.Filesavedir = path_file;


                            f.getfile(str_p[i]);
                            //foreach (string p in str_p)
                            //{

                            //    ftp1.getfile(p);
                            //    //timer1.Start();
                            //    Thread.Sleep(10);
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    //                   path_wav = path_wav = Readstring("FilePath", "savefile", "", PeizhiPath);
                    MessageBox.Show(ex.Message);
                }
            
        }

        private GroupBox contruct_group(int t,int x,int y)
        {
            Zjs.Ftp.ftp ftp_1 = new Zjs.Ftp.ftp(); ;
            GroupBox groupBox_1 = new GroupBox();
            Label label_2 = new Label();
            Label label_1 = new Label();
            // groupBox_1
            // 
            groupBox_1.Controls.Add(label_2);
            groupBox_1.Controls.Add(label_1);
            groupBox_1.Controls.Add(ftp_1);
            groupBox_1.Location = new System.Drawing.Point(x, y);
            groupBox_1.Name = "groupBox_"+t.ToString();
            groupBox_1.Size = new System.Drawing.Size(407, 84);
            groupBox_1.TabIndex = 4;
            groupBox_1.TabStop = false;
            // 
            // label_1
            // 
            label_1.AutoSize = true;
            label_1.Location = new System.Drawing.Point(x+176, x+40);
            label_1.Name = "label_1";
            label_1.Size = new System.Drawing.Size(0, 12);
            label_1.TabIndex = 3;
            // 
            // label_2
            // 
            label_2.AutoSize = true;
            label_2.Location = new System.Drawing.Point(x-1, x+7);
            label_2.Name = "label_2";
            label_2.Size = new System.Drawing.Size(65, 12);
            label_2.TabIndex = 4;
            label_2.Text = "下载文件：";
            // 
            // ftp_1
            // 
            ftp_1.Filename = null;
            ftp_1.Filesavedir = "D:\\Program Files\\Microsoft Visual Studio 10.0\\Common7\\IDE\\";
            ftp_1.Hostname = null;
            ftp_1.Location = new System.Drawing.Point(x-6, x+28);
            ftp_1.Name = "ftp_1";
            ftp_1.Port = 0;
            ftp_1.Receivedbyte = ((long)(0));
            ftp_1.Remote = null;
            ftp_1.Size = new System.Drawing.Size(390, 38);
            ftp_1.TabIndex = 2;

            return groupBox_1;
        }
    }

}
