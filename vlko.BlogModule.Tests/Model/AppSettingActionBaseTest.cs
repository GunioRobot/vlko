﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using vlko.core.Commands;
using vlko.core.Commands.Model;
using vlko.core.Repository;
using vlko.core.Roots;
using vlko.core.Testing;

namespace vlko.BlogModule.Tests.Model
{
	public abstract class AppSettingActionBaseTest : LocalTest
	{
		private AppSetting _setting1;
		private AppSetting _setting2;
		private AppSetting _emptySetting;


		public AppSettingActionBaseTest(ITestProvider testProvider) : base(testProvider)
		{
		}

		public virtual void Init()
		{
			TestProvider.SetUp();

			using (var tran = RepositoryFactory.StartTransaction())
			{
				_setting1 = new AppSetting
				            	{
				            		Id = "setting1",
									Value = "val1"
				            	};
				_setting2 = new AppSetting
				{
					Id = "setting2",
					Value = "val2"
				};
				_emptySetting = new AppSetting
				                	{
				                		Id = "empty_setting",
				                		Value = null
				                	};

				TestProvider.Create(_setting1);
				TestProvider.Create(_setting2);
				TestProvider.Create(_emptySetting);
				tran.Commit();
				var items = TestProvider.FindAll<AppSetting>();
			}
		}

		public virtual void Cleanup()
		{
			TestProvider.TearDown();
		}

		public virtual void Test_get()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Command<IAppSettingCommands>();

				var item = action.Get(_setting1.Id);

				Assert.AreEqual(_setting1.Id, item.Name);
				Assert.AreEqual(_setting1.Value, item.Value);

				item = action.Get(_setting2.Id);

				Assert.AreEqual(_setting2.Id, item.Name);
				Assert.AreEqual(_setting2.Value, item.Value);

				var empty = action.Get(_emptySetting.Id);

				Assert.AreEqual(_emptySetting.Id, empty.Name);
				Assert.AreEqual(null, empty.Value);

				var notExisted = action.Get("not_existed");

				Assert.AreEqual(null, notExisted);
			}
		}

		public virtual void Test_save()
		{
			using (RepositoryFactory.StartUnitOfWork())
			{
				var action = RepositoryFactory.Command<IAppSettingCommands>();

				var item = action.Get(_setting1.Id);
				item.Value = "changed_value";
				// try to update value
				using (var tran = RepositoryFactory.StartTransaction())
				{
					action.Save(item);
					tran.Commit();
				}

				var dbItem = action.Get(_setting1.Id);

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
