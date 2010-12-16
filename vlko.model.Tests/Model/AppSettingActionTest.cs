﻿using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vlko.core.Action;
using vlko.core.Action.Model;
using vlko.core.InversionOfControl;
using vlko.core.Repository;
using vlko.model.Implementation.NH.Repository;
using vlko.model.Implementation.NH.Testing;
using vlko.model.Roots;

namespace vlko.model.Tests.Model
{
	[TestClass]
	public class AppSettingActionTest : InMemoryTest
	{
		private AppSetting _setting1;
		private AppSetting _setting2;
		private AppSetting _emptySetting;
		[TestInitialize]
		public void Init()
		{
			IoC.InitializeWith(new WindsorContainer());
			ApplicationInit.InitializeRepositories();
			base.SetUp();
			DBInit.RegisterSessionFactory(SessionFactoryInstance);
			using (var tran = RepositoryFactory.StartTransaction())
			{
				_setting1 = new AppSetting
				            	{
				            		Name = "setting1",
									Value = "val1"
				            	};
				_setting2 = new AppSetting
				{
					Name = "setting2",
					Value = "val2"
				};
				_emptySetting = new AppSetting
				                	{
				                		Name = "empty_setting",
				                		Value = null
				                	};

				SessionFactory<AppSetting>.Create(_setting1);
				SessionFactory<AppSetting>.Create(_setting2);
				SessionFactory<AppSetting>.Create(_emptySetting);
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
		public void Test_get()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<IAppSettingAction>();

				var item = action.Get(_setting1.Name);

				Assert.AreEqual(_setting1.Name, item.Name);
				Assert.AreEqual(_setting1.Value, item.Value);

				item = action.Get(_setting2.Name);

				Assert.AreEqual(_setting2.Name, item.Name);
				Assert.AreEqual(_setting2.Value, item.Value);

				var empty = action.Get(_emptySetting.Name);

				Assert.AreEqual(_emptySetting.Name, empty.Name);
				Assert.AreEqual(null, empty.Value);

				var notExisted = action.Get("not_existed");

				Assert.AreEqual(null, notExisted);
			}
		}

		[TestMethod]
		public void Test_save()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Action<IAppSettingAction>();

				var item = action.Get(_setting1.Name);
				item.Value = "changed_value";
				// try to update value
				using (var tran = RepositoryFactory.StartTransaction())
				{
					action.Save(item);
					tran.Commit();
				}

				var dbItem = action.Get(_setting1.Name);

				Assert.AreEqual(item.Name, dbItem.Name);
				Assert.AreEqual(item.Value, dbItem.Value);

				var newItem = new AppSettingModel
				              	{
				              		Name = "new_Value",
				              		Value = "changed_value"
				              	};
				// try to update value
				using (var tran = RepositoryFactory.StartTransaction())
				{
					action.Save(newItem);
					tran.Commit();
				}

				dbItem = action.Get(newItem.Name);

				Assert.AreEqual(newItem.Name, dbItem.Name);
				Assert.AreEqual(newItem.Value, dbItem.Value);
			}
		}
	}
}
