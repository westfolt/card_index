﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using card_index_BLL.Interfaces;
using card_index_BLL.Services;
using card_index_DAL.Data;
using card_index_DAL.Entities;
using card_index_DAL.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace card_index_BLL.Infrastructure
{
    public static class BllDependencyConfigurator
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, string connectionString)
        {
            DalDependencyConfigurator.ConfigureServices(serviceCollection, connectionString);

            serviceCollection.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<CardIndexDbContext>();
            serviceCollection.AddTransient<IAuthorService, AuthorService>();
            serviceCollection.AddTransient<ICardService, CardService>();
            serviceCollection.AddTransient<IGenreService, GenreService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();
            serviceCollection.AddAutoMapper(configExpression =>
            {
                configExpression.AddProfile(new CardIndexMapperProfile());
            });
        }
    }
}
