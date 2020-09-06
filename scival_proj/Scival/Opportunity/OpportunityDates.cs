using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class OpportunityDates : UserControl
    {        
        DataTable dttempContact = new DataTable();
        private Opportunity m_parent;
        ErrorLog oErrorLog = new ErrorLog();
        Replace replace = new Replace();
        Int64 pagemode = 0; int rowindex = 0;
        public String FormName = String.Empty;
        static string OppStatus = "";

        public OpportunityDates(Opportunity opp)
        {
            InitializeComponent();
            m_parent = opp;
            LoadinitialVale();

            SharedObjects.DefaultLoad = "";
        }

        void ddlDueDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlExpirationDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlFirstPostDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangDueD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangED_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangFPD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangLMPD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangLoiD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangOpD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLangPPD_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLastModifiedPostDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlLoiDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlOpenDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlPreProposalDate_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadinitialVale()
        {
            try
            {
                CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                culture.DateTimeFormat.LongTimePattern = "";
                Thread.CurrentThread.CurrentCulture = culture;

                #region  LANG Attribute
                DataSet dsOpptunity = SharedObjects.StartWork;
                DataTable tempOppName = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName1 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName2 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName3 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName4 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName5 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName6 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataTable tempOppName7 = dsOpptunity.Tables["LanguageTable"].Copy();
                DataRow dr = tempOppName.NewRow();
                DataRow dr1 = tempOppName1.NewRow();
                DataRow dr2 = tempOppName2.NewRow();
                DataRow dr3 = tempOppName3.NewRow();
                DataRow dr4 = tempOppName4.NewRow();
                DataRow dr5 = tempOppName5.NewRow();
                DataRow dr6 = tempOppName6.NewRow();
                DataRow dr7 = tempOppName7.NewRow();

                dr = tempOppName.NewRow();
                dr["LANGUAGE_CODE"] = "SelectLanguage";
                dr["LANGUAGE_NAME"] = "--Select Language--";

                dr1 = tempOppName1.NewRow();
                dr1["LANGUAGE_CODE"] = "SelectLanguage";
                dr1["LANGUAGE_NAME"] = "--Select Language--";

                dr2 = tempOppName2.NewRow();
                dr2["LANGUAGE_CODE"] = "SelectLanguage";
                dr2["LANGUAGE_NAME"] = "--Select Language--";

                dr3 = tempOppName3.NewRow();
                dr3["LANGUAGE_CODE"] = "SelectLanguage";
                dr3["LANGUAGE_NAME"] = "--Select Language--";

                dr4 = tempOppName4.NewRow();
                dr4["LANGUAGE_CODE"] = "SelectLanguage";
                dr4["LANGUAGE_NAME"] = "--Select Language--";

                dr5 = tempOppName5.NewRow();
                dr5["LANGUAGE_CODE"] = "SelectLanguage";
                dr5["LANGUAGE_NAME"] = "--Select Language--";

                dr6 = tempOppName6.NewRow();
                dr6["LANGUAGE_CODE"] = "SelectLanguage";
                dr6["LANGUAGE_NAME"] = "--Select Language--";

                dr7 = tempOppName7.NewRow();
                dr7["LANGUAGE_CODE"] = "SelectLanguage";
                dr7["LANGUAGE_NAME"] = "--Select Language--";

                tempOppName.Rows.InsertAt(dr, 0);
                tempOppName1.Rows.InsertAt(dr1, 0);
                tempOppName2.Rows.InsertAt(dr2, 0);
                tempOppName3.Rows.InsertAt(dr3, 0);
                tempOppName4.Rows.InsertAt(dr4, 0);
                tempOppName5.Rows.InsertAt(dr5, 0);
                tempOppName6.Rows.InsertAt(dr6, 0);
                tempOppName7.Rows.InsertAt(dr7, 0);

                ddlLangOpD.DataSource = tempOppName;
                ddlLangOpD.ValueMember = "LANGUAGE_CODE";
                ddlLangOpD.DisplayMember = "LANGUAGE_NAME";
                ddlLangOpD.SelectedIndex = 18;

                ddlLangLoiD.DataSource = tempOppName1;
                ddlLangLoiD.ValueMember = "LANGUAGE_CODE";
                ddlLangLoiD.DisplayMember = "LANGUAGE_NAME";
                ddlLangLoiD.SelectedIndex = 18;

                ddlLangDueD.DataSource = tempOppName2;
                ddlLangDueD.ValueMember = "LANGUAGE_CODE";
                ddlLangDueD.DisplayMember = "LANGUAGE_NAME";
                ddlLangDueD.SelectedIndex = 18;

                ddlLangFPD.DataSource = tempOppName3;
                ddlLangFPD.ValueMember = "LANGUAGE_CODE";
                ddlLangFPD.DisplayMember = "LANGUAGE_NAME";
                ddlLangFPD.SelectedIndex = 18;

                ddlLangLMPD.DataSource = tempOppName4;
                ddlLangLMPD.ValueMember = "LANGUAGE_CODE";
                ddlLangLMPD.DisplayMember = "LANGUAGE_NAME";
                ddlLangLMPD.SelectedIndex = 18;

                ddlLangED.DataSource = tempOppName5;
                ddlLangED.ValueMember = "LANGUAGE_CODE";
                ddlLangED.DisplayMember = "LANGUAGE_NAME";
                ddlLangED.SelectedIndex = 18;


                ddlLangPPD.DataSource = tempOppName6;
                ddlLangPPD.ValueMember = "LANGUAGE_CODE";
                ddlLangPPD.DisplayMember = "LANGUAGE_NAME";
                ddlLangPPD.SelectedIndex = 18;

                ddlLangDD.DataSource = tempOppName7;
                ddlLangDD.ValueMember = "LANGUAGE_CODE";
                ddlLangDD.DisplayMember = "LANGUAGE_NAME";
                ddlLangDD.SelectedIndex = 18;
                #endregion

                #region Bind DDl
                ddlOpenDate.Items.Insert(0, "Select Open Date");
                ddlOpenDate.Items.Insert(1, "not-specified");
                ddlOpenDate.SelectedIndex = 0;

                ddlDueDate.Items.Insert(0, "Select Due Date");
                ddlDueDate.Items.Insert(1, "not-specified");
                ddlDueDate.Items.Insert(2, "ongoing");
                ddlDueDate.SelectedIndex = 0;

                ddlLoiDate.Items.Insert(0, "Select Loi Date");
                ddlLoiDate.Items.Insert(1, "not-specified");
                ddlLoiDate.Items.Insert(2, "ongoing");
                ddlLoiDate.SelectedIndex = 0;

                ddlFirstPostDate.Items.Insert(0, "Select First Post Date");
                ddlFirstPostDate.Items.Insert(1, "not-specified");
                ddlFirstPostDate.SelectedIndex = 0;

                ddlLastModifiedPostDate.Items.Insert(0, "Select Last Modified PostDate");
                ddlLastModifiedPostDate.SelectedIndex = 0;

                ddlExpirationDate.Items.Insert(0, "Select Expiration Date");
                ddlExpirationDate.Items.Insert(1, "not-specified");
                ddlExpirationDate.SelectedIndex = 0;

                ddlPreProposalDate.Items.Insert(0, "Select PreProposal Date");
                ddlPreProposalDate.Items.Insert(1, "not-specified");
                ddlPreProposalDate.Items.Insert(2, "ongoing");
                ddlPreProposalDate.SelectedIndex = 0;

                ddlDecisionDate.Items.Insert(0, "Select Decision Date");
                ddlDecisionDate.Items.Insert(1, "not-specified");
                ddlDecisionDate.Items.Insert(2, "ongoing");
                ddlDecisionDate.SelectedIndex = 0;

                ddl_Recurring.Items.Insert(0, "Select Recurring Status");
                ddl_Recurring.Items.Insert(1, "True");
                ddl_Recurring.Items.Insert(2, "False");

                ddl_Recurring.SelectedIndex = Convert.ToInt16(SharedObjects.RecCurrentStatus);

                #endregion

                DataSet dsItems = OpportunityDataOperations.GetOpportunityDates(SharedObjects.WorkId);

                if (dsItems.Tables["OppStatus"].Rows.Count > 0)
                {
                    OppStatus = Convert.ToString(dsItems.Tables["OppStatus"].Rows[0]["OPPORTUNITYSTATUS"]);
                    DateTime dtaTime = new DateTime();

                    if (dsItems.Tables["loi_data"].Rows.Count > 0)
                    {
                        DataView dvLOIDate;
                        dvLOIDate = new DataView(dsItems.Tables["loi_data"].Copy());
                        dvLOIDate.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvLOIDate.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvLOIDate[iContext]["SEQUENCE_ID"]);
                            string isdate = dvLOIDate[iContext]["LOI_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvLOIDate[iContext]["LOI_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvLOIDate[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvLOIDate[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                dtGridloiDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["due_data"].Rows.Count > 0)
                    {
                        DataView dvDueDate;
                        dvDueDate = new DataView(dsItems.Tables["due_data"].Copy());
                        dvDueDate.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvDueDate.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvDueDate[iContext]["SEQUENCE_ID"]);
                            string isdate = dvDueDate[iContext]["DUE_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvDueDate[iContext]["DUE_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvDueDate[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvDueDate[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                dtGridDueDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["expirationdate_data"].Rows.Count > 0)
                    {
                        DataView dvexpirationdate_data;
                        dvexpirationdate_data = new DataView(dsItems.Tables["expirationdate_data"].Copy());
                        dvexpirationdate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvexpirationdate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvexpirationdate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvexpirationdate_data[iContext]["EXPIRATION_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvexpirationdate_data[iContext]["EXPIRATION_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvexpirationdate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvexpirationdate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);
                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                grdExpirationDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["firstpostdate_data"].Rows.Count > 0)
                    {
                        DataView dvfirstpostdate_data;
                        dvfirstpostdate_data = new DataView(dsItems.Tables["firstpostdate_data"].Copy());
                        dvfirstpostdate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvfirstpostdate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvfirstpostdate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvfirstpostdate_data[iContext]["FIRSTPOST_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvfirstpostdate_data[iContext]["FIRSTPOST_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvfirstpostdate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvfirstpostdate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                grdFirstPostDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["lastmodifedpostdate_data"].Rows.Count > 0)
                    {
                        DataView dvlastmodifiedpostdate_data;
                        dvlastmodifiedpostdate_data = new DataView(dsItems.Tables["lastmodifedpostdate_data"].Copy());
                        dvlastmodifiedpostdate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvlastmodifiedpostdate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvlastmodifiedpostdate_data[iContext]["LASTMODIFEDPOST_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvlastmodifiedpostdate_data[iContext]["LASTMODIFEDPOST_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);
                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                grdLastModifiedPostDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["opendate_data"].Rows.Count > 0)
                    {
                        DataView dvopendate_data;
                        dvopendate_data = new DataView(dsItems.Tables["opendate_data"].Copy());
                        dvopendate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvopendate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvopendate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvopendate_data[iContext]["OPEN_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvopendate_data[iContext]["OPEN_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvopendate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvopendate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);
                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                dtGridOpenDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["PreProposalDate_data"].Rows.Count > 0)
                    {
                        DataView dvPreProposalDate_data;
                        dvPreProposalDate_data = new DataView(dsItems.Tables["PreProposalDate_data"].Copy());
                        dvPreProposalDate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvPreProposalDate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvPreProposalDate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvPreProposalDate_data[iContext]["PreProposal_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvPreProposalDate_data[iContext]["PreProposal_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvPreProposalDate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvPreProposalDate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {
                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                grdPreProposalDate.Rows.Add(row);
                            }
                        }
                    }

                    if (dsItems.Tables["DecisionDate_data"].Rows.Count > 0)
                    {
                        DataView dvDecisionDate_data;
                        dvDecisionDate_data = new DataView(dsItems.Tables["DecisionDate_data"].Copy());
                        dvDecisionDate_data.Sort = "SEQUENCE_ID ASC";

                        for (int iContext = 0; iContext < dvDecisionDate_data.Count; iContext++)
                        {
                            string firstCol = Convert.ToString(dvDecisionDate_data[iContext]["SEQUENCE_ID"]);
                            string isdate = dvDecisionDate_data[iContext]["Decision_DATE"].ToString();

                            if (!isdate.Equals(""))
                            {
                                dtaTime = Convert.ToDateTime(dvDecisionDate_data[iContext]["Decision_DATE"].ToString());
                                string secondCol = dtaTime.ToShortDateString();
                                string thrdCol = Convert.ToString(dvDecisionDate_data[iContext]["DATE_REMARKS"]);
                                string FourthCol = Convert.ToString(dvDecisionDate_data[iContext]["LANG"]);

                                if (secondCol == "01-01-1900")
                                {
                                    secondCol = "not-specified";
                                }
                                else if (secondCol == "01-02-1900")
                                {
                                    secondCol = "ongoing";
                                }

                                if (thrdCol != "")
                                {

                                    if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                    {
                                        string Dec_DiffLAng;
                                        Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);
                                        if (Dec_DiffLAng != "")
                                        {
                                            thrdCol = Dec_DiffLAng;
                                        }
                                    }
                                }

                                string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                                grdDecisionDate.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void norecord(int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        dtGridloiDate.Rows.Clear();
                        break;
                    case 2:
                        dtGridDueDate.Rows.Clear();
                        break;
                    case 3:
                        grdExpirationDate.Rows.Clear();
                        break;
                    case 4:
                        grdFirstPostDate.Rows.Clear();
                        break;
                    case 5:
                        grdLastModifiedPostDate.Rows.Clear();
                        break;
                    case 6:
                        dtGridOpenDate.Rows.Clear();
                        break;
                    case 7:
                        grdPreProposalDate.Rows.Clear();
                        break;
                    case 8:
                        grdDecisionDate.Rows.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ddlOpenDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOpenDate.SelectedIndex == 1 || ddlOpenDate.SelectedIndex == 2)
                txtOpenDate_Cal.Enabled = false;
            else
                txtOpenDate_Cal.Enabled = true;
        }

        private void ddlDueDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDueDate.SelectedIndex == 1 || ddlDueDate.SelectedIndex == 2)
                txtDuedate_Cal.Enabled = false;
            else
                txtDuedate_Cal.Enabled = true;
        }

        private void ddlLoiDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoiDate.SelectedIndex == 1 || ddlLoiDate.SelectedIndex == 2)
                txtloidate_Cal.Enabled = false;
            else
                txtloidate_Cal.Enabled = true;
        }

        private void ddlFirstPostDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFirstPostDate.SelectedIndex == 1 || ddlFirstPostDate.SelectedIndex == 2)
                txtFirstPostDate_cal.Enabled = false;
            else
                txtFirstPostDate_cal.Enabled = true;
        }

        private void ddlLastModifiedPostDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLastModifiedPostDate.SelectedIndex == 1 || ddlLastModifiedPostDate.SelectedIndex == 2)
                txtLastModifiedPostDate_cal.Enabled = false;
            else
                txtLastModifiedPostDate_cal.Enabled = true;
        }

        private void ddlExpirationDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExpirationDate.SelectedIndex == 1 || ddlExpirationDate.SelectedIndex == 2)
                txtExpirationDate.Enabled = false;
            else
                txtExpirationDate.Enabled = true;
        }

        private void btnAddDueDate_Click(object sender, EventArgs e)
        {
            string url_txtDueDateDes = txtDueDateDes.Text.TrimStart().TrimEnd();

            if (url_txtDueDateDes.Contains("http://") || url_txtDueDateDes.Contains("https://") || url_txtDueDateDes.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtDueDateDes.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtDueDateDes.Text.Trim(), "Due Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    string calDueDate = string.Empty;
                    lblMsg.Visible = false;

                    if (ddlDueDate.SelectedIndex == 0)
                    {
                        calDueDate = txtDuedate_Cal.Text;

                        if (dtGridloiDate.Rows.Count > 0)
                        {
                            int iCheck = dtGridDueDate.Rows.Count - 1;

                            if (dtGridDueDate.Rows.Count > 0)
                            {
                            HERE:
                                try
                                {
                                    DataGridViewRow rowLoi = dtGridloiDate.Rows[iCheck];
                                    string grdLOIDateCheck = rowLoi.Cells["LoiDate"].Value.ToString();

                                    if (grdLOIDateCheck != "not-specified" && grdLOIDateCheck != "ongoing")
                                    {
                                        if (Convert.ToDateTime(calDueDate) < Convert.ToDateTime(grdLOIDateCheck))
                                        {
                                            MessageBox.Show("DueDate can not be less than LOIDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    oErrorLog.WriteErrorLog(ex);
                                    iCheck--;
                                    goto HERE;
                                }
                            }
                        }

                        if (dtGridDueDate.Rows.Count == 0)
                        {
                            if (dtGridloiDate.Rows.Count > 0)
                            {
                                DataGridViewRow rowLoi = dtGridloiDate.Rows[0];
                                string grdloiDateCheck = rowLoi.Cells["LoiDate"].Value.ToString();

                                if (grdloiDateCheck != "not-specified" && grdloiDateCheck != "ongoing")
                                {
                                    if (Convert.ToDateTime(calDueDate) <= Convert.ToDateTime(grdloiDateCheck))
                                    {
                                        MessageBox.Show("DueDate can not be less than or equal LOIDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                            }
                        }

                        if (SharedObjects.Cycle == 0 || SharedObjects.Cycle == 1)
                        {
                            if (OppStatus.ToLower() == "active")
                            {
                                if (Convert.ToDateTime(calDueDate) <= DateTime.Now.Date)
                                {
                                    MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            if (Convert.ToDateTime(calDueDate) > DateTime.Now.Date && OppStatus.ToLower() == "inactive")
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else if (SharedObjects.TaskId == 7)
                        {
                            if (calDueDate.Trim() != "")
                            {
                                if (OppStatus.ToLower() == "active")
                                {
                                    if (Convert.ToDateTime(calDueDate) < DateTime.Now.Date)
                                    {
                                        MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDateTime(calDueDate) >= DateTime.Now.Date)
                                    {
                                        MessageBox.Show("Invalid Status.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (dtGridloiDate.Rows.Count > 0)
                                {
                                    DataGridViewRow row = dtGridloiDate.Rows[0];
                                    string grdloiDate = row.Cells["LoiDate"].Value.ToString();

                                    if (OppStatus.ToLower() == "active")
                                    {
                                        if (Convert.ToDateTime(grdloiDate) < DateTime.Now.Date)
                                        {
                                            MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                    else if (OppStatus.ToLower() == "inactive")
                                    {
                                        if (Convert.ToDateTime(calDueDate) >= DateTime.Now.Date)
                                        {
                                            MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(grdloiDate) >= DateTime.Now.Date)
                                        {
                                            MessageBox.Show("Invalid Status.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (DataGridViewRow row in dtGridDueDate.Rows)
                        {
                            string grdDueDate = row.Cells["DueDate"].Value.ToString();

                            if (grdDueDate == calDueDate)
                            {
                                MessageBox.Show("Due Date can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (grdDueDate == "not-specified" || grdDueDate == "ongoing")
                            {
                                MessageBox.Show("Invalid input in Due Date.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else if (Convert.ToDateTime(grdDueDate) >= Convert.ToDateTime(calDueDate))
                            {
                                MessageBox.Show("Due Date can't be same or less.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    else
                    {
                        calDueDate = Convert.ToString(ddlDueDate.SelectedItem);

                        if (dtGridDueDate.Rows.Count > 0)
                        {
                            MessageBox.Show("Invalid input in Due Date.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    int SEQUENCE_ID = 0;
                    int sl = Convert.ToInt32(dtGridDueDate.Rows.Count + 1);
                    string DUE_DATE = string.Empty;

                    if (calDueDate == "not-specified" || calDueDate == "ongoing")
                    {
                        DUE_DATE = Convert.ToString(calDueDate);
                    }
                    else
                    {
                        DUE_DATE = Convert.ToDateTime(calDueDate).ToShortDateString();

                        if (OppStatus.ToLower() == "active")
                        {
                            if (Convert.ToDateTime(DUE_DATE) <= DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    string DateRemarks = txtDueDateDes.Text.Trim();
                    string lang = ddlLangDueD.SelectedValue.ToString();
                    string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), DUE_DATE, DateRemarks, lang };

                    dtGridDueDate.Rows.Add(rowGrid);
                    txtDueDateDes.Text = "";
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddLoiDate_Click(object sender, EventArgs e)
        {
            string url_txtLoiDate_dec = txtLoiDate_dec.Text.TrimStart().TrimEnd();

            if (url_txtLoiDate_dec.Contains("http://") || url_txtLoiDate_dec.Contains("https://") || url_txtLoiDate_dec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtLoiDate_dec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtLoiDate_dec.Text.Trim(), "Loi Date Description");

                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    lblMsg.Visible = false;
                    string calLOIdate = "";

                    if (ddlLoiDate.SelectedIndex == 0)
                    {
                        calLOIdate = txtloidate_Cal.Text;

                        if (dtGridDueDate.Rows.Count > 0)
                        {
                            int iCheck = 0;

                            if (dtGridloiDate.Rows.Count > 0)
                            {
                            HERE:
                                try
                                {
                                    DataGridViewRow rowLoi = dtGridDueDate.Rows[iCheck];
                                    string grdDueDateCheck = rowLoi.Cells["DueDate"].Value.ToString();

                                    if (grdDueDateCheck == "not-specified" || grdDueDateCheck == "ongoing")
                                    { }
                                    else
                                    {
                                        if (Convert.ToDateTime(calLOIdate) > Convert.ToDateTime(grdDueDateCheck))
                                        {
                                            MessageBox.Show("LOIDate can not be greater than DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    oErrorLog.WriteErrorLog(ex);
                                    goto HERE;
                                }
                            }

                        }

                        if (dtGridloiDate.Rows.Count == 0)
                        {
                            if (dtGridDueDate.Rows.Count > 0)
                            {
                                DataGridViewRow rowDue = dtGridDueDate.Rows[0];
                                string grdDueDateCheck = rowDue.Cells["DueDate"].Value.ToString();

                                if (grdDueDateCheck == "not-specified" || grdDueDateCheck == "ongoing")
                                { }
                                else
                                {
                                    if (Convert.ToDateTime(calLOIdate) > Convert.ToDateTime(grdDueDateCheck))
                                    {
                                        MessageBox.Show("LOIDate can not be less than DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                            }
                        }

                        foreach (DataGridViewRow row in dtGridloiDate.Rows)
                        {
                            string grdLOIDate = row.Cells["LoiDate"].Value.ToString();

                            if (grdLOIDate == calLOIdate)
                            {
                                MessageBox.Show("LOIDate can't be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else if (Convert.ToDateTime(grdLOIDate) >= Convert.ToDateTime(calLOIdate))
                            {
                                MessageBox.Show("LOIDate can't be same or less.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (dtGridDueDate.RowCount > 0)
                        {
                            DataGridViewRow rowDueChk = dtGridDueDate.Rows[dtGridDueDate.RowCount - 1];
                            string grdDueDateCheck = rowDueChk.Cells["DueDate"].Value.ToString();

                            if (grdDueDateCheck == "not-specified" || grdDueDateCheck == "ongoing")
                            { }
                            else
                            {
                                if (Convert.ToDateTime(grdDueDateCheck) <= Convert.ToDateTime(calLOIdate))
                                {
                                    MessageBox.Show("LOIDate can't be same or less.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                                int i = 0;

                                foreach (DataGridViewRow row in dtGridDueDate.Rows)
                                {
                                    i++;
                                    string grdDueDate = row.Cells["DueDate"].Value.ToString();

                                    if (Convert.ToDateTime(grdDueDate) < Convert.ToDateTime(calLOIdate))
                                    {
                                        MessageBox.Show("DueDate " + i + ": " + grdDueDate + " is less then to LOIDate: " + calLOIdate, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        calLOIdate = Convert.ToString(ddlLoiDate.SelectedItem);

                        if (dtGridloiDate.Rows.Count > 0)
                        {
                            MessageBox.Show("Invalid input in Due Date.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }

                    int SEQUENCE_ID = 0;
                    int sl = Convert.ToInt32(dtGridloiDate.Rows.Count + 1);
                    string LOI_DATE = string.Empty;

                    if (calLOIdate == "not-specified" || calLOIdate == "ongoing")
                    {
                        LOI_DATE = Convert.ToString(calLOIdate);
                    }
                    else
                    {
                        LOI_DATE = Convert.ToDateTime(calLOIdate).ToShortDateString();

                        if (Convert.ToDateTime(LOI_DATE) <= DateTime.Now.Date)
                        {
                            MessageBox.Show("LOI date is past do you want to continue", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LOI_DATE = Convert.ToDateTime(calLOIdate).ToShortDateString();
                        }
                    }

                    string DateRemarks = txtLoiDate_dec.Text.Trim();
                    string lang = ddlLangLoiD.SelectedValue.ToString();
                    string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), LOI_DATE, DateRemarks, lang };
                    dtGridloiDate.Rows.Add(rowGrid);
                    txtLoiDate_dec.Text = "";
                }
                catch { }
            }
        }

        private void btnAddOpenDate_Click(object sender, EventArgs e)
        {
            string url_txtOpenDateDesc = txtOpenDateDesc.Text.TrimStart().TrimEnd();

            if (url_txtOpenDateDesc.Contains("http://") || url_txtOpenDateDesc.Contains("https://") || url_txtOpenDateDesc.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtOpenDateDesc.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtOpenDateDesc.Text.Trim(), "Open Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (dtGridOpenDate.Rows.Count < 20)
                    {
                        lblMsg.Visible = false;
                        string calOpenDate = "";
                        string lang = "";

                        if (ddlOpenDate.SelectedIndex == 0)
                        {
                            calOpenDate = txtOpenDate_Cal.Text;
                        }
                        else
                        {
                            calOpenDate = Convert.ToString(ddlOpenDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(dtGridOpenDate.Rows.Count + 1);
                        string OPEN_DATE = string.Empty;

                        if (calOpenDate == "not-specified" || calOpenDate == "ongoing")
                        {
                            OPEN_DATE = Convert.ToString(calOpenDate);
                        }
                        else
                        {
                            OPEN_DATE = calOpenDate;
                        }

                        string DateRemarks = txtOpenDateDesc.Text.Trim();
                        lang = ddlLangOpD.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), OPEN_DATE, DateRemarks, lang };
                        dtGridOpenDate.Rows.Add(rowGrid);
                        txtOpenDateDesc.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one Open Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddExpirationDate_Click(object sender, EventArgs e)
        {
            string url_txtExpirationDateDec = txtExpirationDateDec.Text.TrimStart().TrimEnd();

            if (url_txtExpirationDateDec.Contains("http://") || url_txtExpirationDateDec.Contains("https://") || url_txtExpirationDateDec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtExpirationDateDec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtExpirationDateDec.Text.Trim(), "Expiration Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (grdExpirationDate.Rows.Count < 1)
                    {

                        lblMsg.Visible = false;
                        string calExpirationDate = "";

                        if (ddlExpirationDate.SelectedIndex == 0)
                        {
                            calExpirationDate = txtExpirationDate.Text;
                        }
                        else
                        {
                            calExpirationDate = Convert.ToString(ddlExpirationDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(grdExpirationDate.Rows.Count + 1);
                        string EXP_DATE = string.Empty;

                        if (calExpirationDate == "not-specified" || calExpirationDate == "ongoing")
                        {
                            EXP_DATE = Convert.ToString(calExpirationDate);
                        }
                        else
                        {
                            EXP_DATE = calExpirationDate;
                        }

                        string DateRemarks = txtExpirationDateDec.Text.Trim();
                        string lang = ddlLangED.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), EXP_DATE, DateRemarks, lang };

                        grdExpirationDate.Rows.Add(rowGrid);
                        txtOpenDateDesc.Text = "";
                        txtExpirationDateDec.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one Expiration Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddFirstPostDate_Click(object sender, EventArgs e)
        {
            string url_txtFirstPostDateDec = txtFirstPostDateDec.Text.TrimStart().TrimEnd();

            if (url_txtFirstPostDateDec.Contains("http://") || url_txtFirstPostDateDec.Contains("https://") || url_txtFirstPostDateDec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtFirstPostDateDec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtFirstPostDateDec.Text.Trim(), "FirstPost Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (grdFirstPostDate.Rows.Count < 1)
                    {
                        lblMsg.Visible = false;
                        string calFirstPostDate = "";

                        if (ddlFirstPostDate.SelectedIndex == 0)
                        {
                            calFirstPostDate = txtFirstPostDate_cal.Text;
                        }
                        else
                        {
                            calFirstPostDate = Convert.ToString(ddlFirstPostDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(grdFirstPostDate.Rows.Count + 1);
                        string FIRST_POST_DATE = string.Empty;

                        if (calFirstPostDate == "not-specified" || calFirstPostDate == "ongoing")
                        {
                            FIRST_POST_DATE = Convert.ToString(calFirstPostDate);
                        }
                        else
                        {
                            FIRST_POST_DATE = calFirstPostDate;
                        }

                        string DateRemarks = txtFirstPostDateDec.Text.Trim();
                        string lang = ddlLangFPD.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), FIRST_POST_DATE, DateRemarks, lang };
                        grdFirstPostDate.Rows.Add(rowGrid);
                        txtFirstPostDateDec.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one FirstPost Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btnAddLastModifiedPostDate_Click(object sender, EventArgs e)
        {
            string url_txtLastModifiedPostDate_dec = txtLastModifiedPostDate_dec.Text.TrimStart().TrimEnd();

            if (url_txtLastModifiedPostDate_dec.Contains("http://") || url_txtLastModifiedPostDate_dec.Contains("https://") || url_txtLastModifiedPostDate_dec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Rantosh---15 nov 2017----//
                    if (txtLastModifiedPostDate_dec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtLastModifiedPostDate_dec.Text.Trim(), "LastModifiedPost Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (grdLastModifiedPostDate.Rows.Count < 1)
                    {
                        lblMsg.Visible = false;
                        string calLastModifiedPostDate = "";

                        if (ddlLastModifiedPostDate.SelectedIndex == 0)
                        {
                            calLastModifiedPostDate = txtLastModifiedPostDate_cal.Text;
                        }
                        else
                        {
                            calLastModifiedPostDate = Convert.ToString(ddlLastModifiedPostDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(grdLastModifiedPostDate.Rows.Count + 1);
                        string LAST_MODFIY_DATE = string.Empty;

                        if (calLastModifiedPostDate == "not-specified" || calLastModifiedPostDate == "ongoing")
                        {
                            LAST_MODFIY_DATE = Convert.ToString(calLastModifiedPostDate);
                        }
                        else
                        {
                            LAST_MODFIY_DATE = calLastModifiedPostDate;
                        }

                        string DateRemarks = txtLastModifiedPostDate_dec.Text.Trim();
                        string lang = ddlLangLMPD.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), LAST_MODFIY_DATE, DateRemarks, lang };
                        grdLastModifiedPostDate.Rows.Add(rowGrid);
                        txtLastModifiedPostDate_dec.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one Last Modified Post Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void dtGridloiDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (dtGridloiDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = dtGridloiDate.Rows[dtGridloiDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["SEQUENCE_ID"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.dtGridloiDate.Rows[dtGridloiDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    dtGridloiDate.Rows.RemoveAt(this.dtGridloiDate.Rows[dtGridloiDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("SEQUENCE_ID");
                                dt.Columns.Add("LoiDate");

                                foreach (DataGridViewRow rowData in dtGridloiDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["SEQUENCE_ID"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["SEQUENCE_ID"].Value.ToString(), rowData.Cells["LoiDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 1, SEQUENCE_ID, 0);
                                dtGridloiDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvLOIDate;
                                    dvLOIDate = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvLOIDate.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvLOIDate.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvLOIDate[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvLOIDate[iContext]["LOI_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvLOIDate[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        dtGridloiDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(1);
                                }                                
                            }

                            MessageBox.Show("LOI Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dtGridOpenDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dtGridOpenDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = dtGridOpenDate.Rows[dtGridOpenDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["SEQUENCEID"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.dtGridOpenDate.Rows[dtGridOpenDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    dtGridOpenDate.Rows.RemoveAt(this.dtGridOpenDate.Rows[dtGridOpenDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("SEQUENCEID");
                                dt.Columns.Add("Open_Date");

                                foreach (DataGridViewRow rowData in dtGridOpenDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["SEQUENCEID"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["SEQUENCEID"].Value.ToString(), rowData.Cells["Open_Date"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 6, SEQUENCE_ID, 0);
                                dtGridOpenDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvopendate_data;
                                    dvopendate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvopendate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvopendate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvopendate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvopendate_data[iContext]["OPEN_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvopendate_data[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        dtGridOpenDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(6);
                                }
                            }

                            MessageBox.Show("OPEN Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dtGridDueDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (dtGridDueDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = dtGridDueDate.Rows[dtGridDueDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["seq"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.dtGridDueDate.Rows[dtGridDueDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    dtGridDueDate.Rows.RemoveAt(this.dtGridDueDate.Rows[dtGridDueDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                                flagchangeforoppdate();
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("seq");
                                dt.Columns.Add("DueDate");

                                foreach (DataGridViewRow rowData in dtGridDueDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["seq"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["seq"].Value.ToString(), rowData.Cells["DueDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 2, SEQUENCE_ID, 0);
                                dtGridDueDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvLOIDate;
                                    dvLOIDate = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvLOIDate.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvLOIDate.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvLOIDate[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvLOIDate[iContext]["DUE_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvLOIDate[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        dtGridDueDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(2);
                                }
                            }

                            MessageBox.Show("DUE Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdFirstPostDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (grdFirstPostDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = grdFirstPostDate.Rows[grdFirstPostDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["SeqID"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.grdFirstPostDate.Rows[grdFirstPostDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    grdFirstPostDate.Rows.RemoveAt(this.grdFirstPostDate.Rows[grdFirstPostDate.SelectedCells[0].RowIndex].Index);
                                }
                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("SeqID");
                                dt.Columns.Add("FirstPostDate");

                                foreach (DataGridViewRow rowData in grdFirstPostDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["SeqID"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["SeqID"].Value.ToString(), rowData.Cells["FirstPostDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 4, SEQUENCE_ID, 0);
                                grdFirstPostDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvfirstpostdate_data;
                                    dvfirstpostdate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvfirstpostdate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvfirstpostdate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvfirstpostdate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvfirstpostdate_data[iContext]["FIRSTPOST_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvfirstpostdate_data[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        grdFirstPostDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(4);
                                }
                            }

                            MessageBox.Show("First Post date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdLastModifiedPostDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (grdLastModifiedPostDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = grdLastModifiedPostDate.Rows[grdLastModifiedPostDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["Seq_Id"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.grdLastModifiedPostDate.Rows[grdLastModifiedPostDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    grdLastModifiedPostDate.Rows.RemoveAt(this.grdLastModifiedPostDate.Rows[grdLastModifiedPostDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Seq_Id");
                                dt.Columns.Add("LastModifiedPostDate");

                                foreach (DataGridViewRow rowData in grdLastModifiedPostDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["Seq_Id"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["Seq_Id"].Value.ToString(), rowData.Cells["LastModifiedPostDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 5, SEQUENCE_ID, 0);
                                grdLastModifiedPostDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvlastmodifiedpostdate_data;
                                    dvlastmodifiedpostdate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvlastmodifiedpostdate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvlastmodifiedpostdate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvlastmodifiedpostdate_data[iContext]["LASTMODIFEDPOST_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        grdLastModifiedPostDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(5);
                                }
                            }

                            MessageBox.Show("Last Modified Post Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdExpirationDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (grdExpirationDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = grdExpirationDate.Rows[grdExpirationDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["SequId"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.grdExpirationDate.Rows[grdExpirationDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    grdExpirationDate.Rows.RemoveAt(this.grdExpirationDate.Rows[grdExpirationDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("SequId");
                                dt.Columns.Add("ExpirationDate");

                                foreach (DataGridViewRow rowData in grdExpirationDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["SequId"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["SequId"].Value.ToString(), rowData.Cells["ExpirationDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 3, SEQUENCE_ID, 0);
                                grdExpirationDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvexpirationdate_data;
                                    dvexpirationdate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvexpirationdate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvexpirationdate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvexpirationdate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvexpirationdate_data[iContext]["EXPIRATION_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvexpirationdate_data[iContext]["DATE_REMARKS"]);
                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }
                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        grdExpirationDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(3);
                                }
                            }

                            MessageBox.Show("Expiration Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public bool getisdatesame(string[] array, string _date)
        {
            bool _result = false;

            for (int i = 0; i < array.Count(); i++)
            {
                int res = Array.IndexOf(array, _date);
                if (res > -1)
                {
                    _result = true;
                    break;
                }
            }

            return _result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((OpportunityDataOperations.CheckOppStatus(SharedObjects.WorkId).ToString().ToLower() == "Inactive".ToLower()) && ddl_Recurring.SelectedIndex == 1)
            {
                MessageBox.Show("This is Recuuring Opportunity ,Check Opportunity Status", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            if (ddl_Recurring.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Recurring Status", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (SharedObjects.Cycle == 0 || SharedObjects.Cycle == 1)
            {
                if (dtGridDueDate.Rows.Count > 0)
                {
                    DataGridViewRow row = dtGridDueDate.Rows[dtGridDueDate.Rows.Count - 1];
                    string grdDueDate = row.Cells["DueDate"].Value.ToString();
                    if (grdDueDate == "not-specified" || grdDueDate == "ongoing")
                    {
                    }
                    else
                    {
                        if (OppStatus.ToLower() == "active")
                        {
                            if (Convert.ToDateTime(grdDueDate) <= DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        if (Convert.ToDateTime(grdDueDate) > DateTime.Now.Date && OppStatus.ToLower() == "inactive")
                        {

                            MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;

                        }
                    }
                }
            }
            else if (SharedObjects.TaskId == 7)
            {
                if (dtGridDueDate.Rows.Count > 0)
                {
                    DataGridViewRow row = dtGridDueDate.Rows[dtGridDueDate.Rows.Count - 1];
                    string grdDueDate = row.Cells["DueDate"].Value.ToString();
                    if (grdDueDate == "not-specified" || grdDueDate == "ongoing")
                    {
                    }
                    else
                    {
                        if (OppStatus.ToLower() == "active")
                        {
                            if (Convert.ToDateTime(grdDueDate) < DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            if (Convert.ToDateTime(grdDueDate) >= DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid Status.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (dtGridloiDate.Rows.Count > 0)
                    {
                        DataGridViewRow row = dtGridloiDate.Rows[0];
                        string grdloiDate = row.Cells["LoiDate"].Value.ToString();
                        if (OppStatus.ToLower() == "active")
                        {
                            if (Convert.ToDateTime(grdloiDate) < DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else if (OppStatus.ToLower() == "inactive")
                        {
                            DataGridViewRow rowDueDate = dtGridDueDate.Rows[0];
                            string grdDueDate = rowDueDate.Cells["DueDate"].Value.ToString();
                            if (Convert.ToDateTime(grdDueDate) >= DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid DueDate.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            if (Convert.ToDateTime(grdloiDate) >= DateTime.Now.Date)
                            {
                                MessageBox.Show("Invalid Status.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }
            }

            if (dtGridloiDate.Rows.Count > 0 && dtGridDueDate.Rows.Count > 0)
            {
                if (dtGridloiDate.Rows.Count > dtGridDueDate.Rows.Count)
                {
                    string grdDueDate = "";
                    foreach (DataGridViewRow row in dtGridDueDate.Rows)
                    {
                        grdDueDate = row.Cells["DueDate"].Value.ToString();
                    }
                    foreach (DataGridViewRow row21 in dtGridloiDate.Rows)
                    {
                        string grdLoiDate = row21.Cells["LoiDate"].Value.ToString();
                        if (valid_date(grdDueDate) == true && valid_date(grdLoiDate) == true)
                        {
                            if (Convert.ToDateTime(grdDueDate) < Convert.ToDateTime(grdLoiDate))
                            {
                                MessageBox.Show("DueDate " + ": " + grdDueDate + " is less then to LOIDate: " + grdLoiDate, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                string strDueDate = "";

                foreach (DataGridViewRow row in dtGridDueDate.Rows)
                {
                    if (row.Cells["DueDate"].Value.ToString() == "not-specified" || row.Cells["DueDate"].Value.ToString() == "ongoing")
                    {
                    }
                    else
                    {
                        strDueDate += row.Cells["DueDate"].Value.ToString() + ",";
                    }
                }

                strDueDate = strDueDate.TrimEnd(',');

                string[] aduedate = strDueDate.Split(',');

                foreach (DataGridViewRow row21 in dtGridloiDate.Rows)
                {
                    string strLoiDate = row21.Cells["LoiDate"].Value.ToString();
                    if (getisdatesame(aduedate, strLoiDate))
                    {
                        MessageBox.Show("DueDate " + ": " + strLoiDate + " and LOIDate: " + strLoiDate + " can not be same.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            #region  Validation for Firstpostdate
            if (grdFirstPostDate.RowCount == 0)
            {
                MessageBox.Show("Please Enter FirstPost Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            DateTime?[] ArrayLoiDate;
            DateTime?[] ArrayDueDate;
            DateTime?[] ArrayExpirationDate;
            DateTime?[] ArrayPreProposalDate;
            DateTime?[] ArrayFirstPostDate;
            DateTime?[] ArrayLastModifedPostDate;
            DateTime?[] ArrayOpenDate;
            DateTime?[] ArrayDecisionDate;

            string[] DecLoiDate = null;
            string[] DecDueDate = null;
            string[] DecExpirationDate = null;
            string[] DecPreProposalDate = null;
            string[] DecFirstPostDate = null;
            string[] DecLastModifedPostDate = null;
            string[] DecOpenDate = null;
            string[] DecDecisionDate = null;
            string[] DecLoiLang = null;
            string[] DecDueLang = null;
            string[] DecExpirationLang = null;
            string[] DecPreProposalLang = null;
            string[] DecFirstPostLang = null;
            string[] DecLastModifedPostLang = null;
            string[] DecOpenLang = null;
            string[] DecDecisionLang = null;

            int incr = 0;

            #region Loi Date

            if (dtGridloiDate.Rows.Count == 0)
            {
                ArrayLoiDate = new DateTime?[1];
                DecLoiDate = new string[1];
                DecLoiLang = new string[1];
                ArrayLoiDate[0] = null;
                DecLoiDate[0] = null;
                DecLoiLang[0] = null;
            }
            else
            {
                ArrayLoiDate = new DateTime?[dtGridloiDate.Rows.Count];
                DecLoiDate = new string[dtGridloiDate.Rows.Count];
                DecLoiLang = new string[dtGridloiDate.Rows.Count];
                if (dtGridloiDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in dtGridloiDate.Rows)
                    {
                        string DecrLoiDate = "";
                        string DecrLoiLang = "";
                        string LoiDate = row.Cells["LoiDate"].Value.ToString();

                        if (replace.chk_OtherLang(row.Cells["Lang_Loi"].Value.ToString().ToLower()) == true)
                        {
                            DecrLoiDate = replace.ConvertTextToUnicode(row.Cells["LoiDateDescription"].Value.ToString());
                            DecrLoiLang = (row.Cells["Lang_Loi"].Value.ToString());
                        }
                        else
                        {
                            DecrLoiDate = row.Cells["LoiDateDescription"].Value.ToString();
                            DecrLoiLang = (row.Cells["Lang_Loi"].Value.ToString());
                        }

                        if (LoiDate == "not-specified")
                        {
                            ArrayLoiDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (LoiDate == "ongoing")
                        {
                            ArrayLoiDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayLoiDate[incr] = Convert.ToDateTime(LoiDate);
                        }

                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrLoiDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrLoiDate.Trim(), "Loi Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion

                        DecLoiDate[incr] = DecrLoiDate;
                        DecLoiLang[incr] = DecrLoiLang;
                        incr++;
                    }
                }
            }

            #endregion

            #region Due Date

            if (dtGridDueDate.Rows.Count == 0)
            {
                MessageBox.Show("Please enter Due Date.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                ArrayDueDate = new DateTime?[dtGridDueDate.Rows.Count];
                DecDueDate = new string[dtGridDueDate.Rows.Count];
                DecDueLang = new string[dtGridDueDate.Rows.Count];
                if (dtGridDueDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in dtGridDueDate.Rows)
                    {
                        string DueDate = row.Cells["DueDate"].Value.ToString();
                        string DecrDueDate = "";
                        string DecrDueLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_Due"].Value.ToString().ToLower()) == true)
                        {
                            DecrDueDate = replace.ConvertTextToUnicode(row.Cells["DueDateDescription"].Value.ToString());
                            DecrDueLang = (row.Cells["Lang_Due"].Value.ToString());
                        }

                        else
                        {
                            DecrDueDate = row.Cells["DueDateDescription"].Value.ToString();
                            DecrDueLang = (row.Cells["Lang_Due"].Value.ToString());
                        }
                        if (DueDate == "not-specified")
                        {
                            ArrayDueDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (DueDate == "ongoing")
                        {
                            ArrayDueDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayDueDate[incr] = Convert.ToDateTime(DueDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrDueDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrDueDate.Trim(), "Due Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecDueDate[incr] = DecrDueDate;
                        DecDueLang[incr] = DecrDueLang;
                        incr++;
                    }
                }
                else
                {
                    ArrayDueDate[incr] = null;
                    DecDueDate[incr] = null;
                }
            }
            #endregion

            #region Expiration Date

            if (grdExpirationDate.Rows.Count == 0)
            {
                ArrayExpirationDate = new DateTime?[1];
                DecExpirationDate = new string[1];
                DecExpirationLang = new string[1];
                ArrayExpirationDate[0] = null;
                DecExpirationDate[0] = null;
                DecExpirationLang[0] = null;
            }
            else
            {
                ArrayExpirationDate = new DateTime?[grdExpirationDate.Rows.Count];
                DecExpirationDate = new string[grdExpirationDate.Rows.Count];
                DecExpirationLang = new string[grdExpirationDate.Rows.Count];
                if (grdExpirationDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in grdExpirationDate.Rows)
                    {
                        string ExpirationDate = row.Cells["ExpirationDate"].Value.ToString();
                        string DecrExpirationDate = "";
                        string DecrExpirationLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_Exp"].Value.ToString().ToLower()) == true)
                        {
                            DecrExpirationDate = replace.ConvertTextToUnicode(row.Cells["ExpirationDateDescription"].Value.ToString());
                            DecrExpirationLang = (row.Cells["Lang_Exp"].Value.ToString());
                        }
                        else
                        {

                            DecrExpirationDate = row.Cells["ExpirationDateDescription"].Value.ToString();
                            DecrExpirationLang = (row.Cells["Lang_Exp"].Value.ToString());
                        }
                        if (ExpirationDate == "not-specified")
                        {
                            ArrayExpirationDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (ExpirationDate == "ongoing")
                        {
                            ArrayExpirationDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayExpirationDate[incr] = Convert.ToDateTime(ExpirationDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrExpirationDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrExpirationDate.Trim(), "Expiration Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecExpirationDate[incr] = DecrExpirationDate;
                        DecExpirationLang[incr] = DecrExpirationLang;
                        incr++;
                    }
                }
            }
            #endregion

            #region PreProposal Date

            if (grdPreProposalDate.Rows.Count == 0)
            {
                ArrayPreProposalDate = new DateTime?[1];
                DecPreProposalDate = new string[1];
                DecPreProposalLang = new string[1];
                ArrayPreProposalDate[0] = null;
                DecPreProposalDate[0] = null;
                DecPreProposalLang[0] = null;
            }
            else
            {
                ArrayPreProposalDate = new DateTime?[grdPreProposalDate.Rows.Count];
                DecPreProposalDate = new string[grdPreProposalDate.Rows.Count];
                DecPreProposalLang = new string[grdPreProposalDate.Rows.Count];
                if (grdPreProposalDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in grdPreProposalDate.Rows)
                    {
                        string PreProposalDate = row.Cells["PreProposalDate"].Value.ToString();
                        string DecrPreProposalDate = "";
                        string DecrPreProposalLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_Pre"].Value.ToString().ToLower()) == true)
                        {
                            DecrPreProposalDate = replace.ConvertTextToUnicode(row.Cells["PreProposalDateDescription"].Value.ToString());
                            DecrPreProposalLang = (row.Cells["Lang_Pre"].Value.ToString());
                        }
                        else
                        {
                            DecrPreProposalDate = row.Cells["PreProposalDateDescription"].Value.ToString();
                            DecrPreProposalLang = (row.Cells["Lang_Pre"].Value.ToString());
                        }
                        if (PreProposalDate == "not-specified")
                        {
                            ArrayPreProposalDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (PreProposalDate == "ongoing")
                        {
                            ArrayPreProposalDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayPreProposalDate[incr] = Convert.ToDateTime(PreProposalDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrPreProposalDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrPreProposalDate.Trim(), "PreProposal Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecPreProposalDate[incr] = DecrPreProposalDate;
                        DecPreProposalLang[incr] = DecrPreProposalLang;
                        incr++;
                    }
                }
            }
            #endregion

            #region FirstPost Date

            if (grdFirstPostDate.Rows.Count == 0)
            {
                ArrayFirstPostDate = new DateTime?[1];
                DecFirstPostDate = new string[1];
                DecFirstPostLang = new string[1];
                ArrayFirstPostDate[0] = null;
                DecFirstPostDate[0] = null;
                DecFirstPostLang[0] = null;
            }
            else
            {
                ArrayFirstPostDate = new DateTime?[grdFirstPostDate.Rows.Count];
                DecFirstPostDate = new string[grdFirstPostDate.Rows.Count];
                DecFirstPostLang = new string[grdFirstPostDate.Rows.Count];
                if (grdFirstPostDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in grdFirstPostDate.Rows)
                    {
                        string FirstPostDate = row.Cells["FirstPostDate"].Value.ToString();
                        string DecrFirstPostDate = "";
                        string DecrFirstPostLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_First"].Value.ToString().ToLower()) == true)
                        {
                            DecrFirstPostDate = replace.ConvertTextToUnicode(row.Cells["FirstPostDateDescription"].Value.ToString());
                            DecrFirstPostLang = (row.Cells["Lang_First"].Value.ToString());
                        }
                        else
                        {
                            DecrFirstPostDate = row.Cells["FirstPostDateDescription"].Value.ToString();
                            DecrFirstPostLang = (row.Cells["Lang_First"].Value.ToString());
                        }
                        if (FirstPostDate == "not-specified")
                        {
                            ArrayFirstPostDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (FirstPostDate == "ongoing")
                        {
                            ArrayFirstPostDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayFirstPostDate[incr] = Convert.ToDateTime(FirstPostDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrFirstPostDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrFirstPostDate.Trim(), "FirstPost Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecFirstPostDate[incr] = DecrFirstPostDate;
                        DecFirstPostLang[incr] = DecrFirstPostLang;
                        incr++;
                    }
                }

            }
            #endregion

            #region LastModifedPost Date

            if (grdLastModifiedPostDate.Rows.Count == 0)
            {
                ArrayLastModifedPostDate = new DateTime?[1];
                DecLastModifedPostDate = new string[1];
                DecLastModifedPostLang = new string[1];
                ArrayLastModifedPostDate[0] = null;
                DecLastModifedPostDate[0] = null;
                DecLastModifedPostLang[0] = null;
            }
            else
            {
                ArrayLastModifedPostDate = new DateTime?[grdLastModifiedPostDate.Rows.Count];
                DecLastModifedPostDate = new string[grdLastModifiedPostDate.Rows.Count];
                DecLastModifedPostLang = new string[grdLastModifiedPostDate.Rows.Count];
                if (grdLastModifiedPostDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in grdLastModifiedPostDate.Rows)
                    {
                        string LastModifedPostDate = row.Cells["LastModifiedPostDate"].Value.ToString();
                        string DecrLastModifedPostDateDate = "";
                        string DecrLastModifedPostLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_Last"].Value.ToString().ToLower()) == true)
                        {
                            DecrLastModifedPostDateDate = replace.ConvertTextToUnicode(row.Cells["LastModifiedPostDateDescription"].Value.ToString());
                            DecrLastModifedPostLang = (row.Cells["Lang_Last"].Value.ToString());
                        }
                        else
                        {
                            DecrLastModifedPostDateDate = row.Cells["LastModifiedPostDateDescription"].Value.ToString();
                            DecrLastModifedPostLang = (row.Cells["Lang_Last"].Value.ToString());
                        }
                        if (LastModifedPostDate == "not-specified")
                        {
                            ArrayLastModifedPostDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (LastModifedPostDate == "ongoing")
                        {
                            ArrayLastModifedPostDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayLastModifedPostDate[incr] = Convert.ToDateTime(LastModifedPostDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrLastModifedPostDateDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrLastModifedPostDateDate.Trim(), "LastModifedPost Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecLastModifedPostDate[incr] = DecrLastModifedPostDateDate;
                        DecLastModifedPostLang[incr] = DecrLastModifedPostLang;
                        incr++;
                    }
                }

            }
            #endregion

            #region Open Date

            if (dtGridOpenDate.Rows.Count == 0)
            {
                ArrayOpenDate = new DateTime?[1];
                DecOpenDate = new string[1];
                DecOpenLang = new string[1];
                ArrayOpenDate[0] = null;
                DecOpenDate[0] = null;
                DecOpenLang[0] = null;
            }
            else
            {
                ArrayOpenDate = new DateTime?[dtGridOpenDate.Rows.Count];
                DecOpenDate = new string[dtGridOpenDate.Rows.Count];
                DecOpenLang = new string[dtGridOpenDate.Rows.Count];
                if (dtGridOpenDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in dtGridOpenDate.Rows)
                    {
                        string OpenDate = row.Cells["Open_Date"].Value.ToString();
                        string DecrOpenDateDate = "";
                        string DecrOpenLang = "";

                        if (replace.chk_OtherLang(row.Cells["Lang_Open"].Value.ToString().ToLower()) == true)
                        {
                            DecrOpenDateDate = replace.ConvertTextToUnicode(row.Cells["OpenDateDescription"].Value.ToString());
                            DecrOpenLang = (row.Cells["Lang_Open"].Value.ToString());
                        }
                        else
                        {
                            DecrOpenDateDate = row.Cells["OpenDateDescription"].Value.ToString();
                            DecrOpenLang = (row.Cells["Lang_Open"].Value.ToString());
                        }
                        if (OpenDate == "not-specified")
                        {
                            ArrayOpenDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (OpenDate == "ongoing")
                        {
                            ArrayOpenDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayOpenDate[incr] = Convert.ToDateTime(OpenDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrOpenDateDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrOpenDateDate.Trim(), "Open Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecOpenDate[incr] = DecrOpenDateDate;
                        DecOpenLang[incr] = DecrOpenLang;
                        incr++;
                    }
                }
            }
            #endregion

            #region Decision Date

            if (grdDecisionDate.Rows.Count == 0)
            {
                ArrayDecisionDate = new DateTime?[1];
                DecDecisionDate = new string[1];
                DecDecisionLang = new string[1];
                ArrayDecisionDate[0] = null;
                DecDecisionDate[0] = null;
                DecDecisionLang[0] = null;
            }
            else
            {
                ArrayDecisionDate = new DateTime?[grdDecisionDate.Rows.Count];
                DecDecisionDate = new string[grdDecisionDate.Rows.Count];
                DecDecisionLang = new string[grdDecisionDate.Rows.Count];
                if (grdDecisionDate.Rows.Count > 0)
                {
                    incr = 0;
                    foreach (DataGridViewRow row in grdDecisionDate.Rows)
                    {
                        string DecisionDate = row.Cells["DecisionDate"].Value.ToString();
                        string DecrDecisionDate = "";
                        string DecrDecisionLang = "";

                        if (replace.chk_OtherLang(row.Cells["LANG_Decision"].Value.ToString().ToLower()) == true)
                        {
                            DecrDecisionDate = replace.ConvertTextToUnicode(row.Cells["DecisionDateDescription"].Value.ToString());
                            DecrDecisionLang = (row.Cells["LANG_Decision"].Value.ToString());
                        }
                        else
                        {
                            DecrDecisionDate = row.Cells["DecisionDateDescription"].Value.ToString();
                            DecrDecisionLang = (row.Cells["LANG_Decision"].Value.ToString());
                        }

                        if (DecisionDate == "not-specified")
                        {
                            ArrayDecisionDate[incr] = Convert.ToDateTime("01-01-1900");
                        }
                        else if (DecisionDate == "ongoing")
                        {
                            ArrayDecisionDate[incr] = Convert.ToDateTime("01-02-1900");
                        }
                        else
                        {
                            ArrayDecisionDate[incr] = Convert.ToDateTime(DecisionDate);
                        }
                        #region //----------validation html tags---------Rantosh---15 nov 2017----//
                        if (DecrDecisionDate != "")
                        {
                            string _result = oErrorLog.htlmtag(DecrDecisionDate.Trim(), "Decision Date Description");
                            if (!_result.Equals(""))
                            {
                                MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        #endregion
                        DecDecisionDate[incr] = DecrDecisionDate;
                        DecDecisionLang[incr] = DecrDecisionLang;
                        incr++;
                    }
                }
            }
            #endregion

            DataSet _resultdatedel = OpportunityDataOperations.Delete_Op_Dates(SharedObjects.WorkId);


            if (dtGridloiDate.Rows.Count > 0)
            {
                for (int i = 0; i < dtGridloiDate.Rows.Count; i++)
                {
                    DataSet _resultloiDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayLoiDate[i]), 1, i + 1, DecLoiDate[i], DecLoiLang[i], URL_LOIDate.Text.ToString().Trim());
                }
            }

            if (dtGridDueDate.Rows.Count > 0)
            {
                for (int i = 0; i < dtGridDueDate.Rows.Count; i++)
                {
                    DataSet _resultloiDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayDueDate[i]), 2, i + 1, DecDueDate[i], DecDueLang[i], URL_ProposalDate.Text.ToString().Trim());
                }
            }

            if (grdExpirationDate.Rows.Count > 0)
            {
                for (int i = 0; i < grdExpirationDate.Rows.Count; i++)
                {
                    DataSet _resultExpirationDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayExpirationDate[i]), 3, i + 1, DecExpirationDate[i], DecExpirationLang[i], URL_ExpiryDate.Text.ToString().Trim());
                }
            }

            if (grdFirstPostDate.Rows.Count > 0)
            {
                for (int i = 0; i < grdFirstPostDate.Rows.Count; i++)
                {
                    DataSet _resultFirstPostDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayFirstPostDate[i]), 4, i + 1, DecFirstPostDate[i], DecFirstPostLang[i], URL_PublishedDate.Text.ToString().Trim());
                }
            }

            if (grdLastModifiedPostDate.Rows.Count > 0)
            {
                for (int i = 0; i < grdLastModifiedPostDate.Rows.Count; i++)
                {
                    DataSet _resultModifiedPostDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayLastModifedPostDate[i]), 5, i + 1, DecLastModifedPostDate[i], DecLastModifedPostLang[i], URL_ModifiedDate.Text.ToString().Trim());
                }
            }

            if (dtGridOpenDate.Rows.Count > 0)
            {
                for (int i = 0; i < dtGridOpenDate.Rows.Count; i++)
                {
                    DataSet _resultOpenDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayOpenDate[i]), 6, i + 1, DecOpenDate[i], DecOpenLang[i], URL_StartDate.Text.ToString().Trim());
                }
            }

            if (grdPreProposalDate.Rows.Count > 0)
            {
                for (int i = 0; i < grdPreProposalDate.Rows.Count; i++)
                {
                    DataSet _resultPreProposalDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayPreProposalDate[i]), 7, i + 1, DecPreProposalDate[i], DecPreProposalLang[i], URL_PreProposalDate.Text.ToString().Trim());
                }
            }

            if (grdDecisionDate.Rows.Count > 0)
            {
                for (int i = 0; i < grdDecisionDate.Rows.Count; i++)
                {
                    DataSet _resultDecisionDate = OpportunityDataOperations.InsUp_Op_Dates(SharedObjects.WorkId, Convert.ToDateTime(ArrayDecisionDate[i]), 8, i + 1, DecDecisionDate[i], DecDecisionLang[i], URL_DecisionDate.Text.ToString().Trim());
                }
            }

            #region
            if (ddl_Recurring.SelectedIndex > 0)
            {
                OpportunityDataOperations.InsRecurring(SharedObjects.WorkId, ddl_Recurring.SelectedItem.ToString().Trim(), Convert.ToInt64(SharedObjects.User.USERID));
                SharedObjects.RecCurrentStatus = ddl_Recurring.SelectedIndex;
            }
            #endregion

            DataSet dsItems = OpportunityDataOperations.get_Op_DateList(SharedObjects.WorkId);

            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());


            #region For Changing Colour in case of Update
            if (SharedObjects.TRAN_TYPE_ID == 1)
            {
                m_parent.GetProcess_update("opportunityDates");
            }
            else
            {
                m_parent.GetProcess();
            }
            #endregion

            #region Bind Data
            if (dsItems.Tables["OppStatus"].Rows.Count > 0)
            {
                dtGridloiDate.Rows.Clear();
                OppStatus = Convert.ToString(dsItems.Tables["OppStatus"].Rows[0]["OPPORTUNITYSTATUS"]);
                DateTime dtaTime = new DateTime();

                if (dsItems.Tables["loi_data"].Rows.Count > 0)
                {
                    DataView dvLOIDate;
                    dvLOIDate = new DataView(dsItems.Tables["loi_data"].Copy());
                    dvLOIDate.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvLOIDate.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvLOIDate[iContext]["SEQUENCE_ID"]);
                        string isdate = dvLOIDate[iContext]["LOI_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvLOIDate[iContext]["LOI_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvLOIDate[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvLOIDate[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);
                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            dtGridloiDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["due_data"].Rows.Count > 0)
                {
                    dtGridDueDate.Rows.Clear();
                    DataView dvDueDate;
                    dvDueDate = new DataView(dsItems.Tables["due_data"].Copy());
                    dvDueDate.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvDueDate.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvDueDate[iContext]["SEQUENCE_ID"]);
                        string isdate = dvDueDate[iContext]["DUE_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvDueDate[iContext]["DUE_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvDueDate[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvDueDate[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            dtGridDueDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["expirationdate_data"].Rows.Count > 0)
                {
                    grdExpirationDate.Rows.Clear();
                    DataView dvexpirationdate_data;
                    dvexpirationdate_data = new DataView(dsItems.Tables["expirationdate_data"].Copy());
                    dvexpirationdate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvexpirationdate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvexpirationdate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvexpirationdate_data[iContext]["EXPIRATION_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvexpirationdate_data[iContext]["EXPIRATION_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvexpirationdate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvexpirationdate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            grdExpirationDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["firstpostdate_data"].Rows.Count > 0)
                {
                    grdFirstPostDate.Rows.Clear();
                    DataView dvfirstpostdate_data;
                    dvfirstpostdate_data = new DataView(dsItems.Tables["firstpostdate_data"].Copy());
                    dvfirstpostdate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvfirstpostdate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvfirstpostdate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvfirstpostdate_data[iContext]["FIRSTPOST_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvfirstpostdate_data[iContext]["FIRSTPOST_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvfirstpostdate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvfirstpostdate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            grdFirstPostDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["lastmodifedpostdate_data"].Rows.Count > 0)
                {
                    grdLastModifiedPostDate.Rows.Clear();
                    DataView dvlastmodifiedpostdate_data;
                    dvlastmodifiedpostdate_data = new DataView(dsItems.Tables["lastmodifedpostdate_data"].Copy());
                    dvlastmodifiedpostdate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvlastmodifiedpostdate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvlastmodifiedpostdate_data[iContext]["LASTMODIFEDPOST_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvlastmodifiedpostdate_data[iContext]["LASTMODIFEDPOST_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvlastmodifiedpostdate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            grdLastModifiedPostDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["opendate_data"].Rows.Count > 0)
                {
                    dtGridOpenDate.Rows.Clear();
                    DataView dvopendate_data;
                    dvopendate_data = new DataView(dsItems.Tables["opendate_data"].Copy());
                    dvopendate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvopendate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvopendate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvopendate_data[iContext]["OPEN_DATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvopendate_data[iContext]["OPEN_DATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvopendate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvopendate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            dtGridOpenDate.Rows.Add(row);
                        }
                    }
                }

                if (dsItems.Tables["PreProposalDate_data"].Rows.Count > 0)
                {
                    grdPreProposalDate.Rows.Clear();
                    DataView dvPreProposalDate_data;
                    dvPreProposalDate_data = new DataView(dsItems.Tables["PreProposalDate_data"].Copy());
                    dvPreProposalDate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvPreProposalDate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvPreProposalDate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvPreProposalDate_data[iContext]["PreProposalDATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvPreProposalDate_data[iContext]["PreProposalDATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvPreProposalDate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvPreProposalDate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            grdPreProposalDate.Rows.Add(row);
                        }
                    }
                }

                #region
                if (dsItems.Tables["DecisionDate_data"].Rows.Count > 0)
                {
                    grdDecisionDate.Rows.Clear();
                    DataView dvDecisionDate_data;
                    dvDecisionDate_data = new DataView(dsItems.Tables["DecisionDate_data"].Copy());
                    dvDecisionDate_data.Sort = "SEQUENCE_ID ASC";

                    for (int iContext = 0; iContext < dvDecisionDate_data.Count; iContext++)
                    {
                        string firstCol = Convert.ToString(dvDecisionDate_data[iContext]["SEQUENCE_ID"]);
                        string isdate = dvDecisionDate_data[iContext]["DecisionDATE"].ToString();

                        if (!isdate.Equals(""))
                        {
                            dtaTime = Convert.ToDateTime(dvDecisionDate_data[iContext]["DecisionDATE"].ToString());
                            string secondCol = dtaTime.ToShortDateString();
                            string thrdCol = Convert.ToString(dvDecisionDate_data[iContext]["DATE_REMARKS"]);
                            string FourthCol = Convert.ToString(dvDecisionDate_data[iContext]["LANG"]);

                            if (secondCol == "01-01-1900")
                            {
                                secondCol = "not-specified";
                            }
                            else if (secondCol == "01-02-1900")
                            {
                                secondCol = "ongoing";
                            }

                            if (thrdCol != "")
                            {
                                if (replace.chk_OtherLang(FourthCol.ToLower()) == true)
                                {
                                    string Dec_DiffLAng;
                                    Dec_DiffLAng = replace.ConvertUnicodeToText(thrdCol);

                                    if (Dec_DiffLAng != "")
                                    {
                                        thrdCol = Dec_DiffLAng;
                                    }
                                }
                            }

                            string[] row = { firstCol, firstCol, secondCol, thrdCol, FourthCol };
                            grdDecisionDate.Rows.Add(row);
                        }
                    }
                }
                #endregion
            }
            #endregion
        }

        private void flagchangeforoppdate()
        {
            if (dtGridloiDate.Rows.Count == 0 || dtGridDueDate.Rows.Count == 0 || grdExpirationDate.Rows.Count == 0 || grdFirstPostDate.Rows.Count == 0
                || grdLastModifiedPostDate.Rows.Count == 0 || dtGridOpenDate.Rows.Count == 0)
            {
                DataSet dsresult = OpportunityDataOperations.SCI_UPD_OPP_DATE_FLG(SharedObjects.WorkId);
            }
        }

        private void dtGridOpenDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dtGridOpenDate.Rows[dtGridOpenDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["OpenDateDescription"].Value);
            string OpenDate = row.Cells["Open_Date"].Value.ToString();

            SharedObjects.OppStatusdisp = "Open Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtOpenDateDesc.Text = Dis;

            if (this.dtGridOpenDate.Rows[e.RowIndex].Index >= 0)
            {
                dtGridOpenDate.Rows.RemoveAt(this.dtGridOpenDate.Rows[e.RowIndex].Index);
            }
        }

        private void dtGridloiDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dtGridloiDate.Rows[dtGridloiDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["LoiDateDescription"].Value);
            string OpenDate = row.Cells["LoiDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "Loi Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtLoiDate_dec.Text = Dis;

            if (this.dtGridloiDate.Rows[e.RowIndex].Index >= 0)
            {
                dtGridloiDate.Rows.RemoveAt(this.dtGridloiDate.Rows[e.RowIndex].Index);
            }
        }

        private void dtGridDueDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dtGridDueDate.Rows[dtGridDueDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["DueDateDescription"].Value);
            string OpenDate = row.Cells["DueDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "Due Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtDueDateDes.Text = Dis;

            if (this.dtGridDueDate.Rows[e.RowIndex].Index >= 0)
            {
                dtGridDueDate.Rows.RemoveAt(this.dtGridDueDate.Rows[e.RowIndex].Index);
            }
        }

        private void grdFirstPostDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = grdFirstPostDate.Rows[grdFirstPostDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["FirstPostDateDescription"].Value);
            string OpenDate = row.Cells["FirstPostDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "FirstPost Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtFirstPostDateDec.Text = Dis;

            if (this.grdFirstPostDate.Rows[e.RowIndex].Index >= 0)
            {
                grdFirstPostDate.Rows.RemoveAt(this.grdFirstPostDate.Rows[e.RowIndex].Index);
            }
        }

        private void grdLastModifiedPostDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = grdLastModifiedPostDate.Rows[grdLastModifiedPostDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["LastModifiedPostDateDescription"].Value);
            string OpenDate = row.Cells["LastModifiedPostDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "LastModifiedPost Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtLastModifiedPostDate_dec.Text = Dis;

            if (this.grdLastModifiedPostDate.Rows[e.RowIndex].Index >= 0)
            {
                grdLastModifiedPostDate.Rows.RemoveAt(this.grdLastModifiedPostDate.Rows[e.RowIndex].Index);
            }
        }

        private void grdExpirationDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = grdExpirationDate.Rows[grdExpirationDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["ExpirationDateDescription"].Value);
            string OpenDate = row.Cells["ExpirationDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "Expiration Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtExpirationDateDec.Text = Dis;

            if (this.grdExpirationDate.Rows[e.RowIndex].Index >= 0)
            {
                grdExpirationDate.Rows.RemoveAt(this.grdExpirationDate.Rows[e.RowIndex].Index);
            }
        }

        private void btnAddPreProposalDate_Click(object sender, EventArgs e)
        {
             string url_txtPreProposalDateDec = txtPreProposalDateDec.Text.TrimStart().TrimEnd();

            if (url_txtPreProposalDateDec.Contains("http://") || url_txtPreProposalDateDec.Contains("https://") || url_txtPreProposalDateDec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Avanish---18 June 2018----//
                    if (txtPreProposalDateDec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtPreProposalDateDec.Text.Trim(), "PreProposal Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (grdPreProposalDate.Rows.Count < 1)
                    {

                        lblMsg.Visible = false;
                        string calPreProposalDate = "";

                        if (ddlPreProposalDate.SelectedIndex == 0)
                        {
                            calPreProposalDate = txtPreProposalDate.Text;
                        }
                        else
                        {
                            calPreProposalDate = Convert.ToString(ddlPreProposalDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(grdPreProposalDate.Rows.Count + 1);
                        string EXP_DATE = string.Empty;

                        if (calPreProposalDate == "not-specified" || calPreProposalDate == "ongoing")
                        {
                            EXP_DATE = Convert.ToString(calPreProposalDate);
                        }
                        else
                        {
                            EXP_DATE = calPreProposalDate;
                        }

                        string DateRemarks = txtPreProposalDateDec.Text.Trim();
                        string lang = ddlLangPPD.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), EXP_DATE, DateRemarks, lang };
                        grdPreProposalDate.Rows.Add(rowGrid);
                        txtOpenDateDesc.Text = "";
                        txtPreProposalDateDec.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one PreProposal Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void grdPreProposalDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (grdPreProposalDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = grdPreProposalDate.Rows[grdPreProposalDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["PreProsalSequId"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.grdPreProposalDate.Rows[grdPreProposalDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    grdPreProposalDate.Rows.RemoveAt(this.grdPreProposalDate.Rows[grdPreProposalDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("PreProsalSequId");
                                dt.Columns.Add("PreProposalDate");

                                foreach (DataGridViewRow rowData in grdPreProposalDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["PreProsalSequId"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["PreProsalSequId"].Value.ToString(), rowData.Cells["PreProposalDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 7, SEQUENCE_ID, 0);
                                grdPreProposalDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvPreProposalDate_data;
                                    dvPreProposalDate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvPreProposalDate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvPreProposalDate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvPreProposalDate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvPreProposalDate_data[iContext]["Proposal_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvPreProposalDate_data[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        grdPreProposalDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(7);
                                }
                            }

                            MessageBox.Show("PreProposal Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdPreProposalDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = grdPreProposalDate.Rows[grdPreProposalDate.SelectedCells[0].RowIndex];
            string Dis = Convert.ToString(row.Cells["PreProposalDateDescription"].Value);
            string OpenDate = row.Cells["PreProposalDate"].Value.ToString();
            SharedObjects.OppStatusdisp = "PreProposal Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
            SharedObjects.OppDis = Dis;

            txtPreProposalDateDec.Text = Dis;

            if (this.grdPreProposalDate.Rows[e.RowIndex].Index >= 0)
            {
                grdPreProposalDate.Rows.RemoveAt(this.grdPreProposalDate.Rows[e.RowIndex].Index);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { }

        private void txtExpirationDateDec_TextChanged(object sender, EventArgs e) { }

        private bool valid_date(string date1)
        {
            try
            {
                DateTime date = Convert.ToDateTime(date1);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void grdDecisionDate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (grdDecisionDate.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataGridViewRow row = grdDecisionDate.Rows[grdDecisionDate.SelectedCells[0].RowIndex];
                            int SEQUENCE_ID = Convert.ToInt32(row.Cells["DecisionSequId"].Value);

                            if (SEQUENCE_ID == 0)
                            {
                                if (this.grdDecisionDate.Rows[grdDecisionDate.SelectedCells[0].RowIndex].Index >= 0)
                                {
                                    grdDecisionDate.Rows.RemoveAt(this.grdDecisionDate.Rows[grdDecisionDate.SelectedCells[0].RowIndex].Index);
                                }

                                lblMsg.Visible = true;
                                lblMsg.Text = "";
                            }

                            DataSet dsItems = new DataSet();

                            if (SEQUENCE_ID > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("DecisionSequId");
                                dt.Columns.Add("DecisionDate");

                                foreach (DataGridViewRow rowData in grdDecisionDate.Rows)
                                {
                                    if (Convert.ToInt32(rowData.Cells["DecisionSequId"].Value) == 0)
                                    {
                                        dt.Rows.Add(rowData.Cells["DecisionSequId"].Value.ToString(), rowData.Cells["DecisionDate"].Value.ToString());
                                    }
                                }

                                dsItems = OpportunityDataOperations.Delete_DateList(SharedObjects.WorkId, 8, SEQUENCE_ID, 0);
                                grdDecisionDate.Rows.Clear();

                                if (dsItems.Tables["DateList"].Rows.Count > 0)
                                {
                                    DateTime dtaTime = new DateTime();
                                    DataView dvDecisionDate_data;
                                    dvDecisionDate_data = new DataView(dsItems.Tables["DateList"].Copy());
                                    dvDecisionDate_data.Sort = "SEQUENCE_ID ASC";

                                    for (int iContext = 0; iContext < dvDecisionDate_data.Count; iContext++)
                                    {
                                        string firstCol = Convert.ToString(dvDecisionDate_data[iContext]["SEQUENCE_ID"]);
                                        dtaTime = Convert.ToDateTime(dvDecisionDate_data[iContext]["Decision_DATE"].ToString());
                                        string secondCol = dtaTime.ToShortDateString();
                                        string thrdCol = Convert.ToString(dvDecisionDate_data[iContext]["DATE_REMARKS"]);

                                        if (secondCol == "01-01-1900")
                                        {
                                            secondCol = "not-specified";
                                        }
                                        else if (secondCol == "01-02-1900")
                                        {
                                            secondCol = "ongoing";
                                        }

                                        string[] row21 = { firstCol, firstCol, secondCol, thrdCol };
                                        grdDecisionDate.Rows.Add(row21);
                                    }
                                }
                                else
                                {
                                    norecord(8);
                                }
                            }

                            MessageBox.Show("PreProposal Date deleted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdDecisionDate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = grdDecisionDate.Rows[grdDecisionDate.SelectedCells[0].RowIndex];
                string Dis = Convert.ToString(row.Cells["DecisionDateDescription"].Value);
                string OpenDate = row.Cells["DecisionDate"].Value.ToString();
                SharedObjects.OppStatusdisp = "Decision Date : " + OpenDate + "   [ Opportunity Status : " + OppStatus + " ]";
                SharedObjects.OppDis = Dis;

                txtDecisionDateDec.Text = Dis;

                if (this.grdDecisionDate.Rows[e.RowIndex].Index >= 0)
                {
                    grdDecisionDate.Rows.RemoveAt(this.grdDecisionDate.Rows[e.RowIndex].Index);
                }
            }
            catch { }
        }

        private void btnAddDecisionDate_Click(object sender, EventArgs e)
        {
            string url_txtDecisionDateDec = txtDecisionDateDec.Text.TrimStart().TrimEnd();

            if (url_txtDecisionDateDec.Contains("http://") || url_txtDecisionDateDec.Contains("https://") || url_txtDecisionDateDec.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    #region //----------validation html tags---------Avanish---18 June 2018----//
                    if (txtDecisionDateDec.Text != "")
                    {
                        string _result = oErrorLog.htlmtag(txtDecisionDateDec.Text.Trim(), "Decision Date Description");
                        if (!_result.Equals(""))
                        {
                            MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    #endregion

                    if (grdDecisionDate.Rows.Count < 1)
                    {
                        lblMsg.Visible = false;
                        string calDecisionDate = "";

                        if (ddlDecisionDate.SelectedIndex == 0)
                        {
                            calDecisionDate = txtDecisionDate.Text;
                        }
                        else
                        {
                            calDecisionDate = Convert.ToString(ddlDecisionDate.SelectedItem);
                        }

                        int SEQUENCE_ID = 0;
                        int sl = Convert.ToInt32(grdDecisionDate.Rows.Count + 1);
                        string EXP_DATE = string.Empty;

                        if (calDecisionDate == "not-specified" || calDecisionDate == "ongoing")
                        {
                            EXP_DATE = Convert.ToString(calDecisionDate);
                        }
                        else
                        {
                            EXP_DATE = calDecisionDate;
                        }

                        string DateRemarks = txtDecisionDateDec.Text.Trim();
                        string lang = ddlLangDD.SelectedValue.ToString();
                        string[] rowGrid = { Convert.ToString(SEQUENCE_ID), Convert.ToString(sl), EXP_DATE, DateRemarks, lang };
                        grdDecisionDate.Rows.Add(rowGrid);
                        txtOpenDateDesc.Text = "";
                        txtDecisionDateDec.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("You can add only one Decision Date", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }
    }
}
