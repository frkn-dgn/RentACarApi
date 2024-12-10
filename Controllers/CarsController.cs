using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Api.Entities;
using RentACar.Api.Services;

namespace RentACar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;

        public CarsController(CarService carService)
        {
            _carService = carService;
        }

       
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Car car)
        {
            var newCar = await _carService.AddCarAsync(car);
            return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, newCar);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Car car)
        {
            if (id != car.Id) return BadRequest();
            var updatedCar = await _carService.UpdateCarAsync(car);
            return Ok(updatedCar);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _carService.DeleteCarAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
