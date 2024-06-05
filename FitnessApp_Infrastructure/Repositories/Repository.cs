using ApplicationCore.IRepositories;
using FitnessApp_Domain.Models.Common;
using FitnessApp_Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Infrastructure.Repositories
{
    public class Repository<T> : ApplicationCore.IRepositories.IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;

        public Repository(DbContext context)
        {
            this._context = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }
        public IEnumerable<T> GetAllWithCriteria(Expression<Func<T, bool>> criteria, string[]? navigationProperties = null)
        {
            var query = _context.Set<T>().AsQueryable();
            IEnumerable<T> items;

            if (navigationProperties != null)
            {
                foreach (string navigationProperty in navigationProperties)
                    query = query.Include(navigationProperty);
            }

            items = query.Where(criteria);

            return items;
        }
        public T Get(int id)
        {
            return _entities.Find(id);
        }
        public T Get(string id)
        {
            return _entities.Find(id);
        }
        public T Get(Guid id)
        {
            return _entities.Find(id);
        }
        public T GetWithInclude(Expression<Func<T, bool>> criteria, string[]? navigationProperties = null)
        {
            var query = _context.Set<T>().AsQueryable();
            T item;
            if (navigationProperties != null)
            {
                foreach (string navigationProperty in navigationProperties)
                    query = query.Include(navigationProperty);
            }

            item = query.Where(criteria).FirstOrDefault();

            return item;
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.AddAsync(entity);
            _context.SaveChanges();
        }
        public void Insert(List<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.AddRange(entity);
            _context.SaveChanges();
        }
        public void Insert(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.AddRange(entity);
            _context.SaveChanges();
        }
        public T InsertWithReturn(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.SaveChanges();
        }

        public void UpdateList(List<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public PaginationResponse<T> GetPaginatedData(GetPaginatedDataRequest<T> request)
        {
            var query = _context.Set<T>().AsQueryable();
            List<T> list;
            if (request.NavigationProperties != null)
            {
                foreach (string navigationProperty in request.NavigationProperties)
                    query = query.Include(navigationProperty);
            }

            var dbContext = request.Criteria != null ? query.Where(request.Criteria) : query;
            var TotalItems = dbContext.Count();
            list = request.OrderByDirection == OrderByDirectionType.DESC.ToString()
                ? dbContext
                    .OrderByDescending(el => EF.Property<object>(el, request.OrderBy))
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList()
                : dbContext
                    .OrderBy(el => EF.Property<object>(el, request.OrderBy))
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

            return new PaginationResponse<T>
            {
                Page = request.Page - 1,
                Pages = TotalItems / request.PageSize,
                Total = TotalItems,
                Data = list
            }; ;
        }
    }
}
