using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Core.Specifications
{
    public abstract class BaseSpecification<T>(Expression<Func<T, bool>>? criteria = null) : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public bool IsDistinct { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; } = [];

        public List<string> IncludeStrings { get; } = [];

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
            => OrderByDescending = orderByDescExpression;

        protected void AddDistinct() => IsDistinct = true;

        protected void AddPagination(int pageIndex, int pageSize)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
            IsPagingEnabled = true;
        }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (criteria is not null)
                query = query.Where(criteria);
            return query;
        }

        protected void AddInclude(params Expression<Func<T, object>>[] includeExpressions) => Includes.AddRange(includeExpressions);

        protected void AddInclude(params string[] includeStrings) => IncludeStrings.AddRange(includeStrings);

    }

    public abstract class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria = null)
        : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        public Expression<Func<T, TResult>>? Select { get; private set; }

        protected void AddSelect(Expression<Func<T, TResult>>? selectExpression) => Select = selectExpression;


    }
}
