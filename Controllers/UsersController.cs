using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enrich.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Enrich.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly userdbContext _context;

        public UsersController(userdbContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Index()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
           catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Users/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userDetail = await _context.Users.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }

        // GET: Users/Create
        [HttpPost]
        public async Task<ActionResult<Users>> Create(Users user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("PostAsync", new { id = user.Id }, user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        // GET: Users/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, Users userDetail)
        {
            if (id != userDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDetail).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return NoContent();
        }


        // GET: Users/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            _context.Users.Remove(users);
            return users;
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
