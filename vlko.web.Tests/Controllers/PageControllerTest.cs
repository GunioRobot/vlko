﻿using System;
using System.IO;
using System.Web.Mvc;
using Castle.ActiveRecord.Testing;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using vlko.core;
using vlko.core.Components;
using vlko.core.InversionOfControl;
using vlko.model.Action;
using vlko.model.Action.CRUDModel;
using vlko.model.Action.ViewModel;
using vlko.model.Repository;
using vlko.model.Search;
using vlko.web.Controllers;
using vlko.web.ViewModel.Page;

namespace vlko.web.Tests.Controllers
{
	[TestClass]
	public class PageControllerTest : BaseControllerTest
	{
		public static int NumberOfGeneratedItems = 100;

		/// <summary>
		/// Fills the db with data.
		/// </summary>
		protected override void FillDbWithData()
		{
			using (var tran = RepositoryFactory.StartTransaction())
			{

				RepositoryFactory.Action<IUserAction>().CreateAdmin("vlko", "vlko@zilina.net", "test");
				var admin = RepositoryFactory.Action<IUserAction>().GetByName("vlko");
				for (int i = 0; i < NumberOfGeneratedItems; i++)
				{
					RepositoryFactory.Action<IStaticTextCrud>().Create(
						new StaticTextCRUDModel
						{
							AllowComments = false,
							Creator = admin,
							Title = "StaticPage" + i,
							FriendlyUrl = "staticpage" + (i == NumberOfGeneratedItems - 1 ? string.Empty : i.ToString()),
							ChangeDate = DateTime.Now,
							PublishDate = DateTime.Now.AddDays(-i),
							Text = "Static page" + i
						});
				}
				tran.Commit();
			}
		}

		[TestMethod]
		public void Index()
		{
			// Arrange
			PageController controller = new PageController();

			TestControllerBuilder builder = new TestControllerBuilder();
			builder.InitializeController(controller);

			// Act
			ActionResult result = controller.Index(new PagedModel<StaticTextViewModel>());

			// Assert
			var model = result.AssertViewRendered().WithViewData<PagedModel<StaticTextViewModel>>();

			Assert.AreEqual(NumberOfGeneratedItems, model.Count);

			int i = 0;
			foreach (var staticText in model)
			{
				Assert.AreEqual("StaticPage" + i, staticText.Title);
				Assert.AreEqual("staticpage" + i, staticText.FriendlyUrl);
				++i;
			}
			Assert.AreEqual(PagedModel<StaticTextViewModel>.DefaultPageItems, i);
		}

		[TestMethod]
		public void ViewPage()
		{
			// Arrange
			PageController controller = new PageController();

			TestControllerBuilder builder = new TestControllerBuilder();
			builder.InitializeController(controller);
			controller.MockUser("vlko");

			// Act
			ActionResult result = controller.ViewPage("staticpage2", new PagedModel<CommentViewModel>(), "flat");

			// Assert
			var model = result.AssertViewRendered().WithViewData<PageViewModel>();

			Assert.AreEqual("StaticPage2", model.StaticText.Title);
			Assert.AreEqual("staticpage2", model.StaticText.FriendlyUrl);
			Assert.AreEqual("Static page2", model.StaticText.Text);
		}
	}
}
