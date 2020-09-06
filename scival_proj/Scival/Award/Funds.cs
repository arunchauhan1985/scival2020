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
namespace Scival.Award
{
    public partial class Funds : UserControl
    {

        Replace r = new Replace();
        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");
        private Awards m_parent;
        int UpdateAWNameTID = 0;
        Int64 WFID = 0;
        Int64 pagemode = 0;
        Int64 mode = 0;
        DataSet dsresult;
        DataSet dsFunds = new DataSet();
        public String FormName = String.Empty;
        ErrorLog oErrorLog = new ErrorLog();
        string InputXmlPath = string.Empty;

        string SEQUENCE_ID = string.Empty;

        void ddlCuurency_f_MouseWheel(object sender, MouseEventArgs e)
        {

            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlCuurency_h_MouseWheel(object sender, MouseEventArgs e)
        {

            ((HandledMouseEventArgs)e).Handled = true;
        }


        void DDLCOUNTRY_MouseWheel(object sender, MouseEventArgs e)
        {

            ((HandledMouseEventArgs)e).Handled = true;
        }

        public Funds(Awards frm)
        {
            InitializeComponent();

            m_parent = frm;
            loadIniitialValua();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void loadIniitialValua()
        {
            try
            {

                SharedObjects.TotalAmountChangedValue = "";

                WFID = SharedObjects.WorkId; ;
                lblMsg.Visible = false;
                FormName = SharedObjects.FundingClickPage;
                this.Controls.Clear();
                this.InitializeComponent();
                pagemode = 4;
                dtPickStart.Text = DateTime.Now.ToString();
                dtPickEnd.Text = DateTime.Now.ToString();
                DataSet dsItems = AwardDataOperations.GetAmount(WFID, pagemode);
                dsFunds = AwardDataOperations.Getfunding_details(WFID);
                
                    if (dsFunds.Tables["Funds"].Rows.Count > 0)
                    {
                        DataTable dtFunds = dsFunds.Tables["Funds"].Copy();
                        txtPostalCode.Text = dtFunds.Rows[0]["POSTALCODE"].ToString();
                        txtpostofficeboxn.Text = dtFunds.Rows[0]["POSTOFFICEBOXNUMBER"].ToString();
                        txt_locality.Text = dtFunds.Rows[0]["LOCALITY"].ToString();
                        txtregion.Text = dtFunds.Rows[0]["REGION"].ToString();
                        TextStreet.Text = dtFunds.Rows[0]["STREET"].ToString();

                        txt_fundingBodyProjectId_f.Text = dtFunds.Rows[0]["FUND_ID"].ToString();
                        txt_fundingBodyProjectId_h.Text = dtFunds.Rows[0]["FUNDINGBODYPROJECTID"].ToString();
                        txt_Amount_f.Text = dtFunds.Rows[0]["BUDGET_AMOUNT"].ToString();
                        txt_Amount_h.Text = dtFunds.Rows[0]["AMOUNT"].ToString();
                        ddlCuurency_f.SelectedValue = dtFunds.Rows[0]["BUDGET_CURRENCY"].ToString(); ;
                        ddlCuurency_h.SelectedValue = dtFunds.Rows[0]["CURRENCY"].ToString();

                        txt_acronym.Text = dtFunds.Rows[0]["ACRONYM"].ToString();
                        txtEndDateDate.Text = dtFunds.Rows[0]["ENDDATE"].ToString();
                        txtSrtDate.Text = dtFunds.Rows[0]["STARTDATE"].ToString();
                        txtStatus.Text = dtFunds.Rows[0]["STATUS"].ToString();
                        txt_link.Text = dtFunds.Rows[0]["LINK"].ToString();
                    }
                

                DataTable temp = dsItems.Tables["Currency"].Copy();

                DataRow dr = temp.NewRow();
                dr["Code"] = "SelectCurrency";
                dr["Value"] = "--Select Currency--";
                temp.Rows.InsertAt(dr, 0);

                ddlCuurency_f.DataSource = temp;
                ddlCuurency_f.DisplayMember = "Value";
                ddlCuurency_f.ValueMember = "Code";

                ddlCuurency_h.DataSource = temp;
                ddlCuurency_h.DisplayMember = "Value";
                ddlCuurency_h.ValueMember = "Code";


                DataSet dsFunding = SharedObjects.StartWork;
                if (dsFunding != null)
                {
                    // Fill Country Dropdown
                    DataTable temp_country = dsFunding.Tables["Country"].Copy();

                    DataRow drc = temp_country.NewRow();
                    drc["LCODE"] = "SelectCountry";
                    drc["NAME"] = "--Select Country--";
                    temp_country.Rows.InsertAt(drc, 0);

                    DDLCOUNTRY.DataSource = temp_country;
                    DDLCOUNTRY.ValueMember = "LCODE";
                    DDLCOUNTRY.DisplayMember = "NAME";
                    DataTable tempAWName = dsFunding.Tables["LanguageTable"].Copy();
                    DataRow drl = tempAWName.NewRow();
                    drl = tempAWName.NewRow();
                    drl["LANGUAGE_CODE"] = "SelectLanguage";
                    drl["LANGUAGE_NAME"] = "--Select Language--";
                    tempAWName.Rows.InsertAt(drl, 0);

                    ddlLangAwName.DataSource = tempAWName;
                    ddlLangAwName.ValueMember = "LANGUAGE_CODE";
                    ddlLangAwName.DisplayMember = "LANGUAGE_NAME";
                    ddlLangAwName.SelectedIndex = 18;

                }
                string awID = Convert.ToString(dsFunding.Tables["FundingBodyTable"].Rows[0]["AWARD_ID"]);
                SharedObjects.AbstarctTitleLang = awID.ToString();
                dtGridName.Visible = true;
                DataSet dsLoadOppLang = AwardDataOperations.LoadLanguageData(awID, Convert.ToInt32(SharedObjects.ModuleId), Convert.ToInt32(SharedObjects.TRAN_TYPE_ID));
                DataView dv;
                // Award NAME
                dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                dv.RowFilter = "column_id='8'";
                if (dv.Count > 0)
                {

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
                }
            }
            catch
            {
            }

        }

        private void txtSubmit_Click(object sender, EventArgs e)
        {

            string url_txtPostalCode = txtPostalCode.Text.TrimStart().TrimEnd();
            string url_txtpostofficeboxn = txtpostofficeboxn.Text.TrimStart().TrimEnd();
            string url_txt_locality = txt_locality.Text.TrimStart().TrimEnd();
            string url_txtregion = txtregion.Text.TrimStart().TrimEnd();
            string url_TextStreet = TextStreet.Text.TrimStart().TrimEnd();

            string url_txt_fundingBodyProjectId_f = txt_fundingBodyProjectId_f.Text.TrimStart().TrimEnd();
            string url_txt_fundingBodyProjectId_h = txt_fundingBodyProjectId_h.Text.TrimStart().TrimEnd();
            string url_txt_Amount_f = txt_Amount_f.Text.TrimStart().TrimEnd();
            string url_txt_Amount_h = txt_Amount_h.Text.TrimStart().TrimEnd();
            string url_ddlCuurency_f = Convert.ToString(ddlCuurency_f.SelectedValue);
            string url_ddlCuurency_h = Convert.ToString(ddlCuurency_h.SelectedValue);

            string url_txt_acronym = txt_acronym.Text.TrimStart().TrimEnd();
            string url_txtEndDateDate = txtEndDateDate.Text.TrimStart().TrimEnd();
            string url_txtSrtDate = txtSrtDate.Text.TrimStart().TrimEnd();
            string url_txtStatus = txtStatus.Text.TrimStart().TrimEnd();
            string url_txt_link = txt_link.Text.TrimStart().TrimEnd();

            string url_ddlLangAwName = Convert.ToString(ddlLangAwName.SelectedValue);

            string url_txtName = txtName.Text.TrimStart().TrimEnd();

            string url_country = Convert.ToString(DDLCOUNTRY.SelectedValue);



            if (
            url_txtPostalCode.Contains("http://") || url_txtpostofficeboxn.Contains("http://") ||
            url_txtPostalCode.Contains("https://") || url_txtpostofficeboxn.Contains("https://") ||
            url_txtPostalCode.Contains("www.") || url_txtpostofficeboxn.Contains("www.") ||

               url_txt_locality.Contains("http://") || url_txtregion.Contains("http://") ||
            url_txt_locality.Contains("https://") || url_txtregion.Contains("https://") ||
            url_txt_locality.Contains("www.") || url_txtregion.Contains("www.") ||

               url_TextStreet.Contains("http://") || url_txt_fundingBodyProjectId_f.Contains("http://") ||
            url_TextStreet.Contains("https://") || url_txt_fundingBodyProjectId_f.Contains("https://") ||
            url_TextStreet.Contains("www.") || url_txt_fundingBodyProjectId_f.Contains("www.") ||

               url_txt_fundingBodyProjectId_h.Contains("http://") || url_txt_Amount_f.Contains("http://") ||
            url_txt_fundingBodyProjectId_h.Contains("https://") || url_txt_Amount_f.Contains("https://") ||
            url_txt_fundingBodyProjectId_h.Contains("www.") || url_txt_Amount_f.Contains("www.") ||

               url_txt_Amount_f.Contains("http://") || url_txt_Amount_h.Contains("http://") ||
            url_txt_Amount_f.Contains("https://") || url_txt_Amount_h.Contains("https://") ||
            url_txt_Amount_f.Contains("www.") || url_txt_Amount_h.Contains("www.") ||

               url_txtStatus.Contains("http://") || url_txtName.Contains("http://") ||
            url_txtStatus.Contains("https://") || url_txtName.Contains("https://") ||
            url_txtStatus.Contains("www.") || url_txtName.Contains("www."))

            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                try
                {
                    lblMsg.Visible = false;


                    if (ddlLangAwName.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else if (ddlCuurency_f.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Currency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlCuurency_h.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Currency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (DDLCOUNTRY.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        dsresult = AwardDataOperations.SaveFunds(url_txt_fundingBodyProjectId_f, url_txt_Amount_f, url_ddlCuurency_f, url_txt_fundingBodyProjectId_h, url_txt_Amount_h, url_ddlCuurency_h, url_txt_acronym, Convert.ToDateTime(url_txtSrtDate), Convert.ToDateTime(url_txtEndDateDate), url_txtStatus, url_txt_link, url_txtpostofficeboxn, url_TextStreet,
                          url_txt_locality, url_txtregion, url_txtPostalCode, url_country, ddlLangAwName.SelectedValue.ToString(), 8, 0, txtName.Text.ToString(), 8, WFID, 0, 0);

                       
                    }
                    if (dtGridName.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dtGridName.Rows)
                        {
                            string aw_name = "";
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

                            DataSet dsOpptunity = SharedObjects.StartWork;
                            dv1 = new DataView(dsOpptunity.Tables["LanguageTable"].Copy());
                            string valueLang = row.Cells["language_code"].Value.ToString();

                            dv = new DataView(dsLoadOppLang.Tables["LanguageData"].Copy());
                            dv1.RowFilter = "LANGUAGE_CODE='" + Convert.ToString(row.Cells["language_code"].Value.ToString().ToLower()) + "'";
                            dv.RowFilter = "column_id='8'and column_desc='" + r.ReadandReplaceCharToHexa(row.Cells["awName"].Value.ToString().Replace("'", "''"), InputXmlPath) + "'" + " and Language_code=" + "'" + valueLang + "'";
                            if (dv.Count > 0)
                            {
                                UpdateAWNameTID = Convert.ToInt32(dv[0]["tran_id"]);
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
                            drLang[3] = 8;
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

                            DataSet dsLang = AwardDataOperations.SaveAwardLang(dtResultLang);
                        }

                    }

                }

                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }



        }

        private void dtPickStart_ValueChanged(object sender, EventArgs e)
        {
            txtSrtDate.Text = dtPickStart.Text; lblMsg.Visible = false;
        }

        private void dtPickEnd_ValueChanged(object sender, EventArgs e)
        {
            txtEndDateDate.Text = dtPickEnd.Text; lblMsg.Visible = false;
        }

        private void btndName_Click(object sender, EventArgs e)
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
                    string AWName = Regex.Replace(txtName.Text,@"[A-Za-z ]", "");

                    if ((pattern.Matches(AWName).Count > 0))
                    {
                        MessageBox.Show("Please enter valid title.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Please enter title.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlLangAwName.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dtGridName.Rows)
                        {

                            string langValue = row.Cells["language_code"].Value.ToString();
                            if (langValue.ToLower() == Convert.ToString(ddlLangAwName.SelectedValue))
                            {
                                MessageBox.Show("Award Language can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }


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
                        dv.RowFilter = "column_id='8'";
                        dtGridName.DataSource = null;
                        dtGridName.Rows.Clear();
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
                        
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
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
    }
}
