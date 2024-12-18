﻿using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Настройка для возможности работать приложению между доменами
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                                 builder.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader());
            });

        /// <summary>
        /// Конфигурирование IIS сервера
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services)
            => services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

            /// <summary>
            /// Вызов форматера для получения ответа данных в виде CSV
            /// </summary>
            /// <param name="builder"></param>
            /// <returns></returns>
            public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
                builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
    }
}
