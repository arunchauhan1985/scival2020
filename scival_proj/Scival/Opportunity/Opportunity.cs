using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class Opportunity : BaseForm
    {       
        List<sci_language_master> sci_Languages;

        String workflow = String.Empty;
        String fundingBodyName = String.Empty;
        String inputXmlPath = string.Empty;

        bool flagSplit = false;

        Replace replace = new Replace();
        ErrorLog errorLog = new ErrorLog();
        UpdateColumn updateColumn = new UpdateColumn();

        public Opportunity()
        {
            InitializeComponent();

            LoadInitialValue();
            SetWorkflow();
            updateColumn.updateChangeColour = new List<string>();

            try
            {
                OpportunityBase opportunityBase = new OpportunityBase(this);

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
                menuStrip1.Left = button2.Right + 20;
                menuStrip2.Left = -100;
                menuStrip1.Show();
                pnl_btns.Left = Screenwidth - pnl_btns.Width - 20;

                splitContainer1.Panel2.Controls.Add(opportunityBase);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public String GetURL()
        {
            return txtURL.Text;
        }

        public void LoadInitialValue()
        {
            inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

            try
            {
                webBrowser1.ScriptErrorsSuppressed = true;

                if (SharedObjects.WorkId != 0)
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                    {
                        btnexit.Text = "Stop --> New";
                        btnexit.Visible = true;
                    }
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                    {
                        btnexit.Text = "Stop --> Next";
                        btnexit.Visible = true;
                    }
                    else
                    {
                        SharedObjects.TRAN_TYPE_ID = 1;
                        btnexit.Visible = false;
                    }

                    try
                    {
                        fundingBodyName = SharedObjects.FundingBodyName;
                        fundingBodyName = replace.ReadandReplaceHexaToChar(fundingBodyName.ToString(), inputXmlPath);

                        DataSet dsSrtWrk = DashboardDataOperations.StartWork(SharedObjects.WorkId, SharedObjects.User.USERID);

                        if (dsSrtWrk.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            DataSet DSLang = new DataSet();
                            sci_Languages = DashboardDataOperations.GetLanguageMasters(2);

                            if (SharedObjects.PageIds != 10)
                            {
                                txtURL.Text = dsSrtWrk.Tables["URL"].Rows[0][0].ToString();
                                webBrowser1.Navigate(txtURL.Text);
                            }

                            string url = SharedObjects.PreviousUrl = SharedObjects.CurrentUrl = txtURL.Text;

                            try
                            {
                                String[] Token = url.Split('/');
                                string Uri = Token[0] + "//" + Token[2];

                                SharedObjects.Domain = Uri;

                                webBrowser1.IsWebBrowserContextMenuEnabled = true;
                                webBrowser1.WebBrowserShortcutsEnabled = true;
                            }
                            catch { }

                            SharedObjects.StartWork = dsSrtWrk;
                            SharedObjects.TransactionId = Convert.ToInt64(dsSrtWrk.Tables["URL"].Rows[0]["TID"]);

                            GetProcess();

                            SharedObjects.FundingClickPage = "OpportunityBase";
                            SharedObjects.ClickPage = "OpportunityBase";
                        }
                    }
                    catch (Exception ex)
                    {
                        errorLog.WriteErrorLog(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        public void GetProcess()
        {
            //DAL.Opportunity Obj = new DAL.Opportunity();
            //DataSet DS = Obj.GetProgress(Convert.ToInt64(SharedObjects.WorkId));
            DataSet DS = null;
            menuStrip1.Items.Clear();

            if (DS.Tables.Count > 0 || DS.Tables["Progress"].Rows.Count > 0)
            {
                ToolStripMenuItem obj = new ToolStripMenuItem("Open");
                menuStrip1.Items.Add(obj);

                for (Int32 i = 0; i < DS.Tables["Progress"].Rows.Count; i++)
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString())
                    {
                        Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString()
                    };

                    MenuItem menuItem = new MenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString())
                    {
                        Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString()
                    };

                    toolStripMenuItem.Click += new EventHandler(NewStripMenuItemClickEvent);
                    menuItem.Click += new EventHandler(MenuItemClickEvent);

                    if (DS.Tables["Progress"].Rows[i]["FLAG"].ToString() == "1")
                    {
                        toolStripMenuItem.Checked = true;
                        menuItem.Checked = true;
                    }

                    obj.DropDownItems.Add(toolStripMenuItem);
                }
            }
        }

        public void GetProcess_update(string tabname)
        {
            try
            {
                //DAL.Opportunity Obj = new DAL.Opportunity();
                //DataSet DS = Obj.GetProgress(Convert.ToInt64(SharedObjects.WorkId));
                DataSet DS = null;
                menuStrip1.Items.Clear();
                if (DS.Tables.Count > 0 || DS.Tables["Progress"].Rows.Count > 0)
                {
                    ToolStripMenuItem obj = new ToolStripMenuItem("Open");
                    menuStrip1.Items.Add(obj);

                    updateColumn.updateChangeColour.Add(tabname);
                    for (Int32 i = 0; i < DS.Tables["Progress"].Rows.Count; i++)
                    {

                        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString())
                        {
                            Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString()
                        };

                        MenuItem menuItem = new MenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString())
                        {
                            Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString()
                        };

                        toolStripMenuItem.Click += new EventHandler(NewStripMenuItemClickEvent);
                        menuItem.Click += new EventHandler(MenuItemClickEvent);

                        if (DS.Tables["Progress"].Rows[i]["FLAG"].ToString() == "1")
                        {
                            toolStripMenuItem.Checked = true;

                            if (tabname.ToLower().Trim() == toolStripMenuItem.Tag.ToString().ToLower())
                                toolStripMenuItem.ForeColor = System.Drawing.Color.Green;

                            for (int j = 0; j < updateColumn.updateChangeColour.Count; j++)
                                if (toolStripMenuItem.Tag.ToString().ToLower().Trim() == updateColumn.updateChangeColour[j].ToString().ToLower())
                                    toolStripMenuItem.ForeColor = System.Drawing.Color.Green;

                            menuItem.Checked = true;
                        }

                        obj.DropDownItems.Add(toolStripMenuItem);
                    }
                }
            }
            catch { }
        }

        void MenuItemClickEvent(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem tag = (ToolStripMenuItem)sender;
                splitContainer1.Panel2.Controls.Clear();
                SharedObjects.FundingClickPage = tag.Tag.ToString();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void NewStripMenuItemClickEvent(object sender, System.EventArgs e)
        {
            try
            {
                ToolStripMenuItem tag = (ToolStripMenuItem)sender;
                splitContainer1.Panel2.Controls.Clear();
                SharedObjects.FundingClickPage = tag.Tag.ToString();

                if (tag.Tag.ToString() == "Eligibilityclassification")
                {
                    SharedObjects.ClickPage = "OP_Eligibilityclassification.aspx";

                    EligibiltyClassification classification = new EligibiltyClassification(this);
                    classification.Width = splitContainer1.Panel2.Width;

                    splitContainer1.Panel2.Controls.Add(classification);
                }
                else if (tag.Tag.ToString() == "Estimatedfunding" || tag.Tag.ToString() == "Awardceiling" || tag.Tag.ToString() == "AwardFloor")
                {
                    if (tag.Tag.ToString() == "Estimatedfunding")
                        SharedObjects.ClickPage = "OP_EstimatedFunding";
                    else if (tag.Tag.ToString() == "Awardceiling")
                        SharedObjects.ClickPage = "OP_Awardceiling";
                    else if (tag.Tag.ToString() == "AwardFloor")
                        SharedObjects.ClickPage = "OP_Awardfloor";

                    EstimatedFunding funding = new EstimatedFunding(this);
                    funding.Width = splitContainer1.Panel2.Width;

                    splitContainer1.Panel2.Controls.Add(funding);
                }
                else if (tag.Tag.ToString().ToLower() == "instruction".ToLower() || tag.Tag.ToString().ToLower() == "licenseInformation".ToLower() || tag.Tag.ToString() == "Duration" || tag.Tag.ToString() == "About" || tag.Tag.ToString() == "Synopsis" || tag.Tag.ToString() == "Application Info" || tag.Tag.ToString() == "Geoscope" || tag.Tag.ToString() == "Related Items" || tag.Tag.ToString() == "eligibilityDescription" || tag.Tag.ToString() == "limitedSubmissionDescription" || tag.Tag.ToString() == "estimatedAmountDescription")
                {
                    if (tag.Tag.ToString() == "About")
                        SharedObjects.ClickPage = "OP_About";
                    else if (tag.Tag.ToString() == "Synopsis")
                        SharedObjects.ClickPage = "OP_Synopsis";
                    else if (tag.Tag.ToString() == "Duration")
                        SharedObjects.ClickPage = "OP_Duration";
                    else if (tag.Tag.ToString() == "Application Info")
                        SharedObjects.ClickPage = "OP_AppInfo";
                    else if (tag.Tag.ToString() == "Geoscope")
                        SharedObjects.ClickPage = "OP_GeoScope";
                    else if (tag.Tag.ToString() == "Related Items")
                        SharedObjects.ClickPage = "OP_RelatedItem";
                    else if (tag.Tag.ToString() == "eligibility Description")
                        SharedObjects.ClickPage = "OP_eligibilityDescription";
                    else if (tag.Tag.ToString() == "limitedSubmission Description")
                        SharedObjects.ClickPage = "OP_limitedSubmissionDescription";
                    else if (tag.Tag.ToString() == "estimatedAmount Description")
                        SharedObjects.ClickPage = "OP_estimatedAmountDescription";
                    else if (tag.Tag.ToString().ToLower() == "instruction".ToLower())
                        SharedObjects.ClickPage = "OP_instruction";
                    else if (tag.Tag.ToString().ToLower() == "licenseInformation".ToLower())
                        SharedObjects.ClickPage = "OP_licenseInformation";

                    Item item = new Item(this);
                    item.Width = splitContainer1.Panel2.Width;

                    splitContainer1.Panel2.Controls.Add(item);
                }
                else if (tag.Tag.ToString() == "Classification Group")
                {
                    SharedObjects.ClickPage = "OP_ClassificationInfo.aspx";
                    Classification obj = new Classification(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Relatedpportunities")
                {
                    SharedObjects.ClickPage = "OP_RelatedOpportunities.aspx";
                    RelatedOpportunities obj = new RelatedOpportunities(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Relatedfundingbodies")
                {
                    SharedObjects.ClickPage = "OP_RelatedOrg.aspx";
                    RelaredOrg obj = new RelaredOrg(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "opportunityDates")
                {
                    SharedObjects.ClickPage = "opportunityDates.aspx";
                    OpportunityDates obj = new OpportunityDates(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "SubjectMatter")
                {
                    SharedObjects.ClickPage = "SubjectMatter.aspx";
                    SubjectMatter obj = new SubjectMatter(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "opportunityLocation")
                {
                    SharedObjects.ClickPage = "opportunityLocation.aspx";
                    opportunityLocation obj = new opportunityLocation(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString().ToUpper() == "changeHistory".ToUpper())
                {
                    SharedObjects.ClickPage = "changeHistory.aspx";
                    ChangeHistory obj = new ChangeHistory(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString().ToLower() == "contacts" || tag.Tag.ToString().ToLower() == "officers"
                    || tag.Tag.ToString().ToLower() == "contactinfo")
                {
                    if (tag.Tag.ToString().ToLower() == "contacts")
                        SharedObjects.ClickPage = "OP_Contact";
                    else if (tag.Tag.ToString().ToLower() == "officers")
                        SharedObjects.ClickPage = "OP_Officers";
                    else if (tag.Tag.ToString().ToLower() == "contactinfo")
                        SharedObjects.ClickPage = "OP_ContactInfo";

                    Contact obj = new Contact(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else
                {
                    SharedObjects.ClickPage = "OpportunityBase.aspx";
                    OpportunityBase opportunityBase = new OpportunityBase(this);
                    opportunityBase.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(opportunityBase);
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            SharedObjects.CurrentUrl = SharedObjects.PreviousUrl = txtURL.Text = webBrowser1.Url.AbsoluteUri;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(txtURL.Text);
            SharedObjects.CurrentUrl = txtURL.Text;
        }

        private void FundingBody_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnpause_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 5;
            RemarkPause remark = new RemarkPause();
            remark.ShowDialog();
        }

        private void btnhold_Click(object sender, EventArgs e) { }

        private void btnPauseandLogoff_Click(object sender, EventArgs e)
        {
            try
            {
                SharedObjects.PageIds = 6;
                RemarksPauseLogOff remarks = new RemarksPauseLogOff();
                remarks.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                SharedObjects.PageIds = 8;

                string rtunStr = ChkManadtorySection("3");
                string[] arr = rtunStr.Split('~');

                if (SharedObjects.IsOpportunityBaseFilled == false)
                {
                    MessageBox.Show("Please fill Opportunity details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Convert.ToInt32(arr[1]) > 1)
                {
                    MessageBox.Show(Convert.ToString(arr[0]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    RemarkStop remark = new RemarkStop();
                    remark.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void NextOpportunity()
        {
            SetWorkflow();
            LoadInitialValue();
            SharedObjects.FundingClickPage = "OpportunityBase";
            OpportunityBase fundBase = new OpportunityBase(this);
            fundBase.Width = splitContainer1.Panel2.Width;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(fundBase);
            SharedObjects.ClickPage = "OpportunityBase.aspx";
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 10;

            string rtunStr = ChkManadtorySection("3");
            string[] arr = rtunStr.Split('~');

            if (SharedObjects.IsOpportunityBaseFilled == false)
            {
                MessageBox.Show("Please fill Opportunity details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (Convert.ToInt32(arr[1]) > 1)
            {
                MessageBox.Show(Convert.ToString(arr[0]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                RemarkExit remark = new RemarkExit();
                remark.ShowDialog();
                NextOpportunity();
            }
        }

        private string ChkManadtorySection(string mid)
        {
            string str = string.Empty;
            try
            {
                Int32 WFID = Convert.ToInt32(SharedObjects.WorkId);

                String XsdPath = String.Empty;
                XsdPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\XSD";

                string validstring = string.Empty;

                if (mid == "4")
                    validstring = XmlDataOperations.AwardValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                else if (mid == "3")
                    validstring = XmlDataOperations.OpportunityValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                else if (mid == "2")
                    validstring = XmlDataOperations.FundingBodyValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                if (validstring == "1")
                {
                    str = validstring;
                    str += "~";
                    str += "1";
                }
                else
                {
                    str = validstring;
                    str += "~";
                    str += "2";
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }

            return str;
        }

        private void SetWorkflow()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);

                if (SharedObjects.TaskId == 1)
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\Collection.png"); // Collection
                else if (SharedObjects.TaskId == 2)
                {
                    if (SharedObjects.Cycle == 0)
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\Quality-Check.png"); // QualityCheck
                    else if (SharedObjects.Cycle > 0)
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\Update.png");  // update
                }
                else if (SharedObjects.TaskId == 7)
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\Expiry-Check.png"); // XMLDelivery
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void Opportunity_FormClosing(object sender, FormClosingEventArgs e)
        {
            SharedObjects.PageIds = 5;

            RemarkPause remark = new RemarkPause();
            remark.btncancel.Visible = false;
            remark.ShowDialog();
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

        public void SetGridURL(string gridurl)
        {
            webBrowser1.Navigate(gridurl);
            SharedObjects.CurrentUrl = gridurl;
            string url = gridurl;

            try
            {
                Int32 index = url.LastIndexOf('/');
                String[] Token = url.Split('/');
                string Uri = Token[0] + "//" + Token[2];

                if (index > 6)
                    SharedObjects.Domain = url.Substring(0, index);
                else
                    SharedObjects.Domain = url;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
