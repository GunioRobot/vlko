﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace vlko.model.Repository
{
	public class EmptyQueryResult<T> : IQueryResult<T>
	{
		/// <summary>
		/// Orders by the query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Ordered IQueryAction.</returns>
		public IQueryResult<T> OrderBy(Expression<Func<T, object>> query)
		{
			return this;
		}

		/// <summary>
		/// Orders by descending the query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Ordered IQueryAction.</returns>
		public IQueryResult<T> OrderByDescending(Expression<Func<T, object>> query)
		{
			return this;
		}

		/// <summary>
		/// Counts of items in query.
		/// </summary>
		/// <returns>Counts of items in query.</returns>
		public int Count()
		{
			return 0;
		}

		/// <summary>
		/// Return the result as array.
		/// </summary>
		/// <returns>All items from query.</returns>
		public T[] ToArray()
		{
			return new T[] {};
		}

		/// <summary>
		/// Return the paged result.
		/// </summary>
		/// <param name="startIndex">The start index (zero based).</param>
		/// <param name="itemsPerPage">The items per page.</param>
		/// <returns>All items in the specified page.</returns>
		public T[] ToPage(int startIndex, int itemsPerPage)
		{
			return new T[] { };
		}
	}
}
