using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MySqlDal;

namespace Scival
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ErrorLog oErrorLog = new ErrorLog();

            try
            {
                //var isApiInitialized = HttpClientInstance.InitializeHttpClient(Convert.ToString(ConfigurationManager.AppSettings["ApiLink"]));
                //if (!isApiInitialized)
                //    MessageBox.Show("Api client is not initialized");
                //else

                if (IsApplicationAlreadyRunning())
                    MessageBox.Show("The application is already running");
                else if (!IsAppAndDbVersionMatch())
                    MessageBox.Show("OLD Version ! \nPlease Update Your Application.");
                else
                    Application.Run(new Login());
            }
            catch (ThreadAbortException ex)
            {
                oErrorLog.WriteErrorLog(ex);
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        static bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);

            if (processes.Length > 1)
                return true;
            else
                return false;
        }

        static bool IsAppAndDbVersionMatch()
        {
            string appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string dbVersion = CommonDataOperation.GetDatabaseVersion();

            if (appVersion == dbVersion)
                return true;
            else
                return false;
        }
    }
}
