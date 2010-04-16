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
    /// Action that deletes a procedure/table/other entity from memory.
    /// </summary>
    public class SvnDeleteAction : IMigrationAction
    {
        private SqlSvnEntity _downEntity;

        public SvnDeleteAction(string entityName, long downRevision)
        {
            _downEntity = new SqlSvnEntity(entityName, downRevision);
        }

        #region IMigrationAction Members

        public void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.Logger.Log("Running SQL entity {0}:{1}", _downEntity.FullName, _downEntity.SVNRevision);
            database.ExecuteSql(
                entityFinder.GetSQL(_downEntity));
        }

        public void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            string nameInDb = string.Format("[{0}].[{1}]", _downEntity.Namespace, _downEntity.Name);
            switch (_downEntity.Type)
            {
                case SqlEntityType.Function:
                    database.DropFunction(nameInDb);
                    break;
                case SqlEntityType.Procedure:
                    database.DropStoredProcedure(nameInDb);
                    break;
                case SqlEntityType.Table:
                    //Database.Logger.Log("Dropping table " + sqlEntity.Name);
                    database.RemoveTable(_downEntity.Name);
                    break;
                case SqlEntityType.View:
                    database.DropView(nameInDb);
                    break;
                default:
                    throw new Exception("SqlFileType is not supported for deletion: " + _downEntity.Type);
            }
        }

        #endregion
    }
}
