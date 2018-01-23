using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambition.Entities.Common;

namespace Ambition.DataAccess
{
    public class AmbitionDataContext: DbContext
    {
        public AmbitionDataContext()
            :base("DbConnection")
        { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Ambition");
        }
    }
}
