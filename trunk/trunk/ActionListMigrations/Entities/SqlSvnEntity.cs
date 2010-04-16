using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActionListMigrations.Entities
{
    /// <summary>
    /// Points to SQL. Currently SVN specific; we might want to allow, say, Git support in the future
    /// by abstracting this to an interface.
    /// </summary>
    public class SqlSvnEntity : SqlNamedEntity
    {
        /// <summary>
        /// The revision of this file in SVN.
        /// </summary>
        public long SVNRevision { get; private set; }

        public SqlSvnEntity(string entityName, long svnRevision): base(entityName)
        {
            SVNRevision = svnRevision;
        }
    }
}
