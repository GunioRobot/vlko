﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Testing;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vlko.core;
using vlko.core.InversionOfControl;
using vlko.core.Repository;
using vlko.model.Action;
using vlko.model.Roots;

namespace vlko.model.Tests.Model
{
	[TestClass]
	public class TwitterStatusActionsTest : InMemoryTest
	{
		private TwitterStatus[] _statuses;

		private User _user;

		[TestInitialize]
		public void Init()
		{
			IoC.InitializeWith(new WindsorContainer());
			ApplicationInit.InitializeRepositories();
			base.SetUp();
			using (var tran = new TransactionScope())
			{
				_statuses = new[]
				            	{
				            		new TwitterStatus
				            			{
				            				TwitterId = 1,
				            				Text = "Status1",
				            				User = "twit_user1",
				            				Hidden = false,
				            				CreatedDate = new DateTime(2010, 10, 10),
				            				Modified = new DateTime(2010, 10, 10),
				            				PublishDate = new DateTime(2010, 10, 10),
				            				Reply = false,
				            				RetweetUser = null,
				            				CreatedBy = null,
				            				AreCommentAllowed = false
				            			},
				            		new TwitterStatus
				            			{
				            				TwitterId = 2,
				            				Text = "Status2",
				            				User = "twit_user1",
				            				Hidden = false,
				            				CreatedDate = new DateTime(2010, 10, 11),
				            				Modified = new DateTime(2010, 10, 11),
				            				PublishDate = new DateTime(2010, 10, 11),
				            				Reply = false,
				            				RetweetUser = null,
				            				CreatedBy = null,
				            				AreCommentAllowed = false
				            			},
				            		new TwitterStatus
				            			{
				            				TwitterId = 3,
				            				Text = "Status3",
				            				User = "twit_user1",
				            				Hidden = false,
				            				CreatedDate = new DateTime(2010, 10, 11),
				            				Modified = new DateTime(2010, 10, 11),
				            				PublishDate = new DateTime(2010, 10, 11),
				            				Reply = false,
				            				RetweetUser = null,
				            				CreatedBy = null,
				            				AreCommentAllowed = false
				            			}
				            	};
				foreach (var twitterStatus in _statuses)
				{
					ActiveRecordMediator<TwitterStatus>.Create(twitterStatus);
				}
			}
		}

		[TestCleanup]
		public void Cleanup()
		{
			TearDown();
		}

		public override Type[] GetTypes()
		{
			return ApplicationInit.ListOfModelTypes();
		}

		[TestMethod]
		public void Test_create()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var item = new TwitterStatus()
				           	{
				           		TwitterId = 4,
				           		Text = "Status4",
				           		User = "twit_user2",
				           		Hidden = false,
				           		CreatedDate = new DateTime(2010, 10, 11),
				           		Modified = new DateTime(2010, 10, 11),
				           		PublishDate = new DateTime(2010, 10, 11),
				           		Reply = false,
				           		RetweetUser = null,
				           		CreatedBy = null,
				           		AreCommentAllowed = false
				           	};

				var action = RepositoryFactory.Action<ITwitterStatusAction>();

				using (var tran = RepositoryFactory.StartTransaction())
				{
					item = action.CreateStatus(item);
					tran.Commit();
				}

				var storedItem = ActiveRecordMediator<TwitterStatus>.FindByPrimaryKey(item.Id);

				Assert.AreEqual(item.Id, storedItem.Id);
				Assert.AreEqual(item.TwitterId, storedItem.TwitterId);
				Assert.AreEqual(item.Text, storedItem.Text);
				Assert.AreEqual(item.Hidden, storedItem.Hidden);
				Assert.AreEqual(item.CreatedDate, storedItem.CreatedDate);
				Assert.AreEqual(item.Modified, storedItem.Modified);
				Assert.AreEqual(item.PublishDate, storedItem.PublishDate);
				Assert.AreEqual(item.Reply, storedItem.Reply);
				Assert.AreEqual(item.RetweetUser, storedItem.RetweetUser);
			}
		}

		[TestMethod]
		public void Get_by_ids()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<ITwitterStatusAction>();
				var result = action.GetByIds(new[] {_statuses[0].Id, _statuses[1].Id})
								.ToArray();
				
				Assert.AreEqual(2, result.Length);
				Assert.AreEqual(_statuses[0].Id, result.First(item => item.Id == _statuses[0].Id).Id);
				Assert.AreEqual(_statuses[1].Id, result.First(item => item.Id == _statuses[1].Id).Id);

			}
		}

		[TestMethod]
		public void Get_by_twitter_ids()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<ITwitterStatusAction>();
				var result = action.GetByTwitterIds(new[] { _statuses[0].TwitterId, _statuses[1].TwitterId })
								.ToArray();

				Assert.AreEqual(2, result.Length);
				Assert.AreEqual(_statuses[0].Id, result.First(item => item.Id == _statuses[0].Id).Id);
				Assert.AreEqual(_statuses[1].Id, result.First(item => item.Id == _statuses[1].Id).Id);

			}
		}

		[TestMethod]
		public void Get_all()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<ITwitterStatusAction>();
				var result = action.GetAll().OrderBy(item => item.CreatedDate).ToArray();

				Assert.AreEqual(3, result.Length);
				Assert.AreEqual(_statuses[0].Id, result[0].Id);
				Assert.AreEqual(_statuses[1].Id, result[1].Id);
				Assert.AreEqual(_statuses[2].Id, result[2].Id);
			}
		}
	}

	
}
