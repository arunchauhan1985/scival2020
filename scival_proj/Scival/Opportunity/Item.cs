using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class Item : UserControl
    {
        Opportunity opportunity;
        Replace replace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();

        Int64 pagemode = 0;
        DataSet dsItems;
        int rowindex = 0;
        string clickPage = "";

        public Item(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void ddlLangOppName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlRelatedItemType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;

        }

        private void LoadInitialValue()
        {
            try
            {
                DataSet dsOpptunity = SharedObjects.StartWork;
                DataTable tempOppName = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr = tempOppName.NewRow();
                dr = tempOppName.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";
                tempOppName.Rows.InsertAt(dr, 0);

                ddlLangOppName.DataSource = tempOppName;
                ddlLangOppName.ValueMember = "LANGUAGE_CODE";
                ddlLangOppName.DisplayMember = "LANGUAGE_NAME";
                ddlLangOppName.SelectedIndex = 18;
                lblMsg.Visible = false;
                clickPage = SharedObjects.FundingClickPage.ToLower();

                if (clickPage == "about")
                {
                    pagemode = 1;
                    grpboxAdd.Text = "Add About";
                    grpboxGrid.Text = "About";
                }
                else if (clickPage == "Funding Policy")
                {
                    pagemode = 2;
                    grpboxAdd.Text = "Add Funding Policy";
                    grpboxGrid.Text = "Funding Policy";

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }
                else if (clickPage == "geoscope")
                {
                    pagemode = 3;
                    grpboxAdd.Text = "Add Geoscope";
                    grpboxGrid.Text = "Geoscope";
                }
                else if (clickPage == "related items")
                {
                    pagemode = 4;
                    grpboxAdd.Text = "Add Related Items";
                    grpboxGrid.Text = "Related Items";
                }
                else if (clickPage == "synopsis")
                {
                    pagemode = 5;
                    grpboxAdd.Text = "Add Synopsis";
                    grpboxGrid.Text = "Synopsis";
                    lblReltype.Visible = false;
                    ddlRelatedItemType.Visible = false;

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                    label1.Visible = false;
                }
                else if (clickPage == "Region")
                {
                    pagemode = 7;
                    grpboxAdd.Text = "Add Region";
                    grpboxGrid.Text = "Region";
                }
                else if (clickPage == "eligibilitydescription")
                {
                    pagemode = 8;
                    grpboxAdd.Text = "Add Eligibility Description";
                    grpboxGrid.Text = "EligibilityDescription";

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }

                else if (clickPage == "limitedsubmissiondescription")
                {
                    pagemode = 9;
                    grpboxAdd.Text = "Add LimitedSubmission Description";
                    grpboxGrid.Text = "LimitedSubmission Description";

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }
                else if (clickPage == "estimatedamountdescription")
                {
                    pagemode = 10;
                    grpboxAdd.Text = "Add estimatedAmount Description";
                    grpboxGrid.Text = "EstimatedAmount Description";
                }
                else if (clickPage == "duration")
                {
                    pagemode = 13;
                    grpboxAdd.Text = "Duration";
                    grpboxGrid.Text = "Duration";

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }
                else if (clickPage.ToLower() == "instruction".ToLower())
                {
                    pagemode = 14;
                    grpboxAdd.Text = "instruction";
                    grpboxGrid.Text = "instruction";
                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }
                else if (clickPage.ToLower() == "licenseInformation".ToLower())
                {
                    pagemode = 15;
                    grpboxAdd.Text = "licenseInformation";
                    grpboxGrid.Text = "licenseInformation";
                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    txtdurationExpression.Visible = false;
                    grdAbout.Columns["LINK_TEXT"].Visible = false;
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public void BindGrid()
        {
            try
            {
                dsItems = OpportunityDataOperations.GetItemsList(SharedObjects.WorkId, pagemode);
                string old_est_amt_desc = OpportunityDataOperations.CheckOldEstFuDesc_OP(SharedObjects.WorkId, pagemode);
                DataTable DT = dsItems.Tables["ItemListDisplay"].Copy();

                #region bind Reltype list

                DataTable DT2 = dsItems.Tables["ItemListDDLDisplay"].Copy();

                DataRow dr = DT2.NewRow();
                dr["VALUE"] = "RELTYPE";
                dr["VALUE"] = "--Select RelType--";
                DT2.Rows.InsertAt(dr, 0);

                ddlRelatedItemType.DataSource = DT2;
                ddlRelatedItemType.DisplayMember = "VALUE";
                ddlRelatedItemType.ValueMember = "VALUE";

                ddlRelatedItemType.SelectedValue = "about";
                ddlRelatedItemType.Enabled = false;

                #endregion

                #region FOR MULTILANGUAL
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR in DT.Rows)
                    {
                        try
                        {
                            if (replace.chk_OtherLang(DR["LANG"].ToString().Trim().ToLower()) == true)
                            {
                                DR["DESCRIPTION"] = Convert.ToString(replace.ConvertUnicodeToText(DR["DESCRIPTION"].ToString()));
                                DR["LINK_TEXT"] = Convert.ToString(replace.ConvertUnicodeToText(DR["LINK_TEXT"].ToString()));
                                DR["URL"] = Convert.ToString(replace.ConvertUnicodeToText(DR["URL"].ToString()));

                                DR.AcceptChanges();
                            }

                        }
                        catch { }
                    }

                    grdAbout.AutoGenerateColumns = false;
                    grdAbout.DataSource = DT;
                }
                else
                {
                    NoRecord();
                }

                #endregion

                if (DT.Rows.Count == 0 && old_est_amt_desc != "" && (pagemode == 10 || pagemode == 9 || pagemode == 8))
                {
                    txtDescr.Text = old_est_amt_desc;
                }
            }
            catch { }
        }

        private void NoRecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("DESCRIPTION");
                dtNoRcrd.Columns.Add("URL");
                dtNoRcrd.Columns.Add("LINK_TEXT");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdAbout.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();
            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")) || ((url_txtLinkUrl.Contains("Not Available")) && pagemode == 5) || ((url_txtLinkUrl.Contains("Not Available")) && pagemode == 8))))
            {
                if (url_txtLinkText.Contains("http://") || url_txtLinkText.Contains("https://") || url_txtLinkText.Contains("www."))
                {
                    MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        string Desc = "";
                        string LinkText = "";
                        string langval = "";
                        string linkUrl = "";
                        lblMsg.Visible = false;
                        txtLinkText.Text = "NULL";

                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (txtDescr.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion

                        if (txtLinkUrl.Text == "")
                        {
                            MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtLinkText.Text == "")
                        {
                            MessageBox.Show("Please enter Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            try
                            {
                                Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                                string reltype = string.Empty;

                                reltype = "";
                                Desc = replace.EntityToUnicode(txtDescr.Text.Trim());
                                LinkText = txtLinkText.Text.Trim();
                                linkUrl = txtLinkUrl.Text.Trim();
                                langval = Convert.ToString(ddlLangOppName.SelectedValue).ToLower();

                                if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                                {
                                    Desc = replace.ConvertTextToUnicode(Desc);
                                    LinkText = replace.ConvertTextToUnicode(LinkText);
                                    linkUrl = replace.ConvertTextToUnicode(linkUrl);
                                }
                                else
                                {
                                    Desc = replace.EntityToUnicode(txtDescr.Text.Trim());
                                    LinkText = txtLinkText.Text.Trim();
                                    linkUrl = txtLinkUrl.Text.Trim();
                                }

                                DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(WFID, pagemode, 0, reltype, Desc.Trim(), linkUrl.Trim(), LinkText.Trim(), langval, 0);

                                if (dsresult.Tables["ItemListDisplay"].Rows.Count > 0)
                                {
                                    BindGrid();

                                    txtLinkText.Text = "";
                                    txtLinkUrl.Text = "";
                                    txtDescr.Text = "";
                                }
                                else
                                {
                                    NoRecord();
                                }

                                #region For Changing Colour in case of Update
                                if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "synopsis")
                                {
                                    opportunity.GetProcess_update("synopsis");
                                }
                                else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "eligibilitydescription")
                                {
                                    opportunity.GetProcess_update("eligibilitydescription");
                                }
                                else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "limitedsubmissiondescription")
                                {
                                    opportunity.GetProcess_update("limitedsubmissiondescription");
                                }
                                else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "estimatedamountdescription")
                                {
                                    opportunity.GetProcess_update("estimatedamountdescription");
                                }
                                else
                                {
                                    opportunity.GetProcess();
                                }
                                #endregion

                                lblMsg.Visible = true;
                                lblMsg.Text = "Record inserted/updated successfuly";

                                if (pagemode.ToString() == "8")
                                {
                                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), "Eligibility description");
                                }
                                else if (pagemode.ToString() == "5")
                                {
                                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), "synopsis");
                                }
                            }
                            catch (Exception ex)
                            {
                                oErrorLog.WriteErrorLog(ex);
                            }
                        }
                    }
                    catch { }
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtLinkUrl.Text = SharedObjects.CurrentUrl.ToString().TrimStart().TrimEnd();
        }

        private void grdAbout_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsItems.Tables["ItemListDisplay"];
                            string langval = Convert.ToString(DT.Rows[rowindex]["LANG"]);

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                txtDescr.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]));
                                txtLinkUrl.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["URL"]));
                                txtLinkText.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]));

                            }
                            else
                            {
                                txtDescr.Text = Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]);
                                txtLinkUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                                txtLinkText.Text = Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]);
                            }

                            ddlLangOppName.SelectedValue = langval;

                            BtnAdd.Visible = false;
                            btnAddURL.Visible = false;

                            btnUpdate.Visible = true;
                            btnCancel.Visible = true;
                        }
                        catch { }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for edit.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdAbout_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(SharedObjects.WorkId, pagemode, 1, null, null, null, null, null, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[grdAbout.SelectedCells[0].RowIndex]["Item_Id"]));
                            BindGrid();
                            MessageBox.Show("Record updated successfuly", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        opportunity.GetProcess();
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnaddurl_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")) || ((url_txtLinkUrl.Contains("Not Available")) && pagemode == 5) || ((url_txtLinkUrl.Contains("Not Available")) && pagemode == 8))))
            {
                if (url_txtLinkText.Contains("http://") || url_txtLinkText.Contains("https://") || url_txtLinkText.Contains("www."))
                {
                    MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    try
                    {
                        string Desc = "", LinkText = "", langval = "", LinkUrl = "";
                        lblMsg.Visible = false;

                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (txtDescr.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion

                        if (txtLinkUrl.Text == "")
                        {
                            MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtLinkText.Text == "")
                        {
                            MessageBox.Show("Please enter Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Desc = replace.EntityToUnicode(txtDescr.Text.Trim());
                            LinkText = txtLinkText.Text.Trim();
                            langval = Convert.ToString(ddlLangOppName.SelectedValue).ToLower();
                            LinkUrl = txtLinkUrl.Text.Trim();

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                Desc = replace.ConvertTextToUnicode(Desc);
                                LinkText = replace.ConvertTextToUnicode(LinkText);
                                LinkUrl = replace.ConvertTextToUnicode(LinkUrl);
                            }
                            else
                            {
                                Desc = replace.EntityToUnicode(txtDescr.Text.Trim());
                                LinkText = txtLinkText.Text.Trim();
                                LinkUrl = txtLinkUrl.Text.Trim();
                            }

                            DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(SharedObjects.WorkId, pagemode, 2, "", Desc.Trim(), LinkUrl.Trim(), LinkText.Trim(), langval, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[rowindex]["Item_Id"]));

                            BindGrid();

                            txtDescr.Text = "";
                            txtLinkUrl.Text = ""; txtLinkText.Text = "";

                            BtnAdd.Visible = true;
                            btnAddURL.Visible = true;

                            btnUpdate.Visible = false;
                            btnCancel.Visible = false;

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "synopsis")
                            {
                                opportunity.GetProcess_update("synopsis");
                            }
                            else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "eligibilitydescription")
                            {
                                opportunity.GetProcess_update("eligibilitydescription");
                            }
                            else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "limitedsubmissiondescription")
                            {
                                opportunity.GetProcess_update("limitedsubmissiondescription");
                            }
                            else if (SharedObjects.TRAN_TYPE_ID == 1 && clickPage == "estimatedamountdescription")
                            {
                                opportunity.GetProcess_update("estimatedamountdescription");
                            }
                            else
                            {
                                opportunity.GetProcess();
                            }
                            #endregion

                            lblMsg.Visible = true;
                            lblMsg.Text = "Record updated successfuly";

                            if (pagemode.ToString() == "8")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), "Eligibility description");
                            }
                            else if (pagemode.ToString() == "5")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), "synopsisn");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                txtDescr.Text = "";
                txtLinkUrl.Text = ""; txtLinkText.Text = "";

                BtnAdd.Visible = true;
                btnAddURL.Visible = true;

                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
    }
}
