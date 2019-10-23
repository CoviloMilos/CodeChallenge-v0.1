using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task AddTask(Models.Task task);
        void Delete<T>(T entity) where T: class;
        Task<IEnumerable<Models.Task>> GetTasks(bool isProduction);
        Task<Models.Task> GetTaskById(int taskId);
        Task<Models.Task> GetTaskByGuid(string taskGuid);
        Task<Case> GetSpecificCase(string taskGuid, int caseNum);
        Task<bool> SaveAll();
    }
}