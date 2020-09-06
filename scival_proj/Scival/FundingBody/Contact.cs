using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class Contact : UserControl
    {
        DataTable dttempContact = new DataTable();
        
        Replace r = new Replace();
        private FundingBody m_parent;
        Int64 pagemode = 0; int rowindex = 0;
        public String FormName = String.Empty;
        string InputXmlPath = string.Empty;
        ErrorLog oErrorLog = new ErrorLog();

        public Contact(FundingBody frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadinitialVale();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
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
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

                lblMsg.Visible = false;
                string clickPage = SharedObjects.FundingClickPage;
                grpcontact.Text = clickPage;
                #region Lang Addition
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
                #endregion
                if (clickPage.ToLower() == "contacts")
                {
                    pagemode = 1;
                }
                else if (clickPage.ToLower() == "officers")
                {
                    pagemode = 2;
                }
                else if (clickPage.ToLower() == "contact info")
                {
                    pagemode = 3;
                }

                DataSet dsItems = FundingBodyDataOperations.GetContactsList(SharedObjects.WorkId, pagemode);
                if (dsItems.Tables["ContactsList"].Rows.Count > 0)
                {
                    grdContact.AutoGenerateColumns = false;
                    grdCntcName.AutoGenerateColumns = false;
                    grdwebsite.AutoGenerateColumns = false;
                    grdAddress.AutoGenerateColumns = false;

                    for (int intCount = 0; intCount < dsItems.Tables["ContactsList"].Rows.Count; intCount++)
                    {
                        if (dsItems.Tables["ContactsList"].Rows[intCount]["WEBSITE_TEXT"].ToString() != "" && dsItems.Tables["ContactsList"].Rows[intCount]["LANG"].ToString() == "en")
                        {
                            dsItems.Tables["ContactsList"].Rows[intCount]["WEBSITE_TEXT"] = r.ReadandReplaceHexaToChar(dsItems.Tables["ContactsList"].Rows[intCount]["WEBSITE_TEXT"].ToString(), InputXmlPath);
                        }
                    }

                    foreach (DataRow DC in dsItems.Tables["ContactsList"].Rows)
                    {
                        if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                        {
                            DC["TYPE"] = Convert.ToString(r.ConvertUnicodeToText(DC["TYPE"].ToString()));
                            DC["title"] = Convert.ToString(r.ConvertUnicodeToText(DC["title"].ToString()));
                            DC["email"] = Convert.ToString(r.ConvertUnicodeToText(DC["email"].ToString()));
                            DC["website_text"] = Convert.ToString(r.ConvertUnicodeToText(DC["website_text"].ToString()));
                            DC["street"] = Convert.ToString(r.ConvertUnicodeToText(DC["street"].ToString()));
                            DC["room"] = Convert.ToString(r.ConvertUnicodeToText(DC["room"].ToString()));
                            DC["city"] = Convert.ToString(r.ConvertUnicodeToText(DC["city"].ToString()));
                            DC["prefix"] = Convert.ToString(r.ConvertUnicodeToText(DC["prefix"].ToString()));
                            DC["givenname"] = Convert.ToString(r.ConvertUnicodeToText(DC["givenname"].ToString()));
                            DC["middlename"] = Convert.ToString(r.ConvertUnicodeToText(DC["middlename"].ToString()));
                            DC["surname"] = Convert.ToString(r.ConvertUnicodeToText(DC["surname"].ToString()));
                            DC["suffix"] = Convert.ToString(r.ConvertUnicodeToText(DC["suffix"].ToString()));
                            DC["url"] = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
                            DC.AcceptChanges();
                        }
                    }

                    dsItems.Tables["ContactsList"].AcceptChanges();

                    dttempContact = dsItems.Tables["ContactsList"].Copy();

                    grdContact.DataSource = dsItems.Tables["ContactsList"];
                    grdCntcName.DataSource = dsItems.Tables["ContactsList"];
                    grdwebsite.DataSource = dsItems.Tables["ContactsList"];
                    grdAddress.DataSource = dsItems.Tables["ContactsList"];

                    grdContact.Visible = false;
                    grdCntcName.Visible = false;

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
            string url_txtLinkUrl = TextURL.Text.TrimStart().TrimEnd();
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
                        InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                        lblMsg.Visible = false;
                        if (ddlLangContact.SelectedIndex == 0)
                        {
                            MessageBox.Show("Please Select Language!!!!", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (TextType.Text.Trim() == "" && TextTitle.Text.Trim() == "" && TextTelephone.Text.Trim() == "" && TextFax.Text.Trim() == "" && TextEmail.Text.Trim() == "" && TextPrefix.Text.Trim() == "" && TextGivenName.Text.Trim() == "" && TextMiddleName.Text.Trim() == "" && TextSurName.Text.Trim() == "" && TextURL.Text.Trim() == "" && TextLinkText.Text.Trim() == "" && TextRoom.Text.Trim() == "" && TextStreet.Text.Trim() == "" && TextCity.Text.Trim() == "" && TextPostalCode.Text.Trim() == "" && ddlState.SelectedIndex == 0 && DDLCOUNTRY.SelectedIndex == 0 && txtOtherState.Text.Trim() == "")
                        {
                            MessageBox.Show("Please fill any field(s)", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (TextGivenName.Text.Trim() == "" && TextSurName.Text.Trim() == "")
                        {
                            if (TextPrefix.Text.Trim() != "" || TextMiddleName.Text.Trim() != "" || TextSuffix.Text.Trim() != "")
                            {
                                MessageBox.Show("Enter GivenName and SurName", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        TextLinkText.Text = "NULL";

                        Int64 WFID = SharedObjects.WorkId;
                        String Country = DDLCOUNTRY.SelectedValue.ToString();
                        if (Country.Length > 3)
                            Country = "";
                        if (TextLinkText.Text.Trim().Length == 0)
                        {
                            TextLinkText.Text = "Not Available";
                        }

                        string state = string.Empty;
                        state = Convert.ToString(txtOtherState.Text.Trim());
                        string LinkText = r.ReadandReplaceCharToHexa(TextLinkText.Text.Trim().ToString(), InputXmlPath);

                        DataSet dsresult = null;

                        if (r.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsresult = FundingBodyDataOperations.SaveAndDeleteContactsLIst(WFID, pagemode, 0, 0, r.ConvertTextToUnicode(TextType.Text.Trim()), r.ConvertTextToUnicode(TextTitle.Text.Trim()), TextTelephone.Text.Trim(), TextFax.Text.Trim(), r.ConvertTextToUnicode(TextEmail.Text.Trim()), r.ConvertTextToUnicode(TextURL.Text.Trim()), r.ConvertTextToUnicode(LinkText), Country, r.ConvertTextToUnicode(TextRoom.Text.Trim()), r.ConvertTextToUnicode(TextStreet.Text.Trim()), state, r.ConvertTextToUnicode(TextCity.Text.Trim()), TextPostalCode.Text.Trim(), r.ConvertTextToUnicode(TextPrefix.Text.Trim()), r.ConvertTextToUnicode(TextGivenName.Text.Trim()), r.ConvertTextToUnicode(TextMiddleName.Text.Trim()), r.ConvertTextToUnicode(TextSurName.Text.Trim()), r.ConvertTextToUnicode(TextSuffix.Text.Trim()), ddlLangContact.SelectedValue.ToString());
                        }
                        else
                        {
                            dsresult = FundingBodyDataOperations.SaveAndDeleteContactsLIst(WFID, pagemode, 0, 0, TextType.Text.Trim(), TextTitle.Text.Trim(), TextTelephone.Text.Trim(), TextFax.Text.Trim(), TextEmail.Text.Trim(), TextURL.Text.Trim(), LinkText, Country, TextRoom.Text.Trim(), TextStreet.Text.Trim(), state, TextCity.Text.Trim(), TextPostalCode.Text.Trim(), TextPrefix.Text.Trim(), TextGivenName.Text.Trim(), TextMiddleName.Text.Trim(), TextSurName.Text.Trim(), TextSuffix.Text.Trim(), ddlLangContact.SelectedValue.ToString());
                        }


                        if (dsresult.Tables["ContactDetails"].Rows.Count > 0)
                        {
                            grdContact.AutoGenerateColumns = false;
                            grdCntcName.AutoGenerateColumns = false;
                            grdwebsite.AutoGenerateColumns = false;
                            grdAddress.AutoGenerateColumns = false;

                            for (int intCount = 0; intCount < dsresult.Tables["ContactDetails"].Rows.Count; intCount++)
                            {
                                if (dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString() != "")
                                {
                                    dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"] = r.ReadandReplaceHexaToChar(dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString(), InputXmlPath);
                                }
                                dsresult.Tables["ContactDetails"].AcceptChanges();
                            }

                            foreach (DataRow DC in dsresult.Tables["ContactDetails"].Rows)
                            {
                                if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                                {
                                    DC["TYPE"] = Convert.ToString(r.ConvertUnicodeToText(DC["TYPE"].ToString()));
                                    DC["title"] = Convert.ToString(r.ConvertUnicodeToText(DC["title"].ToString()));
                                    DC["email"] = Convert.ToString(r.ConvertUnicodeToText(DC["email"].ToString()));
                                    DC["website_text"] = Convert.ToString(r.ConvertUnicodeToText(DC["website_text"].ToString()));
                                    DC["url"] = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
                                    DC["room"] = Convert.ToString(r.ConvertUnicodeToText(DC["room"].ToString()));
                                    DC["street"] = Convert.ToString(r.ConvertUnicodeToText(DC["street"].ToString()));
                                    DC["city"] = Convert.ToString(r.ConvertUnicodeToText(DC["city"].ToString()));
                                    DC["prefix"] = Convert.ToString(r.ConvertUnicodeToText(DC["prefix"].ToString()));
                                    DC["givenname"] = Convert.ToString(r.ConvertUnicodeToText(DC["givenname"].ToString()));
                                    DC["middlename"] = Convert.ToString(r.ConvertUnicodeToText(DC["middlename"].ToString()));
                                    DC["surname"] = Convert.ToString(r.ConvertUnicodeToText(DC["surname"].ToString()));
                                    DC["suffix"] = Convert.ToString(r.ConvertUnicodeToText(DC["suffix"].ToString()));
                                    DC.AcceptChanges();
                                }
                            }

                            dsresult.Tables["ContactDetails"].AcceptChanges();
                            dttempContact = dsresult.Tables["ContactDetails"].Copy();

                            grdContact.DataSource = dsresult.Tables["ContactDetails"];
                            grdCntcName.DataSource = dsresult.Tables["ContactDetails"];
                            grdwebsite.DataSource = dsresult.Tables["ContactDetails"];
                            grdAddress.DataSource = dsresult.Tables["ContactDetails"];

                            grdContact.Visible = false;
                            grdCntcName.Visible = false;

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

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            if (pagemode == 1)
                            {
                                m_parent.GetProcess_update("Contacts");
                            }
                            else if (pagemode == 2)
                            {
                                m_parent.GetProcess_update("officers");
                            }

                            else if (pagemode == 3)
                            {
                                m_parent.GetProcess_update("contact info");
                            }
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

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

            PageURL objPage = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);
        }

        private void DDLCOUNTRY_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

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
                {
                    return false;
                }
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
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                if (dttempContact.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            TextType.Text = Convert.ToString(dttempContact.Rows[rowindex]["type"]);
                            TextTitle.Text = Convert.ToString(dttempContact.Rows[rowindex]["title"]);
                            TextTelephone.Text = Convert.ToString(dttempContact.Rows[rowindex]["telephone"]);
                            TextFax.Text = Convert.ToString(dttempContact.Rows[rowindex]["fax"]);
                            TextEmail.Text = Convert.ToString(dttempContact.Rows[rowindex]["email"]);

                            TextPrefix.Text = Convert.ToString(dttempContact.Rows[rowindex]["prefix"]);
                            TextGivenName.Text = Convert.ToString(dttempContact.Rows[rowindex]["givenName"]);
                            TextMiddleName.Text = Convert.ToString(dttempContact.Rows[rowindex]["middleName"]);
                            TextSurName.Text = Convert.ToString(dttempContact.Rows[rowindex]["surname"]);
                            TextSuffix.Text = Convert.ToString(dttempContact.Rows[rowindex]["suffix"]);

                            TextURL.Text = Convert.ToString(dttempContact.Rows[rowindex]["url"]);

                            TextLinkText.Text = Convert.ToString(r.ReadandReplaceHexaToChar(dttempContact.Rows[rowindex]["website_text"].ToString(), InputXmlPath));

                            TextRoom.Text = Convert.ToString(dttempContact.Rows[rowindex]["room"]);
                            TextStreet.Text = Convert.ToString(dttempContact.Rows[rowindex]["street"]);
                            TextCity.Text = Convert.ToString(dttempContact.Rows[rowindex]["city"]);

                            string othetState = Convert.ToString(dttempContact.Rows[rowindex]["statename"]).Trim();
                            string othetStatecode = Convert.ToString(dttempContact.Rows[rowindex]["statecode"]).Trim();
                            string country = Convert.ToString(dttempContact.Rows[rowindex]["countrycode"]).Trim();
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
                                {
                                    ddlState.SelectedValue = Convert.ToString(dttempContact.Rows[rowindex]["STATEcode"]);
                                }
                                else if (dtStateResult1.Rows.Count > 0)
                                {
                                    ddlState.SelectedValue = Convert.ToString(dttempContact.Rows[rowindex]["STATEcode"]);
                                }
                                else
                                {
                                    ddlState.SelectedValue = "OtherState";
                                    txtOtherState.Enabled = true;
                                    txtOtherState.Text = Convert.ToString(dttempContact.Rows[rowindex]["STATEcode"]);
                                }
                            }

                            TextPostalCode.Text = Convert.ToString(dttempContact.Rows[rowindex]["postalcode"]);

                            txtSubmit.Visible = false;
                            btnaddurl.Visible = false;

                            btnupdate.Visible = true;
                            btncancel.Visible = true;
                        }
                        catch
                        {

                        }
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
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                if (dttempContact.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsItems = FundingBodyDataOperations.SaveAndDeleteContactsLIst(SharedObjects.WorkId, pagemode, 1, Convert.ToInt64(dttempContact.Rows[grdContact.SelectedCells[0].RowIndex]["contact_id"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                            dttempContact = dsItems.Tables["ContactDetails"].Copy();
                            if (dsItems.Tables["ContactDetails"].Rows.Count > 0)
                            {
                                grdContact.AutoGenerateColumns = false;
                                grdCntcName.AutoGenerateColumns = false;
                                grdwebsite.AutoGenerateColumns = false;
                                grdAddress.AutoGenerateColumns = false;

                                for (int intCount = 0; intCount < dsItems.Tables["ContactDetails"].Rows.Count; intCount++)
                                {
                                    if (dsItems.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString() != "")
                                    {
                                        dsItems.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"] = r.ReadandReplaceHexaToChar(dsItems.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString(), InputXmlPath);
                                    }
                                }
                                dsItems.Tables["ContactDetails"].AcceptChanges();

                                grdContact.DataSource = dsItems.Tables["ContactDetails"];
                                grdCntcName.DataSource = dsItems.Tables["ContactDetails"];
                                grdwebsite.DataSource = dsItems.Tables["ContactDetails"];
                                grdAddress.DataSource = dsItems.Tables["ContactDetails"];

                                grdContact.Visible = false;
                                grdCntcName.Visible = false;
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

                            m_parent.GetProcess();
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
            string url_txtLinkUrl = TextURL.Text.TrimStart().TrimEnd();
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
                        InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                        lblMsg.Visible = false;
                        if (ddlLangContact.SelectedIndex == 0)
                        {
                            MessageBox.Show("Please Select Language", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (TextType.Text.Trim() == "" && TextTitle.Text.Trim() == "" && TextTelephone.Text.Trim() == "" && TextFax.Text.Trim() == "" && TextEmail.Text.Trim() == "" && TextPrefix.Text.Trim() == "" && TextGivenName.Text.Trim() == "" && TextMiddleName.Text.Trim() == "" && TextSurName.Text.Trim() == "" && TextURL.Text.Trim() == "" && TextLinkText.Text.Trim() == "" && TextRoom.Text.Trim() == "" && TextStreet.Text.Trim() == "" && TextCity.Text.Trim() == "" && TextPostalCode.Text.Trim() == "" && ddlState.SelectedIndex == 0 && DDLCOUNTRY.SelectedIndex == 0 && txtOtherState.Text.Trim() == "")
                        {
                            MessageBox.Show("Please fill any field(s)", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (TextGivenName.Text.Trim() == "" && TextSurName.Text.Trim() == "")
                        {
                            if (TextPrefix.Text.Trim() != "" || TextMiddleName.Text.Trim() != "" || TextSuffix.Text.Trim() != "")
                            {
                                MessageBox.Show("Enter GivenName and SurName", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (TextEmail.Text.Trim() != "")
                        {
                            if (!CheckEmail(TextEmail.Text.Trim()))
                            {
                                MessageBox.Show("Please enter the valid emailID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        TextLinkText.Text = "NULL";
                        Int64 WFID = SharedObjects.WorkId;

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
                        STATE = Convert.ToString(txtOtherState.Text.Trim());

                        string CITY = TextCity.Text.Trim();
                        string POSTALCODE = TextPostalCode.Text.Trim();
                        string PREFIX = TextPrefix.Text.Trim();
                        string GIVENNAME = TextGivenName.Text.Trim();
                        string MIDDLENAME = TextMiddleName.Text.Trim();
                        string SURNAME = TextSurName.Text.Trim();
                        string SUFFIX = TextSuffix.Text.Trim();

                        if (COUNTRY.Length > 3)
                        {
                            COUNTRY = "";
                        }
                        if (WEBSITE_TEXT.Trim().Length == 0)
                        {
                            WEBSITE_TEXT = "Not Available";
                        }
                        WEBSITE_TEXT = r.ReadandReplaceCharToHexa(WEBSITE_TEXT.ToString(), InputXmlPath);
                        DataSet dsresult = null;

                        if (r.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsresult = FundingBodyDataOperations.SaveAndDeleteContactsLIst(WFID, pagemode, 2, Convert.ToInt64(dttempContact.Rows[rowindex]["contact_id"]), r.ConvertTextToUnicode(TYPE), r.ConvertTextToUnicode(TITLE), TELEPHONE, FAX, r.ConvertTextToUnicode(EMAIL), r.ConvertTextToUnicode(url), r.ConvertTextToUnicode(WEBSITE_TEXT), COUNTRY, r.ConvertTextToUnicode(ROOM), r.ConvertTextToUnicode(STREET), STATE, r.ConvertTextToUnicode(CITY), POSTALCODE, r.ConvertTextToUnicode(PREFIX), r.ConvertTextToUnicode(GIVENNAME), r.ConvertTextToUnicode(MIDDLENAME), r.ConvertTextToUnicode(SURNAME), r.ConvertTextToUnicode(SUFFIX), ddlLangContact.SelectedValue.ToString());
                        }
                        else
                        {
                            dsresult = FundingBodyDataOperations.SaveAndDeleteContactsLIst(WFID, pagemode, 2, Convert.ToInt64(dttempContact.Rows[rowindex]["contact_id"]), TYPE, TITLE, TELEPHONE, FAX, EMAIL, url, WEBSITE_TEXT, COUNTRY, ROOM, STREET, STATE, CITY, POSTALCODE, PREFIX, GIVENNAME, MIDDLENAME, SURNAME, SUFFIX, ddlLangContact.SelectedValue.ToString());
                        }
                        
                            if (dsresult.Tables["ContactDetails"].Rows.Count > 0)
                            {
                                grdContact.AutoGenerateColumns = false;
                                grdCntcName.AutoGenerateColumns = false;
                                grdwebsite.AutoGenerateColumns = false;
                                grdAddress.AutoGenerateColumns = false;

                                for (int intCount = 0; intCount < dsresult.Tables["ContactDetails"].Rows.Count; intCount++)
                                {
                                    if (dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString() != "")
                                    {
                                        dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"] = r.ReadandReplaceHexaToChar(dsresult.Tables["ContactDetails"].Rows[intCount]["WEBSITE_TEXT"].ToString(), InputXmlPath);
                                    }
                                    dsresult.Tables["ContactDetails"].AcceptChanges();
                                }
                                foreach (DataRow DC in dsresult.Tables["ContactDetails"].Rows)
                                {
                                    if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                                    {
                                        DC["TYPE"] = Convert.ToString(r.ConvertUnicodeToText(DC["TYPE"].ToString()));
                                        DC["title"] = Convert.ToString(r.ConvertUnicodeToText(DC["title"].ToString()));
                                        DC["email"] = Convert.ToString(r.ConvertUnicodeToText(DC["email"].ToString()));
                                        DC["website_text"] = Convert.ToString(r.ConvertUnicodeToText(DC["website_text"].ToString()));
                                        DC["url"] = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
                                        DC["room"] = Convert.ToString(r.ConvertUnicodeToText(DC["room"].ToString()));
                                        DC["street"] = Convert.ToString(r.ConvertUnicodeToText(DC["street"].ToString()));
                                        DC["city"] = Convert.ToString(r.ConvertUnicodeToText(DC["city"].ToString()));
                                        DC["prefix"] = Convert.ToString(r.ConvertUnicodeToText(DC["prefix"].ToString()));
                                        DC["givenname"] = Convert.ToString(r.ConvertUnicodeToText(DC["givenname"].ToString()));
                                        DC["middlename"] = Convert.ToString(r.ConvertUnicodeToText(DC["middlename"].ToString()));
                                        DC["surname"] = Convert.ToString(r.ConvertUnicodeToText(DC["surname"].ToString()));
                                        DC["suffix"] = Convert.ToString(r.ConvertUnicodeToText(DC["suffix"].ToString()));
                                        DC.AcceptChanges();
                                    }
                                }

                                dsresult.Tables["ContactDetails"].AcceptChanges();
                                dttempContact = dsresult.Tables["ContactDetails"].Copy();
                                grdContact.DataSource = dsresult.Tables["ContactDetails"];
                                grdCntcName.DataSource = dsresult.Tables["ContactDetails"];
                                grdwebsite.DataSource = dsresult.Tables["ContactDetails"];
                                grdAddress.DataSource = dsresult.Tables["ContactDetails"];

                                grdContact.Visible = false;
                                grdCntcName.Visible = false;
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
                        

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            if (pagemode == 1)
                            {
                                m_parent.GetProcess_update("Contacts");
                            }
                            else if (pagemode == 2)
                            {
                                m_parent.GetProcess_update("officers");
                            }

                            else if (pagemode == 3)
                            {
                                m_parent.GetProcess_update("contact info");
                            }
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

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
