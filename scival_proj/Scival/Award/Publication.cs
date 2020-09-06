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
    public partial class Publication : UserControl
    {
        private Awards m_parent;
        Replace r = new Replace();
        Int64 WfId = 0;
        int rowindex = 0;
        int mode = 0;
        static Int64 grdrowid = 0;
        int UpdateAWNameTID = 0;
        DataTable displayData = new DataTable();
        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");
        ErrorLog oErrorLog = new ErrorLog();
        public Publication(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            loadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
        }

        void ddlItem_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlRelType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        private void loadInitialValue()
        {

            try
            {
                lblMsg.Visible = false;
                WfId = SharedObjects.WorkId;
                DataSet dsOpptunity = SharedObjects.StartWork;
                DataSet dsresult = AwardDataOperations.Save_Update_PublicationData(WfId, 4);
                DataSet dsresult_Title = AwardDataOperations.SavePublication_Title(WfId, 4);
                displayData = dsresult.Tables["PublicationData"].Copy();
                // Fill Language Dropdown
                DataTable tempAWName = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr = tempAWName.NewRow();
                dr = tempAWName.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";
                tempAWName.Rows.InsertAt(dr, 0);

                ddlLangAwName.DataSource = tempAWName;
                ddlLangAwName.ValueMember = "LANGUAGE_CODE";
                ddlLangAwName.DisplayMember = "LANGUAGE_NAME";
                ddlLangAwName.SelectedIndex = 18;


                    txtFundProjID.Text = Convert.ToString(displayData.Rows[0]["FUNDINGBODYPROJECTID"]);
                    txtPublished_Date.Text = Convert.ToString(displayData.Rows[0]["PUBLISHEDDATE"]);
                    txtPublicationURL.Text = Convert.ToString(displayData.Rows[0]["PUBLICATION_URL"]);
                    txtpublicationOutputId.Text = Convert.ToString(displayData.Rows[0]["PUBLICATIONOUTPUTID"]);
                    txtIngestionID.Text = Convert.ToString(displayData.Rows[0]["INGESTIONID"]);
                    txtJournal_Title.Text = Convert.ToString(displayData.Rows[0]["TITLE"]);
                    txtJournal_Identifier.Text = Convert.ToString(displayData.Rows[0]["JOURNAL_IDENTIFIER"]);
                    txtAuthors.Text = Convert.ToString(displayData.Rows[0]["PUBLICATION_AUTHOR"]);
                    txtDescription.Text = Convert.ToString(displayData.Rows[0]["PUB_DESCRIPTION"]);
                

                #region 
                for (int iAbbr = 0; iAbbr < dsresult_Title.Tables["PublicationTitle"].Rows.Count; iAbbr++)
                {
                    string firstCol = Convert.ToString(dsresult_Title.Tables["PublicationTitle"].Rows[iAbbr]["PUBLICATION_ID"]);
                    string secondCol = Convert.ToString(dsresult_Title.Tables["PublicationTitle"].Rows[iAbbr]["Title"].ToString());
                    string thirdCol = Convert.ToString(dsresult_Title.Tables["PublicationTitle"].Rows[iAbbr]["Lang"].ToString());

                    string UpdateFunding_difflang = Convert.ToString(r.Return_WieredChar_Original(secondCol));
                    if (UpdateFunding_difflang != "")
                    {
                        secondCol = UpdateFunding_difflang;
                    }
                    string[] row = { firstCol, secondCol, thirdCol };

                    dtGridName.Rows.Add(row);

                }
                #endregion
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
                DataTable DT = displayData.Copy();
                if (DT.Rows.Count > 0)
                {
                    mode = 2;
                }
                else
                {
                    grdRelOrgnorecord();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdRelOrgnorecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("ID");
                dtNoRcrd.Columns.Add("RELType");
                dtNoRcrd.Columns.Add("Type");
                dtNoRcrd.Columns.Add("DOI");
                dtNoRcrd.Columns.Add("PUBMEDID");
                dtNoRcrd.Columns.Add("PMCID");
                dtNoRcrd.Columns.Add("MEDLINEID");
                dtNoRcrd.Columns.Add("SCOPUSID");
                dtNoRcrd.Columns.Add("ITEMID_COLUMN");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";
                dtNoRcrd.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {


            //pankaj 11 june
            string url_txtItem = txtAuthors.Text.TrimStart().TrimEnd();
            string url_txtPubmed = txtpublicationOutputId.Text.TrimStart().TrimEnd();
            string url_txtPmc = txtIngestionID.Text.TrimStart().TrimEnd();
            string url_txtMedline = txtJournal_Title.Text.TrimStart().TrimEnd();
            string url_txtScopus = txtJournal_Identifier.Text.TrimStart().TrimEnd();

            if (url_txtItem.Contains("http://") || url_txtPubmed.Contains("http://") || url_txtPmc.Contains("http://") || url_txtMedline.Contains("http://") || url_txtScopus.Contains("http://") ||
                url_txtItem.Contains("https://") || url_txtPubmed.Contains("https://") || url_txtPmc.Contains("https://") || url_txtMedline.Contains("https://") || url_txtScopus.Contains("https://") ||
                url_txtItem.Contains("www.") || url_txtPubmed.Contains("www.") || url_txtPmc.Contains("www.") || url_txtMedline.Contains("www.") || url_txtScopus.Contains("www.")
                )
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string hierarchyText = "";
                    string reltypeText = "";
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (txtAuthors.Text.Trim() == "" && txtPublicationURL.Text.Trim() == "" && txtpublicationOutputId.Text.Trim() == "" && txtIngestionID.Text.Trim() == "" && txtJournal_Title.Text.Trim() == "" && txtJournal_Identifier.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter atleast one Id.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        try
                        {
                            Int64 xxx = Convert.ToInt64(txtpublicationOutputId.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Please Enter Numeric Value in publicationOutputId");
                            return;
                        }


                        DataSet dsresult = AwardDataOperations.Save_Update_PublicationData(WfId, mode, txtFundProjID.Text.ToString().Trim(), txtPublished_Date.Text.ToString(),
                            txtPublicationURL.Text.ToString(), Convert.ToInt64(txtpublicationOutputId.Text), txtIngestionID.Text, txtJournal_Title.Text, txtJournal_Identifier.Text,
                            txtAuthors.Text, txtDescription.Text);

                        string x = "";
                        foreach (DataGridViewRow row in dtGridName.Rows)
                        {
                            string Title = "";
                            DataTable dtResultLang = new DataTable();
                            int LangId = 0;
                            DataView dv;

                            Int64 wId = Convert.ToInt32(SharedObjects.WorkId);
                            DataSet ds1 = AwardDataOperations.getWorkFlowDetails(wId);

                            string LANGUAGE_Code = row.Cells[2].Value.ToString();
                            Title = row.Cells[1].Value.ToString();

                            if (Title != "")
                                dsresult = AwardDataOperations.SavePublication_Title(wId, 0, LANGUAGE_Code, Title);
                            MessageBox.Show("Error", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblMsg.Visible = true;
                        }
                        if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Publication");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                        }
                        lblMsg.Visible = true;
                        //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                        //Pankaj start track TrackUnstoppedAward
                        //if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        //{
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        //}
                        //End track TrackUnstoppedAward
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void grdClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (displayData.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        btnsave.Visible = false;
                        btnupdat.Visible = true;
                        btncancel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdClass_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (displayData.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = AwardDataOperations.SaveAbdUpdateScholarlyOutputType_new(SharedObjects.WorkId, 1, Convert.ToInt64(0), "", "", "", "", "", "", "", "");
                           
                                displayData = dsresult.Tables["ResearchOutcome"].Copy();
                                if (dsresult.Tables["ResearchOutcome"].Rows.Count > 0)
                                {
                                    BindGrid();
                                }
                                else
                                {
                                    grdRelOrgnorecord();
                                    mode = 0;

                                }
                            
                            m_parent.GetProcess();
                            lblMsg.Visible = true;
                            //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                            txtAuthors.Text = "";
                            btnsave.Visible = true;
                            btnupdat.Visible = false;
                            btncancel.Visible = false;
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

        private void btnupdat_Click(object sender, EventArgs e)
        {

            //pankaj 11 june
            string url_txtItem = txtAuthors.Text.TrimStart().TrimEnd();
            string url_txtDoi = txtPublicationURL.Text.TrimStart().TrimEnd();
            string url_txtPubmed = txtpublicationOutputId.Text.TrimStart().TrimEnd();
            string url_txtPmc = txtIngestionID.Text.TrimStart().TrimEnd();
            string url_txtMedline = txtJournal_Title.Text.TrimStart().TrimEnd();
            string url_txtScopus = txtJournal_Identifier.Text.TrimStart().TrimEnd();
            if (url_txtItem.Contains("http://") || url_txtDoi.Contains("http://") || url_txtPubmed.Contains("http://") || url_txtPmc.Contains("http://") || url_txtMedline.Contains("http://") || url_txtScopus.Contains("http://") ||
              url_txtItem.Contains("https://") || url_txtDoi.Contains("https://") || url_txtPubmed.Contains("https://") || url_txtPmc.Contains("https://") || url_txtMedline.Contains("https://") || url_txtScopus.Contains("https://") ||
              url_txtItem.Contains("www.") || url_txtDoi.Contains("www.") || url_txtPubmed.Contains("www.") || url_txtPmc.Contains("www.") || url_txtMedline.Contains("www.") || url_txtScopus.Contains("www.")
              )
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string hierarchyText = "";
                    string reltypeText = "";
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");
                    if (txtAuthors.Text.Trim() == "" && txtPublicationURL.Text.Trim() == "" && txtpublicationOutputId.Text.Trim() == "" && txtIngestionID.Text.Trim() == "" && txtJournal_Title.Text.Trim() == "" && txtJournal_Identifier.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter atleast one Id.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string Protext = txtAuthors.Text.Trim();
                        DataSet dsresult = AwardDataOperations.SaveAbdUpdateScholarlyOutputType_new(WfId, mode, grdrowid,
                                       hierarchyText, reltypeText,
                                        txtPublicationURL.Text.Trim(), txtpublicationOutputId.Text.Trim(), txtIngestionID.Text.Trim(), txtJournal_Title.Text.Trim(),
                                        txtJournal_Identifier.Text.Trim(), txtAuthors.Text.Trim());

                        
                            displayData = dsresult.Tables["researchOutcome"].Copy();
                            if (dsresult.Tables["researchOutcome"].Rows.Count > 0)
                            {
                                BindGrid();
                            }
                            else
                            {
                                grdRelOrgnorecord();
                                mode = 0;
                            }
                            txtAuthors.Text = "";
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Research Outcome");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion

                        
                        lblMsg.Visible = true;
                        //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();


                        //Pankaj start track TrackUnstoppedAward
                       
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
                        //End track TrackUnstoppedAward
                        btnsave.Visible = true;
                        btnupdat.Visible = false;
                        btncancel.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                btnsave.Visible = true;


                btnupdat.Visible = false;
                btncancel.Visible = false;



                txtAuthors.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

        }

        private void btnsave_Click_1(object sender, EventArgs e)
        {

        }

        private void dtPickStart_ValueChanged(object sender, EventArgs e)
        {
            txtPublished_Date.Text = dtPickStart.Text; lblMsg.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPublished_Date.Text = ""; lblMsg.Visible = false;
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

        private void dtGridName_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow drLang;
            DataTable dtDelLang = new DataTable();
            DataSet dsLang = new DataSet();

            try
            {

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
                            txtName.Text = Convert.ToString(dtGridName.Rows[e.RowIndex].Cells["Title"].Value.ToString());
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
                            lblMsg.Text = "Publication Name Deleted successfully.";
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtGridName_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                drLang[4] = Convert.ToInt32(SharedObjects.ModuleId)   ;
                drLang[5] = null;
                drLang[6] = null;

                dtGridName.Refresh();
            }
        }

        private void btnupdat_Click_1(object sender, EventArgs e)
        {

        }

        private void txtpublicationOutputId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }









    }
}
