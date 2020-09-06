using System;
using System.IO;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.WebWatcher
{
    public partial class DashBoard : BaseForm
    {
        ErrorLog oErrorLog = new ErrorLog();

        public DashBoard()
        {
            InitializeComponent();
        }

        private void btnImportLevel1_Click(object sender, EventArgs e)
        {
            OpenFileDialogHandle("btnImportLevel1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialogHandle("button1");
        }

        private void OpenFileDialogHandle(string buttonClicked)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Upload Fundings URL";
                openFileDialog.Filter = "CSV Files|*.CSV";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Copy File Into Sql loader Directory
                    string path = openFileDialog.SafeFileName;
                    string filename = path.Substring(0, path.Length - 4);

                    FileStream readStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                    String saveTo = @"C:\oracle\scival\scival.csv";

                    bool folderExists = Directory.Exists(Path.GetDirectoryName(saveTo));

                    if (!folderExists)
                        Directory.CreateDirectory(Path.GetDirectoryName(saveTo));

                    FileStream writeStream = new FileStream(saveTo, FileMode.Create, FileAccess.Write);
                    ReadWriteStream(readStream, writeStream);

                    // Run Sql Loader
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = @"c:\oracle\scival\run.bat";
                    process.Start();
                    process.WaitForExit();
                    process.Close();

                    // Call Procedure To Link Data Into Database
                    if (buttonClicked == "btnImportLevel1")
                    {
                        var fundingBodyCount = WebWatcherDataOperation.GetFundingBodyCountByOrgDbId(Convert.ToInt64(filename));

                        if (fundingBodyCount > 0)
                            WebWatcherDataOperation.InsertFundingUrls(Convert.ToInt64(filename), SharedObjects.User.USERID);
                        else
                            MessageBox.Show("Invalid OrgDbId", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RtnReport rtnExpkobj = new RtnReport("Level2");
                rtnExpkobj.ShowDialog();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);

            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }

            readStream.Close();
            writeStream.Close();
        }

        private void btnRtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                RtnAndDelete rtnDelkobj = new RtnAndDelete();
                rtnDelkobj.Show();
                this.Dispose();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnGrpURL_Click(object sender, EventArgs e)
        {
            try
            {
                Grouping rtnDelkobj = new Grouping();
                rtnDelkobj.Show();
                this.Dispose();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnRtnRprt_Click(object sender, EventArgs e)
        {
            try
            {
                RtnReport rtnExpkobj = new RtnReport("Retain");
                rtnExpkobj.ShowDialog();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void DashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
