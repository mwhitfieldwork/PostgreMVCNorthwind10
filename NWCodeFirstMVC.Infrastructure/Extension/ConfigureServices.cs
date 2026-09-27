using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Infrastructure.Services;

namespace NWCodeFirstMVC.Infrastructure.Extension
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            //sets the service as the intermediary for the interface
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
