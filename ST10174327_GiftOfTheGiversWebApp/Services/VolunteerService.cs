using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly ApplicationDbContext _context;

        public VolunteerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterVolunteerAsync(Volunteer volunteer)
        {
            try
            {
                volunteer.RegistrationDate = DateTime.UtcNow;
                volunteer.IsActive = true;
                _context.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AssignTaskAsync(TaskAssignment task)
        {
            try
            {
                task.AssignmentDate = DateTime.UtcNow;
                task.Status = "Assigned";
                _context.TaskAssignments.Add(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Volunteer>> GetAllVolunteersAsync()
        {
            return await _context.Volunteers
                .Where(v => v.IsActive)
                .OrderBy(v => v.LastName)
                .ThenBy(v => v.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignment>> GetVolunteerTasksAsync(string volunteerId)
        {
            return await _context.TaskAssignments
                .Where(t => t.VolunteerId == volunteerId)
                .OrderByDescending(t => t.AssignmentDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateVolunteerStatusAsync(string volunteerId, bool isActive)
        {
            try
            {
                var volunteer = await _context.Volunteers.FindAsync(volunteerId);
                if (volunteer == null)
                    return false;

                volunteer.IsActive = isActive;
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
