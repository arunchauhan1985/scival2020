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

namespace Scival.Award
{
    public partial class RelatedItems : UserControl
    {
        Replace r = new Replace();
        private Awards m_parent;
        Int64 WfId = 0;
        int rowindex = 0;
        Int64 pagemode = 0;
        static Int64 grdrowid = 0;
        DataTable displayData = new DataTable();
        DataSet dsItems;
        ErrorLog oErrorLog = new ErrorLog();

        public RelatedItems(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;

            LoadinitialVale();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);

        }

        void ddlLangContextName_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void ddlRelType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;

        }

        private void LoadinitialVale()
        {
            try
            {
                lblMsg.Visible = false;
                string clickPage = SharedObjects.FundingClickPage;
                grpItem.Text = clickPage;
                if (clickPage.ToLower() == "relateditems")
                {
                    pagemode = 4;
                }
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
                dsItems = AwardDataOperations.GetItemsList(Convert.ToInt64(SharedObjects.WorkId), pagemode);
                DataTable DT = dsItems.Tables["ItemListDisplay"];
                foreach (DataRow DR in dsItems.Tables["ItemListDisplay"].Rows)
                {
                    try
                    {
                        if (r.chk_OtherLang(DR["LANG"].ToString().Trim().ToLower()) == true)
                        {
                            DR["DESCRIPTION"] = Convert.ToString(r.ConvertUnicodeToText(DR["DESCRIPTION"].ToString()));
                            DR["LINK_TEXT"] = Convert.ToString(r.ConvertUnicodeToText(DR["LINK_TEXT"].ToString()));
                            DR.AcceptChanges();
                        }
                        DR["DESCRIPTION"] = Convert.ToString(DR["DESCRIPTION"].ToString());
                        DR.AcceptChanges();
                    }
                    catch { }
                }
                if (DT.Rows.Count > 0)
                {
                    grdAbout.AutoGenerateColumns = false;
                    grdAbout.DataSource = DT;
                }
                else
                {
                    norecord();
                }

                DataTable DT2 = dsItems.Tables["ItemListDDLDisplay"];
                DataRow dr = DT2.NewRow();
                dr["VALUE"] = "RELTYPE";
                dr["VALUE"] = "--Select RelType--";
                DT2.Rows.InsertAt(dr, 0);

                ddlRelType.DataSource = DT2;
                ddlRelType.ValueMember = "VALUE";
                ddlRelType.DisplayMember = "VALUE";
                ddlRelType.SelectedValue = "grantedBy";


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
                dtNoRcrd.Columns.Add("RELTYPE");
                dtNoRcrd.Columns.Add("DESCRIPTION");
                dtNoRcrd.Columns.Add("URL");
                dtNoRcrd.Columns.Add("LINK_TEXT");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";
                dtNoRcrd.Rows.Add(dr);

                grdAbout.AutoGenerateColumns = false;
                grdAbout.DataSource = dtNoRcrd;

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {

            //pankaj 11 june
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();

            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();
            string lang_value = "", LinkText = "";
            //pankaj 12 july
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.") || (url_txtLinkUrl.Contains("Not Available"))))))
            {
                if (url_txtLinkText.Contains("http://") || url_txtLinkText.Contains("https://") || url_txtLinkText.Contains("www."))
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
                        string Desc = "";

                        lblMsg.Visible = false;
                        #region
                        if (txtDescr.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");

                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion

                        if (txtLinkUrl.Text == "")
                        {
                            MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtLinkText.Text == "")
                        {
                            MessageBox.Show("Please enter Link Text.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        else
                        {
                            try
                            {
                                Int64 WFID = Convert.ToInt64(SharedObjects.WorkId);
                                string reltype = string.Empty;

                                if (Convert.ToString(ddlRelType.SelectedValue) == "RELTYPE")
                                    reltype = "";
                                else
                                    reltype = Convert.ToString(ddlRelType.SelectedValue);

                                Desc = txtDescr.Text.Trim();
                                lang_value = Convert.ToString(ddlLangContextName.SelectedValue);
                                LinkText = txtLinkText.Text.Trim();
                                if (r.chk_OtherLang(lang_value.ToLower()) == true)
                                {
                                    Desc = r.ConvertTextToUnicode(Desc);
                                    LinkText = r.ConvertTextToUnicode(LinkText);
                                }
                                else
                                {
                                    Desc = txtDescr.Text.Trim();
                                    LinkText = txtLinkText.Text.Trim(); ;
                                }
                                DataSet dsresult = AwardDataOperations.SaveAndDeleteItemsLIst(WFID, pagemode, 0, reltype, Desc, txtLinkUrl.Text.Trim(), LinkText, lang_value, 0);
                                    if (dsresult.Tables["ItemListDisplay"].Rows.Count > 0)
                                    {
                                        BindGrid();


                                        txtLinkUrl.Text = "";
                                        txtDescr.Text = "";
                                        txtLinkText.Text = "";
                                    }
                                    else
                                    {
                                        norecord();
                                    }
                                    #region For Changing Colour in case of Update
                                    if (SharedObjects.TRAN_TYPE_ID == 1)
                                    {
                                        m_parent.GetProcess("RelatedItems");
                                    }
                                    else
                                    {
                                        m_parent.GetProcess();
                                    }
                                    #endregion
                                    lblMsg.Visible = true;
                                    lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                                    if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                                    {
                                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                                    }
                                

                            }
                            catch (Exception ex)
                            {
                                oErrorLog.WriteErrorLog(ex);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                MessageBox.Show("Link path is not valid", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtLinkUrl.Text = Convert.ToString(SharedObjects.CurrentUrl.ToString().TrimStart().TrimEnd());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url_txtLinkUrl = txtLinkUrl.Text.TrimStart().TrimEnd();
            //pankaj 11 june
            string url_txtDescr = txtDescr.Text.TrimStart().TrimEnd();
            string url_txtLinkText = txtLinkText.Text.TrimStart().TrimEnd();

            //pankaj 12 july
            if ((url_txtLinkUrl.Contains("http://") || (url_txtLinkUrl.Contains("https://") || (url_txtLinkUrl.Contains("www.")))))
            {
                if (url_txtDescr.Contains("http://") || url_txtLinkText.Contains("http://") || url_txtDescr.Contains("https://") || url_txtLinkText.Contains("https://") || url_txtDescr.Contains("www.") || url_txtLinkText.Contains("www."))
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
                        #region
                        if (txtDescr.Text != "")
                        {
                            string _result = oErrorLog.htlmtag(txtDescr.Text.Trim(), "Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        #endregion
                        if (txtLinkUrl.Text == "")
                        {
                            MessageBox.Show("Please enter Link URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        else
                        {
                            DataSet dsresult = AwardDataOperations.SaveAndDeleteItemsLIst(SharedObjects.WorkId, pagemode, 2, ddlRelType.SelectedValue.ToString(), txtDescr.Text.Trim(), txtLinkUrl.Text.Trim(), txtLinkText.Text.Trim(), Convert.ToString(ddlLangContextName.SelectedValue), Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[rowindex]["Item_Id"]));

                            BindGrid();

                            txtDescr.Text = ""; txtLinkUrl.Text = ""; txtLinkText.Text = "";

                            BtnAdd.Visible = true;

                            btnUpdate.Visible = false;
                            btnCancel.Visible = false;

                            lblMsg.Visible = true;
                            lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();
                            if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("RelatedItems");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (Convert.ToString(pagemode) == "1")
                {
                    ddlRelType.SelectedValue = "about";
                }
                else if (Convert.ToString(pagemode) == "2")
                {
                    ddlRelType.SelectedValue = "applicationInfoFor";
                }

                txtDescr.Text = ""; txtLinkUrl.Text = "";


                BtnAdd.Visible = true;


                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grpItem_Enter(object sender, EventArgs e)
        {

        }

        private void grdAbout_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = AwardDataOperations.SaveAndDeleteItemsLIst(SharedObjects.WorkId, pagemode, 1, null, null, null, null, null, Convert.ToInt64(dsItems.Tables["ItemListDisplay"].Rows[grdAbout.SelectedCells[0].RowIndex]["Item_Id"]));
                            BindGrid();
                            lblMsg.Visible = true;
                            //lblMsg.Text = dsresult.Tables["ERRORCODE"].Rows[0][1].ToString();

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

        private void grdAbout_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dsItems.Tables["ItemListDisplay"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {
                            DataTable DT = dsItems.Tables["ItemListDisplay"];
                            ddlRelType.SelectedValue = Convert.ToString(DT.Rows[rowindex]["reltype"]);
                            txtDescr.Text = Convert.ToString(DT.Rows[rowindex]["DESCRIPTION"]);
                            txtLinkUrl.Text = Convert.ToString(DT.Rows[rowindex]["URL"]);
                            txtLinkText.Text = Convert.ToString(DT.Rows[rowindex]["LINK_TEXT"]);

                            BtnAdd.Visible = false;
                            // btnAddURL.Visible = false;

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

        public string ConvertTextToUnicode(string difflangval)
        {
            try
            {
                string BYtesStream = "";

                string unicodeString = difflangval;
                Encoding ascii = Encoding.UTF8;
                Encoding unicode = Encoding.UTF8;

                // Convert the string into a byte array.
                byte[] unicodeBytes = unicode.GetBytes(unicodeString);

                // Perform the conversion from one encoding to the other.
                byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                string data = Encoding.UTF8.GetString(asciiBytes);
                for (int i = 0; i < asciiBytes.Length; i++)
                {
                    string BYtesStream1 = asciiBytes[i].ToString();
                    BYtesStream = BYtesStream + BYtesStream1 + "|";
                }
                BYtesStream = BYtesStream.Substring(0, BYtesStream.Length - 1);
                char[] splitchar = { '|' };
                string[] str = (BYtesStream.TrimStart().TrimEnd().Split(splitchar));
                byte[] bytes = BYtesStream.Split('|').Select(s => Convert.ToByte(s)).ToArray();
                string data1 = Encoding.UTF8.GetString(bytes);
                char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                string asciiString = new string(asciiChars);
                return BYtesStream;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ConvertUnicodeToText(string difflangval)
        {
            try
            {
                string BYtesStream = "";
                Encoding ascii = Encoding.UTF8;
                Encoding unicode = Encoding.UTF8;
                BYtesStream = difflangval;
                char[] splitchar = { '|' };
                string[] str = (BYtesStream.TrimStart().TrimEnd().Split(splitchar));
                byte[] bytes = BYtesStream.Split('|').Select(s => Convert.ToByte(s)).ToArray();
                byte[] unicodeBytes = bytes;
                byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                string data1 = Encoding.UTF8.GetString(bytes);
                char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                string asciiString = new string(asciiChars);
                return asciiString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void ddlRelType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }







    }
}
