using System.Text;
using istore_api.src.App.IService;
using istore_api.src.App.Service;
using istore_api.src.Domain.Entities.Config;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeDetective;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.App.Provider;
using webApiTemplate.src.App.Service;
using webApiTemplate.src.Domain.Entities.Config;

namespace istore_api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = _config.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new Exception("jwt settings is empty");
            var emailServiceSettings = _config.GetSection(nameof(EmailServiceSettings)).Get<EmailServiceSettings>() ?? throw new Exception("email service settings is empty");
            var telegramBotSettings = _config.GetSection(nameof(TelegramBotSettings)).Get<TelegramBotSettings>() ?? throw new Exception("telegram bot service settings is empty");
            var fileInspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            services.AddControllers(config =>
            {
                config.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
            .AddNewtonsoftJson()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            });

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                });
            });
            services.AddEndpointsApiExplorer();
            services.AddDbContext<AppDbContext>();

            services
                .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                });

            services.AddAuthorization();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "istore_api",
                    Description = "Api",
                });

                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            },
                        },
                        new string[] {}
                    }
                }
    );

                options.EnableAnnotations();
            })
            .AddSwaggerGenNewtonsoftSupport();


            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IFileUploaderService, LocalFileUploaderService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ITelegramBotService, TelegramBotService>();

            services.AddSingleton(fileInspector);
            services.AddSingleton(jwtSettings);
            services.AddSingleton(emailServiceSettings);
            services.AddSingleton(telegramBotSettings);

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Manager")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.Scan(scan =>
            {
                scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            // InitDatabase();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpLogging();


            app.UseRequestLocalization();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

        public void InitDatabase()
        {
            var context = new AppDbContext(new DbContextOptions<AppDbContext>(), _config);
            var adminEmail = "itismymessagebox@mail.ru";

            var admin = context.Users.FirstOrDefault(e => e.Email == adminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    Email = adminEmail,
                    RoleName = UserRole.Admin.ToString(),
                    PasswordHash = Hmac512Provider.Compute("qweasdZXC!1")
                };
                context.Users.Add(admin);

                var categories = new List<ProductCategory>{
                    new()
                    {
                        Name = "Iphone"
                    },

                    new()
                    {
                        Name = "AirPods"
                    },

                    new()
                    {
                        Name = "Watch"
                    },

                    new()
                    {
                        Name = "Mac"
                    },

                    new()
                    {
                        Name = "iPad"
                    },

                    new()
                    {
                        Name = "accessories"
                    },

                    new()
                    {
                        Name = "consoles"
                    },

                    new()
                    {
                        Name = "Dyson"
                    },
                };

                context.ProductCategories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}