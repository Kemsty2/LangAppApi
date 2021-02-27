using AutoMapper;
using LangAppApi.Infrastructure.Filters;
using LangAppApi.Infrastructure.Mapping;
using LangAppApi.Persistence;
using LangAppApi.Service.Contract;
using LangAppApi.Service.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LangAppApi.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection serviceCollection,
             IConfiguration configuration, IConfigurationRoot configRoot)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("LangAppConnection") ?? configRoot["ConnectionStrings:LangAppConnection"]
                        , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                            .CharSetBehavior(CharSetBehavior.NeverAppend)
                            .EnableRetryOnFailure())
                    .EnableDetailedErrors()
            );
        }

        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new LangProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAccountService, AccountService>();
        }

        public static void AddSwaggerOpenApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {
                // note: need a temporary service provider here because one has not been created yet
                var provider = serviceCollection.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                // add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "RequestOcm Api",
                        Version = description.GroupName,
                        Contact = new OpenApiContact()
                        {
                            Email = "steeve.kemgne@orange.com",
                            Name = "Steeve Kemgne",
                            Url = new Uri("https://devopssvr.adcm.orangecm/Digitalisation")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                }

                setupAction.SchemaFilter<SwaggerIgnoreFilter>();
                setupAction.OperationFilter<RemoveVersionParameterFilter>();
                setupAction.DocumentFilter<ReplaceVersionWithExactValuePathFilter>();
                setupAction.EnableAnnotations();

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });

                var xmlCommentsFile = $"LangAppApi.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            serviceCollection.AddSwaggerGenNewtonsoftSupport();
        }

        public static void AddController(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                options.SerializerSettings.Converters.Add(new StringEnumConverter());

            }).AddDataAnnotationsLocalization();
        }

        public static void AddVersion(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            serviceCollection.AddVersionedApiExplorer();
        }

        public static void AddContextService(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }

        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }
    }
}