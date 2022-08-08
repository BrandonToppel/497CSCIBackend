using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WhereToWatchAPI;
using WhereToWatchAPI.Models;

namespace WhereToWatchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IConfiguration Configuration;

        public WatchlistsController(ApplicationDbContext context, UserManager<ApplicationUsers> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            Configuration = configuration;
        }

        // GET: api/Watchlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Watchlist>>> GetWatchlist()
        {
            return await _context.Watchlist.ToListAsync();
        }

        // GET: api/Watchlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Watchlist>> GetWatchlist(int id)
        {
            var watchlist = await _context.Watchlist.FindAsync(id);

            if (watchlist == null)
            {
                return NotFound();
            }

            return watchlist;
        }

        //GET: returns the watchlist of a specific user
        
        [HttpGet]
        [Route("GetWatchlist")]
        public async Task<ActionResult<IEnumerable<Watchlist>>> GetUserWatchlist()
        {
            var jwt = Request.Cookies["jwtCookie"];
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(jwt, validations, out var tokenSecure);
            string user = claims.Identity.Name;

            var myuser = await _userManager.FindByEmailAsync(user);

            var watchlistData = _context.Watchlist.Where(u => u.UserId == myuser.Id).Include(u => u.Movies).ToList();

            return watchlistData;
        }

        // PUT: api/Watchlists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWatchlist(int id, Watchlist watchlist)
        {
            if (id != watchlist.UserId)
            {
                return BadRequest();
            }

            _context.Entry(watchlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WatchlistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Watchlists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
       // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<Watchlist>> PostWatchlist(Watchlist watchlist)
        {
            ApplicationUsers newUser = new ApplicationUsers();
            newUser = await _userManager.FindByNameAsync(watchlist.Users.UserName);
            Movies newMovie = new Movies();
            newMovie = _context.Movies.Where(m => m.MovieTitle == watchlist.Movies.MovieTitle).FirstOrDefault();

            watchlist.Movies = newMovie;
            watchlist.Users = newUser;

            _context.Watchlist.Add(watchlist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WatchlistExists(watchlist.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWatchlist", new { id = watchlist.UserId }, watchlist);
        }

        // DELETE: api/Watchlists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Watchlist>> DeleteWatchlist(int id)
        {
            var watchlist = await _context.Watchlist.FindAsync(id);
            if (watchlist == null)
            {
                return NotFound();
            }

            _context.Watchlist.Remove(watchlist);
            await _context.SaveChangesAsync();

            return watchlist;
        }

        private bool WatchlistExists(int id)
        {
            return _context.Watchlist.Any(e => e.UserId == id);
        }
    }
}
