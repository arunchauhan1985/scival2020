using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class OpportunityBase : UserControl
    {        
        Opportunity m_parent;
        Replace replace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();

        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");

        int UpdateOppNameTID = 0;
        string InputXmlPath = string.Empty;
        int UpdateNameTID = 0;
        int UpdateContTID = 0;

        public OpportunityBase(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
            loadInitailvalue();

            SharedObjects.DefaultLoad = "";
            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void ddlHidden_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLang_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLang_RSource_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangOppName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLmSub_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlloiMandatory_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlOppStatus_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlRepeatingOpportunity_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlpreProposalMandatory_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void loadInitailvalue()
        {
            InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                lblMsg.Visible = false;
                DataSet dsOpptunity = SharedObjects.StartWork;
                Int64 xxx = SharedObjects.WorkId;

                if (SharedObjects.StartWork != null)
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 0;  // New Opp
                    }
                    else if (SharedObjects.TaskId == 1 && SharedObjects.Cycle > 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 1;   // Update Opp
                    }
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 0;    // QA Opp will move in Genxml's New
                    }
                    else if (SharedObjects.TaskId == 7)
                    {
                        SharedObjects.TRAN_TYPE_ID = 1;    // Expiry Opp will move in Genxml's Update
                    }

                    // Fill Type Dropdown
                    DataTable temp = dsOpptunity.Tables["FundingBodyTypes"].Copy();

                    DataRow dr = temp.NewRow();
                    dr["VALUE"] = "SelectType";
                    dr["TYPE"] = "--Select Type--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlType.DataSource = temp;
                    ddlType.ValueMember = "VALUE";
                    ddlType.DisplayMember = "TYPE";

                    ddlHidden.Items.Insert(0, "false");
                    ddlHidden.Items.Insert(1, "true");
                    ddlHidden.SelectedIndex = 0;

                    ddlpreProposalMandatory.Items.Insert(0, "---Select preProposal Mandatory---");
                    ddlpreProposalMandatory.Items.Insert(1, "false");
                    ddlpreProposalMandatory.Items.Insert(2, "true");
                    ddlpreProposalMandatory.SelectedIndex = 0;

                    ddlRepeatingOpportunity.Items.Insert(0, "---Select Repeating Opportunity---");
                    ddlRepeatingOpportunity.Items.Insert(1, "false");
                    ddlRepeatingOpportunity.Items.Insert(2, "true");
                    ddlRepeatingOpportunity.SelectedIndex = 0;

                    ddlloiMandatory.Items.Insert(0, "---Select loi Mandatory---");
                    ddlloiMandatory.Items.Insert(1, "false");
                    ddlloiMandatory.Items.Insert(2, "true");
                    ddlloiMandatory.SelectedIndex = 0;

                    ddlOppStatus.Items.Insert(0, "Active");
                    ddlOppStatus.Items.Insert(1, "Inactive");
                    ddlOppStatus.SelectedIndex = 0;

                    ddlLmSub.Items.Insert(0, "LIMITED");
                    ddlLmSub.Items.Insert(1, "NOTLIMITED");
                    ddlLmSub.Items.Insert(2, "NOTSPECIFIED");

                    ddlLmSub.SelectedIndex = 0;

                    DataTable tempOppName = dsOpptunity.Tables["LanguageTable"].Copy();
                    DataTable tempRSource = dsOpptunity.Tables["LanguageTable"].Copy();
                    DataTable tempKeyword = dsOpptunity.Tables["LanguageTable"].Copy();
                    dr = tempOppName.NewRow();
                    dr["LANGUAGE_CODE"] = "SelectLanguage";
                    dr["LANGUAGE_NAME"] = "--Select Language--";
                    tempOppName.Rows.InsertAt(dr, 0);

                    ddlLangOppName.DataSource = tempOppName;
                    ddlLangOppName.ValueMember = "LANGUAGE_CODE";
                    ddlLangOppName.DisplayMember = "LANGUAGE_NAME";
                    ddlLangOppName.SelectedIndex = 18;

                    dr = tempRSource.NewRow();
                    dr["LANGUAGE_CODE"] = "SelectLanguage";
                    dr["LANGUAGE_NAME"] = "--Select Language--";
                    tempRSource.Rows.InsertAt(dr, 0);

                    ddlLang_RSource.DataSource = tempRSource;
                    ddlLang_RSource.ValueMember = "LANGUAGE_CODE";
                    ddlLang_RSource.DisplayMember = "LANGUAGE_NAME";
                    ddlLang_RSource.SelectedIndex = 18;

                    dr = tempKeyword.NewRow();
                    dr["LANGUAGE_CODE"] = "SelectLanguage";
                    dr["LANGUAGE_NAME"] = "--Select Language--";
                    tempKeyword.Rows.InsertAt(dr, 0);
                    ddlLangKeywords.DataSource = tempKeyword;
                    ddlLangKeywords.ValueMember = "LANGUAGE_CODE";
                    ddlLangKeywords.DisplayMember = "LANGUAGE_NAME";
                    ddlLangKeywords.SelectedIndex = 18;

                    if (dsOpptunity.Tables["FundingBodyTable"].Rows.Count > 0)
                    {
                        if (dsOpptunity.Tables.Contains("Keywords") == true)
                        {
                            if (dsOpptunity.Tables["Keywords"].Rows.Count > 0)
                            {
                                for (int iAbbr = 0; iAbbr < dsOpptunity.Tables["Keywords"].Rows.Count; iAbbr++)
                                {
                                    DataView dv1;
                                    dv1 = new DataView(dsOpptunity.Tables["Keywords"].Copy());
                                    string firstCol = Convert.ToString(dv1[iAbbr]["Keywords_id"]);
                                    string secondCol = Convert.ToString(replace.ReadandReplaceHexaToChar(dv1[iAbbr]["opportunity_Keywords"].ToString(), InputXmlPath));

                                    string UpdateFunding_difflang = Convert.ToString(replace.Return_WieredChar_Original(dv1[iAbbr]["opportunity_Keywords"].ToString()));

                                    if (UpdateFunding_difflang != "")
                                    {
                                        secondCol = UpdateFunding_difflang;
                                    }

                                    string thirdCol = Convert.ToString(dv1[iAbbr]["LANG"]);

                                    if (replace.chk_OtherLang(thirdCol.ToLower()) == true)
                                    {
                                        string secondCol_difflang = replace.ConvertUnicodeToText(secondCol.ToString());

                                        if (secondCol_difflang != "")
                                        {
                                            secondCol = secondCol_difflang;
                                        }
                                    }

                                    string[] row = { firstCol, secondCol, thirdCol };

                                    if (dv1.Count == 1 && secondCol == "")
                                    { }
                                    else
                                    {
                                        dtGridKeywords.Rows.Add(row);
                                    }
                                }
                            }
                        }

                        SharedObjects.IsOpportunityBaseFilled = true;

                        txtFOId.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODyOPPORTUNITYID"]);

                        string Rsource_txtFOId = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODyOPPORTUNITYID"]));

                        if (Rsource_txtFOId != "")
                        {
                            txtFOId.Text = Rsource_txtFOId.TrimStart().TrimEnd();
                        }

                        ddlLmSub.SelectedItem = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LIMITEDSUBMISSION"]);

                        DataSet loiStatus = OpportunityDataOperations.Getopprotunity_Loi_status(SharedObjects.WorkId);

                        string Opp_loi_status = Convert.ToString(loiStatus.Tables["OpportunityCount"].Rows[0][0]);

                        string Opp_preProposalMandatory = Convert.ToString(loiStatus.Tables["OpportunityCount"].Rows[0][1]);

                        if (Opp_preProposalMandatory != "")
                        {
                            ddlpreProposalMandatory.SelectedItem = Opp_preProposalMandatory;
                        }

                        string Opp_RepeatingOpportunity = Convert.ToString(loiStatus.Tables["OpportunityCount"].Rows[0][2]);

                        if (Opp_RepeatingOpportunity != "")
                        {
                            ddlRepeatingOpportunity.SelectedItem = Opp_RepeatingOpportunity;
                        }

                        if (Opp_loi_status != "")
                        {
                            ddlloiMandatory.SelectedItem = Opp_loi_status;
                        }

                        if (dsOpptunity.Tables["FundingBodySubTypes"].Rows.Count > 0)
                        {
                            ddlType.SelectedValue = Convert.ToString(dsOpptunity.Tables["FundingBodySubTypes"].Rows[0]["ID"]);
                        }

                        txtTrust.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["TRUSTING"]);
                        string Rsource_TRUSTING = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["TRUSTING"]));

                        if (Rsource_TRUSTING != "")
                        {
                            txtTrust.Text = Rsource_TRUSTING.TrimStart().TrimEnd();
                        }

                        ddlHidden.SelectedItem = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["HIDDEN"]);

                        txtDescr.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYDESCRIPTION"]);
                        string Rsource_txtDescr = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYDESCRIPTION"]));

                        if (Rsource_txtDescr != "")
                        {
                            txtDescr.Text = Rsource_txtDescr.TrimStart().TrimEnd();
                        }

                        txtEliCatdESC.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYcategory"]);
                        string Rsource_txtEliCatdESC = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYcategory"]));

                        if (Rsource_txtEliCatdESC != "")
                        {
                            txtEliCatdESC.Text = Rsource_txtEliCatdESC.TrimStart().TrimEnd();
                        }

                        txtLinktoText.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LINKTOFULLTEXT"]);

                        string Rsource_txtLinktoText = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LINKTOFULLTEXT"]));

                        if (Rsource_txtLinktoText != "")
                        {
                            txtLinktoText.Text = Rsource_txtLinktoText.TrimStart().TrimEnd();
                        }

                        txtRecordSource.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"]).TrimStart().TrimEnd();
                        ddlLang_RSource.SelectedValue = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Lang"]);
                        string Lang_Rsource = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Lang"]);

                        if (replace.chk_OtherLang(Lang_Rsource.ToLower()) == true)
                        {
                            string Rsource_difflang = replace.ConvertUnicodeToText(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"]));

                            if (Rsource_difflang != "")
                            {
                                txtRecordSource.Text = Rsource_difflang.TrimStart().TrimEnd();
                            }
                        }

                        string Rsource_RECORDSOURCE = replace.Return_WieredChar_Original(Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"]));
                        if (Rsource_RECORDSOURCE != "")
                        {
                            txtRecordSource.Text = Rsource_RECORDSOURCE.TrimStart().TrimEnd();
                        }

                        ddlOppStatus.SelectedItem = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["OPPORTUNITYSTATUS"]);
                        txtNoofAwrad.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["NUMBEROFAWARDS"]);

                        string duration = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["DURATION"]);
                        duration = duration.Replace("P", "");

                        if (duration.IndexOf("Y") > 0)
                            txtYear.Text = duration.Substring(0, duration.IndexOf("Y"));

                        if (duration.IndexOf("M") > 0 && duration.IndexOf("Y") > 0)
                            txtMonth.Text = duration.Substring(duration.IndexOf("Y") + 1, duration.IndexOf("M") - duration.IndexOf("Y") - 1);
                        else if (duration.IndexOf("M") > 0 && duration.IndexOf("Y") < 0)
                            txtMonth.Text = duration.Substring(0, duration.IndexOf("M"));

                        if (duration.IndexOf("D") > 0 && duration.IndexOf("M") > 0)
                            txtdays.Text = duration.Substring(duration.IndexOf("M") + 1, duration.IndexOf("D") - duration.IndexOf("M") - 1);
                        else if (duration.IndexOf("D") > 0 && duration.IndexOf("M") < 0 && duration.IndexOf("Y") > 0)
                            txtdays.Text = duration.Substring(duration.IndexOf("Y") + 1, duration.IndexOf("D") - duration.IndexOf("Y") - 1);
                        else if (duration.IndexOf("D") > 0 && duration.IndexOf("M") < 0 && duration.IndexOf("Y") < 0)
                            txtdays.Text = duration.Substring(0, duration.IndexOf("D"));

                        txtLmSubDesc.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LIMITEDSUBMISSIONDESCRIPTION"]);

                        string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["OPPORTUNITY_ID"]);
                        DataSet dsLoadOppLang = OpportunityDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                        DataView dv;

                        // CANONICAL NAME
                        dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                        dv.RowFilter = "column_id='5'";

                        if (dv.Count > 0)
                        {
                            if (dv.Count > 0)
                            {

                                for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                                {
                                    string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                    string secondCol = Convert.ToString(replace.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath));
                                    string Updateopp_difflang = Convert.ToString(replace.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));

                                    if (Updateopp_difflang != "")
                                    {
                                        secondCol = Updateopp_difflang;
                                    }

                                    string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());

                                    if (replace.chk_OtherLang(thirdCol.ToLower()) == true)
                                    {
                                        string secondCol_difflang = replace.ConvertUnicodeToText(secondCol.ToString());

                                        if (secondCol_difflang != "")
                                        {
                                            secondCol = secondCol_difflang;

                                            string Updateopp_secondCol_difflang = Convert.ToString(replace.Return_WieredChar_Original(secondCol.ToString()));

                                            if (Updateopp_secondCol_difflang != "")
                                            {
                                                secondCol = Updateopp_secondCol_difflang;
                                            }
                                        }
                                    }

                                    string[] row = { firstCol, secondCol, thirdCol };

                                    if (dv.Count == 1 && secondCol == "")
                                    { }
                                    else
                                    {
                                        dtGridName.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SharedObjects.IsOpportunityBaseFilled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnaddURL_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        protected bool CheckUrlExists(string url)
        {
            if (!url.Contains("http://"))
            {
                url = "http://" + url;
            }
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
                return false;
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if ((ddlOppStatus.SelectedIndex == 1) && (OpportunityDataOperations.CheckOppRecStatus(SharedObjects.WorkId).ToString().ToLower() == "true".ToLower()))
            {
                MessageBox.Show("This is Recuuring Opportunity ,Check Opportunity Status", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            string url_txtLinktoText = txtLinktoText.Text.TrimStart().TrimEnd();
            string url_txtNoofAwrad = txtNoofAwrad.Text.TrimStart().TrimEnd();
            string url_txtYear = txtYear.Text.TrimStart().TrimEnd();
            string url_txtMonth = txtMonth.Text.TrimStart().TrimEnd();
            string url_txtdays = txtdays.Text.TrimStart().TrimEnd();
            string url_txtLinkUrl = txtRecordSource.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_txtLinktoText.Contains("http://") || url_txtNoofAwrad.Contains("http://") || url_txtYear.Contains("http://") || url_txtMonth.Contains("http://") || url_txtdays.Contains("http://") ||
                    url_txtLinktoText.Contains("https://") || url_txtNoofAwrad.Contains("https://") || url_txtYear.Contains("https://") || url_txtMonth.Contains("https://") || url_txtdays.Contains("https://") ||
                    url_txtLinktoText.Contains("www.") || url_txtNoofAwrad.Contains("www.") || url_txtYear.Contains("www./") || url_txtMonth.Contains("www.") || url_txtdays.Contains("www."))
                {
                    MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (url_txtLinkUrl.Contains(".docx") || url_txtLinkUrl.Contains(".doc") || url_txtLinkUrl.Contains(".pdf") || url_txtLinkUrl.Contains(".DOCX") || url_txtLinkUrl.Contains(".DOC") || url_txtLinkUrl.Contains(".PDF"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int p_workflowid = Convert.ToInt32(SharedObjects.WorkId);
                    Int64 p_insdel = 0;

                    InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                    String ErrMsg = "";

                    try
                    {
                        lblMsg.Visible = false;
                        Regex strRgx = new Regex(@"[A-Za-z ]");
                        string OPName = Regex.Replace(txtName.Text,@"[A-Za-z ]", "");
                        string FBOPID = Regex.Replace(txtFOId.Text,@"[A-Za-z ]", "");
                        string OPPname = "";
                        string RcordSource = "";
                        string Rsource_OtherLang = "";

                        Regex intRgx = new Regex(@"^[0-9]+");

                        if (dtGridName.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dtGridName.Rows)
                            {
                                string langValue = row.Cells["Name_language_code"].Value.ToString();

                                if (langValue.ToLower() == "en")
                                {
                                    OPPname = row.Cells["Name_desc"].Value.ToString().Trim();
                                }
                            }
                            if (OPPname == "")
                            {
                                string langValue = dtGridName.Rows[0].Cells["Name_language_code"].Value.ToString();

                                if (replace.chk_OtherLang(langValue) == true)
                                {
                                    OPPname = replace.ConvertTextToUnicode(dtGridName.Rows[0].Cells["Name_desc"].Value.ToString());
                                }
                                else
                                {
                                    OPPname = dtGridName.Rows[0].Cells["Name_desc"].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter At least one name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (txtLinktoText.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtLinktoText.Text.Trim(), "Link to full text");

                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (txtLmSubDesc.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtLmSubDesc.Text.Trim(), "Limited Submission description");

                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (ddlType.SelectedIndex == 0 || ddlType.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (txtFOId.Text == "" || (pattern.Matches(FBOPID).Count > 0))
                        {
                            MessageBox.Show("Please enter valid Funding Body opportunity ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else if (txtNoofAwrad.Text != "" && (!intRgx.IsMatch(txtNoofAwrad.Text)))
                        {
                            MessageBox.Show("Please enter numeric value in No. Of Award.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else if (txtRecordSource.Text == "")
                        {
                            MessageBox.Show("Please enter Record Source.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            DataSet dsOpptunity = SharedObjects.StartWork;

                            DataTable dtResult = new DataTable();
                            dtResult.Columns.Add("WFID");
                            dtResult.Columns.Add("LIMITEDSUBMISSION");
                            dtResult.Columns.Add("TRUSTING");
                            dtResult.Columns.Add("COLLECTIONCODE");
                            dtResult.Columns.Add("HIDDEN");
                            dtResult.Columns.Add("NAME");
                            dtResult.Columns.Add("DUEDATEDESCRIPTION");
                            dtResult.Columns.Add("ELIGIBILITYDESCRIPTION");
                            dtResult.Columns.Add("ELIGIBILITYCATEGORY");
                            dtResult.Columns.Add("LINKTOFULLTEXT");
                            dtResult.Columns.Add("OPPORTUNITYSTATUS");
                            dtResult.Columns.Add("NUMBEROFAWARDS");
                            dtResult.Columns.Add("DURATION");
                            dtResult.Columns.Add("LIMITEDSUBMISSIONDESCRIPTION");
                            dtResult.Columns.Add("RAWTEXT");
                            dtResult.Columns.Add("TYPEID");
                            dtResult.Columns.Add("URL");
                            dtResult.Columns.Add("mode");
                            dtResult.Columns.Add("FOIF");
                            dtResult.Columns.Add("RECORDSOURCE");
                            dtResult.Columns.Add("LOIMANDATORY");
                            dtResult.Columns.Add("preProposalMandatory");
                            dtResult.Columns.Add("repeatingOpportunity");
                            dtResult.Columns.Add("lang_RSource");

                            DataRow dr = dtResult.NewRow();
                            dr[0] = Convert.ToString(SharedObjects.WorkId);

                            if (Convert.ToString(ddlLmSub.SelectedItem) != "SelectLMBSUB")
                                dr[1] = Convert.ToString(ddlLmSub.SelectedItem);
                            else
                                dr[1] = "";

                            dr[2] = txtTrust.Text;
                            dr[3] = Convert.ToString("Aptara");
                            dr[4] = Convert.ToString(ddlHidden.SelectedItem);
                            dr[5] = Convert.ToString(replace.ReadandReplaceCharToHexa(OPPname, InputXmlPath));
                            dr[6] = "";

                            string s = Convert.ToString(txtDescr.Text.Trim());
                            char last;

                            if (s == null || s == "")
                            {
                                dr[7] = Convert.ToString(txtDescr.Text.Trim());
                            }
                            else
                            {
                                last = s[s.Length - 1];
                                if (last == ' ')
                                {
                                    dr[7] = s.Remove(s[s.Length - 1]);
                                }
                                else
                                {
                                    dr[7] = s;
                                }

                                if (s[0] == ' ')
                                {
                                    dr[7] = s.Remove(s[0]);
                                }
                                else
                                {
                                    dr[7] = s;
                                }
                            }

                            dr[8] = Convert.ToString(txtEliCatdESC.Text.Trim());
                            dr[9] = Convert.ToString(txtLinktoText.Text.Trim());
                            dr[10] = Convert.ToString(ddlOppStatus.SelectedItem);
                            dr[11] = Convert.ToString(txtNoofAwrad.Text.Trim());

                            string duration = string.Empty;

                            if (txtYear.Text.Trim() != "" && Convert.ToInt64(txtYear.Text.Trim()) > 0)
                                duration = txtYear.Text.Trim() + "Y";
                            if (txtMonth.Text.Trim() != "" && Convert.ToInt64(txtMonth.Text.Trim()) > 0)
                                duration = duration + txtMonth.Text.Trim() + "M";
                            if (txtdays.Text.Trim() != "" && Convert.ToInt64(txtdays.Text.Trim()) > 0)
                                duration = duration + txtdays.Text.Trim() + "D";
                            if (duration.Length >= 2)
                            {
                                dr[12] = Convert.ToString("P" + duration);
                            }
                            else
                            {
                                dr[12] = null;
                            }

                            dr[13] = Convert.ToString(txtLmSubDesc.Text.Trim());
                            dr[14] = "";

                            if (Convert.ToString(ddlType.SelectedValue) != "SelectType")
                                dr[15] = Convert.ToString(ddlType.SelectedValue);
                            else
                                dr[15] = "";

                            dr[16] = SharedObjects.CurrentUrl;

                            if (SharedObjects.IsOpportunityBaseFilled)
                                dr[17] = "1";
                            else
                                dr[17] = "0";

                            dr[18] = Convert.ToString(txtFOId.Text.Trim());

                            if (txtRecordSource.Text.Trim() != "")
                            {
                                string langValue = ddlLang_RSource.SelectedValue.ToString();

                                if (replace.chk_OtherLang(langValue) == true)
                                {
                                    Rsource_OtherLang = replace.ConvertTextToUnicode(txtRecordSource.Text.TrimStart().TrimEnd());
                                    if (Rsource_OtherLang != "")
                                    {
                                        RcordSource = Rsource_OtherLang;
                                        dr[19] = Rsource_OtherLang;
                                    }
                                    else
                                    {
                                        dr[19] = Convert.ToString(txtRecordSource.Text.TrimStart().TrimEnd());
                                    }
                                }
                                else
                                {
                                    dr[19] = Convert.ToString(txtRecordSource.Text.TrimStart().TrimEnd());
                                }
                            }

                            if (ddlloiMandatory.SelectedIndex == 0)
                            {
                                dr[20] = "";
                            }
                            else
                            {
                                dr[20] = Convert.ToString(ddlloiMandatory.SelectedItem);
                            }

                            if (ddlpreProposalMandatory.SelectedIndex == 0)
                            {
                                dr[21] = "";
                            }
                            else
                            {
                                dr[21] = Convert.ToString(ddlpreProposalMandatory.SelectedItem);
                            }

                            if (ddlRepeatingOpportunity.SelectedIndex == 0)
                            {
                                dr[22] = "";
                            }
                            else
                            {
                                dr[22] = Convert.ToString(ddlRepeatingOpportunity.SelectedItem);
                            }

                            if (ddlLang_RSource.SelectedIndex == 0)
                            {
                                dr[23] = "en";
                            }
                            else
                            {
                                dr[23] = Convert.ToString(ddlLang_RSource.SelectedValue);
                            }

                            dtResult.Rows.Add(dr);

                            DataSet ds = OpportunityDataOperations.SaveOpportunity(dtResult);

                            DataTable dtResultLang = new DataTable();
                            DataSet dsresult = new DataSet();

                            DataRow drLang;
                            DataView dv;
                            string OppID = string.Empty;
                            int LangId = 0;

                            int wId = Convert.ToInt32(SharedObjects.WorkId);
                            DataSet ds1 = OpportunityDataOperations.getWorkFlowDetails(wId);

                            #region Save Keyword Data
                            foreach (DataGridViewRow row in dtGridKeywords.Rows)
                            {
                                string Keyword_Value = "";
                                dv = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                                dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangKeywords.SelectedValue) + "'";
                                LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);
                                string LANGUAGE_Code = row.Cells[2].Value.ToString();
                                Keyword_Value = row.Cells[1].Value.ToString();

                                if (replace.chk_OtherLang(LANGUAGE_Code) == true && Keyword_Value != "")
                                {

                                    Keyword_Value = replace.ConvertTextToUnicode(Keyword_Value);
                                }

                                if (Keyword_Value != "")
                                    dsresult = AwardDataOperations.SaveArrayKeywordswithlang(p_workflowid, p_insdel, LANGUAGE_Code, Keyword_Value);

                                lblMsg.Visible = true;
                            }
                            #endregion

                            DataSet dsLoadOppLangData = OpportunityDataOperations.LoadLanguageData(Convert.ToString(ds1.Tables["WFlowTable"].Rows[0]["ID"]), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                            dv = new DataView(dsLoadOppLangData.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='5'";

                            if (dv.Count > 0)
                            {
                                UpdateOppNameTID = Convert.ToInt32(dv[0]["tran_id"]);
                            }
                            else
                            {
                                UpdateOppNameTID = 0;
                            }

                            dtResultLang.Columns.Add("TRAN_ID");
                            dtResultLang.Columns.Add("OPP_ID");
                            dtResultLang.Columns.Add("COLUMN_DESC");
                            dtResultLang.Columns.Add("COLUMN_ID");
                            dtResultLang.Columns.Add("MODE_ID");
                            dtResultLang.Columns.Add("LANGUAGE_ID");
                            dtResultLang.Columns.Add("TRAN_TYPE_ID");
                            dtResultLang.Columns.Add("FLAG_IN");

                            try
                            {
                                OppID = Convert.ToString(ds.Tables["FundingBodyTable"].Rows[0]["OPPORTUNITY_ID"]);
                            }
                            catch
                            {
                                OppID = Convert.ToString(SharedObjects.ID);
                            }

                            //  NAME
                            if (dtGridName.RowCount > 0)
                            {
                                foreach (DataGridViewRow row in dtGridName.Rows)
                                {
                                    dv = new DataView(dsLoadOppLangData.Tables["LanguageData"].Copy());
                                    string valueLang = row.Cells["Name_language_code"].Value.ToString().Trim();

                                    if (replace.chk_OtherLang(valueLang.ToLower()) == true)
                                    {
                                        dv.RowFilter = "column_id='5'and column_desc='" + replace.ConvertTextToUnicode(row.Cells["Name_desc"].Value.ToString()) + "'" + " and Language_code=" + "'" + valueLang + "'";
                                    }
                                    else
                                    {
                                        dv.RowFilter = "column_id='5'and column_desc='" + replace.ReadandReplaceCharToHexa(row.Cells["Name_desc"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'" + " and Language_code=" + "'" + valueLang + "'";
                                    }

                                    if (dv.Count > 0)
                                    {
                                        UpdateNameTID = Convert.ToInt32(dv[0]["tran_id"]);
                                    }
                                    else
                                    {
                                        UpdateNameTID = 0;
                                    }

                                    dv = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                                    dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(valueLang) + "'";
                                    LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                    drLang = dtResultLang.NewRow();
                                    drLang[1] = OppID;

                                    if (replace.chk_OtherLangId(LangId) == true)
                                    {
                                        drLang[2] = replace.ConvertTextToUnicode(row.Cells[1].Value.ToString());
                                    }
                                    else
                                    {
                                        drLang[2] = replace.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);
                                    }

                                    drLang[3] = 5;
                                    drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                    drLang[5] = LangId;
                                    drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);

                                    if (UpdateNameTID > 0)
                                    {
                                        drLang[0] = UpdateNameTID;
                                        drLang[7] = 2;
                                    }
                                    else
                                    {
                                        drLang[0] = 0;
                                        drLang[7] = 1;
                                    }

                                    dtResultLang.Rows.Add(drLang);
                                }

                                DataSet dsLang = OpportunityDataOperations.SaveOpportunityLang(dtResultLang);
                            }

                            m_parent.GetProcess();

                            ErrMsg = Convert.ToString(ds.Tables["ERRORCODE"].Rows[0][1]);
                            SharedObjects.IsOpportunityBaseFilled = true;

                            txtTrust.Text = Convert.ToString(ds.Tables["fundingbodytable"].Rows[0]["trusting"]);

                            DataTable dt = ds.Tables["FundingBodyTable"];
                            DataTable dtCopy = dt.Copy();
                            DataTable dtx = ds.Tables["FundingBodyTable1"];
                            DataTable dtCopyx = dtx.Copy();
                            DataTable dtLoi = ds.Tables["FundingBodyTable2"];
                            DataTable dtCopyLoi = dtLoi.Copy();
                            DataTable dtDue = ds.Tables["FundingBodyTable3"];
                            DataTable dtCopyDue = dtDue.Copy();
                            DataSet dsFund = SharedObjects.StartWork;

                            dsOpptunity.Tables.Remove("FundingBodyTable");
                            dsOpptunity.Tables.Add(dtCopy);

                            dsOpptunity.Tables.Remove("FundingBodySubTypes");
                            dtCopyx.TableName = "FundingBodySubTypes";
                            dsOpptunity.Tables.Add(dtCopyx);

                            dsOpptunity.Tables.Remove("LOIDate");
                            dtCopyLoi.TableName = "LOIDate";
                            dsOpptunity.Tables.Add(dtCopyLoi);

                            dsOpptunity.Tables.Remove("DueDate");
                            dtCopyDue.TableName = "DueDate";
                            dsOpptunity.Tables.Add(dtCopyDue);

                            SharedObjects.StartWork = dsOpptunity;

                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess_update("opportunity");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }

                            txtRecordSource.Text = url_txtLinkUrl.TrimStart().TrimEnd();

                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                    finally
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = ErrMsg;
                    }
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpportunityBase_MouseHover(object sender, EventArgs e)
        {
            this.ddlLang.Focus();
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {
            this.ddlLang.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;

            txtRecordSource.Text = SharedObjects.CurrentUrl.TrimStart().TrimEnd();
        }

        private void btnAddName_Click(object sender, EventArgs e)
        {
            string url_txtName = txtName.Text.TrimStart().TrimEnd();

            if (url_txtName.Contains("http://") || url_txtName.Contains("https://") || url_txtName.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;

                    Regex strRgx = new Regex(@"[A-Za-z ]");
                    string oppName = Regex.Replace(txtName.Text,@"[A-Za-z ]", "");

                    if ((pattern.Matches(oppName).Count > 0))
                    {
                        MessageBox.Show("Please enter valid Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtName.Text == "")
                    {
                        MessageBox.Show("Please enter  Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlLangOppName.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dtGridName.Rows)
                        {
                            string NameValue = row.Cells["Name_desc"].Value.ToString();
                            string langValue = row.Cells["Name_language_code"].Value.ToString().Trim();

                            NameValue = NameValue.TrimEnd().TrimStart();

                            if ((langValue == Convert.ToString(ddlLangOppName.SelectedValue)))
                            {
                                MessageBox.Show(" Name can't  with same language", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        string ddlNameID = Convert.ToString(ddlLangOppName.SelectedValue);
                        string firstColum = Convert.ToString(0);
                        string secondColum = txtName.Text.TrimStart().TrimEnd();
                        string thirdColum = ddlNameID.ToLower();
                        string[] rowGrid = { firstColum, secondColum, thirdColum };

                        dtGridName.Rows.Add(rowGrid);

                        ddlLangOppName.SelectedIndex = 18;
                        txtName.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void dtGridName_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 3)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("OPPORTUNITY_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODE_ID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");
                DataSet dsOpptunity = SharedObjects.StartWork;

                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["OPPORTUNITY_ID"]);
                    DataSet dsLoadOppLang = OpportunityDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());

                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value.ToString());
                        UpdateNameTID = Convert.ToInt32(cloTranID);
                    }
                    else
                    {
                        UpdateNameTID = 0;
                    }
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;

                if (UpdateNameTID > 0)
                {
                    drLang[0] = UpdateNameTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = OpportunityDataOperations.SaveOpportunityLang(dtDelLang);

                    if (this.dtGridName.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridName.Rows.RemoveAt(this.dtGridName.Rows[e.RowIndex].Index);
                    }

                    lblMsg.Visible = true;
                    lblMsg.Text = "Name deleted successfully.";
                }
                else
                {
                    if (this.dtGridName.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridName.Rows.RemoveAt(this.dtGridName.Rows[e.RowIndex].Index);
                    }

                    lblMsg.Visible = true;
                    lblMsg.Text = "Name deleted successfully.";
                }

                dtGridName.Refresh();
            }
        }

        private void dtGridName_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) { }

        private void btnOppNameupdate_Click(object sender, EventArgs e)
        {
            string url_txtName = txtName.Text.TrimStart().TrimEnd();

            if (url_txtName.Contains("http://") || url_txtName.Contains("https://") || url_txtName.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string aw_upatename = string.Empty;
                string txnupdateID = string.Empty;
                string awlanguageID = string.Empty;

                aw_upatename = txtName.Text.ToString().TrimEnd().TrimStart();
                txnupdateID = SharedObjects.FundingBodyContextName.ToString();
                awlanguageID = ddlLangOppName.SelectedValue.ToString();
                DataSet updatetile = new DataSet();

                if (txnupdateID.Length > 0 && aw_upatename.Length > 0)
                {
                    updatetile = OpportunityDataOperations.updatetitle_lang_bytxnid(Convert.ToInt64(txnupdateID), aw_upatename, awlanguageID);

                    lblMsg.Visible = true;
                    lblMsg.Text = "update successfully";
                    string awID = SharedObjects.FundingBodyContextName.ToString();

                    DataSet dsLoadOppLang = OpportunityDataOperations.LoadLanguageData(Convert.ToString(SharedObjects.upOppID), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                    DataView dv;

                    dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                    dv.RowFilter = "column_id='5'";
                    dtGridName.DataSource = null;
                    dtGridName.Rows.Clear();
                    dtGridName.Refresh();

                    if (dv.Count > 0)
                    {
                        for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                        {
                            string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                            string secondCol = Convert.ToString(replace.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath).TrimStart().TrimEnd());
                            string Updateopp_secondCol_difflang = Convert.ToString(replace.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString().TrimStart().TrimEnd()));

                            if (Updateopp_secondCol_difflang != "")
                            {
                                secondCol = Updateopp_secondCol_difflang;
                            }

                            string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());
                            string[] row = { firstCol, secondCol, thirdCol };

                            if (dv.Count == 1 && secondCol == "")
                            { }
                            else
                            {
                                dtGridName.Rows.Add(row);
                            }
                        }
                    }

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
                
                txtName.Text = "";
                SharedObjects.FundingBodyContextName = "";
            }
        }

        private void dtGridName_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) { }

        private void dtGridName_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 2 || e.ColumnIndex == 1)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("FUNDINGBODY_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODE_ID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");

                DataSet dsOpptunity = SharedObjects.StartWork;
                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["OPPORTUNITY_ID"]);
                    DataSet dsLoadOppLang = OpportunityDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                    DataSet dsLoadFundLang = OpportunityDataOperations.LoadLanguageData(Convert.ToString(SharedObjects.ID), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());

                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value.ToString());
                        txtName.Text = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["Name_desc"].Value.ToString());
                        ddlLangOppName.SelectedValue = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["Name_language_code"].Value.ToString());
                        UpdateContTID = Convert.ToInt32(cloTranID);

                        SharedObjects.FundingBodyContextName = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value);
                        SharedObjects.upOppID = oppID.ToString();
                    }
                }
                else
                {
                    UpdateContTID = 0;
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;

                dtGridName.Refresh();
            }
        }

        private void txtLinktoText_TextChanged(object sender, EventArgs e) { }

        private void btnKeywords_Click(object sender, EventArgs e)
        {
            string url_txtKeywords = txtKeywords.Text.TrimStart().TrimEnd();

            if (url_txtKeywords.Contains("http://") || url_txtKeywords.Contains("https://") || url_txtKeywords.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;

                    if (txtKeywords.Text != "")
                    {
                        txtKeywords.Text = txtKeywords.Text.Replace(";", ",");
                    }

                    if (txtKeywords.Text == "")
                    {
                        MessageBox.Show("Please enter Keywords.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (ddlLangKeywords.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dtGridKeywords.Rows)
                        {
                            string langValue = row.Cells["Keylanguage_code"].Value.ToString();
                        }

                        string ddlContextID = Convert.ToString(ddlLangKeywords.SelectedValue);
                        string firstColum = Convert.ToString(0);
                        string secondColum = txtKeywords.Text.TrimStart().TrimEnd();
                        string thirdColum = ddlContextID.ToLower();
                        string[] rowGrid = { firstColum, secondColum, thirdColum };

                        dtGridKeywords.Rows.Add(rowGrid);

                        ddlLangKeywords.SelectedIndex = 18;
                        txtKeywords.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void dtGridKeywords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 3)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("FUNDINGBODY_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODE_ID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");

                DataSet dsLoadFundLang = OpportunityDataOperations.LoadLanguageData(Convert.ToString(SharedObjects.ID), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                DataView dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());

                DataTable dtResultLang = new DataTable();

                string LangId = "en";
                string awkeyword = "";

                int wId = Convert.ToInt32(SharedObjects.WorkId);
                DataSet ds1 = OpportunityDataOperations.getWorkFlowDetails(wId);

                LangId = Convert.ToString(dtGridKeywords.Rows[e.RowIndex].Cells["Keylanguage_code"].Value.ToString());
                awkeyword = Convert.ToString(dtGridKeywords.Rows[e.RowIndex].Cells["awkeyword"].Value.ToString());
                int cloTranID = Convert.ToInt32(dtGridKeywords.Rows[e.RowIndex].Cells["key_tran_id"].Value.ToString());

                dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangKeywords.SelectedValue) + "'";

                dsLang = AwardDataOperations.SaveArrayKeywordswithlang(wId, 1, LangId, awkeyword, cloTranID);

                if (dv.Count > 0)
                {
                    LangId = Convert.ToString(dtGridKeywords.Rows[e.RowIndex].Cells["Keylanguage_code"].Value.ToString());
                    UpdateContTID = Convert.ToInt32(cloTranID);
                }
                else
                {
                    UpdateContTID = 0;
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;

                if (UpdateContTID < 0)
                {
                }
                else
                {
                    if (this.dtGridKeywords.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridKeywords.Rows.RemoveAt(this.dtGridKeywords.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Keyword deleted successfully.";
                }
                dtGridKeywords.Refresh();
            }
        }
    }
}
