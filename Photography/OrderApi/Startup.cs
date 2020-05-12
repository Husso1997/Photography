using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;

namespace OrderApi
{
    public class Startup
    {
        /// <summary>
        /// Connection string for message broker - for DPI
        /// </summary>
        private readonly string cloudAMQPConnectionString = "host=roedeer.rmq.cloudamqp.com;virtualHost=baeynauo;username=baeynauo;password=7mYHJ1effqAkcc1HFZGC4wYZK6F_Iflf"
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IConverter, Converter>();
            services.AddTransient<IDbSeeding, DbSeeding>();

            services.AddSingleton<IMessagePublisher>(new MessagePublisher(cloudAMQPConnectionString))

            // In-memory database for the products:
            services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDatabase"));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // seed the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<OrderApiContext>();
                var dbSeeder = services.GetService<IDbSeeding>();
                dbSeeder.SeedWithData(context);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
