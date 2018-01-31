using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using log4net;

namespace Helpers
{
    public static class ExcelHelper
    {
        #region publics
        public static string connectionString = "";
        public static ILog Log = LogManager.GetLogger(typeof(ExcelHelper));
        public static bool DoFileExist(string filename)
        {
            return File.Exists(filename);
        }
        public static bool DoDirectoryExist(string directory)
        {
            return Directory.Exists(directory);
        }
        #endregion

        #region HelikonOnly
        public static List<List<string>> GetTasks(string filename)
        {
            var path = ProductProperties.NetworkStoragePlace + filename;// System.IO.Path.GetFullPath(filename);
            List<List<string>> result = new List<List<string>>();
            if (DoFileExist(path))
            {
                try
                {
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.DataSet DtSet;
                    System.Data.OleDb.OleDbDataAdapter MyCommand;

                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + ProductProperties.InputFileSheetName + "$]", MyConnection);
                    MyCommand.TableMappings.Add("Table", "TestTable");
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet);
                    var rows = DtSet.Tables[0].Rows;
                    for (int i = 0; i < rows.Count; i++)
                    {

                        var a = rows[i].ItemArray;
                        if (!a.Any(x => x.GetType() == typeof(System.DBNull)))
                            result.Add(new List<string>(){
                            (string)a[0],//CRK_NumerPelny
                            (string)a[1],//Ope_Kod
                            (string)a[2],//Knt_Kod
                            (string)a[3],//Kat_KodOgolny
                            //(string)a[4],//CRK_DataDok
                            Convert.ToString(Convert.ToInt32(a[5])),//CRK_CRKId
                        });
                    }

                    MyConnection.Close();
                    return result;
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }

        public static List<List<string>> GetTasks2(string filename)
        {
            var path = ProductProperties.NetworkStoragePlace + filename;// System.IO.Path.GetFullPath(filename);
            List<List<string>> result = new List<List<string>>();
            if (DoFileExist(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                var range = xlWorksheet.UsedRange;
                var c = range.Rows.Count;
                try
                {
                    for (int i = 2; i <= c; i++)
                    {
                        var partialResult = new List<string>() { };
                        for (int j = 1; j <= 6; j++)
                        {
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            {
                                partialResult.Add(xlRange.Cells[i, j].Value2.ToString());
                            }
                            else
                            {
                                partialResult.Add(0 + "");
                            }
                        }
                        result.Add(partialResult);
                    }


                    return result;
                }
                catch (Exception ex)
                {
                    //return null;
                    throw ex;
                }
                finally
                {

                    xlWorkbook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlApp);
                }
            }

            return null;
        }
        public static List<List<string>> GetTasks3(string filename)
        {
            var path = ProductProperties.NetworkStoragePlace + filename;// System.IO.Path.GetFullPath(filename);
            List<List<string>> result = new List<List<string>>();
            if (DoFileExist(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                var range = xlWorksheet.UsedRange;
                var c = range.Rows.Count; 
                try
                { 
                    object[,] objectArray = xlWorksheet.get_Range("A2:F"+c).Value2;

                    
                    for (int i = 1; i <= c-1; i++)
                    {
                        var partialResult = new List<string>() { };
                        for (int j = 1; j <= 6; j++)
                        {
                            if (objectArray[i, j] != null)
                            {
                                partialResult.Add(objectArray[i, j].ToString());
                            }
                            else
                            {
                                partialResult.Add(0 + "");
                            }
                        }
                        result.Add(partialResult);
                    }
                    return result;

                }
                catch (Exception ex)
                { 
                    Log.Fatal("GetTasks3 thrown an error\t" + ex.ToString());
                    throw ex;
                }
                finally
                {

                    xlWorkbook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlApp);
                }



            }

            return null;
        }

        public static void SaveLine(string filename, List<object> data)
        {
            var dir = ProductProperties.NetworkStoragePlace + (string)data[1];

            var path = dir + @"\" + filename; //System.IO.Path.GetFullPath(filename);
            List<List<string>> result = new List<List<string>>();
            if (!DoDirectoryExist(dir))
            {
                Directory.CreateDirectory(dir);
                File.Copy(System.IO.Path.GetFullPath("emptyresults.xls"), path);
            }


            if (DoFileExist(path))
            {
                try
                {
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                    string sql = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties=\"Excel 8.0;HDR=YES;IMEX=0;MAXSCANROWS=0\"");
                    MyConnection.Open();
                    myCommand.Connection = MyConnection;

                    var inputDataFormatted = formatDataForInsert(data);

                    sql = "Insert into [" + ProductProperties.OutputFileSheetName + "$] (Projekt,Projektantka,Kontrahent,Handlowiec,ProjektantOpiekun,CzasWMinutach,Start,Stop,KorektaWMinutach) values(" + inputDataFormatted + ")";
                    myCommand.CommandText = sql;
                    myCommand.ExecuteNonQuery();
                    MyConnection.Close();
                }
                catch (Exception ex)
                {
                    Log.Fatal("SaveLine thrown an error\t" + ex.ToString());
                    throw ex;
                }
            }
        }

        public static void SaveLine2(string filename, List<object> data)
        {
            var dir = ProductProperties.NetworkStoragePlace + (string)data[1];

            var path = dir + @"\" + filename; //System.IO.Path.GetFullPath(filename);
            List<List<string>> result = new List<List<string>>();
            if (!DoDirectoryExist(dir))
            {
                Directory.CreateDirectory(dir);
                File.Copy(System.IO.Path.GetFullPath("emptyresults.xls"), path);
            }


            if (DoFileExist(path))
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                    Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                     

                    var range = xlWorksheet.UsedRange;
                    var c = range.Rows.Count;

                    //logic here
                    xlWorksheet.Rows[c].Insert(); //2->c

                    xlWorksheet.Cells[1][c] = data[0].ToString();                           //2->c
                    xlWorksheet.Cells[2][c] = data[1].ToString();
                    xlWorksheet.Cells[3][c] = data[2].ToString();
                    xlWorksheet.Cells[4][c] = data[3].ToString();
                    xlWorksheet.Cells[5][c] = data[4].ToString();
                    xlWorksheet.Cells[6][c] = Convert.ToInt32(Convert.ToDecimal(data[5]));
                    xlWorksheet.Cells[7][c] = /*"'" + */ Convert.ToDateTime(data[6]).ToString("yyyy-MM-dd HH:mm:ss"); //data[6].ToString();
                    xlWorksheet.Cells[8][c] = /*"'" +*/ Convert.ToDateTime(data[7]).ToString("yyyy-MM-dd HH:mm:ss"); ;//data[7].ToString();
                    xlWorksheet.Cells[9][c] = data[8].ToString();




                    xlWorkbook.Save();
                    xlWorkbook.Close();
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlApp);
                    /*
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                    string sql = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties=\"Excel 8.0;HDR=YES;IMEX=0;MAXSCANROWS=0\"");
                    MyConnection.Open();
                    myCommand.Connection = MyConnection;

                    var inputDataFormatted = formatDataForInsert(data);

                    sql = "Insert into [" + ProductProperties.OutputFileSheetName + "$] (Projekt,Projektantka,Kontrahent,Handlowiec,ProjektantOpiekun,CzasWMinutach,Start,Stop,KorektaWMinutach) values(" + inputDataFormatted + ")";
                    myCommand.CommandText = sql;
                    myCommand.ExecuteNonQuery();
                    MyConnection.Close();*/
                }
                catch (Exception ex)
                {
                    Log.Fatal("SaveLine2 thrown an error\t"+ ex.ToString());
                    throw ex;
                }
            }
        }

        private static string formatDataForInsert(List<object> data)
        {
            var result = "";
            var k = 0;
            foreach (var item in data)
            {
                if (k != 5)
                {
                    result += "'" + (string)item + "',";
                }
                else
                {
                    result += Convert.ToInt32(Convert.ToDecimal(item)) + ",";
                }
                k++;
            }
            result = result.Remove(result.Length - 1);
            return result;
        }
        #endregion

    }
}
