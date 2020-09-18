using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlDal;
using MySqlDal.DataOpertation;
using MySqlDalAL;

namespace JSONTransformation
{
    class GetJsonData
    {
        public List<preferredName> getPreferredNames()
        {
            List<preferredName> preferredNames = new List<preferredName>();

            return preferredNames;
        }

        public List<alternateName> getAlternateNames()
        {
            List<alternateName> alternateNames = new List<alternateName>();

            return alternateNames;
        }

        public List<abbrevName> getAbbrevNames()
        {
            List<abbrevName> abbrevNames = new List<abbrevName>();

            return abbrevNames;
        }

        public List<acronym> getacronymNames()
        {
            List<acronym> acronymNames = new List<acronym>();

            return acronymNames;
        }

        public List<relation> getrelations()
        {
            List<relation> relation = new List<relation>();

            return relation;
        }
    }
}
