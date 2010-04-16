using System;
using ActionListMigrations.Entities;

namespace ActionListMigrations.EntityFinders
{
    /// <summary>
    /// Classes that implement this interface must be able to take a SqlEntity
    /// and find/create a sql to be executed.
    /// </summary>
    public interface IMigrationEntityFinder
    {
        string GetSQL(SqlEntity sqlEntity);
    }
}
