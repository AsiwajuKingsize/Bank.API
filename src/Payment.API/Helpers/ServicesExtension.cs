using Microsoft.EntityFrameworkCore;
using Payment.API.ServiceInterfaces;

namespace Payment.API.Helpers
{
    public static class ServicesExtension
    {
        /// <summary>
        /// Iservice collection extensions
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomServices (this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
                            options => options.UseInMemoryDatabase("CoreBankingDB"));
            services.AddTransient(typeof(IServiceRepository<>), typeof(ServiceRepository<>));
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IAccountTransactionServices, AccountTransactionServices>();
            return services;
        }    
    }
}
