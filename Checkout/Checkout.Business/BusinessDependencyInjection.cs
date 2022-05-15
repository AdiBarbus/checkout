namespace Checkout.Business
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
