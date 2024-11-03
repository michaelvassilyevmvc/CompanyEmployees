using System.Runtime.CompilerServices;

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

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });
    }
}
