using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public class DisasterService : IDisasterService
    {
        private readonly ApplicationDbContext _context;

        public DisasterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disaster>> GetActiveDisastersAsync()
        {
            return await _context.Disaster
                .Where(d => d.IsActive)
                .OrderByDescending(d => d.DateReported)
                .ToListAsync();
        }

        public async Task<Disaster> GetDisasterByIdAsync(int id)
        {
            return await _context.Disaster.FindAsync(id);
        }

        public async Task<bool> AddDisasterAsync(Disaster disaster)
        {
            try
            {
                disaster.DateReported = DateTime.UtcNow;
                disaster.IsActive = true;
                _context.Disaster.Add(disaster);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDisasterAsync(Disaster disaster)
        {
            try
            {
                _context.Entry(disaster).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDisasterAsync(int id)
        {
            try
            {
                var disaster = await _context.Disaster.FindAsync(id);
                if (disaster == null)
                    return false;

                _context.Disaster.Remove(disaster);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
