using BetFeed.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BetFeed.Infrastructure.Repository
{
    public class EfRepository<T> : IRepository<T>
        where T : class, IBaseModel
    {
        private BetFeedContext dataContext;
        private readonly IDbSet<T> dbSet;

        public EfRepository(BetFeedContext context)
        {
            this.dataContext = context;
            this.dbSet = this.dataContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            this.dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            this.dbSet.Attach(entity);
            var entityInContext = this.dataContext.Entry(entity);
            entityInContext.State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = this.dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                this.dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return this.dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            var all = this.dbSet.ToList();

            return all;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return this.dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return this.dbSet.Where(where).FirstOrDefault<T>();
        }

        public void SaveChanges()
        {
            this.dataContext.SaveChanges();
        }
    }
}