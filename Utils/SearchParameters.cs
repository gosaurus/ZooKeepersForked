using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Net.Http.Headers;

namespace ZooKeepers.SearchParameters
{
    public class SearchParameters
    {
        public string? AnimalName { get; set; }
        public string? Species { get; set; }
        public string? Classification { get; set; }
        public DateOnly? DateAcquired { get; set; }
        public int? Age { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber {get; set; } = 1;
        public string OrderByDescending { get; set; } = "False";

        public override string ToString()
        {
            return $"search params, species = {Species}, class={Classification}\n" +
            $" search params age = {Age}, Acquisition = {DateAcquired}" +
            $" order by desc = {OrderByDescending}";
        }
        public string? GetFilter(SearchParameters searchParameters)
        {
            if (AnimalName != null)
                return "AnimalName";
            else if (Classification != null)
                return "Classification";
            else if (Age != null)
                return "Age";
            else if (DateAcquired != null)
                return "DateAcquired";
            else 
                return "Species";
        }
    }
}
