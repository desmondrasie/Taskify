using Taskify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskify.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Taskify.Services
{
    public class TaskService : ITaskService
    {
        private readonly MyDbContext _context;

        public TaskService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks()
        {
            return await _context.TaskItem.ToListAsync();
        }
        public async Task<IEnumerable<TaskItem>> GetPendingTasks()
        {
            return await _context.TaskItem.Where(t => t.IsChecked == false).ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetPendingTasks(int taskListId)
        {
            return await _context.TaskItem.Where(t => t.IsChecked == false && t.TaskListId == taskListId).ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetCompletedTasks()
        {
            return await _context.TaskItem.Where(t => t.IsChecked == true).ToListAsync();
        }
        public async Task AddTask(TaskItem task,int id)
        {
            task.TaskListId = id;
            _context.TaskItem.Add(task);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTask(int id)
        {
            var task = await _context.TaskItem.FindAsync(id);
            if (task != null)
            {
                _context.TaskItem.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
        public async Task EditTask(TaskItem task)
        {
            _context.Entry(task).State = EntityState.Modified; // Mark the task as modified.
            await _context.SaveChangesAsync(); // Save changes.       
                
        }
        public async Task CheckTask(TaskItem task)
        {
            task.IsChecked = !task.IsChecked;           
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task EditDueDate(TaskItem task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
