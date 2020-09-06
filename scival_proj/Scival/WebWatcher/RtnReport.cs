using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.WebWatcher
{
    public partial class RtnReport : BaseForm
    {
        Replace oReplace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();
        List<FundingBodyMaster> fundingBodyMasters = null;

        string mReportType = string.Empty;

        public RtnReport(string reportType)
        {
            InitializeComponent();

            mReportType = reportType;
            fundingBodyMasters = new List<FundingBodyMaster>();

            FillFundingDropDown();
        }

        private void FillFundingDropDown()
        {
            try
            {
                var InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

                List<FundingBodyMaster> fundingbodyMasterList = null;

                if (mReportType == "Retain")
                    fundingbodyMasterList = WebWatcherDataOperation.GetFundingbodyMasters();
                else
                    fundingbodyMasterList = WebWatcherDataOperation.GetFundingForLevel2();

                foreach (FundingBodyMaster fundingbody in fundingbodyMasterList)
                {
                    if (!string.IsNullOrEmpty(fundingbody.FundingBodyName))
                        fundingbody.FundingBodyName = oReplace.ReadandReplaceHexaToChar(fundingbody.FundingBodyName, InputXmlPath);
                }

                FundingBodyMaster fundingbody1 = new FundingBodyMaster();
                fundingbody1.FundingBodyId = 0;
                fundingbody1.FundingBodyName = "--Select FundingBody--";

                fundingBodyMasters.Add(fundingbody1);
                fundingBodyMasters.AddRange(fundingbodyMasterList);

                ddlFunding.DataSource = fundingBodyMasters;
                ddlFunding.DisplayMember = "fundingbodyname";
                ddlFunding.ValueMember = "fundingbody_id";

                comboBox1.Items.Add("-- Select Batch --");
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void RtnReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ddlFunding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlFunding.SelectedIndex != 0)
                {
                    var fundingId = Convert.ToInt64(ddlFunding.SelectedValue);

                    var batch = fundingBodyMasters.Where(fb => fb.FundingBodyId == fundingId).FirstOrDefault().Batch;

                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("-- Select Batch --");

                    for (int i = 0; i < batch; i++)
                        comboBox1.Items.Add(i + 1);

                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                var fundingId = Convert.ToInt64(ddlFunding.SelectedValue);
                var Batchid = Convert.ToInt64(comboBox1.SelectedItem);

                List<String> urlList = null;

                if (mReportType == "Retain")
                    urlList = WebWatcherDataOperation.GetExportUrl(fundingId, Batchid);

                string FileName = "";

                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "";
                saveFileDialog1.Title = "Save File As";
                saveFileDialog1.Filter = "CSV Files|*.CSV";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveFileDialog1.FileName;

                    ToCSV(urlList, FileName);

                    MessageBox.Show("CSV file saved successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ddlFunding.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void ToCSV(List<String> urlList, string strFilePath)
        {
            StreamWriter streamWriter = new StreamWriter(strFilePath, false);

            //headers  
            streamWriter.Write("URL");
            streamWriter.Write(streamWriter.NewLine);

            foreach (String url in urlList)
            {
                if (!Convert.IsDBNull(url))
                {
                    if (url.Contains(','))
                        streamWriter.Write(String.Format("\"{0}\"", url));
                    else
                        streamWriter.Write(url);
                }

                streamWriter.Write(streamWriter.NewLine);
            }

            streamWriter.Close();
        }
    }
}
