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



        //农历日期 
        private static string[] DayName = {"*","初一","初二","初三","初四","初五", 
                                                 "初六","初七","初八","初九","初十", 
                                                 "十一","十二","十三","十四","十五", 
                                                 "十六","十七","十八","十九","二十", 
                                                 "廿一","廿二","廿三","廿四","廿五", 
                                                 "廿六","廿七","廿八","廿九","三十"};

        //农历月份 
        private static string[] MonthName = { "*", "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

        //公历月计数天 
        private static int[] MonthAdd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        //农历数据 
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

        #region  将时间转换成指定的格式
        /// <summary>
        /// 将时间转换成yyyy-mm-dd格式.
        /// </summary>
        /// <param name="NDate">日期</param>
        /// <returns>返回String对象</returns>
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

        #region  将StatusStrip控件中的信息添加到treeView控件中
        /// <summary>
        /// 读取菜单中的信息.
        /// </summary>
        /// <param name="treeV">TreeView控件</param>
        /// <param name="MenuS">MenuStrip控件</param>
        public void GetMenu(TreeView treeV, MenuStrip MenuS)
        {
            for (int i = 0; i < MenuS.Items.Count; i++) //遍历MenuStrip组件中的一级菜单项
            {
                //将一级菜单项的名称添加到TreeView组件的根节点中，并设置当前节点的子节点newNode1
                TreeNode newNode1 = treeV.Nodes.Add(MenuS.Items[i].Text);
                //将当前菜单项的所有相关信息存入到ToolStripDropDownItem对象中
                ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];
                //判断当前菜单项中是否有二级菜单项
                if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)
                    for (int j = 0; j < newmenu.DropDownItems.Count; j++)    //遍历二级菜单项
                    {
                        
                        if (newmenu.Enabled == true)
                        {
                            //将二级菜单名称添加到TreeView组件的子节点newNode1中，并设置当前节点的子节点newNode2
                            TreeNode newNode2 = newNode1.Nodes.Add(newmenu.DropDownItems[j].Text);
                            //将当前菜单项的所有相关信息存入到ToolStripDropDownItem对象中
                            ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];
                            //判断二级菜单项中是否有三级菜单项
                            if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)
                                for (int p = 0; p < newmenu2.DropDownItems.Count; p++)    //遍历三级菜单项
                                //将三级菜单名称添加到TreeView组件的子节点newNode2中
                                {
                                    if(newmenu2.Enabled==true)
                                    newNode2.Nodes.Add(newmenu2.DropDownItems[p].Text);
                                }
                        }
                    }
            }
        }
        #endregion

        #region  用TreeView控件调用StatusStrip控件下各菜单的单击事件
        /// <summary>
        /// 用TreeView控件调用StatusStrip控件下各菜单的单击事件.
        /// </summary>
        /// <param name="MenuS">MenuStrip控件</param>
        /// <param name="e">TreeView控件的TreeNodeMouseClickEventArgs类</param>
        //public void TreeMenuF(MenuStrip MenuS, TreeNodeMouseClickEventArgs e)
        //{
        //    //MessageBox.Show("111");
        //    string Men = "";
        //    for (int i = 0; i < MenuS.Items.Count; i++) //遍历MenuStrip控件中主菜单项
        //    {
        //        Men = ((ToolStripDropDownItem)MenuS.Items[i]).Name; //获取主菜单项的名称
        //        //MessageBox.Show(Men);
        //        if (Men.IndexOf("Menu") == -1)  //如果MenuStrip控件的菜单项没有子菜单
        //        {
        //            MessageBox.Show("-1");
        //            if (((ToolStripDropDownItem)MenuS.Items[i]).Text == e.Node.Text)    //当节点名称与菜单项名称相等时
        //                if (((ToolStripDropDownItem)MenuS.Items[i]).Enabled == false)   //判断当前菜单项是否可用
        //                {
        //                    MessageBox.Show("当前用户无权限调用" + "\"" + e.Node.Text + "\"" + "窗体");
        //                    break;
        //                }
        //                else
        //                    ShowForm(((ToolStripDropDownItem)MenuS.Items[i]).Text.Trim());  //调用相应的窗体
        //        }
        //        ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];
        //        if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)    //遍历二级菜单项
        //            for (int j = 0; j < newmenu.DropDownItems.Count; j++)
        //            {
                        
        //                Men = newmenu.DropDownItems[j].Name;    //获取二级菜单项的名称
        //                if (Men.IndexOf("Menu") == -1)
        //                {
        //                    if ((newmenu.DropDownItems[j]).Text == e.Node.Text)
        //                        if ((newmenu.DropDownItems[j]).Enabled == false)
        //                        {
        //                            MessageBox.Show("当前用户无权限调用" + "\"" + e.Node.Text + "\"" + "窗体");
        //                            break;
        //                        }
        //                        else
        //                            ShowForm((newmenu.DropDownItems[j]).Text.Trim());
        //                }
        //                ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];
        //                if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)  //遍历三级菜单项
        //                    for (int p = 0; p < newmenu2.DropDownItems.Count; p++)
        //                    {
        //                        if ((newmenu2.DropDownItems[p]).Text == e.Node.Text)
        //                            if ((newmenu2.DropDownItems[p]).Enabled == false)
        //                            {
        //                                MessageBox.Show("当前用户无权限调用" + "\"" + e.Node.Text + "\"" + "窗体");
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
        //    if (FormName == "部门类别设置")
        //    {
        //        Form2.F_Basic_Branch Fbasic = new testi.Form2.F_Basic_Branch("部门类别设置", "Branch", "BranchName");
        //        Fbasic.ShowDialog();
        //        Fbasic.Dispose();
        //    }
        //    if (FormName == "职务类别设置")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("职务类别设置", "Business", "BusinessName");
        //        Fbasic.ShowDialog();
        //        Fbasic.Dispose();
        //    }
        //    if (FormName == "职称类别设置")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("职称类别设置", "KindOfRank", "NameOfRank");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "工资类别设置")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("工资类别设置", "KindOfSalary", "NameOfSalary");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "文化程度类别设置")
        //    {
        //        Form2.F_Basic Fbasic = new testi.Form2.F_Basic("文化程度类别设置", "KindOfKultur", "NameOfKultur");
        //    Fbasic.ShowDialog();
        //    Fbasic.Dispose();
        //    }
        //    if (FormName == "员工生日提示")
        //    {
        //        Form2.F_ClewSet FClewSet = new testi.Form2.F_ClewSet("Brithday");
        //        FClewSet.Text = "员工生日提示";
        //        FClewSet.ShowDialog();
        //        FClewSet.Dispose();
        //    }
        //    if (FormName == "员工合同提示")
        //    {
        //        Form2.F_ClewSet FClewSet = new testi.Form2.F_ClewSet("TimeOfContractEnd");
        //        FClewSet.Text = "员工合同提示";
        //        FClewSet.ShowDialog();
        //        FClewSet.Dispose();
        //    }
        //    if (FormName == "人事信息浏览")
        //    {
        //        Form2.F1_Index Index = new testi.Form2.F1_Index();
        //        Index.Text = "人事信息浏览";
        //        Index.ShowDialog();
        //        Index.Dispose();
        //    }
        //    if (FormName == "人事资料查询")
        //    {
        //        Form2.F1_Find Find = new testi.Form2.F1_Find();
        //        Find.Text = "人事资料查询";
        //        Find.ShowDialog();
        //        Find.Dispose();
        //    }
        //    if (FormName == "人事信息管理")
        //    {
        //        Form2.F1_Input Input = new testi.Form2.F1_Input();
        //        Input.Text = "人事信息管理";
        //        Input.ShowDialog();
        //        Input.Dispose();
        //    }
        //    if (FormName == "还原/备份数据库")
        //    {
        //        Form2.F_HaveBack Fhaveback = new testi.Form2.F_HaveBack();
        //        Fhaveback.Text = "还原/备份数据库";
        //        Fhaveback.ShowDialog();
        //        Fhaveback.Close();
        //    }
           
        //    if (FormName == "系统退出")
        //    {
        //        Application.Exit();
        //    }
        //    if (FormName == "用户管理")
        //    {
        //        Form2.F_LoginSet Floginset = new testi.Form2.F_LoginSet();
        //        Floginset.Text = "用户管理";
        //        Floginset.ShowDialog();
        //        Floginset.Dispose();
        //    }
        //    if (FormName == "备份/还原数据库")
        //    {
        //        Form2.F_HaveBack Fhaveback = new testi.Form2.F_HaveBack();
        //        Fhaveback.Text = "还原/备份数据库";
        //        Fhaveback.ShowDialog();
        //        Fhaveback.Close();
        //    }
        //    if (FormName == "考勤登记")
        //    {
        //        Form2.A_input Ainput = new testi.Form2.A_input();
        //        Ainput.Text = "考勤登记";
        //        Ainput.ShowDialog();
        //        Ainput.Dispose();
        //    }
        //    if (FormName == "考勤统计")
        //    {
        //        Form2.A_Find Afind = new testi.Form2.A_Find();
        //        Afind.Text = "考勤统计";
        //        Afind.ShowDialog();
        //        Afind.Dispose();
        //    }
        //    if (FormName == "工资标准设置")
        //    {
        //        Form2.S_Set Sset = new testi.Form2.S_Set();
        //        Sset.Text = "工资标准设置";
        //        Sset.ShowDialog();
        //        Sset.Dispose();
        //    }
        //    if (FormName == "工资统计")
        //    {
        //        Form2.S_Statistic Sstatistic = new testi.Form2.S_Statistic();
        //        Sstatistic.Text = "工资统计";
        //        Sstatistic.ShowDialog();
        //        Sstatistic.Dispose();
        //    }
            
        //}


       
        
        /// <summary> 
        /// 获取对应日期的农历 
        /// </summary> 
        /// <param name="dtDay">公历日期</param> 
        /// <returns></returns> 
        public string GetLunarCalendar(int year,int month,int day)
        {
           

            int nTheDate;
            int nIsEnd;
            int k, m, n, nBit, i;
            string calendar = string.Empty;
            //计算到初始时间1921年2月8日的天数：1921-2-8(正月初一) 
            nTheDate = (year - 1921) * 365 + (year - 1921) / 4 + day + MonthAdd[month - 1] - 38;
            if ((year % 4 == 0) && (month > 2))
                nTheDate += 1;
            //计算天干，地支，月，日 
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
                    //获取LunarData[m]的第n个二进制位的值 
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
            ////年
            //calendar = year + "年";
            

            //农历月 
            if (month < 1)
                calendar += "闰" + MonthName[-1 * month].ToString() + "月";
            else
                calendar += MonthName[month].ToString() + "月";

            //农历日 
            calendar += DayName[day].ToString() ;

            return calendar;

        }
        public string Holyday(int year, int month, int day)
        {
            string LunarCalendar = GetLunarCalendar(year, month, day);
            switch(LunarCalendar)
            {
                case "腊月三十": return "春节";
                case "正月初一": return "春节";
                case "正月初二": return "春节";
                case "正月初三": return "春节";
                case "正月初四": return "春节";
                case "正月初五": return "春节";
                case "正月初六": return "春节";
                case "三月初九": return "清明";
                case "五月初五": return "端午";
                case "八月十五": return "中秋";
                
                default: break;
                
            }
            if (month == 5 && (day == 1 || day == 2 || day == 3))
                return "劳动节";
            else if (month == 10 && (day == 1 || day == 2 || day == 3 || day == 4 || day == 5 || day == 6 || day == 7))
                return "国庆节";
            else if (month == 1 && day == 1)
                return "元旦";
            else
            {

                string NowDate = year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " 0:00:00";
                DateTime DT = Convert.ToDateTime(NowDate);
                if (DT.DayOfWeek.ToString() == "Saturday" || DT.DayOfWeek.ToString() == "Sunday")
                    return "双休日";
            }
            return "工作日";
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
        #region 计算将生日或合同日期提前preday天后的日期CountDate

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
                else if (Birthday[n] == '年')
                {
                    year = temp;
                    temp = 0;
                }
                else if (Birthday[n] == '月')
                {
                    month = temp;
                    temp = 0;
                }
                else if (Birthday[n] == '日')
                {
                    day = temp;
                    temp = 0;
                }
            }
            //MessageBox.Show(year.ToString());
            //MessageBox.Show(month.ToString());
            //MessageBox.Show(day.ToString());

            int[] everymonth ={ 0, 31, 30, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            everymonth[2] = (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) ? 30 : 29;//每个月里的天数

            //计算提前到几月几号提醒
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
            predate = year.ToString() + "年" + month.ToString() + "月" + day.ToString() + "日";
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
            int n=Date.IndexOf('月');
            
                    Day = Day + charTemp[n + 1].ToString();
                    if (charTemp[n + 2] != '日')
                        Day = Day + charTemp[n + 2].ToString();
    
            return Day;
        }

        public bool ExportDataGridview(DataGridView gridView, bool isShowExcele)
        {
            if (gridView.Rows.Count == 0)
            {
                return false;
            }
            //建立Excel对象
            
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
