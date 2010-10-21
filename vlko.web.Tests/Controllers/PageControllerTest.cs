﻿using System;
using System.IO;
using System.Web.Mvc;
using Castle.ActiveRecord.Testing;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vlko.core;
using vlko.core.Components;
using vlko.core.InversionOfControl;
using vlko.model.Action;
using vlko.model.ActionModel;
using vlko.model.Repository;
using vlko.model.Search;
using vlko.model.ViewModel;
using vlko.web.Controllers;
using vlko.web.ViewModel.Page;

namespace vlko.web.Tests.Controllers
{
	[TestClass]
	public class PageControllerTest : InMemoryTest
	{
		public static int NumberOfGeneratedItems = 100;
		private IUnitOfWork session;
		[TestInitialize]
		public void Init()
		{
			IoC.InitializeWith(new WindsorContainer());
			ApplicationInit.InitializeRepositories();
			ApplicationInit.InitializeServices();
			IoC.Resolve<ISearchProvider>().Initialize(Directory.GetCurrentDirectory());
			base.SetUp();
			using (var tran = RepositoryFactory.StartTransaction())
			{

				RepositoryFactory.Action<IUserAction>().CreateAdmin("vlko", "vlko@zilina.net", "test");
				var admin = RepositoryFactory.Action<IUserAction>().GetByName("vlko");
				for (int i = 0; i < NumberOfGeneratedItems; i++)
				{
					RepositoryFactory.Action<IStaticTextCrud>().Create(
						new StaticTextActionModel
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
			session = RepositoryFactory.StartUnitOfWork();
		}

		[TestCleanup]
		public void Cleanup()
		{
			session.Dispose();
			TearDown();
		}

		public override Type[] GetTypes()
		{
			return ApplicationInit.ListOfModelTypes();
		}

		[TestMethod]
		public void Index()
		{
			// Arrange
			PageController controller = new PageController();
			controller.MockRequest();
			// Act
			ViewResult result = controller.Index(new PagedModel<StaticTextViewModel>()) as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));

			ViewResult viewResult = (ViewResult)result;
			var model = (PagedModel<StaticTextViewModel>)viewResult.ViewData.Model;

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
			controller.MockRequest();
			controller.MockUser("vlko");
			// Act
			ViewResult result = controller.ViewPage("staticpage2", new PagedModel<CommentViewModel>(), "flat") as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));

			ViewResult viewResult = (ViewResult)result;
			var model = (PageViewModel)viewResult.ViewData.Model;

			Assert.AreEqual("StaticPage2", model.StaticText.Title);
			Assert.AreEqual("staticpage2", model.StaticText.FriendlyUrl);
			Assert.AreEqual("Static page2", model.StaticText.Text);
		}
	}
}
