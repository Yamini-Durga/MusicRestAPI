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
    public class AlbumsController : ControllerBase
    {
        private SongsDbContext _dbContext;
        public AlbumsController(SongsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddArtist([FromBody] Album album)
        {
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            int pn = pageNumber ?? 1;
            int ps = pageSize ?? 1;
            var albums = await _dbContext.Albums.Select(a => new
            {
                Id = a.Id,
                Name = a.Name,
                ImageUrl = a.ImageUrl
            }).ToListAsync();
            albums = albums.Skip((pn - 1) * ps).Take(ps).ToList();
            return Ok(albums);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int id)
        {
            var artist = await _dbContext.Albums.Where(a => a.Id == id).Include(s => s.Songs).ToListAsync();
            return Ok(artist);
        }
    }
}
