using System;

namespace MySqlDal
{
    public class ScivalDataException : Exception
    {
        public ScivalDataException(string message) : base(message)
        { }
    }
}
