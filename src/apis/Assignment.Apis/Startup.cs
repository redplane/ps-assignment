using Assignment.Apis.Constants;
using Assignment.Apis.Extensions;
using Assignment.Apis.Models;
using Assignment.Apis.Services.Implementations;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Cores.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Apis
{
    public class Startup
    {
        #region Properties

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration registration.
            services.AddSingleton(_configuration);

            var quest = _configuration.GetSection(ConfigurationKeys.Quest)
                .Get<Quest>(options => options.BindNonPublicProperties = true);
            services.AddSingleton(quest);

            // Services registration
            services.AddApplicationBusinessServices(_configuration);

            // Add database
            services.AddDbContext<AssignmentDbContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString(ConnectionStrings.Application)));


            services
                .AddControllers(options =>
                {
                    options.Filters.AddApplicationExceptionFilters();
                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson(options =>
                {
                    var camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ContractResolver = camelCasePropertyNamesContractResolver;
                    options.SerializerSettings.ConstructorHandling =
                        ConstructorHandling.AllowNonPublicDefaultConstructor;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use application routing.
            app.UseRouting();
            
            // Use application authorization.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        #endregion
    }
}