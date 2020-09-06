using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySqlDal;


namespace Scival.Award
{
    public partial class PageURL : UserControl
    {

        private Awards m_parent;
        string pageURL = string.Empty;
        string pageName = string.Empty;
        Int64 WorkFloeId = 0; Int64 UserId = 0;
        ErrorLog oErrorLog = new ErrorLog();
        List<PageUrl> pageurlst = new List<PageUrl>();
        public PageURL(Awards frm)
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
                    pageurlst = AwardDataOperations.AddAndDeletePageURL(SharedObjects.WorkId, SharedObjects.ClickPage, m_parent.GetURL(), SharedObjects.User.USERID, 0);
                else
                    pageurlst = AwardDataOperations.GetURL(SharedObjects.WorkId, SharedObjects.ClickPage);

                if (pageurlst.Count > 0)
                    grdPageURL.DataSource = pageurlst;
                else
                    NoRecord();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        public void NoRecord()
        {
            if (pageurlst.Count == 0)
            {
                pageurlst[0].Url = "No Record(s) found.";
                grdPageURL.DataSource = pageurlst;
            }
        }
        private void grdPageURL_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (pageurlst.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pageurlst = AwardDataOperations.AddAndDeletePageURL(SharedObjects.WorkId, SharedObjects.ClickPage, pageurlst[0].Url, SharedObjects.User.USERID, 1);
                            if (pageurlst.Count > 0)
                                grdPageURL.DataSource = pageurlst;
                            else
                                NoRecord();
                            MessageBox.Show("Something Went wrong", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (pageurlst.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        try
                        {
                            string URL = pageurlst[e.RowIndex].Url;
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
