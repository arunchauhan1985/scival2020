using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class SubjectMatter : UserControl
    {        
        DataSet dsItems;
        private Opportunity m_parent;
        Replace replace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();

        Int64 pagemode = 0;
        int rowindex = 0;
        static Int64 grdrowid = 0;

        public SubjectMatter(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
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
                BindGrid();
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public void BindGrid()
        {
            dsItems = OpportunityDataOperations.GetItemsList(SharedObjects.WorkId, 6);

            DataTable DT2 = dsItems.Tables["ItemListDDLDisplay"].Copy();

            DataRow dr = DT2.NewRow();
            dr["VALUE"] = "RELTYPE";
            dr["VALUE"] = "--Select RelType--";
            DT2.Rows.InsertAt(dr, 0);

            ddlRelatedItemType.DataSource = DT2;
            ddlRelatedItemType.DisplayMember = "VALUE";
            ddlRelatedItemType.ValueMember = "VALUE";

            ddlRelatedItemType.SelectedValue = "about";
            ddlRelatedItemType.Enabled = true;

            DataTable DT = dsItems.Tables["ItemListDisplay"].Copy();

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
                grdAbout.AutoGenerateColumns = false;
                grdAbout.DataSource = DT;
            }
            else
            {
                norecord();
            }
        }

        private void norecord()
        {
            try
            {
                grdAbout.Rows.Clear();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
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
                        string LinkUrl = "";
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
                        else
                        {
                            try
                            {
                                Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);

                                string reltype = string.Empty;

                                if (Convert.ToString(ddlRelatedItemType.SelectedValue) == "RELTYPE")
                                {
                                    reltype = "";
                                }
                                else
                                {
                                    reltype = Convert.ToString(ddlRelatedItemType.SelectedValue);
                                }

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

                                DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(WFID, 6, 0, reltype, Desc, LinkUrl, LinkText, langval, 0);

                                DataSet _resultgetdata = OpportunityDataOperations.GetItemsList(SharedObjects.WorkId, 6);

                                if (_resultgetdata.Tables["ItemListDisplay"].Rows.Count > 0)
                                {
                                    BindGrid();
                                    txtLinkText.Text = "";
                                    txtLinkUrl.Text = "";
                                    txtDescr.Text = "";
                                }
                                else
                                {
                                    norecord();
                                }

                                #region For Changing Colour in case of Update
                                if (SharedObjects.TRAN_TYPE_ID == 1)
                                {
                                    m_parent.GetProcess_update("SubjectMatter");
                                }
                                else
                                {
                                    m_parent.GetProcess();
                                }
                                #endregion

                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());

                                btnUpdate.Visible = false;
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

                            ddlRelatedItemType.SelectedValue = Convert.ToString(DT.Rows[rowindex]["reltype"]);

                            string langval = Convert.ToString(DT.Rows[rowindex]["LANG"]);

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                txtDescr.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]));
                                txtLinkUrl.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["URL"]));
                                txtLinkText.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]));
                                ddlLangOppName.SelectedValue = langval;
                            }
                            else
                            {
                                txtDescr.Text = Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]);
                                txtLinkUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                                txtLinkText.Text = Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]);
                                ddlLangOppName.SelectedValue = langval;
                            }

                            grdrowid = Convert.ToInt64(DT.Rows[rowindex]["ID"]);
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
                            DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(SharedObjects.WorkId, 6, 1, null, null, null, null, null, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[grdAbout.SelectedCells[0].RowIndex]["Item_Id"]));
                            BindGrid();                            
                        }

                        m_parent.GetProcess();
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

            PageURL objPage = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();
            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
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
                        else
                        {
                            string reltype = string.Empty;

                            if (Convert.ToString(ddlRelatedItemType.SelectedValue) == "RELTYPE")
                                reltype = "";
                            else
                                reltype = Convert.ToString(ddlRelatedItemType.SelectedValue);

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

                            DataSet dsresult = OpportunityDataOperations.SaveAndDeleteItemsList(SharedObjects.WorkId, 6, 2, reltype, Desc, LinkUrl, LinkText, langval, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[rowindex]["Item_Id"]));

                            BindGrid();
                            txtDescr.Text = "";
                            txtLinkUrl.Text = "";
                            txtLinkText.Text = "";
                            BtnAdd.Visible = true;
                            btnAddURL.Visible = true;
                            btnUpdate.Visible = false;
                            btnCancel.Visible = false;
                            lblMsg.Visible = true;
                            
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());                            
             
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess_update("SubjectMatter");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
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
