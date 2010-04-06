﻿using GenericRepository;
using NHibernate.Criterion;

namespace vlko.core.ActiveRecord
{
    public class BaseCriterionQueryAction<T> : IQueryAction<T> where T : class
    {

        /// <summary>
        /// Gets or sets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        protected DetachedCriteria Criteria { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAction&lt;T&gt;"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized { get; set; }

        /// <summary>
        /// Initializes queryAction with the specified repository.
        /// </summary>
        /// <param name="initializeContext">The initialize context.</param>
        public void Initialize(InitializeContext<T> initializeContext)
        {
            Criteria = DetachedCriteria.For<T>();
            Initialized = true;
        }

        /// <summary>
        /// Get queryAction result.
        /// </summary>
        /// <returns>Query result.</returns>
        public virtual IQueryResult<T> Result()
        {
            return new CriterionQueryResult<T>(Criteria);
        }
    }
}


