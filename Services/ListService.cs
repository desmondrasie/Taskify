using Taskify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskify.Data;
using System.Linq;


namespace Taskify.Services
{
    public class ListService : IListService
    {
        private readonly MyDbContext _context;
        public ListService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TaskList>> GetAllLists()
        {
            return await _context.TaskList
                                 .Include(tl => tl.Tasks)
                                 .ToListAsync();
        }

        public async Task AddList(TaskList list)
        {
            _context.TaskList.Add(list);
            await _context.SaveChangesAsync();
        }
        
        public async Task<TaskList> GetListById(int id)
        {
            return await _context.TaskList.FindAsync(id) ?? throw new InvalidOperationException($"Entity with id {id} not found");
        }

        public async Task DeleteList(TaskList list)
        {
            _context.TaskList.Remove(list);
            await _context.SaveChangesAsync();
        }
        public async Task EditListName(TaskList list)
        {
            _context.Entry(list).State = EntityState.Modified; // Mark the task as modified.
            await _context.SaveChangesAsync(); // Save changes.
        }
    }
}
