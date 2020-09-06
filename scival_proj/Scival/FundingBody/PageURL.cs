using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class PageURL : UserControl
    {
        DataSet dsPageURL = null;

        private FundingBody m_parent;
        string pageURL = string.Empty;
        string pageName = string.Empty;
        ErrorLog oErrorLog = new ErrorLog();
        
        public PageURL(FundingBody frm)
        {
            m_parent = frm;
            InitializeComponent();
            loadInitialValue();
        }

        private void loadInitialValue()
        {
            try
            {
                if (SharedObjects.DefaultLoad != "")
                    dsPageURL = CommonDataOperation.AddAndDeletePageURL(SharedObjects.WorkId, SharedObjects.ClickPage, m_parent.GetURL(), SharedObjects.User.USERID, 0);
                else
                    dsPageURL = CommonDataOperation.GetURL(SharedObjects.WorkId, SharedObjects.ClickPage);

                if (dsPageURL.Tables["URL"].Rows.Count > 0)
                {
                    grdPageURL.DataSource = dsPageURL.Tables["URL"];
                }
                else
                {
                    NoRecord();
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        public void NoRecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("URL");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";
                dtNoRcrd.Rows.Add(dr);
                grdPageURL.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdPageURL_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (dsPageURL.Tables["URL"].Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dsPageURL = CommonDataOperation.AddAndDeletePageURL(SharedObjects.WorkId, SharedObjects.ClickPage, Convert.ToString(dsPageURL.Tables["URL"].Rows[grdPageURL.SelectedCells[0].RowIndex]["URL"]), Convert.ToInt64(SharedObjects.User.USERID), 1);

                            if (dsPageURL.Tables["URL"].Rows.Count > 0)
                            {
                                grdPageURL.DataSource = dsPageURL.Tables["URL"];
                            }
                            else
                            {
                                NoRecord();
                            }
                            MessageBox.Show("", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdPageURL_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dsPageURL.Tables["URL"].Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        int rowindex = e.RowIndex;

                        try
                        {
                            string URL = Convert.ToString(dsPageURL.Tables["URL"].Rows[rowindex]["URL"]);

                            if (!URL.Contains("No Record"))
                            {
                                m_parent.SetGridURL(URL);
                            }
                        }
                        catch (Exception ex)
                        {
                            oErrorLog.WriteErrorLog(ex);
                        }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for Navigate URL.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
    }
}
