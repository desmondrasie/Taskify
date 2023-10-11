using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Taskify.Models;

namespace Taskify.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItem { get; set; }
    public DbSet<TaskList> TaskList { get; set; }
    public DbSet<DueDetails> TaskDueDetails { get; set; }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<TaskItem>()
    //        .HasOne(ti => ti.DueDetails)
    //        .WithOne(dd => dd.TaskItem)
    //        .HasForeignKey<DueDetails>(dd => dd.TaskItemId)
    //        .IsRequired(false); // Not mandatory to have DueDetails
    //}
}