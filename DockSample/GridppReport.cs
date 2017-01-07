using System;
using grproLib;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace GridppReportDemo
{
    /// <summary>
    /// GridppReport ��ժҪ˵����
    /// </summary>
    public class Utility
    {
        //public const string GetDatabaseConnectionString()  = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\..\..\Data\Northwind.mdb";

        //�˺�������ע��Grid++Report������������Ӧ�ó�������ʱ���ô˺���
        //�����Լ������кŴ���"AAAAAAA"��"AAAAAAA"��һ����Ч�����к�
        public static void RegisterGridppReport()
        {
            GridppReport TempGridppReport = new GridppReport();
            bool Succeeded = TempGridppReport.Register("AAAAAAA");
            if (!Succeeded)
                System.Windows.Forms.MessageBox.Show("Register Grid++Report Failed, Grid++Report will run in trial mode.", "Register"
                    , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        private struct MatchFieldPairType
        {
            public IGRField grField;
            public int MatchColumnIndex;
        }

        // �� DataReader ������ת���� Grid++Report �����ݼ���
        public static void FillRecordToReport(IGridppReport Report, IDataReader dr)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dr.FieldCount)];

            //�����ֶ������������ƽ���ƥ�䣬����DataReader�ֶ���Grid++Report��¼�����ֶ�֮��Ķ�Ӧ��ϵ
            int MatchFieldCount = 0;
            for (int i = 0; i < dr.FieldCount; ++i)
            {
                foreach (IGRField fld in Report.DetailGrid.Recordset.Fields)
                {
                    if (String.Compare(fld.RunningDBField, dr.GetName(i), true) == 0)
                    {
                        MatchFieldPairs[MatchFieldCount].grField = fld;
                        MatchFieldPairs[MatchFieldCount].MatchColumnIndex = i;
                        ++MatchFieldCount;
                        break;
                    }
                }
            }


            // Loop through the contents of the OleDbDataReader object.
            // �� DataReader �е�ÿһ����¼ת����Grid++Report �����ݼ���ȥ
            while (dr.Read())
            {
                Report.DetailGrid.Recordset.Append();

                for (int i = 0; i < MatchFieldCount; ++i)
                {
                    if (!dr.IsDBNull(MatchFieldPairs[i].MatchColumnIndex))
                        MatchFieldPairs[i].grField.Value = dr.GetValue(MatchFieldPairs[i].MatchColumnIndex);
                }

                Report.DetailGrid.Recordset.Post();
            }
        }

        // �� DataTable ������ת���� Grid++Report �����ݼ���
        public static void FillRecordToReport(IGridppReport Report, DataTable dt)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dt.Columns.Count)];

            //�����ֶ������������ƽ���ƥ�䣬����DataReader�ֶ���Grid++Report��¼�����ֶ�֮��Ķ�Ӧ��ϵ
            int MatchFieldCount = 0;
            for (int i = 0; i < dt.Columns.Count; ++i)
            {
                foreach (IGRField fld in Report.DetailGrid.Recordset.Fields)
                {
                    if (String.Compare(fld.Name, dt.Columns[i].ColumnName, true) == 0)
                    {
                        MatchFieldPairs[MatchFieldCount].grField = fld;
                        MatchFieldPairs[MatchFieldCount].MatchColumnIndex = i;
                        ++MatchFieldCount;
                        break;
                    }
                }
            }


            // �� DataTable �е�ÿһ����¼ת���� Grid++Report �����ݼ���ȥ
            foreach (DataRow dr in dt.Rows)
            {
                Report.DetailGrid.Recordset.Append();

                for (int i = 0; i < MatchFieldCount; ++i)
                {
                    if (!dr.IsNull(MatchFieldPairs[i].MatchColumnIndex))
                        MatchFieldPairs[i].grField.Value = dr[MatchFieldPairs[i].MatchColumnIndex];
                }

                Report.DetailGrid.Recordset.Post();
            }
        }

        public static uint RGBToOleColor(byte r, byte g, byte b)
        {
            return ((uint)b) * 256 * 256 + ((uint)g) * 256 + r;
        }

        public static uint ColorToOleColor(System.Drawing.Color val)
        {
            return RGBToOleColor(val.R, val.G, val.B);
        }

        //public static string GetSampleRootPath()
        //{
        //    string FileName = Application.StartupPath.ToLower();
        //    int Index = FileName.LastIndexOf("samples");
        //    FileName = FileName.Substring(0, Index);
        //    return FileName + @"samples\";
        //}

        //public static string GetReportTemplatePath()
        //{
        //    return GetSampleRootPath() + @"Reports\";
        //}

        //public static string GetReportDataPath()
        //{
        //    return GetSampleRootPath() + @"Data\";
        //}

        //public static string GetReportDataPathFile()
        //{
        //    return GetReportDataPath() + @"NorthWind.mdb";
        //}

        //public static string GetDatabaseConnectionString()
        //{
        //    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GetReportDataPathFile();
        //}																																					 }
    }
}
