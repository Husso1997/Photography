using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Data;
using CustomerApi.Infrastructure;
using CustomerApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomerApi
{
    public class Startup
    {

        /// <summary>
        /// Connection string for message broker - for DPI
        /// </summary>
        private readonly string cloudAMQPConnectionString = "host=roedeer.rmq.cloudamqp.com;virtualHost=baeynauo;username=baeynauo;password=7mYHJ1effqAkcc1HFZGC4wYZK6F_Iflf";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            services.AddScoped<IConverter, Converter>();
            services.AddTransient<IDbSeeding, DbSeeding>();

            // Singleton - MessagePublisher
            services.AddSingleton<IMessagePublisher>(new MessagePublisher(cloudAMQPConnectionString));

            // In-memory database for the products:
            services.AddDbContext<CustomerApiContext>(opt => opt.UseInMemoryDatabase("CustomersDatabase"));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Starting the Subscription for the listener in an other thread.
            Task.Factory.StartNew(() => new MessageListener(app.ApplicationServices, cloudAMQPConnectionString).StartSubscription());

            // seed the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<CustomerApiContext>();
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
