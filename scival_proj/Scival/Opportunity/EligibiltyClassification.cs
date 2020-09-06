using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class EligibiltyClassification : UserControl
    {
        DataTable dtEclassText = new DataTable();
        DataTable DTType = new DataTable();
        DataTable DTDisplay = new DataTable();

        List<sci_countrygroup> countryGroupList;
        List<sci_countries> countryList;

        Opportunity opportunity;
        Replace replace = new Replace();
        ErrorLog oErrorLog = new ErrorLog();

        Int64 WFID = 0;
        Int64 mode = 0;
        int rowindex = 0;

        static bool _isregionSpecific = false;

        string c_eligibilityclassification_id = string.Empty;
        string c_individualeligibility_ID = string.Empty;
        string c_organizationeligibility_id = string.Empty;
        string c_restrictions_id = string.Empty;

        void ddl_IET_NR_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddl_ORG_NR_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddl_ORG_NS_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddl_Rest_NS_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void DDLCOUNTRY_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlCountry_IET_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlcountrylist_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlElClassText_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlcountrylistdtl_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlElClassType_MouseWheel(object sender, MouseEventArgs e)
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

        public EligibiltyClassification(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            loadInitialValue();
            loadSavedValues();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        private void loadInitialValue()
        {
            try
            {
                countryGroupList = MySqlDal.OpportunityDataOperations.GetCountryGroup();
                countryList = MySqlDal.OpportunityDataOperations.GetCountries(1);

                chk_individual_list.DataSource = countryList;
                chk_individual_list.ValueMember = "COUNTRYGROUP_ID";
                chk_individual_list.DisplayMember = "COUNTRYGROUP_NAME";

                chkbox_orgnEligibilityy.DataSource = countryList;
                chkbox_orgnEligibilityy.ValueMember = "COUNTRYGROUP_ID";
                chkbox_orgnEligibilityy.DisplayMember = "COUNTRYGROUP_NAME";

                lblMsg.Visible = false;

                pnlregionspecific.Visible = false;
                Cbhxregionspecific.Visible = false;
                pnl_individualEligibilityType.Visible = true;

                WFID = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsEClass = MySqlDal.OpportunityDataOperations.GetEligibiltyClassification(WFID);

                DataTable temp = dsEClass.Tables["Type"].Copy();

                DataRow dr = temp.NewRow();
                dr["Value"] = "SelectType";
                dr["Type"] = "--Select Type--";
                temp.Rows.InsertAt(dr, 0);

                ddlElClassType.DataSource = temp;
                ddlElClassType.DisplayMember = "Type";
                ddlElClassType.ValueMember = "Value";

                dtEclassText = dsEClass.Tables["EClassText"].Copy(); ;
                DTType = dsEClass.Tables["Type"].Copy();
                DTDisplay = dsEClass.Tables["DisplayData"].Copy();

                if (DTDisplay.Rows.Count > 0)
                {
                    grdElClass.AutoGenerateColumns = false;
                    grdElClass.DataSource = dsEClass.Tables["DisplayData"];
                }
                else
                {
                    NoRecord();
                }

                ddlcountrylistdtl.Visible = true;
                DataTable dt211 = dtEclassText;
                dt211.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                DataTable dtView211 = dt211.DefaultView.ToTable();

                DataTable dtEclassText1 = new DataTable();
                DataSet dsEClass1 = MySqlDal.OpportunityDataOperations.GetEligibiltyClassification(WFID);

                dtEclassText1 = dsEClass1.Tables["country"].Copy(); ;

                DataTable dt212 = dtEclassText1;

                ddlcountrylistdtl.DataSource = dt212;
                ddlcountrylistdtl.DisplayMember = "name";
                ddlcountrylistdtl.ClearSelected();
                ddlcountrylistdtl.ValueMember = "lcode";
                ddlcountrylistdtl.Enabled = true;

                listcountrydtl.Visible = true;
                listcountrydtl.DataSource = dtView211;
                listcountrydtl.DisplayMember = "EligibilityClassification";
                listcountrydtl.ValueMember = "Value";
                listcountrydtl.ClearSelected();
                listcountrydtl.Enabled = true;

                DataTable tempcountry = dsEClass.Tables["Country"].Copy();
                DataRow drcountry = tempcountry.NewRow();
                drcountry["LCODE"] = "SelectCountry";
                drcountry["NAME"] = "--Select Country--";
                tempcountry.Rows.InsertAt(drcountry, 0);
                DDLCOUNTRY.DataSource = tempcountry;
                DDLCOUNTRY.ValueMember = "LCODE";
                DDLCOUNTRY.DisplayMember = "NAME";

                DataTable tempcountry1 = dsEClass.Tables["Country"].Copy();
                DataRow drcountry1 = tempcountry1.NewRow();
                drcountry1["LCODE"] = "SelectCountry";
                drcountry1["NAME"] = "--Select Country--";
                tempcountry1.Rows.InsertAt(drcountry1, 0);
                ddlCountry_IET.DataSource = tempcountry1;
                ddlCountry_IET.ValueMember = "LCODE";
                ddlCountry_IET.DisplayMember = "NAME";

                DataTable dt_NR_Type = new DataTable();
                dt_NR_Type.Columns.Add("Value", typeof(string));
                dt_NR_Type.Columns.Add("Name", typeof(string));
                DataRow dt_NR_Type_row = dt_NR_Type.NewRow();
                dt_NR_Type_row["Value"] = "LIMITED";
                dt_NR_Type_row["Name"] = "LIMITED";
                dt_NR_Type.Rows.InsertAt(dt_NR_Type_row, 0);

                DataRow dt_NR_Type_row1 = dt_NR_Type.NewRow();
                dt_NR_Type_row1["Value"] = "NOTLIMITED";
                dt_NR_Type_row1["Name"] = "NOTLIMITED";
                dt_NR_Type.Rows.InsertAt(dt_NR_Type_row1, 1);

                DataRow dt_NR_Type_row4 = dt_NR_Type.NewRow();
                dt_NR_Type_row4["Value"] = "NOTSPECIFIED";
                dt_NR_Type_row4["Name"] = "NOTSPECIFIED";
                dt_NR_Type.Rows.InsertAt(dt_NR_Type_row4, 2);

                DataTable dt_NR_Type1 = new DataTable();
                dt_NR_Type1.Columns.Add("Value", typeof(string));
                dt_NR_Type1.Columns.Add("Name", typeof(string));
                DataRow dt_NR_Type_row2 = dt_NR_Type1.NewRow();
                dt_NR_Type_row2["Value"] = "false";
                dt_NR_Type_row2["Name"] = "false";
                dt_NR_Type1.Rows.InsertAt(dt_NR_Type_row2, 0);

                DataRow dt_NR_Type_row3 = dt_NR_Type1.NewRow();
                dt_NR_Type_row3["Value"] = "true";
                dt_NR_Type_row3["Name"] = "true";
                dt_NR_Type1.Rows.InsertAt(dt_NR_Type_row3, 1);

                ddl_ORG_NR.DataSource = dt_NR_Type;
                ddl_ORG_NR.ValueMember = "Value";
                ddl_ORG_NR.DisplayMember = "Name";

                ddl_ORG_NS.DataSource = dt_NR_Type;
                ddl_ORG_NS.ValueMember = "Value";
                ddl_ORG_NS.DisplayMember = "Name";

                ddl_IET_NS.DataSource = dt_NR_Type;
                ddl_IET_NS.ValueMember = "Value";
                ddl_IET_NS.DisplayMember = "Name";

                ddl_IET_NR.DataSource = dt_NR_Type1;
                ddl_IET_NR.ValueMember = "Value";
                ddl_IET_NR.DisplayMember = "Name";

                ddl_Rest_NS.DataSource = dt_NR_Type;
                ddl_Rest_NS.ValueMember = "Value";
                ddl_Rest_NS.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void loadSavedValues()
        {
            countryGroupList = MySqlDal.OpportunityDataOperations.GetCountryGroup();
            countryList = MySqlDal.OpportunityDataOperations.GetCountries(1);

            chk_individual_list.DataSource = countryList;
            chk_individual_list.ValueMember = "COUNTRYGROUP_ID";
            chk_individual_list.DisplayMember = "COUNTRYGROUP_NAME";

            lblMsg.Visible = false;
            WFID = Convert.ToInt64(SharedObjects.WorkId);

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

            DataSet dsEClass = MySqlDal.OpportunityDataOperations.GetEligibiltyClassificationDetial(WFID);

            if (dsEClass.Tables["IndividualEligibility"].Rows.Count > 0)
            {
                ddl_IET_NS.SelectedIndex = 0;
                txtDegree.Text = "";
                txtGraduate.Text = "";
                txtNewfaculty.Text = "";
                txtUndergraduate.Text = "";
                ddl_IET_NR.SelectedIndex = 0;
                ddl_IET_NS.SelectedIndex = 0;
                ddlCountry_IET.SelectedIndex = 0;

                foreach (DataRow DC in dsEClass.Tables["IndividualEligibility"].Rows)
                {
                    if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                    {
                        DC["degree"] = Convert.ToString(replace.ConvertUnicodeToText(DC["degree"].ToString()));
                        DC["graduate"] = Convert.ToString(replace.ConvertUnicodeToText(DC["graduate"].ToString()));
                        DC["newfaculty"] = Convert.ToString(replace.ConvertUnicodeToText(DC["newfaculty"].ToString()));
                        DC["undergraduate"] = Convert.ToString(replace.ConvertUnicodeToText(DC["undergraduate"].ToString()));
                        DC.AcceptChanges();

                    }
                }

                dsEClass.Tables["IndividualEligibility"].AcceptChanges();

                dgv_IndividualElg.DataSource = dsEClass.Tables["IndividualEligibility"];
                dgv_IndividualElg.Visible = true;
            }
            else
            {
                dgv_IndividualElg.DataSource = null;
                dgv_IndividualElg.Refresh();
            }

            if (dsEClass.Tables["organizationEligibility"].Rows.Count > 0)
            {
                ddl_ORG_NS.SelectedIndex = 0;
                ddl_ORG_NR.SelectedIndex = 0;
                ddlState.SelectedIndex = 0;
                DDLCOUNTRY.SelectedIndex = 0;

                txtAcademic.Text = "";
                txtCommercial.Text = "";
                txtGovernment.Text = "";
                txtNonprofit.Text = "";
                txtSme.Text = "";
                TextCity.Text = "";

                foreach (DataRow DC in dsEClass.Tables["organizationEligibility"].Rows)
                {
                    if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                    {
                        DC["academic"] = Convert.ToString(replace.ConvertUnicodeToText(DC["academic"].ToString()));
                        DC["commercial"] = Convert.ToString(replace.ConvertUnicodeToText(DC["commercial"].ToString()));
                        DC["government"] = Convert.ToString(replace.ConvertUnicodeToText(DC["government"].ToString()));
                        DC["nonprofit"] = Convert.ToString(replace.ConvertUnicodeToText(DC["nonprofit"].ToString()));
                        DC["sme"] = Convert.ToString(replace.ConvertUnicodeToText(DC["sme"].ToString()));
                        DC.AcceptChanges();
                    }
                }

                dsEClass.Tables["organizationEligibility"].AcceptChanges();

                dgv_orgElg.DataSource = dsEClass.Tables["organizationEligibility"];
                dgv_orgElg.Visible = true;
            }
            else
            {
                dgv_orgElg.DataSource = null;
                dgv_orgElg.Refresh();
            }

            if (dsEClass.Tables["restrictions"].Rows.Count > 0)
            {
                ddl_Rest_NS.SelectedIndex = 0;
                txtDisabilities.Text = "";
                txtInvitationonly.Text = "";
                txtMemberonly.Text = "";
                txtNominationonly.Text = "";
                txtMinorities.Text = "";
                txtWomen.Text = "";
                txtNumberofapplicantsallowed.Text = "";

                foreach (DataRow DC in dsEClass.Tables["restrictions"].Rows)
                {
                    if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
                    {
                        DC["disabilities"] = Convert.ToString(replace.ConvertUnicodeToText(DC["disabilities"].ToString()));
                        DC["invitationonly"] = Convert.ToString(replace.ConvertUnicodeToText(DC["invitationonly"].ToString()));
                        DC["memberonly"] = Convert.ToString(replace.ConvertUnicodeToText(DC["memberonly"].ToString()));
                        DC["nominationonly"] = Convert.ToString(replace.ConvertUnicodeToText(DC["nominationonly"].ToString()));
                        DC["minorties"] = Convert.ToString(replace.ConvertUnicodeToText(DC["minorties"].ToString()));
                        DC["women"] = Convert.ToString(replace.ConvertUnicodeToText(DC["women"].ToString()));
                        DC.AcceptChanges();
                    }
                }

                dsEClass.Tables["restrictions"].AcceptChanges();

                dgv_Restrictions.DataSource = dsEClass.Tables["restrictions"];
                dgv_Restrictions.Visible = true;
            }
            else
            {
                dgv_Restrictions.DataSource = null;
                dgv_Restrictions.Refresh();
            }
        }

        private void NoRecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("ELIGIBILITYCLASSIFICATION_TEXT");
                dtNoRcrd.Columns.Add("DISPLAY");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdElClass.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string lang_val = "";

                if (ddlLangContact.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    lang_val = ddlLangContact.SelectedValue.ToString();
                }

                lblMsg.Visible = false;
                DataSet dsIETClass = new DataSet();

                if (ddlElClassType.SelectedIndex == 0 || ddlElClassType.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Convert.ToString(ddlElClassType.SelectedValue) == "individualEligibility")
                {
                    string url_txtDegree = txtDegree.Text.TrimStart().TrimEnd();
                    string url_txtGraduate = txtGraduate.Text.TrimStart().TrimEnd();
                    string url_txtNewfaculty = txtNewfaculty.Text.TrimStart().TrimEnd();
                    string url_txtUndergraduate = txtUndergraduate.Text.TrimStart().TrimEnd();

                    if (url_txtDegree.Contains("http://") || url_txtGraduate.Contains("http://") || url_txtNewfaculty.Contains("http://") || url_txtNewfaculty.Contains("http://") || url_txtUndergraduate.Contains("http://") ||
                        url_txtDegree.Contains("https://") || url_txtGraduate.Contains("https://") || url_txtNewfaculty.Contains("https://") || url_txtNewfaculty.Contains("https://") || url_txtUndergraduate.Contains("https://") ||
                        url_txtDegree.Contains("www.") || url_txtGraduate.Contains("www.") || url_txtNewfaculty.Contains("www.") || url_txtNewfaculty.Contains("www.") || url_txtUndergraduate.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string country = null;
                        mode = 0;

                        if (ddlCountry_IET.SelectedIndex == 0)
                        {
                            country = null;
                        }
                        else
                        {
                            country = Convert.ToString(ddlCountry_IET.SelectedValue);

                            foreach (DataGridViewRow row in dgv_IndividualElg.Rows)
                            {
                                string grdCountry = row.Cells["Country"].Value.ToString().Trim();

                                if ((grdCountry == Convert.ToString(ddlCountry_IET.SelectedValue)))
                                {
                                    MessageBox.Show("This Country already Exist", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }

                        string grouplistid = string.Empty;

                        if (chk_individual_list.SelectedItems.Count > 0 && chk_individual_list.CheckedItems.Count > 0)
                            grouplistid = chk_individual_list.SelectedValue.ToString();
                        else
                            grouplistid = "0";

                        countryGroupList = MySqlDal.OpportunityDataOperations.GetCountryGroup();
                        countryList = MySqlDal.OpportunityDataOperations.GetCountries(1);

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = MySqlDal.OpportunityDataOperations.SaveIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(txtDegree.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd())),
                               replace.ConvertTextToUnicode(Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd())), Convert.ToString(ddl_IET_NR.SelectedValue), country, "Citizenship", mode, lang_val);
                        }
                        else
                        {
                            foreach (sci_countries oCountry in countryList)
                            {
                                string cname = oCountry.COUNTRY_CODE;
                                string indexname = oCountry.COUNTRY_ID.ToString();
                                int indval = 0;
                                indval = Convert.ToInt32(indexname);

                                dsIETClass = MySqlDal.OpportunityDataOperations.SaveIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), Convert.ToString(txtDegree.Text.TrimStart().TrimEnd()), Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd()),
                                 Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd()), Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_IET_NR.SelectedValue), cname.TrimStart().TrimEnd(), "Citizenship", mode, lang_val);
                            }

                            if (ddlCountry_IET.SelectedIndex == 0 && ddlcountrylistdtl.SelectedIndex == -1 && chk_individual_list.CheckedItems.Count == 0)
                            {
                                dsIETClass = MySqlDal.OpportunityDataOperations.SaveIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), Convert.ToString(txtDegree.Text.TrimStart().TrimEnd()), Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd()),
                                        Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd()), Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_IET_NR.SelectedValue), country, "Citizenship", mode, lang_val);
                            }

                            if (ddlcountrylistdtl.SelectedItems.Count > 0)
                            {
                                for (int i = 0; i < ddlcountrylistdtl.SelectedItems.Count; i++)
                                {
                                    string C_ID = ((System.Data.DataRowView)(ddlcountrylistdtl.SelectedItems[i])).Row.ItemArray[2].ToString();

                                    dsIETClass = MySqlDal.OpportunityDataOperations.SaveIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), Convert.ToString(txtDegree.Text.TrimStart().TrimEnd()), Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd()),
                                          Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd()), Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_IET_NR.SelectedValue), C_ID, "Citizenship", mode, lang_val);
                                }
                            }
                        }


                        lblMsg.Text = "Data inserted Successfully !!!!";
                        loadSavedValues();
                        lblMsg.Visible = true;

                        countryList = null;

                        MySqlDal.OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }

                else if (Convert.ToString(ddlElClassType.SelectedValue) == "organizationEligibility")
                {
                    string url_txtAcademic = txtAcademic.Text.TrimStart().TrimEnd();
                    string url_txtCommercial = txtCommercial.Text.TrimStart().TrimEnd();
                    string url_txtGovernment = txtGovernment.Text.TrimStart().TrimEnd();
                    string url_txtNonprofit = txtNonprofit.Text.TrimStart().TrimEnd();
                    string url_txtSme = txtSme.Text.TrimStart().TrimEnd();
                    string url_TextCity = TextCity.Text.TrimStart().TrimEnd();

                    if (url_txtAcademic.Contains("http://") || url_txtCommercial.Contains("http://") || url_txtGovernment.Contains("http://") || url_txtNonprofit.Contains("http://") || url_txtSme.Contains("http://") || url_TextCity.Contains("http://") ||
                        url_txtAcademic.Contains("https://") || url_txtCommercial.Contains("https://") || url_txtGovernment.Contains("https://") || url_txtNonprofit.Contains("https://") || url_txtSme.Contains("https://") || url_TextCity.Contains("https://") ||
                        url_txtAcademic.Contains("www.") || url_txtCommercial.Contains("www.") || url_txtGovernment.Contains("www.") || url_txtNonprofit.Contains("www.") || url_txtSme.Contains("www.") || url_TextCity.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        mode = 0;
                        string country = "", state = "";
                        string regionSpecific = "";

                        if (txtRegionSpecific.Text.Trim() == "")
                            regionSpecific = "regionspecific";
                        else
                            regionSpecific = txtRegionSpecific.Text.Trim();

                        if (ddlState.SelectedIndex > 0)
                            state = Convert.ToString(ddlState.SelectedValue);

                        foreach (DataGridViewRow row in dgv_orgElg.Rows)
                        {
                            string grdOrgCountry = row.Cells["CountryOrg"].Value.ToString().Trim();

                            if ((grdOrgCountry == Convert.ToString(DDLCOUNTRY.SelectedValue)))
                            {
                                MessageBox.Show("This Country already Exist", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        string orggrouplistid = string.Empty;
                        string chkval = string.Empty;

                        chkval = chkbox_orgnEligibilityy.SelectedValue.ToString();

                        if (chkbox_orgnEligibilityy.SelectedItems.Count > 0 && chkbox_orgnEligibilityy.CheckedItems.Count > 0)
                            orggrouplistid = chkbox_orgnEligibilityy.SelectedValue.ToString();
                        else
                            orggrouplistid = "0";

                        countryList = MySqlDal.OpportunityDataOperations.GetCountries(Convert.ToInt64((orggrouplistid)));

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = MySqlDal.OpportunityDataOperations.SaveOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd())),
                               replace.ConvertTextToUnicode(Convert.ToString(txtGovernment.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtSme.Text.TrimStart().TrimEnd())), Convert.ToString(ddl_ORG_NR.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(TextCity.Text.TrimStart().TrimEnd())), state, country, regionSpecific, "Citizenship", mode, lang_val);
                        }
                        else
                        {
                            foreach (sci_countries count in countryList)
                            {
                                string cname = count.COUNTRY_CODE;
                                string indexname = count.COUNTRY_ID.ToString();

                                dsIETClass = MySqlDal.OpportunityDataOperations.SaveOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd()), Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd()),
                                Convert.ToString(txtGovernment.Text.TrimStart().TrimEnd()), Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd()), Convert.ToString(txtSme.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_ORG_NR.SelectedValue), Convert.ToString(TextCity.Text.TrimStart().TrimEnd()), state, cname, regionSpecific, "Citizenship", mode, lang_val);
                            }

                            if (listcountrydtl.SelectedIndex == -1 && chkbox_orgnEligibilityy.CheckedItems.Count == 0 && DDLCOUNTRY.SelectedIndex == 0)
                            {
                                dsIETClass = MySqlDal.OpportunityDataOperations.SaveOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd()), Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd()),
                                        Convert.ToString(txtGovernment.Text.TrimStart().TrimEnd()), Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd()), Convert.ToString(txtSme.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_ORG_NR.SelectedValue), Convert.ToString(TextCity.Text.TrimStart().TrimEnd()), state, country, regionSpecific, "Citizenship", mode, lang_val);
                            }

                            if (listcountrydtl.SelectedItems.Count > 0)
                            {
                                for (int i = 0; i < listcountrydtl.SelectedItems.Count; i++)
                                {
                                    string C_ID = ((System.Data.DataRowView)(listcountrydtl.SelectedItems[i])).Row.ItemArray[0].ToString();

                                    dsIETClass = MySqlDal.OpportunityDataOperations.SaveOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd()), Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd()),
                                     Convert.ToString(txtGovernment.Text.TrimStart().TrimEnd()), Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd()), Convert.ToString(txtSme.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_ORG_NR.SelectedValue), Convert.ToString(TextCity.Text.TrimStart().TrimEnd()), state, C_ID, regionSpecific, "Citizenship", mode, lang_val);
                                }
                            }
                        }

                        lblMsg.Text = "Data inserted Successfully !!!!";
                        loadSavedValues();
                        lblMsg.Visible = true;

                        countryList = null;

                        MySqlDal.OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }
                else if (Convert.ToString(ddlElClassType.SelectedValue) == "restrictions")
                {
                    string url_txtDisabilities = txtDisabilities.Text.TrimStart().TrimEnd();
                    string url_txtInvitationonly = txtInvitationonly.Text.TrimStart().TrimEnd();
                    string url_txtMemberonly = txtMemberonly.Text.TrimStart().TrimEnd();
                    string url_txtNominationonly = txtNominationonly.Text.TrimStart().TrimEnd();
                    string url_txtMinorities = txtMinorities.Text.TrimStart().TrimEnd();
                    string url_txtWomen = txtWomen.Text.TrimStart().TrimEnd();
                    string url_txtNumberofapplicantsallowed = txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd();

                    if (url_txtDisabilities.Contains("http://") || url_txtInvitationonly.Contains("http://") || url_txtMemberonly.Contains("http://") || url_txtNominationonly.Contains("http://") || url_txtMinorities.Contains("http://") || url_txtWomen.Contains("http://") || url_txtNumberofapplicantsallowed.Contains("http://") ||
                        url_txtDisabilities.Contains("https://") || url_txtInvitationonly.Contains("https://") || url_txtMemberonly.Contains("https://") || url_txtNominationonly.Contains("https://") || url_txtMinorities.Contains("https://") || url_txtWomen.Contains("https://") || url_txtNumberofapplicantsallowed.Contains("https://") ||
                        url_txtDisabilities.Contains("www.") || url_txtInvitationonly.Contains("www.") || url_txtMemberonly.Contains("www.") || url_txtNominationonly.Contains("www.") || url_txtMinorities.Contains("www.") || url_txtWomen.Contains("www.") || url_txtNumberofapplicantsallowed.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        mode = 0;

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = OpportunityDataOperations.SaveRestrictions(WFID, Convert.ToString(ddl_Rest_NS.SelectedValue),
                                replace.ConvertTextToUnicode(Convert.ToString(txtDisabilities.Text.TrimStart().TrimEnd())),
                                replace.ConvertTextToUnicode(Convert.ToString(txtInvitationonly.Text.TrimStart().TrimEnd())),
                                replace.ConvertTextToUnicode(Convert.ToString(txtMemberonly.Text.TrimStart().TrimEnd())),
                                replace.ConvertTextToUnicode(Convert.ToString(txtNominationonly.Text.TrimStart().TrimEnd())),
                                replace.ConvertTextToUnicode(Convert.ToString(txtMinorities.Text.TrimStart().TrimEnd())),
                                replace.ConvertTextToUnicode(Convert.ToString(txtWomen.Text.TrimStart().TrimEnd())),
                                Convert.ToString(txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd()),
                          "limitedsubmission", mode, lang_val);
                        }
                        else
                        {
                            dsIETClass = OpportunityDataOperations.SaveRestrictions(WFID, Convert.ToString(ddl_Rest_NS.SelectedValue),
                                Convert.ToString(txtDisabilities.Text.TrimStart().TrimEnd()), Convert.ToString(txtInvitationonly.Text.TrimStart().TrimEnd()),
                                Convert.ToString(txtMemberonly.Text.TrimStart().TrimEnd()), Convert.ToString(txtNominationonly.Text.TrimStart().TrimEnd()),
                                Convert.ToString(txtMinorities.Text.TrimStart().TrimEnd()), Convert.ToString(txtWomen.Text.TrimStart().TrimEnd()),
                                Convert.ToString(txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd()),
                             "limitedsubmission", mode, lang_val);
                        }

                        if (dsIETClass.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                        {
                            lblMsg.Text = "Data inserted Successfully !!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;

                            if (dsIETClass.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                MySqlDal.OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Something went wrong!!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;
                        }
                    }
                }
                else if (ddlElClassText.SelectedItems.Count == 0) { }
                else
                {
                    string classId = string.Empty; string sep = string.Empty;
                    string ClssText = string.Empty;

                    for (int i = 0; i < ddlElClassText.SelectedItems.Count; i++)
                    {
                        classId = classId + sep + ((System.Data.DataRowView)(ddlElClassText.SelectedItems[i])).Row.ItemArray[0].ToString();
                        ClssText = ClssText + sep + ((System.Data.DataRowView)(ddlElClassText.SelectedItems[i])).Row.ItemArray[2].ToString();
                        sep = ",";
                    }
                    char[] charsToTrim = { ' ', '\t' };
                    string result = ClssText.Trim(charsToTrim);

                    string country = null;
                    string state = string.Empty;

                    if (DDLCOUNTRY.SelectedIndex != 0)
                        country = DDLCOUNTRY.SelectedValue.ToString();

                    DataSet dsEClass = new DataSet();

                    if (_isregionSpecific)
                    {
                        string CountryId = string.Empty; string _sep = string.Empty;

                        for (int i = 0; i < ddlcountrylist.SelectedItems.Count; i++)
                        {
                            CountryId = CountryId + _sep + ((System.Data.DataRowView)(ddlcountrylist.SelectedItems[i])).Row.ItemArray[0].ToString();
                            _sep = ",";
                        }

                        char[] _charsToTrim = { ' ', '\t' };
                        string[] _result = CountryId.Trim(_charsToTrim).Split(',');

                        for (int i = 0; i < _result.Count(); i++)
                        {
                            dsEClass = OpportunityDataOperations.SaveElClassification(WFID, Convert.ToString(ddlElClassType.SelectedValue), classId, result, "", "", "", 0,
                                       _result[i], state, TextCity.Text.Trim(), 0);
                        }
                    }
                    else
                    {
                        dsEClass = OpportunityDataOperations.SaveElClassification(WFID, Convert.ToString(ddlElClassType.SelectedValue), classId, result, "", "", "", 0,
                                       country, state, TextCity.Text.Trim(), 0);
                    }

                    if (Convert.ToString(dsEClass.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        if (dsEClass.Tables["DisplayData"].Rows.Count > 0)
                        {
                            grdElClass.AutoGenerateColumns = false;
                            grdElClass.DataSource = dsEClass.Tables["DisplayData"];
                            DTDisplay = dsEClass.Tables["displaydata"].Copy();
                            ddlElClassType.SelectedIndex = 0;

                            foreach (int i in ChbxCountryList.CheckedIndices)
                                ChbxCountryList.SetItemCheckState(i, CheckState.Unchecked);
                            foreach (int i in Cbhxregionspecific.CheckedIndices)
                                Cbhxregionspecific.SetItemCheckState(i, CheckState.Unchecked);

                            ChbxCountryList.Visible = false;

                            Cbhxregionspecific.Visible = false;
                            DDLCOUNTRY.SelectedIndex = 0;
                            pnlregionspecific.Visible = false;
                        }
                        else
                        {
                            NoRecord();
                        }

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                            opportunity.GetProcess_update("Eligibilityclassification");
                        else
                            opportunity.GetProcess();
                        #endregion
                    }

                    lblMsg.Visible = true;
                    lblMsg.Text = dsEClass.Tables["ERRORCODE"].Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
                lblMsg.Visible = true;
                lblMsg.Text = "ERROR WHILE SAVEING DATA";
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
            pnlURL.Controls.Add(objPage);
        }

        private void ddlElClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                _isregionSpecific = false;

                if (Convert.ToString(ddlElClassType.SelectedValue) != "SelectType")
                {
                    if (Convert.ToString(ddlElClassType.SelectedValue) == "citizenship")
                        ChbxCountryList.Visible = true;
                    else if (Convert.ToString(ddlElClassType.SelectedValue) == "individualEligibility")
                    {
                        btnDeleteGrouping.Visible = true;
                        chk_individual_list.Width = 232;
                        pnl_individualEligibilityType.Visible = true;
                        txtDegree.Text = "";
                        txtGraduate.Text = "";
                        txtNewfaculty.Text = "";
                        txtUndergraduate.Text = "";
                        ddlCountry_IET.SelectedIndex = 0;
                        ddl_IET_NR.SelectedIndex = 0;
                        ddl_IET_NS.SelectedIndex = 0;
                        pnl_organizationEligibilityType.Visible = false;
                        pnlRestrictionsType.Visible = false;
                        ddlcountrylistdtl.Visible = true;
                    }
                    else if (Convert.ToString(ddlElClassType.SelectedValue) == "organizationEligibility")
                    {
                        btnDeleteGrouping.Visible = true;
                        chkbox_orgnEligibilityy.Width = 232;

                        pnl_individualEligibilityType.Visible = false;
                        pnl_organizationEligibilityType.Visible = true;
                        txtAcademic.Text = "";
                        txtCommercial.Text = "";
                        txtGovernment.Text = "";
                        txtNonprofit.Text = "";
                        txtSme.Text = "";
                        txtWomen.Text = "";
                        TextCity.Text = "";
                        DDLCOUNTRY.SelectedIndex = 0;
                        ddlState.SelectedIndex = 0;
                        ddl_ORG_NR.SelectedIndex = 0;
                        ddl_ORG_NS.SelectedIndex = 0;
                        pnlRestrictionsType.Visible = false;
                        ddlcountrylistdtl.Visible = true;
                        chkbox_orgnEligibilityy.Visible = true;
                        DDLCOUNTRY.Visible = false;

                        countryList = MySqlDal.OpportunityDataOperations.GetCountries(1);

                        chkbox_orgnEligibilityy.DataSource = countryList;
                        chkbox_orgnEligibilityy.ValueMember = "COUNTRYGROUP_ID";
                        chkbox_orgnEligibilityy.DisplayMember = "COUNTRYGROUP_NAME";
                    }
                    else if (Convert.ToString(ddlElClassType.SelectedValue) == "restrictions")
                    {
                        btnDeleteGrouping.Visible = false;
                        pnl_individualEligibilityType.Visible = false;
                        pnl_organizationEligibilityType.Visible = false;
                        pnlRestrictionsType.Visible = true;
                        txtDisabilities.Text = "";
                        txtInvitationonly.Text = "";
                        txtMemberonly.Text = "";
                        txtNominationonly.Text = "";
                        txtMinorities.Text = "";
                        txtWomen.Text = "";
                        txtNumberofapplicantsallowed.Text = "";
                        ddl_Rest_NS.SelectedIndex = 0;
                        chkbox_orgnEligibilityy.Visible = false;
                    }
                    else
                    {
                        ChbxCountryList.Visible = false;

                        foreach (int i in ChbxCountryList.CheckedIndices)
                            ChbxCountryList.SetItemCheckState(i, CheckState.Unchecked);
                    }

                    DataTable dt = dtEclassText;
                    dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                    DataTable dtView = dt.DefaultView.ToTable();
                    ddlElClassText.DataSource = dtView;
                    ddlElClassText.DisplayMember = "EligibilityClassification";
                    ddlElClassText.ValueMember = "Value";

                    ddlElClassText.ClearSelected();
                    ddlElClassText.Enabled = true;
                }
                else
                {
                    ddlElClassText.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdElClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Cbhxregionspecific.Visible = false;

                if (DTDisplay.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            ddlElClassType.SelectedValue = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["type"].ToString();

                            DataTable dtClass = dtEclassText;
                            dtClass.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                            DataTable dtClassView = dtClass.DefaultView.ToTable();

                            ddlElClassText.DataSource = dtClassView;
                            ddlElClassText.DisplayMember = "EligibilityClassification";
                            ddlElClassText.ValueMember = "Value";
                            ddlElClassText.SelectedValue = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["ID"].ToString();

                            btnSave.Visible = false;
                            btnaddurl.Visible = false;
                            btnUpdate.Visible = true;
                            btnCancel.Visible = true;

                            ddlElClassText.SelectionMode = SelectionMode.One;

                            TextCity.Text = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["CITY"].ToString();

                            string othetState = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["STATE"].ToString();
                            string othetStatecode = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["STATE"].ToString();
                            string country = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["COUNTRY_CODE"].ToString();

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
                                    ddlState.SelectedValue = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["STATE"].ToString();
                                }
                                else if (dtStateResult1.Rows.Count > 0)
                                {
                                    ddlState.SelectedValue = DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["STATE"].ToString();
                                }
                            }
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

        private void grdElClass_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (DTDisplay.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            Int64 SPECIFIC_ID = 0;

                            if (DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["REGION_SPECIFIC_ID"].ToString() != "")
                                SPECIFIC_ID = Convert.ToInt64(DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["REGION_SPECIFIC_ID"]);

                            DataSet dsresult = OpportunityDataOperations.SaveElClassification(SharedObjects.WorkId, DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["type"].ToString(),
                                                DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["id"].ToString(), DTDisplay.Rows[grdElClass.SelectedCells[0].RowIndex]["eligibilityclassification_text"].ToString(),
                                                "", "", "", 2, "", "", "", SPECIFIC_ID);
                            //BindGrid AGAIN
                            if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                            {
                                grdElClass.AutoGenerateColumns = false;
                                grdElClass.DataSource = dsresult.Tables["displaydata"];
                                DTDisplay = dsresult.Tables["displaydata"].Copy();
                            }
                            else
                            {
                                NoRecord();
                            }

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                                opportunity.GetProcess_update("Eligibilityclassification");
                            else
                                opportunity.GetProcess();
                            #endregion

                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (ddlElClassType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (ddlElClassText.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string classId = string.Empty; string sep = string.Empty;
                    string ClssText = string.Empty;

                    for (int i = 0; i < ddlElClassText.SelectedItems.Count; i++)
                    {
                        classId = classId + sep + ((System.Data.DataRowView)(ddlElClassText.SelectedItems[i])).Row.ItemArray[0].ToString();
                        ClssText = ClssText + sep + ((System.Data.DataRowView)(ddlElClassText.SelectedItems[i])).Row.ItemArray[2].ToString();
                        sep = ",";
                    }

                    char[] charsToTrim = { ' ', '\t' };
                    string result = ClssText.Trim(charsToTrim);

                    string country = null;
                    string state = string.Empty;
                    Int64 SPECIFIC_ID = 0;

                    if (DTDisplay.Rows[rowindex]["REGION_SPECIFIC_ID"].ToString() != "")
                    {
                        SPECIFIC_ID = Convert.ToInt64(DTDisplay.Rows[rowindex]["REGION_SPECIFIC_ID"]);
                    }

                    if (Convert.ToString(ddlElClassType.SelectedValue) == "regionspecific")
                    {
                        if (DDLCOUNTRY.SelectedIndex != 0)
                            country = DDLCOUNTRY.SelectedValue.ToString();

                        if (ddlState.SelectedValue.ToString() != "SelectState") { }
                        else
                            state = "";
                    }

                    DataSet dsEClass = OpportunityDataOperations.SaveElClassification(WFID, DTDisplay.Rows[rowindex]["type"].ToString(), DTDisplay.Rows[rowindex]["id"].ToString(),
                        DTDisplay.Rows[rowindex]["EligibilityClassification_text"].ToString(), Convert.ToString(ddlElClassType.SelectedValue), classId, result,
                        1, country, state, TextCity.Text.Trim(), SPECIFIC_ID);

                    if (Convert.ToString(dsEClass.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        if (dsEClass.Tables["DisplayData"].Rows.Count > 0)
                        {
                            grdElClass.AutoGenerateColumns = false;
                            grdElClass.DataSource = dsEClass.Tables["DisplayData"];
                            DTDisplay = dsEClass.Tables["displaydata"].Copy();

                            ddlElClassType.SelectedIndex = 0;
                            ddlElClassText.Enabled = false;
                            ddlElClassText.SelectionMode = SelectionMode.MultiExtended;

                            foreach (int i in ChbxCountryList.CheckedIndices)
                                ChbxCountryList.SetItemCheckState(i, CheckState.Unchecked);

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                opportunity.GetProcess_update("Eligibilityclassification");
                            }
                            else
                            {
                                opportunity.GetProcess();
                            }
                            #endregion
                        }
                        else
                        {
                            NoRecord();
                        }
                    }

                    btnSave.Visible = true;
                    btnaddurl.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;

                    lblMsg.Visible = true;
                    lblMsg.Text = dsEClass.Tables["ERRORCODE"].Rows[0][1].ToString();

                    MySqlDal.OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            btnSave.Visible = true;
            btnaddurl.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
            ddlElClassType.SelectedIndex = 0;
            ddlElClassText.Enabled = false;
            DDLCOUNTRY.SelectedIndex = 0;

            ddlElClassText.SelectionMode = SelectionMode.MultiExtended;

            foreach (int i in ChbxCountryList.CheckedIndices)
                ChbxCountryList.SetItemCheckState(i, CheckState.Unchecked);

            foreach (int i in Cbhxregionspecific.CheckedIndices)
                Cbhxregionspecific.SetItemCheckState(i, CheckState.Unchecked);
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
            lblMsg.Visible = false;
        }

        private void ChbxCountryList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && ChbxCountryList.CheckedItems.Count > 0)
            {
                ChbxCountryList.ItemCheck -= ChbxCountryList_ItemCheck;
                ChbxCountryList.SetItemChecked(ChbxCountryList.CheckedIndices[0], false);
                ChbxCountryList.ItemCheck += ChbxCountryList_ItemCheck;
            }

            string _countryIndex = ChbxCountryList.SelectedIndex.ToString();
            string _isCountryselected = "21";

            for (int i = 0; i < ChbxCountryList.Items.Count; i++)
                if (ChbxCountryList.GetItemChecked(i))
                    _isCountryselected = (string)ChbxCountryList.SelectedIndex.ToString();

            switch (_countryIndex)
            {
                #region EU Members
                case "0":
                    if (_countryIndex.Equals("0") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Austria" || p == "Belgium" || p == "Bulgaria" || p == "Croatia" || p == "Cyprus" || p == "Czechia" || p == "Denmark"
                                || p == "Estonia" || p == "Finland" || p == "France" || p == "Germany" || p == "Greece" || p == "Hungary" || p == "Ireland" || p == "Italy"
                                 || p == "Latvia" || p == "Lithuania" || p == "Luxembourg" || p == "Malta" || p == "Netherlands" || p == "Poland" || p == "Portugal"
                                 || p == "Romania" || p == "Slovakia" || p == "Slovenia" || p == "Spain" || p == "Sweden" || p == "United Kingdom" || p == "Czechia")
                            {
                                ddlElClassText.SetSelected(i, true);

                            }
                        }
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Low-Income Economies Country
                case "1":
                    if (_countryIndex.Equals("1") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Burkina Faso" ||
                                p == "Burundi" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Comoros" ||
                                p == "Congo" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Gambia" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Haiti" ||
                                p == "North Korea" ||
                                p == "Liberia" ||
                                p == "Madagascar" ||
                                p == "Malawi" ||
                                p == "Mali" ||
                                p == "Mozambique" ||
                                p == "Nepal" ||
                                p == "Niger" ||
                                p == "Rwanda" ||
                                p == "Senegal" ||
                                p == "Sierra Leone" ||
                                p == "Somalia" ||
                                p == "Sudan" ||
                                p == "Tanzania" ||
                                p == "Togo" ||
                                p == "Uganda" ||
                                p == "Zimbabwe" ||
                                p == "Afghanistan" ||
                                p == "Benin")
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Commonwealth Countries
                case "2":
                    if (_countryIndex.Equals("2") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Botswana" ||
                                p == "Cameroon" ||
                                p == "Ghana" ||
                                p == "Kenya" ||
                                p == "Lesotho" ||
                                p == "Malawi" ||
                                p == "Mauritius" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "Nigeria" ||
                                p == "Rwanda" ||
                                p == "Seychelles" ||
                                p == "Sierra Leone" ||
                                p == "South Africa" ||
                                p == "Swaziland" ||
                                p == "Uganda" ||
                                p == "Tanzania" ||
                                p == "Zambia" ||
                                p == "Bangladesh" ||
                                p == "Brunei Darussalam" ||
                                p == "India" ||
                                p == "Malaysia" ||
                                p == "Pakistan" ||
                                p == "Singapore" ||
                                p == "Sri Lanka" ||
                                p == "Antigua and Barbuda" ||
                                p == "Bahamas" ||
                                p == "Barbados" ||
                                p == "Belize" ||
                                p == "Canada" ||
                                p == "Dominica" ||
                                p == "Grenada" ||
                                p == "Guyana" ||
                                p == "Jamaica" ||
                                p == "Saint Lucia" ||
                                p == "Trinidad and Tobago" ||
                                p == "Cyprus" ||
                                p == "Malta" ||
                                p == "United Kingdom" ||
                                p == "Australia" ||
                                p == "Fiji" ||
                                p == "Kiribati" ||
                                p == "Nauru" ||
                                p == "New Zealand" ||
                                p == "Papua New Guinea" ||
                                p == "Samoa" ||
                                p == "Solomon Islands" ||
                                p == "Tonga" ||
                                p == "Tuvalu" ||
                                p == "Vanuatu" ||
                                p == "Saint Kitts and Nevis" ||
                                p == "Saint Vincent and the Grenadines"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Lower-Middle-Income Countries
                case "3":
                    if (_countryIndex.Equals("3") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Angola" ||
                                p == "Armenia" ||
                                p == "Bangladesh" ||
                                p == "Bhutan" ||
                                p == "Bolivia" ||
                                p == "Cape Verde" ||
                                p == "Cambodia" ||
                                p == "Cameroon" ||
                                p == "Congo" ||
                                p == "Cote d'Ivoire" ||
                                p == "Djibouti" ||
                                p == "Egypt" ||
                                p == "El Salvador" ||
                                p == "Georgia" ||
                                p == "Ghana" ||
                                p == "Guatemala" ||
                                p == "Honduras" ||
                                p == "India" ||
                                p == "Indonesia" ||
                                p == "Jordan" ||
                                p == "Kenya" ||
                                p == "Kiribati" ||
                                p == "Kyrgyzstan" ||
                                p == "Laos" ||
                                p == "Lesotho" ||
                                p == "Mauritania" ||
                                p == "Moldova" ||
                                p == "Mongolia" ||
                                p == "Morocco" ||
                                p == "Myanmar" ||
                                p == "Nicaragua" ||
                                p == "Nigeria" ||
                                p == "Pakistan" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "Sao Tome and Principe" ||
                                p == "Solomon Islands" ||
                                p == "Sri Lanka" ||
                                p == "Sudan" ||
                                p == "Swaziland" ||
                                p == "Syrian Arab Republic" ||
                                p == "Tajikistan" ||
                                p == "Timor-Leste" ||
                                p == "Tunisia" ||
                                p == "Ukraine" ||
                                p == "Uzbekistan" ||
                                p == "Vanuatu" ||
                                p == "Viet Nam" ||
                                p == "Yemen" ||
                                p == "Zambia" ||
                                p == "Federated States of Micronesia"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }

                        string _country = string.Empty;
                        _country += "1. Kosovo"
                                + Environment.NewLine + Environment.NewLine +
                                "2. West Bank and Gaza";

                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region African Union Countries
                case "4":
                    if (_countryIndex.Equals("4") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Algeria" ||
                                p == "Angola" ||
                                p == "Benin" ||
                                p == "Botswana" ||
                                p == "Burkina Faso" ||
                                p == "Burundi" ||
                                p == "Cape Verde" ||
                                p == "Cameroon" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Comoros" ||
                                p == "Cote d'Ivoire" ||
                                p == "Djibouti" ||
                                p == "Democratic Republic Congo" ||
                                p == "Egypt" ||
                                p == "Equatorial Guinea" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Gabon" ||
                                p == "Gambia" ||
                                p == "Ghana" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Kenya" ||
                                p == "Lesotho" ||
                                p == "Liberia" ||
                                p == "Libyan Arab Jamahiriya" ||
                                p == "Madagascar" ||
                                p == "Malawi" ||
                                p == "Mali" ||
                                p == "Mauritania" ||
                                p == "Mauritius" ||
                                p == "Morocco" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "Niger" ||
                                p == "Nigeria" ||
                                p == "Congo" ||
                                p == "Rwanda" ||
                                p == "Sao Tome and Principe" ||
                                p == "Senegal" ||
                                p == "Seychelles" ||
                                p == "Sierra Leone" ||
                                p == "Somalia" ||
                                p == "South Africa" ||
                                p == "Sudan" ||
                                p == "Sudan" ||
                                p == "Swaziland" ||
                                p == "Tanzania" ||
                                p == "Togo" ||
                                p == "Tunisia" ||
                                p == "Uganda" ||
                                p == "Zambia" ||
                                p == "Zimbabwe"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                        string _country = string.Empty;
                        _country += "1. Sahrawi Arab Democratic Republic (disputed state)";

                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Asia Pacific
                case "5":
                    if (_countryIndex.Equals("5") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Afghanistan" ||
                                p == "Bahrain" ||
                                p == "Bangladesh" ||
                                p == "Bhutan" ||
                                p == "Brunei Darussalam" ||
                                p == "Cambodia" ||
                                p == "China" ||
                                p == "Cyprus" ||
                                p == "North Korea" ||
                                p == "Fiji" ||
                                p == "India" ||
                                p == "Indonesia" ||
                                p == "Iran" ||
                                p == "Iraq" ||
                                p == "Japan" ||
                                p == "Jordan" ||
                                p == "Kazakhstan" ||
                                p == "Kiribati" ||
                                p == "Kuwait" ||
                                p == "Kyrgyzstan" ||
                                p == "Laos" ||
                                p == "Lebanon" ||
                                p == "Malaysia" ||
                                p == "Maldives" ||
                                p == "Marshall Islands" ||
                                p == "Mongolia" ||
                                p == "Myanmar" ||
                                p == "Nauru" ||
                                p == "Nepal" ||
                                p == "Oman" ||
                                p == "Pakistan" ||
                                p == "Palau" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "Qatar" ||
                                p == "South Korea" ||
                                p == "Samoa" ||
                                p == "Saudi Arabia" ||
                                p == "Singapore" ||
                                p == "Solomon Islands" ||
                                p == "Sri Lanka" ||
                                p == "Syrian Arab Republic" ||
                                p == "Tajikistan" ||
                                p == "Thailand" ||
                                p == "Timor-Leste" ||
                                p == "Tonga" ||
                                p == "Turkey*" ||
                                p == "Turkmenistan" ||
                                p == "Tuvalu" ||
                                p == "United Arab Emirates" ||
                                p == "Uzbekistan" ||
                                p == "Vanuatu" ||
                                p == "Viet Nam" ||
                                p == "Yemen" ||
                                p == "Federated States of Micronesia" ||
                                p == "Turkey"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Developed Countries
                case "6":
                    if (_countryIndex.Equals("6") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Austria" ||
                                p == "Belgium" ||
                                p == "Denmark" ||
                                p == "Finland" ||
                                p == "France" ||
                                p == "Germany" ||
                                p == "Greece" ||
                                p == "Ireland" ||
                                p == "Italy" ||
                                p == "Luxembourg" ||
                                p == "Netherlands" ||
                                p == "Portugal" ||
                                p == "Spain" ||
                                p == "Sweden" ||
                                p == "United Kingdom" ||
                                p == "Bulgaria" ||
                                p == "Croatia" ||
                                p == "Cyprus" ||
                                p == "Czechia" ||
                                p == "Estonia" ||
                                p == "Hungary" ||
                                p == "Latvia" ||
                                p == "Lithuania" ||
                                p == "Malta" ||
                                p == "Poland" ||
                                p == "Romania" ||
                                p == "Slovakia" ||
                                p == "Slovenia" ||
                                p == "Iceland" ||
                                p == "Norway" ||
                                p == "Switzerland" ||
                                p == "Australia" ||
                                p == "Canada" ||
                                p == "Japan" ||
                                p == "New Zealand" ||
                                p == "United States" ||
                                p == "Canada" ||
                                p == "Japan" ||
                                p == "France" ||
                                p == "Germany" ||
                                p == "Italy" ||
                                p == "United Kingdom" ||
                                p == "United States"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }

                    break;
                #endregion

                #region Developing Countries
                case "7":
                    if (_countryIndex.Equals("7") && _isCountryselected.Equals("21"))
                    {
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + Convert.ToString(ddlElClassType.SelectedValue) + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlElClassText.DataSource = dtView;
                        ddlElClassText.DisplayMember = "EligibilityClassification";
                        ddlElClassText.ValueMember = "Value";
                        ddlElClassText.Enabled = true;
                        ddlElClassText.ClearSelected();

                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Algeria" ||
                                p == "Egypt" ||
                                p == "Libyan Arab Jamahiriya" ||
                                p == "Mauritania" ||
                                p == "Morocco" ||
                                p == "Sudan" ||
                                p == "Tunisia" ||
                                p == "Cameroon" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Congo" ||
                                p == "Equatorial Guinea" ||
                                p == "Gabon" ||
                                p == "Sao Tome and Principe" ||
                                p == "Burundi" ||
                                p == "Comoros" ||
                                p == "Democratic Republic Congo" ||
                                p == "Djibouti" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Kenya" ||
                                p == "Madagascar" ||
                                p == "Rwanda" ||
                                p == "Somalia" ||
                                p == "Uganda" ||
                                p == "Tanzania" ||
                                p == "Angola" ||
                                p == "Botswana" ||
                                p == "Lesotho" ||
                                p == "Malawi" ||
                                p == "Mauritius" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "South Africa" ||
                                p == "Zambia" ||
                                p == "Zimbabwe" ||
                                p == "Benin" ||
                                p == "Burkina Faso" ||
                                p == "Cape Verde" ||
                                p == "Cote d'Ivoire" ||
                                p == "Gambia" ||
                                p == "Ghana" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Liberia" ||
                                p == "Mali" ||
                                p == "Niger" ||
                                p == "Nigeria" ||
                                p == "Senegal" ||
                                p == "Sierra Leone" ||
                                p == "Togo" ||
                                p == "Brunei Darussalam" ||
                                p == "China" ||
                                p == "Hong Kong" ||
                                p == "Indonesia" ||
                                p == "Malaysia" ||
                                p == "Myanmar" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "South Korea" ||
                                p == "Singapore" ||
                                p == "Thailand" ||
                                p == "Viet Nam" ||
                                p == "Bangladesh" ||
                                p == "India" ||
                                p == "Iran" ||
                                p == "Nepal" ||
                                p == "Pakistan" ||
                                p == "Sri Lanka" ||
                                p == "Bahrain" ||
                                p == "Iraq" ||
                                p == "Israel" ||
                                p == "Jordan" ||
                                p == "Kuwait" ||
                                p == "Lebanon" ||
                                p == "Oman" ||
                                p == "Qatar" ||
                                p == "Saudi Arabia" ||
                                p == "Syrian Arab Republic" ||
                                p == "Turkey" ||
                                p == "United Arab Emirates" ||
                                p == "Yemen" ||
                                p == "Barbados" ||
                                p == "Cuba" ||
                                p == "Dominican Republic" ||
                                p == "Guyana" ||
                                p == "Haiti" ||
                                p == "Jamaica" ||
                                p == "Trinidad and Tobago" ||
                                p == "Costa Rica" ||
                                p == "El Salvador" ||
                                p == "Guatemala" ||
                                p == "Honduras" ||
                                p == "Mexico" ||
                                p == "Nicaragua" ||
                                p == "Panama" ||
                                p == "Argentina" ||
                                p == "Bolivia" ||
                                p == "Brazil" ||
                                p == "Chile" ||
                                p == "Colombia" ||
                                p == "Ecuador" ||
                                p == "Paraguay" ||
                                p == "Peru" ||
                                p == "Uruguay" ||
                                p == "Venezuela"
                                )
                            {
                                ddlElClassText.SetSelected(i, true);
                            }
                        }
                        string _country = string.Empty;
                        _country += "1. Taiwan Province of China";

                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlElClassText.SelectedIndex = -1;
                        ddlElClassText.Enabled = true;
                    }
                    break;
                    #endregion
            }
        }

        private void Cbhxregionspecific_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && Cbhxregionspecific.CheckedItems.Count > 0)
            {
                Cbhxregionspecific.ItemCheck -= Cbhxregionspecific_ItemCheck;
                Cbhxregionspecific.SetItemChecked(Cbhxregionspecific.CheckedIndices[0], false);
                Cbhxregionspecific.ItemCheck += Cbhxregionspecific_ItemCheck;
            }

            string _countryIndex = Cbhxregionspecific.SelectedIndex.ToString();
            string _isCountryselected = "21";

            for (int i = 0; i < Cbhxregionspecific.Items.Count; i++)
            {
                if (Cbhxregionspecific.GetItemChecked(i))
                {
                    _isCountryselected = (string)Cbhxregionspecific.SelectedIndex.ToString();
                }
            }

            switch (_countryIndex)
            {
                #region EU Members
                case "0":
                    if (_countryIndex.Equals("0") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Austria" || p == "Belgium" || p == "Bulgaria" || p == "Croatia" || p == "Cyprus" || p == "Czechia" || p == "Denmark"
                                || p == "Estonia" || p == "Finland" || p == "France" || p == "Germany" || p == "Greece" || p == "Hungary" || p == "Ireland" || p == "Italy"
                                 || p == "Latvia" || p == "Lithuania" || p == "Luxembourg" || p == "Malta" || p == "Netherlands" || p == "Poland" || p == "Portugal"
                                 || p == "Romania" || p == "Slovakia" || p == "Slovenia" || p == "Spain" || p == "Sweden" || p == "United Kingdom" || p == "Czechia")
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Low-Income Economies Country
                case "1":
                    if (_countryIndex.Equals("1") && _isCountryselected.Equals("21"))
                    {
                        _isregionSpecific = true;
                        pnlregionspecific.Visible = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Burkina Faso" ||
                                p == "Burundi" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Comoros" ||
                                p == "Congo" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Gambia" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Haiti" ||
                                p == "North Korea" ||
                                p == "Liberia" ||
                                p == "Madagascar" ||
                                p == "Malawi" ||
                                p == "Mali" ||
                                p == "Mozambique" ||
                                p == "Nepal" ||
                                p == "Niger" ||
                                p == "Rwanda" ||
                                p == "Senegal" ||
                                p == "Sierra Leone" ||
                                p == "Somalia" ||
                                p == "Sudan" ||
                                p == "Tanzania" ||
                                p == "Togo" ||
                                p == "Uganda" ||
                                p == "Zimbabwe" ||
                                p == "Afghanistan" ||
                                p == "Benin")
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Commonwealth Countries
                case "2":
                    if (_countryIndex.Equals("2") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Botswana" ||
                                p == "Cameroon" ||
                                p == "Ghana" ||
                                p == "Kenya" ||
                                p == "Lesotho" ||
                                p == "Malawi" ||
                                p == "Mauritius" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "Nigeria" ||
                                p == "Rwanda" ||
                                p == "Seychelles" ||
                                p == "Sierra Leone" ||
                                p == "South Africa" ||
                                p == "Swaziland" ||
                                p == "Uganda" ||
                                p == "Tanzania" ||
                                p == "Zambia" ||
                                p == "Bangladesh" ||
                                p == "Brunei Darussalam" ||
                                p == "India" ||
                                p == "Malaysia" ||
                                p == "Pakistan" ||
                                p == "Singapore" ||
                                p == "Sri Lanka" ||
                                p == "Antigua and Barbuda" ||
                                p == "Bahamas" ||
                                p == "Barbados" ||
                                p == "Belize" ||
                                p == "Canada" ||
                                p == "Dominica" ||
                                p == "Grenada" ||
                                p == "Guyana" ||
                                p == "Jamaica" ||
                                p == "Saint Lucia" ||
                                p == "Trinidad and Tobago" ||
                                p == "Cyprus" ||
                                p == "Malta" ||
                                p == "United Kingdom" ||
                                p == "Australia" ||
                                p == "Fiji" ||
                                p == "Kiribati" ||
                                p == "Nauru" ||
                                p == "New Zealand" ||
                                p == "Papua New Guinea" ||
                                p == "Samoa" ||
                                p == "Solomon Islands" ||
                                p == "Tonga" ||
                                p == "Tuvalu" ||
                                p == "Vanuatu" ||
                                p == "Saint Kitts and Nevis" ||
                                p == "Saint Vincent and the Grenadines"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }

                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Lower-Middle-Income Countries
                case "3":
                    if (_countryIndex.Equals("3") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Angola" ||
                                p == "Armenia" ||
                                p == "Bangladesh" ||
                                p == "Bhutan" ||
                                p == "Bolivia" ||
                                p == "Cape Verde" ||
                                p == "Cambodia" ||
                                p == "Cameroon" ||
                                p == "Congo" ||
                                p == "Cote d'Ivoire" ||
                                p == "Djibouti" ||
                                p == "Egypt" ||
                                p == "El Salvador" ||
                                p == "Georgia" ||
                                p == "Ghana" ||
                                p == "Guatemala" ||
                                p == "Honduras" ||
                                p == "India" ||
                                p == "Indonesia" ||
                                p == "Jordan" ||
                                p == "Kenya" ||
                                p == "Kiribati" ||
                                p == "Kyrgyzstan" ||
                                p == "Laos" ||
                                p == "Lesotho" ||
                                p == "Mauritania" ||
                                p == "Moldova" ||
                                p == "Mongolia" ||
                                p == "Morocco" ||
                                p == "Myanmar" ||
                                p == "Nicaragua" ||
                                p == "Nigeria" ||
                                p == "Pakistan" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "Sao Tome and Principe" ||
                                p == "Solomon Islands" ||
                                p == "Sri Lanka" ||
                                p == "Sudan" ||
                                p == "Swaziland" ||
                                p == "Syrian Arab Republic" ||
                                p == "Tajikistan" ||
                                p == "Timor-Leste" ||
                                p == "Tunisia" ||
                                p == "Ukraine" ||
                                p == "Uzbekistan" ||
                                p == "Vanuatu" ||
                                p == "Viet Nam" ||
                                p == "Yemen" ||
                                p == "Zambia" ||
                                p == "Federated States of Micronesia"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                        string _country = string.Empty;
                        _country += "1. Kosovo"
                                + Environment.NewLine + Environment.NewLine +
                                "2. West Bank and Gaza";
                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region African Union Countries
                case "4":
                    if (_countryIndex.Equals("4") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Algeria" ||
                                p == "Angola" ||
                                p == "Benin" ||
                                p == "Botswana" ||
                                p == "Burkina Faso" ||
                                p == "Burundi" ||
                                p == "Cape Verde" ||
                                p == "Cameroon" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Comoros" ||
                                p == "Cote d'Ivoire" ||
                                p == "Djibouti" ||
                                p == "Democratic Republic Congo" ||
                                p == "Egypt" ||
                                p == "Equatorial Guinea" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Gabon" ||
                                p == "Gambia" ||
                                p == "Ghana" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Kenya" ||
                                p == "Lesotho" ||
                                p == "Liberia" ||
                                p == "Libyan Arab Jamahiriya" ||
                                p == "Madagascar" ||
                                p == "Malawi" ||
                                p == "Mali" ||
                                p == "Mauritania" ||
                                p == "Mauritius" ||
                                p == "Morocco" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "Niger" ||
                                p == "Nigeria" ||
                                p == "Congo" ||
                                p == "Rwanda" ||
                                p == "Sao Tome and Principe" ||
                                p == "Senegal" ||
                                p == "Seychelles" ||
                                p == "Sierra Leone" ||
                                p == "Somalia" ||
                                p == "South Africa" ||
                                p == "Sudan" ||
                                p == "Sudan" ||
                                p == "Swaziland" ||
                                p == "Tanzania" ||
                                p == "Togo" ||
                                p == "Tunisia" ||
                                p == "Uganda" ||
                                p == "Zambia" ||
                                p == "Zimbabwe"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                        string _country = string.Empty;
                        _country += "1. Sahrawi Arab Democratic Republic (disputed state)";
                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Asia Pacific
                case "5":
                    if (_countryIndex.Equals("5") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Afghanistan" ||
                                p == "Bahrain" ||
                                p == "Bangladesh" ||
                                p == "Bhutan" ||
                                p == "Brunei Darussalam" ||
                                p == "Cambodia" ||
                                p == "China" ||
                                p == "Cyprus" ||
                                p == "North Korea" ||
                                p == "Fiji" ||
                                p == "India" ||
                                p == "Indonesia" ||
                                p == "Iran" ||
                                p == "Iraq" ||
                                p == "Japan" ||
                                p == "Jordan" ||
                                p == "Kazakhstan" ||
                                p == "Kiribati" ||
                                p == "Kuwait" ||
                                p == "Kyrgyzstan" ||
                                p == "Laos" ||
                                p == "Lebanon" ||
                                p == "Malaysia" ||
                                p == "Maldives" ||
                                p == "Marshall Islands" ||
                                p == "Mongolia" ||
                                p == "Myanmar" ||
                                p == "Nauru" ||
                                p == "Nepal" ||
                                p == "Oman" ||
                                p == "Pakistan" ||
                                p == "Palau" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "Qatar" ||
                                p == "South Korea" ||
                                p == "Samoa" ||
                                p == "Saudi Arabia" ||
                                p == "Singapore" ||
                                p == "Solomon Islands" ||
                                p == "Sri Lanka" ||
                                p == "Syrian Arab Republic" ||
                                p == "Tajikistan" ||
                                p == "Thailand" ||
                                p == "Timor-Leste" ||
                                p == "Tonga" ||
                                p == "Turkey*" ||
                                p == "Turkmenistan" ||
                                p == "Tuvalu" ||
                                p == "United Arab Emirates" ||
                                p == "Uzbekistan" ||
                                p == "Vanuatu" ||
                                p == "Viet Nam" ||
                                p == "Yemen" ||
                                p == "Federated States of Micronesia" ||
                                p == "Turkey"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                        //string _country = string.Empty;
                        //_country += "1. Micronesia (Federated States of)";
                        //SharedObjects._notlistedcountry = _country;
                        //CountryNotInList rmkobj = new CountryNotInList();
                        //rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Developed Countries
                case "6":
                    if (_countryIndex.Equals("6") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Austria" ||
                                p == "Belgium" ||
                                p == "Denmark" ||
                                p == "Finland" ||
                                p == "France" ||
                                p == "Germany" ||
                                p == "Greece" ||
                                p == "Ireland" ||
                                p == "Italy" ||
                                p == "Luxembourg" ||
                                p == "Netherlands" ||
                                p == "Portugal" ||
                                p == "Spain" ||
                                p == "Sweden" ||
                                p == "United Kingdom" ||
                                p == "Bulgaria" ||
                                p == "Croatia" ||
                                p == "Cyprus" ||
                                p == "Czechia" ||
                                p == "Estonia" ||
                                p == "Hungary" ||
                                p == "Latvia" ||
                                p == "Lithuania" ||
                                p == "Malta" ||
                                p == "Poland" ||
                                p == "Romania" ||
                                p == "Slovakia" ||
                                p == "Slovenia" ||
                                p == "Iceland" ||
                                p == "Norway" ||
                                p == "Switzerland" ||
                                p == "Australia" ||
                                p == "Canada" ||
                                p == "Japan" ||
                                p == "New Zealand" ||
                                p == "United States" ||
                                p == "Canada" ||
                                p == "Japan" ||
                                p == "France" ||
                                p == "Germany" ||
                                p == "Italy" ||
                                p == "United Kingdom" ||
                                p == "United States"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);

                            }
                        }
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                #endregion
                #region Developing Countries
                case "7":
                    if (_countryIndex.Equals("7") && _isCountryselected.Equals("21"))
                    {
                        pnlregionspecific.Visible = true;
                        _isregionSpecific = true;
                        DataTable dt = dtEclassText;
                        dt.DefaultView.RowFilter = "Type='" + "citizenship" + "'";
                        DataTable dtView = dt.DefaultView.ToTable();
                        ddlcountrylist.DataSource = dtView;
                        ddlcountrylist.DisplayMember = "EligibilityClassification";
                        ddlcountrylist.ValueMember = "Value";
                        ddlcountrylist.Enabled = true;
                        ddlcountrylist.ClearSelected();
                        for (int i = 0; i < dtView.Rows.Count; i++)
                        {
                            string p = dtView.Rows[i][2].ToString();
                            if (p == "Algeria" ||
                                p == "Egypt" ||
                                p == "Libyan Arab Jamahiriya" ||
                                p == "Mauritania" ||
                                p == "Morocco" ||
                                p == "Sudan" ||
                                p == "Tunisia" ||
                                p == "Cameroon" ||
                                p == "Central African Republic" ||
                                p == "Chad" ||
                                p == "Congo" ||
                                p == "Equatorial Guinea" ||
                                p == "Gabon" ||
                                p == "Sao Tome and Principe" ||
                                p == "Burundi" ||
                                p == "Comoros" ||
                                p == "Democratic Republic Congo" ||
                                p == "Djibouti" ||
                                p == "Eritrea" ||
                                p == "Ethiopia" ||
                                p == "Kenya" ||
                                p == "Madagascar" ||
                                p == "Rwanda" ||
                                p == "Somalia" ||
                                p == "Uganda" ||
                                p == "Tanzania" ||
                                p == "Angola" ||
                                p == "Botswana" ||
                                p == "Lesotho" ||
                                p == "Malawi" ||
                                p == "Mauritius" ||
                                p == "Mozambique" ||
                                p == "Namibia" ||
                                p == "South Africa" ||
                                p == "Zambia" ||
                                p == "Zimbabwe" ||
                                p == "Benin" ||
                                p == "Burkina Faso" ||
                                p == "Cape Verde" ||
                                p == "Cote d'Ivoire" ||
                                p == "Gambia" ||
                                p == "Ghana" ||
                                p == "Guinea" ||
                                p == "Guinea-Bissau" ||
                                p == "Liberia" ||
                                p == "Mali" ||
                                p == "Niger" ||
                                p == "Nigeria" ||
                                p == "Senegal" ||
                                p == "Sierra Leone" ||
                                p == "Togo" ||
                                p == "Brunei Darussalam" ||
                                p == "China" ||
                                p == "Hong Kong" ||
                                p == "Indonesia" ||
                                p == "Malaysia" ||
                                p == "Myanmar" ||
                                p == "Papua New Guinea" ||
                                p == "Philippines" ||
                                p == "South Korea" ||
                                p == "Singapore" ||
                                p == "Thailand" ||
                                p == "Viet Nam" ||
                                p == "Bangladesh" ||
                                p == "India" ||
                                p == "Iran" ||
                                p == "Nepal" ||
                                p == "Pakistan" ||
                                p == "Sri Lanka" ||
                                p == "Bahrain" ||
                                p == "Iraq" ||
                                p == "Israel" ||
                                p == "Jordan" ||
                                p == "Kuwait" ||
                                p == "Lebanon" ||
                                p == "Oman" ||
                                p == "Qatar" ||
                                p == "Saudi Arabia" ||
                                p == "Syrian Arab Republic" ||
                                p == "Turkey" ||
                                p == "United Arab Emirates" ||
                                p == "Yemen" ||
                                p == "Barbados" ||
                                p == "Cuba" ||
                                p == "Dominican Republic" ||
                                p == "Guyana" ||
                                p == "Haiti" ||
                                p == "Jamaica" ||
                                p == "Trinidad and Tobago" ||
                                p == "Costa Rica" ||
                                p == "El Salvador" ||
                                p == "Guatemala" ||
                                p == "Honduras" ||
                                p == "Mexico" ||
                                p == "Nicaragua" ||
                                p == "Panama" ||
                                p == "Argentina" ||
                                p == "Bolivia" ||
                                p == "Brazil" ||
                                p == "Chile" ||
                                p == "Colombia" ||
                                p == "Ecuador" ||
                                p == "Paraguay" ||
                                p == "Peru" ||
                                p == "Uruguay" ||
                                p == "Venezuela"
                                )
                            {
                                ddlcountrylist.SetSelected(i, true);


                            }
                        }
                        string _country = string.Empty;
                        _country += "1. Taiwan Province of China";
                        SharedObjects.NotListedCountry = _country;
                        CountryNotInList rmkobj = new CountryNotInList();
                        rmkobj.ShowDialog();
                    }
                    else
                    {
                        ddlcountrylist.SelectedIndex = -1;
                        ddlcountrylist.Enabled = true;
                        pnlregionspecific.Visible = false;
                        _isregionSpecific = false;
                    }
                    break;
                    #endregion
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        private void dgv_Restrictions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                DataSet dsEClass = MySqlDal.OpportunityDataOperations.GetEligibiltyClassificationDetial(WFID);

                if (dsEClass.Tables["restrictions"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsEClass.Tables["restrictions"];

                            string langval = Convert.ToString(DT.Rows[rowindex]["LANG"]);

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                txtDisabilities.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["disabilities"]));
                                txtInvitationonly.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["invitationonly"]));
                                txtMemberonly.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["memberonly"]));
                                txtNominationonly.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["nominationonly"]));
                                txtMinorities.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["minorties"]));
                                txtWomen.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["women"]));
                                txtNumberofapplicantsallowed.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["numberofapplicantsallowed"]));

                                ddl_Rest_NS.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["not_specified"]));

                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_restrictions_id = Convert.ToString(DT.Rows[rowindex]["restrictions_id"]);
                            }
                            else
                            {
                                txtDisabilities.Text = Convert.ToString(DT.Rows[rowindex]["disabilities"]);
                                txtInvitationonly.Text = Convert.ToString(DT.Rows[rowindex]["invitationonly"]);
                                txtMemberonly.Text = Convert.ToString(DT.Rows[rowindex]["memberonly"]);
                                txtNominationonly.Text = Convert.ToString(DT.Rows[rowindex]["nominationonly"]);
                                txtMinorities.Text = Convert.ToString(DT.Rows[rowindex]["minorties"]);
                                txtWomen.Text = Convert.ToString(DT.Rows[rowindex]["women"]);
                                txtNumberofapplicantsallowed.Text = Convert.ToString(DT.Rows[rowindex]["numberofapplicantsallowed"]);

                                ddl_Rest_NS.SelectedValue = Convert.ToString(DT.Rows[rowindex]["not_specified"]);

                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_restrictions_id = Convert.ToString(DT.Rows[rowindex]["restrictions_id"]);
                            }

                            btnSave.Visible = false;
                            btnaddurl.Visible = false;
                            btnCancel.Visible = true;
                            btnupdateindividual.Visible = true;
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

        private void ddl_IET_NR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ddl_IET_NR.SelectedValue).ToLower() == "true")
                {
                    ddlCountry_IET.SelectedIndex = 0;
                    ddlCountry_IET.Enabled = false;
                }
                else
                {
                    ddlCountry_IET.SelectedIndex = 0;
                    ddlCountry_IET.Enabled = true;
                }
            }
            catch { }
        }

        private void dgv_IndividualElg_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dgv_IndividualElg.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            mode = 2;

                            int eligibilityclassification_id = Convert.ToInt32(dgv_IndividualElg.Rows[dgv_IndividualElg.SelectedCells[0].RowIndex].Cells["ind_elg_id"].Value.ToString());
                            int individualeligibility_ID = Convert.ToInt32(dgv_IndividualElg.Rows[dgv_IndividualElg.SelectedCells[0].RowIndex].Cells["individualeligibility_ID"].Value.ToString());
                            DataSet dsresult = MySqlDal.OpportunityDataOperations.SaveIndividualEligibilityType(WFID, "", "", "", "", "", "", "", "", mode, "", eligibilityclassification_id, individualeligibility_ID);

                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                            if (dsresult.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                            {
                                lblMsg.Text = "Data Delete Successfully !!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }
                            else
                            {
                                lblMsg.Text = "Something went wrong!!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                opportunity.GetProcess_update("Eligibilityclassification");
                            }
                            else
                            {
                                opportunity.GetProcess();
                            }
                            #endregion
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

        private void dgv_Restrictions_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dgv_Restrictions.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            mode = 2;

                            int eligibilityclassification_id = Convert.ToInt32(dgv_Restrictions.Rows[dgv_Restrictions.SelectedCells[0].RowIndex].Cells["id_Restriction"].Value.ToString());
                            int restrictions_id = Convert.ToInt32(dgv_Restrictions.Rows[dgv_Restrictions.SelectedCells[0].RowIndex].Cells["restrictions_id"].Value.ToString());

                            DataSet dsresult = OpportunityDataOperations.SaveRestrictions(WFID, "", "", "", "", "", "", "", "", "", mode, "", eligibilityclassification_id, restrictions_id);
                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                            if (dsresult.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                            {
                                lblMsg.Text = "Data Delete Successfully !!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }
                            else
                            {
                                lblMsg.Text = "Something went wrong!!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                opportunity.GetProcess_update("Eligibilityclassification");
                            }
                            else
                            {
                                opportunity.GetProcess();
                            }
                            #endregion
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

        private void dgv_orgElg_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dgv_orgElg.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            mode = 2;
                            int eligibilityclassification_id = Convert.ToInt32(dgv_orgElg.Rows[dgv_orgElg.SelectedCells[0].RowIndex].Cells["OrgelgCls_id"].Value.ToString());
                            int organizationeligibility_id = Convert.ToInt32(dgv_orgElg.Rows[dgv_orgElg.SelectedCells[0].RowIndex].Cells["organizationeligibility_id"].Value.ToString());
                            DataSet dsresult = MySqlDal.OpportunityDataOperations.SaveOrganizationEligibility(WFID, "", "", "", "", "", "", "", "", "", "", "", "", mode, "", eligibilityclassification_id, organizationeligibility_id);
                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                            if (dsresult.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                            {
                                lblMsg.Text = "Data Delete Successfully !!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }
                            else
                            {
                                lblMsg.Text = "Something went wrong!!!!";
                                loadSavedValues();
                                lblMsg.Visible = true;
                            }

                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                opportunity.GetProcess_update("Eligibilityclassification");
                            }
                            else
                            {
                                opportunity.GetProcess();
                            }
                            #endregion
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

        private void ddl_IET_NS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ddl_IET_NS.SelectedValue).ToLower() == "true")
                {
                    txtDegree.Text = "";

                    txtGraduate.Text = "";
                    txtNewfaculty.Text = "";
                    txtUndergraduate.Text = "";
                    ddlCountry_IET.SelectedIndex = 0;
                    ddl_IET_NR.SelectedIndex = 0;

                    txtGraduate.Enabled = false;
                    txtNewfaculty.Enabled = false;
                    txtUndergraduate.Enabled = false;
                    ddlCountry_IET.Enabled = false;
                    ddl_IET_NR.Enabled = false;
                }
                else
                {
                    txtGraduate.Enabled = true;
                    txtNewfaculty.Enabled = true;
                    txtUndergraduate.Enabled = true;
                    ddlCountry_IET.Enabled = true;
                    ddl_IET_NR.Enabled = true;
                    ddlCountry_IET.SelectedIndex = 0;
                    ddl_IET_NR.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void ddl_Rest_NS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ddl_Rest_NS.SelectedValue).ToLower() == "true")
                {
                    txtDisabilities.Text = "";
                    txtInvitationonly.Text = "";
                    txtMemberonly.Text = "";
                    txtNominationonly.Text = "";
                    txtMinorities.Text = "";
                    txtWomen.Text = "";
                    txtNumberofapplicantsallowed.Text = "";
                    txtDisabilities.Enabled = false;
                    txtInvitationonly.Enabled = false;
                    txtMemberonly.Enabled = false;
                    txtNominationonly.Enabled = false;
                    txtMinorities.Enabled = false;
                    txtWomen.Enabled = false;
                    txtNumberofapplicantsallowed.Enabled = false;
                }
                else
                {
                    txtDisabilities.Enabled = true;
                    txtInvitationonly.Enabled = true;
                    txtMemberonly.Enabled = true;
                    txtNominationonly.Enabled = true;
                    txtMinorities.Enabled = true;
                    txtWomen.Enabled = true;
                    txtNumberofapplicantsallowed.Enabled = true;
                }
            }
            catch { }
        }

        private void ddl_ORG_NS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ddl_ORG_NS.SelectedValue).ToLower() == "true")
                {
                    txtAcademic.Text = "";
                    txtCommercial.Text = "";
                    txtGovernment.Text = "";
                    txtNonprofit.Text = "";
                    txtSme.Text = "";
                    txtWomen.Text = "";
                    TextCity.Text = "";
                    DDLCOUNTRY.SelectedIndex = 0;
                    ddlState.SelectedIndex = 0;
                    ddl_ORG_NR.SelectedIndex = 0;

                    txtAcademic.Enabled = false;
                    txtCommercial.Enabled = false;
                    txtGovernment.Enabled = false;
                    txtNonprofit.Enabled = false;
                    txtSme.Enabled = false;
                    txtWomen.Enabled = false;
                    TextCity.Enabled = false;
                    DDLCOUNTRY.Enabled = false;
                    ddlState.Enabled = false;
                    ddl_ORG_NR.Enabled = false;
                }
                else
                {
                    txtAcademic.Enabled = true;
                    txtCommercial.Enabled = true;
                    txtGovernment.Enabled = true;
                    txtNonprofit.Enabled = true;
                    txtSme.Enabled = true;
                    txtWomen.Enabled = true;
                    TextCity.Enabled = true;
                    DDLCOUNTRY.Enabled = true;
                    ddlState.Enabled = true;
                    ddl_ORG_NR.Enabled = true;
                }
            }
            catch { }
        }

        private void ddl_ORG_NR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ddl_ORG_NR.SelectedValue).ToLower() == "true")
                {
                    DDLCOUNTRY.SelectedIndex = 0;
                    ddlState.SelectedIndex = 0;
                    TextCity.Text = "";
                    DDLCOUNTRY.Enabled = false;
                    TextCity.Enabled = false;
                    ddlState.Enabled = false;
                }
                else
                {
                    DDLCOUNTRY.SelectedIndex = 0;
                    DDLCOUNTRY.Enabled = true;

                    ddlState.SelectedIndex = 0;
                    ddlState.Enabled = true;
                    TextCity.Text = "";
                    TextCity.Enabled = true;
                }
            }
            catch { }
        }

        private void dgv_IndividualElg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                DataSet dsEClass = MySqlDal.OpportunityDataOperations.GetEligibiltyClassificationDetial(WFID);

                if (dsEClass.Tables["IndividualEligibility"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsEClass.Tables["IndividualEligibility"];

                            string langval = Convert.ToString(DT.Rows[rowindex]["LANG"]);

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                txtDegree.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["Degree"]));
                                txtGraduate.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["Graduate"]));
                                txtNewfaculty.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["Newfaculty"]));
                                txtUndergraduate.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["Undergraduate"]));

                                ddl_IET_NS.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["not_specified"]));
                                ddl_IET_NR.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["norestriction"]));
                                ddlcountrylistdtl.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["COUNTRY"]));

                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_individualeligibility_ID = Convert.ToString(DT.Rows[rowindex]["individualeligibility_ID"]);
                            }
                            else
                            {
                                txtDegree.Text = Convert.ToString(DT.Rows[rowindex]["Degree"]);
                                txtGraduate.Text = Convert.ToString(DT.Rows[rowindex]["Graduate"]);
                                txtNewfaculty.Text = Convert.ToString(DT.Rows[rowindex]["Newfaculty"]);
                                txtUndergraduate.Text = Convert.ToString(DT.Rows[rowindex]["Undergraduate"]);

                                ddl_IET_NS.SelectedValue = Convert.ToString(DT.Rows[rowindex]["not_specified"]);
                                ddl_IET_NR.SelectedValue = Convert.ToString(DT.Rows[rowindex]["norestriction"]);
                                ddlcountrylistdtl.SelectedValue = Convert.ToString(DT.Rows[rowindex]["COUNTRY"]);

                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_individualeligibility_ID = Convert.ToString(DT.Rows[rowindex]["individualeligibility_ID"]);
                            }
                            btnSave.Visible = false;
                            btnaddurl.Visible = false;
                            btnCancel.Visible = true;
                            btnupdateindividual.Visible = true;
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

        private void btnupdateindividual_Click(object sender, EventArgs e)
        {
            try
            {
                string lang_val = "";

                if (ddlLangContact.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    lang_val = ddlLangContact.SelectedValue.ToString();
                }

                lblMsg.Visible = false;
                DataSet dsIETClass = new DataSet();

                if (ddlElClassType.SelectedIndex == 0 || ddlElClassType.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Convert.ToString(ddlElClassType.SelectedValue) == "individualEligibility")
                {
                    string url_txtDegree = txtDegree.Text.TrimStart().TrimEnd();
                    string url_txtGraduate = txtGraduate.Text.TrimStart().TrimEnd();
                    string url_txtNewfaculty = txtNewfaculty.Text.TrimStart().TrimEnd();
                    string url_txtUndergraduate = txtUndergraduate.Text.TrimStart().TrimEnd();

                    if (url_txtDegree.Contains("http://") || url_txtGraduate.Contains("http://") || url_txtNewfaculty.Contains("http://") || url_txtNewfaculty.Contains("http://") || url_txtUndergraduate.Contains("http://") ||
                        url_txtDegree.Contains("https://") || url_txtGraduate.Contains("https://") || url_txtNewfaculty.Contains("https://") || url_txtNewfaculty.Contains("https://") || url_txtUndergraduate.Contains("https://") ||
                        url_txtDegree.Contains("www.") || url_txtGraduate.Contains("www.") || url_txtNewfaculty.Contains("www.") || url_txtNewfaculty.Contains("www.") || url_txtUndergraduate.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string country = null;

                        mode = 1;

                        if (ddlCountry_IET.SelectedIndex == 0)
                        {
                            country = null;
                        }
                        else
                        {
                            country = Convert.ToString(ddlCountry_IET.SelectedValue);

                            foreach (DataGridViewRow row in dgv_IndividualElg.Rows)
                            {
                                string grdCountry = row.Cells["Country"].Value.ToString().Trim();

                                if ((grdCountry == Convert.ToString(ddlCountry_IET.SelectedValue)))
                                {
                                    MessageBox.Show("This Country already Exist", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }

                        string C_ID = string.Empty;

                        if (ddlcountrylistdtl.SelectedItems.Count > 0)
                        {
                            C_ID = ddlcountrylistdtl.SelectedValue.ToString();
                        }
                        else
                        {
                            C_ID = "0";
                        }

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = OpportunityDataOperations.SaveIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(txtDegree.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd())),
                               replace.ConvertTextToUnicode(Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd())), Convert.ToString(ddl_IET_NR.SelectedValue), C_ID, "Citizenship", mode, lang_val);
                        }
                        else
                        {
                            dsIETClass = OpportunityDataOperations.UpdateIndividualEligibilityType(WFID, Convert.ToString(ddl_IET_NS.SelectedValue), Convert.ToString(txtDegree.Text.TrimStart().TrimEnd()), Convert.ToString(txtGraduate.Text.TrimStart().TrimEnd()),
                                       Convert.ToString(txtNewfaculty.Text.TrimStart().TrimEnd()), Convert.ToString(txtUndergraduate.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_IET_NR.SelectedValue), C_ID, "Citizenship", mode, c_eligibilityclassification_id, c_individualeligibility_ID, lang_val);
                        }

                        if (dsIETClass.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                        {
                            lblMsg.Text = "Data updated Successfully !!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;
                            if (dsIETClass.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Something went wrong!!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;
                        }

                        btnaddurl.Visible = true;
                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;
                        txtDegree.Text = "";
                        txtGraduate.Text = "";
                        txtNewfaculty.Text = "";
                        txtUndergraduate.Text = "";

                        btnSave.Visible = true;
                        btnupdateindividual.Visible = false;
                    }
                }
                else if (Convert.ToString(ddlElClassType.SelectedValue) == "organizationEligibility")
                {
                    string url_txtAcademic = txtAcademic.Text.TrimStart().TrimEnd();
                    string url_txtCommercial = txtCommercial.Text.TrimStart().TrimEnd();
                    string url_txtGovernment = txtGovernment.Text.TrimStart().TrimEnd();
                    string url_txtNonprofit = txtNonprofit.Text.TrimStart().TrimEnd();
                    string url_txtSme = txtSme.Text.TrimStart().TrimEnd();
                    string url_TextCity = TextCity.Text.TrimStart().TrimEnd();

                    if (url_txtAcademic.Contains("http://") || url_txtCommercial.Contains("http://") || url_txtGovernment.Contains("http://") || url_txtNonprofit.Contains("http://") || url_txtSme.Contains("http://") || url_TextCity.Contains("http://") ||
                        url_txtAcademic.Contains("https://") || url_txtCommercial.Contains("https://") || url_txtGovernment.Contains("https://") || url_txtNonprofit.Contains("https://") || url_txtSme.Contains("https://") || url_TextCity.Contains("https://") ||
                        url_txtAcademic.Contains("www.") || url_txtCommercial.Contains("www.") || url_txtGovernment.Contains("www.") || url_txtNonprofit.Contains("www.") || url_txtSme.Contains("www.") || url_TextCity.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string country = "", state = "";

                        if (DDLCOUNTRY.SelectedIndex == 0)
                        {
                            country = "";
                            state = "";
                        }
                        else
                        {
                            if (ddlState.SelectedIndex > 0)
                                state = Convert.ToString(ddlState.SelectedValue);

                            foreach (DataGridViewRow row in dgv_orgElg.Rows)
                            {
                                string grdOrgCountry = row.Cells["CountryOrg"].Value.ToString().Trim();

                                if ((grdOrgCountry == Convert.ToString(DDLCOUNTRY.SelectedValue)))
                                {
                                    MessageBox.Show("This Country already Exist", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }

                        mode = 1;

                        string C_ID = string.Empty;

                        if (listcountrydtl.SelectedItems.Count > 0)
                        {
                            C_ID = listcountrydtl.SelectedValue.ToString();
                        }
                        else
                        {
                            C_ID = "0";
                        }

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = OpportunityDataOperations.SaveOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd())),
                               replace.ConvertTextToUnicode(Convert.ToString(txtGovernment.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtSme.Text.TrimStart().TrimEnd())), Convert.ToString(ddl_ORG_NR.SelectedValue), replace.ConvertTextToUnicode(Convert.ToString(TextCity.Text.TrimStart().TrimEnd())), state, country, "regionspecific", "Citizenship", mode, lang_val);
                        }
                        else
                        {
                            dsIETClass = OpportunityDataOperations.UpdateOrganizationEligibility(WFID, Convert.ToString(ddl_ORG_NS.SelectedValue), Convert.ToString(txtAcademic.Text.TrimStart().TrimEnd()), Convert.ToString(txtCommercial.Text.TrimStart().TrimEnd()),
                             Convert.ToString(txtGovernment.Text), Convert.ToString(txtNonprofit.Text.TrimStart().TrimEnd()), Convert.ToString(txtSme.Text.TrimStart().TrimEnd()), Convert.ToString(ddl_ORG_NR.SelectedValue), Convert.ToString(TextCity.Text.TrimStart().TrimEnd()), state, C_ID, "regionspecific", "Citizenship", mode, c_eligibilityclassification_id, c_organizationeligibility_id, lang_val);
                        }
                        if (dsIETClass.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                        {
                            lblMsg.Text = "Data updated Successfully !!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;

                            if (dsIETClass.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Something went wrong!!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;
                        }

                        btnaddurl.Visible = true;
                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;
                        txtAcademic.Text = "";
                        txtCommercial.Text = "";
                        txtGovernment.Text = "";
                        txtNonprofit.Text = "";
                        txtSme.Text = "";
                        TextCity.Text = "";

                        btnSave.Visible = true;
                        btnupdateindividual.Visible = false;
                    }
                }
                else if (Convert.ToString(ddlElClassType.SelectedValue) == "restrictions")
                {
                    string url_txtDisabilities = txtDisabilities.Text.TrimStart().TrimEnd();
                    string url_txtInvitationonly = txtInvitationonly.Text.TrimStart().TrimEnd();
                    string url_txtMemberonly = txtMemberonly.Text.TrimStart().TrimEnd();
                    string url_txtNominationonly = txtNominationonly.Text.TrimStart().TrimEnd();
                    string url_txtMinorities = txtMinorities.Text.TrimStart().TrimEnd();
                    string url_txtWomen = txtWomen.Text.TrimStart().TrimEnd();
                    string url_txtNumberofapplicantsallowed = txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd();

                    if (url_txtDisabilities.Contains("http://") || url_txtInvitationonly.Contains("http://") || url_txtMemberonly.Contains("http://") || url_txtNominationonly.Contains("http://") || url_txtMinorities.Contains("http://") || url_txtWomen.Contains("http://") || url_txtNumberofapplicantsallowed.Contains("http://") ||
                        url_txtDisabilities.Contains("https://") || url_txtInvitationonly.Contains("https://") || url_txtMemberonly.Contains("https://") || url_txtNominationonly.Contains("https://") || url_txtMinorities.Contains("https://") || url_txtWomen.Contains("https://") || url_txtNumberofapplicantsallowed.Contains("https://") ||
                        url_txtDisabilities.Contains("www.") || url_txtInvitationonly.Contains("www.") || url_txtMemberonly.Contains("www.") || url_txtNominationonly.Contains("www.") || url_txtMinorities.Contains("www.") || url_txtWomen.Contains("www.") || url_txtNumberofapplicantsallowed.Contains("www."))
                    {
                        MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        mode = 1;

                        if (replace.chk_OtherLang(ddlLangContact.SelectedValue.ToString().Trim().ToLower()) == true)
                        {
                            dsIETClass = OpportunityDataOperations.SaveRestrictions(WFID, Convert.ToString(ddl_Rest_NS.SelectedValue),
                             replace.ConvertTextToUnicode(Convert.ToString(txtDisabilities.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtInvitationonly.Text.TrimStart().TrimEnd())),
                            replace.ConvertTextToUnicode(Convert.ToString(txtMemberonly.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtNominationonly.Text.TrimStart().TrimEnd())), replace.ConvertTextToUnicode(Convert.ToString(txtMinorities.Text.TrimStart().TrimEnd())),
                            replace.ConvertTextToUnicode(Convert.ToString(txtWomen.Text.TrimStart().TrimEnd())), Convert.ToString(txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd()),
                             "limitedsubmission", mode, lang_val);
                        }
                        else
                        {
                            dsIETClass = OpportunityDataOperations.SaveRestrictions(WFID, Convert.ToString(ddl_Rest_NS.SelectedValue),
                        Convert.ToString(txtDisabilities.Text.TrimStart().TrimEnd()), Convert.ToString(txtInvitationonly.Text.TrimStart().TrimEnd()),
                       Convert.ToString(txtMemberonly.Text.TrimStart().TrimEnd()), Convert.ToString(txtNominationonly.Text.TrimStart().TrimEnd()), Convert.ToString(txtMinorities.Text.TrimStart().TrimEnd()),
                       Convert.ToString(txtWomen.Text.TrimStart().TrimEnd()), Convert.ToString(txtNumberofapplicantsallowed.Text.TrimStart().TrimEnd()),
                        "limitedsubmission", mode, lang_val, Convert.ToInt64(c_eligibilityclassification_id), Convert.ToInt64(c_restrictions_id));
                        }
                        if (dsIETClass.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                        {
                            lblMsg.Text = "Data updated Successfully !!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;

                            if (dsIETClass.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Something went wrong!!!!";
                            loadSavedValues();
                            lblMsg.Visible = true;
                        }
                    }

                    btnaddurl.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;
                    txtDisabilities.Text = "";
                    txtInvitationonly.Text = "";
                    txtMemberonly.Text = "";
                    txtNominationonly.Text = "";
                    txtMinorities.Text = "";
                    txtWomen.Text = "";
                    txtNumberofapplicantsallowed.Text = "";
                    btnSave.Visible = true;
                    btnupdateindividual.Visible = false;
                }
            }
            catch { }
        }

        private void dgv_orgElg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                DataSet dsEClass = OpportunityDataOperations.GetEligibiltyClassificationDetial(WFID);

                if (dsEClass.Tables["organizationEligibility"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsEClass.Tables["organizationEligibility"];

                            string langval = Convert.ToString(DT.Rows[rowindex]["LANG"]);

                            if (replace.chk_OtherLang(langval.ToString().Trim().ToLower()) == true)
                            {
                                txtAcademic.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["academic"]));
                                txtCommercial.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["commercial"]));
                                txtGovernment.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["government"]));
                                txtSme.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["sme"]));
                                TextCity.Text = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["city"]));
                                ddl_ORG_NS.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["not_specified"]));
                                ddl_ORG_NR.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["norestriction"]));
                                ddlcountrylistdtl.SelectedValue = replace.ConvertUnicodeToText(Convert.ToString(DT.Rows[rowindex]["country"]));
                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_organizationeligibility_id = Convert.ToString(DT.Rows[rowindex]["organizationeligibility_id"]);
                                txtNonprofit.Text = Convert.ToString(DT.Rows[rowindex]["Nonprofit"]);
                            }
                            else
                            {
                                txtAcademic.Text = Convert.ToString(DT.Rows[rowindex]["academic"]);
                                txtCommercial.Text = Convert.ToString(DT.Rows[rowindex]["commercial"]);
                                txtGovernment.Text = Convert.ToString(DT.Rows[rowindex]["government"]);
                                txtSme.Text = Convert.ToString(DT.Rows[rowindex]["sme"]);
                                TextCity.Text = Convert.ToString(DT.Rows[rowindex]["city"]);
                                ddl_ORG_NS.SelectedValue = Convert.ToString(DT.Rows[rowindex]["not_specified"]);
                                ddl_ORG_NR.SelectedValue = Convert.ToString(DT.Rows[rowindex]["norestriction"]);
                                ddlcountrylistdtl.SelectedValue = Convert.ToString(DT.Rows[rowindex]["country"]);
                                c_eligibilityclassification_id = Convert.ToString(DT.Rows[rowindex]["eligibilityclassification_id"]);
                                c_organizationeligibility_id = Convert.ToString(DT.Rows[rowindex]["organizationeligibility_id"]);
                                txtNonprofit.Text = Convert.ToString(DT.Rows[rowindex]["Nonprofit"]);
                            }

                            btnSave.Visible = false;
                            btnaddurl.Visible = false;
                            btnCancel.Visible = true;
                            btnupdateindividual.Visible = true;
                        }
                        catch { }
                    }
                    else
                        MessageBox.Show("There is no record(s) for edit.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void pnl_individualEligibilityType_Paint(object sender, PaintEventArgs e) { }

        private void chk_individual_list_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && chk_individual_list.CheckedItems.Count > 0)
            {
                chk_individual_list.ItemCheck -= chk_individual_list_ItemCheck;
                chk_individual_list.SetItemChecked(chk_individual_list.CheckedIndices[0], false);
                chk_individual_list.ItemCheck += chk_individual_list_ItemCheck;
            }

            string _countryIndex = chk_individual_list.SelectedIndex.ToString();
            string _isCountryselected = "21";

            for (int i = 0; i < chk_individual_list.Items.Count; i++)
            {
                if (chk_individual_list.GetItemChecked(i))
                {
                    _isCountryselected = (string)chk_individual_list.SelectedIndex.ToString();
                }
            }
        }

        private void chk_individual_list_SelectedIndexChanged(object sender, EventArgs e) { }

        private void ChbxCountryList_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dgv_orgElg_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void EligibiltyClassification_Load(object sender, EventArgs e) { }

        private void ddlcountrylistdtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

                // Fill State Dropdown   
                if (ddlcountrylistdtl.SelectedIndex != 0)
                {
                    dsFunding.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + ddlcountrylistdtl.SelectedValue + "'";
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

        private void btnDeleteGrouping_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(ddlElClassType.SelectedValue) == "individualEligibility")
            {
                try
                {
                    string deletegroupid = string.Empty;

                    if (chk_individual_list.SelectedItems.Count > 0 && chk_individual_list.CheckedItems.Count > 0)
                    {
                        deletegroupid = chk_individual_list.SelectedValue.ToString();

                        lblMsg.Visible = false;

                        if (dgv_IndividualElg.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Do you really  want to delete this group Details ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                mode = 2;
                                int eligibilityclassification_id = 0;
                                int individualeligibility_ID = 0;
                                DataSet dsresult = OpportunityDataOperations.DeleteIndividualEligibilityType(deletegroupid, WFID, "", "", "", "", "", "", "", "", mode, "", eligibilityclassification_id, individualeligibility_ID);
                                lblMsg.Visible = true;
                                lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                                if (dsresult.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                                {
                                    lblMsg.Text = "Data Delete Successfully !!!!";
                                    loadSavedValues();
                                    lblMsg.Visible = true;
                                }
                                else
                                {
                                    lblMsg.Text = "Something went wrong!!!!";
                                    loadSavedValues();
                                    lblMsg.Visible = true;
                                }

                                #region For Changing Colour in case of Update
                                if (SharedObjects.TRAN_TYPE_ID == 1)
                                {
                                    opportunity.GetProcess_update("Eligibilityclassification");
                                }
                                else
                                {
                                    opportunity.GetProcess();
                                }
                                #endregion
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
            else if (Convert.ToString(ddlElClassType.SelectedValue) == "organizationEligibility")
            {
                try
                {
                    string deletegroupid = string.Empty;
                    if (chkbox_orgnEligibilityy.SelectedItems.Count > 0 && chkbox_orgnEligibilityy.CheckedItems.Count > 0)
                    {
                        deletegroupid = chkbox_orgnEligibilityy.SelectedValue.ToString();

                        lblMsg.Visible = false;

                        if (dgv_orgElg.Rows.Count > 0)
                        {
                            if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                mode = 2;
                                int eligibilityclassification_id = 0;
                                int organizationeligibility_id = 0;

                                DataSet dsresult = OpportunityDataOperations.DeleteOrganizationEligibility(deletegroupid, WFID, "", "", "", "", "", "", "", "", "", "", "", "", mode, "", eligibilityclassification_id, organizationeligibility_id);
                                lblMsg.Visible = true;
                                lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                                if (dsresult.Tables["ERRORCODE"].Rows[0][1].ToString() == "success")
                                {
                                    lblMsg.Text = "Data Delete Successfully !!!!";
                                    loadSavedValues();
                                    lblMsg.Visible = true;
                                }
                                else
                                {
                                    lblMsg.Text = "Something went wrong!!!!";
                                    loadSavedValues();
                                    lblMsg.Visible = true;
                                }

                                #region For Changing Colour in case of Update
                                if (SharedObjects.TRAN_TYPE_ID == 1)
                                {
                                    opportunity.GetProcess_update("Eligibilityclassification");
                                }
                                else
                                {
                                    opportunity.GetProcess();
                                }
                                #endregion
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
        }

        private void pnl_organizationEligibilityType_Paint(object sender, PaintEventArgs e) { }

        private void listcountrydtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

                // Fill State Dropdown   
                if (listcountrydtl.SelectedIndex != 0)
                {
                    dsFunding.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + listcountrydtl.SelectedValue + "'";
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

        private void chkbox_orgnEligibilityy_SelectedIndexChanged(object sender, EventArgs e) { }

        private void chkbox_orgnEligibilityy_SelectedValueChanged(object sender, EventArgs e) { }

        private void chkbox_orgnEligibilityy_TabIndexChanged(object sender, EventArgs e) { }

        private void chkbox_orgnEligibilityy_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && chkbox_orgnEligibilityy.CheckedItems.Count > 0)
            {
                chkbox_orgnEligibilityy.ItemCheck -= chkbox_orgnEligibilityy_ItemCheck;
                chkbox_orgnEligibilityy.SetItemChecked(chkbox_orgnEligibilityy.CheckedIndices[0], false);
                chkbox_orgnEligibilityy.ItemCheck += chkbox_orgnEligibilityy_ItemCheck;
            }

            string _countryIndex = chkbox_orgnEligibilityy.SelectedIndex.ToString();
            string _isCountryselected = "21";

            for (int i = 0; i < chkbox_orgnEligibilityy.Items.Count; i++)
            {
                if (chkbox_orgnEligibilityy.GetItemChecked(i))
                {
                    _isCountryselected = (string)chkbox_orgnEligibilityy.SelectedIndex.ToString();
                }
            }
        }
    }
}
