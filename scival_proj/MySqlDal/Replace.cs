using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MySqlDal
{
    public class Replace
    {
        public List<string> cultureList = new List<string>();
        public List<Int64> cultureIds = new List<Int64>();

        public Replace()
        {
            cultureList.Add("zh");//Chinese
            cultureList.Add("ja");//Japanese
            cultureList.Add("ko");//Korean
            cultureList.Add("ru");//Russian
            cultureList.Add("fa");//Persian
            cultureList.Add("hi");//Hindi
            cultureList.Add("he");//Hebrew
            cultureList.Add("th");//Thai
            cultureList.Add("ar");//Arabic
            cultureList.Add("ur");//Urdu
            cultureList.Add("ps");//Pushto
            cultureList.Add("ne");//Nepali
            cultureList.Add("lo");//Laotian
            cultureList.Add("bg");//Bulgarian
            cultureList.Add("el");//Greek
            cultureList.Add("bn");//Bengali
            cultureList.Add("ca");//Catalan
            cultureList.Add("cs");//Czech
            cultureList.Add("ka");//Georgian 
            cultureList.Add("lv");//Latvian
            cultureList.Add("lt");//Lithuanian
            cultureList.Add("mk");//Macedonian
            cultureList.Add("ms");//Malay
            cultureList.Add("mi");//Maori
            cultureList.Add("mn");//Mongolian
            cultureList.Add("pl");//Polish
            cultureList.Add("ro");//Romanian
            cultureList.Add("tr");//Turkish
            cultureList.Add("uk");//Ukrainian
            cultureList.Add("uz");//Uzbek
            cultureList.Add("vi");//Vietnamese
            cultureList.Add("sr");//Serbian
            cultureList.Add("gd");//Scottish Gaelic
            cultureList.Add("si");//Sinhalese

            cultureIds.Add(37);//Chinese
            cultureIds.Add(100);//Japanese
            cultureIds.Add(103);//Korean
            cultureIds.Add(148);//Russian
            cultureIds.Add(130);//Persian
            cultureIds.Add(82);//Hindi
            cultureIds.Add(79);//Hebrew
            cultureIds.Add(175);//Thai
            cultureIds.Add(7);//Arabic
            cultureIds.Add(184);//Urdu
            cultureIds.Add(142);//Pushto
            cultureIds.Add(193);//Nepali
            cultureIds.Add(195);//Laotian
            cultureIds.Add(28);//Bulgarian
            cultureIds.Add(76);//Greek 
            cultureIds.Add(22);//Bengali
            cultureIds.Add(34);//Catalan
            cultureIds.Add(43);//Czech
            cultureIds.Add(70);//Georgian 
            cultureIds.Add(109);//Latvian
            cultureIds.Add(112);//Lithuanian
            cultureIds.Add(115);//Macedonian
            cultureIds.Add(118);//Malay
            cultureIds.Add(121);//Maori
            cultureIds.Add(124);//Mongolian
            cultureIds.Add(133);//Polish
            cultureIds.Add(145);//Romanian
            cultureIds.Add(178);//Turkish
            cultureIds.Add(181);//Ukrainian
            cultureIds.Add(187);//Uzbek
            cultureIds.Add(190);//Vietnamese
            cultureIds.Add(154);//Serbian
            cultureIds.Add(151);//Scottish Gaelic
            cultureIds.Add(157);//Sinhalese
        }

        public string ReadandReplaceCharToHexa(string str, string appPath)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char x in str)
            {
                string HexaValue = GetValueFromXML(Convert.ToString(x).Trim(), "Hexa", appPath);

                if (HexaValue == null)
                    sb.Append(x);
                else
                {
                    if (HexaValue.Trim() != "")
                        sb.Append(HexaValue);
                    else
                        sb.Append(x);
                }
            }

            return Convert.ToString(sb);
        }

        public string ReadandReplaceHexaToChar(string str, string appPath)
        {
            StringBuilder HexaValue = new StringBuilder();
            StringBuilder FinalString = new StringBuilder();
            int Count = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '&' && str[i + 1] == '#')
                    Count++;

                if (Count > 0)
                    HexaValue.Append(str[i]);
                else
                    FinalString.Append(str[i]);

                if (str[i] == ';')
                {
                    Count = 0;
                    string SpecialCharacter = GetValueFromXML(Convert.ToString(HexaValue).Trim(), "Char", appPath);
                    FinalString.Append(SpecialCharacter);
                    HexaValue.Length = 0;
                }
            }

            return Convert.ToString(FinalString);
        }

        public string GetValueFromXML(string _character, string Check, string appfilePath)
        {
            string Character = string.Empty;
            string HexaValue = string.Empty;
            string regexPtrn = string.Empty;

            Regex rgx;
            MatchCollection matches;

            string filePath = appfilePath + @"\XMLFile\SpecialCharacter.xml";

            StreamReader sr = new StreamReader(filePath);

            string passVal = _character, passHexa = _character;
            if (_character != string.Empty)
            {
                if (Check == "Hexa")
                {
                    //Regex pattern for fetch hexavalue where character=_character
                    regexPtrn = "(<Character Value=\")[" + passVal + "](\" Hexa=\")(?<HEX>.+)(\")";
                }
                else
                {
                    //Regex pattern for fetch Value where hexavalue=_hexavalue
                    regexPtrn = "(<Character Value=\")(?<VAL>.+)(\" Hexa=\")" + passHexa + "(\")";
                }

                string XmlFileContent = sr.ReadToEnd();
                rgx = new Regex(regexPtrn, RegexOptions.None);
                matches = rgx.Matches(XmlFileContent);

                foreach (Match mt in matches)
                {
                    if (Check == "Hexa")
                        HexaValue = mt.Groups["HEX"].Value;
                    else
                        Character = mt.Groups["VAL"].Value;
                    break;
                }
            }

            sr.Close();

            if (Check == "Hexa")
                return HexaValue.Trim();
            else
                return Character.Trim();
        }

        public string ConvertTextToUnicode(string difflangval)
        {
            try
            {
                if (difflangval.Trim() != "")
                {
                    string BYtesStream = "";

                    string unicodeString = difflangval;

                    // Create two different encodings.
                    Encoding ascii = Encoding.UTF8;

                    // Encoding unicode = Encoding.Unicode;
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

                    // Convert the new byte[] into a char[] and then into a string.
                    char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];

                    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                    string asciiString = new string(asciiChars);

                    return BYtesStream;
                }
                else
                {
                    return String.Empty;
                }
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
                if (difflangval.Trim() != "")
                {
                    string BYtesStream = "";

                    //// Create two different encodings.
                    Encoding ascii = Encoding.UTF8;
                    //// Encoding unicode = Encoding.Unicode;
                    Encoding unicode = Encoding.UTF8;

                    BYtesStream = difflangval;
                    char[] splitchar = { '|' };
                    string[] str = (BYtesStream.TrimStart().TrimEnd().Split(splitchar));
                    byte[] bytes = BYtesStream.Split('|').Select(s => Convert.ToByte(s)).ToArray();

                    byte[] unicodeBytes = bytes;

                    byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

                    string data1 = Encoding.UTF8.GetString(bytes);

                    // Convert the new byte[] into a char[] and then into a string.
                    char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);

                    string asciiString = new string(asciiChars);

                    return asciiString;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public bool chk_OtherLang(string langcode)
        {
            if (cultureList.Contains(langcode) == true)
                return true;
            else
                return false;
        }

        public bool chk_OtherLangId(int langId)
        {
            if (cultureIds.Contains(langId) == true)
                return true;
            else
                return false;
        }

        public string ToHtml(string s, bool nofollow)
        {
            s = HttpUtility.HtmlEncode(s);
            string[] paragraphs = s.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();

            foreach (string par in paragraphs)
            {
                sb.AppendLine("<p>");
                string p = par.Replace(Environment.NewLine, "<br />\r\n");

                if (nofollow)
                {
                    p = Regex.Replace(p,@"\[\[(.+)\]\[(.+)\]\]", "<a href=\"$2\" rel=\"nofollow\">$1</a>");
                    p = Regex.Replace(p,@"\[\[(.+)\]\]", "<a href=\"$1\" rel=\"nofollow\">$1</a>");
                }
                else
                {
                    p = Regex.Replace(p,@"\[\[(.+)\]\[(.+)\]\]", "<a href=\"$2\">$1</a>");
                    p = Regex.Replace(p,@"\[\[(.+)\]\]", "<a href=\"$1\">$1</a>");
                    sb.AppendLine(p);
                }

                sb.AppendLine("</p>");
            }

            return sb.ToString();
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

                            if (unicode.Length == 1)
                            {

                                string s = String.Format("{0:x4}", (uint)System.Convert.ToUInt32(unicode[0]));
                                replacements.Add(match.Value, string.Concat("&#x", s, ";"));
                            }
                        }
                    }
                }

                foreach (var replacement in replacements)
                    html = html.Replace(replacement.Key, replacement.Value);
            }
            catch { }

            return html;
        }

        public string ConvertStringToHex(string asciiString)
        {
            string hex = "";

            foreach (char c in asciiString)
            {
                int tmp = c;
                string val = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }

            return hex;
        }

        public string ConvertHexToString(string HexValue)
        {
            string StrValue = "";

            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }

            return StrValue;
        }

        public string WieredChar_ReplacementHexValue(string TextDetail)
        {
            //OracleConnection Cn = new OracleConnection(ConfigurationManager.AppSettings["OraCon"].ToString());
            //OracleCommand Cmd = null;
            //OracleDataAdapter Adpter = null;
            //OracleParameter Param = null;

            //DataSet DS = new DataSet();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("value");

            //try
            //{
            //    string correct = "", HexValue = "", intermed = "";

            //    string a = TextDetail;

            //    Cmd = new OracleCommand();
            //    Cmd.CommandText = "Chk_ValidText";
            //    Cmd.CommandType = CommandType.StoredProcedure;
            //    Cmd.Connection = Cn;

            //    //Parameter for CLOB
            //    Param = new OracleParameter();
            //    Param.Direction = ParameterDirection.Input;
            //    Param.OracleType = OracleType.Clob;
            //    Param.ParameterName = "p_Text";
            //    Param.Value = a;
            //    Cmd.Parameters.Add(Param);

            //    //Parameter for NCLOB
            //    Param = new OracleParameter();
            //    Param.Direction = ParameterDirection.Input;
            //    Param.OracleType = OracleType.NClob;
            //    Param.ParameterName = "p_TextN";
            //    Param.Value = a;
            //    Cmd.Parameters.Add(Param);

            //    //Parameter for Ref Cursor
            //    Param = new OracleParameter();
            //    Param.OracleType = OracleType.BFile;// RefCursor;
            //    Param.ParameterName = "p_result1";
            //    Param.Direction = ParameterDirection.Output;
            //    Cmd.Parameters.Add(Param);

            //    Adpter = new OracleDataAdapter(Cmd);

            //    Adpter.Fill(DS, "TEXT");

            //    char[] names1 = a.ToCharArray(); char[] names2 = DS.Tables["TEXT"].Rows[0]["Text"].ToString().ToCharArray();

            //    IEnumerable<char> differenceQuery = names1.Except(names2);

            //    foreach (char s in differenceQuery)
            //    {
            //        HexValue = String.Format("{0:x4}", (uint)System.Convert.ToUInt32(s));
            //        HexValue = string.Concat("&#x", HexValue, ";");
            //        intermed = TextDetail.Replace(Convert.ToString(s), Convert.ToString(HexValue));

            //        TextDetail = intermed;
            //    }

            //    DataRow dr = dt.NewRow();
            //    dr[0] = TextDetail;

            //    dt.Rows.Add(dr);
            //}
            //catch (Exception ex)
            //{
            //    oErrorLog.WriteErrorLog(ex);
            //}
            //finally
            //{
            //    Cn.Close();
            //}

            return TextDetail;
        }

        public string Return_WieredChar_Original(string str)
        {
            try
            {
                StringBuilder HexaValue = new StringBuilder();
                StringBuilder FinalString = new StringBuilder();
                int Count = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '&' && str[i + 1] == '#')
                        Count++;

                    if (Count > 0)
                        HexaValue.Append(str[i]);
                    else
                        FinalString.Append(str[i]);

                    if (str[i] == ';')
                    {
                        Count = 0;
                        string SpecialCharacter = Convert.ToString(HexaValue).Replace("&#x", "").Replace(";", "");

                        char dd = System.Convert.ToChar(System.Convert.ToUInt32(SpecialCharacter, 16));
                        SpecialCharacter = Convert.ToString(dd);

                        if (SpecialCharacter == "" && Convert.ToString(HexaValue).Trim() != "")
                            SpecialCharacter = Convert.ToString(HexaValue).Trim();

                        FinalString.Append(SpecialCharacter);
                        HexaValue.Length = 0;
                    }
                }
                return Convert.ToString(FinalString);
            }
            catch
            {
                return str;
            }
        }

        public string EntityToUnicode2(string html)
        {
            try
            {
                var replacements = new Dictionary<string, string>();
                var regex = new Regex("(&amp;[a-zA-Z]{2,11};)");
                foreach (Match match in regex.Matches(html))
                {
                    if (!replacements.ContainsKey(match.Value))
                    {
                        var unicode = match.Value.Replace("&amp;", "&");

                        replacements.Add(match.Value, unicode);


                    }
                }
                foreach (var replacement in replacements)
                {
                    html = html.Replace(replacement.Key, replacement.Value);
                }
            }
            catch { }

            return html;
        }
    }
}
