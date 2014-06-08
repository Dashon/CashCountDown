using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace CashCountDown.Interfaces
{
   
    public interface IDatabaseInitializer<in TContext> where TContext :
              IDbContext
    {
        void InitializeDatabase(TContext context);
    }

    public interface IDbContext
    {

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        DbEntityEntry Entry(object entity);
        void Dispose();
    }

}