using System;
using System.Management;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Threading;
using MySqlDal;

namespace DAL
{
    public class Transform
    {
        #region Variable Declaration
        private string XmlFile;
        private string TransformationPath;
        private string userName;
        private string password;
        private string machineName;
        private string fpFileExtension;
        private ManagementScope myScope;
        private ConnectionOptions connOptions;
        private ManagementClass manageClass;
       
        static int count = 0;
        #endregion

        #region Constructor
        public Transform(String XmlPathWithoutFileName)
        {
            TransformationPath = XmlPathWithoutFileName;
            machineName = Convert.ToString(ConfigurationSettings.AppSettings["machineName"]);
            userName = Convert.ToString(ConfigurationSettings.AppSettings["userName"]);
            password = Convert.ToString(ConfigurationSettings.AppSettings["password"]);
            fpFileExtension = Convert.ToString(ConfigurationSettings.AppSettings["fpFile"]);
        }
        #endregion

        #region Public User Defined Function
        public Boolean TransformXml(String FileName)
        {
            XmlFile = FileName;
            if (Connect())
            {
                Thread.Sleep(5000);
                 StartProcess(XmlFile);
                 Thread.Sleep(5000);
                 //oErrorLog.WorkProcessLog("Copy completed");
                if (IsCompleted(XmlFile))
                {
                   // oErrorLog.WorkProcessLog("Server Process Completed");
                    Thread.Sleep(5000);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
              //  oErrorLog.WorkProcessLog("Transform Error: Connection can not be established with remote computer.");
                throw new Exception("Transform Error: Connection can not be established with remote computer.");
            }
            return false;
        }
        #endregion

        #region Private User Defined Function
        private Boolean IsCompleted(String FileName)
        {
            Boolean Flag = true;
            Byte[] ITime = new Byte[0];
            while (Flag)
            {
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.FileName = "NET ";
                        process.StartInfo.Arguments = "USE \\" + machineName + @"\C$\TEST /USER:" + userName + " " + password;
                        process.Start();
                        process.WaitForExit();
                    }
                    using (Process process = new Process())
                    {
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true;
                       // process.StartInfo.CreateNoWindow = false;
                        process.StartInfo.FileName = "cmd ";
                        process.StartInfo.Arguments = "/c copy \"\\\\" + machineName + @"\C$\TEST\O_" + FileName + "\" \"" + TransformationPath + "O_" + FileName + "\"";
                        process.Start();
                        process.WaitForExit();
                        Thread.Sleep(5000);
                    }
                    if (File.Exists(TransformationPath + "O_" + FileName)) 
                    {

                        Byte[] DT = File.ReadAllBytes(TransformationPath + "O_" + FileName);
                        if (ITime.Length != DT.Length)
                        {
                            ITime = DT;
                        }
                        else
                        {
                            File.Delete(TransformationPath + FileName);
                            File.Copy(TransformationPath + "O_" + FileName, TransformationPath + FileName);
                            File.Delete(TransformationPath + "O_" + FileName);
                           // oErrorLog.WorkProcessLog("Duplicate file created");

                            StartProcessForVTool(FileName);

                            StartProcessForXMLFiles(FileName);

                            using (Process process = new Process())
                            {
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.FileName = "cmd ";
                                process.StartInfo.Arguments = @"/c del \\" + machineName + @"\C$\TEST\O_" + FileName;
                                process.Start();
                                process.WaitForExit();
                            }
                            using (Process process = new Process())
                            {
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.FileName = "cmd ";
                                process.StartInfo.Arguments = @"/c del \\" + machineName + @"\C$\TEST\" + FileName;
                                process.Start();
                                process.WaitForExit();
                            }
                            string[] delLogFile = FileName.Split('.');
                            
                            using (Process process = new Process())
                            {
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.FileName = "cmd ";
                                process.StartInfo.Arguments = @"/c del \\" + machineName + @"\C$\TEST\" + delLogFile[0] + "_log.xml";
                                process.Start();
                                process.WaitForExit();
                            }

                            Flag = false;
                        }

                    }

                }
                catch (Exception exp)
                {
                   // oErrorLog.WriteErrorLog(exp);
                    return false;
                }

            }
            return !Flag;
        }

        private Boolean IsCompletedLogFile_old(String FileName)
        {
            Boolean Flag = true;
            Byte[] ITime = new Byte[0];
            while (Flag)
                {
                try
                {
                    if (File.Exists(FileName))
                    {
                        Byte[] DT = File.ReadAllBytes(FileName);
                        if (ITime.Length != DT.Length)
                        {
                            ITime = DT;
                           // oErrorLog.WorkProcessLog("Log file copyed...CompletedLogFile");
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                }
                catch (Exception exp)
                {
                   // oErrorLog.WriteErrorLog(exp);
                    return false;
                }
            }
            return !Flag;
        }

        private Boolean IsCompletedLogFile(String logfile,String FileName)
        {
            Boolean Flag = true;
            Byte[] ITime = new Byte[0];

            if (File.Exists(FileName))
            {
                ITime = File.ReadAllBytes(FileName);
            }
            while (Flag)
            {
                try
                {
                    #region New code for copy log file
                    //call bat file
                   // oErrorLog.WorkProcessLog("Start Process For copy log file again");
                    Thread.Sleep(20000);
                    String XsdPath21 = String.Empty;
                    string UserName21 = userName, Password21 = password, LogFile21 = logfile, OutFilePath21 = FileName;
                    XsdPath21 = Environment.CurrentDirectory + "\\BAT\\";

                    String ars21 = " \"" + Password21 + "\"" + " \"" + LogFile21 + "\"" + " \"" + OutFilePath21 + "\"";
                    Process proc21 = new Process();
                    proc21.StartInfo.FileName = XsdPath21 + "DSVTOOLFileCopy.bat";
                    proc21.StartInfo.Arguments = ars21;
                    proc21.StartInfo.CreateNoWindow = true;
                    proc21.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc21.Start();
                    proc21.WaitForExit();
                    Thread.Sleep(20000);
                    //end
                    #endregion

                    if (File.Exists(FileName))
                    {
                        Byte[] DT = File.ReadAllBytes(FileName);
                        if (ITime.Length != DT.Length)
                        {
                            ITime = DT;
                           // oErrorLog.WorkProcessLog("Log file copyed...CompletedLogFile");
                        }
                        else
                        {
                            Flag = false;
                         //   oErrorLog.WorkProcessLog("Log file copied with Full length...CompletedLogFile");
                        }
                    }
                }
                catch (Exception exp)
                {
                   // oErrorLog.WriteErrorLog(exp);
                    return false;
                }
                Thread.Sleep(10000);
            }
            return !Flag;
        }

        private Boolean ChecklogfileLenth(String FileName)
        {
            count = count + 1;
            Boolean Flag = false;
            FileInfo f = new FileInfo(FileName);
            Thread.Sleep(10000);
            long s1 = f.Length;
            if (s1 > 0)
            {
               // oErrorLog.WorkProcessLog("Log completed copy with full length");
                Flag = false;
            }
            else
            {
               // oErrorLog.WorkProcessLog("o kb log file copy");
                Flag = true;
            }
            if (count>15)
            {
                Flag = false;
            }
            return Flag;
        }

        private void StartProcess(string FileName)
        {
           // oErrorLog.WorkProcessLog("Connection Established with server");
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "NET ";
                process.StartInfo.Arguments = @"USE \\" + machineName + @"\C$\TEST /USER:" + userName + " " + password;
                process.Start();
                process.WaitForExit();
            }
            using (Process process = new Process())
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/c copy \"" + TransformationPath + FileName + "\" \"\\\\" + machineName + "\\C$\\TEST\\" + FileName + "\"";
                process.Start();
                process.WaitForExit();
                process.Dispose();
                Thread.Sleep(5000);
            }

            object[] arrParams = { "cmd /c run.bat " + FileName };//," /c run.bat "+FileName };
            try
            {
                manageClass = new ManagementClass(myScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                object obj = manageClass.InvokeMethod("Create", arrParams);
               // oErrorLog.WorkProcessLog("file copyed from local to server..");
            }
            catch (Exception exp)
            {
               // oErrorLog.WriteErrorLog(exp);
               // oErrorLog.WorkProcessLog("Error in Transformation:  run.bat");
                throw new Exception("Error in Transformation:  run.bat");

            }
        }
        private void StartProcessForVTool(string FileName)
        {
            //oErrorLog.WorkProcessLog("Start Process For VTool");
            string errorLogPath = TransformationPath.Replace("XMLZip", "$");
            string[] newErrorLogPath = errorLogPath.Split('$');
            string errorLogFileName = newErrorLogPath[1].Replace("\\", "");
            string[] logFileName = FileName.Split('.');
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "NET ";
                process.StartInfo.Arguments = @"USE \\" + machineName + @"\C$\TEST /USER:" + userName + " " + password;
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
            FileInfo fi;
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd ";
                process.StartInfo.Arguments = "/c del \"\\\\" + machineName + @"\C$\TEST\fp\*.xml" + "\"";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
            
            object[] arrParams = { "cmd /c vtool -log \"C:\\TEST\\" + logFileName[0] + "_log.xml\"" + " -file \"C:\\TEST\\" + "O_" + FileName + "\"" };//," /c run.bat "+FileName };
            try
            {
                manageClass = new ManagementClass(myScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                object obj = manageClass.InvokeMethod("Create", arrParams);
                Thread.Sleep(5000);
               // oErrorLog.WorkProcessLog("Process completed For VTool");
            }
            catch (Exception exp)
            {
                //oErrorLog.WriteErrorLog(exp);
               // oErrorLog.WorkProcessLog("Error in Transformation:  run.bat");
                throw new Exception("Error in Transformation:  run.bat");
                
            }
        }
        private void StartProcessForXMLFiles(string FileName)
        {
           // oErrorLog.WorkProcessLog("Start Process For XML Files");
            string errorLogPath = TransformationPath.Replace("XMLZip", "$");
            string[] newErrorLogPath = errorLogPath.Split('$');
            string errorLogFileName = newErrorLogPath[1].Replace("\\", "");
            string[] logFileName = FileName.Split('.');

            


            
            if (!Directory.Exists(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog"))
            {
                Directory.CreateDirectory(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog");
            }
            if (File.Exists(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog" + @"\" + logFileName[0] + "_log.xml"))
            {
                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.FileName = "cmd ";
                    process.StartInfo.Arguments = "/c del \"" + newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog" + @"\" + logFileName[0] + "_log.xml" + "\"";
                    process.Start();
                    process.WaitForExit();
                    process.Dispose();
                }
            }
            if (File.Exists(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\xmlerrorlog.txt"))
            {
                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.FileName = "cmd ";
                    process.StartInfo.Arguments = "/c del \"" + newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\xmlerrorlog.txt" + "\"";
                    process.Start();
                    process.WaitForExit();
                    process.Dispose();
                }
            }


            #region New code for copy log file
            //call bat file
           //  oErrorLog.WorkProcessLog("Start Process For copy log file");
            Thread.Sleep(20000);
            String XsdPath = String.Empty;
            string UserName = userName, Password = password, LogFile = logFileName[0] + "_log.xml", OutFilePath = newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + logFileName[0] + "_log.xml";
            XsdPath = Environment.CurrentDirectory + "\\BAT\\";

            String ars = " \"" + Password + "\"" + " \"" + LogFile + "\"" + " \"" + OutFilePath + "\"";
            Process proc = new Process();
            proc.StartInfo.FileName = XsdPath + "DSVTOOLFileCopy.bat";
            proc.StartInfo.Arguments = ars;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            Thread.Sleep(10000);
            Thread.Sleep(10000);
            //end
            #endregion

            #region old code for copy log file from server
            //using (Process process = new Process())
            //{
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.RedirectStandardError = true;
            //    process.StartInfo.CreateNoWindow = true;
            //    process.StartInfo.FileName = "cmd";
            //    process.StartInfo.Arguments = "/c copy \"\\\\" + machineName + @"\C$\TEST\" + logFileName[0] + "_log.xml" + "\" \"" + newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + "\"";
            //    process.Start();
            //    process.WaitForExit();
            //    process.Dispose();
            //    Thread.Sleep(5000);
            //}
            #endregion
            
            #region for remove o kb copy file by rantosh temp
            while (ChecklogfileLenth(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + logFileName[0] + "_log.xml"))
            {
                #region New code for copy log file
                //call bat file
               // oErrorLog.WorkProcessLog("Start Process For copy log file again");
                Thread.Sleep(20000);
                String XsdPath21 = String.Empty;
                string UserName21 = userName, Password21 = password, LogFile21 = logFileName[0] + "_log.xml", OutFilePath21 = newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + logFileName[0] + "_log.xml";
                XsdPath21 = Environment.CurrentDirectory + "\\BAT\\";

                String ars21 = " \"" + Password21 + "\"" + " \"" + LogFile21 + "\"" + " \"" + OutFilePath21 + "\"";
                Process proc21 = new Process();
                proc21.StartInfo.FileName = XsdPath21 + "DSVTOOLFileCopy.bat";
                proc21.StartInfo.Arguments = ars21;
                proc21.StartInfo.CreateNoWindow = true;
                proc21.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc21.Start();
                proc21.WaitForExit();
                Thread.Sleep(20000);
                //end
                #endregion
            }
            
            #endregion

            if (!Directory.Exists(TransformationPath + "fp"))
            {
                Directory.CreateDirectory(TransformationPath + "fp");
            }
            Thread.Sleep(10000);


            //if (IsCompletedLogFile(newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + logFileName[0] + "_log.xml"))
            if (IsCompletedLogFile(logFileName[0] + "_log.xml", newErrorLogPath[0] + "XMLZip\\fp_" + errorLogFileName + "_errorlog\\" + logFileName[0] + "_log.xml"))
            {
                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.FileName = "cmd ";
                    process.StartInfo.Arguments = "/c del \"" + TransformationPath + "fp\\*.xml\"";
                    process.Start();
                    process.WaitForExit();
                    process.Dispose();
                }
               // oErrorLog.WorkProcessLog("XML deleted from server..");
            }
            else
            {
              //  oErrorLog.WorkProcessLog("FP log xml Not Copied..");
                throw (new Exception("FP log xml Not Copied.."));
            }
            



            // Start For V-Tool 5.32
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/c copy \"\\\\" + machineName + @"\C$\TEST\fp\O_" + logFileName[0] + fpFileExtension + "\" \"" + TransformationPath + "fp" + "\"";
                process.Start();
                process.WaitForExit();
                process.Dispose();
                Thread.Sleep(5000);
            }
            if (!File.Exists(TransformationPath + "fp" + @"\O_" + logFileName[0] + fpFileExtension))
            {
                throw (new Exception("FP File Not Copied.."));
            }
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/c ren \"" + TransformationPath + "fp\\O_" + logFileName[0] + fpFileExtension + "\" " + logFileName[0] + fpFileExtension + "";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
            // End For V-Tool 5.32
            
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd ";
                process.StartInfo.Arguments = "/c del \"" + TransformationPath + "fp\\O_*.xml"+"\"";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd ";
                process.StartInfo.Arguments = "/c del \"\\\\" + machineName + @"\C$\TEST\fp\*.xml" + "\"";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
        }


        private Boolean Connect()
        {

            try
            {
                connOptions = new ConnectionOptions();
                connOptions.Impersonation = ImpersonationLevel.Impersonate;
                connOptions.EnablePrivileges = true;


                if (machineName.ToUpper() == Environment.MachineName.ToUpper())
                {
                    myScope = new ManagementScope(@"\ROOT\CIMV2", connOptions);
                }
                else
                {
                    connOptions.Username = userName;
                    connOptions.Password = password;
                    myScope = new ManagementScope(@"\\" + machineName + @"\ROOT\CIMV2", connOptions);
                }

                myScope.Connect();
                return true;
            }
            catch (Exception exp)
            {
               // oErrorLog.WriteErrorLog(exp);
                return false;
            }
            finally { }
        }

        public void StartProcessForVTool_JSON(string FileName)
        {
           // oErrorLog.WorkProcessLog("Start Process For VTool");
            //string errorLogPath = TransformationPath.Replace("XMLZip", "$");
            //string[] newErrorLogPath = errorLogPath.Split('$');
            //string errorLogFileName = newErrorLogPath[1].Replace("\\", "");
            string[] logFileName = FileName.Split('.');
            //using (Process process = new Process())
            //{
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.RedirectStandardError = true;
            //    process.StartInfo.CreateNoWindow = true;
            //    process.StartInfo.FileName = "NET ";
            //    process.StartInfo.Arguments = @"USE \\" + machineName + @"\C$\TEST /USER:" + userName + " " + password;
            //    process.Start();
            //    process.WaitForExit();
            //    process.Dispose();
            //}
            FileInfo fi;
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
               
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd ";
               // process.StartInfo.Arguments = "/c del \"\\\\" + @"\C$\TEST\fp\*.xml" + "\"";
               // process.StartInfo.Arguments = "cmd /c vtool -file \"C:\\Temp\\VtoolFB\\" + FileName + "\"" + " -log \"C:\\Temp\\VtoolFB\\" + logFileName[0] + ".xml" + " -vjson grant" + "\"";
                process.StartInfo.Arguments = "cmd /c vtool -file " + FileName  + " -log " + logFileName[0] + ".xml" + " -vjson grant";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }

           // object[] arrParams = { "cmd /c vtool -log \"C:\\TEST\\" + logFileName[0] + "_log.xml\"" + " -file \"C:\\TEST\\" + "O_" + FileName + "\"" };//," /c run.bat "+FileName };
            //try
            //{
            //    manageClass = new ManagementClass(myScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
            //    object obj = manageClass.InvokeMethod("Create", arrParams);
            //    Thread.Sleep(5000);
            //    oErrorLog.WorkProcessLog("Process completed For VTool");
            //}
            //catch (Exception exp)
            //{
            //    oErrorLog.WriteErrorLog(exp);
            //    oErrorLog.WorkProcessLog("Error in Transformation:  run.bat");
            //    throw new Exception("Error in Transformation:  run.bat");

            //}
        }

        public void StartProcessForVTool_NdJSON(string FileName)
        {
           // oErrorLog.WorkProcessLog("Start Process For VTool");
            
            string[] logFileName = FileName.Split('.');
            FileInfo fi;
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd ";
                process.StartInfo.Arguments = "cmd /c vtool -file " + FileName + " -log " + logFileName[0] + ".xml" + " -vjson grant";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }

           
        }

        public void AccessToken(string FileName)
        {
           // oErrorLog.WorkProcessLog("Start Process For VTool");
           
            string[] logFileName = FileName.Split('.');
           
            FileInfo fi;
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "cmd ";
                process.StartInfo.Arguments = "cmd /c vtool -file " + FileName + " -log " + logFileName[0] + ".xml" + " -vjson grant";
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }

        }

        #endregion

    }
}
