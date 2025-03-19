using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace ZooKeepers.Models
{
    public class GenericPaginatedResponse
    {
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; } 
    }

    public class AnimalPaginatedResponse
    {
        public required GenericPaginatedResponse Pagination { get; set; }
        public virtual List<Animal>? Animals { get; set; }
    }

    public class EnclosurePaginatedResponse
    {
        public required GenericPaginatedResponse Pagination { get; set; }
        public virtual List<Enclosure>? Enclosure { get; set; }
    }
    

}