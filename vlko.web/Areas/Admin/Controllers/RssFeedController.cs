﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vlko.BlogModule.Commands;
using vlko.BlogModule.Commands.CRUDModel;
using vlko.BlogModule.Commands.ViewModel;
using vlko.core.Base;
using vlko.core.Base.Scheduler;
using vlko.core.Components;
using vlko.core.InversionOfControl;
using vlko.core.Repository;
using vlko.core.ValidationAtribute;
using vlko.BlogModule;
using vlko.BlogModule.Base.Scheduler;
using vlko.BlogModule.Search;
using vlko.web.Areas.Admin.ViewModel.RssFeed;

namespace vlko.web.Areas.Admin.Controllers
{
	[Authorize]
	[AntiXss]
	[AreaCheck("Admin")]
	public class RssFeedController : BaseController
	{
		/// <summary>
		/// URL: RssFeed/Index
		/// </summary>
		/// <returns>Action result.</returns>
		public ActionResult Index(PagedModel<RssFeedViewModel> pageModel)
		{
			pageModel.LoadData(RepositoryFactory.Command<IRssFeedCommands>().GetAll().OrderByDescending(item => item.Name));
			return ViewWithAjax(pageModel);
		}

		/// <summary>
		/// URL: RssFeed/Details
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>Action result.</returns>
		public ActionResult Details(Guid id)
		{
			var item = RepositoryFactory.Command<IRssFeedCommands>().FindByPk(id);
			return ViewWithAjax(item);
		}

		/// <summary>
		/// URL: RssFeed/Delete
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>Action result.</returns>
		public ActionResult Delete(Guid id)
		{
			return ViewWithAjax(RepositoryFactory.Command<IRssFeedCommands>().FindByPk(id));
		}

		/// <summary>
		/// URL: RssFeed/Delete
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Action result.</returns>
		[HttpPost]
		public ActionResult Delete(RssFeedCRUDModel model)
		{
			var item = RepositoryFactory.Command<IRssFeedCommands>().FindByPk(model.Id);
			using (var tran = RepositoryFactory.StartTransaction())
			{
				RepositoryFactory.Command<IRssFeedCommands>().Delete(model);
				tran.Commit();
			}
			return RedirectToActionWithAjax("Index");
		}

		/// <summary>
		/// URL: RssFeed/Edit
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>Action result.</returns>
		public ActionResult Edit(Guid id)
		{
			return ViewWithAjax(RepositoryFactory.Command<IRssFeedCommands>().FindByPk(id));
		}

		/// <summary>
		/// URL: RssFeed/Edit
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Action result.</returns>
		[HttpPost]
		public ActionResult Edit(RssFeedCRUDModel model)
		{
			if (ModelState.IsValid)
			{
				var crudOperations = RepositoryFactory.Command<IRssFeedCommands>();

				// get original item to test change permissions
				var originalItem = crudOperations.FindByPk(model.Id);

				if (originalItem != null)
				{
					using (var tran = RepositoryFactory.StartTransaction(IoC.Resolve<SearchUpdateContext>()))
					{
						crudOperations.Update(model);
						tran.Commit();
					}
					return RedirectToActionWithAjax("Index");
				}

				ModelState.AddModelError(string.Empty, ModelResources.ItemNotExistsError);
			}
			return ViewWithAjax(model);
		}

		/// <summary>
		/// URL: RssFeed/TestFeed
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Action result.</returns>
		[HttpPost]
		public ActionResult TestFeed(RssFeedCRUDModel model)
		{
			var feedItems = new List<RssItemCRUDModel>();
			try
			{
				var connectionAction = RepositoryFactory.Command<IRssFeedConnection>();
				var rssItems = connectionAction.GetFeedUrlItems(model.Url);
				foreach (var rssItem in rssItems)
				{
					var feedItem = UpdateRssFeedsTask.GetFeedItem(rssItem, model);
					if (feedItem != null)
					{
						feedItems.Add(feedItem);
					}
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.ToString());
			}
			return ViewWithAjax(feedItems.ToArray());
		}

		/// <summary>
		/// URL: RssFeed/Create
		/// </summary>
		/// <returns>Action result.</returns>
		public ActionResult Create()
		{
			return ViewWithAjax(new RssFeedCRUDModel());
		}

		/// <summary>
		/// URL: RssFeed/Create
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Action result.</returns>
		[HttpPost]
		public ActionResult Create(RssFeedCRUDModel model)
		{
			if (ModelState.IsValid)
			{
				var crudOperations = RepositoryFactory.Command<IRssFeedCommands>();

				using (var tran = RepositoryFactory.StartTransaction())
				{
					crudOperations.Create(model);
					tran.Commit();
				}
				return RedirectToActionWithAjax("Index");

			}
			return ViewWithAjax(model);
		}
	}
}
