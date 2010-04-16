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
    public class SvnCreateAction : IMigrationAction
    {
        private SqlSvnEntity _upEntity;

        public SvnCreateAction(string entityName, long upRevision)
        {
            _upEntity = new SqlSvnEntity(entityName, upRevision);
        }

        #region IMigrationAction Members

        public void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            string nameInDb = string.Format("[{0}].[{1}]", _upEntity.Namespace, _upEntity.Name);
            switch (_upEntity.Type)
            {
                case SqlEntityType.Function:
                    database.DropFunction(nameInDb);
                    break;
                case SqlEntityType.Procedure:
                    database.DropStoredProcedure(nameInDb);
                    break;
                case SqlEntityType.Table:
                    //Database.Logger.Log("Dropping table " + sqlEntity.Name);
                    database.RemoveTable(_upEntity.Name);
                    break;
                case SqlEntityType.View:
                    database.DropView(nameInDb);
                    break;
                default:
                    throw new Exception("SqlFileType is not supported for deletion: " + _upEntity.Type);
            }
        }

        public void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.Logger.Log("Running SQL entity {0}:{1}", _upEntity.FullName, _upEntity.SVNRevision);
            database.ExecuteSql(
                entityFinder.GetSQL(_upEntity));
        }

        #endregion
    }
}
