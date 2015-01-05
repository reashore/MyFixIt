
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MyFixIt.Persistence
{
    public class MyFixItContext : DbContext
    {
        public MyFixItContext()
            : base("name=appdb")
        {
        }

        public DbSet<FixItTask> FixItTasks { get; set; }
    }

    // EF follows a Code based Configration model and will look for a class that
    // derives from DbConfiguration for executing any Connection Resiliency strategies
    public class EfConfiguration : DbConfiguration
    {
        public EfConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}