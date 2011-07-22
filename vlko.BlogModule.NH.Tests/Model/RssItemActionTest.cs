﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vlko.BlogModule.NH;
using vlko.core.InversionOfControl;
using vlko.core.NH.Repository;
using vlko.core.NH.Testing;
using vlko.core.Repository;
using vlko.BlogModule.Action;
using vlko.BlogModule.Action.CRUDModel;
using vlko.BlogModule.Roots;

namespace vlko.BlogModule.Tests.Model
{
	[TestClass]
	public class RssItemActionTest : InMemoryTest
	{
		private RssItem[] _rssItems;

		private RssFeed _feed;


		[TestInitialize]
		public void Init()
		{
			IoC.AddCatalogAssembly(Assembly.Load("vlko.Core.NH"));
			IoC.AddCatalogAssembly(Assembly.Load("vlko.BlogModule"));
			IoC.AddCatalogAssembly(Assembly.Load("vlko.BlogModule.NH"));
			base.SetUp();

			using (var tran = RepositoryFactory.StartTransaction())
			{
				_feed = new RssFeed
				        	{
				        		Name = "feed",
				        		Url = "feed_url"
				        	};
				SessionFactory<RssFeed>.Create(_feed);
				_rssItems = new[]
				            	{
				            		new RssItem()
				            			{
				            				FeedItemId = "1",
											Title = "title1",
				            				Text = "Text1",
											Description = "Description1",
				            				Author = "twit_user1",
											Url = "url1",
				            				Hidden = false,
				            				CreatedDate = new DateTime(2010, 10, 10),
				            				Modified = new DateTime(2010, 10, 10),
				            				PublishDate = new DateTime(2010, 10, 10),
				            				CreatedBy = null,
				            				AreCommentAllowed = false,
											RssFeed = _feed
				            			},
				            		new RssItem()
				            			{
				            				FeedItemId = "2",
											Title = "title2",
				            				Text = "Text2",
											Description = "Description2",
				            				Author = "twit_user2",
											Url = "url2",
				            				Hidden = false,
				            				CreatedDate = new DateTime(2010, 10, 12),
				            				Modified = new DateTime(2010, 10, 12),
				            				PublishDate = new DateTime(2010, 10, 12),
				            				CreatedBy = null,
				            				AreCommentAllowed = false,
											RssFeed = _feed
				            			},
									new RssItem()
				            			{
				            				FeedItemId = "3",
											Title = "title3",
				            				Text = "Text3",
											Description = "Description3",
				            				Author = "twit_user3",
											Url = "url3",
				            				Hidden = true,
				            				CreatedDate = new DateTime(2010, 10, 13),
				            				Modified = new DateTime(2010, 10, 13),
				            				PublishDate = new DateTime(2010, 10, 13),
				            				CreatedBy = null,
				            				AreCommentAllowed = false,
											RssFeed = _feed
				            			}
				            	};
				foreach (var rssItem in _rssItems)
				{
					SessionFactory<RssItem>.Create(rssItem);
				}
				tran.Commit();
			}
		}

		[TestCleanup]
		public void Cleanup()
		{
			TearDown();
		}

		public override void ConfigureMapping(NHibernate.Cfg.Configuration configuration)
		{
			DBInit.InitMappings(configuration);
		}

		[TestMethod]
		public void Test_save()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var item = new RssItemCRUDModel()
				           	{
								FeedItemId = "new",
								Title = "title_new",
								Text = "Text_new",
								Description = "DescriptionNew",
								Author = "twit_usernew",
								Url = "url_new",
								FeedId = _feed.Id,
								Published = DateTime.Now
				           	};

				var action = RepositoryFactory.Action<IRssItemAction>();

				using (var tran = RepositoryFactory.StartTransaction())
				{
					item = action.Save(item);
					tran.Commit();
				}

				var storedItem = SessionFactory<RssItem>.Queryable.First(rssItem => rssItem.FeedItemId == item.FeedItemId);

				Assert.AreEqual(item.FeedItemId, storedItem.FeedItemId);
				Assert.AreEqual(item.FeedId, storedItem.RssFeed.Id);
				Assert.AreEqual(item.Title, storedItem.Title);
				Assert.AreEqual(item.Text, storedItem.Text);
				Assert.AreEqual(item.Description, storedItem.Description);
				Assert.AreEqual(item.Published, storedItem.CreatedDate);
				Assert.AreEqual(item.Published, storedItem.Modified);
				Assert.AreEqual(item.Published, storedItem.PublishDate);
				Assert.AreEqual(item.Author, storedItem.Author);
				Assert.AreEqual(item.Url, storedItem.Url);
				Assert.AreEqual(false, storedItem.Hidden);
				Assert.AreEqual(false, storedItem.AreCommentAllowed);

				item = new RssItemCRUDModel()
				{
					FeedItemId = "change",
					Title = "title_change",
					Text = "Text_change",
					Description = "Descriptionchange",
					Author = "twit_userchange",
					Url = "url_change",
					FeedId = _feed.Id,
					Published = DateTime.Now
				};

				using (var tran = RepositoryFactory.StartTransaction())
				{
					item = action.Save(item);
					tran.Commit();
				}

				storedItem = SessionFactory<RssItem>.Queryable.First(rssItem => rssItem.FeedItemId == item.FeedItemId);

				Assert.AreEqual(item.FeedItemId, storedItem.FeedItemId);
				Assert.AreEqual(item.FeedId, storedItem.RssFeed.Id);
				Assert.AreEqual(item.Title, storedItem.Title);
				Assert.AreEqual(item.Text, storedItem.Text);
				Assert.AreEqual(item.Description, storedItem.Description);
				Assert.AreEqual(item.Published, storedItem.CreatedDate);
				Assert.AreEqual(item.Published, storedItem.Modified);
				Assert.AreEqual(item.Published, storedItem.PublishDate);
				Assert.AreEqual(item.Author, storedItem.Author);
				Assert.AreEqual(item.Url, storedItem.Url);
				Assert.AreEqual(false, storedItem.Hidden);
				Assert.AreEqual(false, storedItem.AreCommentAllowed);
			}
		}

		[TestMethod]
		public void Test_delete()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var item = new RssItemCRUDModel()
				           	{
				           		FeedItemId = "new",
				           		Title = "title_new",
				           		Text = "Text_new",
				           		Description = "DescriptionNew",
				           		Author = "twit_usernew",
				           		Url = "url_new",
				           		FeedId = _feed.Id,
								Published = DateTime.Now
				           	};

				var action = RepositoryFactory.Action<IRssItemAction>();

				using (var tran = RepositoryFactory.StartTransaction())
				{
					item = action.Save(item);
					tran.Commit();
				}

				using (var tran = RepositoryFactory.StartTransaction())
				{
					action.Delete(item);
					tran.Commit();
				}

				var items = action.GetAll().ToArray();
				Assert.AreEqual(2, items.Count());
				Assert.IsFalse(items.Any(rssItem => rssItem.FeedItemId == item.FeedItemId));
			}
		}

		[TestMethod]
		public void Get_by_ids()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<IRssItemAction>();
				var result = action.GetByIds(new[] { _rssItems[0].FeedItemId, _rssItems[1].FeedItemId })
								.ToArray();
				
				Assert.AreEqual(2, result.Length);
				Assert.AreEqual(_rssItems[0].FeedItemId, result.First(item => item.FeedItemId == _rssItems[0].FeedItemId).FeedItemId);
				Assert.AreEqual(_rssItems[1].FeedItemId, result.First(item => item.FeedItemId == _rssItems[1].FeedItemId).FeedItemId);
			}
		}

		[TestMethod]
		public void Get_by_feed_ids()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<IRssItemAction>();
				var result = action.GetByFeedIds(new[] { _rssItems[0].FeedItemId, _rssItems[1].FeedItemId })
								.ToArray();

				Assert.AreEqual(2, result.Length);
				Assert.AreEqual(_rssItems[0].FeedItemId, result.First(item => item.FeedItemId == _rssItems[0].FeedItemId).FeedItemId);
				Assert.AreEqual(_rssItems[1].FeedItemId, result.First(item => item.FeedItemId == _rssItems[1].FeedItemId).FeedItemId);
			}
		}

		[TestMethod]
		public void Get_all()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<IRssItemAction>();
				var result = action.GetAll().OrderBy(item => item.Published).ToArray();

				Assert.AreEqual(2, result.Length);
				Assert.AreEqual(_rssItems[0].FeedItemId, result[0].FeedItemId);
				Assert.AreEqual(_rssItems[1].FeedItemId, result[1].FeedItemId);
			}
		}
	}

	
}
