using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using webdev_SIS.DataLayer;
using webdev_SIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webdev_SIS.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AnnouncementsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // CREATE ANNOUNCEMENT
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AnnouncementEntity model)
        {
            if (string.IsNullOrWhiteSpace(model.Header) || string.IsNullOrWhiteSpace(model.Details))
                return BadRequest(new { success = false, message = "Header and Details are required." });

            var parameters = new[] {
                new SqlParameter("@Header", model.Header),
                new SqlParameter("@Details", model.Details)
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync("EXEC AddAnnouncement @Header, @Details", parameters);
                return Ok(new { success = true, message = "Announcement added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        // UPDATE ANNOUNCEMENT
        [HttpPost]
        public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementEntity model)
        {
            if (string.IsNullOrWhiteSpace(model.Header) || string.IsNullOrWhiteSpace(model.Details))
                return BadRequest(new { success = false, message = "Header and Details are required." });

            var parameters = new[] {
                new SqlParameter("@Id", model.id),
                new SqlParameter("@Header", model.Header),
                new SqlParameter("@Details", model.Details)
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync("EXEC UpdateAnnouncement @Id, @Header, @Details", parameters);
                return Ok(new { success = true, message = "Announcement updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // DELETE ANNOUNCEMENT
        [HttpPost]
        public async Task<IActionResult> DeleteAnnouncement([FromForm] int Id)
        {
            try
            {
                var parameter = new SqlParameter("@Id", Id);
                await _db.Database.ExecuteSqlRawAsync("EXEC SoftDeleteAnnouncement @Id", parameter);
                return Ok(new { success = true, message = "Announcement deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

    }
}
