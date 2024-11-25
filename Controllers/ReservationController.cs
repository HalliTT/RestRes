using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using RestRes.Models;
using RestRes.Services;
using RestRes.ViewModels;

namespace RestRes.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _ReservationService;
        private readonly IRestaurantService _RestaurantService;

        public ReservationController(IReservationService ReservationService, IRestaurantService RestaurantService)
        {
            _ReservationService = ReservationService;
            _RestaurantService = RestaurantService;
        }

        public IActionResult Index()
        {
            var groupedReservations = _ReservationService.GetAllReservations()
                .GroupBy(r => new { r.RestaurantId, r.RestaurantName })
                .ToList();

            ReservationListViewModel viewModel = new ReservationListViewModel()
            {
                GroupedReservations = groupedReservations
            };

            return View(viewModel);
        }

        //public IActionResult Add(ObjectId restaurantId)
        //{
        //    var selectedRestaurant = _RestaurantService.GetRestaurantById(restaurantId);

        //    ReservationAddViewModel reservationAddViewModel = new ReservationAddViewModel();

        //    reservationAddViewModel.Reservation = new Reservation();
        //    reservationAddViewModel.Reservation.RestaurantId = selectedRestaurant.Id;
        //    reservationAddViewModel.Reservation.RestaurantName = selectedRestaurant.name;
        //    reservationAddViewModel.Reservation.date = DateTime.UtcNow;

        //    return View(reservationAddViewModel);
        //}
        public IActionResult Add(ObjectId? restaurantId)
        {
            var restaurants = _RestaurantService.GetAllRestaurants();

            ReservationAddViewModel reservationAddViewModel = new ReservationAddViewModel
            {
                AvailableRestaurants = restaurants.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.name
                }).ToList(),
                Reservation = new Reservation()
            };

            if (restaurantId.HasValue)
            {
                var selectedRestaurant = _RestaurantService.GetRestaurantById(restaurantId.Value);
                reservationAddViewModel.Reservation.RestaurantId = selectedRestaurant.Id;
                reservationAddViewModel.Reservation.RestaurantName = selectedRestaurant.name;
            }

            reservationAddViewModel.Reservation.date = DateTime.UtcNow;

            return View(reservationAddViewModel);
        }

        [HttpPost]
        public IActionResult Add(ReservationAddViewModel reservationAddViewModel)
        {
            if (reservationAddViewModel.Reservation.RestaurantId == ObjectId.Empty)
            {
                ModelState.AddModelError("", "You must select a restaurant.");
            }

            if (ModelState.IsValid)
            {
                Reservation newReservation = new()
                {
                    RestaurantId = reservationAddViewModel.Reservation.RestaurantId,
                    date = reservationAddViewModel.Reservation.date,
                };

                _ReservationService.AddReservation(newReservation);
                return RedirectToAction("Index");
            }

            // Re-populate dropdown in case of errors
            reservationAddViewModel.AvailableRestaurants = _RestaurantService.GetAllRestaurants()
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.name
                }).ToList();

            return View(reservationAddViewModel);
        }



        //[HttpPost]
        //public IActionResult Add(ReservationAddViewModel reservationAddViewModel)
        //{
        //    Reservation newReservation = new()
        //    {
        //        RestaurantId = reservationAddViewModel.Reservation.RestaurantId,
        //        date = reservationAddViewModel.Reservation.date,
        //    };

        //    _ReservationService.AddReservation(newReservation);
        //    return RedirectToAction("Index");
        //}

        public IActionResult Edit(string Id)
        {
            if (Id == null || string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var selectedReservation = _ReservationService.GetReservationById(new ObjectId(Id));
            return View(selectedReservation);
        }

        [HttpPost]
        public IActionResult Edit(Reservation reservation)
        {
            try
            {
                var existingReservation = _ReservationService.GetReservationById(reservation.Id);
                if (existingReservation != null)
                {
                    _ReservationService.EditReservation(reservation);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"Reservation with ID {reservation.Id} does not exist!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the reservation failed, please try again! Error: {ex.Message}");
            }

            return View(reservation);
        }

        public IActionResult Delete(string Id)
        {
            if (Id == null || string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var selectedReservation = _ReservationService.GetReservationById(new ObjectId(Id));
            return View(selectedReservation);
        }

        [HttpPost]
        public IActionResult Delete(Reservation reservation)
        {
            if (reservation.Id == null)
            {
                ViewData["ErrorMessage"] = "Deleting the reservation failed, invalid ID!";
                return View();
            }

            try
            {
                _ReservationService.DeleteReservation(reservation);
                TempData["ReservationDeleted"] = "Reservation deleted successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the reservation failed, please try again! Error: {ex.Message}";
            }

            var selectedRestaurant = _ReservationService.GetReservationById(reservation.Id);
            return View(selectedRestaurant);
        }
    }
}