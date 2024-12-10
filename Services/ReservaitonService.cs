using RentACar.Api.Data;
using RentACar.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace RentACar.Api.Services
{
    public class ReservationService
    {

        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.Include(r => r.Car).ToListAsync();
        }

        public async Task<Reservation> AddReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.Include(r => r.Car).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();


            return !await _context.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                r.StartDate < endDate &&
                r.EndDate > startDate);
            
        }
        public async Task<bool> IsCarExistsAsync(int carId)
        {
            return await _context.Cars.AnyAsync(c => c.Id == carId);

        }


    }
}
