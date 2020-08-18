using Identity.Domain.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Persistence.Seed
{
    public class DataSeed
    {

        public static async Task SeedClients(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<ConfigurationDbContext>();
            if (!await context.Clients.AnyAsync())
            {
                foreach (var client in Config.GetClients())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!await context.IdentityResources.AnyAsync())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!await context.ApiResources.AnyAsync())
            {
                foreach (var resource in Config.GetApis())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

        }
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            {
                var userMgr = provider.GetRequiredService<UserManager<User>>();

                if (!await userMgr.Users.AnyAsync())
                {
                    var alice = new User
                    {
                        UserName = "Alice_Popova",
                        Email = "alice@alice.com",
                        PhoneNumber = "0960001122"
                    };

                    await userMgr.CreateAsync(alice, "Pass123$");



                    var vasy = new User
                    {
                        UserName = "Vasy_Pupkin",
                        Email = "vasy@pupkin.com",
                        PhoneNumber = "0960001123"
                    };


                    await userMgr.CreateAsync(vasy, "Pass123$");

                    var adolf = new User
                    {
                        UserName = "Adolf",
                        Email = "Adolf@gmail.com",
                        PhoneNumber = "0960001124"
                    };


                    await userMgr.CreateAsync(adolf, "Fuhrer1488$$");

                    var benito = new User
                    {
                        UserName = "Benito",
                        Email = "Benito@gmail.com",
                        PhoneNumber = "0960001125"
                    };


                    await userMgr.CreateAsync(benito, "Pass123$");

                    var franklin = new User
                    {
                        UserName = "Franklin",
                        Email = "Delano@gmail.com",
                        PhoneNumber = "0960001126"
                    };


                    await userMgr.CreateAsync(franklin, "Pass123$");

                    var stalin = new User
                    {
                        UserName = "Stalin",
                        Email = "Stalin@gmail.com",
                        PhoneNumber = "0960001127"
                    };


                    await userMgr.CreateAsync(stalin, "Pass123$");

                    var isoroku = new User
                    {
                        UserName = "Isoroku",
                        Email = "Isoroku@gmail.com",
                        PhoneNumber = "0960001128"
                    };


                    await userMgr.CreateAsync(isoroku, "Pass123$");

                    var douglas = new User
                    {
                        UserName = "Douglas",
                        Email = "Douglas@gmail.com",
                        PhoneNumber = "0960001129"
                    };


                    await userMgr.CreateAsync(douglas, "Pass123$");

                    var heinrich = new User
                    {
                        UserName = "Heinrich",
                        Email = "Heinrich@gmail.com",
                        PhoneNumber = "0960001130"
                    };

                    await userMgr.CreateAsync(heinrich, "Pass123$");

                    var hermann = new User
                    {
                        UserName = "Hermann",
                        Email = "Hermann@gmail.com",
                        PhoneNumber = "0960001131"
                    };

                    await userMgr.CreateAsync(hermann, "Pass123$");

                    var winston = new User
                    {
                        UserName = "Winston",
                        Email = "Winston@gmail.com",
                        PhoneNumber = "0960001132"
                    };

                    await userMgr.CreateAsync(winston, "Pass123$");

                    var saigo = new User
                    {
                        UserName = "Saigo",
                        Email = "Saigo@gmail.com",
                        PhoneNumber = "0960001133"
                    };

                    await userMgr.CreateAsync(saigo, "Pass123$");
                }
            }

        }
    }
}
