using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllSongs()
        {
            return Ok(_dbContext.Songs.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult GetSongById(int id)
        {
            var existing = _dbContext.Songs.Find(id);
            if (existing == null)
            {
                return NotFound("No record found against this id");
            }
            return Ok(existing);
        }
        [HttpPost]
        public IActionResult AddSong([FromBody]Song song)
        {
            _dbContext.Songs.Add(song);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateSong(int id, [FromBody]Song song)
        {
            var existing = _dbContext.Songs.Find(id);
            if (existing==null)
            {
                return NotFound("No record found against this id");
            }
            existing.Title = song.Title;
            existing.Language = song.Language;
            _dbContext.SaveChanges();
            return Ok("Record updated successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var existing = _dbContext.Songs.Find(id);
            if (existing == null)
            {
                return NotFound("No record found against this id");
            }
            _dbContext.Songs.Remove(existing);
            _dbContext.SaveChanges();
            return Ok("Record deleted successfully");
        }
    }
}
