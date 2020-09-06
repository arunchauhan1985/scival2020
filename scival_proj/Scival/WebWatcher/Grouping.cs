using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.WebWatcher
{
    public partial class Grouping : BaseForm
    {
        ErrorLog errorLog = new ErrorLog();
        Replace replace = new Replace();
        Hashtable hashtableUrl = new Hashtable();

        List<UrlDetailAndCount> urlDetailAndCounts;
        List<UrlGroupDetail> urlGroupDetails;
        List<FundingBodyMaster> fundingBodyMasters;

        bool flagSplit = false;
        string inputXmlPath = string.Empty;

        public Grouping()
        {
            InitializeComponent();
            LoadInitialValue();

            try
            {
                int Screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
                int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                this.Width = Screenwidth;
                this.AutoSize = false;
                this.Height = ScreenHeight;
                this.Left = 0;
                this.Top = 0;
                this.ResizeRedraw = false;
                this.Dock = DockStyle.Fill;

                menuStrip2.Left = -200;
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
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadInitialValue()
        {
            try
            {
                webBrowser1.ScriptErrorsSuppressed = true;

                FillFundingDropDown();
                NoRecord();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                RadioButtonCheckedOperation();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                RadioButtonCheckedOperation();
        }

        private void RadioButtonCheckedOperation()
        {
            if (ddlFunding.SelectedIndex == 0)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                MessageBox.Show("Please select FundingBody", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (ddlBatch.SelectedIndex == 0)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                MessageBox.Show("Please select Batch", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);
                    Int64 Batchid = Convert.ToInt64(ddlBatch.SelectedItem);

                    urlDetailAndCounts = WebWatcherDataOperation.GetUrlForGroup(orgDBid, 4, Batchid);
                    urlGroupDetails = WebWatcherDataOperation.GetUrlGroupDetail();

                    lstLeft.Items.Clear();
                    lstrighjt.Items.Clear();
                    hashtableUrl.Clear();

                    if (urlDetailAndCounts.Count > 0)
                    {
                        foreach (UrlDetailAndCount url in urlDetailAndCounts)
                        {
                            lstLeft.Items.Add(url.Url);
                            hashtableUrl.Add(url.Url, url.UrlId);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No URLs found.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (urlGroupDetails.Count > 0)
                    {
                        grdURL.AutoGenerateColumns = false;
                        grdURL.DataSource = urlGroupDetails;
                    }
                    else
                    {
                        NoRecord();
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void FillFundingDropDown()
        {
            try
            {
                inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

                fundingBodyMasters = WebWatcherDataOperation.GetFundingbodyMasters();

                foreach (FundingBodyMaster funding in fundingBodyMasters)
                    if (!string.IsNullOrEmpty(funding.FundingBodyName))
                        funding.FundingBodyName = replace.ReadandReplaceHexaToChar(funding.FundingBodyName, inputXmlPath);

                List<FundingBodyMaster> fundingBodies = new List<FundingBodyMaster>();

                fundingBodies.Add(new FundingBodyMaster { FundingBodyId = 0, FundingBodyName = "--Select FundingBody--" });
                fundingBodies.AddRange(fundingBodyMasters);

                ddlFunding.DataSource = fundingBodies;
                ddlFunding.DisplayMember = "fundingbodyname";
                ddlFunding.ValueMember = "fundingbody_id";

                ddlBatch.Items.Add("-- Select Batch --");
                ddlBatch.SelectedIndex = 0;
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
                Application.OpenForms["Grouping"].Dispose();
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

                Application.OpenForms["Grouping"].Dispose();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstLeft_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < lstLeft.SelectedItems.Count; i++)
                {
                    string url = Convert.ToString(lstLeft.SelectedItems[i]);
                    webBrowser1.Navigate(url);
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstLeft_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < lstLeft.SelectedItems.Count; i++)
                {
                    string leftURL = Convert.ToString(lstLeft.SelectedItems[i]);
                    int lngt = leftURL.IndexOf(")");
                    string rgtURL = leftURL;

                    if (lngt > 0)
                        rgtURL = leftURL.Substring(lngt);

                    if (!lstrighjt.Items.Contains(leftURL))
                        lstrighjt.Items.Add(Convert.ToString(rgtURL));
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void ddlFunding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;

                lstLeft.Items.Clear();
                lstrighjt.Items.Clear();

                if (ddlFunding.SelectedIndex != 0)
                {
                    string fundingId = Convert.ToString(ddlFunding.SelectedValue);

                    DataTable dstemp = ((DataTable)ddlFunding.DataSource).Copy();
                    dstemp.DefaultView.RowFilter = "fundingbody_id= '" + fundingId + "'";
                    dstemp = dstemp.DefaultView.ToTable();

                    int batch = Convert.ToInt32(dstemp.Rows[0]["batch"]);

                    ddlBatch.Items.Clear();
                    ddlBatch.Items.Add("-- Select Batch --");

                    for (int i = 0; i < batch; i++)
                        ddlBatch.Items.Add(i + 1);

                    ddlBatch.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstrighjt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < lstrighjt.SelectedItems.Count; i++)
                {
                    lstrighjt.Items.Remove(Convert.ToString(lstrighjt.SelectedItems[i]));
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstLeft_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Draw the background of the ListBox control for each item.
                e.DrawBackground();

                // Define the default color of the brush as black.
                Brush myBrush = Brushes.Black;

                if (urlDetailAndCounts.Count > 0)
                {
                    var index = urlDetailAndCounts[e.Index].Count;

                    // Determine the color of the brush to draw each item based on the index of the item to draw.
                    switch (index)
                    {
                        case 0:
                            myBrush = Brushes.Black;
                            break;
                        default:
                            myBrush = Brushes.Black;
                            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
                            break;
                    }

                    // Draw the current item text based on the current Font and the custom brush settings.
                    e.Graphics.DrawString("( " + index + " ) " + lstLeft.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Bold), myBrush, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), StringFormat.GenericDefault);

                    // If the ListBox has focus, draw a focus rectangle around the selected item.
                    e.DrawFocusRectangle();
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstrighjt.Items.Count > 0)
                {
                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);
                    Int64 Batchid = Convert.ToInt64(ddlBatch.SelectedItem);
                    Int64 moduleid = 0;

                    string URLId = string.Empty; string totalStr = string.Empty; string sep = string.Empty;

                    if (radioButton1.Checked)
                        moduleid = 4;
                    else
                        moduleid = 3;

                    for (int i = 0; i < lstrighjt.Items.Count; i++)
                    {
                        URLId = Convert.ToString(hashtableUrl[lstrighjt.Items[i]]);
                        totalStr = totalStr + sep + URLId;
                        sep = ",";
                    }

                    urlDetailAndCounts = null;
                    urlGroupDetails = null;

                    urlDetailAndCounts = WebWatcherDataOperation.Grouping(orgDBid, moduleid, totalStr, SharedObjects.User.USERID, Batchid);
                    urlGroupDetails = WebWatcherDataOperation.GetUrlGroupDetail();

                    if (urlGroupDetails.Count > 0)
                    {
                        grdURL.AutoGenerateColumns = false;
                        grdURL.DataSource = urlGroupDetails;
                    }
                    else
                        NoRecord();

                    hashtableUrl.Clear();
                    lstLeft.Items.Clear();
                    lstrighjt.Items.Clear();

                    if (urlDetailAndCounts.Count > 0)
                    {
                        foreach (UrlDetailAndCount url in urlDetailAndCounts)
                        {
                            lstLeft.Items.Add(url.Url);
                            hashtableUrl.Add(url.Url, url.UrlId);
                        }
                    }

                    MessageBox.Show("Success", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void lstLeft_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (urlDetailAndCounts.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this URL ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 moduleid = 0;
                            Int64 URLId = 0;

                            for (int i = 0; i < lstLeft.SelectedItems.Count; i++)
                                URLId = Convert.ToInt64(hashtableUrl[lstLeft.SelectedItems[i]]);

                            if (radioButton1.Checked)
                                moduleid = 4;
                            else
                                moduleid = 3;

                            string message;

                            urlDetailAndCounts = WebWatcherDataOperation.DeleteUrl(moduleid, URLId, SharedObjects.User.USERID);

                            try
                            {
                                hashtableUrl.Clear();

                                if (lstLeft.Items.Count > 0)
                                    lstLeft.Items.Clear();
                                if (lstrighjt.Items.Count > 0)
                                    lstrighjt.Items.Clear();
                            }
                            catch
                            {
                                hashtableUrl.Clear();

                                if (lstLeft.Items.Count > 0)
                                    lstLeft.Items.Clear();
                                if (lstrighjt.Items.Count > 0)
                                    lstrighjt.Items.Clear();
                            }

                            if (urlDetailAndCounts.Count > 0)
                            {
                                foreach (UrlDetailAndCount url in urlDetailAndCounts)
                                {
                                    lstLeft.Items.Add(url.Url);
                                    hashtableUrl.Add(url.Url, url.UrlId);
                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void grdURL_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (urlGroupDetails != null && urlGroupDetails.Count > 0)
                {
                    int rowindex = e.RowIndex;
                    Int64 orgDBid = Convert.ToInt64(ddlFunding.SelectedValue);
                    Int64 id = urlGroupDetails[rowindex].Id;
                    Int64 intmoduleid = urlGroupDetails[rowindex].ModuleId.Value;
                    Int64 intBatch = urlGroupDetails[rowindex].Batch.Value;

                    GroupDetail grpObj = new GroupDetail(this, id, orgDBid, intmoduleid, intBatch);
                    grpObj.ShowDialog();
                }
                else
                    MessageBox.Show("There is no record(s) for Group Details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        public void RefreshLeftList(List<UrlDetailAndCount> leftUrlList)
        {
            try
            {
                hashtableUrl.Clear();
                lstLeft.Items.Clear();
                lstrighjt.Items.Clear();

                if (leftUrlList.Count > 0)
                {
                    foreach (UrlDetailAndCount url in urlDetailAndCounts)
                    {
                        lstLeft.Items.Add(url.Url);
                        hashtableUrl.Add(url.Url, url.UrlId);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void grdURL_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (urlGroupDetails.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this URL ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 intorgDBid = urlGroupDetails[grdURL.SelectedCells[0].RowIndex].Id;
                            Int64 intgroupid = urlGroupDetails[grdURL.SelectedCells[0].RowIndex].GroupId;
                            Int64 inturlnumber = urlGroupDetails[grdURL.SelectedCells[0].RowIndex].UrlNumber.Value;
                            Int64 intmoduleid = urlGroupDetails[grdURL.SelectedCells[0].RowIndex].ModuleId.Value;
                            Int64 intBatch = urlGroupDetails[grdURL.SelectedCells[0].RowIndex].Batch.Value;

                            urlDetailAndCounts = WebWatcherDataOperation.UrlUngrouping(intgroupid, intorgDBid, intBatch, intmoduleid, inturlnumber);
                            urlGroupDetails = WebWatcherDataOperation.GetUrlGroupDetail();

                            if (urlGroupDetails.Count > 0)
                            {
                                grdURL.AutoGenerateColumns = false;
                                grdURL.DataSource = urlGroupDetails;
                            }
                            else
                                NoRecord();

                            hashtableUrl.Clear();
                            lstLeft.Items.Clear();
                            lstrighjt.Items.Clear();

                            if (urlDetailAndCounts.Count > 0)
                            {
                                foreach (UrlDetailAndCount url in urlDetailAndCounts)
                                {
                                    lstLeft.Items.Add(url.Url);
                                    hashtableUrl.Add(url.Url, url.UrlId);
                                }
                            }

                            MessageBox.Show("Success", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
