using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class RelatedOpportunities : UserControl
    {
        DataTable dtOpportunity = new DataTable();
        DataTable tempRelOpportunity = new DataTable();
        DataTable temp = new DataTable();

        private Opportunity m_parent;
        ErrorLog oErrorLog = new ErrorLog();

        Int64 workflowid = 0;

        public RelatedOpportunities(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void ddlreltype_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitialValue()
        {
            lblMsg.Visible = false;

            try
            {
                workflowid = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsRelatedOpportunity = OpportunityDataOperations.GetRelatedOpportunities(workflowid);
                tempRelOpportunity = dsRelatedOpportunity.Tables["RelatedOpp"];
                dtOpportunity = dsRelatedOpportunity.Tables["Opportunity"];

                if (Convert.ToString(dsRelatedOpportunity.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    if (dsRelatedOpportunity.Tables["RelatedOpp"].Rows.Count > 0)
                    {
                        grdRelOpportunity.AutoGenerateColumns = false;
                        grdRelOpportunity.DataSource = dsRelatedOpportunity.Tables["RelatedOpp"];
                    }
                    else
                    {
                        grdRelOpportunitynorecord();
                    }

                    if (dsRelatedOpportunity.Tables["Opportunity"].Rows.Count > 0)
                    {
                        grdOpportunity.AutoGenerateColumns = false;
                        grdOpportunity.DataSource = dsRelatedOpportunity.Tables["Opportunity"];
                        btnRel.Visible = true;
                    }
                    else
                    {
                        Opportunitynorecord();
                        btnRel.Visible = false; ;
                    }

                    DataTable temp = new DataTable();

                    temp = dsRelatedOpportunity.Tables["RelType"];

                    DataRow dr = temp.NewRow();
                    dr["RELID"] = "0";
                    dr["RELATION_NAME"] = "--Select RelType--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlreltype.DataSource = temp;
                    ddlreltype.ValueMember = "RELID";
                    ddlreltype.DisplayMember = "RELATION_NAME";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdRelOpportunitynorecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("RELTYPE");
                dtNoRcrd.Columns.Add("OPPORTUNITYNAME");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdRelOpportunity.AutoGenerateColumns = false;
                grdRelOpportunity.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void Opportunitynorecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("OPPORTUNITYNAME");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdOpportunity.AutoGenerateColumns = false;
                grdOpportunity.DataSource = dtNoRcrd;
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
                lblMsg.Visible = false;
                int reltype = Convert.ToInt32(ddlreltype.SelectedValue);

                if (reltype == 0)
                {
                    MessageBox.Show("Please select Rel Type", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string totlacHeck = string.Empty;
                string sep = string.Empty;

                foreach (DataGridViewCell dr in grdOpportunity.SelectedCells)
                {
                    if (temp.Rows.Count > 0)
                    {
                        totlacHeck = totlacHeck + sep + ((DataTable)(temp.DefaultView.ToTable())).Rows[dr.RowIndex]["OPPORTUNITYID"].ToString();
                        sep = ",";
                    }
                    else
                    {
                        totlacHeck = totlacHeck + sep + dtOpportunity.Rows[dr.RowIndex]["OPPORTUNITYID"].ToString();
                        sep = ",";
                    }
                }

                //  In Insert there is no nedd to pass second parameter                   
                DataSet dsRelatedOpp = OpportunityDataOperations.SaveAndDeleteRelatedOpps(workflowid, 0, totlacHeck, reltype, txt_Desc.Text.ToString().Trim());
                dtOpportunity = dsRelatedOpp.Tables["Opportunity"];
                temp.Rows.Clear();
                txtSearch.Text = "";
                txt_Desc.Text = "";

                if (Convert.ToString(dsRelatedOpp.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    tempRelOpportunity = dsRelatedOpp.Tables["RelatedOpp"];

                    if (dsRelatedOpp.Tables["RelatedOpp"].Rows.Count > 0)
                    {
                        grdRelOpportunity.AutoGenerateColumns = false;
                        grdRelOpportunity.DataSource = dsRelatedOpp.Tables["RelatedOpp"];
                    }
                    else
                    {
                        grdRelOpportunitynorecord();
                    }

                    if (dsRelatedOpp.Tables["Opportunity"].Rows.Count > 0)
                    {
                        grdOpportunity.AutoGenerateColumns = false;
                        grdOpportunity.DataSource = dsRelatedOpp.Tables["Opportunity"];
                        btnRel.Visible = true; ;
                    }
                    else
                    {
                        Opportunitynorecord();
                        btnRel.Visible = false; ;
                    }

                    ddlreltype.SelectedValue = 0;

                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Relatedpportunities");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                    #endregion

                    lblMsg.Visible = true;
                    lblMsg.Text = dsRelatedOpp.Tables["ERRORCODE"].Rows[0][1].ToString();

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = dsRelatedOpp.Tables["ERRORCODE"].Rows[0][1].ToString();
                }
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
                temp = dtOpportunity.Copy();
                temp.DefaultView.RowFilter = String.Format("Opportunityname LIKE '%{0}%'", srchCond.Replace("'", "''"));

                DataTable dtreslt = temp.DefaultView.ToTable();

                if (dtreslt.Rows.Count > 0)
                {
                    grdOpportunity.DataSource = dtreslt;
                    btnRel.Visible = true;
                }
                else
                {
                    Opportunitynorecord();
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
                DataTable dtreslt = dtOpportunity.Copy();

                if (dtreslt.Rows.Count > 0)
                {
                    grdOpportunity.DataSource = dtreslt;
                    btnRel.Visible = true;
                }
                else
                {
                    Opportunitynorecord();
                    btnRel.Visible = false;
                }

                txtSearch.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdRelOpp_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (tempRelOpportunity.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //  In Insert there is no nedd to pass second parameter
                            DataSet dsresult = OpportunityDataOperations.SaveAndDeleteRelatedOpps(SharedObjects.WorkId, 1, Convert.ToString(tempRelOpportunity.Rows[grdRelOpportunity.SelectedCells[0].RowIndex]["RELATED_OPP_ID"]), null);

                            if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                dtOpportunity = dsresult.Tables["Opportunity"];
                                tempRelOpportunity = dsresult.Tables["RelatedOpp"];

                                if (dsresult.Tables["RelatedOpp"].Rows.Count > 0)
                                {
                                    grdRelOpportunity.DataSource = dsresult.Tables["RelatedOpp"];
                                }
                                else
                                {
                                    grdRelOpportunitynorecord();
                                }

                                if (dsresult.Tables["Opportunity"].Rows.Count > 0)
                                {
                                    grdOpportunity.DataSource = dsresult.Tables["Opportunity"];
                                }
                                else
                                {
                                    Opportunitynorecord();
                                }

                                m_parent.GetProcess();
                            }

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
    }
}
