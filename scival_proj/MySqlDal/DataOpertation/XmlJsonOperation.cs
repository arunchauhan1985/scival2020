﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newtonsoft.Json.Schema;
using System.Web;
using MySqlDal;
using DAL.JsonModel;
using DAL;
using System.Linq.Expressions;
using MySqlDal.DataOpertation;

namespace MySqlDalAL
{
    public class XmlJsonOperation
    {
        #region Variable Declaration
        Int64 ErrorsCount = 0;
        Replace r = new Replace();

        string ErrorMessage = "";
        string Error_text = "JSON file genrate successfully for FB Module! & File copied in temp Folder";
        string Error_text_deatils = "JSON file genrate successfully for OPP Module! & File copied in temp Folder";
        string Error_text_deatils_p = "JSON file genrate successfully for Awards Module and Publication Module! & File copied in temp Folder";
        #endregion

        #region Variable Dec s5
        DataView dataView = new DataView();
        DataSet dsFB = new DataSet();
        //  DataTable dtFundingBody = new DataTable();
        DataSet Source1 = new DataSet();


        DataTable dt_website = new DataTable();
        DataTable dt_contact = new DataTable();

        DataTable dt_address = new DataTable();

        DataTable dt_fundingPolicy = new DataTable();

        DataTable dt_fundingPolicy_Deatails = new DataTable();

        DataTable dt_createddate = new DataTable();

        DataTable dt_reviseddate = new DataTable();

        DataTable dt_revisionhistory = new DataTable();

        DataTable dt_link = new DataTable();

        DataTable dt_OPPORTUNITIESSOURCE = new DataTable();

        DataTable dt_publicationDataset = new DataTable();

        DataTable dt_awardSSOURCE = new DataTable();

        DataTable dt_fundingBodyDataset = new DataTable();

        DataTable dt_fundingdescription = new DataTable();

        DataTable dt_awardSuccessRatedesc = new DataTable();

        DataTable dt_releatedorgs = new DataTable();
        #endregion

        #region Variable opp
        DataSet Source = new DataSet();
        DataSet dsOPP = new DataSet();
        DataTable dtOpportunity = new DataTable();
        DataTable dt_contactName = new DataTable();
        DataTable dt_opportunityLocation = new DataTable();

        DataTable dt_Keyword = new DataTable();
        DataTable dt_title = new DataTable();

        DataTable dt_synopsis = new DataTable();

        DataTable dt_description = new DataTable();

        DataTable dt_subjectMatter = new DataTable();

        DataTable dt_ELIGIBILITYDESCRIPTION = new DataTable();

        DataTable dt_Duration = new DataTable();

        DataTable dt_ReleatedFunder = new DataTable();

        DataTable dt_classfication = new DataTable();

        DataTable dt_hasSubject = new DataTable();

        DataTable dt_instruction = new DataTable();

        DataTable dt_licenseInformation = new DataTable();

        DataTable dt_associatedAmount_desc = new DataTable();

        DataTable dt_ceiling = new DataTable();

        DataTable dt_estimatedTotal = new DataTable();

        DataTable dt_floor = new DataTable();

        DataTable dt_relatedTo = new DataTable();

        DataTable dt_replacedBy = new DataTable();

        DataTable dt_replaces = new DataTable();
        DataTable dt_grantType = new DataTable();


        DataTable dt_limitedSubmission = new DataTable();
        DataTable dt_individualEligibility = new DataTable();
        DataTable dt_organisationEligibility = new DataTable();
        DataTable dt_citizenship = new DataTable();
        DataTable dt_regionSpecific = new DataTable();
        DataTable dt_restrictionScope = new DataTable();

        DataTable dt_opportunityDate = new DataTable();
        DataTable dt_cycle = new DataTable();
        DataTable dt_expirationDateDetail = new DataTable();
        DataTable dt_decision = new DataTable();
        DataTable dt_startDateDetail = new DataTable();
        DataTable dt_endDateDetail = new DataTable();
        DataTable dt_letterOfIntent = new DataTable();
        DataTable dt_preproposal = new DataTable();
        DataTable dt_proposal = new DataTable();

        DataTable dt_homePage = new DataTable();

        #endregion

        #region //Data table Declaration for Award JSON
        DataSet Source_AwJson = new DataSet();
        DataTable dtAw_Award = new DataTable();
        DataTable dtAw_acronym = new DataTable();
        DataTable dtAw_homePage = new DataTable();
        DataTable dtAw_funds = new DataTable();
        DataTable dtAw_AwardeeDetail = new DataTable();
        DataTable dtAw_AffiliationDetail = new DataTable();
        DataTable dtAw_AwardeeAddress = new DataTable();
        DataTable dt_AwardeeIdentityFier = new DataTable();
        DataTable dtAw_Title = new DataTable();
        DataTable dtAw_Synopsis = new DataTable();
        DataTable dtAW_Keyword = new DataTable();
        DataTable dtAw_FundingDetail = new DataTable();
        DataTable dtAw_Classification = new DataTable();
        DataTable dtAw_RelatedOpportunity = new DataTable();
        DataTable dtAw_RelatedFunder = new DataTable();
        DataTable dtAw_Reviseddate = new DataTable();
        DataTable dtAw_Createddate = new DataTable();
        DataTable dtAw_LeadFunder = new DataTable();
        DataTable dtAw_hasFunder = new DataTable();
        // DataTable dtAw_funds = new DataTable();
        DataTable dtAw_haspart_funds = new DataTable();
        DataTable dtAw_titleFunds = new DataTable();
        DataTable dtAw_RevisedDate = new DataTable();
        DataTable dtAw_revisionhistory = new DataTable();
        DataTable dtAw_createddate = new DataTable();
        DataTable dtAw_Replication = new DataTable();
        DataTable dt_AwlicenseInformation = new DataTable();
        #endregion

        private DataSet GetOppotunityXSDSchema(String XsdPath)
        {
            try
            {
                //String Path = XsdPath + "\\opportunities_ForFillData3.0.xsd";
                String Path = XsdPath + "\\opportunities_ForFillData4.1.xsd";
                DataSet FBSCHEMA = new DataSet();
                FBSCHEMA.ReadXmlSchema(Path);
                DataRelation DR = FBSCHEMA.Tables["opportunities"].ChildRelations[0];
                FBSCHEMA.Tables["opportunities"].ChildRelations.Remove(DR);
                FBSCHEMA.Tables["opportunity"].Constraints.Remove("opportunities_opportunity");//
                FBSCHEMA.Tables.Remove("opportunities");
                FBSCHEMA.Tables["opportunity"].Columns.Remove("opportunities_id");
                //FBSCHEMA.WriteXmlSchema(Path.Replace(".xsd", "_2.xsd"));
                return FBSCHEMA;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #region post json method
        public void PostJsonfile(string jsonfile)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ingestion.staging.fundingdiscovery.com/ingestion/funding-body");
                httpWebRequest.Headers.Add("callbackUrl: http://ingestion.staging.fundingdiscovery.com/ingestion/funding-body");
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequest.Proxy = WebRequest.DefaultWebProxy;
                httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials; ;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = @"D:\jsonfile\501100001777_FB.json";
                    string json = jsonfile;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                // var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var _dataResponse = JToken.Parse(JsonConvert.SerializeObject((HttpWebResponse)httpWebRequest.GetResponse()));
                //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    var result = streamReader.ReadToEnd();
                //}
            }
            catch (WebException ex)
            {
                var stream = ex.Response.GetResponseStream();
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
                var _dataResponse = JToken.Parse(JsonConvert.SerializeObject((HttpWebResponse)ex.Response));
                var resp = (HttpWebResponse)ex.Response;

                throw;
            }

        }


        #endregion

        #region Get JSON Data
        public string PopulateJsonData(string json, string elementName)
        {
            string updatedjson = string.Empty;
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
               
            }
            return updatedjson;
        }
        #endregion

        #region Funding Body JSON Formation for FB  SCHEMA 5.0 development

        public string JsonCreationFromModel_FB(string XSDPath, DataTable dtFundingBody, DataTable dt_preferredorgname, DataTable dt_contextname, DataTable dt_abbrevname, DataTable dt_acronym, DataTable dt_subType, DataTable dt_identifier, DataTable dt_fundingdescription, DataTable dt_website, DataTable dt_establishmentInfo, DataTable dt_address, DataTable dt_awardSuccessRatedesc, DataTable dt_revisionhistory, DataTable dt_createddate, DataTable dt_reviseddate)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("lang");
                dt.Columns.Add("value");
                DataRow dr = dt.NewRow();
                dr["lang"] = "EN";
                dr["value"] = "Test";
                dt.Rows.Add(dr);
                DataRow dr1 = dt.NewRow();
                dr1["lang"] = "EN1";
                dr1["value"] = "Test1";
                dt.Rows.Add(dr1);
                DataRow dr2 = dt.NewRow();
                dr2["lang"] = "EN2";
                dr2["value"] = "Test2";
                dt.Rows.Add(dr2);

                FB_JSON_Model fb = new FB_JSON_Model();
                fb.status = dtFundingBody.Rows.Count > 0 && dtFundingBody.Rows[0]["status"].ToString().Trim() != "" ? dtFundingBody.Rows[0]["status"].ToString().ToUpper() : "Notavailable_Status";
                fb.preferredName = (from DataRow dr3 in dt_preferredorgname.Rows
                                    select new preferredName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["preferredorgname_text"].ToString().Trim()
                                    }).ToList();

                fb.acronym = (from DataRow dr3 in dt_acronym.Rows
                              select new acronym()
                              {
                                  language = dr3["acronym_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                  value = dr3["acronym_text"].ToString().Trim()
                              }).ToList();

                fb.abbrevName = (from DataRow dr3 in dt_abbrevname.Rows
                                 select new abbrevName()
                                 {
                                     language = dr3["abbrevname_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                     value = dr3["abbrevname_text"].ToString().Trim()
                                 }).ToList();
                fb.alternateName = (from DataRow dr3 in dt_contextname.Rows
                                    select new alternateName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["contextname_text"].ToString().Trim()
                                    }).ToList();
                fb.profitabilityType = Convert.ToString(dtFundingBody.Rows[0]["profit"].ToString().ToUpper());
                fb.activityType = Convert.ToString(dt_subType.Rows[0]["id"].ToString().ToUpper());
                fb.country = Convert.ToString(dtFundingBody.Rows[0]["country"].ToString().ToLower());
                fb.state = Convert.ToString(dtFundingBody.Rows[0]["state"].ToString().ToUpper());
                fb.identifier = (from DataRow dridf in dt_identifier.Rows
                                 select new identifier()
                                 {
                                     type = dridf["type"].ToString(),
                                     value = dridf["value"].ToString()
                                 }).ToList();
                fb.description = (from DataRow dr3 in dt_fundingdescription.Rows
                                  select new description()
                                  {
                                      abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                      source = dr3["URL"].ToString()
                                  }).ToArray();


                fb.awardSuccessRate = new awardSuccessRate()
                {
                    description = (from DataRow dr3 in dt_awardSuccessRatedesc.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                       source = dr3["URL"].ToString()
                                   }).ToArray(),

                    //percentage = 99
                    percentage = dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString() != "" ? Convert.ToInt32(Convert.ToDouble(dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString())) : 0,
                };
                if (dt_website.Rows.Count > 0)
                {
                    fb.contactInformation = new contactInformation()
                    {
                        link = Convert.ToString(dt_website.Rows[0]["url"].ToString()),

                        hasPostalAddress = new hasPostalAddress()
                        {
                            addressCountry = dt_address.Rows[0]["COUNTRYTEST"].ToString(),
                            addressLocality = dt_address.Rows[0]["city"].ToString() != "Not Available" && dt_address.Rows[0]["city"].ToString().Trim() != "" ? dt_address.Rows[0]["city"].ToString().Trim() : dt_address.Rows[0]["COUNTRY"].ToString(),

                            // addressLocality = "Not Available1",
                            addressPostalCode = dt_address.Rows[0]["postalcode"].ToString(),
                            postOfficeBoxNumber = dt_address.Rows[0]["room"].ToString(),
                            //addressRegion = dt_address.Rows[0]["country"].ToString(),
                            addressRegion = dt_address.Rows[0]["state"].ToString(),
                            streetAddress = dt_address.Rows[0]["street"].ToString(),
                        }
                    };
                }

                fb.establishment = new establishment()
                {
                    establishmentYear = dt_establishmentInfo.Rows.Count > 0 ? (dt_establishmentInfo.Rows[0]["ESTABLISHMENTDATE"].ToString() != "" ? Convert.ToInt32(dt_establishmentInfo.Rows[0]["ESTABLISHMENTDATE"].ToString()) : -99999999) : -99999999,
                    //establishmentYear = 1990,
                    country = dt_establishmentInfo.Rows.Count > 0 ? dt_establishmentInfo.Rows[0]["ESTABLISHMENTCOUNTRYCODE"].ToString() : "",

                    description = (from DataRow dr3 in dt_establishmentInfo.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["ESTABLISHMENTDESCRIPTION"].ToString() },
                                       //26nov 2019//source = dr3["URL"].ToString()
                                       source = dtFundingBody.Rows[0]["recordSource"].ToString()
                                   }).ToArray()

                };


                fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["ORGDBID"].ToString());
                // fb.fundingBodyId = Convert.ToInt64(dtAw_Replication.Rows[0]["Replicated_Id"].ToString());

                fb.financeType = Convert.ToString(dtFundingBody.Rows[0]["type"].ToString().ToUpper());

                fb.homePage = dtFundingBody.Rows[0]["recordSource"].ToString().Trim();
                //fb.homePage = dtAw_Replication.Rows[0]["Rep_Url"].ToString();

                fb.abbrevName = (from DataRow dr3 in dt_abbrevname.Rows
                                 select new abbrevName()
                                 {
                                     language = dr3["lang"].ToString(),
                                     value = dr3["abbrevname_text"].ToString().Trim()
                                 }).ToList();
                try
                {
                    fb.hasProvenance = new hasProvenance()
                    {
                        contactPoint = "fundingprojectteam@aptaracorp.com",
                        //contactPoint = "support@elsevier.com",
                        createdOn = dt_createddate.Rows[0]["CREATEDDATE_TEXT"].ToString(),
                        //defunct = dtFundingBody.Rows[0]["defunct"].ToString() !=""? Convert.ToBoolean( dtFundingBody.Rows[0]["defunct"].ToString()) : false,
                        defunct = false,
                        derivedFrom = dtFundingBody.Rows[0]["RECORDSOURCE"].ToString(),
                        hidden = dtFundingBody.Rows[0]["hidden"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["hidden"].ToString()) : false,
                        lastUpdateOn = dt_reviseddate.Rows[0]["REVISEDDATE_TEXT"].ToString(),
                        status = dt_revisionhistory.Rows[0]["status"].ToString().ToUpper(),
                        //lastUpdateOn = "",
                        //version = dt_reviseddate.Rows[0]["version"].ToString(),
                        version = dt_reviseddate.Rows.Count > 0 ? dt_reviseddate.Rows[0]["VERSION"].ToString() : dt_createddate.Rows[0]["version"].ToString(),
                        //version = "",
                        //wasAttributedTo = dtFundingBody.Rows[0]["collectioncode"].ToString(),
                        wasAttributedTo = "SUP002",
                    };
                }
                catch (Exception ex)
                {

                }
                fb.fundingPolicy = (from DataRow dr3 in dt_fundingPolicy_Deatails.Rows
                                    select new fundingPolicy()
                                    {
                                        abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                        source = dr3["URL"].ToString()
                                    }).ToArray();
                try
                {
                    fb.registry = new registry()
                    {
                        fundingBodyDataset = new fundingBodyDataset()
                        {

                            //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                            collectionCode = "SUP002",
                            extended = dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()) : true,
                            tier = dtFundingBody.Rows[0]["TIERINFO"].ToString() != "" ? Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()) : 1,
                            #region Later Uncomment it
                            //source = (from DataRow dr3 in dt_fundingBodyDataset.Rows
                            //          select new source()
                            //          {
                            //              name = dr3["name"].ToString(),
                            //              url = dr3["url"].ToString(),
                            //              status = dr3["status"].ToString().ToUpper(),
                            //              frequency = dr3["frequency"].ToString().ToUpper(),
                            //              captureStart = dr3["captureStart"].ToString(),
                            //              //captureEnd = dr3["captureEnd"].ToString(),
                            //              captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),

                            //              //comment = dr3["FB_COMMENT"].ToString()
                            //              comment = dr3["FB_COMMENT"].ToString() == "" ? "" : dr3["FB_COMMENT"].ToString()

                            //          }).ToArray()
                            #endregion Later Uncomment it

                        },

                        opportunityDataset = new opportunityDataset()
                        {
                            //capture = "false",
                            capture = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString()) : false,
                            collectionCode = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",

                            #region Later Uncomment it
                            //source = (from DataRow dr3 in dt_OPPORTUNITIESSOURCE.Rows
                            //          select new source()
                            //          {
                            //              name = dr3["name"].ToString().Trim() == "" ? "Funder Website" : dr3["name"].ToString(),
                            //              url = dr3["url"].ToString(),
                            //              status = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString().ToLower() == "true" ? "ACTIVE" : dr3["status"].ToString().ToUpper(),
                            //              //frequency = dr3["frequency"].ToString().ToUpper(),
                            //              frequency = "SIGNAL-BASED",
                            //              captureStart = dr3["captureStart"].ToString(),
                            //              //captureStart = dr3["created_date"].ToString(),
                            //              //captureEnd = dr3["captureEnd"].ToString(),
                            //              captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                            //              //captureEnd = dr3["lastvisited"].ToString() == "" ? "" : dr3["lastvisited"].ToString(),
                            //              //comment = dr3["OPP_COMMENT"].ToString()
                            //              comment = dr3["OPP_COMMENT"].ToString() == "" ? "" : dr3["OPP_COMMENT"].ToString()
                            //          }).ToArray()
                            #endregion Later Uncomment it
                        },

                        awardDataset = new awardDataset()
                        {
                            //capture = "false",
                            capture = dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString()) : false,
                            collectionCode = dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",
                            #region Later Uncomment it
                            //source = (from DataRow dr3 in dt_awardSSOURCE.Rows
                            //          select new source()
                            //          {
                            //              name = dr3["name"].ToString().Trim() == "" ? "Funder Website" : dr3["name"].ToString(),
                            //              url = dr3["url"].ToString(),
                            //              status = dr3["status"].ToString().ToUpper(),
                            //              //frequency = dr3["frequency"].ToString().ToUpper(),
                            //              frequency = "SIGNAL-BASED",
                            //              captureStart = dr3["captureStart"].ToString(),
                            //              // captureStart = dr3["created_date"].ToString(),
                            //              //captureEnd = dr3["captureEnd"].ToString(),
                            //              captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                            //              // captureEnd = dr3["lastvisited"].ToString() == "" ? "" : dr3["lastvisited"].ToString(),
                            //              //comment = dr3["AW_COMMENT"].ToString()
                            //              comment = dr3["AW_COMMENT"].ToString() == "" ? "" : dr3["AW_COMMENT"].ToString()
                            //          }).ToArray()
                            #endregion
                        },


                        publicationDataset = new publicationDataset()
                        {
                            //capture = "false",
                            capture = dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString()) : false,
                            //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                            collectionCode = dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",

                            //collectionCode = "SUP002",
                            //extended = Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()),
                            //tier = Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()),
                            #region Later Uncomment it
                            //source = (from DataRow dr3 in dt_publicationDataset.Rows
                            //          select new source()
                            //          {
                            //              name = dr3["name"].ToString(),
                            //              url = dr3["url"].ToString(),
                            //              status = dr3["status"].ToString().ToUpper(),
                            //              frequency = dr3["frequency"].ToString().ToUpper(),
                            //              captureStart = dr3["captureStart"].ToString(),
                            //              //captureEnd = dr3["captureEnd"].ToString(),
                            //              captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                            //              //comment = dr3["PUB_COMMENT"].ToString()
                            //              comment = dr3["PUB_COMMENT"].ToString() == "" ? "" : dr3["PUB_COMMENT"].ToString()
                            //          }).ToArray()
                            #endregion Later Uncomment it
                        },

                    };
                }
                catch (Exception ex)
                {

                }

                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                settings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(fb, Newtonsoft.Json.Formatting.None, settings);

                settings.NullValueHandling = NullValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(fb, settings);

                #region
                myJson = myJson.Replace("Not Available", "");
                myJson = myJson.Replace("abstracts", "abstract");
                myJson = myJson.Replace("--Select Defunct--", "false");
                myJson = myJson.Replace("--Select Defunct--", "false");
                myJson = myJson.Replace("-99999999", "");

                #endregion

                #region for Handling Empty array and values
                for (int k = 0; k <= 10; k++)
                {
                    myJson = Remove_NullObjects(myJson);

                    myJson = myJson.Replace("\"\",", "");

                    myJson = myJson.Replace("\"\"", "");
                }
                json = myJson.ToString();
                #endregion

                #region//For Default Values
                json = json.Replace("NotAvailable_Locality", "Not Available");
                json = json.Replace("Notavailable_Status", "Not Available");

                #endregion

                json = Regex.Replace(json, @"\t|\n|\r|\r\n", "");

                json = json.Trim();

                string json_1 = Regex.Replace(json, @"\t|\n|\r|\r\n", "");
                json = Regex.Replace(json, @"\r\n?|\n", "");
                json = json.Trim();
                #region
                json = json.Replace(Environment.NewLine, "").Replace("&#x00a0;", " ").Replace("\r", "").Replace("\n", "");
                #region Hex Values which have to replaced into space on 15-Mar-2019

                json = json.Replace("&#x00A0;", " ");
                json = json.Replace("&#x2002;", " ");
                json = json.Replace("&#x2003;", " ");
                json = json.Replace("&#x2004;", " ");
                json = json.Replace("&#x2005;", " ");
                json = json.Replace("&#x2006;", " ");
                json = json.Replace("&#x2007;", " ");
                json = json.Replace("&#x2008;", " ");
                json = json.Replace("&#x2009;", " ");
                json = json.Replace("&#x200A;", " ");
                json = json.Replace("&#x200B;", " ");
                json = json.Replace("&#x3000;", " ");
                json = json.Replace("&#xFEFF;", " ");
                #endregion
                json = Sgml_to_Hexadecimal(json, XSDPath);
                string json1 = Utf_to_Html(json);
                json = anyToHex(json1);
                #endregion

                #region Hex Values which have to replaced into space on 15-Mar-2019
                json = json.Replace("&nbsp;", " ");
                json = json.Replace("&#x00A0;", " ");
                json = json.Replace("&#x00a0;", " ");
                json = json.Replace("&#x2002;", " ");
                json = json.Replace("&#x2003;", " ");
                json = json.Replace("&#x2004;", " ");
                json = json.Replace("&#x2005;", " ");
                json = json.Replace("&#x2006;", " ");
                json = json.Replace("&#x2007;", " ");
                json = json.Replace("&#x2008;", " ");
                json = json.Replace("&#x2009;", " ");
                json = json.Replace("&#x200A;", " ");
                json = json.Replace("&#x200B;", " ");
                json = json.Replace("&#x3000;", " ");
                json = json.Replace("&#xFEFF;", " ");
                json = json.Replace("&#x202f;", " ");
                json = json.Replace("&#x22;", "\"");

                json = json.Replace("&#x0022;", "\"");
                json = json.Replace("&#x0008;", " ");
                json = json.Replace("&#x000C;", " ");
                json = json.Replace("&#x000A;", " ");
                json = json.Replace("&#x000D;", " ");
                json = json.Replace("&#x0009;", " ");

                json = json.Replace("&#x000c;", " ");
                json = json.Replace("&#x000a;", " ");
                json = json.Replace("&#x000d;", " ");

                json = json.Replace("  ", " ");
                json = json.Replace("   ", " ");
                json = json.Replace("   ", " ");
                json = json.Replace("&lrm;", "&#x200E;");
                json = json.Replace(" & ", "&#x200E;");
                json = json.Replace("&amp;nbsp;", " ");
                json = json.Replace("&#x0026;nbsp", " ");
                json = json.Replace("&nbsp", "");
                json = json.Replace("  ", " ");
                json = json.Replace("   ", " ");

                json = json.Replace("&amp;#x", "&#x");
                json = json.Replace("&amp;#", "&#");
                json = json.Replace("&amp;lt;", "&lt;");
                json = json.Replace("&amp;gt;", "&gt;");
                json = json.Replace("&AMP ;", "");
                #endregion


                updatedjson = json;

                //string is_valid = SchemaValidation(updatedjson, "FundingBody", XSDPath);

                if (json.Length > 0)
                {


                    if (!(Directory.Exists(@"C:\Temp\VtoolFB")))
                    {
                        Directory.CreateDirectory(@"C:\Temp\VtoolFB");
                    }
                    string error = "";
                    fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["ORGDBID"].ToString());

                    string FBID = fb.fundingBodyId.ToString();

                    File.AppendAllText(@"C:\Temp\VtoolFB\FB_NDJSON_File_.txt", updatedjson + "\r\n");
                }
                else
                {

                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                }
                return updatedjson;
            }
            catch (Exception ex)
            {
                return ex.Message;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    //oErrorLog.WriteErrorLog(ex);
                    //oErrorLog.WorkProcessLog("Something went worng.");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
            #region Post Json File on client server
            //PostJsonfile(json);

            //Console.WriteLine(json);
            #endregion Post Json File
        }

        #endregion

        public string Add_Items_JSON(string XSDPath, DataTable dt_item, DataTable dtFundingBody, string inputJson, long pagemode)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                FB_JSON_Model fb = new FB_JSON_Model();
                FB_JSON_Model dataJSON = JsonConvert.DeserializeObject<FB_JSON_Model>(inputJson);

                #region Page Mode 1
                if (pagemode == 1)
                {
                    fb.description = (from DataRow dr3 in dt_item.Rows
                                      select new description()
                                      {
                                          abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                          source = dr3["URL"].ToString()
                                      }).ToArray();
                    var existingData = dataJSON.description;
                    int countItem = existingData.Count();
                    dataJSON.description = fb.description;
                }
                #endregion

                #region Page Mode 4
                if (pagemode == 4)
                {
                    fb.awardSuccessRate = new awardSuccessRate()
                    {
                        description = (from DataRow dr3 in dt_item.Rows
                                       select new description()
                                       {
                                           abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                           source = dr3["URL"].ToString()
                                       }).ToArray(),

                        //percentage = 99
                        percentage = dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString() != "" ? Convert.ToInt32(dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString()) : 0,
                    };
                    var existingData = dataJSON.awardSuccessRate;
                    dataJSON.awardSuccessRate = fb.awardSuccessRate;
                }
                #endregion

                string newJson = JsonConvert.SerializeObject(dataJSON);
                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                settings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(dataJSON, Newtonsoft.Json.Formatting.None, settings);

                settings.NullValueHandling = NullValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(dataJSON, settings);
                return GetJSONFB(myJson, json, XSDPath, updatedjson, dtFundingBody, dataJSON);
            }
            catch (Exception ex)
            {
                return inputJson;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        public string saveEstablishmentInfo(string XSDPath, DataTable dt_item, DataTable dtFundingBody, string inputJson)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                FB_JSON_Model fb = new FB_JSON_Model();
                FB_JSON_Model dataJSON = JsonConvert.DeserializeObject<FB_JSON_Model>(inputJson);

                fb.establishment = new establishment()
                {
                    establishmentYear = dt_item.Rows.Count > 0 ? (dt_item.Rows[0]["ESTABLISHMENTDATE"].ToString() != "" ? Convert.ToInt32(dt_item.Rows[0]["ESTABLISHMENTDATE"].ToString()) : -99999999) : -99999999,
                    //establishmentYear = 1990,
                    country = dt_item.Rows.Count > 0 ? dt_item.Rows[0]["ESTABLISHMENTCOUNTRYCODE"].ToString() : "",

                    description = (from DataRow dr3 in dt_item.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["ESTABLISHMENTDESCRIPTION"].ToString() },
                                       //26nov 2019//source = dr3["URL"].ToString()
                                       source = dtFundingBody.Rows[0]["recordSource"].ToString()
                                   }).ToArray()

                };
                dataJSON.establishment = fb.establishment;
                string newJson = JsonConvert.SerializeObject(dataJSON);
                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                settings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(dataJSON, Newtonsoft.Json.Formatting.None, settings);

                settings.NullValueHandling = NullValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(dataJSON, settings);
                return GetJSONFB(myJson, json, XSDPath, updatedjson, dtFundingBody, dataJSON);
            }
            catch (Exception ex)
            {
                return inputJson;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        public string saveContactInfo(string XSDPath, DataTable dt_item, DataTable dtFundingBody, string inputJson)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                FB_JSON_Model fb = new FB_JSON_Model();
                FB_JSON_Model dataJSON = JsonConvert.DeserializeObject<FB_JSON_Model>(inputJson);

                fb.contactInformation = new contactInformation()
                {
                    link = Convert.ToString(dt_website.Rows[0]["url"].ToString()),

                    hasPostalAddress = new hasPostalAddress()
                    {
                        addressCountry = dt_item.Rows[0]["COUNTRYTEST"].ToString(),
                        addressLocality = dt_item.Rows[0]["city"].ToString() != "Not Available" && dt_item.Rows[0]["city"].ToString().Trim() != "" ? dt_item.Rows[0]["city"].ToString().Trim() : dt_item.Rows[0]["COUNTRY"].ToString(),

                        // addressLocality = "Not Available1",
                        addressPostalCode = dt_item.Rows[0]["postalcode"].ToString(),
                        postOfficeBoxNumber = dt_item.Rows[0]["room"].ToString(),
                        //addressRegion = dt_address.Rows[0]["country"].ToString(),
                        addressRegion = dt_item.Rows[0]["state"].ToString(),
                        streetAddress = dt_item.Rows[0]["street"].ToString(),
                    }
                };
                dataJSON.contactInformation = fb.contactInformation;
                string newJson = JsonConvert.SerializeObject(dataJSON);
                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                settings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(dataJSON, Newtonsoft.Json.Formatting.None, settings);

                settings.NullValueHandling = NullValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(dataJSON, settings);
                return GetJSONFB(myJson, json, XSDPath, updatedjson, dtFundingBody, dataJSON);
            }
            catch (Exception ex)
            {
                return inputJson;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        public string JsonCreationFromModel_FundingBody(string XSDPath, DataTable dtFundingBody, DataTable dt_preferredorgname, DataTable dt_contextname, DataTable dt_abbrevname, DataTable dt_acronym, DataTable dt_subType, DataTable dt_identifier, DataTable dt_fundingdescription, DataTable dt_website, DataTable dt_establishmentInfo, DataTable dt_address, DataTable dt_awardSuccessRatedesc, DataTable dt_revisionhistory, DataTable dt_createddate, DataTable dt_reviseddate)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("lang");
                dt.Columns.Add("value");
                DataRow dr = dt.NewRow();
                dr["lang"] = "EN";
                dr["value"] = "Test";
                dt.Rows.Add(dr);
                DataRow dr1 = dt.NewRow();
                dr1["lang"] = "EN1";
                dr1["value"] = "Test1";
                dt.Rows.Add(dr1);
                DataRow dr2 = dt.NewRow();
                dr2["lang"] = "EN2";
                dr2["value"] = "Test2";
                dt.Rows.Add(dr2);

                FB_JSON_Model fb = new FB_JSON_Model();
                fb.status = dtFundingBody.Rows.Count > 0 && dtFundingBody.Rows[0]["status"].ToString().Trim() != "" ? dtFundingBody.Rows[0]["status"].ToString().ToUpper() : "Notavailable_Status";
                fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["FUNDINGBODY_ID"].ToString());
                // fb.fundingBodyId = Convert.ToInt64(dtAw_Replication.Rows[0]["Replicated_Id"].ToString());

                fb.financeType = Convert.ToString(dtFundingBody.Rows[0]["type"].ToString().ToUpper());

                fb.homePage = dtFundingBody.Rows[0]["recordSource"].ToString().Trim();
                //fb.homePage = dtAw_Replication.Rows[0]["Rep_Url"].ToString();
                fb.preferredName = (from DataRow dr3 in dt_preferredorgname.Rows
                                    select new preferredName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["preferredorgname_text"].ToString().Trim()
                                    }).ToList();

                fb.acronym = (from DataRow dr3 in dt_acronym.Rows
                              select new acronym()
                              {
                                  language = dr3["acronym_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                  value = dr3["acronym_text"].ToString().Trim()
                              }).ToList();

                fb.abbrevName = (from DataRow dr3 in dt_abbrevname.Rows
                                 select new abbrevName()
                                 {
                                     language = dr3["abbrevname_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                     value = dr3["abbrevname_text"].ToString().Trim()
                                 }).ToList();
                fb.alternateName = (from DataRow dr3 in dt_contextname.Rows
                                    select new alternateName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["contextname_text"].ToString().Trim()
                                    }).ToList();
                fb.profitabilityType = Convert.ToString(dtFundingBody.Rows[0]["profit"].ToString().ToUpper());
                fb.activityType = Convert.ToString(dt_subType.Rows[0]["id"].ToString().ToUpper());
                fb.country = Convert.ToString(dtFundingBody.Rows[0]["country"].ToString().ToLower());
                fb.state = Convert.ToString(dtFundingBody.Rows[0]["state"].ToString().ToUpper());
                
                fb.identifier = (from DataRow dridf in dt_identifier.Rows
                                 select new identifier()
                                 {
                                     type = dridf["type"].ToString(),
                                     value = dridf["value"].ToString()
                                 }).ToList();

               

                fb.awardSuccessRate = new awardSuccessRate()
                {
                    description = (from DataRow dr3 in dt_awardSuccessRatedesc.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                       source = dr3["URL"].ToString()
                                   }).ToArray(),

                    //percentage = 99
                    percentage = dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString() != "" ? Convert.ToInt32(Convert.ToDouble(dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString())) : 0,
                };
                fb.hasProvenance = new hasProvenance()
                {
                    contactPoint = "fundingprojectteam@aptaracorp.com",
                    //contactPoint = "support@elsevier.com",
                    createdOn = dt_createddate.Rows[0]["CREATEDDATE_TEXT"].ToString(),
                    //defunct = dtFundingBody.Rows[0]["defunct"].ToString() !=""? Convert.ToBoolean( dtFundingBody.Rows[0]["defunct"].ToString()) : false,
                    defunct = false,
                    derivedFrom = dtFundingBody.Rows[0]["RECORDSOURCE"].ToString(),
                    hidden = dtFundingBody.Rows[0]["hidden"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["hidden"].ToString()) : false,
                    lastUpdateOn = dt_reviseddate.Rows[0]["REVISEDDATE_TEXT"].ToString(),
                    status = dt_revisionhistory.Rows[0]["status"].ToString().ToUpper(),
                    //lastUpdateOn = "",
                    //version = dt_reviseddate.Rows[0]["version"].ToString(),
                    version = dt_reviseddate.Rows.Count > 0 ? dt_reviseddate.Rows[0]["VERSION"].ToString() : dt_createddate.Rows[0]["version"].ToString(),
                    //version = "",
                    //wasAttributedTo = dtFundingBody.Rows[0]["collectioncode"].ToString(),
                    wasAttributedTo = "SUP002",
                };


                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                settings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(fb, Newtonsoft.Json.Formatting.None, settings);

                settings.NullValueHandling = NullValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(fb, settings);
                return GetJSONFB(myJson, json, XSDPath, updatedjson, dtFundingBody, fb);
            }
            catch (Exception ex)
            {
                return ex.Message;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        public string GetJSONFB(string myJson, string json, string XSDPath, string updatedjson, DataTable dtFundingBody, FB_JSON_Model fb)
        {
            #region
            myJson = myJson.Replace("Not Available", "");
            myJson = myJson.Replace("abstracts", "abstract");
            myJson = myJson.Replace("--Select Defunct--", "false");
            myJson = myJson.Replace("--Select Defunct--", "false");
            myJson = myJson.Replace("-99999999", "");

            #endregion

            #region for Handling Empty array and values
            for (int k = 0; k <= 10; k++)
            {
                myJson = Remove_NullObjects(myJson);

                myJson = myJson.Replace("\"\",", "");

                myJson = myJson.Replace("\"\"", "");
            }
            json = myJson.ToString();
            #endregion

            #region//For Default Values
            json = json.Replace("NotAvailable_Locality", "Not Available");
            json = json.Replace("Notavailable_Status", "Not Available");

            #endregion

            json = Regex.Replace(json, @"\t|\n|\r|\r\n", "");

            json = json.Trim();

            string json_1 = Regex.Replace(json, @"\t|\n|\r|\r\n", "");
            json = Regex.Replace(json, @"\r\n?|\n", "");
            json = json.Trim();
            #region
            json = json.Replace(Environment.NewLine, "").Replace("&#x00a0;", " ").Replace("\r", "").Replace("\n", "");
            #region Hex Values which have to replaced into space on 15-Mar-2019

            json = json.Replace("&#x00A0;", " ");
            json = json.Replace("&#x2002;", " ");
            json = json.Replace("&#x2003;", " ");
            json = json.Replace("&#x2004;", " ");
            json = json.Replace("&#x2005;", " ");
            json = json.Replace("&#x2006;", " ");
            json = json.Replace("&#x2007;", " ");
            json = json.Replace("&#x2008;", " ");
            json = json.Replace("&#x2009;", " ");
            json = json.Replace("&#x200A;", " ");
            json = json.Replace("&#x200B;", " ");
            json = json.Replace("&#x3000;", " ");
            json = json.Replace("&#xFEFF;", " ");
            #endregion
            json = Sgml_to_Hexadecimal(json, XSDPath);
            string json1 = Utf_to_Html(json);
            json = anyToHex(json1);
            #endregion

            #region Hex Values which have to replaced into space on 15-Mar-2019
            json = json.Replace("&nbsp;", " ");
            json = json.Replace("&#x00A0;", " ");
            json = json.Replace("&#x00a0;", " ");
            json = json.Replace("&#x2002;", " ");
            json = json.Replace("&#x2003;", " ");
            json = json.Replace("&#x2004;", " ");
            json = json.Replace("&#x2005;", " ");
            json = json.Replace("&#x2006;", " ");
            json = json.Replace("&#x2007;", " ");
            json = json.Replace("&#x2008;", " ");
            json = json.Replace("&#x2009;", " ");
            json = json.Replace("&#x200A;", " ");
            json = json.Replace("&#x200B;", " ");
            json = json.Replace("&#x3000;", " ");
            json = json.Replace("&#xFEFF;", " ");
            json = json.Replace("&#x202f;", " ");
            json = json.Replace("&#x22;", "\"");

            json = json.Replace("&#x0022;", "\"");
            json = json.Replace("&#x0008;", " ");
            json = json.Replace("&#x000C;", " ");
            json = json.Replace("&#x000A;", " ");
            json = json.Replace("&#x000D;", " ");
            json = json.Replace("&#x0009;", " ");

            json = json.Replace("&#x000c;", " ");
            json = json.Replace("&#x000a;", " ");
            json = json.Replace("&#x000d;", " ");

            json = json.Replace("  ", " ");
            json = json.Replace("   ", " ");
            json = json.Replace("   ", " ");
            json = json.Replace("&lrm;", "&#x200E;");
            json = json.Replace(" & ", "&#x200E;");
            json = json.Replace("&amp;nbsp;", " ");
            json = json.Replace("&#x0026;nbsp", " ");
            json = json.Replace("&nbsp", "");
            json = json.Replace("  ", " ");
            json = json.Replace("   ", " ");

            json = json.Replace("&amp;#x", "&#x");
            json = json.Replace("&amp;#", "&#");
            json = json.Replace("&amp;lt;", "&lt;");
            json = json.Replace("&amp;gt;", "&gt;");
            json = json.Replace("&AMP ;", "");
            #endregion


            updatedjson = json;

            //string is_valid = SchemaValidation(updatedjson, "FundingBody", XSDPath);

            if (json.Length > 0)
            {


                if (!(Directory.Exists(@"C:\Temp\VtoolFB")))
                {
                    Directory.CreateDirectory(@"C:\Temp\VtoolFB");
                }
                string error = "";
                fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["FUNDINGBODY_ID"].ToString());

                string FBID = fb.fundingBodyId.ToString();

                File.AppendAllText(@"C:\Temp\VtoolFB\FB_NDJSON_File_.txt", updatedjson + "\r\n");
            }
            else
            {

                File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
            }
            return updatedjson;
        }

        


        public string JsonCreationFromModel_FBAll(string XSDPath, DataTable dtFundingBody, DataTable dt_preferredorgname, DataTable dt_contextname, DataTable dt_abbrevname, DataTable dt_acronym, DataTable dt_subType, DataTable dt_identifier, DataTable dt_fundingdescription, DataTable dt_website, DataTable dt_establishmentInfo, DataTable dt_address, DataTable dt_awardSuccessRatedesc, DataTable dt_revisionhistory, DataTable dt_createddate, DataTable dt_reviseddate)
        {
            DAL.Transform XmlTransform = new DAL.Transform("");
            string updatedjson = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("lang");
                dt.Columns.Add("value");

                DataRow dr = dt.NewRow();
                dr["lang"] = "EN";
                dr["value"] = "Test";
                dt.Rows.Add(dr);
                DataRow dr1 = dt.NewRow();
                dr1["lang"] = "EN1";
                dr1["value"] = "Test1";
                dt.Rows.Add(dr1);
                DataRow dr2 = dt.NewRow();
                dr2["lang"] = "EN2";
                dr2["value"] = "Test2";
                dt.Rows.Add(dr2);

                FB_JSON_Model fb = new FB_JSON_Model();

                fb.status = dtFundingBody.Rows.Count > 0 && dtFundingBody.Rows[0]["status"].ToString().Trim() != "" ? dtFundingBody.Rows[0]["status"].ToString().ToUpper() : "Notavailable_Status";

                fb.preferredName = (from DataRow dr3 in dt_preferredorgname.Rows
                                    select new preferredName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["preferredorgname_text"].ToString().Trim()
                                    }).ToList();

                fb.acronym = (from DataRow dr3 in dt_acronym.Rows
                              select new acronym()
                              {
                                  language = dr3["acronym_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                  value = dr3["acronym_text"].ToString().Trim()
                              }).ToList();

                fb.abbrevName = (from DataRow dr3 in dt_abbrevname.Rows
                                 select new abbrevName()
                                 {
                                     language = dr3["abbrevname_text"].ToString().Trim() != "" ? dr3["lang"].ToString() : "",
                                     value = dr3["abbrevname_text"].ToString().Trim()
                                 }).ToList();
                fb.alternateName = (from DataRow dr3 in dt_contextname.Rows
                                    select new alternateName()
                                    {
                                        language = dr3["lang"].ToString(),
                                        value = dr3["contextname_text"].ToString().Trim()
                                    }).ToList();




                fb.profitabilityType = Convert.ToString(dtFundingBody.Rows[0]["profit"].ToString().ToUpper());
                fb.activityType = Convert.ToString(dt_subType.Rows[0]["id"].ToString().ToUpper());


                fb.country = Convert.ToString(dtFundingBody.Rows[0]["country"].ToString().ToLower());

                fb.state = Convert.ToString(dtFundingBody.Rows[0]["state"].ToString().ToUpper());

                fb.identifier = (from DataRow dridf in dt_identifier.Rows
                                 select new identifier()
                                 {
                                     type = dridf["type"].ToString(),
                                     value = dridf["value"].ToString()
                                 }).ToList();


                fb.description = (from DataRow dr3 in dt_fundingdescription.Rows
                                  select new description()
                                  {
                                      abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                      source = dr3["URL"].ToString()
                                  }).ToArray();

                fb.awardSuccessRate = new awardSuccessRate()
                {
                    description = (from DataRow dr3 in dt_awardSuccessRatedesc.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                       source = dr3["URL"].ToString()
                                   }).ToArray(),

                    //percentage = 99
                    percentage = dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString() != "" ? Convert.ToInt32(dtFundingBody.Rows[0]["AWARDSUCCESSRATE"].ToString()) : 0,
                };



                fb.contactInformation = new contactInformation()
                {
                    link = Convert.ToString(dt_website.Rows[0]["url"].ToString()),

                    hasPostalAddress = new hasPostalAddress()
                    {
                        addressCountry = dt_address.Rows[0]["COUNTRYTEST"].ToString(),
                        addressLocality = dt_address.Rows[0]["city"].ToString() != "Not Available" && dt_address.Rows[0]["city"].ToString().Trim() != "" ? dt_address.Rows[0]["city"].ToString().Trim() : dt_address.Rows[0]["COUNTRY"].ToString(),

                        // addressLocality = "Not Available1",
                        addressPostalCode = dt_address.Rows[0]["postalcode"].ToString(),
                        postOfficeBoxNumber = dt_address.Rows[0]["room"].ToString(),
                        //addressRegion = dt_address.Rows[0]["country"].ToString(),
                        addressRegion = dt_address.Rows[0]["state"].ToString(),
                        streetAddress = dt_address.Rows[0]["street"].ToString(),
                    }
                };


                fb.establishment = new establishment()
                {
                    establishmentYear = dt_establishmentInfo.Rows.Count > 0 ? (dt_establishmentInfo.Rows[0]["ESTABLISHMENTDATE"].ToString() != "" ? Convert.ToInt32(dt_establishmentInfo.Rows[0]["ESTABLISHMENTDATE"].ToString()) : -99999999) : -99999999,
                    //establishmentYear = 1990,
                    country = dt_establishmentInfo.Rows.Count > 0 ? dt_establishmentInfo.Rows[0]["ESTABLISHMENTCOUNTRYCODE"].ToString() : "",

                    description = (from DataRow dr3 in dt_establishmentInfo.Rows
                                   select new description()
                                   {
                                       abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["ESTABLISHMENTDESCRIPTION"].ToString() },
                                       //26nov 2019//source = dr3["URL"].ToString()
                                       source = dtFundingBody.Rows[0]["recordSource"].ToString()
                                   }).ToArray()

                };


                fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["ORGDBID"].ToString());
                // fb.fundingBodyId = Convert.ToInt64(dtAw_Replication.Rows[0]["Replicated_Id"].ToString());

                fb.financeType = Convert.ToString(dtFundingBody.Rows[0]["type"].ToString().ToUpper());

                fb.homePage = dtFundingBody.Rows[0]["recordSource"].ToString().Trim();
                //fb.homePage = dtAw_Replication.Rows[0]["Rep_Url"].ToString();


                fb.abbrevName = (from DataRow dr3 in dt_abbrevname.Rows
                                 select new abbrevName()
                                 {
                                     language = dr3["lang"].ToString(),
                                     value = dr3["abbrevname_text"].ToString().Trim()
                                 }).ToList();

                fb.hasProvenance = new hasProvenance()
                {
                    contactPoint = "fundingprojectteam@aptaracorp.com",
                    //contactPoint = "support@elsevier.com",
                    createdOn = dt_createddate.Rows[0]["CREATEDDATE_TEXT"].ToString(),
                    //defunct = dtFundingBody.Rows[0]["defunct"].ToString() !=""? Convert.ToBoolean( dtFundingBody.Rows[0]["defunct"].ToString()) : false,
                    defunct = false,
                    derivedFrom = dtFundingBody.Rows[0]["RECORDSOURCE"].ToString(),
                    hidden = dtFundingBody.Rows[0]["hidden"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["hidden"].ToString()) : false,
                    lastUpdateOn = dt_reviseddate.Rows[0]["REVISEDDATE_TEXT"].ToString(),
                    status = dt_revisionhistory.Rows[0]["status"].ToString().ToUpper(),
                    //lastUpdateOn = "",
                    //version = dt_reviseddate.Rows[0]["version"].ToString(),
                    version = dt_reviseddate.Rows.Count > 0 ? dt_reviseddate.Rows[0]["VERSION"].ToString() : dt_createddate.Rows[0]["version"].ToString(),
                    //version = "",
                    //wasAttributedTo = dtFundingBody.Rows[0]["collectioncode"].ToString(),
                    wasAttributedTo = "SUP002",
                };

                fb.fundingPolicy = (from DataRow dr3 in dt_fundingPolicy_Deatails.Rows
                                    select new fundingPolicy()
                                    {
                                        abstracts = new abstracts { language = dr3["LANG"].ToString(), value = dr3["DESCRIPTION"].ToString() },
                                        source = dr3["URL"].ToString()
                                    }).ToArray();



                fb.registry = new registry()
                {
                    fundingBodyDataset = new fundingBodyDataset()
                    {

                        //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                        collectionCode = "SUP002",
                        extended = dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()) : true,
                        tier = dtFundingBody.Rows[0]["TIERINFO"].ToString() != "" ? Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()) : 1,
                        #region Later Uncomment it
                        source = (from DataRow dr3 in dt_fundingBodyDataset.Rows
                                  select new source()
                                  {
                                      name = dr3["name"].ToString(),
                                      url = dr3["url"].ToString(),
                                      status = dr3["status"].ToString().ToUpper(),
                                      frequency = dr3["frequency"].ToString().ToUpper(),
                                      captureStart = dr3["captureStart"].ToString(),
                                      //captureEnd = dr3["captureEnd"].ToString(),
                                      captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),

                                      //comment = dr3["FB_COMMENT"].ToString()
                                      comment = dr3["FB_COMMENT"].ToString() == "" ? "" : dr3["FB_COMMENT"].ToString()

                                  }).ToArray()
                        #endregion Later Uncomment it

                    },



                    opportunityDataset = new opportunityDataset()
                    {
                        //capture = "false",
                        capture = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString()) : false,
                        //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                        //collectionCode = dtFundingBody.Rows[0]["OPPORTUNITIESSUPPLIER"].ToString(),
                        collectionCode = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",

                        //extended = Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()),
                        //tier = Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()),
                        #region Later Uncomment it
                        source = (from DataRow dr3 in dt_OPPORTUNITIESSOURCE.Rows
                                  select new source()
                                  {
                                      name = dr3["name"].ToString().Trim() == "" ? "Funder Website" : dr3["name"].ToString(),
                                      url = dr3["url"].ToString(),
                                      status = dtFundingBody.Rows[0]["CAPTUREOPPORTUNITIES"].ToString().ToLower() == "true" ? "ACTIVE" : dr3["status"].ToString().ToUpper(),
                                      //frequency = dr3["frequency"].ToString().ToUpper(),
                                      frequency = "SIGNAL-BASED",
                                      captureStart = dr3["captureStart"].ToString(),
                                      //captureStart = dr3["created_date"].ToString(),
                                      //captureEnd = dr3["captureEnd"].ToString(),
                                      captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                                      //captureEnd = dr3["lastvisited"].ToString() == "" ? "" : dr3["lastvisited"].ToString(),
                                      //comment = dr3["OPP_COMMENT"].ToString()
                                      comment = dr3["OPP_COMMENT"].ToString() == "" ? "" : dr3["OPP_COMMENT"].ToString()
                                  }).ToArray()
                        #endregion Later Uncomment it
                    },

                    awardDataset = new awardDataset()
                    {
                        //capture = "false",
                        capture = dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString()) : false,
                        //collectionCode = dtFundingBody.Rows[0]["AWARDSSUPPLIER"].ToString(),
                        //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                        //collectionCode = "SUP002",
                        collectionCode = dtFundingBody.Rows[0]["CAPTUREAWARDS"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",
                        //extended = Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()),
                        //tier = Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()),
                        #region Later Uncomment it
                        source = (from DataRow dr3 in dt_awardSSOURCE.Rows
                                  select new source()
                                  {
                                      name = dr3["name"].ToString().Trim() == "" ? "Funder Website" : dr3["name"].ToString(),
                                      url = dr3["url"].ToString(),
                                      status = dr3["status"].ToString().ToUpper(),
                                      //frequency = dr3["frequency"].ToString().ToUpper(),
                                      frequency = "SIGNAL-BASED",
                                      captureStart = dr3["captureStart"].ToString(),
                                      // captureStart = dr3["created_date"].ToString(),
                                      //captureEnd = dr3["captureEnd"].ToString(),
                                      captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                                      // captureEnd = dr3["lastvisited"].ToString() == "" ? "" : dr3["lastvisited"].ToString(),
                                      //comment = dr3["AW_COMMENT"].ToString()
                                      comment = dr3["AW_COMMENT"].ToString() == "" ? "" : dr3["AW_COMMENT"].ToString()
                                  }).ToArray()
                        #endregion
                    },


                    publicationDataset = new publicationDataset()
                    {
                        //capture = "false",
                        capture = dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString() != "" ? Convert.ToBoolean(dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString()) : false,
                        //collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                        collectionCode = dtFundingBody.Rows[0]["PUBLICATIONCAPTURE"].ToString().ToLower() == "false" ? "NOTSPECIFIED" : "SUP002",

                        //collectionCode = "SUP002",
                        //extended = Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()),
                        //tier = Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()),
                        #region Later Uncomment it
                        source = (from DataRow dr3 in dt_publicationDataset.Rows
                                  select new source()
                                  {
                                      name = dr3["name"].ToString(),
                                      url = dr3["url"].ToString(),
                                      status = dr3["status"].ToString().ToUpper(),
                                      frequency = dr3["frequency"].ToString().ToUpper(),
                                      captureStart = dr3["captureStart"].ToString(),
                                      //captureEnd = dr3["captureEnd"].ToString(),
                                      captureEnd = dr3["captureEnd"].ToString() == "" ? "" : dr3["captureEnd"].ToString(),
                                      //comment = dr3["PUB_COMMENT"].ToString()
                                      comment = dr3["PUB_COMMENT"].ToString() == "" ? "" : dr3["PUB_COMMENT"].ToString()
                                  }).ToArray()
                        #endregion Later Uncomment it
                    },

                };


                //fb.registry = new registry()
                //{
                //    fundingBodyDataset = new fundingBodyDataset()
                //    {
                //        collectionCode = dtFundingBody.Rows[0]["COLLECTIONCODE"].ToString(),
                //        extended = Convert.ToBoolean(dtFundingBody.Rows[0]["EXTENDEDRECORD"].ToString()),
                //        tier = Convert.ToInt64(dtFundingBody.Rows[0]["TIERINFO"].ToString()),

                //        source = new source()
                //        {
                //            name = Convert.ToString(dt_fundingBodyDataset.Rows[0]["name"].ToString()),
                //            url = Convert.ToString(dt_fundingBodyDataset.Rows[0]["url"].ToString()),
                //            status = Convert.ToString(dt_fundingBodyDataset.Rows[0]["status"].ToString()),
                //            frequency = Convert.ToString(dt_fundingBodyDataset.Rows[0]["frequency"].ToString()),
                //            captureStart = Convert.ToString(dt_fundingBodyDataset.Rows[0]["captureStart"].ToString()),
                //            captureEnd = Convert.ToString(dt_fundingBodyDataset.Rows[0]["captureEnd"].ToString()),
                //            comment = Convert.ToString(dt_fundingBodyDataset.Rows[0]["FB_COMMENT"].ToString()),
                //        },
                //    }
                //};

                //fb.relation = null;


                #region data filter
                dataView = dt_releatedorgs.DefaultView;

                DataTable dt_partOf, dt_parentOf, dt_hasPart, dt_CHANGE, dt_affiliatedWith, dt_continuationOf, dt_renamedAs, dt_mergedWith, dt_mergerOf, dt_incrorporatedInto, dt_incorporates, dt_splitInto, dt_splitFrom, dt_isReplacedBy, dt_replaces;

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'partOf'";
                dt_partOf = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'parentOf'";
                dt_parentOf = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'hasPart'";
                dt_hasPart = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'CHANGE'";
                dt_CHANGE = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'affiliatedWith'";
                dt_affiliatedWith = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'continuationOf'";
                dt_continuationOf = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'renamedAs'";
                dt_renamedAs = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'mergedWith'";
                dt_mergedWith = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'mergerOf'";
                dt_mergerOf = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'incrorporatedInto'";
                dt_incrorporatedInto = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'incorporates'";
                dt_incorporates = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'splitInto'";
                dt_splitInto = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'splitFrom'";
                dt_splitFrom = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'isReplacedBy'";
                dt_isReplacedBy = dataView.ToTable();

                dataView = dt_releatedorgs.DefaultView;
                dataView.RowFilter = "reltype = 'replaces'";
                dt_replaces = dataView.ToTable();

                #endregion




                //#endregion
                #region Fb releation
                fb.relation = new relation()
                {
                    //dt_releatedorgs.DataSet =  dataView; 

                    //partOf = (from DataRow dr3 in dt_partOf.Rows
                    //          select new partOf()
                    //          {

                    //              reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //          }).ToArray(),

                    partOf = (from DataRow dr3 in dt_partOf.Rows

                              select Convert.ToInt64(dr3["ORGDBID"].ToString())
              ).ToList(),

                    ////parentOf = (from DataRow dr3 in dt_parentOf.Rows
                    ////            select new parentOf()
                    ////                  {

                    ////                      reltypeValue = dr3["ORGDBID"].ToString()

                    ////                  }).ToArray(),



                    //hasPart = (from DataRow dr3 in dt_hasPart.Rows
                    //           select new hasPart()
                    //           {

                    //               reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //           }).ToArray(),


                    hasPart = (from DataRow dr3 in dt_hasPart.Rows

                               select Convert.ToInt64(dr3["ORGDBID"].ToString())
                          ).ToList(),


                    CHANGE = (from DataRow dr3 in dt_CHANGE.Rows

                              select Convert.ToInt64(dr3["ORGDBID"].ToString())
                                ).ToList(),
                    //affiliatedWith = (from DataRow dr3 in dt_affiliatedWith.Rows
                    //                  select new affiliatedWith()
                    //                  {

                    //                      reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //                  }).ToArray(),

                    affiliatedWith = (from DataRow dr3 in dt_affiliatedWith.Rows

                                      select Convert.ToInt64(dr3["ORGDBID"].ToString())
             ).ToList(),



                    //continuationOf = (from DataRow dr3 in dt_continuationOf.Rows
                    //                  select new continuationOf()
                    //                  {

                    //                      reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //                  }).ToArray(),


                    continuationOf = (from DataRow dr3 in dt_continuationOf.Rows

                                      select Convert.ToInt64(dr3["ORGDBID"].ToString())
             ).ToList(),
                    //renamedAs = (from DataRow dr3 in dt_renamedAs.Rows
                    //             select new renamedAs()
                    //             {

                    //                 reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //             }).ToArray(),

                    renamedAs = (from DataRow dr3 in dt_renamedAs.Rows

                                 select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),

                    //mergedWith = (from DataRow dr3 in dt_mergedWith.Rows
                    //              select new mergedWith()
                    //              {

                    //                  reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //              }).ToArray(),

                    mergedWith = (from DataRow dr3 in dt_mergedWith.Rows

                                  select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),
                    //mergerOf = (from DataRow dr3 in dt_mergerOf.Rows
                    //            select new mergerOf()
                    //            {

                    //                reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //            }).ToArray(),

                    mergerOf = (from DataRow dr3 in dt_mergerOf.Rows

                                select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),
                    //incrorporatedInto = (from DataRow dr3 in dt_incrorporatedInto.Rows
                    //                     select new incrorporatedInto()
                    //                     {

                    //                         reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //                     }).ToArray(),

                    incrorporatedInto = (from DataRow dr3 in dt_incrorporatedInto.Rows

                                         select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),
                    //                    incorporates = (from DataRow dr3 in dt_incorporates.Rows
                    //                                    select new incorporates()
                    //                                    {

                    //                                        reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //                                    }).ToArray(),

                    incorporates = (from DataRow dr3 in dt_incorporates.Rows

                                    select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),
                    //splitInto = (from DataRow dr3 in dt_splitInto.Rows
                    //             select new splitInto()
                    //             {

                    //                 reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //             }).ToArray(),


                    splitInto = (from DataRow dr3 in dt_splitInto.Rows

                                 select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),

                    //splitFrom = (from DataRow dr3 in dt_splitFrom.Rows
                    //             select new splitFrom()
                    //             {

                    //                 reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //             }).ToArray(),


                    splitFrom = (from DataRow dr3 in dt_splitFrom.Rows

                                 select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),
                    //isReplacedBy = (from DataRow dr3 in dt_isReplacedBy.Rows
                    //                select new isReplacedBy()
                    //                {

                    //                    reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //                }).ToArray(),

                    isReplacedBy = (from DataRow dr3 in dt_isReplacedBy.Rows

                                    select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList(),

                    //replaces = (from DataRow dr3 in dt_replaces.Rows
                    //            select new replaces()
                    //            {

                    //                reltypeValue = dr3["ORGDBID"].ToString() + cmp_array

                    //            }).ToArray()

                    replaces = (from DataRow dr3 in dt_replaces.Rows

                                select Convert.ToInt64(dr3["ORGDBID"].ToString())
).ToList()
                };




                #endregion

                //string json = fb.ToString();

                //string json = JsonConvert.SerializeObject(fb);

                var settings = new JsonSerializerSettings { Converters = { new ReplacingStringWritingConverter("\r\n", "") } };
                //  var newJson = JsonConvert.SerializeObject(opp, Newtonsoft.Json.Formatting.None, settings1);

                //JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                //settings.TypeNameHandling = TypeNameHandling.Objects;
                string json = JsonConvert.SerializeObject(fb, Newtonsoft.Json.Formatting.None, settings);
                #region
                //json = json.Replace("abstracts", "abstract");
                //json = json.Replace("--Select Defunct--", "false");
                //json = json.Replace("\"acronym\":[],", "");
                //json = json.Replace("\"alternateName\":[],", "");
                //json = json.Replace("\"abbrevName\":[],", "");
                //json = json.Replace("\"preferredName\":[],", "");
                //json = json.Replace("\"acronym\": [],", "");
                //json = json.Replace("\"alternateName\": [],", "");
                //json = json.Replace("\"abbrevName\": [],", "");
                //json = json.Replace("\"preferredName\": [],", "");
                //json = json.Replace("\"description\": [],", "");
                //json = json.Replace("\"description\":[],", "");

                //json = json.Replace("\"partOf\": [],", "");
                //json = json.Replace("\"parentOf\": [],", "");
                //json = json.Replace("\"hasPart\": [],", "");
                //json = json.Replace("\"CHANGE\": [],", "");
                //json = json.Replace("\"renamedAs\": [],", "");
                //json = json.Replace("\"mergedWith\": [],", "");
                //json = json.Replace("\"mergerOf\": [],", "");
                //json = json.Replace("\"incrorporatedInto\": [],", "");
                //json = json.Replace("\"incorporates\": [],", "");
                //json = json.Replace("\"splitInto\": [],", "");
                //json = json.Replace("\"splitFrom\": [],", "");
                //json = json.Replace("\"isReplacedBy\": [],", "");
                //json = json.Replace("\"replaces\": [],", "");
                //json = json.Replace("\"affiliatedWith\": [],", "");
                //json = json.Replace("\"continuationOf\": [],", "");
                //json = json.Replace("\"replaces\": []", "");
                //json = json.Replace("\"replaces\": [],", "");

                //json = json.Replace("\"partOf\":[],", "");
                //json = json.Replace("\"parentOf\":[],", "");
                //json = json.Replace("\"hasPart\":[],", "");
                //json = json.Replace("\"CHANGE\":[],", "");
                //json = json.Replace("\"renamedAs\":[],", "");
                //json = json.Replace("\"mergedWith\":[],", "");
                //json = json.Replace("\"mergerOf\":[],", "");
                //json = json.Replace("\"incrorporatedInto\":[],", "");
                //json = json.Replace("\"incorporates\":[],", "");
                //json = json.Replace("\"splitInto\":[],", "");
                //json = json.Replace("\"splitFrom\":[],", "");
                //json = json.Replace("\"isReplacedBy\":[],", "");
                //json = json.Replace("\"replaces\":[],", "");
                //json = json.Replace("\"affiliatedWith\":[],", "");
                //json = json.Replace("\"continuationOf\":[],", "");
                //json = json.Replace("\"replaces\":[]", "");
                //json = json.Replace("\"replaces\":[],", "");
                //json = json.Replace("\"fundingPolicy\":[],", "");
                //json = json.Replace("\"identifier\":[],", "");

                //json = json.Replace("Not Available", "");





                ////json = json.Replace("\"partOf\": [      {        ", "");

                ////json = json.Replace("\"partOf\":[{\"reltypeValue\":", "");
                //json = json.Replace("{\"reltypeValue\":\"", "");
                //json = json.Replace("_cmpary\"}]", "]");
                //json = json.Replace("],},", "]},");

                //json = json.Replace("_cmpary\"},", ",");
                //json = json.Replace("],}}", "]}}");
                #endregion
                //26 nov 2019
                //JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                //settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var myJson = JsonConvert.SerializeObject(fb, settings);
                //var myJson1 = JsonConvert.SerializeObject(fb, Newtonsoft.Json.Formatting.Indented);

                #region
                myJson = myJson.Replace("Not Available", "");
                myJson = myJson.Replace("abstracts", "abstract");
                myJson = myJson.Replace("--Select Defunct--", "false");
                myJson = myJson.Replace("--Select Defunct--", "false");
                // json = json.Replace("\"establishmentYear\": -99999999", "");
                myJson = myJson.Replace("-99999999", "");

                #endregion

                #region for Handling Empty array and values
                // var temp = JObject.Parse(myJson);
                // temp.Descendants()
                //     .OfType<JProperty>()
                //     .Where(attr => attr.Value.ToString() == "")
                //     .ToList() // you should call ToList because you're about to changing the result, which is not possible if it is IEnumerable
                //     .ForEach(attr => attr.Remove()); // removing unwanted attributes
                // temp.Descendants()
                //.OfType<JProperty>()
                //.Where(jp => jp.Value.Type == JTokenType.Array && !jp.Value.HasValues)
                //.ToList()
                //.ForEach(jp => jp.Remove());
                // json = temp.ToString();


                for (int k = 0; k <= 10; k++)
                {
                    myJson = Remove_NullObjects(myJson);

                    myJson = myJson.Replace("\"\",", "");

                    myJson = myJson.Replace("\"\"", "");
                }
                json = myJson.ToString();
                #endregion

                #region//For Default Values
                json = json.Replace("NotAvailable_Locality", "Not Available");
                json = json.Replace("Notavailable_Status", "Not Available");

                #endregion

                json = Regex.Replace(json, @"\t|\n|\r|\r\n", "");

                json = json.Trim();
                //string htmlEs = HtmlDecode(json);
                //json = jun(htmlEs);
                // updatedjson = json;

                string json_1 = Regex.Replace(json, @"\t|\n|\r|\r\n", "");
                json = Regex.Replace(json, @"\r\n?|\n", "");
                json = json.Trim();




                #region
                // string json_2 = Regex.Replace(json, "\"[^\"]*(?:\"\"[^\"]*)*\"", m => m.Value.Replace("\r", "").Replace("\n", ""));
                json = json.Replace(Environment.NewLine, "").Replace("&#x00a0;", " ").Replace("\r", "").Replace("\n", "");
                #region Hex Values which have to replaced into space on 15-Mar-2019

                json = json.Replace("&#x00A0;", " ");
                json = json.Replace("&#x2002;", " ");
                json = json.Replace("&#x2003;", " ");
                json = json.Replace("&#x2004;", " ");
                json = json.Replace("&#x2005;", " ");
                json = json.Replace("&#x2006;", " ");
                json = json.Replace("&#x2007;", " ");
                json = json.Replace("&#x2008;", " ");
                json = json.Replace("&#x2009;", " ");
                json = json.Replace("&#x200A;", " ");
                json = json.Replace("&#x200B;", " ");
                json = json.Replace("&#x3000;", " ");
                json = json.Replace("&#xFEFF;", " ");
                #endregion
                json = Sgml_to_Hexadecimal(json, XSDPath);
                string json1 = Utf_to_Html(json);
                json = anyToHex(json1);
                //////json = Sgml_to_Hexadecimal(json, XSDPath);
                #endregion

                #region Hex Values which have to replaced into space on 15-Mar-2019
                json = json.Replace("&nbsp;", " ");
                json = json.Replace("&#x00A0;", " ");
                json = json.Replace("&#x00a0;", " ");
                json = json.Replace("&#x2002;", " ");
                json = json.Replace("&#x2003;", " ");
                json = json.Replace("&#x2004;", " ");
                json = json.Replace("&#x2005;", " ");
                json = json.Replace("&#x2006;", " ");
                json = json.Replace("&#x2007;", " ");
                json = json.Replace("&#x2008;", " ");
                json = json.Replace("&#x2009;", " ");
                json = json.Replace("&#x200A;", " ");
                json = json.Replace("&#x200B;", " ");
                json = json.Replace("&#x3000;", " ");
                json = json.Replace("&#xFEFF;", " ");
                json = json.Replace("&#x202f;", " ");
                json = json.Replace("&#x22;", "\"");

                json = json.Replace("&#x0022;", "\"");
                //json = json.Replace("&#x005C;;", "\"");
                // json = json.Replace("&#x002F;", "\"");
                json = json.Replace("&#x0008;", " ");
                json = json.Replace("&#x000C;", " ");
                json = json.Replace("&#x000A;", " ");
                json = json.Replace("&#x000D;", " ");
                json = json.Replace("&#x0009;", " ");

                json = json.Replace("&#x000c;", " ");
                json = json.Replace("&#x000a;", " ");
                json = json.Replace("&#x000d;", " ");








                json = json.Replace("  ", " ");
                json = json.Replace("   ", " ");
                json = json.Replace("   ", " ");
                json = json.Replace("&lrm;", "&#x200E;");
                json = json.Replace(" & ", "&#x200E;");
                json = json.Replace("&amp;nbsp;", " ");
                json = json.Replace("&#x0026;nbsp", " ");
                json = json.Replace("&nbsp", "");
                json = json.Replace("  ", " ");
                json = json.Replace("   ", " ");


                json = json.Replace("&amp;#x", "&#x");
                json = json.Replace("&amp;#", "&#");
                json = json.Replace("&amp;lt;", "&lt;");
                json = json.Replace("&amp;gt;", "&gt;");
                json = json.Replace("&AMP ;", "");
                #endregion


                updatedjson = json;
                string is_valid = SchemaValidation(updatedjson, "FundingBody", XSDPath);
                // return is_valid;
                if (json.Length > 0)
                {


                    if (!(Directory.Exists(@"C:\Temp\VtoolFB")))
                    {
                        Directory.CreateDirectory(@"C:\Temp\VtoolFB");
                    }
                    string error = "";
                    fb.fundingBodyId = Convert.ToInt64(dtFundingBody.Rows[0]["ORGDBID"].ToString());

                    string FBID = fb.fundingBodyId.ToString();
                    // File.WriteAllText(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".json", updatedjson);

                    File.AppendAllText(@"C:\Temp\VtoolFB\FB_NDJSON_File_.txt", updatedjson + "\r\n");

                    ////File.WriteAllText(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".json", updatedjson);
                    ///// XmlTransform.StartProcessForVTool_JSON(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".json");
                    //////if (File.Exists(@"C:\Temp\VtoolFB\fp\FB_JSON_File_" + FBID + "_json_xsl2751_fp.json"))
                    //////{
                    //////    String FpJSON = File.ReadAllText(@"C:\Temp\VtoolFB\fp\FB_JSON_File_" + FBID + "_json_xsl2751_fp.json");
                    //////    JObject Fp_JSON = JObject.Parse(FpJSON);

                    //////    error = (string)Fp_JSON["fingerprints"]["fingerprint"]["results"]["total-errors"];

                    //////    File.Delete(@"C:\Temp\VtoolFB\fp\FB_JSON_File_" + FBID + "_json_xsl2751_fp.json");
                    //////    File.Delete(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".json");
                    //////    File.Delete(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".xml");

                    //////}

                    //JToken.Parse(updatedjson).ToString();
                    //File.WriteAllText(@"C:\Temp\VtoolFB\FB_JSON_File_" + FBID + ".json", updatedjson);
                    // File.WriteAllText(@"C:\Temp\FB_JSON_File_" + FBID + ".json", JToken.Parse(updatedjson).ToString());
                    //////if (error == "0")
                    //////{
                    //////    File.AppendAllText(@"C:\Temp\FB_NDJSON_File_.txt", updatedjson + "\r\n");
                    //////}
                    //File.WriteAllText(@"C:\Temp\FB_JSON_File_" + FBID + ".txt", updatedjson);



                    // throw new Exception("JSON file genrate successfully! for FB Module & File copied in temp Folder");

                }
                else
                {

                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                }
                return is_valid;
            }
            catch (Exception ex)
            {
                return ex.Message;
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    //oErrorLog.WriteErrorLog(ex);
                    //oErrorLog.WorkProcessLog("Something went worng.");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
            #region Post Json File on client server
            //PostJsonfile(json);

            //Console.WriteLine(json);
            #endregion Post Json File
        }

        private DataSet GetFundingBodyXSDSchema(String XsdPath)
        {
            // String Path = XsdPath + "\\FundingBodies_ForFillData.xsd";
            String Path = XsdPath + "\\FundingBodies_ForFillData4.1.xsd";

            DataSet dataSet = new DataSet();
            dataSet.ReadXmlSchema(Path);

            DataSet FBSCHEMA = new DataSet();
            FBSCHEMA.ReadXmlSchema(Path);
            DataRelation DR = FBSCHEMA.Tables["fundingbodies"].ChildRelations[0];
            FBSCHEMA.Tables["fundingbodies"].ChildRelations.Remove(DR);
            FBSCHEMA.Tables["fundingbody"].Constraints.Remove("fundingBodies_fundingBody");//
            FBSCHEMA.Tables.Remove("fundingbodies");
            FBSCHEMA.Tables["fundingbody"].Columns.Remove("fundingbodies_id");
            //FBSCHEMA.WriteXmlSchema(Path.Replace(".xsd", "_2.xsd"));
            foreach (DataTable DT in FBSCHEMA.Tables)
            {

            }
            return FBSCHEMA;
        }

        private void SequenceFundingBodyXML(XmlDocument Doc)
        {
            try
            {
                DataTable DT = new DataTable();
                DT.Columns.Add("Sequence");
                DataRow DR = DT.NewRow();
                DR[0] = "revisionHistory";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "recordSource";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "canonicalName";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "preferredOrgName";
                DT.Rows.Add(DR);



                DR = DT.NewRow();
                DR[0] = "contextName";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "abbrevName";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "acronym";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "subType";
                DT.Rows.Add(DR);

                #region Added by Avanish for Schema4.0 on 4-June-2018
                DR = DT.NewRow();
                DR[0] = "region";
                DT.Rows.Add(DR);
                #endregion

                #region Added by Avanish for Schema4.1 on 26-June-2019
                DR = DT.NewRow();
                DR[0] = "subRegion";
                DT.Rows.Add(DR);
                #endregion


                DR = DT.NewRow();
                DR[0] = "geoScope";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "fundedProgramsTypes";
                DT.Rows.Add(DR);


                DR = DT.NewRow();
                DR[0] = "eligibilityDescription";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "fundingPolicy";
                DT.Rows.Add(DR);


                DR = DT.NewRow();
                DR[0] = "appInfo";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "about";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "classificationGroup";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "keywords";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "contactInfo";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "relatedOrgs";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "relatedItems";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "financialInfo";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "contacts";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "establishmentInfo";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "awardStatistics";
                DT.Rows.Add(DR);
                DR = DT.NewRow();

                DR[0] = "awardSuccessRate";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "officers";
                DT.Rows.Add(DR);
                DR = DT.NewRow();
                DR[0] = "comments"; // New upadte XSD V1.4 By Harish @ 15-April-2014
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "awardsSource";
                DT.Rows.Add(DR);
                DR = DT.NewRow();


                DR = DT.NewRow();
                DR[0] = "opportunitiesSource";
                DT.Rows.Add(DR);
                DR = DT.NewRow();

                DR = DT.NewRow();
                DR[0] = "funderGroup";
                DT.Rows.Add(DR);
                DR = DT.NewRow();



                //DR[0] = "comments";  
                //DT.Rows.Add(DR);

                XmlElement Root = Doc.DocumentElement;
                XmlNodeList ChildNodes = Root.ChildNodes;
                for (Int32 i = 0; i < ChildNodes.Count; i++)
                {
                    XmlNode Xnode = ChildNodes[i];
                    String Seq = "";
                    String sep = "";
                    for (Int32 z = 0; z < DT.Rows.Count; z++)
                    {
                        for (Int32 x = 0; x < Xnode.ChildNodes.Count; x++)
                        {
                            if (Xnode.ChildNodes[x].Name == DT.Rows[z][0].ToString())
                            {

                                // Add by kunal on 10 Macrh
                                // if (Xnode.ChildNodes[x].Name == "geoScope" || Xnode.ChildNodes[x].Name == "appInfo" || Xnode.ChildNodes[x].Name == "about" || Xnode.ChildNodes[x].Name == "relatedItems")
                                if (Xnode.ChildNodes[x].Name == "geoScope" || Xnode.ChildNodes[x].Name == "appInfo" || Xnode.ChildNodes[x].Name == "about" || Xnode.ChildNodes[x].Name == "relatedItems" || Xnode.ChildNodes[x].Name == "fundingPolicy")
                                {
                                    XmlNode GeoscopenNode = Xnode.ChildNodes[x];   //e.g Geoscope node 

                                    for (Int32 y = 0; y < GeoscopenNode.ChildNodes.Count; y++)
                                    {
                                        XmlNode XItemNode = GeoscopenNode.ChildNodes[y];   //e.g Geoscope--Item node

                                        DataTable dtSeq = getAboutTable();

                                        for (Int32 count = 0; count < dtSeq.Rows.Count; count++)
                                        {
                                            for (Int32 zz = 0; zz < XItemNode.ChildNodes.Count; zz++)
                                            {
                                                if (XItemNode.ChildNodes[zz].Name == dtSeq.Rows[count][0].ToString())
                                                {
                                                    GeoscopenNode.ChildNodes[y].AppendChild(XItemNode.ChildNodes[zz]);

                                                }
                                            }
                                        }

                                    }
                                }
                                else if (Xnode.ChildNodes[x].Name == "officers" || Xnode.ChildNodes[x].Name == "contacts" || Xnode.ChildNodes[x].Name == "contactInfo")
                                {
                                    XmlNode ContactInfoNode = Xnode.ChildNodes[x];   //e.g ContactInfo node 

                                    for (Int32 y = 0; y < ContactInfoNode.ChildNodes.Count; y++)  // For Contact node
                                    {
                                        XmlNode ContactNode = ContactInfoNode.ChildNodes[y];

                                        for (Int32 zz = 0; zz < ContactNode.ChildNodes.Count; zz++)  // For Contact child node
                                        {

                                            DataTable ConatctInofDT = GetContactInfoTable();

                                            for (Int32 count = 0; count < ConatctInofDT.Rows.Count; count++)   // datatable loop
                                            {
                                                for (Int32 Childcount = 0; Childcount < ContactNode.ChildNodes.Count; Childcount++)   // datatable loop
                                                {
                                                    if (ContactNode.ChildNodes[Childcount].Name == ConatctInofDT.Rows[count][0].ToString())
                                                    {
                                                        XmlNode ContactChildNode = ContactNode.ChildNodes[Childcount];
                                                        if (ContactChildNode.Name == "contactName")
                                                        {
                                                            DataTable dtContactSeq = GetContactNameTable();

                                                            for (Int32 count1 = 0; count1 < dtContactSeq.Rows.Count; count1++)   // datatable loop
                                                            {
                                                                for (Int32 Childcount1 = 0; Childcount1 < ContactChildNode.ChildNodes.Count; Childcount1++)   // datatable loop
                                                                {
                                                                    if (ContactChildNode.ChildNodes[Childcount1].Name == dtContactSeq.Rows[count1][0].ToString())
                                                                    {
                                                                        ContactChildNode.AppendChild(ContactChildNode.ChildNodes[Childcount1]);
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            ContactNode.AppendChild(ContactChildNode);
                                                        }
                                                        else if (ContactChildNode.ChildNodes[Childcount].Name == "address")
                                                        {
                                                            DataTable dtContactSeq = GetAddressTable();

                                                            for (Int32 count1 = 0; count1 < dtContactSeq.Rows.Count; count1++)   // datatable loop
                                                            {
                                                                for (Int32 Childcount1 = 0; Childcount1 < ContactChildNode.ChildNodes.Count; Childcount1++)   // datatable loop
                                                                {
                                                                    if (ContactChildNode.ChildNodes[Childcount1].Name == dtContactSeq.Rows[count1][0].ToString())
                                                                    {
                                                                        ContactChildNode.AppendChild(ContactChildNode.ChildNodes[Childcount1]);
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            ContactNode.AppendChild(ContactChildNode);
                                                        }
                                                        else
                                                        {
                                                            ContactNode.AppendChild(ContactChildNode);
                                                            break;
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                    }
                                }


                                else if (Xnode.ChildNodes[x].Name == "awardsSource")
                                {
                                    ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                    while (ChildNodes[i].ChildNodes[x].Name == "awardsSource")
                                    {
                                        ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                    }
                                    break;
                                }
                                else if (Xnode.ChildNodes[x].Name == "opportunitiesSource")
                                {
                                    ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                    while (ChildNodes[i].ChildNodes[x].Name == "opportunitiesSource")
                                    {
                                        ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                    }
                                    break;
                                }
                                if (Xnode.ChildNodes[x].NextSibling != null)
                                {
                                    if (Xnode.ChildNodes[x].Name == Xnode.ChildNodes[x].NextSibling.Name)
                                    {
                                        ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                        x = 0;
                                    }
                                    else
                                    {
                                        ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                        break;
                                    }
                                }
                                else
                                {
                                    ChildNodes[i].AppendChild(ChildNodes[i].ChildNodes[x]);
                                    break;
                                }
                                // End of code

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // oErrorLog.WriteErrorLog(ex);
                Console.WriteLine("Exception: {0}", ex.ToString());
            }

        }

        #region ValidateXML

        private void ValidationHandler(object sender, System.Xml.Schema.ValidationEventArgs args)
        {

            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        public String ValidateXML(XmlDocument XMLDoc, Int64 ModuleId, String XsdPath)
        {
            try
            {
                if (XsdPath.Trim() != String.Empty)
                {
                    String Path = String.Empty;
                    if (ModuleId == 2)
                    {
                        //Path = XsdPath + "\\fundingBodies30.xsd";
                        //Path = XsdPath + "\\fundingBodies40.xsd";
                        Path = XsdPath + "\\fundingBodies41.xsd";

                    }
                    else if (ModuleId == 3)
                    {
                        Path = XsdPath + "\\Opportunities41.xsd";
                    }
                    else if (ModuleId == 4)
                    {
                        // Path = XsdPath + "\\Awards30.xsd";
                        // Path = XsdPath + "\\Awards40.xsd";
                        Path = XsdPath + "\\Awards41.xsd";
                    }

                    // Declare local objects
                    //XmlTextReader tr = new XmlTextReader(new StringReader(XMLDoc.OuterXml));
                    XmlTextReader tr = null;
                    XmlSchemaCollection xsc = null;
                    XmlValidatingReader vr = null;
                    // String FileUrl=strXMLDoc.Substring(0,strXMLDoc.LastIndexOf("\\"));
                    // Text reader object

                    tr = new XmlTextReader(Path);
                    xsc = new XmlSchemaCollection();
                    //xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-1.2", tr);  Comment for updating xsd V 1.2 to 1.4  // Harish @ 15-April-2014
                    //xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-1.4", tr);
                    //xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-2.0", tr); Comment for updating xsd V 2.0 to 3.0  // Rantosh
                    //xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-3.0", tr);
                    // xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-4.0", tr);
                    xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-4.1", tr);
                    // XML validator object
                    tr = new XmlTextReader(new StringReader(XMLDoc.OuterXml));
                    vr = new XmlValidatingReader(tr);

                    vr.Schemas.Add(xsc);

                    // Add validation event handler

                    vr.ValidationType = ValidationType.Schema;
                    vr.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationHandler);

                    // Validate XML data

                    while (vr.Read()) ;

                    vr.Close();

                    // Raise exception, if XML validation fails
                    if (ErrorsCount > 0)
                    {
                        return ErrorMessage;
                    }

                    // XML Validation succeeded
                    return "1";
                }
                else
                {
                    return "XSD PATH NOT FOUND !";
                }
            }
            catch (Exception ex)
            {
                //oErrorLog.WriteErrorLog(ex);
                // XML Validation failed
                //Console.WriteLine("XML validation failed." + "\r\n" +
                //"Error Message: " + error.Message);
                return ex.Message;
            }

        }
        #endregion Validate XML

        #region Sequencing Declaration
        private DataTable GetAffiliationTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");
                DataRow DR = DT.NewRow();
                DR[0] = "org";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "dept";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "startDate";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "endDate";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "address";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "telephone";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "fax";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "email";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "webpage";
                DT.Rows.Add(DR);
            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable GetContactNameTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");
                DataRow DR = DT.NewRow();
                DR[0] = "prefix";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "givenName";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "middleName";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "surname";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "suffix";
                DT.Rows.Add(DR);
            }
            catch (Exception ex)
            {
            }

            return DT;
        }
        private DataTable GetContactInfoTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "contactName";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "title";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "telephone";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "fax";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "email";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "website";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "address";
                DT.Rows.Add(DR);
            }
            catch (Exception ex)
            {
            }

            return DT;
        }
        private DataTable GetAddressTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "room";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "street";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "city";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "state";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "postalCode";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "country";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }
        private DataTable getAboutTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "link";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "description";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }



        #region Added by Avanish on 23-JUne-2018


        private DataTable getKeywordTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "keyword";
                DT.Rows.Add(DR);


            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable getresearchOutcomeTable()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "doi";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "pubmedId";
                DT.Rows.Add(DR);



                DR = DT.NewRow();
                DR[0] = "pmcId";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "medlineId";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "scopusId";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "itemId";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }
        #endregion

        private DataTable getOpportunityDate()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "Date";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "description";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }



        #endregion Added by avanish on 22-June-2018

        private DataTable eligibilityDescription()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "Date";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "description";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable relatedFundingBodies()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("lead");

                DataRow DR = DT.NewRow();
                DR[0] = "lead";
                DT.Rows.Add(DR);



            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable individualEligibility()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "citizenship";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "degree";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "graduate";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "newfaculty";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "undergraduate";
                DT.Rows.Add(DR);



            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable organizationEligibility()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "academic";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "commercial";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "government";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "nonprofit";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "regionspecific";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "sme";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        private DataTable restrictions()
        {
            DataTable DT = new DataTable();
            try
            {
                DT.Columns.Add("Sequence");

                DataRow DR = DT.NewRow();
                DR[0] = "disabilities";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "invitationonly";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "limitedsubmission";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "memberonly";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "nominationonly";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "minorities";
                DT.Rows.Add(DR);

                DR = DT.NewRow();
                DR[0] = "women";
                DT.Rows.Add(DR);

            }
            catch (Exception ex)
            {
            }

            return DT;
        }

        #region Ligation Handler

        private String HandleLegation(String XMLContent)
        {
            XMLContent = XMLContent.Replace("&#x1F2;", "Dz");
            XMLContent = XMLContent.Replace("&#x1F1;", "DZ");
            XMLContent = XMLContent.Replace("&#x1F3;", "dz");
            XMLContent = XMLContent.Replace("&#xFB00;", "ff");
            XMLContent = XMLContent.Replace("&#xFB01;", "fi");
            XMLContent = XMLContent.Replace("&#xFB02;", "fl");
            XMLContent = XMLContent.Replace("&#xFB03;", "ffi");
            XMLContent = XMLContent.Replace("&#xFB04;", "ffl");
            XMLContent = XMLContent.Replace("&#xFB05;", "ft");
            XMLContent = XMLContent.Replace("&#x132;", "IJ");
            XMLContent = XMLContent.Replace("&#x133;", "ij");
            XMLContent = XMLContent.Replace("&#x1C8;", "Lj");
            XMLContent = XMLContent.Replace("&#x1C7;", "LJ");
            XMLContent = XMLContent.Replace("&#x1C9;", "lj");
            XMLContent = XMLContent.Replace("&#x1CB;", "Nj");
            XMLContent = XMLContent.Replace("&#x1CA;", "NJ");
            XMLContent = XMLContent.Replace("&#x1CC;", "nj");
            return XMLContent;
        }

        #endregion

        #region Invalid Character Handler
        private String Filter(String XMLDocument)
        {
            //XMLDocument = Regex.Replace(XMLDocument, "&#x(.*?);", "", RegexOptions.IgnoreCase);//Commented as it replace the hexa decimal values with space
            return XMLDocument;
        }
        #endregion

        #region Load Data Into Xml Error Handler
        static void FillErrorHandler(object sender, FillErrorEventArgs e)
        {
            // You can use the e.Errors value to determine exactly what
            // went wrong.
            if (e.Errors.GetType() == typeof(System.FormatException))
            {
                String Error = "Error when attempting to update the value: " + e.Values[0];
            }

            // Setting e.Continue to True tells the Load
            // method to continue trying. Setting it to False
            // indicates that an error has occurred, and the 
            // Load method raises the exception that got 
            // you here.
            e.Continue = true;
        }
        #endregion

        #region Remove NUll Objects from JSON
        public string Remove_NullObjects(string json)
        {
            try
            {

                int count_Jobj = 0;


                #region for Handling Empty array and values
                var temp = JObject.Parse(json);
                temp.Descendants()
                    .OfType<JProperty>()
                    .Where(attr => attr.Value.ToString() == "")
                    .ToList() // you should call ToList because you're about to changing the result, which is not possible if it is IEnumerable
                    .ForEach(attr => attr.Remove()); // removing unwanted attributes
                temp.Descendants()
               .OfType<JProperty>()
               .Where(jp => jp.Value.Type == JTokenType.Array && !jp.Value.HasValues)
               .ToList()
               .ForEach(jp => jp.Remove());

                temp.Properties()
                         .Where(attr => attr.Name.Equals("TypeId"))
                              .ToList()
                            .ForEach(attr => attr.Remove());



                var xyz =
                          temp.Descendants()
                         .OfType<JObject>()
                         .Where(jp => jp.Type == JTokenType.Object && !jp.HasValues)
                         .ToList();

                count_Jobj = xyz.Count;
                json = temp.ToString();
                while (count_Jobj > 0)
                {
                    json = json.Replace(" {},", "\"\",");
                    json = json.Replace(" {}", "\"\"");
                    json = json.Replace("{},", "\"\"");
                    json = json.Replace("{}", "\"\"");

                    var temp1 = JObject.Parse(json);
                    temp1.Descendants()
                        .OfType<JProperty>()
                        .Where(attr => attr.Value.ToString() == "")
                        .ToList()
                        .ForEach(attr => attr.Remove());
                    temp1.Descendants()
              .OfType<JProperty>()
              .Where(jp => jp.Value.Type == JTokenType.Array && !jp.Value.HasValues)
              .ToList()
              .ForEach(jp => jp.Remove());

                    xyz =
                          temp1.Descendants()
                         .OfType<JObject>()
                         .Where(jp => jp.Type == JTokenType.Object && !jp.HasValues)
                         .ToList();


                    //.ForEach(jp => jp.Remove());
                    count_Jobj = xyz.Count;
                    json = temp1.ToString();
                }

                //      json = json.Replace("\"\",", "");

                //      json = json.Replace("\"\"", "");
                //      var temp2 = JObject.Parse(json);

                //      var xyz1 = temp2.Descendants()
                //.OfType<JArray>()
                //.Where(jp => jp.Type == JTokenType.Object && !jp.HasValues)
                //.ToList();
                //      int count_jArry = xyz1.Count;


                return json;

            }
            catch (Exception ex)
            {
                return json;
            }
            #endregion
        }

        #endregion

        #region
        public void Clear_DataSet_Aw()
        {
            Source_AwJson.Clear();
            dtAw_Award.Clear();
            dtAw_acronym.Clear();
            dtAw_homePage.Clear();
            dtAw_funds.Clear();
            dtAw_AwardeeDetail.Clear();
            dtAw_AffiliationDetail.Clear();
            dtAw_AwardeeAddress.Clear();
            dt_AwardeeIdentityFier.Clear();
            dtAw_Title.Clear();
            dtAw_Synopsis.Clear();
            dtAW_Keyword.Clear();
            dtAw_FundingDetail.Clear();
            dtAw_Classification.Clear();
            dtAw_RelatedOpportunity.Clear();
            dtAw_RelatedFunder.Clear();
            dtAw_Reviseddate.Clear();
            dtAw_Createddate.Clear();
            dtAw_LeadFunder.Clear();
            dtAw_hasFunder.Clear();
            dtAw_haspart_funds.Clear();
            dtAw_titleFunds.Clear();
            dtAw_RevisedDate.Clear();
            dtAw_revisionhistory.Clear();
            dtAw_createddate.Clear();
            dt_AwlicenseInformation.Clear();

            //dt_PublicationData.Clear();
            //dtpub_Title.Clear();
            //dtpub_identifier.Clear();
            //dtpub_hasFunder.Clear();
            //dt_identifier_ml.Clear();
            //dt_lead_has.Clear();
            //dt_name_outcome.Clear();
            ////dt_createddate.Clear(); 
            //dt_outcomeOfPub.Clear();
            dtAw_Replication.Clear();
        }

        #endregion

        public DataTable jsonToDataTable(string jsonString)
        {
            var jsonLinq = JObject.Parse(jsonString);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();

            //var xyz = jsonLinq.Descendants()
            //             .OfType<JProperty>()
            //             .Where(jp => jp.Type == JProperty)
            //             .ToList();

            var xyz1 = jsonLinq.Descendants()
                      .OfType<JProperty>()
                      .Where(attr => attr.Value.ToString() == "")
                      .ToList();


            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        #region Hexval

        public string Hexval(string HexValue)
        {
            try
            {
                string TextDetail = "";
                string intermed = "";
                char[] names1 = HexValue.ToCharArray(); char[] names2 = "1234567890-=qwertyuiop[]asdfghjkl;'zxcvbnm,./!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:ZXCVBNM<>?".ToCharArray();

                IEnumerable<char> differenceQuery = names1.Except(names2);



                // Execute the query.  
                // Console.WriteLine("The following lines are in names1.txt but not names2.txt");  
                foreach (char s in differenceQuery)
                {

                    HexValue = String.Format("{0:x4}", (uint)System.Convert.ToUInt32(s));
                    HexValue = string.Concat("&#x", HexValue, ";");
                    intermed = TextDetail.Replace(Convert.ToString(s), Convert.ToString(HexValue));

                    TextDetail = intermed;


                }
                return TextDetail;
            }
            catch (Exception ex)
            {
                return HexValue;
            }


        }

        static public string jun(string m)
        {
            m = Regex.Replace(m, @"[^\u0000-\u007F]+", delegate (Match x)
            {
                return hex(x.Value);
            });
            byte[] postBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(m);
            m = Encoding.GetEncoding("UTF-8").GetString(postBytes);
            m = Regex.Replace(m, @"[^\u0000-\u007F]", delegate (Match x)
            {
                return @"&#x" +
                System.Text.Encoding.Unicode.GetBytes(x.Value)
                      .Aggregate("", (agg, val) => val.ToString("X2") + agg) + ";";
            });
            return m;
        }
        static public string hex(string m)
        {
            byte[] postBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(m);
            //
            string decodedText = Encoding.GetEncoding("UTF-8").GetString(postBytes);
            string op = System.Text.Encoding.Unicode.GetBytes(decodedText)
                .Aggregate("", (agg, val) => val.ToString("X2") + agg);
            string pp = System.Text.Encoding.Unicode.GetBytes(m)
                .Aggregate("", (agg, val) => val.ToString("X2") + agg);
            if (op == "FFFD")
            {
                return @"&#x" + pp + ";";
            }
            else
            {
                return m;
            }
        }

        public string EntityToUnicode(string html)
        {
            try
            {
                var replacements = new Dictionary<string, string>();
                var regex = new Regex("(&[a-zA-Z]{2,11};)");
                foreach (Match match in regex.Matches(html))
                {
                    if (!replacements.ContainsKey(match.Value))
                    {
                        if (match.Value.ToString() != "&lt;" && match.Value.ToString() != "&gt;" && match.Value.ToString() != "&quot;" && match.Value.ToString() != "&apos;" && match.Value.ToString() != "&amp;")
                        {
                            var unicode = HttpUtility.HtmlDecode(match.Value);
                            //string hex = ConvertStringToHex(Convert.ToString(unicode));
                            //string hex_rev = ConvertHexToString(hex);
                            if (unicode.Length == 1)
                            {

                                string s = "";
                                //replacements.Add(match.Value, string.Concat("&#", Convert.ToInt32(unicode[0]), ";"));
                                //s = String.Format("0x{0:x4}", (uint)System.Convert.ToUInt32(unicode[0]));
                                s = String.Format("{0:x4}", (uint)System.Convert.ToUInt32(unicode[0]));
                                // replacements.Add(match.Value, string.Concat("&#x00", s, ";"));
                                replacements.Add(match.Value, string.Concat("&#x", s, ";"));

                                //replacements.Add(match.Value, string.Concat("&#x00", String.Format("{0:x2}", (uint)System.Convert.ToUInt32(unicode[0].ToString())), ";"));
                            }
                        }
                    }
                }
                foreach (var replacement in replacements)
                {
                    html = html.Replace(replacement.Key, replacement.Value);
                }
            }
            catch (Exception ex)
            {

            }
            return html;
        }

        public readonly Regex HtmlEntityRegex = new Regex("&(#)?([a-zA-Z0-9]*);");

        public string HtmlDecode(string html)
        {
            try
            {
                if (html.Trim() == "") return html;
                return HtmlEntityRegex.Replace(html, x => x.Groups[1].Value == "#"
                    ? ((char)int.Parse(x.Groups[2].Value)).ToString()
                    : HttpUtility.HtmlDecode(x.Groups[0].Value));
            }
            catch (Exception ex)
            {
                return html;
            }
        }
        public static string anyToHex(string result)
        {
            try
            {
                var abc = from f in Regex.Matches(result, "&#([^;]+);", RegexOptions.IgnoreCase).Cast<Match>().Select(s => s.Groups[1].Value).Distinct()
                          where !f.Contains("x")
                          select new
                          {
                              _rplcVal = "&#" + f + ";",
                              _hexaVal = "&#x" + (Convert.ToInt64(f).ToString("X")).PadLeft(4, '0') + ";"
                          };

                foreach (var item in abc)
                {
                    result = result.Replace(item._rplcVal, item._hexaVal);
                }
                return result;

            }
            catch
            { return result; }


        }

        public static string Utf_to_Html(string fileContent)
        {

            // Store the original file in a different variable...
            string result = fileContent;
            try
            {
                // getting only the list of value greater than 127
                char[] chars = result.ToCharArray().Where(s => Convert.ToInt64(s) > 127).Distinct().ToArray();

                Int32 value = 0;
                foreach (char c in chars)
                {
                    value = Convert.ToInt32(c);
                    if (value != 65533)
                        result = result.Replace(c.ToString(), "&#" + value + ";");
                }
                return result;
            }
            catch
            { return fileContent; }

        }
        public static string RemoveQoteEmbededCRLF(string text)
        {
            bool isQuoted = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                //look for '"' if found toggle isquoted
                if (c == '"') isQuoted = !isQuoted;
                //if CR found inside quoted text
                if (c == '\r' && isQuoted && i < text.Length - 1)
                {
                    //See if LF is next char
                    if (text[i + 1] == '\n')
                    {
                        //CRLF found, replace with space
                        c = ' ';
                        //step over LF
                        i += 1;
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }


        #endregion

        #region SchemaValidation
        public string SchemaValidation(string F_json, string module, string XSDPath)
        {
            #region
            try
            {
                string json = "", schema = "", JsonString = "";
                string JSON_String = F_json;
                string JSON_schema = string.Empty;
                json = json.Trim();

                JObject joItems = JObject.Parse(JSON_String);


                if (JSON_String.Length > 0)
                {

                    String jsonSchemaPath = String.Empty;
                    jsonSchemaPath = Path.GetDirectoryName(XSDPath) + "\\JSON_Schemas";
                    if (module == "FundingBody")
                    {
                        JSON_schema = jsonSchemaPath + "\\FundingBody_JSON_SCHEMA.json";

                        schema = File.ReadAllText(JSON_schema);
                        FundingBody_Wrap fb = new FundingBody_Wrap();
                        fb.FundingBody = joItems;
                        JsonString = JsonConvert.SerializeObject(fb);


                    }

                    else if (module == "Opportunity")
                    {
                        //oop_schema = @"C:\JSON_Schemas\OpportunityBody_JSON_SCHEMA.json";
                        JSON_schema = jsonSchemaPath + "\\OpportunityBody_JSON_SCHEMA.json";
                        schema = File.ReadAllText(JSON_schema);
                        Opportunity_Wrap opp = new Opportunity_Wrap();
                        opp.Opportunity = joItems;
                        JsonString = JsonConvert.SerializeObject(opp);


                    }

                    else if (module == "Award")
                    {
                        //oop_schema = @"C:\JSON_Schemas\OpportunityBody_JSON_SCHEMA.json";
                        JSON_schema = jsonSchemaPath + "\\AwardBody_JSON_SCHEMA.json";
                        schema = File.ReadAllText(JSON_schema);
                        Award_Wrap opp = new Award_Wrap();
                        opp.Award = joItems;
                        JsonString = JsonConvert.SerializeObject(opp);


                    }
                    //else if (ddlschema.SelectedItem.ToString() == "Publication")
                    //{
                    //    //oop_schema = @"C:\JSON_Schemas\PublicationBody_JSON_Schema.json";
                    //    JSON_schema = jsonSchemaPath + "\\PublicationBody_JSON_Schema.json";
                    //    schema = File.ReadAllText(JSON_schema);
                    //    Publication_Wrap pub = new Publication_Wrap();
                    //    pub.Publication = joItems;
                    //    JsonString = JsonConvert.SerializeObject(pub);
                    //}

                    //else if (ddlschema.SelectedItem.ToString() == "Bulk Awards")
                    //{
                    //    JSON_schema = jsonSchemaPath + "\\AwardBody_JSON_SCHEMA.json";
                    //    schema = File.ReadAllText(JSON_schema);
                    //    JSchema json_schema_blk = JSchema.Parse(schema);
                    //    json_schema_blk.AllowAdditionalProperties = false;
                    //    AddAdditionalProperties(json_schema_blk);

                    //    string isValid = Bulk_JSON_Validation(JSON_String, 4, json_schema_blk);
                    //    if (isValid.Trim() == "")
                    //    {
                    //        lblMessage.Visible = true;
                    //        lblMessage.Text = "JSON file is validated Successfully !";
                    //        MessageBox.Show("JSON file is validated Successfully !");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(isValid);
                    //    }
                    //    return;

                    //}

                    //else if (ddlschema.SelectedItem.ToString() == "Bulk Publications")
                    //{
                    //    JSON_schema = jsonSchemaPath + "\\PublicationBody_JSON_Schema.json";
                    //    schema = File.ReadAllText(JSON_schema);
                    //    JSchema json_schema_blk = JSchema.Parse(schema);
                    //    json_schema_blk.AllowAdditionalProperties = false;
                    //    AddAdditionalProperties(json_schema_blk);

                    //    string isValid = Bulk_JSON_Validation(JSON_String, 5, json_schema_blk);
                    //    if (isValid.Trim() == "")
                    //    {
                    //        lblMessage.Visible = true;
                    //        lblMessage.Text = "JSON file is validated Successfully !";
                    //        MessageBox.Show("JSON file is validated Successfully !");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(isValid);
                    //    }
                    //    return;

                    //}

                    //else if (ddlschema.SelectedItem.ToString() == "Bulk Opportunities")
                    //{
                    //    JSON_schema = jsonSchemaPath + "\\OpportunityBody_JSON_SCHEMA.json";
                    //    schema = File.ReadAllText(JSON_schema);
                    //    JSchema json_schema_blk = JSchema.Parse(schema);
                    //    json_schema_blk.AllowAdditionalProperties = false;
                    //    AddAdditionalProperties(json_schema_blk);

                    //    string isValid = Bulk_JSON_Validation(JSON_String, 6, json_schema_blk);
                    //    if (isValid.Trim() == "")
                    //    {
                    //        lblMessage.Visible = true;
                    //        lblMessage.Text = "JSON file is validated Successfully !";
                    //        MessageBox.Show("JSON file is validated Successfully !");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(isValid);
                    //    }
                    //    return;

                    //}


                    //schema = File.ReadAllText(Server.MapPath("~/api-latest.json"));
                    JObject json_obj = JObject.Parse(JsonString);

                    json_obj.Properties()
                          .Where(attr => attr.Name.Equals("TypeId"))
                               .ToList()
                             .ForEach(attr => attr.Remove());



                    // string schema1 = File.ReadAllText(oop_schema);
                    var model = JObject.Parse(JsonString);

                    //var json_schema = JSchema.Parse(schema);
                    JSchema json_schema = JSchema.Parse(schema);

                    json_schema.AllowAdditionalProperties = false;
                    //json_schema.AllowAdditionalItems = false;

                    #region //json schema add prop

                    AddAdditionalProperties(json_schema);



                    // json_schema.AnyOf<Title>.AllowAdditionalProperties = false;
                    //json_schema.AnyOf<Title>.AllowAdditionalProperties = false;

                    //var schema = json_schema.FromType<Employee>(new JsonSchemaGeneratorSettings

                    //{

                    //    AllowAdditionalProperties = true

                    //});
                    #endregion
                    //var json_schema = JsonSchema.Parse(schema);
                    IList<string> messages;
                    // bool valid = model.IsValid(json_schema, out messages);
                    bool valid = json_obj.IsValid(json_schema, out messages);

                    if (valid == true)
                    {

                        return "1";

                    }
                    else
                    {
                        //foreach (var el in messages)
                        //    lblMessage.Visible = true;


                        string messages1 = string.Join(",", messages.ToArray());
                        //lblMessage.Text = "Validation error" + messages1.ToString();

                        return messages1;
                        // File.WriteAllText(@"C:\Temp\json_validation.txt", messages1.ToString());
                        //lblMessage.Text = "Validation error file genrated & copied in C:\\Temp\\ Folder!" + messages1 ;

                    }
                }
                else
                {
                    return "Error in JSON";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion
        }


        #endregion

        public void AddAdditionalProperties(JSchema schema)
        {
            try
            {
                schema.AllowAdditionalProperties = false;
                foreach (var item in schema.Properties.Values)
                {
                    AddAdditionalProperties(item);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static string Sgml_to_Hexadecimal(string fileContent, string xsdpath)
        {
            string result = fileContent;
            try
            {
                string entitiesDB = File.ReadAllText(Path.GetDirectoryName(xsdpath) + "\\JSON_Schemas\\charent.txt");
                var abc = from f in Regex.Matches(result, "&([^ ;]+);", RegexOptions.IgnoreCase).Cast<Match>().Select(s => s.Groups[1].Value).Distinct().Where(_a => _a != null)
                          where !string.IsNullOrEmpty(Regex.Match(entitiesDB, " " + f + "\\s*\"&#(x[A-Z0-9]+)").Groups[1].Value)
                          select new
                          {
                              _rplcVal = "&#" + Regex.Match(entitiesDB, " " + f + "\\s*\"&#(x[A-Z0-9]+)").Groups[1].Value + ";",
                              _OrgVal = "&" + f + ";"
                          };


                foreach (var item in abc)
                {
                    result = result.Replace(item._OrgVal, item._rplcVal);
                }
                return result;

            }
            catch
            {
                return fileContent;
            }
        }

        /* To eliminate Duplicate rows */
        public static DataTable RemoveDuplicates(DataTable dt, string columnname)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (Convert.ToString(dt.Rows[i][columnname].ToString()) == Convert.ToString(dt.Rows[j][columnname].ToString()))
                            {
                                dt.Rows[i].Delete();
                                break;
                            }
                        }
                    }
                    dt.AcceptChanges();
                }
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public static string ChangeAmpercent(string Input)
        {
            try
            {
                //string text = "<p>As candidatur&as a & este con&curso de projetos de I&amp;D mobilizadores devem ser lideradas por unidades de I&amp;D que tenham obtido a classifica&#x00e7;&#x00e3;o de Muito Bom ou Excelente na recente <a href=\"https://www.fct.pt/apoios/unidades&amp;/avaliacoes/2017/index.phtml.pt\">avalia&#x00e7;&#x00e3;o realizada pela FCT</a>. Podem ter como parceiros quaisquer entidades n&#x00e3;o empresariais, sob qualquer forma jur&#x00ed;dica e dimens&#x00e3;o, do Sistema Cient&#x00ed;fico e Tecnol&#x00f3;gico Nacional, nomeadamente institui&#x00e7;&#x00f5;es do ensino superior, seus institutos e unidades de I&amp;D, Laborat&#x00f3;rios do Estado ou internacionais com sede em Portugal, institui&#x00e7;&#x00f5;es privadas sem fins lucrativos que tenham como objeto principal atividades de I&amp;D, e ainda outras institui&#x00e7;&#x00f5;es p&#x00fa;blicas e privadas, sem fins lucrativos, que desenvolvam ou participem em atividades de investiga&#x00e7;&#x00e3;o cient&#x00ed;fica. Em caso de cons&#x00f3;rcios, as candidaturas podem tamb&#x00e9;m incluir entidades da administra&#x00e7;&#x00e3;o central e local e do setor p&#x00fa;blico empresarial e Empresas de qualquer natureza e sob qualquer forma jur&#x00ed;dica.</p>";

                StringBuilder xxx = new StringBuilder();

                var rx = new Regex(@"\s+", RegexOptions.Compiled);
                var data = rx.Split(Input);
                foreach (string txt in data)
                {
                    string Md = "";
                    Md = txt;
                    if (Md.Trim() != "")
                    {
                        #region
                        if ((Md.Contains("&amp;") || Md.Contains("&")))
                        {
                            if (!(Md.Contains("href=\"https://") || Md.Contains("href=\"http://")))
                            {
                                Md = Md.Replace("&amp;", "&#x0026;");
                                if (Md.Contains("&") && (!Md.Contains("&#x")))
                                {
                                    Md = Md.Replace("&", "&#x0026;");
                                }
                            }

                        }
                        #endregion

                        xxx.Append(Md);
                        xxx.Append(" ");

                    }

                }
                return xxx.ToString().Trim();
            }
            catch (Exception ex)
            {
                return Input;
            }

        }

        public static string CappInitial(string initial)
        {
            string str = "";
            Console.WriteLine("Initial String= " + str);

            Console.WriteLine("Displaying first letter of each word...");
            string[] strSplit = initial.Split();
            foreach (string res in strSplit)
            {
                str = str + res.Substring(0, 1) + ".";
            }
            return str.Trim();
        }

        public class ReplacingStringWritingConverter : JsonConverter
        {
            readonly string oldValue;
            readonly string newValue;

            public ReplacingStringWritingConverter(string oldValue, string newValue)
            {
                if (string.IsNullOrEmpty(oldValue))
                    throw new ArgumentException("string.IsNullOrEmpty(oldValue)");
                if (newValue == null)
                    throw new ArgumentNullException("newValue");
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead { get { return false; } }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var s = ((string)value).Replace(oldValue, newValue);
                writer.WriteValue(s);
            }
        }

    }
}


