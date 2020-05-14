using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;
using PhotographyConsole.Infrastructure;
using SharedModels;
using System;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace OrderApi
{
    public class Startup
    {
        /// <summary>
        /// Connection string for message broker - for DPI
        /// </summary>
        private readonly string cloudAMQPConnectionString = "host=roedeer.rmq.cloudamqp.com;virtualHost=baeynauo;username=baeynauo;password=7mYHJ1effqAkcc1HFZGC4wYZK6F_Iflf";

        // This is the url when using docker-compose, only works within the containered applications
        private readonly Uri _productSeviceGateway = new Uri("http://productapi/api/products");

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Repository 
            services.AddScoped<IRepository<Order>, OrderRepository>();
            
            // Converter
            services.AddScoped<IConverter, Converter>();
            
            // DB seed 
            services.AddTransient<IDbSeeding, DbSeeding>();

            // Singleton - MessagePublisher
            services.AddSingleton<IMessagePublisher>(new MessagePublisher(cloudAMQPConnectionString));
            
            // Singleton -ServiceGateWay For Product API
            services.AddSingleton<IServiceGateway<ProductDTO>>(new ProductServiceGateway(_productSeviceGateway));


            // In-memory database for the products:
            services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDatabase"));

            // Registering the Swagger generator
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "Order API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Email = "Huss0247@easv365.dk",
                        Name = "Hussain Taha",
                        Url = new Uri("https://github.com/Husso1997"),
                    },
                    
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
            });

            // Starting the Subscription for the listener in an other thread.
            Task.Factory.StartNew(() => new MessageListener(app.ApplicationServices, cloudAMQPConnectionString).StartSubscription());

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
