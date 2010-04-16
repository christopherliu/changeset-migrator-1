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
    public class SvnUpdateAction : IMigrationAction
    {
        private SqlSvnEntity _downEntity, _upEntity;

        public SvnUpdateAction(string entityName, long downRevision, long upRevision)
        {
            _downEntity = new SqlSvnEntity(entityName, downRevision);
            _upEntity = new SqlSvnEntity(entityName, upRevision);
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
            database.Logger.Log("Running SQL entity {0}:{1}", _upEntity.FullName, _upEntity.SVNRevision);
            database.ExecuteSql(
                entityFinder.GetSQL(_upEntity));
        }

        #endregion
    }
}
