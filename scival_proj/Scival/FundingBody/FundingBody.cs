using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class FundingBody : BaseForm
    {        
        //DAL.Mandatorysection MSecobj = new DAL.Mandatorysection();
        //DAL.XML XMLobj = new DAL.XML();
        DataSet dsSec;
        
        public String WorkFlow = String.Empty;
        public String FBNAME = String.Empty;
        public int urlCounter = 0;
        ErrorLog oErrorLog = new ErrorLog();
        UpdateColumn ls = new UpdateColumn();
        bool flagSplit = false;

        public FundingBody()
        {
            InitializeComponent();
            setWorkflow();
            ls.updateChangeColour = new List<string>();

            LoadInitialValue();
            try
            {
                FundingBase fundBase = new FundingBase(this);
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
                menuStrip1.Show();
                menuStrip2.Left = -100;
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
            if (SharedObjects.User.USERID == 0)
            {
                   
            }
            try
            {
                if (SharedObjects.WorkId != 0)
                {
                    if (SharedObjects.TaskId == 1 && Convert.ToInt64(SharedObjects.Cycle) > 0)
                    {
                        btnexit.Visible = false;
                    }
                    try
                    {
                        FBNAME = SharedObjects.FundingBodyName.ToString();
                        Int64 UID = Convert.ToInt64(SharedObjects.User.USERID);
                        DataSet dsSrtWrk = DashboardDataOperations.StartWork(Convert.ToInt64(SharedObjects.WorkId), UID);

                        DataSet DSLang = new DataSet();
                        DSLang = DashboardDataOperations.getLanguageDetails(2);
                        DataTable dt = DSLang.Tables["LanguageTable"];
                        DataTable dtCopy = dt.Copy();
                        dsSrtWrk.Tables.Add(dtCopy);
                        txtURL.Text = dsSrtWrk.Tables["URL"].Rows[0][0].ToString();
                        webBrowser1.Navigate(txtURL.Text);

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

                        SharedObjects.StartWork = dsSrtWrk;
                        //SharedObjects.TransactionId = Convert.ToInt64(dsSrtWrk.Tables["URL"].Rows[0]["TID"]);

                        GetProcess();
                        SharedObjects.FundingClickPage = "FundingBase";
                        SharedObjects.ClickPage = "FundingBase";
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                }                
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public void GetProcess()
        {
            DataSet DS = FundingBodyDataOperations.GetProgress(Convert.ToInt64(SharedObjects.ID));

            menuStrip1.Items.Clear();

            if (DS.Tables.Count > 0 || DS.Tables["Progress"].Rows.Count > 0)
            {
                ToolStripMenuItem obj = new ToolStripMenuItem("Open");
                menuStrip1.Items.Add(obj);

                for (Int32 i = 0; i < DS.Tables["Progress"].Rows.Count; i++)
                {
                    ToolStripMenuItem New = new ToolStripMenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString());
                    MenuItem MI = new MenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString());
                    MI.Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString();
                    MI.Click += new EventHandler(MI_Click);
                    New.Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString();
                    New.Click += new EventHandler(New_Click);
                    if (DS.Tables["Progress"].Rows[i]["FLAG"].ToString() == "1")
                    {
                        New.Checked = true;
                        MI.Checked = true;
                    }

                    obj.DropDownItems.Add(New);
                }
            }
        }

        public void GetProcess_update(string tabname)
        {
            try
            {
                DataSet DS = FundingBodyDataOperations.GetProgress(Convert.ToInt64(SharedObjects.ID));

                menuStrip1.Items.Clear();
                if (DS.Tables.Count > 0 || DS.Tables["Progress"].Rows.Count > 0)
                {
                    ToolStripMenuItem obj = new ToolStripMenuItem("Open");
                    menuStrip1.Items.Add(obj);

                    ls.updateChangeColour.Add(tabname);
                    for (Int32 i = 0; i < DS.Tables["Progress"].Rows.Count; i++)
                    {
                        ToolStripMenuItem New = new ToolStripMenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString());
                        MenuItem MI = new MenuItem(DS.Tables["Progress"].Rows[i]["TAB"].ToString());
                        MI.Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString();
                        MI.Click += new EventHandler(MI_Click);
                        New.Tag = DS.Tables["Progress"].Rows[i]["TAB"].ToString();
                        New.Click += new EventHandler(New_Click);

                        if (DS.Tables["Progress"].Rows[i]["FLAG"].ToString() == "1")
                        {
                            New.Checked = true;
                            if (tabname.ToLower().Trim() == New.Tag.ToString().ToLower())
                            {
                                New.ForeColor = System.Drawing.Color.Green;
                            }

                            for (int j = 0; j < ls.updateChangeColour.Count; j++)
                            {
                                if (New.Tag.ToString().ToLower().Trim() == ls.updateChangeColour[j].ToString().ToLower())
                                {
                                    New.ForeColor = System.Drawing.Color.Green;
                                }
                            }

                            MI.Checked = true;
                        }

                        obj.DropDownItems.Add(New);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        void MI_Click(object sender, EventArgs e)
        {
            try
            {                

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

                if (tag.Tag.ToString() == "Financial Info")
                {
                    SharedObjects.ClickPage = "FinancialInfo.aspx";
                    FinancialInfo obj = new FinancialInfo(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);

                }

                else if (tag.Tag.ToString() == "About" || tag.Tag.ToString() == "Funding Policy" || tag.Tag.ToString() == "Geoscope" || tag.Tag.ToString() == "Related Items" || tag.Tag.ToString() == "Region" || tag.Tag.ToString() == "SubRegion" || tag.Tag.ToString() == "FunderGroup" || tag.Tag.ToString() == "AwardSuccessRate Description" || tag.Tag.ToString() == "Funding Description" || tag.Tag.ToString() == "Identifier" || tag.Tag.ToString() == "Discription")
                {
                    if (tag.Tag.ToString() == "About" || tag.Tag.ToString() == "Discription")
                        SharedObjects.ClickPage = "About";
                    else if (tag.Tag.ToString() == "Funding Policy")
                        SharedObjects.ClickPage = "FundingPolicy";
                    else if (tag.Tag.ToString() == "Geoscope")
                        SharedObjects.ClickPage = "GeoScope";
                    else if (tag.Tag.ToString() == "Related Items")
                        SharedObjects.ClickPage = "RelatedItem";
                    else if (tag.Tag.ToString() == "Region")
                        SharedObjects.ClickPage = "Region";

                    else if (tag.Tag.ToString() == "SubRegion")
                        SharedObjects.ClickPage = "SubRegion";
                    else if (tag.Tag.ToString() == "FunderGroup")
                        SharedObjects.ClickPage = "FunderGroup";

                    else if (tag.Tag.ToString() == "Funding Description")
                        SharedObjects.ClickPage = "Funding Description";

                    else if (tag.Tag.ToString() == "AwardSuccessRate Description")
                        SharedObjects.ClickPage = "AwardSuccessRate Description";

                    else if (tag.Tag.ToString() == "Identifier")
                        SharedObjects.ClickPage = "Identifier";

                    Item obj = new Item(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Related Orgs")
                {
                    SharedObjects.ClickPage = "RelatedOrg.aspx";
                    RelaredOrg obj = new RelaredOrg(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Establishment Info")
                {
                    SharedObjects.ClickPage = "establish.aspx";
                    EstablishmentInfo obj = new EstablishmentInfo(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Award Statistics")
                {
                    SharedObjects.ClickPage = "AWARDSTATISTICS.aspx";
                    AwardStatistics obj = new AwardStatistics(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Funded Program Types")
                {
                    SharedObjects.ClickPage = "FundedProgramType.aspx";
                    FundedProgramType obj = new FundedProgramType(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Classification Group")
                {
                    SharedObjects.ClickPage = "ClassificationInfo.aspx";
                    Classification obj = new Classification(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Contacts" || tag.Tag.ToString() == "Officers" || tag.Tag.ToString() == "Contact Info")
                {
                    if (tag.Tag.ToString() == "Contacts")
                        SharedObjects.ClickPage = "Contact";
                    else if (tag.Tag.ToString() == "Contacts")
                        SharedObjects.ClickPage = "Officers";
                    else if (tag.Tag.ToString() == "Contacts")
                        SharedObjects.ClickPage = "ContactInfo";

                    Contact obj = new Contact(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Opportunities Source")
                {
                    SharedObjects.ClickPage = "OpportunitiesSource.aspx";
                    OPPORTUNITIESSOURCE obj = new OPPORTUNITIESSOURCE(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Awards Source")
                {
                    SharedObjects.ClickPage = "AwardsSource.aspx";
                    AWARDSSOURCE obj = new AWARDSSOURCE(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Fundingbodies Source")
                {
                    SharedObjects.ClickPage = "fundingBodyDataSource.cs";
                    fundingBodyDataSource obj = new fundingBodyDataSource(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else if (tag.Tag.ToString() == "Publications Source")
                {
                    SharedObjects.ClickPage = "publicationDataSource.cs";
                    publicationDataSource obj = new publicationDataSource(this);
                    obj.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(obj);
                }
                else
                {
                    SharedObjects.ClickPage = "FundingBase.aspx";
                    FundingBase fundBase = new FundingBase(this);
                    fundBase.Width = splitContainer1.Panel2.Width;
                    splitContainer1.Panel2.Controls.Add(fundBase);
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
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

        private void btnpause_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 5;
            //Remark rmkobj = new Remark();
            Remark_Pause rmkobj = new Remark_Pause();
            rmkobj.ShowDialog();
        }

        private void btnhold_Click(object sender, EventArgs e)
        {
        }

        private void btnPauseandLogoff_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 6;
            Remark_Pause_Logoff rmkobj = new Remark_Pause_Logoff();
            rmkobj.ShowDialog();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 8;
            Int32 WFID = Convert.ToInt32(SharedObjects.WorkId);

            string ChkHidden = FundingBodyDataOperations.CheckHiddenStatus_fb(WFID);
            if (ChkHidden == "0")
            {
                MessageBox.Show("Please check Funding Body hidden status.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string rtunStr = ChkManadtorySection("2");
            string[] arr = rtunStr.Split('~');

            if (SharedObjects.IsFundingBaseFilled == false)
            {
                MessageBox.Show("Please fill Funding Body details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (Convert.ToInt32(arr[1]) > 1)
            {
                MessageBox.Show(Convert.ToString(arr[0]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Remark_Stop rmkobj = new Remark_Stop();
                rmkobj.ShowDialog();
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            SharedObjects.PageIds = 10;
            if (SharedObjects.IsFundingBaseFilled)
            {
                Remark_Exit rmkobj = new Remark_Exit();
                rmkobj.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please fill Funding Body details.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
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
                dsSec = FundingBodyDataOperations.Mandatorysectionfunction(WFID);
                
                    string validstring = string.Empty;
                    if (Convert.ToString(dsSec.Tables["MSG"].Rows[0]["flag"]) == "1")
                    {
                        if (mid == "4")
                        {
                            validstring = XmlDataOperations.AwardValidate(WFID, XsdPath, Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
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
                //}
                //else
                //{                    
                //    str += "~";
                //    str += "4";
                //}
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

        private void FundingBody_FormClosing(object sender, FormClosingEventArgs e)
        {
            SharedObjects.PageIds = 5;
            Remark_Pause rmkobj = new Remark_Pause();

            //rmkobj.btncancel.Visible = false;
            rmkobj.ShowDialog();
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

        private void FundingBody_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            this.ActiveControl = null;
        }
    }
}
