using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Facades;
using API.Mowizz2.EHH.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Mowizz2.EHH
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
            services.Configure<DataBaseSettings>(
                Configuration.GetSection(nameof(DataBaseSettings)));

            services.AddSingleton<IDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DataBaseSettings>>().Value);

            services.AddSingleton<BankAccountsService>();
            services.AddSingleton<UsersService>();

            services.AddControllers();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                  .AllowAnyOrigin()
                  .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
