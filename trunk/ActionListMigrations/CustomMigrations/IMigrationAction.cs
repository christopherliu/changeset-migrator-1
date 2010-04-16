using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migrator.Framework;
using ActionListMigrations.EntityFinders;

namespace ActionListMigrations.CustomMigrations
{
    /// <summary>
    /// Represents one action in an ActionListMigration. Actions move up and down, given
    /// a target (ITransformationProvider) and a resource locator (IMigrationEntityFinder).
    /// </summary>
    public interface IMigrationAction
    {
        void Down(ITransformationProvider database, IMigrationEntityFinder entityFinder);
        void Up(ITransformationProvider database, IMigrationEntityFinder entityFinder);
    }
}
