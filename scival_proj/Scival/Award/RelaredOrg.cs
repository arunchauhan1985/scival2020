using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySqlDal;

namespace Scival.Award
{
    public partial class RelaredOrg : UserControl
    {
        Replace r = new Replace();
        string InputXmlPath = string.Empty;
        private Awards m_parent;
        Int64 workflowid = 0;
        DataTable dtFunding = new DataTable();
        DataTable tempRelOrgs = new DataTable();
        DataTable temp = new DataTable();
        ErrorLog oErrorLog = new ErrorLog();
        string FBName_difflang = "";

        string relatedorgsid = string.Empty;
        int rowindex = 0;
        DataSet dsRelatedOrgs = new DataSet();

        Int64 pagemode = 0;

        DataSet dsRelatedOrgs1 = new DataSet();

        public RelaredOrg(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";
            SharedObjects.Relatedorgs_ORGDBIDUdateID = "";
            SharedObjects.RelatedorgsUdateID = "";


            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);

            Control obj = this.Parent;

        }



        void ddlCuurency_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
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
            if (SharedObjects.User.USERID.ToString() == null)
            {
            }
            try
            {
                workflowid = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsItems = AwardDataOperations.GetAmount(workflowid, pagemode);

                DataTable tempcur = dsItems.Tables["Currency"].Copy();

                DataRow drcur = tempcur.NewRow();
                drcur["Code"] = "SelectCurrency";
                drcur["Value"] = "--Select Currency--";
                tempcur.Rows.InsertAt(drcur, 0);

                ddlCuurency.DataSource = tempcur;
                ddlCuurency.DisplayMember = "Value";
                ddlCuurency.ValueMember = "Code";
                dsRelatedOrgs = AwardDataOperations.GetRelatedOrgs(workflowid);
                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    }

                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                    {
                        FBName_difflang = r.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                        if (FBName_difflang != "")
                        {
                            dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                        }
                    }
                }
                dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();
                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    }

                    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                    {
                        FBName_difflang = r.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                        if (FBName_difflang != "")
                        {
                            dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                        }
                    }
                }
                dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();

                tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                dtFunding = dsRelatedOrgs.Tables["FundingBody"];
                if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                    {
                        grdRelOrg.AutoGenerateColumns = false;
                        grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                    }
                    else
                    {
                        DataSet dsAutoLeadRelorgs = AwardDataOperations.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(4), Convert.ToInt64(1));
                        if (dsAutoLeadRelorgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            if (dsAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0]["hierarchy"] == null)
                            {
                                DataSet dsGetAutoLeadRelorgs = AwardDataOperations.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(4), Convert.ToInt64(0));
                                if (dsGetAutoLeadRelorgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "1")
                                {
                                }
                            }
                        }
                        else
                        {
                            string COMPONENT_FUNDINGBODYID_Other = string.Empty;
                            string hierarchy = "lead";
                            string totlacHeck = string.Empty;


                            DataSet dsGetAutoLeadRelorgs = AwardDataOperations.GetAutoLeadRelorgs(Convert.ToInt64(workflowid), Convert.ToInt64(4), Convert.ToInt64(0));
                            if (dsGetAutoLeadRelorgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                COMPONENT_FUNDINGBODYID_Other = Convert.ToString(dsGetAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0]["COMPONENT_FUNDINGBODYID_OTHER"]);

                                //  In Insert there is no nedd to pass second parameter                   
                                int iloop = 3;
                                if (COMPONENT_FUNDINGBODYID_Other == "")
                                    iloop = 2;

                                for (int i = 0; i < iloop; i++)
                                {
                                    totlacHeck = Convert.ToString(dsGetAutoLeadRelorgs.Tables["AutoLeadDetail"].Rows[0][i]);
                                    if (i > 0)
                                        hierarchy = "component";
                                    dsRelatedOrgs1 = AwardDataOperations.SaveAndDeleteRelatedOrgs(workflowid, "", "", 0, hierarchy, totlacHeck, "fundedBy", relatedorgsid);
                                }
                            }
                            else
                            {
                                hierarchy = "lead";
                                DataSet dsFBId = AwardDataOperations.GetFBId(workflowid);
                                //totlacHeck = dsFBId.Tables["FBTable"].Rows[0]["FBID"].ToString();
                                dsRelatedOrgs1 = AwardDataOperations.SaveAndDeleteRelatedOrgs(workflowid, "", "", 0, hierarchy, totlacHeck, "fundedBy", relatedorgsid);

                                
                                    if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows.Count > 0)
                                    {
                                        for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                                        {
                                            if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                            {
                                                dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                            }

                                            if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                                            {
                                                FBName_difflang = r.ConvertUnicodeToText(dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                                                if (FBName_difflang != "")
                                                {
                                                    dsRelatedOrgs1.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                                                }
                                            }
                                        }
                                        grdRelOrg.AutoGenerateColumns = false;
                                        grdRelOrg.DataSource = dsRelatedOrgs1.Tables["RelatedOrgs"];
                                    }
                                    else
                                    {
                                        grdRelOrgnorecord();
                                    }
                                    m_parent.GetProcess();                               
                            } 
                        }

                        if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows.Count > 0)
                        {
                            grdRelOrg.AutoGenerateColumns = false;
                            grdRelOrg.DataSource = dsRelatedOrgs1.Tables["RelatedOrgs"];
                        }
                        else
                        {
                            grdRelOrgnorecord();
                        }
                        m_parent.GetProcess();
                    }

                    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                    {
                        grdFundingBody.AutoGenerateColumns = false;
                        grdFundingBody.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                        btnRel.Visible = true;
                    }
                    else
                    {
                        Fundingnorecord();
                        btnRel.Visible = false; ;
                    }


                    DataTable temp = new DataTable();

                    temp = dsRelatedOrgs.Tables["RelType"];

                    DataRow dr = temp.NewRow();
                    dr["VALUE"] = "SelectRelType";
                    dr["VALUE"] = "--Select RelType--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlreltype.DataSource = temp;
                    ddlreltype.ValueMember = "VALUE";
                    ddlreltype.DisplayMember = "VALUE";

                    ddlreltype.SelectedValue = "fundedBy";

                    temp = dsRelatedOrgs.Tables["Hirarchy"];

                    dr = temp.NewRow();
                    dr["HIERARCHY"] = "SelectHirarchy";
                    dr["HIERARCHY"] = "--Select Hierarchy--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlHerchy.DataSource = temp;
                    ddlHerchy.DisplayMember = "HIERARCHY";
                    ddlHerchy.ValueMember = "HIERARCHY";

                    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                    {
                        ddlHerchy.SelectedValue = Convert.ToString(dsRelatedOrgs.Tables["FundingBody"].Rows[0]["HIERARCHY"]);
                    }

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
            //pankaj 11 june
            string url_txtAmount = txtAmount.Text.TrimStart().TrimEnd();

            if (url_txtAmount.Contains("http://") || url_txtAmount.Contains("https://") || url_txtAmount.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
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
                        //  In Insert there is no nedd to pass second parameter    
                        string currency = Convert.ToString(ddlCuurency.SelectedValue);
                        DataSet dsRelatedOrgs = AwardDataOperations.SaveAndDeleteRelatedOrgs(workflowid, txtAmount.Text.ToString().TrimStart().TrimEnd(), currency, 0, hierarchy, totlacHeck, reltype, relatedorgsid);
                        for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                        {
                            if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                            {
                                dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                            }


                            if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                            {
                                FBName_difflang = r.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString());
                                if (FBName_difflang != "")
                                {
                                    dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                                }
                            }

                        }
                        dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();

                        dtFunding = dsRelatedOrgs.Tables["FundingBody"];
                        temp.Rows.Clear();
                        txtSearch.Text = "";
                        if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                            {
                                if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                {
                                    dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                }
                            }
                            dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();

                            if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                            {
                                tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                                grdRelOrg.AutoGenerateColumns = false;
                                grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                            }
                            else
                            {
                                grdRelOrgnorecord();
                            }

                            if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                            {
                                grdFundingBody.AutoGenerateColumns = false;
                                grdFundingBody.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                                btnRel.Visible = true; ;
                            }
                            else
                            {
                                Fundingnorecord();
                                btnRel.Visible = false; ;
                            }

                            ddlreltype.SelectedValue = "fundedBy";
                            ddlHerchy.SelectedIndex = 0;
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Related Fundingbodies");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                            lblMsg.Visible = true;
                            lblMsg.Text = Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][1]);
                            if (dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show(Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][1]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }


                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
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
                            DataSet dsresult = AwardDataOperations.SaveAndDeleteRelatedOrgs(SharedObjects.WorkId, "", "", 1, Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["hierarchy"]), Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["ORGDBID"]), "", Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["RELATEDORGS_ID"]));

                                for (int intCount = 0; intCount < dsresult.Tables["RelatedOrgs"].Rows.Count; intCount++)
                                {
                                    if (dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                    {
                                        dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                    }
                                }
                                dsresult.Tables["RelatedOrgs"].AcceptChanges();

                                for (int intCount = 0; intCount < dsresult.Tables["FundingBody"].Rows.Count; intCount++)
                                {
                                    if (dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                    {
                                        dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                    }
                                }
                                dsresult.Tables["FundingBody"].AcceptChanges();

                                dtFunding = dsresult.Tables["FundingBody"];
                                tempRelOrgs = dsresult.Tables["RelatedOrgs"];
                                if (dsresult.Tables["RelatedOrgs"].Rows.Count > 0)
                                {
                                    grdRelOrg.DataSource = dsresult.Tables["RelatedOrgs"];

                                }
                                else
                                {
                                    grdRelOrgnorecord();
                                }

                                if (dsresult.Tables["FundingBody"].Rows.Count > 0)
                                {
                                    grdFundingBody.DataSource = dsresult.Tables["FundingBody"];
                                }
                                else
                                {
                                    Fundingnorecord();

                                }
                           

                            m_parent.GetProcess();
                            lblMsg.Visible = true;
                            lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //pankaj 11 june
            string url_txtAmount = txtAmount.Text.TrimStart().TrimEnd();

            if (url_txtAmount.Contains("http://") || url_txtAmount.Contains("https://") || url_txtAmount.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
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
                        totlacHeck = SharedObjects.Relatedorgs_ORGDBIDUdateID.ToString();
                        relatedorgsid = SharedObjects.RelatedorgsUdateID.ToString();
                        string currency = Convert.ToString(ddlCuurency.SelectedValue);
                        DataSet dsRelatedOrgs = AwardDataOperations.SaveAndDeleteRelatedOrgs(workflowid, txtAmount.Text.ToString().TrimStart().TrimEnd(), currency, 2, hierarchy, totlacHeck, reltype, relatedorgsid);

                        dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();

                        dtFunding = dsRelatedOrgs.Tables["FundingBody"];
                        temp.Rows.Clear();
                        txtSearch.Text = "";
                        if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                            {
                                if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                {
                                    dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                }
                            }
                            dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();

                            if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                            {
                                tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                                grdRelOrg.AutoGenerateColumns = false;
                                grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                            }
                            else
                            {
                                grdRelOrgnorecord();
                            }

                            if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                            {
                                grdFundingBody.AutoGenerateColumns = false;
                                grdFundingBody.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                                btnRel.Visible = true; ;
                            }
                            else
                            {
                                Fundingnorecord();
                                btnRel.Visible = false; ;
                            }

                            ddlreltype.SelectedValue = "fundedBy";
                            ddlHerchy.SelectedIndex = 0;

                            // m_parent.GetProcess(); commneted on 13-JUne-2019
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Related Fundingbodies");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                            lblMsg.Visible = true;
                            lblMsg.Text = Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][1]);

                            //Pankaj start track TrackUnstoppedAward
                            if (dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                            //End track TrackUnstoppedAward
                        }
                        else
                        {
                            MessageBox.Show(Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][1]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }


                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                btnRel.Visible = true;


                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdRelOrg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdRelOrg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //pankaj
            try
            {
                lblMsg.Visible = false;
                if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {

                        rowindex = e.RowIndex;
                        DataTable dt = dsRelatedOrgs.Tables["RelatedOrgs"];
                        ddlHerchy.SelectedValue = Convert.ToString(dt.Rows[rowindex]["HIERARCHY"]);
                        ddlCuurency.SelectedValue = Convert.ToString(dt.Rows[rowindex]["Currency"]);
                        ddlreltype.SelectedValue = Convert.ToString(dt.Rows[rowindex]["RELTYPE"]);
                        txtAmount.Text = Convert.ToString(dt.Rows[rowindex]["Amount"]);

                        SharedObjects.Relatedorgs_ORGDBIDUdateID = Convert.ToString(dt.Rows[rowindex]["ORGDBID"]);
                        SharedObjects.RelatedorgsUdateID = Convert.ToString(dt.Rows[rowindex]["RELATEDORGS_ID"]);


                        btnUpdate.Visible = true;
                        btnCancel.Visible = true;
                    }
                }

                else if (dsRelatedOrgs1.Tables["RelatedOrgs"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {

                        rowindex = e.RowIndex;
                        DataTable dt = dsRelatedOrgs1.Tables["RelatedOrgs"];
                        ddlHerchy.SelectedValue = Convert.ToString(dt.Rows[rowindex]["HIERARCHY"]);
                        ddlCuurency.SelectedValue = Convert.ToString(dt.Rows[rowindex]["Currency"]);
                        ddlreltype.SelectedValue = Convert.ToString(dt.Rows[rowindex]["RELTYPE"]);
                        txtAmount.Text = Convert.ToString(dt.Rows[rowindex]["Amount"]);

                        SharedObjects.Relatedorgs_ORGDBIDUdateID = Convert.ToString(dt.Rows[rowindex]["ORGDBID"]);
                        SharedObjects.RelatedorgsUdateID = Convert.ToString(dt.Rows[rowindex]["RELATEDORGS_ID"]);


                        btnUpdate.Visible = true;
                        btnCancel.Visible = true;
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


    }
}
