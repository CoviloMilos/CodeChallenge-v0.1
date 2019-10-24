using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TaskManagementAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private DataContext _context;

        public TaskRepository(DataContext context) 
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task AddTask(Models.Task task)
        {
            await _context.AddAsync(task);

            foreach (var item in task.Cases)
            {
                await _context.AddAsync(item);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                _context.Remove(entity);
            }
            catch (System.Exception)
            {
                throw new Exception($"Task delete failed.");  
            }
        }

        public async System.Threading.Tasks.Task<Case> GetSpecificCase(string taskGuid, int caseNum)
        {
            try
            {
                return await (from cases in _context.Cases
                              where cases.TaskGuid == Guid.Parse(taskGuid) && cases.CaseNum == caseNum
                              select cases).SingleOrDefaultAsync();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task<Models.Task> GetTaskByGuid(string taskGuid)
        {
            return await _context.Tasks.Include(t => t.Cases).FirstOrDefaultAsync(t => t.TaskGuid == Guid.Parse(taskGuid));
        }

        public async System.Threading.Tasks.Task<Models.Task> GetTaskById(int taskId)
        {
            return await _context.Tasks.Include(t => t.Cases).FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasks(bool isProduction)
        {
           return await _context.Tasks
                            .Include(t => t.Cases)
                            .OrderBy(t => t.TaskNum)
                            .Where(t => t.IsProdcution == isProduction)
                            .ToListAsync();
        }

        public async System.Threading.Tasks.Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}