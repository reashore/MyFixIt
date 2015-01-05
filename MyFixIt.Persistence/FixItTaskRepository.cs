
using MyFixIt.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyFixIt.Persistence
{
    public class FixItTaskRepository : IFixItTaskRepository, IDisposable
    {
        private MyFixItContext _db = new MyFixItContext();
        private readonly ILogger _log;

        public FixItTaskRepository(ILogger logger)
        {
            _log = logger;
        }

        public async Task<FixItTask> FindTaskByIdAsync(int id)
        {
            FixItTask fixItTask;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                fixItTask = await _db.FixItTasks.FindAsync(id);
                
                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.FindTaskByIdAsync", timespan.Elapsed, "id={0}", id);
            }
            catch(Exception e)
            {
               _log.Error(e, "Error in FixItTaskRepository.FindTaskByIdAsync(id={0})", id);
               throw;
            }

            return fixItTask;
        }

        public async Task<List<FixItTask>> FindOpenTasksByOwnerAsync(string userName)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await _db.FixItTasks
                    .Where(t => t.Owner == userName)
                    .Where(t=>t.IsDone == false)
                    .OrderByDescending(t => t.FixItTaskId).ToListAsync();

                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByOwnerAsync", timespan.Elapsed, "username={0}", userName);

                return result;
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Error in FixItTaskRepository.FindTasksByOwnerAsync(userName={0})", userName);
                throw;
            }
        }

        public async Task<List<FixItTask>> FindTasksByCreatorAsync(string creator)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await _db.FixItTasks
                    .Where(t => t.CreatedBy == creator)
                    .OrderByDescending(t => t.FixItTaskId).ToListAsync();

                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByCreatorAsync", timespan.Elapsed, "creater={0}", creator);

                return result;
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Error in FixItTaskRepository.FindTasksByCreatorAsync(creater={0})", creator);
                throw;
            }
        }

        public async Task CreateAsync(FixItTask taskToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try {
                _db.FixItTasks.Add(taskToAdd);
                await _db.SaveChangesAsync();

                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.CreateAsync", timespan.Elapsed, "taskToAdd={0}", taskToAdd);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Error in FixItTaskRepository.CreateAsync(taskToAdd={0})", taskToAdd);
                throw;
            }
        }

        public async Task UpdateAsync(FixItTask taskToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try {
                _db.Entry(taskToSave).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.UpdateAsync", timespan.Elapsed, "taskToSave={0}", taskToSave);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Error in FixItTaskRepository.UpdateAsync(taskToSave={0})", taskToSave);
                throw;
            }
        }
        
        public async Task DeleteAsync(Int32 id)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                FixItTask fixittask = await _db.FixItTasks.FindAsync(id);
                _db.FixItTasks.Remove(fixittask);
                await _db.SaveChangesAsync();

                timespan.Stop();
                _log.TraceApi("SQL Database", "FixItTaskRepository.DeleteAsync", timespan.Elapsed, "id={0}", id);

            }
            catch (Exception exception)
            {
                _log.Error(exception, "Error in FixItTaskRepository.DeleteAsync(id={0})", id);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }
    }
}