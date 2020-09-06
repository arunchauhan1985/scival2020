using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class FundedProgramType : UserControl
    {
        DataSet dsFundedProType;
        DataTable TempFundedProType;
        DataTable dtDDLValue = new DataTable();

        private FundingBody m_parent;
        int rowindex = 0;
        ErrorLog oErrorLog = new ErrorLog();

        public FundedProgramType(FundingBody frm)
        {
            InitializeComponent();
            loadInitailValue();
            m_parent = frm;
            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        private void loadInitailValue()
        {
            try
            {
                lblMsg.Visible = false;
                Int64 WfId = Convert.ToInt64(SharedObjects.WorkId);
                dsFundedProType = FundingBodyDataOperations.GetFundedProgramType(WfId);
                TempFundedProType = dsFundedProType.Tables["FundedProgramTypesForDisplay"];
                if (dsFundedProType.Tables["FundedProgramTypesForDisplay"].Rows.Count > 0)
                {
                    grdFDPRoType.AutoGenerateColumns = false;
                    grdFDPRoType.DataSource = dsFundedProType.Tables["FundedProgramTypesForDisplay"];
                }
                else
                {
                    norecord();
                }

                dtDDLValue = dsFundedProType.Tables["FundedProgramTypes"];

                ddlFnProType.DataSource = dtDDLValue;
                ddlFnProType.DisplayMember = "FUNDEDPROGRAMTYPESTEXT";
                ddlFnProType.ValueMember = "TYPEID";
                ddlFnProType.SelectedIndex = -1;
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
                dtNoRcrd.Columns.Add("FUNDEDPROGRAMSTYPE_TEXT");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";
                dtNoRcrd.Rows.Add(dr);
                grdFDPRoType.AutoGenerateColumns = false;
                grdFDPRoType.DataSource = dtNoRcrd;
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
                if (ddlFnProType.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);

                    string ddlID = string.Empty; string sep = string.Empty;

                    for (int i = 0; i < ddlFnProType.SelectedItems.Count; i++)
                    {
                        ddlID = ddlID + sep + ((System.Data.DataRowView)(ddlFnProType.SelectedItems[i])).Row.ItemArray[0].ToString();
                        sep = ",";
                    }

                    dsFundedProType = FundingBodyDataOperations.SaveAndDeleteFundedProType_For_Window(WFID, "0", ddlID, "", 0);

                    TempFundedProType = dsFundedProType.Tables["FundedProgramTypeForDisplay"];
                    if (dsFundedProType.Tables["FundedProgramTypeForDisplay"].Rows.Count > 0)
                    {
                        grdFDPRoType.AutoGenerateColumns = false;
                        grdFDPRoType.DataSource = dsFundedProType.Tables["FundedProgramTypeForDisplay"];
                    }
                    else
                    {
                        norecord();
                    }

                    ddlFnProType.SelectedIndex = -1;
                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Funded Program Types");
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

        private void grdFDPRoType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (TempFundedProType.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            ddlFnProType.SelectedValue = -1;
                            ddlFnProType.DataSource = dtDDLValue;
                            ddlFnProType.DisplayMember = "FUNDEDPROGRAMTYPESTEXT";
                            ddlFnProType.ValueMember = "TYPEID";

                            ddlFnProType.SelectedValue = Convert.ToString(TempFundedProType.Rows[grdFDPRoType.SelectedCells[0].RowIndex]["ID"]);

                            btnsave.Visible = false;
                            btnaddUrl.Visible = false;

                            btnupdate.Visible = true;
                            btncancel.Visible = true;
                        }
                        catch (Exception ex)
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

        private void grdFDPRoType_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (TempFundedProType.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string ddlID = Convert.ToString(TempFundedProType.Rows[grdFDPRoType.SelectedCells[0].RowIndex]["ID"]);
                            string strTotalCheck = Convert.ToString(TempFundedProType.Rows[grdFDPRoType.SelectedCells[0].RowIndex]["FUNDEDPROGRAMSTYPES_ID"]);

                            // In Insert there is no nedd to pass third parameter
                            dsFundedProType = FundingBodyDataOperations.SaveAndDeleteFundedProType(SharedObjects.WorkId, strTotalCheck, ddlID, "", 1);

                            TempFundedProType = dsFundedProType.Tables["FundedProgramTypeForDisplay"];
                            if (dsFundedProType.Tables["FundedProgramTypeForDisplay"].Rows.Count > 0)
                            {
                                grdFDPRoType.DataSource = dsFundedProType.Tables["FundedProgramTypeForDisplay"];
                            }
                            else
                            {
                                norecord();
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

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (ddlFnProType.SelectedValue.ToString() == "SelectFPType")
                {
                    MessageBox.Show("Please select Type.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DataSet dsresult = FundingBodyDataOperations.SaveAndDeleteFundedProType(SharedObjects.WorkId, Convert.ToString(TempFundedProType.Rows[grdFDPRoType.SelectedCells[0].RowIndex]["FUNDEDPROGRAMSTYPES_ID"]), ddlFnProType.SelectedValue.ToString(), Convert.ToString(TempFundedProType.Rows[grdFDPRoType.SelectedCells[0].RowIndex]["ID"]), 2);

                    TempFundedProType = dsresult.Tables["FundedProgramTypeForDisplay"];
                    if (dsresult.Tables["FundedProgramTypeForDisplay"].Rows.Count > 0)
                    {
                        grdFDPRoType.AutoGenerateColumns = false;
                        grdFDPRoType.DataSource = dsresult.Tables["FundedProgramTypeForDisplay"];
                    }
                    else
                    {
                        norecord();
                    }

                    ddlFnProType.SelectedIndex = -1;

                    btnsave.Visible = true;
                    btnaddUrl.Visible = true;

                    btnupdate.Visible = false;
                    btncancel.Visible = false;
                    #region For Changing Colour in case of Update
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Funded Program Types");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                    #endregion
                    lblMsg.Visible = true;

                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            ddlFnProType.SelectedIndex = -1;

            btnsave.Visible = true;
            btnaddUrl.Visible = true;

            btnupdate.Visible = false;
            btncancel.Visible = false;
        }

        private void btnaddUrl_Click(object sender, EventArgs e)
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
