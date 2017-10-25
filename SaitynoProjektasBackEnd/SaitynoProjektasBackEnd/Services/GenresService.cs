using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetGenres() => _context.Genres.Select(g => g.Name).ToList();
    }
}
