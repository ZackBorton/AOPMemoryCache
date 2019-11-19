using System;
using API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace API
{
    /// <summary>
    ///     Called when the app host is built to setup the Inversion of Control container to register services that can be
    ///     Dependency Injected
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAPIVersions();
            services.AddSwaggerToAPI();

            //StructureMap Container
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    // Registering to allow for Interfaces to be dynamically mapped
                    _.AssemblyContainingType(typeof(Startup));
                    //List assembly's here
                    _.Assembly("API");
                    _.Assembly("Logic");
                    _.Assembly("Models");
                    _.WithDefaultConventions();
                });
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // Enforces the HTTP Strict Transport Security, which forces all communication over https
            // It also enforces the browser to disallow a user from using untrusted or invalid certificates
            app.UseHsts();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
            });
        }
    }
}