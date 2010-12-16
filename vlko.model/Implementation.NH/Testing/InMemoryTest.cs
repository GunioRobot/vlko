﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using Environment = System.Environment;

namespace vlko.model.Implementation.NH.Testing
{
	/// <summary>
	/// Base class for in memory unit tests. This class does not contain any
	/// attributes specific to a testing framework.
	/// </summary>
	public abstract class InMemoryTest
	{
		protected Configuration Configuration;
		protected ISessionFactory SessionFactoryInstance;

		/// <summary>
		/// The common test setup code. To activate it in a specific test framework,
		/// it must be called from a framework-specific setup-Method.
		/// </summary>
		public virtual void SetUp()
		{
			Configuration = new Configuration()
				.SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect")
				.SetProperty("connection.driver_class", "NHibernate.Driver.SQLite20Driver")
				.SetProperty("connection.connection_string", "Data Source=:memory:")
				//.SetProperty("dialect", "NHibernate.Dialect.MsSql2008Dialect")
				//.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver")
				//.SetProperty("connection.connection_string", @"Data Source=.\SQL2008;Initial Catalog=test;Integrated Security=True;Pooling=False")
				.SetProperty("connection.provider", typeof(InMemoryConnectionProvider).AssemblyQualifiedName)
				.SetProperty("use_proxy_validator", "false")
				.SetProperty("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");

			ConfigureMapping(Configuration);

			SchemaMetadataUpdater.QuoteTableAndColumns(Configuration);

			var schema = new SchemaExport(Configuration);
			schema.Create(false, true);


			SessionFactoryInstance = Configuration.BuildSessionFactory();

		}

		/// <summary>
		/// The common test teardown code. To activate it in a specific test framework,
		/// it must be called from a framework-specific teardown-Method.
		/// </summary>
		public virtual void TearDown()
		{
			SessionFactoryInstance.Close();
			InMemoryConnectionProvider.Restart();
		}

		/// <summary>
		/// Configures the mapping.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public abstract void ConfigureMapping(Configuration configuration);

		/// <summary>
		/// Gets the mapping types.
		/// </summary>
		/// <returns>Types for mapping</returns>
		public virtual Type[] GetMappingTypes()
		{
			return new Type[] {};
		}
	}
}
