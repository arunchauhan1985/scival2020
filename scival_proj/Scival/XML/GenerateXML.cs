using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip;
using MySqlDal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scival.XML
{
    public partial class GenerateXML : BaseForm
    {
        ErrorLog errorLog = new ErrorLog();
        XMLProcess xmlProcess = new XMLProcess();
        FTPClass ftp = new FTPClass();

        static Int64 moduleId = 0;
        static String opportunityId = "";

        Thread thread = null;
        Thread proStart = null;

        Int64 MaxXMLCount = 0;
        string XsdPath = String.Empty;
        string serverip = string.Empty;
        string userid = string.Empty;
        string pass = string.Empty;
        string checksum = string.Empty;

        //DAL.XML objXML = new DAL.XML();
        //DAL.XMLGenerate objXMLGenerate = new DAL.XMLGenerate(ref Cn);
        //DAL.Opportunity dalopp = new DAL.Opportunity();

        public GenerateXML()
        {
            InitializeComponent();
            loadInitialValue();
            ClearZipFromUser();

            lblCurrentLimits.Text = XmlDataOperations.GetXmlGenrationLimit(0, 1);
        }

        protected void btnFBNew_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    #region Scival 5.0 JSON UPLOAD

            //    string uploadStatus = "false";
            //    string url = "https://ingestion.staging.fundingdiscovery.com/ingestion/funding-body";
            //    string filePath = @"D:\SCIVAL_NewDevelopment\SampleJson\100000999.json";
            //    string json = File.ReadAllText(filePath);

            //    Random rnd = new Random();

            //    POST(url, json);

            //    if (uploadStatus == "true")
            //        MessageBox.Show("File Uploaded");
            //    else
            //        MessageBox.Show("File Not Uploaded");

            //    #endregion Scival 5.0 JSON UPLOAD

            //    errorLog.WorkProcessLog("-------------Funding body New Statred--------------", "FBNew");

            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        errorLog.WorkProcessLog("Unable to connect Archive server.");
            //        return;
            //    }

            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationManager.AppSettings["FTPFBPath"]);

            //    ftp.FtpClient(serverip, userid, pass, remoteloc);

            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();

            //    Int64 FBCOUNT = Convert.ToInt64(((DataSet)objXML.GetFundingCount(1)).Tables["FundingBodyCount"].Rows[0]["Count"]);

            //    errorLog.WorkProcessLog("Total FB count : " + FBCOUNT.ToString());

            //    if (FBCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (FBCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = FBCOUNT / MaxXMLCount;

            //            if (FBCOUNT % MaxXMLCount != 0)
            //                Pagecount++;
            //        }

            //        Int64 Start = 0;
            //        Int64 End = 0;

            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            errorLog.WorkProcessLog("Funding Body data get from database");
            //            DT = XmlDataOperations.GetFundingBody(1, XsdPath, Start, End);
            //            errorLog.WorkProcessLog("Data recived and xmx created");

            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //                DSXML.Tables.Add(DT);
            //            else
            //                DSXMLErr.Tables.Add(DT);
            //        }

            //        oleTrnas = Cn.BeginTransaction();

            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            errorLog.WorkProcessLog("Error in XML: " + xmlerrmsg);
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }

            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";

            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(2, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 1);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }

            //        DataSet zipdtl = new DataSet();

            //        if (xmldtl.Tables.Count > 1)
            //            zipdtl = objXMLGenerate.ZipArchieve(2, 1, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());

            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;

            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                errorLog.WorkProcessLog("Server Process Started");
            //                Zippath = WriteXmlfile(DSXML);

            //                if (!FTPUpload(2, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;

            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }

            //                TreeView1.Nodes.Add(TN);

            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No new FundingBody is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnOPPNew_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    errorLog.WorkProcessLog("-------------Opportunity New XML Started--------------", "OppNew");
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        errorLog.WorkProcessLog("Unable to connect Archive server.");
            //        return;
            //    }

            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPOPPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();

            //    DataSet DSOppId = new DataSet();

            //    DataTable DT = new DataTable();
            //    #region
            //    DataSet DSchkAw45 = new DataSet();
            //    Int64 OPPCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DSchkAw45 = objXML.GetOpportunityCount(1);


            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Opportunitys with Fundingbody status 45. Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Opportunities with Fundingbody . Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }

            //    OPPCOUNT = Convert.ToInt64((DSchkAw45).Tables["OpportunityCount"].Rows[0]["Count"]);
            //    #endregion

            //    errorLog.WorkProcessLog("Opp Count : " + OPPCOUNT);
            //    if (OPPCOUNT > 0)
            //    {
            //        DSOppId = objXML.GetOpportunityIDList();
            //        Int64 Pagecount = 1;
            //        if (OPPCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = OPPCOUNT / MaxXMLCount;
            //            if (OPPCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            errorLog.WorkProcessLog("Start fetch data from database");
            //            DT = objXML.GetOpportunity(1, XsdPath, Start, End);
            //            errorLog.WorkProcessLog("Data Recived from database and XML created");
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }


            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            errorLog.WorkProcessLog("Error in XML : " + xmlerrmsg);
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(3, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 1);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }

            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(3, 1, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (!FTPUpload(3, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);
            //                ClearZipFromUser();
            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        #region Added for change history ----24-07-2017

            //                        for (int i = 0; i < DSOppId.Tables["QCCompleteopp"].Rows.Count; i++)
            //                        {
            //                            string fdghgf = DSOppId.Tables["QCCompleteopp"].Rows[i]["ID"].ToString();
            //                            DataSet dsresult = dalopp.SaveAndUpdateChangeHistory(null, Convert.ToInt64(DSOppId.Tables["QCCompleteopp"].Rows[i]["ID"]), "update", null, null, 1, 0);
            //                        }

            //                        #endregion
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        errorLog.WorkProcessLog("Xml Deliverd successfully.");
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No new Opportunity is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnAWNew_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    errorLog.WorkProcessLog("-------------Award New Statred--------------", "AWNew");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPAWPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    #region
            //    Int64 AWDCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DataSet DSchkAw45 = new DataSet();
            //    DSchkAw45 = objXML.GetAwardCount(1);
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Awards with Fundingbody status 45. Error : AwID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Awrads with Fundingbody . Error : Awards_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    AWDCOUNT = Convert.ToInt64((DSchkAw45).Tables["AwardCount"].Rows[0]["Count"]);
            //    #endregion
            //    // Int64 AWDCOUNT = Convert.ToInt64(((DataSet)objXML.GetAwardCount(1)).Tables["AwardCount"].Rows[0]["Count"]);
            //    if (AWDCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (AWDCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = AWDCOUNT / MaxXMLCount;
            //            if (AWDCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetAward(1, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }

            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        objXMLGenerate = new DAL.XMLGenerate(ref Cn);
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(4, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 1);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(4, 1, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);

            //                try
            //                {
            //                    if (!FTPUpload(4, Zippath))
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Rollback();

            //                    }

            //                    TreeNode TN = new TreeNode();
            //                    Int64 flg = 0;
            //                    foreach (DataTable DTtree in DSXML.Tables)
            //                    {
            //                        if (flg == 0)
            //                        {
            //                            TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                            flg = 1;
            //                        }
            //                        TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                        TN.Nodes.Add(TNC);
            //                    }
            //                    TreeView1.Nodes.Add(TN);



            //                    ClearZipFromUser();
            //                }
            //                catch (Exception ex)
            //                {
            //                    if (Zippath == "" || Zippath == null)
            //                    {
            //                        errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                        HideReportPopup();
            //                        oleTrnas.Rollback();
            //                        MessageBox.Show("SORRY! we are not able to deliver the XML cause of some error in XML.\nSee Error Log.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        //oleTrnas.Rollback();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }

            //            }
            //            if (DSXMLErr.Tables.Count > 0)
            //            {
            //                String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //                xmlerrmsg.Replace("<Error>", "");
            //                xmlerrmsg.Replace("</Error>", "");
            //                HideReportPopup();
            //                oleTrnas.Rollback();
            //                MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show("SORRY! we are not able to deliver the XML.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No new Award is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        #region Update XML

        protected void btnFBUpdate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    errorLog.WorkProcessLog("-------------Funding Body update Statred--------------", "FBUpdate");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPFBPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    Int64 FBCOUNT = Convert.ToInt64(((DataSet)objXML.GetFundingCount(2)).Tables["FundingBodyCount"].Rows[0]["Count"]);
            //    if (FBCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (FBCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = FBCOUNT / MaxXMLCount;
            //            if (FBCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetFundingBody(2, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);

            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }

            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(2, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 2);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(2, 2, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (Zippath == "zeero")
            //                {
            //                    MessageBox.Show("Xml is not deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //                else
            //                {
            //                    if (!FTPUpload(2, Zippath))
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Rollback();

            //                    }
            //                    TreeNode TN = new TreeNode();
            //                    Int64 flg = 0;
            //                    foreach (DataTable DTtree in DSXML.Tables)
            //                    {
            //                        if (flg == 0)
            //                        {
            //                            TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                            flg = 1;
            //                        }
            //                        TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                        TN.Nodes.Add(TNC);
            //                    }
            //                    TreeView1.Nodes.Add(TN);




            //                    ClearZipFromUser();

            //                    try
            //                    {
            //                        DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                        if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                        {
            //                            HideReportPopup();
            //                            oleTrnas.Commit();
            //                            MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                        HideReportPopup();
            //                        oleTrnas.Rollback();
            //                        MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No Updated FundingBody is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnOPPUpdate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    errorLog.WorkProcessLog("-------------Oppourtunity update Statred--------------", "OppUpdate");
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        errorLog.WorkProcessLog("Unable to connect Archive server.");
            //        return;
            //    }

            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPOPPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    DataSet DSOppId = new DataSet();
            //    #region
            //    DataSet DSchkAw45 = new DataSet();
            //    Int64 OPPCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DSchkAw45 = objXML.GetOpportunityCount(2);
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Opportunitys with Fundingbody status 45. Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Opportunities with Fundingbody . Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    //Int64 AWDCOUNT = Convert.ToInt64(((DataSet)objXML.GetAwardCount(2)).Tables["AwardCount"].Rows[0]["Count"]);
            //    OPPCOUNT = Convert.ToInt64((DSchkAw45).Tables["OpportunityCount"].Rows[0]["Count"]);
            //    #endregion

            //    // Int64 OPPCOUNT = Convert.ToInt64(((DataSet)objXML.GetOpportunityCount(2)).Tables["OpportunityCount"].Rows[0]["Count"]);
            //    errorLog.WorkProcessLog("Opportunity Count= " + OPPCOUNT.ToString());
            //    if (OPPCOUNT > 0)
            //    {
            //        DSOppId = objXML.GetOpportunityIDList();

            //        Int64 Pagecount = 1;
            //        if (OPPCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = OPPCOUNT / MaxXMLCount;
            //            if (OPPCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            errorLog.WorkProcessLog("Get data from database and create XML");
            //            DT = objXML.GetOpportunity(2, XsdPath, Start, End);
            //            errorLog.WorkProcessLog("XLM Created");
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }


            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {

            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            errorLog.WorkProcessLog("Error in XLM : " + xmlerrmsg);
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(3, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 2);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(3, 2, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {

            //                errorLog.WorkProcessLog("xmlid,xmlname,zipid,zipname created");
            //                Zippath = WriteXmlfile(DSXML);



            //                if (!FTPUpload(3, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);


            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        #region Added for change history ----24-07-2017

            //                        for (int i = 0; i < DSOppId.Tables["QCCompleteopp"].Rows.Count; i++)
            //                        {
            //                            string fdghgf = DSOppId.Tables["QCCompleteopp"].Rows[i]["ID"].ToString();
            //                            DataSet dsresult = dalopp.SaveAndUpdateChangeHistory(null, Convert.ToInt64(DSOppId.Tables["QCCompleteopp"].Rows[i]["ID"]), "update", null, null, 1, 0);
            //                        }

            //                        #endregion
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        errorLog.WorkProcessLog("Xml Deliverd successfully.");
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No Updated Opportunity is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnAWUpdate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    errorLog.WorkProcessLog("-------------Award update Statred--------------", "AWUpdate");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPAWPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    #region
            //    Int64 AWDCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DataSet DSchkAw45 = new DataSet();
            //    DSchkAw45 = objXML.GetAwardCount(2);
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Awards with Fundingbody status 45. Error : AwID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Awrads with Fundingbody . Error : Awrads_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    AWDCOUNT = Convert.ToInt64((DSchkAw45).Tables["AwardCount"].Rows[0]["Count"]);
            //    #endregion
            //    //Int64 AWDCOUNT = Convert.ToInt64(((DataSet)objXML.GetAwardCount(2)).Tables["AwardCount"].Rows[0]["Count"]);
            //    if (AWDCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (AWDCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = AWDCOUNT / MaxXMLCount;
            //            if (AWDCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetAward(2, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }

            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(4, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 2);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(4, 2, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (!FTPUpload(4, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);



            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No Updated Award is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }
        #endregion

        #region Delete XML
        protected void btnFBDel_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    errorLog.WorkProcessLog("-------------Funding body Delete Statred--------------", "FBDelete");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPFBPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    Int64 FBCOUNT = Convert.ToInt64(((DataSet)objXML.GetFundingCount(3)).Tables["FundingBodyCount"].Rows[0]["Count"]);
            //    if (FBCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (FBCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = FBCOUNT / MaxXMLCount;
            //            if (FBCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetFundingBody(3, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }

            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(2, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 3);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(2, 3, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (!FTPUpload(2, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }
            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);




            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No Deleted FundingBody is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnOPPDel_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    errorLog.WorkProcessLog("-------------Oppourtunity Delete Statred--------------", "OppDelete");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();


            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPOPPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    #region
            //    DataSet DSchkAw45 = new DataSet();
            //    Int64 OPPCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DSchkAw45 = objXML.GetOpportunityCount(3);
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Opportunitys with Fundingbody status 45. Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Opportunities with Fundingbody . Error : OPP_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    //Int64 AWDCOUNT = Convert.ToInt64(((DataSet)objXML.GetAwardCount(2)).Tables["AwardCount"].Rows[0]["Count"]);
            //    OPPCOUNT = Convert.ToInt64((DSchkAw45).Tables["OpportunityCount"].Rows[0]["Count"]);
            //    #endregion
            //    // Int64 OPPCOUNT = Convert.ToInt64(((DataSet)objXML.GetOpportunityCount(3)).Tables["OpportunityCount"].Rows[0]["Count"]);
            //    if (OPPCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (OPPCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = OPPCOUNT / MaxXMLCount;
            //            if (OPPCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetOpportunity(3, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }
            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(3, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 3);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(3, 3, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (!FTPUpload(3, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);



            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No Deleted Opportunity is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        protected void btnAWDel_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!CheckArchiveServer())
            //    {
            //        MessageBox.Show("Unable to connect Archive server.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    errorLog.WorkProcessLog("-------------Award Delete Statred--------------", "AWDelete");
            //    proStart = new Thread(new ThreadStart(showReportPopup));
            //    proStart.Start();

            //    string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPAWPath"]);
            //    ftp.FtpClient(serverip, userid, pass, remoteloc);
            //    DataSet DSXML = new DataSet();
            //    DataSet DSXMLErr = new DataSet();
            //    DataTable DT = new DataTable();
            //    #region
            //    Int64 AWDCOUNT = 0;
            //    string chk_Errmsg_45 = "";
            //    DataSet DSchkAw45 = new DataSet();
            //    DSchkAw45 = objXML.GetAwardCount(3);
            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "2")
            //    {
            //        chk_Errmsg_45 = "Error occurred during generation of list of Awards with Fundingbody status 45. Error : AwID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }

            //    if (Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["error"]) == "3")
            //    {
            //        chk_Errmsg_45 = "Conflict in Fundingbody Extended Status Please Check   list of Awrads with Fundingbody . Error : Awrads_ID In " + Convert.ToString((DSchkAw45).Tables["ERRORCODE"].Rows[0]["ErrorMessage"]);
            //        MessageBox.Show(chk_Errmsg_45, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;

            //    }
            //    AWDCOUNT = Convert.ToInt64((DSchkAw45).Tables["AwardCount"].Rows[0]["Count"]);
            //    #endregion
            //    //Int64 AWDCOUNT = Convert.ToInt64(((DataSet)objXML.GetAwardCount(3)).Tables["AwardCount"].Rows[0]["Count"]);
            //    if (AWDCOUNT > 0)
            //    {
            //        Int64 Pagecount = 1;
            //        if (AWDCOUNT > MaxXMLCount)
            //        {
            //            Pagecount = AWDCOUNT / MaxXMLCount;
            //            if (AWDCOUNT % MaxXMLCount != 0)
            //            {
            //                Pagecount++;
            //            }
            //        }
            //        Int64 Start = 0;
            //        Int64 End = 0;
            //        for (Int32 count = 1; count <= Pagecount; count++)
            //        {
            //            Start = End + 1;
            //            End = count * MaxXMLCount;
            //            DT = objXML.GetAward(3, XsdPath, Start, End);
            //            if (DT.Rows[0][0].ToString().IndexOf("<Error>") != 0)
            //            {
            //                DSXML.Tables.Add(DT);
            //            }
            //            else
            //            {
            //                DSXMLErr.Tables.Add(DT);
            //            }
            //        }

            //        if (Cn.State == ConnectionState.Closed) Cn.Open();
            //        oleTrnas = Cn.BeginTransaction();
            //        if (DSXMLErr.Tables.Count > 0)
            //        {
            //            String xmlerrmsg = DSXMLErr.Tables[0].Rows[0][0].ToString();
            //            xmlerrmsg.Replace("<Error>", "");
            //            xmlerrmsg.Replace("</Error>", "");
            //            HideReportPopup();

            //            oleTrnas.Rollback();
            //            MessageBox.Show(xmlerrmsg, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            objXML.CompleteDelivery();
            //            return;
            //        }
            //        DataSet xmldtl = new DataSet();
            //        StringBuilder xmlids = new StringBuilder();
            //        string sep = "";
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            xmldtl = objXMLGenerate.XMLArchieve(4, DSXML.Tables[xmlcount].Rows[0][0].ToString(), Convert.ToInt64(DAL.SharedClass.USERID), DSXML.Tables[xmlcount].Rows[0][1].ToString(), 3);
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("xmlid");
            //                DSXML.Tables[xmlcount].Columns.Add("xmlname");
            //                DSXML.Tables[xmlcount].Rows[0]["xmlid"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"];
            //                DSXML.Tables[xmlcount].Rows[0]["xmlname"] = xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLNAME"];
            //                xmlids.Append(sep);
            //                xmlids.Append(xmldtl.Tables["XMLDETAILS"].Rows[0]["XMLID"].ToString());
            //                sep = ",";
            //            }
            //        }
            //        DataSet zipdtl = new DataSet();
            //        if (xmldtl.Tables.Count > 1)
            //        {
            //            zipdtl = objXMLGenerate.ZipArchieve(4, 3, Convert.ToInt64(DAL.SharedClass.USERID), xmlids.ToString());
            //        }
            //        for (Int32 xmlcount = 0; xmlcount < DSXML.Tables.Count; xmlcount++)
            //        {
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                DSXML.Tables[xmlcount].Columns.Add("zipid");
            //                DSXML.Tables[xmlcount].Columns.Add("zipname");
            //                DSXML.Tables[xmlcount].Rows[0]["zipid"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"];
            //                DSXML.Tables[xmlcount].Rows[0]["zipname"] = zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"];
            //            }
            //        }

            //        try
            //        {
            //            string Zippath = string.Empty;// Request.PhysicalApplicationPath + "XMLZip/" + zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPNAME"].ToString();
            //            if (xmldtl.Tables.Count > 1)
            //            {
            //                Zippath = WriteXmlfile(DSXML);
            //                if (!FTPUpload(4, Zippath))
            //                {
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();

            //                }

            //                TreeNode TN = new TreeNode();
            //                Int64 flg = 0;
            //                foreach (DataTable DTtree in DSXML.Tables)
            //                {
            //                    if (flg == 0)
            //                    {
            //                        TN = new TreeNode(DTtree.Rows[0]["ZIPNAME"].ToString());
            //                        flg = 1;
            //                    }
            //                    TreeNode TNC = new TreeNode(DTtree.Rows[0]["XMLNAME"].ToString());
            //                    TN.Nodes.Add(TNC);
            //                }
            //                TreeView1.Nodes.Add(TN);




            //                ClearZipFromUser();

            //                try
            //                {
            //                    DataSet dsApp = objXMLGenerate.ConfirmDelivery(Convert.ToInt64(zipdtl.Tables["ZIPDETAILS"].Rows[0]["ZIPID"]), Convert.ToInt64(DAL.SharedClass.USERID));

            //                    if (Convert.ToString(dsApp.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Commit();
            //                        MessageBox.Show("Xml Deliverd successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //                    HideReportPopup();
            //                    oleTrnas.Rollback();
            //                    MessageBox.Show("Error in FTP Upload. //n " + ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            HideReportPopup();
            //            errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //            oleTrnas.Rollback();
            //            MessageBox.Show(ex.Message, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        HideReportPopup();
            //        MessageBox.Show("No deleted Award is avilable for XMl Delivery.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            //}
            //finally
            //{
            //    Cn.Close();
            //    objXML.CompleteDelivery();
            //}
        }

        #endregion

        #region XML Save And Zip Creation

        private string WriteXmlfile(DataSet xmldetails)
        {
            XmlDocument Xdoc = new XmlDocument();

            try
            {
                string Zippath = string.Empty;
                string tempPath = System.IO.Path.GetTempPath();
                string xmlFileName = string.Empty;

                if (!Directory.Exists(tempPath + "XMLZip"))
                    Directory.CreateDirectory(tempPath + "XMLZip");

                string path = tempPath + "XMLZip";

                foreach (DataTable DT in xmldetails.Tables)
                {
                    if (!Directory.Exists(tempPath + "XMLZip\\" + DT.Rows[0]["ZIPID"].ToString()))
                    {
                        Directory.CreateDirectory(tempPath + "XMLZip\\" + DT.Rows[0]["ZIPID"].ToString());
                        path = tempPath + "XMLZip\\" + DT.Rows[0]["ZIPID"].ToString();
                    }
                    else
                        path = tempPath + "XMLZip\\" + DT.Rows[0]["ZIPID"].ToString();

                    Xdoc.LoadXml(DT.Rows[0]["XML"].ToString());
                    string xmlpath = path + "\\" + DT.Rows[0]["XMLNAME"].ToString();
                    xmlFileName = DT.Rows[0]["XMLNAME"].ToString();
                    Zippath = path + "\\" + DT.Rows[0]["ZIPNAME"].ToString();

                    string[] existFiles = Directory.GetFiles(path, "*.xml");

                    if (existFiles.Length > 0)
                    {
                        for (int delCount = 0; delCount < existFiles.Length; delCount++)
                            File.Delete(existFiles[delCount]);
                    }

                    if (Directory.Exists(path + "\\fp"))
                    {
                        string[] existFpFiles = Directory.GetFiles(path + "\\fp", "*.xml");

                        if (existFpFiles.Length > 0)
                            for (int delCount = 0; delCount < existFpFiles.Length; delCount++)
                                File.Delete(existFpFiles[delCount]);
                    }

                    if (File.Exists(xmlpath))
                        File.Delete(xmlpath);

                    Xdoc.Save(xmlpath);

                    errorLog.WorkProcessLog("Xml Transform started");
                    //DAL.Transform XmlTransform = new DAL.Transform(tempPath + "XMLZip\\" + DT.Rows[0]["ZIPID"].ToString() + "\\");

                    //if (!XmlTransform.TransformXml(DT.Rows[0]["XMLNAME"].ToString()))
                    //{
                    //    errorLog.WorkProcessLog("Transformation Error: Error In Transformation.");
                    //    throw new Exception("Transformation Error: Error In Transformation.");
                    //}
                }

                string[] errorFilePath = path.Replace("XMLZip", "$").Split('$');
                string newErrorFilePath = errorFilePath[1].Replace("\\", "");
                string XMLFilePath = errorFilePath[0] + "XMLZip\\fp_" + newErrorFilePath + "_errorlog\\" + xmlFileName.Split('.')[0] + "_log.xml";// + "\\" + DT.Rows[0]["xmlid"].ToString() + "_log.xml";
                string XMLFilePathVale = path + "\\" + xmlFileName;

                string xmlerror = GetVal("error", "position", XMLFilePath, XMLFilePathVale);

                if (xmlerror.ToString() == "zeero" || xmlerror.ToString() == "")
                {
                    errorLog.WorkProcessLog("XML is error free and ready for delivery");

                    checksum = GetMD5HashFromFile(XMLFilePathVale);

                    bool flagCheckum = false;
                    XmlDataDocument xmldoc = new XmlDataDocument();
                    string fpFilePath = path + @"\fp\" + xmlFileName.Split('.')[0] + Convert.ToString(ConfigurationSettings.AppSettings["fpFile"]); ;
                    
                    using (FileStream fs = File.Open(fpFilePath, FileMode.Open, FileAccess.Read))
                    {
                        xmldoc.Load(fs);
                        XmlNodeList node = xmldoc.GetElementsByTagName("checksum");
                        string fpfilechecksum = node.Item(0).InnerText;
                        
                        if (checksum == fpfilechecksum)
                            flagCheckum = true;
                        else
                        {
                            flagCheckum = false;
                            Zippath = "1";
                        }
                    }
                    if (flagCheckum == true)
                    {
                        if (File.Exists(Zippath))
                            File.Delete(Zippath);

                        ZipFiles(path, Zippath);
                    }
                }
                else
                {
                    Zippath = null;
                    File.WriteAllText(errorFilePath[0] + "XMLZip\\fp_" + newErrorFilePath + "_errorlog\\xmlerrorlog.txt", xmlerror);
                }

                return Zippath;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
                errorLog.WorkProcessLog("Something went worng..See log file");
                throw new Exception("FP log xml Not Copied.. Please try again..");
            }
        }

        public static String GetMD5HashFromFile(String fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));

            return sb.ToString();
        }

        public string GetVal(string AttributeName1, string AttributeName2, string XMLFilePath, string XMLFilePath123)
        {
            errorLog.WorkProcessLog("Start Reading log file");
            FileInfo f = new FileInfo(XMLFilePath);
            long s1 = f.Length;

            if (s1 > 0)
                errorLog.WorkProcessLog("Log completed copy");
            else
                errorLog.WorkProcessLog("Log not completed copy");

            string attrVal1 = "";
            string attrVal = "";
            string test = "";
            string test12 = "";
            string errorPosition = "";
            string error = "";
            string ErrorIDName = "";
            XmlDocument doc = new XmlDocument();
            string fpath = Path.GetFullPath(XMLFilePath);
            doc.Load(fpath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLFilePath123);
            XmlNodeList elemList = doc.GetElementsByTagName("message");
            for (int i = 0; i < elemList.Count; i++)
            {

                if (elemList[i].Attributes[AttributeName2] != null)
                {
                    if (elemList[i].Attributes["type"].Value == "error")
                    {
                        attrVal1 = elemList[i].Attributes[AttributeName2].Value;
                        string[] attrVal11 = attrVal1.Split(':');
                        //string XMLFilePath123 = "C:\\Temp\\XMLZip\\gwd_nAW501000002049.xml";
                        XNode x = FindNode(XMLFilePath123, Convert.ToInt32(attrVal11[0]), Convert.ToInt32(attrVal11[1]));
                        if (error == "" || error == null)
                        {
                            error = error + elemList[i].InnerText;
                            if (x.NodeType.ToString() == "Text")
                            {
                                while (x.Parent.Name.LocalName != "award" || x.Parent.Name.LocalName != "opportunity" || x.Parent.Name.LocalName != "fundingBody")
                                {

                                    if (x.Parent.Name.LocalName == "award")
                                    {
                                        ErrorIDName = "Award";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    if (x.Parent.Name.LocalName == "opportunity")
                                    {
                                        ErrorIDName = "Opportunity";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    if (x.Parent.Name.LocalName == "fundingBody")
                                    {
                                        ErrorIDName = "FundingBody";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    x = x.Parent;
                                }
                            }
                            else
                            {
                                if (((System.Xml.Linq.XElement)(x)).Name.LocalName == "award" || ((System.Xml.Linq.XElement)(x)).Name.LocalName == "opportunity" || ((System.Xml.Linq.XElement)(x)).Name.LocalName == "fundingBody")
                                {
                                    if (((System.Xml.Linq.XElement)(x)).Name.LocalName == "award")
                                    {
                                        ErrorIDName = "Award";
                                        test12 = ((System.Xml.Linq.XElement)(x)).FirstAttribute.Value.ToString();
                                        //break;
                                    }
                                    if (((System.Xml.Linq.XElement)(x)).Name.LocalName == "opportunity")
                                    {
                                        ErrorIDName = "Opportunity";
                                        test12 = ((System.Xml.Linq.XElement)(x)).FirstAttribute.Value.ToString();
                                        //break;
                                    }
                                    if (((System.Xml.Linq.XElement)(x)).Name.LocalName == "fundingBody")
                                    {
                                        ErrorIDName = "FundingBody";
                                        test12 = ((System.Xml.Linq.XElement)(x)).FirstAttribute.Value.ToString();
                                        //break;
                                    }
                                }
                                else
                                {
                                    while (x.Parent.Name.LocalName != "award" || x.Parent.Name.LocalName != "opportunity" || x.Parent.Name.LocalName != "fundingBody")
                                    {

                                        if (x.Parent.Name.LocalName == "award")
                                        {
                                            ErrorIDName = "Award";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        if (x.Parent.Name.LocalName == "opportunity")
                                        {
                                            ErrorIDName = "Opportunity";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        if (x.Parent.Name.LocalName == "fundingBody")
                                        {
                                            ErrorIDName = "FundingBody";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        x = x.Parent;
                                    }
                                }
                            }
                            error = error + " in " + ErrorIDName + " ID: " + test12 + ".\n";

                        }
                        else
                        {
                            //try
                            //{
                            // if (test12 != x.Parent.Parent.Parent.FirstAttribute.Value.ToString())
                            string parentVal = "";



                            //if (x.ToString().ToLower()!="citizenship")
                            if (x.Parent.Parent.Parent.HasAttributes)
                            {
                                parentVal = Convert.ToString(x.Parent.Parent.Parent.FirstAttribute.Value);
                            }
                            else
                            {
                                parentVal = Convert.ToString(x.Parent.Parent.Parent.Parent.FirstAttribute.Value);
                            }
                            if (test12 != parentVal)
                            {
                                //error = error + " in " + ErrorIDName + " ID: " + test12 + ". ";
                                //error = error + " " + elemList[i].InnerText;
                                while (x.Parent.Name.LocalName != "award" || x.Parent.Name.LocalName != "opportunity" || x.Parent.Name.LocalName != "fundingBody")
                                {
                                    if (x.Parent.Name.LocalName == "award")
                                    {
                                        ErrorIDName = "Award";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    if (x.Parent.Name.LocalName == "opportunity")
                                    {
                                        ErrorIDName = "Opportunity";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    if (x.Parent.Name.LocalName == "fundingBody")
                                    {
                                        ErrorIDName = "FundingBody";
                                        test12 = x.Parent.FirstAttribute.Value.ToString();
                                        break;
                                    }
                                    x = x.Parent;
                                }
                                error = error + " " + elemList[i].InnerText + " in " + ErrorIDName + " ID: " + test12 + ".\n";
                            }
                            else
                            {
                                if (!Regex.IsMatch(error, elemList[i].InnerText, RegexOptions.IgnoreCase))
                                    error = error + " " + elemList[i].InnerText;
                                if (!Regex.IsMatch(test12, x.Parent.Parent.Parent.FirstAttribute.Value.ToString(), RegexOptions.IgnoreCase))
                                {
                                    while (x.Parent.Name.LocalName != "award" || x.Parent.Name.LocalName != "opportunity" || x.Parent.Name.LocalName != "fundingBody")
                                    {
                                        if (x.Parent.Name.LocalName == "award")
                                        {
                                            ErrorIDName = "Award";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        if (x.Parent.Name.LocalName == "opportunity")
                                        {
                                            ErrorIDName = "Opportunity";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        if (x.Parent.Name.LocalName == "fundingBody")
                                        {
                                            ErrorIDName = "FundingBody";
                                            test12 = x.Parent.FirstAttribute.Value.ToString();
                                            break;
                                        }
                                        x = x.Parent;
                                    }
                                    error = error + " " + elemList[i].InnerText + " in " + ErrorIDName + " ID: " + test12 + ".\n";
                                }
                            }
                        }
                    }
                }
            }
            attrVal1 = error;
            if (attrVal1 == "")
                return "zeero";
            else
                return attrVal1;
        }

        XNode FindNode(string path, int line, int column)
        {
            XDocument doc = XDocument.Load(path, LoadOptions.SetLineInfo);
            var query =
                from node in doc.DescendantNodes()
                let lineInfo = (IXmlLineInfo)node
                where lineInfo.LineNumber == line
                && lineInfo.LinePosition <= column
                select node;
            return query.LastOrDefault();
        }

        private static string GetCommandLineArugments(string[] args)
        {
            string retVal = string.Empty;

            foreach (string arg in args)
                retVal += " " + arg;

            return retVal;
        }

        public static void ZipFiles(string inputFolderPath, string outputPathAndFile)
        {
            try
            {
                ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
                int TrimLength = inputFolderPath.Length;//(Directory.GetParent(inputFolderPath)).ToString().Length;
                // find number of chars to remove     // from orginal file path
                TrimLength += 1; //remove '\'
                FileStream ostream = null;
                byte[] obuffer;
                string outPath = outputPathAndFile; //inputFolderPath +(@"\" + outputPathAndFile;
                ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
                //if (password != null && password != String.Empty)
                //    oZipStream.Password = password;
                oZipStream.SetLevel(9); // maximum compression
                ZipEntry oZipEntry;
                foreach (string Fil in ar) // for each file, generate a zipentry
                {
                    oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                    oZipStream.PutNextEntry(oZipEntry);

                    if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                    {
                        ostream = File.OpenRead(Fil);
                        obuffer = new byte[ostream.Length];
                        ostream.Read(obuffer, 0, obuffer.Length);
                        oZipStream.Write(obuffer, 0, obuffer.Length);
                    }
                }
                oZipStream.Finish();
                oZipStream.Close();
                ostream.Close();
                ostream.Dispose();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static ArrayList GenerateFileList(string Dir)
        {
            ArrayList fils = new ArrayList();
            try
            {

                bool Empty = true;
                foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
                {
                    fils.Add(file);
                    Empty = false;
                }

                if (Empty)
                {
                    if (Directory.GetDirectories(Dir).Length == 0)
                    // if directory is completely empty, add it
                    {
                        fils.Add(Dir + @"/");
                    }
                }

                foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
                {
                    foreach (object obj in GenerateFileList(dirs))
                    {
                        fils.Add(obj);
                    }
                }

            }
            catch
            {
                throw;
            }
            return fils; // return file list
        }

        #endregion

        #region Check Archive

        public bool CheckArchiveServer()
        {
            bool flag = false;
            try
            {
                ftp.Login();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }

            return flag;
        }

        public void ClearZipFromUser()
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                /////////////////////// delete this segment on delivery time /////////////////
                //tempPath = System.IO.Path.
                //string[] s1 = tempPath.Split(':');
                tempPath = "D:";
                if (Directory.Exists(tempPath + "\\TestScivalVtool"))
                {
                    Directory.CreateDirectory(tempPath + "\\TestScivalVtool");
                }
                else
                    tempPath = tempPath + "\\TestScivalVtool";

                ////////////////////////////////////////////////////////////////


                if (Directory.Exists(tempPath + "\\XMLZip"))
                {
                    Directory.Delete(tempPath + "\\XMLZip", true);
                    // true for delete all subdirectiry and sub files
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                throw;
            }
        }

        #endregion

        #region User Defined Functions

        private void GenerateXML_Load(object sender, EventArgs e)
        {
            String path = Application.StartupPath;
            pnlContent.Top = (this.Height - pnlContent.Height) / 2;
            pnlbtn.Left = (this.Width - pnlbtn.Width - 20);

            btnHome.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
            btnsignout.BackgroundImage = Image.FromFile(path + "\\Images\\gray_b.png");
        }

        private void GenerateXML_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void loadInitialValue()
        {
            try
            {
                MaxXMLCount = Convert.ToInt64(ConfigurationSettings.AppSettings["XMLMaxValue"]);

                XsdPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\XSD";

                serverip = Convert.ToString(ConfigurationSettings.AppSettings["FTPip"]);
                userid = Convert.ToString(ConfigurationSettings.AppSettings["FTPUserName"]);
                pass = Convert.ToString(ConfigurationSettings.AppSettings["FTPPwd"]);
                string remoteloc = Convert.ToString(ConfigurationSettings.AppSettings["FTPPath"]);
                ftp.FtpClient(serverip, userid, pass, remoteloc);

                string ArchiveIP = Convert.ToString(ConfigurationSettings.AppSettings["Archive"]);
                string ArchiveUserd = Convert.ToString(ConfigurationSettings.AppSettings["ArchiveUserName"]);
                string Archivepass = Convert.ToString(ConfigurationSettings.AppSettings["Pwd"]);
                string Archiveloc = Convert.ToString(ConfigurationSettings.AppSettings["Path"]);

                ftp.FtpClient(ArchiveIP, ArchiveUserd, Archivepass, Archiveloc);

                try
                {
                    //if (DAL.SharedClass.ModuleID != null)
                    //{
                    //    moduleId = Convert.ToInt64(DAL.SharedClass.ModuleID);

                    //}

                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                    throw;
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                throw;
            }
        }

        public void showReportPopup()
        {
            try
            {

                if (xmlProcess.Visible == true)
                {
                    xmlProcess.Close();
                }
                xmlProcess.Visible = false;
                xmlProcess.ShowDialog();

            }
            catch (ThreadAbortException ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                Thread.ResetAbort();
            }
            catch (SystemException exp)
            {
                errorLog.WriteErrorLog(exp);//For Error Logs Created By Rantosh 28/07/2016
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            }
        }

        public void HideReportPopup()
        {
            try
            {
                if (proStart.IsAlive)
                {
                    try
                    {
                        proStart.Abort();
                        Thread.Sleep(100);
                    }
                    catch (ThreadAbortException ex)
                    {
                        errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                        Thread.ResetAbort();
                    }
                    catch (SystemException exp)
                    {
                        errorLog.WriteErrorLog(exp);//For Error Logs Created By Rantosh 28/07/2016
                    }
                    catch (Exception ex)
                    {
                        errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
                    }
                }

            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            }
        }

        private void btnsignout_Click(object sender, EventArgs e)
        {
            try
            {
                Application.OpenForms["GenerateXML"].Close();
                //Application.Restart();
                Application.OpenForms["Login"].Show();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                //DAL.SharedClass.TaskBoard = null;
                Application.OpenForms["GenerateXML"].Dispose();

                TaskBoard taskobj = new TaskBoard();
                taskobj.Show();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (TreeView1.SelectedNode != null)
            {
                if (!TreeView1.SelectedNode.Text.Contains("zip"))
                    showXML(TreeView1.SelectedNode.Text.ToString(), TreeView1.SelectedNode.Parent.Text.ToString());
            }
        }

        public void showXML(string xmlName, string ZipName)
        {
            try
            {
                //gwd_nFB501000000008.xml
                string foldername = ZipName.Substring(7, ZipName.Length - 7).Replace(".zip", "");
                string path = foldername + "\\" + xmlName;

                ShowXml oobj = new ShowXml(path);
                oobj.ShowDialog();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);//For Error Logs Created By Rantosh 28/07/2016
            }
        }

        #endregion

        #region FTP Upload

        private Boolean FTPUpload(Int32 ModuleId, String ZipPath)
        {
            //if (ZipPath != null)
            //{
            //    if (ZipPath == "1")
            //    {
            //        HideReportPopup();
            //        oleTrnas.Rollback();
            //        MessageBox.Show("SORRY! we are not able to deliver the XML cause of FINGERPRINT_CHECKSUM_MISMATCH.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //    else
            //    {
            //        DataSet FTP = objXML.GetFTPDetails(ModuleId);
            //        String HOST = String.Empty;
            //        String USERID = String.Empty;
            //        String PASSWORD = String.Empty;
            //        String PATH = String.Empty;
            //        if (FTP.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
            //        {
            //            if (FTP.Tables["FTPDETAILS"].Rows.Count > 0)
            //            {
            //                for (int x = 0; x < FTP.Tables["FTPDETAILS"].Rows.Count; x++)
            //                {
            //                    try
            //                    {
            //                        HOST = FTP.Tables["FTPDETAILS"].Rows[x]["HOST"].ToString();
            //                        USERID = FTP.Tables["FTPDETAILS"].Rows[x]["USERID"].ToString();
            //                        PASSWORD = FTP.Tables["FTPDETAILS"].Rows[x]["PASSWORD"].ToString();
            //                        PATH = FTP.Tables["FTPDETAILS"].Rows[x]["PATH"].ToString();
            //                        ftp = new DAL.FTPClass();
            //                        ftp.FtpClient(HOST, USERID, PASSWORD, PATH);
            //                        ftp.Upload(ZipPath);
            //                        ftp.Close();
            //                        Thread.Sleep(500);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        HideReportPopup();
            //                        oleTrnas.Rollback();
            //                        MessageBox.Show(ex.Message, FTP.Tables["FTPDETAILS"].Rows[x]["HOST"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                        return false;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                HideReportPopup();
            //                oleTrnas.Rollback();
            //                MessageBox.Show("No Active FTP Found.", "FTP ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            HideReportPopup();
            //            oleTrnas.Rollback();
            //            MessageBox.Show("Error in Fetching FTP Details. " + FTP.Tables["ERRORCODE"].Rows[0][1].ToString(), "FTP ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return false;
            //        }
            //    }
            //}
            //else
            //{
            //    HideReportPopup();
            //    oleTrnas.Rollback();
            //    MessageBox.Show("SORRY! we are not able to deliver the XML cause of some error in XML.\nSee Error Log.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            return true;
        }

        #endregion

        public string GetBatfile()
        {
            String XsdPath = String.Empty;
            XsdPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\BAT";
            return XsdPath;
        }


        private void btnFixLimit_Click(object sender, EventArgs e)
        {
            ////pankaj 
            //if (txtgenrationLimit.Text.Length > 0)
            //{
            //    Int64 XMLgenratioLimit = 0;
            //    XMLgenratioLimit = Convert.ToInt64(txtgenrationLimit.Text);
            //    DataSet dsresult = objXML.SaveXMLgenrationLimit(XMLgenratioLimit, 0);


            //    if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
            //    {
            //        //lblCurrentLimit.Text = Convert.ToString(dsresult.Tables["p_limits"].Rows[0][0]);

            //        MessageBox.Show(Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        txtgenrationLimit.Text = "";
            //        //MessageBox.Show("XMl genration Limit change successfully!.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        //return;
            //        this.Refresh();
            //        DataSet dsgetresult = objXML.GetXMLgenrationLimit(0, 1);
            //        lblCurrentLimits.Text = Convert.ToString(dsgetresult.Tables["ERRORCODE"].Rows[0][1]);
            //    }
            //}

        }

        public static void POST(string url, string jsonContent)
        {
            string ddd = "";
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ingestion.staging.fundingdiscovery.com/ingestion/funding-body");
                //curl -X POST "https://ingestion.staging.fundingdiscovery.com/ingestion/funding-body" -H "accept: application/json" -H "callbackUrl: http://ingestion.staging.fundingdiscovery.com/ingestion/funding-body" -H "Content-Type: application/json"
                //   httpWebRequest.Headers.Add("curl -X POST" ,"https://ingestion.staging.fundingdiscovery.com/ingestion/funding-body");
                httpWebRequest.Headers.Add("callbackUrl: http://ingestion.staging.fundingdiscovery.com/ingestion/funding-body");
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequest.Proxy = WebRequest.DefaultWebProxy;
                httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = jsonContent;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var _dataResponse = JToken.Parse(JsonConvert.SerializeObject((HttpWebResponse)httpWebRequest.GetResponse()));

                WebResponse webResponse = httpWebRequest.GetResponse();
                var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                ddd = ex.Message.ToString();
                var sss = (HttpWebResponse)ex.Response;

            }
        }


    }
}
