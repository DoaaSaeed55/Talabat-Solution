using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Core.Services.Interfaces.ServiceInterface;
using Talabat.Repositry;
using Talabat.Repositry.Repositres;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            //services.AddScoped<IUnitOfWork,UnitOWork>();
           
            services.AddAutoMapper(typeof(MappingProfile));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                            .SelectMany(p => p.Value.Errors)
                                            .Select(e => e.ErrorMessage).ToArray();

                    var validationErrorsResponse = new ApiValidationErrorsResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorsResponse);
                };
            });

            return services;

        }




    }
}
