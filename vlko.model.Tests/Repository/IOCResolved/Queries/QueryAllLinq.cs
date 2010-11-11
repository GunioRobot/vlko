﻿using vlko.core.Repository;
using vlko.model.Implementation.NH.Repository;

namespace vlko.model.Tests.Repository.IOCResolved.Queries
{
    public class QueryActionAllCriterion<T> : BaseLinqQueryAction<T>, IQueryActionAll<T> where T : class
    {
        /// <summary>
        /// Executes queryAction.
        /// </summary>
        /// <returns>Query result.</returns>
        public IQueryResult<T> Execute()
        {
        	return Result(Queryable);
        }
    }
}
