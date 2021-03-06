using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using vlko.BlogModule.Commands;
using vlko.BlogModule.Roots;
using vlko.core.Repository;

namespace vlko.BlogModule.NH.Commands.QueryHelpers
{
	public class TimelineResult : IQueryResult<object>
	{
		public const int MaximumResults = 200;

		private readonly IQueryable<TimelineData> _timelineItems;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimelineResult"/> class.
		/// </summary>
		/// <param name="timelineItems">The timeline items.</param>
		public TimelineResult(IQueryable<TimelineData> timelineItems)
		{
			_timelineItems = timelineItems;
		}

		/// <summary>
		/// Orders the by (NotImplementedException).
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Exception: NotImplementedException.</returns>
		public IQueryResult<object> OrderBy<TKey>(Expression<Func<object, TKey>> query)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Orders the by descending (NotImplementedException).
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Exception: NotImplementedException.</returns>
		public IQueryResult<object> OrderByDescending<TKey>(Expression<Func<object, TKey>> query)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Counts of items in query.
		/// </summary>
		/// <returns>Counts of items in query.</returns>
		public int Count()
		{
			return _timelineItems.Count();
		}

		/// <summary>
		/// Return the result as array.
		/// </summary>
		/// <returns>All items from query.</returns>
		public object[] ToArray()
		{
			return ToPage(0, MaximumResults);
		}

		/// <summary>
		/// Return the paged result.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="itemsPerPage">The items per page.</param>
		/// <returns>All items in the specified page.</returns>
		public object[] ToPage(int startIndex, int itemsPerPage)
		{
			var rssItemIdents = new List<Guid>();
			var staticTextIds = new List<Guid>();
			var twitterStatusIds = new List<Guid>();

			// check ranges
			if (itemsPerPage > MaximumResults)
			{
				itemsPerPage = MaximumResults;
			}

			// check ranges
			var dataItems =
				_timelineItems.Skip(startIndex * itemsPerPage).Take(itemsPerPage).ToArray();

			// get ids from search results
			for (int i = 0; i < dataItems.Length; i++)
			{
				var id = dataItems[i].Id;
				var type = dataItems[i].ContentType;

				switch (type)
				{
					case ContentType.RssItem:
						rssItemIdents.Add(id);
						break;
					case ContentType.StaticText:
						staticTextIds.Add(id);
						break;
					case ContentType.TwitterStatus:
						twitterStatusIds.Add(id);
						break;
					default:
						break;
				}
			}
			// get real data from db
			var staticTexts = RepositoryFactory.Command<IStaticTextData>().GetByIds(staticTextIds).ToArray().ToDictionary(staticText => staticText.Id);
			var twitterStatuses = RepositoryFactory.Command<ITwitterStatusCommands>().GetByIds(twitterStatusIds).ToArray().ToDictionary(twitterStatus => twitterStatus.Id);
			var rssItems = RepositoryFactory.Command<IRssItemCommands>().GetByIds(rssItemIdents).ToArray().ToDictionary(rssItem => rssItem.Id);

			// compute result
			var result = new List<object>();
			foreach (var item in dataItems)
			{
				switch (item.ContentType)
				{
					case ContentType.RssItem:
						if (rssItems.ContainsKey(item.Id))
						{
							result.Add(rssItems[item.Id]);
						}
						break;
					case ContentType.StaticText:
						if (staticTexts.ContainsKey(item.Id))
						{
							result.Add(staticTexts[item.Id]);
						}
						break;
					case ContentType.TwitterStatus:
						if (twitterStatuses.ContainsKey(item.Id))
						{
							result.Add(twitterStatuses[item.Id]);
						}
						break;
					default:
						break;
				}
			}

			return result.ToArray();
		}
	}
}