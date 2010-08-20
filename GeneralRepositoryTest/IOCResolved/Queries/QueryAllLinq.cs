﻿using vlko.core.ActiveRecord;

namespace GeneralRepositoryTest.IOCResolved.Queries
{
    public class QueryActionAllCriterion<T> : BaseLinqQueryAction<T>, IQueryActionAll<T> where T : class
    {
        /// <summary>
        /// Executes queryAction.
        /// </summary>
        /// <returns>Query result.</returns>
        public GenericRepository.IQueryResult<T> Execute()
        {
        	return Result(Queryable);
        }
    }
}
