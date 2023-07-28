
using Data;
using Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
namespace Project.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

[Route("api/[controller]")]
public class UsersController : ControllerBase

{
    public readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var entities = await _context.Users.ToListAsync();
        return Ok(entities);
    }
    [HttpPost]
    public async Task<ActionResult> Post([FromForm] UserDto entityDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = new User()
        {
            Firstname = entityDto.Firstname,
            Lastname = entityDto.Lastname,
            Username = entityDto.Username,
            Address = entityDto.Address,
            Phone = entityDto.Phone,
            Email = entityDto.Email,
            Password = entityDto.Password,
        };
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return Ok("success");

    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] UserDto entityDto)
    {
        var entity = _context.Users.FirstOrDefault(e => e.Id == id);
        if (entity == null)
        {
            return BadRequest("User with Id " + id.ToString() + " not found to update");
        }
        else
        {
            entity.Username = entityDto.Username;
            entity.Firstname = entityDto.Firstname;
            entity.Lastname = entityDto.Lastname;
            entity.Address = entityDto.Address;
            entity.Phone = entityDto.Phone;
            entity.Email = entityDto.Email;
            entity.Password = entityDto.Password;
            await _context.SaveChangesAsync();
            return Ok("success");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = _context.Users.FirstOrDefault(e => e.Id == id);
        if (entity == null)
        {
            return BadRequest("User with Id " + id.ToString() + " not found to delete");
        }
        else
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("success");
        }
    }
}