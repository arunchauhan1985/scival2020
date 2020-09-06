using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Award
{
    public partial class Awards : BaseForm
    {

        public String WorkFlow = String.Empty;
        public String FBNAME = String.Empty;
        ErrorLog oErrorLog = new ErrorLog();
        DataSet dsSec;
        bool flagSplit = false;
        UpdateColumn updateColumn = new UpdateColumn();
        List<startwork> startwork = new List<startwork>();
        List<sci_language_master> languageMaster = new List<sci_language_master>();
        List<ProgressTable> progresslst = new List<ProgressTable>();
        //AwardBase fundBase = new AwardBase(this);
        public Awards()
        {
            InitializeComponent();
            updateColumn.UpdateChangeColour = new List<string>();
            setWorkflow();
            LoadInitialValue();
            try
            {
                AwardBase fundBase = new AwardBase(this); 
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
                splitContainer1.Panel2.Controls.Add(fundBase);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public String GetURL()
        {
            return txtURL.Text;
        }
        public void LoadInitialValue()
        {
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
                        btnexit.Visible = false;
                    }
                    try
                    {
                        FBNAME = SharedObjects.FundingBodyName.ToString();
                        Int64 UID = Convert.ToInt64(SharedObjects.User.USERID);
                        startwork = AwardDataOperations.GetStartWork(SharedObjects.WorkId, SharedObjects.User.USERID);
                        if (startwork.Count() > 0)
                        {
                            languageMaster = AwardDataOperations.GetLanguageMasterDetails(2);
                            //startwork.Add(languageMaster);
                            if (SharedObjects.PageIds != 10)
                            {
                                txtURL.Text = startwork[0].Url;
                                webBrowser1.Navigate(txtURL.Text);
                            }
                            SharedObjects.PreviousUrl = txtURL.Text;
                            SharedObjects.CurrentUrl = txtURL.Text;
                            string url = txtURL.Text;
                            try
                            {
                                Int32 index = url.LastIndexOf('/');
                                String[] Token = url.Split('/');
                                string Uri = Token[0] + "//" + Token[2];
                                SharedObjects.Domain = Uri;
                            }
                            catch (Exception ex)
                            {
                                oErrorLog.WriteErrorLog(ex);
                            }

                            SharedObjects.startworks = startwork;
                            SharedObjects.TransactionId = startwork[0].TransitionalID;

                            GetProcess();
                            SharedObjects.FundingClickPage = "Awardbase";
                            SharedObjects.ClickPage = "Awardbase";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public void GetProcess(string tabname = null)
        {
            progresslst = AwardDataOperations.GetProgress(SharedObjects.WorkId);
            menuStrip1.Items.Clear();
            if (progresslst.Count() > 0)
            {
                ToolStripMenuItem obj = new ToolStripMenuItem("Open");
                menuStrip1.Items.Add(obj);
                if (tabname != string.Empty && tabname != "")
                {
                    updateColumn.updateChangeColour.Add(tabname);
                }
                for (int i = 0; i < progresslst.Count; i++)
                {
                    ToolStripMenuItem New = new ToolStripMenuItem(progresslst[i].Tab);
                    MenuItem MI = new MenuItem(progresslst[i].Tab);
                    MI.Tag = progresslst[i].Tab;
                    MI.Click += new EventHandler(MI_Click);
                    New.Tag = progresslst[i].Tab;
                    New.Click += new EventHandler(New_Click);
                    if (progresslst[i].flag == 1)
                    {
                        New.Checked = true;
                        MI.Checked = true;
                        if (tabname != string.Empty && tabname != "")
                        {
                            if (tabname.ToLower().Trim() == New.Tag.ToString().ToLower())
                                New.ForeColor = System.Drawing.Color.Green;

                            for (int j = 0; j < updateColumn.updateChangeColour.Count; j++)
                            {
                                if (New.Tag.ToString().ToLower().Trim() == updateColumn.updateChangeColour[j].ToLower())
                                {
                                    New.ForeColor = System.Drawing.Color.Green;
                                }
                            }
                        }
                    }
                    obj.DropDownItems.Add(New);
                }
            }
        }

        void MI_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem tag = (ToolStripMenuItem)sender;
                splitContainer1.Panel2.Controls.Clear();
                SharedObjects.FundingClickPage = tag.Tag.ToString();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void New_Click(object sender, System.EventArgs e)
        {
            try
            {
                ToolStripMenuItem tag = (ToolStripMenuItem)sender;
                splitContainer1.Panel2.Controls.Clear();
                SharedObjects.FundingClickPage = tag.Tag.ToString();
                if (tag.Tag.ToString() == "AwardAmount")
                {

                    SharedObjects.ClickPage = "AW_Amount";
                    Amount obj = new Amount(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }
                else if (tag.Tag.ToString() == "Classification Group")
                {
                    SharedObjects.ClickPage = "OP_ClassificationInfo.aspx";
                    Classification obj = new Classification(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }
                else if (tag.Tag.ToString() == "Awardees")
                {
                    SharedObjects.ClickPage = "OP_AwardeeInfo.aspx";
                    Awardee obj = new Awardee(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }
                else if (tag.Tag.ToString() == "Award Manager")
                {
                    SharedObjects.ClickPage = "AW_AwardManager";
                    Contact obj = new Contact(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }
                else if (tag.Tag.ToString() == "Related Programs")
                {
                    SharedObjects.ClickPage = "AW_Relatedprograms.aspx";
                    RelatedProgram obj = new RelatedProgram(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }

                else if (tag.Tag.ToString() == "Related Fundingbodies")
                {
                    SharedObjects.ClickPage = "AW_RelatedOrg.aspx";
                    RelaredOrg obj = new RelaredOrg(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                #region Added for Schema v4.0 By pankaj
                else if (tag.Tag.ToString() == "AwardLocation")
                {
                    SharedObjects.ClickPage = "AwardLocation.aspx";
                    AwardLocation obj = new AwardLocation(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }
                #endregion

                #region Added for Schema v3.0 By Rantosh
                else if (tag.Tag.ToString().ToLower() == "Publication identifier".ToLower())
                {
                    SharedObjects.ClickPage = "ScholarlyOutput.aspx";
                    ScholarlyOutputType obj = new ScholarlyOutputType(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }

                #endregion

                #region Added for Schema v3.0 By Avanish on 6-June-2018
                else if (tag.Tag.ToString() == "RelatedItems")
                {
                    SharedObjects.ClickPage = "RelatedItems.aspx";
                    RelatedItems obj = new RelatedItems(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }

                #endregion
                else if (tag.Tag.ToString().ToLower() == "Relatedpportunities".ToLower())
                {
                    SharedObjects.ClickPage = "RelatedOpportunities.aspx";
                    RelatedOpportunities obj = new RelatedOpportunities(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString().ToLower() == "Publication".ToLower())
                {
                    SharedObjects.ClickPage = "Publication.aspx";
                    Publication obj = new Publication(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString().ToLower() == "Funds".ToLower())
                {
                    SharedObjects.ClickPage = "Funds.aspx";
                    Funds obj = new Funds(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else
                {
                    SharedObjects.ClickPage = "AwrdBase.aspx";
                    SharedObjects.FundingClickPage = "Awardbase";
                    AwardBase fundBase = new AwardBase(this);
                    fundBase.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(fundBase);

                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        public void NextAward()
        {
            setWorkflow();
            LoadInitialValue();
            SharedObjects.ClickPage = "AwrdBase.aspx";
            SharedObjects.FundingClickPage = "Awardbase";
            AwardBase fundBase = new AwardBase(this);
            fundBase.Width = splitContainer1.Panel2.Width;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(fundBase);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            txtURL.Text = webBrowser1.Url.AbsoluteUri;
            SharedObjects.PreviousUrl = txtURL.Text;
            SharedObjects.CurrentUrl = txtURL.Text;
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
            Remarks_Pause rmkobj = new Remarks_Pause();
            rmkobj.ShowDialog();
        }

        private void btnhold_Click(object sender, EventArgs e)
        {
        }

        private void btnPauseandLogoff_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 6;
            Remarks_Pause_Logoff rmkobj = new Remarks_Pause_Logoff();
            rmkobj.ShowDialog();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 8;
            string rtunStr = ChkManadtorySection("4");
            string[] arr = rtunStr.Split('~');
            if (SharedObjects.IsAwardBaseFilled == false)
            {
                MessageBox.Show("Please fill Award details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (Convert.ToInt32(arr[1]) > 1)
            {
                MessageBox.Show(Convert.ToString(arr[0]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                Int64 WFId = SharedObjects.WorkId;
                Int64 TransId = SharedObjects.TransactionId;
                DataSet dsResult = AwardDataOperations.CheckAwardDuplicate(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), 0, "other");
                if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    Remark_Stop rmkobj = new Remark_Stop();
                rmkobj.ShowDialog();
                }
                else
                {
                    DialogResult result3 = MessageBox.Show("Duplicate award detail found. Do you want to move it in delete dashboard?",
                "Scival",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    dsResult = AwardDataOperations.CheckAwardDuplicate(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), 1, "other");
                    TaskBoard taskobj = new TaskBoard();
                    taskobj.Show();
                    this.Dispose();
                }
                }
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            try
            {
                SharedObjects.PageIds = 10;
                string rtunStr = ChkManadtorySection("4");
                string[] arr = rtunStr.Split('~');
                if (SharedObjects.IsAwardBaseFilled == false)
                {
                    MessageBox.Show("Please fill Award details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Convert.ToInt32(arr[1]) > 1)
                {
                    MessageBox.Show(Convert.ToString(arr[0]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                    Int64 WFId = SharedObjects.WorkId;
                    Int64 TransId = SharedObjects.TransactionId;

                    DataSet dsResult =  AwardDataOperations.CheckAwardDuplicate(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), 0, "other");

                    if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        Remark_Exit rmkobj = new Remark_Exit();
                        rmkobj.ShowDialog();
                        NextAward();
                    }
                    else
                    {

                        DialogResult result3 = MessageBox.Show("Duplicate award detail found. Do you want to move it in delete dashboard?",
                            "Scival",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);
                        if (result3 == DialogResult.Yes)
                        {
                            if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                            {
                                dsResult = AwardDataOperations.CheckAwardDuplicate(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), 1, "QA");
                                if (dsResult.Tables["Result"].Rows.Count > 0)
                                {
                                    SharedObjects.WorkId = Convert.ToInt64(dsResult.Tables["Result"].Rows[0]["WORKFLOWID"]);
                                }
                                else
                                {
                                    SharedObjects.TaskBoard = null;
                                    Application.OpenForms["Awards"].Dispose();
                                    TaskBoard taskobj = new TaskBoard();
                                    taskobj.Show();
                                }
                            }
                            else
                            {
                                dsResult = AwardDataOperations.CheckAwardDuplicate(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), 1, "New");
                                if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                                {
                                    SharedObjects.WorkId = Convert.ToInt64(dsResult.Tables["Result"].Rows[0]["WORKFLOWID"]);
                                }
                            }
                            NextAward();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
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
                //dsSec = MSecobj.Mandatorysectionfunction(WFID);
                if (dsSec.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                {
                    string validstring = string.Empty;
                    if (Convert.ToString(dsSec.Tables["MSG"].Rows[0]["flag"]) == "1")
                    {
                        if (mid == "4")
                        {
                            validstring = XmlDataOperations.AwardValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        }
                        else if (mid == "5")
                        {
                            validstring = XmlDataOperations.AwardValidate(WFID, XsdPath, Convert.ToInt32(5));

                        }
                        else if (mid == "3")
                        {
                            validstring = XmlDataOperations.OpportunityValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        }
                        else if (mid == "2")
                        {
                            validstring = XmlDataOperations.FundingBodyValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        }

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
                    else
                    {
                        str = dsSec.Tables["MSG"].Rows[0]["message"].ToString();
                        str += "~";
                        str += "3";
                    }
                }
                else
                {
                    str = dsSec.Tables["ERRORCODE"].Rows[0][1].ToString();
                    str += "~";
                    str += "4";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

            return str;
        }

        private void setWorkflow()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);

                if (SharedObjects.TaskId == 1)
                {
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\Collection.png");
                }
                else if (SharedObjects.TaskId == 2)
                {
                    if (SharedObjects.Cycle == 0)
                    {
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\Quality-Check.png");
                    }
                    else if (SharedObjects.Cycle > 0)
                    {
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\Update.png");
                    }
                }
                else if (SharedObjects.TaskId == 7)
                {
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\Expiry-Check.png");
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void Awards_FormClosing(object sender, FormClosingEventArgs e)
        {
            SharedObjects.PageIds = 5;

            Remarks_Pause rmkobj = new Remarks_Pause();

            rmkobj.btncancel.Visible = false;

            rmkobj.ShowDialog();
        }

        private void Awards_Load(object sender, EventArgs e)
        {
            String path = Application.StartupPath;
            btnpause.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            btnPauseandLogoff.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            btnhold.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            btnStop.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            btnexit.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
        }

        private void button3_Click(object sender, EventArgs e)
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
                oErrorLog.WriteErrorLog(ex);
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
                oErrorLog.WriteErrorLog(ex);
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
                {
                    SharedObjects.Domain = url.Substring(0, index);
                }
                else
                {
                    SharedObjects.Domain = url;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

        }
    }
}
