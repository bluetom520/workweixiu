using System;
using grproLib;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace GridppReportDemo
{
    /// <summary>
    /// GridppReport 的摘要说明。
    /// </summary>
    public class Utility
    {
        //public const string GetDatabaseConnectionString()  = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\..\..\Data\Northwind.mdb";

        //此函数用来注册Grid++Report，你必须在你的应用程序启动时调用此函数
        //用你自己的序列号代替"AAAAAAA"，"AAAAAAA"是一个无效的序列号
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

        // 将 DataReader 的数据转储到 Grid++Report 的数据集中
        public static void FillRecordToReport(IGridppReport Report, IDataReader dr)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dr.FieldCount)];

            //根据字段名称与列名称进行匹配，建立DataReader字段与Grid++Report记录集的字段之间的对应关系
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
            // 将 DataReader 中的每一条记录转储到Grid++Report 的数据集中去
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

        // 将 DataTable 的数据转储到 Grid++Report 的数据集中
        public static void FillRecordToReport(IGridppReport Report, DataTable dt)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dt.Columns.Count)];

            //根据字段名称与列名称进行匹配，建立DataReader字段与Grid++Report记录集的字段之间的对应关系
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


            // 将 DataTable 中的每一条记录转储到 Grid++Report 的数据集中去
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
