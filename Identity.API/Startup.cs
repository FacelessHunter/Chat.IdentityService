using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Identity.Domain.Entities;
using Identity.Persistence.Contexts;
using IdentityServer4.EntityFramework.DbContexts;
using Identity.Persistence.Seed;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using Identity.Infrastructure.Mapping;
using System.IO;
using Identity.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.Services;
using Identity.Infrastructure.Implementation;
using Common.Implementations.Extensions;
using System.Reflection;
using Common.Domain.Options;

namespace Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            SwaggerOptions = new SwaggerOptions();

            OAuthOptions = new OAuthOptions();

            Configuration.GetSection(nameof(SwaggerOptions)).Bind(SwaggerOptions);

            Configuration.GetSection(nameof(OAuthOptions)).Bind(OAuthOptions);
        }

        public IConfiguration Configuration { get; }

        public SwaggerOptions SwaggerOptions { get; }

        public OAuthOptions OAuthOptions { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var asdf = new MappingProfile(); // For load 
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString));

            services.ConfigureOptions(Configuration);

            services.AddControllers();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddSwagger(SwaggerOptions, OAuthOptions, xmlPath);

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<User>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly("Identity.Persistence"));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString,
                        sql => sql.MigrationsAssembly("Identity.Persistence"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = OAuthOptions.AuthServer;

                // name of the API resource
                options.Audience = OAuthOptions.ApiResourse;

                options.RequireHttpsMetadata = false;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.ConfigureOptions(Configuration);

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.SeedTestData();
            }

            app.SeedClients();

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseCors(option => option
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.Migrate();

            app.ConfigureSwagger(SwaggerOptions, OAuthOptions);

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
