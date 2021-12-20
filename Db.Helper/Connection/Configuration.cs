using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Db.Helpers.Connection
{
    public static class Configuration
    {
        public static void SetConnection(string connectionString, bool isSql)
        {
            Db = connectionString;
            IsSql = isSql;
        }

        public static bool IsSql { get; set; }
        public static string Db { get; set; }
    }
}
