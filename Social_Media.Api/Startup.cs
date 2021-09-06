using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Data;
using SocialMedia.Infraestructure.Filters;
using SocialMedia.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
      services.AddControllers().AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      })
        .ConfigureApiBehaviorOptions(options =>
        {
          //Decimos que se cree un middleware que evite validar el modelo incluso usando el ApiController
          options.SuppressModelStateInvalidFilter = true;
        });
      //Creamos otro servicio para conectarnos directamente a la base de datos de sqlserver a paratir del appsettings.json
      services.AddDbContext<SocialMediaContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));
      //Injección de dependencias (PostRepository o PostMongoRepository), indica que 
      services.AddTransient<IPostRepository, PostRepository>();
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
