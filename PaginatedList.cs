using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC
{
     public class PaginatedList<T> where T : class
     {
          public int PageIndex { get; }
          public int PageSize { get; }
          public int TotalPages { get; }
          public string Route { get; } = default!;
    

          public List<T> Items { get; } = new();

          public PaginatedList(List<T> items, int count, int pageIndex = 1,
               int pageSize = 10, string route = "/")
          {
               PageIndex = pageIndex;
               PageSize = pageSize;
               TotalPages = (int)Math.Ceiling(count / (double)pageSize);
               Items.AddRange(items);
               Route = route;
          }
        
          public bool HasPreviousPage => PageIndex > 1;
          public bool HasNextPage => PageIndex < TotalPages;

          
     }
}
