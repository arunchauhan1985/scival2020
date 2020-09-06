using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;
using System.Data;

namespace Scival.Award
{
    public partial class Amount : UserControl
    {
        private Awards m_parent;
        Int64 WFID = 0;
        Int64 pagemode = 0;
        Int64 mode = 0;//Added by avanish on 11-June-2018
        public String FormName = String.Empty;
        ErrorLog Objerror = new ErrorLog();
        private object dsresult;

        void ddlCuurency_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        public Amount(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            loadIniitialValua();
            SharedObjects.DefaultLoad = "";
            PageURL objPage = new PageURL(frm);
            pnlURL.Controls.Add(objPage);

            SharedObjects.TotalAmountChangedValue = "";
        }

        private void loadIniitialValua()
        {
            try
            {
                SharedObjects.TotalAmountChangedValue = "";
                WFID = SharedObjects.WorkId;
                FormName = SharedObjects.FundingClickPage;
                SharedObjects.TotalAmountChangedValue = "";
                WFID = SharedObjects.WorkId;
                lblMsg.Visible = false;
                FormName = SharedObjects.FundingClickPage;
                this.Controls.Clear();
                this.InitializeComponent();

                pagemode = 4;
                grpName.Text = "Installment";

                txtTotalAmount.Text = SharedObjects.TotalAmountChangedValue;


                DataSet dsItems = AwardDataOperations.GetAmount(WFID, pagemode);

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
                    mode = 1;
                    string chvalue = SharedObjects.TotalAmountChangedValue.ToString();
                    if (chvalue.Length > 0)
                    {
                        txtTotalAmount.Text = SharedObjects.TotalAmountChangedValue;
                    }
                    else
                    {
                        txtTotalAmount.Text = Convert.ToString(dsItems.Tables["DisplayData"].Rows[0]["total_amount"]);
                    }
                    txtAmount.Text = "0";
                    dateTimePicker_start.Text = DateTime.Now.ToString();
                    dateTimePickerEnd.Text = DateTime.Now.ToString();

                    dtGridInstallment.DataSource = null;
                    dtGridInstallment.Update();

                    dtGridInstallment.Refresh();
                    dtGridInstallment.Parent.Refresh();


                    #region Added by avanish
                    foreach (DataRow DC in dsItems.Tables["Currency2"].Rows)
                    {
                        if (DC["SEQUENCE_ID"].ToString() != "0")
                        {
                            string firstColum = Convert.ToString(DC["SEQUENCE_ID"]);
                            string secondColum = Convert.ToString(DC["CURRENCY"]);
                            string thirdColum = Convert.ToString(DC["AMOUNT"]);
                            string fourthColum = Convert.ToString(DC["INSTALLMENTSTART_DATE"]);
                            string fifthColum = Convert.ToString(DC["INSTALLMENTEND_DATE"]); ;
                            string[] rowGrid = { firstColum, secondColum, thirdColum, fourthColum, fifthColum };
                            dtGridInstallment.Rows.Add(rowGrid);

                        }
                    }

                    #endregion


                }


            }
            catch (Exception ex)
            {
                Objerror.WriteErrorLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url_txtAmount = txtAmount.Text.TrimStart().TrimEnd();
            string url_txtTotalAmount = txtTotalAmount.Text.TrimStart().TrimEnd();
            if (url_txtAmount.Contains("http://") || url_txtTotalAmount.Contains("http://") ||
                url_txtAmount.Contains("https://") || url_txtTotalAmount.Contains("https://") ||
                url_txtAmount.Contains("www.") || url_txtTotalAmount.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                try
                {

                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^\d*\.?\d*[0-9]+\d*$");
                    if ((!intRgx.IsMatch(txtTotalAmount.Text)))
                    {
                        MessageBox.Show("Please enter numeric Value in Total Amount.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (dtGridInstallment.Rows.Count == 0)
                    {
                        MessageBox.Show("Please enter installment amount Total Amount.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        string currency = Convert.ToString(ddlCuurency.SelectedValue);
                        string Amount = Convert.ToString(txtAmount.Text.Trim());
                        Int32 TotalAmount = Convert.ToInt32(txtTotalAmount.Text);
                        DateTime strtDate = new DateTime(); DateTime endDate = new DateTime();

                        pagemode = 5;
                        if (dateTimePicker_start.Text != "")
                        {
                            strtDate = Convert.ToDateTime(dateTimePicker_start.Text);

                        }

                        else
                        {
                            MessageBox.Show("Please enter startdate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (dateTimePickerEnd.Text != "")
                        {
                            endDate = Convert.ToDateTime(dateTimePickerEnd.Text.Trim());

                        }
                        else
                        {
                            MessageBox.Show("Please enter enddate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        string datestart = string.Empty;
                        string dateend = string.Empty;
                        datestart = dateTimePicker_start.Text.ToString();

                        dateend = dateTimePickerEnd.Text.ToString();
                        string datestart1 = FormatDate(Convert.ToString(strtDate));
                        string datestart2 = FormatDate(Convert.ToString(endDate));
                        #region Added by avanish
                        foreach (DataGridViewRow row in dtGridInstallment.Rows)
                        {
                            Int64 SEQUENCE_ID = Convert.ToInt64(row.Cells["sequence_tran_id"].Value.ToString());
                            string StartDate = FormatDate(row.Cells["InstallmentStart"].Value.ToString());
                            string EndDate = FormatDate(row.Cells["InstallmentEnd"].Value.ToString());
                            string grdcurrency = row.Cells["Currency"].Value.ToString();
                            string Installmentamt = row.Cells["InstallmentAmount"].Value.ToString().TrimStart().TrimEnd();
                            dsresult = AwardDataOperations.SaveAmount(TotalAmount, StartDate, EndDate, WFID, grdcurrency, Installmentamt, pagemode, mode, SEQUENCE_ID);

                            
                                SharedObjects.TotalAmountChangedValue = txtTotalAmount.Text.ToString();
                            //}
                        }

                        #endregion

                        #region For Changing Colour in case of Update
                        if (SharedObjects.TRAN_TYPE_ID == 1)
                        {
                            m_parent.GetProcess("AwardAmount");
                        }
                        else
                        {
                            m_parent.GetProcess();
                        }
                        #endregion
                        lblMsg.Visible = true;
                        //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                        //MessageBox.Show(dsresult.Tables["ERRORCODE"].Rows[0][1].ToString(), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Pankaj start track TrackUnstoppedAward
                        
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                        
                        //End track TrackUnstoppedAward


                        ddlCuurency.SelectedIndex = 0;
                        txtAmount.Text = "";
                        dateTimePicker_start.Text = DateTime.Now.ToString();
                        dateTimePickerEnd.Text = DateTime.Now.ToString();

                        this.Refresh();


                        loadIniitialValua();
                    }
                }
                catch (Exception ex)
                {
                    Objerror.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddurl_Click(object sender, EventArgs e)
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

                    Int32 TotalAmount = Convert.ToInt32(txtTotalAmount.Text);
                    string datestart1 = string.Empty;
                    string datestart2 = string.Empty;
                    DataSet dsresult = AwardDataOperations.SaveAmount(TotalAmount, datestart1, datestart2, WFID, currency, Amount, pagemode, 2,0);

                        ddlCuurency.SelectedIndex = 0;
                        txtAmount.Text = "";
                    
                    m_parent.GetProcess();
                    lblMsg.Visible = true;
                    //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                Objerror.WriteErrorLog(ex);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_start_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {

        }

        private String FormatDate(String _Date)
        {
            try
            {
                DateTime Dt = new DateTime();
                IFormatProvider mFomatter = new System.Globalization.CultureInfo("en-US");
                Dt = Convert.ToDateTime(_Date);
                return Dt.ToString("dd-MMM-yyyy");
            }
            catch
            {
                return "";
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                Regex intRgx = new Regex(@"^\d*\.?\d*[0-9]+\d*$");
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
                    MessageBox.Show("Please select Currency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (dateTimePicker_start.Text == "")
                {
                    MessageBox.Show("Please fill Installment start date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (dateTimePickerEnd.Text == "")
                {
                    MessageBox.Show("Please fill Installment End date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string ddlcurrency = Convert.ToString(ddlCuurency.SelectedValue);
                    string installment = txtAmount.Text;
                    string Inst_Start = dateTimePicker_start.Text;
                    string Inst_End = dateTimePickerEnd.Text;

                    string firstColum = Convert.ToString(0);
                    string secondColum = ddlcurrency.ToLower();
                    string thirdColum = installment;
                    string fourthColum = Inst_Start;
                    string fifthColum = Inst_End;
                    string[] rowGrid = { firstColum, secondColum, thirdColum, fourthColum, fifthColum };
                    dtGridInstallment.Rows.Add(rowGrid);
                    string Installmentamt = installment;
                    if (SharedObjects.TotalAmountChangedValue.Length == 0 && Installmentamt.Length > 0)
                    {
                        SharedObjects.TotalAmountChangedValue = Installmentamt.TrimStart().TrimEnd();
                    }
                    else
                    {
                        SharedObjects.TotalAmountChangedValue = Convert.ToString(Convert.ToInt32(txtTotalAmount.Text.TrimStart().TrimEnd()) + Convert.ToInt32(Installmentamt.TrimStart().TrimEnd()));
                    }
                    txtTotalAmount.Text = SharedObjects.TotalAmountChangedValue.TrimStart().TrimEnd();

                    ddlCuurency.SelectedIndex = 0;
                    txtAmount.Text = "0";
                    dateTimePicker_start.Text = DateTime.Now.ToString();
                    dateTimePickerEnd.Text = DateTime.Now.ToString();

                }



            }

            catch (Exception ex)
            { }
        }

        private void dtGridInstallment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    Int64 SEQUENCE_ID = Convert.ToInt64(dtGridInstallment.Rows[e.RowIndex].Cells["sequence_tran_id"].Value.ToString());

                    if (SEQUENCE_ID > 0)
                    {
                        pagemode = 5;
                        string StartDate = FormatDate(dtGridInstallment.Rows[e.RowIndex].Cells["InstallmentStart"].Value.ToString());
                        string EndDate = FormatDate(dtGridInstallment.Rows[e.RowIndex].Cells["InstallmentEnd"].Value.ToString());
                        string grdcurrency = dtGridInstallment.Rows[e.RowIndex].Cells["Currency"].Value.ToString();
                        string Installmentamt = dtGridInstallment.Rows[e.RowIndex].Cells["InstallmentAmount"].Value.ToString().TrimStart().TrimEnd();

                        SharedObjects.TotalAmountChangedValue = Convert.ToString(Convert.ToInt32(txtTotalAmount.Text) - Convert.ToInt32(Installmentamt));
                        txtTotalAmount.Text = SharedObjects.TotalAmountChangedValue.TrimStart().TrimEnd();

                        dsresult = AwardDataOperations.SaveAmount(0, StartDate, EndDate, WFID, grdcurrency, Installmentamt.TrimStart().TrimEnd(), pagemode, 3, SEQUENCE_ID);
                        this.Refresh();
                        loadIniitialValua();
                    }

                    else if (SEQUENCE_ID == 0)
                    {
                        if (this.dtGridInstallment.Rows[e.RowIndex].Index >= 0)
                        {
                            string Installmentamt = dtGridInstallment.Rows[e.RowIndex].Cells["InstallmentAmount"].Value.ToString().TrimStart().TrimEnd();
                            SharedObjects.TotalAmountChangedValue = Convert.ToString(Convert.ToInt32(txtTotalAmount.Text) - Convert.ToInt32(Installmentamt.TrimStart().TrimEnd()));
                            txtTotalAmount.Text = SharedObjects.TotalAmountChangedValue.TrimStart().TrimEnd();
                            dtGridInstallment.Rows.RemoveAt(this.dtGridInstallment.Rows[e.RowIndex].Index);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
