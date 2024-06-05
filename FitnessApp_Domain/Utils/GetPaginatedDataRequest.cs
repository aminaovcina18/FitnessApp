using FitnessApp_Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Utils
{
    public class GetPaginatedDataRequest<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public string[]? NavigationProperties { get; set; }
        public string OrderByDirection { get; set; } = OrderByDirectionType.DESC.ToString();

        public GetPaginatedDataRequest()
        {
            this.Page = 1;
            this.PageSize = 10;
            this.OrderBy = "Id";
            this.OrderByDirection = OrderByDirectionType.DESC.ToString();
            this.Criteria = null;
            this.NavigationProperties = null;
        }
        public GetPaginatedDataRequest(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.OrderBy = "Id";
            this.OrderByDirection = OrderByDirectionType.DESC.ToString();
            this.Criteria = null;
            this.NavigationProperties = null;
        }
        public GetPaginatedDataRequest(int page, int pageSize, Expression<Func<T, bool>> criteria)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.OrderBy = "Id";
            this.OrderByDirection = OrderByDirectionType.DESC.ToString();
            this.Criteria = criteria;
            this.NavigationProperties = null;
        }
        public GetPaginatedDataRequest(int page, int pageSize, Expression<Func<T, bool>> criteria, string[] navigationProperties)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.OrderBy = "Id";
            this.OrderByDirection = OrderByDirectionType.DESC.ToString();
            this.Criteria = criteria;
            this.NavigationProperties = navigationProperties;
        }

    }
}
