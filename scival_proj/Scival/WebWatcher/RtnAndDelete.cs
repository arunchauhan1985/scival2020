using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.WebWatcher
{
    public partial class RtnAndDelete : BaseForm
    {
        bool flagSplit = false;
        string InputXmlPath = string.Empty;

        Replace replace = new Replace();
        ErrorLog errorLog = new ErrorLog();

        List<WebWatcherUrl> urlList;
        List<FundingBodyMaster> fundingBodyMasters;

        public RtnAndDelete()
        {
            InitializeComponent();
            LoadInitialValue();

            try
            {
                menuStrip2.Left = -200;

                int Screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
                int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                this.Width = Screenwidth;
                this.AutoSize = false;
                this.Height = ScreenHeight;
                this.Left = 0;
                this.Top = 0;
                this.ResizeRedraw = false;
                this.Dock = DockStyle.Fill;

                splitContainer1.SplitterDistance = Screenwidth * 50 / 100;
                btnexit.Left = this.Width - btnexit.Width - 20;
                btnexit.Top = 40;
                btnexit.Visible = true;

                btnHome.Left = this.Width - btnHome.Width - btnexit.Width - 20;
                btnHome.Top = 40;
                btnHome.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadInitialValue()
        {
            try
            {
                webBrowser1.ScriptErrorsSuppressed = true;
                FillFundingDropDown();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void Awards_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btntoggle_Click(object sender, EventArgs e)
        {
            try
            {
                int Screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
                int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                if (flagSplit)
                {
                    splitContainer1.SplitterDistance = (Screenwidth * 20) / 100;
                    flagSplit = false;
                }
                else
                {
                    splitContainer1.SplitterDistance = (Screenwidth * 80) / 100;
                    flagSplit = true;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pnl_With.Visible = false;

                try
                {
                    if (ddlFunding.SelectedIndex == 0)
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        MessageBox.Show("Please select fundingBody", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (comboBox1.SelectedIndex == 0)
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        MessageBox.Show("Please select Batch", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        if (urlList != null && urlList.Count > 0)
                        {
                            Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);

                            if (MessageBox.Show("Do you really want to delete all URLs ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                WebWatcherDataOperation.DeleteAndRetainAll(orgDBid, 4, SharedObjects.User.USERID, 0);
                                urlList = new List<WebWatcherUrl>();
                                NoRecord();
                                FillFundingDropDown();
                                MessageBox.Show("Success", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    if (ddlFunding.SelectedIndex == 0)
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        MessageBox.Show("Please select fundingBody", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (comboBox1.SelectedIndex == 0)
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        MessageBox.Show("Please select Batch", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        if (urlList != null && urlList.Count > 0)
                        {
                            pnl_With.Visible = true;
                            rbWithAwrd.Checked = false;
                            rbWithOpp.Checked = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void ddlFunding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnl_With.Visible = false;

                if (ddlFunding.SelectedIndex != 0)
                {
                    string fundingId = Convert.ToString(ddlFunding.SelectedValue);

                    DataTable dsSrc = (DataTable)ddlFunding.DataSource;
                    DataTable dstemp = dsSrc.Copy();

                    dstemp.DefaultView.RowFilter = "fundingbody_id= '" + fundingId + "'";
                    dstemp = dstemp.DefaultView.ToTable();

                    int batch = Convert.ToInt32(dstemp.Rows[0]["batch"]);

                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("-- Select Batch --");

                    for (int i = 0; i < batch; i++)
                        comboBox1.Items.Add(i + 1);

                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void FillFundingDropDown()
        {
            try
            {
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

                fundingBodyMasters = WebWatcherDataOperation.GetFundingBodyList();

                foreach (FundingBodyMaster master in fundingBodyMasters)
                {
                    if (!string.IsNullOrEmpty(master.FundingBodyName))
                        master.FundingBodyName = replace.ReadandReplaceHexaToChar(master.FundingBodyName, InputXmlPath);
                }

                List<FundingBodyMaster> fundingBodies = new List<FundingBodyMaster>();

                fundingBodies.Add(new FundingBodyMaster { FundingBodyId = 0, FundingBodyName = "--Select FundingBody--" });
                fundingBodies.AddRange(fundingBodyMasters);

                ddlFunding.DataSource = fundingBodies;
                ddlFunding.DisplayMember = "fundingbodyname";
                ddlFunding.ValueMember = "fundingbody_id";

                comboBox1.Items.Add("-- Select Batch --");
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void NoRecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("URL");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdURL.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void grdURL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                pnl_With.Visible = false;

                if (urlList != null && urlList.Count > 0)
                {
                    int ColIndex = e.ColumnIndex;

                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);
                    Int64 Userid = Convert.ToInt64(SharedObjects.User.USERID);
                    Int32 url_id = Convert.ToInt32(urlList[e.RowIndex].UrlId);

                    if (ColIndex == 4)
                    {
                        string url = Convert.ToString(urlList[e.RowIndex].Url);
                        webBrowser1.Navigate(url);
                    }
                    else
                    {
                        string message = "URL retain successfully";

                        if (ColIndex == 0) // Delete  // module_id will be zero. // Mode will be zero
                        {
                            urlList = WebWatcherDataOperation.DeleteAndRetainUrl(orgDBid, 0, 0, url_id, Userid);
                            message = "Delete Successfully";
                        }
                        else if (ColIndex == 1) // award  // Retain // module_id will be four.  // Mode will be one
                            urlList = WebWatcherDataOperation.DeleteAndRetainUrl(orgDBid, 4, 1, url_id, Userid);
                        else if (ColIndex == 2) // Opportunity  // Retain // module_id will be three. // Mode will be one
                            urlList = WebWatcherDataOperation.DeleteAndRetainUrl(orgDBid, 3, 1, url_id, Userid);
                        else if (ColIndex == 3) // Award and Opportunity  // Retain // module_id will be zero. // Mode will be one
                            urlList = WebWatcherDataOperation.DeleteAndRetainUrl(orgDBid, 0, 1, url_id, Userid);

                        if (urlList.Count > 0)
                        {
                            grdURL.AutoGenerateColumns = false;
                            grdURL.DataSource = urlList;
                        }
                        else
                        {
                            NoRecord();
                            FillFundingDropDown();
                        }

                        MessageBox.Show(message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void rbWithAwrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWithAwrd.Checked)
            {
                try
                {
                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);

                    if (MessageBox.Show("Do you really want to retain all with Award ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        WebWatcherDataOperation.DeleteAndRetainAll(orgDBid, 4, SharedObjects.User.USERID, 1);
                        urlList = new List<WebWatcherUrl>();
                        NoRecord();
                        FillFundingDropDown();
                        MessageBox.Show("Success", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void rbWithOpp_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWithOpp.Checked)
            {
                try
                {
                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);

                    if (MessageBox.Show("Do you really want to retain all with Opportunity ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        WebWatcherDataOperation.DeleteAndRetainAll(orgDBid, 3, SharedObjects.User.USERID, 1);
                        urlList = new List<WebWatcherUrl>();
                        NoRecord();
                        FillFundingDropDown();
                        MessageBox.Show("Success", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int Screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
                int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                if (flagSplit)
                {
                    splitContainer1.SplitterDistance = (Screenwidth * 20) / 100;
                    flagSplit = false;
                }
                else
                {
                    splitContainer1.SplitterDistance = (Screenwidth * 80) / 100;
                    flagSplit = true;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            try
            {
                Application.OpenForms["RtnAndDelete"].Dispose();
                Application.OpenForms["Login"].Show();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                DashBoard dsb = new DashBoard();
                dsb.Show();

                Application.OpenForms["RtnAndDelete"].Dispose();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnl_With.Visible = false;

                if (comboBox1.SelectedIndex != 0)
                {
                    Int64 fundindID = Convert.ToInt64(ddlFunding.SelectedValue);
                    Int64 Batchid = Convert.ToInt64(comboBox1.SelectedItem);

                    urlList = WebWatcherDataOperation.GetUrlList(fundindID, Batchid);

                    if (urlList.Count > 0)
                    {
                        grdURL.AutoGenerateColumns = false;
                        grdURL.DataSource = urlList;
                    }
                    else
                        NoRecord();
                }
                else
                    NoRecord();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
