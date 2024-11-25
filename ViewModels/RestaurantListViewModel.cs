using RestRes.Models;

namespace RestRes.ViewModels
{
    public class RestaurantListViewModel
    {
        public IEnumerable<Restaurant>? Restaurants { get; set; }
    }
}