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
    public class ArtistsController : ControllerBase
    {
        private SongsDbContext _dbContext;
        public ArtistsController(SongsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddArtist([FromBody] Artist artist)
        {
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet]
        public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
        {
            int pn = pageNumber ?? 1;
            int ps = pageSize ?? 1;
            var artists = await _dbContext.Artists.Select(a => 
                new { Id = a.Id, Name = a.Name, ImageUrl = a.ImageUrl }).ToListAsync();
            artists = artists.Skip((pn - 1) * ps).Take(ps).ToList();
            return Ok(artists);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int id)
        {
            var artist = await _dbContext.Artists.Where(a => a.Id == id).Include(s => s.Songs).ToListAsync();
            return Ok(artist);
        }
    }
}
