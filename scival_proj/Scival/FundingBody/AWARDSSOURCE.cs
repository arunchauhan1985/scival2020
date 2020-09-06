using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class AWARDSSOURCE : UserControl
    {
        private FundingBody m_parent;        
        DataSet dsItems;
        Int64 pagemode = 0;
        int rowindex = 0;
        ErrorLog oErrorLog = new ErrorLog();

        public AWARDSSOURCE(FundingBody frm)
        {
            InitializeComponent();
            LoadinitialVale();
            m_parent = frm;
        }

        void ddlLangContextName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlStatus_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtUrl.Text.TrimStart().TrimEnd();
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtUrl.Text.Length > 0 && txt_name.Text.Length > 0)
                {
                    try
                    {
                        Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                        pagemode = 1;
                        DateTime awardsourceDate = new DateTime();
                        DateTime date_captureStart = new DateTime();
                        DateTime date_captureEnd = new DateTime();
                        if (dateTimePicker1.Text != "")
                        {
                            awardsourceDate = Convert.ToDateTime(dateTimePicker1.Text);
                        }
                        date_captureStart = Convert.ToDateTime(dateTimePicker2.Text);
                        date_captureEnd = Convert.ToDateTime(dateTimePicker3.Text);
                        string strawardsourceDate = FormatDate(Convert.ToString(awardsourceDate));
                        string captureStart = FormatDate(Convert.ToString(date_captureStart));
                        string captureEnd = FormatDate(Convert.ToString(date_captureEnd));
                        DataSet dsresult = FundingBodyDataOperations.SaveAndupdateAwardlist5(WFID, pagemode, ddlStatus.Text.ToString(), strawardsourceDate, txtUrl.Text.TrimStart().TrimEnd(), ddlLangContextName.SelectedValue.ToString(), txt_name.Text.Trim().ToString(), ddl_Frequency.SelectedItem.ToString(), captureStart, captureEnd, txt_Comment.Text.Trim().ToString());
                        if (dsresult!=null && dsresult.Tables["ItemListDisplay"].Rows.Count > 0)
                        {
                            BindGrid();
                            txtUrl.Text = "";
                            lblMsg.Visible = true;
                            lblMsg.Text = "Inserted Successfully";
                        }

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess_update("Awards Source");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion

                        lblMsg.Visible = true;                        
                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());                        
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadinitialVale()
        {
            try
            {
                lblMsg.Visible = false;
                string clickPage = SharedObjects.FundingClickPage;
                grpItem.Text = clickPage;

                if (clickPage.ToLower() == "about")
                {
                    pagemode = 1;
                }
                else if (clickPage.ToLower() == " Awards Source")
                {
                    pagemode = 1;
                }
                else if (clickPage.ToLower() == "geoscope")
                {
                    pagemode = 3;
                }
                else if (clickPage.ToLower() == "related items")
                {
                    pagemode = 4;
                }
                ddlStatus.Text = "active";

                DataSet dsFunding = SharedObjects.StartWork;
                DataTable tempCont = dsFunding.Tables["LanguageTable"].Copy();
                DataRow dr = tempCont.NewRow();
                dr = tempCont.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";
                tempCont.Rows.InsertAt(dr, 0);

                ddlLangContextName.DataSource = tempCont;
                ddlLangContextName.ValueMember = "LANGUAGE_CODE";
                ddlLangContextName.DisplayMember = "LANGUAGE_NAME";
                ddlLangContextName.SelectedIndex = 18;

                ddl_Frequency.SelectedIndex = 0;

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
                Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                pagemode = 3;
                dsItems = FundingBodyDataOperations.GetAwardSource(Convert.ToInt64(SharedObjects.WorkId), pagemode);
                DataTable DT = dsItems.Tables["ItemListDDLDisplay"];
                if (DT.Rows.Count > 0)
                {
                    grdAwardSource.AutoGenerateColumns = false;
                    grdAwardSource.DataSource = DT;
                }
                else
                {
                    lblMsg.Text = "No record Found";
                    grdAwardSource.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdAwardSource_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDDLDisplay"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;
                        try
                        {
                            DataTable DT = dsItems.Tables["ItemListDDLDisplay"];
                            ddlStatus.SelectedValue = (DT.Rows[rowindex]["STATUS"]).ToString();
                            txtUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                            BtnAdd.Visible = false;
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

        private void grdAwardSource_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtUrl.Text.TrimStart().TrimEnd();
            //pankaj 12 july
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                try
                {
                    Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                    string awdid = "";
                    awdid = AWARD_SOURCE_ID.ToString();
                    pagemode = 2;

                    DateTime updateawardsourceDate = new DateTime();

                    if (dateTimePicker1.Text != "")
                    {
                        updateawardsourceDate = Convert.ToDateTime(dateTimePicker1.Text);
                    }
                    
                    string strupdateawardsourceDate = FormatDate(Convert.ToString(updateawardsourceDate));
                    DataSet dsresult = FundingBodyDataOperations.updateAwardlist(WFID, pagemode, Convert.ToInt64(dsItems.Tables["ItemListDDLDisplay"].Rows[rowindex]["AWARD_SOURCE_ID"]), ddlStatus.Text.ToString(), strupdateawardsourceDate, txtUrl.Text.TrimStart().TrimEnd(), ddlLangContextName.SelectedValue.ToString());

                    BindGrid();

                    txtUrl.Text = "";
                    BtnAdd.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;
                    lblMsg.Visible = true;
                    
                    OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());                    
                    
                    if (SharedObjects.TRAN_TYPE_ID == 1)
                    {
                        m_parent.GetProcess_update("Awards Source");
                    }
                    else
                    {
                        m_parent.GetProcess();
                    }
                    
                    ClearFields();
                }

                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private String FormatDate(String _Date)
        {
            try
            {
                DateTime Dt = new DateTime();
                IFormatProvider mFomatter = new System.Globalization.CultureInfo("en-US");
                Dt = (Convert.ToDateTime(_Date));
                return Dt.ToString("dd-MMM-yyyy");
            }
            catch
            {
                return "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtUrl.Text = "";
                BtnAdd.Visible = true;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdAwardSource_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10)
            {
                try
                {
                    Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                    pagemode = 4;
                    DataSet dsresult = FundingBodyDataOperations.updateAwardlist5(WFID, pagemode, Convert.ToInt64(dsItems.Tables["ItemListDDLDisplay"].Rows[rowindex]["AWARD_SOURCE_ID"]), ddlStatus.Text.ToString(), "", txtUrl.Text, ddlLangContextName.SelectedValue.ToString(), "", "", "", "", "");
                    BindGrid();
                    txtUrl.Text = "";
                    BtnAdd.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;
                    lblMsg.Visible = true;                    
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    if (dsItems.Tables["ItemListDDLDisplay"].Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            rowindex = e.RowIndex;
                            try
                            {
                                DataTable DT = dsItems.Tables["ItemListDDLDisplay"];
                                ddlStatus.SelectedValue = (DT.Rows[rowindex]["STATUS"]).ToString();
                                txtUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                                txt_name.Text = Convert.ToString(DT.Rows[rowindex]["Name"]);
                                ddl_Frequency.SelectedValue = Convert.ToString(DT.Rows[rowindex]["Frequency"]);
                                dateTimePicker2.Text = Convert.ToString(DT.Rows[rowindex]["CaptureStart"]);
                                dateTimePicker3.Text = Convert.ToString(DT.Rows[rowindex]["CaptureEnd"]);
                                txt_Comment.Text = Convert.ToString(DT.Rows[rowindex]["Aw_comment"]);
                                BtnAdd.Visible = false;
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
            m_parent.GetProcess();
        }

        private void ClearFields()
        {
            txt_name.Text = "";
            txtUrl.Text = "";
            ddlStatus.SelectedIndex = 0;
            ddlLangContextName.SelectedValue = "en";
            ddl_Frequency.SelectedIndex = 0;
            txt_Comment.Text = "";
        }
    }
}
