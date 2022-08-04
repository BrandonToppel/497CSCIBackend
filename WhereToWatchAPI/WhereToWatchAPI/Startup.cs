using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereToWatchAPI.Models;

namespace WhereToWatchAPI
{
    public class Startup
    {
        //This is to deal with CORS 
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder.SetIsOriginAllowed(isOriginAllowed: _ => true)
                    .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
            //TODO: Below is the ability to easily connect to a data. First put the connection string for the database in appsettings.json
            //Next a context model needs to be created for ApplicationDbContext. Once that is completed uncomment below

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUsers, ApplicationRoles>(options =>
            {
                //username and password requirements
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllers();

            services.AddSwaggerGen();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Swagger implementation for the UI to see how API is working.
                app.UseSwagger();
                app.UseSwaggerUI();
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
