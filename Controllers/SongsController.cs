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
        public async Task<IActionResult> GetAllSongs()
        {
            return Ok(await _dbContext.Songs.ToListAsync());
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
        public async Task<IActionResult> AddSong([FromBody]Song song)
        {
            await _dbContext.Songs.AddAsync(song);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody]Song song)
        {
            var existing = await _dbContext.Songs.FindAsync(id);
            if (existing==null)
            {
                return NotFound("No record found against this id");
            }
            existing.Title = song.Title;
            existing.Language = song.Language;
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
