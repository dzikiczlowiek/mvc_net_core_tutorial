namespace SportStore.Web
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SportStore.DataAccess.Orders;
    using SportStore.DataAccess.Products;
    using SportStore.Web.Models;

    using StackExchange.Redis;

    public class Startup
    {
        private IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {

            configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedRedisCache(
                options =>
                    {
                        options.Configuration = "localhost:6379";
                        options.InstanceName = "SampleInstance";
                    });

            services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(
                _ => ConnectionMultiplexer.Connect(configuration["Data:SportStoreProducts:ConnectionString"]));
            services.AddScoped<IDatabase>(_ => _.GetRequiredService<IConnectionMultiplexer>().GetDatabase(1));
            services.AddScoped<IProductsObjectStore, ProductsObjectStore>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrdersObjectStore, OrdersObjectStore>();
            services.AddMvc();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(
                options =>
                    {
                        options.IdleTimeout = TimeSpan.FromMinutes(10);
                        options.Cookie.HttpOnly = true;
                    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes => {
                routes.MapRoute(
                        name: null,
                        template: "{category}/Page{page:int}",
                        defaults: new { controller = "Product", action = "List" }
                    );
                    routes.MapRoute(
                        name: null,
                        template: "Page{page:int}",
                        defaults: new { controller = "Product", action = "List", page = 0 }
                    );
                    routes.MapRoute(
                        name: null,
                        template: "{category}",
                        defaults: new { controller = "Product", action = "List", page = 0 }
                    );
                    routes.MapRoute(
                        name: null,
                        template: "",
                        defaults: new { controller = "Product", action = "List", page = 0 });
                    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
