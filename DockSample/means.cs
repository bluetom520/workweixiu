using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DockSample
{
    class means
    {
        #region  ȫ�ֱ���


        public static string M_str_sqlcon = MainForm.connetstring;
        //public static int Login_n = 0; //�û���¼�����µ�¼�ı�ʶ
        //public static int  Login_ID = 0; //����ȫ�ֱ�������¼��ǰ��¼���û����
        
        //public static string Login_Name = "";  //����ȫ�ֱ�������¼��ǰ��¼���û���
        public static string Mean_SQL = "", Mean_Table = "", Mean_Field = "";  //����ȫ�ֱ�������¼��������Ϣ���������еı�����SQL���
        public static SqlConnection My_con;  //����һ��SqlConnection���͵Ĺ�������My_con�������ж����ݿ��Ƿ����ӳɹ�
      
        //public static int res = 0;
        #endregion

        #region  �������ݿ�����
        /// <summary>
        /// �������ݿ�����.
        /// </summary>
        /// <returns>����SqlConnection����</returns>
        public static SqlConnection getcon()
        {
            My_con = new SqlConnection(M_str_sqlcon);   //��SqlConnection������ָ�������ݿ�������
            My_con.Open();  //�����ݿ�����
            return My_con;  //����SqlConnection�������Ϣ
        }
        #endregion

        #region  �������ݿ��Ƿ񸳼�
        /// <summary>
        /// �������ݿ��Ƿ񸳼�
        /// </summary>
        public void con_open()
        {
            getcon();
            //con_close();
        }
        #endregion


        #region  �ر����ݿ�����
        /// <summary>
        /// �ر������ݿ������.
        /// </summary>
        public void con_close()
        {
            if (My_con.State == ConnectionState.Open)   //�ж��Ƿ�������ݿ������
            {
                My_con.Close();   //�ر����ݿ������
                My_con.Dispose();   //�ͷ�My_con���������пռ�
            }
        }
        #endregion

        #region  ��ȡָ�����е���Ϣ
        /// <summary>
        /// ��ȡָ�����е���Ϣ.
        /// </summary>
        /// <param name="SQLstr">SQL���</param>
        /// <returns>����bool��</returns>
        public SqlDataReader getcom(string SQLstr)
        {
            getcon();   //�������ݿ������
            SqlCommand My_com = My_con.CreateCommand(); //����һ��SqlCommand��������ִ��SQL���
            My_com.CommandText = SQLstr;    //��ȡָ����SQL���
            SqlDataReader My_read = My_com.ExecuteReader(); //ִ��SQL�����䣬����һ��SqlDataReader����
            return My_read;
        }
        #endregion

        #region ִ��SqlCommand����
        /// <summary>
        /// ִ��SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL���</param>
        public void getsqlcom(string SQLstr)
        {
            getcon();   //�������ݿ������
            SqlCommand SQLcom = new SqlCommand(SQLstr, My_con); //����һ��SqlCommand��������ִ��SQL���
            SQLcom.ExecuteNonQuery();   //ִ��SQL���
            SQLcom.Dispose();   //�ͷ����пռ�
            con_close();    //����con_close()�������ر������ݿ������
        }
        #endregion

        public int  getsqlcomAndrowsNo(string SQLstr)
        {
            getcon();   //�������ݿ������
            SqlCommand SQLcom = new SqlCommand(SQLstr, My_con); //����һ��SqlCommand��������ִ��SQL���
            int n=SQLcom.ExecuteNonQuery();   //ִ��SQL���
            SQLcom.Dispose();   //�ͷ����пռ�
            con_close();    //����con_close()�������ر������ݿ������
            return n;
        }
  

        #region  ����DataSet����
        /// <summary>
        /// ����һ��DataSet����
        /// </summary>
        /// <param name="M_str_sqlstr">SQL���</param>
        /// <param name="M_str_table">����</param>
        /// <returns>����DataSet����</returns>
        public DataSet getDataSet(string SQLstr, string tableName)
        {
            getcon();   //�������ݿ������
            SqlDataAdapter SQLda = new SqlDataAdapter(SQLstr, My_con);  //����һ��SqlDataAdapter���󣬲���ȡָ�����ݱ����Ϣ
            DataSet My_DataSet = new DataSet(); //����DataSet����
            SQLda.Fill(My_DataSet, tableName);  //ͨ��SqlDataAdapter�����Fill()�����������ݱ���Ϣ��ӵ�DataSet������
            con_close();    //�ر����ݿ������
            return My_DataSet;  //����DataSet�������Ϣ

            //WritePrivateProfileString(string section, string key, string val, string filePath);
        }
        #endregion





    }
}
