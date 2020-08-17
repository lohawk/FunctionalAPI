using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionalAPI.Data;

namespace FunctionalAPI.API
{
    public class Startup
    {
        protected IWebHostEnvironment _env { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _env = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var dbRepository = _env.EnvironmentName switch
            {
                //"PROD" => new SqlDbRepository();
                _ => new MockDbRepository()
            };

            // Create the repository pipeline
            //   Cached -> Validating -> Versioning -> MockDb
            services.AddSingleton<IManageItemState>(_ => new CachedRepository(new ValidatingRepository(new VersioningRespository(dbRepository))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _env = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
