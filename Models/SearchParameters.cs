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
        private int _pageSize = 10;
        public int PageSize
        { 
            get
            { return _pageSize; }
            set
            { _pageSize = value < 50 ? _pageSize = value : _pageSize = MaxPageSize; } 
        }
        private int MaxPageSize = 50;
        public int PageNumber {get; set; } = 1;
        public string OrderByDescending { get; set; } = "False";

        public override string ToString()
        {
            return $"search params, species = {Species}, class={Classification}" +
            $" search params age = {Age}, Acquisition = {DateAcquired}" +
            $" order by desc = {OrderByDescending}";
        }

        public string? GetFilter()
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
