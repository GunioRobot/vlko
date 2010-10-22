﻿using System;
using vlko.model.Action.CRUDModel;
using vlko.model.Repository;
using vlko.model.Search;

namespace vlko.model.Action
{
	public interface ISearchAction : IAction<SearchRoot>
	{
		/// <summary>
		/// Indexes the comment.
		/// </summary>
		/// <param name="transaction">The transaction.</param>
		/// <param name="comment">The comment.</param>
		void IndexComment(ITransaction transaction, CommentCRUDModel comment);

		/// <summary>
		/// Indexes the static text.
		/// </summary>
		/// <param name="transaction">The transaction.</param>
		/// <param name="staticText">The static text.</param>
		void IndexStaticText(ITransaction transaction, StaticTextCRUDModel staticText);

		/// <summary>
		/// Deletes from index.
		/// </summary>
		/// <param name="transaction">The transaction.</param>
		/// <param name="id">The id.</param>
		void DeleteFromIndex(ITransaction transaction, Guid id);

		/// <summary>
		/// Searches for data.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="queryString">The query string.</param>
		/// <returns>Search result wrapper.</returns>
		SearchResult Search(IUnitOfWork session, string queryString);

		/// <summary>
		/// Searches for data sorted by date.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="queryString">The query string.</param>
		/// <returns>Search result wrapper.</returns>
		SearchResult SearchByDate(IUnitOfWork session, string queryString);
	}
}
