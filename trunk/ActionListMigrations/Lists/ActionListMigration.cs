using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migrator.Framework;
using ActionListMigrations.EntityFinders;
using ActionListMigrations.CustomMigrations;

namespace ActionListMigrations.Lists
{
    /// <summary>
    /// Author: Christopher Liu, 4/2/2010
    /// This class is a convenient default class for migrations that are
    /// lists of SQL entities.
    /// 
    /// Instructions:
    /// 
    /// Pass in an ordered list of IMigrationActions for all of your
    /// stored procedures, views, tables, etc. Not all MigrationActions exist (for example, there's
    /// no CreateIndexAction); see examples if you need to make one.
    /// </summary>
    public abstract class ActionListMigration : Migration
    {
        private List<IMigrationAction> _allSqlMigrationActions;
        private IMigrationEntityFinder _migrationEntityFinder;
        private bool _dontReverseWhenGoingDown;

        #region "Constructors and public functions"
        /// <summary>
        /// Create a new ActionListMigration. By default, down will go in the
        /// reverse order of up.
        /// </summary>
        /// <param name="_allSQLEntities"></param>
        public ActionListMigration(List<IMigrationAction> allSqlMigrationActions)
            : this(allSqlMigrationActions, false)
        {
        }
        /// <summary>
        /// Create a new ActionListMigration. You can specify whether to
        /// reverse the order of application when going down vs. up.
        /// </summary>
        /// <param name="_allSQLEntities"></param>
        /// <param name="dontReverseWhenGoingDown"></param>
        public ActionListMigration(List<IMigrationAction> allSqlMigrationActions, bool dontReverseWhenGoingDown)
        {
            _allSqlMigrationActions = allSqlMigrationActions;
            _dontReverseWhenGoingDown = dontReverseWhenGoingDown;
            //TODO I think it would be nice to pull this out of the library.
            _migrationEntityFinder
                = new SvnMigrationEntityFinder(
                    ActionListMigrations.Properties.Settings.Default.SqlSvnRoot);
        }
        #endregion

        private void _runSQLEntityDown(IMigrationAction entity)
        {
            entity.Down(Database, _migrationEntityFinder);
        }
        private void _runSQLEntityUp(IMigrationAction entity)
        {
            entity.Up(Database, _migrationEntityFinder);
        }

        public override void Down()
        {
            //Look for the latest down
            //Database.Logger.Log("ActionListMigration: Removing version {0}, case {1}", MyMRMigrationAttribute.Version, MyMRMigrationAttribute.CaseNumber);

            if (!_dontReverseWhenGoingDown)
                _allSqlMigrationActions.Reverse();
            foreach (IMigrationAction s in _allSqlMigrationActions)
                _runSQLEntityDown(s);//the .ForEach method doesn't actually reverse
            if (!_dontReverseWhenGoingDown)
                _allSqlMigrationActions.Reverse();
        }
        public override void Up()
        {
            //Database.Logger.Log("ActionListMigration: Applying version {0}, case {1}", MyMRMigrationAttribute.Version, MyMRMigrationAttribute.CaseNumber);

            _allSqlMigrationActions.ForEach(_runSQLEntityUp);
        }
    }
}
