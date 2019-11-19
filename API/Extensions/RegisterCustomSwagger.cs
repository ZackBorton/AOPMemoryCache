using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    /// <summary>
    ///    Enable Swagger for the application
    /// </summary>
    public static class RegisterCustomSwagger
    {
        /// <summary>
        ///     Add swagger to the application
        /// </summary>
        /// <param name="services">The services registered</param>
        /// <returns>The service collection pipeline of registered services.</returns>
        public static IServiceCollection AddSwaggerToAPI(this IServiceCollection services) {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // Versioned API Example
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API", Version = "v2" });

                SwaggerGenOptionsExtensions.DescribeAllEnumsAsStrings(c);
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"API.xml"));
            });
        }
    }
}