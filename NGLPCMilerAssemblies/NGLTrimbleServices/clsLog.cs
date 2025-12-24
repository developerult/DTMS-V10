using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NGLTrimbleServices
{
    public class clsLog
    {
        public clsLog() : base()
        {
        }

        public clsLog(string strFileName, int intKeepLogDays, bool blnSaveOldLog) : base()
        {
            _strFileName = strFileName;
            _intKeepLogDays = intKeepLogDays;
            _blnSaveOldLog = blnSaveOldLog;
            Open(strFileName, intKeepLogDays, blnSaveOldLog);
        }



        private bool _blnDebug = false;
        public bool Debug
        {
            get
            {
                return _blnDebug;
            }
            set
            {
                _blnDebug = value;
            }
        }

        private string _strLastErr = "";
        public string LastError
        {
            get
            {
                return _strLastErr;
            }
            protected set
            {
                _strLastErr = value;
            }
        }


        private string _strFileName = @"C:\Data\TMSLogs\NGLApplication-Log.txt";
        public string FileName
        {
            get
            {
                return _strFileName;
            }
            set
            {
                _strFileName = value;
            }
        }

        private int _intKeepLogDays = 7;
        public int KeepLogDays
        {
            get
            {
                return _intKeepLogDays;
            }
            set
            {
                _intKeepLogDays = value;
            }
        }

        private bool _blnSaveOldLog = false;
        public bool SaveOldLog
        {
            get
            {
                return _blnSaveOldLog;
            }
            set
            {
                _blnSaveOldLog = value;
            }
        }







        public StreamWriter Open()
        {
            return Open(_strFileName, _intKeepLogDays, _blnSaveOldLog);
        }

        public StreamWriter Open(string strFileName)
        {
            return Open(strFileName, 30, true);
        }

        public StreamWriter Open(string strFileName, int intKeepLogDays)
        {
            return Open(strFileName, intKeepLogDays, true);
        }

        public StreamWriter Open(string strFileName, int intKeepLogDays, bool blnSaveOldLog)
        {
            StreamWriter ioLog = null;
            _strFileName = strFileName;
            _intKeepLogDays = intKeepLogDays;
            _blnSaveOldLog = blnSaveOldLog;
            try
            {
                FileInfo fi = new FileInfo(strFileName);
                if (!File.Exists(strFileName))
                {
                    // create it
                    ioLog = fi.CreateText();
                    ioLog.Close();
                }
                else
                {
                    DateTime ndate = DateTime.Now;
                    DateTime fdate = fi.CreationTime;
                    int daysDiff = ((TimeSpan)(ndate - fdate)).Days;
                    if (daysDiff > intKeepLogDays)
                    {
                        if (blnSaveOldLog)
                            fi.MoveTo(timeStampFileName(strFileName));
                        else
                            fi.Delete();
                        fi = new FileInfo(strFileName);
                        ioLog = fi.CreateText();
                        ioLog.Close();
                        fi.CreationTime = ndate;
                        if (_blnDebug)
                            Console.WriteLine("File Date = " + fi.CreationTime.ToString());
                    }
                }
                ioLog = File.AppendText(strFileName);
                if (_blnDebug)
                {
                    Console.WriteLine("***********************************");
                    Console.WriteLine("Log Open: " + strFileName);
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (FileNotFoundException ex)
            {
                if (_blnDebug)
                {
                    Console.WriteLine("clsLog.Open File Not Found Error Re-creating file.");
                    LastError = ex.ToString();
                }
                else
                    LastError = ex.Message;
            }

            catch (Exception ex)
            {
                // ignore any errors
                if (_blnDebug)
                {
                    Console.WriteLine("clsLog.Open Error: " + ex.ToString());
                    LastError = ex.ToString();
                }
                else
                    LastError = ex.Message;
            }
            finally
            {
                try
                {
                    ioLog.Close();
                }
                catch (Exception ex)
                {
                }
            }
            return ioLog;
        }


        public static int intlines = 0;
        public void Write(string logMessage, ref StreamWriter w) {
           
            intlines ++;
            try
            {
                if(_blnDebug )
                {
                    Console.WriteLine(intlines.ToString() + " => " + logMessage);
                }
            
                w = File.AppendText(_strFileName);
                try
                {
                    w.Write("\r\n Entry ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    w.WriteLine("  {0}", logMessage);
                    w.WriteLine("----------------------------------------------");
                    //Update the underlying file.
                    w.Flush();
                }
                catch (Exception ex)
                {
                    // ignore any errors when closing the log file
                    if (_blnDebug)
                    {
                        Console.WriteLine("Write To Log Failure:" + ex.ToString());
                    }
                } finally {
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                // ignore any errors when closing the log file
                if (_blnDebug)
                {
                    Console.WriteLine("Write To Log Failure:" + ex.ToString());
                }
            }
    }


        public void closeLog(int intReturn, ref StreamWriter ioLog)
        {
            try
            {
                ioLog = File.AppendText(_strFileName);
                try
                {
                    ioLog.WriteLine("Return Value: " + intReturn.ToString());
                    ioLog.Flush();
                }
                catch (Exception ex)
                {
                    if (_blnDebug)
                    {
                        Console.WriteLine("Close Log Error (ignored when debug is off):" + ex.ToString());
                        LastError = ex.ToString();
                    }
                    else
                        LastError = ex.Message;
                }
                finally
                {
                    ioLog.Close();
                }
                if (_blnDebug)
                {
                    Console.WriteLine("***********************************");
                    Console.WriteLine("Log Closed");
                    Console.WriteLine("-----------------------------------");
                }
            }
            catch (Exception ex)
            {
                // ignore any errors when closing the log file
                if (_blnDebug)
                {
                    Console.WriteLine("Close Log Error (Ignored when debug is off):" + ex.ToString());
                    LastError = ex.ToString();
                }
                else
                    LastError = ex.Message;
            }
        }

        public void closeLog(ref StreamWriter ioLog)
        {
            try
            {
                ioLog.Close();
            }
            catch (Exception ex)
            {
                // ignore any errors when closing the log file
                if (_blnDebug)
                {
                    Console.WriteLine("Close Log Error (Ignored when debug is off):" + ex.ToString());
                    LastError = ex.ToString();
                }
                else
                    LastError = ex.Message;
            }
        }

        public void DumpLog(string strLogFile)
        {
            StreamReader r;
            try
            {
                r = File.OpenText(strLogFile);

                try
                {


                    // While not at the end of the file, read and write lines.
                    string line;
                    line = r.ReadLine();
                    while (!string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine(line);
                        line = r.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                }
                // do nothing
                finally
                {
                    try
                    {
                        r.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            catch (Exception ex)
            {
            }
        }



        public string timeStampFileName(string strFileName, string strNewExtension = "", bool blnNoSpaces = false)
        {
            try
            {
                DateTime dt = DateTime.Now;
                System.Globalization.DateTimeFormatInfo dfi = new System.Globalization.DateTimeFormatInfo();
                dfi.DateSeparator = "-";
                dfi.TimeSeparator = "-";

                if (strNewExtension.Length < 1)
                    strNewExtension = strFileName.Substring(strFileName.Length - 4, 4);
                if (blnNoSpaces)
                    return strFileName.Substring(0, strFileName.Length - 4) + dt.Month.ToString() + dt.Day.ToString() + dt.Year.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + strNewExtension;
                else
                    return strFileName.Substring(0, strFileName.Length - 4) + "-" + dt.ToString("g", dfi) + strNewExtension;
            }


            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
