﻿using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
  public interface IRepository<T> where T: BaseEntity
  {
    //Para cumplir con el patrón unit of work necesitamos cambiar las variables asíncronas
    //Task<IEnumerable<T>> GetAll();
    IEnumerable<T> GetAll();
    Task<T> GetById(int id);
    Task Add(T entity);
    //Task Update(T entity);
    void Update(T entity);

    Task Delete(int id);
  }
}
