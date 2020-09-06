using System.Windows.Forms;

namespace Scival.Opportunity
{
    public partial class CountryNotInList : BaseForm
    {
        public CountryNotInList()
        {
            InitializeComponent();
            loadInitailValue();
        }

        private void loadInitailValue()
        {
            rchTextRemark.Text = SharedObjects.NotListedCountry;
        }
    }
}
