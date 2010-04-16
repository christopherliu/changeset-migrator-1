using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migrator.Framework;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using System.Xml.Linq;

namespace ActionListMigrations.Extensions
{

    public static class DatabaseExtensions
    {

        private static string _connectionString = null;

        static DatabaseExtensions()
        {
            _connectionString = getNAntConnectionString();
        }
        /// <summary>
        /// Does not log - you must log this from the outer calling function.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sqlToExecute"></param>
        public static void ExecuteSql(this ITransformationProvider database, string sqlToExecute)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            Server server = new Server(new ServerConnection(connection));
            server.ConnectionContext.ExecuteNonQuery(sqlToExecute);            

        }
        /// <summary>
        /// Execute an arbitrary SQL file.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="file"></param>
        public static void ExecuteSqlFile(this ITransformationProvider database, string file)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            Server server = new Server(new ServerConnection(connection));
            server.ConnectionContext.ExecuteNonQuery(File.ReadAllText(file));
            database.Logger.Log("Ran SQL file {0}", file);

        }

        /// <summary>
        /// Drops the function, if it exists.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="viewName"></param>
        public static void DropFunction(this ITransformationProvider database, string functionName)
        {
            database.ExecuteSql(
                string.Format(@"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
    DROP FUNCTION {0}
GO", functionName));
            database.Logger.Log("Dropped function {0}", functionName);
        }
        /// <summary>
        /// Drops the stored procedure, if it exists.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        public static void DropStoredProcedure(this ITransformationProvider database, string procedureName)
        {
            database.ExecuteSql(
                string.Format(@"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'P', N'PC'))
    DROP PROCEDURE {0}
GO", procedureName));
            database.Logger.Log("Dropped stored procedure {0}", procedureName);
        }


        /// <summary>
        /// Drops the view, if it exists.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="viewName"></param>
        public static void DropView(this ITransformationProvider database, string viewName)
        {
            database.ExecuteSql(
                string.Format(@"
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'{0}'))
    DROP VIEW {0}
GO", viewName));
            database.Logger.Log("Dropped view {0}", viewName);
        }

        /// <summary>
        /// Drops the table, if it exists.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="viewName"></param>
        public static void DropTable(this ITransformationProvider database, string tableName) {
            database.ExecuteSql(
                string.Format(@"
IF EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}')
	DROP TABLE {0}
GO", tableName));
            database.Logger.Log("Dropped table {0}", tableName);
        }


        public static void RenameStoredProcedure(this ITransformationProvider database, string oldProcedureName, string newProcedureName)
        {
            database.ExecuteSql(
                string.Format(@"
sp_rename '{0}', '{1}'
GO", oldProcedureName, newProcedureName));
            database.Logger.Log("Renamed stored procedure {0} to {1}", oldProcedureName, newProcedureName);
        }

        private static string getNAntConnectionString()
        {
            return XDocument.Load("NAnt.build")
                    .Element("project")
                    .Element("target")
                    .Element("migrate")
                    .Attribute("connectionstring")
                    .Value;
        }

    }
}
