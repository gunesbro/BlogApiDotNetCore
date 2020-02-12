using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.DAL.Concrete.EFCore;
using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceStack.Redis;

namespace BlogProjectAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Issuer"],
                        ValidAudience = Configuration["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"])),
                        RoleClaimType = ClaimTypes.Role
                    };
                    options.Events = new JwtBearerEvents
                    {
                        //Gerekirse burada gelen token içerisindeki çeþitli bilgilere göre doðrulam yapýlabilir.
                        OnTokenValidated = vld => Task.CompletedTask,
                        //Eskimiþ bir Token ile gelindiðinde OnAuthenticationFailed metodu devreye girecektir
                        OnAuthenticationFailed = fld => Task.CompletedTask
                    };
                });
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AzureConnection"),
            b => b.MigrationsAssembly("BlogProjectAPI")));

            services.AddMvc();

            services.AddSwaggerGen(x =>
                {
                    x.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogProjectApi", Version = "v1" });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    x.IncludeXmlComments(xmlPath);
                    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {{
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }});
                });

            //services.AddDistributedRedisCache(option =>
            //{
            //    option.Configuration = Configuration.GetConnectionString("RedisConnection");
            //    option.InstanceName = "blogApi";
            //    option.ConfigurationOptions.DefaultDatabase = 0;
            //    option.ConfigurationOptions.ConnectTimeout = 3;
            //});
            services.AddRazorPages();
            services.AddTransient<ITokenRepository, EfTokenRepository>();
            services.AddTransient(typeof(IRedisRepository<>), typeof(RedisRepository<>));
            services.AddTransient<IPostsRepository, PostsRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();//Bunsuz Authorize attiribute ü patlak veriyor.
            app.UseAuthorization();

            app.UseStaticFiles(); //UseStaticFiles kullanýlmazsa Swagger çalýþmýyor.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogProjectApi V1");
                c.RoutePrefix = "api/docs";
                c.InjectStylesheet("/css/SwaggerInject.css");
                c.DocumentTitle = "BlogProjectApi";
            });
            app.UseMiddleware<ExceptionHandler>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
