#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Model;
using ToDoAPI.JWT.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ToDoesController : ControllerBase
    {
        private readonly ToDoAPIContext _context;
        private readonly ITokenService _tokenService;
        public ToDoesController(ToDoAPIContext context , ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/ToDoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoDTO>>> GetToDo()
        {
            var name = await GetIdentityClaim();
            var resultDTO = new List<ToDoDTO>();
            var user = await _context.Users.Where(x => (x.Username == name.Value)).Include(u => u.ToDoList).FirstOrDefaultAsync();
            var result = user.ToDoList;
            foreach (var item in result)
                resultDTO.Add(TodoToDTO(item));
            return resultDTO;
        }

        // GET: api/ToDoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoDTO>> GetToDo(int id)
        {
            var name = await GetIdentityClaim();
            var user = await _context.Users.Where(x => (x.Username == name.Value)).Include(u => u.ToDoList).FirstOrDefaultAsync();

            var toDo =  user.ToDoList.Where(t => t.Id == id).First();

            if (toDo == null)
            {
                return NotFound();
            }

            return TodoToDTO(toDo);
        }

        // PUT: api/ToDoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDoDTO toDoDTO)
        {
            if(toDoDTO.Id != id)
            {
                return BadRequest("id and entity id not equal");
            }
            var name = await GetIdentityClaim();
            var user = await _context.Users.Where(x => (x.Username == name.Value)).Include(u => u.ToDoList).FirstOrDefaultAsync();

            var toDo = user.ToDoList.Where(t => t.Id == id).First();

            toDo = new ToDo()
            {
                Id = toDoDTO.Id,
                Text = toDoDTO.Text,
                IsChecked = toDoDTO.IsChecked,
                CreateDate = toDoDTO.CreateDate
            };

            //_context.Entry(toDo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
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

        // POST: api/ToDoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoDTO>> PostToDo(ToDoDTO toDoDTO)
        {
            var name = await GetIdentityClaim();
            var toDo =  new ToDo()
            {
                Text = toDoDTO.Text,
                IsChecked = toDoDTO.IsChecked,
                CreateDate = toDoDTO.CreateDate,
                User = await _context.Users.FirstOrDefaultAsync(x => (x.Username == name.Value))
            };

            _context.ToDoes.Add(toDo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDo", new { id = toDo.Id }, TodoToDTO(toDo));
        }

        // DELETE: api/ToDoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var name = await GetIdentityClaim();
            var user = await _context.Users.Where(x => (x.Username == name.Value)).Include(u => u.ToDoList).FirstOrDefaultAsync();
            var toDo = user.ToDoList.Where(t => t.Id == id).First();

            if (toDo == null)
            {
                return NotFound();
            }

            _context.ToDoes.Remove(toDo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<Claim> GetIdentityClaim()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var claims = _tokenService.DecodeToken(token) as List<Claim>;
            return claims[0];
        }
        private bool ToDoExists(int id)
        {
            return _context.ToDoes.Any(e => e.Id == id);
        }
        private static ToDoDTO TodoToDTO(ToDo todo) =>
        new ToDoDTO
        {
            Id = todo.Id,
            Text = todo.Text,
            IsChecked = todo.IsChecked
        };
    }
}
