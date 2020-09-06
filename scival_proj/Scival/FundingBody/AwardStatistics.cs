using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;
namespace Scival.FundingBody
{
    public partial class AwardStatistics : UserControl
    {
        private FundingBody m_parent;
        Int64 UserId = 0; Int64 WFID = 0;
        ErrorLog oErrorLog = new ErrorLog();
        bool flag = false;

        public AwardStatistics(FundingBody frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitailValue();
            SharedObjects.DefaultLoad = "";
            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);
        }

        void ddlCurr_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitailValue()
        {
            try
            {
                lblMsg.Visible = false;
                UserId = Convert.ToInt64(SharedObjects.WorkId);
                WFID = Convert.ToInt64(SharedObjects.WorkId);
                DataSet dsTexIds = FundingBodyDataOperations.GetAwardCurrency();
                DataRow dr = dsTexIds.Tables["AwardCurrency"].NewRow();
                dr["CODE"] = "SelectCurrency";
                dr["VALUE"] = "--Select Currency--";
                dsTexIds.Tables[0].Rows.InsertAt(dr, 0);
                ddlCurr.DataSource = dsTexIds.Tables["AwardCurrency"];
                ddlCurr.DisplayMember = "VALUE";
                ddlCurr.ValueMember = "CODE";

                dsTexIds.Tables.Clear();
                dsTexIds = FundingBodyDataOperations.GetAwardStatistics(WFID);

                if (dsTexIds.Tables["AwardStatistics"].Rows.Count > 0)
                {
                    txtAmount.Text = Convert.ToString(dsTexIds.Tables["AwardStatistics"].Rows[0]["TOTALFUNDING_TEXT"]);
                    txtURL.Text = Convert.ToString(dsTexIds.Tables["AwardStatistics"].Rows[0]["URL"]);
                    txtLinkText.Text = Convert.ToString(dsTexIds.Tables["AwardStatistics"].Rows[0]["LINK_TEXT"]);
                    ddlCurr.SelectedValue = Convert.ToString(dsTexIds.Tables["AwardStatistics"].Rows[0]["CURRENCY"]);
                    flag = true;
                }
                else
                    flag = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtURL.Text.TrimStart().TrimEnd();
            string url_txtAmount = txtAmount.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))) || (url_txtLinkUrl.Contains("")))
            {
                if (url_txtAmount.Contains("http://") || url_txtLinkText.Contains("http://") ||
                    url_txtAmount.Contains("https://") || url_txtLinkText.Contains("https://") ||
                    url_txtAmount.Contains("www.") || url_txtLinkText.Contains("www."))
                {
                    MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (url_txtLinkUrl.Contains("file:///C:/") || url_txtLinkUrl.Contains("///C:/") || url_txtLinkUrl.Contains("C:/") || url_txtLinkUrl.Contains("file:///C:/Users/"))
                {
                    MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        lblMsg.Visible = false;
                        Regex intRgx = new Regex(@"^[0-9]+");
                        
                        if (txtLinkText.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtLinkText.Text.Trim(), "Link Text");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        
                        if (ddlCurr.SelectedIndex > 0)
                        {
                            if (ddlCurr.SelectedValue.ToString() == "SelectCurrency" && txtAmount.Text == "" && txtURL.Text == "" && txtLinkText.Text == "")
                            {
                                MessageBox.Show("Please fill any field.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (ddlCurr.SelectedValue.ToString() == "SelectCurrency" && txtAmount.Text != "")
                            {
                                MessageBox.Show("Please select Currency", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (ddlCurr.SelectedValue.ToString() != "SelectCurrency" && txtAmount.Text == "")
                            {
                                MessageBox.Show("Please enter Amount", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (ddlCurr.SelectedValue.ToString() != "SelectCurrency" && txtAmount.Text != "" && (!intRgx.IsMatch(txtAmount.Text)))
                            {
                                MessageBox.Show("Please enter valid Amount", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                string Currency = string.Empty; string url = string.Empty;
                                string amount = string.Empty; string Link_Text = string.Empty;

                                if (ddlCurr.SelectedValue.ToString() != "SelectCurrency")
                                    Currency = Convert.ToString(ddlCurr.SelectedValue);

                                if (txtAmount.Text != "")
                                {
                                    amount = txtAmount.Text.Trim();
                                }

                                if (txtURL.Text != "")
                                    url = txtURL.Text.Trim();

                                if (txtLinkText.Text != "")
                                    Link_Text = txtLinkText.Text.Trim();

                                DataSet dsresult = new DataSet();

                                if (flag == true)
                                {
                                    dsresult = FundingBodyDataOperations.SaveAndUpdateAwardSta(WFID, 2, Currency, amount, url, Link_Text);
                                }
                                else
                                {
                                    dsresult = FundingBodyDataOperations.SaveAndUpdateAwardSta(WFID, 0, Currency, amount, url, Link_Text);
                                    flag = true;
                                }

                                m_parent.GetProcess();

                                lblMsg.Visible = true;

                                txtAmount.Text = url_txtAmount.TrimStart().TrimEnd();
                                txtLinkText.Text = url_txtLinkText.TrimStart().TrimEnd();
                                txtURL.Text = url_txtLinkUrl.TrimStart().TrimEnd();

                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Currency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (ddlCurr.SelectedValue.ToString() == "SelectCurrency" && txtAmount.Text == "" && txtURL.Text == "" && txtLinkText.Text == "")
                {
                    MessageBox.Show("No record(s) available for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    DataSet dsresult = FundingBodyDataOperations.SaveAndUpdateAwardSta(WFID, 1, "", "", "", "");

                    ddlCurr.SelectedIndex = 0;
                    txtAmount.Text = "";
                    txtURL.Text = "";
                    txtLinkText.Text = "";
                    flag = false;

                    m_parent.GetProcess();

                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnAddUrl_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtURL.Text = SharedObjects.CurrentUrl;
        }
    }
}
