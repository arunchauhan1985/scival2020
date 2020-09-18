using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;
using MySqlDal.DataOpertation;
using MySqlDalAL;
using Newtonsoft.Json;

namespace Scival.FundingBody
{
    public partial class Item : UserControl
    {
        DataSet dsItems;

        Replace r = new Replace();
        private FundingBody m_parent;
        Int64 pagemode = 0;
        int rowindex = 0;
        ErrorLog oErrorLog = new ErrorLog();
        public Item(FundingBody frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadinitialVale();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        private void LoadinitialVale()
        {
            try
            {
                txtDescr.Visible = true;
                txtLinkUrl.Visible = true;
                button1.Visible = true;
                btnAddURL.Visible = true;
                txtLinkText.Visible = true;
                ddlRegion.Visible = false;
                label3.Visible = true;
                txtLinkText.Visible = true;
                label4.Visible = true;
                pnlURL.Visible = true;
                label2.Text = "Description";
                grdAbout.Visible = true;
                BtnAdd.Text = "Add";
                lblMsg.Visible = false;
                string clickPage = SharedObjects.FundingClickPage;
                grpItem.Text = clickPage;

                DataSet dsOpptunity = SharedObjects.StartWork;
                DataTable tempOppName = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr = tempOppName.NewRow();
                dr = tempOppName.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";
                tempOppName.Rows.InsertAt(dr, 0);

                ddlLangOppName.DataSource = tempOppName;
                ddlLangOppName.ValueMember = "LANGUAGE_CODE";
                ddlLangOppName.DisplayMember = "LANGUAGE_NAME";
                if(dsOpptunity.Tables["LanguageTable"].Rows.Count>17)
                ddlLangOppName.SelectedIndex = 18;

                if (clickPage.ToLower() == "about" || clickPage.ToLower() == "Discription".ToLower())
                {
                    grpItem.Text = "Discription";
                    pagemode = 1;

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    grdAbout.Columns["Linktext"].Visible = false;
                }
                else if (clickPage.ToLower() == "funding policy")
                {
                    pagemode = 2;

                    label4.Visible = false;
                    txtLinkText.Visible = false;
                    grdAbout.Columns["Linktext"].Visible = false;
                }
                else if (clickPage.ToLower() == "geoscope" || clickPage.ToLower() == "funding description".ToLower())
                {
                    pagemode = 3;
                    ddlRelType.Visible = false;
                    label1.Visible = false;
                    txtLinkText.Visible = false;
                    label4.Visible = false;
                    grdAbout.Columns["RelType"].Visible = false;
                    grdAbout.Columns["Linktext"].Visible = false;
                    grdAbout.Columns["Description"].Width = 270;
                }
                else if (clickPage.ToLower() == "related items" || clickPage.ToLower() == "AwardSuccessRate Description".ToLower())
                {
                    pagemode = 4;
                    ddlRelType.Visible = false;
                    label1.Visible = false;
                    txtLinkText.Visible = false;
                    label4.Visible = false;
                    grdAbout.Columns["RelType"].Visible = false;
                    grdAbout.Columns["Linktext"].Visible = false;
                    grdAbout.Columns["Description"].Width = 270;
                }
                else if (clickPage.ToLower() == "region")
                {
                    pagemode = 7;
                    txtDescr.Visible = false;
                    txtLinkUrl.Visible = false;
                    button1.Visible = false;
                    btnAddURL.Visible = false;
                    txtLinkText.Visible = false;
                    ddlRegion.Visible = true;
                    label3.Visible = false;
                    txtLinkText.Visible = false;
                    label4.Visible = false;
                    pnlURL.Visible = false;
                    label2.Text = "Region Name";
                    grdAbout.Visible = false;
                    BtnAdd.Text = "Save";
                    ddlRegion.Items.Insert(0, "--Select Region Name--");
                    ddlRegion.Items.Insert(1, "Asia");
                    ddlRegion.Items.Insert(2, "Africa");
                    ddlRegion.Items.Insert(3, "Americas");
                    ddlRegion.Items.Insert(4, "Europe");
                    ddlRegion.Items.Insert(5, "Oceania");
                    ddlRegion.SelectedIndex = 0;
                    ddlRelType.Enabled = false;
                    ddlLangOppName.Visible = false;
                }
                else if (clickPage.ToLower() == "identifier")
                {
                    pagemode = 7;
                    txtDescr.Visible = false;
                    label1.Visible = false;
                    txtLinkUrl.Visible = false;
                    button1.Visible = false;
                    btnAddURL.Visible = false;
                    txtLinkText.Visible = true;
                    ddlRegion.Visible = true;
                    label3.Visible = false;
                    txtLinkText.Visible = true;
                    label4.Visible = true;
                    label4.Text = "Identifier Values";
                    pnlURL.Visible = false;
                    label2.Text = "Identifier";
                    BtnAdd.Text = "Save";
                    ddlRegion.Items.Insert(0, "--Select Identifier Name--");
                    ddlRegion.Items.Insert(1, "CROSSREFID");
                    ddlRegion.Items.Insert(2, "GRID");
                    ddlRegion.Items.Insert(3, "ROR");
                    ddlRegion.Items.Insert(4, "ISNI");
                    ddlRegion.Items.Insert(5, "WIKIDATA");
                    ddlRegion.SelectedIndex = 0;
                    ddlRelType.Visible = false;
                    ddlLangOppName.Visible = false;
                    grdAbout.Columns["RelType"].HeaderText = "Identifier";
                    grdAbout.Columns["Linktext"].HeaderText = "Value";
                    grdAbout.Columns["RelType"].Visible = true;
                    grdAbout.Columns["Linktext"].Visible = true;
                    grdAbout.Columns["Description"].Visible = false;
                    grdAbout.Columns["Linkurl"].Visible = false;
                }
                else if (clickPage.ToLower() == "subregion")
                {
                    pagemode = 11;
                    txtDescr.Visible = true;
                    ddlLangOppName.Visible = true;
                    txtLinkUrl.Visible = false;
                    button1.Visible = false;
                    btnAddURL.Visible = false;
                    txtLinkText.Visible = false;
                    ddlRegion.Visible = false;
                    label3.Visible = false;
                    txtLinkText.Visible = false;
                    label4.Visible = false;
                    pnlURL.Visible = false;
                    label2.Text = "SubRegion Name";
                    grdAbout.Visible = false;
                    BtnAdd.Text = "Save";

                    ddlRelType.SelectedValue = "SubRegion";
                    ddlRelType.Enabled = false;
                }
                else if (clickPage.ToLower() == "fundergroup")
                {
                    pagemode = 12;
                    txtDescr.Visible = true;
                    ddlLangOppName.Visible = true;
                    txtLinkUrl.Visible = false;
                    button1.Visible = false;
                    btnAddURL.Visible = false;
                    txtLinkText.Visible = false;
                    ddlRegion.Visible = false;
                    label3.Visible = false;
                    txtLinkText.Visible = false;
                    label4.Visible = false;
                    pnlURL.Visible = false;
                    label2.Text = "FunderGroup Name";
                    grdAbout.Visible = false;
                    BtnAdd.Text = "Save";

                    ddlRelType.Items.Insert(0, "FunderGroup");
                    ddlRelType.SelectedValue = "FunderGroup";
                    ddlRelType.Enabled = false;
                }

                BindGrid();
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

              //  var FundingBodyMainJson = FundingBodyDataOperations.GetFundingBodyMainJson(SharedObjects.ID);
              //  FB_JSON_Model model = JsonConvert.DeserializeObject<FB_JSON_Model>(FundingBodyMainJson);

                dsItems = FundingBodyDataOperations.GetItemsList(Convert.ToInt64(SharedObjects.WorkId), pagemode);
                DataTable DT = dsItems.Tables["ItemListDisplay"];
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR in DT.Rows)
                    {
                        try
                        {
                            if (r.chk_OtherLang(DR["lang"].ToString().Trim().ToLower()) == true)
                            {
                                DR["DESCRIPTION"] = Convert.ToString(r.ConvertUnicodeToText(DR["DESCRIPTION"].ToString()));
                                DR["LINK_TEXT"] = Convert.ToString(r.ConvertUnicodeToText(DR["LINK_TEXT"].ToString()));
                                DR.AcceptChanges();
                            }
                        }
                        catch { }
                    }
                    grdAbout.AutoGenerateColumns = false;
                    grdAbout.DataSource = DT;
                }
                else
                {
                    norecord();
                    grdAbout.DataSource = null;
                }
                
                if (dsItems.Tables["ItemListDisplay"] != null && dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    ddlRelType.Items.Add("--Select RelType--");
                    ddlRelType.Items.Add(dsItems.Tables["ItemListDisplay"].Rows[0][0]);
                    ddlRelType.Items.Add("about");
                    if (pagemode == 1)
                    {
                        ddlRelType.SelectedValue = "about";
                    }
                    else if (pagemode == 2)
                    {
                        ddlRelType.SelectedValue = "FundingPolicy";
                    }
                    else if (pagemode == 7)
                    {
                        ddlRelType.SelectedValue = "region";
                        ddlRegion.SelectedItem = DT.Rows[0]["REGION_TEXT"].ToString();
                    }
                    else if (pagemode == 11)
                    {
                        ddlRelType.SelectedValue = "subRegion";
                        txtDescr.Text = DT.Rows[0]["subRegion_TEXT"].ToString();
                    }
                    else if (Convert.ToString(pagemode) == "12")
                    {
                        ddlRelType.SelectedValue = "funderGroup";
                        txtDescr.Text = DT.Rows[0]["funderGroup_TEXT"].ToString();
                    }
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
                dtNoRcrd.Columns.Add("RELTYPE");
                dtNoRcrd.Columns.Add("DESCRIPTION");
                dtNoRcrd.Columns.Add("URL");
                dtNoRcrd.Columns.Add("LINK_TEXT");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdAbout.AutoGenerateColumns = false;
                grdAbout.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string Desc = "";
                string LinkText = "";
                string langval = "";
                lblMsg.Visible = false;

                if (txtDescr.Text != "")
                {
                    string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");
                    if (!_result.Equals(""))
                    {
                        MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (Convert.ToString(ddlRelType.SelectedValue) == "region" && pagemode == 7)
                {
                    txtDescr.Text = ddlRegion.SelectedItem.ToString();
                }

                if (txtLinkUrl.Text == "" && !(Convert.ToString(ddlRelType.SelectedValue) == "region" || Convert.ToString(ddlRelType.SelectedValue) == "subRegion" || Convert.ToString(ddlRelType.SelectedValue) == "funderGroup" || pagemode == 7))
                {
                    MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                        string reltype = string.Empty;

                        if (Convert.ToString(ddlRelType.SelectedValue) == "RELTYPE")
                            reltype = "";
                        else
                            reltype = Convert.ToString(ddlRelType.SelectedValue);

                        if (pagemode == 7)
                        {
                            reltype = "identifier";
                        }

                        Desc = txtDescr.Text.Trim();
                        
                        LinkText = txtLinkText.Text.Trim();
                        langval = Convert.ToString(ddlLangOppName.SelectedValue).ToLower();
                        if (r.chk_OtherLang(langval.ToLower()) == true)
                        {
                            Desc = r.ConvertTextToUnicode(Desc);
                        }
                        else
                        {
                            Desc = txtDescr.Text.Trim();
                            LinkText = txtLinkText.Text.Trim();
                        }
                        if ((Desc == "") || (Desc == null))
                        {
                            Desc = ddlRegion.SelectedItem.ToString();
                        }
                        if (pagemode == 1 || pagemode == 2)
                        {
                            LinkText = "NULL";
                        }
                        
                        DataTable dtFundingBody = new DataTable();
                        dtFundingBody.Columns.Add("FUNDINGBODY_ID");
                        dtFundingBody.Columns.Add("AWARDSUCCESSRATE");
                        DataTable dt_fundingdescription = new DataTable();
                        dt_fundingdescription.Columns.Add("WFID");
                        dt_fundingdescription.Columns.Add("PAGEMODE");
                        dt_fundingdescription.Columns.Add("WORKMODE");
                        dt_fundingdescription.Columns.Add("RELTYPE");
                        dt_fundingdescription.Columns.Add("DESCRIPTION");
                        dt_fundingdescription.Columns.Add("URL");
                        dt_fundingdescription.Columns.Add("LINKTEXT");
                        dt_fundingdescription.Columns.Add("LANG");

                        DataRow fbr = dtFundingBody.NewRow();
                        fbr["FUNDINGBODY_ID"] = SharedObjects.ID;
                        dtFundingBody.Rows.Add(fbr);
                        if (pagemode == 4)
                        {
                            fbr = dtFundingBody.NewRow();
                            fbr["AWARDSUCCESSRATE"] =0;
                            dtFundingBody.Rows.Add(fbr);
                        }
                        for (int i = 0; i < grdAbout.Rows.Count; i++)
                        {

                            string RELTYPE = "";
                            if (grdAbout["RelType", 0].Value != null)
                                RELTYPE = grdAbout["RelType", 0].Value.ToString();

                            string DESCRIPTION = "";
                            if (grdAbout["Description", 0].Value != null)
                                DESCRIPTION = grdAbout["Description", 0].Value.ToString();

                            string URL = "";
                            if (grdAbout["Linkurl", 0].Value != null)
                                URL = grdAbout["Linkurl", 0].Value.ToString();

                            string LINKTEXT = "";
                            if (grdAbout["Linktext", 0].Value != null)
                                LINKTEXT = grdAbout["Linktext", 0].Value.ToString();

                            string LANG = "";
                            if (grdAbout["lang", 0].Value != null)
                                LANG = grdAbout["lang", 0].Value.ToString();
                            DataRow dr = dt_fundingdescription.NewRow();
                            dr["WFID"] = WFID;
                            dr["PAGEMODE"] = pagemode;
                            dr["WORKMODE"] = 0;
                            dr["RELTYPE"] = RELTYPE;
                            dr["DESCRIPTION"] = DESCRIPTION;
                            dr["URL"] = URL;
                            dr["LINKTEXT"] = LINKTEXT;
                            dr["LANG"] = Convert.ToString(LANG);
                            dt_fundingdescription.Rows.Add(dr);
                        }
                        DataRow dt = dt_fundingdescription.NewRow();
                        dt["WFID"] = WFID;
                        dt["PAGEMODE"] = pagemode;
                        dt["WORKMODE"] = 0;
                        dt["RELTYPE"] = reltype;
                        dt["DESCRIPTION"] = Desc;
                        dt["URL"] = txtLinkUrl.Text.Trim();
                        dt["LINKTEXT"] = LinkText;
                        dt["LANG"] = Convert.ToString(ddlLangOppName.SelectedValue);
                        dt_fundingdescription.Rows.Add(dt);
                        string json = FundingBodyDataOperations.GetFundingBodyMainJson(SharedObjects.ID);
                        XmlJsonOperation xmlJsonOperation = new XmlJsonOperation();
                        string updatedJSON = xmlJsonOperation.Add_Items_JSON("", dt_fundingdescription, dtFundingBody, json, pagemode);
                        string Loginid = "0"; 

                        Loginid = Convert.ToString(SharedObjects.User.USERID);
                        FundingBodyDataOperations.saveandUpdateJSONinTable(SharedObjects.ID.ToString(), updatedJSON, "","", Convert.ToString(Loginid),DateTime.Now.ToString(), 2);
                         DataSet dsresult = FundingBodyDataOperations.SaveAndDeleteItemsLIst(WFID, pagemode, 0, reltype, Desc, txtLinkUrl.Text.Trim(), LinkText, Convert.ToString(ddlLangOppName.SelectedValue), 0);

                        BindGrid();

                        txtLinkText.Text = "";
                        txtLinkUrl.Text = "";
                        if (!(pagemode == 11 || pagemode == 12))
                        {
                            txtDescr.Text = "";
                        }

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            if (pagemode == 1)
                            {
                                m_parent.GetProcess_update("about");
                            }
                            else if (pagemode == 2)
                            {
                                m_parent.GetProcess_update("funding policy");
                            }
                            else if (pagemode == 3)
                            {
                                m_parent.GetProcess_update("related items");
                            }
                            else if (pagemode == 7)
                            {
                                m_parent.GetProcess_update("region");
                            }
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtLinkUrl.Text = SharedObjects.CurrentUrl.ToString();
        }
        private void grdAbout_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsItems.Tables["ItemListDisplay"];
                            ddlRelType.SelectedValue = Convert.ToString(DT.Rows[rowindex]["reltype"]);
                            txtDescr.Text = Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]);
                            txtLinkUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                            txtLinkText.Text = Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]);
                            ddlRegion.SelectedText = Convert.ToString(DT.Rows[rowindex]["reltype"]);

                            BtnAdd.Visible = false;
                            btnAddURL.Visible = false;

                            btnUpdate.Visible = true;
                            btnCancel.Visible = true;
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
        private void grdAbout_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = FundingBodyDataOperations.SaveAndDeleteItemsLIst(SharedObjects.WorkId, pagemode, 1, null, null, null, null, null, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[grdAbout.SelectedCells[0].RowIndex]["Item_Id"]));
                            BindGrid();                            
                        }

                        m_parent.GetProcess();
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

                string reltype = string.Empty;
                if (txtDescr.Text != "")
                {
                    string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");
                    if (!_result.Equals(""))
                    {
                        MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (Convert.ToString(ddlRelType.SelectedValue) == "region" && pagemode == 7)
                {
                    txtDescr.Text = ddlRegion.SelectedItem.ToString();
                }
                if (Convert.ToString(ddlRelType.SelectedValue) == "RELTYPE")
                    reltype = "";
                else
                    reltype = Convert.ToString(ddlRelType.SelectedValue);

                if (pagemode == 7)
                {
                    reltype = "identifier";
                }
                if (txtLinkUrl.Text == "" && pagemode != 7)
                {
                    MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (txtLinkText.Text == "" && !(pagemode == 3 || pagemode == 4 || pagemode == 7))
                {
                    MessageBox.Show("Please enter Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DataSet dsresult = FundingBodyDataOperations.SaveAndDeleteItemsLIst(SharedObjects.WorkId, pagemode, 2, Convert.ToString(reltype), txtDescr.Text.Trim(), txtLinkUrl.Text.Trim(), txtLinkText.Text.Trim(), Convert.ToString(ddlLangOppName.SelectedValue), Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[rowindex]["Item_Id"]));

                    BindGrid();

                    txtDescr.Text = ""; txtLinkUrl.Text = ""; txtLinkText.Text = "";

                    BtnAdd.Visible = true;
                    btnAddURL.Visible = true;

                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;

                    lblMsg.Visible = true;
                    
                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        if (pagemode == 1)
                        {
                            m_parent.GetProcess_update("about");
                        }
                        else if (pagemode == 2)
                        {
                            m_parent.GetProcess_update("funding policy");
                        }
                        else if (pagemode == 3)
                        {
                            m_parent.GetProcess_update("related items");
                        }
                        else if (pagemode == 7)
                        {
                            m_parent.GetProcess_update("region");
                        }
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (Convert.ToString(pagemode) == "1")
                {
                    ddlRelType.SelectedValue = "about";
                }
                else if (Convert.ToString(pagemode) == "2")
                {
                    ddlRelType.SelectedValue = "applicationInfoFor";
                }

                txtDescr.Text = ""; txtLinkUrl.Text = ""; txtLinkText.Text = "";

                BtnAdd.Visible = true;
                btnAddURL.Visible = true;

                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAddURL_Click(object sender, EventArgs e)
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

        private void grpItem_Enter(object sender, EventArgs e)
        {

        }

        private void grdAbout_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}