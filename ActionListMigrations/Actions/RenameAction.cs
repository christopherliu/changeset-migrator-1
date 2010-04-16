using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migrator.Framework;

using ActionListMigrations.Extensions;
using ActionListMigrations.Entities;
using ActionListMigrations.CustomMigrations;
using ActionListMigrations.EntityFinders;

namespace ActionListMigrations.Actions
{
    /// <summary>
    /// Renames a procedure/table/other entity. Does not require SVN access.
    /// </summary>
    public class RenameAction : IMigrationAction
    {
        private SqlNamedEntity oldSqlEntity;
        private string _newName;
        public RenameAction(string entityName, string newName)
        {
            oldSqlEntity = new SqlNamedEntity(entityName);
            this._newName = newName;
        }

        #region IMigrationAction Members

        public void Down(ITransformationProvider Database, IMigrationEntityFinder entityFinder)
        {
            switch (oldSqlEntity.Type)
            {
                case SqlEntityType.Procedure:
                    Database.RenameStoredProcedure(_newName, oldSqlEntity.Name);
                    break;
                case SqlEntityType.Table:
                    Database.RenameTable(_newName, oldSqlEntity.Name);
                    break;
                default:
                    throw new Exception("SqlEntityType is not supported for renaming: " + oldSqlEntity.Type);
            }
        }

        public void Up(ITransformationProvider Database, IMigrationEntityFinder entityFinder)
        {
            switch (oldSqlEntity.Type)
            {
                case SqlEntityType.Procedure:
                    Database.RenameStoredProcedure(oldSqlEntity.Name, _newName);
                    break;
                case SqlEntityType.Table:
                    Database.RenameTable(oldSqlEntity.Name, _newName);
                    break;
                default:
                    throw new Exception("SqlEntityType is not supported for renaming: " + oldSqlEntity.Type);
            }
        }

        #endregion
    }
}
