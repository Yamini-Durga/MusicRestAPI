using Microsoft.EntityFrameworkCore;
using MusicRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRestAPI.Data
{
    public class SongsDbContext : DbContext
    {
        public SongsDbContext(DbContextOptions<SongsDbContext> options) : base(options) { }

        public DbSet<Song> Songs { get; set; }
    }
}
