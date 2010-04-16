using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActionListMigrations.Entities
{
    /// <summary>
    /// Points to SQL.
    /// </summary>
    public class SqlNamedEntity : SqlEntity
    {
        /// <summary>
        /// The type of SQL entity this is (table, function, etc.)
        /// </summary>
        public SqlEntityType Type { get; private set; }
        /// <summary>
        /// The name without prefixes like tables\, functions\, etc.
        /// </summary>
        public String Name { get; private set; }

        public SqlNamedEntity(string entityName)
        {
            FullName = entityName;
            Type = GetEntityType(entityName);
            Name = entityName.Substring(entityName.IndexOf("\\") + 1);
        }

        private static SqlEntityType GetEntityType(string entityName)
        {
            entityName = entityName.ToLower();
            SqlEntityType entityType = SqlEntityType.Procedure;
            if (entityName.StartsWith("functions\\"))
                entityType = SqlEntityType.Function;
            else if (entityName.StartsWith("views\\"))
                entityType = SqlEntityType.View;
            else if (entityName.StartsWith("tables\\"))
                entityType = SqlEntityType.Table;
            else if (entityName.StartsWith("indices\\"))
                entityType = SqlEntityType.Index;
            else if (entityName.StartsWith("dml\\"))
                entityType = SqlEntityType.Dml;
            else if (entityName.StartsWith("triggers\\"))
                entityType = SqlEntityType.Trigger;
            return entityType;
        }
    }
}
