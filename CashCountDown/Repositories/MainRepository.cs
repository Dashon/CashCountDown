using CashCountDown.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CashCountDown.Repositories
{
    public class MainRepository<TEntity> : IMainRepository<TEntity> where TEntity : class, IEntity
    {
        private IDbContext db;

        public MainRepository(IDbContext context)
        {
            db = context;
        }
        public MainRepository()
        {
            db = new CashCountDownContext();
        }

        private IDbSet<TEntity> DbSet
        {
            get
            {
                return db.Set<TEntity>();
            }
        }
        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public void Deactivate(int id)
        {
            //Todo: uncomment after id name change
            //  DbSet.Find(id).Active = false;

            db.SaveChanges();
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            db.SaveChanges();
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
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }




    }
}