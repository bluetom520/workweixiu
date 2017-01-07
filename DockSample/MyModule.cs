using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace DockSample
{
    class MyModule
    {



        //ũ������ 
        private static string[] DayName = {"*","��һ","����","����","����","����", 
                                                 "����","����","����","����","��ʮ", 
                                                 "ʮһ","ʮ��","ʮ��","ʮ��","ʮ��", 
                                                 "ʮ��","ʮ��","ʮ��","ʮ��","��ʮ", 
                                                 "إһ","إ��","إ��","إ��","إ��", 
                                                 "إ��","إ��","إ��","إ��","��ʮ"};

        //ũ���·� 
        private static string[] MonthName = { "*", "��", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ", "ʮһ", "��" };

        //�����¼����� 
        private static int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        //ũ������ 
        private static int[] LunarData = {2635,333387,1701,1748,267701,694,2391,133423,1175,396438 
                                             ,3402,3749,331177,1453,694,201326,2350,465197,3221,3402 
                                             ,400202,2901,1386,267611,605,2349,137515,2709,464533,1738 
                                             ,2901,330421,1242,2651,199255,1323,529706,3733,1706,398762 
                                             ,2741,1206,267438,2647,1318,204070,3477,461653,1386,2413 
                                             ,330077,1197,2637,268877,3365,531109,2900,2922,398042,2395 
                                             ,1179,267415,2635,661067,1701,1748,398772,2742,2391,330031 
                                             ,1175,1611,200010,3749,527717,1452,2742,332397,2350,3222 
                                             ,268949,3402,3493,133973,1386,464219,605,2349,334123,2709 
                                             ,2890,267946,2773,592565,1210,2651,395863,1323,2707,265877};

        #region  ��ʱ��ת����ָ���ĸ�ʽ
        /// <summary>
        /// ��ʱ��ת����yyyy-mm-dd��ʽ.
        /// </summary>
        /// <param name="NDate">����</param>
        /// <returns>����String����</returns>
        public string Time_Format(string NDate)
        {
            string sh, sm, se;
            int hh, mm, ss;
            try
            {
                hh = Convert.ToDateTime(NDate).Hour;
                mm = Convert.ToDateTime(NDate).Minute;
                ss = Convert.ToDateTime(NDate).Second;

            }
            catch
            {
                return "";
            }
            sh = Convert.ToString(hh);
            if (sh.Length < 2)
                sh = "0" + sh;
            sm = Convert.ToString(mm);
            if (sm.Length < 2)
                sm = "0" + sm;
            se = Convert.ToString(ss);
            if (se.Length < 2)
                se = "0" + se;
            return sh + sm + se;
        }
        #endregion

        #region  ��StatusStrip�ؼ��е���Ϣ��ӵ�treeView�ؼ���
        /// <summary>
        /// ��ȡ�˵��е���Ϣ.
        /// </summary>
        /// <param name="treeV">TreeView�ؼ�</param>
        /// <param name="MenuS">MenuStrip�ؼ�</param>
        public void GetMenu(TreeView treeV, MenuStrip MenuS)
        {
            for (int i = 0; i < MenuS.Items.Count; i++) //����MenuStrip����е�һ���˵���
            {
                //��һ���˵����������ӵ�TreeView����ĸ��ڵ��У������õ�ǰ�ڵ���ӽڵ�newNode1
                TreeNode newNode1 = treeV.Nodes.Add(MenuS.Items[i].Text);
                //����ǰ�˵�������������Ϣ���뵽ToolStripDropDownItem������
                ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];
                //�жϵ�ǰ�˵������Ƿ��ж����˵���
                if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)
                    for (int j = 0; j < newmenu.DropDownItems.Count; j++)    //���������˵���
                    {
                        
                        if (newmenu.Enabled == true)
                        {
                            //�������˵�������ӵ�TreeView������ӽڵ�newNode1�У������õ�ǰ�ڵ���ӽڵ�newNode2
                            TreeNode newNode2 = newNode1.Nodes.Add(newmenu.DropDownItems[j].Text);
                            //����ǰ�˵�������������Ϣ���뵽ToolStripDropDownItem������
                            ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];
                            //�ж϶����˵������Ƿ��������˵���
                            if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)
                                for (int p = 0; p < newmenu2.DropDownItems.Count; p++)    //���������˵���
                                //�������˵�������ӵ�TreeView������ӽڵ�newNode2��
                                {
                                    if(newmenu2.Enabled==true)
                                    newNode2.Nodes.Add(newmenu2.DropDownItems[p].Text);
                                }
                        }
                    }
            }
        }
        #endregion

        #region  ��TreeView�ؼ�����StatusStrip�ؼ��¸��˵��ĵ����¼�
        /// <summary>
        /// ��TreeView�ؼ�����StatusStrip�ؼ��¸��˵��ĵ����¼�.
        /// </summary>
        /// <param name="MenuS">MenuStrip�ؼ�</param>
        /// <param name="e">TreeView�ؼ���TreeNodeMouseClickEventArgs��</param>
        //public void TreeMenuF(MenuStrip MenuS, TreeNodeMouseClickEventArgs e)
        //{
        //    //MessageBox.Show("111");
        //    string Men = "";
        //    for (int i = 0; i < MenuS.Items.Count; i++) //����MenuStrip�ؼ������˵���
        //    {
        //        Men = ((ToolStripDropDownItem)MenuS.Items[i]).Name; //��ȡ���˵��������
        //        //MessageBox.Show(Men);
        //        if (Men.IndexOf("Menu") == -1)  //���MenuStrip�ؼ��Ĳ˵���û���Ӳ˵�
        //        {
        //            MessageBox.Show("-1");
        //            if (((ToolStripDropDownItem)MenuS.Items[i]).Text == e.Node.Text)    //���ڵ�������˵����������ʱ
        //                if (((ToolStripDropDownItem)MenuS.Items[i]).Enabled == false)   //�жϵ�ǰ�˵����Ƿ����
        //                {
        //                    MessageBox.Show("��ǰ�û���Ȩ�޵���" + "\"" + e.Node.Text + "\"" + "����");
        //                    break;
        //                }
        //                else
        //                    ShowForm(((ToolStripDropDownItem)MenuS.Items[i]).Text.Trim());  //������Ӧ�Ĵ���
        //        }
        //        ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];
        //        if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)    //���������˵���
        //            for (int j = 0; j < newmenu.DropDownItems.Count; j++)
        //            {
                        
        //                Men = newmenu.DropDownItems[j].Name;    //��ȡ�����˵��������
        //                if (Men.IndexOf("Menu") == -1)
        //                {
        //                    if ((newmenu.DropDownItems[j]).Text == e.Node.Text)
        //                        if ((newmenu.DropDownItems[j]).Enabled == false)
        //                        {
        //                            MessageBox.Show("��ǰ�û���Ȩ�޵���" + "\"" + e.Node.Text + "\"" + "����");
        //                            break;
        //                        }
        //                        else
        //                            ShowForm((newmenu.DropDownItems[j]).Text.Trim());
        //                }
        //                ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];
        //                if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)  //���������˵���
        //                    for (int p = 0; p < newmenu2.DropDownItems.Count; p++)
        //                    {
        //                        if ((newmenu2.DropDownItems[p]).Text == e.Node.Text)
        //                            if ((newmenu2.DropDownItems[p]).Enabled == false)
        //                            {
        //                                MessageBox.Show("��ǰ�û���Ȩ�޵���" + "\"" + e.Node.Text + "\"" + "����");
        //                                break;
        //                            }
        //                            else
        //                                ShowForm((newmenu2.DropDownItems[p]).Text.Trim());
        //                    }
        //            }
        //    }

        //}
        #endregion

        //private void ShowForm(string FormName)
        //{
        // //   MessageBox.Show(FormName);
        //    if (FormName == "�����������")
        //    {
        //        Form2.F_Basic_Branch Fbasic = new testi.Form2.F_Basic_Branch("�����������", "Branch", "BranchName");
        //        Fbasic.ShowDialog();
        //        Fbasic.Dispose();
        //    }
        //    if (FormName == "ְ���������")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("ְ���������", "Business", "BusinessName");
        //        Fbasic.ShowDialog();
        //        Fbasic.Dispose();
        //    }
        //    if (FormName == "ְ���������")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("ְ���������", "KindOfRank", "NameOfRank");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "�����������")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("�����������", "KindOfSalary", "NameOfSalary");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "�Ļ��̶��������")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("�Ļ��̶��������", "KindOfKultur", "NameOfKultur");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "Ա��������ʾ")
        //    {
        //        Form2.F_ClewSet FClewSet = new testi.Form2.F_ClewSet("Brithday");
        //        FClewSet.Text = "Ա��������ʾ";
        //        FClewSet.ShowDialog();
        //        FClewSet.Dispose();
        //    }
        //    if (FormName == "Ա����ͬ��ʾ")
        //    {
        //        Form2.F_ClewSet FClewSet = new testi.Form2.F_ClewSet("TimeOfContractEnd");
        //        FClewSet.Text = "Ա����ͬ��ʾ";
        //        FClewSet.ShowDialog();
        //        FClewSet.Dispose();
        //    }
        //    if (FormName == "������Ϣ���")
        //    {
        //        Form2.F1_Index Index = new testi.Form2.F1_Index();
        //        Index.Text = "������Ϣ���";
        //        Index.ShowDialog();
        //        Index.Dispose();
        //    }
        //    if (FormName == "�������ϲ�ѯ")
        //    {
        //        Form2.F1_Find Find = new testi.Form2.F1_Find();
        //        Find.Text = "�������ϲ�ѯ";
        //        Find.ShowDialog();
        //        Find.Dispose();
        //    }
        //    if (FormName == "������Ϣ����")
        //    {
        //        Form2.F1_Input Input = new testi.Form2.F1_Input();
        //        Input.Text = "������Ϣ����";
        //        Input.ShowDialog();
        //        Input.Dispose();
        //    }
        //    if (FormName == "��ԭ/�������ݿ�")
        //    {
        //        Form2.F_HaveBack Fhaveback = new testi.Form2.F_HaveBack();
        //        Fhaveback.Text = "��ԭ/�������ݿ�";
        //        Fhaveback.ShowDialog();
        //        Fhaveback.Close();
        //    }
           
        //    if (FormName == "ϵͳ�˳�")
        //    {
        //        Application.Exit();
        //    }
        //    if (FormName == "�û�����")
        //    {
        //        Form2.F_LoginSet Floginset = new testi.Form2.F_LoginSet();
        //        Floginset.Text = "�û�����";
        //        Floginset.ShowDialog();
        //        Floginset.Dispose();
        //    }
        //    if (FormName == "����/��ԭ���ݿ�")
        //    {
        //        Form2.F_HaveBack Fhaveback = new testi.Form2.F_HaveBack();
        //        Fhaveback.Text = "��ԭ/�������ݿ�";
        //        Fhaveback.ShowDialog();
        //        Fhaveback.Close();
        //    }
        //    if (FormName == "���ڵǼ�")
        //    {
        //        Form2.A_input Ainput = new testi.Form2.A_input();
        //        Ainput.Text = "���ڵǼ�";
        //        Ainput.ShowDialog();
        //        Ainput.Dispose();
        //    }
        //    if (FormName == "����ͳ��")
        //    {
        //        Form2.A_Find Afind = new testi.Form2.A_Find();
        //        Afind.Text = "����ͳ��";
        //        Afind.ShowDialog();
        //        Afind.Dispose();
        //    }
        //    if (FormName == "���ʱ�׼����")
        //    {
        //        Form2.S_Set Sset = new testi.Form2.S_Set();
        //        Sset.Text = "���ʱ�׼����";
        //        Sset.ShowDialog();
        //        Sset.Dispose();
        //    }
        //    if (FormName == "����ͳ��")
        //    {
        //        Form2.S_Statistic Sstatistic = new testi.Form2.S_Statistic();
        //        Sstatistic.Text = "����ͳ��";
        //        Sstatistic.ShowDialog();
        //        Sstatistic.Dispose();
        //    }
            
        //}


       
        
        /// <summary> 
        /// ��ȡ��Ӧ���ڵ�ũ�� 
        /// </summary> 
        /// <param name="dtDay">��������</param> 
        /// <returns></returns> 
        public string GetLunarCalendar(int year,int month,int day)
        {
           

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            //���㵽��ʼʱ��1921��2��8�յ�������1921-2-8(���³�һ) 
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;
            //������ɣ���֧���£��� 
            nIsEnd = 0;
            m = 0;
            k = 0;
            n = 0;
            while (nIsEnd != 1)
            {
                if (LunarData[m] < 4095)
                    k = 11;
                else
                    k = 12;
                n = k;
                while (n >= 0)
                {
                    //��ȡLunarData[m]�ĵ�n��������λ��ֵ 
                    nBit = LunarData[m];
                    for (i = 1; i < n + 1; i++)
                        nBit = nBit / 2;
                    nBit = nBit % 2;
                    if (nTheDate <= (29 + nBit))
                    {
                        nIsEnd = 1;
                        break;
                    }
                    nTheDate = nTheDate - 29 - nBit;
                    n = n - 1;
                }
                if (nIsEnd == 1)
                    break;
                m = m + 1;
            }
            year = 1921 + m;
            month = k - n + 1;
            day = nTheDate;
            //return year + "-" + month + "-" + day;

            if (k == 12)
            {
                if (month == LunarData[m] / 65536 + 1)
                    month = 1 - month;
                else if (month > LunarData[m] / 65536 + 1)
                    month = month - 1;
            }
            ////��
            //calendar = year + "��";
            

            //ũ���� 
            if (month < 1)
                calendar += "��" + MonthName[-1 * month].ToString() + "��";
            else
                calendar += MonthName[month].ToString() + "��";

            //ũ���� 
            calendar += DayName[day].ToString() ;

            return calendar;

        }
        public string Holyday(int year, int month, int day)
        {
            string LunarCalendar = GetLunarCalendar(year, month, day);
            switch(LunarCalendar)
            {
                case "������ʮ": return "����";
                case "���³�һ": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "���³���": return "����";
                case "����ʮ��": return "����";
                
                default: break;
                
            }
            if (month == 5 && (day == 1 || day == 2 || day == 3))
                return "�Ͷ���";
            else if (month == 10 && (day == 1 || day == 2 || day == 3 || day == 4 || day == 5 || day == 6 || day == 7))
                return "�����";
            else if (month == 1 && day == 1)
                return "Ԫ��";
            else
            {

                string NowDate = year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " 0:00:00";
                DateTime DT = Convert.ToDateTime(NowDate);
                if (DT.DayOfWeek.ToString() == "Saturday" || DT.DayOfWeek.ToString() == "Sunday")
                    return "˫����";
            }
            return "������";
        }


        public float StringToFloat(string str)
        {
            if (str.Length == 0)
                return 0.0f;
            char[] Array = str.ToCharArray();
            float Integer=0,Fraction=0;
            int n = 0;
            for (; n < str.Length&&Array[n]!='.';n++)
                Integer=Integer*10+Array[n]-'0';
            
            n++;
            for(float m=1.0f;n < str.Length;n++)
            {
                m = m * 10;
                Fraction = Fraction + (Array[n] - '0') / m;
            }
            
            return Integer + Fraction;
 
        }
        #region ���㽫���ջ��ͬ������ǰpreday��������CountDate

        public  string CountDate(string Birthday, int preday)
        {
            Char[] charBirthday = Birthday.ToCharArray();
            int year = 0, month = 0, day = 0;
            int temp = 0;
            string predate;

            for (int n = 0; n < Birthday.Length; n++)
            {
                if (charBirthday[n] >= '0' && charBirthday[n] <= '9')
                {
                    temp = temp * 10 + charBirthday[n] - '0';
                }
                else if (Birthday[n] == '��')
                {
                    year = temp;
                    temp = 0;
                }
                else if (Birthday[n] == '��')
                {
                    month = temp;
                    temp = 0;
                }
                else if (Birthday[n] == '��')
                {
                    day = temp;
                    temp = 0;
                }
            }
            //MessageBox.Show(year.ToString());
            //MessageBox.Show(month.ToString());
            //MessageBox.Show(day.ToString());

            int[] everymonth ={ 0, 31, 30, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            everymonth[2] = (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) ? 30 : 29;//ÿ�����������

            //������ǰ�����¼�������
            if (preday < day)
                day = day - preday;
            else
            {
                month--;
                if (month <= 0)
                {
                    year--;
                    month = month + 12;
                }
                preday = preday - day;

                for (; everymonth[month] < preday; )
                {
                    preday = preday - everymonth[month];
                    month--;
                    if (month <= 0)
                    {
                        year--;
                        month = month + 12;
                    }

                }
                day = everymonth[month] - preday;

            }
            predate = year.ToString() + "��" + month.ToString() + "��" + day.ToString() + "��";
            //MessageBox.Show(predate);
            return predate;

        }
        #endregion

        public string DateToMonth(string Date)
        {
            string Month="";
            string Temp = Date.Substring(5, 2);
            char[] charTemp = Temp.ToCharArray();
            for (int n = 0; n < 2 && (charTemp[n] > '0' && charTemp[n] <= '9'); n++)
            {
                Month = Month + charTemp[n].ToString();

            }
            return Month;
        }

        public int StringArrayIndexOf(string[] ArrayOfString,string str)
        {
            
            int m;
            for (m = 0; m < ArrayOfString.Length; m++)
            {
                try
                {
                    if (ArrayOfString[m] == str)
                        break;
                }
                catch
                {
                    return -1;
                }



            }
            if (m == ArrayOfString.Length)
                return -1;
            return m;
 
        }
        public string DateToDay(string Date)
        {
            string Day = "";
            
            char[] charTemp = Date.ToCharArray();
            int n=Date.IndexOf('��');
            
                    Day = Day + charTemp[n + 1].ToString();
                    if (charTemp[n + 2] != '��')
                        Day = Day + charTemp[n + 2].ToString();
    
            return Day;
        }

        public bool ExportDataGridview(DataGridView gridView, bool isShowExcele)
        {
            if (gridView.Rows.Count == 0)
            {
                return false;
            }
            //����Excel����
            
                Excel.Application excel = new Excel.Application();
            
            excel.Application.Workbooks.Add(true);
            excel.Visible = isShowExcele;
            int rowCount = gridView.Rows.Count;
            int colCount = gridView.Columns.Count;
            object[,] dataArray = new object[rowCount, colCount];
            for (int i = 0; i < gridView.ColumnCount; i++)
            {
                dataArray[0, i] = gridView.Columns[i].HeaderText;
            }
            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (gridView[j, i].Value.GetType() == typeof(string))
                    {
                        dataArray[i + 1, j] = "'" + gridView[j, i].Value.ToString();
                    }
                    else
                    {
                        dataArray[i + 1, j] = gridView[j, i].Value.ToString();
                    }
                }
            }
            excel.get_Range("A1", excel.Cells[rowCount, colCount]).Value2 = dataArray;
            excel.Cells.EntireColumn.AutoFit();
            return true;
        }
        public string[] SubOfStringArray(string[] A, string[] B)
        {
            string[] c=new string[A.Length>B.Length?A.Length:B.Length];
            int n=0;
            foreach (string str in A)
            {
                if (StringArrayIndexOf(B, str) == -1)
                    c[n++] = str;

            }
            return c;
        }

    }
}
