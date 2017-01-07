using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using DockSample.Customization;
using DevComponents.DotNetBar;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data;
namespace DockSample
{
    public partial class MainForm : Office2007Form
    {
        //public static string connetstring = "server=192.168.0.105;database=wxxt;uid=sa;pwd=sa";
        public static string connetstring;
    	        private bool bcancle = false;
        private bool m_bSaveLayout = true;
                public static int hwnd_jwgl;
        private DeserializeDockContent m_deserializeDockContent;
        SqlConnection con;
        DateTime curDate;
        string info;
        string warn_messge = "";
        public static string str_khzl;//S_客户资料
        public static string str_sfjs;//S_收费结算
        public static string str_wxgl;//S_收费结算
        public static string str_zlxg;//S_资料修改
        public static string str_sjdc;//S_数据导出
        public static string str_zlsc;//S_数据删除
        public static string str_zlxz;//S_资料下载
        public static string str_jsbc;//S_结算保存
        public static string str_pjsh;//S_配件审核
        //private DummySolutionExplorer m_solutionExplorer = new DummySolutionExplorer();
        //private DummyPropertyWindow m_propertyWindow = new DummyPropertyWindow();
        //private DummyToolbox m_toolbox = new DummyToolbox();
        //private DummyOutputWindow m_outputWindow = new DummyOutputWindow();
        //private DummyTaskList m_taskList = new DummyTaskList();
        private ShowStatus m_dummyDoc = new ShowStatus();

        public MainForm()
        {
            InitializeComponent();
            IniFiles inifile = new IniFiles("config.ini");

            string ds = inifile.ReadString("数据库", "IP", "");
            string db = inifile.ReadString("数据库", "DB", "");
            string user = inifile.ReadString("数据库", "USER", "");
            string pass = inifile.ReadString("数据库", "PASSWORD", "");
            connetstring = "Data Source=" + ds + ";Initial Catalog=" + db + " ;User ID=" + user + " ;Password=" + pass;


            DateTime d1 = Convert.ToDateTime("2014-06-01"); 
            DateTime d2 = DateTime.Now;
            TimeSpan t1 = d2 - d1;
            //if (t1.Days > 220)
            //{
            //    this.Close();
            //    this.Dispose();
            //    Application.Exit();
            //}
            //m_solutionExplorer = new DummySolutionExplorer();
            //m_solutionExplorer.RightToLeftLayout = RightToLeftLayout;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                        string modulename = Process.GetCurrentProcess().MainModule.ModuleName;
            string processname = Path.GetFileNameWithoutExtension(modulename);
            Process[] proc = Process.GetProcessesByName(processname);
            if (proc.Length > 1)
            {
                MessageBox.Show("程序已经运行！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bcancle = true;
            }
            else
            {


                Process.Start("AutoUpdate.exe");



                LoginXT lg = new LoginXT();
                if (lg.ShowDialog() == DialogResult.OK)
                {
                    lg.BackgroundImage = Properties.Resources.login;


                }
                else
                {
                    bcancle = true;
                }
            }

        }

        #region Methods

        private IDockContent FindDocument(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }

        private ShowStatus CreateNewDocument()//打开主页面
        {
            ShowStatus dummyDoc = new ShowStatus(); ;
            //int count = 1;
            //string text = "C:\\MADFDKAJ\\ADAKFJASD\\ADFKDSAKFJASD\\ASDFKASDFJASDF\\ASDFIJADSFJ\\ASDFKDFDA" + count.ToString();
            string text = "Document";
            if (FindDocument(text) != null)
            {
                //text = "C:\\MADFDKAJ\\ADAKFJASD\\ADFKDSAKFJASD\\ASDFKASDFJASDF\\ASDFIJADSFJ\\ASDFKDFDA" + count.ToString();
                text = "Document";
                dummyDoc.Text = text;
                
            }
               
            return dummyDoc;        
        }

        private ShowStatus CreateNewDocument(string text)
        {
            ShowStatus dummyDoc = new ShowStatus();
            dummyDoc.Text = text;
            return dummyDoc;
        }

        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                for (int index = dockPanel.Contents.Count - 1; index >= 0; index--)
                {
                    if (dockPanel.Contents[index] is IDockContent)
                    {
                        IDockContent content = (IDockContent)dockPanel.Contents[index];
                        content.DockHandler.Close();
                    }
                }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            //if (persistString == typeof(DummySolutionExplorer).ToString())
            //    return m_solutionExplorer;
            //else if (persistString == typeof(DummyPropertyWindow).ToString())
            //    return m_propertyWindow;
            //else if (persistString == typeof(DummyToolbox).ToString())
            //    return m_toolbox;
            //else if (persistString == typeof(DummyOutputWindow).ToString())
            //    return m_outputWindow;
            //else if (persistString == typeof(DummyTaskList).ToString())
            //    return m_taskList;
            //else            
            //{
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.
                
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(ShowStatus).ToString())
                    return null;

                //ShowStatus dummyDoc = new ShowStatus();
                //if (parsedStrings[1] != string.Empty)
                //    dummyDoc.FileName = parsedStrings[1];
                //if (parsedStrings[2] != string.Empty)
                //    dummyDoc.Text = parsedStrings[2];
                
                return m_dummyDoc;
            //}
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            //m_solutionExplorer.DockPanel = null;
            //m_propertyWindow.DockPanel = null;
            //m_toolbox.DockPanel = null;
            //m_outputWindow.DockPanel = null;
            //m_taskList.DockPanel = null;

            //// Close all other document windows
            //CloseAllDocuments();
        }

        private void SetSchema(object sender, System.EventArgs e)
        {
            CloseAllContents();

        }

        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
        }

        private void SetDockPanelSkinOptions(bool isChecked)
        {
            if (isChecked)
            {
                // All of these options may be set in the designer.
                // This is not a complete list of possible options available in the skin.

                AutoHideStripSkin autoHideSkin = new AutoHideStripSkin();
                autoHideSkin.DockStripGradient.StartColor = Color.AliceBlue;
                autoHideSkin.DockStripGradient.EndColor = Color.Blue;
                autoHideSkin.DockStripGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                autoHideSkin.TabGradient.StartColor = SystemColors.Control;
                autoHideSkin.TabGradient.EndColor = SystemColors.ControlDark;
                autoHideSkin.TabGradient.TextColor = SystemColors.ControlText;
                autoHideSkin.TextFont = new Font("Showcard Gothic", 10);

                dockPanel.Skin.AutoHideStripSkin = autoHideSkin;

                DockPaneStripSkin dockPaneSkin = new DockPaneStripSkin();
                dockPaneSkin.DocumentGradient.DockStripGradient.StartColor = Color.Red;
                dockPaneSkin.DocumentGradient.DockStripGradient.EndColor = Color.Pink;

                dockPaneSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.White;

                dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.Black;

                dockPaneSkin.TextFont = new Font("SketchFlow Print", 10);

                dockPanel.Skin.DockPaneStripSkin = dockPaneSkin;
            }
            else
            {
                dockPanel.Skin = new DockPanelSkin();
            }

        }

        #endregion

        #region Event Handlers

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItemSolutionExplorer_Click(object sender, System.EventArgs e)
        {
            //m_solutionExplorer.Show(dockPanel);
        }

        private void menuItemPropertyWindow_Click(object sender, System.EventArgs e)
        {
            //m_propertyWindow.Show(dockPanel);
        }

        private void menuItemToolbox_Click(object sender, System.EventArgs e)
        {
            //m_toolbox.Show(dockPanel);

        }

        private void menuItemOutputWindow_Click(object sender, System.EventArgs e)
        {
            //m_outputWindow.Show(dockPanel);
        }

        private void menuItemTaskList_Click(object sender, System.EventArgs e)
        {
            //m_taskList.Show(dockPanel);
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
            m_dummyDoc.MinimizeBox = false;
            m_dummyDoc.MaximizeBox = false;
           // m_dummyDoc.CloseButtonVisible = false;

            m_dummyDoc.Show(dockPanel);
        }

        private void menuItemNew_Click(object sender, System.EventArgs e)
        {
            m_dummyDoc.MinimizeBox = false;
            m_dummyDoc.MaximizeBox = false;
            m_dummyDoc.CloseButtonVisible = false;

            m_dummyDoc.Show(dockPanel);

        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            //OpenFileDialog openFile = new OpenFileDialog();

            //openFile.InitialDirectory = Application.ExecutablePath;
            //openFile.Filter = "rtf files (*.rtf)|*.rtf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //openFile.FilterIndex = 1;
            //openFile.RestoreDirectory = true;

            //if (openFile.ShowDialog() == DialogResult.OK)
            //{
            //    string fullName = openFile.FileName;
            //    string fileName = Path.GetFileName(fullName);

            //    if (FindDocument(fileName) != null)
            //    {
            //        MessageBox.Show("The document: " + fileName + " has already opened!");
            //        return;
            //    }

            //    DummyDoc dummyDoc = new DummyDoc();
            //    dummyDoc.Text = fileName;
            //    if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            //    {
            //        dummyDoc.MdiParent = this;
            //        dummyDoc.Show();
            //    }
            //    else
            //        dummyDoc.Show(dockPanel);
            //    try
            //    {
            //        dummyDoc.FileName = fullName;
            //    }
            //    catch (Exception exception)
            //    {
            //        dummyDoc.Close();
            //        MessageBox.Show(exception.Message);
            //    }

            //}
        }

        private void menuItemFile_Popup(object sender, System.EventArgs e)
        {
            //if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            //{
            //    menuItemClose.Enabled = menuItemCloseAll.Enabled = (ActiveMdiChild != null);
            //}
            //else
            //{
            //    menuItemClose.Enabled = (dockPanel.ActiveDocument != null);
            //    menuItemCloseAll.Enabled = (dockPanel.DocumentsCount > 0);
            //}
        }

        private void menuItemClose_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ActiveMdiChild.Close();
            else if (dockPanel.ActiveDocument != null)
                dockPanel.ActiveDocument.DockHandler.Close();
        }

        private void menuItemCloseAll_Click(object sender, System.EventArgs e)
        {
            CloseAllDocuments();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {

            con = new SqlConnection(MainForm.connetstring);
            //string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            //if (File.Exists(configFile))
            //    dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
                        hwnd_jwgl = (int)this.Handle;
            if (bcancle)
            {
                this.Close();
                this.Dispose();
                Application.Exit();
            }
            else
            {
                //this.机故处理ToolStripMenuItem.Visible = false;
//                SetRowStyle();
//                Data_initial();
//                Queue_1();
//                Queue_2();
                toolStripStatusLabel2.Text ="用户："+ LoginXT.username;
                Purview_set();
                curDate = Convert.ToDateTime(DateTime.Now.ToString());
                toolStripStatusLabel3.Text = cal_year(curDate);
                toolStripStatusLabel5.Text ="权限：" +LoginXT.quanxian.Trim();

                if (ShowStatus.hwnd <= 0)
                {
                    ShowStatus frm = new ShowStatus();
                    frm.Show(dockPanel);
                }
//                if (LoginXT.quanxian.Trim() != "40")
//                {
//
//                    timer2.Start();
//                    
//                }
                //else
                //{
                //    timer3.Start();
                //}
//                Purview_set();
                timer1.Start();
            }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private void menuItemToolBar_Click(object sender, System.EventArgs e)
        {
            //toolBar.Visible = menuItemToolBar.Checked = !menuItemToolBar.Checked;
        }

        private void menuItemStatusBar_Click(object sender, System.EventArgs e)
        {
            //statusBar.Visible = menuItemStatusBar.Checked = !menuItemStatusBar.Checked;
        }


        private void menuItemNewWindow_Click(object sender, System.EventArgs e)
        {
            MainForm newWindow = new MainForm();
            newWindow.Text = newWindow.Text + " - New";
            newWindow.Show();
        }

        private void menuItemTools_Popup(object sender, System.EventArgs e)
        {
        }

        private void menuItemLockLayout_Click(object sender, System.EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
        }

        private void menuItemLayoutByCode_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            CloseAllDocuments();

            //m_solutionExplorer = new DummySolutionExplorer();
            //m_propertyWindow = new DummyPropertyWindow();
            //m_toolbox = new DummyToolbox();
            //m_outputWindow = new DummyOutputWindow();
            //m_taskList = new DummyTaskList();

            //m_solutionExplorer.Show(dockPanel, DockState.DockRight);
            //m_propertyWindow.Show(m_solutionExplorer.Pane, m_solutionExplorer);
            //m_toolbox.Show(dockPanel, new Rectangle(98, 133, 200, 383));
            //m_outputWindow.Show(m_solutionExplorer.Pane, DockAlignment.Bottom, 0.35);
            //m_taskList.Show(m_toolbox.Pane, DockAlignment.Left, 0.4);

            ShowStatus doc1 = CreateNewDocument("Document1");
            ShowStatus doc2 = CreateNewDocument("Document2");
            ShowStatus doc3 = CreateNewDocument("Document3");
            ShowStatus doc4 = CreateNewDocument("Document4");
            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(doc1.Pane, null);
            doc3.Show(doc1.Pane, DockAlignment.Bottom, 0.5);
            doc4.Show(doc3.Pane, DockAlignment.Right, 0.5);

            dockPanel.ResumeLayout(true, true);
        }

        private void menuItemLayoutByXml_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            // In order to load layout from XML, we need to close all the DockContents
            CloseAllContents();

            Assembly assembly = Assembly.GetAssembly(typeof(MainForm));
            Stream xmlStream = assembly.GetManifestResourceStream("DockSample.Resources.DockPanel.xml");
            dockPanel.LoadFromXml(xmlStream, m_deserializeDockContent);
            xmlStream.Close();

            dockPanel.ResumeLayout(true, true);
        }

        private void menuItemCloseAllButThisOne_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                Form activeMdi = ActiveMdiChild;
                foreach (Form form in MdiChildren)
                {
                    if (form != activeMdi)
                        form.Close();
                }
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    if (!document.DockHandler.IsActivated)
                        document.DockHandler.Close();
                }
            }
        }

        private void menuItemShowDocumentIcon_Click(object sender, System.EventArgs e)
        {
        }


        private void exitWithoutSavingLayout_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = false;
            Close();
            m_bSaveLayout = true;
        }

        #endregion

        private void dummydocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dummyDoc.Show(dockPanel);
        }

        private void 审核关闭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Form_SHGB.hwnd <= 0)
            {
                Form_SHGB frm = new Form_SHGB();
                frm.DataArrivalEvent += new Form_SHGB.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("审核关闭") as Form;
                f.Focus();
            }
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {

        }

        private void 权限管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_QXGL frm = new Form_QXGL();
            frm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("确定要退出本系统吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    //login_out();
            //    this.Close();
            //    this.Dispose();
            //    Application.Exit();
            //}
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出本系统吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
               // login_out();
                this.Close();
                this.Dispose();
                Application.Exit();
            }
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_MMXG frm = new Form_MMXG();
            frm.Show();
        }

        private void Purview_set()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                con.Open();
                string str1 = "select * from J_权限管理表 where 角色='"+LoginXT.quanxian+"'";
                SqlDataAdapter da = new SqlDataAdapter(str1, con);
                SqlCommandBuilder t_bu = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 2; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (ds.Tables[0].Columns[i].ColumnName.IndexOf("S") < 0)
                    {
                        string str = ds.Tables[0].Rows[0][i].ToString();
                        if (str != "1")
                        {
                            string str2 = ds.Tables[0].Columns[i].ColumnName;
                            show_menu(str2, false);
                            //ToolStripMenuItem toolmenu = (ToolStripMenuItem)this.Controls[str2 + "ToolStripMenuItem"];
                        }
                        else
                        {
                            string str2 = ds.Tables[0].Columns[i].ColumnName;
                            show_menu(str2, true);
                        }
                    }
                }

                str_khzl = ds.Tables[0].Rows[0]["S_客户资料"].ToString();
                str_sfjs = ds.Tables[0].Rows[0]["S_收费结算"].ToString();
                str_wxgl = ds.Tables[0].Rows[0]["S_维修管理"].ToString();
                str_zlxg = ds.Tables[0].Rows[0]["S_资料修改"].ToString();
                str_sjdc = ds.Tables[0].Rows[0]["S_数据导出"].ToString();
                str_zlsc = ds.Tables[0].Rows[0]["S_数据删除"].ToString();
                str_zlxz = ds.Tables[0].Rows[0]["S_资料下载"].ToString();
                str_jsbc = ds.Tables[0].Rows[0]["S_结算保存"].ToString();
                str_pjsh = ds.Tables[0].Rows[0]["S_配件审核"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void show_menu(string str, bool flag)
        {
            foreach (ToolStripMenuItem con3 in this.MainMenuStrip.Items)
            {

                foreach (ToolStripItem con2 in con3.DropDownItems)
                {
                    if (con2 is ToolStripMenuItem)
                    {
                        if (con2.Text == str)
                        {
                            con2.Enabled = flag;
                        }
                    }


                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoginXT lg = new LoginXT();
            if (lg.ShowDialog() == DialogResult.OK)
            {
                //login_out();
                toolStripStatusLabel2.Text = "用户：" + LoginXT.username;
                toolStripStatusLabel5.Text = "权限：" + LoginXT.quanxian.Trim();
                Purview_set();
                //if(dockPanel.DocumentsCount>0)
                //    dockPanel.Cl
                CloseAllDocuments();
                if (ShowStatus.hwnd <= 0)
                {
                    ShowStatus frm = new ShowStatus();
                    //frm.Show(dockPanel);

                    frm.MinimizeBox = false;
                    frm.MaximizeBox = false;
                    frm.CloseButtonVisible = false;

                    frm.Show(dockPanel);
                }
                
            }
        }

        private void 数据备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_HaveBack Fhaveback = new F_HaveBack();
            Fhaveback.Text = "还原/备份数据库";
            Fhaveback.ShowDialog();
            Fhaveback.Close();
        }

        private void 数据恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_HaveBack Fhaveback = new F_HaveBack();
            Fhaveback.Text = "还原/备份数据库";
            Fhaveback.ShowDialog();
            Fhaveback.Close();
        }

        private void 部门类别设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("部门");
            frm.ShowDialog();
        }

        private void 职务类别设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("职位");
            frm.ShowDialog();
        }

        private void 员工信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_YGXX frm = new Form_YGXX();
            frm.Show();
        }

        private void 工资级别设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_GZJB frm = new Form_GZJB();
            frm.Show();
        }

        private void 接修登记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WXDJ frm = new Form_WXDJ();
            frm.Show();
        }

        private void 修品大类设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("维修");
            frm.ShowDialog();
        }

        private void 修品类别管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WXBJ frm = new Form_WXBJ();
            frm.Show();
        }
        private string cal_year(DateTime dat)
        {
            info = curDate.Year.ToString() + "年" + curDate.Month.ToString() + "月" + curDate.Day.ToString() + "日";
            string strDay = curDate.Day.ToString();
            info += "  " + GetDayOfWeekString(curDate) + "  ";

            ChineseCalendarInfo cn = new ChineseCalendarInfo(curDate);
            string cnInfo = cn.LunarYearSexagenary;
            cnInfo += cn.LunarYearAnimal + "年";
            cnInfo += cn.LunarMonthText + "月";
            cnInfo += cn.LunarDayText;
            info += cnInfo;
            return info;
        }
        private string GetDayOfWeekString(DateTime date)
        {
            string weekday;
            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 1)
                weekday = "星期一 ";
            else if (dayOfWeek == 2)
                weekday = "星期二 ";
            else if (dayOfWeek == 3)
                weekday = "星期三 ";
            else if (dayOfWeek == 4)
                weekday = "星期四 ";
            else if (dayOfWeek == 5)
                weekday = "星期五 ";
            else if (dayOfWeek == 6)
                weekday = "星期六 ";
            else
                weekday = "星期日 ";
            return weekday;
        }

        private void 配件分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form_BMLB frm = new Form_BMLB("配件");
            //frm.ShowDialog();
            Form_PJLB frm = new Form_PJLB();
            frm.ShowDialog();
        }

        private void 配件目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PJML frm = new Form_PJML();
            frm.Show();
        }

        private void 配件入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PJRK frm = new Form_PJRK();
            frm.Show();
        }

        private void 配件库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_PJKC.hwnd <= 0)
            {
                Form_PJKC frm = new Form_PJKC();
                frm.DataArrivalEvent += new Form_PJKC.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("配件库存") as Form;
                f.Focus();
            }

        }

        private void 检测报价ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_JCBJ.hwnd <= 0)
            {
                Form_JCBJ frm = new Form_JCBJ();
                frm.DataArrivalEvent += new Form_JCBJ.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("检测报价") as Form;
                f.Focus();
            }
        }

        private void 收支审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_SZSH.hwnd <= 0)
            {
                Form_SZSH frm = new Form_SZSH();
                frm.DataArrivalEvent += new Form_SZSH.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("收支审核") as Form;
                f.Focus();
            }
        }


        private void 维修派工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_WXPG.hwnd <= 0)
            {
                Form_WXPG frm = new Form_WXPG();
                frm.DataArrivalEvent += new Form_WXPG.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("维修派工") as Form;
                f.Focus();
            }
        }

        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ZHGL frm = new Form_ZHGL();
            frm.Show();
        }

        private void 新建收支ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_XJSZ frm = new Form_XJSZ("");
            frm.Show();

        }

        private void 收支流水ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_SZLS.hwnd <= 0)
            {
                Form_SZLS frm = new Form_SZLS();
                frm.DataArrivalEvent += new Form_SZLS.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("收支流水") as Form;
                f.Focus();
            }
        }

        private void 检测维修ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_JCWX.hwnd <= 0)
            {
                Form_JCWX frm = new Form_JCWX();
                frm.DataArrivalEvent += new Form_JCWX.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("检测维修") as Form;
                f.Focus();
            }
        }

        private void 修品质检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_XPZJ.hwnd <= 0)
            {
                Form_XPZJ frm = new Form_XPZJ();
                frm.DataArrivalEvent += new Form_XPZJ.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("修品质检") as Form;
                f.Focus();
            }
        }

        private void 维修查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_WXCX.hwnd <= 0)
            {
                Form_WXCX frm = new Form_WXCX();
                frm.DataArrivalEvent += new Form_WXCX.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
                
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("维修查询") as Form;
                f.Focus();
            }
        }

        private void 取机结算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_LJSH.hwnd <= 0)
            {
                Form_LJSH frm = new Form_LJSH();
                frm.DataArrivalEvent += new Form_LJSH.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("取机结算") as Form;
                f.Focus();
            }
        }

        private void 维修资料管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_WXZL.hwnd <= 0)
            {
                Form_WXZL frm = new Form_WXZL();
                frm.DataArrivalEvent += new Form_WXZL.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("维修资料") as Form;
                f.Focus();
            }
        }

        private void 应收账款ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_YSZK.hwnd <= 0)
            {
                Form_YSZK frm = new Form_YSZK();
                frm.DataArrivalEvent += new Form_YSZK.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("应收账款") as Form;
                f.Focus();
            }
            
        }

        private void 发布公告管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bulletin frm = new Bulletin();
            frm.ShowDialog();
        }

        private void 查看更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("AutoUpdate2.exe");
        }

        private void 员工积分管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_JFGL frm = new Form_JFGL();
            frm.ShowDialog();
        }

        private void 客户信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_KHXX frm = new Form_KHXX();
            frm.ShowDialog();
        }


        private void 公司信息设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_GSXX frm = new Form_GSXX();
            frm.ShowDialog();
        }

        private void 员工考勤管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

                Form_KQGL frm = new Form_KQGL();
                frm.Show();
           
        }

        private void 员工薪酬管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_GZGL.hwnd <= 0)
            {
                Form_GZGL frm = new Form_GZGL();
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("员工薪酬管理") as Form;
                f.Focus();
            }
        }

        private void 客户类别设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("客户");
            frm.ShowDialog();
        }

        private void 送修公司设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("送修");
            frm.ShowDialog();
        }

        private void 主机注册管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ZJZC frm = new Form_ZJZC();
            frm.ShowDialog();
        }

        ///// 在dockPanel中查找已经打开的窗口
        ///// </summary>
        ///// <param name="text">传入的窗口标题</param>
        ///// <returns>返回的窗口</returns>
        //private IDockContent FindDocument(string text)
        //{
        //    if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
        //    {
        //        foreach (Form form in MdiChildren)
        //            if (form.Text == text)
        //                return form as IDockContent;

        //        return null;
        //    }
        //    else
        //    {
        //        foreach (IDockContent content in dockPanel1.Documents)
        //            if (content.DockHandler.TabText == text)
        //                return content;

        //        return null;
        //    }
        //}
        //事件处理
        void childForm_DataArrivalEvent(string msg)
        {
            toolStripStatusLabel6.Text = "总数:" + msg;
        }

        private void 工作流程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form_LCT.hwnd <= 0)
            {
                Form_LCT frm = new Form_LCT();
                //frm.DataArrivalEvent += new Form_LCT.DataArrivalEventHandler(childForm_DataArrivalEvent);
                frm.Show(dockPanel);
            }
            else
            {
                //dockPanel.DockWindows.Contains(Form_WXCX);
                //DockContent.FromChildHandle(Form_WXCX.hwnd);

                Form f = FindDocument("工作流程") as Form;
                f.Focus();
            }
        }

        private void 配件申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PJSH frm = new Form_PJSH();
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 60*60* 1000;
            Scan_data();
            if (warn_messge != "")
            {
                MessageBox.Show(warn_messge);
            }
        }
        private void Scan_data()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string str = "select a.维修编号,a.修品大类,a.修品小类,b.客户名称 from J_维修处理表 a left outer join  J_客户信息 b on a.客户编号=b.客户编号 where  预约日期 between  '" +Convert.ToDateTime( DateTime.Now.AddDays(1).ToShortDateString() )+ "' and '" +Convert.ToDateTime(DateTime.Now.AddDays(2).ToShortDateString())+ "' and (当前状态<=3 or 当前状态=10)";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                warn_messge = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        warn_messge += dt.Rows[i]["客户名称"].ToString()+"-";
                        warn_messge += dt.Rows[i]["修品大类"].ToString() + "-";
                        warn_messge += dt.Rows[i]["修品小类"].ToString() + "-";
                        warn_messge += dt.Rows[i]["维修编号"].ToString() + "\r\n";
                    }
                    warn_messge += "明天到截止日期，请注意！！！";
                }
                
            }
            catch
            {

            }
            con.Close();
        }
    }
}