using System;
using System.Windows.Forms;
using System.Xml;

namespace Scival.XML
{
    public partial class ShowXml : BaseForm
    {
        ErrorLog oErrorLog = new ErrorLog();
        string Xmlpath = string.Empty;

        public ShowXml(string path)
        {
            InitializeComponent();

            Xmlpath = path;
            LoadInitialValue();
        }

        private void LoadInitialValue()
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath() + "XMLZip\\" + Xmlpath;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(tempPath);
                webBrowser1.Url = new Uri(xmlDoc.BaseURI);

                this.Dispose();
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ShowXml_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
