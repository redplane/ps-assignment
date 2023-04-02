using System.Reflection;
using Assignment.Apis.PipelineBehaviors;
using Assignment.Apis.Services.Implementations;
using Assignment.Businesses.Services.Abstractions;
using FluentValidation;
using MediatR;

namespace Assignment.Apis.Extensions
{
    public static class ApplicationBusinessServiceExtensions
    {
        #region Methods

        public static IServiceCollection AddApplicationBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Http context accessor.
            services.AddHttpContextAccessor();
            
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IQuestService, QuestService>();

            // Fluent validator registration
            services.AddMediatR(serviceConfiguration =>
            {
                serviceConfiguration.RegisterServicesFromAssemblies(typeof(Startup).GetTypeInfo().Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(Startup).GetTypeInfo().Assembly);

            // Register command & query handlers.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }

        #endregion
    }
}
