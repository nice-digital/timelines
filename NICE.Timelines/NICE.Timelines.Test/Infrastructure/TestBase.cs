﻿using System;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NICE.Timelines.Configuration;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.Test.Infrastructure
{
    public class TestBase
    {
        private const string DatabaseName = "TimelinesDB";
        private readonly DbContextOptions<TimelinesContext> _options;

        public TestBase()
        {
            var databaseName = DatabaseName + Guid.NewGuid();
            _options = new DbContextOptionsBuilder<TimelinesContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        protected static IHttpClientFactory CreateHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var providerFactory = new AutofacServiceProviderFactory();
            ContainerBuilder builder = providerFactory.CreateBuilder(services);

            IServiceProvider serviceProvider = providerFactory.CreateServiceProvider(builder);
            IHttpClientFactory factory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            return factory;
        }

        protected static DbContextOptions<TimelinesContext> GetContextOptions()
        {
            var databaseName = DatabaseName + Guid.NewGuid();
            return new DbContextOptionsBuilder<TimelinesContext>()
                .UseInMemoryDatabase(databaseName)
                .EnableSensitiveDataLogging()
                .Options;
        }

        protected static ClickUpConfig GetClickUpConfig()
        {
            var clickUpConfig = new ClickUpConfig();

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();
            Configuration.Bind("ClickUp", clickUpConfig);

            return clickUpConfig;
        }

        protected static TestConfig GetTestConfig()
        {
            var testConfig = new TestConfig();

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("testappsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();
            Configuration.Bind("Test", testConfig);

            return testConfig;
        }


        protected int AddTimelineTask(TimelinesContext context, string taskName, int acid, int taskTypeId, int phaseId, int orderInPhase, string clickUpSpaceId, string clickUpFolderId, string clickUpListName, string clickUpListId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate, bool keyDate, bool keyInfo, bool masterSchedule)
        {
            var task = new TimelineTask(taskName, acid, taskTypeId,  phaseId, orderInPhase, clickUpSpaceId, clickUpFolderId, clickUpListName, clickUpListId, clickUpTaskId, dueDate, actualDate, keyDate, keyInfo, masterSchedule, null);

            context.TimelineTasks.Add(task);
            context.SaveChanges();

            return task.TimelineTaskId;
        }
	}
}