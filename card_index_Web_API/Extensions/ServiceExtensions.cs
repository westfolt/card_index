using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
using card_index_BLL.Validation;
using FluentValidation;

namespace card_index_Web_API.Extensions
{
    /// <summary>
    /// Configures services for DI
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures Swagger options
        /// </summary>
        /// <param name="services">App services collection</param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CardIndex API",
                    Description = "An ASP.NET Core Web API for managing index of text cards"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.AddSecurityDefinition(
                    "token",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Name = HeaderNames.Authorization
                    }
                );
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "token"
                                },
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            });
        }
        /// <summary>
        /// Configures CORS options
        /// </summary>
        /// <param name="services">App services collection</param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CardCorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }
        /// <summary>
        /// Configures app authentication and JWT settings
        /// </summary>
        /// <param name="services">App services collection</param>
        /// <param name="configuration">App configuration</param>
        /// <param name="tokenValidHours">Lifespan of token</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration, int tokenValidHours)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(tokenValidHours));

            var jwtSettings = configuration.GetSection("JWTSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });
        }
        /// <summary>
        /// Configures validation of dto objects
        /// </summary>
        /// <param name="services">App services collection</param>
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<GenreDto>, GenreDtoValidator>();
            services.AddScoped<IValidator<AuthorDto>, AuthorDtoValidator>();
            services.AddScoped<IValidator<RateDetailDto>, RateDetailDtoValidator>();
            services.AddScoped<IValidator<TextCardDto>, TextCardDtoValidator>();
            services.AddScoped<IValidator<UserInfoModel>, UserInfoModelValidator>();
            services.AddScoped<IValidator<UserRoleInfoModel>, UserRoleInfoModelValidator>();
            services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator>();
            services.AddScoped<IValidator<UserRegistrationModel>, UserRegistrationModelValidator>();
            services.AddScoped<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();
        }
    }
}
