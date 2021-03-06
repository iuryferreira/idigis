using System;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Filters;

namespace Server
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        
        public void ConfigureServices (IServiceCollection services)
        {   
            
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(Configuration.GetValue<string>("Client").ToString()).AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(NotificatorFilter));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() {Title = "Server", Version = "v1"});
            });
            services.AddApplication(Configuration);
        }

        public  static void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Idigis v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
