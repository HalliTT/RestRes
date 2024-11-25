using MongoDB.Bson;
using RestRes.Models;

namespace RestRes.Services
{
    public interface IRestaurantService
    {
        IEnumerable<Restaurant> GetAllRestaurants();
        Restaurant? GetRestaurantById(ObjectId id);

        void AddRestaurant(Restaurant newRestaurant);

        void EditRestaurant(Restaurant updatedRestaurant);

        void DeleteRestaurant(Restaurant restaurantToDelete);
    }
}