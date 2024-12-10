using Microsoft.AspNetCore.Mvc;
using RentACar.Api.Entities;
using RentACar.Api.Models;
using RentACar.Api.Services;

namespace RentACar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {

        private readonly ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ReservationRequest reservationRequest)
        {
            
            var carExists = await _reservationService.IsCarExistsAsync(reservationRequest.CarId);
            if (!carExists) return BadRequest("The specified car does not exist.");

            
            reservationRequest.StartDate = reservationRequest.StartDate.ToUniversalTime();
            reservationRequest.EndDate = reservationRequest.EndDate.ToUniversalTime();

            var isAvailable = await _reservationService.IsCarAvailableAsync(reservationRequest.CarId, reservationRequest.StartDate, reservationRequest.EndDate);
            if (!isAvailable) return BadRequest("Car is not available for the selected dates.");

            var reservation = new Reservation
            {
                CarId = reservationRequest.CarId,
                CustomerName = reservationRequest.CustomerName,
                StartDate = reservationRequest.StartDate,
                EndDate = reservationRequest.EndDate,
                IsConfirmed = false
            };

            var newReservation = await _reservationService.AddReservationAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = newReservation.Id }, newReservation);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reservationService.DeleteReservationAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

    }
}
