using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infraestructure.Repositories
{
  public class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    //Para poder continuar con el patrón de diseño UnitOfWork necesitamos cambiar el ámbito del _context a Protected y así cuando lo heredemos poder usarlo
    //protected readonly SocialMediaContext _context;
    private readonly SocialMediaContext _context;
    //En lugar de poner el _context en protected lo haremos con _entities porque lo estamos seteando con un _entities
    protected readonly DbSet<T> _entities;

    public BaseRepository(SocialMediaContext context)
    {
      _context = context;
      //Cuando seteamos de ésta manera solo tendríamos acceso al DB de posts si ponemos posts
      _entities = context.Set<T>();
    }
    //Para poder coinsidir con el patrón Unit Of Work debemos quitar el async en el metodo getALl
    //public async Task<IEnumerable<T>> GetAll()
    //{
    //  return await _entities.ToListAsync();
    //}
    public IEnumerable<T> GetAll()
    {
      //Añadimos el AsEnumerable para que no se ejecute e'ste método inmediatamente al inocarlo sino que me permita hacerle algunoos filtros primero, tambien debemos modificar la interfaz, el AsEnumerable carga los valores en mmoria antes de guardarlos
      return _entities.AsEnumerable();
    }
    public async Task<T> GetById(int id)
    {
      return await _entities.FindAsync(id);
    }
    public async Task Add(T entity)
    {
      //Para que el añadido coincida con el patrón Unit Of Work, luego de quitar el await saveChanges debemos agregar un addasync
      //_entities.Add(entity);
      await _entities.AddAsync(entity);
      //El responsable de gardar combios será la clase con implementación del patón Unit of Work
      //await _context.SaveChangesAsync();

    }
    public void Update(T entity)
    {
      //Para el update no hay forma de volverlo asíncrono porque no eiste un updateAsync y no podemos emparejarlo con el Task async por eso lo cambiamos on un void y delegaríaos la responsabilidad asíncrona a la clase Unit Of Work que usaría el UpdateAsync
      _entities.Update(entity);
      //El responsable de gardar combios será la clase con implementación del patón Unit of Work
      //await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
      T entity = await GetById(id);
      _entities.Remove(entity);
      await _context.SaveChangesAsync();
    }


  }
}
