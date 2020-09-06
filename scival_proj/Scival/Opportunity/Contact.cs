using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class Contact : UserControl
    {
        DataTable dttempContact = new DataTable();

        Opportunity opportunity;
        Replace replace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();

        Int64 pageMode = 0;
        int rowIndex = 0;
        public String FormName = String.Empty;

        public Contact(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            LoadinitialVale();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void DDLCOUNTRY_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangContact_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlState_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadinitialVale()
        {
            try
            {
                lblMsg.Visible = false;
                string clickPage = SharedObjects.FundingClickPage;
                grpcontact.Text = clickPage;

                DataSet dsFBLang = SharedObjects.StartWork;
                DataTable templang = dsFBLang.Tables["LanguageTable"].Copy();
                DataRow dr1 = templang.NewRow();
                dr1 = templang.NewRow();
                dr1["LANGUAGE_CODE"] = "SelectLanguage";
                dr1["LANGUAGE_NAME"] = "--Select Language--";
                templang.Rows.InsertAt(dr1, 0);

                ddlLangContact.DataSource = templang;
                ddlLangContact.ValueMember = "LANGUAGE_CODE";
                ddlLangContact.DisplayMember = "LANGUAGE_NAME";
                ddlLangContact.SelectedIndex = 18;

                if (clickPage.ToLower() == "contacts")
                {
                    pageMode = 1;
                    grdContact.Columns["TYPE"].Visible = false;
                    grdContact.Columns["title"].Visible = false;
                    grdContact.Columns["TYPE"].Visible = false;
                    grdContact.Columns["TYPE"].Visible = false;

                }
                else if (clickPage.ToLower() == "officers")
                {
                    pageMode = 2;
                }
                else if (clickPage.ToLower() == "contactinfo")
                {
                    pageMode = 3;
                }

                DataTable ContactsList = OpportunityDataOperations.GetContactsList(SharedObjects.WorkId, pageMode);

                foreach (DataRow DC in ContactsList.Rows)
                {
                    if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                    {
                        DC["TYPE"] = Convert.ToString(replace.ConvertUnicodeToText(DC["TYPE"].ToString()));
                        DC["title"] = Convert.ToString(replace.ConvertUnicodeToText(DC["title"].ToString()));
                        DC["email"] = Convert.ToString(replace.ConvertUnicodeToText(DC["email"].ToString()));
                        DC["website_text"] = Convert.ToString(replace.ConvertUnicodeToText(DC["website_text"].ToString()));
                        DC["street"] = Convert.ToString(replace.ConvertUnicodeToText(DC["street"].ToString()));
                        DC["room"] = Convert.ToString(replace.ConvertUnicodeToText(DC["room"].ToString()));
                        DC["city"] = Convert.ToString(replace.ConvertUnicodeToText(DC["city"].ToString()));
                        DC["prefix"] = Convert.ToString(replace.ConvertUnicodeToText(DC["prefix"].ToString()));
                        DC["givenname"] = Convert.ToString(replace.ConvertUnicodeToText(DC["givenname"].ToString()));
                        DC["middlename"] = Convert.ToString(replace.ConvertUnicodeToText(DC["middlename"].ToString()));
                        DC["surname"] = Convert.ToString(replace.ConvertUnicodeToText(DC["surname"].ToString()));
                        DC["suffix"] = Convert.ToString(replace.ConvertUnicodeToText(DC["suffix"].ToString()));
                        DC["url"] = Convert.ToString(replace.ConvertUnicodeToText(DC["url"].ToString()));
                        DC.AcceptChanges();
                    }
                }

                ContactsList.AcceptChanges();

                dttempContact = ContactsList.Copy();

                if (ContactsList.Rows.Count > 0)
                {
                    grdContact.AutoGenerateColumns = false;
                    grdCntcName.AutoGenerateColumns = false;
                    grdwebsite.AutoGenerateColumns = false;
                    grdAddress.AutoGenerateColumns = false;

                    grdContact.DataSource = ContactsList;
                    grdCntcName.DataSource = ContactsList;
                    grdwebsite.DataSource = ContactsList;
                    grdAddress.DataSource = ContactsList;

                    grdContact.Visible = true;
                    grdCntcName.Visible = true;
                    grdwebsite.Visible = true;
                    grdAddress.Visible = true;
                    grdnoRcrd.Visible = false;
                }
                else
                {
                    grdContact.Visible = false;
                    grdCntcName.Visible = false;
                    grdwebsite.Visible = false;
                    grdAddress.Visible = false;
                    grdnoRcrd.Visible = true;

                    norecord();
                }

                DataSet dsFunding = SharedObjects.StartWork;

                if (dsFunding != null)
                {
                    // Fill Country Dropdown
                    DataTable temp = dsFunding.Tables["Country"].Copy();

                    DataRow dr = temp.NewRow();
                    dr["LCODE"] = "SelectCountry";
                    dr["NAME"] = "--Select Country--";
                    temp.Rows.InsertAt(dr, 0);

                    DDLCOUNTRY.DataSource = temp;
                    DDLCOUNTRY.ValueMember = "LCODE";
                    DDLCOUNTRY.DisplayMember = "NAME";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void norecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("contact");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdnoRcrd.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void txtSubmit_Click(object sender, EventArgs e)
        {
            string url_TextType = TextType.Text.TrimStart().TrimEnd();
            string url_TextTitle = TextTitle.Text.TrimStart().TrimEnd();
            string url_TextTelephone = TextTelephone.Text.TrimStart().TrimEnd();
            string url_TextFax = TextFax.Text.TrimStart().TrimEnd();
            string url_TextEmail = TextEmail.Text.TrimStart().TrimEnd();
            string url_TextPrefix = TextPrefix.Text.TrimStart().TrimEnd();
            string url_TextGivenName = TextGivenName.Text.TrimStart().TrimEnd();
            string url_TextMiddleName = TextMiddleName.Text.TrimStart().TrimEnd();
            string url_TextSurName = TextSurName.Text.TrimStart().TrimEnd();
            string url_TextLinkText = TextLinkText.Text.TrimStart().TrimEnd();
            string url_TextRoom = TextRoom.Text.TrimStart().TrimEnd();
            string url_TextStreet = TextStreet.Text.TrimStart().TrimEnd();
            string url_TextCity = TextCity.Text.TrimStart().TrimEnd();
            string url_TextPostalCode = TextPostalCode.Text.TrimStart().TrimEnd();
            string url_txtOtherState = txtOtherState.Text.TrimStart().TrimEnd();
            string url_txtLinkUrl = TextURL.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_TextType.Contains("http://") || url_TextTitle.Contains("http://") || url_TextTelephone.Contains("http://") || url_TextFax.Contains("http://") || url_TextEmail.Contains("http://") || url_TextPrefix.Contains("http://") || url_TextGivenName.Contains("http://") || url_TextMiddleName.Contains("http://") || url_TextSurName.Contains("http://") || url_TextLinkText.Contains("http://") || url_TextRoom.Contains("http://") || url_TextStreet.Contains("http://") || url_TextCity.Contains("http://") || url_txtOtherState.Contains("http://") || url_txtOtherState.Contains("http://") ||
                    url_TextType.Contains("https://") || url_TextTitle.Contains("https://") || url_TextTelephone.Contains("https://") || url_TextFax.Contains("https://") || url_TextEmail.Contains("https://") || url_TextPrefix.Contains("https://") || url_TextGivenName.Contains("https://") || url_TextMiddleName.Contains("https://") || url_TextSurName.Contains("https://") || url_TextLinkText.Contains("https://") || url_TextRoom.Contains("https://") || url_TextStreet.Contains("https://") || url_TextCity.Contains("https://") || url_txtOtherState.Contains("https://") || url_txtOtherState.Contains("https://") ||
                    url_TextType.Contains("www.") || url_TextTitle.Contains("www.") || url_TextTelephone.Contains("www.") || url_TextFax.Contains("www.") || url_TextEmail.Contains("www.") || url_TextPrefix.Contains("www.") || url_TextGivenName.Contains("www.") || url_TextMiddleName.Contains("www.") || url_TextSurName.Contains("www.") || url_TextLinkText.Contains("www.") || url_TextRoom.Contains("www.") || url_TextStreet.Contains("www.") || url_TextCity.Contains("www.") || url_txtOtherState.Contains("www.") || url_txtOtherState.Contains("http://"))
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
                        lblMsg.Visible = false;

                        if (TextType.Text.Trim() == "" && TextTitle.Text.Trim() == "" && TextTelephone.Text.Trim() == "" && TextFax.Text.Trim() == "" && TextEmail.Text.Trim() == "" && TextPrefix.Text.Trim() == "" && TextGivenName.Text.Trim() == "" && TextMiddleName.Text.Trim() == "" && TextSurName.Text.Trim() == "" && TextURL.Text.Trim() == "" && TextLinkText.Text.Trim() == "" && TextRoom.Text.Trim() == "" && TextStreet.Text.Trim() == "" && TextCity.Text.Trim() == "" && TextPostalCode.Text.Trim() == "" && ddlState.SelectedIndex == 0 && DDLCOUNTRY.SelectedIndex == 0 && txtOtherState.Text.Trim() == "")
                        {
                            MessageBox.Show("Please fill any field(s)", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (TextGivenName.Text.Trim() == "" || TextSurName.Text.Trim() == "")
                        {
                            if (TextPrefix.Text.Trim() != "" || TextMiddleName.Text.Trim() != "" || TextSuffix.Text.Trim() != "")
                            {
                                MessageBox.Show("Enter GivenName & SurName", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextURL.Text.Trim() == "")
                        {
                            if (TextLinkText.Text.Trim() != "")
                            {
                                MessageBox.Show("Please enter URL", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (DDLCOUNTRY.SelectedIndex == 0)
                        {
                            MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (TextCity.Text.Trim() == "")
                        {
                            MessageBox.Show("Please Enter Locality Information", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (TextRoom.Text.Trim() != "" || TextStreet.Text.Trim() != "" || TextCity.Text.Trim() != "" || TextPostalCode.Text.Trim() != "" || ddlState.SelectedIndex > 0)
                        {
                            if (DDLCOUNTRY.SelectedIndex == 0)
                            {
                                MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextEmail.Text.Trim() != "")
                        {
                            if (!CheckEmail(TextEmail.Text.Trim()))
                            {
                                MessageBox.Show("Please enter the valid emailID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextLinkText.Text.Trim() == "")
                        {
                            MessageBox.Show("Please enter the Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        Int64 WFID = SharedObjects.WorkId;
                        txtOtherState.Enabled = false;

                        // In Insert there is no nedd to pass second parameter
                        String Country = DDLCOUNTRY.SelectedValue.ToString();

                        if (Country.Length > 3)
                            Country = "";

                        if (TextLinkText.Text.Trim().Length == 0)
                            TextLinkText.Text = "Not Available";

                        string state = string.Empty;

                        if (ddlState.SelectedValue.ToString() != "SelectState")
                        {
                            if (ddlState.SelectedValue.ToString() == "OtherState")
                                state = Convert.ToString(txtOtherState.Text.Trim());
                            else
                                state = Convert.ToString(ddlState.SelectedValue);
                        }
                        else
                        {
                            state = "";
                        }

                        DataTable ContactDetails = null;

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            ContactDetails = OpportunityDataOperations.SaveAndDeleteContactsList(WFID, pageMode, 0, 0, replace.ConvertTextToUnicode(TextType.Text.Trim()), replace.ConvertTextToUnicode(TextTitle.Text.Trim()), TextTelephone.Text.Trim(), TextFax.Text.Trim(), replace.ConvertTextToUnicode(TextEmail.Text.Trim()), replace.ConvertTextToUnicode(TextURL.Text.Trim()), replace.ConvertTextToUnicode(TextLinkText.Text.Trim()), Country, replace.ConvertTextToUnicode(TextRoom.Text.Trim()), replace.ConvertTextToUnicode(TextStreet.Text.Trim()), replace.ConvertTextToUnicode(state), replace.ConvertTextToUnicode(TextCity.Text.Trim()), TextPostalCode.Text.Trim(), replace.ConvertTextToUnicode(TextPrefix.Text.Trim()), replace.ConvertTextToUnicode(TextGivenName.Text.Trim()), replace.ConvertTextToUnicode(TextMiddleName.Text.Trim()), replace.ConvertTextToUnicode(TextSurName.Text.Trim()), replace.ConvertTextToUnicode(TextSuffix.Text.Trim()), ddlLangContact.SelectedValue.ToString());

                            foreach (DataRow DC in ContactDetails.Rows)
                            {
                                if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                                {
                                    DC["TYPE"] = Convert.ToString(replace.ConvertUnicodeToText(DC["TYPE"].ToString()));
                                    DC["title"] = Convert.ToString(replace.ConvertUnicodeToText(DC["title"].ToString()));
                                    DC["email"] = Convert.ToString(replace.ConvertUnicodeToText(DC["email"].ToString()));
                                    DC["website_text"] = Convert.ToString(replace.ConvertUnicodeToText(DC["website_text"].ToString()));
                                    DC["street"] = Convert.ToString(replace.ConvertUnicodeToText(DC["street"].ToString()));
                                    DC["room"] = Convert.ToString(replace.ConvertUnicodeToText(DC["room"].ToString()));
                                    DC["city"] = Convert.ToString(replace.ConvertUnicodeToText(DC["city"].ToString()));
                                    DC["prefix"] = Convert.ToString(replace.ConvertUnicodeToText(DC["prefix"].ToString()));
                                    DC["givenname"] = Convert.ToString(replace.ConvertUnicodeToText(DC["givenname"].ToString()));
                                    DC["middlename"] = Convert.ToString(replace.ConvertUnicodeToText(DC["middlename"].ToString()));
                                    DC["surname"] = Convert.ToString(replace.ConvertUnicodeToText(DC["surname"].ToString()));
                                    DC["suffix"] = Convert.ToString(replace.ConvertUnicodeToText(DC["suffix"].ToString()));
                                    DC["url"] = Convert.ToString(replace.ConvertUnicodeToText(DC["url"].ToString()));

                                    DC.AcceptChanges();
                                }
                            }

                            ContactDetails.AcceptChanges();
                        }
                        else
                        {
                            ContactDetails = OpportunityDataOperations.SaveAndDeleteContactsList(WFID, pageMode, 0, 0, TextType.Text.Trim(), TextTitle.Text.Trim(), TextTelephone.Text.Trim(), TextFax.Text.Trim(), TextEmail.Text.Trim(), TextURL.Text.Trim(), TextLinkText.Text.Trim(), Country, TextRoom.Text.Trim(), TextStreet.Text.Trim(), state, TextCity.Text.Trim(), TextPostalCode.Text.Trim(), TextPrefix.Text.Trim(), TextGivenName.Text.Trim(), TextMiddleName.Text.Trim(), TextSurName.Text.Trim(), TextSuffix.Text.Trim(), ddlLangContact.SelectedValue.ToString());
                        }

                        dttempContact = ContactDetails.Copy();

                        if (ContactDetails.Rows.Count > 0)
                        {
                            grdContact.AutoGenerateColumns = false;
                            grdCntcName.AutoGenerateColumns = false;
                            grdwebsite.AutoGenerateColumns = false;
                            grdAddress.AutoGenerateColumns = false;

                            grdContact.DataSource = ContactDetails;
                            grdCntcName.DataSource = ContactDetails;
                            grdwebsite.DataSource = ContactDetails;
                            grdAddress.DataSource = ContactDetails;

                            grdContact.Visible = true;
                            grdCntcName.Visible = true;
                            grdwebsite.Visible = true;
                            grdAddress.Visible = true;
                            grdnoRcrd.Visible = false;
                        }
                        else
                        {
                            grdContact.Visible = false;
                            grdCntcName.Visible = false;
                            grdwebsite.Visible = false;
                            grdAddress.Visible = false;
                            grdnoRcrd.Visible = true;

                            norecord();
                        }

                        txtOtherState.Text = ""; DDLCOUNTRY.SelectedIndex = 0;
                        TextType.Text = ""; TextTitle.Text = ""; TextTelephone.Text = ""; TextFax.Text = "";
                        TextEmail.Text = ""; TextURL.Text = ""; TextLinkText.Text = ""; TextRoom.Text = "";
                        TextStreet.Text = ""; TextCity.Text = ""; TextPostalCode.Text = ""; TextPrefix.Text = "";
                        TextGivenName.Text = ""; TextMiddleName.Text = ""; TextSurName.Text = ""; TextSuffix.Text = "";
                        ddlState.SelectedIndex = 0;

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                            opportunity.GetProcess_update("Contactinfo");
                        else
                            opportunity.GetProcess();

                        lblMsg.Visible = true;
                        lblMsg.Text = "Contact Saved Successfully";

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
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

        private void DDLCOUNTRY_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

                // Fill State Dropdown   
                if (DDLCOUNTRY.SelectedIndex != 0)
                {
                    dsFunding.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + DDLCOUNTRY.SelectedValue + "'";
                    DataTable dtState = dsFunding.Tables["State"].DefaultView.ToTable();

                    DataRow dr = dtState.NewRow();
                    dr["CODE"] = "SelectState";
                    dr["NAME"] = "--Select State--";
                    dtState.Rows.InsertAt(dr, 0);

                    dr = dtState.NewRow();
                    dr["CODE"] = "OtherState";
                    dr["NAME"] = "Other State";
                    dtState.Rows.InsertAt(dr, dtState.Rows.Count);

                    ddlState.DataSource = dtState;
                    ddlState.ValueMember = "CODE";
                    ddlState.DisplayMember = "NAME";
                }
                else
                {
                    DataTable dtstate = new DataTable();
                    dtstate.Columns.Add("CODE");
                    dtstate.Columns.Add("NAME");

                    DataRow dr = dtstate.NewRow();
                    dr["CODE"] = "SelectState";
                    dr["NAME"] = "--Select State--";
                    dtstate.Rows.InsertAt(dr, 0);

                    ddlState.DataSource = dtstate;
                    ddlState.ValueMember = "CODE";
                    ddlState.DisplayMember = "NAME";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (ddlState.SelectedValue.ToString() == "OtherState")
                    txtOtherState.Enabled = true;
                else
                    txtOtherState.Enabled = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public bool CheckEmail(string EMAIL)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                             @".)+))([a-zA-Z]{2,10}|[0-9]{1,3})(\]?)$";

                Regex re = new Regex(strRegex);

                if (!re.IsMatch(EMAIL))
                    return false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

            return true;
        }

        private void grdContact_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dttempContact.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowIndex = e.RowIndex;

                        try
                        {
                            TextType.Text = Convert.ToString(dttempContact.Rows[rowIndex]["type"]);
                            TextTitle.Text = Convert.ToString(dttempContact.Rows[rowIndex]["title"]);
                            TextTelephone.Text = Convert.ToString(dttempContact.Rows[rowIndex]["telephone"]);
                            TextFax.Text = Convert.ToString(dttempContact.Rows[rowIndex]["fax"]);
                            TextEmail.Text = Convert.ToString(dttempContact.Rows[rowIndex]["email"]);
                            TextPrefix.Text = Convert.ToString(dttempContact.Rows[rowIndex]["prefix"]);
                            TextGivenName.Text = Convert.ToString(dttempContact.Rows[rowIndex]["givenName"]);
                            TextMiddleName.Text = Convert.ToString(dttempContact.Rows[rowIndex]["middleName"]);
                            TextSurName.Text = Convert.ToString(dttempContact.Rows[rowIndex]["surname"]);
                            TextSuffix.Text = Convert.ToString(dttempContact.Rows[rowIndex]["suffix"]);
                            TextURL.Text = Convert.ToString(dttempContact.Rows[rowIndex]["url"]);
                            TextLinkText.Text = Convert.ToString(dttempContact.Rows[rowIndex]["website_text"]);
                            TextRoom.Text = Convert.ToString(dttempContact.Rows[rowIndex]["room"]);
                            TextStreet.Text = Convert.ToString(dttempContact.Rows[rowIndex]["street"]);
                            TextCity.Text = Convert.ToString(dttempContact.Rows[rowIndex]["city"]);
                            string othetState = Convert.ToString(dttempContact.Rows[rowIndex]["statename"]).Trim();
                            string othetStatecode = Convert.ToString(dttempContact.Rows[rowIndex]["statecode"]).Trim();
                            string country = Convert.ToString(dttempContact.Rows[rowIndex]["countrycode"]).Trim();

                            DDLCOUNTRY.SelectedValue = country;

                            DataSet dsfund = SharedObjects.StartWork;
                            dsfund.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + DDLCOUNTRY.SelectedValue + "'";
                            DataTable dtOtherState = dsfund.Tables["State"].DefaultView.ToTable();

                            DataRow dr = dtOtherState.NewRow();
                            dr["CODE"] = "SelectState";
                            dr["NAME"] = "--Select State--";
                            dtOtherState.Rows.InsertAt(dr, 0);

                            dr = dtOtherState.NewRow();
                            dr["CODE"] = "OtherState";
                            dr["NAME"] = "Other State";
                            dtOtherState.Rows.InsertAt(dr, dtOtherState.Rows.Count);

                            ddlState.DataSource = dtOtherState;
                            ddlState.ValueMember = "CODE";
                            ddlState.DisplayMember = "NAME";

                            if (othetState != "")
                            {
                                DataTable dt = dtOtherState.DefaultView.ToTable();
                                DataTable dt1 = dtOtherState.DefaultView.ToTable();

                                dt.DefaultView.RowFilter = "CODE='" + othetState + "'";
                                DataTable dtStateResult = dt.DefaultView.ToTable();

                                dt1.DefaultView.RowFilter = "CODE='" + othetStatecode + "'";
                                DataTable dtStateResult1 = dt1.DefaultView.ToTable();

                                if (dtStateResult.Rows.Count > 0)
                                    ddlState.SelectedValue = Convert.ToString(dttempContact.Rows[rowIndex]["STATEcode"]);
                                else if (dtStateResult1.Rows.Count > 0)
                                    ddlState.SelectedValue = Convert.ToString(dttempContact.Rows[rowIndex]["STATEcode"]);
                                else
                                {
                                    ddlState.SelectedValue = "OtherState";
                                    txtOtherState.Enabled = true;
                                    txtOtherState.Text = Convert.ToString(dttempContact.Rows[rowIndex]["STATEcode"]);
                                }
                            }

                            TextPostalCode.Text = Convert.ToString(dttempContact.Rows[rowIndex]["postalcode"]);

                            txtSubmit.Visible = false;
                            btnaddurl.Visible = false;
                            btnupdate.Visible = true;
                            btncancel.Visible = true;
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

        private void grdContact_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dttempContact.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataTable contactDetails = OpportunityDataOperations.SaveAndDeleteContactsList(SharedObjects.WorkId, pageMode, 1, Convert.ToInt64(dttempContact.Rows[grdContact.SelectedCells[0].RowIndex]["contact_id"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                            dttempContact = contactDetails.Copy();

                            if (contactDetails.Rows.Count > 0)
                            {
                                grdContact.AutoGenerateColumns = false;
                                grdCntcName.AutoGenerateColumns = false;
                                grdwebsite.AutoGenerateColumns = false;
                                grdAddress.AutoGenerateColumns = false;

                                grdContact.DataSource = contactDetails;
                                grdCntcName.DataSource = contactDetails;
                                grdwebsite.DataSource = contactDetails;
                                grdAddress.DataSource = contactDetails;

                                grdContact.Visible = true;
                                grdCntcName.Visible = true;
                                grdwebsite.Visible = true;
                                grdAddress.Visible = true;
                                grdnoRcrd.Visible = false;
                            }
                            else
                            {
                                grdContact.Visible = false;
                                grdCntcName.Visible = false;
                                grdwebsite.Visible = false;
                                grdAddress.Visible = false;
                                grdnoRcrd.Visible = true;

                                norecord();
                            }

                            txtOtherState.Text = ""; DDLCOUNTRY.SelectedIndex = 0;
                            TextType.Text = ""; TextTitle.Text = ""; TextTelephone.Text = ""; TextFax.Text = "";
                            TextEmail.Text = ""; TextURL.Text = ""; TextLinkText.Text = ""; TextRoom.Text = "";
                            TextStreet.Text = ""; TextCity.Text = ""; TextPostalCode.Text = ""; TextPrefix.Text = "";
                            TextGivenName.Text = ""; TextMiddleName.Text = ""; TextSurName.Text = ""; TextSuffix.Text = "";
                            ddlState.SelectedIndex = 0;

                            opportunity.GetProcess();

                            lblMsg.Visible = true;
                            lblMsg.Text = "Contact Saves Successfully";
                        }
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

        private void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtSubmit.Visible = true;
                btnaddurl.Visible = true;

                btnupdate.Visible = false;
                btncancel.Visible = false;

                txtOtherState.Text = ""; DDLCOUNTRY.SelectedIndex = 0;
                TextType.Text = ""; TextTitle.Text = ""; TextTelephone.Text = ""; TextFax.Text = "";
                TextEmail.Text = ""; TextURL.Text = ""; TextLinkText.Text = ""; TextRoom.Text = "";
                TextStreet.Text = ""; TextCity.Text = ""; TextPostalCode.Text = ""; TextPrefix.Text = "";
                TextGivenName.Text = ""; TextMiddleName.Text = ""; TextSurName.Text = ""; TextSuffix.Text = "";
                ddlState.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string url_TextType = TextType.Text.TrimStart().TrimEnd();
            string url_TextTitle = TextTitle.Text.TrimStart().TrimEnd();
            string url_TextTelephone = TextTelephone.Text.TrimStart().TrimEnd();
            string url_TextFax = TextFax.Text.TrimStart().TrimEnd();
            string url_TextEmail = TextEmail.Text.TrimStart().TrimEnd();
            string url_TextPrefix = TextPrefix.Text.TrimStart().TrimEnd();
            string url_TextGivenName = TextGivenName.Text.TrimStart().TrimEnd();
            string url_TextMiddleName = TextMiddleName.Text.TrimStart().TrimEnd();
            string url_TextSurName = TextSurName.Text.TrimStart().TrimEnd();
            string url_TextLinkText = TextLinkText.Text.TrimStart().TrimEnd();
            string url_TextRoom = TextRoom.Text.TrimStart().TrimEnd();
            string url_TextStreet = TextStreet.Text.TrimStart().TrimEnd();
            string url_TextCity = TextCity.Text.TrimStart().TrimEnd();
            string url_TextPostalCode = TextPostalCode.Text.TrimStart().TrimEnd();
            string url_txtOtherState = txtOtherState.Text.TrimStart().TrimEnd();
            string url_txtLinkUrl = TextURL.Text.TrimStart().TrimEnd();

            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_TextType.Contains("http://") || url_TextTitle.Contains("http://") || url_TextTelephone.Contains("http://") || url_TextFax.Contains("http://") || url_TextEmail.Contains("http://") || url_TextPrefix.Contains("http://") || url_TextGivenName.Contains("http://") || url_TextMiddleName.Contains("http://") || url_TextSurName.Contains("http://") || url_TextLinkText.Contains("http://") || url_TextRoom.Contains("http://") || url_TextStreet.Contains("http://") || url_TextCity.Contains("http://") || url_txtOtherState.Contains("http://") || url_txtOtherState.Contains("http://") ||
                    url_TextType.Contains("https://") || url_TextTitle.Contains("https://") || url_TextTelephone.Contains("https://") || url_TextFax.Contains("https://") || url_TextEmail.Contains("https://") || url_TextPrefix.Contains("https://") || url_TextGivenName.Contains("https://") || url_TextMiddleName.Contains("https://") || url_TextSurName.Contains("https://") || url_TextLinkText.Contains("https://") || url_TextRoom.Contains("https://") || url_TextStreet.Contains("https://") || url_TextCity.Contains("https://") || url_txtOtherState.Contains("https://") || url_txtOtherState.Contains("https://") ||
                    url_TextType.Contains("www.") || url_TextTitle.Contains("www.") || url_TextTelephone.Contains("www.") || url_TextFax.Contains("www.") || url_TextEmail.Contains("www.") || url_TextPrefix.Contains("www.") || url_TextGivenName.Contains("www.") || url_TextMiddleName.Contains("www.") || url_TextSurName.Contains("www.") || url_TextLinkText.Contains("www.") || url_TextRoom.Contains("www.") || url_TextStreet.Contains("www.") || url_TextCity.Contains("www.") || url_txtOtherState.Contains("www.") || url_txtOtherState.Contains("http://"))
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
                        lblMsg.Visible = false;

                        if (TextType.Text.Trim() == "" && TextTitle.Text.Trim() == "" && TextTelephone.Text.Trim() == "" && TextFax.Text.Trim() == "" && TextEmail.Text.Trim() == "" && TextPrefix.Text.Trim() == "" && TextGivenName.Text.Trim() == "" && TextMiddleName.Text.Trim() == "" && TextSurName.Text.Trim() == "" && TextURL.Text.Trim() == "" && TextLinkText.Text.Trim() == "" && TextRoom.Text.Trim() == "" && TextStreet.Text.Trim() == "" && TextCity.Text.Trim() == "" && TextPostalCode.Text.Trim() == "" && ddlState.SelectedIndex == 0 && DDLCOUNTRY.SelectedIndex == 0 && txtOtherState.Text.Trim() == "")
                        {
                            MessageBox.Show("Please fill any field(s)", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (TextGivenName.Text.Trim() == "" || TextSurName.Text.Trim() == "")
                        {
                            if (TextPrefix.Text.Trim() != "" || TextMiddleName.Text.Trim() != "" || TextSuffix.Text.Trim() != "")
                            {
                                MessageBox.Show("Enter GivenName & SurName", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextURL.Text.Trim() == "")
                        {
                            if (TextLinkText.Text.Trim() != "")
                            {
                                MessageBox.Show("Please enter URL", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextRoom.Text.Trim() != "" || TextStreet.Text.Trim() != "" || TextCity.Text.Trim() != "" || TextPostalCode.Text.Trim() != "" || ddlState.SelectedIndex > 0)
                        {
                            if (DDLCOUNTRY.SelectedIndex == 0)
                            {
                                MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextCity.Text.Trim() == "")
                        {
                            MessageBox.Show("Please Enter Locality Information", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (TextEmail.Text.Trim() != "")
                        {
                            if (!CheckEmail(TextEmail.Text.Trim()))
                            {
                                MessageBox.Show("Please enter the valid emailID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (TextLinkText.Text.Trim() == "")
                        {
                            MessageBox.Show("Please enter the Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        Int64 WFID = SharedObjects.WorkId;

                        // In Insert there is no nedd to pass second parameter
                        string TYPE = TextType.Text.Trim();
                        string TITLE = TextTitle.Text.Trim();
                        string TELEPHONE = TextTelephone.Text.Trim();
                        string FAX = TextFax.Text.Trim();
                        string EMAIL = TextEmail.Text.Trim();
                        string url = TextURL.Text.Trim();
                        string WEBSITE_TEXT = TextLinkText.Text.Trim();
                        string COUNTRY = DDLCOUNTRY.SelectedValue.ToString();
                        string ROOM = TextRoom.Text.Trim();
                        string STREET = TextStreet.Text.Trim();

                        string STATE = string.Empty;

                        if (ddlState.SelectedValue.ToString() != "SelectState")
                        {
                            if (ddlState.SelectedValue.ToString() == "OtherState")
                                STATE = Convert.ToString(txtOtherState.Text.Trim());
                            else
                                STATE = Convert.ToString(ddlState.SelectedValue);
                        }
                        else
                            STATE = "";

                        string CITY = TextCity.Text.Trim();
                        string POSTALCODE = TextPostalCode.Text.Trim();
                        string PREFIX = TextPrefix.Text.Trim();
                        string GIVENNAME = TextGivenName.Text.Trim();
                        string MIDDLENAME = TextMiddleName.Text.Trim();
                        string SURNAME = TextSurName.Text.Trim();
                        string SUFFIX = TextSuffix.Text.Trim();

                        if (COUNTRY.Length > 3)
                            COUNTRY = "";
                        if (WEBSITE_TEXT.Trim().Length == 0)
                            WEBSITE_TEXT = "Not Available";

                        DataTable contactDetails = null;

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().ToLower()) == true)
                            contactDetails = OpportunityDataOperations.SaveAndDeleteContactsList(WFID, pageMode, 2, Convert.ToInt64(dttempContact.Rows[rowIndex]["contact_id"]), replace.ConvertTextToUnicode(TYPE), replace.ConvertTextToUnicode(TITLE), TELEPHONE, FAX, replace.ConvertTextToUnicode(EMAIL), replace.ConvertTextToUnicode(url), replace.ConvertTextToUnicode(WEBSITE_TEXT), COUNTRY, replace.ConvertTextToUnicode(ROOM), replace.ConvertTextToUnicode(STREET), STATE, replace.ConvertTextToUnicode(CITY), POSTALCODE, replace.ConvertTextToUnicode(PREFIX), replace.ConvertTextToUnicode(GIVENNAME), replace.ConvertTextToUnicode(MIDDLENAME), replace.ConvertTextToUnicode(SURNAME), replace.ConvertTextToUnicode(SUFFIX), ddlLangContact.SelectedValue.ToString());
                        else
                            contactDetails = OpportunityDataOperations.SaveAndDeleteContactsList(WFID, pageMode, 2, Convert.ToInt64(dttempContact.Rows[rowIndex]["contact_id"]), TYPE, TITLE, TELEPHONE, FAX, EMAIL, url, WEBSITE_TEXT, COUNTRY, ROOM, STREET, STATE, CITY, POSTALCODE, PREFIX, GIVENNAME, MIDDLENAME, SURNAME, SUFFIX);

                        dttempContact = contactDetails.Copy();

                        if (contactDetails.Rows.Count > 0)
                        {
                            grdContact.AutoGenerateColumns = false;
                            grdCntcName.AutoGenerateColumns = false;
                            grdwebsite.AutoGenerateColumns = false;
                            grdAddress.AutoGenerateColumns = false;

                            grdContact.DataSource = contactDetails;
                            grdCntcName.DataSource = contactDetails;
                            grdwebsite.DataSource = contactDetails;
                            grdAddress.DataSource = contactDetails;

                            grdContact.Visible = true;
                            grdCntcName.Visible = true;
                            grdwebsite.Visible = true;
                            grdAddress.Visible = true;
                            grdnoRcrd.Visible = false;
                        }
                        else
                        {
                            grdContact.Visible = false;
                            grdCntcName.Visible = false;
                            grdwebsite.Visible = false;
                            grdAddress.Visible = false;
                            grdnoRcrd.Visible = true;

                            norecord();
                        }

                        txtOtherState.Text = ""; DDLCOUNTRY.SelectedIndex = 0;
                        TextType.Text = ""; TextTitle.Text = ""; TextTelephone.Text = ""; TextFax.Text = "";
                        TextEmail.Text = ""; TextURL.Text = ""; TextLinkText.Text = ""; TextRoom.Text = "";
                        TextStreet.Text = ""; TextCity.Text = ""; TextPostalCode.Text = ""; TextPrefix.Text = "";
                        TextGivenName.Text = ""; TextMiddleName.Text = ""; TextSurName.Text = ""; TextSuffix.Text = "";
                        ddlState.SelectedIndex = 0;

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                            opportunity.GetProcess_update("Contactinfo");
                        else
                            opportunity.GetProcess();

                        lblMsg.Visible = true;
                        lblMsg.Text = "Contact Updated Successfully";

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());

                        txtSubmit.Visible = true;
                        btnaddurl.Visible = true;

                        btnupdate.Visible = false;
                        btncancel.Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            TextURL.Text = SharedObjects.CurrentUrl.TrimStart().TrimEnd();
        }
    }
}
