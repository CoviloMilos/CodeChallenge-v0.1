using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) {}

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Case> Cases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                        .Property(t => t.TaskId)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Task>()
                        .Property(t => t.TaskNum)
                        .HasDefaultValueSql("nextval('seq_task_tasknum')");

            modelBuilder.Entity<Case>()
                        .Property(t => t.CaseId)
                        .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Case>()
                        .Property(t => t.CaseNum)
                        .HasDefaultValueSql("nextval('seq_case_casenum')");

            modelBuilder.Entity<Case>()
                        .HasOne(c => c.Task)
                        .WithMany(t => t.Cases)
                        .HasForeignKey(c => c.TaskGuid)
                        .HasPrincipalKey(t => t.TaskGuid);
        }
        
    }
}