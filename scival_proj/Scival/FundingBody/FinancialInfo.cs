using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class FinancialInfo : UserControl
    {
        DataTable tempTex = new DataTable();
        DataTable tempFiscal = new DataTable();

        private FundingBody m_parent;
        Int64 UserId = 0; Int64 WFID = 0;
        ErrorLog oErrorLog = new ErrorLog();
        int rowindex = 0;

        public FinancialInfo(FundingBody frm)
        {
            InitializeComponent();
            BindGrid();
            m_parent = frm;
            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
            lblMsg.Visible = false;
        }

        private void ddlIds_MouseHover(object sender, EventArgs e)
        {

        }

        void ddlIds_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void BindGrid()
        {
            if (SharedObjects.User.USERID == 0)
            {
                
            }
            try
            {
                UserId = Convert.ToInt64(SharedObjects.User.USERID);
                WFID = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsTexIds = FundingBodyDataOperations.getTextIdsForddl();

                DataRow dr = dsTexIds.Tables[0].NewRow();
                dr["VALUE"] = "SelectIds";
                dr["VALUE"] = "--Select Tex IDs--";
                dsTexIds.Tables[0].Rows.InsertAt(dr, 0);

                ddlIds.DataSource = dsTexIds.Tables["texid"];
                ddlIds.ValueMember = "VALUE";
                ddlIds.DisplayMember = "VALUE";

                GrdTextBind();

                dsTexIds.Tables.Clear();
                dsTexIds = FundingBodyDataOperations.getFiscal(WFID);
                tempFiscal = dsTexIds.Tables["Fiscal"];
                if (dsTexIds.Tables["Fiscal"].Rows.Count > 0)
                {
                    grdFiscal.AutoGenerateColumns = false;
                    grdFiscal.DataSource = dsTexIds.Tables["Fiscal"];
                }
                else
                {
                    noFiscalrecord();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        public void GrdTextBind()
        {
            try
            {
                DataSet dsTexIds = FundingBodyDataOperations.getTex(WFID);
                tempTex = dsTexIds.Tables["Texs"];
                if (dsTexIds.Tables["Texs"].Rows.Count > 0)
                {
                    dtGridTex.AutoGenerateColumns = false;
                    dtGridTex.DataSource = dsTexIds.Tables["Texs"];
                }
                else
                {
                    noTexrecord();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noTexrecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("TYPE");
                dtNoRcrd.Columns.Add("TAXIDS_TEXT");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                dtGridTex.AutoGenerateColumns = false;
                dtGridTex.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void noFiscalrecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("FISCALCLOSEDATE_COLUMN");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdFiscal.AutoGenerateColumns = false;
                grdFiscal.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url_txtTexDetail = txtTexDetail.Text.TrimStart().TrimEnd();

            if (url_txtTexDetail.Contains("http://") || url_txtTexDetail.Contains("https://") || url_txtTexDetail.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    if (ddlIds.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Tex IDs.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtTexDetail.Text == "")
                    {
                        MessageBox.Show("Please enter Text text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string ddlID = Convert.ToString(ddlIds.SelectedValue);

                        DataSet dsresult = FundingBodyDataOperations.AddandUpdateTexIDS(Convert.ToInt64(SharedObjects.WorkId), ddlID, txtTexDetail.Text.Trim(), 0, Convert.ToInt64(SharedObjects.User.USERID), 0);

                        tempTex = dsresult.Tables["Texs"]; ;
                        if (dsresult.Tables["Texs"].Rows.Count > 0)
                        {
                            dtGridTex.AutoGenerateColumns = false;
                            dtGridTex.DataSource = dsresult.Tables["Texs"];
                            ddlIds.SelectedIndex = 0;
                            txtTexDetail.Text = "";
                        }
                        else
                        {
                            noTexrecord();
                        }
                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess_update("Financial Info");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (datePick.Text == "")
                {
                    MessageBox.Show("Please select fiscal date.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DateTime fiscaldate;
                    if (datePick.Text != "")
                    {
                        fiscaldate = Convert.ToDateTime(datePick.Text.Trim());
                    }
                    else
                    {
                        fiscaldate = new DateTime();
                    }

                    DataSet dsresult = FundingBodyDataOperations.AddandUpdateFiscalDate(WFID, fiscaldate, 0, UserId, 0);

                    tempFiscal = dsresult.Tables["Texs"];
                    if (dsresult.Tables["Texs"].Rows.Count > 0)
                    {
                        grdFiscal.AutoGenerateColumns = false;
                        grdFiscal.DataSource = dsresult.Tables["Texs"];
                    }
                    else
                    {
                        noFiscalrecord();
                    }
                   
                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdFiscal_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (tempFiscal.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DateTime fiscaldate = Convert.ToDateTime(tempFiscal.Rows[grdFiscal.SelectedCells[0].RowIndex]["FISCALCLOSEDATE_COLUMN"]);
                            Int64 financialId = Convert.ToInt64(tempFiscal.Rows[grdFiscal.SelectedCells[0].RowIndex]["FINANCIALINFO_ID"]);

                            DataSet dsresult = FundingBodyDataOperations.AddandUpdateFiscalDate(SharedObjects.WorkId, fiscaldate, 1, Convert.ToInt64(SharedObjects.User.USERID), financialId);


                            tempFiscal = dsresult.Tables["Texs"];
                            if (dsresult.Tables["Texs"].Rows.Count > 0)
                            {
                                grdFiscal.AutoGenerateColumns = false;
                                grdFiscal.DataSource = dsresult.Tables["Texs"];
                            }
                            else
                            {
                                noFiscalrecord();
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

        private void dtGridTex_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (tempTex.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = FundingBodyDataOperations.AddandUpdateTexIDS(SharedObjects.WorkId, Convert.ToString(tempTex.Rows[dtGridTex.SelectedCells[0].RowIndex]["TYPE"]), "", 1, Convert.ToInt64(SharedObjects.User.USERID), Convert.ToInt64(tempTex.Rows[dtGridTex.SelectedCells[0].RowIndex]["FINANCIALINFO_ID"]));

                            GrdTextBind();                            
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

        private void dtGridTex_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (tempTex.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;
                        try
                        {
                            ddlIds.SelectedValue = Convert.ToString(tempTex.Rows[rowindex]["TYPE"]);
                            txtTexDetail.Text = Convert.ToString(tempTex.Rows[rowindex]["TAXIDS_TEXT"]);

                            button1.Visible = false;
                            button2.Visible = false;

                            btnUpdate.Visible = true;
                            btnCancel.Visible = true;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url_txtTexDetail = txtTexDetail.Text.TrimStart().TrimEnd();

            if (url_txtTexDetail.Contains("http://") || url_txtTexDetail.Contains("https://") || url_txtTexDetail.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    if (ddlIds.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Tex IDs.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtTexDetail.Text == "")
                    {
                        MessageBox.Show("Please enter Text text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DataSet dsresult = FundingBodyDataOperations.AddandUpdateTexIDS(SharedObjects.WorkId, Convert.ToString(ddlIds.SelectedValue), Convert.ToString(txtTexDetail.Text.Trim()), 2, Convert.ToInt64(SharedObjects.User.USERID), Convert.ToInt64(tempTex.Rows[dtGridTex.SelectedCells[0].RowIndex]["FINANCIALINFO_ID"]));

                        GrdTextBind();

                        ddlIds.SelectedIndex = 0;
                        txtTexDetail.Text = "";

                        button1.Visible = true;
                        button2.Visible = true;

                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;

                        lblMsg.Visible = true;
                        
                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
                    }
                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Financial Info");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                    #endregion
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
                ddlIds.SelectedIndex = 0;
                txtTexDetail.Text = "";

                button1.Visible = true;
                button2.Visible = true;

                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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
    }
}