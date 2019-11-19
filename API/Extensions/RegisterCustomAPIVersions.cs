using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    /// <summary>
    ///    Enable API versions for the application
    /// </summary>
    public static class RegisterCustomAPIVersions
    {
        /// <summary>
        ///     Add api versions middleware
        /// </summary>
        /// <param name="services">The services registered</param>
        /// <returns>The service collection pipeline of registered services.</returns>
        public static IServiceCollection AddAPIVersions(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                } );
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                } );
            return services;
        }
    }
}