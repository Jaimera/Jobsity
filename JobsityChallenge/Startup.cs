using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using JobsityChallenge.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobsityChallenge.Hubs;
using JobsityChallenge.Setup;
using Microsoft.Extensions.Options;
using JobsityChallenge.CrossCutting.RabbitMQ;
using RabbitMQ.Client;
using JobsityChallenge.CrossCutting.CsvHelper;
using CsvHelper;
using JobsityChallenge.CrossCutting.StockExchange;

namespace JobsityChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            var rabbitMQConfig = new RabbitMQConfig();
            new ConfigureFromConfigurationOptions<RabbitMQConfig>(
                Configuration.GetSection("RabbitMQConfig"))
                    .Configure(rabbitMQConfig);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages(opts =>
            {
                opts.Conventions.AuthorizePage("/Chat");
            });

            services.AddSingleton<ICsvParserService, CsvParserService>();
            services.AddSingleton<IStockClient, StockClient>();

            IdentitySetup.ConfigureServices(services);
            //services.AddSingleton<IMessageQueue>(sp =>
            //{
            //    var connectionFactory = new ConnectionFactory()
            //    {
            //        HostName = rabbitMQConfig.HostName,
            //        Port = rabbitMQConfig.Port,
            //        UserName = rabbitMQConfig.UserName,
            //        Password = rabbitMQConfig.Password
            //    };

            //    return new QueueManager(connectionFactory);
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
