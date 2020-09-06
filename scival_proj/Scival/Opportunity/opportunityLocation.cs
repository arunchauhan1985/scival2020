using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class opportunityLocation : UserControl
    {        
        DataTable dttempContact = new DataTable();
        private Opportunity m_parent;
        ErrorLog oErrorLog = new ErrorLog();

        Int64 WFID = 0;
        Int64 pagemode = 0; int rowindex = 0;
        public String FormName = String.Empty;
        static Int64 locationid = 0;

        public opportunityLocation(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
            LoadinitialVale();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void DDLCOUNTRY_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlcountrylistdtl_MouseWheel(object sender, MouseEventArgs e)
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
                DataTable dtEclassText = new DataTable();

                WFID = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsEClass = OpportunityDataOperations.GetEligibiltyClassification(WFID);

                dtEclassText = dsEClass.Tables["country"].Copy(); ;

                DataTable dt21 = dtEclassText;

                ddlcountrylistdtl.DataSource = dt21;
                ddlcountrylistdtl.DisplayMember = "name";
                ddlcountrylistdtl.ClearSelected();
                ddlcountrylistdtl.ValueMember = "lcode";
                ddlcountrylistdtl.Enabled = true;

                DataSet dsgrouping = OpportunityDataOperations.GetCountryList_bygroupID(Convert.ToInt64((1)));

                DataTable groupingtable = dsgrouping.Tables["DisplayData"].Copy();
                DataTable groupingnametable = dsgrouping.Tables["DisplayData1"].Copy();

                if (dsgrouping.Tables[0].Rows.Count > 0)
                {
                    chk_individual_list.DataSource = groupingnametable;
                    chk_individual_list.ValueMember = "COUNTRYGROUP_ID";
                    chk_individual_list.DisplayMember = "COUNTRYGROUP_NAME";
                }

                lblMsg.Visible = false;
                DataSet dsItems = OpportunityDataOperations.Getopportunity_location(SharedObjects.WorkId);
                dttempContact = dsItems.Tables["LocationList"].Copy();

                if (dsItems.Tables["LocationList"].Rows.Count > 0)
                {
                    grdAddress.AutoGenerateColumns = false;
                    grdAddress.DataSource = dttempContact;
                }
                else
                {
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
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void txtSubmit_Click(object sender, EventArgs e)
        {
            string url_TextRoom = TextRoom.Text.TrimStart().TrimEnd();
            string url_TextStreet = TextStreet.Text.TrimStart().TrimEnd();
            string url_TextCity = TextCity.Text.TrimStart().TrimEnd();
            string url_TextPostalCode = TextPostalCode.Text.TrimStart().TrimEnd();
            string url_txtOtherState = txtOtherState.Text.TrimStart().TrimEnd();
            
            if (url_TextRoom.Contains("http://") || url_TextStreet.Contains("http://") || url_TextCity.Contains("http://") || url_TextPostalCode.Contains("http://") || url_txtOtherState.Contains("http://") ||
              url_TextRoom.Contains("https://") || url_TextStreet.Contains("https://") || url_TextCity.Contains("https://") || url_TextPostalCode.Contains("https://") || url_txtOtherState.Contains("https://") ||
              url_TextRoom.Contains("www.") || url_TextStreet.Contains("www.") || url_TextCity.Contains("www.") || url_TextPostalCode.Contains("www.") || url_txtOtherState.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;

                    if (ddlcountrylistdtl.SelectedIndex == -1 && chk_individual_list.CheckedItems.Count == 0)
                    {
                        MessageBox.Show("Please select Country / group country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string grouplistid = string.Empty;

                    if (chk_individual_list.SelectedItems.Count > 0 && chk_individual_list.CheckedItems.Count > 0)
                    {
                        grouplistid = chk_individual_list.SelectedValue.ToString();
                    }
                    else
                    {
                        grouplistid = "0";
                    }

                    DataSet dsgrouping = OpportunityDataOperations.GetCountryList_bygroupID(Convert.ToInt64((grouplistid)));

                    DataTable groupingtable = dsgrouping.Tables["DisplayData"].Copy();
                    DataTable groupingnametable = dsgrouping.Tables["DisplayData1"].Copy();

                    if (TextRoom.Text.Trim() != "" || TextStreet.Text.Trim() != "" || TextCity.Text.Trim() != "" || TextPostalCode.Text.Trim() != "" || ddlState.SelectedIndex > 0)
                    {
                        if (ddlcountrylistdtl.SelectedIndex == -1 && chk_individual_list.CheckedItems.Count == 0)
                        {
                            MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    Int64 WFID = SharedObjects.WorkId;
                    txtOtherState.Enabled = false;
                    String Country = string.Empty;
                    Country = "";

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

                    DataSet dsresult = new DataSet();

                    if (groupingtable.Rows.Count > 0)
                    {
                        foreach (DataRow item in groupingtable.Rows)
                        {
                            string cname = Convert.ToString(item["COUNTRY_CODE"]);
                            string indexname = Convert.ToString(item["COUNTRY_ID"]);
                            int indval = 0;
                            indval = Convert.ToInt32(indexname);

                            dsresult = OpportunityDataOperations.SaveUpDelLocation(WFID, 0, cname.ToLower(), TextRoom.Text.Trim(), TextStreet.Text.Trim(), TextCity.Text.Trim(), state, TextPostalCode.Text.Trim(), 0);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ddlcountrylistdtl.SelectedItems.Count; i++)
                        {
                            string C_ID = ((System.Data.DataRowView)(ddlcountrylistdtl.SelectedItems[i])).Row.ItemArray[2].ToString();

                            dsresult = OpportunityDataOperations.SaveUpDelLocation(WFID, 0, C_ID, TextRoom.Text.Trim(), TextStreet.Text.Trim(), TextCity.Text.Trim(), state, TextPostalCode.Text.Trim(), 0);
                        }
                    }

                    dttempContact = dsresult.Tables["LocationList"].Copy();

                    if (dsresult.Tables["LocationList"].Rows.Count > 0)
                    {
                        grdAddress.AutoGenerateColumns = false;
                        grdAddress.DataSource = dttempContact;
                        grdAddress.Visible = true;
                    }
                    else
                    {
                        grdAddress.Visible = false;
                        norecord();
                    }

                    txtOtherState.Text = "";
                    DDLCOUNTRY.SelectedIndex = 0;
                    TextRoom.Text = "";
                    TextStreet.Text = "";
                    TextCity.Text = "";
                    TextPostalCode.Text = "";
                    ddlState.SelectedIndex = 0;
                    m_parent.GetProcess();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Data inserted Successfully !!!!";

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());

                    txtOtherState.Text = "";
                    DDLCOUNTRY.SelectedIndex = 0;
                    TextRoom.Text = "";
                    TextStreet.Text = "";
                    TextCity.Text = "";
                    TextPostalCode.Text = "";
                    ddlState.SelectedIndex = 0;

                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("opportunityLocation");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                    #endregion

                    lblMsg.Visible = true;

                    if (ddlcountrylistdtl.SelectedItems.Count > 0)
                    {
                        lblMsg.Text = "Error: Country already exists. Cannot insert/update same country.";
                    }
                    else
                    {
                        lblMsg.Text = "Data inserted Successfully. !!!!";
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
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
                lblMsg.Visible = false;

                if (dttempContact.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            locationid = Convert.ToInt64(dttempContact.Rows[rowindex]["location_id"]);
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
                        catch (Exception ex)
                        {
                            oErrorLog.WriteErrorLog(ex);
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
                lblMsg.Visible = false;

                if (dttempContact.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = OpportunityDataOperations.SaveUpDelLocation(SharedObjects.WorkId, 1, "", "", "", "", "", "", Convert.ToInt64(dttempContact.Rows[grdAddress.SelectedCells[0].RowIndex]["location_id"]));

                            dttempContact = dsresult.Tables["LocationList"].Copy();

                            if (dsresult.Tables["LocationList"].Rows.Count > 0)
                            {
                                grdAddress.AutoGenerateColumns = false;
                                grdAddress.DataSource = dttempContact;
                                grdAddress.Visible = true;
                            }
                            else
                            {
                                grdAddress.Visible = false;
                                norecord();
                            }

                            txtOtherState.Text = "";
                            DDLCOUNTRY.SelectedIndex = 0;
                            TextRoom.Text = "";
                            TextStreet.Text = "";
                            TextCity.Text = "";
                            TextPostalCode.Text = "";
                            ddlState.SelectedIndex = 0;
                            btnupdate.Visible = false;
                            txtSubmit.Visible = true;
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
                txtOtherState.Text = "";
                DDLCOUNTRY.SelectedIndex = 0;
                TextRoom.Text = "";
                TextStreet.Text = "";
                TextCity.Text = "";
                TextPostalCode.Text = "";
                ddlState.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string url_TextRoom = TextRoom.Text.TrimStart().TrimEnd();
            string url_TextStreet = TextStreet.Text.TrimStart().TrimEnd();
            string url_TextCity = TextCity.Text.TrimStart().TrimEnd();
            string url_TextPostalCode = TextPostalCode.Text.TrimStart().TrimEnd();
            string url_txtOtherState = txtOtherState.Text.TrimStart().TrimEnd();
         
            if (url_TextRoom.Contains("http://") || url_TextStreet.Contains("http://") || url_TextCity.Contains("http://") || url_TextPostalCode.Contains("http://") || url_txtOtherState.Contains("http://") ||
              url_TextRoom.Contains("https://") || url_TextStreet.Contains("https://") || url_TextCity.Contains("https://") || url_TextPostalCode.Contains("https://") || url_txtOtherState.Contains("https://") ||
              url_TextRoom.Contains("www.") || url_TextStreet.Contains("www.") || url_TextCity.Contains("www.") || url_TextPostalCode.Contains("www.") || url_txtOtherState.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;

                    if (TextRoom.Text.Trim() != "" || TextStreet.Text.Trim() != "" || TextCity.Text.Trim() != "" || TextPostalCode.Text.Trim() != "" || ddlState.SelectedIndex > 0)
                    {
                        if (DDLCOUNTRY.SelectedIndex == 0)
                        {
                            MessageBox.Show("Please select Country.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    Int64 WFID = SharedObjects.WorkId;
                    txtOtherState.Enabled = false;
                    String Country = DDLCOUNTRY.SelectedValue.ToString();

                    if (Country.Length > 3)
                        Country = "";

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

                    DataSet dsresult = OpportunityDataOperations.SaveUpDelLocation(WFID, 2, Country, TextRoom.Text.Trim(), TextStreet.Text.Trim(), TextCity.Text.Trim(), state, TextPostalCode.Text.Trim(), locationid);

                    dttempContact = dsresult.Tables["LocationList"].Copy();

                    if (dsresult.Tables["LocationList"].Rows.Count > 0)
                    {
                        grdAddress.AutoGenerateColumns = false;
                        grdAddress.DataSource = dttempContact;
                        grdAddress.Visible = true;
                    }
                    else
                    {
                        grdAddress.Visible = false;
                        norecord();
                    }

                    txtOtherState.Text = "";
                    DDLCOUNTRY.SelectedIndex = 0;
                    TextRoom.Text = "";
                    TextStreet.Text = "";
                    TextCity.Text = "";
                    TextPostalCode.Text = "";
                    ddlState.SelectedIndex = 0;

                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("opportunityLocation");
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

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
        }

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

        private void grpcontact_Enter(object sender, EventArgs e) { }

        private void btnDeleteGrouping_Click(object sender, EventArgs e)
        {
            try
            {
                string deletegroupid = string.Empty;

                if (chk_individual_list.SelectedItems.Count > 0 && chk_individual_list.CheckedItems.Count > 0)
                {
                    deletegroupid = chk_individual_list.SelectedValue.ToString();

                    lblMsg.Visible = false;

                    if (dttempContact.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Do you really  want to delete this group details record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = OpportunityDataOperations.DeleteUpDelLocation(deletegroupid, SharedObjects.WorkId, 1, "", "", "", "", "", "", Convert.ToInt64(dttempContact.Rows[grdAddress.SelectedCells[0].RowIndex]["location_id"]));

                            dttempContact = dsresult.Tables["LocationList"].Copy();

                            if (dsresult.Tables["LocationList"].Rows.Count > 0)
                            {
                                grdAddress.AutoGenerateColumns = false;
                                grdAddress.DataSource = dttempContact;
                                grdAddress.Visible = true;
                            }
                            else
                            {
                                grdAddress.Visible = false;
                                norecord();
                            }

                            txtOtherState.Text = "";
                            DDLCOUNTRY.SelectedIndex = 0;
                            TextRoom.Text = "";
                            TextStreet.Text = "";
                            TextCity.Text = "";
                            TextPostalCode.Text = "";
                            ddlState.SelectedIndex = 0;
                            btnupdate.Visible = false;
                            txtSubmit.Visible = true;
                            m_parent.GetProcess();
                            lblMsg.Visible = true;
                            lblMsg.Text = "Data Delete Successfully !!!!";
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
}
