﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using vlko.BlogModule.Commands.CRUDModel;
using vlko.BlogModule.Commands.ViewModel;
using vlko.BlogModule.Roots;
using vlko.core.Repository;

namespace vlko.BlogModule.Commands
{
	[InheritedExport]
	public interface IRssItemCommands : ICommandGroup<RssItem>
	{

		/// <summary>
		/// Gets by the feed ids.
		/// </summary>
		/// <param name="feedIds">The feed ids.</param>
		/// <returns>Feed items with matching id.</returns>
		RssItemCRUDModel[] GetByFeedIds(IEnumerable<string> feedIds);

		/// <summary>
		/// Gets the by ids.
		/// </summary>
		/// <param name="feedIds">The feed ids.</param>
		/// <returns>Only Feed items matching specified ids.</returns>
		IQueryResult<RssItemViewModel> GetByIds(IEnumerable<string> feedIds);

		/// <summary>
		/// Gets the by ids.
		/// </summary>
		/// <param name="feedIds">The feed ids.</param>
		/// <returns>Only Feed items matching specified ids.</returns>
		IQueryResult<RssItemViewModelWithId> GetByIds(IEnumerable<Guid> feedIds);

		/// <summary>
		/// Saves the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Saved item.</returns>
		RssItemCRUDModel Save(RssItemCRUDModel item);

		/// <summary>
		/// Deletes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		void Delete(RssItemCRUDModel item);

		/// <summary>
		/// Gets all.
		/// </summary>
		/// <returns>Query result with all rss feeds.</returns>
		IQueryResult<RssItemViewModel> GetAll();
	}
}
