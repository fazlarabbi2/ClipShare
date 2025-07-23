﻿using ClipShare.Core.IRepo;
using ClipShare.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClipShare.DataAccess.Repo
{
    public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
    {
        private Context _context;
        internal DbSet<T> _contextSet;

        public BaseRepo(Context context)
        {
            _context = context;
            _contextSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Update(T source, T destination)
        {
            _contextSet.Entry(source).CurrentValues.SetValues(destination);
        }

        public void Remove(T entity)
        {
            _contextSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _contextSet.RemoveRange(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _contextSet;
            query = query.Where(criteria);

            return await query.AnyAsync();
        }

        public async Task<T> GetByIdAsync(int id, string includeProperties = null)
        {
            IQueryable<T> query = _contextSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = GetQueryWithIncludedProperties(query, includeProperties);
            }

            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> criteria, string includeProperties = null)
        {
            IQueryable<T> query = _contextSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = GetQueryWithIncludedProperties(query, includeProperties);
            }

            return await query.Where(criteria).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria = null, string includeProperties = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null)
        {
            IQueryable<T> query = _contextSet;

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = GetQueryWithIncludedProperties(query, includeProperties);
            }

            if (orderedBy != null)
            {
                return await orderedBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<int> countAsync(Expression<Func<T, bool>> criteria = null)
        {
            IQueryable<T> query = _contextSet;

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            return await query?.CountAsync();
        }


        // "Video, Subscriber"

        #region Static Methods
        public static IQueryable<T> GetQueryWithIncludedProperties(IQueryable<T> query, string includeProperties)
        {
            var props = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var prop in props)
            {
                query = query.Include(prop);
            }

            return query;
        }
        #endregion

    }
}
