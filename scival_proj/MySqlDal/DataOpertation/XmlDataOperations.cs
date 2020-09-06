using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using MySqlDal.DataOpertation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MySqlDal
{
    public static class XmlDataOperations
    {
        static Replace replace = new Replace();

        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        public static string GetXmlGenrationLimit(Int64 limits, Int64 select)
        {
            string message;

            if (select == 0)
            {
                if (limits < 1)
                    message = "limits value impractical";
                else
                {
                    var xmlLimits = ScivalEntities.sci_xml_limits.Where(x => x.NAME == "xml_generation_limit").ToList();
                    ScivalEntities.sci_xml_limits.RemoveRange(xmlLimits);

                    sci_xml_limits xml_Limits = new sci_xml_limits
                    {
                        NAME = "xml_generation_limit",
                        LIMITS = limits
                    };

                    ScivalEntities.sci_xml_limits.Add(xml_Limits);
                    ScivalEntities.SaveChanges();

                    message = string.Format("XML Generation Limit set successfully as {0}", limits);
                }
            }
            else if (select == 1)
            {
                var limit = ScivalEntities.sci_xml_limits.Where(x => x.NAME == "xml_generation_limit").FirstOrDefault();

                if (limits < 1)
                    message = "limits value impractical";
                else
                    message = string.Empty;
            }
            else
                message = "select value can be either 0 (for insert) or 1 (for read)";

            return message;
        }

        //public static DataTable GetFundingBody(Int64 mode, String xsdPath, Int64 start, Int64 last)
        //{
        //    String TableName = "";

        //    DataSet Source = new DataSet();
        //    DataSet FundingBody = GetFundingBodyXSDSchema(xsdPath);

        //    DataTable DTXML = new DataTable();

        //    DataColumn xmlColumn = new DataColumn("XML");
        //    DataColumn idsColumn = new DataColumn("IDS");

        //    DTXML.Columns.Add(xmlColumn);
        //    DTXML.Columns.Add(idsColumn);

        //    try
        //    {
        //        GetBody(Source, start, last, mode);
        //        DataSet TempDataset = new DataSet();
        //        Adpter = new OracleDataAdapter("select Name from FundingBodyTables order by sequence", Cn);

        //        Adpter.Fill(TempDataset);

        //        if (TempDataset.Tables.Count > 0)
        //        {
        //            for (Int32 count = 0; count < TempDataset.Tables[0].Rows.Count; count++)
        //            {
        //                for (Int32 countt = 0; countt < FundingBody.Tables.Count; countt++)
        //                {
        //                    if (TempDataset.Tables[0].Rows[count]["Name"].ToString().ToLower() == FundingBody.Tables[countt].TableName.ToLower())
        //                    {
        //                        TableName = FundingBody.Tables[countt].TableName.ToLower();
        //                        Source.Tables[count + 1].TableName = TableName;

        //                        if (TableName.ToLower() == "address")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["ROOM"].ToString() == "")
        //                                    DC["ROOM"] = "Not Available";
        //                                if (DC["STREET"].ToString() == "")
        //                                    DC["STREET"] = "Not Available";
        //                                if (DC["CITY"].ToString() == "")
        //                                    DC["CITY"] = "Not Available";
        //                                if (DC["STATE"].ToString() == "")
        //                                    DC["STATE"] = "Not Available";
        //                                if (DC["POSTALCODE"].ToString() == "")
        //                                    DC["POSTALCODE"] = "Not Available";

        //                                DC.AcceptChanges();
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "canonicalname")
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                                DC.AcceptChanges();

        //                        if (TableName.ToLower() == "item")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC[2].ToString() != "")
        //                                    if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
        //                                        DC[2] = Convert.ToString(replace.ConvertUnicodeToText(DC[2].ToString()));

        //                                DC.AcceptChanges();
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "link")
        //                        {
        //                            string Link_text_MultiLang = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC[2].ToString() != "")
        //                                {
        //                                    Link_text_MultiLang = Convert.ToString(replace.ConvertUnicodeToText(DC[1].ToString()));

        //                                    if (Link_text_MultiLang != "")
        //                                    {
        //                                        DC[1] = Link_text_MultiLang;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                         Multi lang Contact

        //                        if (TableName.ToLower() == "contact")
        //                        {
        //                            string MultilangText = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["type"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["type"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["type"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["email"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["email"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["email"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["title"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["title"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["title"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "contactname")
        //                        {
        //                            string MultilangText = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["prefix"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["prefix"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["prefix"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["givenName"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["givenName"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["givenName"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["middleName"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["middleName"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["middleName"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["surname"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["surname"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["surname"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["suffix"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["suffix"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["suffix"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "website")
        //                        {
        //                            string MultilangText = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["URL"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["URL"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["URL"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["WEBSITE_TEXT"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["WEBSITE_TEXT"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["WEBSITE_TEXT"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "address")
        //                        {
        //                            string MultilangText = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["ROOM"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["ROOM"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["ROOM"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["STREET"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["STREET"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["STREET"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }

        //                                if (DC["CITY"].ToString() != "")
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["CITY"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["CITY"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "org")
        //                        {
        //                            string MultilangText = "";

        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["org_text"].ToString() != "" && DC["org_text"].ToString().Contains("|"))
        //                                {
        //                                    MultilangText = Convert.ToString(replace.ConvertUnicodeToText(DC["org_text"].ToString()));

        //                                    if (MultilangText != "")
        //                                    {
        //                                        DC["org_text"] = MultilangText;
        //                                        DC.AcceptChanges();
        //                                    }
        //                                }
        //                            }
        //                        }

        //                         Multilang Contact

        //                        if (TableName.ToLower() == "preferredorgname")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC[1].ToString() == "")
        //                                    DC[1] = "Not Available";

        //                                if (replace.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
        //                                {
        //                                    string chk_preON = "";
        //                                    chk_preON = Convert.ToString(replace.ConvertUnicodeToText(DC[1].ToString()));

        //                                    if (chk_preON != "")
        //                                        DC[1] = Convert.ToString(replace.ConvertUnicodeToText(DC[1].ToString()));
        //                                }

        //                                DC.AcceptChanges();
        //                            }
        //                        }

        //                        if (TableName.ToLower() == "contextname")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                                DC.AcceptChanges();
        //                        }

        //                        if (TableName.ToLower() == "abbrevname")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                                DC.AcceptChanges();
        //                        }

        //                        if (TableName.ToLower() == "acronym")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                                DC.AcceptChanges();
        //                        }

        //                        if (TableName.ToLower() == "opportunitiessource" || TableName.ToLower() == "awardssource")
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                FundingBody.Tables[TableName].Columns[0].DataType = Source.Tables[TableName].Columns[0].DataType;
        //                                DC.AcceptChanges();
        //                            }
        //                        }

        //                        if (TableName.ToLower() == Convert.ToString("fundingbody").ToLower())
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["RECORDSOURCE"].ToString() != "")
        //                                {
        //                                    string recordSource = DC["RECORDSOURCE"].ToString();
        //                                    DC["RECORDSOURCE"] = null;
        //                                    DC["RECORDSOURCE"] = "\"" + recordSource.ToString() + "\"";
        //                                }

        //                                DC.AcceptChanges();
        //                                Source.Tables[TableName].AcceptChanges();
        //                            }
        //                        }

        //                        if (TableName.ToLower() == Convert.ToString("contact").ToLower())
        //                        {
        //                            foreach (DataRow DC in Source.Tables[TableName].Rows)
        //                            {
        //                                if (DC["email"].ToString() != "")
        //                                {
        //                                    string email = DC["email"].ToString();
        //                                    DC["email"] = null;
        //                                    DC["email"] = "\"" + email.ToString() + "\"";
        //                                }

        //                                DC.AcceptChanges();
        //                                Source.Tables[TableName].AcceptChanges();
        //                            }
        //                        }

        //                        foreach (DataRow DR in Source.Tables[TableName].Rows)
        //                        {
        //                            try
        //                            {
        //                                DR["fundingbody_id"] = Convert.ToInt32(DR["fundingbody_id"].ToString().Substring(4, 8));
        //                                DR.AcceptChanges();
        //                            }
        //                            catch { }
        //                        }
        //                        try
        //                        {
        //                            DataTableReader reader = new DataTableReader(Source.Tables[count + 1]);
        //                            FundingBody.Load(reader, LoadOption.OverwriteChanges, FillErrorHandler, FundingBody.Tables[TableName]);
        //                        }
        //                        catch { }
        //                    }
        //                }
        //            }
        //        }

        //         Added for Schema v3.0

        //        XmlDocument Data = new XmlDocument();

        //        if (FundingBody.Tables.Count > 0)
        //        {
        //            StringWriter stringWriter = new StringWriter();
        //            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

        //            FundingBody.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);

        //            string contentAsXmlString = stringWriter.ToString();
        //            contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <fundingBodies xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1/fundingBodies41.xsd\"");
        //            contentAsXmlString = contentAsXmlString.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
        //            contentAsXmlString = contentAsXmlString.Replace("</NewDataSet>", "</fundingBodies>");
        //            contentAsXmlString = contentAsXmlString.Replace("<canonicalName d3p1:lang=\"en\" xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\">Not Available</canonicalName>", "<canonicalName />");
        //            contentAsXmlString = contentAsXmlString.Replace("<contextName d3p1:lang=\"en\" xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\">Not Available</contextName>", "<contextName />");
        //            contentAsXmlString = contentAsXmlString.Replace("<abbrevName d3p1:lang=\"en\" xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\">Not Available</abbrevName>", "<abbrevName />");
        //            // Replace "d3p1:lang" to "xml:lang"
        //            contentAsXmlString = contentAsXmlString.Replace("d3p1:lang", "xml:lang");
        //            // Replace xmlns:d3p1="http://www.w3.org/XML/1998/namespace" in ""
        //            contentAsXmlString = contentAsXmlString.Replace("xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\"", "");
        //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<address country=\"(.*?)\">(.*?)</address>", "<address country=\"$1\">$2<country>$1</country></address>");
        //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<establishmentDate>(.*?)-(.*?)-(.*?)</establishmentDate>", "<establishmentDate>$1</establishmentDate>");
        //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<fundingBodies(.*?)>", "<fundingBodies xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-4.1\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1 http://www.elsevier.com/xml/schema/grant/grant-4.1/fundingBodies41.xsd\">");
        //            contentAsXmlString = contentAsXmlString.Replace("xml:space=\"preserve\"", "");
        //            contentAsXmlString = contentAsXmlString.Replace("<eligibilityDescription >", "<eligibilityDescription>");
        //            contentAsXmlString = contentAsXmlString.Replace("xsi:nil=\"true\"", "");
        //            contentAsXmlString = contentAsXmlString.Replace("&amp;#", "&#");
        //            contentAsXmlString = contentAsXmlString.Replace("&amp;lt;", "&lt;");
        //            contentAsXmlString = contentAsXmlString.Replace("&amp;gt;", "&gt;");
        //            contentAsXmlString = replace.ReadandReplaceHexaToChar(contentAsXmlString, xsdPath.Replace("\\XSD", ""));
        //            contentAsXmlString = contentAsXmlString.Replace("<room>Not Available</room>", "<room></room>").Replace("<street>Not Available</street>", "<street></street>");
        //            contentAsXmlString = contentAsXmlString.Replace("<city>Not Available</city>", "<city></city>").Replace("<state>Not Available</state>", "<state></state>");
        //            contentAsXmlString = contentAsXmlString.Replace("<postalCode>Not Available</postalCode>", "<postalCode></postalCode>");
        //            contentAsXmlString = contentAsXmlString.Replace(">0</totalFunding>", ">not-specified</totalFunding>");
        //            contentAsXmlString = Regex.Replace(contentAsXmlString, "\\s+(</[-A-Za-z]+>)", "$1");
        //            contentAsXmlString = Regex.Replace(contentAsXmlString, "(<[-A-Za-z]+>)\\s+", "$1");
        //            contentAsXmlString = contentAsXmlString.Replace("<recordSource>" + "\"", "<recordSource>");
        //            contentAsXmlString = contentAsXmlString.Replace("\"" + "</recordSource>", "</recordSource>");
        //            contentAsXmlString = contentAsXmlString.Replace("<email>" + "\"", "<email>");
        //            contentAsXmlString = contentAsXmlString.Replace("\"" + "</email>", "</email>");

        //             Hex Values which have to replaced into space

        //            contentAsXmlString = replace.EntityToUnicode2(contentAsXmlString);
        //            contentAsXmlString = replace.EntityToUnicode(contentAsXmlString);
        //            contentAsXmlString = contentAsXmlString.Replace("&#x00A0;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2002;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2003;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2004;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2005;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2006;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2007;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2008;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x2009;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x200A;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x200B;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x3000;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#xFEFF;", " ");

        //            

        //            

        //            contentAsXmlString = contentAsXmlString.Replace("xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");
        //            contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");
        //            contentAsXmlString = contentAsXmlString.Replace("<region xml:lang=\"en\">", "<region>");
        //            contentAsXmlString = contentAsXmlString.Replace("<region xml:lang=\"en\" >", "<region>");
        //            contentAsXmlString = contentAsXmlString.Replace("<acronym xml:lang=\"en\">Not Available</acronym>", "<acronym></acronym>").Replace("<acronym xml:lang=\"en\" >Not Available</acronym>", "<acronym></acronym>");
        //            contentAsXmlString = contentAsXmlString.Replace("<abbrevName xml:lang=\"en\">Not Available</abbrevName>", "<abbrevName></abbrevName>").Replace("<abbrevName xml:lang=\"en\" >Not Available</abbrevName>", "<abbrevName></abbrevName>");
        //            contentAsXmlString = contentAsXmlString.Replace("<contextname xml:lang=\"en\">Not Available</contextname>", "<contextname></contextname>").Replace("<contextname xml:lang=\"en\" >Not Available</contextname>", "<contextname></contextname>");
        //            contentAsXmlString = contentAsXmlString.Replace("<canonicalname xml:lang=\"en\">Not Available</canonicalname>", "<canonicalname></canonicalname>").Replace("<canonicalname xml:lang=\"en\" >Not Available</canonicalname>", "<canonicalname></canonicalname>");
        //            contentAsXmlString = contentAsXmlString.Replace("&amp;nbsp;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&nbsp;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("&#x00A0;", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("  ", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("   ", " ");
        //            contentAsXmlString = contentAsXmlString.Replace("countryTest", "country");

        //            

        //            MatchCollection Col = Regex.Matches(contentAsXmlString, "<preferredOrgName>([^<]*?)(\\–)+([^<]*?)</preferredOrgName>");
        //            int lencount = 0;

        //            foreach (Match Matchstr in Col)
        //            {
        //                int len = contentAsXmlString.Length;
        //                int diff = Matchstr.Value.Length;
        //                contentAsXmlString = contentAsXmlString.Substring(0, Matchstr.Index + lencount) + Regex.Replace(Matchstr.Value, "–", "&#x0096;") + contentAsXmlString.Substring((Matchstr.Index + lencount + Matchstr.Length), len - (Matchstr.Index + lencount + Matchstr.Length));
        //                lencount = lencount + (Regex.Replace(Matchstr.Value, "–", "&#x0096;").Length - diff);
        //            }

        //            Col = Regex.Matches(contentAsXmlString, "<preferredOrgName>([^<]*?)(\\—)+([^<]*?)</preferredOrgName>");
        //            lencount = 0;

        //            foreach (Match Matchstr in Col)
        //            {
        //                int len = contentAsXmlString.Length;
        //                int diff = Matchstr.Value.Length;
        //                contentAsXmlString = contentAsXmlString.Substring(0, Matchstr.Index + lencount) + Regex.Replace(Matchstr.Value, "—", "&#x0097;") + contentAsXmlString.Substring((Matchstr.Index + lencount + Matchstr.Length), len - (Matchstr.Index + lencount + Matchstr.Length));
        //                lencount = lencount + (Regex.Replace(Matchstr.Value, "—", "&#x0097;").Length - diff);
        //            }

        //            Col = Regex.Matches(contentAsXmlString, "<contextName>([^<]*?)(\\-)+([^<]*?)</contextName>");
        //            lencount = 0;

        //            foreach (Match Matchstr in Col)
        //            {
        //                int len = contentAsXmlString.Length;
        //                int diff = Matchstr.Value.Length;
        //                contentAsXmlString = contentAsXmlString.Substring(0, Matchstr.Index + lencount) + Regex.Replace(Matchstr.Value, "–", "&#x0096;") + contentAsXmlString.Substring((Matchstr.Index + lencount + Matchstr.Length), len - (Matchstr.Index + lencount + Matchstr.Length));
        //                lencount = lencount + (Regex.Replace(Matchstr.Value, "–", "&#x0096;").Length - diff);
        //            }

        //            Col = Regex.Matches(contentAsXmlString, "<contextName>([^<]*?)(\\—)+([^<]*?)</contextName>");
        //            lencount = 0;

        //            foreach (Match Matchstr in Col)
        //            {
        //                int len = contentAsXmlString.Length;
        //                int diff = Matchstr.Value.Length;
        //                contentAsXmlString = contentAsXmlString.Substring(0, Matchstr.Index + lencount) + Regex.Replace(Matchstr.Value, "—", "&#x0097;") + contentAsXmlString.Substring((Matchstr.Index + lencount + Matchstr.Length), len - (Matchstr.Index + lencount + Matchstr.Length));
        //                lencount = lencount + (Regex.Replace(Matchstr.Value, "—", "&#x0097;").Length - diff);
        //            }

        //            contentAsXmlString = HandleLegation(contentAsXmlString);
        //            contentAsXmlString = Filter(contentAsXmlString);

        //            Data.LoadXml(contentAsXmlString);

        //            SequenceFundingBodyXML(Data);
        //        }

        //        String Error = ValidateXML(Data, 2, xsdPath);
        //        Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-4.1", "");
        //        Error = Error.Replace("'", ""); Error = Error.Replace(":", "");
        //        Error = Error.Replace("in namespace", "");

        //        StringBuilder ids = new StringBuilder();
        //        string sep = "";

        //        foreach (DataRow idr in Source.Tables[0].Rows)
        //        {
        //            ids.Append(sep);
        //            ids.Append(idr[0].ToString());
        //            sep = ",";
        //        }

        //        DataRow DRxml = DTXML.NewRow();

        //        if (Error == "1")
        //        {
        //            DRxml[0] = Data.InnerXml;
        //            DRxml[1] = ids;
        //            DTXML.Rows.Add(DRxml);
        //            return DTXML;
        //        }
        //        else
        //        {
        //            DRxml[0] = "<Error>" + Error + "</Error>";
        //            DRxml[1] = ids;
        //            DTXML.Rows.Add(DRxml);
        //            return DTXML;
        //        }

        //        
        //    }
        //    catch (Exception ex)
        //    {
        //        DataRow DRxml = DTXML.NewRow();
        //        DRxml[0] = "<Error>" + ex.Message + "</Error>"; ;
        //        DRxml[1] = "";
        //        DTXML.Rows.Add(DRxml);
        //        return DTXML;
        //    }
        //}

        //private static DataSet GetFundingBodyXSDSchema(String XsdPath)
        //{
        //    String Path = XsdPath + "\\FundingBodies_ForFillData4.1.xsd";

        //    DataSet fundingBodySchema = new DataSet();
        //    fundingBodySchema.ReadXmlSchema(Path);

        //    DataRelation DR = fundingBodySchema.Tables["fundingbodies"].ChildRelations[0];

        //    fundingBodySchema.Tables["fundingbodies"].ChildRelations.Remove(DR);
        //    fundingBodySchema.Tables["fundingbody"].Constraints.Remove("fundingBodies_fundingBody");
        //    fundingBodySchema.Tables.Remove("fundingbodies");
        //    fundingBodySchema.Tables["fundingbody"].Columns.Remove("fundingbodies_id");

        //    return fundingBodySchema;
        //}

        //static void FillErrorHandler(object sender, FillErrorEventArgs e)
        //{
        //    e.Continue = true;
        //}

        static string Error_text_deatils_p = "JSON file genrate successfully for Awards Module and Publication Module! & File copied in temp Folder";
        static string Publication_stop_check = "0";
        static DataSet Source_AwJson = new DataSet();
        static DataSet Source_PubJson = new DataSet();
        static DataTable dtAw_Award = new DataTable();
        static DataTable dtAw_acronym = new DataTable();
        static DataTable dtAw_homePage = new DataTable();
        static DataTable dtAw_funds = new DataTable();
        static DataTable dtAw_AwardeeDetail = new DataTable();
        static DataTable dtAw_AffiliationDetail = new DataTable();
        static DataTable dtAw_AwardeeAddress = new DataTable();
        static DataTable dt_AwardeeIdentityFier = new DataTable();
        static DataTable dtAw_Title = new DataTable();
        static DataTable dtAw_Synopsis = new DataTable();
        static DataTable dtAW_Keyword = new DataTable();
        static DataTable dtAw_FundingDetail = new DataTable();
        static DataTable dtAw_Classification = new DataTable();
        static DataTable dtAw_RelatedOpportunity = new DataTable();
        static DataTable dtAw_RelatedFunder = new DataTable();
        static DataTable dtAw_Reviseddate = new DataTable();
        static DataTable dtAw_Createddate = new DataTable();
        static DataTable dtAw_LeadFunder = new DataTable();
        static DataTable dtAw_hasFunder = new DataTable();
        static DataTable dtAw_haspart_funds = new DataTable();
        static DataTable dtAw_titleFunds = new DataTable();
        static DataTable dtAw_RevisedDate = new DataTable();
        static DataTable dtAw_revisionhistory = new DataTable();
        static DataTable dtAw_createddate = new DataTable();
        static DataTable dt_PublicationData = new DataTable();
        static DataTable dtpub_identifier = new DataTable();
        static DataTable dtpub_RelatedFunder = new DataTable();
        static DataTable dtpub_Reviseddate = new DataTable();
        static DataTable dtpub_Createddate = new DataTable();
        static DataTable dtpub_LeadFunder = new DataTable();
        static DataTable dtpub_hasFunder = new DataTable();
        static DataTable dtpub_Title = new DataTable();
        static DataTable dt_identifier_ml = new DataTable();
        static DataTable dt_lead_has = new DataTable();
        static DataTable dt_name_outcome = new DataTable();
        static DataTable dt_outcomeOfPub = new DataTable();
        static DataTable dt_createddate = new DataTable();

        public static String AwardValidate(Int64 WorkFlowId, String XSDPath, int tranTypeId)
        {
            DataTable DTXML = new DataTable();
            DataColumn XMLcol = new DataColumn("XML");
            DTXML.Columns.Add(XMLcol);
            XMLcol = new DataColumn("IDS");
            DTXML.Columns.Add(XMLcol);
            DataSet Award = GetAwardXSDSchema(XSDPath);
            String TableName = "";
            DataSet Source = new DataSet();

            try
            {
                //GetAwardBodyForValidation(Source_AwJson, WorkFlowId, tranTypeId);

                //GetPublicationBodyForValidation(Source_PubJson, WorkFlowId, tranTypeId);

                Publication_stop_check = "1";

                Source = Source_AwJson;

                if (Publication_stop_check == "1")
                {
                    Source = Source_PubJson;
                }
                if (Convert.ToString(Source.Tables["ERRORCODE"].Rows[0][0]) == "1")
                {
                    String Error = Source.Tables["ERRORCODE"].Rows[0][1].ToString();
                    return Error;
                }
                else
                {
                    var awardTableNames = ScivalEntities.award_tables.OrderBy(a => a.SEQUENCE).Select(a => a.NAME).ToList();

                    for (Int32 count = 0; count < awardTableNames.Count; count++)
                    {
                        for (Int32 countt = 0; countt < Award.Tables.Count; countt++)
                        {
                            if (awardTableNames[count].ToLower() == Award.Tables[countt].TableName.ToLower())
                            {
                                TableName = Award.Tables[countt].TableName.ToLower();
                                Source.Tables[count].TableName = TableName;

                                if (TableName.ToLower() == "address")
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["ROOM"].ToString() == "")
                                        {
                                            DC["ROOM"] = "Not Available";
                                        }
                                        if (DC["STREET"].ToString() == "")
                                        {
                                            DC["STREET"] = "Not Available";
                                        }
                                        if (DC["CITY"].ToString() == "")
                                        {
                                            DC["CITY"] = "Not Available";
                                        }
                                        if (DC["STATE"].ToString() == "")
                                        {
                                            DC["STATE"] = "Not Available";
                                        }
                                        if (DC["POSTALCODE"].ToString() == "")
                                        {
                                            DC["POSTALCODE"] = "Not Available";
                                        }
                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }
                                if (TableName.ToLower() == "awardaddress" || TableName.ToLower() == "location")
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["ROOM"].ToString() == "")
                                        {
                                            DC["ROOM"] = "Not Available";
                                        }
                                        if (DC["STREET"].ToString() == "")
                                        {
                                            DC["STREET"] = "Not Available";
                                        }
                                        if (DC["CITY"].ToString() == "")
                                        {
                                            DC["CITY"] = "Not Available";
                                        }
                                        if (DC["STATE"].ToString() == "")
                                        {
                                            DC["STATE"] = "Not Available";
                                        }
                                        if (DC["POSTALCODE"].ToString() == "")
                                        {
                                            DC["POSTALCODE"] = "Not Available";
                                        }
                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == "dept")
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["DEPT_TEXT"].ToString() == "")
                                        {
                                            DC["DEPT_TEXT"] = "Not Available";
                                        }

                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == "itemtest")
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {

                                        if (DC["itemtest_Id"].ToString() == "")
                                        {
                                            DC["itemtest_Id"] = 0;
                                        }
                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == Convert.ToString("award").ToLower())
                                {

                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["RECORDSOURCE"].ToString() != "")
                                        {
                                            string recordSource = DC["RECORDSOURCE"].ToString();
                                            DC["RECORDSOURCE"] = null;
                                            DC["RECORDSOURCE"] = "\"" + recordSource.ToString() + "\"";
                                        }

                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == Convert.ToString("contact").ToLower())
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["email"].ToString() != "")
                                        {
                                            string email = DC["email"].ToString();
                                            DC["email"] = null;
                                            DC["email"] = "\"" + email.ToString() + "\"";
                                        }

                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == Convert.ToString("awardManager").ToLower())
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["email"].ToString() != "")
                                        {
                                            string email = DC["email"].ToString();
                                            DC["email"] = null;
                                            DC["email"] = "\"" + email.ToString() + "\"";
                                        }

                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                if (TableName.ToLower() == "awardeename" || TableName.ToLower() == "indexedname" || TableName.ToLower() == "givenname" || TableName.ToLower() == "surname" || TableName.ToLower() == "initials")
                                {
                                    int awardeename_id = 0;

                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        if (DC["awardeename_id"].ToString() == "")
                                        {
                                            DC["awardeename_id"] = awardeename_id;
                                            awardeename_id++;
                                        }

                                        DC.AcceptChanges();
                                        Source.Tables[TableName].AcceptChanges();
                                    }
                                }

                                foreach (DataRow DR in Source.Tables[TableName].Rows)
                                {
                                    try
                                    {
                                        DR["award_id"] = Convert.ToInt32(DR["award_id"].ToString().Substring(4, 8));
                                        DR.AcceptChanges();
                                    }
                                    catch { }
                                }

                                if (TableName.ToLower() == "installment")
                                {
                                    foreach (DataRow DC in Source.Tables[TableName].Rows)
                                    {
                                        Award.Tables[TableName].Columns[0].DataType = Source.Tables[TableName].Columns[0].DataType;
                                        Award.Tables[TableName].Columns[1].DataType = Source.Tables[TableName].Columns[0].DataType;

                                        DC.AcceptChanges();
                                    }
                                }

                                DataTableReader reader = new DataTableReader(Source.Tables[count]);
                                Award.Load(reader, LoadOption.OverwriteChanges, FillErrorHandler, Award.Tables[TableName]);
                            }
                        }
                    }

                    if (Award.Tables.Count > 0)
                    {
                        dtAw_Award = Source_AwJson.Tables["AwardXML"];
                        dtAw_Title = Source_AwJson.Tables["AwardXML29"];
                        dtAw_Synopsis = Source_AwJson.Tables["AwardXML30"];
                        dtAW_Keyword = Source_AwJson.Tables["AwardXML3"];
                        dtAw_FundingDetail = Source_AwJson.Tables["AwardXML40"];
                        dtAw_Classification = Source_AwJson.Tables["AwardXML12"];
                        dtAw_RelatedOpportunity = Source_AwJson.Tables["AwardXML46"];
                        dtAw_RelatedFunder = Source_AwJson.Tables["AwardXML7"];
                        dtAw_Reviseddate = Source_AwJson.Tables["AwardXML9"];
                        dtAw_Createddate = Source_AwJson.Tables["AwardXML10"];
                        dtAw_AwardeeDetail = Source_AwJson.Tables["AwardXML53"];
                        dtAw_AffiliationDetail = Source_AwJson.Tables["AwardXML54"];
                        dtAw_AwardeeAddress = Source_AwJson.Tables["AwardXML55"];
                        dt_AwardeeIdentityFier = Source_AwJson.Tables["AwardXML56"];
                        dtAw_funds = Source_AwJson.Tables["AwardXML57"];
                        dtAw_haspart_funds = Source_AwJson.Tables["AwardXML58"];
                        dtAw_titleFunds = Source_AwJson.Tables["AwardXML59"];
                        dtAw_RevisedDate = Source_AwJson.Tables["AwardXML9"];
                        dtAw_revisionhistory = Source_AwJson.Tables["AwardXML1"];
                        dtAw_createddate = Source_AwJson.Tables["AwardXML10"];

                        if (Publication_stop_check == "1")
                        {
                            dt_PublicationData = Source_PubJson.Tables["AwardXML47"];
                            dtpub_Title = Source_PubJson.Tables["AwardXML48"];
                            dtpub_identifier = Source_PubJson.Tables["AwardXML49"];
                            dtpub_hasFunder = Source_PubJson.Tables["AwardXML50"];
                            dt_identifier_ml = Source_PubJson.Tables["AwardXML51"];
                            dt_lead_has = Source_PubJson.Tables["AwardXML7"];
                            dt_name_outcome = Source_PubJson.Tables["AwardXML29"];
                            dt_createddate = Source_PubJson.Tables["AwardXML10"];
                            dt_outcomeOfPub = Source_PubJson.Tables["AwardXML52"];

                            JsonCreationFromModel_Award();

                            if (dt_PublicationData.Rows.Count > 0)
                            {
                                JsonCreationFromModel_Publication();
                            }
                            else
                            {
                                Clear_DataSet_Aw();
                                Source_PubJson.Clear();
                            }
                        }
                        else
                        {
                            JsonCreationFromModel_Award();
                        }
                    }

                    XmlDocument Data = new XmlDocument();

                    if (Award.Tables.Count > 0)
                    {
                        // Create a string writer that will write the Xml to a string
                        StringWriter stringWriter = new StringWriter();
                        // The Xml Text writer acts as a bridge between the xml stream and the text stream
                        XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                        // Now take the Dataset and extract the Xml from it, it will write to the string writer
                        Award.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);
                        // Write the Xml out to a string
                        string contentAsXmlString = stringWriter.ToString();
                        contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> <awards xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1/awards41.xsd\"");
                        contentAsXmlString = contentAsXmlString.Replace("</NewDataSet>", "</awards>");
                        // Replace "d3p1:lang" to "xml:lang"
                        contentAsXmlString = contentAsXmlString.Replace("d3p1:lang", "xml:lang");
                        // Replace xmlns:d3p1="http://www.w3.org/XML/1998/namespace" in ""
                        contentAsXmlString = contentAsXmlString.Replace(" xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("<awardaddress", "<address");
                        contentAsXmlString = contentAsXmlString.Replace("</awardaddress>", "</address>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<address country=\"(.*?)\">(.*?)</address>", "<address country=\"$1\">$2<country>$1</country></address>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<address>(.*?)<country>(.*?)</country></address>", "<address country=\"$2\">$1<country>$2</country></address>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<startDate>(.*?)T(.*?)</startDate>", "<startDate>$1</startDate>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<lastAmendedDate>(.*?)T(.*?)</lastAmendedDate>", "<lastAmendedDate>$1</lastAmendedDate>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<endDate>(.*?)T(.*?)</endDate>", "<endDate>$1</endDate>");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<awards(.*?)>", "<awards xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-4.1\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1/awards41.xsd\">");
                        contentAsXmlString = contentAsXmlString.Replace("xml:space=\"preserve\"", "");
                        // load the string of Xml Int64o the document
                        contentAsXmlString = contentAsXmlString.Replace("xsi:nil=\"true\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("<room>Not Available</room>", "<room></room>");
                        contentAsXmlString = contentAsXmlString.Replace("<street>Not Available</street>", "<street></street>");
                        contentAsXmlString = contentAsXmlString.Replace("<city>Not Available</city>", "<city></city>");
                        contentAsXmlString = contentAsXmlString.Replace("<state>Not Available</state>", "<state></state>");
                        contentAsXmlString = contentAsXmlString.Replace("<postalCode>Not Available</postalCode>", "<postalCode></postalCode>");
                        contentAsXmlString = contentAsXmlString.Replace(">0</amount>", ">not-specified</amount>");

                        // Replace "itemtest" to "item"
                        contentAsXmlString = contentAsXmlString.Replace("itemtest", "item");
                        contentAsXmlString = contentAsXmlString.Replace("&amp;#", "&#");
                        contentAsXmlString = contentAsXmlString.Replace("&amp;lt;", "&lt;");
                        contentAsXmlString = contentAsXmlString.Replace("&amp;gt;", "&gt;");
                        contentAsXmlString = replace.ReadandReplaceHexaToChar(contentAsXmlString, XSDPath.Replace("\\XSD", ""));
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "\\s+(</[-A-Za-z]+>)", "$1");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "(<[-A-Za-z]+>)\\s+", "$1");

                        contentAsXmlString = replace.EntityToUnicode2(contentAsXmlString);
                        contentAsXmlString = replace.EntityToUnicode(contentAsXmlString);
                        contentAsXmlString = contentAsXmlString.Replace("&#x00A0;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2002;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2003;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2004;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2005;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2006;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2007;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2008;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x2009;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x200A;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x200B;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#x3000;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("&#xFEFF;", " ");


                        contentAsXmlString = contentAsXmlString.Replace("<location countryTest=", "<location country=");
                        contentAsXmlString = contentAsXmlString.Replace("<address countryTest=", " <address country=");
                        contentAsXmlString = contentAsXmlString.Replace("ORG", "org");
                        contentAsXmlString = contentAsXmlString.Replace(" xmlns:d6p1=\"http://www.w3.org/XML/1998/namespace\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("d6p1:lang", "xml:lang");
                        contentAsXmlString = contentAsXmlString.Replace(" xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");
                        contentAsXmlString = contentAsXmlString.Replace(" xmlns:d7p1=\"http://www.w3.org/XML/1998/namespace\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("d7p1:lang", "xml:lang");
                        contentAsXmlString = contentAsXmlString.Replace("orgtest", "org");
                        contentAsXmlString = contentAsXmlString.Replace("<COUNTRYTEST xmlns=\"\">", "<country>");
                        contentAsXmlString = contentAsXmlString.Replace("COUNTRYTEST", "country");
                        contentAsXmlString = contentAsXmlString.Replace("<recordSource>" + "\"", "<recordSource>");
                        contentAsXmlString = contentAsXmlString.Replace("\"" + "</recordSource>", "</recordSource>");
                        contentAsXmlString = contentAsXmlString.Replace("<email>" + "\"", "<email>");
                        contentAsXmlString = contentAsXmlString.Replace("\"" + "</email>", "</email>");
                        contentAsXmlString = contentAsXmlString.Replace(@"<itemid xmlns=""""", "<itemId");
                        contentAsXmlString = contentAsXmlString.Replace("</itemid>", "</itemId>");
                        contentAsXmlString = contentAsXmlString.Replace("<itemId  />", "");
                        contentAsXmlString = contentAsXmlString.Replace("<itemId />", "");
                        contentAsXmlString = contentAsXmlString.Replace("<researchOutcome  />", "");
                        contentAsXmlString = contentAsXmlString.Replace("<researchOutcome />", "");
                        contentAsXmlString = contentAsXmlString.Replace("&amp;nbsp;", " ");
                        contentAsXmlString = contentAsXmlString.Replace("  ", " ");
                        contentAsXmlString = contentAsXmlString.Replace("   ", " ");
                        contentAsXmlString = contentAsXmlString.Replace("xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");
                        contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");
                        contentAsXmlString = contentAsXmlString.Replace("<amount currency=\"USD\">not-specified</amount>", "");
                        contentAsXmlString = contentAsXmlString.Replace("<totalAmount amount=\"0\"", "<totalAmount amount=\"not-specified\"");
                        contentAsXmlString = Regex.Replace(contentAsXmlString, "<awardNoticeDate>(.*?)T(.*?)</awardNoticeDate>", "<awardNoticeDate>$1</awardNoticeDate>");
                        contentAsXmlString = HandleLegation(contentAsXmlString);
                        contentAsXmlString = Filter(contentAsXmlString);
                    }

                    String Error = ValidateXML(Data, 4, XSDPath);
                    Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-4.1", "");
                    Error = Error.Replace("'", ""); Error = Error.Replace(":", "");
                    Error = Error.Replace("in namespace", "");
                    Error = Error_text_deatils_p;
                    return Error;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static DataSet GetAwardXSDSchema(String XsdPath)
        {
            String Path = XsdPath + "\\Awards_ForFillData4.1.xsd";
            DataSet FBSCHEMA = new DataSet();
            FBSCHEMA.ReadXmlSchema(Path);
            DataRelation DR = FBSCHEMA.Tables["awards"].ChildRelations[0];
            FBSCHEMA.Tables["awards"].ChildRelations.Remove(DR);
            FBSCHEMA.Tables["award"].Constraints.Remove("awards_award");//
            FBSCHEMA.Tables.Remove("awards");
            FBSCHEMA.Tables["award"].Columns.Remove("awards_id");
            DR = FBSCHEMA.Tables["awardmanager"].ChildRelations[2];
            FBSCHEMA.Tables["awardmanager"].ChildRelations.Remove(DR);
            FBSCHEMA.Tables["address"].Constraints.Remove("awardManager_address");
            FBSCHEMA.Tables["address"].Columns.Remove("awardManager_Id");
            DataTable AwardAddress = new DataTable();
            DataColumn DC = new DataColumn("room", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("street", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("city", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("state", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("postalCode", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("country", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("countryTest", Type.GetType("System.String"));
            DC.AllowDBNull = false;
            AwardAddress.Columns.Add(DC);
            DC = new DataColumn("awardmanager_id", Type.GetType("System.Int32"));
            DC.AllowDBNull = true;
            DC.ColumnMapping = MappingType.Hidden;
            AwardAddress.Columns.Add(DC);
            AwardAddress.TableName = "awardaddress";
            FBSCHEMA.Tables.Add(AwardAddress);
            DR = new DataRelation("awardManager_awardaddress", FBSCHEMA.Tables["awardmanager"].Columns["awardmanager_id"], FBSCHEMA.Tables["awardaddress"].Columns["awardmanager_id"], false);
            DR.Nested = true;
            FBSCHEMA.Relations.Add(DR);
            return FBSCHEMA;
        }

        static void FillErrorHandler(object sender, FillErrorEventArgs e)
        {
            if (e.Errors.GetType() == typeof(System.FormatException))
            {
                _ = "Error when attempting to update the value: " + e.Values[0];
            }

            e.Continue = true;
        }

        public static void JsonCreationFromModel_Award()
        {
            string updatedjson = string.Empty;
            try
            {
                DataView dataView = new DataView();
                DataView dataView1 = new DataView();
                dataView = dtAw_RelatedFunder.DefaultView;
                dataView1 = dtAw_RelatedFunder.DefaultView;
                dataView.RowFilter = "HIERARCHY = 'lead'";
                dtAw_LeadFunder = dataView.ToTable();
                dtAw_hasFunder = dataView1.ToTable();

                AwardJson_Model Aw = new AwardJson_Model();

                Aw.grantAwardId = Convert.ToInt64(dtAw_Award.Rows.Count > 0 ? dtAw_Award.Rows[0]["grantAwardId"].ToString() : "Not Available_Award");
                Aw.fundingBodyAwardId = dtAw_Award.Rows[0]["fundingbodyawardid"].ToString();
                Aw.title = (from DataRow dr3 in dtAw_Title.Rows
                            select new Title()
                            {
                                language = dr3["lang"].ToString(),
                                value = dr3["name_text"].ToString()
                            }).ToList();

                Aw.noticeDate = dtAw_Award.Rows.Count > 0 ? dtAw_Award.Rows[0]["noticeDate"].ToString() : "01-01-1900";
                Aw.startDate = Convert.ToDateTime(dtAw_Award.Rows.Count > 0 && dtAw_Award.Rows[0]["startdate"].ToString() != "" ? dtAw_Award.Rows[0]["startdate"].ToString() : "01-01-1900");
                Aw.endDate = Convert.ToDateTime(dtAw_Award.Rows.Count > 0 && dtAw_Award.Rows[0]["enddate"].ToString() != "" ? dtAw_Award.Rows[0]["enddate"].ToString() : "01-01-1900");
                Aw.grantType = dtAw_Award.Rows[0]["grantType"].ToString().ToUpper();
                Aw.funderSchemeType = dtAw_Award.Rows.Count > 0 ? dtAw_Award.Rows[0]["funderSchemeType"].ToString() : "Not Available_Award";
                Aw.homePage = new HomePage()
                {
                    link = dtAw_Award.Rows.Count > 0 ? dtAw_Award.Rows[0]["recordsource"].ToString() : "Not Available_Award",
                    publishedDate = Convert.ToDateTime(dtAw_Award.Rows[0]["publishedDate"].ToString().Trim() != "" ? dtAw_Award.Rows[0]["publishedDate"].ToString() : "01-01-1900"),
                    modifiedDate = Convert.ToDateTime(dtAw_Award.Rows[0]["modifiedDate"].ToString().Trim() != "" ? dtAw_Award.Rows[0]["modifiedDate"].ToString() : "01-01-1900")
                };


                Aw.keyword = (from DataRow drkey in dtAW_Keyword.Rows
                              select new Keyword()
                              {
                                  language = drkey["LANG"].ToString(),
                                  value = drkey["value"].ToString()
                              }).ToList();

                Aw.synopsis = (from DataRow drsynopsis in dtAw_Synopsis.Rows
                               select new Synopsis()
                               {
                                  @abstract = new Abstract
                                   {

                                       language = drsynopsis["LANG"].ToString(),
                                       value = drsynopsis["abstract_text"].ToString()
                                   },

                                   source = dtAw_Award.Rows.Count > 0 ? dtAw_Award.Rows[0]["recordsource"].ToString() : "Not Available_Award"
                               }).ToList();

                Aw.fundingDetail = new FundingDetail()
                {
                    installment = (from DataRow dr_installment in dtAw_FundingDetail.Rows
                                   select new Installment()
                                   {
                                       financialYear = Convert.ToInt32(dr_installment["financialYear"].ToString() != "" ? dr_installment["financialYear"].ToString() : "1900"),
                                       index = Convert.ToInt32(dr_installment["index_txt"].ToString() != "" ? dr_installment["index_txt"].ToString() : "1900"),
                                       fundedAmount = (from DataRow dr_fundedamount in dtAw_FundingDetail.Rows
                                                       select new FundedAmount()
                                                       {
                                                           amount = dr_fundedamount["AMOUNT"].ToString().Trim() != "" ? Convert.ToInt64(dr_fundedamount["AMOUNT"].ToString()) : -100,
                                                           currency = dr_fundedamount["inst_CURRENCY"].ToString().Trim() != "" ? dr_fundedamount["inst_CURRENCY"].ToString() : "Not Available_Award"
                                                       }
                                       ).ToList()
                                   }).ToList(),

                    fundingTotal = (from DataRow dr_fundingTotal in dtAw_FundingDetail.Rows
                                    select new FundingTotal()
                                    {
                                        currency = dr_fundingTotal["CURRENCY"].ToString(),
                                        amount = Convert.ToInt64(dr_fundingTotal["totalAmount"].ToString()),
                                    }).ToList()
                };

                Aw.classification = (from DataRow drclassification in dtAw_Classification.Rows
                                     select new Classification()
                                     {
                                         type = dtAw_Classification.Rows.Count > 0 ? dtAw_Classification.Rows[0]["cls_Type"].ToString() : "Not Available_Award",
                                         hasSubject = new HasSubject()
                                         {
                                             preferredLabel = drclassification["preferredLabel"].ToString(),

                                             identifier = new Identifier()
                                             {
                                                 type = drclassification["type"].ToString(),

                                                 value = drclassification["value"].ToString()
                                             }
                                         }
                                     }).ToList();


                Aw.relatedOpportunity = (from DataRow dr_RelatedOpportunity in dtAw_RelatedOpportunity.Rows
                                         select new RelatedOpportunity()
                                         {
                                             grantOpportunityId = Convert.ToInt64(dtAw_RelatedOpportunity.Rows[0]["grantOpportunityId"]),
                                             fundingBodyOpportunityId = dtAw_RelatedOpportunity.Rows[0]["fundingBodyOpportunityId"].ToString(),
                                             title = (from DataRow dr_title in dtAw_RelatedOpportunity.Rows
                                                      select new Title()
                                                      {
                                                          value = dr_title["opportunityname"].ToString(),
                                                          language = dr_title["lang"].ToString(),
                                                      }
                                                       ).ToList()

                                         }).ToList();


                Aw.relatedFunder = new RelatedFunder()
                {
                    leadFunder = new LeadFunder()
                    {

                        fundingBodyId = Convert.ToInt64(dtAw_LeadFunder.Rows[0]["fundingBodyId"].ToString())
                    },
                    hasFunder = (from DataRow drhasFunder in dtAw_hasFunder.Rows
                                 select new HasFunder()
                                 {

                                     fundingBodyId = Convert.ToInt64(drhasFunder["fundingBodyId"].ToString())
                                 }).ToList(),

                };

                Aw.awardeeDetail = (from DataRow dr_awardeeDetail in dtAw_AwardeeDetail.Rows
                                    select new AwardeeDetail()
                                    {
                                        role = dtAw_AwardeeDetail.Rows.Count > 0 ? dtAw_AwardeeDetail.Rows[0]["TYPE"].ToString() : "Not Available_Award",
                                        name = (from DataRow dr_Name in dtAw_AwardeeDetail.Rows
                                                select new Name()
                                                {
                                                    value = dr_Name["Name"].ToString(),
                                                    language = dr_Name["Name"].ToString().Trim() != "" ? dr_Name["Language"].ToString() : "Not Available_Award"

                                                }

                                             ).ToList(),

                                        awardeeAffiliationId = dtAw_AwardeeDetail.Rows.Count > 0 ? dtAw_AwardeeDetail.Rows[0]["awardeeAffiliationId"].ToString() : "Not Available_Award",
                                        fundingTotal = (from DataRow dr_fundingTotal in dtAw_AwardeeDetail.Rows
                                                        select new FundingTotal()
                                                        {
                                                            amount = Convert.ToInt64(dr_fundingTotal["AMOUNT_TEXT"].ToString()),
                                                            currency = dr_fundingTotal["AMOUNT_TEXT"].ToString().Trim() != "" ? dr_fundingTotal["currency"].ToString() : "Not Available_Award"

                                                        }
                                             ).ToList(),

                                        fundingBodyOrganizationId = dtAw_AwardeeDetail.Rows.Count > 0 ? dtAw_AwardeeDetail.Rows[0]["FBORGANIZATIONID"].ToString() : "Not Available_Award",
                                        vatNumber = dtAw_AwardeeDetail.Rows[0]["VATNUMBER"].ToString(),
                                        activityType = dtAw_AwardeeDetail.Rows[0]["activityType"].ToString(),

                                        hasPostalAddress = new HasPostalAddress()
                                        {
                                            addressCountry = dtAw_AwardeeAddress.Rows.Count > 0 ? dtAw_AwardeeAddress.Rows[0]["COUNTRYNAME"].ToString() : "Not Available_Award",
                                            addressRegion = dtAw_AwardeeAddress.Rows.Count > 0 ? dtAw_AwardeeAddress.Rows[0]["CITY"].ToString() : "Not Available_Award",
                                            addressLocality = dtAw_AwardeeAddress.Rows.Count > 0 ? dtAw_AwardeeAddress.Rows[0]["ROOM"].ToString() : "Not Available_Award",
                                            addressPostalCode = dtAw_AwardeeAddress.Rows.Count > 0 ? dtAw_AwardeeAddress.Rows[0]["POSTALCODE"].ToString() : "Not Available_Award",
                                            streetAddress = dtAw_AwardeeAddress.Rows.Count > 0 ? dtAw_AwardeeAddress.Rows[0]["Street"].ToString() : "Not Available_Award"

                                        },


                                        identifier = (from DataRow dr_identifier in dt_AwardeeIdentityFier.Rows

                                                      select new Identifier()
                                                      {
                                                          type = dr_identifier["value_text"].ToString() != "" ? dr_identifier["Identifier_text"].ToString() : "Not Available_Award",
                                                          value = dr_identifier["value_text"].ToString()

                                                      }
                                             ).ToList(),

                                        departmentName = (from DataRow dr_departmentName in dtAw_AwardeeDetail.Rows
                                                          select new DepartmentName()
                                                          {

                                                              value = dr_departmentName["departmentName"].ToString(),
                                                              language = dr_departmentName["departmentName"].ToString() != "" ? "en" : "Not Available_Award"

                                                          }
                                             ).ToList(),

                                        affiliationOf = (from DataRow dr_affiliationOf in dtAw_AffiliationDetail.Rows
                                                         select new AffiliationOf()
                                                         {
                                                             role = dr_affiliationOf["ROLE"].ToString(),

                                                             name = (from DataRow dr_Name in dtAw_AffiliationDetail.Rows
                                                                     select new Name()
                                                                     {
                                                                         language = dr_Name["NAME"].ToString() != "" ? "en" : "Not Available_Award",
                                                                         value = dr_Name["NAME"].ToString()
                                                                     }).ToList(),

                                                             givenName = dr_affiliationOf["GIVENNAME"].ToString(),
                                                             familyName = dr_affiliationOf["FAMILYNAME"].ToString(),
                                                             initials = dr_affiliationOf["INITIALS"].ToString(),
                                                             emailAddress = dr_affiliationOf["EMAIL"].ToString(),
                                                             fundingBodyPersonId = dr_affiliationOf["FUNDINGBODYPERSONID"].ToString(),
                                                             awardeePersonId = dr_affiliationOf["AWARDEEPERSONID"].ToString(),
                                                             identifier = (from DataRow dr_IdentiAff in dtAw_AffiliationDetail.Rows
                                                                           select new Identifier()
                                                                           {
                                                                               type = dr_IdentiAff["ORCID"].ToString().Trim() != "" ? "ORCID" : "Not Available_Award",
                                                                               value = dr_IdentiAff["ORCID"].ToString()
                                                                           }).ToList()

                                                         }
                                             ).ToList(),
                                    }
                                    ).ToList();

                Aw.funds = (from DataRow dr_funds in dtAw_funds.Rows
                            select new Funds()
                            {
                                fundingBodyProjectId = dr_funds["FUNDINGBODYPROJECTID"].ToString(),
                                acronym = dr_funds["ACRONYM"].ToString(),
                                startDate = Convert.ToDateTime(dr_funds["STARTDATE"].ToString()),
                                endDate = Convert.ToDateTime(dr_funds["ENDDATE"].ToString()),
                                link = dr_funds["LINK"].ToString(),
                                status = dr_funds["STATUS"].ToString(),
                                title = (from DataRow dr_title in dtAw_titleFunds.Rows
                                         select new Title()
                                         {
                                             value = dr_title["value_text"].ToString(),
                                             language = dr_title["language"].ToString()
                                         }
                                         ).ToList(),

                                budget = (from DataRow dr_budget in dtAw_funds.Rows
                                          select new Budget()
                                          {
                                              amount = dr_budget["amount"].ToString(),
                                              currency = dr_budget["currency"].ToString()
                                          }
                                                         ).ToList(),

                                hasPart = (from DataRow dr_hasPart in dtAw_haspart_funds.Rows
                                           select new HasPart()
                                           {
                                               fundingBodyProjectId = dr_hasPart["fundingBodyProjectId"].ToString(),
                                               budget = (from DataRow dr_budget in dtAw_haspart_funds.Rows
                                                         select new Budget()
                                                         {
                                                             amount = dr_budget["amount"].ToString(),
                                                             currency = dr_budget["currency"].ToString()
                                                         }
                                                         ).ToList(),
                                           }
                                          ).ToList(),

                                hasPostalAddress = new HasPostalAddress()
                                {
                                    addressCountry = dr_funds["Country"].ToString(),
                                    addressRegion = dr_funds["Region"].ToString(),
                                    addressLocality = dr_funds["Locality"].ToString(),
                                    addressPostalCode = dr_funds["PostalCode"].ToString(),
                                    streetAddress = dr_funds["street"].ToString()
                                }
                            }
                           ).ToList();


                Aw.hasProvenance = new HasProvenance()
                {
                    contactPoint = "fundingprojectteam@aptaracorp.com",
                    createdOn = Convert.ToDateTime(dtAw_createddate.Rows[0]["CREATEDDATE_TEXT"].ToString()),
                    defunct = Convert.ToBoolean(dtAw_Award.Rows[0]["defunct"].ToString()),

                    derivedFrom = dtAw_Award.Rows[0]["RECORDSOURCE"].ToString(),
                    hidden = Convert.ToBoolean(dtAw_Award.Rows[0]["hidden"].ToString()),

                    lastUpdateOn = dtAw_RevisedDate.Rows.Count > 0 ? Convert.ToDateTime(dtAw_RevisedDate.Rows[0]["reviseddate_text"].ToString()) : Convert.ToDateTime("01-01-1900"),
                    status = dtAw_revisionhistory.Rows[0]["status"].ToString().ToUpper(),
                    version = dtAw_createddate.Rows[0]["version"].ToString(),
                    wasAttributedTo = "SUP002",
                };

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var json = JsonConvert.SerializeObject(Aw, Newtonsoft.Json.Formatting.Indented, settings);
                json = json.Replace("\"TypeId\": \"DAL.JsonModel.AwardJson_Model, DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"", "");
                json = json.Replace("replaces_f", "replaces");
                json = json.Replace("\"amount\": -100", "");
                json = json.Replace("\"financialYear\": -100,", "");
                json = json.Replace("\"index\": -100,", "");
                json = json.Replace("01-01-1900", "");
                json = json.Replace("1900-01-01T00:00:00", "");
                json = json.Replace("-100", "");
                json = json.Replace("Not Available_Award", "");

                for (int k = 0; k <= 10; k++)
                {
                    json = Remove_NullObjects(json);
                    json = json.Replace("\"\",", "");
                    json = json.Replace("\"\"", "");
                }

                updatedjson = json;

                if (updatedjson.Length > 0)
                {
                    string AwID = Aw.grantAwardId.ToString();

                    File.WriteAllText(@"C:\Temp\Aw_JSON_File_" + AwID + ".json", updatedjson);
                }
                else
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                }
            }
            catch
            {
                if (updatedjson.Length == 0)
                {
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        //private static void GetAwardBodyForValidation(DataSet DS, Int64 WorkFlowId, int tranTypeId)
        //{
        //    Cmd = new OracleCommand();
        //    Cmd.CommandText = "sci_aw_QCcheckxml50_2";
        //    Cmd.CommandType = CommandType.StoredProcedure;
        //    Cmd.Connection = Cn;
        //    DataSet TempDataset = new DataSet();
        //    Adpter = new OracleDataAdapter("select MapTable,Name from Award_TABLES order by sequence", Cn);
        //    Adpter.Fill(TempDataset);

        //    if (TempDataset.Tables.Count > 0)
        //    {
        //        OracleParameter Param = new OracleParameter();
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = " p_tran_type_id ";
        //        Param.Value = tranTypeId;
        //        Cmd.Parameters.Add(Param);

        //        Param = new OracleParameter();
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = " p_workflowid ";
        //        Param.Value = WorkFlowId;
        //        Cmd.Parameters.Add(Param);


        //        for (Int32 count = 0; count < TempDataset.Tables[0].Rows.Count; count++)
        //        {
        //            Param = new OracleParameter();
        //            Param.OracleDbType = OracleDbType.RefCursor;
        //            Param.Direction = ParameterDirection.Output;
        //            Param.ParameterName = ("P_" + TempDataset.Tables[0].Rows[count]["MapTable"].ToString()).ToLower();
        //            Cmd.Parameters.Add(Param);
        //        }

        //        Param = new OracleParameter();
        //        Param.Direction = ParameterDirection.Output;
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = "p_o_status";
        //        Cmd.Parameters.Add(Param);

        //        Param = new OracleParameter();
        //        Param.Direction = ParameterDirection.Output;
        //        Param.OracleDbType = OracleDbType.Varchar2;
        //        Param.Size = 200;
        //        Param.ParameterName = "p_o_error";
        //        Cmd.Parameters.Add(Param);
        //    }

        //    try
        //    {
        //        Adpter = new OracleDataAdapter(Cmd);
        //        Adpter.Fill(DS, "AwardXML");
        //        DataTable DTbl = new DataTable();
        //        DTbl.Columns.Add("Error");
        //        DTbl.Columns.Add("ErrorMessage");
        //        DataRow DR = DTbl.NewRow();
        //        DR[0] = Cmd.Parameters["p_o_status"].Value.ToString();
        //        DR[1] = Cmd.Parameters["p_o_error"].Value.ToString();
        //        DTbl.Rows.Add(DR);
        //        DTbl.TableName = "ERRORCODE";
        //        if (!(DS.Tables.Contains("ERRORCODE")))
        //            DS.Tables.Add(DTbl);
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorLog.WriteErrorLog(ex);
        //        DataTable DTbl = new DataTable();
        //        DTbl.Columns.Add("Error");
        //        DTbl.Columns.Add("ErrorMessage");
        //        DataRow DR = DTbl.NewRow();
        //        DR[0] = "1";
        //        DR[1] = ex.Message;
        //        DTbl.Rows.Add(DR);
        //        DTbl.TableName = "ERRORCODE";
        //        DS.Tables.Add(DTbl);
        //    }
        //}

        //private static void GetPublicationBodyForValidation(DataSet DS, Int64 WorkFlowId, int tranTypeId)
        //{
        //    Cmd = new OracleCommand();
        //    Cmd.CommandText = "sci_aw_QCcheckxml50_2";
        //    Cmd.CommandType = CommandType.StoredProcedure;
        //    Cmd.Connection = Cn;
        //    DataSet TempDataset = new DataSet();
        //    Adpter = new OracleDataAdapter("select MapTable,Name from Award_TABLES order by sequence", Cn);
        //    Adpter.Fill(TempDataset);

        //    if (TempDataset.Tables.Count > 0)
        //    {
        //        OracleParameter Param = new OracleParameter();
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = " p_tran_type_id ";
        //        Param.Value = tranTypeId;
        //        Cmd.Parameters.Add(Param);

        //        Param = new OracleParameter();
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = " p_workflowid ";
        //        Param.Value = WorkFlowId;
        //        Cmd.Parameters.Add(Param);

        //        for (Int32 count = 0; count < TempDataset.Tables[0].Rows.Count; count++)
        //        {
        //            Param = new OracleParameter();
        //            Param.OracleDbType = OracleDbType.RefCursor;
        //            Param.Direction = ParameterDirection.Output;
        //            Param.ParameterName = ("P_" + TempDataset.Tables[0].Rows[count]["MapTable"].ToString()).ToLower();
        //            Cmd.Parameters.Add(Param);
        //        }

        //        Param = new OracleParameter();
        //        Param.Direction = ParameterDirection.Output;
        //        Param.OracleDbType = OracleDbType.Int64;
        //        Param.ParameterName = "p_o_status";
        //        Cmd.Parameters.Add(Param);

        //        Param = new OracleParameter();
        //        Param.Direction = ParameterDirection.Output;
        //        Param.OracleDbType = OracleDbType.Varchar2;
        //        Param.Size = 200;
        //        Param.ParameterName = "p_o_error";
        //        Cmd.Parameters.Add(Param);
        //    }

        //    try
        //    {
        //        Adpter = new OracleDataAdapter(Cmd);
        //        Adpter.Fill(DS, "AwardXML");
        //        DataTable DTbl = new DataTable();
        //        DTbl.Columns.Add("Error");
        //        DTbl.Columns.Add("ErrorMessage");
        //        DataRow DR = DTbl.NewRow();
        //        DR[0] = Cmd.Parameters["p_o_status"].Value.ToString();
        //        DR[1] = Cmd.Parameters["p_o_error"].Value.ToString();
        //        DTbl.Rows.Add(DR);
        //        DTbl.TableName = "ERRORCODE";

        //        if (DS.Tables.Contains("ERRORCODE") && DS.Tables["ERRORCODE"].Rows.Count == 0)
        //        {
        //            DS.Tables.Remove("ERRORCODE");
        //        }

        //        if (!(DS.Tables.Contains("ERRORCODE")))
        //            DS.Tables.Add(DTbl);
        //    }
        //    catch (Exception ex)
        //    {
        //        DataTable DTbl = new DataTable();
        //        DTbl.Columns.Add("Error");
        //        DTbl.Columns.Add("ErrorMessage");
        //        DataRow DR = DTbl.NewRow();
        //        DR[0] = "1";
        //        DR[1] = ex.Message;
        //        DTbl.Rows.Add(DR);
        //        DTbl.TableName = "ERRORCODE";
        //        DS.Tables.Add(DTbl);
        //    }
        //}

        static DataTable dt_reviseddate = new DataTable();

        public static void JsonCreationFromModel_Publication()
        {
            string updatedjson = string.Empty;

            try
            {
                Publication_JSONModel pub = new Publication_JSONModel();

                DataTable dt_has_Auth = new DataTable();
                dt_has_Auth.Columns.Add("Auth_Name");

                string[] auth_Name = dt_PublicationData.Rows[0]["PUBLICATION_AUTHOR"].ToString().Split(',');

                for (int i = 0; i < auth_Name.Length; i++)
                {
                    string name = auth_Name[i].ToString();
                    DataRow dr_Auth = dt_has_Auth.NewRow();

                    dr_Auth["Auth_Name"] = name;
                    dt_has_Auth.Rows.InsertAt(dr_Auth, i);
                }

                DataView dataView = new DataView();
                DataView dataView1 = new DataView();
                dataView = dt_lead_has.DefaultView;
                dataView1 = dt_lead_has.DefaultView;
                dataView.RowFilter = "HIERARCHY = 'lead'";
                dtpub_LeadFunder = dataView.ToTable();
                dtpub_hasFunder = dataView1.ToTable();

                pub.author = dt_PublicationData.Rows[0]["PUBLICATION_AUTHOR"].ToString();

                pub.HasAuthor = (from DataRow drhasauthor in dt_has_Auth.Rows
                                 select new HasAuthor()
                                 {
                                     name = drhasauthor["Auth_Name"].ToString()

                                 }).ToArray();

                pub.hasJournal = new HasJournal()
                {
                    identifier = new Identifier()
                    {
                        type = dtpub_identifier.Rows[0]["type"].ToString(),
                        value = dtpub_identifier.Rows[0]["journal_identifier"].ToString()
                    },
                    Title = (from DataRow drtitle in dtpub_identifier.Rows
                             select new Title()
                             {
                                 language = drtitle["lang"].ToString(),
                                 value = drtitle["IDENTIFIER_TITLE"].ToString()
                             }).ToArray()
                };

                pub.HasProvenance = new HasProvenance()
                {
                    wasAttributedTo = "SUP002",
                    derivedFrom = dtAw_Award.Rows[0]["RECORDSOURCE"].ToString(),
                    createdOn = Convert.ToDateTime(dt_createddate.Rows[0]["CREATEDDATE_TEXT"].ToString()),
                    contactPoint = "fundingprojectteam@aptaracorp.com",
                    status = "NEW",
                    lastUpdateOn = dt_reviseddate.Rows.Count > 0 && dt_reviseddate.Rows[0]["REVISEDDATE_TEXT"].ToString() != "" ? Convert.ToDateTime(dt_reviseddate.Rows[0]["REVISEDDATE_TEXT"].ToString()) : Convert.ToDateTime("01-01-1900"),
                    version = "1.0",
                    hidden = false,
                    defunct = false

                };

                pub.Identifier_P = (from DataRow dridentifier in dt_identifier_ml.Rows
                                    select new Identifier_Pub()
                                    {
                                        type = dridentifier["Type"].ToString(),
                                        value = dridentifier["Value"].ToString()
                                    }).ToArray();

                pub.Title = (from DataRow drtitle in dtpub_Title.Rows
                             select new Title_Pub()
                             {
                                 language = drtitle["lang"].ToString(),
                                 value = drtitle["TITLE"].ToString()
                             }).ToArray();

                pub.publicationOutputId = Convert.ToInt64(dt_PublicationData.Rows[0]["publication_id"].ToString());
                pub.publicationURL = dt_PublicationData.Rows[0]["PUBLICATION_URL"].ToString();
                pub.publishedDate = dt_PublicationData.Rows[0]["PUBLISHEDDATE"].ToString();

                pub.relatedAward = new RelatedAward()
                {
                    OutcomeOf = (from DataRow drhasFunder in dt_lead_has.Rows
                                 select new OutcomeOf()
                                 {
                                     description = dt_outcomeOfPub.Rows[0]["description"].ToString(),
                                     fundingBodyAwardId = dtAw_Award.Rows[0]["fundingbodyawardid"].ToString(),
                                     fundingBodyProjectId = dt_outcomeOfPub.Rows[0]["fundingBodyProjectId"].ToString(),
                                     grantAwardId = Convert.ToInt64(dtAw_Award.Rows[0]["grantAwardId"].ToString()),

                                     Title = (from DataRow drtitle in dt_name_outcome.Rows
                                              select new Title()
                                              {
                                                  language = drtitle["lang"].ToString(),
                                                  value = drtitle["name_text"].ToString()
                                              }).ToArray(),

                                 }).ToArray(),
                };

                pub.relatedFunder = new RelatedFunder_Pub()
                {
                    leadFunder = new LeadFunder_Pub()
                    {
                        fundingBodyId = Convert.ToInt64(dtpub_LeadFunder.Rows[0]["fundingBodyId"].ToString())
                    },

                    HasFunder = (from DataRow drhasFunder in dtpub_hasFunder.Rows
                                 select new HasFunder_Pub()
                                 {
                                     fundingBodyId = Convert.ToInt64(drhasFunder["fundingBodyId"].ToString())
                                 }).ToArray(),
                };

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                var json = JsonConvert.SerializeObject(pub, Newtonsoft.Json.Formatting.Indented, settings);
                json = json.Replace("\"TypeId\": \"DAL.Publication_JSONModel, DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"", "");
                json = json.Replace("Identifier_P", "Identifier");
                json = json.Replace("HasProvenance_pub", "hasProvenance");
                json = json.Replace("LeadFunder_Pub", "LeadFunder");
                json = json.Replace("RelatedFunder_Pub", "RelatedFunder");
                json = json.Replace("Title_Pub", "Title");
                json = json.Replace("01-01-1900", "");
                json = json.Replace("1900-01-01T00:00:00", "");
                json = json.Replace("HasProvenance", "hasProvenance");

                for (int k = 0; k <= 5; k++)
                {
                    json = Remove_NullObjects(json);
                    json = json.Replace("\"\",", "");
                    json = json.Replace("\"\"", "");
                }

                updatedjson = json;
                if (updatedjson.Length > 0)
                {
                    Source_PubJson.Clear();

                    string PubID = pub.aw_id_pub = dtAw_Award.Rows[0]["grantAwardId"].ToString();

                    File.WriteAllText(@"C:\Temp\Pub_JSON_File_" + PubID + ".json", updatedjson);
                    Clear_DataSet_Aw();

                    throw new Exception("JSON file genrate successfully! for Publication Module & File copied in temp Folder");
                }
                else
                {
                    Clear_DataSet_Aw();
                    Source_PubJson.Clear();
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                }
            }
            catch
            {
                if (updatedjson.Length == 0)
                {
                    Clear_DataSet_Aw();

                    Source_PubJson.Clear();
                    File.WriteAllText(@"C:\Temp\json_file.txt", "Something went worng..Some required parameter missing during JSON file generation..");
                    throw new Exception("Some required parameter missing during JSON file generation.");
                }
            }
        }

        public static void Clear_DataSet_Aw()
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
            dt_PublicationData.Clear();
            dtpub_Title.Clear();
            dtpub_identifier.Clear();
            dtpub_hasFunder.Clear();
            dt_identifier_ml.Clear();
            dt_lead_has.Clear();
            dt_name_outcome.Clear();
            dt_outcomeOfPub.Clear();
        }

        private static String HandleLegation(String XMLContent)
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

        private static String Filter(String XMLDocument)
        {
            return XMLDocument;
        }

        [Obsolete]
        public static String ValidateXML(XmlDocument XMLDoc, Int64 ModuleId, String XsdPath)
        {
            try
            {
                if (XsdPath.Trim() != String.Empty)
                {
                    String Path = String.Empty;
                    if (ModuleId == 2)
                    {
                        Path = XsdPath + "\\fundingBodies41.xsd";
                    }
                    else if (ModuleId == 3)
                    {
                        Path = XsdPath + "\\Opportunities41.xsd";
                    }
                    else if (ModuleId == 4)
                    {
                        Path = XsdPath + "\\Awards41.xsd";
                    }

                    XmlTextReader tr = null;
                    XmlSchemaCollection xsc = null;
                    XmlValidatingReader vr = null;

                    tr = new XmlTextReader(Path);
                    xsc = new XmlSchemaCollection();

                    xsc.Add("http://www.elsevier.com/xml/schema/grant/grant-4.1", tr);
                    tr = new XmlTextReader(new StringReader(XMLDoc.OuterXml));
                    vr = new XmlValidatingReader(tr);

                    vr.Schemas.Add(xsc);

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

                    return "1";
                }
                else
                {
                    return "XSD PATH NOT FOUND !";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static Int64 ErrorsCount = 0;
        static string ErrorMessage = "";

        private static void ValidationHandler(object sender, System.Xml.Schema.ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        public static string Remove_NullObjects(string json)
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

                    count_Jobj = xyz.Count;
                    json = temp1.ToString();
                }

                return json;
            }
            catch
            {
                return json;
            }
            #endregion
        }

        public static string OpportunityValidate(Int64 WorkFlowId, String XSDPath, int tranTypeId)
        {
            //String Error = String.Empty;
            //DataTable DTXML = new DataTable();
            //DataColumn XMLcol = new DataColumn("XML");
            //DTXML.Columns.Add(XMLcol);
            //XMLcol = new DataColumn("IDS");
            //DTXML.Columns.Add(XMLcol);
            //DataSet Opportunity = GetOppotunityXSDSchema(XSDPath);
            //String TableName = "";

            //try
            //{
            //    #region source_xml47-52
            //    GetOPPForValidate1(Source1, WorkFlowId, tranTypeId);
            //    Source = Source1;
            //    #endregion

            //    string oppdataset = "2";
            //    if (oppdataset == "1")
            //    {
            //        oppdataset = "3";
            //    }
            //    else
            //    {
            //        DataSet TempDataset = new DataSet();
            //        Adpter = new OracleDataAdapter("select Name from OPPORTUNITY_TABLES_s5 order by sequence", Cn);

            //        Adpter.Fill(TempDataset);

            //        if (TempDataset.Tables.Count > 0)
            //            Opportunity = TempDataset;
            //        #region seq
            //        for (Int32 count = 0; count < TempDataset.Tables[0].Rows.Count; count++)
            //        {
            //            for (Int32 countt = 0; countt < Opportunity.Tables.Count; countt++)
            //            {
            //                if (TempDataset.Tables[0].Rows[count]["Name"].ToString().ToLower() == Opportunity.Tables[countt].TableName.ToLower())
            //                {
            //                    TableName = Opportunity.Tables[countt].TableName.ToLower();
            //                    Source.Tables[count].TableName = TableName;
            //                    if (TableName.ToLower() == "opportunity")
            //                    {
            //                        foreach (DataRow DR in Source.Tables[count].Rows)
            //                        {
            //                            try
            //                            {
            //                                DR["duration"] = System.Xml.XmlConvert.ToTimeSpan(DR["duration"].ToString());

            //                                if (Convert.ToString(DR["id"]) == String.Empty)
            //                                {
            //                                    DR["id"] = Convert.ToInt64("9" + DR["opportunity_id"].ToString());
            //                                }
            //                                DR.AcceptChanges();
            //                            }
            //                            catch
            //                            {
            //                                DR["duration"] = null;
            //                                if (Convert.ToString(DR["id"]) == String.Empty)
            //                                {
            //                                    DR["id"] = Convert.ToInt64("9" + DR["opportunity_id"].ToString());
            //                                }
            //                                DR.AcceptChanges();
            //                            }
            //                        }
            //                    }

            //                    #region Multilang Opp Validation


            //                    if (TableName.ToLower() == "opportunity")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string recordSource_Multilang = "";
            //                            if (DC["recordSource"].ToString() != "")
            //                            {
            //                                recordSource_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["recordSource"].ToString()));
            //                                if (recordSource_Multilang != "")
            //                                {
            //                                    DC["recordSource"] = recordSource_Multilang;
            //                                }

            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "opportunitydate")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string recordSource_Multilang = "";
            //                            if (DC["DESCRIPTION"].ToString() != "")
            //                            {
            //                                recordSource_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["DESCRIPTION"].ToString()));
            //                                if (recordSource_Multilang != "")
            //                                {
            //                                    DC["DESCRIPTION"] = recordSource_Multilang;
            //                                }

            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "link")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string url_Multilang = "";
            //                            string linkText_Multilang = "";
            //                            if (DC["url"].ToString() != "")
            //                            {
            //                                url_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
            //                                if (url_Multilang != "")
            //                                {
            //                                    DC["url"] = url_Multilang;
            //                                }

            //                            }

            //                            if (DC["link_text"].ToString() != "")
            //                            {
            //                                linkText_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["link_text"].ToString()));
            //                                if (linkText_Multilang != "")
            //                                {
            //                                    DC["link_text"] = linkText_Multilang;
            //                                }

            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "website")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string url_Multilang = "";
            //                            string Text_Multilang = "";
            //                            if (DC["url"].ToString() != "")
            //                            {
            //                                url_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
            //                                if (url_Multilang != "")
            //                                {
            //                                    DC["url"] = url_Multilang;
            //                                }

            //                            }

            //                            if (DC["website_text"].ToString() != "")
            //                            {
            //                                Text_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["website_text"].ToString()));
            //                                if (Text_Multilang != "")
            //                                {
            //                                    DC["website_text"] = Text_Multilang;
            //                                }

            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "name")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string name_Multilang = "";
            //                            if (DC[1].ToString() != "")
            //                            {
            //                                // if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                                if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                                {
            //                                    name_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));
            //                                    if (name_Multilang != "")
            //                                    {
            //                                        DC[1] = name_Multilang;
            //                                    }

            //                                }
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "item")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string item_Multilang = "";
            //                            if (DC["description"].ToString() != "")
            //                            {
            //                                // if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                                if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                                {
            //                                    item_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["description"].ToString()));

            //                                    if (item_Multilang != "")
            //                                    {
            //                                        DC["description"] = item_Multilang;
            //                                    }

            //                                }
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "website")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string url_Multilang = "";
            //                            string Text_Multilang = "";
            //                            if (DC["url"].ToString() != "")
            //                            {
            //                                url_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["url"].ToString()));
            //                                if (url_Multilang != "")
            //                                {
            //                                    DC["url"] = url_Multilang;
            //                                }

            //                            }

            //                            if (DC["website_text"].ToString() != "")
            //                            {
            //                                Text_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["website_text"].ToString()));
            //                                if (Text_Multilang != "")
            //                                {
            //                                    DC["website_text"] = Text_Multilang;
            //                                }

            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "individualeligibility")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string degree_Multilang = "", graduate_Multilang = "", newfaculty_Multilang = "", undergraduate_Multilang = "";
            //                            if (DC["degree"].ToString() != "")
            //                            {
            //                                degree_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["degree"].ToString()));
            //                                if (degree_Multilang != "")
            //                                {
            //                                    DC["degree"] = degree_Multilang;
            //                                }

            //                            }

            //                            if (DC["graduate"].ToString() != "")
            //                            {
            //                                graduate_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["graduate"].ToString()));
            //                                if (graduate_Multilang != "")
            //                                {
            //                                    DC["graduate"] = graduate_Multilang;
            //                                }

            //                            }

            //                            if (DC["newfaculty"].ToString() != "")
            //                            {
            //                                newfaculty_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["newfaculty"].ToString()));
            //                                if (newfaculty_Multilang != "")
            //                                {
            //                                    DC["newfaculty"] = newfaculty_Multilang;
            //                                }
            //                            }

            //                            if (DC["undergraduate"].ToString() != "")
            //                            {
            //                                undergraduate_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["undergraduate"].ToString()));
            //                                if (undergraduate_Multilang != "")
            //                                {
            //                                    DC["undergraduate"] = undergraduate_Multilang;
            //                                }
            //                            }

            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "organizationeligibility")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string academic_Multilang = "", commercial_Multilang = "", government_Multilang = "", nonprofit_Multilang = "", sme_Multilang = "";
            //                            if (DC["academic"].ToString() != "")
            //                            {
            //                                academic_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["academic"].ToString()));
            //                                if (academic_Multilang != "")
            //                                {
            //                                    DC["academic"] = academic_Multilang;
            //                                }

            //                            }

            //                            if (DC["commercial"].ToString() != "")
            //                            {
            //                                commercial_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["commercial"].ToString()));
            //                                if (commercial_Multilang != "")
            //                                {
            //                                    DC["commercial"] = commercial_Multilang;
            //                                }

            //                            }

            //                            if (DC["government"].ToString() != "")
            //                            {
            //                                government_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["government"].ToString()));
            //                                if (government_Multilang != "")
            //                                {
            //                                    DC["government"] = government_Multilang;
            //                                }
            //                            }

            //                            if (DC["nonprofit"].ToString() != "")
            //                            {
            //                                nonprofit_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["nonprofit"].ToString()));
            //                                if (nonprofit_Multilang != "")
            //                                {
            //                                    DC["nonprofit"] = nonprofit_Multilang;
            //                                }
            //                            }

            //                            if (DC["sme"].ToString() != "")
            //                            {
            //                                sme_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["sme"].ToString()));
            //                                if (sme_Multilang != "")
            //                                {
            //                                    DC["sme"] = sme_Multilang;
            //                                }
            //                            }

            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "restrictions")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string disabilities_Multilang = "", invitationonly_Multilang = "", memberonly_Multilang = "", nominationonly_Multilang = "", minorities_Multilang = "", women_Multilang = "";
            //                            if (DC["disabilities"].ToString() != "")
            //                            {
            //                                disabilities_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["disabilities"].ToString()));
            //                                if (disabilities_Multilang != "")
            //                                {
            //                                    DC["disabilities"] = disabilities_Multilang;
            //                                }

            //                            }

            //                            if (DC["invitationonly"].ToString() != "")
            //                            {
            //                                invitationonly_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["invitationonly"].ToString()));
            //                                if (invitationonly_Multilang != "")
            //                                {
            //                                    DC["invitationonly"] = invitationonly_Multilang;
            //                                }

            //                            }

            //                            if (DC["memberonly"].ToString() != "")
            //                            {
            //                                memberonly_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["memberonly"].ToString()));
            //                                if (memberonly_Multilang != "")
            //                                {
            //                                    DC["memberonly"] = memberonly_Multilang;
            //                                }
            //                            }

            //                            if (DC["nominationonly"].ToString() != "")
            //                            {
            //                                nominationonly_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["nominationonly"].ToString()));
            //                                if (nominationonly_Multilang != "")
            //                                {
            //                                    DC["nominationonly"] = nominationonly_Multilang;
            //                                }
            //                            }


            //                            //if (DC["minorities"].ToString() != "")
            //                            //  {
            //                            //      minorities_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["minorities"].ToString()));
            //                            //      if (minorities_Multilang != "")
            //                            //      {
            //                            //          DC["minorities"] = minorities_Multilang;
            //                            //      }
            //                            //  }


            //                            if (DC["women"].ToString() != "")
            //                            {
            //                                women_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["women"].ToString()));
            //                                if (women_Multilang != "")
            //                                {
            //                                    DC["women"] = women_Multilang;
            //                                }
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }



            //                    if (TableName.ToLower() == "contact")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string type_Multilang = "", title_Multilang = "", email_Multilang = "", nominationonly_Multilang = "", minorities_Multilang = "", women_Multilang = "";
            //                            if (DC["type"].ToString() != "")
            //                            {
            //                                type_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["type"].ToString()));
            //                                if (type_Multilang != "")
            //                                {
            //                                    DC["type"] = type_Multilang;
            //                                }

            //                            }

            //                            if (DC["title"].ToString() != "")
            //                            {
            //                                title_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["title"].ToString()));
            //                                if (title_Multilang != "")
            //                                {
            //                                    DC["title"] = title_Multilang;
            //                                }

            //                            }

            //                            if (DC["email"].ToString() != "")
            //                            {
            //                                email_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["email"].ToString()));
            //                                if (email_Multilang != "")
            //                                {
            //                                    DC["email"] = email_Multilang;
            //                                }
            //                            }

            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "contactname")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string prefix_Multilang = "", Gname_Multilang = "", Mname_Multilang = "", surname_Multilang = "", suffix_Multilang = "", women_Multilang = "";
            //                            if (DC["prefix"].ToString() != "")
            //                            {
            //                                prefix_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["prefix"].ToString()));
            //                                if (prefix_Multilang != "")
            //                                {
            //                                    DC["prefix"] = prefix_Multilang;
            //                                }

            //                            }

            //                            if (DC["givenName"].ToString() != "")
            //                            {
            //                                Gname_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["givenName"].ToString()));
            //                                if (Gname_Multilang != "")
            //                                {
            //                                    DC["givenName"] = Gname_Multilang;
            //                                }

            //                            }

            //                            if (DC["middleName"].ToString() != "")
            //                            {
            //                                Mname_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["middleName"].ToString()));
            //                                if (Mname_Multilang != "")
            //                                {
            //                                    DC["middleName"] = Mname_Multilang;
            //                                }
            //                            }

            //                            if (DC["surname"].ToString() != "")
            //                            {
            //                                surname_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["surname"].ToString()));
            //                                if (surname_Multilang != "")
            //                                {
            //                                    DC["surname"] = surname_Multilang;
            //                                }
            //                            }

            //                            if (DC["suffix"].ToString() != "")
            //                            {
            //                                suffix_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["suffix"].ToString()));
            //                                if (suffix_Multilang != "")
            //                                {
            //                                    DC["suffix"] = suffix_Multilang;
            //                                }
            //                            }

            //                            DC.AcceptChanges();
            //                        }
            //                    }



            //                    if (TableName.ToLower() == "address")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {

            //                            string room_Multilang = "", street_Multilang = "", city_Multilang = "";
            //                            if (DC["room"].ToString() != "")
            //                            {
            //                                room_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["room"].ToString()));
            //                                if (room_Multilang != "")
            //                                {
            //                                    DC["room"] = room_Multilang;
            //                                }

            //                            }

            //                            if (DC["street"].ToString() != "")
            //                            {
            //                                street_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["street"].ToString()));
            //                                if (street_Multilang != "")
            //                                {
            //                                    DC["street"] = street_Multilang;
            //                                }

            //                            }

            //                            if (DC["city"].ToString() != "")
            //                            {
            //                                city_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["city"].ToString()));
            //                                if (city_Multilang != "")
            //                                {
            //                                    DC["city"] = city_Multilang;
            //                                }
            //                            }



            //                            DC.AcceptChanges();
            //                        }
            //                    }



            //                    if (TableName.ToLower() == "org")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            string org_Multilang = "";
            //                            if (DC["org_text"].ToString() != "")
            //                            {
            //                                org_Multilang = Convert.ToString(r.ConvertUnicodeToText(DC["org_text"].ToString()));
            //                                if (org_Multilang != "")
            //                                {
            //                                    DC["org_text"] = org_Multilang;
            //                                }

            //                            }

            //                            DC.AcceptChanges();
            //                        }
            //                    }
            //                    #endregion Multilang Opp Validation


            //                    if (TableName.ToLower() == "address")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["ROOM"].ToString() == "")
            //                            {
            //                                DC["ROOM"] = "Not Available";
            //                            }
            //                            if (DC["STREET"].ToString() == "")
            //                            {
            //                                DC["STREET"] = "Not Available";
            //                            }
            //                            if (DC["CITY"].ToString() == "")
            //                            {
            //                                DC["CITY"] = "Not Available";
            //                            }
            //                            if (DC["STATE"].ToString() == "")
            //                            {
            //                                DC["STATE"] = "Not Available";
            //                            }
            //                            if (DC["POSTALCODE"].ToString() == "")
            //                            {
            //                                DC["POSTALCODE"] = "Not Available";
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "opportunitylocation")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["ROOM"].ToString() == "")
            //                            {
            //                                DC["ROOM"] = "Not Available";
            //                            }
            //                            if (DC["STREET"].ToString() == "")
            //                            {
            //                                DC["STREET"] = "Not Available";
            //                            }
            //                            if (DC["CITY"].ToString() == "")
            //                            {
            //                                DC["CITY"] = "Not Available";
            //                            }
            //                            if (DC["STATE"].ToString() == "")
            //                            {
            //                                DC["STATE"] = "Not Available";
            //                            }
            //                            if (DC["POSTALCODE"].ToString() == "")
            //                            {
            //                                DC["POSTALCODE"] = "Not Available";
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }



            //                    if (TableName.ToLower() == Convert.ToString("limitedSubmissionDescription").ToLower())
            //                    {
            //                        Source.Tables[TableName].Columns["LIMITEDSUBMISSIONDESC_ID"].ColumnName = Convert.ToString("LIMITEDSUBMISSIONDESCRIPTION_ID").ToLower();
            //                        Source.Tables[TableName].AcceptChanges();

            //                    }
            //                    if (TableName.ToLower() == Convert.ToString("item").ToLower())
            //                    {
            //                        Source.Tables[TableName].Columns["LIMITEDSUBMISSIONDESC_ID"].ColumnName = Convert.ToString("LIMITEDSUBMISSIONDESCRIPTION_ID").ToLower();
            //                        Source.Tables[TableName].AcceptChanges();

            //                    }
            //                    if (TableName.ToLower() == Convert.ToString("opportunity").ToLower())
            //                    {

            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["RECORDSOURCE"].ToString() != "")
            //                            {
            //                                string recordSource = DC["RECORDSOURCE"].ToString();
            //                                DC["RECORDSOURCE"] = null;
            //                                DC["RECORDSOURCE"] = "\"" + recordSource.ToString() + "\"";
            //                            }


            //                            DC.AcceptChanges();
            //                            Source.Tables[TableName].AcceptChanges();

            //                        }

            //                    }

            //                    if (TableName.ToLower() == Convert.ToString("contact").ToLower())
            //                    {

            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["email"].ToString() != "")
            //                            {
            //                                string email = DC["email"].ToString();
            //                                DC["email"] = null;
            //                                DC["email"] = "\"" + email.ToString() + "\"";
            //                            }


            //                            DC.AcceptChanges();
            //                            Source.Tables[TableName].AcceptChanges();

            //                        }

            //                    }




            //                    foreach (DataRow DR in Source.Tables[TableName].Rows)
            //                    {
            //                        try
            //                        {
            //                            DR["opportunity_id"] = Convert.ToInt32(DR["opportunity_id"].ToString().Substring(4, 8));
            //                            DR.AcceptChanges();
            //                        }
            //                        catch { }
            //                    }

            //                    //if (Source.Tables[TableName].ToString() == "change")
            //                    //{
            //                    //    Opportunity.Tables["change"].Columns["postdate"].DataType = typeof(string);
            //                    //}

            //                    //if (Source.Tables[TableName].ToString() != "item" && Source.Tables[TableName].ToString() != "link"
            //                    //    && Source.Tables[TableName].ToString() != "individualEligibility" 
            //                    //    && Source.Tables[TableName].ToString() != "citizenship"
            //                    //     && Source.Tables[TableName].ToString() != "organizationEligibility"
            //                    //     && Source.Tables[TableName].ToString() != "regionspecific"
            //                    //    && Source.Tables[TableName].ToString() != "restrictions")
            //                    //{
            //                    //  DataTableReader reader = new DataTableReader(Source.Tables[count]);
            //                    //     Opportunity.Load(reader, LoadOption.OverwriteChanges, FillErrorHandler, Opportunity.Tables[TableName]);
            //                    //// }

            //                    DataTableReader reader = new DataTableReader(Source.Tables[count]);
            //                    Opportunity.Load(reader, LoadOption.OverwriteChanges, FillErrorHandler, Opportunity.Tables[TableName]);



            //                    string test = Opportunity.Tables[TableName].ToString();
            //                    if (test.Equals("change"))
            //                    {
            //                        string fhgf = test;

            //                    }
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }
            //        #endregion
            //        //pankaj 
            //        if (Opportunity.Tables.Count > 0)
            //        {

            //            #region new


            //            dt_grantType = Source1.Tables["OPPORTUNITYXML4"];
            //            dtOpportunity = Source1.Tables["OPPORTUNITYXML"];

            //            dt_revisionhistory = Source1.Tables["OPPORTUNITYXML1"];
            //            dt_reviseddate = Source1.Tables["OPPORTUNITYXML2"];
            //            dt_createddate = Source1.Tables["OPPORTUNITYXML3"];
            //            //p_type
            //            dt_contactName = Source1.Tables["OPPORTUNITYXML23"];
            //            dt_contact = Source1.Tables["OPPORTUNITYXML22"];
            //            dt_website = Source1.Tables["OPPORTUNITYXML24"];
            //            dt_address = Source1.Tables["OPPORTUNITYXML25"];

            //            dt_opportunityLocation = Source1.Tables["OPPORTUNITYXML32"];
            //            dt_Keyword = Source1.Tables["OPPORTUNITYXML18"];
            //            dt_synopsis = Source1.Tables["OPPORTUNITYXML8"];
            //            //26
            //            dt_description = Source1.Tables["description"];
            //            dt_link = Source1.Tables["link"];
            //            dt_subjectMatter = Source1.Tables["OPPORTUNITYXML15"];

            //            dt_ELIGIBILITYDESCRIPTION = Source1.Tables["OPPORTUNITYXML35"];
            //            dt_Duration = Source1.Tables["OpportunityXML46"];

            //            //dt_ReleatedFunder = Source1.Tables["relatedorgs"];
            //            //OPPORTUNITYXML28 / 14
            //            //dt_ReleatedFunder = Source1.Tables["OPPORTUNITYXML20"];

            //            dt_classfication = Source1.Tables["OPPORTUNITYXML16"];
            //            dt_hasSubject = Source1.Tables["OPPORTUNITYXML17"];

            //            //dt_associatedAmount_desc = Source1.Tables["estimatedamountdescription"];
            //            dt_associatedAmount_desc = Source1.Tables["OPPORTUNITYXML42"];
            //            //dt_estimatedTotal = Source1.Tables["estimatedfunding"];
            //            dt_estimatedTotal = Source1.Tables["OPPORTUNITYXML6"];
            //            dt_floor = Source1.Tables["OPPORTUNITYXML9"];
            //            dt_ceiling = Source1.Tables["OPPORTUNITYXML7"];

            //            //dt_citizenship = Source1.Tables["citizenship"];
            //            //dt_opportunityDate = Source1.Tables["Date"];

            //            dt_instruction = Source1.Tables["OPPORTUNITYXML57"];
            //            dt_licenseInformation = Source1.Tables["OPPORTUNITYXML56"];


            //            //dt_expirationDateDetail = Source1.Tables["OPPORTUNITYXML47"];
            //            //dt_decision = Source1.Tables["OPPORTUNITYXML48"];
            //            //dt_letterOfIntent = Source1.Tables["OPPORTUNITYXML49"];
            //            //dt_preproposal = Source1.Tables["OPPORTUNITYXML50"];
            //            //dt_startDateDetail = Source1.Tables["OPPORTUNITYXML51"];
            //            //dt_endDateDetail = Source1.Tables["OPPORTUNITYXML52"];
            //            //dt_proposal = Source1.Tables["OPPORTUNITYXML53"];

            //            dt_expirationDateDetail = Source1.Tables["OPPORTUNITYXML47"];
            //            dt_decision = Source1.Tables["OPPORTUNITYXML48"];
            //            dt_letterOfIntent = Source1.Tables["OPPORTUNITYXML49"];
            //            dt_preproposal = Source1.Tables["OPPORTUNITYXML50"];
            //            dt_startDateDetail = Source1.Tables["OPPORTUNITYXML51"];
            //            dt_endDateDetail = Source1.Tables["OPPORTUNITYXML53"];
            //            dt_proposal = Source1.Tables["OPPORTUNITYXML52"];

            //            dt_cycle = Source1.Tables["OPPORTUNITYXML54"];
            //            dt_homePage = Source1.Tables["OPPORTUNITYXML55"];

            //            dt_limitedSubmission = Source1.Tables["OPPORTUNITYXML41"];
            //            dt_individualEligibility = Source1.Tables["OPPORTUNITYXML36"];
            //            dt_citizenship = Source1.Tables["OPPORTUNITYXML37"];
            //            dt_organisationEligibility = Source1.Tables["OPPORTUNITYXML38"];
            //            dt_regionSpecific = Source1.Tables["OPPORTUNITYXML39"];
            //            dt_restrictionScope = Source1.Tables["OPPORTUNITYXML40"];

            //            dt_relatedTo = Source1.Tables["OPPORTUNITYXML58"];
            //            dt_replaces = Source1.Tables["OPPORTUNITYXML59"];
            //            dt_replacedBy = Source1.Tables["OPPORTUNITYXML60"];
            //            #endregion

            //            //DataTable dt_Title = new DataTable();
            //            dt_title = Source1.Tables["OPPORTUNITYXML26"];


            //            string jsonFbAlldata = JsonConvert.SerializeObject(Source);
            //            string jsonFundingxmldata = JsonConvert.SerializeObject(dtOpportunity);

            //            // JsonCreationFromModel();
            //            //pankaj 27 jan
            //            JsonCreationFromModel_opp();
            //        }




            //        //return FundingBody;
            //        // This is the final document

            //        #region xml
            //        XmlDocument Data = new XmlDocument();

            //        if (Opportunity.Tables.Count > 0)
            //        {
            //            //string input = "http://webapps.cihr-irsc.gc.ca/funding/Search?p&#x005F;language&#x003D;E&amp;p&#x005F;version&#x003D;CIHR";


            //            //var match = Regex.Match(input,@"&#x(.+?);").Groups[1].Value;
            //            //input = r.ReadandReplaceHexaToChar(input, XSDPath.Replace("\\XSD", ""));



            //            // Create a string writer that will write the Xml to a string
            //            StringWriter stringWriter = new StringWriter();
            //            // The Xml Text writer acts as a bridge between the xml stream and the text stream
            //            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            //            // Now take the Dataset and extract the Xml from it, it will write to the string writer
            //            Opportunity.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);
            //            // Write the Xml out to a string
            //            string contentAsXmlString = stringWriter.ToString();
            //            contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <opportunities");
            //            contentAsXmlString = contentAsXmlString.Replace("</NewDataSet>", "</opportunities>");
            //            // Replace "d3p1:lang" to "xml:lang"
            //            contentAsXmlString = contentAsXmlString.Replace("d3p1:lang", "xml:lang");
            //            contentAsXmlString = contentAsXmlString.Replace("xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\"", "");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<address country=\"(.*?)\">(.*?)</address>", "<address country=\"$1\">$2<country>$1</country></address>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<establishmentDate>(.*?)-(.*?)-(.*?)</establishmentDate>", "<establishmentDate>$1</establishmentDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<address>(.*?)<country>(.*?)</country></address>", "<address country=\"$2\">$1<country>$2</country></address>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<loiDate>(.*?)T(.*?)</loiDate>", "<loiDate>$1</loiDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<dueDate>(.*?)T(.*?)</dueDate>", "<dueDate>$1</dueDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<expirationDate>(.*?)T(.*?)</expirationDate>", "<expirationDate>$1</expirationDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<firstPostDate>(.*?)T(.*?)</firstPostDate>", "<firstPostDate>$1</firstPostDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<lastModifiedPostDate>(.*?)T(.*?)</lastModifiedPostDate>", "<lastModifiedPostDate>$1</lastModifiedPostDate>");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<change (.*?) postDate=\"(.*?)T(.*?)\"(.*?)>", "<change $1 postDate=\"$2\"$4>");
            //            //contentAsXmlString = Regex.Replace(contentAsXmlString, "<opportunities(.*?)>", "<opportunities xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-3.0\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-3.0/opportunities30.xsd\">");
            //            //contentAsXmlString = Regex.Replace(contentAsXmlString, "<opportunities(.*?)>", "<opportunities xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-4.0\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.0/opportunities40.xsd\">");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "<opportunities(.*?)>", "<opportunities xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-4.1\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1 http://www.elsevier.com/xml/schema/grant/grant-4.1/opportunities41.xsd\">");
            //            contentAsXmlString = contentAsXmlString.Replace("xml:space=\"preserve\"", "");
            //            // load the string of Xml Int64o the document
            //            contentAsXmlString = contentAsXmlString.Replace("<rawText>", "<rawText><![CDATA[");
            //            contentAsXmlString = contentAsXmlString.Replace("</rawText>", "]]></rawText>");
            //            contentAsXmlString = contentAsXmlString.Replace("xsi:nil=\"true\"", "");
            //            contentAsXmlString = contentAsXmlString.Replace("<room>Not Available</room>", "<room></room>");
            //            contentAsXmlString = contentAsXmlString.Replace("<street>Not Available</street>", "<street></street>");
            //            contentAsXmlString = contentAsXmlString.Replace("<city>Not Available</city>", "<city></city>");
            //            contentAsXmlString = contentAsXmlString.Replace("<state>Not Available</state>", "<state></state>");
            //            contentAsXmlString = contentAsXmlString.Replace("<postalCode>Not Available</postalCode>", "<postalCode></postalCode>");
            //            contentAsXmlString = contentAsXmlString.Replace(">0</estimatedFunding>", ">not-specified</estimatedFunding>");
            //            contentAsXmlString = contentAsXmlString.Replace(">0</awardCeiling>", ">not-specified</awardCeiling>");
            //            contentAsXmlString = contentAsXmlString.Replace(">0</awardFloor>", ">not-specified</awardFloor>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"LoiDate\">1900-01-01</date>", "<date type=\"LoiDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"LoiDate\">1900-02-01</date>", "<date type=\"LoiDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"ExpirationDate\">1900-01-01</date>", "<date type=\"ExpirationDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"ExpirationDate\">1900-02-01</date>", "<date type=\"ExpirationDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"DueDate\">1900-01-01</date>", "<date type=\"DueDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"DueDate\">1900-02-01</date>", "<date type=\"DueDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"OpenDate\">1900-01-01</date>", "<date type=\"OpenDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"OpenDate\">1900-02-01</date>", "<date type=\"OpenDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"LastModifiedPostDate\">1900-01-01</date>", "<date type=\"LastModifiedPostDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"LastModifiedPostDate\">1900-02-01</date>", "<date type=\"LastModifiedPostDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"FirstPostDate\">1900-01-01</date>", "<date type=\"FirstPostDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"FirstPostDate\">1900-02-01</date>", "<date type=\"FirstPostDate\">ongoing</date>");

            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"PreProposalDate\">1900-01-01</date>", "<date type=\"PreProposalDate\">not-specified</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("<date type=\"PreProposalDate\">1900-02-01</date>", "<date type=\"PreProposalDate\">ongoing</date>");
            //            contentAsXmlString = contentAsXmlString.Replace("countryTest", "country");

            //            #region Added by avanish on 22-june-2018
            //            contentAsXmlString = contentAsXmlString.Replace("DESCRIPTION", Convert.ToString("description").ToLower());
            //            contentAsXmlString = contentAsXmlString.Replace(@"<description xmlns=""""", "<description");
            //            contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");
            //            contentAsXmlString = contentAsXmlString.Replace("xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");

            //            contentAsXmlString = contentAsXmlString.Replace("<recordSource>" + "\"", "<recordSource>");
            //            contentAsXmlString = contentAsXmlString.Replace("\"" + "</recordSource>", "</recordSource>");
            //            contentAsXmlString = contentAsXmlString.Replace("<email>" + "\"", "<email>");
            //            contentAsXmlString = contentAsXmlString.Replace("\"" + "</email>", "</email>");


            //            #endregion
            //            contentAsXmlString = contentAsXmlString.Replace(@"<MINORTIES xmlns=""""", "<minorities");
            //            contentAsXmlString = contentAsXmlString.Replace("</MINORTIES>", "</minorities>");
            //            contentAsXmlString = contentAsXmlString.Replace(@"<LIMITEDSUBMISSIONdescription xmlns=""""", "<limitedSubmissionDescription");
            //            contentAsXmlString = contentAsXmlString.Replace("</LIMITEDSUBMISSIONdescription>", "</limitedSubmissionDescription>");

            //            #region

            //            contentAsXmlString = contentAsXmlString.Replace("xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");
            //            contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");
            //            contentAsXmlString = contentAsXmlString.Replace("&amp;nbsp;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("  ", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("   ", " ");
            //            #endregion

            //            contentAsXmlString = contentAsXmlString.Replace("&amp;#x", "&#x");
            //            contentAsXmlString = contentAsXmlString.Replace("&amp;#", "&#");
            //            contentAsXmlString = contentAsXmlString.Replace("&amp;lt;", "&lt;");
            //            contentAsXmlString = contentAsXmlString.Replace("&amp;gt;", "&gt;");
            //            contentAsXmlString = contentAsXmlString.Replace("&AMP ;", "");

            //            #region Hex Values which have to replaced into space on 15-Mar-2019
            //            contentAsXmlString = r.EntityToUnicode2(contentAsXmlString);
            //            contentAsXmlString = r.EntityToUnicode(contentAsXmlString);
            //            contentAsXmlString = contentAsXmlString.Replace("&#x00A0;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2002;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2003;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2004;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2005;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2006;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2007;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2008;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x2009;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x200A;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x200B;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#x3000;", " ");
            //            contentAsXmlString = contentAsXmlString.Replace("&#xFEFF;", " ");
            //            #endregion
            //            contentAsXmlString = r.ReadandReplaceHexaToChar(contentAsXmlString, XSDPath.Replace("\\XSD", ""));
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "\\s+(</[-A-Za-z]+>)", "$1");
            //            contentAsXmlString = Regex.Replace(contentAsXmlString, "(<[-A-Za-z]+>)\\s+", "$1");
            //            contentAsXmlString = HandleLegation(contentAsXmlString);
            //            // contentAsXmlString = Filter(contentAsXmlString); Commneted on 15-Mar-2019 for allowing hex values 
            //            Data.PreserveWhitespace = true;
            //            Data.LoadXml(contentAsXmlString);
            //            //XmlTextReader TxtRd = new XmlTextReader(contentAsXmlString);
            //            SequenceOpportunityXML(Data);
            //        }
            //        /// commented by pankaj Error = ValidateXML(Data, 3, XSDPath);


            //        //Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-4.1", "");
            //        //Error = Error.Replace("'", ""); Error = Error.Replace(":", "");
            //        //Error = Error.Replace("in namespace", "");
            //        Error = Error_text_deatils;
            //        Source1.Clear();
            //    }
            //    Error = Error_text_deatils;
            //}
            //catch (Exception exp)
            //{
            //    oErrorLog.WriteErrorLog(exp);
            //    Error = exp.Message + "****" + TableName + "****";
            //}
            //return Error;

            return string.Empty;
        }

        public static string FundingBodyValidate(Int64 WorkFlowId, String XSDPath, int tranTypeId)
        {
            return string.Empty;

            //String Error = string.Empty;
            //DataTable DTXML = new DataTable();
            //DataColumn XMLcol = new DataColumn("XML");
            //DTXML.Columns.Add(XMLcol);
            //XMLcol = new DataColumn("IDS");
            //DTXML.Columns.Add(XMLcol);
            //DataSet FundingBody = GetFundingBodyXSDSchema(XSDPath);
            //String TableName = "";
            //DataSet Source = new DataSet();
            //try
            //{
            //    if (WorkFlowId > 0)
            //    {
            //        DataSet ChkFBHidden_Dataset = new DataSet();
            //        Adpter = new OracleDataAdapter("Select Count(*) Count from fundingbody fb ,relatedorgs rg,org where fb.fundingbody_id=rg.fundingbody_id and org.RELATEDORGS_ID=rg.RELATEDORGS_ID and org.RELTYPE in ('renamedAs','mergedWith','incorporatedInto','isReplacedBy') and fb.Hidden in ('false','False','FALSE') and fb.FUNDINGBODY_ID in (SELECT  ID FROM   sci_workflow WHERE   WORKFLOWID =" + WorkFlowId + ")", Cn);
            //        Adpter.Fill(ChkFBHidden_Dataset);
            //        if (ChkFBHidden_Dataset.Tables[0].Rows[0]["Count"].ToString() == "0")
            //        {

            //        }
            //        else
            //        {
            //            return "Please check Hidden status of the fundingBody as per Rel-Type";
            //        }
            //    }

            //    GetFundingBodyForValidate(Source, WorkFlowId, tranTypeId);
            //    DataSet TempDataset = new DataSet();
            //    // Adpter = new OracleDataAdapter("select Name from FundingBodyTables order by sequence", Cn);
            //    //Adpter = new OracleDataAdapter("select name from FUNDINGBODYTABLES_S5 where sequence not in (34,35,36,37,38,42,43,44)order by sequence", Cn);
            //    Adpter = new OracleDataAdapter("select name from FUNDINGBODYTABLES_S5_2 where sequence not in (34,35,36,37,38,42,43,44)order by sequence", Cn);
            //    Adpter.Fill(TempDataset);
            //    if (TempDataset.Tables.Count > 0)
            //        for (Int32 count = 0; count < TempDataset.Tables[0].Rows.Count; count++)
            //        {
            //            for (Int32 countt = 0; countt < FundingBody.Tables.Count; countt++)
            //            {
            //                if (TempDataset.Tables[0].Rows[count]["Name"].ToString().ToLower() == FundingBody.Tables[countt].TableName.ToLower())
            //                {
            //                    TableName = FundingBody.Tables[countt].TableName.ToLower();
            //                    Source.Tables[count].TableName = TableName;
            //                    if (TableName.ToLower() == "address")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["ROOM"].ToString() == "")
            //                            {
            //                                DC["ROOM"] = "Not Available";
            //                            }
            //                            if (DC["STREET"].ToString() == "")
            //                            {
            //                                DC["STREET"] = "Not Available";
            //                            }
            //                            if (DC["CITY"].ToString() == "")
            //                            {
            //                                DC["CITY"] = "Not Available";
            //                            }
            //                            if (DC["STATE"].ToString() == "")
            //                            {
            //                                DC["STATE"] = "Not Available";
            //                            }
            //                            if (DC["POSTALCODE"].ToString() == "")
            //                            {
            //                                DC["POSTALCODE"] = "Not Available";
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }
            //                    if (TableName.ToLower() == "canonicalname")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC[1].ToString() == "")
            //                            {
            //                                DC[1] = "Not Available";
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "item")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC[2].ToString() != "")
            //                            {
            //                                // if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                                if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                                {
            //                                    DC[2] = Convert.ToString(r.ConvertUnicodeToText(DC[2].ToString()));

            //                                }
            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }


            //                    if (TableName.ToLower() == "link")
            //                    {
            //                        string Link_text_MultiLang = "";
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC[2].ToString() != "")
            //                            {
            //                                Link_text_MultiLang = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));

            //                                if (Link_text_MultiLang != "")
            //                                {
            //                                    DC[1] = Link_text_MultiLang;
            //                                    DC.AcceptChanges();
            //                                }



            //                            }

            //                        }
            //                    }


            //                    #region Multi lang Contact
            //                    if (TableName.ToLower() == "contact")
            //                    {
            //                        string MultilangText = "";
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["type"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["type"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["type"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["email"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["email"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["email"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["title"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["title"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["title"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                        }
            //                    }



            //                    if (TableName.ToLower() == "contactname")
            //                    {
            //                        string MultilangText = "";
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["prefix"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["prefix"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["prefix"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["givenName"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["givenName"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["givenName"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["middleName"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["middleName"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["middleName"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["surname"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["surname"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["surname"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }


            //                            if (DC["suffix"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["suffix"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["suffix"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }
            //                        }
            //                    }



            //                    if (TableName.ToLower() == "website")
            //                    {
            //                        string MultilangText = "";
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["URL"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["URL"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["URL"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["WEBSITE_TEXT"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["WEBSITE_TEXT"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["WEBSITE_TEXT"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }


            //                        }
            //                    }


            //                    if (TableName.ToLower() == "address")
            //                    {
            //                        string MultilangText = "";
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["ROOM"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["ROOM"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["ROOM"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["STREET"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["STREET"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["STREET"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }

            //                            if (DC["CITY"].ToString() != "")
            //                            {
            //                                MultilangText = Convert.ToString(r.ConvertUnicodeToText(DC["CITY"].ToString()));

            //                                if (MultilangText != "")
            //                                {
            //                                    DC["CITY"] = MultilangText;
            //                                    DC.AcceptChanges();
            //                                }

            //                            }


            //                        }
            //                    }



            //                    #endregion Multilang Contact

            //                    if (TableName.ToLower() == "preferredorgname")
            //                    {
            //                        if (Source.Tables[TableName].Rows.Count == 0)
            //                        {
            //                            throw new NotImplementedException("Preferred ognisation name missing");
            //                        }
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC[1].ToString() == "")
            //                            {
            //                                DC[1] = "Not Available";
            //                            }
            //                            //if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                            if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                            {
            //                                string chk_preON = "";
            //                                chk_preON = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));
            //                                if (chk_preON != "")
            //                                {
            //                                    DC[1] = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));
            //                                }


            //                            }
            //                            DC.AcceptChanges();
            //                        }
            //                    }
            //                    if (TableName.ToLower() == "contextname")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            //if (DC[1].ToString() == "")
            //                            //{
            //                            //    DC[1] = "Not Available";
            //                            //}
            //                            //if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                            //if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                            //{
            //                            //    DC[1] = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));

            //                            //}
            //                            DC.AcceptChanges();
            //                        }
            //                    }
            //                    if (TableName.ToLower() == "abbrevname")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            //if (DC[1].ToString() == "")
            //                            //{
            //                            //    DC[1] = "Not Available";
            //                            //}
            //                            //if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                            //if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                            //{
            //                            //    DC[1] = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));

            //                            //}
            //                            DC.AcceptChanges();
            //                        }
            //                    }

            //                    if (TableName.ToLower() == "acronym")
            //                    {
            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC[1].ToString() == "")
            //                            {
            //                                //DC[1] = "Not Available";
            //                                //DC[1] = "";
            //                            }
            //                            //if (DC["LANG"].ToString().Trim().ToLower() == "zh" || DC["LANG"].ToString().Trim().ToLower() == "ja" || DC["LANG"].ToString().Trim().ToLower() == "ko" || DC["LANG"].ToString().Trim().ToLower() == "ru" || DC["LANG"].ToString().Trim().ToLower() == "fa" || DC["LANG"].ToString().Trim().ToLower() == "hi" || DC["LANG"].ToString().Trim().ToLower() == "he" || DC["LANG"].ToString().Trim().ToLower() == "th")
            //                            //if (r.chk_OtherLang(DC["LANG"].ToString().Trim().ToLower()) == true)
            //                            //{
            //                            //    DC[1] = Convert.ToString(r.ConvertUnicodeToText(DC[1].ToString()));

            //                            //}
            //                            DC.AcceptChanges();
            //                        }
            //                    }
            //                    //commnet for schema 5
            //                    //if (TableName.ToLower() == "opportunitiessource" || TableName.ToLower() == "awardssource")
            //                    //{
            //                    //    foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                    //    {
            //                    //        FundingBody.Tables[TableName].Columns[0].DataType = Source.Tables[TableName].Columns[0].DataType;

            //                    //        //var dataTT=FundingBody.Tables[TableName].Columns[0].DataType;

            //                    //        DC.AcceptChanges();
            //                    //    }
            //                    //}

            //                    if (TableName.ToLower() == Convert.ToString("fundingbody").ToLower())
            //                    {

            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["RECORDSOURCE"].ToString() != "")
            //                            {
            //                                string recordSource = DC["RECORDSOURCE"].ToString();
            //                                DC["RECORDSOURCE"] = null;
            //                                //DC["RECORDSOURCE"] = "\"" + recordSource.ToString() + "\"";
            //                                DC["RECORDSOURCE"] = recordSource.ToString();
            //                            }


            //                            DC.AcceptChanges();
            //                            Source.Tables[TableName].AcceptChanges();

            //                        }

            //                    }

            //                    if (TableName.ToLower() == Convert.ToString("contact").ToLower())
            //                    {

            //                        foreach (DataRow DC in Source.Tables[TableName].Rows)
            //                        {
            //                            if (DC["email"].ToString() != "")
            //                            {
            //                                string email = DC["email"].ToString();
            //                                DC["email"] = null;
            //                                DC["email"] = "\"" + email.ToString() + "\"";
            //                            }


            //                            DC.AcceptChanges();
            //                            Source.Tables[TableName].AcceptChanges();

            //                        }

            //                    }





            //                    foreach (DataRow DR in Source.Tables[TableName].Rows)
            //                    {
            //                        try
            //                        {
            //                            DR["fundingbody_id"] = Convert.ToInt32(DR["fundingbody_id"].ToString().Substring(4, 8));
            //                            DR.AcceptChanges();
            //                        }
            //                        catch { }
            //                    }
            //                    try
            //                    {
            //                        DataTableReader reader = new DataTableReader(Source.Tables[count]);

            //                        FundingBody.Load(reader, LoadOption.OverwriteChanges, FillErrorHandler, FundingBody.Tables[TableName]);

            //                    }
            //                    catch { }
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }




            //    #region Commented for Schema v3.0 By Rantosh
            //    //return FundingBody;
            //    // This is the final document
            //    //XmlDocument Data = new XmlDocument();
            //    //if (FundingBody.Tables.Count > 0)
            //    //{
            //    //    // Create a string writer that will write the Xml to a string
            //    //    StringWriter stringWriter = new StringWriter();
            //    //    // The Xml Text writer acts as a bridge between the xml stream and the text stream
            //    //    XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            //    //    // Now take the Dataset and extract the Xml from it, it will write to the string writer
            //    //    FundingBody.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);
            //    //    // Write the Xml out to a string
            //    //    string contentAsXmlString = stringWriter.ToString();
            //    //    contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <fundingBodies xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-2.0 FundingBodies.xsd\"");
            //    //    contentAsXmlString = contentAsXmlString.Replace("</NewDataSet>", "</fundingBodies>");
            //    //    // Replace "d3p1:lang" to "xml:lang"
            //    //    contentAsXmlString = contentAsXmlString.Replace("d3p1:lang", "xml:lang");
            //    //    // Replace xmlns:d3p1="http://www.w3.org/XML/1998/namespace" in ""
            //    //    contentAsXmlString = contentAsXmlString.Replace("xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\"", "");
            //    //    contentAsXmlString = Regex.Replace(contentAsXmlString, "<address country=\"(.*?)\">(.*?)</address>", "<address country=\"$1\">$2<country>$1</country></address>");
            //    //    contentAsXmlString = Regex.Replace(contentAsXmlString, "<establishmentDate>(.*?)-(.*?)-(.*?)</establishmentDate>", "<establishmentDate>$1</establishmentDate>");
            //    //    //contentAsXmlString = Regex.Replace(contentAsXmlString, "xmlns:xsi=\"(.*?)\"", "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema\"");
            //    //    contentAsXmlString = Regex.Replace(contentAsXmlString, "<fundingBodies(.*?)>", "<fundingBodies xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-2.0\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-2.0 fundingBodies20.xsd\">");
            //    //    contentAsXmlString = contentAsXmlString.Replace("xml:space=\"preserve\"", "");
            //    //    //xml:space="preserve"
            //    //    //<fundingBodies xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://www.elsevier.com/xml/schema/grant/grant-1.4" xsi:schemaLocation="http://www.elsevier.com/xml/schema/grant/grant-1.4 FundingBodies1.xsd">
            //    //    // load the string of Xml Int64o the document//xmlns:xsi="http://www.w3.org/2001/XMLSchema"
            //    //    contentAsXmlString = contentAsXmlString.Replace("<eligibilityDescription >", "<eligibilityDescription>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("<eligibilityDescription>","<eligibilityDescription><![CDATA[");
            //    //    contentAsXmlString = contentAsXmlString.Replace("</eligibilityDescription>", "]]></eligibilityDescription>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("xsi:nil=\"true\"", "");
            //    //    //contentAsXmlString = contentAsXmlString.Replace("Not Available", "");
            //    //    // Updated by Harish(@ 11-Nov-2014
            //    //    /* contentAsXmlString = contentAsXmlString.Replace("&amp;#7897;", "ộ").Replace("&amp;#7909;", "ụ").Replace("&amp;#7841;", "ạ").Replace("&amp;#351;", "ş").Replace("&amp;#355;", "ț")
            //    //                        .Replace("&amp;#244;", "ô").Replace("&amp;#212;", "Ô").Replace("&amp;#7879;", "ệ").Replace("&amp;#7875;", "ể").Replace("&amp;#367;", "ů")
            //    //                        .Replace("&amp;#283;", "ě").Replace("&amp;#268;", "Č").Replace("&amp;#233;", "é").Replace("&amp;#380;", "ż").Replace("&amp;#379;", "Ż")
            //    //                        .Replace("&amp;#324;", "ń").Replace("&amp;#323;", "Ń").Replace("&amp;#287;", "ğ").Replace("&amp;#286;", "Ğ").Replace("&amp;#601;", "ə")
            //    //                        .Replace("&amp;#536;", "Ș").Replace("&amp;#537;", "ș").Replace("&amp;#246;", "ö").Replace("&amp;#214;", "Ö").Replace("&amp;#243;", "ó").Replace("&amp;#211;", "Ó")
            //    //                        .Replace("&amp;#205;", "Í").Replace("&amp;#237;", "í").Replace("&amp;#202;", "Ê").Replace("&amp;#234;", "ê").Replace("&amp;#199;", "Ç")
            //    //                        .Replace("&amp;#231;", "ç").Replace("&amp;#192;", "À").Replace("&amp;#224;", "à").Replace("&amp;#196;", "Ä").Replace("&amp;#228;", "ä")
            //    //                        .Replace("&amp;#193;", "Á").Replace("&amp;#225;", "á").Replace("&amp;#258;", "Ă").Replace("&amp;#259;", "ă").Replace("&amp;#256;", "Ā").Replace("&amp;#257;", "ā").Replace("&amp;#298;", "Ī").Replace("&amp;#299;", "ī"); */
            //    //    contentAsXmlString = contentAsXmlString.Replace("&amp;#", "&#");
            //    //contentAsXmlString = contentAsXmlString.Replace("&amp;lt;", "&lt;");
            //    //contentAsXmlString = contentAsXmlString.Replace("&amp;gt;", "&gt;");
            //    //    /* contentAsXmlString = contentAsXmlString.Replace("&#7897;", "ộ").Replace("&#7909;", "ụ").Replace("&#7841;", "ạ").Replace("&#351;", "ş").Replace("&#355;", "ț")
            //    //                    .Replace("&#244;", "ô").Replace("&#212;", "Ô").Replace("&#7879;", "ệ").Replace("&#7875;", "ể").Replace("&#367;", "ů")
            //    //                    .Replace("&#283;", "ě").Replace("&#268;", "Č").Replace("&#233;", "é").Replace("&#380;", "ż").Replace("&#379;", "Ż")
            //    //                    .Replace("&#324;", "ń").Replace("&#323;", "Ń").Replace("&#287;", "ğ").Replace("&#286;", "Ğ").Replace("&#601;", "ə")
            //    //                    .Replace("&#536;", "Ș").Replace("&#537;", "ș").Replace("&#246;", "ö").Replace("&#214;", "Ö").Replace("&#243;", "ó").Replace("&#211;", "Ó")
            //    //                    .Replace("&#205;", "Í").Replace("&#237;", "í").Replace("&#202;", "Ê").Replace("&#234;", "ê").Replace("&#199;", "Ç")
            //    //                    .Replace("&#231;", "ç").Replace("&#192;", "À").Replace("&#224;", "à").Replace("&#196;", "Ä").Replace("&#228;", "ä")
            //    //                    .Replace("&#193;", "Á").Replace("&#225;", "á").Replace("&#258;", "Ă").Replace("&#259;", "ă").Replace("&#256;", "Ā").Replace("&#257;", "ā").Replace("&#298;", "Ī").Replace("&#299;", "ī"); */
            //    //    contentAsXmlString = r.ReadandReplaceHexaToChar(contentAsXmlString, XSDPath.Replace("\\XSD", ""));
            //    //    //
            //    //    contentAsXmlString = contentAsXmlString.Replace("<room>Not Available</room>", "<room></room>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("<street>Not Available</street>", "<street></street>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("<city>Not Available</city>", "<city></city>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("<state>Not Available</state>", "<state></state>");
            //    //    contentAsXmlString = contentAsXmlString.Replace("<postalCode>Not Available</postalCode>", "<postalCode></postalCode>");
            //    //    contentAsXmlString = contentAsXmlString.Replace(">0</totalFunding>", ">not-specified</totalFunding>");
            //    //    contentAsXmlString = Regex.Replace(contentAsXmlString, "\\s+(</[-A-Za-z]+>)", "$1");
            //    //    contentAsXmlString = Regex.Replace(contentAsXmlString, "(<[-A-Za-z]+>)\\s+", "$1");
            //    //    contentAsXmlString = HandleLegation(contentAsXmlString);
            //    //    contentAsXmlString = Filter(contentAsXmlString);
            //    //    Data.LoadXml(contentAsXmlString);
            //    //    SequenceFundingBodyXML(Data);
            //    //}
            //    //Error = ValidateXML(Data, 2, XSDPath);
            //    //Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-2.0", "");
            //    //Error = Error.Replace("'", ""); Error = Error.Replace(":", "");
            //    //Error = Error.Replace("in namespace", "");

            //    #endregion

            //    #region Added for Schema v3.0 By Rantosh
            //    //return FundingBody;
            //    // This is the final document
            //    XmlDocument Data = new XmlDocument();
            //    if (FundingBody.Tables.Count > 0)
            //    {
            //        // Create a string writer that will write the Xml to a string
            //        StringWriter stringWriter = new StringWriter();
            //        // The Xml Text writer acts as a bridge between the xml stream and the text stream
            //        XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            //        // Now take the Dataset and extract the Xml from it, it will write to the string writer
            //        FundingBody.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);

            //        // Write the Xml out to a string
            //        string contentAsXmlString = stringWriter.ToString();
            //        // contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <fundingBodies xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-3.0/fundingBodies30.xsd\""); by Avanish on 26 MAy-2018
            //        contentAsXmlString = contentAsXmlString.Replace("<NewDataSet", "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <fundingBodies xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.0/fundingBodies41.xsd\"");
            //        contentAsXmlString = contentAsXmlString.Replace("</NewDataSet>", "</fundingBodies>");
            //        // Replace "d3p1:lang" to "xml:lang"
            //        contentAsXmlString = contentAsXmlString.Replace("d3p1:lang", "xml:lang");
            //        // Replace xmlns:d3p1="http://www.w3.org/XML/1998/namespace" in ""
            //        contentAsXmlString = contentAsXmlString.Replace("xmlns:d3p1=\"http://www.w3.org/XML/1998/namespace\"", "");
            //        contentAsXmlString = Regex.Replace(contentAsXmlString, "<address country=\"(.*?)\">(.*?)</address>", "<address country=\"$1\">$2<country>$1</country></address>");
            //        contentAsXmlString = Regex.Replace(contentAsXmlString, "<establishmentDate>(.*?)-(.*?)-(.*?)</establishmentDate>", "<establishmentDate>$1</establishmentDate>");
            //        //contentAsXmlString = Regex.Replace(contentAsXmlString, "<fundingBodies(.*?)>", "<fundingBodies xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-3.0\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-3.0/fundingBodies30.xsd\">");by Avanish on 26 MAy-2018
            //        contentAsXmlString = Regex.Replace(contentAsXmlString, "<fundingBodies(.*?)>", "<fundingBodies xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.elsevier.com/xml/schema/grant/grant-4.1\" xsi:schemaLocation=\"http://www.elsevier.com/xml/schema/grant/grant-4.1/fundingBodies41.xsd\">");
            //        contentAsXmlString = contentAsXmlString.Replace("xml:space=\"preserve\"", "");
            //        contentAsXmlString = contentAsXmlString.Replace("<eligibilityDescription >", "<eligibilityDescription>");
            //        //contentAsXmlString = contentAsXmlString.Replace("<eligibilityDescription>", "<eligibilityDescription><![CDATA[");
            //        //contentAsXmlString = contentAsXmlString.Replace("</eligibilityDescription>", "]]></eligibilityDescription>");
            //        contentAsXmlString = contentAsXmlString.Replace("xsi:nil=\"true\"", "");
            //        contentAsXmlString = contentAsXmlString.Replace("&amp;#", "&#");
            //        contentAsXmlString = contentAsXmlString.Replace("&amp;lt;", "&lt;");
            //        contentAsXmlString = contentAsXmlString.Replace("&amp;gt;", "&gt;");
            //        contentAsXmlString = r.ReadandReplaceHexaToChar(contentAsXmlString, XSDPath.Replace("\\XSD", ""));
            //        contentAsXmlString = contentAsXmlString.Replace("<room>Not Available</room>", "<room></room>");
            //        contentAsXmlString = contentAsXmlString.Replace("<street>Not Available</street>", "<street></street>");
            //        contentAsXmlString = contentAsXmlString.Replace("<city>Not Available</city>", "<city></city>");
            //        contentAsXmlString = contentAsXmlString.Replace("<state>Not Available</state>", "<state></state>");
            //        contentAsXmlString = contentAsXmlString.Replace("<postalCode>Not Available</postalCode>", "<postalCode></postalCode>");

            //        #region
            //        contentAsXmlString = contentAsXmlString.Replace("<recordSource>" + "\"", "<recordSource>");
            //        contentAsXmlString = contentAsXmlString.Replace("\"" + "</recordSource>", "</recordSource>");
            //        contentAsXmlString = contentAsXmlString.Replace("<email>" + "\"", "<email>");
            //        contentAsXmlString = contentAsXmlString.Replace("\"" + "</email>", "</email>");
            //        #endregion

            //        #region Hex Values which have to replaced into space on 15-Mar-2019
            //        contentAsXmlString = r.EntityToUnicode2(contentAsXmlString);
            //        contentAsXmlString = r.EntityToUnicode(contentAsXmlString);
            //        contentAsXmlString = contentAsXmlString.Replace("&#x00A0;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2002;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2003;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2004;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2005;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2006;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2007;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2008;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x2009;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x200A;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x200B;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#x3000;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("&#xFEFF;", " ");
            //        #endregion


            //        #region

            //        contentAsXmlString = contentAsXmlString.Replace("xmlns:d4p1=\"http://www.w3.org/XML/1998/namespace\"", "");
            //        contentAsXmlString = contentAsXmlString.Replace("d4p1:lang", "xml:lang");

            //        contentAsXmlString = contentAsXmlString.Replace("<region xml:lang=\"en\">", "<region>");
            //        contentAsXmlString = contentAsXmlString.Replace("<region xml:lang=\"en\" >", "<region>");
            //        contentAsXmlString = contentAsXmlString.Replace("&amp;nbsp;", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("  ", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("   ", " ");
            //        contentAsXmlString = contentAsXmlString.Replace("countryTest", "country");

            //        #endregion
            //        contentAsXmlString = contentAsXmlString.Replace(">0</totalFunding>", ">not-specified</totalFunding>");
            //        contentAsXmlString = Regex.Replace(contentAsXmlString, "\\s+(</[-A-Za-z]+>)", "$1");
            //        contentAsXmlString = Regex.Replace(contentAsXmlString, "(<[-A-Za-z]+>)\\s+", "$1");
            //        contentAsXmlString = HandleLegation(contentAsXmlString);
            //        contentAsXmlString = Filter(contentAsXmlString);
            //        Data.LoadXml(contentAsXmlString);
            //        SequenceFundingBodyXML(Data);
            //    }
            //    Error = ValidateXML(Data, 2, XSDPath);
            //    //pankaj 10 october s5
            //    string json2 = JsonConvert.SerializeObject(Source);

            //    // Newtonsoft.Json.Schema.JsonSchema schema21 = Newtonsoft.Json.Schema.JsonSchema.Parse(@"C:\Users\pankaj.tiwari\Desktop\9october\schema_made_bypankaj.json");



            //    #region SCHEMA 5.0 development
            //    //pankaj 18 october s5
            //    #region data capture for all module

            //    dtFundingBody = Source.Tables["FundingBody"];
            //    dt_preferredorgname = Source.Tables["preferredorgname"];
            //    dt_acronym = Source.Tables["acronym"];
            //    dt_abbrevname = Source.Tables["abbrevname"];
            //    dt_contextname = Source.Tables["contextname"];
            //    dt_subType = Source.Tables["subType"];
            //    dt_contact = Source.Tables["contact"];
            //    dt_website = Source.Tables["website"];
            //    dt_address = Source.Tables["address"];
            //    dt_establishmentInfo = Source.Tables["establishmentInfo"];
            //    dt_fundingPolicy = Source.Tables["fundingPolicy"];
            //    // dt_fundingPolicy_Deatails = Source.Tables["item"];

            //    dt_createddate = Source.Tables["createddate"];
            //    dt_reviseddate = Source.Tables["reviseddate"];
            //    dt_revisionhistory = Source.Tables["revisionhistory"];

            //    dt_link = Source.Tables["link"];
            //    //dt_fundingPolicy_Deatails = Source.Tables["fundingPolicyDeatails"];
            //    //dt_fundingBodyDataset = Source.Tables["fundingbodydataset"];
            //    //dt_awardSSOURCE = Source.Tables["awardssourcedataset"];
            //    //dt_OPPORTUNITIESSOURCE = Source.Tables["opportunitiessourcedataset"];
            //    //dt_publicationDataset = Source.Tables["publicationdataset"];

            //    dt_fundingPolicy_Deatails = Source.Tables["FundingXML36"];
            //    //dt_fundingBodyDataset = Source.Tables["FundingXML37"];
            //    //dt_awardSSOURCE = Source.Tables["FundingXML38"];
            //    //dt_OPPORTUNITIESSOURCE = Source.Tables["FundingXML39"];
            //    //dt_publicationDataset = Source.Tables["FundingXML40"];

            //    dt_fundingBodyDataset = Source.Tables["FundingXML40"];
            //    dt_awardSSOURCE = Source.Tables["FundingXML39"];
            //    dt_OPPORTUNITIESSOURCE = Source.Tables["FundingXML37"];
            //    dt_publicationDataset = Source.Tables["FundingXML38"];
            //    dt_identifier = Source.Tables["FundingXML41"];
            //    dt_fundingdescription = Source.Tables["FundingXML42"];
            //    // dt_fundingdescription = Source.Tables["item"];
            //    dt_awardSuccessRatedesc = Source.Tables["FundingXML43"];
            //    dt_releatedorgs = Source.Tables["org"];
            //    string jsonFbAlldata = JsonConvert.SerializeObject(Source);
            //    string jsonFundingxmldata = JsonConvert.SerializeObject(dtFundingBody);

            //    #endregion

            //    JsonCreationFromModel();

            //    #endregion


            //    //Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-3.0", "");by Avanish on 26 MAy-2018
            //    //commented by pankaj on 8 nov // Error = Error.Replace("http://www.elsevier.com/xml/schema/grant/grant-4.1", "");
            //    // Error = Error.Replace("'", ""); Error = Error.Replace(":", "");
            //    //  Error = Error.Replace("in namespace", "");


            //    Error = Error_text;
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    oErrorLog.WriteErrorLog(ex);
            //    Error = ex.Message;
            //}
            //return Error;
            ////throw new Exception("JSON file genrate successfully! for FB Module & File copied in temp Folder");
        }
    }
}