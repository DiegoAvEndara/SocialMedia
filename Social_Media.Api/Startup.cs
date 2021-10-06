using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infraestructure.Data;
using SocialMedia.Infraestructure.Filters;
using SocialMedia.Infraestructure.Intefaces;
using SocialMedia.Infraestructure.Repositories;
using SocialMedia.Infraestructure.Services;
using System;
using System.IO;
using System.Reflection;

namespace Social_Media.Api
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
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddControllers(options =>
      {
        //Agregamos un GlobalExceptionFilter que creamos para controlar las excepciones y que no nos salga un mensaje innecesariamente largo
        options.Filters.Add<GlobalExceptionFilter>();
      }).AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
      })
      .ConfigureApiBehaviorOptions(options =>
      {
        //Decimos que se cree un middleware que evite validar el modelo incluso usando el ApiController
        options.SuppressModelStateInvalidFilter = true;
      });
      //Agregaremos una configuración desde appsettings para usarlo en el proyecto, ebe de llamarse del mismo modo. Le decimos que quiero configurar la clase PaginationOptions creado recientemente con el archivod de configuración appettings
      //Al usar la propiedad Configure<PaginationOptions> se indica que se hará con un patrón singleton
      services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
      //Creamos otro servicio para conectarnos directamente a la base de datos de sqlserver a paratir del appsettings.json
      services.AddDbContext<SocialMediaContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));
      //Injección de dependencias (PostRepository o PostMongoRepository), indica que 
      /*services.AddTransient<IPostRepository, PostRepository>();*/
      services.AddTransient<IPostService, PostService>();
     /* services.AddTransient<IUserRepository, UserRepository>();*/
     //Se eliminan los repositorios del usuario y del post por que estan repetidos ye n su lugar se usa Base repository segun el patrón de diseño GENERIC REPOSITORY
      services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      //Para usar una sola instancia en lo que dura el uso del consumo de los sercvicios usamos el patrón singleton
      //Para poder asignar el vallor que recibimos en éste método usamos las propiedades GetRequiredService<> que devuelve una instancia
      services.AddSingleton<IUriService>(provider =>
      {
        //El IHttpContextAccesor Nos permite acceder al httpcontext de nuestra aplicación unicamente
        var accesor = provider.GetRequiredService<IHttpContextAccessor>();
        var request = accesor.HttpContext.Request;
        var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        return new UriService(absoluteUri);
      });
      //Para empezar a usar Swagger, además necesitamos agregar a nivel de filtro un app.UseSwagger
      services.AddSwaggerGen(doc=>
      {
        doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Api", Version = "v1" });
        //Para poder generar la documentación de swagger importamos un Assembly, luego añadimos una dirección de un xml a los comentarios de swagger
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        doc.IncludeXmlComments(xmlPath);
      });
      services.AddMvc(options =>
      {
        options.Filters.Add<ValidationFilter>();
      }).AddFluentValidation(options =>
      {
        options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();
      //Para usar swagger
      app.UseSwagger();

      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API v1");
        options.RoutePrefix = string.Empty;
      });

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
