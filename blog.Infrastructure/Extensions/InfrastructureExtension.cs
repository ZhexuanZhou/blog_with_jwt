using System;
using blog.Core.Interfaces;
using blog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace blog.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructureExtension(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
