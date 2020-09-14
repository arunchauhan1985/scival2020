using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;
using MySqlDal.DataOpertation;
using MySqlDalAL;
using Newtonsoft.Json;

namespace Scival.FundingBody
{
    public partial class FundingBase : UserControl
    {        
        Replace r = new Replace();
        private FundingBody m_parent;
        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");
        public string isFund = "";
        bool flagContext = false;
        int UpdateCanonTID = 0;
        int UpdatePreOrgTID = 0;
        int UpdateAbbTID = 0;
        int UpdateContTID = 0;
        string InputXmlPath = string.Empty;
        ErrorLog oErrorLog = new ErrorLog();

        public FundingBase(FundingBody frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadBaseValue();

            SharedObjects.DefaultLoad = "";
            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        void ddlDefunct_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlTierInfo_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlCOpp_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlextendedRecord_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangAbbreviation_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangAcronym_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangCanonicalName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangContextName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangPreferredOrgName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlsubType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlCountry_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlHidden_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlState_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlTrusting_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlCAwards_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        public string GetFundingBodyJson(string FundingBodyId)
        {
            return FundingBodyDataOperations.GetFundingBodyMainJson(Convert.ToInt64(FundingBodyId));
        }
        public void LoadBaseValue()
        {
            InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
            lblMsg.Visible = false;

            if (SharedObjects.User.USERID == 0)
            {

            }
            else
            {
                try
                {
                    DataSet dsFunding = SharedObjects.StartWork;

                    
                    if (dsFunding != null)
                    {
                        var FundingBodyId = dsFunding.Tables[1].Rows[0]["FUNDINGBODY_ID"].ToString();
                        var FundingBodyMainJson = GetFundingBodyJson(FundingBodyId);
                        DefaultFBModel model = JsonConvert.DeserializeObject<DefaultFBModel>(FundingBodyMainJson);
                        if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                        {
                            SharedObjects.TRAN_TYPE_ID = 0;  // New FB
                        }
                        else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle > 0)
                        {
                            SharedObjects.TRAN_TYPE_ID = 1;   // Update FB
                        }
                        else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                        {
                            SharedObjects.TRAN_TYPE_ID = 0;    // QA FB
                        }


                        DataTable temp = dsFunding.Tables["Country"].Copy();

                        DataRow dr = temp.NewRow();
                        dr["LCODE"] = "SelectCountry";
                        dr["NAME"] = "--Select Country--";
                        temp.Rows.InsertAt(dr, 0);

                        ddlCountry.DataSource = temp;
                        ddlCountry.ValueMember = "LCODE";
                        ddlCountry.DisplayMember = "NAME";

                        ddlsubType.Items.Insert(0, "--Select SubType--");
                        ddlsubType.SelectedIndex = 0;

                        ddlTrusting.Items.Insert(0, "--Select Trusting--");
                        ddlTrusting.Items.Insert(1, "False");
                        ddlTrusting.Items.Insert(2, "True");
                        ddlTrusting.SelectedIndex = 0;

                        ddl_profit.Items.Insert(0, "--Select Profit--");
                        ddl_profit.Items.Insert(1, "False");
                        ddl_profit.Items.Insert(2, "True");
                        ddl_profit.SelectedIndex = model.profitabilityType.ToLower() == "true" ? 2 : 1;


                        ddl_opportunitiesFrequency.Items.Insert(0, "--Select OpportunitiesFrequency--");
                        ddl_opportunitiesFrequency.Items.Insert(1, "weekly");
                        ddl_opportunitiesFrequency.Items.Insert(2, "monthly");
                        ddl_opportunitiesFrequency.Items.Insert(3, "yearly");
                        ddl_opportunitiesFrequency.SelectedIndex = 0;

                        ddl_awardsFrequency.Items.Insert(0, "--Select AwardsFrequency--");
                        ddl_awardsFrequency.Items.Insert(1, "weekly");
                        ddl_awardsFrequency.Items.Insert(2, "monthly");
                        ddl_awardsFrequency.Items.Insert(3, "yearly");
                        ddl_awardsFrequency.SelectedIndex = 0;

                        Y_opportunitiesFrequency.Items.Insert(0, "--Select Month--");
                        Y_opportunitiesFrequency.Items.Insert(1, "January");
                        Y_opportunitiesFrequency.Items.Insert(2, "Februray");
                        Y_opportunitiesFrequency.Items.Insert(3, "March");

                        Y_opportunitiesFrequency.Items.Insert(4, "April");
                        Y_opportunitiesFrequency.Items.Insert(5, "May");
                        Y_opportunitiesFrequency.Items.Insert(6, "June");
                        Y_opportunitiesFrequency.Items.Insert(7, "July");

                        Y_opportunitiesFrequency.Items.Insert(8, "August");
                        Y_opportunitiesFrequency.Items.Insert(9, "September");
                        Y_opportunitiesFrequency.Items.Insert(10, "October");
                        Y_opportunitiesFrequency.Items.Insert(11, "November");
                        Y_opportunitiesFrequency.Items.Insert(12, "December");

                        Y_opportunitiesFrequency.SelectedIndex = 0;

                        Y_awardsFrequency.Items.Insert(0, "--Select Month--");
                        Y_awardsFrequency.Items.Insert(1, "January");
                        Y_awardsFrequency.Items.Insert(2, "Februray");
                        Y_awardsFrequency.Items.Insert(3, "March");

                        Y_awardsFrequency.Items.Insert(4, "April");
                        Y_awardsFrequency.Items.Insert(5, "May");
                        Y_awardsFrequency.Items.Insert(6, "June");
                        Y_awardsFrequency.Items.Insert(7, "July");

                        Y_awardsFrequency.Items.Insert(8, "August");
                        Y_awardsFrequency.Items.Insert(9, "September");
                        Y_awardsFrequency.Items.Insert(10, "October");
                        Y_awardsFrequency.Items.Insert(11, "November");
                        Y_awardsFrequency.Items.Insert(12, "December");

                        Y_awardsFrequency.SelectedIndex = 0;

                        temp = dsFunding.Tables["FundingBodyTypes"].Copy();
                        dr = temp.NewRow();
                        dr["VALUE"] = "SelectType";
                        dr["TYPE"] = "--Select Type--";
                        temp.Rows.InsertAt(dr, 0);

                        ddlType.DataSource = temp;
                        ddlType.ValueMember = "VALUE";
                        ddlType.DisplayMember = "TYPE";

                        DataTable tempCanon = dsFunding.Tables["LanguageTable"].Copy();
                        dr = tempCanon.NewRow();
                        dr["LANGUAGE_CODE"] = "SelectLanguage";
                        dr["LANGUAGE_NAME"] = "--Select Language--";
                        tempCanon.Rows.InsertAt(dr, 0);

                        ddlLangCanonicalName.DataSource = tempCanon;
                        ddlLangCanonicalName.ValueMember = "LANGUAGE_CODE";
                        ddlLangCanonicalName.DisplayMember = "LANGUAGE_NAME";
                        ddlLangCanonicalName.SelectedIndex = 18;

                        DataTable tempPref = dsFunding.Tables["LanguageTable"].Copy();
                        dr = tempPref.NewRow();
                        dr["LANGUAGE_CODE"] = "SelectLanguage";
                        dr["LANGUAGE_NAME"] = "--Select Language--";
                        tempPref.Rows.InsertAt(dr, 0);

                        ddlLangPreferredOrgName.DataSource = tempPref;
                        ddlLangPreferredOrgName.ValueMember = "LANGUAGE_CODE";
                        ddlLangPreferredOrgName.DisplayMember = "LANGUAGE_NAME";
                        ddlLangPreferredOrgName.SelectedIndex = 18;

                        DataTable tempCont = dsFunding.Tables["LanguageTable"].Copy();
                        dr = tempCont.NewRow();
                        dr["LANGUAGE_CODE"] = "SelectLanguage";
                        dr["LANGUAGE_NAME"] = "--Select Language--";
                        tempCont.Rows.InsertAt(dr, 0);

                        ddlLangContextName.DataSource = tempCont;
                        ddlLangContextName.ValueMember = "LANGUAGE_CODE";
                        ddlLangContextName.DisplayMember = "LANGUAGE_NAME";
                        ddlLangContextName.SelectedIndex = 18;

                        DataTable tempAbbr = dsFunding.Tables["LanguageTable"].Copy();
                        dr = tempAbbr.NewRow();
                        dr["LANGUAGE_CODE"] = "SelectLanguage";
                        dr["LANGUAGE_NAME"] = "--Select Language--";
                        tempAbbr.Rows.InsertAt(dr, 0);

                        ddlLangAbbreviation.DataSource = tempAbbr;
                        ddlLangAbbreviation.ValueMember = "LANGUAGE_CODE";
                        ddlLangAbbreviation.DisplayMember = "LANGUAGE_NAME";
                        ddlLangAbbreviation.SelectedIndex = 18;

                        txtCollCode.Text = "Aptara";

                        ddlHidden.Items.Insert(0, "--Select Hidden--");
                        ddlHidden.Items.Insert(1, "False");
                        ddlHidden.Items.Insert(2, "True");
                        ddlHidden.SelectedIndex = 0;

                        ddlDefunct.Items.Insert(0, "--Select Defunct--");
                        ddlDefunct.Items.Insert(1, "False");
                        ddlDefunct.Items.Insert(2, "True");
                        ddlDefunct.SelectedIndex = 0;


                        ddlextendedRecord.Items.Insert(0, "--Select Extended Record--");
                        ddlextendedRecord.Items.Insert(1, "False");
                        ddlextendedRecord.Items.Insert(2, "True");
                        ddlextendedRecord.SelectedIndex = 0;

                        ddlCOpp.Items.Insert(0, "--Select Capture Opportunities--");
                        ddlCOpp.Items.Insert(1, "False");
                        ddlCOpp.Items.Insert(2, "True");
                        ddlCOpp.SelectedIndex = 0;

                        ddlCAwards.Items.Insert(0, "--Select Capture Awards--");
                        ddlCAwards.Items.Insert(1, "False");
                        ddlCAwards.Items.Insert(2, "True");
                        ddlCAwards.SelectedIndex = 0;

                        ddlTierInfo.Items.Insert(0, "--Select Tier Info--");
                        ddlTierInfo.Items.Insert(1, "1");
                        ddlTierInfo.Items.Insert(2, "2");
                        ddlTierInfo.Items.Insert(3, "3");
                        ddlTierInfo.Items.Insert(4, "4");
                        ddlTierInfo.SelectedIndex = 0;


                        DataTable tempAcro = dsFunding.Tables["LanguageTable"].Copy();
                        dr = tempAcro.NewRow();
                        dr["LANGUAGE_CODE"] = "SelectLanguage";
                        dr["LANGUAGE_NAME"] = "--Select Language--";
                        tempAcro.Rows.InsertAt(dr, 0);

                        ddlLangAcronym.DataSource = tempAbbr;
                        ddlLangAcronym.ValueMember = "LANGUAGE_CODE";
                        ddlLangAcronym.DisplayMember = "LANGUAGE_NAME";
                        ddlLangAcronym.SelectedIndex = 18;

                        if (dsFunding.Tables["FundingBodyTable"].Rows.Count > 0)
                        {
                            SharedObjects.IsFundingBaseFilled = true;
                            ddlType.SelectedValue = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["TYPE"]);

                            DataTable dtSubType = dsFunding.Tables["FundingBodySubTypes"].DefaultView.ToTable();

                            dr = dtSubType.NewRow();
                            dr["VALUE"] = "SelectState";
                            dr["SUBTYPE"] = "--Select SubType--";
                            dtSubType.Rows.InsertAt(dr, 0);

                            ddlsubType.DataSource = dtSubType;
                            ddlsubType.ValueMember = "VALUE";
                            ddlsubType.DisplayMember = "SUBTYPE";

                            ddlsubType.SelectedValue = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["SUBTYPEID"]);

                            ddlTrusting.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["TRUSTING"]);

                            ddlCountry.SelectedValue = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["COUNTRY"]).Trim();

                            string othetState = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["STATE"]).Trim();

                            DataSet dsfund = SharedObjects.StartWork;
                            dsfund.Tables["State"].DefaultView.RowFilter = "COUNTRYCODE='" + ddlCountry.SelectedValue + "'";
                            DataTable dtOtherState = dsfund.Tables["State"].DefaultView.ToTable();

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
                                DataTable dtStateResult = dt.DefaultView.ToTable();

                                if (dtStateResult.Rows.Count > 0)
                                {
                                    ddlState.SelectedValue = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["STATE"]);
                                }
                                else
                                {
                                    ddlState.SelectedValue = "OtherState";
                                    txtOtherState.Enabled = true;
                                    txtOtherState.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["STATE"]);

                                    string UpdateFunding_STATE = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["STATE"].ToString());
                                    if (UpdateFunding_STATE != "")
                                    {
                                        txtOtherState.Text = UpdateFunding_STATE;
                                    }
                                }
                            }

                            txtCollCode.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["COLLECTIONCODE"]);
                            string UpdateFunding_COLLECTIONCODE = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["COLLECTIONCODE"].ToString());
                            if (UpdateFunding_COLLECTIONCODE != "")
                            {
                                txtCollCode.Text = UpdateFunding_COLLECTIONCODE;
                            }
                            ddlHidden.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["HIDDEN"]);
                            ddlDefunct.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["DEFUNCT"]);
                            txtCrossRefID.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["CROSSREFID"]);
                            string UpdateFunding_CROSSREFID = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["CROSSREFID"].ToString());
                            if (UpdateFunding_CROSSREFID != "")
                            {
                                txtCrossRefID.Text = UpdateFunding_CROSSREFID;
                            }
                            txtDesc.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYDESCRIPTION"]);
                            string UpdateFunding_ELIGIBILITYDESCRIPTION = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["ELIGIBILITYDESCRIPTION"].ToString());
                            if (UpdateFunding_ELIGIBILITYDESCRIPTION != "")
                            {
                                txtDesc.Text = UpdateFunding_ELIGIBILITYDESCRIPTION;
                            }
                            txtRecoSource.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"]);
                            string UpdateFunding_RECORDSOURCE = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"].ToString());
                            if (UpdateFunding_RECORDSOURCE != "")
                            {
                                txtRecoSource.Text = UpdateFunding_RECORDSOURCE;
                            }
                            txtAwardSuccesRate.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["AWARDSUCCESRATE"]);
                            string UpdateFunding_AWARDSUCCESRATE = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["AWARDSUCCESRATE"].ToString());
                            if (UpdateFunding_AWARDSUCCESRATE != "")
                            {
                                txtAwardSuccesRate.Text = UpdateFunding_AWARDSUCCESRATE;
                            }

                            if (Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["extendedRecord"]).Equals(""))
                            {
                                ddlextendedRecord.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlextendedRecord.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["extendedRecord"]);
                            }

                            if (Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["CaptureOpportunities"]).Equals(""))
                            {
                                ddlCOpp.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlCOpp.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["CaptureOpportunities"]);
                            }

                            if (Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["CaptureAwards"]).Equals(""))
                            {
                                ddlCAwards.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlCAwards.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["CaptureAwards"]);
                            }

                            if (Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["TierInfo"]).Equals(""))
                            {
                                ddlTierInfo.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlTierInfo.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["TierInfo"]);
                            }

                            textAwardsSup.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["AwardsSupplier"]);
                            string UpdateFunding_AwardsSupplier = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["AwardsSupplier"].ToString());
                            if (UpdateFunding_AwardsSupplier != "")
                            {
                                textAwardsSup.Text = UpdateFunding_AwardsSupplier;
                            }
                            textOppSup.Text = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["OpportunitiesSupplier"]);
                            string UpdateFunding_OpportunitiesSupplier = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["OpportunitiesSupplier"].ToString());
                            if (UpdateFunding_OpportunitiesSupplier != "")
                            {
                                textOppSup.Text = UpdateFunding_OpportunitiesSupplier;
                            }

                            if (Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["profit"]) != "")
                            {
                                ddl_profit.SelectedItem = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["profit"]);
                            }
                            string oppfreq = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["profit"]);
                            string[] Month_Seq_OPP = dsFunding.Tables["FundingBodyTable"].Rows[0]["opportunitiesfrequency"].ToString().Split(',');
                            if (Month_Seq_OPP.Length == 1)
                            {
                                ddl_opportunitiesFrequency.SelectedItem = Month_Seq_OPP[0].ToString();
                                Y_opportunitiesFrequency.Visible = false;
                            }
                            else if (Month_Seq_OPP.Length == 2)
                            {
                                ddl_opportunitiesFrequency.SelectedItem = Month_Seq_OPP[0].ToString().Trim();
                                Y_opportunitiesFrequency.SelectedItem = Month_Seq_OPP[1].ToString().Trim();
                                Y_opportunitiesFrequency.Visible = true;
                            }

                            string[] Month_Seq_AW = dsFunding.Tables["FundingBodyTable"].Rows[0]["awardsfrequency"].ToString().Split(',');
                            if (Month_Seq_AW.Length == 1)
                            {
                                ddl_awardsFrequency.SelectedItem = Month_Seq_AW[0].ToString();
                                Y_awardsFrequency.Visible = false;
                            }
                            else if (Month_Seq_AW.Length == 2)
                            {
                                ddl_awardsFrequency.SelectedItem = Month_Seq_AW[0].ToString().Trim();
                                Y_awardsFrequency.SelectedItem = Month_Seq_AW[1].ToString().Trim();
                                Y_awardsFrequency.Visible = true;
                            }

                            string COMMENT_DESC = string.Empty;
                            COMMENT_DESC = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["COMMENT_DESC"]);
                            if (COMMENT_DESC != "")
                            {
                                txtComment.Text = COMMENT_DESC;

                                string UpdateFunding_COMMENT_DESC = r.Return_WieredChar_Original(dsFunding.Tables["FundingBodyTable"].Rows[0]["COMMENT_DESC"].ToString());
                                if (UpdateFunding_COMMENT_DESC != "")
                                {
                                    txtComment.Text = UpdateFunding_COMMENT_DESC;
                                }
                            }

                            string fndID = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODY_ID"]);
                            DataSet dsLoadFundLang = OpportunityDataOperations.LoadLanguageData(fndID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                            DataView dv;
                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='1'";
                            if (dv.Count > 0)
                            {
                                UpdateCanonTID = Convert.ToInt32(dv[0]["tran_id"]);
                                // txtCanoName.Text = Convert.ToString(r.ReadandReplaceHexaToChar(dv[0]["column_desc"].ToString(), InputXmlPath));
                                txtCanoName.Text = Convert.ToString(dv[0]["column_desc"].ToString());
                                string UpdateFunding_txtCanoName = r.Return_WieredChar_Original(dv[0]["column_desc"].ToString());
                                if (UpdateFunding_txtCanoName != "")
                                {
                                    txtCanoName.Text = UpdateFunding_txtCanoName;
                                }

                                ddlLangCanonicalName.SelectedIndex = Convert.ToInt32(dv[0]["language_group_id"]);
                            }

                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='2'";
                            if (dv.Count > 0)
                            {
                                for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                                {
                                    string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                    string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath));

                                    string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                    if (UpdateFunding_difflang != "")
                                    {
                                        secondCol = UpdateFunding_difflang;
                                    }
                                    string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());
                                    if (r.chk_OtherLang(thirdCol) == true)
                                    {
                                        string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                        if (secondCol_difflang != "")
                                        {
                                            secondCol = secondCol_difflang;

                                            string UpdateFunding_PREFERREDorname = r.Return_WieredChar_Original(secondCol.ToString());
                                            if (UpdateFunding_PREFERREDorname != "")
                                            {
                                                secondCol = UpdateFunding_PREFERREDorname;
                                            }
                                        }
                                    }
                                    string[] row = { firstCol, secondCol, thirdCol };
                                    if (dv.Count == 1 && secondCol == "")
                                    { }
                                    else
                                    {
                                        dtGridPreOrg.Rows.Add(row);
                                    }
                                }
                            }

                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='4'";
                            for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                            {
                                string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["column_desc"].ToString(), InputXmlPath));

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
                                }

                                string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());
                                if (r.chk_OtherLang(thirdCol) == true)
                                {
                                    string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                    if (secondCol_difflang != "")
                                    {
                                        secondCol = secondCol_difflang;

                                        string UpdateFunding_ABBREV = r.Return_WieredChar_Original(secondCol.ToString());
                                        if (UpdateFunding_ABBREV != "")
                                        {
                                            secondCol = UpdateFunding_ABBREV;
                                        }
                                    }
                                }

                                string[] row = { firstCol, secondCol, thirdCol };
                                if (dv.Count == 1 && secondCol == "")
                                { }
                                else
                                {
                                    dtGridAbbreviation.Rows.Add(row);
                                }
                            }

                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='7'";
                            for (int iAcr = 0; iAcr < dv.Count; iAcr++)
                            {
                                string firstCol = Convert.ToString(dv[iAcr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAcr]["column_desc"].ToString(), InputXmlPath));
                                string thirdCol = Convert.ToString(dv[iAcr]["language_code"].ToString().ToLower());
                                if (r.chk_OtherLang(thirdCol) == true)
                                {
                                    string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                    if (secondCol_difflang != "")
                                    {
                                        secondCol = secondCol_difflang;
                                        string UpdateFunding_Acronym = r.Return_WieredChar_Original(secondCol.ToString());
                                        if (UpdateFunding_Acronym != "")
                                        {
                                            secondCol = UpdateFunding_Acronym;
                                        }
                                    }
                                }
                                string[] row = { firstCol, secondCol, thirdCol };
                                if (dv.Count == 1 && secondCol == "")
                                { }
                                else
                                {
                                    dtGridAcronym.Rows.Add(row);
                                }
                            }

                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                            dv.RowFilter = "column_id='3'";
                            for (int iContext = 0; iContext < dv.Count; iContext++)
                            {
                                string firstCol = Convert.ToString(dv[iContext]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iContext]["column_desc"].ToString(), InputXmlPath));

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iContext]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
                                }

                                string thirdCol = Convert.ToString(dv[iContext]["language_code"].ToString().ToLower());
                                if (r.chk_OtherLang(thirdCol) == true)
                                {
                                    string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                    if (secondCol_difflang != "")
                                    {
                                        secondCol = secondCol_difflang;
                                        string UpdateFunding_CONTEXT = r.Return_WieredChar_Original(secondCol.ToString());
                                        if (UpdateFunding_CONTEXT != "")
                                        {
                                            secondCol = UpdateFunding_CONTEXT;
                                        }
                                    }
                                }
                                string[] row = { firstCol, secondCol, thirdCol };
                                dtGridContextName.Rows.Add(row);
                            }
                        }
                        else
                        {
                            txtPreOrgName.Text = Convert.ToString(r.ReadandReplaceHexaToChar(SharedObjects.FundingBodyName.ToString(), InputXmlPath));

                            string UpdateFunding_txtPreOrgName = Convert.ToString(r.Return_WieredChar_Original(SharedObjects.FundingBodyName.ToString()));
                            if (UpdateFunding_txtPreOrgName != "")
                            {
                                txtPreOrgName.Text = UpdateFunding_txtPreOrgName;
                            }
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, DataTable> dtTables = new System.Collections.Generic.Dictionary<string, DataTable>();
                DataTable dt_preferredorgname = new DataTable();
                DataTable dt_acronym = new DataTable();
                DataTable dt_abbrevname = new DataTable();
                DataTable dt_contextname = new DataTable();
                DataTable dt_subType = new DataTable();
                DataTable dt_website = new DataTable();
                DataTable dt_contact = new DataTable();
                DataTable dt_address = new DataTable();
                DataTable dt_establishmentInfo = new DataTable();
                DataTable dt_fundingPolicy = new DataTable();
                DataTable dt_fundingPolicy_Deatails = new DataTable();
                DataTable dt_createddate = new DataTable();
                DataTable dt_reviseddate = new DataTable();
                DataTable dt_revisionhistory = new DataTable();
                DataTable dt_link = new DataTable();
                DataTable dt_OPPORTUNITIESSOURCE = new DataTable();
                DataTable dt_publicationDataset = new DataTable();
                DataTable dt_awardSSOURCE = new DataTable();
                DataTable dt_fundingBodyDataset = new DataTable();

                DataTable dt_identifier = new DataTable();
                DataTable dt_fundingdescription = new DataTable();
                DataTable dt_awardSuccessRatedesc = new DataTable();
                DataTable dt_releatedorgs = new DataTable();

                StringBuilder jsondata = new StringBuilder();
                string P_fundingBodyProjectId=null;
                string createdby = "1";
                string createdTime = DateTime.Now.ToString();
                string ModifiedTime = "";
                string modifiedBy = null;

                #region Save FundingBody data
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                Regex intRgx = new Regex(@"^[0-9]+");
                Regex strRgx = new Regex(@"[A-Za-z ]");
                string orgName = Regex.Replace(txtPreOrgName.Text,@"[A-Za-z ]", "");
                string preferedOrgName = "", opportunitiesFrequency = "", awardsFrequency = "", Profit = "";

                if (ddl_opportunitiesFrequency.SelectedIndex == 3)
                {
                    if (Y_opportunitiesFrequency.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Select opportunitiesFrequency Month .", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        opportunitiesFrequency = ddl_opportunitiesFrequency.SelectedItem.ToString() + ", " + Y_opportunitiesFrequency.SelectedItem.ToString();
                    }
                }

                else if (ddl_opportunitiesFrequency.SelectedIndex != 0)
                {
                    opportunitiesFrequency = ddl_opportunitiesFrequency.SelectedItem.ToString();
                }

                if (ddl_awardsFrequency.SelectedIndex == 3)
                {
                    if (Y_awardsFrequency.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Select awardsFrequency Month .", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        awardsFrequency = ddl_awardsFrequency.SelectedItem.ToString() + ", " + Y_awardsFrequency.SelectedItem.ToString();
                    }
                }

                else if (ddl_awardsFrequency.SelectedIndex != 0)
                {
                    awardsFrequency = ddl_awardsFrequency.SelectedItem.ToString();
                }

                if (ddl_profit.SelectedIndex > 0)
                {
                    Profit = ddl_profit.SelectedItem.ToString();
                }
                
                dt_preferredorgname.Columns.Add("lang");
                dt_preferredorgname.Columns.Add("preferredorgname_text");
                if (dtGridPreOrg.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dtGridPreOrg.Rows)
                    {
                        string langValue = row.Cells["PreOrg_language_code"].Value.ToString();
                        if (langValue.ToLower() == "en")
                        {
                            preferedOrgName = row.Cells["PreOrg_desc"].Value.ToString();
                            dt_preferredorgname.Rows.Add();
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][0] = langValue;
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][1] = preferedOrgName;
                        }
                    }
                    if (preferedOrgName == "")
                    {
                        string langValue = dtGridPreOrg.Rows[0].Cells["PreOrg_language_code"].Value.ToString();

                        if (r.chk_OtherLang(langValue) == true)
                        {
                            preferedOrgName = r.ConvertTextToUnicode(dtGridPreOrg.Rows[0].Cells["PreOrg_desc"].Value.ToString());
                            dt_preferredorgname.Rows.Add();
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][0] = langValue;
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][1] = preferedOrgName;
                        }
                        else
                        {
                            preferedOrgName = dtGridPreOrg.Rows[0].Cells["PreOrg_desc"].Value.ToString();
                            dt_preferredorgname.Rows.Add();
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][0] = langValue;
                            dt_preferredorgname.Rows[dt_preferredorgname.Rows.Count - 1][1] = preferedOrgName;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter At least one preferedOrgName.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtDesc.Text != "")
                {
                    string _result = oErrorLog.htlmtag(txtDesc.Text.Trim(), "Eligibility Description ");
                    if (!_result.Equals(""))
                    {
                        MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (pattern.Matches(orgName).Count > 0)
                {
                    MessageBox.Show("Please enter valid Prefered Org Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Type", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlsubType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select SubType", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlCountry.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Country", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlHidden.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Hidden", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlTierInfo.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Tier Info", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtRecoSource.Text == "")
                {
                    MessageBox.Show("Please Fill Record Source", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAwardSuccesRate.Text.Trim() != "" && (!intRgx.IsMatch(txtAwardSuccesRate.Text)))
                {
                    MessageBox.Show("Please enter numeric in Award Succes Rate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                try
                {

                    Int64 Loginid = 0; string fundindID = "0"; string ORGBId = "0";

                        Loginid = Convert.ToInt64(SharedObjects.User.USERID);

                        DataSet dsFunding = SharedObjects.StartWork;
                        if (dsFunding.Tables["FundingBodyTable"].Rows.Count > 0)
                        {
                            fundindID = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODY_ID"]);
                            ORGBId = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["ORGDBID"]);
                        }

                        #region Columns Declaration
                        DataTable dtResult = new DataTable();
                        dtResult.Columns.Add("FUNDINGBODY_ID");//0
                        dtResult.Columns.Add("ORGDBID");//1
                        dtResult.Columns.Add("type");//2
                        dtResult.Columns.Add("TRUSTING");//3
                        dtResult.Columns.Add("state");//4
                        dtResult.Columns.Add("COLLECTIONCODE");//5
                        dtResult.Columns.Add("HIDDEN");//6
                        dtResult.Columns.Add("CANONICALNAME");//7
                        dtResult.Columns.Add("PREFERREDORGNAME");//8
                        dtResult.Columns.Add("CONTEXTNAME");//9
                        dtResult.Columns.Add("ABBREVNAME");//10
                        dtResult.Columns.Add("ELIGIBILITYDESCRIPTION");//11
                        dtResult.Columns.Add("SUBTYPEID");//12
                        dtResult.Columns.Add("SUBTYPE_TEXT");//13
                        dtResult.Columns.Add("WFID");//14
                        dtResult.Columns.Add("LOGINId");//15
                        dtResult.Columns.Add("PAGENAME");//16
                        dtResult.Columns.Add("recordSource");//17
                        dtResult.Columns.Add("AWARDSUCCESSRATE");//18
                        dtResult.Columns.Add("COMMENT");//19
                        dtResult.Columns.Add("DEFUNCT");//20
                        dtResult.Columns.Add("CROSSREFID");//21
                        dtResult.Columns.Add("EXTENDEDRECORD");//22
                        dtResult.Columns.Add("CapOpp");//23
                        dtResult.Columns.Add("OPPSup");//24
                        dtResult.Columns.Add("CapAwards");//25
                        dtResult.Columns.Add("AwardsSup");//26
                        dtResult.Columns.Add("TIERINFO");//27
                        dtResult.Columns.Add("profit");//28
                        dtResult.Columns.Add("opportunitiesFrequency");//29
                        dtResult.Columns.Add("awardsFrequency");//30
                        dtResult.Columns.Add("status");//31
                        dtResult.Columns.Add("country");//32
                        #endregion

                        DataRow dr = dtResult.NewRow();
                        dr[0] = fundindID;
                        P_fundingBodyProjectId = fundindID;
                        dr[1] = ORGBId;
                        dr[2] = Convert.ToString(ddlType.SelectedValue);
                        dr[3] = Convert.ToString(ddlTrusting.Text);
                        dr[4] = Convert.ToString(ddlCountry.SelectedValue);
                        dr[32] = Convert.ToString(ddlCountry.SelectedValue);

                        if (ddlState.SelectedValue.ToString() != "SelectState")
                        {
                            if (ddlState.SelectedValue.ToString() == "OtherState")
                                dr[4] = Convert.ToString(txtOtherState.Text.Trim());

                            string UpdateFunding_STATE = r.Return_WieredChar_Original(txtOtherState.Text.Trim().ToString());
                            if (UpdateFunding_STATE != "")
                            {
                                dr[4] = UpdateFunding_STATE;
                            }
                            else
                                dr[4] = Convert.ToString(ddlState.SelectedValue);
                        }
                        else
                            dr[4] = "";

                        if (txtCollCode.Text.Trim() != "")
                            dr[5] = Convert.ToString(txtCollCode.Text);
                        else
                            dr[5] = "Aptara";

                        dr[6] = Convert.ToString(ddlHidden.SelectedItem.ToString());
                        dr[7] = Convert.ToString(r.ReadandReplaceCharToHexa(txtCanoName.Text.Trim(), InputXmlPath));
                        dr[8] = Convert.ToString(r.ReadandReplaceCharToHexa(preferedOrgName, InputXmlPath));

                       
                        dt_acronym = GridTodt(dtGridAcronym, "lang", "acronym_text");
                        dt_abbrevname= GridTodt(dtGridAbbreviation, "lang", "abbrevname_text");
                        dt_contextname = GridTodt(dtGridContextName, "lang", "contextname_text");

                        dr[9] = Convert.ToString(r.ReadandReplaceCharToHexa(txtContextname.Text, InputXmlPath));
                        dr[10] = Convert.ToString(r.ReadandReplaceCharToHexa(txtAbb.Text, InputXmlPath));
                        dr[11] = Convert.ToString(txtDesc.Text);

                        #region SubType
                        //dt_subType.Columns.Add("id");
                        //dt_subType.Columns.Add("SUBTYPE_TEXT");
                        //DataRow drsub_type = dt_subType.NewRow();
                        //drsub_type[0]= Convert.ToString(ddlsubType.SelectedValue);
                        //drsub_type[1] = Convert.ToString(ddlsubType.Text);
                        dt_subType= SubTypeTodt("id", "SUBTYPE_TEXT", Convert.ToString(ddlsubType.SelectedValue), Convert.ToString(ddlsubType.Text));
                        #endregion

                        dr[12] = Convert.ToString(ddlsubType.SelectedValue);
                        dr[13] = Convert.ToString(ddlsubType.Text);
                        dr[14] = Convert.ToString(SharedObjects.WorkId);
                        dr[15] = Convert.ToString(Loginid);
                        createdby = Convert.ToString(Loginid);
                        dr[16] = Convert.ToString("FUNDINGBASE.ASPX");
                        dr[17] = Convert.ToString(txtRecoSource.Text);
                        dr[18] = Convert.ToString(txtAwardSuccesRate.Text);
                        string comment = string.Empty;
                        if (txtComment.Text == "crossref_identifier=")
                        {
                            comment = "";
                        }
                        else
                        {
                            comment = Convert.ToString(txtComment.Text);
                        }
                        dr[19] = Convert.ToString(comment);
                        dr[20] = Convert.ToString(ddlDefunct.Text);
                        if (txtCrossRefID.Text.Trim() != "")
                            dr[21] = Convert.ToString(txtCrossRefID.Text.Trim());
                        else
                            dr[21] = "";

                        if (ddlextendedRecord.SelectedIndex != 0)
                            dr[22] = Convert.ToString(ddlextendedRecord.SelectedItem);
                        else
                            dr[22] = "";
                        if (ddlCOpp.SelectedIndex != 0)
                        {
                            dr[23] = Convert.ToString(ddlCOpp.SelectedItem);
                        }
                        else
                        {
                            dr[23] = "";
                        }
                        if (textOppSup.Text != "")
                        {
                            dr[24] = Convert.ToString(textOppSup.Text.Trim());
                            string UpdateFunding_oppSupp = r.Return_WieredChar_Original(textOppSup.Text.Trim());
                            if (UpdateFunding_oppSupp != "")
                            {
                                dr[24] = UpdateFunding_oppSupp;
                            }
                        }
                        else
                        {
                            dr[24] = "";
                        }

                        if (ddlCAwards.SelectedIndex != 0)
                        {
                            dr[25] = Convert.ToString(ddlCAwards.SelectedItem);
                        }
                        else
                        {
                            dr[25] = "";
                        }
                        if (textAwardsSup.Text != "")
                        {
                            dr[26] = Convert.ToString(textAwardsSup.Text.Trim());

                            string UpdateFunding_awradSupp = r.Return_WieredChar_Original(textAwardsSup.Text.Trim());
                            if (UpdateFunding_awradSupp != "")
                            {
                                dr[26] = UpdateFunding_awradSupp;
                            }
                        }
                        else
                        {
                            dr[26] = "";
                        }

                        if (ddlTierInfo.SelectedIndex != 0)
                        {
                            dr[27] = Convert.ToString(ddlTierInfo.SelectedItem);
                        }
                        else
                        {
                            dr[27] = "";
                        }

                        dr[28] = Profit;
                        dr[29] = opportunitiesFrequency;
                        dr[30] = awardsFrequency;
                        dr[31] = "";
                        dtResult.Rows.Add(dr);
                        
                        DataSet ds = FundingBodyDataOperations.SaveFundingBody(dtResult);
                        dt_identifier = FundingBodyDataOperations.GetIdentifiers(fundindID);
                        DataTable dtAddress = new DataTable();
                        dtAddress = FundingBodyDataOperations.GetAddress(fundindID);
                        dt_address = addressTodt(dtAddress, Convert.ToString(ddlCountry.SelectedValue));
                        dt_fundingdescription = FundingBodyDataOperations.GetDescription(fundindID);
                        var URL = SharedObjects.CurrentUrl;
                        dt_awardSuccessRatedesc = FundingBodyDataOperations.GetAwardSucessData(fundindID);
                        dt_revisionhistory = FundingBodyDataOperations.GetRevisionHostory(fundindID);
                        dt_website = websiteTodt(URL);
                        dt_createddate = FundingBodyDataOperations.GetCreateDateData(fundindID);
                        dt_reviseddate = createdateToRevised(dt_createddate);
                        dt_fundingPolicy_Deatails = fundingPolicyToDT(URL, Convert.ToString(txtDesc.Text), "en");
                        DataTable EstData= FundingBodyDataOperations.GetEstablishmentInfoData(fundindID);
                        dt_establishmentInfo = establishmenetInfoTodt(EstData, Convert.ToString(txtRecoSource.Text));
                        #region Create JSON using Data Table
                        XmlJsonOperation xmlJsonOperation = new XmlJsonOperation();

                         string json= xmlJsonOperation.JsonCreationFromModel_FB(@"", dtResult, dt_preferredorgname, dt_contextname, dt_abbrevname, dt_acronym, dt_subType, dt_identifier, dt_fundingdescription, dt_website, dt_establishmentInfo, dt_address, dt_awardSuccessRatedesc, dt_revisionhistory, dt_createddate, dt_reviseddate);

                        // string json = xmlJsonOperation.JsonCreationFromModel_FBAll(@"", dtResult, dt_preferredorgname, dt_contextname, dt_abbrevname, dt_acronym, dt_subType, dt_identifier, dt_fundingdescription, dt_website, dt_establishmentInfo, dt_address, dt_awardSuccessRatedesc, dt_revisionhistory, dt_createddate, dt_reviseddate);
                        #endregion
                        FundingBodyDataOperations.saveandUpdateJSONinTable(fundindID, json, Convert.ToString(Loginid), DateTime.Now.ToString(), "", "", 1);


                        //jsondata = CreateJSONdata(json);
                        string fndID = string.Empty;
                        DataTable dtResultLang = new DataTable();

                        dtResultLang.Columns.Add("TRAN_ID");
                        dtResultLang.Columns.Add("FUNDINGBODY_ID");
                        dtResultLang.Columns.Add("COLUMN_DESC");
                        dtResultLang.Columns.Add("COLUMN_ID");
                        dtResultLang.Columns.Add("MODE_ID");
                        dtResultLang.Columns.Add("LANGUAGE_ID");
                        dtResultLang.Columns.Add("TRAN_TYPE_ID");
                        dtResultLang.Columns.Add("FLAG_IN");
                        try
                        {
                            fndID = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODY_ID"]);
                        }
                        catch (Exception ex)
                        {
                            fndID = Convert.ToString(SharedObjects.ID);
                        }

                        DataSet dsLoadFundLang = OpportunityDataOperations.LoadLanguageData(fndID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                        for (int iLang = 0; iLang <= 4; iLang++)
                        {
                            DataRow drLang;
                            int LangId = 0;
                            DataView dv;
                            try
                            {
                                if (iLang == 0)
                                {
                                    dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                    dv.RowFilter = "column_id='1'";
                                    if (dv.Count > 0)
                                    {
                                        UpdateCanonTID = Convert.ToInt32(dv[0]["tran_id"]);
                                    }
                                    else
                                    {
                                        UpdateCanonTID = 0;
                                    }

                                    dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                    dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangCanonicalName.SelectedValue) + "'";
                                    LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                    drLang = dtResultLang.NewRow();

                                    drLang[1] = fndID;
                                    drLang[2] = Convert.ToString(r.ReadandReplaceCharToHexa(txtCanoName.Text, InputXmlPath));

                                    string UpdateFunding_CanoName = Convert.ToString(r.Return_WieredChar_Original(txtCanoName.Text));
                                    if (UpdateFunding_CanoName != "")
                                    {
                                        drLang[2] = UpdateFunding_CanoName;
                                    }
                                    drLang[3] = 1;
                                    drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                    drLang[5] = LangId;
                                    drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                    if (UpdateCanonTID > 0)
                                    {
                                        drLang[0] = UpdateCanonTID;
                                        drLang[7] = 2;
                                    }
                                    else
                                    {
                                        drLang[0] = 0;
                                        drLang[7] = 1;
                                    }

                                    dtResultLang.Rows.Add(drLang);
                                }
                                else if (iLang == 1)
                                {
                                    if (dtGridPreOrg.RowCount > 0)
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());

                                        dv.RowFilter = "column_id='2' and IsNull(column_desc, 'Null Column')='Null Column'";
                                        if (dv.Count > 0)
                                        {
                                            UpdatePreOrgTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateContTID = 0;
                                        }

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = null;
                                        drLang[2] = null;
                                        drLang[3] = null;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = null;
                                        drLang[6] = null;
                                        if (UpdateContTID > 0)
                                        {
                                            drLang[0] = UpdateContTID;
                                            drLang[7] = 2;
                                        }
                                        else
                                        {
                                            drLang[0] = 0;
                                            drLang[7] = 1;
                                        }
                                        dtResultLang.Rows.Add(drLang);
                                        foreach (DataGridViewRow row in dtGridPreOrg.Rows)
                                        {
                                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                            string valueLang = row.Cells["PreOrg_language_code"].Value.ToString();
                                            dv.RowFilter = "column_id='2'and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["PreOrg_desc"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'" + " and Language_code=" + "'" + valueLang + "'";

                                            if (dv.Count > 0)
                                            {
                                                UpdateContTID = Convert.ToInt32(dv[0]["tran_id"]);
                                            }
                                            else
                                            {
                                                UpdateContTID = 0;
                                            }

                                            dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                            dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(valueLang) + "'";
                                            
                                            LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                            drLang = dtResultLang.NewRow();
                                            drLang[1] = fndID;
                                            if (r.chk_OtherLangId(LangId) == true)
                                            {
                                                drLang[2] = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());
                                            }
                                            else
                                            {
                                                drLang[2] = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);
                                            }
                                            drLang[3] = 2;
                                            drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                            drLang[5] = LangId;
                                            drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                            if (UpdateContTID > 0)
                                            {
                                                drLang[0] = UpdateContTID;
                                                drLang[7] = 2;
                                            }
                                            else
                                            {
                                                drLang[0] = 0;
                                                drLang[7] = 1;
                                            }
                                            dtResultLang.Rows.Add(drLang);
                                        }
                                    }
                                    else
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                        dv.RowFilter = "column_id='2'";
                                        if (dv.Count > 0)
                                        {
                                            UpdateAbbTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateAbbTID = 0;
                                        }

                                        dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                        dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangPreferredOrgName.SelectedValue) + "'";
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = fndID;
                                        if (r.chk_OtherLangId(LangId) == true)
                                        {
                                            drLang[2] = r.ConvertTextToUnicode(txtPreOrgName.Text.ToString());
                                        }
                                        else
                                        {
                                            drLang[2] = Convert.ToString(r.ReadandReplaceCharToHexa(txtPreOrgName.Text, InputXmlPath));

                                            string UpdateFunding_txtPreOrgName = Convert.ToString(r.Return_WieredChar_Original(txtPreOrgName.Text));
                                            if (UpdateFunding_txtPreOrgName != "")
                                            {
                                                drLang[2] = UpdateFunding_txtPreOrgName;
                                            }
                                        }
                                        drLang[3] = 4;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = LangId;
                                        drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                        if (UpdateAbbTID > 0)
                                        {
                                            drLang[0] = UpdateAbbTID;
                                            drLang[7] = 2;
                                        }
                                        else
                                        {
                                            drLang[0] = 0;
                                            drLang[7] = 1;
                                        }

                                        dtResultLang.Rows.Add(drLang);
                                    }
                                }
                                else if (iLang == 2)
                                {
                                    if (dtGridAbbreviation.RowCount > 0)
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());

                                        dv.RowFilter = "column_id='4' and IsNull(column_desc, 'Null Column')='Null Column'";
                                        if (dv.Count > 0)
                                        {
                                            UpdateContTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateContTID = 0;
                                        }

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = null;
                                        drLang[2] = null;
                                        drLang[3] = null;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = null;
                                        drLang[6] = null;
                                        if (UpdateContTID > 0)
                                        {
                                            drLang[0] = UpdateContTID;
                                            drLang[7] = 3;
                                        }

                                        dtResultLang.Rows.Add(drLang);
                                        foreach (DataGridViewRow row in dtGridAbbreviation.Rows)
                                        {
                                            dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                            dv.RowFilter = "column_id='4' and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["column_descr"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'";
                                            if (dv.Count > 0)
                                            {
                                                UpdateContTID = Convert.ToInt32(dv[0]["tran_id"]);
                                            }
                                            else
                                            {
                                                UpdateContTID = 0;
                                            }
                                            string valueLang = row.Cells["AbbLanguage_code"].Value.ToString();
                                            dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                            dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(valueLang) + "'";
                                            LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                            drLang = dtResultLang.NewRow();
                                            drLang[1] = fndID;
                                            if (r.chk_OtherLangId(LangId) == true)
                                            {
                                                drLang[2] = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());

                                                string UpdateFunding_Abbreviation = Convert.ToString(r.Return_WieredChar_Original(row.Cells[1].Value.ToString()));
                                                if (UpdateFunding_Abbreviation != "")
                                                {
                                                    drLang[2] = UpdateFunding_Abbreviation;
                                                }
                                            }
                                            else
                                            {
                                                drLang[2] = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);

                                                string UpdateFunding_Abbreviation = Convert.ToString(r.Return_WieredChar_Original(row.Cells[1].Value.ToString()));
                                                if (UpdateFunding_Abbreviation != "")
                                                {
                                                    drLang[2] = UpdateFunding_Abbreviation;
                                                }
                                            }
                                            drLang[3] = 4;
                                            drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                            drLang[5] = LangId;
                                            drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                            if (UpdateContTID > 0)
                                            {
                                                drLang[0] = UpdateContTID;
                                                drLang[7] = 2;
                                            }
                                            else
                                            {
                                                drLang[0] = 0;
                                                drLang[7] = 1;
                                            }
                                            dtResultLang.Rows.Add(drLang);
                                        }
                                    }
                                    else
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                        dv.RowFilter = "column_id='4'";
                                        if (dv.Count > 0)
                                        {
                                            UpdateAbbTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateAbbTID = 0;
                                        }

                                        dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                        dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangAbbreviation.SelectedValue) + "'";
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = fndID;

                                        if (r.chk_OtherLangId(LangId) == true)
                                        {
                                            drLang[2] = r.ConvertTextToUnicode(txtAbb.Text.ToString());
                                        }
                                        else
                                        {
                                            drLang[2] = Convert.ToString(r.ReadandReplaceCharToHexa(txtAbb.Text, InputXmlPath));
                                        }
                                        drLang[3] = 4;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = LangId;
                                        drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                        if (UpdateAbbTID > 0)
                                        {
                                            drLang[0] = UpdateAbbTID;
                                            drLang[7] = 2;
                                        }
                                        else
                                        {
                                            drLang[0] = 0;
                                            drLang[7] = 1;
                                        }
                                        dtResultLang.Rows.Add(drLang);
                                    }
                                }
                                else if (iLang == 3)
                                {
                                    foreach (DataGridViewRow row in dtGridContextName.Rows)
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                        dv.RowFilter = "column_id='3' and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["column_desc"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'";
                                        if (dv.Count > 0)
                                        {
                                            UpdateContTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateContTID = 0;
                                        }
                                        string valueLang = row.Cells["language_code"].Value.ToString().ToUpper();
                                        dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                        dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(valueLang) + "'";
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = fndID;

                                        if (r.chk_OtherLangId(LangId) == true)
                                        {
                                            drLang[2] = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());

                                            string UpdateFunding_ContextNam = Convert.ToString(r.Return_WieredChar_Original(row.Cells[1].Value.ToString()));
                                            if (UpdateFunding_ContextNam != "")
                                            {
                                                drLang[2] = UpdateFunding_ContextNam;
                                            }
                                        }
                                        else
                                        {
                                            drLang[2] = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);
                                        }
                                        drLang[3] = 3;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = LangId;
                                        drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                        if (UpdateContTID > 0)
                                        {
                                            drLang[0] = UpdateContTID;
                                            drLang[7] = 2;
                                        }
                                        else
                                        {
                                            drLang[0] = 0;
                                            drLang[7] = 1;
                                        }
                                        dtResultLang.Rows.Add(drLang);
                                    }
                                }
                                else if (iLang == 4)
                                {
                                    foreach (DataGridViewRow row in dtGridAcronym.Rows)
                                    {
                                        dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());
                                        dv.RowFilter = "column_id='7' and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["Acr_column_desc"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'";
                                        if (dv.Count > 0)
                                        {
                                            UpdateContTID = Convert.ToInt32(dv[0]["tran_id"]);
                                        }
                                        else
                                        {
                                            UpdateContTID = 0;
                                        }
                                        string valueLang = row.Cells["Acr_language_code"].Value.ToString().ToUpper();
                                        dv = new DataView(dsFunding.Tables["LanguageTable"].Copy());
                                        dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(valueLang) + "'";
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                        drLang = dtResultLang.NewRow();
                                        drLang[1] = fndID;
                                        if (r.chk_OtherLangId(LangId) == true)
                                        {
                                            drLang[2] = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());

                                            string UpdateFunding_Acronym = Convert.ToString(r.Return_WieredChar_Original(row.Cells[1].Value.ToString()));
                                            if (UpdateFunding_Acronym != "")
                                            {
                                                drLang[2] = UpdateFunding_Acronym;
                                            }
                                        }
                                        else
                                        {
                                            drLang[2] = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);

                                            string UpdateFunding_Acronym = Convert.ToString(r.Return_WieredChar_Original(row.Cells[1].Value.ToString()));
                                            if (UpdateFunding_Acronym != "")
                                            {
                                                drLang[2] = UpdateFunding_Acronym;
                                            }
                                        }
                                        drLang[3] = 7;
                                        drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                        drLang[5] = LangId;
                                        drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                        if (UpdateContTID > 0)
                                        {
                                            drLang[0] = UpdateContTID;
                                            drLang[7] = 2;
                                        }
                                        else
                                        {
                                            drLang[0] = 0;
                                            drLang[7] = 1;
                                        }
                                        dtResultLang.Rows.Add(drLang);
                                    }
                                }
                            }
                            catch (Exception ex)
                            { 
                            
                            }
                        }
                        DataSet dsLang = FundingBodyDataOperations.SaveFundingBodyLang(dtResultLang);
                        dtGridContextName.Refresh();
                        
                        SharedObjects.IsFundingBaseFilled = true;
                        DataTable dt = ds.Tables["FundingBodyTable"];
                        DataTable dtCopy = dt.Copy();
                        DataSet dsFund = SharedObjects.StartWork;
                        dsFunding.Tables.Remove("FundingBodyTable");
                        dsFunding.Tables.Add(dtCopy);




                        SharedObjects.StartWork = dsFunding;

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess_update("Fundingbody");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        statusLbl.Text = "Data Saved Successfully.";
            }
                    catch (Exception ex)
            {
                statusLbl.Text = "Error";
                oErrorLog.WriteErrorLog(ex);

            }
        }
                #endregion

                //jsondata = jsondata.Replace("{{", "{").Replace("}}", "}");
                //string jsondatafinal = jsondata.ToString().Replace("\"","'");

            }
            catch (Exception ex)
            {
                statusLbl.Text = ex.Message;
                statusLbl.ForeColor = Color.Red;
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private DataTable GridTodt(DataGridView dataGridView1, string lang, string preferredorgname_text)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("lang");
            dt.Columns.Add(preferredorgname_text);

            //Adding the Rows.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = row.Cells[2].Value.ToString();
                dt.Rows[dt.Rows.Count - 1][1] = row.Cells[1].Value.ToString();
            }
            return dt;
        }
        private DataTable fundingPolicyToDT(string url, string description, string lang)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LANG");
            dt.Columns.Add("DESCRIPTION");
            dt.Columns.Add("URL");

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = url;
            dt.Rows[dt.Rows.Count - 1][1] = description;
            dt.Rows[dt.Rows.Count - 1][2] = lang;
            return dt;
        }
        private DataTable SubTypeTodt(string id, string subType_txt, string idValue, string subTypevalue)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("SUBTYPE_TEXT");

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = idValue;
            dt.Rows[dt.Rows.Count - 1][1] = subTypevalue;
            return dt;
        }
        private DataTable createdateToRevised(DataTable createdDate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("VERSION");
            dt.Columns.Add("REVISEDDATE_TEXT");
            if (createdDate.Rows.Count == 0)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = "";
                dt.Rows[dt.Rows.Count - 1][1] = "";
            }
            else
            {
                foreach (DataRow row in createdDate.Rows)
                {
                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1][0] = row[1];
                    dt.Rows[dt.Rows.Count - 1][1] = row[0];
                }
            }
            return dt;
        }
        private DataTable websiteTodt(string website_txt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("url");
            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = website_txt;
            return dt;
        }

        private DataTable addressTodt(DataTable dtAddress, string country)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("COUNTRYTEST");
            dt.Columns.Add("city");
            dt.Columns.Add("COUNTRY");
            dt.Columns.Add("postalcode");
            dt.Columns.Add("room");
            dt.Columns.Add("state");
            dt.Columns.Add("street");
            if (dtAddress.Rows.Count == 0)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = "";
                dt.Rows[dt.Rows.Count - 1][1] = "";
                dt.Rows[dt.Rows.Count - 1][2] = country;
                dt.Rows[dt.Rows.Count - 1][3] = "";
                dt.Rows[dt.Rows.Count - 1][4] = "";
                dt.Rows[dt.Rows.Count - 1][5] = "";
                dt.Rows[dt.Rows.Count - 1][6] = "";
            }
            else
            {
                foreach (DataRow row in dtAddress.Rows)
                {
                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1][0] = row[0];
                    dt.Rows[dt.Rows.Count - 1][1] = row[3];
                    dt.Rows[dt.Rows.Count - 1][2] = country;
                    dt.Rows[dt.Rows.Count - 1][3] = row[5];
                    dt.Rows[dt.Rows.Count - 1][4] = row[1];
                    dt.Rows[dt.Rows.Count - 1][5] = row[4];
                    dt.Rows[dt.Rows.Count - 1][6] = row[2];
                }
            }
            return dt;
        }

        private DataTable establishmenetInfoTodt(DataTable EstData, string recordsource)
        {
            EstData.Columns.Add("ESTABLISHMENTDATE");
            EstData.Columns.Add("ESTABLISHMENTCOUNTRYCODE");
            EstData.Columns.Add("LANG");
            EstData.Columns.Add("ESTABLISHMENTDESCRIPTION");
            EstData.Columns.Add("recordSource");
            if (EstData.Rows.Count == 0)
            {
                EstData.Rows.Add();
                EstData.Rows[EstData.Rows.Count - 1][0] = "";
                EstData.Rows[EstData.Rows.Count - 1][1] = "";
                EstData.Rows[EstData.Rows.Count - 1][2] = "";
                EstData.Rows[EstData.Rows.Count - 1][3] = "";
                EstData.Rows[EstData.Rows.Count - 1][4] = recordsource;
            }
            else
            {
                foreach (DataRow row in EstData.Rows)
                {
                    EstData.Rows.Add();
                    EstData.Rows[EstData.Rows.Count - 1][0] = row[0];
                    EstData.Rows[EstData.Rows.Count - 1][1] = row[1];
                    EstData.Rows[EstData.Rows.Count - 1][2] = row[2];
                    EstData.Rows[EstData.Rows.Count - 1][3] = row[3];
                    EstData.Rows[EstData.Rows.Count - 1][4] = recordsource;
                }
            }
            return EstData;
        }

        private StringBuilder CreateJSONdata(DataTable dtResult)
        {
            StringBuilder str = new StringBuilder();
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dtResult);
            str.Append(JSONresult).Replace("[{","{").Replace("}]", "}");
            //str.Replace("\"",  "/"+"\"");
            return str;
        }
        private void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

                if (ddlCountry.SelectedIndex != 0)
                {                    
                    DataTable temp = dsFunding.Tables["State"].Copy();
                    DataView dv1 = temp.DefaultView;
                    //temp.DefaultView.RowFilter = "COUNTRYCODE='" + ddlCountry.SelectedValue + "'";
                    // DataTable dtState = temp.DefaultView.ToTable();
                    dv1.RowFilter = "countrycode='" + ddlCountry.SelectedValue + "'";
                    DataTable dtState = dv1.ToTable();

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

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                DataSet dsFunding = SharedObjects.StartWork;

                DataTable temp = dsFunding.Tables["FundingBodySubTypes"];
                DataTable dtSubType = temp.DefaultView.ToTable();

                DataRow dr = dtSubType.NewRow();
                dr["VALUE"] = "SelectSubType";
                dr["SUBTYPE"] = "--Select SubType--";
                dtSubType.Rows.InsertAt(dr, 0);

                ddlsubType.DataSource = dtSubType;
                ddlsubType.ValueMember = "VALUE";
                ddlsubType.DisplayMember = "SUBTYPE";

                if (Convert.ToString(ddlType.SelectedValue) == "gov")
                    ddlTrusting.SelectedIndex = 2;
                else
                    ddlTrusting.SelectedIndex = -1;// 0;
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
                if (ddlState.SelectedValue == "OtherState")
                    txtOtherState.Enabled = true;
                else
                    txtOtherState.Enabled = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
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

        private void button3_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtRecoSource.Text = SharedObjects.CurrentUrl;
        }

        private void btnSaveContextNameList_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (txtContextname.Text == "")
                {
                    MessageBox.Show("Please enter Context Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangContextName.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataGridViewRow row in dtGridContextName.Rows)
                    {
                        string ContextNameValue = row.Cells["column_desc"].Value.ToString();
                        if (ContextNameValue.ToUpper() == txtContextname.Text.ToUpper())
                        {
                            MessageBox.Show("Context Name can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    string ddlContextID = Convert.ToString(ddlLangContextName.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = txtContextname.Text;
                    string thirdColum = ddlContextID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridContextName.Rows.Add(rowGrid);

                    ddlLangContextName.SelectedIndex = 18;
                    txtContextname.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void noTexrecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("TYPE");
                dtNoRcrd.Columns.Add("ConrextName");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                dtGridContextName.AutoGenerateColumns = false;
                dtGridContextName.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void loadDdlLang()
        {
            try
            {
                DataSet dslanguage = DashboardDataOperations.getLanguageDetails(2);

                DataTable tempLangCanonicalName = dslanguage.Tables["LANGUAGE"].Copy();
                DataRow drLangCanonicalName = tempLangCanonicalName.NewRow();
                drLangCanonicalName["LANGUAGE_CODE"] = "SelectLanguage";
                drLangCanonicalName["LANGUAGE_NAME"] = "--Select Language--";
                tempLangCanonicalName.Rows.InsertAt(drLangCanonicalName, 0);

                ddlLangCanonicalName.DataSource = tempLangCanonicalName;
                ddlLangCanonicalName.ValueMember = "LANGUAGE_CODE";
                ddlLangCanonicalName.DisplayMember = "LANGUAGE_NAME";
                ddlLangCanonicalName.SelectedIndex = 18;

                DataTable tempLangPreferredOrgName = dslanguage.Tables["LANGUAGE"].Copy();
                DataRow drLangPreferredOrgName = tempLangPreferredOrgName.NewRow();
                drLangPreferredOrgName["LANGUAGE_CODE"] = "SelectLanguage";
                drLangPreferredOrgName["LANGUAGE_NAME"] = "--Select Language--";
                tempLangPreferredOrgName.Rows.InsertAt(drLangPreferredOrgName, 0);

                ddlLangPreferredOrgName.DataSource = tempLangPreferredOrgName;
                ddlLangPreferredOrgName.ValueMember = "LANGUAGE_CODE";
                ddlLangPreferredOrgName.DisplayMember = "LANGUAGE_NAME";
                ddlLangPreferredOrgName.SelectedIndex = 18;

                DataTable tempLangContextName = dslanguage.Tables["LANGUAGE"].Copy();
                DataRow drLangContextName = tempLangContextName.NewRow();
                drLangContextName["LANGUAGE_CODE"] = "SelectLanguage";
                drLangContextName["LANGUAGE_NAME"] = "--Select Language--";
                tempLangContextName.Rows.InsertAt(drLangContextName, 0);

                ddlLangContextName.DataSource = tempLangContextName;
                ddlLangContextName.ValueMember = "LANGUAGE_CODE";
                ddlLangContextName.DisplayMember = "LANGUAGE_NAME";
                ddlLangContextName.SelectedIndex = 18;

                DataTable tempLangAbbreviation = dslanguage.Tables["LANGUAGE"].Copy();
                DataRow drLangAbbreviation = tempLangAbbreviation.NewRow();
                drLangAbbreviation["LANGUAGE_CODE"] = "SelectLanguage";
                drLangAbbreviation["LANGUAGE_NAME"] = "--Select Language--";
                tempLangAbbreviation.Rows.InsertAt(drLangAbbreviation, 0);

                ddlLangAbbreviation.DataSource = tempLangAbbreviation;
                ddlLangAbbreviation.ValueMember = "LANGUAGE_CODE";
                ddlLangAbbreviation.DisplayMember = "LANGUAGE_NAME";
                ddlLangAbbreviation.SelectedIndex = 18;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        public string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void dtGridContextName_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                if (dv.Count > 0)
                {
                    int cloTranID = Convert.ToInt32(dtGridContextName.Rows[e.RowIndex].Cells["TRAN_ID"].Value.ToString());
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
                if (UpdateContTID > 0)
                {
                    drLang[0] = UpdateContTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = FundingBodyDataOperations.SaveFundingBodyLang(dtDelLang);

                    if (this.dtGridContextName.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridContextName.Rows.RemoveAt(this.dtGridContextName.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Context Name deleted successfully.";
                }
                else
                {
                    if (this.dtGridContextName.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridContextName.Rows.RemoveAt(this.dtGridContextName.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Context Name deleted successfully.";
                }
                dtGridContextName.Refresh();
            }
        }

        private void btnAddAbbreviation_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (txtAbb.Text == "")
                {
                    MessageBox.Show("Please enter Abbreviation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangAbbreviation.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataGridViewRow row in dtGridAbbreviation.Rows)
                    {
                        string AbbreviationValue = row.Cells["column_descr"].Value.ToString();
                        if (AbbreviationValue.ToUpper() == txtAbb.Text.ToUpper())
                        {
                            MessageBox.Show("Abbreviation can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    string ddlAbbreviationID = Convert.ToString(ddlLangAbbreviation.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = txtAbb.Text;
                    string thirdColum = ddlAbbreviationID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridAbbreviation.Rows.Add(rowGrid);

                    ddlLangAbbreviation.SelectedIndex = 18;
                    txtAbb.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void dtGridAbbreviation_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                if (dv.Count > 0)
                {
                    int cloTranID = Convert.ToInt32(dtGridAbbreviation.Rows[e.RowIndex].Cells["TRANID"].Value.ToString());
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
                if (UpdateContTID > 0)
                {
                    drLang[0] = UpdateContTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = FundingBodyDataOperations.SaveFundingBodyLang(dtDelLang);

                    if (this.dtGridAbbreviation.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridAbbreviation.Rows.RemoveAt(this.dtGridAbbreviation.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Abbreviation deleted successfully.";
                }
                else
                {
                    if (this.dtGridAbbreviation.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridAbbreviation.Rows.RemoveAt(this.dtGridAbbreviation.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Abbreviation deleted successfully.";
                }
                dtGridAbbreviation.Refresh();
            }
        }

        private void btnAddAcronym_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (textAcronym.Text == "")
                {
                    MessageBox.Show("Please enter Acronym.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangAcronym.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataGridViewRow row in dtGridAcronym.Rows)
                    {
                        string AcronymValue = row.Cells["Acr_column_desc"].Value.ToString();
                        if (AcronymValue.ToUpper() == textAcronym.Text.ToUpper())
                        {
                            MessageBox.Show("Abbreviation can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    string ddlAcronymID = Convert.ToString(ddlLangAcronym.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = textAcronym.Text;
                    string thirdColum = ddlAcronymID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridAcronym.Rows.Add(rowGrid);

                    ddlLangAcronym.SelectedIndex = 18;
                    textAcronym.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void dtGridAcronym_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                if (dv.Count > 0)
                {
                    int cloTranID = Convert.ToInt32(dtGridAcronym.Rows[e.RowIndex].Cells["Acr_tran_id"].Value.ToString());
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
                if (UpdateContTID > 0)
                {
                    drLang[0] = UpdateContTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = FundingBodyDataOperations.SaveFundingBodyLang(dtDelLang);

                    if (this.dtGridAcronym.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridAcronym.Rows.RemoveAt(this.dtGridAcronym.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Acronym deleted successfully.";
                }
                else
                {
                    if (this.dtGridAcronym.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridAcronym.Rows.RemoveAt(this.dtGridAcronym.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Acronym deleted successfully.";
                }
                dtGridAcronym.Refresh();
            }
        }

        private void ddlCOpp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCOpp.SelectedIndex == 2)
            {
                textOppSup.Enabled = true;
            }
            else
            {
                textOppSup.Enabled = false;
            }
        }

        private void ddlCAwards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCAwards.SelectedIndex == 2)
            {
                textAwardsSup.Enabled = true;
            }
            else
            {
                textAwardsSup.Enabled = false;
            }
        }

        private void btnAddPreOrg_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (txtPreOrgName.Text == "")
                {
                    MessageBox.Show("Please enter Prefered Org Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangPreferredOrgName.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataGridViewRow row in dtGridPreOrg.Rows)
                    {
                        string PreOrgValue = row.Cells["PreOrg_desc"].Value.ToString();
                        string langValue = row.Cells["PreOrg_language_code"].Value.ToString().Trim();
                        if ((PreOrgValue.ToUpper().Trim() == txtPreOrgName.Text.ToUpper().Trim()) && (langValue == Convert.ToString(ddlLangPreferredOrgName.SelectedValue)))
                        {
                            MessageBox.Show("Prefered Org Name can't be same with same language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    string ddlPreOrgID = Convert.ToString(ddlLangPreferredOrgName.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = txtPreOrgName.Text;
                    string thirdColum = ddlPreOrgID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridPreOrg.Rows.Add(rowGrid);

                    ddlLangPreferredOrgName.SelectedIndex = 18;
                    txtPreOrgName.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void dtGridPreOrg_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                if (dv.Count > 0)
                {
                    int cloTranID = Convert.ToInt32(dtGridPreOrg.Rows[e.RowIndex].Cells["PreOrg_tran_id"].Value.ToString());
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
                if (UpdateContTID > 0)
                {
                    drLang[0] = UpdateContTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = FundingBodyDataOperations.SaveFundingBodyLang(dtDelLang);

                    if (this.dtGridPreOrg.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridPreOrg.Rows.RemoveAt(this.dtGridPreOrg.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Prefered org deleted successfully.";
                }
                else
                {
                    if (this.dtGridPreOrg.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridPreOrg.Rows.RemoveAt(this.dtGridPreOrg.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Prefered org deleted successfully.";
                }
                dtGridPreOrg.Refresh();
            }
        }

        private void ddlsubType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddl_opportunitiesFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_opportunitiesFrequency.SelectedIndex == 3)
            {
                Y_opportunitiesFrequency.Visible = true;
            }
            else
            {
                Y_opportunitiesFrequency.Visible = false;
            }
        }

        private void ddl_awardsFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_awardsFrequency.SelectedIndex == 3)
            {
                Y_awardsFrequency.Visible = true;
            }
            else
            {
                Y_awardsFrequency.Visible = false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtContextname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void ddlTierInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddlLangContextName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}