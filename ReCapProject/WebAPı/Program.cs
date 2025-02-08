
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // React uygulamanýzýn adresi
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();

                });
            });



            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacBusinessModule());
            });


            builder.Services.AddControllers();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "AuthCookie";
                options.LoginPath = "/account/login"; // Yetkilendirme ba?ar?s?z olursa yönlendirme
                options.AccessDeniedPath = "/account/accessdenied"; // Yetki yoksa yönlendirme
                options.Cookie.HttpOnly = true; // XSS korumas?
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS üzerinden gönderim
                options.Cookie.SameSite = SameSiteMode.Strict; // CSRF korumas?
                options.ExpireTimeSpan = TimeSpan.FromHours(2); // Cookie'nin geçerlilik süresi
            });






            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });

                // Cookie Authentication için güvenlik tanýmý
                c.AddSecurityDefinition("CookieAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Cookie",
                    In = ParameterLocation.Header,
                    Description = "Cookie-based authentication. Example: AuthCookie={value}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CookieAuth"
                }
            },
            new string[] { }
        }
    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();



            app.MapControllers();

            app.Run();
        }
    }
}
