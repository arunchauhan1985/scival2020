using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySqlDal;

namespace Scival
{
    public partial class DashBoard : BaseForm
    {
        readonly ErrorLog errorLog = new ErrorLog();
        readonly Replace replace = new Replace();

        string InputXmlPath = string.Empty;

        List<DashboardUserFunding> dashboardUserFundings;
        List<DashboardTask> dashboardTasks;
        List<DashboardRemark> dashboardRemarks;

        public DashBoard()
        {
            InitializeComponent();
            SetWorkflow();
            LoadModeImage();

            try
            {
                String path = Application.StartupPath;
                btnback.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
                btnstart.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            TaskDataOperation.BackToTaskBoard(SharedObjects.User.USERID, SharedObjects.ID, SharedObjects.ModuleId, SharedObjects.TaskId, SharedObjects.Cycle);

            SharedObjects.TaskBoard = null;

            TaskBoard taskBoard = new TaskBoard();
            taskBoard.Show();
            this.Dispose();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                string FundingBodyName = string.Empty;
                InputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

                btnstart.Location = new Point(this.Width - (btnstart.Width + 50), this.Height - (btnstart.Height + 120));
                btnstart.Visible = true;

                btnback.Location = new Point(btnstart.Left - 100, this.Height - (btnstart.Height + 120));

                groupBox3.Top = (this.Height - groupBox3.Height) / 2;
                groupBox3.Left = (this.Width - groupBox3.Width) / 2;
                groupBox3.Top -= 50;

                imgMode.Top = groupBox3.Top - 9;
                imgMode.Left = groupBox3.Left + 10;

                FOAname.Text = replace.ReadandReplaceHexaToChar(SharedObjects.FundingBodyName.ToString(), InputXmlPath);

                string url;
                Int64 workflowId = 0;

                if (SharedObjects.ModuleId == 2)
                {
                    dashboardUserFundings = FundingBodyDataOperations.GetDashBoardDetails(SharedObjects.User.USERID, SharedObjects.User.USERNAME, SharedObjects.ID, SharedObjects.ModuleId, SharedObjects.TaskId, SharedObjects.Cycle);
                    dashboardTasks = FundingBodyDataOperations.GetDashBoardDetailsTaskList();
                    dashboardRemarks = FundingBodyDataOperations.GetDashBoardDetailsRemarkList();
                }
                else if (SharedObjects.ModuleId == 3)
                {
                    if (SharedObjects.TaskId != 7)
                    {
                        if (SharedObjects.ID == 99999999)
                            SharedObjects.ID = OpportunityDataOperations.GetOpportunityWorkflowId(Convert.ToInt64(SharedObjects.OPFBID), SharedObjects.User.USERID);

                        dashboardUserFundings = OpportunityDataOperations.GetDashBoardDetails(SharedObjects.User.USERID, SharedObjects.User.USERNAME, SharedObjects.ID);
                        url = OpportunityDataOperations.GetDashBoardDetailsUrl();
                        dashboardTasks = OpportunityDataOperations.GetDashBoardDetailsTask();
                        dashboardRemarks = OpportunityDataOperations.GetDashBoardDetailsRemarks();
                    }
                    else
                    {
                        dashboardUserFundings = OpportunityDataOperations.GetExpiryDashBoardDetail(SharedObjects.ID, SharedObjects.User.USERID, SharedObjects.User.USERNAME);
                        url = OpportunityDataOperations.GetDashBoardDetailsUrl();
                        dashboardTasks = OpportunityDataOperations.GetDashBoardDetailsTask();
                        dashboardRemarks = OpportunityDataOperations.GetDashBoardDetailsRemarks();
                        workflowId = OpportunityDataOperations.GetDashBoardDetailsWorkflowId();
                    }
                }
                else if (SharedObjects.ModuleId == 4)
                {
                    if (SharedObjects.ID == 99999999)
                    {
                        if (SharedObjects.ID == 99999999)
                            SharedObjects.ID = AwardDataOperations.GetAwardWorkflowId(Convert.ToInt64(SharedObjects.OPFBID), SharedObjects.User.USERID);

                        dashboardUserFundings = AwardDataOperations.GetDashBoardDetails(SharedObjects.User.USERID, SharedObjects.User.USERNAME, SharedObjects.ID);

                        url = AwardDataOperations.GetDashBoardDetailsUrl();
                        dashboardTasks = AwardDataOperations.GetDashBoardDetailsDashboardTask();
                        dashboardRemarks = AwardDataOperations.GetDashBoardDetailsDashboardRemark();
                    }
                }

                string TaskFlow = string.Empty;
                string sep = string.Empty;

                foreach (DashboardTask task in dashboardTasks)
                {
                    TaskFlow += sep + task.TaskName;
                    sep = ":";
                }

                SharedObjects.TaskFlow = TaskFlow;

                if (SharedObjects.ModuleId == 2)
                    SharedObjects.WorkId = dashboardTasks.Where(dt => dt.TaskId == SharedObjects.TaskId).Select(dt => dt.WorkFlowId).FirstOrDefault();
                else if (SharedObjects.ModuleId == 3 && SharedObjects.TaskId == 7)
                    SharedObjects.WorkId = workflowId;
                else
                    SharedObjects.WorkId = SharedObjects.ID;

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dashboardUserFundings;
                dataGridView1.Columns[1].HeaderText = "Details " + "(Due Date:- " + SharedObjects.DueDate + ")";
                dataGridView1.Rows[0].Cells[0].Selected = false;

                DesginWorkflow();

                if (dashboardRemarks.Count() > 0)
                {
                    StringBuilder SB = new StringBuilder();

                    foreach (DashboardRemark remark in dashboardRemarks)
                    {
                        lstComment.Items.Add(remark.Remark);
                        lstComment.Items.Add("BY " + remark.UserName + " on " + remark.CreatedDate1);
                    }
                }
                else
                {
                    lstComment.Items.Add("No comments recieved.");
                    lstComment.Items.Add("By Scival CMS.");
                }
            }
            catch (ScivalDataException ex)
            {
                SharedObjects.Message = ex.Message;
                this.Dispose();
                TaskBoard TB = new TaskBoard();
                TB.Show();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesginWorkflow()
        {
            DataGridViewImageCell DGImgC;
            Image image;
            Image Resizeimage;
            Bitmap bitmap;
            string appPath;
            int index = 0;

            foreach (DashboardUserFunding userFunding in dashboardUserFundings)
            {
                if (userFunding.Count == 0)
                {
                    appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Images\\cross.png";
                    image = Image.FromFile(appPath);
                    bitmap = new Bitmap(image);
                    Resizeimage = new Bitmap(bitmap, 10, 10 * bitmap.Height / bitmap.Width);
                    DGImgC = (DataGridViewImageCell)dataGridView1.Rows[index].Cells[0];
                    DGImgC.Value = Resizeimage;
                }
                else
                {
                    appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Images\\checkmark.png";
                    image = Image.FromFile(appPath);
                    bitmap = new Bitmap(image);
                    Resizeimage = new Bitmap(bitmap, 10, 10 * bitmap.Height / bitmap.Width);
                    DGImgC = (DataGridViewImageCell)dataGridView1.Rows[index].Cells[0];
                    DGImgC.Value = Resizeimage;
                }

                index++;
            }
        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            if (SharedObjects.ModuleId == 2)
            {
                FundingBody.FundingBody fundingBody = new FundingBody.FundingBody();
                fundingBody.Show();
            }
            else if (SharedObjects.ModuleId == 3)
            {
                Opportunity.Opportunity opportunity = new Opportunity.Opportunity();
                opportunity.Show();
            }
            else if (SharedObjects.ModuleId == 4)
            {
                Award.Awards award = new Scival.Award.Awards();
                award.Show();
            }

            this.Dispose();
        }

        private void SetWorkflow()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);

                if (SharedObjects.TaskId == 1)
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\flow_new_1.png");
                else if (SharedObjects.TaskId == 2)
                {
                    if (SharedObjects.Cycle == 0)
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\flow_new_1.png");
                    else if (SharedObjects.Cycle > 0)
                        imgFlow.Image = Image.FromFile(appPath + "\\Images\\flow_new_2.png");
                }
                else if (SharedObjects.TaskId == 3)
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\XML-Delivery.png");
                else if (SharedObjects.TaskId == 7)
                    imgFlow.Image = Image.FromFile(appPath + "\\Images\\flow_new_3.png");
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void DashBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lstComment_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            // Define the default color of the brush as black.
            Brush myBrush = Brushes.Black;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.
            switch (e.Index % 2)
            {
                case 0:
                    myBrush = Brushes.White;
                    break;
                default:
                    myBrush = Brushes.Orange;
                    break;
            }

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            e.Graphics.DrawString(lstComment.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void LoadModeImage()
        {
            try
            {
                Image imgObj = null;
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);

                if (SharedObjects.ModuleId == 2) // Funding Body
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\FB_New.png");
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle > 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\FB_Update.png");
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\FB_QA.png");
                }
                else if (SharedObjects.ModuleId == 3) // Opportunity
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\OPP_New.png");
                    else if (SharedObjects.TaskId == 1 && SharedObjects.Cycle > 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\OPP_Update.png");
                    else if (SharedObjects.TaskId == 7)
                        imgObj = Image.FromFile(appPath + "\\Images\\OPP_Expiry.png");
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\OPP_QA.png");
                }
                else if (SharedObjects.ModuleId == 4) // Award
                {
                    if (SharedObjects.TaskId == 1 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\AW_New.png");
                    else if (SharedObjects.TaskId == 1 && SharedObjects.Cycle > 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\AW_Update.png");
                    else if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                        imgObj = Image.FromFile(appPath + "\\Images\\AW_QA.png");
                }

                if (imgObj != null)
                {
                    imgMode.Width = imgObj.Width;
                    imgMode.Height = imgObj.Height;
                    imgMode.Image = imgObj;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
