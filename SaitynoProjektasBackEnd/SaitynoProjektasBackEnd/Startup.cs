using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Development");
            if (_environment.IsProduction())
            {
                connectionString = Configuration.GetConnectionString("Production");
            }

            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(connectionString));

            connectionString = $"Data Source={System.Environment.CurrentDirectory}\\AudioCloud.db";
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddCors();

            services.AddMvc();

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddTransient<ISongsService, SongsService>();
            services.AddTransient<IPlaylistsService, PlaylistsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ILikesService, LikesService>();
            services.AddTransient<IEventsService, EventsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(builder =>
                builder.WithOrigins(
                    "http://localhost:3000",
                    "https://audiocloud.surge.sh"
                ));


            app.UseMvc();
        }
    }
}
