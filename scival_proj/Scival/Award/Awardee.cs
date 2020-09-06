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
    public partial class Awardee : UserControl
    {
        private Awards m_parent;
        Int64 WfId = 0;
        DataTable dtAwadee = new DataTable();
        DataTable dtAffiliation = new DataTable();
        DataTable dtFax = new DataTable();
        DataTable dtTelephones = new DataTable();
        DataTable dtAddress = new DataTable();
        DataTable dtddlType = new DataTable();
        Regex pattern = new Regex(@"([?]|[#]|[*]|[<]|[>])");
        int rowindex = 0;
        int affiliationID = 0;
        int awadeeID = 0;
        ErrorLog oErrorLog = new ErrorLog();
        bool FromAffiliationGrid = false;
        bool FromAffDtlEdit = false;
        bool FromAffEdit = false;

        string Initial = string.Empty;
        string InitialDeatils = string.Empty;

        #region Schema5.0 Changes
        string activityType = string.Empty;
        string awardeeAffiliationId = string.Empty;
        string departmentName = string.Empty;
        string fundingBodyOrganizationId = string.Empty;
        string link = string.Empty;
        string vatNumber = string.Empty;
        string name = string.Empty;
        string role = string.Empty;
        string DUNS = string.Empty;
        string ROR = string.Empty;
        string WIKIDATA = string.Empty;

        string awardeePersonId = string.Empty;
        string emailAddress = string.Empty;
        string familyName = string.Empty;
        string fundingBodyPersonId = string.Empty;
        string givenName = string.Empty;
        string ORCID_Aff = string.Empty;
        string initials_Aff = string.Empty;
        string role_Aff = string.Empty;
        string name_Aff = string.Empty;


        #endregion




        public Awardee(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitialValue();
            DataSet dsFunding = SharedObjects.StartWork;
            if (SharedObjects.StartWork != null)
            {
               DataTable temp = dsFunding.Tables["Country"];

                DataRow dr = temp.NewRow();
                dr["LCODE"] = "SelectCountry";
                dr["NAME"] = "--Select Country--";
                temp.Rows.InsertAt(dr, 0);

                DDLCOUNTRY.DataSource = temp;
                DDLCOUNTRY.ValueMember = "LCODE";
                DDLCOUNTRY.DisplayMember = "NAME";

            }
            SharedObjects.DefaultLoad = "";
            SharedObjects.MultipleInitial = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        private void LoadInitialValue()
        {
            try
            {
                lblMsg.Visible = false;
                WfId = SharedObjects.WorkId;
                DataSet dsOpptunity = SharedObjects.StartWork;
                DataSet dsClassiFication = AwardDataOperations.GetAwardee(WfId);
                dtAwadee = dsClassiFication.Tables["DisplayData"].Copy();
                dtddlType = dsClassiFication.Tables["Type"].Copy();
                DataTable tempcur = dsClassiFication.Tables["DisplayData2"].Copy();

                    DataRow drcur = tempcur.NewRow();
                    drcur["Code"] = "SelectCurrency";
                    drcur["Value"] = "--Select Currency--";
                    tempcur.Rows.InsertAt(drcur, 0);

                    ddlCurency.DataSource = tempcur;
                    ddlCurency.DisplayMember = "Value";
                    ddlCurency.ValueMember = "Code";
                    ddlCurency.SelectedValue = "USD";
                    BindGrid();
                
               DataTable templang = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr = templang.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";
                templang.Rows.InsertAt(dr, 0);

                ddlLangOrg.DataSource = templang;
                ddlLangOrg.ValueMember = "LANGUAGE_CODE";
                ddlLangOrg.DisplayMember = "LANGUAGE_NAME";
                ddlLangOrg.SelectedIndex = 18;


                ddlLangDept.DataSource = templang;
                ddlLangDept.ValueMember = "LANGUAGE_CODE";
                ddlLangDept.DisplayMember = "LANGUAGE_NAME";
                ddlLangDept.SelectedIndex = 18;

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
                DataTable DT = dtAwadee.Copy();
                if (DT.Rows.Count > 0)
                {
                    grdClass.AutoGenerateColumns = false;
                    grdClass.DataSource = DT;
                }
                else
                {
                    norecord();
                }
                DataTable DT2 = dtddlType.Copy();

                DataRow dr = DT2.NewRow();
                dr["CODE"] = "SelectType";
                dr["VALUE"] = "--Select Type--";
                DT2.Rows.InsertAt(dr, 0);


                ddlType.DataSource = DT2;
                ddlType.DisplayMember = "VALUE";
                ddlType.ValueMember = "CODE";

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
                dtNoRcrd.Columns.Add("INDEXEDNAME");
                dtNoRcrd.Columns.Add("GIVENNAME");
                dtNoRcrd.Columns.Add("INITIALS");
                dtNoRcrd.Columns.Add("SURNAME");
                dtNoRcrd.Columns.Add("VALUE");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdClass.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                FromAffiliationGrid = false;
                Regex strRgx = new Regex(@"[A-Za-z ]");
                string orcid = "";
                string indexName = Regex.Replace(txtAddIndex.Text,@"[A-Za-z ]", "");
                string givenName = Regex.Replace(txtAddGiven.Text,@"[A-Za-z ]", "");
                string surName = Regex.Replace(txtAddSurname.Text,@"[A-Za-z ]", "");

                if (txt_ORCID.Text != "")
                {
                    orcid = txt_ORCID.Text;
                }

                if (ddlType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (false)
                {
                    MessageBox.Show("Please enter Index Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddIndex.Text != "" && (pattern.Matches(indexName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Index Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddGiven.Text != "" && (pattern.Matches(givenName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Given Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddSurname.Text != "" && (pattern.Matches(surName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Sur Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddGiven.Text == "" && txtAddInitial.Text != "")
                {
                    MessageBox.Show("Please enter Given Name First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    string currency = Convert.ToString(ddlCurency.SelectedValue);
                    Int64 amount = 0;
                    if (txtAmount.Text.ToString().Trim() != "")
                    {
                        amount = Convert.ToInt64(txtAmount.Text.ToString().Trim());
                    }
                    string ddlTryp = string.Empty;
                    Int64 scopusid = 0;
                    if (ddlType.SelectedValue.ToString() == "SelectType")
                        ddlTryp = "";
                    else
                        ddlTryp = ddlType.SelectedValue.ToString();

                    scopusid = 0;

                    #region Value Assigning Schema 5.0 

                    activityType = txtAddSurname.Text.Trim();
                    awardeeAffiliationId = txtAddInitial.Text.Trim();
                    departmentName = txtAddIndex.Text.Trim();
                    fundingBodyOrganizationId = txt_ORCID.Text.Trim();
                    link = txt_Link.Text.Trim();
                    name = txtAddGiven.Text.Trim();
                    role = ddlType.SelectedValue.ToString();
                    vatNumber = txt_EXTRes_ID.Text.Trim();
                    DUNS = txt_DUNS.Text.Trim();
                    ROR = txt_ROR.Text.Trim();
                    WIKIDATA = txt_WikiData.Text.Trim();

                    #endregion


                    DataSet dsresult = AwardDataOperations.SaveAndDeleteAwardee50(WfId, 0, 0, activityType, awardeeAffiliationId, departmentName, fundingBodyOrganizationId, link, name, role, vatNumber, DUNS, ROR, WIKIDATA, currency, amount);


                    if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {


                        dtAwadee = dsresult.Tables["DisplayData"];
                        awadeeID = Convert.ToInt32(dsresult.Tables["DisplayData"].Rows[0]["Awardee_Id"]);

                        if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                        {
                            BindGrid();

                            if (ddlTryp != "institution")
                                txtOrg.Text = "Not Available";
                            else
                                txtOrg.Text = txtAddIndex.Text;

                            awadeeID = Convert.ToInt32(dsresult.Tables["DisplayData"].Rows[0]["Awardee_Id"]);


                            ddlType.SelectedIndex = 0;
                            txtAddIndex.Text = "";
                            txtAddGiven.Text = "";
                            txtAddInitial.Text = "";
                            txtAddSurname.Text = "";
                            txt_ORCID.Text = "";
                            txt_EXTRes_ID.Text = "";

                            #region
                            txtAddSurname.Text = "";
                            txtAddInitial.Text = "";
                            txtAddIndex.Text = "";
                            txt_ORCID.Text = "";
                            txt_Link.Text = "";
                            txtAddGiven.Text = "";

                            txt_EXTRes_ID.Text = "";
                            txt_DUNS.Text = "";
                            txt_ROR.Text = "";
                            txt_WikiData.Text = "";
                            #endregion
                        }
                        else
                        {
                            norecord();

                        }


                        grpbpxawardee.Visible = false;
                        grpBoxAddAffiliation.Visible = true;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;//Change by Avanish on 29-03-2018
                        grpAffDtlGrid.Visible = false;

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess("Awardees");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

                    }
                    lblMsg.Visible = true;
                    
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAwadee.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        ddlType.SelectedValue = Convert.ToString(dtAwadee.Rows[rowindex]["type"]);
                        txtAddIndex.Text = Convert.ToString(dtAwadee.Rows[rowindex]["INDEXEDNAME"]);
                        txtAddGiven.Text = Convert.ToString(dtAwadee.Rows[rowindex]["GIVENNAME"]);
                        txtAddInitial.Text = Convert.ToString(dtAwadee.Rows[rowindex]["INITIALS"]);
                        txtAddSurname.Text = Convert.ToString(dtAwadee.Rows[rowindex]["SURNAME"]);
                        txt_ORCID.Text = Convert.ToString(dtAwadee.Rows[rowindex]["ORCID"]);
                        txt_EXTRes_ID.Text = Convert.ToString(dtAwadee.Rows[rowindex]["externalResearcherIdentifier"]);
                        if (Convert.ToString(dtAwadee.Rows[rowindex]["currency"]) == "" || Convert.ToString(dtAwadee.Rows[rowindex]["currency"]) == "0")
                        {
                        }
                        else
                        {
                            ddlCurency.SelectedValue = Convert.ToString(dtAwadee.Rows[rowindex]["currency"]);
                        }

                        txtAmount.Text = Convert.ToString(dtAwadee.Rows[rowindex]["AMOUNT_TEXT"]);

                        btnsave.Visible = false;
                        btnaddurl.Visible = false;

                        btnupdate.Visible = true;
                        btncancel.Visible = true;

                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = false;
                        grpAffDtlGrid.Visible = false;
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
                if (dtAwadee.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //  In Insert there is no nedd to pass second parameter
                            DataSet dsresult = AwardDataOperations.SaveAndDeleteAwardee(SharedObjects.WorkId, 1, "", 0, "", "", "", "", Convert.ToInt64(dtAwadee.Rows[grdClass.SelectedCells[0].RowIndex]["AWARDEE_ID"]), "", 0, "", ""); //Change by Avanish on 13-June-2018
                            if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                dtAwadee = dsresult.Tables["DisplayData"].Copy();
                                if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                                {
                                    BindGrid();
                                }
                                else
                                {
                                    norecord();

                                }

                                #region For Changing Colour in case of Update
                                if (SharedObjects.TRAN_TYPE_ID == 1)
                                {
                                    m_parent.GetProcess("Awardees");
                                }
                                else
                                {
                                    m_parent.GetProcess();
                                }
                                #endregion
                            }

                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                            grpbpxawardee.Visible = true;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = false;
                            grpAffDtlGrid.Visible = false;
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
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Regex strRgx = new Regex(@"[A-Za-z ]");
                string orcid = "";

                if (txt_ORCID.Text != "")
                {
                    orcid = txt_ORCID.Text;
                }


                string indexName = Regex.Replace(txtAddIndex.Text,@"[A-Za-z ]", "");
                string givenName = Regex.Replace(txtAddGiven.Text,@"[A-Za-z ]", "");
                string surName = Regex.Replace(txtAddSurname.Text,@"[A-Za-z ]", "");

                if (ddlType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddIndex.Text == "")
                {
                    MessageBox.Show("Please enter Index Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddIndex.Text != "" && (pattern.Matches(indexName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Index Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddGiven.Text != "" && (pattern.Matches(givenName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Given Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddSurname.Text != "" && (pattern.Matches(surName).Count > 0))
                {
                    MessageBox.Show("Please enter valid Sur Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddGiven.Text == "" && txtAddInitial.Text != "")
                {
                    MessageBox.Show("Please enter Given Name First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    string currency = Convert.ToString(ddlCurency.SelectedValue);
                    Int64 amount = 0;
                    if (txtAmount.Text.ToString().Trim() != "")
                    {
                        amount = Convert.ToInt64(txtAmount.Text.ToString().Trim());
                    }
                    Int64 scopusId = 0;
                    string indexname = txtAddIndex.Text.Trim();
                    givenName = txtAddGiven.Text.Trim();
                    string initial = txtAddInitial.Text.Trim();
                    string surname = txtAddSurname.Text.Trim();

                    DataSet dsresult = AwardDataOperations.SaveAndDeleteAwardee(SharedObjects.WorkId, 2, ddlType.SelectedValue.ToString(), scopusId, indexname, givenName, initial, surname, Convert.ToInt64(dtAwadee.Rows[rowindex]["AWARDEE_ID"]), currency, amount, orcid, txt_EXTRes_ID.Text);

                    if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {

                        dtAwadee = dsresult.Tables["DisplayData"];

                        if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                        {
                            BindGrid();
                        }
                        else
                        {
                            norecord();

                        }

                        ddlType.SelectedIndex = 0;
                        txtAddIndex.Text = "";
                        txtAddGiven.Text = "";
                        txtAddInitial.Text = "";
                        txtAddSurname.Text = "";

                        btnsave.Visible = true;
                        btnaddurl.Visible = true;

                        btnupdate.Visible = false;
                        btncancel.Visible = false;

                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = false;
                        grpAffDtlGrid.Visible = false;
                    }
                    lblMsg.Visible = true;
                    //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();


                }
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
                ddlType.SelectedIndex = 0;
                txtAddIndex.Text = "";
                txtAddGiven.Text = "";
                txtAddInitial.Text = "";
                txtAddSurname.Text = "";

                btnsave.Visible = true;
                btnaddurl.Visible = true;

                btnupdate.Visible = false;
                btncancel.Visible = false;


                grpbpxawardee.Visible = true;
                grpBoxAddAffiliation.Visible = false;
                grpBoxAddAfffDtl.Visible = false;

                grpAwardeeGird.Visible = true;
                grpAffiliationGrid.Visible = false;
                grpAffDtlGrid.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAwardeeDtl_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                FromAffDtlEdit = false;
                FromAffEdit = false;
                if (dtAwadee.Rows.Count > 0)
                {
                    if (grdClass.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please select any record for Awardee detail.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;
                        grpAffDtlGrid.Visible = false;

                        awadeeID = Convert.ToInt32(dtAwadee.Rows[grdClass.SelectedCells[0].RowIndex]["AWARDEE_ID"]);

                        DataSet dsAffGrid = AwardDataOperations.Getaffiliation(Convert.ToInt64(awadeeID));
                        if (Convert.ToString(dsAffGrid.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            dtAffiliation = dsAffGrid.Tables["AffiliationGrid"].Copy();
                            if (dtAffiliation.Rows.Count > 0)
                            {
                                grdAff_Edit.AutoGenerateColumns = false;
                                grdAff_Edit.DataSource = dtAffiliation;

                            }
                            else
                            {
                                NoAffGrid();
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnAWAffiAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                FromAffDtlEdit = false;
                FromAffEdit = false;
                if (dtAwadee.Rows.Count > 0)
                {
                    if (grdClass.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please select any record for Awardee detail.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        awadeeID = Convert.ToInt32(dtAwadee.Rows[grdClass.SelectedCells[0].RowIndex]["AWARDEE_ID"]);

                        grpbpxawardee.Visible = false;
                        grpBoxAddAffiliation.Visible = true;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;//Change by Avanish
                        grpAffDtlGrid.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void NoAffGrid()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();

                dtNoRcrd.Columns.Add("org");
                dtNoRcrd.Columns.Add("dept");
                dtNoRcrd.Columns.Add("startdate");
                dtNoRcrd.Columns.Add("enddate");
                dtNoRcrd.Columns.Add("email");
                dtNoRcrd.Columns.Add("webpage");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdAff_Edit.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void BindEditGrid()
        {
            try
            {
                Int64 scopusid = 0;

                #region Get the Fax data
                DataSet dsFax = AwardDataOperations.GetFax(Convert.ToInt64(affiliationID));
                dtFax = dsFax.Tables["FaxData"].Copy();
                if (Convert.ToString(dsFax.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    if (dsFax.Tables["FaxData"].Rows.Count > 0)
                    {
                        grdFax.AutoGenerateColumns = false;
                        grdFax.DataSource = dsFax.Tables["FaxData"];
                    }
                    else
                    {
                        noFaxrecord();
                    }
                }
                #endregion Get the Fax data

                #region Get the Telephone Date

                DataSet dsTele = AwardDataOperations.GetTelephone(Convert.ToInt64(affiliationID));
                
                    dtTelephones = dsTele.Tables["TelephoneData"].Copy();
                    if (dsTele.Tables["TelephoneData"].Rows.Count > 0)
                    {
                        grdTelephone.AutoGenerateColumns = false;
                        grdTelephone.DataSource = dsTele.Tables["TelephoneData"];
                    }
                    else
                    {
                        noTelephonerecord();
                    }
                

                #endregion Get the Telephone Date

                #region Get the Address data

                DataSet dsAdd = AwardDataOperations.GetAddress(Convert.ToInt64(affiliationID));
                if (Convert.ToString(dsAdd.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    dtAddress = dsAdd.Tables["AddressData"].Copy();
                    if (dsAdd.Tables["AddressData"].Rows.Count > 0)
                    {
                        grdAddress.AutoGenerateColumns = false;
                        grdAddress.DataSource = dsAdd.Tables["AddressData"];
                    }
                    else
                    {
                        noAddressrecord();
                    }
                }
                #endregion Get the Address data
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnFaxSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (txtfax.Text.Trim() == "")
                {
                    return;
                }
                else
                {
                    DataSet drFaxResult = new DataSet();

                    if (FromAffDtlEdit == false)
                        drFaxResult = AwardDataOperations.SaveAndUpdateFax(Convert.ToInt64(affiliationID), 0, txtfax.Text.Trim(), "");
                    else
                        drFaxResult = AwardDataOperations.SaveAndUpdateFax(Convert.ToInt64(affiliationID), 2, txtfax.Text.Trim(), Convert.ToString(dtFax.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["Fax_column"]));


                    if (Convert.ToString(drFaxResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        dtFax = drFaxResult.Tables["DisplayFax"].Copy();

                        if (drFaxResult.Tables["DisplayFax"].Rows.Count > 0)
                        {
                            grdFax.AutoGenerateColumns = false;
                            grdFax.DataSource = drFaxResult.Tables["DisplayFax"];
                            txtfax.Text = "";
                        }
                        else
                        {
                            noFaxrecord();
                        }

                        BindEditGrid();
                    }

                    lblMsg.Visible = true;
                    //lblMsg.Text = drFaxResult.Tables["ERRORCODE"].Rows[0][1].ToString();

                    if (FromAffDtlEdit)
                    {
                        FromAffDtlEdit = false;
                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;
                        grpAffDtlGrid.Visible = true;

                        grdFax.Visible = true;
                        tabAddDetails.TabPages.Add(tabPage2);
                        tabAddDetails.TabPages.Add(tabPage3);
                        BindDisplayGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnFaxCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtfax.Text = "";
                txttelephone.Text = "";
                DDLCOUNTRY.SelectedIndex = 0;
                txtcity.Text = "";
                ddlState.SelectedIndex = 0;
                txtOtherState.Text = "";
                txtStreet.Text = "";
                txtPostalCode.Text = "";
                txtRoom.Text = "";

                if (!FromAffiliationGrid)
                {
                    grpbpxawardee.Visible = false;
                    grpBoxAddAffiliation.Visible = true;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = false;
                    grpAffDtlGrid.Visible = false;
                }
                else
                {
                    FromAffiliationGrid = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = false;
                }

                if (FromAffDtlEdit)
                {
                    FromAffDtlEdit = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = true;

                    grdFax.Visible = true;
                    tabAddDetails.TabPages.Add(tabPage2);
                    tabAddDetails.TabPages.Add(tabPage3);
                    BindDisplayGrid();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noFaxrecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("fax_column");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdFax.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnTelSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Regex intRgx = new Regex(@"^[0-9]+");

                if (txttelephone.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Telephone.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    DataSet drTeleResult = new DataSet();

                    if (FromAffDtlEdit == false)
                        drTeleResult = AwardDataOperations.SaveAndUpdateTelephone(Convert.ToInt64(affiliationID), 0, txttelephone.Text.Trim(), "");
                    else
                        drTeleResult = AwardDataOperations.SaveAndUpdateTelephone(Convert.ToInt64(affiliationID), 2, txttelephone.Text.Trim(), Convert.ToString(dtTelephones.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["telephone_column"]));

                    if (Convert.ToString(drTeleResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        dtTelephones = drTeleResult.Tables["DisplayTelephone"].Copy();

                        if (drTeleResult.Tables["DisplayTelephone"].Rows.Count > 0)
                        {
                            grdTelephone.AutoGenerateColumns = false;
                            grdTelephone.DataSource = drTeleResult.Tables["DisplayTelephone"];
                            txttelephone.Text = "";
                        }
                        else
                        {
                            noTelephonerecord();
                        }


                        BindEditGrid();
                    }
                    lblMsg.Visible = true;
                    
                    if (FromAffDtlEdit)
                    {
                        FromAffDtlEdit = false;
                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;
                        grpAffDtlGrid.Visible = true;

                        grdTelephone.Visible = true;
                        tabAddDetails.TabPages.Add(tabPage1);
                        tabAddDetails.TabPages.Add(tabPage3);
                        BindDisplayGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnTelCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtfax.Text = "";
                txttelephone.Text = "";
                DDLCOUNTRY.SelectedIndex = 0;
                txtcity.Text = "";
                //txtState.Text = "";
                ddlState.SelectedIndex = 0;
                txtOtherState.Text = "";
                txtStreet.Text = "";
                txtPostalCode.Text = "";
                txtRoom.Text = "";

                if (!FromAffiliationGrid)
                {
                    grpbpxawardee.Visible = false;
                    grpBoxAddAffiliation.Visible = true;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = false;
                    grpAffDtlGrid.Visible = false;
                }
                else
                {
                    FromAffiliationGrid = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = false;

                }
                if (FromAffDtlEdit)
                {
                    FromAffDtlEdit = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = true;

                    grdTelephone.Visible = true;
                    BindDisplayGrid();

                    tabAddDetails.TabPages.Add(tabPage1);
                    tabAddDetails.TabPages.Add(tabPage3);
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noTelephonerecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("telephone_column");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdTelephone.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                Regex intRgx = new Regex(@"^[0-9]+");
                if (txtcity.Text.Trim() == "" && DDLCOUNTRY.SelectedIndex == 0 && txtRoom.Text.Trim() == "" && txtStreet.Text.Trim() == "" && ddlState.SelectedIndex == 0 && txtOtherState.Text.Trim() == "" && txtPostalCode.Text.Trim() == "")
                {
                    return;
                }
                else if (txtcity.Text.Trim() != "" && DDLCOUNTRY.SelectedIndex == 0 && txtRoom.Text.Trim() != "" && txtStreet.Text.Trim() != "" && ddlState.SelectedIndex == 0 && txtOtherState.Text.Trim() != "" && txtPostalCode.Text.Trim() != "")
                {
                    MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    string country = string.Empty;
                    if (Convert.ToString(DDLCOUNTRY.SelectedValue) == "SelectCountry")
                        country = "";
                    else
                        country = Convert.ToString(DDLCOUNTRY.SelectedValue);

                    string state = string.Empty;
                    if (ddlState.SelectedValue.ToString() != "SelectState")
                    {
                        if (ddlState.SelectedValue.ToString() == "OtherState")
                            state = Convert.ToString(txtOtherState.Text.Trim());
                        else
                            state = Convert.ToString(ddlState.SelectedValue);
                    }
                    else
                        state = "";

                    DataSet drAddressResult = new DataSet();

                    if (FromAffDtlEdit == false)
                        drAddressResult = AwardDataOperations.SaveAndUpdateAddress(Convert.ToInt64(affiliationID), 0, country, txtRoom.Text.Trim(), txtStreet.Text.Trim(), txtcity.Text.Trim(), state.Trim(), txtPostalCode.Text.Trim(), "", "", "", "", "", "");
                    else
                        drAddressResult = AwardDataOperations.SaveAndUpdateAddress(Convert.ToInt64(affiliationID), 2, country, txtRoom.Text.Trim(), txtStreet.Text.Trim(), txtcity.Text.Trim(), state.Trim(), txtPostalCode.Text.Trim(), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["country"]), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["room"]), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["street"]), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["city"]), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["statecode"]), Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["postalcode"]));
                    if (Convert.ToString(drAddressResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        dtAddress = drAddressResult.Tables["DisplayAddress"].Copy();

                        if (drAddressResult.Tables["DisplayAddress"].Rows.Count > 0)
                        {
                            grdAddress.AutoGenerateColumns = false;
                            grdAddress.DataSource = drAddressResult.Tables["DisplayAddress"];

                            txtcity.Text = "";
                            DDLCOUNTRY.SelectedIndex = 0;
                            txtRoom.Text = "";
                            txtStreet.Text = "";
                            // txtState.Text = "";
                            ddlState.SelectedIndex = 0;
                            txtOtherState.Text = "";
                            txtPostalCode.Text = "";
                        }

                        else
                        {
                            noAddressrecord();
                        }


                        BindEditGrid();
                    }
                    lblMsg.Visible = true;
                    //lblMsg.Text = drAddressResult.Tables["ERRORCODE"].Rows[0][1].ToString();
                                  if (FromAffDtlEdit)
                    {
                        FromAffDtlEdit = false;
                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;
                        grpAffDtlGrid.Visible = true;

                        grdTelephone.Visible = true;
                        BindDisplayGrid();

                        tabAddDetails.TabPages.Add(tabPage1);
                        tabAddDetails.TabPages.Add(tabPage2);
                    }
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtfax.Text = "";
                txttelephone.Text = "";
                txtcity.Text = "";
                DDLCOUNTRY.SelectedIndex = 0;
                txtRoom.Text = "";
                txtStreet.Text = "";
                //txtState.Text = "";
                ddlState.SelectedIndex = 0;
                txtOtherState.Text = "";
                txtPostalCode.Text = "";

                if (!FromAffiliationGrid)
                {
                    grpbpxawardee.Visible = false;
                    grpBoxAddAffiliation.Visible = true;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = false;
                    grpAffDtlGrid.Visible = false;
                }
                else
                {
                    FromAffiliationGrid = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = false;

                }

                if (FromAffDtlEdit)
                {
                    FromAffDtlEdit = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = true;

                    grdAddress.Visible = true;
                    BindDisplayGrid();

                    tabAddDetails.TabPages.Add(tabPage1);
                    tabAddDetails.TabPages.Add(tabPage2);
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noAddressrecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();

                dtNoRcrd.Columns.Add("countryname");
                dtNoRcrd.Columns.Add("Room");
                dtNoRcrd.Columns.Add("Street");
                dtNoRcrd.Columns.Add("City");
                dtNoRcrd.Columns.Add("State");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdAddress.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAffDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                FromAffDtlEdit = false;
                FromAffEdit = false;
                if (dtAffiliation.Rows.Count > 0)
                {
                    affiliationID = Convert.ToInt32(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["affiliation_ID"]);

                    BindDisplayGrid();

                    tabAddDetails.TabIndex = 0;

                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = true;
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnAffDtlAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                FromAffDtlEdit = false;
                FromAffEdit = false;
                if (dtAffiliation.Rows.Count > 0)
                {

                    affiliationID = Convert.ToInt32(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["affiliation_ID"]);
                    awadeeID = Convert.ToInt32(dtAwadee.Rows[grdClass.SelectedCells[0].RowIndex]["AWARDEE_ID"]);

                    FromAffiliationGrid = true;
                    BindEditGrid();

                    grpbpxawardee.Visible = false;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = true;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = true;//Change by avanish on 29-mar-2018
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noFaxrecorddisplay()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("fax_column");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdFaxDispaly.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noTelephonerecorddisplay()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("telephone_column");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdTeleDisplay.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noAddressrecorddisplay()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();

                dtNoRcrd.Columns.Add("countryname");
                dtNoRcrd.Columns.Add("Room");
                dtNoRcrd.Columns.Add("Street");
                dtNoRcrd.Columns.Add("City");
                dtNoRcrd.Columns.Add("State");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdAddressDisplay.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void BindDisplayGrid()
        {
            try
            {

                Int64 scopusid = 0;

                #region Get the Fax data
                DataSet dsFax = AwardDataOperations.GetFax(Convert.ToInt64(affiliationID));
                dtFax = dsFax.Tables["FaxData"].Copy();
                if (Convert.ToString(dsFax.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    if (dsFax.Tables["FaxData"].Rows.Count > 0)
                    {
                        grdFaxDispaly.AutoGenerateColumns = false;
                        grdFaxDispaly.DataSource = dsFax.Tables["FaxData"];
                    }
                    else
                    {
                        noFaxrecorddisplay();
                    }
                }
                #endregion Get the Fax data

                #region Get the Telephone Date

                DataSet dsTele = AwardDataOperations.GetTelephone(Convert.ToInt64(affiliationID));
                if (Convert.ToString(dsTele.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    dtTelephones = dsTele.Tables["TelephoneData"].Copy();
                    if (dsTele.Tables["TelephoneData"].Rows.Count > 0)
                    {
                        grdTeleDisplay.AutoGenerateColumns = false;
                        grdTeleDisplay.DataSource = dsTele.Tables["TelephoneData"];
                    }
                    else
                    {
                        noTelephonerecorddisplay();
                    }
                }

                #endregion Get the Telephone Date

                #region Get the Address data

                DataSet dsAdd = AwardDataOperations.GetAddress(Convert.ToInt64(affiliationID));
                if (Convert.ToString(dsAdd.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    dtAddress = dsAdd.Tables["AddressData"].Copy();
                    if (dsAdd.Tables["AddressData"].Rows.Count > 0)
                    {
                        grdAddressDisplay.AutoGenerateColumns = false;
                        grdAddressDisplay.DataSource = dsAdd.Tables["AddressData"];
                    }
                    else
                    {
                        noAddressrecorddisplay();
                    }
                }
                #endregion Get the Address data
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Regex strRgx = new Regex(@"[A-Za-z ]");
                string ORGName = Regex.Replace(txtOrg.Text,@"[A-Za-z ]", "");
                if (false)
                {
                    MessageBox.Show("Please enter data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtOrg.Text != "" && (pattern.Matches(ORGName).Count > 0))
                {
                    MessageBox.Show("Please enter valid data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtemail.Text != "" && !CheckEmail(txtemail.Text))
                {
                    MessageBox.Show("Please enter valid email", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    Int64 scopusid = 0;
                    Int64 awarddeId = Convert.ToInt64(awadeeID);
                    DataSet dsresult = new DataSet();
                    scopusid = 0;
                    #region
                    awardeePersonId = txtOrg.Text.Trim();
                    emailAddress = txtemail.Text.Trim();
                    familyName = txtwebPage.Text.Trim();
                    fundingBodyPersonId = txt_FBPersonID.Text.Trim();
                    givenName = txt_givenName.Text.Trim();
                    ORCID_Aff = txt_ExtAffi_Id.Text.Trim();
                    initials_Aff = txt_initials.Text.Trim();
                    role_Aff = ddl_affi_Role.SelectedItem.ToString();
                    name_Aff = txtDaprt.Text.Trim();
                    #endregion


                    if (FromAffEdit == false)
                    {
                        if (affiliationID == 0)
                            dsresult = AwardDataOperations.SaveAndDeleteAffiliation50(awarddeId, 0, 0, awardeePersonId, emailAddress, familyName, fundingBodyPersonId, givenName, ORCID_Aff, initials_Aff, role_Aff, name_Aff);
                        else
                            dsresult = AwardDataOperations.SaveAndDeleteAffiliation(awarddeId, 2, scopusid, txtOrg.Text.Trim(), txtDaprt.Text.Trim(), txtSrtDate.Text.Trim(), txtEndDate.Text.Trim(), txtemail.Text.Trim(), txtwebPage.Text.Trim(), txt_ExtAffi_Id.Text.Trim(), Convert.ToInt64(affiliationID));
                    }
                    else
                    {
                        dsresult = AwardDataOperations.SaveAndDeleteAffiliation(awarddeId, 2, scopusid, txtOrg.Text.Trim(), txtDaprt.Text.Trim(), txtSrtDate.Text.Trim(), txtEndDate.Text.Trim(), txtemail.Text.Trim(), txtwebPage.Text.Trim(), txt_ExtAffi_Id.Text.Trim(), Convert.ToInt64(affiliationID));
                    }
                    if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    {
                        dtAffiliation = dsresult.Tables["DisplayAffiliation"].Copy();

                        dtAffiliation = dsresult.Tables["DisplayAffiliation"].Copy();
                        if (dtAffiliation.Rows.Count > 0)
                        {
                            grdAff_Edit.AutoGenerateColumns = false;
                            grdAff_Edit.DataSource = dtAffiliation;

                        }
                        else
                        {
                            NoAffGrid();
                        }

                        txtOrg.Text = "";
                        txtDaprt.Text = "";
                        txtSrtDate.Text = "";
                        txtEndDate.Text = "";
                        txtemail.Text = "";
                        txtwebPage.Text = "";

                        txtOrg.Text = "";
                        txtemail.Text = "";
                        txtwebPage.Text = "";
                        txt_FBPersonID.Text = "";
                        txt_givenName.Text = "";
                        txt_ExtAffi_Id.Text = "";
                        txt_initials.Text = "";
                        ddl_affi_Role.SelectedItem = "PI";
                        txtDaprt.Text = "";

                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = false;
                        grpAffDtlGrid.Visible = false;

                    }

                    lblMsg.Visible = true;
                    lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();


                    if (FromAffEdit)
                    {
                        FromAffEdit = false;
                        grpbpxawardee.Visible = true;
                        grpBoxAddAffiliation.Visible = false;
                        grpBoxAddAfffDtl.Visible = false;

                        grpAwardeeGird.Visible = true;
                        grpAffiliationGrid.Visible = true;
                        grpAffDtlGrid.Visible = false;

                        btnNext.Visible = true;
                        btndtl.Visible = true;

                    }
                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Regex strRgx = new Regex(@"[A-Za-z ]");

                string ORGName = Regex.Replace(txtOrg.Text,@"[A-Za-z ]", "");

                if (txtOrg.Text == "")
                {
                    MessageBox.Show("Please enter data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtOrg.Text != "" && (pattern.Matches(ORGName).Count > 0))
                {
                    MessageBox.Show("Please enter valid data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtemail.Text != "" && !CheckEmail(txtemail.Text))
                {
                    MessageBox.Show("Please enter valid email", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    Int64 scopusid = 0;
                    Int64 awarddeId = Convert.ToInt64(awadeeID);
                    DataSet dsresult = new DataSet();

                    scopusid = 0;

                    if (affiliationID == 0)
                        dsresult = AwardDataOperations.SaveAndDeleteAffiliation(awarddeId, 0, scopusid, txtOrg.Text.Trim(), txtDaprt.Text.Trim(), txtSrtDate.Text.Trim(), txtEndDate.Text.Trim(), txtemail.Text.Trim(), txtwebPage.Text.Trim(), txt_ExtAffi_Id.Text.Trim(), 0);
                    else
                        dsresult = AwardDataOperations.SaveAndDeleteAffiliation(awarddeId, 2, scopusid, txtOrg.Text.Trim(), txtDaprt.Text.Trim(), txtSrtDate.Text.Trim(), txtEndDate.Text.Trim(), txtemail.Text.Trim(), txtwebPage.Text.Trim(), txt_ExtAffi_Id.Text.Trim(), Convert.ToInt64(affiliationID));


                        dtAffiliation = dsresult.Tables["DisplayAffiliation"].Copy();

                        txtOrg.Text = "";
                        txtDaprt.Text = "";
                        txtSrtDate.Text = "";
                        txtEndDate.Text = "";
                        txtemail.Text = "";
                        txtwebPage.Text = "";

                    
                    affiliationID = 0;
                    lblMsg.Visible = true;
                    //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btndtl_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                Regex strRgx = new Regex(@"[A-Za-z ]");

                string ORGName = Regex.Replace(txtOrg.Text,@"[A-Za-z ]", "");

                FromAffDtlEdit = false;
                FromAffEdit = true;
                if (txtOrg.Text == "")
                {
                    MessageBox.Show("Please enter data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtOrg.Text != "" && (pattern.Matches(ORGName).Count > 0))
                {
                    MessageBox.Show("Please enter valid data in Organisation.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtemail.Text != "" && !CheckEmail(txtemail.Text))
                {
                    MessageBox.Show("Please enter valid email", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (affiliationID == 0)
                    {
                        Int64 scopusid = 0;
                        Int64 awarddeId = Convert.ToInt64(awadeeID);
                        DataSet dsresult = AwardDataOperations.SaveAndDeleteAffiliation(awarddeId, 0, scopusid, txtOrg.Text.Trim(), txtDaprt.Text.Trim(), txtSrtDate.Text.Trim(), txtEndDate.Text.Trim(), txtemail.Text.Trim(), txtwebPage.Text.Trim(), txt_ExtAffi_Id.Text.Trim(), 0);

                        if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            affiliationID = Convert.ToInt32(dsresult.Tables["DisplayAffiliation"].Rows[0]["AFFILIATION_ID"]);


                            #region Get the Fax data
                            DataSet dsFax = AwardDataOperations.GetFax(Convert.ToInt64(affiliationID));
                            dtFax = dsFax.Tables["FaxData"].Copy();
                            if (Convert.ToString(dsFax.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                if (dsFax.Tables["FaxData"].Rows.Count > 0)
                                {
                                    grdFax.AutoGenerateColumns = false;
                                    grdFax.DataSource = dsFax.Tables["FaxData"];
                                }
                                else
                                {
                                    noFaxrecord();
                                }
                            }
                            #endregion Get the Fax data

                            #region Get the Telephone Date

                            DataSet dsTele = AwardDataOperations.GetTelephone(Convert.ToInt64(affiliationID));
                            if (Convert.ToString(dsTele.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                dtTelephones = dsTele.Tables["TelephoneData"].Copy();
                                if (dsTele.Tables["TelephoneData"].Rows.Count > 0)
                                {
                                    grdTelephone.AutoGenerateColumns = false;
                                    grdTelephone.DataSource = dsTele.Tables["TelephoneData"];
                                }
                                else
                                {
                                    noTelephonerecord();
                                }
                            }

                            #endregion Get the Telephone Date

                            #region Get the Address data

                            DataSet dsAdd = AwardDataOperations.GetAddress(Convert.ToInt64(affiliationID));
                            if (Convert.ToString(dsAdd.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                dtAddress = dsAdd.Tables["AddressData"].Copy();
                                if (dsAdd.Tables["AddressData"].Rows.Count > 0)
                                {
                                    grdAddress.AutoGenerateColumns = false;
                                    grdAddress.DataSource = dsAdd.Tables["AddressData"];
                                }
                                else
                                {
                                    noAddressrecord();
                                }
                            }
                            #endregion Get the Address data
                        }
                        
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
                    }
                    else
                    {

                        BindEditGrid();
                    }
                    grpbpxawardee.Visible = false;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = true;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;//Change by Avanish on 2-Apr-2018
                    grpAffDtlGrid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAffiliation.Rows.Count > 0)
                {
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = false;
                    grpAffDtlGrid.Visible = false;
                }
                else
                {
                    MessageBox.Show("Please fill the Affiliation", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (FromAffEdit)
                {
                    FromAffEdit = false;
                    grpbpxawardee.Visible = true;
                    grpBoxAddAffiliation.Visible = false;
                    grpBoxAddAfffDtl.Visible = false;

                    grpAwardeeGird.Visible = true;
                    grpAffiliationGrid.Visible = true;
                    grpAffDtlGrid.Visible = false;

                    btnNext.Visible = true;
                    btndtl.Visible = true;

                }

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void dtPickStart_ValueChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtSrtDate.Text = dtPickStart.Text;
        }
        private void dtPickEnd_ValueChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtEndDate.Text = dtPickEnd.Text;
        }
        private void btnstrtClr_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtSrtDate.Text = "";
        }
        private void btnendClr_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtEndDate.Text = "";
        }

        private void grdAff_Edit_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAffiliation.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            FromAffEdit = true;
                            affiliationID = Convert.ToInt32(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["affiliation_ID"]);
                            awadeeID = Convert.ToInt32(dtAwadee.Rows[grdClass.SelectedCells[0].RowIndex]["AWARDEE_ID"]);

                            txtOrg.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["org"]);
                            txtDaprt.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["dept"]);

                            txtSrtDate.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["startdate"]);

                            if (txtSrtDate.Text != "")
                                dtPickStart.Text = txtSrtDate.Text;

                            txtEndDate.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["enddate"]);

                            if (txtEndDate.Text != "")
                                dtPickEnd.Text = txtEndDate.Text;

                            txtemail.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["email"]);
                            txtwebPage.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["webpage"]);
                            txt_ExtAffi_Id.Text = Convert.ToString(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["ExternalAffiliationIdentifier"]);



                            grpbpxawardee.Visible = false;
                            grpBoxAddAffiliation.Visible = true;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = false;

                            btnNext.Visible = false;
                            btndtl.Visible = false;
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
        private void grdAff_Edit_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAffiliation.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 AffilationId = Convert.ToInt64(dtAffiliation.Rows[grdAff_Edit.SelectedCells[0].RowIndex]["affiliation_ID"]);
                            DataSet dsresult = AwardDataOperations.SaveAndDeleteAffiliation(Convert.ToInt64(awadeeID), 1, 0, "", "", "", "", "", "", "", AffilationId);

                            dtAffiliation = dsresult.Tables["DisplayAffiliation"].Copy();
                            if (dtAffiliation.Rows.Count > 0)
                            {
                                grdAff_Edit.AutoGenerateColumns = false;
                                grdAff_Edit.DataSource = dtAffiliation;

                            }
                            else
                            {
                                NoAffGrid();
                            }

                            grpbpxawardee.Visible = true;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = false;


                            // MessageBox.Show(dsresult.Tables["ERRORCODE"].Rows[0][1].ToString(), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblMsg.Visible = true;
                            //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
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
        private void btnAffDtlCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            grpbpxawardee.Visible = true;
            grpBoxAddAffiliation.Visible = false;
            grpBoxAddAfffDtl.Visible = false;

            grpAwardeeGird.Visible = true;
            grpAffiliationGrid.Visible = true;
            grpAffDtlGrid.Visible = false;
        }

        private void grdFaxDispaly_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtFax.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 AffilationId = Convert.ToInt64(affiliationID);
                            string faxtext = Convert.ToString(dtFax.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["Fax_column"]);

                            DataSet dsresult = AwardDataOperations.SaveAndUpdateFax(AffilationId, 1, faxtext, "");

                            dtFax = dsresult.Tables["displayfax"].Copy();

                            if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                if (dtFax.Rows.Count > 0)
                                {
                                    grdFaxDispaly.AutoGenerateColumns = false;
                                    grdFaxDispaly.DataSource = dsresult.Tables["displayfax"];
                                }
                                else
                                {
                                    noFaxrecorddisplay();
                                }
                            }

                            grpbpxawardee.Visible = true;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;

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
        private void grdFaxDispaly_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtFax.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            txtfax.Text = Convert.ToString(dtFax.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["Fax_column"]);

                            FromAffDtlEdit = true;
                            grdFax.Visible = true;//Change by Avanish on 2-Apr-2018

                            grpbpxawardee.Visible = false;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = true;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;

                            tabAddDetails.TabPages.Remove(tabPage2);
                            tabAddDetails.TabPages.Remove(tabPage3);

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

        private void grdTeleDisplay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtTelephones.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            txttelephone.Text = Convert.ToString(dtTelephones.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["telephone_column"]);

                            FromAffDtlEdit = true;
                            grdTelephone.Visible = false;

                            grpbpxawardee.Visible = false;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = true;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;

                            tabAddDetails.TabPages.Remove(tabPage1);
                            tabAddDetails.TabPages.Remove(tabPage3);
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
        private void grdTeleDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtTelephones.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 AffilationId = Convert.ToInt64(affiliationID);
                            string telText = Convert.ToString(dtTelephones.Rows[grdFaxDispaly.SelectedCells[0].RowIndex]["telephone_column"]);

                            DataSet dsresult = AwardDataOperations.SaveAndUpdateTelephone(AffilationId, 1, telText, "");

                            dtTelephones = dsresult.Tables["DisplayTelephone"].Copy();

                            if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                if (dsresult.Tables["DisplayTelephone"].Rows.Count > 0)
                                {
                                    grdTeleDisplay.AutoGenerateColumns = false;
                                    grdTeleDisplay.DataSource = dsresult.Tables["DisplayTelephone"];
                                }
                                else
                                {
                                    noTelephonerecorddisplay();
                                }
                            }

                            grpbpxawardee.Visible = true;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;
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

        private void grdAddressDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAddress.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Int64 AffilationId = Convert.ToInt64(affiliationID);
                            //string AddText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["address_text"]);
                            string countText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["country"]);
                            string RoomText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["Room"]);
                            string StreetText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["Street"]);
                            string CityText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["City"]);
                            string StateText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["State"]);
                            string PostalcodeText = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["Postalcode"]);

                            DataSet dsresult = AwardDataOperations.SaveAndUpdateAddress(AffilationId, 1, countText, RoomText, StreetText, CityText, StateText, PostalcodeText, "", "", "", "", "", "");

                            dtAddress = dsresult.Tables["DisplayAddress"].Copy();

                            if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                if (dsresult.Tables["DisplayAddress"].Rows.Count > 0)
                                {
                                    grdAddressDisplay.AutoGenerateColumns = false;
                                    grdAddressDisplay.DataSource = dsresult.Tables["DisplayAddress"];
                                }
                                else
                                {
                                    noAddressrecorddisplay();
                                }
                            }

                            grpbpxawardee.Visible = true;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = false;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;
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
        private void grdAddressDisplay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtAddress.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            txtRoom.Text = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["Room"]);
                            txtcity.Text = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["City"]);

                            string othetState = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["State"]).Trim();
                            string othetStatecode = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["statecode"]).Trim();
                            string country = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["country"]);
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
                                    ddlState.SelectedValue = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["statecode"]);
                                }
                                else if (dtStateResult1.Rows.Count > 0)
                                {
                                    ddlState.SelectedValue = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["statecode"]);
                                }
                                else
                                {
                                    ddlState.SelectedValue = "OtherState";
                                    txtOtherState.Enabled = true;
                                    txtOtherState.Text = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["statecode"]);
                                }

                            }


                            txtPostalCode.Text = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["PostalCode"]);
                            txtStreet.Text = Convert.ToString(dtAddress.Rows[grdAddressDisplay.SelectedCells[0].RowIndex]["Street"]);
                            ///////////////////////////////

                            FromAffDtlEdit = true;
                            grdAddress.Visible = false;

                            grpbpxawardee.Visible = false;
                            grpBoxAddAffiliation.Visible = false;
                            grpBoxAddAfffDtl.Visible = true;

                            grpAwardeeGird.Visible = true;
                            grpAffiliationGrid.Visible = true;
                            grpAffDtlGrid.Visible = true;


                            tabAddDetails.TabPages.Remove(tabPage1);
                            tabAddDetails.TabPages.Remove(tabPage2);

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


        private void btnAffCabncel_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            FromAffDtlEdit = false;
            FromAffEdit = false;
            grpbpxawardee.Visible = true;
            grpBoxAddAffiliation.Visible = false;
            grpBoxAddAfffDtl.Visible = false;

            grpAwardeeGird.Visible = true;
            grpAffiliationGrid.Visible = false;
            grpAffDtlGrid.Visible = false;
        }

        public bool CheckEmail(string EMAIL)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
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

        private void button2_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtwebPage.Text = SharedObjects.CurrentUrl;
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            SharedObjects.MultipleInitial = "";
            string hycheck = string.Empty;
            if (txtAddGiven.Text != "")
            {
                string strKeyWords = txtAddGiven.Text.ToString();
                string[] KeyWordsArr = strKeyWords.Split(' ');

                DataSet dsresult = new DataSet();
                foreach (string Keyword in KeyWordsArr)
                {
                    string[] KeyWordsArr_withhy = null;
                    if (Keyword.Contains("-"))
                    {

                        string trimkeyword = Keyword.Trim();
                        trimkeyword = Keyword.TrimStart();
                        trimkeyword = Keyword.TrimEnd();
                        if (trimkeyword == "-")
                        {
                            SharedObjects.MultipleInitial = SharedObjects.MultipleInitial + "-";
                        }

                        KeyWordsArr_withhy = trimkeyword.Split('-');



                        string InitialDeatils2 = string.Empty;

                        foreach (string Keyword_hy in KeyWordsArr_withhy)
                        {
                            string length = KeyWordsArr_withhy.Length.ToString();
                            if (Keyword_hy == "")
                                hycheck = "1";

                            if (Keyword_hy.Length > 0)
                            {
                                if (hycheck == "1")

                                    InitialDeatils2 = InitialDeatils2 + "-" + Keyword_hy.Substring(0, 1) + ".";

                                else

                                    InitialDeatils2 = InitialDeatils2 + Keyword_hy.Substring(0, 1) + ".-";

                            }
                            else
                            {

                            }


                        }
                        if (hycheck == "1")
                        {
                        }
                        else
                        {
                            InitialDeatils2 = InitialDeatils2.Substring(0, InitialDeatils2.Length - 1);
                        }
                        SharedObjects.MultipleInitial = SharedObjects.MultipleInitial + InitialDeatils2;
                    }
                    else
                    {
                        InitialDeatils = Keyword.Substring(0, 1);
                        string multipalInital = InitialDeatils.ToString() + ".";
                        SharedObjects.MultipleInitial = SharedObjects.MultipleInitial + multipalInital;
                    }
                }
                txtAddInitial.Text = SharedObjects.MultipleInitial;
            }

            string allowedchar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.- ";
            string index = "";
            if (txtAddSurname.Text != "")
            {
                if (txtAddGiven.Text != "")
                {
                    InitialDeatils = SharedObjects.MultipleInitial.ToString();
                    if (InitialDeatils != "")
                    {
                        index = txtAddSurname.Text.Trim() + " " + InitialDeatils + " " + txtAddGiven.Text.Trim();
                    }
                    else
                        //pankaj 
                        MessageBox.Show("Please enter Initials First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtAddGiven.Text == "" && txtAddInitial.Text == "")
                {
                    index = txtAddSurname.Text.Trim();
                }
                else
                {
                    MessageBox.Show("Please enter Given Name First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (txtAddGiven.Text != "")
                {
                    InitialDeatils = SharedObjects.MultipleInitial.ToString();
                    if (InitialDeatils != "")
                    {

                        index = InitialDeatils + " " + txtAddGiven.Text.Trim();
                    }
                    else
                        MessageBox.Show("Please enter Initials First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter Given Name First.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtAddIndex.Text = index;

            #region It is commented for bypassing spetial char on 25-Oct-2018

            #endregion


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

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (e.KeyChar != '\b')
            {
                e.Handled = !char.IsNumber(e.KeyChar);
            }
        }

        private void btnAddOrg_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (txtOrg.Text == "")
                {
                    MessageBox.Show("Please enter Orgnisation Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangOrg.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {


                    string ddlContextID = Convert.ToString(ddlLangOrg.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = txtOrg.Text;
                    string thirdColum = ddlContextID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridOrg.Rows.Add(rowGrid);
                    ddlLangOrg.SelectedIndex = 18;
                    txtOrg.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAddDept_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (txtDaprt.Text == "")
                {
                    MessageBox.Show("Please enter Award Name.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (ddlLangDept.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Language.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string ddlContextID = Convert.ToString(ddlLangDept.SelectedValue);
                    string firstColum = Convert.ToString(0);
                    string secondColum = txtDaprt.Text;
                    string thirdColum = ddlContextID.ToLower();
                    string[] rowGrid = { firstColum, secondColum, thirdColum };
                    dtGridDept.Rows.Add(rowGrid);
                    ddlLangDept.SelectedIndex = 18;
                    txtDaprt.Text = "";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void txtAddGiven_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddGiven_TabIndexChanged(object sender, EventArgs e)
        {
            if (txtAddGiven.Text != "")
            {
                string strKeyWords = txtAddGiven.Text.ToString();
                string[] KeyWordsArr = strKeyWords.Split(' ');

                DataSet dsresult = new DataSet();
                foreach (string Keyword in KeyWordsArr)
                {
                    InitialDeatils = Keyword.Substring(0, 1);
                    if (InitialDeatils == "-")
                    {
                        InitialDeatils = Keyword.Substring(0, 2);
                    }
                    string multipalInital = InitialDeatils.ToString() + ".";
                    SharedObjects.MultipleInitial = SharedObjects.MultipleInitial + multipalInital;
                }
                txtAddInitial.Text = SharedObjects.MultipleInitial;
            }
        }

        private void txtAddGiven_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btninitails_Click(object sender, EventArgs e)
        {
            if (txtAddGiven.Text != "")
            {
                string strKeyWords = txtAddGiven.Text.ToString();
                string[] KeyWordsArr = strKeyWords.Split(' ');

                DataSet dsresult = new DataSet();
                foreach (string Keyword in KeyWordsArr)
                {
                    InitialDeatils = Keyword.Substring(0, 1);
                    if (InitialDeatils == "-")
                    {
                        InitialDeatils = Keyword.Substring(0, 2);
                    }
                    string multipalInital = InitialDeatils.ToString() + ".";
                    SharedObjects.MultipleInitial = SharedObjects.MultipleInitial + multipalInital;
                }
                txtAddInitial.Text = SharedObjects.MultipleInitial;
            }
        }

        private void txtAddInitial_TextChanged(object sender, EventArgs e)
        {

        }












    }
}
