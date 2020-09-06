using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Scival
{
    public class ErrorLog
    {
        public bool WriteErrorLog(Exception ex)
        {
            bool Status = false;
            string LogDirectory = Convert.ToString(ConfigurationManager.AppSettings["LogDirectory"]);

            CheckCreateLogDirectory(LogDirectory);

            string logLine = BuildLogLine(ex);

            LogDirectory = (LogDirectory + "Log_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;

                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(logLine);
                    Status = true;
                }
                catch { }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }

            return Status;
        }

        public bool WorkProcessLog(string CurrentStatus = "", string fileName = "")
        {
            bool Status = false;
            string LogDirectory = Convert.ToString(ConfigurationManager.AppSettings["LogDirectory"]);

            string msg = BuildWorkProcessLog(CurrentStatus, fileName);

            CheckCreateLogDirectory(LogDirectory);

            LogDirectory = (LogDirectory + "WorkProcessLog_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;

                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(msg);
                    Status = true;
                }
                catch { }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }

            return Status;
        }

        private bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);

            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                    // Logging failure
                }
            }

            return loggingDirectoryExists;
        }

        private string BuildLogLine(Exception ex)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(" \t");

            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;

            if (ex.InnerException != null)
            {
                message += string.Format("InnerException: {0}", ex.InnerException.ToString());
                message += Environment.NewLine;
            }

            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            loglineStringBuilder.Append(message);
            loglineStringBuilder.Append("__________________________");

            return loglineStringBuilder.ToString();
        }

        public string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }

        private string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd_MM_yyyy");
        }

        public string BuildWorkProcessLog(string CurrentStatus, string action = "")
        {
            string message = "";
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(" \t");

            if (!action.Equals(""))
            {
                message += Environment.NewLine;
                message += "-------------------*************New Process Start*************--------------------";
            }

            message += Environment.NewLine;
            message += string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += string.Format("Action: {0}", CurrentStatus);

            loglineStringBuilder.Append(message);

            return loglineStringBuilder.ToString();
        }

        public string htlmtag(string value, string controlName)
        {
            string _result = "";
            var tagList = new List<string>();
            string text = value.Replace("<", "< ").Replace(">", " >").Replace("/>", " />").Replace("\"", "'");
            Regex regStart = new Regex(@"< (.+?) >");
            MatchCollection matchesStart = regStart.Matches(text);

            foreach (Match match in matchesStart)
                tagList.Add(match.ToString().Replace(" />", "/>").Replace(" >", ">").Replace("< ", "<"));

            tagList = tagList.Distinct().ToList();
            string[] alowTagList = { "<br/>", "<p>", "</p>", "<ul>", "</ul>", "<li>", "</li>", "<ol>", "</ol>", "<h1>", "</h1>", "<h2>", "</h2>", "<h3>", "</h3>", "<h4>", "</h4>", "<h5>", "</h5>", "<h6>", "</h6>", "</a>", "<a href='#http'>", "<a href='#https'>", "<a href='#http'/>", "<a href='#https'/>" };

            foreach (string com in tagList)
            {
                string _ahref = "";
                Regex reg = new Regex("<a\\s\\w+");
                string _rep = removeSpace(com.ToString());
                MatchCollection matches = reg.Matches(_rep);

                foreach (Match match21 in matches)
                    _ahref = _rep;

                if (!_ahref.Equals(""))
                {
                    string _val = "";
                    string[] _arr = _ahref.Split('\'');
                    int count = _arr.Count();

                    for (int i = 0; i < count; i++)
                    {
                        if (i == 0)
                            _val += _arr[i].ToString();
                        else if (i == 1)
                        {
                            string[] _htt = _arr[i].ToString().Split(':');
                            _val += "'#" + _htt[0].ToString() + "'";
                        }
                        else if (i == count - 1)
                            _val += _arr[i].ToString();
                        else
                            _val += _arr[i].ToString();
                    }

                    if (!alowTagList.Contains(_val))
                        _result += _rep + ",";
                }
                else
                {
                    if (!alowTagList.Contains(com))
                    {
                        _result += com + ",";
                    }
                }
            }

            _result = _result.TrimEnd(',').Replace("'", "\"");

            if (!_result.Equals(""))
                _result += " html tags are not alowed at " + controlName + ".";

            return _result;
        }

        public string removeSpace(string _input)
        {
            string _result = "";
            string _output = "";
            int spcctr = 0;
            string str1;

            for (int i = 0; i < _input.Length; i++)
            {
                str1 = _input.Substring(i, 1);

                if (str1 == " ")
                {
                    _result += " ";
                    spcctr++;
                }
            }

            if (spcctr > 1)
                _output = _input.Replace(_result, " ");
            else
                _output = _input;

            return _output;
        }
    }
}
