using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CashCountDown.Models;
using System.Linq.Expressions;
using System.Data.Entity;

namespace CashCountDown.Interfaces
{
    public interface IMainRepository<TEntity> : IDisposable where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();
        void Deactivate(int id);
        void Add(TEntity entity);
       // Boolean NewSeach(String Term);
    } 

   

    public interface IEntity
    {
        //TODO: alter all models to have simple id like below remove "?"
       //int id { get; set; }
       bool Active { get; set; }
    } 
}