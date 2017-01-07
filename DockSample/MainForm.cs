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
        public static string str_khzl;//S_�ͻ�����
        public static string str_sfjs;//S_�շѽ���
        public static string str_wxgl;//S_�շѽ���
        public static string str_zlxg;//S_�����޸�
        public static string str_sjdc;//S_���ݵ���
        public static string str_zlsc;//S_����ɾ��
        public static string str_zlxz;//S_��������
        public static string str_jsbc;//S_���㱣��
        public static string str_pjsh;//S_������
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

            string ds = inifile.ReadString("���ݿ�", "IP", "");
            string db = inifile.ReadString("���ݿ�", "DB", "");
            string user = inifile.ReadString("���ݿ�", "USER", "");
            string pass = inifile.ReadString("���ݿ�", "PASSWORD", "");
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
                MessageBox.Show("�����Ѿ����У�", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private ShowStatus CreateNewDocument()//����ҳ��
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
                //this.���ʴ���ToolStripMenuItem.Visible = false;
//                SetRowStyle();
//                Data_initial();
//                Queue_1();
//                Queue_2();
                toolStripStatusLabel2.Text ="�û���"+ LoginXT.username;
                Purview_set();
                curDate = Convert.ToDateTime(DateTime.Now.ToString());
                toolStripStatusLabel3.Text = cal_year(curDate);
                toolStripStatusLabel5.Text ="Ȩ�ޣ�" +LoginXT.quanxian.Trim();

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

        private void ��˹ر�ToolStripMenuItem1_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��˹ر�") as Form;
                f.Focus();
            }
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {

        }

        private void Ȩ�޹���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_QXGL frm = new Form_QXGL();
            frm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("ȷ��Ҫ�˳���ϵͳ��", "����", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    //login_out();
            //    this.Close();
            //    this.Dispose();
            //    Application.Exit();
            //}
        }

        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ȷ��Ҫ�˳���ϵͳ��", "����", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
               // login_out();
                this.Close();
                this.Dispose();
                Application.Exit();
            }
        }

        private void �޸�����ToolStripMenuItem_Click(object sender, EventArgs e)
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
                string str1 = "select * from J_Ȩ�޹���� where ��ɫ='"+LoginXT.quanxian+"'";
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

                str_khzl = ds.Tables[0].Rows[0]["S_�ͻ�����"].ToString();
                str_sfjs = ds.Tables[0].Rows[0]["S_�շѽ���"].ToString();
                str_wxgl = ds.Tables[0].Rows[0]["S_ά�޹���"].ToString();
                str_zlxg = ds.Tables[0].Rows[0]["S_�����޸�"].ToString();
                str_sjdc = ds.Tables[0].Rows[0]["S_���ݵ���"].ToString();
                str_zlsc = ds.Tables[0].Rows[0]["S_����ɾ��"].ToString();
                str_zlxz = ds.Tables[0].Rows[0]["S_��������"].ToString();
                str_jsbc = ds.Tables[0].Rows[0]["S_���㱣��"].ToString();
                str_pjsh = ds.Tables[0].Rows[0]["S_������"].ToString();
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
                toolStripStatusLabel2.Text = "�û���" + LoginXT.username;
                toolStripStatusLabel5.Text = "Ȩ�ޣ�" + LoginXT.quanxian.Trim();
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

        private void ���ݱ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_HaveBack Fhaveback = new F_HaveBack();
            Fhaveback.Text = "��ԭ/�������ݿ�";
            Fhaveback.ShowDialog();
            Fhaveback.Close();
        }

        private void ���ݻָ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_HaveBack Fhaveback = new F_HaveBack();
            Fhaveback.Text = "��ԭ/�������ݿ�";
            Fhaveback.ShowDialog();
            Fhaveback.Close();
        }

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("����");
            frm.ShowDialog();
        }

        private void ְ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("ְλ");
            frm.ShowDialog();
        }

        private void Ա����Ϣ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_YGXX frm = new Form_YGXX();
            frm.Show();
        }

        private void ���ʼ�������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_GZJB frm = new Form_GZJB();
            frm.Show();
        }

        private void ���޵Ǽ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WXDJ frm = new Form_WXDJ();
            frm.Show();
        }

        private void ��Ʒ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("ά��");
            frm.ShowDialog();
        }

        private void ��Ʒ������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WXBJ frm = new Form_WXBJ();
            frm.Show();
        }
        private string cal_year(DateTime dat)
        {
            info = curDate.Year.ToString() + "��" + curDate.Month.ToString() + "��" + curDate.Day.ToString() + "��";
            string strDay = curDate.Day.ToString();
            info += "  " + GetDayOfWeekString(curDate) + "  ";

            ChineseCalendarInfo cn = new ChineseCalendarInfo(curDate);
            string cnInfo = cn.LunarYearSexagenary;
            cnInfo += cn.LunarYearAnimal + "��";
            cnInfo += cn.LunarMonthText + "��";
            cnInfo += cn.LunarDayText;
            info += cnInfo;
            return info;
        }
        private string GetDayOfWeekString(DateTime date)
        {
            string weekday;
            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 1)
                weekday = "����һ ";
            else if (dayOfWeek == 2)
                weekday = "���ڶ� ";
            else if (dayOfWeek == 3)
                weekday = "������ ";
            else if (dayOfWeek == 4)
                weekday = "������ ";
            else if (dayOfWeek == 5)
                weekday = "������ ";
            else if (dayOfWeek == 6)
                weekday = "������ ";
            else
                weekday = "������ ";
            return weekday;
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form_BMLB frm = new Form_BMLB("���");
            //frm.ShowDialog();
            Form_PJLB frm = new Form_PJLB();
            frm.ShowDialog();
        }

        private void ���Ŀ¼ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PJML frm = new Form_PJML();
            frm.Show();
        }

        private void ������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PJRK frm = new Form_PJRK();
            frm.Show();
        }

        private void ������ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("������") as Form;
                f.Focus();
            }

        }

        private void ��ⱨ��ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��ⱨ��") as Form;
                f.Focus();
            }
        }

        private void ��֧���ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��֧���") as Form;
                f.Focus();
            }
        }


        private void ά���ɹ�ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("ά���ɹ�") as Form;
                f.Focus();
            }
        }

        private void �˻�����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ZHGL frm = new Form_ZHGL();
            frm.Show();
        }

        private void �½���֧ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_XJSZ frm = new Form_XJSZ("");
            frm.Show();

        }

        private void ��֧��ˮToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��֧��ˮ") as Form;
                f.Focus();
            }
        }

        private void ���ά��ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("���ά��") as Form;
                f.Focus();
            }
        }

        private void ��Ʒ�ʼ�ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��Ʒ�ʼ�") as Form;
                f.Focus();
            }
        }

        private void ά�޲�ѯToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("ά�޲�ѯ") as Form;
                f.Focus();
            }
        }

        private void ȡ������ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("ȡ������") as Form;
                f.Focus();
            }
        }

        private void ά�����Ϲ���ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("ά������") as Form;
                f.Focus();
            }
        }

        private void Ӧ���˿�ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("Ӧ���˿�") as Form;
                f.Focus();
            }
            
        }

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bulletin frm = new Bulletin();
            frm.ShowDialog();
        }

        private void �鿴����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("AutoUpdate2.exe");
        }

        private void Ա�����ֹ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_JFGL frm = new Form_JFGL();
            frm.ShowDialog();
        }

        private void �ͻ���Ϣ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_KHXX frm = new Form_KHXX();
            frm.ShowDialog();
        }


        private void ��˾��Ϣ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_GSXX frm = new Form_GSXX();
            frm.ShowDialog();
        }

        private void Ա�����ڹ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {

                Form_KQGL frm = new Form_KQGL();
                frm.Show();
           
        }

        private void Ա��н�����ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("Ա��н�����") as Form;
                f.Focus();
            }
        }

        private void �ͻ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("�ͻ�");
            frm.ShowDialog();
        }

        private void ���޹�˾����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_BMLB frm = new Form_BMLB("����");
            frm.ShowDialog();
        }

        private void ����ע�����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ZJZC frm = new Form_ZJZC();
            frm.ShowDialog();
        }

        ///// ��dockPanel�в����Ѿ��򿪵Ĵ���
        ///// </summary>
        ///// <param name="text">����Ĵ��ڱ���</param>
        ///// <returns>���صĴ���</returns>
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
        //�¼�����
        void childForm_DataArrivalEvent(string msg)
        {
            toolStripStatusLabel6.Text = "����:" + msg;
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
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

                Form f = FindDocument("��������") as Form;
                f.Focus();
            }
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
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
                string str = "select a.ά�ޱ��,a.��Ʒ����,a.��ƷС��,b.�ͻ����� from J_ά�޴���� a left outer join  J_�ͻ���Ϣ b on a.�ͻ����=b.�ͻ���� where  ԤԼ���� between  '" +Convert.ToDateTime( DateTime.Now.AddDays(1).ToShortDateString() )+ "' and '" +Convert.ToDateTime(DateTime.Now.AddDays(2).ToShortDateString())+ "' and (��ǰ״̬<=3 or ��ǰ״̬=10)";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                warn_messge = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        warn_messge += dt.Rows[i]["�ͻ�����"].ToString()+"-";
                        warn_messge += dt.Rows[i]["��Ʒ����"].ToString() + "-";
                        warn_messge += dt.Rows[i]["��ƷС��"].ToString() + "-";
                        warn_messge += dt.Rows[i]["ά�ޱ��"].ToString() + "\r\n";
                    }
                    warn_messge += "���쵽��ֹ���ڣ���ע�⣡����";
                }
                
            }
            catch
            {

            }
            con.Close();
        }
    }
}