

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WZTB
{
    internal class ChineseCalendarInfo
    {
        private DateTime m_SolarDate;
        private int m_LunarYear, m_LunarMonth, m_LunarDay;
        private bool m_IsLeapMonth = false;
        private string m_LunarYearSexagenary = null, m_LunarYearAnimal = null;
        private string m_LunarYearText = null, m_LunarMonthText = null, m_LunarDayText = null;
        private string m_SolarWeekText = null, m_SolarConstellation = null, m_SolarBirthStone = null;

        #region ���캯��

        public ChineseCalendarInfo()
            : this(DateTime.Now.Date)
        {

        }

        /// <summary>
        /// ��ָ�����������ڴ����й�������Ϣʵ����
        /// </summary>
        /// <param name="date">ָ������������</param>
        public ChineseCalendarInfo(DateTime date)
        {
            m_SolarDate = date;
            LoadFromSolarDate();
        }

        private void LoadFromSolarDate()
        {
            m_IsLeapMonth = false;
            m_LunarYearSexagenary = null;
            m_LunarYearAnimal = null;
            m_LunarYearText = null;
            m_LunarMonthText = null;
            m_LunarDayText = null;
            m_SolarWeekText = null;
            m_SolarConstellation = null;
            m_SolarBirthStone = null;

            m_LunarYear = calendar.GetYear(m_SolarDate);
            m_LunarMonth = calendar.GetMonth(m_SolarDate);
            int leapMonth = calendar.GetLeapMonth(m_LunarYear);

            if (leapMonth == m_LunarMonth)
            {
                m_IsLeapMonth = true;
                m_LunarMonth -= 1;
            }
            else if (leapMonth > 0 && leapMonth < m_LunarMonth)
            {
                m_LunarMonth -= 1;
            }

            m_LunarDay = calendar.GetDayOfMonth(m_SolarDate);

            CalcConstellation(m_SolarDate, out m_SolarConstellation, out m_SolarBirthStone);
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime SolarDate
        {
            get { return m_SolarDate; }
            set
            {
                if (m_SolarDate.Equals(value))
                    return;
                m_SolarDate = value;
                LoadFromSolarDate();
            }
        }
        /// <summary>
        /// ���ڼ�
        /// </summary>
        public string SolarWeekText
        {
            get
            {
                if (string.IsNullOrEmpty(m_SolarWeekText))
                {
                    int i = (int)m_SolarDate.DayOfWeek;
                    m_SolarWeekText = ChineseWeekName[i];
                }
                return m_SolarWeekText;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string SolarConstellation
        {
            get { return m_SolarConstellation; }
        }
        /// <summary>
        /// ��������ʯ
        /// </summary>
        public string SolarBirthStone
        {
            get { return m_SolarBirthStone; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public int LunarYear
        {
            get { return m_LunarYear; }
        }
        /// <summary>
        /// �����·�
        /// </summary>
        public int LunarMonth
        {
            get { return m_LunarMonth; }
        }
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsLeapMonth
        {
            get { return m_IsLeapMonth; }
        }
        /// <summary>
        /// ������������
        /// </summary>
        public int LunarDay
        {
            get { return m_LunarDay; }
        }

        /// <summary>
        /// �������֧
        /// </summary>
        public string LunarYearSexagenary
        {
            get
            {
                if (string.IsNullOrEmpty(m_LunarYearSexagenary))
                {
                    int y = calendar.GetSexagenaryYear(this.SolarDate);
                    m_LunarYearSexagenary = CelestialStem.Substring((y - 1) % 10, 1) + TerrestrialBranch.Substring((y - 1) % 12, 1);
                }
                return m_LunarYearSexagenary;
            }
        }
        /// <summary>
        /// ��������Ф
        /// </summary>
        public string LunarYearAnimal
        {
            get
            {
                if (string.IsNullOrEmpty(m_LunarYearAnimal))
                {
                    int y = calendar.GetSexagenaryYear(this.SolarDate);
                    m_LunarYearAnimal = Animals.Substring((y - 1) % 12, 1);
                }
                return m_LunarYearAnimal;
            }
        }


        /// <summary>
        /// �������ı�
        /// </summary>
        public string LunarYearText
        {
            get
            {
                if (string.IsNullOrEmpty(m_LunarYearText))
                {
                    m_LunarYearText = Animals.Substring(calendar.GetSexagenaryYear(new DateTime(m_LunarYear, 1, 1)) % 12 - 1, 1);
                    StringBuilder sb = new StringBuilder();
                    int year = this.LunarYear;
                    int d;
                    do
                    {
                        d = year % 10;
                        sb.Insert(0, ChineseNumber[d]);
                        year = year / 10;
                    } while (year > 0);
                    m_LunarYearText = sb.ToString();
                }
                return m_LunarYearText;
            }
        }
        /// <summary>
        /// �������ı�
        /// </summary>
        public string LunarMonthText
        {
            get
            {
                if (string.IsNullOrEmpty(m_LunarMonthText))
                {
                    m_LunarMonthText = (this.IsLeapMonth ? "��" : "") + ChineseMonthName[this.LunarMonth - 1];
                }
                return m_LunarMonthText;
            }
        }

        /// <summary>
        /// �������������ı�
        /// </summary>
        public string LunarDayText
        {
            get
            {
                if (string.IsNullOrEmpty(m_LunarDayText))
                    m_LunarDayText = ChineseDayName[this.LunarDay - 1];
                return m_LunarDayText;
            }
        }

        #endregion

        /// <summary>
        /// ����ָ���������ڼ�������������ʯ
        /// </summary>
        /// <param name="date">ָ����������</param>
        /// <param name="constellation">����</param>
        /// <param name="birthstone">����ʯ</param>
        public static void CalcConstellation(DateTime date, out string constellation, out string birthstone)
        {
            int i = Convert.ToInt32(date.ToString("MMdd"));
            int j;
            if (i >= 321 && i <= 419)
                j = 0;
            else if (i >= 420 && i <= 520)
                j = 1;
            else if (i >= 521 && i <= 621)
                j = 2;
            else if (i >= 622 && i <= 722)
                j = 3;
            else if (i >= 723 && i <= 822)
                j = 4;
            else if (i >= 823 && i <= 922)
                j = 5;
            else if (i >= 923 && i <= 1023)
                j = 6;
            else if (i >= 1024 && i <= 1121)
                j = 7;
            else if (i >= 1122 && i <= 1221)
                j = 8;
            else if (i >= 1222 || i <= 119)
                j = 9;
            else if (i >= 120 && i <= 218)
                j = 10;
            else if (i >= 219 && i <= 320)
                j = 11;
            else
            {
                constellation = "δ֪����";
                birthstone = "δ֪����ʯ";
                return;
            }
            constellation = Constellations[j];
            birthstone = BirthStones[j];

            #region ��������
            //��������   3��21��------4��19��     ����ʯ��   ��ʯ   
            //��ţ����   4��20��------5��20��   ����ʯ��   ����ʯ   
            //˫������   5��21��------6��21��     ����ʯ��   ���   
            //��з����   6��22��------7��22��   ����ʯ��   ����   
            //ʨ������   7��23��------8��22��   ����ʯ��   �챦ʯ   
            //��Ů����   8��23��------9��22��   ����ʯ��   ���������   
            //�������   9��23��------10��23��     ����ʯ��   ����ʯ   
            //��Ы����   10��24��-----11��21��     ����ʯ��   è��ʯ   
            //��������   11��22��-----12��21��   ����ʯ��   �Ʊ�ʯ   
            //Ħ������   12��22��-----1��19��   ����ʯ��   ��������   
            //ˮƿ����   1��20��-----2��18��   ����ʯ��   ��ˮ��   
            //˫������   2��19��------3��20��   ����ʯ��   �³�ʯ��Ѫʯ  
            #endregion
        }


        #region ����ת����

        /// <summary>
        /// ��ȡָ����ݴ��ڵ��գ����³�һ������������
        /// </summary>
        /// <param name="year">ָ�������</param>
        private static DateTime GetLunarNewYearDate(int year)
        {
            DateTime dt = new DateTime(year, 1, 1);
            int cnYear = calendar.GetYear(dt);
            int cnMonth = calendar.GetMonth(dt);

            int num1 = 0;
            int num2 = calendar.IsLeapYear(cnYear) ? 13 : 12;

            while (num2 >= cnMonth)
            {
                num1 += calendar.GetDaysInMonth(cnYear, num2--);
            }

            num1 = num1 - calendar.GetDayOfMonth(dt) + 1;
            return dt.AddDays(num1);
        }

        /// <summary>
        /// ����ת����
        /// </summary>
        /// <param name="year">������</param>
        /// <param name="month">������</param>
        /// <param name="day">������</param>
        /// <param name="IsLeapMonth">�Ƿ�����</param>
        public static DateTime GetDateFromLunarDate(int year, int month, int day, bool IsLeapMonth)
        {
            if (year < 1902 || year > 2100)
                throw new Exception("ֻ֧��1902��2100�ڼ��ũ����");
            if (month < 1 || month > 12)
                throw new Exception("��ʾ�·ݵ����ֱ�����1��12֮��");

            if (day < 1 || day > calendar.GetDaysInMonth(year, month))
                throw new Exception("ũ��������������");

            int num1 = 0, num2 = 0;
            int leapMonth = calendar.GetLeapMonth(year);

            if (((leapMonth == month + 1) && IsLeapMonth) || (leapMonth > 0 && leapMonth <= month))
                num2 = month;
            else
                num2 = month - 1;

            while (num2 > 0)
            {
                num1 += calendar.GetDaysInMonth(year, num2--);
            }

            DateTime dt = GetLunarNewYearDate(year);
            return dt.AddDays(num1 + day - 1);
        }

        /// <summary>
        /// ����ת����
        /// </summary>
        /// <param name="date">��������</param>
        /// <param name="IsLeapMonth">�Ƿ�����</param>
        public static DateTime GetDateFromLunarDate(DateTime date, bool IsLeapMonth)
        {
            return GetDateFromLunarDate(date.Year, date.Month, date.Day, IsLeapMonth);
        }

        #endregion

        #region ��������������

        /// <summary>
        /// ��������������ʵ��
        /// </summary>
        /// <param name="year">������</param>
        /// <param name="month">������</param>
        /// <param name="day">������</param>
        /// <param name="IsLeapMonth">�Ƿ�����</param>
        public static ChineseCalendarInfo FromLunarDate(int year, int month, int day, bool IsLeapMonth)
        {
            DateTime dt = GetDateFromLunarDate(year, month, day, IsLeapMonth);
            return new ChineseCalendarInfo(dt);
        }
        /// <summary>
        /// ��������������ʵ��
        /// </summary>
        /// <param name="date">��������</param>
        /// <param name="IsLeapMonth">�Ƿ�����</param>
        public static ChineseCalendarInfo FromLunarDate(DateTime date, bool IsLeapMonth)
        {
            return FromLunarDate(date.Year, date.Month, date.Day, IsLeapMonth);
        }

        /// <summary>
        /// ��������������ʵ��
        /// </summary>
        /// <param name="date">��ʾ�������ڵ�8λ���֣����磺20070209</param>
        /// <param name="IsLeapMonth">�Ƿ�����</param>
        public static ChineseCalendarInfo FromLunarDate(string date, bool IsLeapMonth)
        {
            Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{7}(\d)$");
            Match mc = rg.Match(date);
            if (!mc.Success)
            {
                throw new Exception("�����ַ�����������");
            }
            DateTime dt = DateTime.Parse(string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2)));
            return FromLunarDate(dt, IsLeapMonth);
        }


        #endregion

        private static ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
        public const string ChineseNumber = "��һ�����������߰˾�";
        public const string CelestialStem = "���ұ����켺�����ɹ�";
        public const string TerrestrialBranch = "�ӳ���î������δ�����纥";
        public const string Animals = "��ţ������������Ｆ����";
        public static readonly string[] ChineseWeekName = new string[] { "������", "����һ", "���ڶ�", "������", "������", "������", "������" };
        public static readonly string[] ChineseDayName = new string[] {
            "��һ","����","����","����","����","����","����","����","����","��ʮ",
            "ʮһ","ʮ��","ʮ��","ʮ��","ʮ��","ʮ��","ʮ��","ʮ��","ʮ��","��ʮ",
            "إһ","إ��","إ��","إ��","إ��","إ��","إ��","إ��","إ��","��ʮ"};
        public static readonly string[] ChineseMonthName = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ", "ʮһ", "ʮ��" };
        public static readonly string[] Constellations = new string[] { "������", "��ţ��", "˫����", "��з��", "ʨ����", "��Ů��", "�����", "��Ы��", "������", "Ħ����", "ˮƿ��", "˫����" };
        public static readonly string[] BirthStones = new string[] { "��ʯ", "����ʯ", "���", "����", "�챦ʯ", "���������", "����ʯ", "è��ʯ", "�Ʊ�ʯ", "��������", "��ˮ��", "�³�ʯ��Ѫʯ" };
    }
}
