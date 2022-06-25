using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicRestAPI.Data;
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
    }
}
