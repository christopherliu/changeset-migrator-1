using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionListMigrations.Properties;

namespace ActionListMigrations.Entities
{
    public abstract class SqlEntity
    {
        /// <summary>
        /// What database namespace are we working out of?
        /// </summary>
        public string Namespace = Settings.Default.SqlDbNamespace;
        /// <summary>
        /// The name of the entity, INCLUDING prefixes (the table name, the stored proc name, etc.)
        /// </summary>
        public string FullName { get; protected set; }
    }
}
