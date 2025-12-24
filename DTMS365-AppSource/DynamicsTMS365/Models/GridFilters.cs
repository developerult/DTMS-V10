using Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Models
{
    public class GridFilters
    {
        public string Name { get; set; }

        public string FilterId { get; set; }

        public FilterDetails[] Filters { get; set; }
    }
}