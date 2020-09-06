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
    public partial class RelatedProgram : UserControl
    {

        private Awards m_parent;
        Int64 WfId = 0;
        int rowindex = 0;
        DataTable displayData = new DataTable();
        ErrorLog oErrorLog = new ErrorLog();
        public RelatedProgram(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            loadInitialValue();

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
        private void loadInitialValue()
        {

            try
            {
                lblMsg.Visible = false;
                WfId = SharedObjects.WorkId;

                DataSet dsRelatedOrgs = AwardDataOperations.GetRelatedPrograms(WfId);
                displayData = dsRelatedOrgs.Tables["DisplayData"].Copy();

                if (dsRelatedOrgs.Tables.Count>0)
                {
                    BindGrid();

                    DataTable temp = dsRelatedOrgs.Tables["RelType"].Copy();

                    DataRow dr = temp.NewRow();
                    dr["VALUE"] = "SelectRelType";
                    dr["VALUE"] = "--Select RelType--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlreltype.DataSource = temp;
                    ddlreltype.DisplayMember = "VALUE";
                    ddlreltype.ValueMember = "VALUE";

                    temp = dsRelatedOrgs.Tables["Hierarchy"].Copy();
                    dr = temp.NewRow();
                    dr["HIERARCHY"] = "SelectHierarchy";
                    dr["HIERARCHY"] = "--Select Hierarchy--";
                    temp.Rows.InsertAt(dr, 0);

                    ddlHerchy.DataSource = temp;
                    ddlHerchy.DisplayMember = "HIERARCHY";
                    ddlHerchy.ValueMember = "HIERARCHY";

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
                dtNoRcrd.Columns.Add("RELTYPE");
                dtNoRcrd.Columns.Add("HIERARCHY");
                dtNoRcrd.Columns.Add("ID");
                dtNoRcrd.Columns.Add("RELATEDPROGRAM_TEXT");

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

            //pankaj 11 june
            string url_txtId = txtId.Text.TrimStart().TrimEnd();
            string url_txtProText = txtProText.Text.TrimStart().TrimEnd();

            if (url_txtId.Contains("http://") || url_txtProText.Contains("http://") || url_txtId.Contains("https://") || url_txtProText.Contains("https://") || url_txtId.Contains("www.") || url_txtProText.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlHerchy.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Hiearachy.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlreltype.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select RelType.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtId.Text == "")
                    {
                        MessageBox.Show("Please enter ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtId.Text != "" && (!intRgx.IsMatch(txtId.Text)))
                    {
                        MessageBox.Show("Please enter numeric data in ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtProText.Text == "")
                    {
                        MessageBox.Show("Please enter Program Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string hierarchy = ddlHerchy.SelectedValue.ToString();
                        string reltype = ddlreltype.SelectedValue.ToString();
                        Int64 Id = Convert.ToInt64(txtId.Text.Trim());
                        string Protext = txtProText.Text.Trim();

                        //  In Insert there is no nedd to pass second parameter
                        DataSet dsresult = AwardDataOperations.SaveAbdUpdateRelatedLProgram(WfId, 0, Id, hierarchy, Protext, reltype, 0, "", "");

                        if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            displayData = dsresult.Tables["DisplayData"].Copy();
                            if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                            {
                                BindGrid();
                            }
                            else
                            {
                                grdRelOrgnorecord();
                            }


                            txtId.Text = "";
                            txtProText.Text = "";
                            ddlHerchy.SelectedIndex = 0;
                            ddlreltype.SelectedIndex = 0;
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Related Programs");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                        }
                        lblMsg.Visible = true;
                        lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

                        //Pankaj start track TrackUnstoppedAward
                        if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        }
                        //End track TrackUnstoppedAward


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

        private void grdClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (displayData.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        ddlHerchy.SelectedValue = Convert.ToString(displayData.Rows[rowindex]["hierarchy"]);
                        ddlreltype.SelectedValue = Convert.ToString(displayData.Rows[rowindex]["reltype"]);

                        txtId.Text = Convert.ToString(displayData.Rows[rowindex]["id"]);
                        txtProText.Text = Convert.ToString(displayData.Rows[rowindex]["RELATEDPROGRAM_TEXT"]);

                        btnsave.Visible = false;
                        btnaddurl.Visible = false;

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
                            DataSet dsresult = AwardDataOperations.SaveAbdUpdateRelatedLProgram(SharedObjects.WorkId, 1, Convert.ToInt64(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["ID"]), Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["hierarchy"]), Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["RELATEDPROGRAM_TEXT"]), Convert.ToString(displayData.Rows[grdClass.SelectedCells[0].RowIndex]["RelType"]), 0, "", "");

                            if (dsresult.Tables.Count>0)
                            {
                                displayData = dsresult.Tables["DisplayData"].Copy();
                                if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                                {
                                    BindGrid();
                                }
                                else
                                {
                                    grdRelOrgnorecord();

                                }
                            }
                            m_parent.GetProcess();
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

        private void btnupdat_Click(object sender, EventArgs e)
        {
            //pankaj 11 june
            string url_txtId = txtId.Text.TrimStart().TrimEnd();
            string url_txtProText = txtProText.Text.TrimStart().TrimEnd();

            if (url_txtId.Contains("http://") || url_txtProText.Contains("http://") || url_txtId.Contains("https://") || url_txtProText.Contains("https://") || url_txtId.Contains("www.") || url_txtProText.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlHerchy.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Hiearachy.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlreltype.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select RelType.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtId.Text == "")
                    {
                        MessageBox.Show("Please enter ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtId.Text != "" && (!intRgx.IsMatch(txtId.Text)))
                    {
                        MessageBox.Show("Please enter numeric data in ID.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtProText.Text == "")
                    {
                        MessageBox.Show("Please enter Program Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlHerchy.SelectedValue.ToString() != Convert.ToString(displayData.Rows[rowindex]["hierarchy"]))
                    {
                        ddlHerchy.SelectedValue = Convert.ToString(displayData.Rows[rowindex]["hierarchy"]);
                        MessageBox.Show("You can not update Hierarchy field.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string OldHierar = Convert.ToString(displayData.Rows[rowindex]["hierarchy"]);
                        string OldfRelType = Convert.ToString(displayData.Rows[rowindex]["reltype"]);
                        Int64 OldID = Convert.ToInt64(displayData.Rows[rowindex]["id"]);
                        string OldProText = Convert.ToString(displayData.Rows[rowindex]["RELATEDPROGRAM_TEXT"]);

                        string NewRelType = Convert.ToString(ddlreltype.SelectedValue);
                        Int64 NewID = Convert.ToInt64(txtId.Text.Trim());
                        string NewProText = txtProText.Text.Trim();

                        DataSet dsresult = AwardDataOperations.SaveAbdUpdateRelatedLProgram(WfId, 2, NewID, OldHierar, NewProText, NewRelType, OldID, OldfRelType, OldProText);

                        if (dsresult.Tables.Count>0)
                        {
                            displayData = dsresult.Tables["DisplayData"].Copy();

                            if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                            {
                                BindGrid();
                            }
                            else
                            {
                                grdRelOrgnorecord();

                            }

                            ddlreltype.SelectedIndex = 0;
                            ddlHerchy.SelectedIndex = 0;
                            txtId.Text = "";
                            txtProText.Text = "";
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Related Programs");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion

                        }
                        lblMsg.Visible = true;
                        lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                        if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                        {
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        }
                        btnsave.Visible = true;
                        btnaddurl.Visible = true;

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
                btnaddurl.Visible = true;

                btnupdat.Visible = false;
                btncancel.Visible = false;

                ddlreltype.SelectedIndex = 0;
                ddlHerchy.SelectedIndex = 0;
                txtId.Text = "";
                txtProText.Text = "";
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }

        }

        private void ddlHerchy_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;

        }

        private void ddlreltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
        }
    }
}
