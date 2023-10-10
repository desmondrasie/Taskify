using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Taskify.Models;

namespace Taskify.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItem { get; set; }
    public DbSet<TaskList> TaskList { get; set; }
    public DbSet<TaskDueDetails> TaskDueDetails { get; set; }

}