using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.WebWatcher
{
    public partial class GroupDetail : BaseForm
    {
        Int64 mId, mFundingId, mModuleId, mBatch = 0;

        ErrorLog errorLog = new ErrorLog();
        Grouping mGrouping = null;

        Hashtable htLeftURL = new Hashtable();
        Hashtable htRightURL = new Hashtable();

        List<UrlGroupDetail> rightUrlList;
        List<UrlDetailAndCount> leftUrlList;

        public GroupDetail(Grouping grouping, Int64 id, Int64 fundingId, Int64 moduleId, Int64 batch)
        {
            try
            {
                InitializeComponent();

                mGrouping = grouping;
                mId = id;
                mFundingId = fundingId;
                mModuleId = moduleId;
                mBatch = batch;

                LoadInitialValue();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void LoadInitialValue()
        {
            try
            {
                lstLeft.Items.Clear();
                lstrighjt.Items.Clear();
                htLeftURL.Clear();
                htRightURL.Clear();

                rightUrlList = WebWatcherDataOperation.GetUrlDetail(mFundingId, mId, mModuleId, mBatch);
                leftUrlList = WebWatcherDataOperation.GetUrlDetailAndCount();

                foreach (UrlDetailAndCount url in leftUrlList)
                {
                    lstLeft.Items.Add(url.Url);
                    htLeftURL.Add(url.Url, url.UrlId);
                }

                foreach (UrlGroupDetail url in rightUrlList)
                {
                    lstrighjt.Items.Add(url.Url);
                    htRightURL.Add(url.Url, url.UrlNumber);
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstLeft_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            Brush myBrush;

            Int64 index = leftUrlList[e.Index].Count.Value;

            // Determine the color of the brush to draw each item based on the index of the item to draw.
            switch (index)
            {
                case 0:
                    myBrush = Brushes.Black;
                    break;
                default:
                    myBrush = Brushes.Black;
                    e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
                    break;
            }

            // Draw the current item text based on the current Font and the custom brush settings.
            e.Graphics.DrawString("( " + index + " ) " + lstLeft.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Bold), myBrush, 
                new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void lstLeft_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < lstLeft.SelectedItems.Count; i++)
                {
                    string leftURL = Convert.ToString(lstLeft.SelectedItems[i]);
                    int lngt = leftURL.IndexOf(")");
                    string rgtURL = leftURL;

                    if (lngt > 0)
                        rgtURL = leftURL.Substring(lngt);

                    if (!lstrighjt.Items.Contains(leftURL))
                        lstrighjt.Items.Add(Convert.ToString(rgtURL));
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void lstrighjt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstrighjt.Items.Count == 1)
                {
                    MessageBox.Show("You can not delete last URL of Group.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    for (int i = 0; i < lstrighjt.SelectedItems.Count; i++)
                        lstrighjt.Items.Remove(Convert.ToString(lstrighjt.SelectedItems[i]));
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void GroupDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstrighjt.Items.Count > 0)
                {
                    string URLId = string.Empty; string totalStr = string.Empty; string sep = string.Empty;

                    List<String> urlIds = new List<string>();

                    for (int i = 0; i < lstrighjt.Items.Count; i++)
                    {
                        URLId = Convert.ToString(htLeftURL[lstrighjt.Items[i]]);
                        urlIds.Add(URLId);
                    }

                    leftUrlList = WebWatcherDataOperation.UnGrouping(mId, mFundingId, urlIds, SharedObjects.User.USERID, mModuleId, mBatch);

                    mGrouping.RefreshLeftList(leftUrlList);

                    MessageBox.Show("Updated Successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
