using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;
using Snake.Db.Helpers.Connection;
using Dapper;

namespace Snake.Db.Helpers.Query
{
    public class Query
    {
        private static MySqlConnection Connection => new MySqlConnection(Configuration.Db);
               
        /// <summary>
        /// Obtenir seulement un résultat exemple (SELECT * WHERE Id = 10)
        /// </summary>
        /// <typeparam name="t">Type de retour</typeparam>
        /// <param name="sql">Cmmande SQL à exécuter</param>
        /// <returns>Une model de type T</returns>
        public static t Get<t>(string sql, object model = null)
        {
            StringBuilder sqlStatement = FormatSql(sql);
            sqlStatement.Append(" LIMIT 1;");

            return Connection.Query<t>(sql, model).FirstOrDefault();
        }

        /// <summary>
        /// Obtenir une liste de résultat
        /// </summary>
        /// <typeparam name="t">Type de retour</typeparam>
        /// <param name="sql">Commande SQL à exécuter</param>
        /// <param name="model">Paramètre à passer</param>
        /// <returns>La liste de résultat obtenue</returns>
        public static List<t> Fetch<t>(string sql, object model = null)
        {
            StringBuilder sqlStatement = FormatSql(sql);

            return Connection.Query<t>(sqlStatement.ToString(), model).ToList();
        }


        /// <summary>
        /// Insérer une nouvelle ligne
        /// </summary>
        /// <param name="sql">Commande SQL à exécuter</param>
        /// <param name="model">Model Insert</param>
        /// <returns>Le Id de la ligne ajouté</returns>
        public static int Insert(string sql, object model)
        {
            StringBuilder sqlStatement = FormatSql(sql);
            VerifyModel(model);

            sqlStatement.Append(" SELECT LAST_INSERT_ID();");

            return Connection.ExecuteScalar<int>(sql.ToString(), model);
        }

        public static int? Update(string sql, object model)
        {
            StringBuilder sqlStatement = FormatSql(sql);
            VerifyModel(model);


            int rowAffected = Connection.Execute(sqlStatement.ToString(), model);
            if (rowAffected == 0)
            {
                return null;
            }
            return rowAffected;
        }

        private static StringBuilder FormatSql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("Sql statement NULL");
            }

            var sqlQuery = new StringBuilder();
            sqlQuery.Append(sql);
            sqlQuery.Append(';');
            return sqlQuery;
        }

        private static void VerifyModel(object model)
        {
            if (model is null)
            {
                throw new ArgumentNullException("Model is Null");
            }
        }
    }
}