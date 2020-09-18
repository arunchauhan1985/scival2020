using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySqlDal;

namespace Scival
{
    public partial class TaskBoard : BaseForm
    {
        ErrorLog errorLog = new ErrorLog();
        Replace replace = new Replace();

        List<UserTaskList> usersTasks;
        List<UserFunding> userFundings;
        List<DummyTaskList> dummyTaskList;
        List<ExpiryDetail> expireDetailList;
        List<NewTaskList> newTaskLists;
        List<OpportunityList> opportunityLists;
        List<AwardList> awardLists;

        ToolBar toolBar;
        ToolBarButton toolBarButtonNew;
        ToolBarButton toolBarButtonUpdate;
        ToolBarButton toolBarButtonExpiry;
        ToolBarButton toolBarButtonQA;
        ToolBarButton toolBarButtonDelivery;

        String selectedTask = String.Empty;
        String inputXmlPath = string.Empty;

        public int indexFB = 0;
        public int indexOP = 1;
        public int indexAW = 2;
        public int indexNew = 0;
        public int indexUpdate = 1;
        public int indexExpiry = 2;
        public int indexQA = 3;
        public int indexXMLDelivery = 4;
        public int currentBindedListCount = 0;

        bool flagTaskAllocation = true;

        bool AWFB_searchflag = false;
        bool AW_searchflag = false;
        bool OP_searchflag = false;
        bool Fbody_searchflag = false;
        bool CheckTAB = true;

        DataTable DTAWFBSearch = null;
        DataTable DTFbodySearch = null;

        public TaskBoard()
        {
            InitializeComponent();
        }

        private void TaskBoard_Load(object sender, EventArgs e)
        {
            //indexFB = 3;
            //ScivalTabControl.TabPages[0].ImageIndex = indexFB;
            //indexFB = 0;

            SetTabPageIndex(2, 2, 3, 1, 2);

            int pnlwidth = (ClientRectangle.Width - PnlInstructFB.Width) / 2;
            PnlInstructFB.Left = pnlwidth;
            PnlInstructFB.Visible = true;
            pnlwidth = (PnllblInstrucFB.Width - lblInstructFB.Width) / 2;
            lblInstructFB.Left = pnlwidth;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            DashBoardBT.Location = new Point(ClientRectangle.Width - (DashBoardBT.Width + 20), ClientRectangle.Height - (DashBoardBT.Height + 100));
            DashBoardBT.Visible = true;

            btnoppDash.Location = new Point(ClientRectangle.Width - (btnoppDash.Width + 20), ClientRectangle.Height - (btnoppDash.Height + 100));
            btnoppDash.Visible = true;

            btnAwrdDash.Location = new Point(ClientRectangle.Width - (btnAwrdDash.Width + 20), ClientRectangle.Height - (btnAwrdDash.Height + 100));
            btnAwrdDash.Visible = true;

            try
            {
                String path = Application.StartupPath;
                DashBoardBT.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");

                pnlwidth = (ClientRectangle.Width - PnlInstructFB.Width) / 2;
                //PnlInstructFB.Left = pnlwidth;
                PnlInstructFB.Visible = true;
                pnlwidth = (PnllblInstrucFB.Width - lblInstructFB.Width) / 2;
                lblInstructFB.Left = pnlwidth;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }

            usersTasks = null; //TaskDataOperation.GetTaskForUser(SharedObjects.User.USERID);

            if (usersTasks != null && usersTasks.Count > 0)
            {
                SharedObjects.ModuleId = usersTasks[0].MODULEID;
                SharedObjects.TaskId = usersTasks[0].TASKID;

                if (SharedObjects.ModuleId == 3 && SharedObjects.TaskId == 7)
                    SharedObjects.ID = usersTasks[0].ID;
                else if (SharedObjects.ModuleId == 3 || SharedObjects.ModuleId == 4)
                    SharedObjects.ID = usersTasks[0].WORKFLOWID;
                else
                    SharedObjects.ID = usersTasks[0].ID;

                SharedObjects.WorkId = usersTasks[0].WORKFLOWID;
                SharedObjects.Cycle = usersTasks[0].CYCLE;
                SharedObjects.FundingBodyName = usersTasks[0].FUNDINGBODYNAME;

                DashBoard dashBoard = new DashBoard();
                dashBoard.Show();
                this.Dispose();
            }
            else if (TaskDataOperation.GetModuleWiseUserTasksCount(SharedObjects.User.USERID, SharedObjects.User.NAME) > 0)
            {
                if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 2) > 0)
                {
                    indexFB = 0;

                    if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 3) > 0)
                    {
                        indexOP = 1;

                        if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 4) > 0)
                            indexAW = 2;
                        else
                            ScivalTabControl.TabPages.Remove(TabAW);
                    }
                    else
                    {
                        ScivalTabControl.TabPages.Remove(TabOP);

                        if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 4) > 0)
                            indexAW -= 1;
                        else
                            ScivalTabControl.TabPages.Remove(TabAW);
                    }
                }
                else
                {
                    if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 3) > 0)
                    {
                        indexOP -= 1;

                        if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 4) > 0)
                            indexAW -= 1;
                        else
                            ScivalTabControl.TabPages.Remove(TabAW);
                    }
                    else
                    {
                        if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(SharedObjects.User.USERID, SharedObjects.User.NAME, 4) > 0)
                            indexAW -= 2;
                        else
                            ScivalTabControl.TabPages.Remove(TabAW);

                        ScivalTabControl.TabPages.Remove(TabOP);
                    }

                    ScivalTabControl.TabPages.Remove(TabFB);
                }

                var ModuleId = TaskDataOperation.GetUserModuleId(SharedObjects.User.USERID);

                if (ModuleId > 0)
                {
                    if (ModuleId == 2)
                    {
                        if (ScivalTabControl.TabPages.Contains(TabFB))
                        {
                            SetTabPageIndex(2, 2, 3, 1, 2);

                            ((Control)this.TabFB).Enabled = true;
                            ((Control)this.TabOP).Enabled = false;
                            ((Control)this.TabAW).Enabled = false;

                            this.ScivalTabControl.SelectedIndex = 0;
                        }
                    }
                    if (ModuleId == 3)
                    {
                        if (ScivalTabControl.TabPages.Contains(TabOP))
                        {
                            panelOpportunity.Visible = false;
                            DashBoardBT.Visible = true;

                            SetTabPageIndex(3, 3, 0, 4, 2);

                            ((Control)this.TabFB).Enabled = false;
                            ((Control)this.TabOP).Enabled = true;
                            ((Control)this.TabAW).Enabled = false;

                            this.ScivalTabControl.SelectedIndex = 1;
                        }
                    }
                    if (ModuleId == 4)
                    {
                        if (ScivalTabControl.TabPages.Contains(TabAW))
                        {
                            SetTabPageIndex(4, 4, 0, 1, 5);

                            ((Control)this.TabFB).Enabled = false;
                            ((Control)this.TabOP).Enabled = false;
                            ((Control)this.TabAW).Enabled = true;

                            this.ScivalTabControl.SelectedIndex = 2;
                        }
                    }
                }
                else
                {
                    if (ScivalTabControl.TabPages.Contains(TabFB))
                    {
                        SetTabPageIndex(2, 2, 3, 1, 2);
                    }
                }
            }

            if (!String.IsNullOrEmpty(SharedObjects.Message))
            {
                MessageBox.Show(SharedObjects.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SharedObjects.Message = String.Empty;
            }
            if (SharedObjects.ExpireAlertCount > 0)
            {
                MessageBox.Show(SharedObjects.ExpireAlertCount.ToString() + " Opportunity(s) Expired.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SharedObjects.ExpireAlertCount = 0;
            }
        }

        private void SetTabPageIndex(int taskMode, int ModuleId, int indexFB, int indexOP, int indexAW)
        {
            SetTaskMode(taskMode);
            ResetBefore();

            SharedObjects.ModuleId = ModuleId;

            PanelFBTaskList.Visible = false;
            //int CurrentIndex = ScivalTabControl.SelectedIndex;
            //if(CurrentIndex==0)
            //{
            //    indexFB = 3;
            //}
            if (ScivalTabControl.TabPages.Contains(TabFB)) // Demo && indexFB!=3
                ScivalTabControl.TabPages[0].ImageIndex = indexFB;
            if (ScivalTabControl.TabPages.Contains(TabOP))
                ScivalTabControl.TabPages[1].ImageIndex = indexOP;
            if (ScivalTabControl.TabPages.Contains(TabAW))
                ScivalTabControl.TabPages[2].ImageIndex = indexAW;
        }

        private void ResetBefore()
        {
            if (toolBar.Buttons.Contains(toolBarButtonNew))
                toolBar.Buttons[indexNew].ImageIndex = 0;
            if (toolBar.Buttons.Contains(toolBarButtonUpdate))
                toolBar.Buttons[indexUpdate].ImageIndex = 1;
            if (toolBar.Buttons.Contains(toolBarButtonExpiry))
                toolBar.Buttons[indexExpiry].ImageIndex = 2;
            if (toolBar.Buttons.Contains(toolBarButtonQA))
                toolBar.Buttons[indexQA].ImageIndex = 3;
            if (toolBar.Buttons.Contains(toolBarButtonDelivery) && indexXMLDelivery != 3) // Demo && indexXMLDelivery!=3
                toolBar.Buttons[indexXMLDelivery].ImageIndex = 4;
        }

        private void Reset()
        {
            PanelFBTaskList.Visible = false;
            RBauto.Checked = false;
            RBmanual.Checked = false;
            flagTaskAllocation = false;
            dataGridView1.Controls.Clear();
        }

        private void SetTaskMode(int ModuleId)
        {
            int Width = 0;

            indexNew = 0;
            indexUpdate = 1;
            indexExpiry = 2;
            indexQA = 3;
            indexXMLDelivery = 4;

            toolBar = new ToolBar
            {
                Appearance = ToolBarAppearance.Flat,
                BorderStyle = BorderStyle.None,
                ImageList = TaskModelist
            };

            toolBar.MouseMove += new MouseEventHandler(toolbar_MouseMove);
            toolBar.ButtonClick += new ToolBarButtonClickEventHandler(toolbar_ButtonClick);

            toolBarButtonNew = new ToolBarButton { Name = "New", Style = ToolBarButtonStyle.PushButton };
            toolBarButtonUpdate = new ToolBarButton { Name = "Update" };
            toolBarButtonExpiry = new ToolBarButton { Name = "Expiry" };
            toolBarButtonQA = new ToolBarButton { Name = "QA" };
            toolBarButtonDelivery = new ToolBarButton { Name = "Delivery" };

            Boolean bButtonNew = false;
            Boolean bButtonExpiry = false;
            Boolean bButtonQa = false;

            PanelFB.Controls.Clear();
            PanelOP.Controls.Clear();
            PanelAW.Controls.Clear();

            if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleIdAndTaskId(SharedObjects.User.USERID, SharedObjects.User.NAME, ModuleId, 1) > 0)
            {
                toolBarButtonNew.ImageIndex = 0;
                toolBar.Buttons.Add(toolBarButtonNew);
                Width += toolBar.Buttons[0].Rectangle.Width;

                toolBarButtonUpdate.ImageIndex = 1;
                toolBar.Buttons.Add(toolBarButtonUpdate);
                Width += toolBar.Buttons[0].Rectangle.Width;

                bButtonNew = true;
            }

            if (!bButtonNew)
                indexExpiry -= 2;

            if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleIdAndTaskId(SharedObjects.User.USERID, SharedObjects.User.NAME, ModuleId, 7) > 0)
            {
                toolBarButtonExpiry.ImageIndex = 2;
                toolBar.Buttons.Add(toolBarButtonExpiry);
                Width += toolBar.Buttons[0].Rectangle.Width;

                bButtonExpiry = true;
            }

            if (!bButtonExpiry)
            {
                if (!bButtonNew)
                    indexQA -= 3;
                else
                    indexQA -= 1;
            }
            else
            {
                if (!bButtonNew)
                    indexQA -= 2;
            }

            if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleIdAndTaskId(SharedObjects.User.USERID, SharedObjects.User.NAME, ModuleId, 2) > 0)
            {
                toolBarButtonQA.ImageIndex = 3;
                toolBar.Buttons.Add(toolBarButtonQA);
                Width += toolBar.Buttons[0].Rectangle.Width;

                bButtonQa = true;
            }

            if (!bButtonQa)
            {
                if (!bButtonExpiry)
                {
                    if (!bButtonNew)
                        indexXMLDelivery -= 2;
                    else
                        indexXMLDelivery -= 1;
                }
                else
                    indexXMLDelivery -= 1;
            }
            else
            {
                if (!bButtonNew)
                {
                    if (!bButtonExpiry)
                        indexXMLDelivery -= 3;
                    else
                        indexXMLDelivery -= 2;
                }
                else
                {
                    if (!bButtonExpiry)
                        indexXMLDelivery -= 1;
                }
            }

            if (TaskDataOperation.GetModuleWiseUserTasksCountByModuleIdAndTaskId(SharedObjects.User.USERID, SharedObjects.User.NAME, ModuleId, 3) > 0)
            {
                toolBarButtonDelivery.ImageIndex = 4;
                toolBar.Buttons.Add(toolBarButtonDelivery);
                Width += toolBar.Buttons[0].Rectangle.Width;
            }

            toolBar.Width = Width;
            Width = (ClientRectangle.Width - Width) / 2;
            toolBar.Left = Width;

            if (ModuleId == 2)
            {
                Width = 0;

                for (int x = 0; x < toolBar.Buttons.Count; x++)
                    Width += toolBar.Buttons[0].Rectangle.Width;

                PanelFB.Width = Width;
                Width = (ClientRectangle.Width - Width) / 2;
                PanelFB.Left = Width;
                PanelTaskMessage.Left = (ClientRectangle.Width - PanelTaskMessage.Width) / 2;
                PanelFB.Controls.Add(toolBar);
                PanelTaskMessage.Left = (this.Width - PanelTaskMessage.Width) / 2;
            }
            else if (ModuleId == 3)
            {
                Width = 0;

                for (int x = 0; x < toolBar.Buttons.Count; x++)
                    Width += toolBar.Buttons[0].Rectangle.Width;

                PanelOP.Width = Width;
                Width = (ClientRectangle.Width - Width) / 2;
                PanelOP.Left = Width;
                panel1.Left = (ClientRectangle.Width - panel1.Width) / 2;
                PanelOP.Controls.Add(toolBar);
            }
            else if (ModuleId == 4)
            {
                Width = 0;

                for (int x = 0; x < toolBar.Buttons.Count; x++)
                    Width += toolBar.Buttons[0].Rectangle.Width;

                PanelAW.Width = Width;
                Width = (ClientRectangle.Width - Width) / 2;
                PanelAW.Left = Width;
                pnlsearchAW.Left = (ClientRectangle.Width - pnlsearchAW.Width) / 2;
                PanelAW.Controls.Add(toolBar);
            }
        }

        void toolbar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            CheckTAB = false;

            int toolbarButtonIndex = toolBar.Buttons.IndexOf(e.Button);
            int buttonIndex = Getbutton(toolbarButtonIndex);

            toolbarButtonIndex += 1;

            if (SharedObjects.ModuleId == 3)
            {
                switch (toolBar.Buttons.IndexOf(e.Button))
                {
                    case 0:
                        SharedObjects.Cycle = 0;
                        break;
                    case 1:
                        SharedObjects.Cycle = 1;
                        break;
                    case 2:
                    case 3:
                    case 4:
                        SharedObjects.Cycle = 2;
                        break;
                }

                Toggel(buttonIndex);
                SharedObjects.TaskId = toolbarButtonIndex;
                selectedTask = e.Button.Name;
                BindOpTaskList();
            }
            else if (SharedObjects.ModuleId == 4)
            {
                switch (toolBar.Buttons.IndexOf(e.Button))
                {
                    case 0:
                        SharedObjects.Cycle = 0;
                        SharedObjects.TRAN_TYPE_ID = 0;
                        break;
                    case 1:
                        SharedObjects.Cycle = 1;
                        SharedObjects.TRAN_TYPE_ID = 1;
                        break;
                    case 2:
                        SharedObjects.Cycle = 2;
                        SharedObjects.TRAN_TYPE_ID = 2;
                        break;
                    case 3:
                    case 4:
                        SharedObjects.Cycle = 2;
                        break;
                }

                Toggel(buttonIndex);
                SharedObjects.TaskId = toolbarButtonIndex;
                selectedTask = e.Button.Name;
                BindAWTaskList();
            }
            else
            {
                switch (toolBar.Buttons.IndexOf(e.Button))
                {
                    case 0:
                        SharedObjects.Cycle = 0;
                        SharedObjects.TRAN_TYPE_ID = 0;
                        break;
                    case 1:
                        SharedObjects.Cycle = 1;
                        SharedObjects.TRAN_TYPE_ID = 1;
                        break;
                    case 2:
                        SharedObjects.Cycle = 2;
                        SharedObjects.TRAN_TYPE_ID = 2;
                        break;
                    case 3:
                    case 4:
                        SharedObjects.Cycle = 2;
                        break;
                }

                Toggel(buttonIndex);
                SharedObjects.TaskId = toolbarButtonIndex;
                selectedTask = e.Button.Name;
                PnlInstructFB.Visible = true;

                if (e.Button.Name == "Delivery")
                    Bind();
            }
        }

        private int Getbutton(int butoonIndex)
        {
            if (butoonIndex < 2)
            {
                if (!toolBar.Buttons.Contains(toolBarButtonNew))
                {
                    if (toolBar.Buttons.Contains(toolBarButtonExpiry))
                    {
                        if (butoonIndex == 0)
                            butoonIndex = 2;
                        else
                            butoonIndex = 3;
                    }
                    else
                    {
                        if (toolBar.Buttons.Contains(toolBarButtonQA))
                        {
                            if (butoonIndex == 0)
                                butoonIndex = 3;
                            else
                                butoonIndex = 4;
                        }
                        else
                            butoonIndex = 4;
                    }
                }
            }
            else
            {
                if (butoonIndex == 2)
                {
                    if (toolBar.Buttons.Contains(toolBarButtonNew))
                    {
                        if (!toolBar.Buttons.Contains(toolBarButtonExpiry))
                        {
                            if (toolBar.Buttons.Contains(toolBarButtonQA))
                                butoonIndex += 1;
                            else
                                butoonIndex += 2;
                        }
                    }
                    else
                        butoonIndex += 2;
                }
                else if (butoonIndex == 3)
                {
                    if (!toolBar.Buttons.Contains(toolBarButtonExpiry))
                        butoonIndex += 1;
                }
            }

            return butoonIndex;
        }

        private void Toggel(int buttonIndex)
        {
            Reset();
            ResetBefore();

            if (buttonIndex == 0)
                toolBar.Buttons[indexNew].ImageIndex = 5;
            else if (buttonIndex == 1)
                toolBar.Buttons[indexUpdate].ImageIndex = 6;
            else if (buttonIndex == 2)
                toolBar.Buttons[indexExpiry].ImageIndex = 7;
            else if (buttonIndex == 3)
                toolBar.Buttons[indexQA].ImageIndex = 8;
            else if (buttonIndex == 4)
                toolBar.Buttons[indexXMLDelivery].ImageIndex = 9;
        }

        private void BindOpTaskList()
        {
            userFundings = null;
            pnlInstructOP.Visible = false;

            inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

            int pnlwidth = ClientRectangle.Width * 99 / 100;

            gridViewFundingBodyList.Width = pnlwidth * 49 / 100;
            gridViewFundingBodyList.Columns[0].Width = gridViewFundingBodyList.Width - 110;
            gridViewFundingBodyList.Columns[1].Width = 90;
            gridViewFundingBodyList.Controls.Clear();
            gridViewFundingBodyList.AllowUserToResizeRows = false;
            gridViewFundingBodyList.MultiSelect = false;
            gridViewFundingBodyList.ReadOnly = true;
            gridViewFundingBodyList.AutoGenerateColumns = false;

            gridViewOpportunityList.Left = gridViewFundingBodyList.ClientRectangle.Right + 20;
            gridViewOpportunityList.Width = pnlwidth * 49 / 100;
            gridViewOpportunityList.Columns[0].Width = gridViewOpportunityList.Width - 20;
            gridViewOpportunityList.DataSource = null;

            panelOpportunity.Width = pnlwidth;
            panelOpportunity.Left = (ClientRectangle.Width - (pnlwidth)) / 2;
            panelOpportunity.Visible = true;
            panelOpportunityList.Visible = false;

            rbFunding.Enabled = true;
            rbFunding.Checked = true;
            rbOpportunity.Checked = false;

            if (SharedObjects.ModuleId == 3 && selectedTask == "New")
            {
                currentBindedListCount = 4;

                SharedObjects.TaskId = 1;
                SharedObjects.Cycle = 0;

                userFundings = FundingBodyDataOperations.GetUserFundingLists(SharedObjects.User.USERID, SharedObjects.ModuleId);

                foreach (UserFunding userFunding in userFundings)
                {
                    if (!String.IsNullOrEmpty(userFunding.FundingBodyName))
                    {
                        string multiLangFundingBodyNameHexa = replace.ReadandReplaceHexaToChar(userFunding.FundingBodyName, inputXmlPath);
                        string multiLangFundingBodyNameUnicode = replace.ConvertUnicodeToText(multiLangFundingBodyNameHexa);

                        if (multiLangFundingBodyNameUnicode != "")
                            userFunding.FundingBodyName = multiLangFundingBodyNameUnicode;
                        else
                            userFunding.FundingBodyName = multiLangFundingBodyNameHexa;
                    }
                }

                gridViewFundingBodyList.DataSource = userFundings;
            }
            else if (SharedObjects.ModuleId == 3 && selectedTask == "QA")
            {
                currentBindedListCount = 5;

                SharedObjects.TaskId = 2;
                SharedObjects.Cycle = 0;

                userFundings = FundingBodyDataOperations.GetUserFundingListsByTask(SharedObjects.User.USERID, SharedObjects.ModuleId, SharedObjects.TaskId);

                foreach (UserFunding userFunding in userFundings)
                {
                    if (!String.IsNullOrEmpty(userFunding.FundingBodyName))
                    {
                        string multiLangFundingBodyNameHexa = replace.ReadandReplaceHexaToChar(userFunding.FundingBodyName, inputXmlPath);
                        string multiLangFundingBodyNameUnicode = replace.ConvertUnicodeToText(multiLangFundingBodyNameHexa);

                        if (multiLangFundingBodyNameUnicode != "")
                            userFunding.FundingBodyName = multiLangFundingBodyNameUnicode;
                        else
                            userFunding.FundingBodyName = multiLangFundingBodyNameHexa;
                    }
                }

                gridViewFundingBodyList.DataSource = userFundings;
            }
            else if (SharedObjects.ModuleId == 3 && selectedTask == "Update")
            {
                currentBindedListCount = 6;

                SharedObjects.TaskId = 1;
                SharedObjects.Cycle = 1;

                dummyTaskList = TaskDataOperation.GetDummyTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, 1, 1, 1);

                foreach (DummyTaskList dummyTaskList in dummyTaskList)
                {
                    if (!string.IsNullOrEmpty(dummyTaskList.FundingBodyName))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(dummyTaskList.FundingBodyName, inputXmlPath);

                        MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName);

                        if (MultiLang_FBName != "")
                            dummyTaskList.FundingBodyName = MultiLang_FBName;
                        else
                            dummyTaskList.FundingBodyName = replace.ReadandReplaceHexaToChar(dummyTaskList.FundingBodyName, inputXmlPath);
                    }
                }

                gridViewFundingBodyList.DataSource = dummyTaskList;
            }
            else if (SharedObjects.ModuleId == 3 && selectedTask == "Expiry")
            {
                currentBindedListCount = 7;

                rbFunding.Checked = false;
                rbFunding.Enabled = false;
                rbOpportunity.Checked = true;

                op_updatelist.DataSource = null;

                expireDetailList = TaskDataOperation.GetUserExpireDetails(SharedObjects.User.USERID);

                foreach (ExpiryDetail expiryDetail in expireDetailList)
                {
                    if (!string.IsNullOrEmpty(expiryDetail.OpportunityName))
                    {
                        string MultiLang_OppName = replace.ReadandReplaceHexaToChar(expiryDetail.OpportunityName, inputXmlPath);
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(expiryDetail.FundingBodyName, inputXmlPath);

                        MultiLang_OppName = replace.ConvertUnicodeToText(MultiLang_OppName);
                        MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName);

                        if (MultiLang_OppName != "")
                            expiryDetail.OpportunityName = MultiLang_OppName;
                        else
                            expiryDetail.OpportunityName = replace.ReadandReplaceHexaToChar(expiryDetail.OpportunityName, inputXmlPath);

                        if (MultiLang_FBName != "")
                            expiryDetail.FundingBodyName = MultiLang_FBName;
                        else
                            expiryDetail.FundingBodyName = replace.ReadandReplaceHexaToChar(expiryDetail.FundingBodyName, inputXmlPath);
                    }
                }

                panelOpportunity.Visible = false;
                panelOpportunityList.Visible = true;
                panelOpportunityList.Left = ((ClientRectangle.Width - panelOpportunityList.Width - 100) / 2);

                SharedObjects.TaskId = 7;
                SharedObjects.Cycle = 1;

                op_updatelist.Controls.Clear();
                op_updatelist.AllowUserToResizeRows = false;
                op_updatelist.MultiSelect = false;
                op_updatelist.ReadOnly = true;
                op_updatelist.AutoGenerateColumns = true;

                op_updatelist.DataSource = expireDetailList;
                op_updatelist.Columns["MODULENAME"].Visible = false;
                op_updatelist.Columns["ModuleId"].Visible = false;
                op_updatelist.Columns["TASKNAME"].Visible = false;
                op_updatelist.Columns["TaskId"].Visible = false;
                op_updatelist.Columns["CYCLE"].Visible = false;
                op_updatelist.Columns["ID"].Visible = false;
                op_updatelist.Columns[9].HeaderText = "Id";
                op_updatelist.Columns[8].HeaderText = "DueDate";
                op_updatelist.Columns[2].HeaderText = "Opportunity Name";
                op_updatelist.Columns[3].HeaderText = "FundingBodyName";
                op_updatelist.Columns[2].Width = 500;
                op_updatelist.Columns[3].Width = 185;
                op_updatelist.Columns[8].Width = 80;
                op_updatelist.Columns[9].Width = 100;
                op_updatelist.Height = 425;
                op_updatelist.Width = 865;

                panelOpportunityList.Width = 878;
                panelOpportunityList.Height = 432;
            }
            else
            {
                XML.GenerateXML objXml = new XML.GenerateXML();
                objXml.Show();
                this.Dispose();
            }
        }

        private void BindAWTaskList()
        {
            inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

            int pnlwidth = ClientRectangle.Width * 90 / 100;

            pnlInstructAW.Visible = false;
            panelAward.Visible = true;

            AW_FBLIST.Width = pnlwidth * 49 / 100;
            AW_FBLIST.Columns[0].Width = AW_FBLIST.Width - 110;
            AW_FBLIST.Columns[1].Width = 90;

            AW_list.Left = AW_FBLIST.ClientRectangle.Right + 20;
            AW_list.Width = pnlwidth * 49 / 100;
            AW_list.Columns[0].Width = AW_list.Width - 110;
            AW_list.Columns[1].Width = AW_list.Width - 20;

            panelAward.Width = pnlwidth;
            panelAward.Left = (ClientRectangle.Width - (pnlwidth)) / 2;

            pnlsearchAW.Left = (ClientRectangle.Width - pnlsearchAW.Width) / 2;

            if (SharedObjects.ModuleId == 4 && selectedTask == "New")
            {
                currentBindedListCount = 8;

                AW_list.DataSource = null;

                userFundings = FundingBodyDataOperations.GetUserFundingLists(SharedObjects.User.USERID, SharedObjects.ModuleId);

                foreach (UserFunding userFunding in userFundings)
                {
                    if (!String.IsNullOrEmpty(userFunding.FundingBodyName))
                    {
                        string multiLangFundingBodyNameHexa = replace.ReadandReplaceHexaToChar(userFunding.FundingBodyName, inputXmlPath);
                        string multiLangFundingBodyNameUnicode = replace.ConvertUnicodeToText(multiLangFundingBodyNameHexa);

                        if (multiLangFundingBodyNameUnicode != "")
                            userFunding.FundingBodyName = multiLangFundingBodyNameUnicode;
                        else
                            userFunding.FundingBodyName = multiLangFundingBodyNameHexa;
                    }
                }

                panelAward.Visible = true;

                SharedObjects.TaskId = 1;
                SharedObjects.Cycle = 0;

                AW_FBLIST.Controls.Clear();
                AW_FBLIST.AllowUserToResizeRows = false;
                AW_FBLIST.MultiSelect = false;
                AW_FBLIST.ReadOnly = true;
                AW_FBLIST.AutoGenerateColumns = false;
                AW_FBLIST.DataSource = userFundings;
            }
            else if (SharedObjects.ModuleId == 4 && selectedTask == "QA")
            {
                currentBindedListCount = 9;

                AW_list.DataSource = null;

                userFundings = FundingBodyDataOperations.GetUserFundingListsByTask(SharedObjects.User.USERID, SharedObjects.ModuleId, SharedObjects.TaskId);

                foreach (UserFunding userFunding in userFundings)
                {
                    if (!String.IsNullOrEmpty(userFunding.FundingBodyName))
                    {
                        string multiLangFundingBodyNameHexa = replace.ReadandReplaceHexaToChar(userFunding.FundingBodyName, inputXmlPath);
                        string multiLangFundingBodyNameUnicode = replace.ConvertUnicodeToText(multiLangFundingBodyNameHexa);

                        if (multiLangFundingBodyNameUnicode != "")
                            userFunding.FundingBodyName = multiLangFundingBodyNameUnicode;
                        else
                            userFunding.FundingBodyName = multiLangFundingBodyNameHexa;
                    }
                }

                panelAward.Visible = true;

                SharedObjects.TaskId = 2;
                SharedObjects.Cycle = 0;

                AW_FBLIST.Controls.Clear();
                AW_FBLIST.AllowUserToResizeRows = false;
                AW_FBLIST.MultiSelect = false;
                AW_FBLIST.ReadOnly = true;
                AW_FBLIST.AutoGenerateColumns = false;
                AW_FBLIST.DataSource = userFundings;
            }
            else if (SharedObjects.ModuleId == 4 && selectedTask == "Update")
            {
                currentBindedListCount = 10;

                AW_list.DataSource = null;

                dummyTaskList = TaskDataOperation.GetDummyTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, 1, 1, 1);

                foreach (DummyTaskList dummyTaskList in dummyTaskList)
                {
                    if (!string.IsNullOrEmpty(dummyTaskList.FundingBodyName))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(dummyTaskList.FundingBodyName, inputXmlPath);

                        MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName);

                        if (MultiLang_FBName != "")
                            dummyTaskList.FundingBodyName = MultiLang_FBName;
                        else
                            dummyTaskList.FundingBodyName = replace.ReadandReplaceHexaToChar(dummyTaskList.FundingBodyName, inputXmlPath);
                    }
                }

                panelOpportunity.Visible = true;
                panelOpportunityList.Visible = false;

                SharedObjects.TaskId = 1;
                SharedObjects.Cycle = 1;

                AW_FBLIST.Controls.Clear();
                AW_FBLIST.AllowUserToResizeRows = false;
                AW_FBLIST.MultiSelect = false;
                AW_FBLIST.ReadOnly = true;
                AW_FBLIST.AutoGenerateColumns = false;
                AW_FBLIST.DataSource = dummyTaskList;
            }
            else
            {
                Scival.XML.GenerateXML objXml = new Scival.XML.GenerateXML();
                objXml.Show();
                this.Dispose();
            }
        }

        private void Bind()
        {
            inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);

            PnlInstructFB.Visible = false;
            PanelTaskMessage.Left = (this.Width - PanelTaskMessage.Width) / 2;

            if (SharedObjects.ModuleId == 2 && selectedTask == "Delivery")
            {
                XML.GenerateXML objXml = new XML.GenerateXML();
                objXml.Show();
                this.Dispose();
            }

            if (SharedObjects.ModuleId == 2 && selectedTask == "QA")
            {
                currentBindedListCount = 1;

                int pnlwidth = (ClientRectangle.Width - PanelFBTaskList.Width) / 2;
                PanelFBTaskList.Left = pnlwidth;
                PanelFBTaskList.Visible = true;

                SharedObjects.TaskId = 2;
                SharedObjects.Cycle = 0;
                SharedObjects.TRAN_TYPE_ID = 2;

                newTaskLists = TaskDataOperation.GetNewTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, 2, 0, SharedObjects.Allocation);

                foreach (NewTaskList newTask in newTaskLists)
                {
                    if (!string.IsNullOrEmpty(newTask.FundingBodyName))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(newTask.FundingBodyName, inputXmlPath);

                        //MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName); -- Demo

                        if (MultiLang_FBName != "")
                            newTask.FundingBodyName = MultiLang_FBName;
                        else
                            newTask.FundingBodyName = replace.ReadandReplaceHexaToChar(newTask.FundingBodyName, inputXmlPath);
                    }
                }

                dataGridView1.Controls.Clear();
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = newTaskLists;
            }

            else if (SharedObjects.ModuleId == 2 && selectedTask == "Update")
            {
                currentBindedListCount = 2;

                int pnlwidth = (ClientRectangle.Width - PanelFBTaskList.Width) / 2;
                PanelFBTaskList.Left = pnlwidth;
                PanelFBTaskList.Visible = true;

                SharedObjects.TaskId = 2;
                SharedObjects.Cycle = 0;
                SharedObjects.TRAN_TYPE_ID = 1;
                // SharedObjects.Allocation = 0;

                //dummyTaskList = TaskDataOperation.GetDummyTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, 2, 1, SharedObjects.Allocation);
                dummyTaskList = TaskDataOperation.GetDummyTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, 2, 1, SharedObjects.Allocation);

                foreach (DummyTaskList dummyTask in dummyTaskList)
                {
                    if (!string.IsNullOrEmpty(dummyTask.FundingBodyName))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(dummyTask.FundingBodyName, inputXmlPath);

                        MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName);

                        if (MultiLang_FBName != "")
                            dummyTask.FundingBodyName = MultiLang_FBName;
                        else
                            dummyTask.FundingBodyName = replace.ReadandReplaceHexaToChar(dummyTask.FundingBodyName, inputXmlPath);
                    }
                }

                dataGridView1.Controls.Clear();
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dummyTaskList;
            }

            else
            {
                currentBindedListCount = 3;

                int pnlwidth = (ClientRectangle.Width - PanelFBTaskList.Width) / 2;
                PanelFBTaskList.Left = pnlwidth;
                PanelFBTaskList.Visible = true;

                SharedObjects.TaskId = 1;
                SharedObjects.Cycle = 0;
                SharedObjects.Allocation = 0;
                newTaskLists = TaskDataOperation.GetNewTaskList(SharedObjects.User.USERID, SharedObjects.ModuleId, SharedObjects.TaskId, SharedObjects.Cycle, SharedObjects.Allocation);

                foreach (NewTaskList newTask in newTaskLists)
                {
                    if (!string.IsNullOrEmpty(newTask.FundingBodyName))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(newTask.FundingBodyName, inputXmlPath);

                        // MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName); --Demo

                        if (MultiLang_FBName != "")
                            newTask.FundingBodyName = MultiLang_FBName;
                        else
                            newTask.FundingBodyName = replace.ReadandReplaceHexaToChar(newTask.FundingBodyName, inputXmlPath);
                    }
                }

                dataGridView1.Controls.Clear();
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = newTaskLists;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabIndex = ScivalTabControl.SelectedIndex;
            Int64 tabSelectionId = 0;

            var tabSelectionList = TaskDataOperation.GetTabSelectionValue(SharedObjects.User.USERID);

            if (tabSelectionList != null && tabSelectionList.Count > 0)
            {
                tabSelectionId = tabSelectionList.First().Value;

                if (tabSelectionId == 2 && ScivalTabControl.TabPages.Contains(TabFB))
                {
                    ((Control)this.TabFB).Enabled = true;
                    ((Control)this.TabOP).Enabled = false;
                    ((Control)this.TabAW).Enabled = false;
                }
                else if (tabSelectionId == 3 && ScivalTabControl.TabPages.Contains(TabOP))
                {
                    ((Control)this.TabFB).Enabled = false;
                    ((Control)this.TabOP).Enabled = true;
                    ((Control)this.TabAW).Enabled = false;
                }
                else if (tabSelectionId == 4 && ScivalTabControl.TabPages.Contains(TabAW))
                {
                    ((Control)this.TabFB).Enabled = false;
                    ((Control)this.TabOP).Enabled = false;
                    ((Control)this.TabAW).Enabled = true;
                }
                else
                {
                    ((Control)this.TabFB).Enabled = true;
                    ((Control)this.TabOP).Enabled = true;
                    ((Control)this.TabAW).Enabled = true;
                }
            }

            if (tabIndex == 0)
            {
                if (ScivalTabControl.TabPages.Contains(TabFB))
                {
                    SetTabPageIndex(2, 2, 3, 1, 2);

                    int pnlwidth = (ClientRectangle.Width - PnlInstructFB.Width) / 2;
                    PnlInstructFB.Left = pnlwidth;
                    PnlInstructFB.Visible = true;
                    pnlwidth = (PnllblInstrucFB.Width - lblInstructFB.Width) / 2;
                    lblInstructFB.Left = pnlwidth;
                }
                else
                {
                    if (ScivalTabControl.TabPages.Contains(TabOP))
                    {
                        SetTabPageIndex(3, 3, 0, 4, 2);

                        int pnlwidth = (ClientRectangle.Width - pnlInstructOP.Width) / 2;
                        pnlInstructOP.Left = pnlwidth;
                        pnlInstructOP.Visible = true;
                        pnlwidth = (pnllbInstructOP.Width - lbInstructOP.Width) / 2;
                        lbInstructOP.Left = pnlwidth;
                    }
                    else
                    {
                        if (ScivalTabControl.TabPages.Contains(TabAW))
                        {
                            SetTabPageIndex(4, 4, 0, 1, 5);
                        }
                    }
                }
            }
            else if (tabIndex == 1)
            {
                if (ScivalTabControl.TabPages.Contains(TabFB))
                {
                    if (ScivalTabControl.TabPages.Contains(TabOP))
                    {
                        SetTabPageIndex(3, 3, 0, 4, 2);

                        int pnlwidth = (ClientRectangle.Width - pnlInstructOP.Width) / 2;
                        pnlInstructOP.Left = pnlwidth;
                        pnlInstructOP.Visible = true;
                        pnlwidth = (pnllbInstructOP.Width - lbInstructOP.Width) / 2;
                        lbInstructOP.Left = pnlwidth;
                    }
                    else
                    {
                        if (ScivalTabControl.TabPages.Contains(TabAW))
                        {
                            SetTabPageIndex(4, 4, 0, 1, 5);

                            int pnlwidth = (ClientRectangle.Width - pnlInstructAW.Width) / 2;
                            pnlInstructAW.Left = pnlwidth;
                            pnlInstructAW.Visible = true;
                            pnlwidth = (PnllblInstructAW.Width - lblInstructAW.Width) / 2;
                            lblInstructAW.Left = pnlwidth;
                            panelAward.Visible = false;
                        }
                    }
                }
                else
                {
                    SetTabPageIndex(4, 4, 0, 1, 5);

                    int pnlwidth = (ClientRectangle.Width - pnlInstructAW.Width) / 2;
                    pnlInstructAW.Left = pnlwidth;
                    pnlInstructAW.Visible = true;
                    pnlwidth = (PnllblInstructAW.Width - lblInstructAW.Width) / 2;
                    lblInstructAW.Left = pnlwidth;
                    panelAward.Visible = false;
                }
            }
            else if (tabIndex == 2)
            {
                SetTabPageIndex(4, 4, 0, 1, 5);

                int pnlwidth = (ClientRectangle.Width - pnlInstructAW.Width) / 2;
                pnlInstructAW.Left = pnlwidth;
                pnlInstructAW.Visible = true;
                pnlwidth = (PnllblInstructAW.Width - lblInstructAW.Width) / 2;
                lblInstructAW.Left = pnlwidth;
                panelAward.Visible = false;
            }
        }

        void toolbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CheckTAB)
                return;

            for (int i = 0; i < toolBar.Buttons.Count; i++)
            {
                if ((e.X >= toolBar.Buttons[i].Rectangle.Left) && (e.X <= toolBar.Buttons[i].Rectangle.Left + toolBar.Buttons[i].Rectangle.Width)
                    && (e.Y >= toolBar.Buttons[i].Rectangle.Top && e.Y <= toolBar.Buttons[i].Rectangle.Top + toolBar.Buttons[i].Rectangle.Height))
                {
                    ResetBefore();
                    Reset();

                    int BTindex = Getbutton(i);

                    if (toolBar.Buttons[i].Enabled)
                    {
                        switch (BTindex)
                        {
                            case 0:
                                toolBar.Buttons[i].ImageIndex = 10;
                                break;
                            case 1:
                                toolBar.Buttons[i].ImageIndex = 11;
                                break;
                            case 2:
                                toolBar.Buttons[i].ImageIndex = 12;
                                break;
                            case 3:
                                toolBar.Buttons[i].ImageIndex = 13;
                                break;
                            case 4:
                                toolBar.Buttons[i].ImageIndex = 14;
                                break;
                        }
                    }
                }
            }
        }

        private void DashBoardBT_Click(object sender, EventArgs e)
        {
            bool flag = true;

            try
            {
                if (SharedObjects.ModuleId == 2)
                {
                    if (dataGridView1.SelectedCells.Count > 0)
                    {
                        SharedObjects.ID = Convert.ToInt64(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value);
                        SharedObjects.FundingBodyName = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                        SharedObjects.DueDate = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                    }
                    else
                    {
                        flag = false;
                        MessageBox.Show("Please select task.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (SharedObjects.ModuleId == 3)
                {
                    if (SharedObjects.TaskId != 7)
                    {
                        DataTable DT = (DataTable)gridViewOpportunityList.DataSource;

                        if (DT != null)
                        {
                            if (DT.Rows.Count > 0)
                            {
                                if (gridViewOpportunityList.SelectedCells.Count > 0)
                                {
                                    DataTable DTFB = (DataTable)gridViewFundingBodyList.DataSource;

                                    if (gridViewFundingBodyList.SortedColumn != null)
                                    {
                                        DTFB.DefaultView.Sort = gridViewFundingBodyList.SortedColumn.DataPropertyName;
                                        DTFB = DTFB.DefaultView.ToTable();
                                    }

                                    SharedObjects.ID = Convert.ToInt64(DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][0]);
                                    SharedObjects.OPFBID = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex][0]);
                                    SharedObjects.FundingBodyName = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex]["FUNDINGBODYNAME"]) + " (" + DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][3].ToString() + ")";
                                }
                                else
                                {
                                    flag = false;
                                    MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        DataTable DT = (DataTable)op_updatelist.DataSource;
                        if (DT != null)
                        {
                            if (DT.Rows.Count > 0)
                            {
                                if (op_updatelist.SelectedCells.Count > 0)
                                {
                                    int rowindex = op_updatelist.CurrentCell.RowIndex;
                                    int columnindex = op_updatelist.CurrentCell.ColumnIndex;
                                    string name = op_updatelist.Rows[rowindex].Cells[2].Value.ToString();

                                    DataView dv = new DataView(DT.Copy());
                                    dv.RowFilter = String.Format("OPPNAME = '{0}'", name.Replace("'", "''"));
                                    SharedObjects.ID = Convert.ToInt64(dv[0]["id"]);
                                    SharedObjects.FundingBodyName = Convert.ToString(dv[0]["OPPNAME"]);
                                }
                                else
                                {
                                    flag = false;
                                    MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (SharedObjects.ModuleId == 4)
                {
                    DataTable DT = (DataTable)AW_list.DataSource;
                    if (DT != null)
                    {
                        if (DT.Rows.Count > 0)
                        {
                            if (AW_list.SelectedCells.Count > 0)
                            {
                                DataTable DTFB = (DataTable)AW_FBLIST.DataSource;

                                if (AW_FBLIST.SortedColumn != null)
                                {
                                    DTFB.DefaultView.Sort = AW_FBLIST.SortedColumn.DataPropertyName;
                                    DTFB = DTFB.DefaultView.ToTable();
                                }

                                SharedObjects.ID = Convert.ToInt64(AW_list.Rows[AW_list.SelectedCells[0].RowIndex].Cells[1].Value);
                                SharedObjects.OPFBID = Convert.ToString(DTFB.Rows[AW_FBLIST.SelectedCells[0].RowIndex][0]);
                                SharedObjects.FundingBodyName = Convert.ToString(DTFB.Rows[AW_FBLIST.SelectedCells[0].RowIndex]["FUNDINGBODYNAME"]) + " (" + AW_list.Rows[AW_list.SelectedCells[0].RowIndex].Cells[0].Value.ToString() + ")";
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        flag = false;
                        MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (flag && SharedObjects.TaskId != 7)
                {
                    DialogResult Result = MessageBox.Show("Do you want to continue with " + SharedObjects.FundingBodyName, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (Result == DialogResult.OK)
                    {
                        DashBoard DB = new DashBoard();
                        DB.Show();
                        this.Dispose();
                    }
                }
                else
                {
                    DashBoard DB = new DashBoard();
                    DB.Show();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RBauto_CheckedChanged(object sender, EventArgs e)
        {
            if (flagTaskAllocation)
            {
                return;
            }
            else if (RBauto.Checked)
            {
                SharedObjects.Allocation = 0;
                Bind();
            }
        }

        private void RBmanual_CheckedChanged(object sender, EventArgs e)
        {
            if (flagTaskAllocation)
            {
                return;
            }
            else if (RBmanual.Checked)
            {
                SharedObjects.Allocation = 1;
                Bind();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SharedObjects.ID = GetIdFromBindSource(e.RowIndex);
                SharedObjects.FundingBodyName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                SharedObjects.DueDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());

                try
                {
                    if (SharedObjects.TaskId != 7)
                    {
                        DialogResult Result = MessageBox.Show("Do you want to continue with " + SharedObjects.FundingBodyName, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (Result == DialogResult.OK)
                        {
                            DashBoard DB = new DashBoard();
                            DB.Show();
                            this.Dispose();
                        }
                    }
                }
                catch (Exception ex) { 
                }
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridView DV = (DataGridView)sender;

            if (e.KeyChar == 13 && DV.CurrentCell.ColumnIndex == 0)
            {
                SharedObjects.ID = GetIdFromBindSource(DV.CurrentRow.Index);
                SharedObjects.FundingBodyName = DV.Rows[DV.CurrentRow.Index].Cells[0].Value.ToString();
                SharedObjects.DueDate = Convert.ToDateTime(DV.Rows[DV.CurrentRow.Index].Cells[1].Value.ToString());
                DashBoard DB = new DashBoard();
                DB.Show();
                this.Dispose();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
                dataGridView1.Rows[e.RowIndex].Selected = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SharedObjects.Allocation = 1;
            BindOpTaskList();
        }

        private void OP_FBLIST_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataTable DT = (DataTable)gridViewFundingBodyList.DataSource;

                if (gridViewFundingBodyList.SortedColumn != null)
                {
                    DT.DefaultView.Sort = gridViewFundingBodyList.SortedColumn.DataPropertyName;
                    DT = DT.DefaultView.ToTable();
                }

                currentBindedListCount = 11;

                if (SharedObjects.ModuleId == 3 && selectedTask == "New")
                {
                    var dbOpportunityLists = OpportunityDataOperations.GetOpportunityListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 1, 0, SharedObjects.User.USERID);

                    OpportunityList opportunityList = new OpportunityList
                    {
                        WORKFLOWID = 99999999,
                        OPPORTUNITY_ID = 99999999,
                        FUNDINGBODYOPPORTUNITYID = 99999999,
                        Name = "New Opportunity"
                    };

                    opportunityLists = new List<OpportunityList>();

                    opportunityLists.Add(opportunityList);
                    opportunityLists.AddRange(dbOpportunityLists);
                }
                else if (SharedObjects.ModuleId == 3 && selectedTask == "Update")
                {
                    opportunityLists = OpportunityDataOperations.GetOpportunityListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 2, 1, SharedObjects.User.USERID);
                }
                else if (SharedObjects.ModuleId == 3 && selectedTask == "QA")
                {
                    opportunityLists = OpportunityDataOperations.GetOpportunityListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 2, 0, SharedObjects.User.USERID);
                }

                foreach (OpportunityList opportunity in opportunityLists)
                {
                    if (!string.IsNullOrEmpty(opportunity.Name))
                    {
                        string MultiLang_FBName = replace.ReadandReplaceHexaToChar(opportunity.Name, inputXmlPath);

                        MultiLang_FBName = replace.ConvertUnicodeToText(MultiLang_FBName);

                        if (MultiLang_FBName != "")
                            opportunity.Name = MultiLang_FBName;
                        else
                            opportunity.Name = replace.ReadandReplaceHexaToChar(opportunity.Name, inputXmlPath);
                    }
                }

                gridViewOpportunityList.Controls.Clear();
                gridViewOpportunityList.AllowUserToResizeRows = false;
                gridViewOpportunityList.MultiSelect = false;
                gridViewOpportunityList.ReadOnly = true;
                gridViewOpportunityList.AutoGenerateColumns = false;
                gridViewOpportunityList.DataSource = opportunityLists;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbFunding.Checked)
            {
                if (textBox1.Text.Trim() != "")
                {
                    userFundings = (List<UserFunding>)gridViewFundingBodyList.DataSource;
                    gridViewFundingBodyList.DataSource = userFundings.Where(uf => uf.FundingBodyName.Contains(textBox1.Text.Replace("'", "''"))).ToList();
                }
                else
                {
                    MessageBox.Show("Please provide search keyword.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (rbOpportunity.Checked)
            {
                if (textBox1.Text.Trim() != "")
                {
                    if (!OP_searchflag)
                    {
                        if (selectedTask == "Expiry")
                        {
                            op_updatelist.DataSource = expireDetailList.Where(ed => ed.OpportunityName.Contains(textBox1.Text.Replace("'", "''"))).ToList();
                            OP_searchflag = true;
                        }
                        else
                        {
                            DataTable DTOPSearch = (DataTable)gridViewOpportunityList.DataSource;
                            DTOPSearch.DefaultView.RowFilter = String.Format("NAME LIKE '%{0}%'", textBox1.Text.Replace("'", "''"));
                            gridViewOpportunityList.DataSource = DTOPSearch.DefaultView.ToTable();
                            OP_searchflag = true;
                        }
                    }
                    else
                    {
                        if (selectedTask == "Expiry")
                        {
                            op_updatelist.DataSource = expireDetailList.Where(ed => ed.OpportunityName.Contains(textBox1.Text.Replace("'", "''"))).ToList();
                        }
                        else
                        {
                            DataTable DTSearch = (DataTable)gridViewOpportunityList.DataSource;
                            DTSearch.DefaultView.RowFilter = String.Format("NAME LIKE '%{0}%'", textBox1.Text.Replace("'", "''"));
                            gridViewOpportunityList.DataSource = DTSearch.DefaultView.ToTable();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please provide search keyword.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select search mode.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OP_LIST_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (SharedObjects.ModuleId == 2)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Selected)
                        {
                            SharedObjects.ID = GetIdFromBindSource(i);
                            SharedObjects.FundingBodyName = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            SharedObjects.DueDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            break;
                        }
                    }
                }
                else if (SharedObjects.ModuleId == 3)
                {
                    DataTable DT = (DataTable)gridViewOpportunityList.DataSource;
                    DataTable DTFB = (DataTable)gridViewFundingBodyList.DataSource;

                    if (gridViewFundingBodyList.SortedColumn != null)
                    {
                        DTFB.DefaultView.Sort = gridViewFundingBodyList.SortedColumn.DataPropertyName;
                        DTFB = DTFB.DefaultView.ToTable();
                    }

                    SharedObjects.ID = Convert.ToInt64(DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][0]);
                    SharedObjects.OPFBID = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex][0]);
                    SharedObjects.FundingBodyName = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex]["FUNDINGBODYNAME"]) + " (" + DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][3].ToString() + ")";
                }

                DashBoard DB = new DashBoard();
                DB.Show();
                this.Dispose();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AW_FBLIST_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                inputXmlPath = Path.GetDirectoryName(Application.ExecutablePath);
                DataTable DT = (DataTable)AW_FBLIST.DataSource;

                if (AW_FBLIST.SortedColumn != null)
                {
                    DT.DefaultView.Sort = AW_FBLIST.SortedColumn.DataPropertyName;
                    DT = DT.DefaultView.ToTable();
                }

                currentBindedListCount = 12;

                if (SharedObjects.ModuleId == 4 && selectedTask == "New")
                {
                    var dbAwardLists = AwardDataOperations.GetAwardListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 1, 0, SharedObjects.User.USERID);

                    AwardList awardList = new AwardList
                    {
                        WORKFLOWID = 99999999,
                        AWARD_ID = 99999999,
                        FUNDINGBODYAWARDID = 99999999,
                        Name = "New Award"
                    };

                    awardLists = new List<AwardList>();

                    awardLists.Add(awardList);
                    awardLists.AddRange(dbAwardLists);
                }
                else if (SharedObjects.ModuleId == 4 && selectedTask == "Update")
                {
                    awardLists = AwardDataOperations.GetAwardListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 2, 1, SharedObjects.User.USERID);
                }
                else if (SharedObjects.ModuleId == 4 && selectedTask == "QA")
                {
                    awardLists = AwardDataOperations.GetAwardListsByTask(Convert.ToInt64(DT.Rows[e.RowIndex][0]), 2, 0, SharedObjects.User.USERID);
                }

                foreach (AwardList awardList in awardLists)
                {
                    if (!string.IsNullOrEmpty(awardList.Name))
                    {
                        string MultiLang_AWName = replace.ReadandReplaceHexaToChar(awardList.Name, inputXmlPath);

                        MultiLang_AWName = replace.ConvertUnicodeToText(MultiLang_AWName);

                        if (MultiLang_AWName != "")
                            awardList.Name = MultiLang_AWName;
                        else
                            awardList.Name = replace.ReadandReplaceHexaToChar(awardList.Name, inputXmlPath);
                    }
                }

                AW_list.Controls.Clear();
                AW_list.AllowUserToResizeRows = false;
                AW_list.MultiSelect = false;
                AW_list.ReadOnly = true;
                AW_list.AutoGenerateColumns = false;
                AW_list.DataSource = awardLists;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            if (userFundings != null)
                gridViewFundingBodyList.DataSource = userFundings;

            if (selectedTask == "Expiry")
                op_updatelist.DataSource = expireDetailList;

            textBox1.Text = "";
        }

        private void AW_list_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = true;

            try
            {
                if (SharedObjects.ModuleId == 2)
                {
                    if (dataGridView1.SelectedCells.Count > 0)
                    {
                        SharedObjects.ID = GetIdFromBindSource(dataGridView1.SelectedCells[0].RowIndex);
                        SharedObjects.FundingBodyName = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                        SharedObjects.DueDate = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                    }
                    else
                    {
                        flag = false;
                        MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (SharedObjects.ModuleId == 3)
                {
                    if (SharedObjects.TaskId != 7)
                    {
                        DataTable DT = (DataTable)gridViewOpportunityList.DataSource;
                        if (DT != null)
                        {
                            if (DT.Rows.Count > 0)
                            {
                                if (gridViewOpportunityList.SelectedCells.Count > 0)
                                {
                                    DataTable DTFB = (DataTable)gridViewFundingBodyList.DataSource;

                                    if (gridViewFundingBodyList.SortedColumn != null)
                                    {
                                        DTFB.DefaultView.Sort = gridViewFundingBodyList.SortedColumn.DataPropertyName;
                                        DTFB = DTFB.DefaultView.ToTable();
                                    }

                                    SharedObjects.ID = Convert.ToInt64(DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][0]);
                                    SharedObjects.OPFBID = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex][0]);
                                    SharedObjects.FundingBodyName = Convert.ToString(DTFB.Rows[gridViewFundingBodyList.SelectedCells[0].RowIndex]["FUNDINGBODYNAME"]) + " (" + DT.Rows[gridViewOpportunityList.SelectedCells[0].RowIndex][3].ToString() + ")";
                                }
                                else
                                {
                                    flag = false;
                                    MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        DataTable DT = (DataTable)op_updatelist.DataSource;

                        if (DT != null)
                        {
                            if (DT.Rows.Count > 0)
                            {
                                if (op_updatelist.SelectedCells.Count > 0)
                                {

                                    SharedObjects.ID = Convert.ToInt64(DT.Rows[op_updatelist.SelectedCells[0].RowIndex][3]);
                                }
                                else
                                {
                                    flag = false;
                                    MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (SharedObjects.ModuleId == 4)
                {
                    DataTable DT = (DataTable)AW_list.DataSource;

                    if (DT != null)
                    {
                        if (DT.Rows.Count > 0)
                        {
                            if (AW_list.SelectedCells.Count > 0)
                            {
                                DataTable DTFB = (DataTable)AW_FBLIST.DataSource;
                                if (AW_FBLIST.SortedColumn != null)
                                {
                                    DTFB.DefaultView.Sort = AW_FBLIST.SortedColumn.DataPropertyName;
                                    DTFB = DTFB.DefaultView.ToTable();
                                }
                                SharedObjects.ID = Convert.ToInt64(DT.Rows[AW_list.SelectedCells[0].RowIndex][0]);
                                SharedObjects.OPFBID = Convert.ToString(DTFB.Rows[AW_FBLIST.SelectedCells[0].RowIndex][0]);
                                SharedObjects.FundingBodyName = Convert.ToString(DTFB.Rows[AW_FBLIST.SelectedCells[0].RowIndex]["FUNDINGBODYNAME"]) + " (" + DT.Rows[AW_list.SelectedCells[0].RowIndex][3].ToString() + ")";
                            }
                            else
                            {
                                flag = false;
                                MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            flag = false;
                            MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        flag = false;
                        MessageBox.Show("Please select task. ", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (flag)
                {
                    DashBoard DB = new DashBoard();
                    DB.Show();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message, "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnawsearch_Click(object sender, EventArgs e)
        {
            if (rbsearchfb.Checked)
            {
                if (txtsearchaw.Text.Trim() != "")
                {
                    if (!AWFB_searchflag)
                    {
                        DTAWFBSearch = ((DataTable)AW_FBLIST.DataSource).Copy();
                        DTAWFBSearch.DefaultView.RowFilter = String.Format("FUNDINGBODYNAME LIKE '%{0}%'", txtsearchaw.Text.Replace("'", "''"));
                        AW_FBLIST.DataSource = DTAWFBSearch.DefaultView.ToTable();
                        AWFB_searchflag = true;
                    }
                    else
                    {
                        DataTable DTSearch = (DataTable)AW_FBLIST.DataSource;
                        DTSearch.DefaultView.RowFilter = String.Format("FUNDINGBODYNAME LIKE '%{0}%'", txtsearchaw.Text.Replace("'", "''"));
                        AW_FBLIST.DataSource = DTSearch.DefaultView.ToTable();
                    }
                }
                else
                {
                    MessageBox.Show("Please provide search keyword.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (rbawsearch.Checked)
            {
                if (txtsearchaw.Text.Trim() != "")
                {
                    if (!OP_searchflag)
                    {
                        DataTable DTAWSearch = (DataTable)AW_list.DataSource;
                        DTAWSearch.DefaultView.RowFilter = String.Format("NAME LIKE '%{0}%'", txtsearchaw.Text.Replace("'", "''"));
                        AW_list.DataSource = DTAWSearch.DefaultView.ToTable();
                        AW_searchflag = true;
                    }
                    else
                    {
                        DataTable DTSearch = (DataTable)AW_list.DataSource;
                        DTSearch.DefaultView.RowFilter = String.Format("NAME LIKE '%{0}%'", txtsearchaw.Text.Replace("'", "''"));
                        AW_list.DataSource = DTSearch.DefaultView.ToTable();
                    }
                }
                else
                {
                    MessageBox.Show("Please provide search keyword.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select search mode.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnsearchFB_Click(object sender, EventArgs e)
        {
            if (txtFBsearch.Text.Trim() != "")
            {
                if (!Fbody_searchflag)
                {
                    if (dataGridView1.DataSource != null)
                    {
                        DTFbodySearch = ((DataTable)dataGridView1.DataSource).Copy();
                        DTFbodySearch.DefaultView.RowFilter = String.Format("FUNDINGBODYNAME LIKE '%{0}%'", txtFBsearch.Text.Replace("'", "''"));
                        dataGridView1.DataSource = DTFbodySearch.DefaultView.ToTable();
                        Fbody_searchflag = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select Fundingbody Type & Task.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DataTable DTSearch = (DataTable)dataGridView1.DataSource;
                    DTSearch.DefaultView.RowFilter = String.Format("FUNDINGBODYNAME LIKE '%{0}%'", txtFBsearch.Text.Replace("'", "''"));
                    dataGridView1.DataSource = DTSearch.DefaultView.ToTable();
                }
            }
            else
            {
                MessageBox.Show("Please provide search keyword.", "SCIVAL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnawsearchreset_Click(object sender, EventArgs e)
        {
            if (DTAWFBSearch != null)
            {
                DataTable dt = DTAWFBSearch.Copy();
                AW_FBLIST.DataSource = dt;
            }
            txtsearchaw.Text = "";
        }

        private void btnResetFB_Click(object sender, EventArgs e)
        {
            if (DTFbodySearch != null)
            {
                DataTable dt = DTFbodySearch.Copy();
                dataGridView1.DataSource = dt;
            }
            txtFBsearch.Text = "";
        }

        private void AW_list_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
                AW_list.Rows[e.RowIndex].Selected = false;
        }

        private void TaskBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private Int64 GetIdFromBindSource(int rowNum)
        {
            Int64 id = 0;

            switch (currentBindedListCount)
            {
                case 1:
                case 3:
                    id = newTaskLists[rowNum].Id;
                    break;
                case 2:
                case 6:
                case 10:
                    id = dummyTaskList[rowNum].Id;
                    break;
                case 7:
                    id = expireDetailList[rowNum].Id;
                    break;
                case 11:
                    id = opportunityLists[rowNum].Id;
                    break;
                case 12:
                    id = awardLists[rowNum].Id;
                    break;
                case 4:
                case 5:
                case 8:
                case 9:
                default:
                    id = 0;
                    break;
            }

            return id;
        }

        private void TabFB_Click(object sender, EventArgs e)
        {

        }
    }
}
