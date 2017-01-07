using System;
using System.Collections.Generic;

using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DockSample
{
    class Table_ToExcel
    {
        public Excel.Application m_xlApp = null;
        #region 外部接口
        /// <summary>将一个DataTable的数据导出多个Excel文件（每一个Excel文件的数据行数由函数控制）
        /// 将一个DataTable的数据导出多个Excel文件（每一个Excel文件的数据行数由函数控制）
        /// </summary>
        /// <param name="tempDataTable">数据源</param>
        /// <param name="PathFileName">保存excel的路径</param>
        /// <param name="ExcelRows">每一个Excel文件的行数</param>
        /// <param name="ExcelVersion">导出Excel的版本(2003,2007)</param>
        public void u_DataTableToExcel1(DataTable tempDataTable, string PathFileName, long ExcelRows, string ExcelVersion)
        {
            if (tempDataTable == null)
            {
                return;
            }
            long rowNum = tempDataTable.Rows.Count;//导出数据的行数  
            int columnNum = tempDataTable.Columns.Count;//导出数据的列数  
            string sFileName = "";
            if (rowNum > ExcelRows)
            {
                long excelRows = ExcelRows;//定义个excel文件显示的行数，最大的行数为65535，不能超过65535                    
                int scount = (int)(rowNum / excelRows);//生成excel文件的个数  
                if (scount * excelRows < rowNum)//当总行数不被excelRows整除时，经过四舍五入可能excel的个数不准  
                {
                    scount = scount + 1;
                }
                for (int sc = 1; sc <= scount; sc++)
                {
                    int init = int.Parse(((sc - 1) * excelRows).ToString());
                    sFileName = PathFileName + sc.ToString();
                    long start = init;
                    long end = sc * excelRows - 1;
                    if (sc == scount)
                        end = rowNum - 1;
                    u_OutExcel(tempDataTable, start, end, sFileName, ExcelVersion);
                }
            }
            else
            {
                u_OutExcel(tempDataTable, 0, rowNum - 1, PathFileName, ExcelVersion);
            }
        }
        /// <summary>将一个DataTable的数据导出一个Excel文件:可能包含多个sheet文件，由sheet行数决定 (每一个sheet文件的行数由函数控制)
        /// 将一个DataTable的数据导出一个Excel文件:可能包含多个sheet文件，由sheet行数决定 (每一个sheet文件的行数由函数控制)
        /// </summary>
        /// <param name="tempDataTable">数据源</param>
        /// <param name="PathFileName">导出excel的路径</param>
        /// <param name="SheetRows">excel的文件中sheet的行数</param>
        /// <param name="ExcelVersion">导出Excel的版本</param>
        public void u_DataTableToExcel2(DataTable tempDataTable, string PathFileName, long SheetRows, string ExcelVersion)
        {
            if (tempDataTable == null)
            {
                return;
            }
            long rowNum = tempDataTable.Rows.Count;//行数  
            int columnNum = tempDataTable.Columns.Count;//列数  
            Excel.Application m_xlApp = new Excel.Application();
            m_xlApp.DisplayAlerts = false;//不显示更改提示  
            m_xlApp.Visible = false;
            Excel.Workbooks workbooks = m_xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
            try
            {
                if (rowNum > SheetRows)//单张Sheet表格最大行数  
                {
                    long sheetRows = SheetRows;//定义每页显示的行数,行数必须小于65536  
                    int scount = (int)(rowNum / sheetRows);//导出数据生成的表单数  
                    if (scount * sheetRows < rowNum)//当总行数不被sheetRows整除时，经过四舍五入可能页数不准  
                    {
                        scount = scount + 1;
                    }
                    for (int sc = 1; sc <= scount; sc++)
                    {
                        if (sc > 1)
                        {
                            object missing = System.Reflection.Missing.Value;
                            worksheet = (Excel.Worksheet)workbook.Worksheets.Add(missing, missing, missing, missing);//添加一个sheet  
                        }
                        else
                        {
                            worksheet = (Excel.Worksheet)workbook.Worksheets[sc];//取得sheet1  
                        }
                        string[,] datas = new string[sheetRows + 1, columnNum];

                        for (int i = 0; i < columnNum; i++) //写入字段  
                        {
                            datas[0, i] = tempDataTable.Columns[i].Caption;//表头信息  
                        }
                        Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]);
                        range.Interior.ColorIndex = 15;//15代表灰色  
                        range.Font.Bold = true;
                        range.Font.Size = 9;
                        int init = int.Parse(((sc - 1) * sheetRows).ToString());
                        int r = 0;
                        int index = 0;
                        int result;
                        if (sheetRows * sc >= rowNum)
                        {
                            result = (int)rowNum;
                        }
                        else
                        {
                            result = int.Parse((sheetRows * sc).ToString());
                        }
                        for (r = init; r < result; r++)
                        {
                            index = index + 1;
                            for (int i = 0; i < columnNum; i++)
                            {
                                object obj = tempDataTable.Rows[r][tempDataTable.Columns[i].ToString()];
                                datas[index, i] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式  
                            }
                            System.Windows.Forms.Application.DoEvents();
                            //添加进度条  
                        }
                        Excel.Range fchR = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[index + 1, columnNum]);
                        fchR.Value2 = datas;
                        worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。  
                        m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;//Sheet表最大化  
                        range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[index + 1, columnNum]);
                        //range.Interior.ColorIndex = 15;//15代表灰色  
                        range.Font.Size = 9;
                        range.RowHeight = 14.25;
                        range.Borders.LineStyle = 1;
                        range.HorizontalAlignment = 1;
                    }
                }
                else
                {
                    string[,] datas = new string[rowNum + 1, columnNum];
                    for (int i = 0; i < columnNum; i++) //写入字段  
                    {
                        datas[0, i] = tempDataTable.Columns[i].Caption;
                    }
                    Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]);
                    range.Interior.ColorIndex = 15;//15代表灰色  
                    range.Font.Bold = true;
                    range.Font.Size = 9;
                    int r = 0;
                    for (r = 0; r < rowNum; r++)
                    {
                        for (int i = 0; i < columnNum; i++)
                        {
                            object obj = tempDataTable.Rows[r][tempDataTable.Columns[i].ToString()];
                            datas[r + 1, i] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式  
                        }
                        System.Windows.Forms.Application.DoEvents();
                        //添加进度条  
                    }
                    Excel.Range fchR = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]);
                    fchR.Value2 = datas;
                    worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。  
                    m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;
                    range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowNum + 1, columnNum]);
                    //range.Interior.ColorIndex = 15;//15代表灰色  
                    range.Font.Size = 9;
                    range.RowHeight = 14.25;
                    range.Borders.LineStyle = 1;
                    range.HorizontalAlignment = 1;
                }
                workbook.Saved = true;
                switch (ExcelVersion)
                {
                    case "2003":
                        string s2003 = PathFileName + ".xls";
                        object ob = System.Reflection.Missing.Value;
                        workbook.SaveAs(s2003, Excel.XlFileFormat.xlExcel8, ob, ob, ob, ob, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, ob, ob, ob, ob, ob);
                        break;
                    case "2007":
                        string s2007 = PathFileName + ".xlsx";
                        workbook.SaveCopyAs(s2007);
                        break;
                    default: break;
                }
                KillProcess("EXCEL");//杀死excel进程
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出异常" + ex.Message, "导出异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                KillProcess("EXCEL");//杀死excel进程
            }
        }
        /// <summary>将一个DataTable的数据导出一个Excel文件:可能包含多个sheet文件，由sheet行数决定 (每一个sheet文件的行数由函数控制)
        /// 将一个DataTable的数据导出一个Excel文件:可能包含多个sheet文件，由sheet行数决定 (每一个sheet文件的行数由函数控制)
        /// </summary>
        /// <param name="tempDataTable">数据源</param>
        /// <param name="PathFileName">导出excel的路径</param>
        /// <param name="SheetRows">excel的文件中sheet的行数</param>
        /// <param name="ExcelVersion">导出Excel的版本</param>
        public void u_ArrayToExcel2(string[, ,] tempArray, int[, ,] tempArray2, string PathFileName, string ExcelVersion, string[,] pd, int[,] sj, double[,] jl, int xx)
        {
            if (tempArray == null)
            {
                return;
            }
            int scount=tempArray.GetLength(0);//导出数据生成的表单数
            int rowsNum = tempArray.GetLength(1);//行数
            int columnsNum = tempArray.GetLength(2);//列数
            Excel.Application m_xlApp = new Excel.Application();
            m_xlApp.DisplayAlerts = false;//不显示更改提示  
            m_xlApp.Visible = false;
            Excel.Workbooks workbooks = m_xlApp.Workbooks;
            Excel.Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            //Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
            Excel.Worksheet worksheet;
            try
            {
            for (int sc = 1; sc <= scount; sc++)
            {
                object missing = System.Reflection.Missing.Value;
                worksheet = (Excel.Worksheet)workbook.Worksheets.Add(missing, missing, missing, missing);//添加一个sheet 
                worksheet.Name = jl[0, sc - 1].ToString() + "-" + jl[1, sc - 1].ToString();
                string[,] datas = new string[rowsNum + 1, columnsNum + 1];
                for (int i = 0; i < columnsNum + 1; i++) //写入字段  
                {
                    //datas[0, i] = tempDataTable.Columns[i].Caption;//表头信息  
                    if (i == 0)
                    {
                        datas[0, i] = "频段";
                    }
                    else
                    {
                        datas[0, i] = sj[0, i - 1].ToString() + "-" + sj[1, i - 1].ToString();
                    }
                }
                Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnsNum + 1]);
                range.Interior.ColorIndex = 15;//15代表灰色  
                range.Font.Bold = true;
                range.Font.Size = 9;
                for (int i = 0; i < rowsNum; i++) //写入字段  
                {
                    //datas[0, i] = tempDataTable.Columns[i].Caption;//列头信息  
                    
                        datas[i+1,0] = pd[0, i].ToString() + "-" + pd[1, i].ToString();
                    
                }
                range = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[rowsNum+1, 1]);
                range.Interior.ColorIndex = 16;//15代表灰色  
                range.Font.Bold = true;
                range.Font.Size = 9;
                for (int r = 0; r < rowsNum; r++)
                {

                    for (int i = 0; i < columnsNum; i++)
                    {

                        object obj = tempArray[sc - 1, r, i].ToString();
                        datas[r + 1, i + 1] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式 
                        if (tempArray2[sc - 1, r, i] <= xx && tempArray2[sc - 1, r, i] > 0)
                        {
                            range = worksheet.get_Range(worksheet.Cells[r +2, i + 2], worksheet.Cells[r + 2, i + 2]);
                            range.Interior.ColorIndex = 20;
                        }

                    }
                    System.Windows.Forms.Application.DoEvents();
                    //添加进度条  

                }
                Excel.Range fchR = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowsNum + 1, columnsNum + 1]);
                fchR.Value2 = datas;
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。  
                m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;//Sheet表最大化  
                range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[rowsNum + 1, columnsNum+1]);
                //range.Interior.ColorIndex = 15;//15代表灰色  
                range.Font.Size = 9;
                range.RowHeight = 14.25;
                range.Borders.LineStyle = 1;
                range.HorizontalAlignment = 1;

            }
            workbook.Saved = true;
            switch (ExcelVersion)
            {
                case "2003":
                    string s2003 = PathFileName + ".xls";
                    object ob = System.Reflection.Missing.Value;
                    workbook.SaveAs(s2003, Excel.XlFileFormat.xlExcel8, ob, ob, ob, ob, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, ob, ob, ob, ob, ob);
                    break;
                case "2007":
                    //PathFileName =@"20120910094327\夏\乔司";

                    string s2007 = PathFileName + ".xlsx";
                    workbook.SaveCopyAs(s2007);
                    break;
                default: break;
            }

                KillProcess("EXCEL");//杀死excel进程
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出异常" + ex.Message, "导出异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                KillProcess("EXCEL");//杀死excel进程
            }

        }

        #endregion
        #region 内部接口
        //作用将dt的（startindex到endindex的数据导出到filename）---用于将海量数据导出到多个excel文件
        private void u_OutExcel(DataTable dt, long startindex, long endindex, string filename, string ExcelVersion)
        {
            try
            {
                long columnNum = dt.Columns.Count;
                long excelRows = endindex - startindex+1;
                Excel.Application m_xlApp = new Excel.Application();
                m_xlApp.DisplayAlerts = false;//不显示更改提示  
                m_xlApp.Visible = false;
                Excel.Workbooks workbooks = m_xlApp.Workbooks;
                Excel.Workbook workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 
                string[,] datas = new string[excelRows + 1, columnNum];
                for (int i = 0; i < columnNum; i++) //写入表头字段  
                {
                    string sTitle = dt.Columns[i].ColumnName;
                    datas[0, i] = sTitle;//表头信息  
                }
                Excel.Range range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnNum]);
                range.Interior.ColorIndex = 15;//15代表灰色  
                range.Font.Bold = true;
                range.Font.Size = 9;
                int r = 0;
                int row = 0;
                for (r = Convert.ToInt32(startindex); r <=endindex; r++)
                {
                    row++;
                    for (int i = 0; i < columnNum; i++)
                    {
                        string sname = dt.Columns[i].ToString().Trim();
                        object obj=null;
                        if (sname == "当前状态")
                        {
                            string str= dt.Rows[r][sname].ToString();
                            switch (str)
                            {
                                case "0": obj = "待报价";
                                    break;
                                case "1": obj = "待派工";
                                    break;
                                case "2": obj = "维修中";
                                    break;
                                case "3": obj = "待质检";
                                    break;
                                case "4": obj = "待结算";
                                    break;
                                case "5": obj = "待审核";
                                    break;
                                case "6": obj = "已结束";
                                    break;
                                case "7": obj = "已取消";
                                    break;
                                case "8": obj = "待验收";
                                    break;
                                case "9": obj = "待收款";
                                    break;
                                case "10": obj = "送修中";
                                    break;
                            }
                        }
                        else
                        {
                            obj = dt.Rows[r][sname];
                        }
                        datas[row, i] = obj == null ? "" : "'" + obj.ToString().Trim();//在obj.ToString()前加单引号是为了防止自动转化格式  
                    }
                    System.Windows.Forms.Application.DoEvents();
                    //添加进度条  
                }
                Excel.Range fchR = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[excelRows + 1, columnNum]);
                fchR.Value2 = datas;
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。  
                m_xlApp.WindowState = Excel.XlWindowState.xlMaximized;
                range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[excelRows + 1, columnNum]);
                //range.Interior.ColorIndex = 15;//15代表灰色  
                range.Font.Size = 9;
                range.RowHeight = 14.25;
                range.Borders.LineStyle = 1;//1边框为实线 0为excel样式
                range.HorizontalAlignment = 1;
                workbook.Saved = true;
                string temp="";
               switch (ExcelVersion)
                {
                    case "2003":
                        string s2003 = filename + ".xls";
                        temp = s2003;
                        object ob = System.Reflection.Missing.Value;
                        workbook.SaveAs(s2003, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, ob, ob, ob, ob, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, ob, ob, ob, ob, ob);
                        break;
                    case "2007":
                        string s2007 = filename + ".xlsx";
                        workbook.SaveCopyAs(s2007);
                        break;
                    default: break;
                }

                KillProcess("EXCEL");//杀死excel进程
                System.Diagnostics.Process.Start(temp); 
            }
            catch (Exception ex)
            {
                ;
            }

        }
        //关闭进程
        private void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            try
            {
                foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    thisproc.Kill();
                }
            }
            catch
            {

            }
        }
        #endregion
        #region 附调用方法
        //Table_ToExcel outtoexcel = new Table_ToExcel();
        //SaveFileDialog sfd = new SaveFileDialog();
        //sfd.Filter = "Excel File|*.xlsx";
        //sfd.FileName = DateTime.Now.ToString("yyyy.MM.dd HH.MM.SS");
        //DataTable dt = gridControl1.DataSource as DataTable;
        //DialogResult dd = sfd.ShowDialog(this);
        //if (dd == DialogResult.OK)
        //{
        //    outtoexcel.u_DataTableToExcel1(dt, sfd.FileName, 100, "2003");
        //}
        #endregion
    }
}