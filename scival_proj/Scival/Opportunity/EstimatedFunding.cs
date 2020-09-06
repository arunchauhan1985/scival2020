using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class EstimatedFunding : UserControl
    {
        Opportunity opportunity;
        ErrorLog oErrorLog = new ErrorLog();
        Replace replace = new Replace();

        Int64 WFID = 0;
        Int64 pagemode = 0;
        Int64 mode = 0;
        public String FormName = String.Empty;

        public EstimatedFunding(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            loadIniitialValua();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opp);
            pnlURL.Controls.Add(objPage);
        }

        void ddlCuurency_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void loadIniitialValua()
        {
            try
            {
                lblMsg.Visible = false;
                WFID = SharedObjects.WorkId; ;

                FormName = SharedObjects.FundingClickPage;

                if (FormName == "Awardceiling")
                {
                    pagemode = 1;
                    groupBox1.Text = "Award Ceiling";
                    TxtestimatedAmountDescription.Visible = false;
                    txtLink.Visible = false;
                    txtLinkText.Visible = false;
                    label10.Visible = false;
                    label4.Visible = false;
                    label3.Visible = false;
                }
                else if (FormName == "AwardFloor")
                {
                    pagemode = 2;
                    groupBox1.Text = "Award Floor";
                }
                else if (FormName == "Estimatedfunding")
                {
                    pagemode = 3;
                    groupBox1.Text = "Estimated Funding";
                }

                DataSet dsItems = OpportunityDataOperations.GetESTFundings(WFID, pagemode);

                DataTable temp = dsItems.Tables["Currency"].Copy();

                DataRow dr = temp.NewRow();
                dr["Code"] = "SelectCurrency";
                dr["Value"] = "--Select Currency--";
                temp.Rows.InsertAt(dr, 0);

                ddlCuurency.DataSource = temp;
                ddlCuurency.DisplayMember = "Value";
                ddlCuurency.ValueMember = "Code";

                if (dsItems.Tables["DisplayData"].Rows.Count > 0)
                {
                    ddlCuurency.SelectedValue = Convert.ToString(dsItems.Tables["DisplayData"].Rows[0]["Currency"]);
                    txtAmount.Text = Convert.ToString(dsItems.Tables["DisplayData"].Rows[0]["estimatedfunding_text"]);
                    TxtestimatedAmountDescription.Text = Convert.ToString(dsItems.Tables["DisplayData"].Rows[0]["amount_description"]);

                    mode = 1;
                }
                if (dsItems.Tables["Currency2"].Rows.Count > 0)
                {
                    if (Convert.ToString(dsItems.Tables["Currency2"].Rows[0]["URL"]) != "")
                    {
                        txtLinkText.Text = Convert.ToString(dsItems.Tables["Currency2"].Rows[0]["Link_Text"]);
                        txtLink.Text = Convert.ToString(dsItems.Tables["Currency2"].Rows[0]["URL"]);
                        mode = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url_txtAmount = txtAmount.Text.TrimStart().TrimEnd();
            string url_TxtestimatedAmountDescription = TxtestimatedAmountDescription.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();
            if (url_txtAmount.Contains("http://") ||
                url_txtAmount.Contains("https://") ||
                url_txtAmount.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (url_txtLinkText.Contains("file:///C:/") || url_txtLinkText.Contains("///C:/") || url_txtLinkText.Contains("C:/") || url_txtLinkText.Contains("file:///C:/Users/"))
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlCuurency.SelectedIndex == 0 && txtAmount.Text != "")
                    {
                        MessageBox.Show("Please select Currency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (ddlCuurency.SelectedIndex != 0 && txtAmount.Text == "")
                    {
                        MessageBox.Show("Please enter Amount.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (ddlCuurency.SelectedIndex != 0 && txtAmount.Text != "" && (!intRgx.IsMatch(txtAmount.Text)))
                    {
                        MessageBox.Show("Please enter numeric in Amount.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (ddlCuurency.SelectedIndex == 0 && txtAmount.Text == "")
                    {
                        return;
                    }
                    else
                    {
                        int len = TxtestimatedAmountDescription.Text.Length;

                        if (len > 1980)
                        {
                            MessageBox.Show("Amount description is too long. please shorten it and try again ", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        string currency = Convert.ToString(ddlCuurency.SelectedValue);
                        string Amount = Convert.ToString(txtAmount.Text.Trim());
                        string Linktext = Convert.ToString(txtLinkText.Text.Trim());
                        Linktext = replace.EntityToUnicode(Linktext);

                        DataSet dsresult = OpportunityDataOperations.SaveESTFundings(WFID, currency, Amount, TxtestimatedAmountDescription.Text, Convert.ToString(txtLink.Text), Linktext, pagemode, mode);

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1 && FormName == "Awardceiling")
                        {
                            opportunity.GetProcess_update("Awardceiling");
                        }
                        else if (SharedObjects.TRAN_TYPE_ID == 1 && FormName == "Estimatedfunding")
                        {
                            opportunity.GetProcess_update("Estimatedfunding");
                        }
                        else
                        {
                            opportunity.GetProcess();
                        }
                        #endregion

                        lblMsg.Visible = true;
                        lblMsg.Text = "Record inserted/updated successfully";

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddurl_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            pnlURL.Controls.Add(objPage);
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (ddlCuurency.SelectedIndex == 0 && txtAmount.Text == "")
                {
                    return;
                }
                else
                {
                    string currency = Convert.ToString(ddlCuurency.SelectedValue);
                    string Amount = Convert.ToString(txtAmount.Text.Trim());

                    DataSet dsresult = OpportunityDataOperations.SaveESTFundings(WFID, currency, Amount, TxtestimatedAmountDescription.Text, Convert.ToString(txtLink.Text), Convert.ToString(txtLinkText.Text), pagemode, 2);

                    if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0].ToString()) == "0")
                    {
                        ddlCuurency.SelectedIndex = 0;
                        txtAmount.Text = "";
                        TxtestimatedAmountDescription.Text = "";
                    }

                    opportunity.GetProcess();

                    lblMsg.Visible = true;
                    lblMsg.Text = "Record deleted successfully";
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
    }
}
