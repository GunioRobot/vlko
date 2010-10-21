﻿using vlko.model.Repository;
using vlko.model.Repository.NH;

namespace vlko.model.Tests.Repository.IOCResolved.Queries
{
    public class QueryActionAllLinq<T> : BaseCriterionQueryAction<T>, IQueryActionAll<T> where T : class
    {
        /// <summary>
        /// Executes queryAction.
        /// </summary>
        /// <returns>Query result.</returns>
        public IQueryResult<T> Execute()
        {
            return Result();
        }
    }
}
