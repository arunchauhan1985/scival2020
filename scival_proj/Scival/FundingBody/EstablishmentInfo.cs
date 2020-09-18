using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySqlDal;
using MySqlDalAL;
using MySqlDal.DataOpertation;
using Newtonsoft.Json;

namespace Scival.FundingBody
{
    public partial class EstablishmentInfo : UserControl
    {
        DataSet dsFunding = new DataSet();
        //DAL.FundingBody dalobj = new DAL.FundingBody();

        private FundingBody m_parent;
        Int64 workflowid = 0;
        ErrorLog oErrorLog = new ErrorLog();
        Replace r = new Replace();
        public EstablishmentInfo(FundingBody frm)
        {
            InitializeComponent();
            LoadInitailValue();
            m_parent = frm;
            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        void ddlCountry_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlState_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitailValue()
        {
            try
            {
                lblMsg.Visible = false;
                string lang = "";
                workflowid = Convert.ToInt64(SharedObjects.WorkId);

                dsFunding = FundingBodyDataOperations.GetEstablishInfo(workflowid);

                DataTable temp = dsFunding.Tables["Country"];

                DataRow dr = temp.NewRow();
                dr["LCODE"] = "SelectCountry";
                dr["NAME"] = "--Select Country--";
                temp.Rows.InsertAt(dr, 0);

                ddlCountry.DataSource = temp;
                ddlCountry.ValueMember = "LCODE";
                ddlCountry.DisplayMember = "NAME";

                DataSet dsOpptunity = SharedObjects.StartWork;
                DataTable tempOppName = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr1 = tempOppName.NewRow();
                dr1 = tempOppName.NewRow();
                dr1["LANGUAGE_CODE"] = "SelectLanguage";
                dr1["LANGUAGE_NAME"] = "--Select Language--";
                tempOppName.Rows.InsertAt(dr1, 0);

                ddlLang_EstInfo.DataSource = tempOppName;
                ddlLang_EstInfo.ValueMember = "LANGUAGE_CODE";
                ddlLang_EstInfo.DisplayMember = "LANGUAGE_NAME";
                ddlLang_EstInfo.SelectedIndex = 18;

                if (dsFunding.Tables["DisplayData"].Rows.Count > 0)
                {
                    ddlCountry.SelectedValue = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTCOUNTRYCODE"]);
                    txtTodate.Text = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTDATE"]).Split('-').Last();
                    txtcity.Text = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTCITY"]);
                    richtxtDesc.Text = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTDESCRIPTION"]);
                    ddlLang_EstInfo.SelectedValue = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["LANG"]);
                    lang = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["LANG"]);

                    if (r.chk_OtherLang(lang.ToLower()) == true)
                    {
                        richtxtDesc.Text = Convert.ToString(r.ConvertUnicodeToText(richtxtDesc.Text));
                    }

                    string othetState = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTSTATE"]).Trim();

                    dsFunding.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + ddlCountry.SelectedValue + "'";
                    DataTable dtOtherState = dsFunding.Tables["State"].DefaultView.ToTable();

                    dr = dtOtherState.NewRow();
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
                        dt.DefaultView.RowFilter = "CODE='" + othetState + "'";
                        DataTable dtStateResult = dtOtherState.DefaultView.ToTable();

                        if (dtOtherState.Rows.Count > 2)
                        {
                            ddlState.SelectedValue = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTSTATE"]);
                        }
                        else
                        {
                            ddlState.SelectedValue = "OtherState";
                            txtState.Enabled = true;
                            txtState.Text = Convert.ToString(dsFunding.Tables["DisplayData"].Rows[0]["ESTABLISHMENTSTATE"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (ddlCountry.SelectedIndex != 0)
                {
                    DataTable temp = dsFunding.Tables["State"];
                    temp.DefaultView.RowFilter = "COUNTRYCODE='" + ddlCountry.SelectedValue + "'";
                    DataTable dtState = temp.DefaultView.ToTable();

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
                    txtState.Enabled = true;
                else
                    txtState.Enabled = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string url_txtDescr = richtxtDesc.Text.TrimStart().TrimEnd();
            string url_txtcity = txtcity.Text.TrimStart().TrimEnd();
            string url_txtState = txtState.Text.TrimStart().TrimEnd();
            string url_txtTodate = txtTodate.Text.TrimStart().TrimEnd();

            if (url_txtcity.Contains("http://") || url_txtState.Contains("http://") || url_txtTodate.Contains("http://") ||
                 url_txtcity.Contains("https://") || url_txtState.Contains("https://") || url_txtTodate.Contains("https://") ||
                 url_txtcity.Contains("www.") || url_txtState.Contains("www.") || url_txtTodate.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (richtxtDesc.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(richtxtDesc.Text.Trim(), "Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    if (txtTodate.Text == "")
                    {
                        MessageBox.Show("Please enter Year.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (!intRgx.IsMatch(txtTodate.Text))
                    {
                        MessageBox.Show("Please enter valid Year.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlCountry.SelectedValue.ToString() == "SelectCountry")
                    {
                        MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string strcoountry = ddlCountry.SelectedValue.ToString();

                        string strstate = string.Empty;
                        if (ddlState.SelectedValue.ToString() != "SelectState")
                        {
                            if (ddlState.SelectedValue.ToString() == "OtherState")
                                strstate = Convert.ToString(txtState.Text.Trim());
                            else
                                strstate = Convert.ToString(ddlState.SelectedValue);
                        }
                        else
                            strstate = "";

                        string strdesc = richtxtDesc.Text.Trim();
                        string strCity = txtcity.Text.Trim();
                        string strdate = txtTodate.Text.Trim();
                        string langval = ddlLang_EstInfo.SelectedValue.ToString();
                        DataSet ds = null;
                        langval = Convert.ToString(ddlLang_EstInfo.SelectedValue).ToLower();

                        DataTable dtFundingBody = new DataTable();
                        dtFundingBody.Columns.Add("FUNDINGBODY_ID");
                        dtFundingBody.Columns.Add("recordSource");

                        DataTable dt_fundingdescription = new DataTable();
                        dt_fundingdescription.Columns.Add("ESTABLISHMENTDATE");
                        dt_fundingdescription.Columns.Add("ESTABLISHMENTCOUNTRYCODE");
                        dt_fundingdescription.Columns.Add("ESTABLISHMENTDESCRIPTION");
                        dt_fundingdescription.Columns.Add("LANG");

                        DataRow fbr = dtFundingBody.NewRow();
                        fbr["FUNDINGBODY_ID"] = SharedObjects.ID;
                        dtFundingBody.Rows.Add(fbr);

                       

                        //for (int i = 0; i < grdAbout.Rows.Count; i++)
                        //{

                        //    string RELTYPE = "";
                        //    if (grdAbout["RelType", 0].Value != null)
                        //        RELTYPE = grdAbout["RelType", 0].Value.ToString();

                        //    string DESCRIPTION = "";
                        //    if (grdAbout["Description", 0].Value != null)
                        //        DESCRIPTION = grdAbout["Description", 0].Value.ToString();

                        //    string URL = "";
                        //    if (grdAbout["Linkurl", 0].Value != null)
                        //        URL = grdAbout["Linkurl", 0].Value.ToString();

                        //    string LINKTEXT = "";
                        //    if (grdAbout["Linktext", 0].Value != null)
                        //        LINKTEXT = grdAbout["Linktext", 0].Value.ToString();

                        //    string LANG = "";
                        //    if (grdAbout["lang", 0].Value != null)
                        //        LANG = grdAbout["lang", 0].Value.ToString();
                        //    DataRow dr = dt_fundingdescription.NewRow();
                        //    dr["WFID"] = WFID;
                        //    dr["PAGEMODE"] = pagemode;
                        //    dr["WORKMODE"] = 0;
                        //    dr["RELTYPE"] = RELTYPE;
                        //    dr["DESCRIPTION"] = DESCRIPTION;
                        //    dr["URL"] = URL;
                        //    dr["LINKTEXT"] = LINKTEXT;
                        //    dr["LANG"] = Convert.ToString(LANG);
                        //    dt_fundingdescription.Rows.Add(dr);
                        //}
                        DataRow dt = dt_fundingdescription.NewRow();
                        dt["ESTABLISHMENTDATE"] = strdate;
                        dt["ESTABLISHMENTCOUNTRYCODE"] = strcoountry;
                        dt["ESTABLISHMENTDESCRIPTION"] = strdesc;
                        dt["LANG"] = langval;
                        dt_fundingdescription.Rows.Add(dt);


                        if (r.chk_OtherLang(langval.ToLower()) == true)
                        {
                            strdesc = r.ConvertTextToUnicode(strdesc);

                            #region Saving JSON Function
                            string json = FundingBodyDataOperations.GetFundingBodyMainJson(SharedObjects.ID);

                            fbr = dtFundingBody.NewRow();
                            FB_JSON_Model dataJSON = JsonConvert.DeserializeObject<FB_JSON_Model>(json);
                            fbr["recordSource"] = dataJSON.homePage;
                            dtFundingBody.Rows.Add(fbr);

                            XmlJsonOperation xmlJsonOperation = new XmlJsonOperation();
                            string updatedJSON = xmlJsonOperation.saveEstablishmentInfo("", dt_fundingdescription, dtFundingBody, json);
                            string Loginid = "0";

                            Loginid = Convert.ToString(SharedObjects.User.USERID);
                            FundingBodyDataOperations.saveandUpdateJSONinTable(SharedObjects.ID.ToString(), updatedJSON, "", "", Convert.ToString(Loginid), DateTime.Now.ToString(), 2);
                            #endregion
                            ds = FundingBodyDataOperations.SaveEstablishInfo(workflowid, strdate, strCity, strstate, strcoountry, strdesc, langval);
                           

                        }
                        else
                        {
                            #region Saving JSON Function
                            string json = FundingBodyDataOperations.GetFundingBodyMainJson(SharedObjects.ID);

                            fbr = dtFundingBody.NewRow();
                            FB_JSON_Model dataJSON = JsonConvert.DeserializeObject<FB_JSON_Model>(json);
                            fbr["recordSource"] = dataJSON.homePage;
                            dtFundingBody.Rows.Add(fbr);

                            XmlJsonOperation xmlJsonOperation = new XmlJsonOperation();
                            string updatedJSON = xmlJsonOperation.saveEstablishmentInfo("", dt_fundingdescription, dtFundingBody, json);
                            string Loginid = "0";

                            Loginid = Convert.ToString(SharedObjects.User.USERID);
                            FundingBodyDataOperations.saveandUpdateJSONinTable(SharedObjects.ID.ToString(), updatedJSON, "", "", Convert.ToString(Loginid), DateTime.Now.ToString(), 2);
                            #endregion

                            ds = FundingBodyDataOperations.SaveEstablishInfo(workflowid, strdate, strCity, strstate, strcoountry, strdesc, langval);
                        }

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess_update("Establishment Info");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

                        lblMsg.Visible = true;
                        
                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
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
    }
}
