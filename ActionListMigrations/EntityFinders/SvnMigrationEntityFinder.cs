using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionListMigrations.Entities;
using SharpSvn;
using System.IO;

namespace ActionListMigrations.EntityFinders
{
    /// <summary>
    /// Maps a SqlSvnEntity to the text of the procedure.
    /// </summary>
    public class SvnMigrationEntityFinder : IMigrationEntityFinder
    {
        private string _sqlSvnRoot;

        public SvnMigrationEntityFinder(string sqlSvnRoot)
        {
            _sqlSvnRoot = sqlSvnRoot;
        }

        protected const string PROCEDURES_DIRECTORY = @"/procedures/";
        protected const string FUNCTIONS_DIRECTORY = @"/functions/";
        protected const string TABLES_DIRECTORY = @"/tables/";
        protected const string VIEWS_DIRECTORY = @"/views/";
        protected const string INDICES_DIRECTORY = @"/indices/";
        protected const string DML_DIRECTORY = @"/dml/";
        protected const string TRIGGERS_DIRECTORY = @"/triggers/";

        protected string GetSqlFilePath(string fileName, SqlEntityType type)
        {
            switch (type)
            {
                case SqlEntityType.Function:
                    return Path.Combine(FUNCTIONS_DIRECTORY, fileName);
                case SqlEntityType.Table:
                    return Path.Combine(TABLES_DIRECTORY, fileName);
                case SqlEntityType.View:
                    return Path.Combine(VIEWS_DIRECTORY, fileName);
                case SqlEntityType.Index:
                    return Path.Combine(INDICES_DIRECTORY, fileName);
                case SqlEntityType.Dml:
                    return Path.Combine(DML_DIRECTORY, fileName);
                case SqlEntityType.Trigger:
                    return Path.Combine(TRIGGERS_DIRECTORY, fileName);
                default:
                    return Path.Combine(PROCEDURES_DIRECTORY, fileName);
            }

        }

        #region IMigrationEntityFinder Members

        public string GetSQL(ActionListMigrations.Entities.SqlEntity sqlEntity)
        {
            if (sqlEntity is SqlSvnEntity)
            {
                SqlSvnEntity sqlSvnEntity = (SqlSvnEntity)sqlEntity;

                using (SvnClient client = new SvnClient())
                {
                    string path = _sqlSvnRoot
                                            + GetSqlFilePath(sqlSvnEntity.Name, sqlSvnEntity.Type)
                                            + ".sql";
                    SvnTarget x = SvnTarget.FromString(path);
                    //Console.WriteLine("SQL: \"" + path + "\"");

                    using (MemoryStream tempStream = new MemoryStream(9000))
                    {
                        SvnWriteArgs writeArgs = new SvnWriteArgs();
                        writeArgs.Revision = sqlSvnEntity.SVNRevision;
                        client.Write(x, tempStream, writeArgs);
                        tempStream.Flush();
                        tempStream.Position = 0;
                        using (StreamReader reader = new StreamReader(tempStream))
                        {
                            string readerReadToEnd = reader.ReadToEnd();
                            //Console.WriteLine("SQL: \"" + readerReadToEnd + "\"");
                            return readerReadToEnd;
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException("SvnMigrationFileMapper currently works only on SqlSvnEntities.");
            }
        }

        #endregion
    }
}
