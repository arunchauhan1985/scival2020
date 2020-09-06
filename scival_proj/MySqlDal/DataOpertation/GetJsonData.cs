using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONTransformation
{
    class GetJsonData
    {
        public List<DAL.preferredName> getPreferredNames()
        {
            List<DAL.preferredName> preferredNames = new List<DAL.preferredName>();

            return preferredNames;
        }

        public List<DAL.alternateName> getAlternateNames()
        {
            List<DAL.alternateName> alternateNames = new List<DAL.alternateName>();

            return alternateNames;
        }

        public List<DAL.abbrevName> getAbbrevNames()
        {
            List<DAL.abbrevName> abbrevNames = new List<DAL.abbrevName>();

            return abbrevNames;
        }

        public List<DAL.acronym> getacronymNames()
        {
            List<DAL.acronym> acronymNames = new List<DAL.acronym>();

            return acronymNames;
        }

        public List<DAL.relation> getrelations()
        {
            List<DAL.relation> relation = new List<DAL.relation>();

            return relation;
        }
    }
}
