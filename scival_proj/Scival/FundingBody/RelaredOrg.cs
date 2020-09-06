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

namespace Scival.FundingBody
{
    public partial class RelaredOrg : UserControl
    {
        //DAL.FundingBody dalobj = new DAL.FundingBody();
        DataTable dtFunding = new DataTable();
        DataTable tempRelOrgs = new DataTable();
        DataTable temp = new DataTable();
        DataTable dtVendorFunding = new DataTable();
        DataTable VendorTemp = new DataTable();

        Replace r = new Replace();
        private FundingBody m_parent;
        Int64 workflowid = 0;
        string InputXmlPath = string.Empty;
        string FBName_difflang = "";
        ErrorLog oErrorLog = new ErrorLog();
        public RelaredOrg(FundingBody frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
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
        void ddlVreltype_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitialValue()
        {
            InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
            lblMsg.Visible = false;
            FBName_difflang = "";
            if (SharedObjects.User.USERID == 0)
            {

            }
            try
            {
                workflowid = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsRelatedOrgs = FundingBodyDataOperations.GetRelatedOrgs(workflowid);

                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                        if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                        {
                            FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString()));
                            if (FBName_difflang != "")
                            {
                                dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                            }
                        }
                    }
                    dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount].AcceptChanges();
                }
                dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();
                dtFunding = dsRelatedOrgs.Tables["RelatedOrgs"];

                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);

                        if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                        {
                            FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString()));
                            if (FBName_difflang != "")
                            {
                                dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                            }
                        }
                    }

                    dsRelatedOrgs.Tables["FundingBody"].Rows[intCount].AcceptChanges();
                }
                dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();
                tempRelOrgs = dsRelatedOrgs.Tables["FundingBody"];


                if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                {
                    grdRelOrg.AutoGenerateColumns = false;
                    grdRelOrg.DataSource = dsRelatedOrgs.Tables["FundingBody"];
                }
                else
                {
                    grdRelOrgnorecord();
                }

                if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                {
                    grdFundingBody.AutoGenerateColumns = false;
                    grdFundingBody.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                    btnRel.Visible = true;
                }
                else
                {
                    Fundingnorecord();
                    btnRel.Visible = false; ;
                }

                DataSet dsVendorFB = FundingBodyDataOperations.GetVendorFundingbody(null, null, "", "V");
                for (int intCount = 0; intCount < dsVendorFB.Tables["VendorFundingbody"].Rows.Count; intCount++)
                {
                    if (dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_FUNDINGBODY_NAME"].ToString() != "")
                    {
                        dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_FUNDINGBODY_NAME"] = r.ReadandReplaceHexaToChar(dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_FUNDINGBODY_NAME"].ToString(), InputXmlPath);
                    }
                }
                dsVendorFB.Tables["VendorFundingbody"].AcceptChanges();
                dtVendorFunding = dsVendorFB.Tables["VendorFundingbody"];

                if (dsVendorFB.Tables["VendorFundingbody"].Rows.Count > 0)
                {
                    grdVendorFundingBody.AutoGenerateColumns = false;
                    grdVendorFundingBody.DataSource = dsVendorFB.Tables["VendorFundingbody"];
                    btnVRel.Visible = true;
                }
                else
                {
                    VendorFundingnorecord();
                    btnVRel.Visible = false;
                }

                DataTable temp = new DataTable();
                temp = dsRelatedOrgs.Tables["RelatedOrgs"];
                //DataRow dr ;
                ddlreltype.Items.Add("SelectRelType");
                //dr = temp.NewRow();
                //dr["VALUE"] = "SelectRelType";
                //dr["VALUE"] = "--Select RelType--";
                //temp.Rows.InsertAt(dr, 0);
                if (temp != null)
                {
                    foreach (DataRow row in temp.Rows)
                    {
                        if (!ddlreltype.Items.Contains(row["RelType"].ToString()))
                        {
                            ddlreltype.Items.Add(row["RelType"].ToString());
                        }
                    }
                }
                ddlVreltype.Items.Add("VALUE");
                ddlVreltype.Items.Add("fundedBy");

                temp = dsRelatedOrgs.Tables["RelatedOrgs"];
                ddlHerchy.Items.Add("SelectHirarchy");
                if (temp != null)
                {
                    foreach (DataRow row in temp.Rows)
                    {
                        if (!ddlHerchy.Items.Contains(row["HIERARCHY"].ToString()))
                        {
                            ddlHerchy.Items.Add(row["HIERARCHY"].ToString());
                        }
                    }
                }
                try
                {
                    if (dsRelatedOrgs.Tables["FundingBody"].Rows.Count > 0)
                    {
                        if (Convert.ToString(dsRelatedOrgs.Tables["FundingBody"].Rows[0]["HIERARCHY"]) == "")
                        {
                            ddlHerchy.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlHerchy.SelectedValue = Convert.ToString(dsRelatedOrgs.Tables["FundingBody"].Rows[0]["HIERARCHY"]);
                        }
                    }
                }
                catch (Exception ex)
                { }
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
        private void VendorFundingnorecord()
        {
            try
            {
                DataTable DT = new DataTable();
                DT.Columns.Add(new DataColumn("vendor_id"));
                DT.Columns.Add(new DataColumn("vendor_fundingbody_name"));
                DataRow DR = DT.NewRow();
                DR[1] = "No Record(s) found !";
                DT.Rows.Add(DR);

                grdVendorFundingBody.DataSource = DT;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnHierachy_Click(object sender, EventArgs e)
        {
            try
            {
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                string strHierarchy = string.Empty;

                if (ddlHerchy.SelectedValue.ToString() != "SelectHirarchy")
                    strHierarchy = ddlHerchy.SelectedValue.ToString();
                else
                    strHierarchy = "";

                DataSet dsRelatedOrgs = FundingBodyDataOperations.SaveAndDeleteRelatedOrgs(workflowid, 2, strHierarchy, "", "");

                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    }
                }
                dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();
                dtFunding = dsRelatedOrgs.Tables["FundingBody"];

                for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                {
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                    {
                        dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                    }
                }
                dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();
                tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                {
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

                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
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
                FBName_difflang = "";
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblMsg.Visible = false;
                string reltype = ddlreltype.SelectedValue.ToString();

                if (reltype == "SelectRelType")
                {
                    MessageBox.Show("Please select Rel Type", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string totlacHeck = string.Empty;
                    string sep = string.Empty;
                    string hierarchy = string.Empty;

                    if (ddlHerchy.SelectedIndex != 0)
                        hierarchy = ddlHerchy.SelectedValue.ToString();
                    else
                        hierarchy = null;

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
                    DataSet dsRelatedOrgs = FundingBodyDataOperations.SaveAndDeleteRelatedOrgs(workflowid, 0, hierarchy, totlacHeck, reltype);

                    for (int intCount = 0; intCount < dsRelatedOrgs.Tables["FundingBody"].Rows.Count; intCount++)
                    {
                        if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                        {
                            dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);

                            if (dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                            {
                                FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString()));
                                if (FBName_difflang != "")
                                {
                                    dsRelatedOrgs.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                                }
                            }
                        }
                    }
                    dsRelatedOrgs.Tables["FundingBody"].AcceptChanges();
                    dtFunding = dsRelatedOrgs.Tables["FundingBody"];
                    temp.Rows.Clear();
                    txtSearch.Text = "";

                    for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                    {
                        if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                        {
                            dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);

                            if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                            {
                                FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString()));
                                if (FBName_difflang != "")
                                {
                                    dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                                }
                            }
                        }
                    }
                    dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();
                    tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                    {
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

                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Related Orgs");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
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
                            DataSet dsresult = FundingBodyDataOperations.SaveAndDeleteRelatedOrgs(SharedObjects.WorkId, 1, "", Convert.ToString(tempRelOrgs.Rows[grdRelOrg.SelectedCells[0].RowIndex]["ORGDBID"]), "");

                            for (int intCount = 0; intCount < dsresult.Tables["FundingBody"].Rows.Count; intCount++)
                            {
                                if (dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                {
                                    dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsresult.Tables["FundingBody"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                }
                            }
                            dsresult.Tables["FundingBody"].AcceptChanges();
                            dtFunding = dsresult.Tables["FundingBody"];

                            for (int intCount = 0; intCount < dsresult.Tables["RelatedOrgs"].Rows.Count; intCount++)
                            {
                                if (dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                                {
                                    dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsresult.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);
                                }
                            }
                            dsresult.Tables["RelatedOrgs"].AcceptChanges();
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
                    temp.DefaultView.RowFilter = ("ORGDBID =" + srchCond);
                    dtreslt = temp.DefaultView.ToTable();
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

        private void btnVRel_Click(object sender, EventArgs e)
        {
            try
            {
                FBName_difflang = "";
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                lblVMsg.Visible = false;
                string VReltype = ddlVreltype.SelectedValue.ToString();

                if (VReltype == "SelectRelType")
                {
                    MessageBox.Show("Please select Rel Type", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string totlacHeck = string.Empty;
                    string sep = string.Empty;
                    string hierarchy = string.Empty;

                    if (ddlHerchy.SelectedIndex != 0)
                        hierarchy = ddlHerchy.SelectedValue.ToString();
                    else
                        hierarchy = null;

                    foreach (DataGridViewCell dr in grdVendorFundingBody.SelectedCells)
                    {
                        if (VendorTemp.Rows.Count > 0)
                        {
                            totlacHeck = totlacHeck + sep + ((DataTable)(VendorTemp.DefaultView.ToTable())).Rows[dr.RowIndex]["VENDOR_ID"].ToString();
                            sep = ",";
                        }
                        else
                        {
                            totlacHeck = totlacHeck + sep + dtVendorFunding.Rows[dr.RowIndex]["VENDOR_ID"].ToString();
                            sep = ",";
                        }
                    }
                    DataSet dsRelatedOrgs = FundingBodyDataOperations.SaveAndDeleteRelatedOrgs(workflowid, 0, hierarchy, totlacHeck, VReltype);
                    DataSet dsVendorFB = FundingBodyDataOperations.GetVendorFundingbody(null, null, "", "V");
                    for (int intCount = 0; intCount < dsVendorFB.Tables["VendorFundingbody"].Rows.Count; intCount++)
                    {
                        if (dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"].ToString() != "")
                        {
                            dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"] = r.ReadandReplaceHexaToChar(dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"].ToString(), InputXmlPath);
                            if (dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"].ToString().Contains("|"))
                            {
                                FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"].ToString()));
                                if (FBName_difflang != "")
                                {
                                    dsVendorFB.Tables["VendorFundingbody"].Rows[intCount]["Vendor_Fundingbody_Name"] = FBName_difflang;
                                }
                            }
                        }
                    }
                    dsVendorFB.Tables["VendorFundingbody"].AcceptChanges();
                    dtVendorFunding = dsVendorFB.Tables["VendorFundingbody"];
                    VendorTemp.Rows.Clear();
                    txtSearch.Text = "";
                    for (int intCount = 0; intCount < dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count; intCount++)
                    {
                        if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString() != "")
                        {
                            dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = r.ReadandReplaceHexaToChar(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString(), InputXmlPath);

                            if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString().Contains("|"))
                            {
                                FBName_difflang = Convert.ToString(r.ConvertUnicodeToText(dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"].ToString()));
                                if (FBName_difflang != "")
                                {
                                    dsRelatedOrgs.Tables["RelatedOrgs"].Rows[intCount]["FUNDINGBODYNAME"] = FBName_difflang;
                                }
                            }
                        }
                    }
                    dsRelatedOrgs.Tables["RelatedOrgs"].AcceptChanges();
                    tempRelOrgs = dsRelatedOrgs.Tables["RelatedOrgs"];
                    if (dsRelatedOrgs.Tables["RelatedOrgs"].Rows.Count > 0)
                    {
                        grdRelOrg.AutoGenerateColumns = false;
                        grdRelOrg.DataSource = dsRelatedOrgs.Tables["RelatedOrgs"];
                    }
                    else
                    {
                        grdRelOrgnorecord();
                    }

                    if (dsVendorFB.Tables["VendorFundingbody"].Rows.Count > 0)
                    {
                        grdVendorFundingBody.AutoGenerateColumns = false;
                        grdVendorFundingBody.DataSource = dsVendorFB.Tables["VendorFundingbody"];
                        btnVRel.Visible = true; ;
                    }
                    else
                    {
                        VendorFundingnorecord();
                        btnVRel.Visible = false; ;
                    }

                    ddlVreltype.SelectedValue = "fundedBy";

                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Related Orgs");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnVAddurl_Click(object sender, EventArgs e)
        {
            lblVMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);
        }

        private void btnVSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblVMsg.Visible = false;
                if (txtVSearch.Text == "")
                {
                    MessageBox.Show("Please enter search keyword.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string srchCond = txtVSearch.Text.Trim();
                VendorTemp = dtVendorFunding.Copy();
                VendorTemp.DefaultView.RowFilter = String.Format("Vendor_FUNDINGBODY_NAME LIKE '%{0}%'", srchCond.Replace("'", "''"));
                DataTable dtreslt = VendorTemp.DefaultView.ToTable();

                if (dtreslt.Rows.Count > 0)
                {
                    grdVendorFundingBody.DataSource = dtreslt;
                    btnVRel.Visible = true;
                }
                else
                {
                    VendorTemp.DefaultView.RowFilter = ("Vendor_ID =" + srchCond);
                    dtreslt = VendorTemp.DefaultView.ToTable();
                    if (dtreslt.Rows.Count > 0)
                    {
                        grdVendorFundingBody.DataSource = dtreslt;
                        btnVRel.Visible = true;
                    }
                    else
                    {
                        VendorFundingnorecord();
                        btnVRel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnVReset_Click(object sender, EventArgs e)
        {
            try
            {
                lblVMsg.Visible = false;
                VendorTemp.Rows.Clear();
                DataTable dtreslt = dtVendorFunding.Copy();
                if (dtreslt.Rows.Count > 0)
                {
                    grdVendorFundingBody.DataSource = dtreslt;
                    btnVRel.Visible = true;
                }
                else
                {
                    VendorFundingnorecord();
                    btnVRel.Visible = false;
                }

                txtVSearch.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}