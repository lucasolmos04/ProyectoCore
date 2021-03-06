using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Persistencia.DapperConexion.Paginacion;
using Seguridad.TokenSeguridad;
using WebAPI.Middleware;

namespace WebAPI
{
    /// <summary>
    /// Clase de start del proyecto
    /// </summary>
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
            services.AddCors(o => o.AddPolicy("corsApp", build=> {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // Generamos un servico al EntityFramework para poder tener acceso a la base de datos
            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddOptions();
            // Agregamos conexion para Dapper
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            // Indicamos que clase es al que se va a validar RegisterValidatorsFromAssemblyContaining
            services.AddControllers(opt => {
                // agregamos una poliza para que cada controlador requiera la autorizacion del Token
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            // Agregamos como servicio a Usuarios con Core Identity
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            // ***** ROL Manager
            // Agregamos los Roles 
            identityBuilder.AddRoles<IdentityRole>();
            // Data de los roles dentro de la data del token
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();
            // ****************

            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            // Manejo del loggin
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            // TryAddSingleton: el registro no se agregará cuando ya existan registros para el tipo de servicio dado. 
            services.TryAddSingleton<ISystemClock, SystemClock>();

            // Servicio de Autenticación
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false, // El pedido de Token va a ser free, no tenemos restricciones, va a ser publico
                    ValidateIssuer = false
                };
            });

            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            services.AddAutoMapper(typeof(Consulta.Manejador));

            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();
            services.AddScoped<IPaginacion, PaginacionRepositorio>();

            // agregamos configuracion de Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services para mantenimiento de cursos",
                    Version = "v1"
                });
                c.CustomSchemaIds(s => s.FullName); // Para que no tire conflictos los endpoints que tienen el mismo nombre de clase
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("corsApp");

            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage(); // Middleware propio de c#
            }

            //app.UseHttpsRedirection(); // Solo usado en produccion
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos Online v1");
           });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Configuramos el controller de navegacion que se usara como inicio
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=index}/{id}"
                );

                endpoints.MapFallbackToController("Index", "Home");
            });
        }
    }
}
