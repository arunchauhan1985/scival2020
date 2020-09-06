using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class RelaredOrg : UserControl
    {
        //DAL.Opportunity dalobj = new DAL.Opportunity();
        DataTable dtFunding = new DataTable();
        DataTable tempRelOrgs = new DataTable();
        DataTable temp = new DataTable();

        ErrorLog oErrorLog = new ErrorLog();
        Replace replace = new Replace();
        private Opportunity m_parent;

        Int64 workflowid = 0;

        string InputXmlPath = string.Empty;
        string FBName_difflang = "";

        public RelaredOrg(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void ddlHerchy_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlreltype_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitialValue()
        {
            InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
            lblMsg.Visible = false;

            if (SharedObjects.User.USERID.ToString() == null) { }
            try
            {
                workflowid = Convert.ToInt64(SharedObjects.WorkId);

                //DataSet dsRelatedOrgs = dalobj.GetRelatedOrgs(workflowid);

                //for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                //{
                //    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                //    {
                //        dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                //    }

                //    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                //    {
                //        FBName_difflang = replace.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString());

                //        if (FBName_difflang != "")
                //        {
                //            dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                //        }
                //    }
                //}

                //dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();

                //for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                //{
                //    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                //    {
                //        dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                //    }

                //    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                //    {
                //        FBName_difflang = replace.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString());

                //        if (FBName_difflang != "")
                //        {
                //            dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                //        }
                //    }
                //}

                //dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();

                //tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                //dtFunding = dsRelatedOrgs.Tables["FundingBody"];

                //if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                //{
                //    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                //    {
                //        grdRelOrg.AutoGenerateColumns = false;
                //        grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                //    }
                //    else
                //    {
                //        // Check org details exist or not
                //        DataSet dsRelatedOrgs1 = new DataSet();
                //        DataSet dsAutoLeadRelorgs = dalobj.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(3), Convert.ToInt64(1));

                //        if (dsAutoLeadRelorgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                //        {
                //            if (dsAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0]["hierarchy"] == null)
                //            {
                //                DataSet dsGetAutoLeadRelorgs = dalobj.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(3), Convert.ToInt64(0));
                //            }
                //        }
                //        else
                //        {
                //            string COMPONENT_FUNDINGBODYID_Other = string.Empty;
                //            string hierarchy = "lead";
                //            string totlacHeck = string.Empty;

                //            DataSet dsGetAutoLeadRelorgs = dalobj.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(3), Convert.ToInt64(0));

                //            if (dsGetAutoLeadRelorgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                //            {
                //                COMPONENT_FUNDINGBODYID_Other = Convert.ToString(dsGetAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0]["COMPONENT_FUNDINGBODYID_OTHER"]);

                //                //  In Insert there is no nedd to pass second parameter                   
                //                int iloop = 3;
                //                if (COMPONENT_FUNDINGBODYID_Other == "")
                //                    iloop = 2;

                //                for (int i = 0; i < iloop; i++)
                //                {
                //                    totlacHeck = Convert.ToString(dsGetAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0][i]);
                //                    if (i > 0)
                //                        hierarchy = "component";

                //                    dsRelatedOrgs1 = dalobj.SaveAndDeleteRelatedOrgs(workflowid, 0, hierarchy, totlacHeck, "fundedBy", "");
                //                }
                //            }
                //            else
                //            {
                //                hierarchy = "lead";
                //                DataSet dsFBId = dalobj.GetFBId(Convert.ToInt64(workflowid));
                //                totlacHeck = dsFBId.Tables["FBTable"].Rows[0]["FBID"].ToString();

                //                //  In Insert there is no nedd to pass second parameter                   
                //                dsRelatedOrgs1 = dalobj.SaveAndDeleteRelatedOrgs(workflowid, 0, hierarchy, totlacHeck, "fundedBy", "");
                //            }
                //        }

                //        if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows.Count > 0)
                //        {
                //            for (int intCount = 0; intCount < dsRelatedOrgs1.Tables["RelatedOrgs"].Rows.Count; intCount++)
                //            {
                //                if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                //                {
                //                    FBName_difflang = replace.ConvertUnicodeToText(dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                                    
                //                    if (FBName_difflang != "")
                //                    {
                //                        dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                //                    }
                //                }
                //            }

                //            grdRelOrg.AutoGenerateColumns = false;
                //            grdRelOrg.DataSource = dsRelatedOrgs1.Tables["RelatedOrgs"];
                //        }
                //        else
                //        {
                //            grdRelOrgnorecord();
                //        }

                //        m_parent.GetProcess();
                //    }

                //    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                //    {
                //        grdFundingBody.AutoGenerateColumns = false;
                //        grdFundingBody.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                //        btnRel.Visible = true;
                //    }
                //    else
                //    {
                //        Fundingnorecord();
                //        btnRel.Visible = false; ;
                //    }

                //    DataTable temp = new DataTable();

                //    temp = dsRelatedOrgs.Tables["RelType"];

                //    DataRow dr = temp.NewRow();
                //    dr["VALUE"] = "SelectRelType";
                //    dr["VALUE"] = "--Select RelType--";
                //    temp.Rows.InsertAt(dr, 0);

                //    ddlreltype.DataSource = temp;
                //    ddlreltype.ValueMember = "VALUE";
                //    ddlreltype.DisplayMember = "VALUE";
                //    ddlreltype.SelectedValue = "fundedBy";

                //    temp = dsRelatedOrgs.Tables["Hirarchy"];

                //    dr = temp.NewRow();
                //    dr["HIERARCHY"] = "SelectHirarchy";
                //    dr["HIERARCHY"] = "--Select Hierarchy--";
                //    temp.Rows.InsertAt(dr, 0);

                //    ddlHerchy.DataSource = temp;
                //    ddlHerchy.DisplayMember = "HIERARCHY";
                //    ddlHerchy.ValueMember = "HIERARCHY";

                //    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                //    {
                //        ddlHerchy.SelectedValue = Convert.ToString(dsRelatedOrgs.Tables["FundingBody"].Rows[0]["HIERARCHY"]);
                //    }
                //}
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
                dtNoRcrd.Columns.Add("RELTYPE");
                dtNoRcrd.Columns.Add("FUNDINGBODYNAME");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdRelOrg.AutoGenerateColumns = false;
                grdRelOrg.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void Fundingnorecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("FUNDINGBODYNAME");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdFundingBody.AutoGenerateColumns = false;
                grdFundingBody.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnRel_Click(object sender, EventArgs e)
        {
            try
            {
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                string reltype = ddlreltype.SelectedValue.ToString();
                string hierarchy = ddlHerchy.SelectedValue.ToString();

                if (reltype == "SelectRelType")
                {
                    MessageBox.Show("Please select Rel Type", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (ddlHerchy.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select Hierarchy", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string totlacHeck = string.Empty;
                    string sep = string.Empty;

                    if (ddlHerchy.SelectedIndex != 0)
                        hierarchy = ddlHerchy.SelectedValue.ToString();
                    else
                        hierarchy = "";

                    foreach (DataGridViewCell dr in grdFundingBody.SelectedCells)
                    {
                        if (temp.Rows.Count > 0)
                        {
                            totlacHeck = totlacHeck + sep + ((DataTable)(temp.DefaultView.ToTable())).Rows[dr.RowIndex]["ORGDBID"].ToString();
                            sep = ",";
                        }
                        else
                        {
                            totlacHeck = totlacHeck + sep + dtFunding.Rows[dr.RowIndex]["ORGDBID"].ToString();
                            sep = ",";
                        }
                    }

                    ////  In Insert there is no nedd to pass second parameter                   
                    //DataSet dsRelatedOrgs = dalobj.SaveAndDeleteRelatedOrgs(workflowid, 0, hierarchy, totlacHeck, reltype, "");
                   
                    //for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                    //{
                    //    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    //    {
                    //        dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    //    }

                    //    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                    //    {
                    //        FBName_difflang = replace.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                    //        if (FBName_difflang != "")
                    //        {
                    //            dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                    //        }
                    //    }
                    //}

                    //dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();

                    //dtFunding = dsRelatedOrgs.Tables["FundingBody"];
                    //temp.Rows.Clear();
                    //txtSearch.Text = "";

                    //if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                    //{
                    //    for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                    //    {
                    //        if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    //        {
                    //            dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    //        }

                    //        if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                    //        {
                    //            FBName_difflang = replace.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                    //            if (FBName_difflang != "")
                    //            {
                    //                dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                    //            }
                    //        }
                    //    }

                    //    dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();

                    //    tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];

                    //    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                    //    {
                    //        grdRelOrg.AutoGenerateColumns = false;
                    //        grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                    //    }
                    //    else
                    //    {
                    //        grdRelOrgnorecord();
                    //    }

                    //    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                    //    {
                    //        grdFundingBody.AutoGenerateColumns = false;
                    //        grdFundingBody.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                    //        btnRel.Visible = true; ;
                    //    }
                    //    else
                    //    {
                    //        Fundingnorecord();
                    //        btnRel.Visible = false; ;
                    //    }

                    //    ddlreltype.SelectedValue = "fundedBy";
                    //    ddlHerchy.SelectedIndex = 0;

                    //    #region For Changing Colour in case of Update
                    //    if (SharedObjects.TRAN_TYPE_ID == 1)
                    //    {
                    //        m_parent.GetProcess_update("Relatedfundingbodies");
                    //    }
                    //    else
                    //    {
                    //        m_parent.GetProcess();
                    //    }
                    //    #endregion

                    //    lblMsg.Visible = true;
                    //    lblMsg.Text = dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][1].ToString();
                        
                    //    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());                        
                    //}                    
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void grdRelOrg_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;

                if (tempRelOrgs.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //DataSet dsresult = dalobj.SaveAndDeleteRelatedOrgs(SharedObjects.WorkId, 1, Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["hierarchy"]), Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["ORGDBID"]), "", Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["RELATEDORGS_ID"]));

                            //for (int intCount = 0; intCount < dsresult.Tables["RelatedOrgs"].Rows.Count; intCount++)
                            //{
                            //    if (dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                            //    {
                            //        dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                            //    }
                            //}

                            //dsresult.Tables["RelatedOrgs"].AcceptChanges();

                            //for (int intCount = 0; intCount < dsresult.Tables["FundingBody"].Rows.Count; intCount++)
                            //{
                            //    if (dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                            //    {
                            //        dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = replace.ReadandReplaceHexaToChar(dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                            //    }
                            //}

                            //dsresult.Tables["FundingBody"].AcceptChanges();

                            //dtFunding = dsresult.Tables["FundingBody"];
                            //tempRelOrgs = dsresult.Tables["RelatedOrgs"];

                            //if (dsresult.Tables["RelatedOrgs"].Rows.Count > 0)
                            //{
                            //    grdRelOrg.DataSource = dsresult.Tables["RelatedOrgs"];
                            //}
                            //else
                            //{
                            //    grdRelOrgnorecord();
                            //}

                            //if (dsresult.Tables["FundingBody"].Rows.Count > 0)
                            //{
                            //    grdFundingBody.DataSource = dsresult.Tables["FundingBody"];
                            //}
                            //else
                            //{
                            //    Fundingnorecord();
                            //}

                            //m_parent.GetProcess();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (txtSearch.Text == "")
                {
                    MessageBox.Show("Please enter search keyword.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string srchCond = txtSearch.Text.Trim();
                temp = dtFunding.Copy();
                temp.DefaultView.RowFilter = String.Format("fundingbodyname LIKE '%{0}%'", srchCond.Replace("'", "''"));

                DataTable dtreslt = temp.DefaultView.ToTable();

                if (dtreslt.Rows.Count > 0)
                {
                    grdFundingBody.DataSource = dtreslt;
                    btnRel.Visible = true;
                }
                else
                {
                    Fundingnorecord();
                    btnRel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                temp.Rows.Clear();
                DataTable dtreslt = dtFunding.Copy();

                if (dtreslt.Rows.Count > 0)
                {
                    grdFundingBody.DataSource = dtreslt;
                    btnRel.Visible = true;
                }
                else
                {
                    Fundingnorecord();
                    btnRel.Visible = false;
                }

                txtSearch.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
    }
}
