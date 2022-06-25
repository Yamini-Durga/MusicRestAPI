using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicRestAPI.Data;
using MusicRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly SongsDbContext _dbContext;

        public SongsController(SongsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int pn = pageNumber ?? 1;
            int ps = pageSize ?? 1;
            var songs = await _dbContext.Songs.Select(s => new
            {
                Id = s.Id,
                Name = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).ToListAsync();
            songs = songs.Skip((pn - 1) * ps).Take(ps).ToList();
            return Ok(songs);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await _dbContext.Songs.Where(s => s.IsFeatured == true).Select(s => new
            {
                Id = s.Id,
                Name = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).ToListAsync();
            return Ok(songs);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await _dbContext.Songs.OrderByDescending(s => s.UploadedDate).Select(s => new
            {
                Id = s.Id,
                Name = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).Take(1).ToListAsync();
            return Ok(songs);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string name)
        {
            var songs = await _dbContext.Songs.Where(s => s.Title.StartsWith(name)).Select(s => new
            {
                Id = s.Id,
                Name = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).ToListAsync();
            return Ok(songs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSongById(int id)
        {
            var existing = await _dbContext.Songs.FindAsync(id);
            if (existing == null)
            {
                return NotFound("No record found against this id");
            }
            return Ok(existing);
        }
        [HttpPost]
        public async Task<IActionResult> AddSong([FromBody] Song song)
        {
            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] Song song)
        {
            var existing = await _dbContext.Songs.FindAsync(id);
            if (existing == null)
            {
                return NotFound("No record found against this id");
            }
            existing.Title = song.Title;
            await _dbContext.SaveChangesAsync();
            return Ok("Record updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var existing = await _dbContext.Songs.FindAsync(id);
            if (existing == null)
            {
                return NotFound("No record found against this id");
            }
            _dbContext.Songs.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return Ok("Record deleted successfully");
        }
    }
}