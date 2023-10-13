using ThoughtHarbour.Models;

namespace ThoughtHarbour.Services;

public interface IListService
{
    Task<IEnumerable<TaskList>> GetAllLists();
    Task AddList(TaskList list);
    Task<TaskList> GetListById(int id);
    Task DeleteList(TaskList list);
    Task EditListName(TaskList list);
}
