using System;
using System.Collections.Generic;
using System.Text;
using card_index_DAL.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace card_index_BLL.Infrastructure
{
    public static class BllDependencyConfigurator
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, string connectionString)
        {
            DalDependencyConfigurator.ConfigureServices(serviceCollection, connectionString);

        }
    }
}
