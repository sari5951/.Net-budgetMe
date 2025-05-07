using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IDBService
    {
        MyMoneyAContext entities { get; }

        int Save();
        void DisposeInternal();
    }

    public class DBService : IDBService
    {
        private ConcurrentDictionary<int, MyMoneyAContext> threads;
        private IDbContextFactory<MyMoneyAContext> dbContextFactory;
        private IConfiguration configuration;

        public DBService(IDbContextFactory<MyMoneyAContext> dbContextFactory, IConfiguration configuration)
        {
            threads = new ConcurrentDictionary<int, MyMoneyAContext>();
            this.dbContextFactory = dbContextFactory;
            this.configuration = configuration;
        }

        public MyMoneyAContext entities
        {
            get
            {
                return threads.GetOrAdd(Thread.CurrentThread.ManagedThreadId, x =>
                {
                    var ef = dbContextFactory.CreateDbContext();
                    return ef;
                });
            }
        }

        public int Save()
        {
            return entities.SaveChanges();
        }

        public void DisposeInternal()
        {
            if (threads.TryRemove(Thread.CurrentThread.ManagedThreadId, out MyMoneyAContext entities1))
            {
                entities1.Dispose();
            }
        }
    }
}
