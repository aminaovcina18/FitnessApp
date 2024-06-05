using FitnessApp_Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWithCriteria(Expression<Func<T, bool>> criteria, string[]? navigationProperties = null);
        T Get(int id);
        T Get(string id);
        T Get(Guid id);
        T GetWithInclude(Expression<Func<T, bool>> criteria, string[]? navigationProperties = null);
        void Insert(T entity);
        void Insert(List<T> entity);
        void Insert(IEnumerable<T> entity);
        T InsertWithReturn(T entity);
        void Update(T entity);
        void UpdateList(List<T> entity);
        void SaveChanges();
        void Delete(T entity);

        PaginationResponse<T> GetPaginatedData(GetPaginatedDataRequest<T> request);
    }
}
