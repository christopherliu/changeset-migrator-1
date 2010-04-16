using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionListMigrations.CustomMigrations;
using ActionListMigrations.Entities;
using ActionListMigrations.EntityFinders;
using ActionListMigrations.Extensions;
using Migrator.Framework;

namespace ActionListMigrations.Actions
{
    //new TableCreateAction("companies_subsidiaries_exploded_", 
    //new Column("companyId", System.Data.DbType.Int32, ColumnProperty.NotNull), 
    //new Column("childId", System.Data.DbType.Int32, ColumnProperty.NotNull), 
    //new Column("rptMonth", System.Data.DbType.DateTime, ColumnProperty.NotNull), 
    //new Column("iteration", System.Data.DbType.Int32, ColumnProperty.NotNull))

    /// <summary>
    /// Automatically creates a table. Example above.
    /// </summary>
    public class TableCreateAction : IMigrationAction
    {
        private string _tableName;
        private Column[] _columns;

        public TableCreateAction(string table, params Column[] columns)
        {
            _tableName = table;
            _columns = columns;
        }

        public void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.DropTable(_tableName);
        }

        public void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.AddTable(_tableName, _columns);
        }
    }
}
