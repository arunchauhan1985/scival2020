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
using System.IO;
using MySqlDal;

namespace Scival.Award
{
    public partial class AwardBase : UserControl
    {
        Replace r = new Replace();
        private Awards m_parent;
        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");

        int UpdateAWNameTID = 0;
        int UpdateAWAbstTID = 0;
        string InputXmlPath = string.Empty;
        ErrorLog oErrorLog = new ErrorLog();



        string secondColum_AWT = string.Empty;

        int UpdateContTID = 0;
        public AwardBase(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }


        void ddlHidden_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangAbstract_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangAwName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlLangKeywords_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        private void LoadInitialValue()
        {
            InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                lblMsg.Visible = false;
                DataSet dsOpptunity = SharedObjects.StartWork;

                if (SharedObjects.WorkId != 0)
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 0;  // New Award


                    }
                    else if (SharedObjects.TaskId == 1 && SharedObjects.Cycle > 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 1;   // Update Award
                    }
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle > 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 1;   // Update Award
                    }
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                    {
                        SharedObjects.TRAN_TYPE_ID = 0;    // QA Award
                    }

                    // Fill Type Dropdown

                    DataTable temp = dsOpptunity.Tables["FundingBodyTypes"].Copy();

                    DataRow dr = temp.NewRow();
                    dr["TYPEID"] = "SelectType";
                    dr["FUNDEDPROGRAMTYPESTEXT"] = "--Select Type--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlType.DataSource = temp;
                    ddlType.ValueMember = "TYPEID";
                    ddlType.DisplayMember = "FUNDEDPROGRAMTYPESTEXT";

                    ddlHidden.Items.Insert(0, "false");
                    ddlHidden.Items.Insert(1, "true");
                    ddlHidden.SelectedIndex = 0;

                    // Fill Language Dropdown
                    DataTable tempAWName = dsOpptunity.Tables["LanguageTable"].Copy();
                    dr = tempAWName.NewRow();
                    dr["LANGUAGE_CODE"] = "SelectLanguage";
                    dr["LANGUAGE_NAME"] = "--Select Language--";
                    tempAWName.Rows.InsertAt(dr, 0);

                    ddlLangAwName.DataSource = tempAWName;
                    ddlLangAwName.ValueMember = "LANGUAGE_CODE";
                    ddlLangAwName.DisplayMember = "LANGUAGE_NAME";
                    ddlLangAwName.SelectedIndex = 18;

                    DataTable tempAbst = dsOpptunity.Tables["LanguageTable"].Copy();
                    dr = tempAbst.NewRow();
                    dr["LANGUAGE_CODE"] = "SelectLanguage";
                    dr["LANGUAGE_NAME"] = "--Select Language--";
                    tempAbst.Rows.InsertAt(dr, 0);

                    ddlLangAbstract.DataSource = tempAbst;
                    ddlLangAbstract.ValueMember = "LANGUAGE_CODE";
                    ddlLangAbstract.DisplayMember = "LANGUAGE_NAME";
                    ddlLangAbstract.SelectedIndex = 18;

                    DataTable tempKeyword = dsOpptunity.Tables["LanguageTable"].Copy();
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
                        SharedObjects.IsAwardBaseFilled = true;


                        ddlType.SelectedValue = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["TYPE"]);
                        txtTrust.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["TRUSTING"]);
                        ddlHidden.SelectedItem = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["HIDDEN"]);

                        DateTime strt = new DateTime();

                        if (Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["STARTDATE"]) != "")
                        {
                            strt = Convert.ToDateTime(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["STARTDATE"]);
                            txtSrtDate.Text = strt.ToShortDateString();
                        }
                        if (Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LASTAMENDEDDATE"]) != "")
                        {
                            strt = Convert.ToDateTime(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["LASTAMENDEDDATE"]);
                            txtAmendate.Text = strt.ToShortDateString();
                        }
                        if (Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ENDDATE"]) != "")
                        {
                            strt = Convert.ToDateTime(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["ENDDATE"]);
                            txtEndDateDate.Text = strt.ToShortDateString();
                        }
                        #region//Added by avanish on 7-june-2018
                        if (Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["AwardNoticeDate"]) != "")
                        {
                            strt = Convert.ToDateTime(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["AwardNoticeDate"]);
                            txtAwardNoticeDate.Text = strt.ToShortDateString();
                        }
                        #endregion


                        #region//Added by avanish on 17-Feb-2020
                        if (Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["PUBLISHEDDATE"]) != "")
                        {
                            txtPublishedDate.Text = Convert.ToDateTime(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["PUBLISHEDDATE"]).ToShortDateString();

                        }
                        #endregion

                        txtFAID.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["FUNDINGBODYAWARDID"]);
                        txtRecordSource.Text = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["RECORDSOURCE"]).TrimStart().TrimStart();

                        if (dsOpptunity.Tables.Contains("Keywords") == true)
                        {
                            if (dsOpptunity.Tables["Keywords"].Rows.Count > 0)
                            {
                                #region Added by avanish
                                for (int iAbbr = 0; iAbbr < dsOpptunity.Tables["Keywords"].Rows.Count; iAbbr++)
                                {
                                    DataView dv1;
                                    dv1 = new DataView(dsOpptunity.Tables["Keywords"].Copy());
                                    string firstCol = Convert.ToString(dv1[iAbbr]["Keywords_id"]);
                                    string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv1[iAbbr]["Award_Keywords"].ToString(), InputXmlPath));

                                    //pankaj WieredChar managed mechanism 

                                    string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv1[iAbbr]["Award_Keywords"].ToString()));
                                    if (UpdateFunding_difflang != "")
                                    {
                                        secondCol = UpdateFunding_difflang;
                                    }

                                    string thirdCol = Convert.ToString(dv1[iAbbr]["LANG"]);

                                    if (r.chk_OtherLang(thirdCol.ToLower()) == true)
                                    {
                                        string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
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
                                #endregion
                            }
                        }
                        // Get Language Details- Added by Harish(@ 07-March-2014
                        string awID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["AWARD_ID"]);
                        //pankaj
                        SharedObjects.AbstarctTitleLang = awID.ToString();
                        DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(awID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        DataView dv;
                        // Award NAME
                        dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                        dv.RowFilter = "column_id='5'";
                        if (dv.Count > 0)
                        {

                            #region Added by avanish
                            for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                            {
                                string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath));
                                //pankaj WieredChar managed mechanism 

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
                                }
                                string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());

                                if (r.chk_OtherLang(thirdCol.ToLower()) == true)
                                {
                                    string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                    if (secondCol_difflang != "")
                                    {
                                        secondCol = secondCol_difflang;
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
                            #endregion



                        }
                        // Abstract
                        dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                        dv.RowFilter = "column_id='6'";
                        if (dv.Count > 0)
                        {
                            #region Added by avanish
                            for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                            {
                                string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath));
                                //pankaj WieredChar managed mechanism 

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
                                }
                                string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());
                                if (r.chk_OtherLang(thirdCol.ToLower()) == true)
                                {
                                    string secondCol_difflang = r.ConvertUnicodeToText(secondCol.ToString());
                                    if (secondCol_difflang != "")
                                    {
                                        secondCol = secondCol_difflang;
                                    }
                                }
                                string[] row = { firstCol, secondCol, thirdCol };

                                if (dv.Count == 1 && secondCol == "")
                                { }
                                else
                                {
                                    dtGridAbstract.Rows.Add(row);
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        SharedObjects.IsAwardBaseFilled = false;
                    }


                }
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
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            string url_txtLinkUrl = txtRecordSource.Text.TrimStart().TrimEnd();
            //pankaj 12 july
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        //created by pankaj for V4
                        int p_workflowid = Convert.ToInt32(SharedObjects.WorkId);
                        Int64 p_insdel = 0;

                        string strKeyWords = txtKeywords.Text.ToString();
                        string[] KeyWordsArr = strKeyWords.Split(',');
                        string p_LANG = ddlLangKeywords.Text.ToString();
                        CheckAwardBase(p_workflowid);
                        string p_DESCRIPTION = Convert.ToString(KeyWordsArr);




                        InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                        lblMsg.Visible = false;
                        Regex strRgx = new Regex(@"[A-Za-z ]");

                        string AWName = "";
                        string AWAbstract = "";
                        string FBAWID = Regex.Replace(txtFAID.Text,@"[A-Za-z ]", "");

                        Regex intRgx = new Regex(@"^[0-9]+");
                        DateTime strtDate = new DateTime(); DateTime endDate = new DateTime(), PublishedDate = new DateTime();


                        #region

                        if (dtGridName.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dtGridName.Rows)
                            {
                                string langValue = row.Cells["language_code"].Value.ToString();
                                if (langValue.ToLower() == "en")
                                {

                                    AWName = row.Cells["awName"].Value.ToString();
                                    AWName = AwardDataOperations.InsertDemoData(AWName);

                                    break;

                                }




                            }
                            if (AWName == "")
                            {


                                string langValue = dtGridName.Rows[0].Cells["language_code"].Value.ToString();
                                if (r.chk_OtherLang(langValue) == true)
                                {
                                    AWName = r.ConvertTextToUnicode(dtGridName.Rows[0].Cells["awName"].Value.ToString());

                                }
                                else
                                {
                                    AWName = AwardDataOperations.InsertDemoData(dtGridName.Rows[0].Cells["awName"].Value.ToString());

                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter At least one name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;

                        }


                        if (dtGridAbstract.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dtGridAbstract.Rows)
                            {


                                string langValue = row.Cells["language_codes"].Value.ToString();
                                if (langValue.ToLower() == "en")
                                {
                                    AWAbstract = row.Cells["Abstract"].Value.ToString();
                                    AWAbstract = Regex.Replace(AWAbstract,@"[A-Za-z ]", "");
                                    AWAbstract = Convert.ToString(r.ReadandReplaceCharToHexa(AWAbstract, InputXmlPath));
                                    break;
                                }




                            }
                            if (AWAbstract == "")
                            {
                                string _result = oErrorLog.htlmtag(dtGridAbstract.Rows[0].Cells["Abstract"].Value.ToString().Trim(), "Abstract");
                                if (!_result.Equals(""))
                                {
                                    MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                                string langValue = dtGridAbstract.Rows[0].Cells["language_codes"].Value.ToString();
                                if (r.chk_OtherLang(langValue) == true)
                                {
                                    AWAbstract = r.ConvertTextToUnicode(dtGridAbstract.Rows[0].Cells["Abstract"].Value.ToString());

                                }
                                else
                                {

                                    AWAbstract = dtGridAbstract.Rows[0].Cells["Abstract"].Value.ToString();
                                    AWAbstract = Regex.Replace(AWAbstract,@"[A-Za-z ]", "");
                                    AWAbstract = Convert.ToString(r.ReadandReplaceCharToHexa(AWAbstract, InputXmlPath));


                                }
                            }
                        }

                        if (txtSrtDate.Text != "")
                            strtDate = Convert.ToDateTime(txtSrtDate.Text);
                        else
                        {
                            MessageBox.Show("Please enter startdate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtEndDateDate.Text != "")
                            endDate = Convert.ToDateTime(txtEndDateDate.Text);

                        if (txtPublishedDate.Text.Trim() != "")
                            PublishedDate = Convert.ToDateTime(txtPublishedDate.Text);

                        if (txtFAID.Text.Trim() == "" || (pattern.Matches(FBAWID).Count > 0))
                        {
                            MessageBox.Show("Please enter valid Funding Body Award ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (ddlType.SelectedIndex == 0)
                        {
                            MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtSrtDate.Text.Trim() != "" && txtEndDateDate.Text.Trim() != "" && strtDate.CompareTo(endDate) > 0)
                        {
                            MessageBox.Show("Start Date can not greater than Enddate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        /////////////  bY pIYUSH ON 14-fEB-2013 /////////////////////
                        else if (txtRecordSource.Text == "")
                        {
                            MessageBox.Show("Please Fill Record Source", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        /////////////  bY pIYUSH ON 14-fEB-2013 /////////////////////
                        else
                        {
                            DataSet dsOpptunity = SharedObjects.StartWork;

                            DataTable dtResult = new DataTable();
                            dtResult.Columns.Add("WFID");
                            dtResult.Columns.Add("TYPEID");
                            dtResult.Columns.Add("TRUSTING");
                            dtResult.Columns.Add("COLLECTIONCODE");
                            dtResult.Columns.Add("HIDDEN");
                            dtResult.Columns.Add("NAME");
                            dtResult.Columns.Add("STARTDATE");
                            dtResult.Columns.Add("AMENDEDDATE");
                            dtResult.Columns.Add("ENDDATE");
                            dtResult.Columns.Add("ABSTRACT");
                            dtResult.Columns.Add("mode");
                            dtResult.Columns.Add("URL");
                            dtResult.Columns.Add("FAID");
                            //// New update on 14-Feb-2013 by Piyush //
                            dtResult.Columns.Add("RECORDSOURCE");
                            ////////////////////////////////////////////

                            #region added by avanish on 7-June-2018
                            dtResult.Columns.Add("AwardNoticeDate");
                            #endregion
                            #region added by avanish on 7-June-2018
                            dtResult.Columns.Add("PublishedDate");
                            #endregion

                            DataRow dr = dtResult.NewRow();
                            dr[0] = Convert.ToString(SharedObjects.WorkId);

                            if (Convert.ToString(ddlType.SelectedValue) != "SelectType")
                                dr[1] = Convert.ToString(ddlType.SelectedValue);
                            else
                                dr[1] = "";

                            dr[2] = txtTrust.Text.Trim();


                            dr[3] = Convert.ToString("Aptara");
                            dr[4] = Convert.ToString(ddlHidden.SelectedItem);
                            dr[5] = Convert.ToString(r.ReadandReplaceCharToHexa(AWName, InputXmlPath));
                            dr[6] = FormatDate(Convert.ToString(txtSrtDate.Text.Trim()));
                            dr[7] = FormatDate(Convert.ToString(txtAmendate.Text.Trim()));
                            dr[8] = FormatDate(Convert.ToString(txtEndDateDate.Text.Trim()));
                            dr[9] = Convert.ToString(r.ReadandReplaceCharToHexa("NONE", InputXmlPath));
                            CheckAwardBase(p_workflowid);
                            if (SharedObjects.IsAwardBaseFilled == false)
                                dr[10] = "0";
                            else if (SharedObjects.IsAwardBaseFilled == true)
                                dr[10] = "1";


                            dr[11] = SharedObjects.CurrentUrl;
                            dr[12] = txtFAID.Text.Trim();
                            //// New update on 14-Feb-2013 by Piyush //
                            dr[13] = Convert.ToString(txtRecordSource.Text.TrimStart().TrimEnd());
                            /////////////////////////////////////////////////////

                            #region //Added by avanish on 7-June-2018
                            dr[14] = FormatDate(Convert.ToString(txtAwardNoticeDate.Text.Trim()));
                            #endregion

                            #region //Added by avanish on 7-June-2018
                            dr[15] = FormatDate(Convert.ToString(txtPublishedDate.Text.Trim()));
                            #endregion

                            dtResult.Rows.Add(dr);

                            //comments By pankaj 25-june- 2018
                            DataSet ds = AwardDataOperations.SaveAwrad(dtResult);

                            DataSet dsresult = new DataSet();

                            foreach (DataGridViewRow row in dtGridKeywords.Rows)
                            {
                                string Keyword_Value = "";
                                DataTable dtResultLang = new DataTable();
                                //DataRow drLang;
                                int LangId = 0;
                                DataView dv;

                                int wId = Convert.ToInt32(SharedObjects.WorkId);
                                DataSet ds1 = AwardDataOperations.getWorkFlowDetails(wId);

                                dv = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                                dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangKeywords.SelectedValue) + "'";
                                LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);

                                string LANGUAGE_Code = row.Cells[2].Value.ToString();
                                Keyword_Value = row.Cells[1].Value.ToString();
                                if (r.chk_OtherLang(LANGUAGE_Code) == true && Keyword_Value != "")
                                {

                                    Keyword_Value = r.ConvertTextToUnicode(Keyword_Value);
                                }
                                if (Keyword_Value != "")
                                    dsresult = AwardDataOperations.SaveArrayKeywordswithlang(p_workflowid, p_insdel, LANGUAGE_Code, Keyword_Value);
                                MessageBox.Show(dsresult.Tables["ERRORCODE"].Rows[0][1].ToString(), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblMsg.Visible = true;
                            }
                            if (Convert.ToString(ds.Tables["ERRORCODE"].Rows[0][0]) == "0" || Convert.ToString(ds.Tables["ERRORCODE"].Rows[0][0]) == "1")
                            {
                                // ****** New update on 26-March-2014 by Harish ******
                                DataTable dtResultLang = new DataTable();
                                DataRow drLang;
                                int LangId = 0;
                                DataView dv;
                                DataView dv1;

                                int wId = Convert.ToInt32(SharedObjects.WorkId);
                                DataSet ds1 = AwardDataOperations.getWorkFlowDetails(wId);
                                DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(Convert.ToString(ds1.Tables["WFlowTable"].Rows[0]["ID"]), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));



                                dtResultLang.Columns.Add("TRAN_ID");
                                dtResultLang.Columns.Add("AW_ID");
                                dtResultLang.Columns.Add("COLUMN_DESC");
                                dtResultLang.Columns.Add("COLUMN_ID");
                                dtResultLang.Columns.Add("MODULEID");
                                dtResultLang.Columns.Add("LANGUAGE_ID");
                                dtResultLang.Columns.Add("TRAN_TYPE_ID");
                                dtResultLang.Columns.Add("FLAG_IN");
                                foreach (DataGridViewRow row in dtGridName.Rows)
                                {
                                    string aw_name = "";


                                    dv1 = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                                    string valueLang = row.Cells["language_code"].Value.ToString();

                                    dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                                    dv1.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(row.Cells["language_code"].Value.ToString().ToLower()) + "'";

                                    dv.RowFilter = "column_id='5'and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["awName"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'" + " and Language_code=" + "'" + valueLang + "'";


                                    if (dv.Count > 0)
                                    {
                                        UpdateAWNameTID = Convert.ToInt32(dv[0]["tran_id"]); //Convert.ToInt32(row.Cells["tran_id"].Value);
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);
                                    }
                                    else
                                    {
                                        UpdateAWNameTID = 0;
                                        LangId = Convert.ToInt32(dv1[0]["LANGUAGE_ID"]); ;
                                    }

                                    if (r.chk_OtherLang(valueLang) == true)
                                    {
                                        aw_name = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());
                                    }
                                    else
                                    {

                                        aw_name = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);

                                    }

                                    drLang = dtResultLang.NewRow();

                                    drLang[1] = ds1.Tables["WFlowTable"].Rows[0]["ID"];
                                    drLang[2] = aw_name;
                                    drLang[3] = 5;
                                    drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                    drLang[5] = LangId;
                                    drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                    if (UpdateAWNameTID > 0)
                                    {
                                        drLang[0] = UpdateAWNameTID;
                                        drLang[7] = 2;
                                    }
                                    else
                                    {
                                        drLang[0] = 0;
                                        drLang[7] = 1;
                                    };

                                    dtResultLang.Rows.Add(drLang);
                                }

                                // Abstract NAME
                                foreach (DataGridViewRow row in dtGridAbstract.Rows)
                                {
                                    string abs_value = "";
                                    dv1 = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                                    string valueLang = row.Cells["language_codes"].Value.ToString();
                                    dv1.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(row.Cells["language_codes"].Value.ToString().ToLower()) + "'";

                                    abs_value = row.Cells[1].Value.ToString();
                                    dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                                    dv.RowFilter = "column_id='6'and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["Abstract"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'" + " and Language_code=" + "'" + valueLang + "'";
                                    if (dv.Count > 0)
                                    {
                                        UpdateAWNameTID = Convert.ToInt32(dv[0]["tran_id"]); //Convert.ToInt32(row.Cells["tran_id"].Value);
                                        LangId = Convert.ToInt32(dv[0]["LANGUAGE_ID"]);
                                    }
                                    else
                                    {
                                        UpdateAWNameTID = 0;
                                        LangId = Convert.ToInt32(dv1[0]["LANGUAGE_ID"]);
                                    }


                                    if (r.chk_OtherLang(valueLang) == true)
                                    {
                                        abs_value = r.ConvertTextToUnicode(row.Cells[1].Value.ToString());
                                    }
                                    else
                                    {

                                        abs_value = r.ReadandReplaceCharToHexa(row.Cells[1].Value.ToString(), InputXmlPath);

                                    }

                                    drLang = dtResultLang.NewRow();

                                    drLang[1] = ds1.Tables["WFlowTable"].Rows[0]["ID"];
                                    drLang[2] = abs_value;
                                    drLang[3] = 6;
                                    drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                                    drLang[5] = LangId;
                                    drLang[6] = Convert.ToInt32(SharedObjects.TRAN_TYPE_ID);
                                    if (UpdateAWNameTID > 0)
                                    {
                                        drLang[0] = UpdateAWNameTID;
                                        drLang[7] = 2;
                                    }
                                    else
                                    {
                                        drLang[0] = 0;
                                        drLang[7] = 1;
                                    };

                                    dtResultLang.Rows.Add(drLang);
                                }
                                m_parent.GetProcess();



                                DataTable dt = ds.Tables["FundingBodyTable"];
                                DataTable dtCopy = dt.Copy();
                                DataSet dsFund = SharedObjects.StartWork;
                                if (dsOpptunity.Tables.Contains("FundingBodyTable"))
                                {
                                    dsOpptunity.Tables.Remove("FundingBodyTable");
                                }
                                if (dsOpptunity.Tables.Contains("Keywords"))
                                {
                                    dsOpptunity.Tables.Remove("Keywords");
                                }
                                dsOpptunity.Tables.Add(dtCopy);

                                if (dsresult.Tables.Contains("Keywords"))
                                {
                                    DataTable dt1 = dsresult.Tables["Keywords"];
                                    DataTable dtCopy1 = dt1.Copy();
                                    dsOpptunity.Tables.Add(dtCopy1);
                                }
                                SharedObjects.StartWork = dsOpptunity;

                                m_parent.GetProcess();
                                SharedObjects.IsAwardBaseFilled = true;
                            }

                            lblMsg.Visible = true;
                            lblMsg.Text = Convert.ToString(ds.Tables["ERRORCODE"].Rows[0][1]);

                            txtRecordSource.Text = url_txtLinkUrl.TrimStart().TrimEnd();

                            if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = Convert.ToString(ds.Tables["ERRORCODE"].Rows[0][1]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                    //pankaj
                    SharedObjects.AbstarctTitle2018 = "";
                    SharedObjects.AbstarctTitleLang = "";
                    SharedObjects.AwardTitle2018 = "";
                    SharedObjects.AwardTitleLang2018 = "";
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

        private void dtPickStart_ValueChanged(object sender, EventArgs e)
        {
            txtSrtDate.Text = dtPickStart.Text; lblMsg.Visible = false;
        }

        private void dtPickAmend_ValueChanged(object sender, EventArgs e)
        {
            txtAmendate.Text = dtPickAmend.Text; lblMsg.Visible = false;
        }

        private void dtPickEnd_ValueChanged(object sender, EventArgs e)
        {
            txtEndDateDate.Text = dtPickEnd.Text; lblMsg.Visible = false;
        }

        private void btnAmdClear_Click(object sender, EventArgs e)
        {
            txtAmendate.Text = ""; lblMsg.Visible = false;
        }

        private void btnEndClear_Click(object sender, EventArgs e)
        {
            txtEndDateDate.Text = ""; lblMsg.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSrtDate.Text = ""; lblMsg.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            lblMsg.Visible = false;
            txtRecordSource.Text = SharedObjects.CurrentUrl.TrimStart().TrimEnd();

        }

        private void dtPickAwardND_ValueChanged(object sender, EventArgs e)
        {
            txtAwardNoticeDate.Text = dtPickAwardND.Text; lblMsg.Visible = false;
        }

        private void btnAwardNDClear_Click(object sender, EventArgs e)
        {
            txtAwardNoticeDate.Text = ""; lblMsg.Visible = false;
        }
        private String FormatDate(String _Date)
        {
            try
            {
                DateTime Dt = new DateTime();

                IFormatProvider mFomatter = new System.Globalization.CultureInfo("en-US");

                Dt = Convert.ToDateTime(_Date);
                return Dt.ToString("dd-MMM-yyyy");
            }
            catch
            {
                return "";
            }
        }


        private void txtupdateKeywords_Click(object sender, EventArgs e)
        {


            string url_txtKeywords = txtKeywords.Text.TrimStart().TrimEnd();
            if (url_txtKeywords.Contains("http://") || url_txtKeywords.Contains("https://") || url_txtKeywords.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                string strKeyWords = txtKeywords.Text.ToString();
                string[] KeyWordsArr = strKeyWords.Split(',');



                foreach (string Keyword in KeyWordsArr)
                {
                    string p_DESCRIPTION = Keyword;

                    DataSet dsOpptunity = SharedObjects.StartWork;
                    string keywordid = Convert.ToString(dsOpptunity.Tables["keywords"].Rows[0]["KEYWORDS_id"]);
                    string keywords = Convert.ToString(dsOpptunity.Tables["keywords"].Rows[0]["AWARD_KEYWORDS"]);
                    int p_workflowid = Convert.ToInt32(SharedObjects.WorkId);


                    string[] KeyWordsolds = keywords.Split(',');
                    string p_DESCRIPTIONold = string.Empty;
                    foreach (string oldKeyword in KeyWordsolds)
                    {
                        p_DESCRIPTIONold = oldKeyword;
                    }

                    Int64 p_insdel = 2;

                    string p_LANG = ddlLangKeywords.Text.ToString();
                    DataSet dsresult = AwardDataOperations.updateKeywords(p_workflowid, p_insdel, p_LANG, p_DESCRIPTION, keywordid, p_DESCRIPTIONold);
                    txtKeywords.Text = Convert.ToString(dsresult.Tables["Keywords"].Rows[0]["AWARD_KEYWORDS"]).TrimStart().TrimEnd();
                    MessageBox.Show(dsresult.Tables["ERRORCODE"].Rows[0][1].ToString(), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }
        /// create by pankaj for v4 Date: 26-june-2018
        private void btndName_Click(object sender, EventArgs e)
        {
            //pankaj 11 june
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
                    string AWName = Regex.Replace(txtName.Text,@"[A-Za-z ]", "");

                    if ((pattern.Matches(AWName).Count > 0))
                    {
                        MessageBox.Show("Please enter valid Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtName.Text == "")
                    {
                        MessageBox.Show("Please enter Award Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlLangAwName.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dtGridName.Rows)
                        { string langValue = row.Cells["language_code"].Value.ToString();
                            if (langValue.ToLower() == Convert.ToString(ddlLangAwName.SelectedValue))
                            {
                                MessageBox.Show("Award Language can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }


                        //pankaj--24Dec 2018



                        string ddlContextID = Convert.ToString(ddlLangAwName.SelectedValue);
                        string firstColum = Convert.ToString(0);
                        string secondColum = txtName.Text.TrimStart().TrimEnd();

                        string thirdColum = ddlContextID.ToLower();
                        string[] rowGrid = { firstColum, secondColum, thirdColum };
                        dtGridName.Rows.Add(rowGrid);

                        ddlLangAwName.SelectedIndex = 18;
                        txtName.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }
        /// create by pankaj for v4 Date: 26-june-2018
        private void btnAbstarct_Click(object sender, EventArgs e)
        {
            //pankaj 10 june
            string url_txtAbstarct = txtAbstarct.Text.TrimStart().TrimEnd();

            if (false)
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else

            {
                try
                {

                    if (txtAbstarct.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtAbstarct.Text.Trim(), "Abstract");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    lblMsg.Visible = false;
                    if (txtAbstarct.Text == "")
                    {
                        MessageBox.Show("Please enter Abstarct Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else if (ddlLangAbstract.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dtGridAbstract.Rows)
                        {
                            string langValue = row.Cells["language_codes"].Value.ToString();
                            if (langValue.ToLower() == Convert.ToString(ddlLangAbstract.SelectedValue))
                            {
                                MessageBox.Show("Abstarct Language can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        string ddlContextID = Convert.ToString(ddlLangAbstract.SelectedValue);
                        string firstColum = Convert.ToString(0);
                        string secondColum = txtAbstarct.Text.TrimStart().TrimEnd();
                        string thirdColum = ddlContextID.ToLower();
                        string[] rowGrid = { firstColum, secondColum, thirdColum };
                        dtGridAbstract.Rows.Add(rowGrid);

                        ddlLangAbstract.SelectedIndex = 18;
                        txtAbstarct.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }
        /// create by pankaj for v4 Date: 26-june-2018
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
                            if (langValue.ToLower() == Convert.ToString(ddlLangKeywords.SelectedValue))
                            {
                                MessageBox.Show("Keywords Language  can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
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

        private void dtGridName_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 3)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("AW_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODULEID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");
                DataSet dsOpptunity = SharedObjects.StartWork;

                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Award_ID"]);
                    DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());


                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value.ToString());
                        txtName.Text = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["awName"].Value.ToString());
                        ddlLangAwName.SelectedValue = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["language_code"].Value.ToString());
                        UpdateAWNameTID = Convert.ToInt32(cloTranID);

                    }
                    else
                    {
                        UpdateAWNameTID = 0;
                    }
                }



                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;
                if (UpdateAWNameTID > 0)
                {
                    drLang[0] = UpdateAWNameTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = AwardDataOperations.SaveAwardLang(dtDelLang);
                    if (Convert.ToString(dsLang.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        if (this.dtGridName.Rows[e.RowIndex].Index >= 0)
                        {
                            //pankaj
                            dtGridName.Rows.RemoveAt(this.dtGridName.Rows[e.RowIndex].Index);
                        }

                        lblMsg.Visible = true;
                        lblMsg.Text = "Award Name Deleted successfully.";
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = dsLang.Tables["ERRORCODE"].Rows[0][1].ToString();
                    }
                }
                else
                {
                    if (this.dtGridName.Rows[e.RowIndex].Index >= 0)
                    {
                        //pankaj
                        dtGridName.Rows.RemoveAt(this.dtGridName.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Award Name deleted successfully.";
                }
                dtGridName.Refresh();
            }
        }

        private void dtGridAbstract_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 3)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("AW_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODULEID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");
                DataSet dsOpptunity = SharedObjects.StartWork;
                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Award_ID"]);
                    DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());


                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridAbstract.Rows[e.RowIndex].Cells["Ab_tran_id"].Value.ToString());
                        txtAbstarct.Text = Convert.ToString(dtGridAbstract.Rows[e.RowIndex].Cells["Abstract"].Value.ToString());
                        ddlLangAbstract.SelectedValue = Convert.ToString(dtGridAbstract.Rows[e.RowIndex].Cells["language_codes"].Value.ToString());
                        UpdateAWAbstTID = Convert.ToInt32(cloTranID);


                        //pankaj 27 Dec
                        SharedObjects.AbstarctTitle2018 = txtAbstarct.Text;
                        SharedObjects.AbstarctTitleLang = ddlLangAbstract.SelectedValue.ToString();


                    }
                    else
                    {
                        UpdateAWAbstTID = 0;
                    }
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;
                if (UpdateAWAbstTID > 0)
                {
                    drLang[0] = UpdateAWAbstTID;
                    drLang[7] = 3;
                    dtDelLang.Rows.Add(drLang);
                    dsLang = AwardDataOperations.SaveAwardLang(dtDelLang);

                    if (this.dtGridAbstract.Rows[e.RowIndex].Index >= 0)
                    {
                        dtGridAbstract.Rows.RemoveAt(this.dtGridAbstract.Rows[e.RowIndex].Index);
                    }
                    lblMsg.Visible = true;
                    lblMsg.Text = "Abstract Deleted successfully.";
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = dsLang.Tables["ERRORCODE"].Rows[0][1].ToString();
                }

                dtGridAbstract.Refresh();
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

                DataSet dsLoadFundLang = AwardDataOperations.LoadLanguageData(Convert.ToString(SharedObjects.ID), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                DataView dv = new DataView(dsLoadFundLang.Tables["LanguageData"].Copy());

                #region

                DataTable dtResultLang = new DataTable();

                string LangId = "en";
                string awkeyword = "";

                int wId = Convert.ToInt32(SharedObjects.WorkId);
                DataSet ds1 = AwardDataOperations.getWorkFlowDetails(wId);


                LangId = Convert.ToString(dtGridKeywords.Rows[e.RowIndex].Cells["Keylanguage_code"].Value.ToString());
                awkeyword = Convert.ToString(dtGridKeywords.Rows[e.RowIndex].Cells["awkeyword"].Value.ToString());
                int cloTranID = Convert.ToInt32(dtGridKeywords.Rows[e.RowIndex].Cells["key_tran_id"].Value.ToString());

                dv.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(ddlLangKeywords.SelectedValue) + "'";

                dsLang = AwardDataOperations.SaveArrayKeywordswithlang(wId, 1, LangId, awkeyword, cloTranID);
                #endregion



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

        private void dtGridName_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 2 || e.ColumnIndex == 1)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("AW_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODULEID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");
                DataSet dsOpptunity = SharedObjects.StartWork;

                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Award_ID"]);
                    DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataSet dsLoadFundLang = AwardDataOperations.LoadLanguageData(Convert.ToString(SharedObjects.ID), Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));

                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());


                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value.ToString());
                        txtName.Text = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["awName"].Value.ToString());
                        ddlLangAwName.SelectedValue = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["language_code"].Value.ToString());
                        UpdateAWNameTID = Convert.ToInt32(cloTranID);
                        //pankaj 26 dec

                        SharedObjects.AwardTitle2018 = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["Name_tran_id"].Value);

                    }
                    else
                    {
                        UpdateAWNameTID = 0;
                    }
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["awName"].Value.ToString()); ;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;
                dtGridName.Refresh();
            }
        }

        private void dtGridAbstract_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            if (e.ColumnIndex == 2 || e.ColumnIndex == 1)
            {
                dtDelLang.Columns.Add("TRAN_ID");
                dtDelLang.Columns.Add("AW_ID");
                dtDelLang.Columns.Add("COLUMN_DESC");
                dtDelLang.Columns.Add("COLUMN_ID");
                dtDelLang.Columns.Add("MODULEID");
                dtDelLang.Columns.Add("LANGUAGE_ID");
                dtDelLang.Columns.Add("TRAN_TYPE_ID");
                dtDelLang.Columns.Add("FLAG_IN");
                DataSet dsOpptunity = SharedObjects.StartWork;

                if ((dsOpptunity.Tables["FundingBodyTable"].Rows.Count) > 0)
                {
                    string oppID = Convert.ToString(dsOpptunity.Tables["FundingBodyTable"].Rows[0]["Award_ID"]);
                    DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(oppID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                    DataView dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());


                    if (dv.Count > 0)
                    {
                        int cloTranID = Convert.ToInt32(dtGridAbstract.Rows[e.RowIndex].Cells["Ab_tran_id"].Value.ToString());
                        txtAbstarct.Text = Convert.ToString(dtGridAbstract.Rows[e.RowIndex].Cells["Abstract"].Value.ToString());
                        ddlLangAbstract.SelectedValue = Convert.ToString(dtGridAbstract.Rows[e.RowIndex].Cells["language_codes"].Value.ToString());
                        UpdateAWAbstTID = Convert.ToInt32(cloTranID);

                        SharedObjects.AbstarctTitle2018 = cloTranID.ToString();
                    }
                    else
                    {
                        UpdateAWAbstTID = 0;
                    }
                }

                drLang = dtDelLang.NewRow();
                drLang[1] = null;
                drLang[2] = null;
                drLang[3] = null;
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId);
                drLang[5] = null;
                drLang[6] = null;
                dtGridAbstract.Refresh();
            }
        }

        private void CheckAwardBase(int wfid)
        {
            try
            {
                DataSet awbaseDS = new DataSet();
                awbaseDS = AwardDataOperations.GetAwardBase(wfid);
                if (awbaseDS.Tables["Progress"].Rows.Count > 0)
                {

                    SharedObjects.IsAwardBaseFilled = true;
                }
                else
                {
                    SharedObjects.IsAwardBaseFilled = false;
                }
            }
            catch (Exception ex)
            {

            }


        }

        private void btnawtitleupdate_Click(object sender, EventArgs e)
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
                aw_upatename = txtName.Text.ToString();
                txnupdateID = SharedObjects.AwardTitle2018.ToString();
                awlanguageID = ddlLangAwName.SelectedValue.ToString();
                DataSet updatetile = new DataSet();
                if (txnupdateID.Length > 0 && aw_upatename.Length > 0)
                {
                    updatetile = AwardDataOperations.updatetitlebytxnid(Convert.ToInt64(txnupdateID), aw_upatename);

                    if (Convert.ToString(updatetile.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {

                        lblMsg.Visible = true;
                        lblMsg.Text = "update successfully";
                        string awID = SharedObjects.AbstarctTitleLang.ToString();
                        DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(awID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        DataView dv;
                        // Award NAME
                        dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                        dv.RowFilter = "column_id='5'";
                        dtGridName.DataSource = null;
                        dtGridName.Rows.Clear();
                        //dtGridName.Columns.Clear();
                        dtGridName.Refresh();

                        if (dv.Count > 0)
                        {

                            for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                            {
                                string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath)).TrimStart().TrimEnd();

                                //pankaj WieredChar managed mechanism 

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
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

                        //Pankaj start track TrackUnstoppedAward
                        if (updatetile.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        }
                        //End track TrackUnstoppedAward
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = updatetile.Tables["ERRORCODE"].Rows[0][1].ToString();
                    }
                }
                else
                {
                }

                txtName.Text = "";
                SharedObjects.AwardTitle2018 = "";
            }
        }



        private void btnabsupdate_Click(object sender, EventArgs e)
        {
            string url_txtAbstarct = txtAbstarct.Text.TrimStart().TrimEnd();
            if (false)
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {

                string aw_upateAbs = string.Empty;
                string txnupdateID = string.Empty;
                string awlanguageID = string.Empty;
                aw_upateAbs = txtAbstarct.Text.ToString();
                txnupdateID = SharedObjects.AbstarctTitle2018.ToString();
                awlanguageID = ddlLangAbstract.SelectedValue.ToString();
                DataSet updatetile = new DataSet();
                if (txnupdateID.Length > 0 && aw_upateAbs.Length > 0)
                {
                    updatetile = AwardDataOperations.updatetitlebytxnid(Convert.ToInt64(txnupdateID), aw_upateAbs);

                    if (Convert.ToString(updatetile.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {

                        lblMsg.Visible = true;
                        lblMsg.Text = "update successfully";
                        string awID = SharedObjects.AbstarctTitleLang.ToString();
                        DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(awID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                        DataView dv;
                        // Award NAME
                        dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                        dv.RowFilter = "column_id='6'";
                        dtGridAbstract.DataSource = null;
                        dtGridAbstract.Rows.Clear();
                        dtGridAbstract.Refresh();

                        if (dv.Count > 0)
                        {

                            for (int iAbbr = 0; iAbbr < dv.Count; iAbbr++)
                            {
                                string firstCol = Convert.ToString(dv[iAbbr]["tran_id"]);
                                string secondCol = Convert.ToString(r.ReadandReplaceHexaToChar(dv[iAbbr]["COLUMN_DESC"].ToString(), InputXmlPath).TrimStart().TrimEnd());
                                //pankaj WieredChar managed mechanism 

                                string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(dv[iAbbr]["COLUMN_DESC"].ToString()));
                                if (UpdateFunding_difflang != "")
                                {
                                    secondCol = UpdateFunding_difflang;
                                }
                                string thirdCol = Convert.ToString(dv[iAbbr]["language_code"].ToString().ToLower());
                                string[] row = { firstCol, secondCol, thirdCol };
                                if (dv.Count == 1 && secondCol == "")
                                { }
                                else
                                {
                                    dtGridAbstract.Rows.Add(row);
                                }
                            }

                        }
                        //Pankaj start track TrackUnstoppedAward
                        if (updatetile.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        }
                        //End track TrackUnstoppedAward
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = updatetile.Tables["ERRORCODE"].Rows[0][1].ToString();
                    }
                }
                else
                {
                }

                txtAbstarct.Text = "";
                SharedObjects.AbstarctTitle2018 = "";
            }
        }

        private void dtGridAbstract_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dtPickPubDate_ValueChanged(object sender, EventArgs e)
        {
            txtPublishedDate.Text = dtPickPubDate.Text; lblMsg.Visible = false;
        }

        private void btnPubDateClear_Click(object sender, EventArgs e)
        {
            txtPublishedDate.Text = ""; lblMsg.Visible = false;
        }
        #endregion
    }
}