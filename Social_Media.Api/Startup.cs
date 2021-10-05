using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infraestructure.Data;
using SocialMedia.Infraestructure.Filters;
using SocialMedia.Infraestructure.Intefaces;
using SocialMedia.Infraestructure.Repositories;
using SocialMedia.Infraestructure.Services;
using System;

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
      //Agregaremos una configuraci�n desde appsettings para usarlo en el proyecto, ebe de llamarse del mismo modo. Le decimos que quiero configurar la clase PaginationOptions creado recientemente con el archivod de configuraci�n appettings
      //Al usar la propiedad Configure<PaginationOptions> se indica que se har� con un patr�n singleton
      services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
      //Creamos otro servicio para conectarnos directamente a la base de datos de sqlserver a paratir del appsettings.json
      services.AddDbContext<SocialMediaContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));
      //Injecci�n de dependencias (PostRepository o PostMongoRepository), indica que 
      /*services.AddTransient<IPostRepository, PostRepository>();*/
      services.AddTransient<IPostService, PostService>();
     /* services.AddTransient<IUserRepository, UserRepository>();*/
     //Se eliminan los repositorios del usuario y del post por que estan repetidos ye n su lugar se usa Base repository segun el patr�n de dise�o GENERIC REPOSITORY
      services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      //Para usar una sola instancia en lo que dura el uso del consumo de los sercvicios usamos el patr�n singleton
      //Para poder asignar el vallor que recibimos en �ste m�todo usamos las propiedades GetRequiredService<> que devuelve una instancia
      services.AddSingleton<IUriService>(provider =>
      {
        //El IHttpContextAccesor Nos permite acceder al httpcontext de nuestra aplicaci�n unicamente
        var accesor = provider.GetRequiredService<IHttpContextAccessor>();
        var request = accesor.HttpContext.Request;
        var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        return new UriService(absoluteUri);
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

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
