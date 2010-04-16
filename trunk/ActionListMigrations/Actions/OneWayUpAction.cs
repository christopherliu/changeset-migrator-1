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
    /// A migration that cannot be undone.
    /// Be careful using this - it's generally only advisable in very specific circumstances.
    /// The first use of this was considered for a primary key update, the next for an item
    /// that generates foreign key constraints (the log).
    /// </summary>
    public class OneWayUpAction : IMigrationAction
    {
        private IMigrationAction _upOnlyAction;
        public OneWayUpAction(IMigrationAction upOnlyAction)
        {
            _upOnlyAction = upOnlyAction;
        }

        #region IMigrationAction Members

        public void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            database.Logger.Log("No reverse for this update.");
        }

        public void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder)
        {
            _upOnlyAction.Up(database, entityFinder);
        }

        #endregion
    }
}
