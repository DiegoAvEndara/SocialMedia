using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Core.CustomEntities
{
  public class PaginationList<T> : List<T>
  {
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;
    public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : (int?)null;
    public PaginationList(List<T> items, int count, int pageNumber, int pageSize)
    {
      TotalCount = count;
      PageSize = pageSize;
      CurrentPage = pageNumber;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      //Manejaremos el rango de páginas y los guardaremos en AddRange, es decir que cada elemento agragado al final de la lista tambien tendrá todas las propiedades antes asignadas
      AddRange(items); //ésto para poder manejar las págnas y la paginación con el número de páginas
      //Además tendrá las propiedades adicionales que asignamos anteriormente

    }
    public static PaginationList<T> Create (IEnumerable<T> source, int pageNumber, int pageSize){
      var count = source.Count();
      //El Skip no permite omtir los primeros registros de acuerdo al número que ele mandemos, el Take se quedará con una cantidad determinada de elementos de una lista
      var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
      return new PaginationList<T>(items, count, pageNumber, pageSize);
    }
  }
}
