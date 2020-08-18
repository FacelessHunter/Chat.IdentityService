using Identity.Persistence.Contexts;
using Identity.Persistence.Seed;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Persistence.Extensions
{
    public static class MiggrationsExtensions
    {
        public static void Migrate(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            }
        }

        public static void SeedTestData(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                DataSeed.EnsureSeedData(scope.ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        public static void SeedClients(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                DataSeed.SeedClients(scope.ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
