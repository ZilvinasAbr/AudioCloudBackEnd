using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Services;
using SaitynoProjektasBackEnd.Services.Classes;
using SaitynoProjektasBackEnd.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

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

              services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(connectionString));

//            connectionString = $"Data Source={Environment.CurrentDirectory}\\AudioCloud.db";
//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlite(connectionString));

            services.AddCors();

            services.AddMvc();

            // Add this if circular referencing happens.
            // services.AddMvc().AddJsonOptions(options => {
            //     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // });

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

            // if (_environment.IsDevelopment())
            // {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("AudioCloud", new Info());
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                });
            // }

            services.AddTransient<ISongsService, SongsService>();
            services.AddTransient<IPlaylistsService, PlaylistsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ILikesService, LikesService>();
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IDropBoxService, DropBoxService>();
            services.AddTransient<IGenresService, GenresService>();
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
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );


            app.UseMvc();

            // if (env.IsDevelopment())
            // {
                const string swaggerUrl = "/swagger/AudioCloud/swagger.json";
                app.UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.DocExpansion("none");
                        c.SwaggerEndpoint(swaggerUrl, "AudioCloud");
                    });
            // }
        }
    }
}
