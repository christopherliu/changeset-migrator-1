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
    /// <summary>
    /// Automatically inserts and deletes a row into a table.
    /// </summary>
    public class TableInsertAction : IMigrationAction
    {
        private string _table;
        private string[] _columns;
        private string[] _values;

        public TableInsertAction(string table, string[] columns, string[] values)
        {
            _table = table;
            _columns = columns;
            _values = values;
        }

        public void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.Delete(_table, _columns, _values);
        }

        public void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.Insert(_table, _columns, _values);
        }
    }
}
