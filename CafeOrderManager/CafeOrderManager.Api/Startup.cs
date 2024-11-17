using CafeOrderManager.Infrastructure.Configuration;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Service.Auth;
using CafeOrderManager.Service.Category;
using CafeOrderManager.Service.Order;
using CafeOrderManager.Service.OrderItem;
using CafeOrderManager.Service.Product;
using CafeOrderManager.Service.Security;
using CafeOrderManager.Service.Table;
using CafeOrderManager.Service.User;
using CafeOrderManager.Storage;
using CafeOrderManager.Storage.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CafeOrderManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Console.WriteLine(connectionString + "????");
            services.AddDbContext<CafeOrderManagerDbContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    opts
                        => opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });
            services.AddControllers().AddXmlSerializerFormatters();

            // services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; })
            //     .AddXmlSerializerFormatters();

            #region Cors

            var corsLinks = Configuration["CorsLinks"];
            var appUrl = corsLinks.Split(";");
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithOrigins(appUrl)
                        .Build());
            });

            // 3. Swagger: API dökümantasyonu
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            #endregion

            #region Authentication

            var jwtSecurityKey = Configuration["Settings:JwtSecurityKey"];
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AuthService.JWT_ISSUER,
                        ValidAudience = AuthService.JWT_AUDIENCE,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
                        ClockSkew = TimeSpan.Zero, // bunu yazmay�nca expired tarihi i�e aram�yor
                    };
                });

            #endregion

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services
                .AddControllersWithViews(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson();

            RegisterService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<CafeOrderManagerDbContext>().Database.Migrate();
            }

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeTracker API V1");
                });
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void RegisterService(IServiceCollection services)
        {
            //services.AddScoped<MailManager>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<SettingsConfig>(Configuration.GetSection("Settings"));
            //services.Configure<S3SettingsConfig>(Configuration.GetSection("S3Settings"));
            //services.Configure<MailSettingsDto>(Configuration.GetSection("MailSettings"));


            // Mappers
            services.AddScoped<TableMapper>();
            services.AddScoped<UserMapper>();
            services.AddScoped<CategoryMapper>();
            services.AddScoped<ProductMapper>();
            services.AddScoped<OrderMapper>();
            services.AddScoped<OrderItemMapper>();


            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<AuthService>();
            //services.AddScoped<UserService>();
            services.AddScoped<SecurityService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ProductService>();
            services.AddScoped<TableService>();
            services.AddScoped<OrderService>();
            services.AddScoped<OrderItemService>();



            //Repositories
            services.AddScoped<UserRepository>();
            services.AddScoped<TableRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<OrderItemRepository>();



        }
    }
}