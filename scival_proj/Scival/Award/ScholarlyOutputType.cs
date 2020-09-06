using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Award
{
    public partial class ScholarlyOutputType : UserControl
    {
        private Awards m_parent;
        Int64 WfId = 0;
        int rowindex = 0;
        int mode = 0;
        static Int64 grdrowid = 0;
        DataTable displayData = new DataTable();
        ErrorLog oErrorLog = new ErrorLog();
        public ScholarlyOutputType(Awards frm)
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

                DataSet dsRelatedOrgs =AwardDataOperations.GetScholarlyOutput(WfId);
                displayData = dsRelatedOrgs.Tables["DisplayData"].Copy();
                if (Convert.ToString(dsRelatedOrgs.Tables["ERRORCODE"].Rows[0][0]) == "0")
                {
                    BindGrid();
                    DataTable temp = dsRelatedOrgs.Tables["RelType"].Copy();

                    DataRow dr = temp.NewRow();
                    ddlRelType.DataSource = temp;
                    ddlRelType.DisplayMember = "NAME";
                    ddlRelType.ValueMember = "ID";

                    temp = dsRelatedOrgs.Tables["Type"].Copy();
                    dr = temp.NewRow();
                    dr["ID"] = 0;
                    ddlType.DataSource = temp;
                    ddlType.DisplayMember = "NAME";
                    ddlType.ValueMember = "ID";

                    temp = dsRelatedOrgs.Tables["Item"].Copy();
                    dr = temp.NewRow();
                    dr["ID"] = 0;
                    dr["NAME"] = "--Select Item--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlItem.DataSource = temp;
                    ddlItem.DisplayMember = "NAME";
                    ddlItem.ValueMember = "ID";
                    ddlRelType.SelectedIndex = 1;
                    ddlType.SelectedIndex = 1;
                }
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
                    grdClass.AutoGenerateColumns = false;
                    grdClass.DataSource = DT;
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
                grdClass.DataSource = null;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {


            //pankaj 11 june
            string url_txtItem = txtItem.Text.TrimStart().TrimEnd();
            string url_txtDoi = txtDoi.Text.TrimStart().TrimEnd();
            string url_txtPubmed = txtPubmed.Text.TrimStart().TrimEnd();
            string url_txtPmc = txtPmc.Text.TrimStart().TrimEnd();
            string url_txtMedline = txtMedline.Text.TrimStart().TrimEnd();
            string url_txtScopus = txtScopus.Text.TrimStart().TrimEnd();

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
                    if (txtItem.Text.Trim() == "" && txtDoi.Text.Trim() == "" && txtPubmed.Text.Trim() == "" && txtPmc.Text.Trim() == "" && txtMedline.Text.Trim() == "" && txtScopus.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter atleast one Id.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Int64 hierarchy = Convert.ToInt64(ddlRelType.SelectedValue);
                        string reltype = ddlType.SelectedValue.ToString();
                        string Protext = txtItem.Text.Trim();
                        if (hierarchy == 1)
                        {
                            hierarchyText = "resultedIn";
                        }
                        if (reltype == "1")
                        {
                            reltypeText = "journalArticle";
                        }
                        else if (reltype == "2")
                        {
                            reltypeText = "other";
                        }
                        if (grdClass.RowCount > 0)
                        {
                            mode = 2;
                        }
                        else if (grdClass.RowCount == 0)
                        {
                            mode = 0;
                        }
                        DataSet dsresult = AwardDataOperations.SaveAbdUpdateScholarlyOutputType_new(WfId, mode, 0,
                                        hierarchyText, reltypeText,
                                         txtDoi.Text.Trim(), txtPubmed.Text.Trim(), txtPmc.Text.Trim(), txtMedline.Text.Trim(),
                                         txtScopus.Text.Trim(), txtItem.Text.Trim());
                        
                           displayData = dsresult.Tables["ResearchOutcome"].Copy();
                            if (dsresult.Tables["ResearchOutcome"].Rows.Count > 0)
                            {
                                BindGrid();
                            }
                            else
                            {
                                grdRelOrgnorecord();
                            }
                            hierarchyText = "";
                            reltypeText = "";
                            txtItem.Text = "";
                            txtDoi.Text = "";
                            txtPubmed.Text = "";
                            txtPmc.Text = "";
                            txtMedline.Text = "";
                            txtScopus.Text = "";
                            ddlRelType.SelectedIndex = 0;
                            ddlType.SelectedIndex = 0;
                            ddlItem.SelectedIndex = 0;
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
                        grdrowid = Convert.ToInt64(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["ID"]);
                        string SelectedItemText = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["TYPE"]);
                        if (SelectedItemText == "journalArticle")
                        {
                            ddlType.SelectedValue = 1;
                            ddlType.SelectedIndex = 1;
                        }
                        else if (SelectedItemText == "other")
                        {
                            ddlType.SelectedValue = 2;
                            ddlType.SelectedIndex = 2;
                        }

                        txtDoi.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["doi"]);
                        txtPubmed.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["PUBMEDID"]);
                        txtPmc.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["PMCID"]);
                        txtMedline.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["MEDLINEID"]);
                        txtScopus.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["SCOPUSID"]);
                        txtItem.Text = Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["ITEMID_COLUMN"]);
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
                            DataSet dsresult = AwardDataOperations.SaveAbdUpdateScholarlyOutputType_new(SharedObjects.WorkId, 1, Convert.ToInt64(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["ID"]), "", "", "", "", "", "", "", "");
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
                            lblMsg.Text = "Error"; dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                            txtItem.Text = "";
                            ddlRelType.SelectedIndex = 0;
                            ddlType.SelectedIndex = 0;
                            ddlItem.SelectedIndex = 0;
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
            string url_txtItem = txtItem.Text.TrimStart().TrimEnd();
            string url_txtDoi = txtDoi.Text.TrimStart().TrimEnd();
            string url_txtPubmed = txtPubmed.Text.TrimStart().TrimEnd();
            string url_txtPmc = txtPmc.Text.TrimStart().TrimEnd();
            string url_txtMedline = txtMedline.Text.TrimStart().TrimEnd();
            string url_txtScopus = txtScopus.Text.TrimStart().TrimEnd(); if (url_txtItem.Contains("http://") || url_txtDoi.Contains("http://") || url_txtPubmed.Contains("http://") || url_txtPmc.Contains("http://") || url_txtMedline.Contains("http://") || url_txtScopus.Contains("http://") ||
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
                    if (txtItem.Text.Trim() == "" && txtDoi.Text.Trim() == "" && txtPubmed.Text.Trim() == "" && txtPmc.Text.Trim() == "" && txtMedline.Text.Trim() == "" && txtScopus.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter atleast one Id.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Int64 hierarchy = Convert.ToInt64(ddlRelType.SelectedValue);
                        string reltype = ddlType.SelectedValue.ToString();
                        string Protext = txtItem.Text.Trim();
                        if (hierarchy == 1)
                        {
                            hierarchyText = "resultedIn";
                        }
                        if (reltype == "1")
                        {
                            reltypeText = "journalArticle";
                        }
                        else if (reltype == "2")
                        {
                            reltypeText = "other";
                        }
                        if (grdClass.RowCount > 0)
                        {
                            mode = 2;
                        }
                        else if (grdClass.RowCount == 0)
                        {
                            mode = 0;
                        }
                        DataSet dsresult = AwardDataOperations.SaveAbdUpdateScholarlyOutputType_new(WfId, mode, grdrowid,
                                       hierarchyText, reltypeText,
                                        txtDoi.Text.Trim(), txtPubmed.Text.Trim(), txtPmc.Text.Trim(), txtMedline.Text.Trim(),
                                        txtScopus.Text.Trim(), txtItem.Text.Trim());

                        
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
                            txtItem.Text = "";
                            ddlRelType.SelectedIndex = 0;
                            ddlType.SelectedIndex = 0;
                            ddlItem.SelectedIndex = 0;
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
                        lblMsg.Text = "Error";//dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
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
                ddlType.SelectedIndex = 0;
                ddlRelType.SelectedIndex = 0;
                ddlItem.SelectedIndex = 0;

                txtItem.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

        }


    }
}
