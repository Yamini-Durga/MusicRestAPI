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
        public IEnumerable<Song> GetAllSongs()
        {
            return _dbContext.Songs.ToList();
        }
        [HttpGet("{id}")]
        public Song GetSongById(int id)
        {
            return _dbContext.Songs.Find(id);
        }
        [HttpPost]
        public void AddSong([FromBody]Song song)
        {
            _dbContext.Songs.Add(song);
            _dbContext.SaveChanges();
        }
        [HttpPut("{id}")]
        public void UpdateSong(int id, [FromBody]Song song)
        {
            var existing = _dbContext.Songs.Find(id);
            existing.Title = song.Title;
            existing.Duration = song.Duration;
            existing.ImageUrl = song.ImageUrl;
            _dbContext.SaveChanges();
        }
        [HttpDelete("{id}")]
        public void DeleteSong(int id)
        {
            var existing = _dbContext.Songs.Find(id);
            _dbContext.Songs.Remove(existing);
            _dbContext.SaveChanges();
        }
    }
}
