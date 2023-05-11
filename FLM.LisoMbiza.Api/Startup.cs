using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;

namespace FLM.LisoMbiza
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

            services.AddControllers();

            services.AddDbContext<FLMContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FLMDB"));
            });

            ConfigureSwagger(services);

            services.AddHealthChecks();

            services.AddTransient<IImportService, ImportService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IBranchProductService, BranchProductService>();
            
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBranchProductRepository, BranchProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Liso Mbiza - FLM Software Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors("AllowAnyOrigin");

            app.UseHealthChecks("/health");

            loggerFactory.AddFile($"{Directory.GetCurrentDirectory()}\\Logs\\FLM.LisoMbiza.logs");
        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Liso Mbiza - FLM Software Api", Version = "v1" });
            });
        }
    }
}
