using Microsoft.AspNetCore.Mvc.Rendering;
using RestRes.Models;

namespace RestRes.ViewModels
{
    public class ReservationAddViewModel
    {
        public Reservation? Reservation { get; set; }
        public List<SelectListItem> AvailableRestaurants { get; set; }
    }
}