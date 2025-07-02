using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TAINATechTest.Data.Data;
using TAINATechTest.Data.Repositories;
using TAINATechTest.Services;

namespace TAINATechTest
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
            services.AddControllersWithViews()
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                config.ImplicitlyValidateChildProperties = true;
            });
            services.AddLogging(builder => { builder.AddLog4Net("log4net.config"); });
            services.AddDbContext<PersonContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddValidatorsFromAssemblyContaining(typeof(Startup));

            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IPersonService, PersonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Person/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Person/NotFound/{0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Person}/{action=Index}/{id?}");
            });
        }
    }
}
